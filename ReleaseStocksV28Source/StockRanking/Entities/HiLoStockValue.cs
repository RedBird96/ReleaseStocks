using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class HiLoStockValue : StockValue
    {
        public int ThreeMonthsHigh = 0;
        public int ThreeMonthsLow = 0;

        //auxiliary values for HI LO model
        public decimal High = 0;
        public decimal Low = 0;

        public HiLoStockValue(StockValue stockValue):base(stockValue)
        {
            
        }

        public HiLoStockValue() : base()
        {

        }
        public override void SaveHistoricPrice(int stockId)
        {
            StockValue.savedParameters = new List<SQLiteParameter>();

            StockValue.savedParameters.Add(new SQLiteParameter("@p1", stockId));
            StockValue.savedParameters.Add(new SQLiteParameter("@p2", Volume));
            StockValue.savedParameters.Add(new SQLiteParameter("@p3", Close));
            StockValue.savedParameters.Add(new SQLiteParameter("@p4", Date));
            StockValue.savedParameters.Add(new SQLiteParameter("@p5", ChangePercent));

            StockValue.savedParameters.Add(new SQLiteParameter("@p6", ThreeMonthsHigh));
            StockValue.savedParameters.Add(new SQLiteParameter("@p7", ThreeMonthsLow));

            //if (!DatabaseSingleton.Instance.Exists("SELECT COUNT(*) FROM PRICE_HISTORY WHERE ID_STOCK = " + stockId.ToString() + " and DATE = " + Date.ToString()))
            // {
            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO PRICE_HISTORY (ID_STOCK, VOLUME, CLOSE_PRICE, DATE, CHANGE_PERCENT, THREE_MONTHS_HIGH, THREE_MONTHS_LOW) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7)", savedParameters.ToArray(), CommandSaved);
            StockValue.CommandSaved = true;
            // }
        }

    }
}
