using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using StockRanking.IExtrading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.Data;
using System.Data.SQLite;

namespace StockRanking
{
    public class ZachsSourceReader
    {
        private static String APIKey1 = "";
        private static String APIKey2 = "";

        public static String ErrorExtraData = "";
        public static long LastProcessedLine = 0;

        public static bool CancelProcess = false;
        static IProgress<int> progress = null;
        static bool updateFromFiles = false;
         
        public static async Task<object> GenerateStocksList(ProcessingPanelControl panel)
        {
            APIKey1 = Properties.Settings.Default.ApiKey1;
            APIKey2 = Properties.Settings.Default.ApiKey2;

            if (CancelProcess)
                return null;

            //read file with industries and sectors to replace
            var replaceIndustriesByStock = ReadReplaceIndustriesByStock();

            //read all stocks to avoid duplicating
            List<String> currentTickers = Stock.GetTickersList();

            TextFieldParser textParser = null;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.quandl.com/api/v3/datatables/SHARADAR/TICKERS.json");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            String urlQuery = "https://www.quandl.com/api/v3/datatables/SHARADAR/TICKERS.json?qopts.export=true&api_key=" + APIKey2 + "&table=SF1&qopts.columns=ticker,name,industry,famaindustry,currency,sector,exchange,category,relatedtickers";

            int retryAttemps = 0;
            String fileToDownload = "";
            int countRecords = 0;

            while (fileToDownload == "")
            {
                retryAttemps++;
                HttpResponseMessage response = client.GetAsync(urlQuery).Result;

                panel.PerformStep();

                DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
                settings.UseSimpleDictionaryFormat = true;

                if ((int)response.StatusCode == 429)
                {
                    throw new Exception("Cannot download tickers data, Too many request to Quandl API, please wait 1 hour and try again.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    System.Threading.Thread.Sleep(5000);
                    continue;
                }

                String responseStr = response.Content.ReadAsStringAsync().Result;
                JObject results = JObject.Parse(responseStr);

                fileToDownload = results["datatable_bulk_download"]["file"]["link"].ToString();
                String status = results["datatable_bulk_download"]["file"]["status"].ToString();

                if (fileToDownload == "" || status == "regenerating")
                {
                    if (retryAttemps > 10)
                        throw new Exception("Cannot download Tickers, timeout or bad Quandl API response. Response: " + responseStr);

                    if (status == "regenerating")
                    {
                        panel.SetTitle("Retrieving Tickers... Generating data file... Quandl is regenerating the file, please wait, this could take a couple of minutes");
                        panel.PerformStep();

                        System.Threading.Thread.Sleep(20000);
                    }
                    else
                    {
                        panel.SetTitle("Retrieving Tickers... Generating data file...");
                        panel.PerformStep();

                        System.Threading.Thread.Sleep(10000);
                    }
                }
            }

            panel.SetTitle("Retrieving Tickers... Downloading data file...");

            var progressHandler = new System.Net.Http.Handlers.ProgressMessageHandler();

            progress = new Progress<int>(percent =>
            {
                panel.PerformStep();
            });

            lastBytes = 0;
            progressHandler.HttpReceiveProgress += (sender, args) =>
            {
                progressCallback(args.BytesTransferred, args.TotalBytes, panel);
            };
            HttpClient httpClientDownload = HttpClientFactory.Create(progressHandler);

            HttpResponseMessage responseMessage = null;
            Stream stream = null;

            httpClientDownload.Timeout = TimeSpan.FromMinutes(10);

            responseMessage = await httpClientDownload.GetAsync(fileToDownload);
            stream = responseMessage.Content.ReadAsStreamAsync().Result;

            if (CancelProcess)
                return null;

            ZipArchive archive = new ZipArchive(stream);
            textParser = new TextFieldParser(archive.Entries[0].Open());

            textParser.SetDelimiters(new string[] { "," });
            textParser.HasFieldsEnclosedInQuotes = false;

            panel.SetTitle("Retrieving Tickers... parsing file");

            bool firstRow = true;
            Stock.TickerCommandSaved = false;

            DatabaseSingleton.Instance.ExecuteNonQuery("DROP INDEX 'STOCK_ID_X'", null);
            DatabaseSingleton.Instance.StartTransaction();

            int recordsAccum = 0;
            int stocksCount = 0;

            while (!textParser.EndOfData)
            {
                if (CancelProcess)
                    return null;

                if (firstRow)
                {
                    String firstLine = textParser.ReadLine();
                    Console.WriteLine(firstLine);
                    if (!firstLine.StartsWith("ticker,name,industry,famaindustry,currency,sector,exchange,category,relatedtickers"))
                    {
                        DatabaseSingleton.Instance.RollbackTransaction();
                        throw new Exception("Ticker list format is incorrect, please try again.");
                    }
                    firstRow = false;
                    continue;
                }

                String[] data = textParser.ReadFields();

                recordsAccum++;
#if TESTDEV || DEBUG
                if (data[0].CompareTo("EEE")>0) continue;
#endif
                Stock stock = new Stock();
                
                if (recordsAccum > 300)
                {
                    recordsAccum = 0;
                    DatabaseSingleton.Instance.EndTransaction();

                    panel.SetTitle("Retrieving Tickers. Symbols " + stocksCount.ToString());
                    Application.DoEvents();

                    DatabaseSingleton.Instance.StartTransaction();

                    panel.PerformStep();
                }

                if (!currentTickers.Contains(data[0].ToString()))
                {
                    stock.Symbol = data[0];
                    stock.CompanyName = data[1];
                    stock.Industry2 = data[3];
                    stock.Currency = data[4];
                    stock.Sector = data[5];
                    stock.Industry = data[2];

                    stock.Exchange = data[6];
                    stock.IsForeign = data[7] != "Domestic";
                    if (data[8] != "none" && data[8] != "None")
                        stock.RelatedTickers = data[8].Trim();

                    if (replaceIndustriesByStock.ContainsKey(stock.Symbol))
                    {
                        stock.Industry2 = replaceIndustriesByStock[stock.Symbol];
                    }
                    /*else
                    {
                        foreach (String related in stock.RelatedTickers.Split(','))
                            if (replaceIndustriesByStock.ContainsKey(related))
                                stock.Industry2 = replaceIndustriesByStock[related];
                    }*/


                    //save stock in the database
                    if (stock.Currency == "USD")
                        stock.Save();
                }

                stocksCount++;
            }

            DatabaseSingleton.Instance.EndTransaction();

            DatabaseSingleton.Instance.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'STOCK_ID_X' ON 'STOCK'('ID')", null);

            return new object();
        }

        private static Dictionary<String, string> ReadReplaceIndustriesByStock()
        {
            var replaces = new Dictionary<string, string>();
            var industrySector = new Dictionary<string, string>();

            //read input file
            ZipArchive archive = new ZipArchive(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("StockRanking.StocksData.replacements.zip"));
            TextFieldParser textParser = new TextFieldParser(archive.Entries[0].Open());
            textParser.SetDelimiters(new string[] { "," });
            textParser.HasFieldsEnclosedInQuotes = true;

            //read all symbols and their industry
            //also read all industry/sector relations
            String firstLine = textParser.ReadLine();
            if (!firstLine.StartsWith("symbol,company,GICS,sector,industrygroup"))
                throw new Exception("Error reading Industries File, incorrect titles row");

            while (!textParser.EndOfData)
            {
                String[] data = textParser.ReadFields();

                replaces.Add(data[0].Trim(), data[4].Trim());

                if (!industrySector.ContainsKey(data[4].Trim()))
                    industrySector.Add(data[4].Trim(), data[3].Trim());
            }

            //add industry/sector relation in the database if it does not exists
            foreach(String key in industrySector.Keys)
            {
                if(DatabaseSingleton.Instance.GetData("SELECT COUNT(*) FROM SECTOR_INDUSTRY WHERE INDUSTRY = '" + key + "'").Rows[0][0].ToString() == "0")
                {
                    //add record
                    List<SQLiteParameter> parameters = new List<SQLiteParameter>();
                    parameters.Add(new SQLiteParameter("@industry", key));
                    parameters.Add(new SQLiteParameter("@sector", industrySector[key]));

                    DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO SECTOR_INDUSTRY " +
                        "SELECT ID, @industry FROM SECTOR WHERE NAME = @sector", parameters.ToArray());
                }
            }

            return replaces;
        }

