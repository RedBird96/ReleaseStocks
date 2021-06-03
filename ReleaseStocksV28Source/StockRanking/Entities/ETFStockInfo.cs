using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace StockRanking
{
    public class ETFStockInfo
    {
        public String Symbol = "";
        public String CompanyName = "";
        public String Exchange = "";
        public String Category = "";
        public int FirstDate = 0;
        public int LastDate = 0;

        public static bool CommandSaved = false;

        public ETFStockInfo()
        {

        }

        public ETFStockInfo(DataRow row)
        {
            Symbol = Convert.ToString(row["SYMBOL"]);
            CompanyName = Convert.ToString(row["COMPANY_NAME"]);
            Exchange = Convert.ToString(row["EXCHANGE"]);
            Category = Convert.ToString(row["CATEGORY"]);
            FirstDate = Convert.ToInt32(row["FIRST_DATE"]);
            LastDate = Convert.ToInt32(row["LAST_DATE"]);
        }

        public static List<ETFStockInfo> GetAvailableETFSymbols()
        {
            var result = new List<ETFStockInfo>();
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT * FROM ETF_TMP_LIST E WHERE NOT EXISTS (SELECT SYMBOL FROM STOCK S WHERE S.SYMBOL = E.SYMBOL)");

            foreach (DataRow row in data.Rows)
            {
                result.Add(new ETFStockInfo(row));
            }

            return result;
        }

        public void Save()
        {
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();

            parameters.Add(new SQLiteParameter("@p1", Symbol));
            parameters.Add(new SQLiteParameter("@p2", CompanyName));
            parameters.Add(new SQLiteParameter("@p3", Exchange));
            parameters.Add(new SQLiteParameter("@p4", Category));
            parameters.Add(new SQLiteParameter("@p5", FirstDate));
            parameters.Add(new SQLiteParameter("@p6", LastDate));

            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO ETF_TMP_LIST (SYMBOL, COMPANY_NAME, EXCHANGE, CATEGORY, FIRST_DATE, LAST_DATE) VALUES (@p1,@p2,@p3,@p4,@p5,@p6)", parameters.ToArray(), CommandSaved);

            CommandSaved = true;
        }

        public void SaveAsNewStock()
        {
            var Stock = new Stock();
            Stock.Symbol = this.Symbol;
            Stock.CompanyName = this.CompanyName;
            Stock.Exchange = this.Exchange;
            Stock.Type = StockTypes.Benchmark;

            Stock.Save(true);
        }

    }
}
