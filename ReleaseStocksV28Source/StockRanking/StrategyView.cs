﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using StockRanking.Entities;

namespace StockRanking
{
    public partial class StrategyView : Form
    {
        List<FeatureWeight> featureWeights = new List<FeatureWeight>();
        public static bool CancelResultsProcessing = false;
        public static bool refreshFeatures = false;

        List<CustomFilter> filters = new List<CustomFilter>();
        List<EquationFilter> equfilters = new List<EquationFilter>();
        List<EquationFilterControl> equfiltercontrols = new List<EquationFilterControl>();
        List<DataGridViewColumn> reducedViewColumns = new List<DataGridViewColumn>();
        List<Stock> stocks = null;
        int selectedStrategyId = 0;
        PortfolioParameters portfolioParameters = new PortfolioParameters();
        private Strategy strategy = null;
        bool IsViewReduced = true;
        int FirstFeaturesCell = 7;
        int FeatureColumnsCount = 0;

        bool ExportDebugData = true;
        String exportDebugDataString = "";
        Dictionary<Stock, double> compositeAllRank = new Dictionary<Stock, double>();
        Dictionary<Stock, double> compositeSectorRank = new Dictionary<Stock, double>();
        Dictionary<int, double> compositeHistoryRank = new Dictionary<int, double>();

        public StrategyView(Strategy _strategy)
        {
            InitializeComponent();

            strategy = _strategy;

            if (strategy != null)
                selectedStrategyId = strategy.Id;

            populateFilters();
             
            if (WindowState == FormWindowState.Normal)
            {
                if (Properties.Settings.Default.FormSizeStrategy.Width > 0)
                    Size = Properties.Settings.Default.FormSizeStrategy;
            }

            pnlProcessingProgress.CancelProcess += cancelProcessEvent;
 
            //fill features and weights
            featureWeights = Feature.generateFeatureWeightsList();

            grdFeatures.AutoGenerateColumns = false;
            grdFeatures.DataSource = featureWeights;
            
            Type dgvType = grdStocks.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(grdStocks, true, null);
            
            reducedViewColumns.Add(grdStocks.Columns[0]);
            reducedViewColumns.Add(grdStocks.Columns[1]);
            reducedViewColumns.Add(grdStocks.Columns[2]);
            reducedViewColumns.Add(grdStocks.Columns[3]);
            reducedViewColumns.Add(grdStocks.Columns[4]);
            reducedViewColumns.Add(grdStocks.Columns[5]);

            grdStocks.Columns.Add(addColumn("Mkt Cap"));

            FirstFeaturesCell = 7;

            grdStocks.Columns.Add(addColumn("FCFPS Growth 1 year"));
            grdStocks.Columns.Add(addColumn("FCFPS Growth 3 year"));
            grdStocks.Columns.Add(addColumn("FCFPS Growth 5 year"));
            grdStocks.Columns.Add(addColumn("FCFPS Median 1 year"));
            grdStocks.Columns.Add(addColumn("FCFPS Median 3 year"));
            grdStocks.Columns.Add(addColumn("FCFPS Median 5 year"));

            grdStocks.Columns.Add(addColumn("ROIC Growth 1 year"));
            grdStocks.Columns.Add(addColumn("ROIC Growth 3 year"));
            grdStocks.Columns.Add(addColumn("ROIC Growth 5 year"));
            grdStocks.Columns.Add(addColumn("ROIC Median 1 year"));
            grdStocks.Columns.Add(addColumn("ROIC Median 3 year"));
            grdStocks.Columns.Add(addColumn("ROIC Median 5 year"));

            grdStocks.Columns.Add(addColumn("EBITDA Growth 1 year"));
            grdStocks.Columns.Add(addColumn("EBITDA Growth 3 year"));
            grdStocks.Columns.Add(addColumn("EBITDA Growth 5 year"));
            grdStocks.Columns.Add(addColumn("EBITDA Median 1 year"));
            grdStocks.Columns.Add(addColumn("EBITDA Median 3 year"));
            grdStocks.Columns.Add(addColumn("EBITDA Median 5 year"));

            grdStocks.Columns.Add(addColumn("FIP Score"));
            
            grdStocks.Columns.Add(addColumn("EV Growth 1 year"));
            grdStocks.Columns.Add(addColumn("EV Growth 3 year"));
            grdStocks.Columns.Add(addColumn("EV Growth 5 year"));
            grdStocks.Columns.Add(addColumn("EV Median 1 year"));
            grdStocks.Columns.Add(addColumn("EV Median 3 year"));
            grdStocks.Columns.Add(addColumn("EV Median 5 year"));
            
            grdStocks.Columns.Add(addColumn("BVPS Growth 1 year"));
            grdStocks.Columns.Add(addColumn("BVPS Growth 3 year"));
            grdStocks.Columns.Add(addColumn("BVPS Growth 5 year"));
            grdStocks.Columns.Add(addColumn("BVPS Median 1 year"));
            grdStocks.Columns.Add(addColumn("BVPS Median 3 year"));
            grdStocks.Columns.Add(addColumn("BVPS Median 5 year"));

            grdStocks.Columns.Add(addColumn("CROIC Growth 1 year"));
            grdStocks.Columns.Add(addColumn("CROIC Growth 3 year"));
            grdStocks.Columns.Add(addColumn("CROIC Growth 5 year"));
            grdStocks.Columns.Add(addColumn("CROIC Median 1 year"));
            grdStocks.Columns.Add(addColumn("CROIC Median 3 year"));
            grdStocks.Columns.Add(addColumn("CROIC Median 5 year"));

            grdStocks.Columns.Add(addColumn("MTUM"));

            grdStocks.Columns.Add(addColumn("Sortino Score"));
             
            grdStocks.Columns.Add(addColumn("SALES Growth 1 year"));
            grdStocks.Columns.Add(addColumn("SALES Growth 3 year"));
            grdStocks.Columns.Add(addColumn("SALES Growth 5 year"));
            grdStocks.Columns.Add(addColumn("SALES Median 1 year"));
            grdStocks.Columns.Add(addColumn("SALES Median 3 year"));
            grdStocks.Columns.Add(addColumn("SALES Median 5 year"));

            grdStocks.Columns.Add(addColumn("PEG RATIO"));
            grdStocks.Columns.Add(addColumn("EBITDA / Liabilities"));
            grdStocks.Columns.Add(addColumn("GP / Assets"));
            grdStocks.Columns.Add(addColumn("FCF / Sales"));
            grdStocks.Columns.Add(addColumn("EV / EBITDA"));
            grdStocks.Columns.Add(addColumn("Close / FCFPS"));
            grdStocks.Columns.Add(addColumn("EV / Revenue"));
            grdStocks.Columns.Add(addColumn("Dividend + BuyBackRate"));
            grdStocks.Columns.Add(addColumn("Operating Margin"));
            grdStocks.Columns.Add(addColumn("ROIC / (EV / EBITDA)"));

            grdStocks.Columns.Add(addColumn("ROIC Score"));
            grdStocks.Columns.Add(addColumn("CROIC Score"));
            grdStocks.Columns.Add(addColumn("FCFPS Score"));
            grdStocks.Columns.Add(addColumn("EBITDA Score"));
            grdStocks.Columns.Add(addColumn("SALES Score"));

            grdStocks.Columns.Add(addColumn("VR Universe Rank"));
            grdStocks.Columns.Add(addColumn("VR Sector Rank"));
            grdStocks.Columns.Add(addColumn("VR History Rank"));

            grdStocks.Columns.Add(addColumn("Sharpe FIP"));

            grdStocks.Columns.Add(addColumn("Value Est."));
            grdStocks.Columns.Add(addColumn("% UnderOver"));
            grdStocks.Columns.Add(addColumn("VR Universe"));
            grdStocks.Columns.Add(addColumn("VR Sector"));
            grdStocks.Columns.Add(addColumn("VR History"));

            DataGridViewColumn col = addColumn("Total Rank");
            reducedViewColumns.Add(col);
            grdStocks.Columns.Add(col);

            FeatureColumnsCount = grdStocks.ColumnCount;

            if(strategy != null)
                loadStrategyData(strategy);

            grdFeatures.Refresh();
            RefreshViewCols();

            portfolioParameters = PortfolioParameters.Load(selectedStrategyId);

            grdFeatures.Update();
            grdFeatures.Refresh();
        }