        public static async Task<object> GetSharadarRatiosFromFile(List<Stock> stocks, ProcessingPanelControl panel, DateTime startDate)
        {
            if (CancelProcess)
                return null;

            ErrorExtraData = "";

            //update in the database the ratios for all the stocks

            panel.SetMaxValue(5);
            panel.SetTitle("Retrieving Sharadar fundamentals... Accessing quandl service...");

            TextFieldParser textParser = null;

            //it will check for an available file for debug and testing pourposes, to avoid having to always download the data (quandl max limit of 10 downloads per hour)
            if (!updateFromFiles || !File.Exists("D:\\Proyectos\\Upwork\\StockRanking\\SHARADAR_SF1_RED.csv") || startDate != DateTime.MinValue)
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://www.quandl.com/api/v3/datatables/SHARADAR/SF1.json");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                String startDateStr = "2001-01-01";
                
                if (startDate != DateTime.MinValue)
                    startDateStr = startDate.AddMonths(-6).ToString("yyyy-MM-dd");

                String urlQuery = "https://www.quandl.com/api/v3/datatables/SHARADAR/SF1.json?api_key=" + APIKey1 + "&dimension=ARY,ARQ,ART&calendardate.gte=" + startDateStr + "&qopts.export=true&qopts.columns=ticker,dimension,calendardate,EBITDA,EBITDAUSD,EBITDAMARGIN,SHARESBAS,DEBT,DEBTUSD,FCF,ROIC,CURRENTRATIO,divyield,EV,BVPS,MARKETCAP,INVCAP,ReportPeriod,revenueusd,eps,pe1,ps1,pb,price,sps,fcfps,datekey," +
                    "cor,netinc,epsdil,shareswa,capex,assets,cashneq,liabilities,assetsc,liabilitiesc,tangibles,roe,roa,gp,grossmargin,netmargin,ros,assetturnover,payoutratio,workingcapital,tbvps";

                int retryAttemps = 0;
                String fileToDownload = "";
                int countRecords = 0;

                panel.SetMaxValue(5);
                panel.SetTitle("Retrieving Sharadar fundamentals... Generating data file...");
                panel.PerformStep();


                while (fileToDownload == "")
                {
                    retryAttemps++;
                    HttpResponseMessage response = client.GetAsync(urlQuery).Result;

                    panel.PerformStep();

                    DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
                    settings.UseSimpleDictionaryFormat = true;

                    if ((int)response.StatusCode == 429)
                    {
                        throw new Exception("Cannot download fundamentals data, Too many request to Quandl API, please wait 1 hour and try again.");
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        System.Threading.Thread.Sleep(5000);
                        continue;
                    }

                    String responseStr = response.Content.ReadAsStringAsync().Result;
                    JObject results = JObject.Parse(responseStr);

                    fileToDownload = results["datatable_bulk_download"]["file"]["link"].ToString();
                    String status = results["datatable_bulk_download"]["file"]["status"].ToString();
                    //Console.WriteLine("FileToDownload: " + fileToDownload);
                    //Console.WriteLine("Status : " + status);

                    if (fileToDownload == "" || status == "regenerating" || status == "creating")
                    {

                        if (retryAttemps > 50)
                            throw new Exception("Cannot download fundamentals data, timeout or bad Quandl API response. Response:" + responseStr);

                        if (status == "regenerating")
                        {
                            panel.SetTitle("Retrieving Sharadar fundamentals... Generating data file... Quandl is regenerating the file, please wait, this could take a couple of minutes");
                            panel.PerformStep();

                            System.Threading.Thread.Sleep(20000);
                        }
                        else
                        {
                            panel.SetTitle("Retrieving Sharadar fundamentals... Generating data file...");
                            panel.PerformStep();

                            System.Threading.Thread.Sleep(10000);
                        }
                    }
                }

                panel.SetMaxValue(40);
                if (startDate != DateTime.MinValue)
                    panel.SetMaxValue(8);

                panel.SetTitle("Retrieving Sharadar fundamentals... Downloading data file...");

                var progressHandler = new System.Net.Http.Handlers.ProgressMessageHandler();

                progress = new Progress<int>(percent =>
                {
                    panel.PerformStep();
                });

                lastBytes = 0;
                progressHandler.HttpReceiveProgress += (sender, args) =>
                {
                    progressCallback(args.BytesTransferred, args.TotalBytes, panel);
                };

                HttpClient httpClientDownload = HttpClientFactory.Create(progressHandler);
                httpClientDownload.Timeout = TimeSpan.FromMinutes(30);

                HttpResponseMessage responseMessage = await httpClientDownload.GetAsync(fileToDownload);
                Stream stream = responseMessage.Content.ReadAsStreamAsync().Result;

                if (CancelProcess)
                    return null;

                ZipArchive archive = new ZipArchive(stream);
                textParser = new TextFieldParser(archive.Entries[0].Open());
            }
            else
            {
                textParser = new TextFieldParser("D:\\Proyectos\\Upwork\\StockRanking\\SHARADAR_SF1_RED.csv");
            }

