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
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Data;
using System.Data.SQLite;

namespace StockRanking
{
    public class StockSourcesReader
    {
        public static bool CancelProcess = false;
        
        public static void GenerateHistoryValues(String stock, int idStock)
        {
            throw new Exception("Replaced with Sharadar Data");

            //load history values from ZIP file
            ZipArchive archive = new ZipArchive(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("StockRanking.StocksData." + stock + ".zip"));
            TextReader streamReader = new StreamReader(archive.Entries[0].Open());

            DataTable data = DatabaseSingleton.Instance.GetData("SELECT IFNULL(MAX(DATE), 0) FROM PRICE_HISTORY WHERE ID_STOCK = " + idStock);

            int lastDate = Convert.ToInt32(data.Rows[0][0]);

            Stock.TickerCommandSaved = false;
            DatabaseSingleton.Instance.StartTransaction();

            String line = "";
            int lastDateSaved = lastDate;
            streamReader.ReadLine();
            while ((line = streamReader.ReadLine()) != null)
            {
                StockValue newValue = new StockValue();
                String[] dataFields = line.Split('\t');
                try
                {
                    newValue.Date = Convert.ToInt32(dataFields[0]);
                    newValue.Close = Convert.ToDecimal(dataFields[4], CultureInfo.InvariantCulture);
                    newValue.Volume = Convert.ToInt64(Convert.ToDecimal(dataFields[6], CultureInfo.InvariantCulture));
                }
                catch (Exception e)
                {
                    throw e;
                }

                if (newValue.Date > lastDate && newValue.Date < Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")))
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

            }

            DatabaseSingleton.Instance.EndTransaction();

        }

        
        public static List<Stock> GetStocksList()
        {
            if (CancelProcess)
                return null;

            List<Stock> results = new List<Stock>();

            String apikey = Properties.Settings.Default.ApiKey3;
            //read all symbols from iextrading

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://cloud.iexapis.com/stable/ref-data/symbols");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("https://cloud.iexapis.com/stable/ref-data/symbols?token=" + apikey).Result;
            List<IExtradingStock> responseStocks;

            if (response.IsSuccessStatusCode)
            {
                responseStocks = response.Content.ReadAsAsync<List<IExtradingStock>>().Result;

                foreach (IExtradingStock responseStock in responseStocks)
                {
                    Stock newStock = new Stock();

                    newStock.CompanyName = responseStock.name;
                    newStock.Symbol = responseStock.symbol;

                    //Console.WriteLine("AAAAAAAA : " + newStock.CompanyName + ", " + newStock.Symbol);
                    if (CancelProcess)
                        return null;
                }
            }
            else
            {
                throw new Exception("Error getting stocks List");
            }

            return results;
        }
        
        public static List<StockValue> QueryIndexPriceHistory(ProcessingPanelControl progressProcessing, int maxDateDiff, int stockId, String symbol)
        {
            throw new Exception("Replaced with Sharadar data");

            symbol = symbol.ToUpper();

            if (CancelProcess)
                return null;

            String range = "1m";
            if (maxDateDiff > 26)
                range = "3m";
            if (maxDateDiff > 78)
                range = "6m";
            if (maxDateDiff > 168)
                range = "1y";
            if (maxDateDiff > 300)
                range = "2y";
            if (maxDateDiff > 600)
                range = "5y";
            /*
           progressProcessing.SetTitle("Updating SPY Data");
           progressProcessing.SetMaxValue(maxDateDiff);
           progressProcessing.PerformStep();
           */

            String apikey = Properties.Settings.Default.ApiKey3;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://cloud.iexapis.com/stable/stock/market/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("https://cloud.iexapis.com/stable/stock/market/batch?symbols=" + symbol + "&token=" + apikey + "&types=chart&range=" + range).Result;

            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
            settings.UseSimpleDictionaryFormat = true;

            if (response.IsSuccessStatusCode)
            { 
                String responseStocks = response.Content.ReadAsStringAsync().Result;

                JObject results = JObject.Parse(responseStocks);

                List<StockValue> resultValues = new List<StockValue>();
                 
                if (CancelProcess)
                    return null;

                Application.DoEvents();

                try
                {
                    JArray arrayValues = ((JArray)results.GetValue(symbol)["chart"]);

                    foreach (JToken value in arrayValues)
                    {
                        StockValue newValue = new StockValue();
                        resultValues.Add(newValue);
                             
                        newValue.Date = Convert.ToInt32(value["date"].ToString().Replace("-", ""));
                        newValue.ChangePercent = Convert.ToDecimal(value["changePercent"]);
                        newValue.Close = Convert.ToDecimal(value["close"]);
                        newValue.Volume = Convert.ToInt32(value["volume"]);

                        //Console.WriteLine("BBBBBBB: " + newValue.Date + ", " + newValue.ChangePercent + ", " + newValue.Close + ", " + newValue.Volume);
                    }

                    return resultValues;

                }
                catch (Exception e)
                {
                    throw new Exception("Error downloading " + symbol + " Data: " + e.Message + " / " + e.StackTrace);
                }
                 
            }

            throw new Exception("Error downloading " + symbol + " Data, error downloading data from iextrading, please retry in a couple of minutes.");
            
        }

        public static async Task<StockValue> QueryRealtimePrice(String _ticker)
        {
            String ticker = _ticker.Replace("*", "");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://cloud.iexapis.com/stable/stock/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            StockValue result = new StockValue();

            Dictionary<String, decimal> results = new Dictionary<string, decimal>();
            String apikey = Properties.Settings.Default.ApiKey3;

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://cloud.iexapis.com/stable/stock/" + ticker + "/quote?token=" + apikey);

                String responsePrice = response.Content.ReadAsStringAsync().Result;

                JObject resultJSON = JObject.Parse(responsePrice);

                result.Close = Convert.ToDecimal(resultJSON["latestPrice"]); 
                result.ChangePercent = Convert.ToDecimal(resultJSON["changePercent"]);
                result.AuxYtdChange = Convert.ToDecimal(resultJSON["ytdChange"]);

                //Console.WriteLine(result.Close + ", " + result.ChangePercent + ", " + result.AuxYtdChange);
                //latestSource 
            }
            catch (Exception e)
            { return null; }

            return result;
        }



    }
}
