using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using StockRanking.Entities;
using System.Diagnostics;
using StockRanking.BusinessLogic;

namespace StockRanking
{
    public class RankingCalculator
    {
        public static bool CancelResultsProcessing = false;
        bool ExportDebugData = true;
        public static String DebugExport = "";
        PortfolioParameters Portfolio = null;

        public Boolean isOptimalRun = false;

        public RankingCalculator(PortfolioParameters portfolio, Boolean isOptimal = false)
        {
            Portfolio = portfolio;
            isOptimalRun = isOptimal;
        }

        public Dictionary<int, RankingResult> ProcessRanking(DateTime startDate, List<Stock> stocks, List<FilterConditions> filtersList, List<FeatureWeight> featureWeights, int monthsPeriod, bool loadForwardGain)
        {
            bool Needs1Year = false;
            bool Needs3Year = false;
            bool Needs5Year = false;

            bool needRatios = false;

            foreach (FilterConditions f in filtersList)
            {
                if (f.isCustom > 0)
                {
                    needRatios = true;
                }
            }

            //read which features to ignore
            bool[] featuresEnabled = new bool[featureWeights.Count];
            for (int i = 0; i < featureWeights.Count; i++)
            {
                featuresEnabled[i] = false;
                if (featureWeights[i].IsEnabled && featureWeights[i].Weight != 0)
                {
                    if (featureWeights[i].Years == 1)
                        Needs1Year = true;
                    if (featureWeights[i].Years == 3)
                        Needs3Year = true;
                    if (featureWeights[i].Years == 5)
                        Needs5Year = true;

                    featuresEnabled[i] = true;
                }
            }

            int dateInt = Utils.ConvertDateTimeToInt(startDate);
            int yearId = (Needs1Year ? 4 : 0) + (Needs3Year ? 2 : 0) + (Needs5Year ? 1 : 0);
            int id = 0;

            Boolean needToRun = true;
            if (isOptimalRun == true && CachedFeatures.Instance.dateIndex.ContainsKey(dateInt))
            {
                id = CachedFeatures.Instance.dateIndex[dateInt];
                if (CachedFeatures.Instance.features[id].featureList[yearId] != null && 
                    CachedFeatures.Instance.features[id].featureList[yearId].Count > 0)
                {
                    needToRun = false;
                }
            }

            Dictionary<int, RankingResult> rankings = new Dictionary<int, RankingResult>();

            if (needToRun)
            {
                foreach (Stock stock in stocks)
                {
                    if (CancelResultsProcessing)
                    {
                        return null;
                    }

                    stock.Features.Clear();
                    stock.CurrentFilters = null;
                    if (stock.FeatureTables.Count > 0 && stock.CurrentPrice > 0)
                    {
                        stock.GenerateFeaturesFromTable(featuresEnabled, startDate);

                        //stock.CurrentFilters = stock.FiltersTables.Find(x => x.Date == dateInt);
                        stock.CurrentFilters = Utility.BinarySearchFilterTable(stock.FiltersTables, dateInt);

                        if (stock.CurrentFilters == null && stock.FiltersTables.Count > 1)
                        {
                            stock.CurrentFilters = stock.FiltersTables[stock.FiltersTables.Count - 1];
                            //if last filter date is > last 6 months, use that data
                            if (stock.CurrentFilters.Date < Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-6)))
                                stock.CurrentFilters = null;
                        }
                    }
                }