            textParser.SetDelimiters(new string[] { "," });
            textParser.HasFieldsEnclosedInQuotes = false;
            
            Stock selectedStock = new Stock();
            StockRatios.CommandSaved = false;

            //get last ratio date for all stocks
            Dictionary<int, int> ratiosLastDates = getRatiosLastDates(stocks);
            Dictionary<int, int> ratiosLastDatesYearly = getRatiosLastDatesYearly(stocks);

            DatabaseSingleton.Instance.ExecuteNonQuery("DROP INDEX RATIO_DATE_X", null);
            DatabaseSingleton.Instance.ExecuteNonQuery("DROP INDEX RATIO_DATE_Y", null);
            
            DatabaseSingleton.Instance.StartTransaction();
            int recordsAccum = 0;
            int stocksCount = 0;

            String firstRowStr = textParser.ReadLine();
            //Console.WriteLine("QUANDL" + firstRowStr);
            if (!firstRowStr.Contains("ticker,dimension,calendardate,ebitda,ebitdausd,ebitdamargin,sharesbas,debt,debtusd,fcf,roic,currentratio,divyield,ev,bvps,marketcap,invcap,reportperiod,revenueusd,eps,pe1,ps1,pb,price,sps,fcfps"))
            {
                throw new Exception("Incorrect input data format, please check column headers order.");
            }

            panel.SetMaxValue(100);
            int lastId = -1;
            int lastDate = 0;
            String lastDimension = "";
            var updatedStocks = new HashSet<Stock>();