        private void populateFilters()
        {
            List<FilterConditions> filtersList = new List<FilterConditions>();
            foreach (CustomFilter filter in filters)
            {
                if (filter.IsActive)
                {
                    filtersList.Add(filter.GetFilterConditions());
                }
            }
            foreach (EquationFilterControl filter in equfiltercontrols)
            {
                if (filter.IsActive)
                {
                    filtersList.Add(filter.GetFilterConditions());
                }
            }

            pnlFilters.Controls.Clear();

            int filterPosition = 3;
            filters.Clear();
            equfiltercontrols.Clear();
            equfilters.Clear();
            foreach (FilterTypes filterType in Enum.GetValues(typeof(FilterTypes)))
            {
                filters.Add(createNewFilterControl(filterType, filterPosition));
                filterPosition += 25;
            }

            equfilters = EquationFilter.GetAllFilters();
            foreach (EquationFilter eqf in equfilters)
            {
                equfiltercontrols.Add(createNewEquFilterControl(eqf, filterPosition));
                filterPosition += 50;
            }

            
            foreach (CustomFilter filter in filters)
            {
                FilterConditions filterFound = filtersList.Find(x => (x.FilterType == filter.FilterType) && (x.isCustom == 0));

                if (filterFound == null)
                {
                    filter.SetDisabled();
                    continue;
                }

                filter.FillConditions(filterFound);
            }

            foreach (EquationFilterControl efilter in equfiltercontrols)
            {
                FilterConditions filterFound = filtersList.Find(x => (x.isCustom == efilter.equFilter.Id) && (x.isCustom > 0));

                if (filterFound == null)
                {
                    efilter.SetDisabled();
                    continue;
                }

                efilter.FillConditions(filterFound);
            }
        }


        EquationFilterControl createNewEquFilterControl(EquationFilter _filter, int filterPosition)
        {
            EquationFilterControl fcontrol= new EquationFilterControl(_filter);
            fcontrol.OnChangedHandler += this.EquationFilterChanged;
            fcontrol.Location = new System.Drawing.Point(3, filterPosition);
            fcontrol.Size = new System.Drawing.Size(800, 50);
            fcontrol.TabIndex = 0;
            fcontrol.TabStop = false;

            pnlFilters.Controls.Add(fcontrol);

            return fcontrol;
        }

        private void EquationFilterChanged(object sender, EventArgs e) 
        {
            populateFilters();
        }

        CustomFilter createNewFilterControl(FilterTypes filterType, int filterPosition)
        {
            CustomFilter filter = new CustomFilter();

            filter.FilterName = Enum.GetName(typeof(FilterTypes), filterType);
            filter.FilterType = filterType;
            filter.Location = new System.Drawing.Point(3, filterPosition);
            filter.Name = "filter" + (int)filterType;
            filter.Size = new System.Drawing.Size(800, 26);
            filter.TabIndex = 0;
            filter.TabStop = false;

            pnlFilters.Controls.Add(filter);

            return filter;
        }

        void loadStrategyData(Strategy strategy)
        {
            txtStrategyName.Text = strategy.Name;

            for (int i = 0; i < featureWeights.Count; i++)
            {
                FeatureWeight fw = featureWeights[i];

                fw.IsEnabled = false;
                fw.Weight = 0;

                try
                {
                    Feature featureFound = strategy.Features.Find(x => x.FeatureId == i);

                    if (featureFound == null)
                        continue;

                    fw.Weight = (float)featureFound.FeatureWeight;

                    if (fw.Weight != 0)
                        fw.IsEnabled = true;
                }
                catch (Exception)
                { }
            }


            foreach (CustomFilter filter in filters)
            {
                FilterConditions filterFound = strategy.Filters.Find(x => (x.FilterType == filter.FilterType) && (x.isCustom == 0));

                if (filterFound == null)
                {
                    filter.SetDisabled();
                    continue;
                }

                filter.FillConditions(filterFound);
            }
            foreach (EquationFilterControl efilter in equfiltercontrols)
            {
                FilterConditions filterFound = strategy.Filters.Find(x => (x.isCustom == efilter.equFilter.Id) && (x.isCustom > 0));

                if (filterFound == null)
                {
                    efilter.SetDisabled();
                    continue;
                }

                efilter.FillConditions(filterFound);
            }

        }

