using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace StockRanking
{
    public static class DataUpdater
    {
        static List<Stock> stocksList = new List<Stock>();
        static int stockGroups = 100;

        //indicates that the DB is being updated (no other process can be run)
        public static bool UpdatingDatabase = false;
        public static bool reuseLastCommand = false;
        public static async Task<object> DownloadNewData(ProcessingPanelControl pnlProcessing)
        {
            DataUpdater.UpdatingDatabase = true;

            ZachsSourceReader.CancelProcess = false;
            StockSourcesReader.CancelProcess = false;

            pnlProcessing.StartProcess();

            try
            {
                object a;
                
                a = await DownloadTickers(pnlProcessing);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }

                Stock.ClearCache();

                stocksList = Stock.ReadStockSymbols(false, null);

                a = await UpdateSPYSymbols(pnlProcessing);

                a = await UpdateBenchmarkValues(pnlProcessing);
                
                a = await DownloadRatiosFromFile(pnlProcessing, DateTime.Now);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }

                
                Stock.ClearCache();

                stocksList = Stock.ReadStockSymbols(false, null);

                a = await DownloadPricesDataFromFile(pnlProcessing, DateTime.Now);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }


                ProcessPriceMetrics(pnlProcessing);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }


                ProcessFeatureMetrics(pnlProcessing, false);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }



                ProcessPriceAndRatioFeaturesMetrics(pnlProcessing, false);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }

                ProcessFilterMetrics(pnlProcessing, false);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }


                ProcessFilterScoreMetrics(pnlProcessing, false);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }

                ProcessFeatureScoreMetrics(pnlProcessing, false);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }



                a = await UpdateCAPEValues(pnlProcessing);


                a = await UpdateSpyHiLoValues(pnlProcessing);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }

                GenerateVRHistoryValues(pnlProcessing, true);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }

                UpdateFileBasedStocks(pnlProcessing, false);

                a = await GenerateETFsList(pnlProcessing);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }

                a = await GenerateETFPrices(pnlProcessing, false);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }
                
                DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM SYSTEM_PARAMETERS WHERE ID = 2", null);
                
                DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO SYSTEM_PARAMETERS VALUES (" + (int)SystemParameters.LastUpdateDate + ", '" + Utils.ConvertDateTimeToInt(DateTime.Now).ToString() + "')", null);

                Stock.ClearCache();

                pnlProcessing.StopProcess();
            }
            catch (Exception ex)
            {
                pnlProcessing.ShowError(ex.Message + " Stack: " + ex.StackTrace + "\n\n\n" + ZachsSourceReader.ErrorExtraData);
                DataUpdater.UpdatingDatabase = false;
                return null;
            }


            DataUpdater.UpdatingDatabase = false;
            return (object)1;
        }

        public static async Task<object> DownloadFullData(ProcessingPanelControl pnlProcessing)
        {
            DataUpdater.UpdatingDatabase = true;

            ZachsSourceReader.CancelProcess = false;
            StockSourcesReader.CancelProcess = false;

            pnlProcessing.StartProcess();
            pnlProcessing.SetTitle("Clearing database");
            pnlProcessing.SetMaxValue(2);
            pnlProcessing.PerformStep();
            
            try
            {
                object a;

                ClearDatabase(pnlProcessing);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }

                a = await DownloadTickers(pnlProcessing);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }
                
                Stock.ClearCache();

                stocksList = Stock.ReadStockSymbols(false, null);
                
                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }
                 
                a = await UpdateSPYSymbols(pnlProcessing);

                a = await DownloadRatiosFromFile(pnlProcessing, DateTime.MinValue);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }
                
                Stock.ClearCache();

                stocksList = Stock.ReadStockSymbols(false, null);

                a = await DownloadPricesDataFromFile(pnlProcessing, DateTime.MinValue);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }


                //process each FIP score and store monthly prices

                ProcessPriceMetrics(pnlProcessing);


                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }


                ProcessFeatureMetrics(pnlProcessing, true);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }
                
                ProcessPriceAndRatioFeaturesMetrics(pnlProcessing, true);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }
                

                ProcessFilterMetrics(pnlProcessing, true);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }

                //stocksList = Stock.ReadStockSymbols(false, null);

                ProcessFilterScoreMetrics(pnlProcessing, true);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }

                //stocksList = Stock.ReadStockSymbols(false, null);

                ProcessFeatureScoreMetrics(pnlProcessing, true);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }

                a = await UpdateCAPEValues(pnlProcessing);

                a = await UpdateSpyHiLoValues(pnlProcessing);

                GenerateVRHistoryValues(pnlProcessing);

                a = await UpdateBenchmarkValues(pnlProcessing);

                UpdateFileBasedStocks(pnlProcessing, true);

                a = await GenerateETFsList(pnlProcessing);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }

                a = await GenerateETFPrices(pnlProcessing, true);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }

                DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO SYSTEM_PARAMETERS VALUES (" + (int)SystemParameters.FullDownloadCompleted + ", '1')", null);

                DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM SYSTEM_PARAMETERS WHERE ID = 2", null);

                DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO SYSTEM_PARAMETERS VALUES (" + (int)SystemParameters.LastUpdateDate + ", '" + Utils.ConvertDateTimeToInt(DateTime.Now).ToString() + "')", null);

                Stock.ClearCache();

                pnlProcessing.StopProcess();

            }
            catch (Exception ex)
            {
                pnlProcessing.ShowError(ex.Message + " Stack: " + ex.StackTrace);
                DataUpdater.UpdatingDatabase = false;
                return null;
            }


            DataUpdater.UpdatingDatabase = false;
            return new object();
        }

        public static async Task<object> ProcessVRHistory(ProcessingPanelControl pnlProcessing)
        {
            DataUpdater.UpdatingDatabase = true;

            ZachsSourceReader.CancelProcess = false;
            StockSourcesReader.CancelProcess = false;

            pnlProcessing.StartProcess();

            try
            {
                object a;

                Stock.ClearCache();

                stocksList = Stock.ReadStockSymbols(false, null);
                
                GenerateVRHistoryValues(pnlProcessing);

                if (StockSourcesReader.CancelProcess)
                {
                    pnlProcessing.StopProcess();
                    DataUpdater.UpdatingDatabase = false;
                    return null;
                }

                Stock.ClearCache();

                pnlProcessing.StopProcess();
            }
            catch (Exception ex)
            {
                pnlProcessing.ShowError(ex.Message + " Stack: " + ex.StackTrace);
                DataUpdater.UpdatingDatabase = false;
                return null;
            }


            DataUpdater.UpdatingDatabase = false;
            return null;

        }


        private static void ClearDatabase(ProcessingPanelControl pnlProcessing)
        {
            DatabaseSingleton.Instance.DropDataTables(pnlProcessing);
            pnlProcessing.PerformStep();
            DatabaseSingleton.Instance.CreateDataTables();
            pnlProcessing.PerformStep();

        }

        private static async Task<object> DownloadTickers(ProcessingPanelControl pnlProcessing)
        {
            pnlProcessing.SetTitle("Retrieving Tickers...");
            pnlProcessing.SetMaxValue(14000 / 300);
            pnlProcessing.PerformStep();

            object a = await ZachsSourceReader.GenerateStocksList(pnlProcessing);
            return a;
        }
        
        private static async Task<object> DownloadPricesDataFromFile(ProcessingPanelControl pnlProcessing, DateTime lastDate)
        {
            if (ZachsSourceReader.CancelProcess)
                return null;

            StockValue.CommandSaved = false;
             
            try
            { 
                object test = await ZachsSourceReader.GetSharadarPricesFromFile(stocksList, pnlProcessing, lastDate);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error importing prices: " + e.StackTrace + "   " + e.Message);
                throw e;
            }

            return null;
        }


        private static async Task<object> DownloadRatiosFromFile(ProcessingPanelControl pnlProcessing, DateTime lastDate)
        {
            if (ZachsSourceReader.CancelProcess)
                return null;
             
            object a = await ZachsSourceReader.GetSharadarRatiosFromFile(stocksList, pnlProcessing, lastDate);

            return null;
        }
         

        private static void ProcessPriceMetrics(ProcessingPanelControl pnlProcessing)
        {
            if (ZachsSourceReader.CancelProcess)
                return;
            
            //get last price dates
            Dictionary<int, int> lastPrices = getPricesLastDates(stocksList);
            
            DatabaseSingleton.Instance.ExecuteNonQuery("DROP INDEX IF EXISTS 'PRICE_DATE_X'", null);
            DatabaseSingleton.Instance.ExecuteNonQuery("DROP INDEX IF EXISTS 'PRICE_HISTORY_DATE_X'", null);

            pnlProcessing.SetMaxValue(12000/100);
            pnlProcessing.SetTitle("Generating price related metrics");

            DatabaseSingleton.Instance.StartTransaction();

            int stocksCount = 0;
            int performStepCounter = 100;
            foreach (Stock stock in stocksList)
            {
                int records = 0;
                int lastDate = 0;
                int countRecords = 0;
                decimal lastValue = 0;
                int upDays = 0;
                int downDays = 0;
                Queue<decimal> valueChanges = new Queue<decimal>();
                Queue<decimal> valueCloses = new Queue<decimal>();
                int lastQuarterSaved = 0;

                if (stock.stockValues.Count == 0)
                    continue;

                stock.stockValues = stock.stockValues.OrderBy(x => x.Date).ToList();
                
                //calc change % for all values and avg 20 days volume and 3 months high/low
                List<long> volumeValues = new List<long>();
                List<decimal> minValues = new List<decimal>();
                List<decimal> maxValues = new List<decimal>();
                List<int> closeDates = new List<int>();
                foreach (StockValue value in stock.stockValues)
                {
                    if (lastValue != 0)
                        if (value.Close == 0)
                            value.ChangePercent = 0;
                        else
                            value.ChangePercent = (value.Close - lastValue) / lastValue;

                    //fill list to calculate if in 3 months high or low
                    if (value is HiLoStockValue hiloStock)
                    {
                        minValues.Add(hiloStock.Low);
                        maxValues.Add(hiloStock.High);
                        closeDates.Add(value.Date);

                        while (closeDates.Count > 0 && Utils.ConvertIntToDateTime(value.Date).AddMonths(-3) > Utils.ConvertIntToDateTime(closeDates[0]))
                        {
                            minValues.RemoveAt(0);
                            maxValues.RemoveAt(0);
                            closeDates.RemoveAt(0);
                        }

                        //calc 3 months high and low
                        if (maxValues.Max() == hiloStock.High)
                            hiloStock.ThreeMonthsHigh = 1;
                        if (minValues.Min() == hiloStock.Low)
                            hiloStock.ThreeMonthsLow = 1;

                    }

                    //fill volumes list in order to calculate 20 days AVG
                    volumeValues.Add(value.Volume);
                    if (volumeValues.Count > 20)
                        volumeValues.RemoveAt(0);

                    lastValue = value.Close;
                    value.Avg20DaysVolume = volumeValues.Sum() / volumeValues.Count;
                }

                int lastDateSaved = stock.LastValuesDate;

                int lastPriceDateFromDB = 0;
                if (lastPrices.ContainsKey(stock.Id))
                {
                    lastPriceDateFromDB = lastPrices[stock.Id];
                }

                StockValue lastDatePrice = null;

                //save prices for the 1st of each month
                foreach (StockValue value in stock.stockValues)
                {
                    if (lastDateSaved + 50 > value.Date)
                    {
                        lastDatePrice = value;
                        continue;
                    }

                    lastDateSaved = value.Date;
                    StockValue.CommandSaved = false;

                    //only save if new data
                    if (lastPriceDateFromDB < value.Date)
                        value.Save(stock.Id, true);
                }

                //save history prices for every day
                StockValue.CommandSaved = false;
                foreach (StockValue sValue in stock.stockValues)
                {
                    records++;
                    //only save if new data
                    if (lastPriceDateFromDB < sValue.Date)
                        sValue.SaveHistoricPrice(stock.Id);
                }

                //save dividends
                foreach (var dividendsPair in stock.stockDividends.Where(x => x.Key > lastPriceDateFromDB && x.Key < Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"))))
                {
                    Stock.SaveDividend(stock.Id, dividendsPair.Key, dividendsPair.Value);
                }

                lastDateSaved = 0;
                foreach (StockValue value in stock.stockValues)
                {
                    countRecords++;

                    valueChanges.Enqueue(value.ChangePercent);
                    valueCloses.Enqueue(value.Close);

                    if (value.ChangePercent < 0)
                        downDays++;
                    if (value.ChangePercent >= 0)
                        upDays++;

                    if (countRecords < 253)
                        continue;

                    decimal oldValue = valueChanges.Dequeue();
                    decimal oldClosePrice = valueCloses.Dequeue();

                    //use first price of the queue in order to calc FIP
                    oldClosePrice = valueCloses.Peek();

                    //update down and up days
                    if (oldValue < 0)
                        downDays--;
                    if (oldValue >= 0)
                        upDays--;

                    //it will only save the value if it's the 1st value of this month
                    if (lastDateSaved + 50 > value.Date)
                        continue;
                    
                    decimal average = valueChanges.Average();
                    decimal stdDev = CalcStdDev(valueChanges, average);
                    decimal semivariance = CalcSemivariance(valueChanges, 0);
                    decimal totaldays = valueChanges.Count;

                    decimal pctupdays = ((decimal)upDays / totaldays);
                    decimal pctdndays = ((decimal)downDays / totaldays);

                    //pctChg = Close / Close[252] - 1 (just percent change over last 252 days)
                    //fipScore = pctChg > 0 ? pctChg * (pctupdays - pctdndays) : pctChg * (pctdndays - pctupdays);
                    if (oldClosePrice == 0)
                        oldClosePrice = value.Close;
                    decimal pctChg = 0;
                    if(oldClosePrice != 0)
                        pctChg = (value.Close / oldClosePrice) - 1m;
                    decimal fipScore = pctChg > 0 ? pctChg * (pctupdays - pctdndays) : pctChg * (pctdndays - pctupdays);

                    if (stdDev == 0)
                        stdDev = 0.01m;
                    if (semivariance == 0)
                        semivariance = 0.01m;

                    decimal sharpeFip = (average / stdDev) * Convert.ToDecimal(Math.Sqrt(252));
                    decimal sortino = (average / semivariance) * Convert.ToDecimal(Math.Sqrt(252));

                    if (sharpeFip > 0)
                        sharpeFip = sharpeFip * (pctupdays - pctdndays);
                    else
                        sharpeFip = sharpeFip * (pctdndays - pctupdays);
                    if (sortino > 0)
                        sortino = sortino * (pctupdays - pctdndays);
                    else
                        sortino = sortino * (pctdndays - pctupdays);

                    lastDateSaved = Convert.ToInt32(value.Date.ToString().Substring(0, 6) + "01");

                    //if this price was already in the database, ignore FIP value and continue
                    if (value.Date <= lastPriceDateFromDB)
                        continue;

                    //save feature value
                    //the corresponding quarter values is not 100% correct but it will be overriden with the features calculation
                    Feature.SaveFIPFeature(stock.Id, lastDateSaved, fipScore, sortino, sharpeFip, Utils.ConvertDateTimeToInt(Utils.ConvertIntToDateTime(Utils.GetCorrespondingQuarter(value.Date)).AddDays(1)));
                }

                stocksCount++;

                stock.stockValues.Clear();
                stock.stockValues.TrimExcess();

                if (performStepCounter-- == 0)
                {
                    pnlProcessing.PerformStep();
                    performStepCounter = 100;
                    DatabaseSingleton.Instance.EndTransaction();
                    GC.Collect();
                    DatabaseSingleton.Instance.StartTransaction();
                }
            }

            DatabaseSingleton.Instance.EndTransaction();

            DatabaseSingleton.Instance.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'PRICE_DATE_X' ON 'PRICE'(ID_STOCK, DATE)", null);
            DatabaseSingleton.Instance.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'PRICE_HISTORY_DATE_X' ON 'PRICE_HISTORY'(ID_STOCK, DATE)", null);
        }


        public static decimal CalcStdDev(IEnumerable<decimal> magnitudes, decimal average = 0)
        {
            if (magnitudes.Count() == 0)
                return 0;

            if (average == 0)
            {
                if(magnitudes.Count() > 0)
                    average = magnitudes.Average();
            }

            decimal sumOfSquaresOfDifferences = magnitudes.Select(val => (val - average) * (val - average)).Sum();
            return (decimal)Math.Sqrt((double)(sumOfSquaresOfDifferences / magnitudes.Count()));
        }
        
        public static decimal CalcSemivariance(IEnumerable<decimal> magnitudes, decimal average)
        {
            var auxList = new List<decimal>();
            foreach (var mag in magnitudes)
                auxList.Add(mag < 0 ? mag: 0m);
                
            return CalcStdDev(auxList);
        }
         

        private static void ProcessFeatureMetrics(ProcessingPanelControl pnlProcessing, bool fullDownload)
        {
            if (ZachsSourceReader.CancelProcess)
                return;
             
            int stocksCount = 0;
            pnlProcessing.SetMaxValue(stocksList.Count/100);

            DatabaseSingleton.Instance.StartTransaction();

            int performStepCounter = 100;
            foreach (Stock stock in stocksList)
            {
                int lastDate = 0;
                int countRecords = 0;
                decimal lastValue = 0;
                int upDays = 0;
                int downDays = 0;
                Queue<decimal> valueChanges = new Queue<decimal>();
                int lastQuarterSaved = 0;

                int lastDateForUpdates = Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-8));
                int lastDateForProcess = Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-3));
                if (fullDownload)
                {
                    StockRatios.LoadRatios(stock);
                    lastDateForProcess = 0;
                }
                else
                    StockRatios.LoadRatios(stock, lastDateForUpdates);

                stocksCount++;

                if (stock.Ratios.Count == 0)
                    continue;

                //for each quarter calc features and save the same value for the following months until new quarter

                for (int ratioIndex = 0; ratioIndex < stock.Ratios.Count; ratioIndex++)
                {
                    //ignore everything older than last update
                    if (stock.Ratios[ratioIndex].Date <= lastDateForProcess)
                        continue;

                    //calc features for all values for all quarters and save for all months
                    FeatureTable featureTable = Feature.CalcFeaturesTable(stock.Ratios, ratioIndex);

                    //save feature value 
                    DateTime quarterStart = Utils.ConvertIntToDateTime(stock.Ratios[ratioIndex].DateFirstMonth);
                    DateTime correspondingQuarter = Utils.ConvertIntToDateTime(stock.Ratios[ratioIndex].DateFirstMonth);

                    //aca se podria guardar un ID del ratio usado para este feature ??

                    featureTable.Date = stock.Ratios[ratioIndex].DateFirstMonth;
                    featureTable.CorrespondingQuarter = Utils.ConvertDateTimeToInt(correspondingQuarter);
                    featureTable.SaveAllExceptFIP();
                    
                    if (ZachsSourceReader.CancelProcess)
                    {
                        DatabaseSingleton.Instance.EndTransaction();
                        return;
                    }
                }

                stock.Ratios.Clear();
                stock.Ratios.TrimExcess();

                if (performStepCounter-- == 0)
                {
                    pnlProcessing.SetTitle("Generating Features. Stocks: " + stocksCount.ToString());
                    pnlProcessing.PerformStep();
                    performStepCounter = 100;
                    DatabaseSingleton.Instance.EndTransaction();
                    DatabaseSingleton.Instance.StartTransaction();
                }

            }

            DatabaseSingleton.Instance.EndTransaction();

        }



        private static bool ProcessFilterMetrics(ProcessingPanelControl pnlProcessing, bool fullUpdate)
        {
            if (ZachsSourceReader.CancelProcess)
                return false;

            int divisions = 300;
            String dateFilter = "";

            pnlProcessing.SetTitle("Generating filter data");
            pnlProcessing.SetMaxValue(stocksList.Count / divisions);
            pnlProcessing.PerformStep();

            if (fullUpdate)
            {
                DatabaseSingleton.Instance.ExecuteNonQuery("DROP INDEX  IF EXISTS FILTER_DATE_X", null);
                DatabaseSingleton.Instance.ExecuteNonQuery("DROP INDEX  IF EXISTS FILTER_DATE_ONLY", null);
            }
            else
            {
                //if only partial update delete last 4 months
                DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM FILTER WHERE DATE > " + Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-4)).ToString(), null);
                divisions = 1000;
                dateFilter = " AND FEATURE.DATE > " + Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-4)).ToString() + " ";
            }


            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "FiltersSQL.txt";
                string SQLString = "";
                
                using (Stream stream = assembly.GetManifestResourceStream("StockRanking." + resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    SQLString = reader.ReadToEnd();
                }

                for(int i = 0; i < (stocksList.Count / divisions)+1; i++)
                {
                    String sqlBetween = (i * divisions).ToString() + " AND " + ((i + 1) * divisions - 1).ToString();
                    
                    DatabaseSingleton.Instance.ExecuteNonQuery(SQLString.Replace("##BETWEEN##",  sqlBetween).Replace("##DATEFILTER##", dateFilter).Replace("##YEARSVAR##", (PortfolioParameters.GetCompositeRollingMedianYearsStatic() * 10000).ToString()), null);

                    pnlProcessing.PerformStep();
                }

            }
            catch(Exception e)
            {
                throw e;
            }


            if (fullUpdate)
            {

                DatabaseSingleton.Instance.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'FILTER_DATE_X' ON 'FILTER'(ID_STOCK, DATE)", null);
                DatabaseSingleton.Instance.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'FILTER_DATE_ONLY' ON 'FILTER'(DATE)", null);
            }
            
            return true;
        }


        private static bool ProcessPriceAndRatioFeaturesMetrics(ProcessingPanelControl pnlProcessing, bool fullUpdate)
        {
            if (ZachsSourceReader.CancelProcess)
                return false;

            int divisions = 300;

            pnlProcessing.SetTitle("Generating Price and Ratio related metrics data");
            pnlProcessing.SetMaxValue(stocksList.Count / divisions);
            pnlProcessing.PerformStep();
            
            try
            {  
                string SQLString = "UPDATE FEATURE SET MTUM = (SELECT CAST(IFNULL(RATIO.MARKET_CAP, 0) AS DOUBLE) FROM RATIO WHERE RATIO.ID_STOCK = FEATURE.ID_STOCK AND RATIO.DATE_FIRST_MONTH = FEATURE.DATE LIMIT 1) * FIP ";
                
                for (int i = 0; i < (stocksList.Count / divisions) + 1; i++)
                {
                    String sqlBetween = " WHERE ID_STOCK BETWEEN " + (i * divisions).ToString() + " AND " + ((i + 1) * divisions - 1).ToString();

                    if (!fullUpdate)
                        sqlBetween += " AND DATE > " + Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-3)).ToString();

                    DatabaseSingleton.Instance.ExecuteNonQuery(SQLString + sqlBetween, null);
                    
                    if (ZachsSourceReader.CancelProcess)
                        return false;
                    
                    pnlProcessing.PerformStep();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
             
            return true;
        }


        private static bool ProcessFilterScoreMetrics(ProcessingPanelControl pnlProcessing, bool fullUpdate)
        {
            if (ZachsSourceReader.CancelProcess)
                return false;

            int stocksCount = 0;

            pnlProcessing.SetMaxValue(stocksList.Count/100);

            int lastDateForUpdates = Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-8));
            int lastDateForProcess = Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-7));

            DatabaseSingleton.Instance.StartTransaction();

            int performStepCounter = 100;
            foreach (Stock stock in stocksList)
            {
                int lastDate = 0;
                int countRecords = 0;
                decimal lastValue = 0;
                int upDays = 0;
                int downDays = 0;
                Queue<decimal> valueChanges = new Queue<decimal>();
                int lastQuarterSaved = 0;

                if (fullUpdate)
                {
                    StockRatios.LoadRatios(stock);
                    lastDateForProcess = 0;
                }
                else
                    StockRatios.LoadRatios(stock, lastDateForUpdates);
                
                stocksCount++;

                if (stock.Ratios.Count == 0)
                    continue;

                //for each quarter calc scores and update Filters table
                FilterTable.CommandSaved = false;

                FilterTable.ResetScoreValues();

                for (int ratioIndex = 0; ratioIndex < stock.Ratios.Count; ratioIndex++)
                {
                    //ignore everything older than last update
                    if (stock.Ratios[ratioIndex].Date <= lastDateForProcess)
                        continue;

                    decimal ROICScore = FilterTable.GenerateScoreNew(FilterTypes.ROICScore, ratioIndex, stock.Ratios);
                    decimal CROICScore = FilterTable.GenerateScoreNew(FilterTypes.CROICScore, ratioIndex, stock.Ratios);
                    decimal FCFScore = FilterTable.GenerateScoreNew(FilterTypes.FCFScore, ratioIndex, stock.Ratios);
                    decimal EBITDAScore = FilterTable.GenerateScoreNew(FilterTypes.EBITDAScore, ratioIndex, stock.Ratios);
                    decimal SALESScore = FilterTable.GenerateScoreNew(FilterTypes.SALESScore, ratioIndex, stock.Ratios);

                    //save feature value 
                    DateTime quarterStart = Utils.ConvertIntToDateTime(stock.Ratios[ratioIndex].DateFirstMonth);

                    FilterTable.UpdateFilterScores(ROICScore, CROICScore, FCFScore, EBITDAScore, SALESScore, stock, quarterStart, quarterStart.AddMonths(2));

                    if (ZachsSourceReader.CancelProcess)
                    {
                        DatabaseSingleton.Instance.EndTransaction();
                        return false;
                    }
                }

                if (performStepCounter-- == 0)
                {
                    pnlProcessing.SetTitle("Generating Filter Scores. Stocks: " + stocksCount.ToString());
                    pnlProcessing.PerformStep();
                    performStepCounter = 100;
                    DatabaseSingleton.Instance.EndTransaction();
                    DatabaseSingleton.Instance.StartTransaction();
                    stock.Ratios = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                
            }

            DatabaseSingleton.Instance.EndTransaction();

            if (ZachsSourceReader.CancelProcess)
                return false;

            pnlProcessing.SetTitle("Ranking filter scores...");
            pnlProcessing.SetMaxValue(300);
            pnlProcessing.PerformStep();

            String sqlQuery = "SELECT DATE, ID_STOCK, ROIC_SCORE_AUX, CROIC_SCORE_AUX, FCF_SCORE_AUX, EBITDA_SCORE_AUX, SALES_SCORE_AUX FROM FILTER";
            if (lastDateForProcess > 0)
            {
                sqlQuery += " WHERE DATE > " + lastDateForProcess.ToString();
            }
            sqlQuery += " ORDER BY DATE DESC";

            DataTable data = DatabaseSingleton.Instance.GetData(sqlQuery);

            List<FilterScore> flist = new List<FilterScore>();
            int lastKeyDate = -1;
            
            List<FilterScore> allFilters = new List<FilterScore>();
            foreach (DataRow row in data.Rows)
            {
                FilterScore fscore = new FilterScore(row);

                if (Convert.ToInt32(row[0]) == lastKeyDate)
                {
                    flist.Add(fscore);
                } else
                {
                    UpdateFilterScores(flist);
                    allFilters.AddRange(flist);
                    
                    flist.Clear();
                    flist.Add(fscore);
                    lastKeyDate = Convert.ToInt32(row[0]);

                    pnlProcessing.PerformStep();

                    if (ZachsSourceReader.CancelProcess)
                    {
                        return false;
                    }
                }
            }
            UpdateFilterScores(flist);
            allFilters.AddRange(flist);
            flist.Clear();
            flist.TrimExcess();
            pnlProcessing.PerformStep();

            DatabaseSingleton.Instance.StartTransaction();
            reuseLastCommand = false;
            
            String sqlQuery1 = "UPDATE FILTER SET " +
                                " ROIC_score = ROUND(@p1, 4), CROIC_score = ROUND(@p2, 4), FCF_score = ROUND(@p3, 4), EBITDA_score = ROUND(@p4, 4), SALES_score = ROUND(@p5, 4) WHERE ID_STOCK = @p6 AND DATE = @p7";
            int runcounts = 0;

            allFilters.Sort((x, y) => (x.IdStock != y.IdStock) ? (x.IdStock.CompareTo(y.IdStock)):(x.Date.CompareTo(y.Date)));

            foreach (var x in allFilters)
            {
                List<SQLiteParameter> parameters = new List<SQLiteParameter>();

                parameters.Add(new SQLiteParameter("@p1", x.RoicScore));
                parameters.Add(new SQLiteParameter("@p2", x.CroicScore));
                parameters.Add(new SQLiteParameter("@p3", x.FcfScore));
                parameters.Add(new SQLiteParameter("@p4", x.EbitdaScore));
                parameters.Add(new SQLiteParameter("@p5", x.SalesScore));
                parameters.Add(new SQLiteParameter("@p6", x.IdStock));
                parameters.Add(new SQLiteParameter("@p7", x.Date));

                runcounts++;
                if (runcounts > 10000)
                {
                    runcounts = 0;
                    DatabaseSingleton.Instance.EndTransaction();
                    DatabaseSingleton.Instance.StartTransaction();
                    pnlProcessing.PerformStep();
                }
                
                DatabaseSingleton.Instance.ExecuteNonQuery(sqlQuery1, parameters.ToArray(), reuseLastCommand);
                reuseLastCommand = true;
            }
            DatabaseSingleton.Instance.EndTransaction();

            allFilters.Clear();
            allFilters.TrimExcess();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return true;
             
        }

        private static void UpdateFilterScores(List<FilterScore> flist)
        {
            if (flist.Count == 0) return;

            flist.Sort((x, y) => -(x.EbitdaScoreAux.CompareTo(y.EbitdaScoreAux)));
            FilterScore prev = null;
            decimal id = flist.Count;
            
            foreach (var x in flist)
            {
                if (prev != null && x.EbitdaScoreAux == prev.EbitdaScoreAux)
                {
                    x.EbitdaScore = prev.EbitdaScore;
                } else
                {
                    x.EbitdaScore = id / flist.Count * 100;
                }
                id = id - 1;
                prev = x;
            }

            flist.Sort((x, y) => -(x.CroicScoreAux.CompareTo(y.CroicScoreAux)));
            prev = null;
            id = flist.Count;
            foreach (var x in flist)
            {
                if (prev != null && x.CroicScoreAux == prev.CroicScoreAux)
                {
                    x.CroicScore = prev.CroicScore;
                }
                else
                {
                    x.CroicScore = id / flist.Count * 100;
                }
                id = id - 1;
                prev = x;
            }

            flist.Sort((x, y) => -(x.FcfScoreAux.CompareTo(y.FcfScoreAux)));
            prev = null;
            id = flist.Count;
            foreach (var x in flist)
            {
                if (prev != null && x.FcfScoreAux == prev.FcfScoreAux)
                {
                    x.FcfScore = prev.FcfScore;
                }
                else
                {
                    x.FcfScore = id / flist.Count * 100;
                }
                id = id - 1;
                prev = x;
            }

            flist.Sort((x, y) => -(x.RoicScoreAux.CompareTo(y.RoicScoreAux)));
            prev = null;
            id = flist.Count;
            foreach (var x in flist)
            {
                if (prev != null && x.RoicScoreAux == prev.RoicScoreAux)
                {
                    x.RoicScore = prev.RoicScore;
                }
                else
                {
                    x.RoicScore = id / flist.Count * 100;
                }
                id = id - 1;
                prev = x;
            }

            flist.Sort((x, y) => -(x.SalesScoreAux.CompareTo(y.SalesScoreAux)));
            prev = null;
            id = flist.Count;
            foreach (var x in flist)
            {
                if (prev != null && x.SalesScoreAux == prev.SalesScoreAux)
                {
                    x.SalesScore = prev.SalesScore;
                }
                else
                {
                    x.SalesScore = id / flist.Count * 100;
                }
                id = id - 1;
                prev = x;
            }
        }

        private static bool ProcessFeatureScoreMetrics(ProcessingPanelControl pnlProcessing, bool fullUpdate)
        {
            if (ZachsSourceReader.CancelProcess)
                return false;


            pnlProcessing.SetMaxValue(stocksList.Count);

            int lastDateForUpdates = Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-8));
            int lastDateForProcess = Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-7));
            
            int divisions = 100;

            pnlProcessing.SetTitle("Updating Feature Scores...");
            pnlProcessing.SetMaxValue(stocksList.Count / divisions);
            pnlProcessing.PerformStep();

            if (fullUpdate)
                lastDateForProcess = 0;

            try
            {
                for (int i = 0; i < (stocksList.Count / divisions) + 1; i++)
                {
                    String sqlQuery = "UPDATE FEATURE SET " +
                                " ROIC_score =  (select CAST(FILTER.ROIC_SCORE AS DOUBLE) FROM FILTER WHERE FILTER.DATE = FEATURE.DATE AND FILTER.ID_STOCK = FEATURE.ID_STOCK), " +
                                " CROIC_score =  (select CAST(FILTER.CROIC_score AS DOUBLE) FROM FILTER WHERE FILTER.DATE = FEATURE.DATE AND FILTER.ID_STOCK = FEATURE.ID_STOCK), " +
                                " FCF_score =  (select CAST(FILTER.FCF_score AS DOUBLE) FROM FILTER WHERE FILTER.DATE = FEATURE.DATE AND FILTER.ID_STOCK = FEATURE.ID_STOCK), " +
                                " EBITDA_score =  (select CAST(FILTER.EBITDA_score AS DOUBLE) FROM FILTER WHERE FILTER.DATE = FEATURE.DATE AND FILTER.ID_STOCK = FEATURE.ID_STOCK), " +
                                " SALES_score =  (select CAST(FILTER.SALES_score AS DOUBLE) FROM FILTER WHERE FILTER.DATE = FEATURE.DATE AND FILTER.ID_STOCK = FEATURE.ID_STOCK) ";

                    String sqlBetween = " WHERE ID_STOCK BETWEEN " + (i * divisions).ToString() + " AND " + ((i + 1) * divisions - 1).ToString();

                    if (lastDateForProcess > 0)
                    {
                        sqlBetween += " AND DATE > " + lastDateForProcess.ToString();
                    }

                    DatabaseSingleton.Instance.ExecuteNonQuery(sqlQuery + sqlBetween, null);

                    if (ZachsSourceReader.CancelProcess)
                    {
                        return false;
                    }

                    pnlProcessing.PerformStep();

                }

            }
            catch (Exception e)
            {
                throw e;
            }


            return true;

        }

        public static async Task<object> UpdateBenchmarkValues(ProcessingPanelControl pnlProcessing)
        { 
            pnlProcessing.SetTitle("Updating Benckmark data");
            pnlProcessing.SetMaxValue(9);
            pnlProcessing.PerformStep();

            object a;
            a = await UpdateBenchmarkSymbolValues(pnlProcessing, stocksList.Find(x => x.Id == -1), false);
            pnlProcessing.PerformStep();
            a = await UpdateBenchmarkSymbolValues(pnlProcessing, stocksList.Find(x => x.Id == -2), false);
            pnlProcessing.PerformStep();

            if (StockSourcesReader.CancelProcess)
            {
                pnlProcessing.StopProcess(); DataUpdater.UpdatingDatabase = false;
                return null;
            }

            a = await UpdateBenchmarkSymbolValues(pnlProcessing, stocksList.Find(x => x.Id == -3), false);
            pnlProcessing.PerformStep();
            a = await UpdateBenchmarkSymbolValues(pnlProcessing, stocksList.Find(x => x.Id == -4), false);
            pnlProcessing.PerformStep();
            a = await UpdateBenchmarkSymbolValues(pnlProcessing, stocksList.Find(x => x.Id == -5), false);
            pnlProcessing.PerformStep();

            if (StockSourcesReader.CancelProcess)
            {
                pnlProcessing.StopProcess(); DataUpdater.UpdatingDatabase = false;
                return null;
            }

            a = await UpdateBenchmarkSymbolValues(pnlProcessing, stocksList.Find(x => x.Id == -6), false);
            pnlProcessing.PerformStep();
            a = await UpdateBenchmarkSymbolValues(pnlProcessing, stocksList.Find(x => x.Id == -7), false);
            pnlProcessing.PerformStep();
            a = await UpdateBenchmarkSymbolValues(pnlProcessing, stocksList.Find(x => x.Id == -8), false);
            pnlProcessing.PerformStep();
            a = await UpdateBenchmarkSymbolValues(pnlProcessing, stocksList.Find(x => x.Id == -9), false);
            pnlProcessing.PerformStep();
            a = await UpdateBenchmarkSymbolValues(pnlProcessing, stocksList.Find(x => x.Id == -11), false);
            pnlProcessing.PerformStep();

            return null;
        }

        public static async Task<object> UpdateBenchmarkSymbolValues(ProcessingPanelControl pnlProcessing, Stock stock, bool applyMtumRatio)
        {
            //read last SPY value in the database
            var idStock = stock.Id;
            var symbol = stock.Symbol;
            int lastPriceDate = Convert.ToInt32(DatabaseSingleton.Instance.GetData("SELECT IFNULL(MAX(DATE), 0) FROM PRICE_HISTORY WHERE ID_STOCK = " + idStock).Rows[0][0]);

            if (lastPriceDate == 0)
                lastPriceDate = 20020101;
            
            if (lastPriceDate >= Utils.ConvertDateTimeToInt(DateTime.Now.AddDays(-1)))
                return null;

            List<KeyValuePair<int, decimal>> dividendsList = new List<KeyValuePair<int, decimal>>();
            List<StockValue> pricesList = ZachsSourceReader.GetSharadarETFPricesFromFile(stock, pnlProcessing, Utils.ConvertIntToDateTime(lastPriceDate).AddMonths(-1), dividendsList);
            int lastDateSaved = lastPriceDate;

            decimal ratioToApply = 1m;
            if(applyMtumRatio)
            {
                var lastPriceValues = Stock.GetAllPrices(idStock, 20130101, lastPriceDate);

                lastPriceValues = lastPriceValues.OrderByDescending(x=>x.Date).ToList();

                StockValue downloadedValue = null;
                StockValue fileValue = null;
                for (int i = 0; i < 30; i++)
                {
                    downloadedValue = pricesList.Find(x => x.Date == lastPriceValues[i].Date);
                    fileValue = lastPriceValues[i];
                    if (downloadedValue != null)
                        break;
                }
                
                if (downloadedValue == null)
                {
                    throw new Exception("Error when trying to apply ratio to MTUM benchmark, please check the imported file.");
                }

                ratioToApply = downloadedValue.Close / fileValue.Close;
            }

            DatabaseSingleton.Instance.StartTransaction();

            //update old prices if needed
            if(ratioToApply != 1)
            {
                List<SQLiteParameter> parameters = new List<SQLiteParameter>();

                parameters.Add(new SQLiteParameter("@p1", ratioToApply));

                DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE PRICE_HISTORY SET CLOSE_PRICE = CLOSE_PRICE * @p1 WHERE ID_STOCK = " + idStock, parameters.ToArray());
                DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE PRICE SET CLOSE_PRICE = CLOSE_PRICE * @p1 WHERE ID_STOCK = " + idStock, parameters.ToArray());
            }

            StockValue lastDatePrice = null;

            pricesList = pricesList.OrderBy(x => x.Date).ToList();
            foreach (StockValue newValue in pricesList)
            {
                //System.Diagnostics.Debug.WriteLine(newValue.Date);

                if (newValue.Date > lastPriceDate && newValue.Date < Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")))
                {
                    if (lastDateSaved + 50 < newValue.Date)
                    {
                        lastDateSaved = newValue.Date;
                        StockValue.CommandSaved = false;
                        newValue.Save(idStock, true);
                    }

                    StockValue.CommandSaved = false;
                    newValue.SaveHistoricPrice(idStock);
                }

                lastDatePrice = newValue;
            }

            foreach(var dividendsPair in dividendsList.Where(x => x.Key > lastPriceDate && x.Key < Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"))))
            {
                Stock.SaveDividend(stock.Id, dividendsPair.Key, dividendsPair.Value);
            }

            DatabaseSingleton.Instance.EndTransaction();

            return null;
        }
        
        private static Dictionary<int, int> getPricesLastDates(List<Stock> stocks)
        {
            DataTable results = DatabaseSingleton.Instance.GetData("select IFNULL((select MAX(DATE) from PRICE_HISTORY WHERE PRICE_HISTORY.ID_STOCK = STOCK.ID), 0), ID FROM STOCK WHERE ID != -1");

            Dictionary<int, int> lastDates = new Dictionary<int, int>();

            foreach (DataRow row in results.Rows)
            {
                lastDates.Add(Convert.ToInt32(row[1]), Convert.ToInt32(row[0]));
            }

            return lastDates;
        }

        public static bool CheckDatabaseBloqued()
        {
            if (!UpdatingDatabase)
                return false;

            System.Windows.Forms.MessageBox.Show("Database is being updated, please wait until the update is completed.");

            return true;
        }


        public static async Task<object> UpdateCAPEValues(ProcessingPanelControl pnlProcessing)
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT YEAR, MONTH FROM CAPE_VALUE ORDER BY YEAR DESC, MONTH DESC LIMIT 1");
            int year = 1990;
            int month = 1;

            if (data.Rows.Count > 0)
            { 
                year = Convert.ToInt32(data.Rows[0][0]);
                month = Convert.ToInt32(data.Rows[0][1]);
            }

            //get iextrading values
            if (year >= DateTime.Now.Year && month >= DateTime.Now.Month)
                return null;

            pnlProcessing.SetTitle("Updating CAPE values...");
            pnlProcessing.SetMaxValue(2);

            List<KeyValuePair<int, decimal>>  capeValues = await CAPEModelDownloader.DownloadCAPE();

            DatabaseSingleton.Instance.StartTransaction();
            int lastCape = Convert.ToInt32(year.ToString() + month.ToString().PadLeft(2,'0'));

            foreach(KeyValuePair<int, decimal> capePair in capeValues)
            {
                if(capePair.Key > lastCape)
                {
                    //save cape

                    List<SQLiteParameter> parameters = new List<SQLiteParameter>();

                    parameters.Add(new SQLiteParameter("@p1", capePair.Key.ToString().Substring(0,4)));
                    parameters.Add(new SQLiteParameter("@p2", capePair.Key.ToString().Substring(4, 2)));
                    parameters.Add(new SQLiteParameter("@p3", capePair.Value));

                    DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO CAPE_VALUE (YEAR, MONTH, CAPE) VALUES (@p1,@p2,@p3)", parameters.ToArray());
                }
            }
            
            DatabaseSingleton.Instance.EndTransaction();

            return null;
        }


        public static async Task<object> UpdateSPYSymbols(ProcessingPanelControl pnlProcessing)
        {
            //update SPY Symbols
            pnlProcessing.SetTitle("UpdateSPYSymbols...");
            pnlProcessing.SetMaxValue(3);
            pnlProcessing.PerformStep();

            List<String> symbols = await SPYModelDownloader.DownloadSPY500Symbols();
            
            if (symbols.Count == 0)
                return null;

            pnlProcessing.PerformStep();

            //check symbols and update table if needed
            DatabaseSingleton.Instance.StartTransaction();

            try
            {
                DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM SPY_STOCKS", null);

                Dictionary<String, int> tickers = Stock.GetTickersListWithId();
                HashSet<int> insertedData = new HashSet<int>();
                foreach(String spy in symbols)
                {
                    if (!tickers.ContainsKey(spy))
                        continue;

                    int id = tickers[spy];

                    if (insertedData.Contains(id))
                        continue;

                    //this is just to avoid duplicates
                    insertedData.Add(id);

                    DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO SPY_STOCKS (ID_STOCK) VALUES (" + id + ")", null);
                }


                DatabaseSingleton.Instance.EndTransaction();

            }
            catch (Exception e)
            {
                DatabaseSingleton.Instance.RollbackTransaction();
            }
            
            return null;
        }


        public static async Task<object> UpdateSpyHiLoValues(ProcessingPanelControl pnlProcessing)
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT IFNULL(MAX(DATE), 0) FROM SPY_HILOW");
            int lastDate = 20010101;

            if (data.Rows.Count > 0)
                lastDate = Convert.ToInt32(data.Rows[0][0]);

            if(lastDate == 0)
                lastDate = 20010101;

            if (lastDate >= Utils.ConvertDateTimeToInt(DateTime.Now))
                return null;

            DataTable dates = DatabaseSingleton.Instance.GetData("select distinct date from price_history WHERE date > " + lastDate.ToString() + " AND ID_STOCK = -1");

            pnlProcessing.SetTitle("Updating SPY Hi Lo values...");
            pnlProcessing.SetMaxValue(dates.Rows.Count);
             
            try
            {
                DatabaseSingleton.Instance.StartTransaction();

                foreach (DataRow datesRow in dates.Rows)
                {
                    int processDate = Convert.ToInt32(datesRow[0]);

                    DataTable resultsCount = DatabaseSingleton.Instance.GetData("select COUNT(*) TOTAL, IFNULL(SUM(P.THREE_MONTHS_HIGH), 0) THREE_MONTHS_HIGH, IFNULL(SUM(THREE_MONTHS_LOW), 0) THREE_MONTHS_LOW from price_history P, spy_stocks S where P.date = " + processDate.ToString() + " and P.id_stock = S.ID_STOCK");

                    int total = Convert.ToInt32(resultsCount.Rows[0][0]);
                    int high = Convert.ToInt32(resultsCount.Rows[0][1]);
                    int low = Convert.ToInt32(resultsCount.Rows[0][2]);

                    double resultValue = 0;
                    if (total != 0)
                        resultValue = ((double)high / (double)total - (double)low / (double)total) * 100d;
                     
                    List<SQLiteParameter> parameters = new List<SQLiteParameter>();

                    parameters.Add(new SQLiteParameter("@p1", processDate));
                    parameters.Add(new SQLiteParameter("@p2", resultValue));

                    DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO SPY_HILOW (DATE, VALUE) VALUES (@p1,@p2)", parameters.ToArray());

                    if(StockSourcesReader.CancelProcess)
                    {
                        DatabaseSingleton.Instance.RollbackTransaction();
                        return null;
                    }

                    pnlProcessing.PerformStep();
                }
                
                DatabaseSingleton.Instance.EndTransaction();
            }
            catch(Exception e)
            {
                DatabaseSingleton.Instance.RollbackTransaction();
            }

            return null;
        }

        public static int GenerateVRHistoryId(PortfolioParameters portfolioParams)
        {
            bool newRecord = false;
            return GenerateVRHistoryId(portfolioParams, out newRecord);
        }

        public static int GenerateVRHistoryId(PortfolioParameters portfolioParams, out bool newRecord)
        {
            List<SQLiteParameter> parameters;
            newRecord = false;

            parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("@p1", portfolioParams.VRHistoryYears));
            parameters.Add(new SQLiteParameter("@p2", portfolioParams.CompositePEWeight));
            parameters.Add(new SQLiteParameter("@p3", portfolioParams.CompositePSWeight));
            parameters.Add(new SQLiteParameter("@p4", portfolioParams.CompositePBWeight));
            parameters.Add(new SQLiteParameter("@p5", portfolioParams.CompositePFCFWeight));

            DataTable vrHistoryId = DatabaseSingleton.Instance.GetData("SELECT ID FROM VR_HISTORY WHERE " +
                " YEARS = @p1 AND " +
                " PE_WEIGHT = @p2 AND " +
                " PS_WEIGHT = @p3 AND " +
                " PB_WEIGHT = @p4 AND " +
                " PFCF_WEIGHT = @p5 ", parameters.ToArray());

            if (vrHistoryId.Rows.Count > 0)
                return Convert.ToInt32(vrHistoryId.Rows[0][0]);

            //add new history id
            int newId = Convert.ToInt32(DatabaseSingleton.Instance.GetData("SELECT IFNULL(MAX(ID), 0) FROM VR_HISTORY").Rows[0][0]) + 1;
            newRecord = true;

            parameters.Add(new SQLiteParameter("@p6", newId));
            parameters.Add(new SQLiteParameter("@p7", Utils.ConvertDateTimeToInt(DateTime.Now)));
            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO VR_HISTORY (ID, YEARS, PE_WEIGHT, PS_WEIGHT, PB_WEIGHT, PFCF_WEIGHT, CREATION_DATE) VALUES (@p6, @p1, @p2, @p3, @p4, @p5, @p7)", parameters.ToArray());

            return newId;
        }

        public static void GenerateVRHistoryValues(ProcessingPanelControl pnlProcessing)
        {
            GenerateVRHistoryValues(pnlProcessing, false);
        }

        public static void GenerateVRHistoryValues(ProcessingPanelControl pnlProcessing, bool clearLastMonths)
        {
            pnlProcessing.SetTitle("Generating VR History Values...");
            pnlProcessing.SetMaxValue(505);
            pnlProcessing.PerformStep();

            //clean unused VR History IDs
            String sqlClean = "DELETE FROM VR_HISTORY WHERE ID IN (select H.ID from VR_HISTORY H WHERE NOT EXISTS  " +
                                " (SELECT P.ID FROM PORFOLIO_PARAMETERS P WHERE P.VR_HISTORY_YEARS = H.YEARS  " +
                                "		AND P.PE_WEIGHT = H.PE_WEIGHT " +
                                "		AND P.PS_WEIGHT = H.PS_WEIGHT " +
                                "		AND P.PB_WEIGHT = H.PB_WEIGHT " +
                                "		AND P.PFCF_WEIGHT = H.PFCF_WEIGHT  " +
                                "		AND (EXISTS(SELECT S.ID FROM STRATEGY S WHERE S.ID = P.ID) " +
                                "			OR P.ID = 0) ) ) AND CREATION_DATE < " + Utils.ConvertDateTimeToInt(DateTime.Now.AddDays(-2));

            DatabaseSingleton.Instance.ExecuteNonQuery(sqlClean, null);

            //clean last months if needed, this is for data updates (as we might have new filter data for old dates)
            if(clearLastMonths)
                DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM VR_HISTORY_RANKING WHERE DATE >= " + Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-3)), null);


            //generate all VR IDs if needed
            var portfoliosRaw = PortfolioParameters.LoadAll();
            List<PortfolioParameters> portfolios = new List<PortfolioParameters>();

            //get distinct portfolio parameters
            foreach (var portfolio in portfoliosRaw)
            {
                bool alreadyAdded = false;
                foreach (var portf in portfolios)
                {
                    if (portf.CompositePBWeight == portfolio.CompositePBWeight &&
                       portf.CompositePSWeight == portfolio.CompositePSWeight &&
                       portf.CompositePFCFWeight == portfolio.CompositePFCFWeight &&
                       portf.CompositePEWeight == portfolio.CompositePEWeight &&
                       portf.VRHistoryYears == portfolio.VRHistoryYears)
                        alreadyAdded = true;
                }

                if (!alreadyAdded)
                    portfolios.Add(portfolio);
            }


            //get last date for each composite saved for this stock
            DataTable maxDatesData = DatabaseSingleton.Instance.GetData("SELECT MAX(R.DATE) LAST_DATE, R.ID_STOCK, R.VR_HISTORY_ID, H.YEARS, cast(H.PE_WEIGHT as double) PE_WEIGHT, cast(H.PS_WEIGHT as double) PS_WEIGHT, cast(H.PB_WEIGHT as double) PB_WEIGHT, cast(H.PFCF_WEIGHT as double) PFCF_WEIGHT  FROM VR_HISTORY H, VR_HISTORY_RANKING R WHERE R.VR_HISTORY_ID = H.ID GROUP BY R.VR_HISTORY_ID, R.ID_STOCK");
            Dictionary<PortfolioParameters, Dictionary<int, int>> lastDates = new Dictionary<PortfolioParameters, Dictionary<int, int>>();
            foreach (var portfolio in portfolios)
                lastDates.Add(portfolio, new Dictionary<int, int>());
            int lastId = -1;
            PortfolioParameters lastPortf = null;
            foreach (DataRow row in maxDatesData.Rows)
            {
                if (lastId != Convert.ToInt32(row["VR_HISTORY_ID"]))
                {
                    lastPortf = null;
                    foreach (var portfolio in portfolios)
                    {
                        if (Convert.ToInt32(row["YEARS"]) == portfolio.VRHistoryYears &&
                            Convert.ToDecimal(row["PE_WEIGHT"]) == portfolio.CompositePEWeight &&
                            Convert.ToDecimal(row["PS_WEIGHT"]) == portfolio.CompositePSWeight &&
                            Convert.ToDecimal(row["PB_WEIGHT"]) == portfolio.CompositePBWeight &&
                            Convert.ToDecimal(row["PFCF_WEIGHT"]) == portfolio.CompositePFCFWeight)
                        {
                            portfolio.AuxHistoryId = Convert.ToInt32(row["VR_HISTORY_ID"]);
                            lastPortf = portfolio;
                            lastId = Convert.ToInt32(row["VR_HISTORY_ID"]);
                        }
                    }
                }

                if(lastPortf != null)
                    lastDates[lastPortf].Add(Convert.ToInt32(row["ID_STOCK"]), Convert.ToInt32(row["LAST_DATE"]));
            }


            DatabaseSingleton.Instance.ExecuteNonQuery("DROP INDEX IF EXISTS 'VR_HISTORY_RANKING_DATE_X'", null);

            //delete records without a History ID
            DataTable table = DatabaseSingleton.Instance.GetData("SELECT ID FROM VR_HISTORY");
            String idsToKeep = "-10 ";
            foreach (DataRow row in table.Rows)
                idsToKeep += ", " + row[0].ToString();
            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM VR_HISTORY_RANKING WHERE VR_HISTORY_ID NOT IN (" + idsToKeep + ")", null);

            var lastFilterDates = FilterTable.GetLastFilterDates(Utils.ConvertDateTimeToInt(DateTime.Now.AddDays(-1)));
                
            int totalSteps = stocksList.Count / 500;
            bool commandSaved = false;
            

            DatabaseSingleton.Instance.StartTransaction();
            foreach (Stock st in stocksList)
            {
                if (totalSteps-- < 0)
                {
                    totalSteps = stocksList.Count / 500;
                    pnlProcessing.PerformStep();

                    DatabaseSingleton.Instance.EndTransaction();

                    DatabaseSingleton.Instance.StartTransaction();
                }

                if (StockSourcesReader.CancelProcess)
                {
                    DatabaseSingleton.Instance.EndTransaction();

                    return;
                }

                //check if it has to be processed
                bool hasProcessToDo = true;
                int lastFilterDate = 0;
                if (lastFilterDates.ContainsKey(st.Id))
                    lastFilterDate = lastFilterDates[st.Id];
                foreach (var portfolio in portfolios)
                {
                    if (!lastDates.ContainsKey(portfolio))
                    {
                        hasProcessToDo = true;
                        break;
                    }

                    if(!lastDates[portfolio].ContainsKey(st.Id))
                    {
                        hasProcessToDo = true;
                        break;
                    }

                    if (lastDates[portfolio][st.Id] >= lastFilterDate)
                    {
                        hasProcessToDo = false;
                    }
                }

                if (!hasProcessToDo)
                    continue;

                //armo la tabla de registros por dia
                var filtersTables = FilterTable.LoadFilters(st.Id);
                var prices = Stock.GetAllPrices(st.Id);

                if (filtersTables.Count == 0)
                    continue;
                if (prices.Count == 0)
                    continue;

                //generate missing portfolio IDs
                foreach(var portfolio in portfolios)
                {
                    if (!lastDates.ContainsKey(portfolio))
                        lastDates.Add(portfolio, new Dictionary<int, int>());

                    if (portfolio.AuxHistoryId == -1)
                    {
                        commandSaved = false;
                        portfolio.AuxHistoryId = GenerateVRHistoryId(portfolio);
                    }
                }

                //generate dummy filters up until today (if needed)
                if (filtersTables[filtersTables.Count - 1].Date < Utils.ConvertDateTimeToInt(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)))
                {
                    var firstDayPrices = Stock.GetFirstDayPrices(st.Id, filtersTables[filtersTables.Count - 1].Date+1);
                    var lastFilter = filtersTables[filtersTables.Count - 1];

                    foreach (var priceAux in firstDayPrices)
                    {
                        var filterNew = new FilterTable(lastFilter);
                        filterNew.ClosePrice = priceAux.Close;
                        filterNew.Date = priceAux.Date;
                        filtersTables.Add(filterNew);
                    }
                    
                }


                foreach (var portfolio in portfolios)
                {
                    try
                    {
                        int dateFilter = 0;
                        if (lastDates[portfolio].ContainsKey(st.Id))
                            dateFilter = lastDates[portfolio][st.Id];

                        //for each stock generate the composite value for the whole history
                        int currentFilter = 0;
                        foreach (var value in prices)
                        {
                            if (currentFilter + 1 < filtersTables.Count)
                            {
                                while (value.Date >= filtersTables[currentFilter + 1].Date)
                                {
                                    currentFilter++;
                                    if (currentFilter + 1 >= filtersTables.Count)
                                    {
                                        currentFilter = filtersTables.Count - 1;
                                        break;
                                    }
                                }
                            }

                            var filter = filtersTables[currentFilter];
                            value.AuxComposite = filter.GetCompositeForPrice(portfolio, (double)value.Close)*-1;
                        }

                        var sortedPrices = prices.OrderBy(x => x.AuxComposite).ToArray();
                        
                        //loop each filter record and generate it´s composite value
                        foreach (var value in filtersTables)
                        {
                            //ignore future values
                            if (value.Date > Utils.ConvertDateTimeToInt(DateTime.Now.AddDays(-1)))
                                continue;

                            if (value.Date <= dateFilter)
                                continue;
                            
                            List<double> composites = new List<double>();
                            int initDate = value.Date - portfolio.VRHistoryYears * 10000;
                            foreach (var priceAux in sortedPrices)
                            {
                                if (priceAux.Date >= initDate && priceAux.Date <= value.Date)
                                    composites.Add(priceAux.AuxComposite);
                            }
                            
                            //get position in the collection of current composite
                            double currentComposite = value.GetComposite(portfolio)*-1;

                            int currentPosition = 0;
                            foreach (double composite in composites)
                            {
                                if (currentComposite <= composite)
                                    break;
                                currentPosition++;
                            }

                            double result = 0;
                            if (composites.Count() > 0)
                                result = Math.Round((double)currentPosition / (double)(composites.Count())*100, 2);

                            List<SQLiteParameter> parameters;

                            parameters = new List<SQLiteParameter>();
                            parameters.Add(new SQLiteParameter("@p1", portfolio.AuxHistoryId));
                            parameters.Add(new SQLiteParameter("@p2", st.Id));
                            parameters.Add(new SQLiteParameter("@p3", value.Date));
                            parameters.Add(new SQLiteParameter("@p4", result));

                            //save this composite value
                            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO VR_HISTORY_RANKING (VR_HISTORY_ID, ID_STOCK, DATE, RANK_VALUE) VALUES ( @p1,@p2,@p3, @p4)", parameters.ToArray(), commandSaved);
                            commandSaved = true;
                        }
                        
                    }
                    catch (Exception e)
                    {
                        int exa = 1;
                    }
                }

            }


            DatabaseSingleton.Instance.EndTransaction();

            DatabaseSingleton.Instance.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'VR_HISTORY_RANKING_DATE_X' ON 'VR_HISTORY_RANKING'(ID_STOCK, VR_HISTORY_ID, DATE)", null);
        }


        public static void ReprocessRollingMedianYears(int newYears)
        {
            int divisions = 100;
            String dateFilter = "";
            StrategyView.CancelResultsProcessing = false;
            
            try
            {
                DatabaseSingleton.Instance.StartTransaction();

                DatabaseSingleton.Instance.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'RATIO_DATE_FIRST' ON 'RATIO'(ID_STOCK, DATE_FIRST_MONTH)", null);

                Stock.ClearCache();

                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "ReprocessRollingMedian.txt";
                string SQLString = "";
                stocksList = Stock.ReadStockSymbols(false, null);

                using (Stream stream = assembly.GetManifestResourceStream("StockRanking." + resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    SQLString = reader.ReadToEnd();
                }

                for (int i = 0; i < (stocksList.Count / divisions) + 1; i++)
                {
                    String sqlBetween = (i * divisions).ToString() + " AND " + ((i + 1) * divisions - 1).ToString();

                    DatabaseSingleton.Instance.ExecuteNonQuery(SQLString.Replace("##BETWEEN##", sqlBetween).Replace("##YEARS##", newYears.ToString()), null);

                    Application.DoEvents();
                }

                DatabaseSingleton.Instance.ExecuteNonQuery("DROP INDEX 'RATIO_DATE_FIRST'", null);
                
                DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM SYSTEM_PARAMETERS WHERE ID = 3", null);
                DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO SYSTEM_PARAMETERS (ID, VALUE) VALUES (3, " + newYears + ")", null);

                DatabaseSingleton.Instance.EndTransaction();

                PortfolioParameters.RefreshCompositeYearsValue();

                Stock.ClearCache();
            }
            catch (Exception e)
            {
                DatabaseSingleton.Instance.RollbackTransaction();
                throw e;
            }

        }

        public static void ImportCustomMTUMData(String mtumFile)
        {
            DatabaseSingleton.Instance.StartTransaction();

            try
            {
                TextFieldParser textParser = new TextFieldParser(mtumFile);
                if (textParser.ReadLine().Contains(";"))
                    textParser.SetDelimiters(new string[] { ";" });
                else
                    textParser.SetDelimiters(new string[] { "," });
                textParser.HasFieldsEnclosedInQuotes = false;

                int mtumBackfillDate = Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-3));
                int mtumFirstDate = 20010101;

                DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM PRICE WHERE ID_STOCK IN (-9,-10)", null);
                DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM PRICE_HISTORY WHERE ID_STOCK IN (-9,-10)", null);
                int lastDateSaved = 0;

                List<StockValue> allValues = new List<StockValue>();

                while (!textParser.EndOfData)
                {
                    String[] data = textParser.ReadFields();

                    int date = Utils.ConvertStringDateToInt(data[0]);

                    if (data[1] == "@NA" || data[1].Trim() == "")
                        continue;

                    decimal value = Convert.ToDecimal(data[1].Replace(",", ""));

                    StockValue newValue = new StockValue();
                    newValue.Close = value;
                    newValue.Date = date;

                    allValues.Add(newValue);
                }

                allValues = allValues.OrderBy(x => x.Date).ToList();

                StockValue lastDatePrice = null;
                foreach (var newValue in allValues)
                {
                    if (newValue.Date > mtumFirstDate)
                    {
                        if (lastDateSaved + 50 < newValue.Date)
                        {
                            lastDateSaved = newValue.Date;
                            StockValue.CommandSaved = false;
                            newValue.Save(-10, true);

                            if (newValue.Date < mtumBackfillDate)
                                newValue.Save(-9, true);

                        }

                        StockValue.CommandSaved = false;
                        newValue.SaveHistoricPrice(-10);
                        if (newValue.Date < mtumBackfillDate)
                            newValue.SaveHistoricPrice(-9);
                        
                    }

                    lastDatePrice = newValue;
                }

                DatabaseSingleton.Instance.EndTransaction();
            }
            catch(Exception ex)
            {
                DatabaseSingleton.Instance.RollbackTransaction();
                throw ex;
            }
        }


        public static bool CheckMTUMData()
        {
            try
            {
                int resultsCount = Convert.ToInt32(DatabaseSingleton.Instance.GetData("select COUNT(*) FROM PRICE WHERE ID_STOCK = -9").Rows[0][0]);
                if (resultsCount > 0)
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        
        public static void UpdateFileBasedStocks(ProcessingPanelControl pnlProcessing, bool completeReprocess)
        {
            var stocks = FileBasedStock.GetAllFileBasedStocks();

            pnlProcessing.SetTitle("Updating File Based Symbols...");
            pnlProcessing.SetMaxValue(stocks.Count+1);

            foreach (var stock in stocks)
            {
                pnlProcessing.PerformStep();

                if (!stock.KeepUpdated)
                    continue;

                if (stock.LastPriceDate >= Utils.ConvertDateTimeToInt(DateTime.Now.AddDays(-1)) && !completeReprocess)
                    continue;

                if (!File.Exists(stock.Path))
                    continue;
                
                if (!FileBasedStock.ImportFileData(stock))
                {
                    throw new Exception("There was an error importing the data file for " + stock.Stock.Symbol + ", please check the file and try again. File Path: " + stock.Path, null);
                }

            }

        }
        
        private static async Task<object> GenerateETFsList(ProcessingPanelControl pnlProcessing)
        {
            if (ZachsSourceReader.CancelProcess)
                return null;

            ETFStockInfo.CommandSaved = false;

            try
            {
                object test = await ZachsSourceReader.GenerateETFsList(pnlProcessing);
            }
            catch (Exception e)
            {
                //ignore exception, file will be retrieved on the next update
                int a = 12;
            }

            return null;
        }
        
        public static async Task<object> ProcessETFPrices(ProcessingPanelControl pnlProcessing)
        {
            DataUpdater.UpdatingDatabase = true;

            ZachsSourceReader.CancelProcess = false;
            StockSourcesReader.CancelProcess = false;

            pnlProcessing.StartProcess();

            try
            {
                object a;

                a = await GenerateETFPrices(pnlProcessing, false);

                pnlProcessing.StopProcess();
            }
            catch (Exception ex)
            {
                pnlProcessing.ShowError(ex.Message + " Stack: " + ex.StackTrace);
                DataUpdater.UpdatingDatabase = false;
                return null;
            }


            DataUpdater.UpdatingDatabase = false;
            return null;
        }
        
        public static async Task<object> GenerateETFPrices(ProcessingPanelControl pnlProcessing, bool completeReprocess)
        {
            DataUpdater.UpdatingDatabase = true;

            ZachsSourceReader.CancelProcess = false;
            StockSourcesReader.CancelProcess = false;

            var etfStocks = Stock.GetAllETFStocks();

            pnlProcessing.SetTitle("Updating ETF Prices...");
            pnlProcessing.SetMaxValue(etfStocks.Count+1);
            pnlProcessing.PerformStep();

            String lastStock = "";
            bool openedTransaction = false;
            try
            {

                //first delete all values if needed while index is in place
                foreach (var etfStock in etfStocks)
                {
                    if (completeReprocess)
                        etfStock.LastValuesDate = 0;

                    if (etfStock.LastValuesDate < Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-3)))
                        etfStock.DeleteAllPrices();
                }

                foreach (var etfStock in etfStocks)
                {
                    if (ZachsSourceReader.CancelProcess)
                        return null;
                    
                    lastStock = etfStock.Symbol;
                    List<StockValue> stockValues;
                    var dividends = new List<KeyValuePair<int, decimal>>();

                    if (etfStock.LastValuesDate < Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-3)))
                    {
                        stockValues = ZachsSourceReader.GetSharadarETFPricesFromFile(etfStock, pnlProcessing, DateTime.MinValue, dividends);
                    }
                    else
                    {
                        stockValues = ZachsSourceReader.GetSharadarETFPricesFromFile(etfStock, pnlProcessing, Utils.ConvertIntToDateTime(etfStock.LastValuesDate), dividends);
                    }
                    
                    if (stockValues.Count == 0)
                        continue;

                    int lastDateSaved = etfStock.LastValuesDate;

                    DatabaseSingleton.Instance.StartTransaction();
                    openedTransaction = true;

                    stockValues = stockValues.OrderBy(x => x.Date).ToList();
                    StockValue lastDatePrice = null;

                    foreach (StockValue newValue in stockValues)
                    {
                        if (newValue.Date > etfStock.LastValuesDate && newValue.Date < Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")))
                        {
                            if (lastDateSaved + 50 < newValue.Date)
                            {
                                lastDateSaved = newValue.Date;
                                StockValue.CommandSaved = false;
                                newValue.Save(etfStock.Id, true);
                            }

                            StockValue.CommandSaved = false;
                            newValue.SaveHistoricPrice(etfStock.Id);
                        }

                        lastDatePrice = newValue;
                    }

                    foreach (var dividendsPair in dividends.Where(x => x.Key > etfStock.LastValuesDate && x.Key < Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"))))
                    {
                        Stock.SaveDividend(etfStock.Id, dividendsPair.Key, dividendsPair.Value);
                    }

                    DatabaseSingleton.Instance.EndTransaction();
                    openedTransaction = false;

                    pnlProcessing.PerformStep();
                }
            }
            catch (Exception ex)
            {
                if (openedTransaction)
                    DatabaseSingleton.Instance.RollbackTransaction();

                pnlProcessing.ShowError("Error trying to update " + lastStock + ". Error: " + ex.Message + " Stack: " + ex.StackTrace);
                DataUpdater.UpdatingDatabase = false;
                return null;
            }

            return null;
        }


    }
}