            while (!textParser.EndOfData)
            {
                if (CancelProcess)
                    return null;

                String[] data = textParser.ReadFields();

                if (selectedStock == null || selectedStock.Symbol != data[0])
                {
                    ErrorExtraData = String.Join("\t", data);
                    selectedStock = stocks.Find(x => x.Symbol == data[0]);

                    if (selectedStock != null)
                    {
                        if (!updatedStocks.Contains(selectedStock))
                        {
                            updatedStocks.Add(selectedStock);
                        }

                        stocksCount++;

                        if ((stocksCount & 511) == 0)
                        {
                            DatabaseSingleton.Instance.EndTransaction();
                            //stocksCount = 0;
                            panel.SetTitle("Retrieving Fundamentals. Symbol " + data[0]);
                            panel.PerformStep();
                            DatabaseSingleton.Instance.StartTransaction();
                        }
                    }
                }

                if (selectedStock == null)
                    continue;

                StockRatios newRatio = new StockRatios();
                newRatio.IdStock = selectedStock.Id;
                newRatio.SnapDate = Convert.ToInt32(data[2].Replace("-", ""));

                if (data[1] != "")
                    newRatio.DataRange = data[1];
                if (data[3] != "")
                    newRatio.EBITDA = Convert.ToDecimal(data[3], CultureInfo.InvariantCulture);
                if (data[4] != "")
                    newRatio.ebitdausd = Convert.ToDecimal(data[4], CultureInfo.InvariantCulture);
                if (data[5] != "")
                    newRatio.ebitdamargin = Convert.ToDecimal(data[5], CultureInfo.InvariantCulture);
                if (data[6] != "")
                    newRatio.SharesOut = Convert.ToInt64(data[6], CultureInfo.InvariantCulture);
                if (data[7] != "")
                    newRatio.LtermTotalDebt = Convert.ToDouble(data[7], CultureInfo.InvariantCulture);
                if (data[9] != "")
                    newRatio.freeCashFlow = Convert.ToDecimal(data[9], CultureInfo.InvariantCulture);
                if (data[10] != "")
                    newRatio.returnInvestedCap = Convert.ToDecimal(data[10], CultureInfo.InvariantCulture);
                if (data[11] != "")
                    newRatio.CurrRatio = Convert.ToDecimal(data[11], CultureInfo.InvariantCulture);
                if (data[12] != "")
                    newRatio.DividendYield = Convert.ToDecimal(data[12], CultureInfo.InvariantCulture);
                if (data[13] != "")
                    newRatio.EV = Convert.ToDecimal(data[13], CultureInfo.InvariantCulture);
                if (data[14] != "")
                    newRatio.BVPS = Convert.ToDecimal(data[14], CultureInfo.InvariantCulture);
                if (data[15] != "")
                    newRatio.MarketCap = Convert.ToDecimal(data[15], CultureInfo.InvariantCulture);
                if (data[16] != "")
                    newRatio.InvestedCapital = Convert.ToDecimal(data[16], CultureInfo.InvariantCulture);
                if (data[17] != "")
                    newRatio.Date = Convert.ToInt32(data[17].Replace("-", ""));
                if (data[18] != "")
                    newRatio.SalesUSD = Convert.ToDecimal(data[18], CultureInfo.InvariantCulture);
                if (data[19] != "")
                    newRatio.EPS = Convert.ToDecimal(data[19], CultureInfo.InvariantCulture);
                if (data[20] != "")
                    newRatio.PE = Convert.ToDecimal(data[20], CultureInfo.InvariantCulture);
                if (data[21] != "")
                    newRatio.PS = Convert.ToDecimal(data[21], CultureInfo.InvariantCulture);
                if (data[22] != "")
                    newRatio.PB = Convert.ToDecimal(data[22], CultureInfo.InvariantCulture);
                if (data[23] != "")
                    newRatio.StockPrice = Convert.ToDecimal(data[23], CultureInfo.InvariantCulture);
                if (data[24] != "")
                    newRatio.SPS = Convert.ToDecimal(data[24], CultureInfo.InvariantCulture);
                if (data[25] != "")
                    newRatio.FCFPS = Convert.ToDecimal(data[25], CultureInfo.InvariantCulture);

                newRatio.DateKey = Convert.ToInt32(data[26].Replace("-", ""));

#if TRYCATCH
                try
                {
#endif
                    if (data[27] != "")
                        newRatio.cor = Convert.ToDecimal(data[27], CultureInfo.InvariantCulture);
                    if (data[28] != "")
                        newRatio.netinc = Convert.ToDecimal(data[28], CultureInfo.InvariantCulture);
                    if (data[29] != "")
                        newRatio.epsdil = Convert.ToDecimal(data[29], CultureInfo.InvariantCulture);
                    if (data[30] != "")
                        newRatio.shareswa = Convert.ToDecimal(data[30], CultureInfo.InvariantCulture);
                    if (data[31] != "")
                        newRatio.capex = Convert.ToDecimal(data[31], CultureInfo.InvariantCulture);
                    if (data[32] != "")
                        newRatio.assets = Convert.ToDecimal(data[32], CultureInfo.InvariantCulture);
                    if (data[33] != "")
                        newRatio.cashneq = Convert.ToDecimal(data[33], CultureInfo.InvariantCulture);
                    if (data[34] != "")
                        newRatio.liabilities = Convert.ToDecimal(data[34], CultureInfo.InvariantCulture);
                    if (data[35] != "")
                        newRatio.assetsc = Convert.ToDecimal(data[35], CultureInfo.InvariantCulture);
                    if (data[36] != "")
                        newRatio.liabilitiesc = Convert.ToDecimal(data[36], CultureInfo.InvariantCulture);
                    if (data[37] != "")
                        newRatio.tangibles = Convert.ToDecimal(data[37], CultureInfo.InvariantCulture);
                    if (data[38] != "")
                        newRatio.roe = Convert.ToDecimal(data[38], CultureInfo.InvariantCulture);
                    if (data[39] != "")
                        newRatio.roa = Convert.ToDecimal(data[39], CultureInfo.InvariantCulture);
                    if (data[40] != "")
                        newRatio.gp = Convert.ToDecimal(data[40], CultureInfo.InvariantCulture);
                    if (data[41] != "")
                        newRatio.grossmargin = Convert.ToDecimal(data[41], CultureInfo.InvariantCulture);
                    if (data[42] != "")
                        newRatio.netmargin = Convert.ToDecimal(data[42], CultureInfo.InvariantCulture);
                    if (data[43] != "")
                        newRatio.ros = Convert.ToDecimal(data[43], CultureInfo.InvariantCulture);
                    if (data[44] != "")
                        newRatio.assetturnover = Convert.ToDecimal(data[44], CultureInfo.InvariantCulture);
                    if (data[45] != "")
                        newRatio.payoutratio = Convert.ToDecimal(data[45], CultureInfo.InvariantCulture);
                    if (data[46] != "")
                        newRatio.workingcapital = Convert.ToDecimal(data[46], CultureInfo.InvariantCulture);
                    if (data[47] != "")
                        newRatio.tbvps = Convert.ToDecimal(data[47], CultureInfo.InvariantCulture);
#if TRYCATCH
            } catch (Exception e5)
                {
                    //for (int i = 27; i <= 47; i++) Console.Write(data[i] + ", ");
                    //Console.WriteLine("\n");
                }
#endif

                newRatio.Date = Utils.ConvertDateTimeToInt((new DateTime(Utils.ConvertIntToDateTime(newRatio.Date).Year, Utils.ConvertIntToDateTime(newRatio.Date).Month, 01)).AddMonths(1).AddDays(-1));

                newRatio.Year = Convert.ToInt32(newRatio.SnapDate.ToString().Substring(0, 4));

                newRatio.DateFirstMonth = Utils.ConvertDateTimeToInt(Utils.ConvertIntToDateTime(newRatio.Date).AddDays(1));

                if (newRatio.SnapDate.ToString().Substring(4, 4) == "0331")
                    newRatio.Quarter = 1;
                if (newRatio.SnapDate.ToString().Substring(4, 4) == "0630")
                    newRatio.Quarter = 2;
                if (newRatio.SnapDate.ToString().Substring(4, 4) == "0930")
                    newRatio.Quarter = 3;
                if (newRatio.SnapDate.ToString().Substring(4, 4) == "1231")
                    newRatio.Quarter = 4;

                if (lastId == newRatio.IdStock && lastDate == newRatio.Date && lastDimension == newRatio.DataRange)
                {
                    continue;
                }

                lastId = newRatio.IdStock;
                lastDate = newRatio.Date;
                lastDimension = newRatio.DataRange;

                //save ratio in the database
                if (newRatio.DataRange == "ARY")
                {
                    //when partially updating only save if date is > than last value
                    if (ratiosLastDatesYearly.ContainsKey(selectedStock.Id))
                    {
                        if (ratiosLastDates[selectedStock.Id] >= newRatio.Date)
                            continue;
                    }

                    newRatio.Save(selectedStock.Id, newRatio.Date, newRatio.Quarter, newRatio.Year, newRatio.DateFirstMonth, newRatio.returnInvestedCap);
                }
                else
                {
                    if (newRatio.DataRange == "ART")
                        selectedStock.TempRatios.Add(newRatio);
                    else
                        selectedStock.Ratios.Add(newRatio);
                }
            }