                //select a sublist of filtered stocks that all have features
                List<Stock> filteredStocks = new List<Stock>();
                int co1 = 0, co2 = 0, co3 = 0, co4 = 0;
                foreach (Stock stock in stocks)
                {
                    if (stock.Features.Count == 0 || stock.CurrentFilters == null)
                        continue;

                    if (dateInt < stock.FirstHistoryPricesDate)
                        continue;

                    bool filtersPassed = true;

                    //check if it has valid years for the features selected (it depends if there are features for 1, 3 or 5 years back
                    if (Needs1Year && !stock.Has1YearBackData)
                        filtersPassed = false;
                    if (Needs3Year && !stock.Has3YearBackData)
                        filtersPassed = false;
                    if (Needs5Year && !stock.Has5YearBackData)
                        filtersPassed = false;

                    //check if industry allowed
                    if (Portfolio.IndustriesIncluded.Count > 0)
                    {
                        if (!Portfolio.IndustriesIncluded.Contains(stock.getStockSector()))
                            filtersPassed = false;
                    }

                    if (needRatios && filtersPassed)
                    {
                        try
                        {
                            //stock.CurrentRatio = stock.Ratios.FindLast(x => x.DateFirstMonth <= dateInt);
                            stock.CurrentRatio = Utility.BinarySearchLastRatio(stock.Ratios, dateInt);
                        }
                        catch (Exception e)
                        {

                        }

                        if (stock.CurrentRatio == null)
                        {
                            co1++;
                            continue;
                        }
                        else
                        {
                            co2++;
                            if (stock.CurrentRatio.DateFirstMonth == dateInt) co3++;
                            else co4++;
                        }
                        filteredStocks.Add(stock);
                    }
                    else if (filtersPassed)
                    {
                        filteredStocks.Add(stock);
                    }
                }

                if (filteredStocks.Count == 0)
                {
                    return null;
                }

