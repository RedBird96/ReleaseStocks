using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockRanking
{
    public class Stock : CachedValuesStock
    {
        public int Id = 0;
        public String Symbol = "";
        public String CompanyName = "";

        public String Industry = "";
        public String Industry2 = "";
        public String Currency = "";
        public String Sector = "";
        public String Exchange = "";
        public bool IsForeign = false;
        public String RelatedTickers = "";

        public int FirstValuesDate = 0;
        public int LastValuesDate = 0;
        public int LastRatiosDate = 0;
        public int LastFundamentalsDate = 0;
        public int FirstHistoryPricesDate = 0;

        public float RankValue = 0;
        public decimal FIP = 0;
        public int PositiveEBITYears = 0;
        public long SharesOut = 0;
        public double CloseJan1 = 0;

        public StockTypes Type = StockTypes.Regular;

        //auxiliary for backtest processing
        public decimal CurrentPrice = 0;
        public decimal CurrentVolume = 0;

        public StockValue CurrentValues = new StockValue();
        public static bool TickerCommandSaved = false;

        public List<StockRatios> Ratios = new List<StockRatios>();
        public List<StockFundamentals> Fundamentals = new List<StockFundamentals>();
        public List<FeatureTable> FeatureTables = new List<FeatureTable>();
        public List<FilterTable> FiltersTables = new List<FilterTable>();
         
        public List<StockValue> stockValues = new List<StockValue>();
        public List<KeyValuePair<int,decimal>> stockDividends => this.dividends;

        List<Feature> features = new List<Feature>();

        public List<Feature> Features { get => features; set => features = value; }
        public FilterTable CurrentFilters { get; set; }
        //used for equation filters in RankingCalculator.cs
        public StockRatios CurrentRatio { get; set; }

        public static List<Stock> CachedStocks = null;

        public static Dictionary<String, decimal> CachedPrice = new Dictionary<string, decimal>();

        public Dictionary<String, Double> filterResults = new Dictionary<string, Double>();

        public bool Has1YearBackData = false;
        public bool Has3YearBackData = false;
        public bool Has5YearBackData = false;

        //auxiliary value for ranking processes
        public double AuxVal = 0;
        public double AuxRank = 0;
        public List<StockRatios> TempRatios = new List<StockRatios>(); //used to download and save ratios

        double avgVolumeAux = double.MinValue;

        public double filterValue = double.MinValue;
        public double AvgVolume
        {
            get
            {
                if(avgVolumeAux == double.MinValue)
                {
                    if (stockValues.Count == 0)
                        return 0;

                    avgVolumeAux = stockValues.Average(x => x.Volume);
                }

                return avgVolumeAux;
            }
        }

        public override String StockSymbol => Symbol;

        public override int IdStock { get => this.Id; }

        public Stock()
        {

        }

        public Stock(DataRow row)
        {
            Id = Convert.ToInt32(row["ID"]);
            Symbol = Convert.ToString(row["SYMBOL"]);
            CompanyName = Convert.ToString(row["COMPANY_NAME"]);
            Industry = Convert.ToString(row["INDUSTRY"]);
            Industry2 = Convert.ToString(row["INDUSTRY2"]);
            IsForeign = Convert.ToInt32(row["ISFOREIGN"]) == 1;
            Currency = Convert.ToString(row["CURRENCY"]);
            Sector = Convert.ToString(row["SECTOR"]);
            Exchange = Convert.ToString(row["EXCHAGE"]);
            RelatedTickers = Convert.ToString(row["RELATED_TICKERS"]);
            Type = (StockTypes)Convert.ToInt32(row["TYPE"]);
        }

        public String getStockIndustry()
        {
            return getStockIndustry(false);
        }

        public String getStockSector()
        {
            return getStockIndustry(true);
        }

        public String getStockIndustry(bool groupBySectors)
        {
            if (groupBySectors)
                return IndustryGroup.GetSector(Industry2);
            else
                return Industry2;
        }

        public static Stock GetStock(int id)
        {
            Stock stock = null;
            if(CachedStocks != null)
            {
                stock = CachedStocks.Find(x => x.Id == id);
            }

            if (stock == null)
            {
                DataTable data = DatabaseSingleton.Instance.GetData("SELECT * FROM STOCK WHERE ID = " + id.ToString());

                if (data.Rows.Count > 0)
                    stock = new Stock(data.Rows[0]);
            }

            return stock;
        }

        public void Save()
        {
            Save(false);
        }

        public void Save(bool useNegativeId)
        {
            //get last ID
            int newId = 0;
            if (this.Id == 0)
            {
                if (useNegativeId)
                    newId = GetNextNegativeId();
                else
                    newId = Convert.ToInt32(DatabaseSingleton.Instance.GetData("SELECT IFNULL(MAX(ID), 0) FROM STOCK").Rows[0][0]) + 1;

                List<SQLiteParameter> parameters = new List<SQLiteParameter>();

                parameters.Add(new SQLiteParameter("@p1", newId));
                parameters.Add(new SQLiteParameter("@p2", Symbol));
                parameters.Add(new SQLiteParameter("@p3", CompanyName));
                parameters.Add(new SQLiteParameter("@p4", Industry));
                parameters.Add(new SQLiteParameter("@p5", Industry2));
                parameters.Add(new SQLiteParameter("@p6", Currency));
                parameters.Add(new SQLiteParameter("@p7", Sector));
                parameters.Add(new SQLiteParameter("@p8", Exchange));
                parameters.Add(new SQLiteParameter("@p9", IsForeign ? 1 : 0));
                parameters.Add(new SQLiteParameter("@p10", RelatedTickers));
                parameters.Add(new SQLiteParameter("@p11", (int)Type));

                DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO STOCK (ID, SYMBOL, COMPANY_NAME, INDUSTRY, INDUSTRY2, CURRENCY, SECTOR, EXCHAGE, ISFOREIGN, RELATED_TICKERS, TYPE) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11)", parameters.ToArray());

                this.Id = newId;
            }
            else
            {
                List<SQLiteParameter> parameters = new List<SQLiteParameter>();

                parameters.Add(new SQLiteParameter("@p1", newId));
                parameters.Add(new SQLiteParameter("@p2", Symbol));
                parameters.Add(new SQLiteParameter("@p3", CompanyName));
                
                DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE STOCK SET SYMBOL = @p2, COMPANY_NAME = @p3 WHERE ID = @p1", parameters.ToArray());
            }
        }
        
        public static void Delete(int idStock)
        {
            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STOCK WHERE ID = " + idStock, null);
            var stock = CachedStocks.Find(x => x.Id == idStock);
            if (stock != null)
                CachedStocks.Remove(stock);
        }

        public static List<Stock> GetCurrentStockNames()
        {
            List<Stock> result = new List<Stock>();

            DataTable data = DatabaseSingleton.Instance.GetData("SELECT * FROM STOCK");

            foreach (DataRow row in data.Rows)
            {
                result.Add(new Stock(row));
            }

            return result;
        }
        
        public static List<String> GetTickersList()
        {
            List<String> result = new List<String>();

            DataTable data = DatabaseSingleton.Instance.GetData("SELECT SYMBOL FROM STOCK");

            foreach (DataRow row in data.Rows)
            {
                result.Add(Convert.ToString(row["SYMBOL"]));
            }

            return result;
        }

        public static Dictionary<String, int> GetTickersListWithId()
        {
            Dictionary<String, int> result = new Dictionary<string, int>();
            Dictionary<String, int> relatedDictionary = new Dictionary<string, int>();

            DataTable data = DatabaseSingleton.Instance.GetData("SELECT SYMBOL, ID, IFNULL(RELATED_TICKERS, '') RELATED_TICKERS FROM STOCK");

            foreach (DataRow row in data.Rows)
            {
                result.Add(Convert.ToString(row["SYMBOL"]), Convert.ToInt32(row["ID"]));

                String relatedTickers = Convert.ToString(row["RELATED_TICKERS"]);
                if (relatedTickers.Trim() != "")
                {
                    foreach (String related in relatedTickers.Split(','))
                    {
                        if(!relatedDictionary.ContainsKey(related))
                            relatedDictionary.Add(related, Convert.ToInt32(row["ID"]));
                    }
                }
            }

            //add related tickers if not repeated
            foreach(String key in relatedDictionary.Keys)
            {
                if (!result.ContainsKey(key))
                    result.Add(key, relatedDictionary[key]);
            }

            return result;
        }


        public static int GetStockIdFromTicker(String symbol)
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT ID FROM STOCK WHERE SYMBOL = '" + symbol + "'");
            if(data.Rows.Count > 0)
                return Convert.ToInt32(data.Rows[0][0]);
            return 0;
        }


        public static void ClearCache()
        {
            if (CachedStocks != null)
            {
                CachedStocks.Clear();
                CachedStocks.TrimExcess();
                CachedStocks = null;
                GC.Collect();
            }
        }
         
        public static List<Stock> ReadStockSymbols(bool fillFeaturesAndFilters, ProcessingPanelControl pnlProgress)
        { 
            if (CachedStocks != null)
                return CachedStocks;

            pnlProgress?.SetTitle("Reading stocks from database...");
            pnlProgress?.SetMaxValue(14000 / 100);

            List<Stock> result = new List<Stock>();

            DataTable data = DatabaseSingleton.Instance.GetData("SELECT STOCK.*, (SELECT IFNULL(MAX(PRICE.DATE), 0) FROM PRICE WHERE PRICE.ID_STOCK = STOCK.ID) LAST_DATE, (SELECT IFNULL(MIN(PRICE_HISTORY.DATE), 0) FROM PRICE_HISTORY WHERE PRICE_HISTORY.ID_STOCK = STOCK.ID) FIRST_HISTORY_PRICE_DATE FROM STOCK ORDER BY ID ASC");// ORDER BY ID ASC LIMIT 300");

            int count = 0;

            foreach (DataRow row in data.Rows)
            {
                count++;

                if(count > 100)
                {
                    count = 0;
                    if (pnlProgress != null)
                        pnlProgress.PerformStep();
                }

                Stock newS = new Stock(row);
                newS.LastValuesDate = Convert.ToInt32(row["LAST_DATE"]);
                newS.FirstHistoryPricesDate = Convert.ToInt32(row["FIRST_HISTORY_PRICE_DATE"]);
                result.Add(newS);
                
                if (fillFeaturesAndFilters)
                {
                    //load features
                    newS.FeatureTables = FeatureTable.LoadFeatures(newS.Id);
                    //load filters
                    newS.FiltersTables = FilterTable.LoadFilters(newS.Id);
                    //load dividends
                    newS.dividends = Stock.LoadDividends(newS.Id);

                    StockRatios.LoadRatios(newS);

                }

                if (StrategyView.CancelResultsProcessing)
                {
                    return null;
                }
            }

            if (StrategyView.CancelResultsProcessing)
                return null;

            CachedStocks = result;

            return result;
        }

        public static int ProgressValues = 0;
        const int threadsTotal = 4;
        public static bool[] ThreadsFinished = new bool[threadsTotal];
        public static object ProgressValuesObject = new object();
        public static List<Stock> ReadStockSymbolsThreads(bool fillFeaturesAndFilters, ProcessingPanelControl pnlProgress)
        {
            if (CachedStocks != null)
            {
                return CachedStocks;
            }
                

            pnlProgress?.SetTitle("Reading stocks from database...");
            pnlProgress?.SetMaxValue(14000 / 100);

            if (!fillFeaturesAndFilters || Environment.ProcessorCount < 4)
                return ReadStockSymbols(fillFeaturesAndFilters, pnlProgress);
            
            ProgressValues = 0;

            List<Stock> result = new List<Stock>();
            
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT STOCK.*, (SELECT IFNULL(MAX(PRICE.DATE), 0) FROM PRICE WHERE PRICE.ID_STOCK = STOCK.ID) LAST_DATE, (SELECT IFNULL(MIN(PRICE_HISTORY.DATE), 0) FROM PRICE_HISTORY WHERE PRICE_HISTORY.ID_STOCK = STOCK.ID) FIRST_HISTORY_PRICE_DATE FROM STOCK ORDER BY ID ASC");// ORDER BY ID ASC LIMIT 300");

            int count = 0;

            DateTime stamp = DateTime.Now;


            foreach (DataRow row in data.Rows)
            {
                Stock newS = new Stock(row);
                newS.LastValuesDate = Convert.ToInt32(row["LAST_DATE"]);
                newS.FirstHistoryPricesDate = Convert.ToInt32(row["FIRST_HISTORY_PRICE_DATE"]);
                result.Add(newS);
            }

            //divide stocks by 4
            List<BackgroundWorker> workerThreads = new List<BackgroundWorker>();
            int init = 0;
            for (int i = 0; i < threadsTotal; i++)
            {
                ThreadsFinished[i] = false;
                int topl = result.Count / threadsTotal;
                if (i == threadsTotal-1)
                    topl = result.Count - 1 - init;
                List<Stock> stockToProcess = result.GetRange(init, topl);
                init += topl;

                BackgroundWorker newThread = new BackgroundWorker();
                workerThreads.Add(newThread);

                newThread.DoWork += new DoWorkEventHandler(threadDoWork);
                newThread.WorkerReportsProgress = true;
                newThread.WorkerSupportsCancellation = true;
                KeyValuePair<int, List<Stock>> pair = new KeyValuePair<int, List<Stock>>(i, stockToProcess);
                newThread.RunWorkerAsync(argument: pair);
            }

            int lastProgressValue = 0;
            while (true)
            {
                bool processEnded = true;
                foreach (bool val in ThreadsFinished)
                {
                    if (!val)
                    {
                        processEnded = false;
                        break;
                    }
                }

                if (processEnded)
                    break;
                
                Thread.Sleep(100);

                Application.DoEvents();

                int tmp = ProgressValues;
                if (lastProgressValue != tmp)
                {
                    int tmp1 = tmp - lastProgressValue;
                    lastProgressValue = tmp;
                    while (tmp1-- > 0)
                        pnlProgress.PerformStep();
                }
            }

            if (StrategyView.CancelResultsProcessing)
                return null;

            double seconds = (DateTime.Now - stamp).TotalSeconds;

            CachedStocks = result;
             
            return result;
        }
        
        public static void threadDoWork(object sender, DoWorkEventArgs e)
        {
            KeyValuePair<int, List<Stock>> pair = (KeyValuePair<int, List<Stock>>)e.Argument;
            List<Stock> stockToProcess = pair.Value;
            int count = 0;
            DatabaseSingleton dbConn = new DatabaseSingleton(true);
            foreach (Stock stock in stockToProcess)
            {
                count++;

                if (count > 100)
                {
                    count = 0;
                    lock(ProgressValuesObject)
                        ProgressValues++;
                }

                //load features
                stock.FeatureTables = FeatureTable.LoadFeatures(stock.Id, dbConn);
                //load filters
                stock.FiltersTables = FilterTable.LoadFilters(stock.Id, dbConn);
                //load dividends
                stock.dividends = Stock.LoadDividends(stock.Id, dbConn);

                StockRatios.LoadRatios(stock);

                if (StrategyView.CancelResultsProcessing)
                {
                    break;
                }
            }

            dbConn.CloseDatabase();

            lock (ProgressValuesObject)
                ThreadsFinished[pair.Key] = true;
        }

        private FeatureTable BinarySearchFeatureTable(int dateInt)
        {
            int count = this.FeatureTables.Count;

            if (count == 0) return null;

            int low = 0, high = count, mid;

            while (high - low > 1)
            {
                mid = (high + low) / 2;
                if (this.FeatureTables[mid].Date == dateInt)
                {
                    return this.FeatureTables[mid];
                }
                else if (this.FeatureTables[mid].Date > dateInt)
                {
                    high = mid;
                }
                else
                {
                    low = mid;
                }
            }
            if (this.FeatureTables[low].Date == dateInt)
            {
                return this.FeatureTables[low];
            }
            return null;
        }

        public void GenerateFeaturesFromTable(bool[] featuresEnabled, DateTime selectedDate)
        {
            //the orden in which the features are added relates to the index of FeatureWeight 
            //(it will also be the FeatureIndex in the features collection in each stock)

            features.Clear();
            
            int dateInt = Utils.ConvertDateTimeToInt(selectedDate);

            //FeatureTable currentFeatures = this.FeatureTables.Find(x => x.Date == dateInt);
            FeatureTable currentFeatures = BinarySearchFeatureTable(dateInt);

            if (this.FeatureTables.Count > 1 && selectedDate > DateTime.Now.AddMonths(-6))
            {
                if (currentFeatures == null)
                    currentFeatures = this.FeatureTables[this.FeatureTables.Count - 1];
                currentFeatures = copyFeatureIfAll0(currentFeatures, this.FeatureTables[this.FeatureTables.Count - 2]);
            }
            
            if (currentFeatures == null)
                return;

            int i = 0;

            //add FCF features
            features.Add(new Feature(FeatureTypes.FCFGrowth, (float)currentFeatures.FCF1G, 1));
            features.Add(new Feature(FeatureTypes.FCFGrowth, (float)currentFeatures.FCF3G, 3));
            features.Add(new Feature(FeatureTypes.FCFGrowth, (float)currentFeatures.FCF5G, 5));
            features.Add(new Feature(FeatureTypes.FCFMedian, (float)currentFeatures.FCF1M, 1));
            features.Add(new Feature(FeatureTypes.FCFMedian, (float)currentFeatures.FCF3M, 3));
            features.Add(new Feature(FeatureTypes.FCFMedian, (float)currentFeatures.FCF5M, 5));

            //add ROIC features
            features.Add(new Feature(FeatureTypes.ROICGrowth, (float)currentFeatures.ROIC1G, 1));
            features.Add(new Feature(FeatureTypes.ROICGrowth, (float)currentFeatures.ROIC3G, 3));
            features.Add(new Feature(FeatureTypes.ROICGrowth, (float)currentFeatures.ROIC5G, 5));
            features.Add(new Feature(FeatureTypes.ROICMedian, (float)currentFeatures.ROIC1M, 1));
            features.Add(new Feature(FeatureTypes.ROICMedian, (float)currentFeatures.ROIC3M, 3));
            features.Add(new Feature(FeatureTypes.ROICMedian, (float)currentFeatures.ROIC5M, 5));
             
            //add EBITDA features
            features.Add(new Feature(FeatureTypes.EBITDAGrowth, (float)currentFeatures.EBITA1G, 1));
            features.Add(new Feature(FeatureTypes.EBITDAGrowth, (float)currentFeatures.EBITA3G, 3));
            features.Add(new Feature(FeatureTypes.EBITDAGrowth, (float)currentFeatures.EBITA5G, 5));
            features.Add(new Feature(FeatureTypes.EBITDAMedian, (float)currentFeatures.EBITA1M, 1));
            features.Add(new Feature(FeatureTypes.EBITDAMedian, (float)currentFeatures.EBITA3M, 3));
            features.Add(new Feature(FeatureTypes.EBITDAMedian, (float)currentFeatures.EBITA5M, 5));

            features.Add(new Feature(FeatureTypes.FIPScore, (float)currentFeatures.FIP, 1));

            //add EBITDA features
            features.Add(new Feature(FeatureTypes.EVGrowth, (float)currentFeatures.EV1G, 1));
            features.Add(new Feature(FeatureTypes.EVGrowth, (float)currentFeatures.EV3G, 3));
            features.Add(new Feature(FeatureTypes.EVGrowth, (float)currentFeatures.EV5G, 5));
            features.Add(new Feature(FeatureTypes.EVMedian, (float)currentFeatures.EV1M, 1));
            features.Add(new Feature(FeatureTypes.EVMedian, (float)currentFeatures.EV3M, 3));
            features.Add(new Feature(FeatureTypes.EVMedian, (float)currentFeatures.EV5M, 5));

            //add BVPS features
            features.Add(new Feature(FeatureTypes.BVPSGrowth, (float)currentFeatures.BVPS1G, 1));
            features.Add(new Feature(FeatureTypes.BVPSGrowth, (float)currentFeatures.BVPS3G, 3));
            features.Add(new Feature(FeatureTypes.BVPSGrowth, (float)currentFeatures.BVPS5G, 5));
            features.Add(new Feature(FeatureTypes.BVPSMedian, (float)currentFeatures.BVPS1M, 1));
            features.Add(new Feature(FeatureTypes.BVPSMedian, (float)currentFeatures.BVPS3M, 3));
            features.Add(new Feature(FeatureTypes.BVPSMedian, (float)currentFeatures.BVPS5M, 5));

            //add CROIC features
            features.Add(new Feature(FeatureTypes.CROICGrowth, (float)currentFeatures.CROIC1G, 1));
            features.Add(new Feature(FeatureTypes.CROICGrowth, (float)currentFeatures.CROIC3G, 3));
            features.Add(new Feature(FeatureTypes.CROICGrowth, (float)currentFeatures.CROIC5G, 5));
            features.Add(new Feature(FeatureTypes.CROICMedian, (float)currentFeatures.CROIC1M, 1));
            features.Add(new Feature(FeatureTypes.CROICMedian, (float)currentFeatures.CROIC3M, 3));
            features.Add(new Feature(FeatureTypes.CROICMedian, (float)currentFeatures.CROIC5M, 5));

            features.Add(new Feature(FeatureTypes.MTUM, (float)currentFeatures.MTUM, 1));

            features.Add(new Feature(FeatureTypes.SORTINO_FIP, (float)currentFeatures.SortinoFIP, 1));

            //add SALES features
            features.Add(new Feature(FeatureTypes.SALESGrowth, (float)currentFeatures.SALES1G, 1));
            features.Add(new Feature(FeatureTypes.SALESGrowth, (float)currentFeatures.SALES3G, 3));
            features.Add(new Feature(FeatureTypes.SALESGrowth, (float)currentFeatures.SALES5G, 5));
            features.Add(new Feature(FeatureTypes.SALESMedian, (float)currentFeatures.SALES1M, 1));
            features.Add(new Feature(FeatureTypes.SALESMedian, (float)currentFeatures.SALES3M, 3));
            features.Add(new Feature(FeatureTypes.SALESMedian, (float)currentFeatures.SALES5M, 5));

            features.Add(new Feature(FeatureTypes.PEG_RATIO, (float)currentFeatures.PEG_RATIO, 1));
            features.Add(new Feature(FeatureTypes.EBITDA_Liabilities, (float)currentFeatures.EBITDA_Liabilities, 1));
            features.Add(new Feature(FeatureTypes.GP_Assets, (float)currentFeatures.GP_Assets, 1));
            features.Add(new Feature(FeatureTypes.FCF_Sales, (float)currentFeatures.FCF_Sales, 1));
            features.Add(new Feature(FeatureTypes.EV_EBITDA, (float)currentFeatures.EV_EBITDA, 1));
            features.Add(new Feature(FeatureTypes.Close_FCF, (float)currentFeatures.Close_FCF, 1));
            features.Add(new Feature(FeatureTypes.EV_Revenue, (float)currentFeatures.EV_Revenue, 1));
            features.Add(new Feature(FeatureTypes.Dividend_BuyBackRate, (float)currentFeatures.Dividend_BuyBackRate, 1));
            features.Add(new Feature(FeatureTypes.Operating_Margin, (float)currentFeatures.Operating_Margin, 1));
            features.Add(new Feature(FeatureTypes.ROIC_EV_EBITDA, (float)currentFeatures.ROIC_EV_EBITDA, 1));

            features.Add(new Feature(FeatureTypes.ROICScore, (float)currentFeatures.ROICSCORE, 1));
            features.Add(new Feature(FeatureTypes.CROICScore, (float)currentFeatures.CROICSCORE, 1));
            features.Add(new Feature(FeatureTypes.FCFScore, (float)currentFeatures.FCFSCORE, 1));
            features.Add(new Feature(FeatureTypes.EBITDAScore, (float)currentFeatures.EBITDASCORE, 1));
            features.Add(new Feature(FeatureTypes.SALESScore, (float)currentFeatures.SALESSCORE, 1));

            features.Add(new Feature(FeatureTypes.VRUniverse, 0f, 1));
            features.Add(new Feature(FeatureTypes.VRSector, 0f, 1));
            features.Add(new Feature(FeatureTypes.VRHistory, 0f, 1));

            features.Add(new Feature(FeatureTypes.SHARPE_FIP, (float)currentFeatures.SharpeFIP, 1));

            Has1YearBackData = currentFeatures.Has1YearBackData;
            Has3YearBackData = currentFeatures.Has3YearBackData;
            Has5YearBackData = currentFeatures.Has5YearBackData;
        }

        public static void UpdateCurrentValues(DateTime date, List<Stock> stocks)
        {
            int dateInt = Utils.ConvertDateTimeToInt(date);

            DataTable table = DatabaseSingleton.Instance.GetData("select P1.ID_STOCK, cast(ifnull(cast(P1.CLOSE_PRICE as double), 0)as double) CLOSE_PRICE, ifnull(P1.VOLUME, 0) VOLUME FROM PRICE P1 WHERE P1.DATE = " + dateInt.ToString() + " AND P1.CLOSE_PRICE IS NOT NULL");
            Dictionary<int, Stock> stocksTmp = new Dictionary<int, Stock>();

            foreach(Stock st in stocks)
            {
                st.CurrentPrice = 0;
                st.CurrentVolume = 0;
                stocksTmp.Add(st.Id, st);
            }
            
            foreach (DataRow row in table.Rows)
            {
                int idStock = Convert.ToInt32(row["ID_STOCK"]);
                if (stocksTmp.ContainsKey(idStock))
                {
                    stocksTmp[idStock].CurrentPrice = Convert.ToDecimal(row[1]);
                    stocksTmp[idStock].CurrentVolume = Convert.ToDecimal(row[2]);
                }
            }

        }
        
        //asign last available price to all the stocks
        public static void UpdateLastPrice(List<Stock> stocks)
        {
            DataTable table = DatabaseSingleton.Instance.GetData("select S.id, IFNULL((select cast(ifnull(cast(P1.CLOSE_PRICE as double), 0) as double) CLOSE_PRICE FROM PRICE P1 WHERE P1.CLOSE_PRICE IS NOT NULL AND P1.ID_STOCK = S.ID ORDER BY P1.DATE DESC LIMIT 1) , 1) from stock S");
            Dictionary<int, Stock> stocksTmp = new Dictionary<int, Stock>();

            foreach (Stock st in stocks)
            {
                st.CurrentPrice = 0;
                stocksTmp.Add(st.Id, st);
            }

            foreach (DataRow row in table.Rows)
            {
                int idStock = Convert.ToInt32(row["ID"]);
                if (stocksTmp.ContainsKey(idStock))
                {
                    stocksTmp[idStock].CurrentPrice = Convert.ToDecimal(row[1]);
                }
            }

        }
        
        public static decimal GetLastPrice(int stockId)
        {
            int date = 0;

            return GetLastPrice(stockId, out date);
        }

        public static decimal GetLastPrice(int stockId, out int lastDate)
        {
            DataTable table = DatabaseSingleton.Instance.GetData("select cast(ifnull(cast(P1.CLOSE_PRICE as double), 0) as double) CLOSE_PRICE, DATE FROM PRICE_HISTORY P1 WHERE P1.CLOSE_PRICE IS NOT NULL AND P1.ID_STOCK = " + stockId.ToString() + " ORDER BY P1.DATE DESC LIMIT 1");
            lastDate = 0;

            if (table.Rows.Count > 0)
            {
                lastDate = Convert.ToInt32(table.Rows[0][1]);
                return Convert.ToDecimal(table.Rows[0][0]);
            }

            return 0;
        }

        public static decimal GetPrice(int stockId, int date)
        {
            int volume = 0;

            String keystr = stockId.ToString() + "_" + date.ToString();
            if (CachedPrice.ContainsKey(keystr))
            {
                return CachedPrice[keystr];
            }

            decimal price = GetPrice(stockId, date, out volume);
            CachedPrice[keystr] = price;
            return price;
        }

        public static decimal GetPrice(int stockId, int date, out int volume)
        {
            DataTable table = DatabaseSingleton.Instance.GetData("select cast(ifnull(cast(P1.CLOSE_PRICE as double), 0)as double) PRICE_HISTORY, IFNULL(P1.VOLUME, 0) VOLUME FROM PRICE_HISTORY P1 WHERE P1.DATE >= " + date.ToString() + " AND P1.CLOSE_PRICE IS NOT NULL AND P1.ID_STOCK = " + stockId.ToString() + " ORDER BY P1.DATE ASC LIMIT 1");
            volume = 0;

            if (table.Rows.Count > 0)
            {
                volume = Convert.ToInt32(table.Rows[0][1]);
                return Convert.ToDecimal(table.Rows[0][0]);
            }
            else
            {
                table = DatabaseSingleton.Instance.GetData("select cast(ifnull(cast(P1.CLOSE_PRICE as double), 0)as double) PRICE_HISTORY, IFNULL(P1.VOLUME, 0) VOLUME FROM PRICE_HISTORY P1 WHERE P1.DATE <= " + date.ToString() + " AND P1.CLOSE_PRICE IS NOT NULL AND P1.ID_STOCK = " + stockId.ToString() + " ORDER BY P1.DATE DESC LIMIT 1");
                if (table.Rows.Count > 0)
                {
                    volume = Convert.ToInt32(table.Rows[0][1]);
                    return Convert.ToDecimal(table.Rows[0][0]);
                }
            }

            return 0;
        }


        public static List<StockValue> GetFirstDayPrices(int stockId, int startDate)
        {
            DataTable table = DatabaseSingleton.Instance.GetData("select P1.DATE, cast(ifnull(cast(P1.CLOSE_PRICE as double), 0)as double) PRICE FROM PRICE P1 WHERE P1.DATE > " + startDate.ToString() + " AND P1.CLOSE_PRICE IS NOT NULL AND P1.ID_STOCK = " + stockId.ToString() + " ORDER BY P1.DATE ASC");
            List<StockValue> result = new List<StockValue>();

            if (table.Rows.Count > 0)
            {
                StockValue val = new StockValue();
                val.Date = Convert.ToInt32(table.Rows[0][0]);
                val.Close = Convert.ToDecimal(table.Rows[0][1]);
                result.Add(val);
            }

            return result;
        }
        

        public static List<StockValue> GetAllPrices(int stockId)
        {
            return GetAllPrices(stockId, 19500101, 21000101);
        }

        public static List<StockValue> GetAllPrices(int stockId, int startDate, int endDate)
        {
            List<StockValue> values = new List<StockValue>();
            DataTable table = DatabaseSingleton.Instance.GetData("select cast(ifnull(cast(P1.CLOSE_PRICE as double), 0)as double) CLOSE, 0 CHANGE_PERCENT, IFNULL(P1.VOLUME, 0) VOLUME, DATE, 0 AVG_20DAYS_VOLUME FROM PRICE_HISTORY P1 WHERE P1.CLOSE_PRICE IS NOT NULL AND P1.ID_STOCK = " + stockId.ToString() + " AND P1.DATE BETWEEN " + startDate + " AND " + endDate + " ORDER BY P1.DATE ASC");

            foreach(DataRow row in table.Rows)
            {
                values.Add(new StockValue(row));
            }

            return values;
        }

        public static List<String> getAllSectors()
        {
            List<String> result = new List<string>();

            DataTable table = DatabaseSingleton.Instance.GetData("select NAME FROM SECTOR");

            foreach (DataRow row in table.Rows)
            {
                result.Add(row[0].ToString());
            }

            return result;
        }

        public static decimal GetPrice1StJan(int idStock)
        {
            DataTable table = DatabaseSingleton.Instance.GetData("select cast(P1.CLOSE_PRICE as double) PRICE_HISTORY FROM PRICE_HISTORY P1 WHERE P1.DATE >= " + DateTime.Now.Year + "0101 AND P1.CLOSE_PRICE IS NOT NULL AND P1.ID_STOCK = " + idStock.ToString() + " ORDER BY P1.DATE ASC LIMIT 1");

            if (table.Rows.Count > 0)
            {
                return Convert.ToDecimal(table.Rows[0][0]);
            }

            return 0;
        }


        public static bool IsSPYStock(int idStock)
        {
            DataTable table = DatabaseSingleton.Instance.GetData("select * FROM SPY_STOCKS WHERE ID_STOCK = " + idStock.ToString());

            if (table.Rows.Count > 0)
                return true;

            return false;
        }

        public static HashSet<int> GetSPYStocksIds()
        {
            DataTable table = DatabaseSingleton.Instance.GetData("select ID_STOCK FROM SPY_STOCKS");

            HashSet<int> result = new HashSet<int>();
            foreach(DataRow row in table.Rows)
            {
                result.Add(Convert.ToInt32(row[0]));
            }

            return result;
        }

        FeatureTable copyFeatureIfAll0(FeatureTable currentFeatures, FeatureTable oldFeatures)
        {
            //if last feature date is < last 3 months, do nothing
            if (oldFeatures.Date < Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-5)))
                return currentFeatures;

            if (currentFeatures == null)
            {
                return oldFeatures;
            }

            //check if current features is all 0
            if (!currentFeatures.IsAll0())
                return currentFeatures;

            currentFeatures.CopyAllExcepFIP(oldFeatures);

            return currentFeatures;
        }

        public static int GetNextNegativeId()
        {
            DataTable table = DatabaseSingleton.Instance.GetData("select MIN(ID) FROM STOCK WHERE ID < 0");
            int newId = 0;

            if (table.Rows.Count > 0)
                newId = Convert.ToInt32(table.Rows[0][0]);

            if (newId > -100)
                newId = -100;

            return newId-1;
        }

        public static List<Stock> GetAllETFStocks()
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT STOCK.*, (SELECT IFNULL(MAX(PRICE_HISTORY.DATE), 0) FROM PRICE_HISTORY WHERE PRICE_HISTORY.ID_STOCK = STOCK.ID) LAST_DATE, (SELECT IFNULL(MIN(PRICE.DATE), 0) FROM PRICE WHERE PRICE.ID_STOCK = STOCK.ID) FIRST_DATE FROM STOCK WHERE TYPE = 2 ORDER BY ID ASC");
            var result = new List<Stock>();

            foreach (DataRow row in data.Rows)
            {
                Stock etfStock = new Stock(row);
                etfStock.LastValuesDate = Convert.ToInt32(row["LAST_DATE"]);
                etfStock.FirstValuesDate = Convert.ToInt32(row["FIRST_DATE"]);
                result.Add(etfStock);
            }

            return result;
        }

        public static List<Stock> GetDefaultETFStocks()
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT STOCK.*, (SELECT IFNULL(MAX(PRICE_HISTORY.DATE), 0) FROM PRICE_HISTORY WHERE PRICE_HISTORY.ID_STOCK = STOCK.ID) LAST_DATE, (SELECT IFNULL(MIN(PRICE.DATE), 0) FROM PRICE WHERE PRICE.ID_STOCK = STOCK.ID) FIRST_DATE FROM STOCK WHERE TYPE = 1 AND ID BETWEEN -20 AND -1 ORDER BY ID ASC");
            var result = new List<Stock>();

            foreach (DataRow row in data.Rows)
            {
                Stock etfStock = new Stock(row);
                etfStock.LastValuesDate = Convert.ToInt32(row["LAST_DATE"]);
                etfStock.FirstValuesDate = Convert.ToInt32(row["FIRST_DATE"]);
                result.Add(etfStock);
            }

            return result;
        }
        public void DeleteAllPrices()
        {
            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM PRICE WHERE ID_STOCK = " + this.Id, null);
            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM PRICE_HISTORY WHERE ID_STOCK = " + this.Id, null);
            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM DIVIDEND_HISTORY WHERE ID_STOCK = " + this.Id, null);
        }

        public static void DeleteETFStock(Stock stock)
        {
            if (stock.Id > -100)
                return;

            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STOCK WHERE ID = " + stock.Id, null);

            stock.DeleteAllPrices();
        }
        
        protected override void loadPricesInternal()
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT DATE, CAST(CLOSE_PRICE as double) VALUE FROM PRICE_HISTORY WHERE ID_STOCK = " + this.IdStock + " ORDER BY DATE ASC");
            prices = new List<KeyValuePair<int, decimal>>();

            foreach (DataRow row in data.Rows)
            {
                var date = Convert.ToInt32(row["DATE"]);
                var value = Convert.ToDecimal(row["VALUE"]);

                prices.Add(new KeyValuePair<int, decimal>(date, value));
            }

            //fill dividends
            this.dividends = LoadDividends(this.Id);
        }

        public static void SaveDividend(int idStock, int date, decimal value)
        {
            if (value == 0)
                return;

            DataTable table = DatabaseSingleton.Instance.GetData("select * FROM DIVIDEND_HISTORY WHERE ID_STOCK = " + idStock + " AND DATE = " + date.ToString() + " LIMIT 1");
            if (table.Rows.Count > 0)
                return;

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();

            parameters.Add(new SQLiteParameter("@p1", idStock));
            parameters.Add(new SQLiteParameter("@p2", value));
            parameters.Add(new SQLiteParameter("@p3", date));

            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO DIVIDEND_HISTORY (ID_STOCK, DIVIDEND, DATE) VALUES (@p1, @p2, @p3)", parameters.ToArray());
        }

        public static List<KeyValuePair<int, decimal>> LoadDividends(int id)
        {
            return LoadDividends(id, DatabaseSingleton.Instance);
        }

        public static void LoadAllDividends(Dictionary<int, Stock> dict, DatabaseSingleton dbConn)
        {
            DataTable data = dbConn.GetData("SELECT ID_STOCK, DIVIDEND, DATE FROM DIVIDEND_HISTORY ORDER BY DATE ASC");
            var dividends = new List<KeyValuePair<int, decimal>>();

            foreach (DataRow row in data.Rows)
            {
                var date = Convert.ToInt32(row["DATE"]);
                var value = Convert.ToDecimal(row["DIVIDEND"]);
                var id = Convert.ToInt32(row["ID_STOCK"]);
                Stock stock = dict[id];
                stock.dividends.Add(new KeyValuePair<int, decimal>(date, value));
            }
        }

        public static List<KeyValuePair<int, decimal>> LoadDividends(int id, DatabaseSingleton dbConn)
        {
            DataTable data = dbConn.GetData("SELECT DIVIDEND, DATE FROM DIVIDEND_HISTORY WHERE ID_STOCK = " + id + " ORDER BY DATE ASC");
            var dividends = new List<KeyValuePair<int, decimal>>();

            foreach (DataRow row in data.Rows)
            {
                var date = Convert.ToInt32(row["DATE"]);
                var value = Convert.ToDecimal(row["DIVIDEND"]);

                dividends.Add(new KeyValuePair<int, decimal>(date, value));
            }

            return dividends;
        }

        public static int GetLastMonthlyPriceDate(int stockId)
        {
            //returns the last date from PRICE table, it will always return first day of month
            DataTable table = DatabaseSingleton.Instance.GetData("select MAX(DATE) FROM PRICE WHERE ID_STOCK = " + stockId + " AND DATE < " + Utils.ConvertDateTimeToInt(DateTime.Now) + " AND CLOSE_PRICE IS NOT NULL");
            if (table.Rows.Count > 0)
            {
                return Convert.ToInt32(table.Rows[0][0]);
            }

            return 20040101;
        }

        public static int GetLastPriceDate(int stockId)
        {
            int date = 0;

            GetLastPrice(stockId, out date);

            return date;
        }

    }
}