            stocksCount = 0;
            foreach (var stock in updatedStocks)
            {
                stocksCount++;

                if ((stocksCount & 511) == 0)
                {
                    DatabaseSingleton.Instance.EndTransaction();
                    //stocksCount = 0;
                    panel.SetTitle("Storing Fundamentals. Symbol " + stock.Symbol);
                    panel.PerformStep();
                    DatabaseSingleton.Instance.StartTransaction();
                }

                int lastDateSaved = 20031201;
                if (ratiosLastDates[stock.Id] > 0)
                    lastDateSaved = Utils.AddDays(ratiosLastDates[stock.Id], 1);
                
                if (stock.Ratios.Count == 0)
                    continue;

                StockRatios foundRatio = stock.Ratios[0];
                StockRatios tempRoicRatio = null;
                int today = Utils.ConvertDateTimeToInt(DateTime.Now);
                int lastRatio = Utils.AddMonths(stock.Ratios[stock.Ratios.Count-1].Date, 3);
                while (lastDateSaved < today &&
                    lastDateSaved < lastRatio)
                {
                    int nextDateToSave = Utils.AddMonths(lastDateSaved, 1);

#if TRYCATCH
                    try
                    {
#endif
                        if (nextDateToSave >= foundRatio.DateKey)
                        {
                            foundRatio = stock.Ratios.FindAll(x => x.DateKey >= foundRatio.DateKey && x.DateKey <= nextDateToSave && x.SnapDate <= nextDateToSave).OrderBy(x => x.DateKey).Last();
                        }
#if TRYCATCH
                    }
                    catch (Exception e)
                    {
                        
                    }
#endif
                    tempRoicRatio = stock.TempRatios.FirstOrDefault(x => x.DateKey == foundRatio.DateKey && x.SnapDate == foundRatio.SnapDate);

                    decimal roic = 0;
                    if (tempRoicRatio == null)
                        roic = 0;
                    else
                        roic = tempRoicRatio.returnInvestedCap;

                    DateTime lastDateDT = Utils.ConvertIntToDateTime(nextDateToSave);
                    foundRatio.Save(stock.Id, Utils.AddDays(nextDateToSave, -1), lastDateDT.Month, lastDateDT.Year, nextDateToSave, roic);
                    lastDateSaved = nextDateToSave;

                    /*stocksCount++;
                    if ((stocksCount & 127) == 0)
                    {
                        DatabaseSingleton.Instance.EndTransaction();
                        //stocksCount = 0;
                        panel.SetTitle("Storing Fundamentals. Symbol " + stock.Symbol);
                        Application.DoEvents();
                        if ((stocksCount & 1023) == 0) System.Threading.Thread.Sleep(100);
                        DatabaseSingleton.Instance.StartTransaction();
                    }*/
                }
            }
            
            DatabaseSingleton.Instance.EndTransaction();

            DatabaseSingleton.Instance.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'RATIO_DATE_X' ON 'RATIO'(ID_STOCK, DATE)", null);
            DatabaseSingleton.Instance.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS 'RATIO_DATE_Y' ON 'RATIO'(ID_STOCK, YEAR)", null);
            
            return null;
        }

        static long lastBytes = 0;
        public static void progressCallback(long bytesTransferred, long? totalBytes, ProcessingPanelControl panel)
        {
            if(bytesTransferred - lastBytes > 2*1024*1024)
            {
                lastBytes = bytesTransferred;
                progress.Report(1);
            }
        }

