using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class MailingParameters
    {
        public int IdStrategy { get; set; }
        public DateTime LastMailSent { get; set; }
        public List<String> Destinations { get; set; }
        public bool SendRebalance { get; set; }
        public String SendWeekdays { get; set; }
        public bool SendSells { get; set; }
        public bool IncludePortfolio { get; set; }
        public int IncludeTopStocks { get; set; }
        public bool IncludeRiskModel { get; set; }
        public bool IncludeBondsModel { get; set; }
        public bool IncludePerformanceTable { get; set; }

        public String MailText { get; set; }
        public String StrategyName { get; set; }
        
        public MailingParameters()
        {
            Destinations = new List<string>();
            SendRebalance = true;
            SendWeekdays = "12345";
            SendSells = true;
            IncludePortfolio = true;
            IncludeTopStocks = 50;
            IncludeRiskModel = true;
            IncludeBondsModel = true;
            IncludePerformanceTable = true;
            LastMailSent = DateTime.MinValue;
        }

        public MailingParameters(DataRow row)
        {
            LastMailSent = DateTime.MinValue;
            IdStrategy = Convert.ToInt32(row["ID_STRATEGY"]);
            var DestinationsStr = Convert.ToString(row["EMAILS"]);

            if (DestinationsStr == "")
                this.Destinations = new List<string>();
            else
                this.Destinations = DestinationsStr.Split(';').ToList();

            SendWeekdays = Convert.ToString(row["SEND_WEEKDAYS"]);

            SendRebalance = Utils.ConvertDBIntToBoolean(row["SEND_REBALANCE"]);
            SendSells = Utils.ConvertDBIntToBoolean(row["SEND_SELLS"]);
            IncludePortfolio = Utils.ConvertDBIntToBoolean(row["INCLUDE_PORTFOLIO"]);
            IncludeTopStocks = Convert.ToInt32(row["INCLUDE_TOP_STOCKS"]);
            IncludeRiskModel = Utils.ConvertDBIntToBoolean(row["INCLUDE_RISK_MODEL"]);
            IncludeBondsModel = Utils.ConvertDBIntToBoolean(row["INCLUDE_BONDS_MODEL"]);
            IncludePerformanceTable = Utils.ConvertDBIntToBoolean(row["INCLUDE_PERFORMANCE_TABLE"]);
            if(Convert.ToInt32(row["LAST_MAIL_SENT"]) > 20190000)
                LastMailSent = Utils.ConvertIntToDateTime(Convert.ToInt32(row["LAST_MAIL_SENT"]));

            /*STRATEGY_MAILING 

            ID_STRATEGY
            EMAILS 
            SEND_REBALANCE 
            SEND_WEEKDAYS 
            SEND_SELLS 
            INCLUDE_PORTFOLIO 
            INCLUDE_TOP_STOCKS 
            INCLUDE_RISK_MODEL 
            INCLUDE_BONDS_MODEL 
            INCLUDE_PERFORMANCE_TABLE
            LAST_MAIL_SENT*/

        }

        public static MailingParameters Load(int idStrategy)
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT ID_STRATEGY, EMAILS, SEND_REBALANCE, SEND_WEEKDAYS, SEND_SELLS, INCLUDE_PORTFOLIO, INCLUDE_TOP_STOCKS, INCLUDE_RISK_MODEL, INCLUDE_BONDS_MODEL, INCLUDE_PERFORMANCE_TABLE, LAST_MAIL_SENT FROM STRATEGY_MAILING WHERE ID_STRATEGY = " + idStrategy);

            if (data.Rows.Count > 0)
                return new MailingParameters(data.Rows[0]);

            return new MailingParameters();
        }
        
        public void Save()
        {
            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STRATEGY_MAILING WHERE ID_STRATEGY = " + this.IdStrategy, null);

            List<SQLiteParameter> parameters;

            parameters = new List<SQLiteParameter>();

            parameters.Add(new SQLiteParameter("@p1", this.IdStrategy));
            parameters.Add(new SQLiteParameter("@p2", String.Join(";", this.Destinations)));
            parameters.Add(new SQLiteParameter("@p3", this.SendWeekdays));
            parameters.Add(new SQLiteParameter("@p4", Utils.ConvertBooleanToDBInt(this.SendRebalance)));
            parameters.Add(new SQLiteParameter("@p5", Utils.ConvertBooleanToDBInt(this.SendSells)));
            parameters.Add(new SQLiteParameter("@p6", Utils.ConvertBooleanToDBInt(this.IncludePortfolio)));
            parameters.Add(new SQLiteParameter("@p7", this.IncludeTopStocks));
            parameters.Add(new SQLiteParameter("@p8", Utils.ConvertBooleanToDBInt(this.IncludeRiskModel)));
            parameters.Add(new SQLiteParameter("@p9", Utils.ConvertBooleanToDBInt(this.IncludeBondsModel)));
            parameters.Add(new SQLiteParameter("@p10", Utils.ConvertBooleanToDBInt(this.IncludePerformanceTable)));
            parameters.Add(new SQLiteParameter("@p11", Utils.ConvertDateTimeToInt(this.LastMailSent)));

            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO STRATEGY_MAILING (ID_STRATEGY, EMAILS, SEND_WEEKDAYS, SEND_REBALANCE, SEND_SELLS, INCLUDE_PORTFOLIO, INCLUDE_TOP_STOCKS, INCLUDE_RISK_MODEL, INCLUDE_BONDS_MODEL, INCLUDE_PERFORMANCE_TABLE, LAST_MAIL_SENT) " +
                " VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11)", parameters.ToArray());
        }

        public void SaveMailSent(int dateToProcess)
        {
            DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE STRATEGY_MAILING SET LAST_MAIL_SENT = " + dateToProcess + " WHERE ID_STRATEGY = " + this.IdStrategy, null);
        }

    }
}