                List<FilterConditions> orderedFilters = filtersList.OrderBy(si => si.Order).ToList();
                foreach (FilterConditions filter in orderedFilters)
                {
                    List<Stock> stocksToRemove = new List<Stock>();
                    FilterConditions newFilter = new FilterConditions();

                    double outValue = 0;

                    if (filter.FilterType == FilterTypes.VRSector)
                    {
                        Dictionary<Stock, double> compositeAllRank = GetCompositeSectorRank(filteredStocks, true);
                        foreach (var stock in compositeAllRank.Keys)
                        {
                            if (!filter.CheckFilter(stock, Portfolio, Math.Round(compositeAllRank[stock] * 100, 2), out outValue))
                            {
                                stocksToRemove.Add(stock);
                            } else
                            {
                                stock.filterValue = outValue;
                            }
                        }
                    }
                    else if (filter.FilterType == FilterTypes.VRUniverse)
                    {
                        Dictionary<Stock, double> compositeAllRank = GetCompositeUniverseRank(filteredStocks, true);
                        foreach (var stock in compositeAllRank.Keys)
                        {
                            if (!filter.CheckFilter(stock, Portfolio, Math.Round(compositeAllRank[stock] * 100, 2), out outValue))
                            {
                                stocksToRemove.Add(stock);
                            } else
                            {
                                stock.filterValue = outValue;
                            }
                        }
                    }
                    else if (filter.FilterType == FilterTypes.VRHistory)
                    {
                        Dictionary<int, Stock> indexedStocks = new Dictionary<int, Stock>();
                        foreach (var st in filteredStocks)
                            indexedStocks.Add(st.Id, st);
                        Dictionary<int, double> compositeAllRank = GetCompositeHistoryRank(filteredStocks, Utils.ConvertDateTimeToInt(startDate));
                        if (compositeAllRank != null)
                        {
                            foreach (var stock in compositeAllRank.Keys)
                            {
                                if (!filter.CheckFilter(indexedStocks[stock], Portfolio, Math.Round(compositeAllRank[stock] * 100, 2), out outValue))
                                {
                                    stocksToRemove.Add(indexedStocks[stock]);
                                } else
                                {
                                    indexedStocks[stock].filterValue = outValue;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Stock stock in filteredStocks)
                        {
                            if (!filter.CheckFilter(stock, Portfolio, out outValue))
                            {
                                stocksToRemove.Add(stock);
                            } else
                            {
                                stock.filterValue = outValue;
                            }
                        }
                    }

                    foreach (var stock in stocksToRemove)
                        filteredStocks.Remove(stock);

                    stocksToRemove.Clear();

                    if (filteredStocks.Count == 0)
                    {
                        return null;
                    }

                    if (filter.FilterRange != FilterRanges.CompareEquation && filter.Option != 0)
                    {
                        int step = 1, start = 0, end = filteredStocks.Count;
                        switch (filter.Option)
                        {
                            case 1:
                                step = 10;
                                break;
                            case 2:
                                step = 5;
                                break;
                            case 3:
                                step = 4;
                                break;
                            case 4:
                                step = 2;
                                break;
                        }
                        switch (filter.FilterRange)
                        {
                            case FilterRanges.MoreThan:
                                start = Convert.ToInt32(start + (end- start) * (filter.Value1 - 1) / step);
                                break;
                            case FilterRanges.LessThan:
                                end = Convert.ToInt32(start + (end - start) * filter.Value1 / step);
                                break;
                            case FilterRanges.Between:
                                int tmp = Convert.ToInt32(start + (end - start) * (filter.Value1 - 1) / step);
                                end = Convert.ToInt32(start + (end - start) * filter.Value2 / step);
                                start = tmp;
                                break;
                        }

                        filteredStocks.Sort(delegate (Stock x, Stock y)
                        {
                            return x.filterValue.CompareTo(y.filterValue);
                        });

                        Console.WriteLine(start + ", " + end + ", " + filteredStocks.Count);

                        if (start > 0)
                        {
                            filteredStocks.RemoveRange(0, start);
                            end -= start;
                        }
                        if (end != filteredStocks.Count)
                        {
                            filteredStocks.RemoveRange(end, filteredStocks.Count - end);
                        }
                        
                        if (filteredStocks.Count == 0)
                        {

                            return null;
                        }
                    }
                }

                /*
                //generate VRHistory and VRSector rank and remove filtered stocks
                List<Stock> stocksToRemove = new List<Stock>();
                FilterConditions filterVRSector = filtersList.FirstOrDefault(x => x.FilterType == FilterTypes.VRSector);
                if (filterVRSector != null)
                {
                    Dictionary<Stock, double> compositeAllRank = GetCompositeSectorRank(filteredStocks, true);
                    foreach (var stock in compositeAllRank.Keys)
                    {
                        if (!filterVRSector.CheckFilter(stock, Portfolio, Math.Round(compositeAllRank[stock] * 100, 2)))
                        {
                            stocksToRemove.Add(stock);
                        }
                    }
                }

                FilterConditions filterVRUniverse = filtersList.FirstOrDefault(x => x.FilterType == FilterTypes.VRUniverse);
                if (filterVRUniverse != null)
                {
                    Dictionary<Stock, double> compositeAllRank = GetCompositeUniverseRank(filteredStocks, true);
                    foreach (var stock in compositeAllRank.Keys)
                    {
                        if (!filterVRUniverse.CheckFilter(stock, Portfolio, Math.Round(compositeAllRank[stock] * 100, 2)))
                        {
                            stocksToRemove.Add(stock);
                        }
                    }
                }

                FilterConditions filterVRHistory = filtersList.FirstOrDefault(x => x.FilterType == FilterTypes.VRHistory);
                if (filterVRHistory != null)
                {
                    Dictionary<int, Stock> indexedStocks = new Dictionary<int, Stock>();
                    foreach (var st in filteredStocks)
                        indexedStocks.Add(st.Id, st);
                    Dictionary<int, double> compositeAllRank = GetCompositeHistoryRank(filteredStocks, Utils.ConvertDateTimeToInt(startDate));
                    if (compositeAllRank != null)
                    {
                        foreach (var stock in compositeAllRank.Keys)
                        {
                            if (!filterVRHistory.CheckFilter(indexedStocks[stock], Portfolio, Math.Round(compositeAllRank[stock] * 100, 2)))
                            {
                                stocksToRemove.Add(indexedStocks[stock]);
                            }
                        }
                    }
                }
                foreach (var stock in stocksToRemove)
                    filteredStocks.Remove(stock);

                if (filteredStocks.Count == 0)
                    return null;
                */

                //fill VR features
                int vrUniverseIndex = featureWeights.First(x => x.ShortName == "VRUNIVERSE").FeatureIndex;
                if (featuresEnabled[vrUniverseIndex])
                {
                    Dictionary<Stock, double> compositeAllRank = GetCompositeUniverseRank(filteredStocks, true);
                    foreach (Stock stock in filteredStocks)
                        stock.Features[vrUniverseIndex].FeatureValue = (float)compositeAllRank[stock];
                }
                int vrSectorIndex = featureWeights.First(x => x.ShortName == "VRSECTOR").FeatureIndex;
                if (featuresEnabled[vrSectorIndex])
                {
                    Dictionary<Stock, double> compositeAllRank = GetCompositeSectorRank(filteredStocks, true);
                    foreach (Stock stock in filteredStocks)
                        stock.Features[vrSectorIndex].FeatureValue = (float)compositeAllRank[stock];
                }
                int vrHistoryIndex = featureWeights.First(x => x.ShortName == "VRHISTORY").FeatureIndex;
                if (featuresEnabled[vrHistoryIndex])
                {
                    Dictionary<int, double> compositeAllRank = GetCompositeHistoryRank(filteredStocks, Utils.ConvertDateTimeToInt(startDate));
                    if (compositeAllRank != null)
                        foreach (Stock stock in filteredStocks)
                            if (compositeAllRank.ContainsKey(stock.Id))
                                stock.Features[vrHistoryIndex].FeatureValue = (float)compositeAllRank[stock.Id];
                }

                //rank each feature
                int featuresCount = filteredStocks[0].Features.Count;
                for (int i = 0; i < featuresCount; i++)
                {
                    if (!featuresEnabled[i])
                        continue;

                    List<Feature> features = new List<Feature>();

                    foreach (Stock stock in filteredStocks)
                    {
                        if (stock.Features.Count > 0)
                            features.Add(stock.Features[i]);
                    }

                    Feature.rankFeatures(features);
                }

                foreach (Stock stock in filteredStocks)
                {
                    if (stock.Features.Count == 0)
                        continue;

                    float finalValue = 0;
                    foreach (FeatureWeight featureWeight in featureWeights)
                    {
                        if (featureWeight.IsEnabled)
                        {
                            finalValue += stock.Features[featureWeight.FeatureIndex].RankedValue * featureWeight.Weight;
                        }
                    }

                    stock.RankValue = finalValue;

                    rankings.Add(stock.Id, new RankingResult(startDate, stock.Id, (decimal)finalValue));
                }

                if (isOptimalRun)
                {
                    CachedFeatures.Instance.AddNewDate(dateInt, yearId, filteredStocks);
                }
                
            } else
            {
                List<CachedFeatures.StockFeature> cachedfeatures = CachedFeatures.Instance.features[id].featureList[yearId];

                foreach (var val in cachedfeatures)
                {
                    float finalValue = 0;
                    foreach (FeatureWeight featureWeight in featureWeights)
                    {
                        if (featureWeight.IsEnabled)
                        {
                            finalValue += val.featurevalues[featureWeight.FeatureIndex] * featureWeight.Weight;
                        }
                    }
                    rankings.Add(val.IdStock, new RankingResult(startDate, val.IdStock, (decimal)finalValue));
                }
            }
            
            if (rankings.Count == 0)
            {
                return null;
            }


            //finally calc ranking from 0 to 1 for each stock
            RankingResult.RankRankingResults(rankings.Values.ToList<RankingResult>());
            if (loadForwardGain)
            {
                //load forward gain for each stock
                DataTable table = DatabaseSingleton.Instance.GetData("select P1.ID_STOCK, (ifnull(cast(P2.CLOSE_PRICE as double), 0) - ifnull(cast(P1.CLOSE_PRICE as double), 0)) / cast(P1.CLOSE_PRICE as double) FORWARD_GAIN FROM PRICE P1, PRICE P2 " +
                                            " WHERE P1.ID_STOCK = P2.ID_STOCK AND P1.DATE = " + Utils.ConvertDateTimeToInt(startDate).ToString() + " AND P2.DATE = " + Utils.ConvertDateTimeToInt(startDate.AddMonths(monthsPeriod)).ToString() + " AND P1.CLOSE_PRICE IS NOT NULL " +
                                            " AND P1.CLOSE_PRICE != 0 ");
                foreach (DataRow row in table.Rows)
                {
                    int idStock = Convert.ToInt32(row["ID_STOCK"]);
                    if (rankings.ContainsKey(idStock))
                    {
                        rankings[idStock].ForwardGain = Convert.ToDecimal(row["FORWARD_GAIN"]);
                    }
                }
            }

            GC.Collect();

            return rankings;
        }