        public static async Task<object> GetSharadarPricesFromFile(List<Stock> stocks, ProcessingPanelControl panel, DateTime startDate)
        {
            if (CancelProcess)
                return null;
            
            panel.SetMaxValue(5);
            panel.SetTitle("Retrieving Prices... Accessing quandl service...");
            TextReader textParser = null;
            JObject resultsDividend = null;
            Dictionary<string, List<JToken>> dicDividend = null;
            if (!updateFromFiles || !File.Exists("D:\\Proyectos\\Upwork\\StockRanking\\SHARADAR_SEP.csv") || startDate != DateTime.MinValue)
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://www.quandl.com/api/v3/datatables/SHARADAR/SEP.json");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                String startDateStr = "2004-01-01";
                if (startDate != DateTime.MinValue)
                    startDateStr = startDate.AddMonths(-18).ToString("yyyy-MM-dd");
                String urlQuery = "https://www.quandl.com/api/v3/datatables/SHARADAR/SEP.json?qopts.export=true&api_key=" + APIKey1 + "&qopts.columns=ticker,date,close,volume,high,low&date.gte=" + startDateStr;
                String urlQueryDividend = "https://www.quandl.com/api/v3/datatables/SHARADAR/ACTIONS?api_key=" + APIKey1 + "&action=dividend&date.gte=" + startDateStr;
                int retryAttemps = 0;
                String fileToDownload = "";
                int countRecords = 0;

                panel.SetMaxValue(5);
                panel.SetTitle("Retrieving Prices... Generating data file...");
                panel.PerformStep();

                while (fileToDownload == "")
                {
                    retryAttemps++;
                    HttpResponseMessage response = client.GetAsync(urlQuery).Result;
                    HttpResponseMessage responseDividend = client.GetAsync(urlQueryDividend).Result;

                    panel.PerformStep();

                    DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
                    settings.UseSimpleDictionaryFormat = true;

                    if ((int)response.StatusCode == 429)
                    {
                        throw new Exception("Cannot download prices data, Too many request to Quandl API, please wait 1 hour and try again.");
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        System.Threading.Thread.Sleep(5000);
                        continue;
                    }

                    String responseStr = response.Content.ReadAsStringAsync().Result;
                    String responseStrDividend = responseDividend.Content.ReadAsStringAsync().Result;
                    JObject results = JObject.Parse(responseStr);
                    resultsDividend = JObject.Parse(responseStrDividend);

                    fileToDownload = results["datatable_bulk_download"]["file"]["link"].ToString();
                    String status = results["datatable_bulk_download"]["file"]["status"].ToString();

                    if (fileToDownload == "" || status == "regenerating")
                    {

                        if (retryAttemps > 100)
                            throw new Exception("Cannot download prices data, timeout or bad Quandl API response. Response: " + responseStr);

                        if (status == "regenerating")
                        {
                            panel.SetTitle("Retrieving Prices... Generating data file... Quandl is regenerating the file, please wait, this could take a couple of minutes");
                            panel.PerformStep();

                            System.Threading.Thread.Sleep(20000);
                        }
                        else
                        {
                            panel.SetTitle("Retrieving Prices... Generating data file...");
                            panel.PerformStep();

                            System.Threading.Thread.Sleep(20000);
                        }
                    }
                }


                panel.SetMaxValue(400);
                if (startDate != DateTime.MinValue)
                    panel.SetMaxValue(80);

                panel.SetTitle("Retrieving Prices... Downloading data file...");

                var progressHandler = new System.Net.Http.Handlers.ProgressMessageHandler();

                progress = new Progress<int>(percent =>
                {
                    panel.PerformStep();
                });

                lastBytes = 0;
                progressHandler.HttpReceiveProgress += (sender, args) =>
                {
                    progressCallback(args.BytesTransferred, args.TotalBytes, panel);
                };
                HttpClient httpClientDownload = HttpClientFactory.Create(progressHandler);

                HttpResponseMessage responseMessage = null;
                Stream stream = null;
                 
                httpClientDownload.Timeout = TimeSpan.FromMinutes(30);

                responseMessage = await httpClientDownload.GetAsync(fileToDownload);
                stream = responseMessage.Content.ReadAsStreamAsync().Result;
                
                if (CancelProcess)
                    return null;

                ZipArchive archive = new ZipArchive(stream);
                textParser = new StreamReader(archive.Entries[0].Open());
            }
            else
            {
                textParser = new StreamReader("D:\\Proyectos\\Upwork\\StockRanking\\SHARADAR_SEP.csv");
            }

            panel.SetMaxValue(25000000 / 10000);
            if (startDate != DateTime.MinValue)
                panel.SetMaxValue(25000000 / 10000 / 20);
            panel.SetTitle("Retrieving Sharadar Prices... parsing file");
             
            Dictionary<String, Stock> stocksDictionary = new Dictionary<string, Stock>(stocks.Count);
            foreach (Stock st in stocks)
                stocksDictionary.Add(st.Symbol, st);

            var spyStockList = Stock.GetSPYStocksIds();

            int recordsAccum = 0;
            String firstRowStr = textParser.ReadLine();

            if (!firstRowStr.Contains("ticker,date,close,volume,high,low"))
            {
                throw new Exception("Incorrect input data format, please check column headers order.");
            }

            if (resultsDividend != null)
            {
                dicDividend = new Dictionary<string, List<JToken>>();
                JToken[] dataDividend = resultsDividend["datatable"]["data"].ToArray();
                int index;
                for(index = 0; index < dataDividend.Length; index++)
                {
                    string symbol = dataDividend[index].ElementAt(2).ToString();
                    if (!dicDividend.ContainsKey(symbol))
                    { 
                        dicDividend.Add(symbol, new List<JToken>() { dataDividend[index] });
                        continue;
                    }
                    dicDividend[symbol].Add(dataDividend[index]);
                }
            }

            LastProcessedLine = 0;
            String[] data;
            String lineData;
            while ((lineData = textParser.ReadLine()) != null)
            {
                if (CancelProcess)
                    return null;

                LastProcessedLine++;
                
                recordsAccum++;

                data = lineData.Split(',');

                if (!stocksDictionary.ContainsKey(data[0]))
                    continue;

                var stock = stocksDictionary[data[0]];

                StockValue newValue;
                if (spyStockList.Contains(stock.Id))
                {
                    var newValHiLo = new HiLoStockValue();
                    newValue = newValHiLo;

                    if (data[4] != "" && !data[4].Contains("e"))
                        newValHiLo.High = Convert.ToDecimal(data[4], CultureInfo.InvariantCulture);

                    if (data[5] != "" && !data[5].Contains("e"))
                        newValHiLo.Low = Convert.ToDecimal(data[5], CultureInfo.InvariantCulture);
                }
                else
                    newValue = new StockValue();

                newValue.Date = Convert.ToInt32(data[1].Replace("-", ""));

                if (data[2] != "" && !data[2].Contains("e"))
                    newValue.Close = Convert.ToDecimal(data[2], CultureInfo.InvariantCulture);

                if (data[3] != "" && !data[3].Contains("e"))
                    newValue.Volume = Convert.ToInt64(Convert.ToDecimal(data[3], CultureInfo.InvariantCulture));

                stocksDictionary[data[0]].stockValues.Add(newValue);
                
                if (resultsDividend != null)
                {
                    if (dicDividend.ContainsKey(data[0]))
                    { 
                        List<JToken> lstDividendToken = dicDividend[data[0]];
                        if (lstDividendToken.Count != 0)
                        {
                            stocksDictionary[data[0]].stockDividends.Add(new KeyValuePair<int, decimal>(newValue.Date, Convert.ToDecimal(lstDividendToken.Last().ElementAt(4), CultureInfo.InvariantCulture)));
/*                            int index;
                            for(index = 0; index < lstDividendToken.Count; index ++)
                            {
                                if (lstDividendToken[index].First.ToString() == data[1])
                                {
                                    
                                    break;
                                }
                            }*/
                        }
                    }
                }
                /*if (data[4] != "" && data[4] != "0" && !data[4].Contains("e"))
                    stocksDictionary[data[0]].stockDividends.Add(new KeyValuePair<int, decimal>(newValue.Date, Convert.ToDecimal(data[4], CultureInfo.InvariantCulture)));
                */
                if (recordsAccum > 10000)
                { 
                    recordsAccum = 0;
                    panel.PerformStep();
                }

            }

