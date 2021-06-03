using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class StockValue
    {
        public long Volume = 0;
        public int Date = 0;
        public decimal ChangePercent = 0;
        public decimal Close = 0;
        public decimal Avg20DaysVolume = 0;

        //auxiliary in order to plot composite
        public List<decimal> CompositeValues = null;

        public static bool CommandSaved = false;
        protected static List<SQLiteParameter> savedParameters = new List<SQLiteParameter>();

        public double AuxComposite = 0;

        private decimal auxYtdChange = 0;
        public decimal AuxYtdChange { get => this.auxYtdChange; set => this.auxYtdChange = value; }

        public StockValue()
        {

        }

        public StockValue(StockValue stockValue)
        {
            this.Volume = stockValue.Volume;
            this.Date = stockValue.Date;
            this.ChangePercent = stockValue.ChangePercent;
            this.Close = stockValue.Close;
            this.Avg20DaysVolume = stockValue.Avg20DaysVolume;
        }

        public StockValue(DataRow row)
        {
            Volume = Convert.ToInt64(row["VOLUME"]);
            Date = Convert.ToInt32(row["DATE"]);
            Avg20DaysVolume = Convert.ToDecimal(row["AVG_20DAYS_VOLUME"]);

            Close = Convert.ToDecimal(row["CLOSE"]);
            ChangePercent = Convert.ToDecimal(row["CHANGE_PERCENT"]);
        }


        public void Save(int stockId, bool saveAsFirstOfMonth)
        {
            int dateConverted = this.Date;
            if(saveAsFirstOfMonth)
            {
                DateTime firstMonthDate = Utils.ConvertIntToDateTime(dateConverted);
                dateConverted = Convert.ToInt32(firstMonthDate.ToString("yyyyMM") + "01");
            }

            StockValue.savedParameters = new List<SQLiteParameter>();

            StockValue.savedParameters.Add(new SQLiteParameter("@p1", stockId));
            StockValue.savedParameters.Add(new SQLiteParameter("@p2", Volume));
            StockValue.savedParameters.Add(new SQLiteParameter("@p3", Close));
            StockValue.savedParameters.Add(new SQLiteParameter("@p4", Avg20DaysVolume));
            StockValue.savedParameters.Add(new SQLiteParameter("@p5", dateConverted));
             
            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO PRICE (ID_STOCK, VOLUME, CLOSE_PRICE, AVG_20DAYS_VOLUME, DATE) VALUES (@p1,@p2,@p3,@p4,@p5)", savedParameters.ToArray(), CommandSaved);
            StockValue.CommandSaved = true;
        }

        public virtual void SaveHistoricPrice(int stockId)
        {
            StockValue.savedParameters = new List<SQLiteParameter>();

            StockValue.savedParameters.Add(new SQLiteParameter("@p1", stockId));
            StockValue.savedParameters.Add(new SQLiteParameter("@p2", Volume));
            StockValue.savedParameters.Add(new SQLiteParameter("@p3", Close));
            StockValue.savedParameters.Add(new SQLiteParameter("@p4", Date));
            StockValue.savedParameters.Add(new SQLiteParameter("@p5", ChangePercent));
            
            //if (!DatabaseSingleton.Instance.Exists("SELECT COUNT(*) FROM PRICE_HISTORY WHERE ID_STOCK = " + stockId.ToString() + " and DATE = " + Date.ToString()))
            // {
            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO PRICE_HISTORY (ID_STOCK, VOLUME, CLOSE_PRICE, DATE, CHANGE_PERCENT, THREE_MONTHS_HIGH, THREE_MONTHS_LOW) VALUES (@p1,@p2,@p3,@p4,@p5,0,0)", savedParameters.ToArray(), CommandSaved);
            StockValue.CommandSaved = true;
           // }
        }


        public static void LoadValues(Stock stock, int lastN)
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT * FROM PRICE WHERE ID_STOCK = " + stock.Id.ToString() + " ORDER BY DATE DESC LIMIT " + lastN.ToString());

            foreach (DataRow row in data.Rows)
            {
                stock.stockValues.Add(new StockValue(row));
            }


        }

    }
}