        private void cmdProcessFeatures_Click(object sender, EventArgs e)
        {
            RankStocksPopup popup = new RankStocksPopup();
            if (popup.ShowDialog() != DialogResult.OK)
                return;

            if (DataUpdater.CheckDatabaseBloqued())
                return;


            grdStocks.Rows.Clear();
            
            if (RankStocksPopup.SelectedIndex == 0)
            {
                processStocksRanking();
            }

            if (RankStocksPopup.SelectedIndex == 1)
            {
                processIndustriesRanking(false);
            }

            if (RankStocksPopup.SelectedIndex == 2)
            {
                processIndustriesRanking(true);
            }

        }

        private void processStocksRanking()
        {  
            CancelResultsProcessing = false;
            
            ShowProcessing("Stop Process", "Reading Database...", 55);
            
            List<RankingResult> rankingResults;
            Dictionary<int, Stock> stocksIndexed;

            DateTime processDate = new DateTime(RankStocksPopup.SelectedDate.Year, RankStocksPopup.SelectedDate.Month, 1);
            RankingCalculator rankingCalculator;

            if (!rankStocks(out rankingResults, out stocksIndexed, processDate, out rankingCalculator))
            {
                HideProcessing();
                return;
            }
            
            StringBuilder outputStr = new StringBuilder();

#region export debug data
            if (ExportDebugData)
            {
                outputStr.Append("STOCK");
                outputStr.Append("\tClosePrice");
                outputStr.Append("\tCloseJan1");
                outputStr.Append("\tVolume");
                outputStr.Append("\tAvgVolume");
                outputStr.Append("\tCurrRatio");
                outputStr.Append("\tDebtToEbit");
                outputStr.Append("\tMktCap");
                outputStr.Append("\tPositiveEbitYears");
                outputStr.Append("\tDividendYield");
                outputStr.Append("\tBuyBackRate");
                outputStr.Append("\tDebtReduction");
                outputStr.Append("\tQoQEarnings");
                outputStr.Append("\tEV Ebitda");
                outputStr.Append("\tROIC Score");
                outputStr.Append("\tCROIC Score");
                outputStr.Append("\tFCF Score");
                outputStr.Append("\tEBITDA Score\t");

                outputStr.Append("FCF Growth 1 year VALUE\tRANKED\t");
                outputStr.Append("FCF Growth 3 year VALUE\tRANKED\t");
                outputStr.Append("FCF Growth 5 year VALUE\tRANKED\t");
                outputStr.Append("FCF Median 1 year VALUE\tRANKED\t");
                outputStr.Append("FCF Median 3 year VALUE\tRANKED\t");
                outputStr.Append("FCF Median 5 year VALUE\tRANKED\t");

                outputStr.Append("ROIC Growth 1 year VALUE\tRANKED\t");
                outputStr.Append("ROIC Growth 3 year VALUE\tRANKED\t");
                outputStr.Append("ROIC Growth 5 year VALUE\tRANKED\t");
                outputStr.Append("ROIC Median 1 year VALUE\tRANKED\t");
                outputStr.Append("ROIC Median 3 year VALUE\tRANKED\t");
                outputStr.Append("ROIC Median 5 year VALUE\tRANKED\t");

                outputStr.Append("EBITDA Growth 1 year VALUE\tRANKED\t");
                outputStr.Append("EBITDA Growth 3 year VALUE\tRANKED\t");
                outputStr.Append("EBITDA Growth 5 year VALUE\tRANKED\t");
                outputStr.Append("EBITDA Median 1 year VALUE\tRANKED\t");
                outputStr.Append("EBITDA Median 3 year VALUE\tRANKED\t");
                outputStr.Append("EBITDA Median 5 year VALUE\tRANKED\t");

                outputStr.Append("FIP Score VALUE\tRANKED\t");

                outputStr.Append("EV Growth 1 year VALUE\tRANKED\t");
                outputStr.Append("EV Growth 3 year VALUE\tRANKED\t");
                outputStr.Append("EV Growth 5 year VALUE\tRANKED\t");
                outputStr.Append("EV Median 1 year VALUE\tRANKED\t");
                outputStr.Append("EV Median 3 year VALUE\tRANKED\t");
                outputStr.Append("EV Median 5 year VALUE\tRANKED\t");

                outputStr.Append("BVPS Growth 1 year VALUE\tRANKED\t");
                outputStr.Append("BVPS Growth 3 year VALUE\tRANKED\t");
                outputStr.Append("BVPS Growth 5 year VALUE\tRANKED\t");
                outputStr.Append("BVPS Median 1 year VALUE\tRANKED\t");
                outputStr.Append("BVPS Median 3 year VALUE\tRANKED\t");
                outputStr.Append("BVPS Median 5 year VALUE\tRANKED\t");

                outputStr.Append("CROIC Growth 1 year VALUE\tRANKED\t");
                outputStr.Append("CROIC Growth 3 year VALUE\tRANKED\t");
                outputStr.Append("CROIC Growth 5 year VALUE\tRANKED\t");
                outputStr.Append("CROIC Median 1 year VALUE\tRANKED\t");
                outputStr.Append("CROIC Median 3 year VALUE\tRANKED\t");
                outputStr.Append("CROIC Median 5 year VALUE\tRANKED\t");

                outputStr.Append("MTUM VALUE\tRANKED\t");
                outputStr.Append("SORTINO FIP VALUE\tRANKED\t");

                outputStr.Append("SALES Growth 1 year VALUE\tRANKED\t");
                outputStr.Append("SALES Growth 3 year VALUE\tRANKED\t");
                outputStr.Append("SALES Growth 5 year VALUE\tRANKED\t");
                outputStr.Append("SALES Median 1 year VALUE\tRANKED\t");
                outputStr.Append("SALES Median 3 year VALUE\tRANKED\t");
                outputStr.Append("SALES Median 5 year VALUE\tRANKED\t");


                outputStr.Append("ROIC Score\tRANKED\t");
                outputStr.Append("CROIC Score\tRANKED\t");
                outputStr.Append("FCF Score\tRANKED\t");
                outputStr.Append("EBITDA Score\tRANKED\t");
                outputStr.Append("SALES Score\tRANKED\t");

                outputStr.Append("VR UNIVERSE\tRANKED\t");
                outputStr.Append("VR SECTOR\tRANKED\t");
                outputStr.Append("VR HISTORY\tRANKED\t");

                outputStr.Append("SHARPE FIP Score\tRANKED\t");

                outputStr.Append("PE Price\t");
                outputStr.Append("PFCF Price\t");
                outputStr.Append("PS Price\t");
                outputStr.Append("PB Price\t");

                outputStr.Append("PE %\t");
                outputStr.Append("PFCF %\t");
                outputStr.Append("PS %\t");
                outputStr.Append("PB %\t");

                outputStr.Append("TOTAL RANK");

                outputStr.Append("\n");

            }
#endregion export debug data

            List<Stock> stocks = new List<Stock>();
            foreach (var result in rankingResults)
                stocks.Add(stocksIndexed[result.IdStock]);
            compositeAllRank = rankingCalculator.GetCompositeUniverseRank(stocks, true);
            compositeSectorRank = rankingCalculator.GetCompositeSectorRank(stocks, true);
            compositeHistoryRank = rankingCalculator.GetCompositeHistoryRank(stocks, Utils.ConvertDateTimeToInt(processDate));

            while (grdStocks.ColumnCount > FeatureColumnsCount)
            {
                grdStocks.Columns.RemoveAt(FeatureColumnsCount);
            }

            List<FilterConditions> filtersList = new List<FilterConditions>();
            foreach (CustomFilter filter in filters)
            {
                if (filter.IsActive)
                {
                    filtersList.Add(filter.GetFilterConditions());
                    if (filter.FilterType == FilterTypes.MovingAverage)
                    {
                        grdStocks.Columns.Add(addColumnWithTag("ClosePrice - MovingAverage", Enum.GetName(typeof(FilterTypes), filter.FilterType)));
                    } else if (filter.FilterType == FilterTypes.RoCVSBenchmark)
                    {
                        grdStocks.Columns.Add(addColumnWithTag("RoC - SPYRoC", Enum.GetName(typeof(FilterTypes), filter.FilterType)));
                    } else
                    {
                        grdStocks.Columns.Add(addColumnWithTag(Enum.GetName(typeof(FilterTypes), filter.FilterType), Enum.GetName(typeof(FilterTypes), filter.FilterType)));
                    }
                }
            }
            foreach (EquationFilterControl filter in equfiltercontrols)
            {
                if (filter.IsActive)
                {
                    if (filter.equFilter.FilterType == 3)
                    {
                        grdStocks.Columns.Add(addColumnWithTag(filter.equFilter.FilterName + " (Left - Right)", filter.equFilter.Id.ToString()));
                    } else
                    {
                        grdStocks.Columns.Add(addColumnWithTag(filter.equFilter.FilterName, filter.equFilter.Id.ToString()));
                    }
                    filtersList.Add(filter.GetFilterConditions());
                }
            }

            foreach (RankingResult results in rankingResults)
            {
                //add items to the list
                Stock stock = stocksIndexed[results.IdStock];
                int n = grdStocks.Rows.Add();
                
                int mcell = 3;

                grdStocks.Rows[n].Cells[0].Value = stock.Symbol;
                grdStocks.Rows[n].Tag = stock;
                grdStocks.Rows[n].Cells[1].Value = stock.CompanyName;
                grdStocks.Rows[n].Cells[2].Value = stock.getStockSector();

                grdStocks.Rows[n].Cells[3].Value = stock.CurrentFilters.ClosePrice;
                grdStocks.Rows[n].Cells[4].Value = stock.CurrentFilters.GetYTD();
                grdStocks.Rows[n].Cells[5].Value = stock.CurrentFilters.Volume;
                grdStocks.Rows[n].Cells[6].Value = stock.CurrentFilters.MktCap;

                int cell = FirstFeaturesCell;
                foreach (Feature feature in stock.Features)
                    grdStocks.Rows[n].Cells[cell++].Value = feature.RankedValue;

                try
                {
                    //add new composite values
                    grdStocks.Rows[n].Cells[cell++].Value = stock.CurrentFilters.GetCompositePrice(portfolioParameters);
                    grdStocks.Rows[n].Cells[cell++].Value = stock.CurrentFilters.GetComposite(portfolioParameters) * -1;
                    grdStocks.Rows[n].Cells[cell++].Value = Math.Round(compositeAllRank[stock] * 100, 2);
                    grdStocks.Rows[n].Cells[cell++].Value = Math.Round(compositeSectorRank[stock] * 100, 2);
                    if(compositeHistoryRank.ContainsKey(stock.Id))
                        grdStocks.Rows[n].Cells[cell++].Value = Math.Round(compositeHistoryRank[stock.Id] * 100, 2);
                    else
                        grdStocks.Rows[n].Cells[cell++].Value = (double)0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(stock.Id + " - " + stock.Symbol);
                }

                grdStocks.Rows[n].Cells[cell].Value = results.RankPosition;

                foreach (DataGridViewColumn col in grdStocks.Columns)
                {
                    if (col.Index < FeatureColumnsCount) continue;
                    grdStocks.Rows[n].Cells[col.Index].Value = stock.filterResults[col.Tag.ToString()];
                }



#region export debug data

                if (ExportDebugData)
                {

                    outputStr.Append(stock.Symbol);

                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.ClosePrice.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.CloseJan1.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.Volume.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.AvgVolume.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.CurrRatio.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.DebtToEbit.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.MktCap.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.PositiveEbitYears.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.ShareholdersYield.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.BuyBackRate.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.DebtReduction.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.QoQEarnings.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.EvEBITDA.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.RoicScore.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.CroicScore.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.FcfScore.ToString("n4"));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.EbitdaScore.ToString("n4"));

                    outputStr.Append("\t");
                    foreach (Feature feature in stock.Features)
                    {
                        outputStr.Append(feature.FeatureValue.ToString("n7"));
                        outputStr.Append("\t");
                        outputStr.Append(feature.RankedValue.ToString("n5"));
                        outputStr.Append("\t");
                    }

                    outputStr.Append(stock.CurrentFilters.PE);
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.PFCF);
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.PS);
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.PB);
                    outputStr.Append("\t");

                    outputStr.Append(stock.CurrentFilters.GetPriceOverPercent(stock.CurrentFilters.PE));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.GetPriceOverPercent(stock.CurrentFilters.PFCF));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.GetPriceOverPercent(stock.CurrentFilters.PS));
                    outputStr.Append("\t");
                    outputStr.Append(stock.CurrentFilters.GetPriceOverPercent(stock.CurrentFilters.PB));
                    outputStr.Append("\t");

                    outputStr.Append(results.RankPosition.ToString("n4"));

                    outputStr.Append("\n");

                }