            return null;

        }

        public static async Task<object> GenerateETFsList(ProcessingPanelControl panel)
        {
            if (CancelProcess)
                return null;

            panel.SetMaxValue(5);
            panel.SetTitle("Retrieving ETF Tickers... Accessing quandl service...");
            TextFieldParser textParser = null;

            if (!updateFromFiles || !File.Exists("D:\\Proyectos\\Upwork\\StockRanking\\SHARADAR_TICKERS_ETF.csv"))
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://www.quandl.com/api/v3/datatables/SHARADAR/TICKERS.json");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                String urlQuery = "https://www.quandl.com/api/v3/datatables/SHARADAR/TICKERS.json?qopts.export=true&api_key=" + APIKey2 + "&table=SFP&qopts.columns=ticker,name,exchange,category,firstpricedate,lastpricedate";
                                   
                int retryAttemps = 0;
                String fileToDownload = "";
                int countRecords = 0;

                panel.SetMaxValue(5);
                panel.SetTitle("Retrieving ETF Tickers... Generating data file...");
                panel.PerformStep();

                while (fileToDownload == "")
                {
                    retryAttemps++;
                    HttpResponseMessage response = client.GetAsync(urlQuery).Result;

                    panel.PerformStep();

                    DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
                    settings.UseSimpleDictionaryFormat = true;

                    if ((int)response.StatusCode == 429)
                    {
                        throw new Exception("Cannot download ETF tickers data, Too many request to Quandl API, please wait 1 hour and try again.");
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        System.Threading.Thread.Sleep(5000);
                        continue;
                    }

                    String responseStr = response.Content.ReadAsStringAsync().Result;
                    JObject results = JObject.Parse(responseStr);

                    fileToDownload = results["datatable_bulk_download"]["file"]["link"].ToString();
                    String status = results["datatable_bulk_download"]["file"]["status"].ToString();

                    if (fileToDownload == "" || status == "regenerating")
                    {
                        if (retryAttemps > 10)
                            throw new Exception("Cannot download ETF Tickers, timeout or bad Quandl API response. Response: " + responseStr);

                        if (status == "regenerating")
                        {
                            panel.SetTitle("Retrieving ETF Tickers... Generating data file... Quandl is regenerating the file, please wait, this could take a couple of minutes");
                            panel.PerformStep();

                            System.Threading.Thread.Sleep(20000);
                        }
                        else
                        {
                            panel.SetTitle("Retrieving ETF Tickers... Generating data file...");
                            panel.PerformStep();

                            System.Threading.Thread.Sleep(10000);
                        }
                    }
                }

                panel.SetMaxValue(3);

                panel.SetTitle("Retrieving ETF Tickers... Downloading data file...");

                var progressHandler = new System.Net.Http.Handlers.ProgressMessageHandler();

                progress = new Progress<int>(percent =>
                {
                    panel.PerformStep();
                });

                lastBytes = 0;
                progressHandler.HttpReceiveProgress += (sender, args) =>
                {
                    progressCallback(args.BytesTransferred, args.TotalBytes, panel);
                };
                HttpClient httpClientDownload = HttpClientFactory.Create(progressHandler);

                HttpResponseMessage responseMessage = null;
                Stream stream = null;

                httpClientDownload.Timeout = TimeSpan.FromMinutes(10);

                responseMessage = await httpClientDownload.GetAsync(fileToDownload);
                stream = responseMessage.Content.ReadAsStreamAsync().Result;

                if (CancelProcess)
                    return null;

                ZipArchive archive = new ZipArchive(stream);
                textParser = new TextFieldParser(archive.Entries[0].Open());
            }
            else
            {
                textParser = new TextFieldParser("D:\\Proyectos\\Upwork\\StockRanking\\SHARADAR_TICKERS_ETF.csv");
            }

            textParser.SetDelimiters(new string[] { "," });
            textParser.HasFieldsEnclosedInQuotes = false;

            panel.SetTitle("Retrieving ETF Tickers... parsing file");

            bool firstRow = true;
            Stock.TickerCommandSaved = false;

            DatabaseSingleton.Instance.StartTransaction();

            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM ETF_TMP_LIST", null);

            int recordsAccum = 0;
            int stocksCount = 0;

            while (!textParser.EndOfData)
            {
                if (CancelProcess)
                    return null;

                if (firstRow)
                {
                    String firstLine = textParser.ReadLine();
                    if (!firstLine.StartsWith("ticker,name,exchange,category,firstpricedate,lastpricedate"))
                    {
                        DatabaseSingleton.Instance.RollbackTransaction();

                        throw new Exception("ETF Tickers list format is incorrect, please try again.");
                    }
                    firstRow = false;
                    continue;
                }

                String[] data = textParser.ReadFields();
#if TESTDEV || DEBUG
                if (data[0].CompareTo("EEE") > 0) continue;
#endif
                ETFStockInfo stock = new ETFStockInfo();
                
                stock.Symbol = data[0];
                stock.CompanyName = data[1];
                stock.Exchange = data[2];
                stock.Category = data[3];
                stock.FirstDate = Convert.ToInt32(data[4].Replace("-", ""));
                stock.LastDate = Convert.ToInt32(data[5].Replace("-", ""));

                stock.Save();
            }

            DatabaseSingleton.Instance.EndTransaction();

            return null;
        }
        
        public static List<StockValue> GetSharadarETFPricesFromFile(Stock etfStock, ProcessingPanelControl panel, DateTime startDate, List<KeyValuePair<int,decimal>> dividendsReturnList)
        {
            TextFieldParser textParser = null;
            JObject resultsDividend = null;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.quandl.com/api/v3/datatables/SHARADAR/SEP.json");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            String startDateStr = "2004-01-01";
            if (startDate != DateTime.MinValue)
                startDateStr = startDate.AddMonths(-1).ToString("yyyy-MM-dd");

            APIKey2 = Properties.Settings.Default.ApiKey2;

            String urlQuery = "https://www.quandl.com/api/v3/datatables/SHARADAR/SFP.csv?ticker=" + etfStock.Symbol + "&api_key=" + APIKey2 + "&date.gte=" + startDateStr;
            String urlQueryDividend = "https://www.quandl.com/api/v3/datatables/SHARADAR/ACTIONS?ticker=" + etfStock.Symbol + "&api_key=" + APIKey1 + "&action=dividend&date.gte=" + startDateStr;

            int retryAttemps = 0;
            String responseStr = "";
            String responseStrDividend = "";
            HttpResponseMessage response;
            HttpResponseMessage response_dividend;

            while (true)
            {
                response = client.GetAsync(urlQuery).Result;
                response_dividend = client.GetAsync(urlQueryDividend).Result;

                if (response.IsSuccessStatusCode && response_dividend.IsSuccessStatusCode)
                    break;

                System.Threading.Thread.Sleep(50000);

                if (retryAttemps++ > 25)
                    throw new Exception("Cannot download ETF prices data, timeout or bad Quandl API response.");
            }

            responseStr = response.Content.ReadAsStringAsync().Result;
            responseStrDividend = response_dividend.Content.ReadAsStringAsync().Result;
            resultsDividend = JObject.Parse(responseStrDividend);
            var list = resultsDividend["datatable"]["data"].ToArray();
            using (TextReader stringReader = new StringReader(responseStr))
            {
                textParser = new TextFieldParser(stringReader);

                textParser.SetDelimiters(new string[] { "," });
                textParser.HasFieldsEnclosedInQuotes = false;

                var result = new List<StockValue>();

                String firstRowStr = textParser.ReadLine();

                if (!firstRowStr.Contains("ticker,date,open,high,low,close,volume"))
                {
                    throw new Exception("Incorrect input data format, please check column headers order.");
                }

                while (!textParser.EndOfData)
                {
                    if (CancelProcess)
                        return null;
                    
                    String[] data = textParser.ReadFields();

                    StockValue newValue = new StockValue();
                    newValue.Date = Convert.ToInt32(data[1].Replace("-", ""));

                    if (data[5] != "" && !data[5].Contains("e"))
                        newValue.Close = Convert.ToDecimal(data[5], CultureInfo.InvariantCulture);

                    if (data[6] != "" && !data[6].Contains("e"))
                        newValue.Volume = Convert.ToInt64(Convert.ToDecimal(data[6], CultureInfo.InvariantCulture));
                    /*
                    if (data[3] != "" && !data[3].Contains("e"))
                        newValue.High = Convert.ToDecimal(data[3], CultureInfo.InvariantCulture);

                    if (data[4] != "" && !data[4].Contains("e"))
                        newValue.Low = Convert.ToDecimal(data[4], CultureInfo.InvariantCulture);
                    */
                    if (list.Length != 0)
                    {
                        var last = list.Last();
                        object val = last[4];
                        if (list.Length != 0)
                        {
                            foreach (var dividelt in list)
                            {
                                if (String.Compare(dividelt[0].ToString(), data[1]) <= 0)
                                {
                                    val = dividelt[4];
                                    break;
                                }
                            }
                        }
                        dividendsReturnList.Add(new KeyValuePair<int, decimal>(newValue.Date, Convert.ToDecimal(val, CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        if (data[7] != "" && data[7] != "0")
                            dividendsReturnList.Add(new KeyValuePair<int, decimal>(newValue.Date, Convert.ToDecimal(data[7], CultureInfo.InvariantCulture)));
                    }
                    result.Add(newValue);
                }

                return result;
            }
        }

        private static Dictionary<int, int> getRatiosLastDates(List<Stock> stocks)
        {
            DataTable results = DatabaseSingleton.Instance.GetData("select IFNULL((select MAX(DATE) from ratio WHERE RATIO.ID_STOCK = STOCK.ID AND RATIO.DATA_RANGE = 'ARQ'), 0), ID FROM STOCK WHERE ID != -1");

            Dictionary<int, int> ratiosLastDates = new Dictionary<int, int>();

            foreach(DataRow row in results.Rows)
            {
                ratiosLastDates.Add(Convert.ToInt32(row[1]), Convert.ToInt32(row[0]));
            }

            return ratiosLastDates;
        }

        private static Dictionary<int, int> getRatiosLastDatesYearly(List<Stock> stocks)
        {
            DataTable results = DatabaseSingleton.Instance.GetData("select IFNULL((select MAX(DATE) from ratio WHERE RATIO.ID_STOCK = STOCK.ID AND RATIO.DATA_RANGE = 'ARY'), 0), ID FROM STOCK WHERE ID != -1");

            Dictionary<int, int> ratiosLastDates = new Dictionary<int, int>();

            foreach (DataRow row in results.Rows)
            {
                ratiosLastDates.Add(Convert.ToInt32(row[1]), Convert.ToInt32(row[0]));
            }

            return ratiosLastDates;
        }

    }
}
