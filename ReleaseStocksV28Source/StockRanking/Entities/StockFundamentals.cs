using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class StockFundamentals : IStockData
    {
        public String Stock = "";
        public String CompanyName = "";
        public String CompanyName2 = "";
        public double ebit = 0;
        public long SharesOut = 0;
        public String DataRange = "Q";
        public double LtermTotalDebt = 0;


        public int Date { get; set; }
        public int Quarter { get; set; }
        public int Year { get; set; } 
        public static bool CommandSaved = false;

        public StockFundamentals()
        {

        }

        public StockFundamentals(DataRow row)
        {
            Stock = Convert.ToString(row["SYMBOL"]);
            ebit = Convert.ToDouble(row["EBIT"]);
            SharesOut = Convert.ToInt64(row["SHARES_OUT"]);
            LtermTotalDebt = Convert.ToDouble(row["TOT_LTERM_DEBT"]);
            Date = Convert.ToInt32(row["DATE"]);
            Quarter = Convert.ToInt32(row["QUARTER"]);
            Year = Convert.ToInt32(row["YEAR"]);
            DataRange = row["DATA_RANGE"].ToString();
        }

        public void Save(int stockId)
        {
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();

            parameters.Add(new SQLiteParameter("@p1", stockId));
            parameters.Add(new SQLiteParameter("@p2", Stock));
            parameters.Add(new SQLiteParameter("@p3", ebit));
            parameters.Add(new SQLiteParameter("@p4", SharesOut));
            parameters.Add(new SQLiteParameter("@p5", LtermTotalDebt));
            parameters.Add(new SQLiteParameter("@p6", Date));
            parameters.Add(new SQLiteParameter("@p7", Quarter));
            parameters.Add(new SQLiteParameter("@p8", Year));
            parameters.Add(new SQLiteParameter("@p9", DataRange));

            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO FUNDAMENTAL (ID_STOCK, SYMBOL,EBIT, SHARES_OUT,TOT_LTERM_DEBT, DATE,QUARTER,YEAR, DATA_RANGE) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)", parameters.ToArray(), CommandSaved);
            CommandSaved = true;
        }

        public static void LoadFundamentals(Stock stock)
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT * FROM FUNDAMENTAL WHERE ID_STOCK = " + stock.Id.ToString() + " AND DATA_RANGE = 'Q' ORDER BY DATE DESC");

            foreach (DataRow row in data.Rows)
            {
                stock.Fundamentals.Add(new StockFundamentals(row));
            }

            //load consecutive EBIT years
            data = DatabaseSingleton.Instance.GetData("SELECT COUNT(*) FROM FUNDAMENTAL WHERE ID_STOCK = " + stock.Id.ToString() + " AND DATA_RANGE = 'A' AND DATE > " +
                " (SELECT IFNULL(MAX(DATE), 0) FROM FUNDAMENTAL WHERE ID_STOCK = " + stock.Id.ToString() + " AND DATA_RANGE = 'A' AND EBIT <= 0) ");

            stock.PositiveEBITYears = Convert.ToInt32(data.Rows[0][0]);


            //load shares out
            data = DatabaseSingleton.Instance.GetData("SELECT IFNULL(SHARES_OUT, 0) FROM FUNDAMENTAL WHERE ID_STOCK = " + stock.Id.ToString() + " AND SHARES_OUT != 0 ORDER BY DATE DESC LIMIT 1 ");

            if(data.Rows.Count > 0)
                stock.SharesOut = Convert.ToInt64(data.Rows[0][0]);

        }

    }
}