#endregion export debug data



            }

            if (ExportDebugData)
            {
                exportDebugDataString = outputStr.ToString();
            }

            grdStocks.Enabled = true;
            
            RefreshViewCols();

            HideProcessing();

        }

        bool rankStocks(out List<RankingResult> rankingResults, out Dictionary<int, Stock> stocksIndexed, DateTime processDate, out RankingCalculator rankingCalculator)
        {
            rankingResults = new List<RankingResult>();
            stocksIndexed = new Dictionary<int, Stock>();

            rankingCalculator = null;

            try
            {
                //read all the data from the database and calc the features
                pnlProcessingProgress.SetMaxValue(100);
                if (stocks == null)
                    stocks = Stock.ReadStockSymbols(true, pnlProcessingProgress);

                if (CancelResultsProcessing)
                {
                    return false;
                }

                pnlProcessingProgress.SetTitle("Performing Features Calculation...");
                Application.DoEvents();

                Stock.UpdateCurrentValues(processDate, stocks);

                rankingCalculator = new RankingCalculator(portfolioParameters);

                List<FilterConditions> filtersList = new List<FilterConditions>();
                foreach (CustomFilter filter in filters)
                {
                    if (filter.IsActive)
                        filtersList.Add(filter.GetFilterConditions());
                }
                foreach (EquationFilterControl filter in equfiltercontrols)
                {
                    if (filter.IsActive)
                        filtersList.Add(filter.GetFilterConditions());
                }
                Dictionary<int, RankingResult> rankedStocks = rankingCalculator.ProcessRanking(processDate, stocks, filtersList, featureWeights, 12, false);

                if (rankedStocks == null)
                    rankedStocks = new Dictionary<int, RankingResult>();

                rankingResults = rankedStocks.Values.ToList().OrderByDescending(x => x.RankPosition).ToList();
                stocksIndexed = stocks.ToDictionary(x => x.Id);

            }
            catch (Exception ex)
            {
                String errorText = "";

                errorText = ex.Message;
                errorText += "\n\r\n\r";
                errorText += "STACK: " + ex.StackTrace;

                Exception innerEx = ex;
                while (innerEx.InnerException != null)
                {
                    innerEx = innerEx.InnerException;

                    errorText += "\n\r\n\r";
                    errorText += "INNER EXCEPTION: ";
                    errorText += innerEx.Message;
                    errorText += "\n\r\n\r";
                    errorText += "STACK: " + innerEx.StackTrace;
                }

                pnlProcessingProgress.ShowError(errorText);
            }

            return true;
        }

        private void cmdRankSectors_Click(object sender, EventArgs e)
        { }
        
        private void processIndustriesRanking(bool groupBySectors)
        { 
            CancelResultsProcessing = false;

            ShowProcessing("Stop Process", "Reading Database...", 55);

            List<RankingResult> rankingResults;
            Dictionary<int, Stock> stocksIndexed;

            DateTime processDate = new DateTime(RankStocksPopup.SelectedDate.Year, RankStocksPopup.SelectedDate.Month, 1);
            RankingCalculator rankingCalculator;

            if (!rankStocks(out rankingResults, out stocksIndexed, processDate, out rankingCalculator))
            {
                HideProcessing();
                return;
            }

            //group stocks by industry
            Dictionary<String, IndustryGroup> industries = new Dictionary<string, IndustryGroup>();
            int featuresCount = 0;
            foreach (RankingResult results in rankingResults)
            {
                Stock stock = stocksIndexed[results.IdStock];

                featuresCount = stock.Features.Count;

                if (!industries.ContainsKey(stock.getStockIndustry(groupBySectors)))
                {
                    IndustryGroup indGroup = new IndustryGroup(stock.getStockIndustry(groupBySectors));
                    for (int i = 0; i < stock.Features.Count; i++)
                        indGroup.featureSum.Add(0);

                    industries.Add(stock.getStockIndustry(groupBySectors), indGroup);
                }

                IndustryGroup ind = industries[stock.getStockIndustry(groupBySectors)];
                ind.count++;

                for (int i = 0; i < stock.Features.Count; i++)
                {
                    ind.featureSum[i] += stock.Features[i].RankedValue;
                }

                ind.rankingSum += (double)results.RankPosition;
                ind.closingPriceSum += stock.CurrentFilters.ClosePrice;
                ind.ytdSum += stock.CurrentFilters.GetYTD();
                ind.volumeSum += stock.CurrentFilters.Volume;
                ind.marketCapSum += stock.CurrentFilters.MktCap;
            }

            foreach (IndustryGroup industry in industries.Values)
            {
                //add items to the list
                int n = grdStocks.Rows.Add();

                grdStocks.Rows[n].Cells[0].Value = "";
                grdStocks.Rows[n].Tag = industry;
                grdStocks.Rows[n].Cells[1].Value = industry.industry;
                if(groupBySectors)
                    grdStocks.Rows[n].Cells[2].Value = industry.industry;
                else
                    grdStocks.Rows[n].Cells[2].Value = IndustryGroup.GetSector(industry.industry);

                grdStocks.Rows[n].Cells[3].Value = Math.Round(industry.closingPriceSum / industry.count, 4);
                grdStocks.Rows[n].Cells[4].Value = Math.Round(industry.ytdSum / industry.count, 2);
                grdStocks.Rows[n].Cells[5].Value = Math.Round(industry.volumeSum / industry.count, 0);
                grdStocks.Rows[n].Cells[6].Value = Math.Round(industry.marketCapSum / industry.count, 0);

                int cell = FirstFeaturesCell;
                for(int i = 0; i < featuresCount; i++)
                    grdStocks.Rows[n].Cells[cell++].Value = Math.Round(industry.featureSum[i] / industry.count, 2);

                //add new composite values
                grdStocks.Rows[n].Cells[cell++].Value = 0;
                grdStocks.Rows[n].Cells[cell++].Value = 0;
                grdStocks.Rows[n].Cells[cell++].Value = 0;
                grdStocks.Rows[n].Cells[cell++].Value = 0;
                grdStocks.Rows[n].Cells[cell++].Value = 0;

                grdStocks.Rows[n].Cells[cell].Value = Math.Round(industry.rankingSum / industry.count, 4);
            }
            
            grdStocks.Enabled = true;
            
            RefreshViewCols();

            HideProcessing();

        }
         
        DataGridViewColumn addColumn(String colName)
        {
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.HeaderText = colName;
            column.Width = 70;
            column.MinimumWidth = 70;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DefaultCellStyle.Format = "n2";

            return column;
        }

        DataGridViewColumn addColumnWithTag(String colName, String tag)
        {
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.HeaderText = colName;
            column.Width = 80;
            column.Tag = tag;
            column.MinimumWidth = 80;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DefaultCellStyle.Format = "n5";

            return column;
        }

        private void lstStocks_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            
        }

        private void cmdRefreshData_Click(object sender, EventArgs e)
        {
            //RefreshData();
        }

        private void frmMainUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            ZachsSourceReader.CancelProcess = true;
            StockSourcesReader.CancelProcess = true;
            BacktestCalculator.CancelProcess = true;
            StrategyView.CancelResultsProcessing = true;
        }

        private void cmdExport_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(export_DataGridView(grdStocks));
            MessageBox.Show("Data copied to clipboard.");
        }


        private string export_DataGridView(DataGridView grid)
        {
            StringBuilder output = new StringBuilder();

            // Write Column titles
            bool firstCol = true;

            for (int i = 0; i < grid.Columns.Count; i++)
            {

                //always hide disabled feature columns
                if (i - FirstFeaturesCell < featureWeights.Count && i - FirstFeaturesCell >= 0)
                {
                    if (!featureWeights[i - FirstFeaturesCell].IsEnabled ||
                        featureWeights[i - FirstFeaturesCell].Weight == 0)
                    {
                        continue;
                    }
                }

                if (!firstCol)
                    output.Append("\t");

                output.Append(grid.Columns[i].HeaderText);

                firstCol = false;
                 
                if (i == FirstFeaturesCell-1)
                {
                    //append filter values
                    output.Append("\tAvg Volume\tDebt To EBITDA\tPositive EBITDA Years\tCurrent Ratio\tBuy Back Rate\tDebt Reduction\tQoQ Earning Change");
                }
                 
            }

            output.Append("\n");

            // Write Rows
            foreach (DataGridViewRow row in grid.Rows)
            {
                firstCol = true;

                for (int i = 0; i < grid.Columns.Count; i++)
                {
                   
                    //always hide disabled feature columns
                    if (i - FirstFeaturesCell < featureWeights.Count && i - FirstFeaturesCell >= 0)
                    {
                        if (!featureWeights[i - FirstFeaturesCell].IsEnabled ||
                            featureWeights[i - FirstFeaturesCell].Weight == 0)
                        {
                            continue;
                        }
                    }


                    if (!firstCol)
                        output.Append("\t");


                    if (row.Cells[i].Value != null)
                    {
                        if (row.Cells[i].Value.GetType() == typeof(DateTime))
                            output.Append(((DateTime)row.Cells[i].Value).ToString("MM/dd/yyyy"));
                        else if (row.Cells[i].Value.GetType() == typeof(double))
                            output.Append(((double)row.Cells[i].Value).ToString("n4"));
                        else
                            output.Append(row.Cells[i].Value.ToString().Replace("\r", " ").Replace("\n", " "));
                    }

                    if (i == FirstFeaturesCell - 1)
                    {
                        
                        //append filter values
                        Stock stock = (Stock)row.Tag;
                        output.Append("\t");
                        output.Append(stock.CurrentFilters.AvgVolume.ToString());
                        output.Append("\t");
                        output.Append(stock.CurrentFilters.DebtToEbit.ToString());
                        output.Append("\t");
                        output.Append(stock.CurrentFilters.PositiveEbitYears.ToString());
                        output.Append("\t");
                        output.Append(stock.CurrentFilters.CurrRatio.ToString());
                        output.Append("\t");
                        output.Append(stock.CurrentFilters.BuyBackRate.ToString());
                        output.Append("\t");
                        output.Append(stock.CurrentFilters.DebtReduction.ToString());
                        output.Append("\t");
                        output.Append(stock.CurrentFilters.QoQEarnings.ToString());
                        
                    }

                    firstCol = false;
                }

                output.Append("\n");
            }

            return output.ToString();
        }
         
        private void cancelProcessEvent(object sender, EventArgs e)
        {
            ZachsSourceReader.CancelProcess = true;
            StockSourcesReader.CancelProcess = true;
            CancelResultsProcessing = true;
        }

        void ShowProcessing(String cmdText, String firstTitle, int maxValue)
        {
            pnlProcessingProgress.StartProcess();
            pnlProcessingProgress.SetTitle(firstTitle);
            pnlProcessingProgress.SetCommand(cmdText);
            pnlProcessingProgress.SetMaxValue(maxValue);
        }

        void HideProcessing()
        {
            pnlProcessingProgress.StopProcess();
        }

        private void chkClosingPrice_CheckedChanged(object sender, EventArgs e)
        {

        } 

        private void cmdSwitchView_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            IsViewReduced = !IsViewReduced;

            if(IsViewReduced)
            {
                cmdSwitchView.Text = "Switch to Full View";
                grdStocks.Columns[0].Frozen = false;
                grdStocks.Columns[1].Frozen = false;
                grdStocks.Columns[2].Frozen = false;
                grdStocks.Columns[3].Frozen = false;
                grdStocks.Columns[4].Frozen = false;
            }
            else
            {
                cmdSwitchView.Text = "Switch to Reduced View";
                grdStocks.Columns[0].Frozen = true;
                grdStocks.Columns[1].Frozen = true;
                grdStocks.Columns[2].Frozen = true;
                grdStocks.Columns[3].Frozen = true;
                grdStocks.Columns[4].Frozen = true;
            }

            RefreshViewCols();
        }

        void RefreshViewCols()
        {
            foreach (DataGridViewColumn column in grdStocks.Columns)
            {
                column.Visible = !IsViewReduced;

                if(reducedViewColumns.Contains(column))
                    column.Visible = true;

                //always disable disabled feature columns
                if(column.Index - FirstFeaturesCell < featureWeights.Count && column.Index - FirstFeaturesCell >= 0)
                {
                    if (!featureWeights[column.Index - FirstFeaturesCell].IsEnabled ||
                        featureWeights[column.Index - FirstFeaturesCell].Weight == 0)
                        column.Visible = false;
                }
            }
        }

        private void cmdResetFeatures_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (FeatureWeight weight in featureWeights)
            {
                weight.IsEnabled = true;
                weight.Weight = 1;
            }


            grdFeatures.Refresh();
            RefreshFeaturesGrid();
        }

        private void grdFeatures_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            grdFeatures.RefreshEdit();
        }

        private void grdFeatures_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!(grdFeatures.CurrentCell is DataGridViewCheckBoxCell))
                return;
            
            DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)grdFeatures.CurrentCell;

            if (cell != null && !cell.ReadOnly)
            {
                cell.Value = cell.Value == null || !((bool)cell.Value);
                grdFeatures.RefreshEdit();
                grdFeatures.NotifyCurrentCellDirty(true);
            }

            RefreshFeaturesGrid();
        }

        void RefreshFeaturesGrid()
        {
            foreach(DataGridViewRow row in grdFeatures.Rows)
            {
                if((bool)row.Cells[0].Value == true)
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.Cells[2].ReadOnly = false;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.ForeColor = Color.Gray;
                    row.Cells[2].ReadOnly = true;
                }
            }
        }

        private void grdFeatures_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 2) 
            {
                try
                {
                    Convert.ToDecimal(e.FormattedValue);
                }
                catch(Exception)
                {
                    MessageBox.Show("Please write a numeric value", "Conversion error");
                    e.Cancel = true;
                }
            }
            
        }

        private void frmMainUI_Shown(object sender, EventArgs e)
        {

            RefreshFeaturesGrid();

        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            //create strategy object and save it into the db
            Strategy strategy = createStrategyObject();

            strategy.Save();

            this.DialogResult = DialogResult.OK;

            MessageBox.Show("Strategy saved correctly.", "Save Strategy");

            this.Close();
        }

        private Strategy createStrategyObject()
        {
            Strategy strategy = new Strategy();

            strategy.Name = txtStrategyName.Text;
            strategy.Id = selectedStrategyId;
            strategy.PortfolioParameters = portfolioParameters;

            foreach (FeatureWeight weight in (List<FeatureWeight>)grdFeatures.DataSource)
            {
                if (weight.IsEnabled)
                {
                    strategy.Features.Add(new Feature(weight));
                }
            }

            foreach (CustomFilter filter in filters)
            {
                if (filter.IsActive)
                {
                    strategy.Filters.Add(filter.GetFilterConditions());
                }
            }
            foreach (EquationFilterControl filter in equfiltercontrols)
            {
                if (filter.IsActive)
                    strategy.Filters.Add(filter.GetFilterConditions());
            }
            return strategy;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdBacktest_Click(object sender, EventArgs e)
        {
            BacktestPopup popup = new BacktestPopup();
            if (popup.ShowDialog() != DialogResult.OK)
                return;


            if (DataUpdater.CheckDatabaseBloqued())
                return;


            Strategy strategy = createStrategyObject();
            List<FeatureWeight> featuresW = Feature.generateFeatureWeightsList();
            
            for (int i = 0; i < featuresW.Count; i++)
            {
                FeatureWeight fw = featuresW[i];

                fw.IsEnabled = false;
                fw.Weight = 0;

                try
                {
                    Feature featureFound = strategy.Features.Find(x => x.FeatureId == i);

                    if (featureFound == null)
                        continue;

                    fw.Weight = (float)featureFound.FeatureWeight;

                    if (fw.Weight != 0)
                        fw.IsEnabled = true;
                }
                catch (Exception)
                { }
            }

            portfolioParameters.RebalanceFrequencyMonths = BacktestPopup.RebalancingInterval;
            
            BacktestCalculator backtest = new BacktestCalculator(portfolioParameters, featuresW, strategy.Filters);
            BacktestResults newView = new BacktestResults(backtest);
            newView.ShowDialog();

        }

        private void cmdExportDebug_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(exportDebugDataString);
        }

        private void StrategyView_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.FormSizeStrategy = Size;
                Properties.Settings.Default.Save();
            }
        }


        private void cmdScatterPlot_Click(object sender, EventArgs e)
        {
            ScatterPlotPopup popup = new ScatterPlotPopup();
            if (popup.ShowDialog() != DialogResult.OK)
                return;


            if (DataUpdater.CheckDatabaseBloqued())
                return;


            Dictionary<int, Dictionary<int, RankingResult>> rankingResults = getRankingResultsForScatterplot();

            if (CancelResultsProcessing)
            {
                HideProcessing();
                return;
            }

            ScatterPlot scatterForm = null;
            if (ScatterPlotPopup.SelectedIndex == 0)
            {
                scatterForm = new ScatterPlot(rankingResults, ScatterPlotPopup.RebalancingInterval < 3 && ScatterPlotPopup.SelectedIndex == 0);
                scatterForm.ShowDialog();

                HideProcessing();
                return;
            }
             
            Dictionary<int, Dictionary<int, RankingResult>> groupedResults = new Dictionary<int, Dictionary<int, RankingResult>>();

            bool groupBySectors = (ScatterPlotPopup.SelectedIndex == 2);
            
            //summarize by sector
            Dictionary<int, Stock> stocksIndexed = stocks.ToDictionary(x => x.Id);
            
            foreach (int rankResultGroupKey in rankingResults.Keys)
            {
                int auxId = 0;
                Dictionary<int, RankingResult> rankResultGroup = rankingResults[rankResultGroupKey];
                Dictionary<String, IndustryGroup> industryGroups = new Dictionary<String, IndustryGroup>();

                foreach (RankingResult rank in rankResultGroup.Values)
                {
                    if (Math.Abs(rank.ForwardGain) > 30)
                        continue;

                    Stock stock = stocksIndexed[rank.IdStock];
                    
                    if (!industryGroups.ContainsKey(stock.getStockIndustry(groupBySectors)))
                        industryGroups.Add(stock.getStockIndustry(groupBySectors), new IndustryGroup(stock.getStockIndustry()));

                    IndustryGroup ind = industryGroups[stock.getStockIndustry(groupBySectors)];

                    ind.count++;
                    ind.rankingSum += (double)rank.RankPosition;
                    ind.forwardGain += rank.ForwardGain;
                }

                //create new ranking results list with grouped industries
                rankResultGroup = new Dictionary<int, RankingResult>();
                foreach (IndustryGroup ind in industryGroups.Values)
                {
                    auxId++;
                    RankingResult ranking = new RankingResult(Utils.ConvertIntToDateTime(rankResultGroupKey), auxId, 0);

                    ranking.ForwardGain = ind.forwardGain / ind.count;
                    ranking.RankPosition = (decimal)(ind.rankingSum / ind.count);

                    rankResultGroup.Add(auxId, ranking);
                }

                groupedResults.Add(rankResultGroupKey, rankResultGroup);

            }

            scatterForm = new ScatterPlot(groupedResults, ScatterPlotPopup.RebalancingInterval < 3 && ScatterPlotPopup.SelectedIndex == 0);
            scatterForm.ShowDialog();

            HideProcessing();
        }

        Dictionary<int, Dictionary<int, RankingResult>> getRankingResultsForScatterplot()
        {
            CancelResultsProcessing = false;

            ShowProcessing("Stop Process", "Reading Database...", 110);

            //read all the data from the database
            if (stocks == null)
                stocks = Stock.ReadStockSymbols(true, pnlProcessingProgress);

            if (CancelResultsProcessing)
            {
                HideProcessing();
                return null;
            }

            int monthsPeriod = ScatterPlotPopup.RebalancingInterval;

            //process all features for the whole date range and calc scatter plot
            List<FilterConditions> filtersList = new List<FilterConditions>();
            foreach (CustomFilter filter in filters)
            {
                if (filter.IsActive)
                    filtersList.Add(filter.GetFilterConditions());
            }
            foreach (EquationFilterControl filter in equfiltercontrols)
            {
                if (filter.IsActive)
                    filtersList.Add(filter.GetFilterConditions());
            }

            RankingCalculator rankingCalc = new RankingCalculator(portfolioParameters);
            Dictionary<int, Dictionary<int, RankingResult>> rankingResults = rankingCalc.GenerateScatterPlot(stocks, filtersList, featureWeights, monthsPeriod, pnlProcessingProgress);

            return rankingResults;
        }

        private void cmdPortfolioParameters_Click(object sender, EventArgs e)
        {
            EditPortfolio frm = new EditPortfolio(portfolioParameters, false, false, this.selectedStrategyId, true);
            frm.ShowDialog();
            if(frm.ReprocessVRHistory)
            {
                ReprocessVRHistory();
            }
        }

        private void grdStocks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if(grdStocks.Rows[e.RowIndex].Tag is Stock)
                    (new CompositeGraph(((Stock)grdStocks.Rows[e.RowIndex].Tag).Id, portfolioParameters)).ShowDialog();
            }
            catch (Exception ex)
            { }

            this.Cursor = Cursors.Arrow;
        }
        
        private void ReprocessVRHistory()
        {
            var tmp = DataUpdater.ProcessVRHistory(pnlProcessingProgress).Result;
            stocks = null;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AddCustomFilter addCustomDialog = new AddCustomFilter();
            DialogResult result = addCustomDialog.ShowDialog();
            populateFilters();
        }

        private Strategy createStrategyObjectWithoutFeature()
        {
            Strategy strategy = new Strategy();

            strategy.Name = txtStrategyName.Text;
            strategy.Id = selectedStrategyId;
            strategy.PortfolioParameters = portfolioParameters;

            strategy.OptimalFeatures = Feature.LoadOptimalFeatures(selectedStrategyId);
            
            foreach (CustomFilter filter in filters)
            {
                if (filter.IsActive)
                {
                    strategy.Filters.Add(filter.GetFilterConditions());
                }
            }
            foreach (EquationFilterControl filter in equfiltercontrols)
            {
                if (filter.IsActive)
                    strategy.Filters.Add(filter.GetFilterConditions());
            }
            return strategy;
        }

        private void btnOptimize_Click(object sender, EventArgs e)
        {
            if (DataUpdater.CheckDatabaseBloqued())
                return;

            Strategy curStrategy = createStrategyObject();
            OptimizeSetting settingform = new OptimizeSetting(curStrategy);
            settingform.ShowDialog();
        }

        private void StrategyView_Activated(object sender, EventArgs e)
        {
            if (strategy != null && refreshFeatures == true)
            {
                refreshFeatures = false;

                strategy.Features = Feature.LoadFeatures(strategy.Id);

                for (int i = 0; i < featureWeights.Count; i++)
                {
                    FeatureWeight fw = featureWeights[i];

                    fw.IsEnabled = false;
                    fw.Weight = 0;

                    try
                    {
                        Feature featureFound = strategy.Features.Find(x => x.FeatureId == i);

                        if (featureFound == null)
                            continue;

                        fw.Weight = (float)featureFound.FeatureWeight;

                        if (fw.Weight != 0)
                            fw.IsEnabled = true;
                    }
                    catch (Exception)
                    { }
                }
                grdFeatures.Refresh();
                RefreshFeaturesGrid();
            }
        }
    }
}