        public Dictionary<int, Dictionary<int, RankingResult>> GenerateScatterPlot(List<Stock> stocks, List<FilterConditions> filtersList, List<FeatureWeight> featureWeights, int monthsPeriod, ProcessingPanelControl pnlProcessing)
        {
            Dictionary<int, Dictionary<int, RankingResult>> rankingResults = new Dictionary<int, Dictionary<int, RankingResult>>();

            //start with the minimun date
            DateTime startDate = new DateTime(2004, 01, 01);

            pnlProcessing.SetMaxValue(12 * 12 / monthsPeriod);

            while (startDate < DateTime.Now)
            {
                pnlProcessing.SetTitle("Generating Scatter Plot... Date: " + startDate.ToString("MM-dd-yyyy"));
                pnlProcessing.PerformStep();

                Dictionary<int, RankingResult> rankings = ProcessRanking(startDate, stocks, filtersList, featureWeights, monthsPeriod, true);

                if (rankings != null)
                    rankingResults.Add(Utils.ConvertDateTimeToInt(startDate), rankings);

                startDate = startDate.AddMonths(monthsPeriod);
            }

            if (ExportDebugData)
            {
                StringBuilder outputStr = new StringBuilder();
                outputStr.Append("Stock\tdate\tweight\tposition\tforwardgain\tsector\n");
                foreach (int date in rankingResults.Keys)
                {
                    foreach (Stock stock in stocks)
                    {
                        if (rankingResults[date].ContainsKey(stock.Id))
                        {
                            RankingResult result = rankingResults[date][stock.Id];
                            outputStr.Append(stock.Symbol);
                            outputStr.Append("\t");
                            outputStr.Append(date.ToString());
                            outputStr.Append("\t");
                            outputStr.Append(result.TotalWeight.ToString());
                            outputStr.Append("\t");
                            outputStr.Append(result.RankPosition.ToString());
                            outputStr.Append("\t");
                            outputStr.Append(result.ForwardGain.ToString());
                            outputStr.Append("\t");
                            outputStr.Append(stock.getStockSector());
                            outputStr.Append("\n");
                        }
                    }
                }

                RankingCalculator.DebugExport = outputStr.ToString();
            }

            return rankingResults;
        }


