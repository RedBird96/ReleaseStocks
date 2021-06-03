using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.Data.SQLite;
using System.Data;

namespace StockRanking
{
    public class FileBasedStock : CachedValuesStock
    {
        private int idStock = 0;
        public String Path = "";
        public bool KeepUpdated = false;

        public Stock Stock = null;

        //aux used to show last price date on the UI
        public int LastPriceDate = 0;

        //aux used to show name in comboboxes
        public override String StockSymbol => Stock == null ? " Not Set" : Stock.Symbol;

        public override int IdStock { get => this.idStock;}

        public FileBasedStock()
        {
            
        }

        public FileBasedStock(DataRow row, Stock stock)
        {
            this.idStock = Convert.ToInt32(row["ID"]);
            this.Path = Convert.ToString(row["FILE_PATH"]);
            this.KeepUpdated = Convert.ToInt32(row["KEEP_UPDATED"]) == 1;
            this.LastPriceDate = Convert.ToInt32(row["LAST_PRICE"]);
            this.Stock = stock;
        }

        public static List<FileBasedStock> GetAllFileBasedStocks()
        {
            var result = new List<FileBasedStock>();
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT S.*, (SELECT IFNULL(MAX(P.DATE), 0) FROM STOCK_FILE_PRICE P WHERE P.ID_STOCK = S.ID) LAST_PRICE FROM STOCK_FILE S");

            foreach(DataRow row in data.Rows)
            {
                var stock = Stock.GetStock(Convert.ToInt32(row["ID"]));
                result.Add(new FileBasedStock(row, stock));
            }

            return result;
        }

        protected override void loadPricesInternal()
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT DATE, CAST(VALUE as double) VALUE FROM STOCK_FILE_PRICE WHERE ID_STOCK = " + this.IdStock + " ORDER BY DATE ASC");
            prices = new List<KeyValuePair<int, decimal>>();

            foreach (DataRow row in data.Rows)
            {
                var date = Convert.ToInt32(row["DATE"]);
                var value = Convert.ToDecimal(row["VALUE"]);

                prices.Add(new KeyValuePair<int, decimal>(date, value));
            }
        }

        public static bool CheckFileFormat(String path)
        {
            if (!File.Exists(path))
                return false;

            try
            {
                TextFieldParser textParser = new TextFieldParser(path);
                if (textParser.ReadLine().Contains(";"))
                    textParser.SetDelimiters(new string[] { ";" });
                else
                    textParser.SetDelimiters(new string[] { "," });
                textParser.HasFieldsEnclosedInQuotes = false;
                
                while (!textParser.EndOfData)
                {
                    String[] data = textParser.ReadFields();

                    if (data[1] == "@NA" || data[1].Trim() == "")
                        continue;

                    decimal value = Convert.ToDecimal(data[1].Replace(",", ""));
                    Convert.ToInt32(data[0]);
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
        
        public static bool ImportFileData(FileBasedStock fileBasedStock)
        {
            if (!File.Exists(fileBasedStock.Path))
                return false;

            var values = new Dictionary<int, decimal>();

            try
            {
                TextFieldParser textParser = new TextFieldParser(fileBasedStock.Path);
                if (textParser.ReadLine().Contains(";"))
                    textParser.SetDelimiters(new string[] { ";" });
                else
                    textParser.SetDelimiters(new string[] { "," });
                textParser.HasFieldsEnclosedInQuotes = false;

                while (!textParser.EndOfData)
                {
                    String[] data = textParser.ReadFields();

                    if (data[1] == "@NA" || data[1].Trim() == "")
                        continue;

                    decimal value = Convert.ToDecimal(data[1].Replace(",", ""));
                    int date = Convert.ToInt32(data[0]);

                    if (!values.ContainsKey(Convert.ToInt32(data[0])))
                        values.Add(Convert.ToInt32(data[0]), value);
                }

                if (values.Count < 10)
                    return false;

                SaveValues(fileBasedStock, values);
                
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public static void SaveValues(FileBasedStock stock, Dictionary<int, decimal> values)
        {
            try
            {
                DatabaseSingleton.Instance.StartTransaction();

                DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STOCK_FILE_PRICE WHERE ID_STOCK = " + stock.IdStock, null);

                var reuseLast = false;
                foreach (var value in values)
                {
                    List<SQLiteParameter> parameters = new List<SQLiteParameter>();
                    parameters.Add(new SQLiteParameter("@p1", stock.IdStock));
                    parameters.Add(new SQLiteParameter("@p2", value.Key));
                    parameters.Add(new SQLiteParameter("@p3", value.Value));

                    DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO STOCK_FILE_PRICE (ID_STOCK, DATE, VALUE) VALUES (@p1,@p2,@p3)", parameters.ToArray(), reuseLast);
                    reuseLast = true;
                }

                DatabaseSingleton.Instance.EndTransaction();
            }
            catch(Exception e)
            {
                DatabaseSingleton.Instance.RollbackTransaction();
                throw e;
            }
        }

        public bool Save()
        {
            try
            {
                DatabaseSingleton.Instance.StartTransaction();

                this.Stock.Type = StockTypes.FileBased;
                this.Stock.Save(true);

                if (this.IdStock == 0)
                {
                    this.idStock = this.Stock.Id;

                    List<SQLiteParameter> parameters = new List<SQLiteParameter>();
                    parameters.Add(new SQLiteParameter("@p1", this.IdStock));
                    parameters.Add(new SQLiteParameter("@p2", this.Path));
                    parameters.Add(new SQLiteParameter("@p3", this.KeepUpdated ? 1 : 0));

                    DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO STOCK_FILE (ID, FILE_PATH, KEEP_UPDATED) VALUES (@p1,@p2,@p3)", parameters.ToArray());
                }
                else
                {
                    List<SQLiteParameter> parameters = new List<SQLiteParameter>();
                    parameters.Add(new SQLiteParameter("@p1", this.IdStock));
                    parameters.Add(new SQLiteParameter("@p2", this.Path));
                    parameters.Add(new SQLiteParameter("@p3", this.KeepUpdated ? 1 : 0));

                    DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE STOCK_FILE SET FILE_PATH = @p2, KEEP_UPDATED = @p3 WHERE ID = @p1", parameters.ToArray());
                }

                DatabaseSingleton.Instance.EndTransaction();

                return true;
            }
            catch (Exception e)
            {
                DatabaseSingleton.Instance.RollbackTransaction();

                return false;
            }
        }

        public static bool Delete(FileBasedStock stock)
        {
            try
            {
                DatabaseSingleton.Instance.StartTransaction();

                Stock.Delete(stock.IdStock);

                

                DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STOCK_FILE WHERE ID = " + stock.IdStock, null);

                DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STOCK_FILE_PRICE WHERE ID_STOCK = " + stock.IdStock, null);

                DatabaseSingleton.Instance.EndTransaction();

                return true;
            }
            catch (Exception e)
            {
                DatabaseSingleton.Instance.RollbackTransaction();

                return false;
            }
        }
        
    }
}
