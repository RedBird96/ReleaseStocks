using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class MailConfiguration
    {
        public String From { get; set; }
        public String Smtp { get; set; }
        public int Port { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }

        public MailConfiguration()
        {

        }

        public static MailConfiguration Load()
        {
            var result = new MailConfiguration();

            var dataTable = DatabaseSingleton.Instance.GetData("SELECT * FROM SYSTEM_PARAMETERS WHERE ID IN (5,6,7,8,9)", null);

            if (dataTable.Rows.Count == 0)
                return null;

            foreach(DataRow row in dataTable.Rows)
            {
                switch((SystemParameters)Convert.ToInt32(row["ID"]))
                {
                    case SystemParameters.MailConfigFrom:
                        result.From = row["Value"].ToString();
                        break;
                    case SystemParameters.MailConfigPort:
                        result.Port = Convert.ToInt32(row["Value"]);
                        break;
                    case SystemParameters.MailConfigSmtp:
                        result.Smtp = row["Value"].ToString();
                        break;
                    case SystemParameters.MailConfigUser:
                        result.Username = row["Value"].ToString();
                        break;
                    case SystemParameters.MailConfigPass:
                        result.Password = row["Value"].ToString();
                        break;
                }
            }
            
            return result;
        }

        public void Save()
        {
            try
            {
                DatabaseSingleton.Instance.StartTransaction();

                DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM SYSTEM_PARAMETERS WHERE ID IN (5,6,7,8,9)", null);
                DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO SYSTEM_PARAMETERS VALUES (" + (int)SystemParameters.MailConfigFrom + ", '" + this.From + "')", null);
                DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO SYSTEM_PARAMETERS VALUES (" + (int)SystemParameters.MailConfigPort + ", " + this.Port + ")", null);
                DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO SYSTEM_PARAMETERS VALUES (" + (int)SystemParameters.MailConfigSmtp + ", '" + this.Smtp + "')", null);
                DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO SYSTEM_PARAMETERS VALUES (" + (int)SystemParameters.MailConfigUser + ", '" + this.Username + "')", null);
                DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO SYSTEM_PARAMETERS VALUES (" + (int)SystemParameters.MailConfigPass + ", '" + this.Password + "')", null);

                DatabaseSingleton.Instance.EndTransaction();
            }
            catch (Exception e)
            {
                DatabaseSingleton.Instance.RollbackTransaction();
            }
        }
    }
}