        //returns a dictionary with a rank position for each stock object
        public static Dictionary<Stock, double> RankStocksByValue(Dictionary<Stock, double> values)
        {
            Dictionary<Stock, double> result = new Dictionary<Stock, double>();
            List<Stock> stocks = new List<Stock>();

            foreach (Stock st in values.Keys)
            {
                st.AuxVal = values[st];
                stocks.Add(st);
            }

            stocks.Sort(delegate (Stock x, Stock y)
            {
                return x.AuxVal.CompareTo(y.AuxVal);
            });

            int totalFeatures = stocks.Count;
            double lastFeatureValue = double.MinValue;
            int lastCurrentFeature = 0;
            for (int currentFeature = 0; currentFeature < totalFeatures; currentFeature++)
            {
                if (lastFeatureValue != stocks[currentFeature].AuxVal)
                {
                    lastFeatureValue = stocks[currentFeature].AuxVal;
                    lastCurrentFeature = currentFeature;
                }

                double rankValue = 1;
                if (totalFeatures != 1)
                    rankValue = (double)lastCurrentFeature / (double)(totalFeatures - 1);

                result.Add(stocks[currentFeature], rankValue);
            }

            return result;
        }


        public Dictionary<Stock, double> GetCompositeUniverseRank(List<Stock> stocks, bool useFilterPrice)
        {
            //rank within all the stocks
            Dictionary<Stock, double> compositesList = new Dictionary<Stock, double>();
            foreach (var st in stocks)
            {
                compositesList.Add(st, -1 * st.CurrentFilters.GetCompositeForPrice(this.Portfolio, useFilterPrice ? (double)st.CurrentFilters.ClosePrice : (double)st.CurrentPrice));
            }

            return RankingCalculator.RankStocksByValue(compositesList);
        }

        public Dictionary<Stock, double> GetCompositeSectorRank(List<Stock> stocks, bool useFilterPrice)
        {
            //rank grouping by sector
            var sectorGroups = new Dictionary<String, Dictionary<Stock, double>>();
            foreach (var st in stocks)
            {
                if (!sectorGroups.ContainsKey(st.getStockSector()))
                    sectorGroups.Add(st.getStockSector(), new Dictionary<Stock, double>());
                var sectorStocksList = sectorGroups[st.getStockSector()];
                sectorStocksList.Add(st, -1*st.CurrentFilters.GetCompositeForPrice(this.Portfolio, useFilterPrice ? (double)st.CurrentFilters.ClosePrice : (double)st.CurrentPrice));
            }

            Dictionary<Stock, double> compositeSectorRank = new Dictionary<Stock, double>();
            foreach (var stocksList in sectorGroups.Values)
            {
                var compositeSingleSectorRank = RankingCalculator.RankStocksByValue(stocksList);
                foreach (var stock in compositeSingleSectorRank.Keys)
                    compositeSectorRank.Add(stock, compositeSingleSectorRank[stock]);
            }

            return compositeSectorRank;
        }

        public Dictionary<int, double> GetCompositeHistoryRank(List<Stock> stocks, int date)
        {
            if (stocks.Count == 0)
                return null;

            Dictionary<int, double> result = new Dictionary<int, double>();

            //read all history rank for the selected stocks
            StringBuilder stocksIds = new StringBuilder();
            List<String> stockIdsList = new List<string>();
            int count = 0;
            foreach (var st in stocks)
            {
                count++;
                if (count > 200)
                {
                    count = 0;
                    stockIdsList.Add("-100" + stocksIds.ToString());
                    stocksIds = new StringBuilder();
                }

                stocksIds.Append(", ");
                stocksIds.Append(st.Id);
            }
            stockIdsList.Add("-100" + stocksIds.ToString());

            int historyId = DataUpdater.GenerateVRHistoryId(Portfolio);
            if (historyId < 0)
                return null;

            foreach (String ids in stockIdsList)
            {
                DataTable tableRes = DatabaseSingleton.Instance.GetData("SELECT ID_STOCK, CAST(RANK_VALUE AS DOUBLE) RANK_VALUE FROM VR_HISTORY_RANKING WHERE VR_HISTORY_ID = " + historyId + " AND DATE = " + date.ToString() + " AND ID_STOCK IN (" + ids +  ")");

                foreach(DataRow row in tableRes.Rows)
                {
                    if(!result.ContainsKey(Convert.ToInt32(row[0])))
                        result.Add(Convert.ToInt32(row[0]), Convert.ToDouble(row[1])/100);
                }
            }

            return result;
        }



    }
}
