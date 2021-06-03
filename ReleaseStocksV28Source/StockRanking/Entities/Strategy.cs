using StockRanking.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockRanking
{
    public class Strategy
    {
        public List<Feature> Features = new List<Feature>();
        public List<Feature> OptimalFeatures = new List<Feature>();
        public List<FilterConditions> Filters = new List<FilterConditions>();
        public String Name = "";
        public int Id = 0;
        public bool Selected = false;
        public PortfolioParameters PortfolioParameters = new PortfolioParameters();

        public Strategy()
        {

        }

        public Strategy(DataRow row)
        {
            Id = Convert.ToInt32(row["ID"]);
            Name = row["NAME"].ToString();
            Selected = row["SELECTED"].ToString() == "1";

            this.Features = Feature.LoadFeatures(Id);
            this.OptimalFeatures = Feature.LoadOptimalFeatures(Id);
            this.Filters = FilterConditions.LoadFilters(Id);
            this.PortfolioParameters = PortfolioParameters.Load(Id);
        }

        public void Save()
        {
            DatabaseSingleton.Instance.StartTransaction();

            try
            {
                List<SQLiteParameter> parameters;

                if (Id == 0)
                {
                    //get last ID
                    int newId = Convert.ToInt32(DatabaseSingleton.Instance.GetData("SELECT IFNULL(MAX(ID), 0) FROM STRATEGY").Rows[0][0]) + 1;

                    parameters = new List<SQLiteParameter>();

                    parameters.Add(new SQLiteParameter("@p1", newId));
                    parameters.Add(new SQLiteParameter("@p2", Name));
                    parameters.Add(new SQLiteParameter("@p3", 0));

                    DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO STRATEGY (ID, NAME, BENCHMARK_RESULT) VALUES (@p1,@p2,@p3)", parameters.ToArray());

                    this.Id = newId;
                }
                else
                {
                    parameters = new List<SQLiteParameter>();

                    parameters.Add(new SQLiteParameter("@p1", Id));
                    parameters.Add(new SQLiteParameter("@p2", Name));

                    DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE STRATEGY SET NAME = @p2 WHERE ID = @p1", parameters.ToArray());
                }

                //save features and filters collections
                parameters = new List<SQLiteParameter>();
                parameters.Add(new SQLiteParameter("@p1", Id));

                DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STRATEGY_FEATURE WHERE ID_STRATEGY = @p1", parameters.ToArray());
                DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STRATEGY_FILTER WHERE ID_STRATEGY = @p1", parameters.ToArray());

                foreach(FilterConditions filter in this.Filters)
                {
                    filter.Save(Id);
                }

                foreach (Feature feature in this.Features)
                {
                    feature.Save(Id);
                }

                //save portfolio parameters
                this.PortfolioParameters.Id = Id;
                this.PortfolioParameters.Save();

                this.PortfolioParameters.MailingParameters.IdStrategy = Id;
                this.PortfolioParameters.MailingParameters.Save();

                DatabaseSingleton.Instance.EndTransaction();
            }
            catch (Exception e)
            {
                DatabaseSingleton.Instance.RollbackTransaction();
            }

        }

        public static List<Strategy> GetStrategies()
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT * FROM STRATEGY");
            List<Strategy> result = new List<Strategy>();

            foreach (DataRow row in data.Rows)
                result.Add(new Strategy(row));

            return result;
        }


        public String GetFeaturesText()
        {
            List<FeatureWeight> featureLabels = Feature.generateFeatureWeightsList();
            StringBuilder textFeatures = new StringBuilder();

            foreach (Feature feature in Features)
            {
                textFeatures.Append(featureLabels[feature.FeatureId].ShortName);
                textFeatures.Append(" ");
            }

            return textFeatures.ToString().Trim();
        }


        public String GetFiltersText()
        {
            StringBuilder filtersText = new StringBuilder();

            foreach (FilterConditions filter in Filters)
            {
                filtersText.Append(", ");
                if (filter.isCustom == 0)
                {
                    filtersText.Append(Enum.GetName(typeof(FilterTypes), filter.FilterType));
                } else
                {
                    EquationFilter efilter = EquationFilter.GetFilter(filter.isCustom);
                    if (efilter != null)
                        filtersText.Append(efilter.FilterName);
                }
                
            }
            filtersText.Append(" ");

            return filtersText.ToString().Substring(1).Trim();
        }

        public static void SetSelectedStrategy(Strategy s)
        {
            DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE STRATEGY SET SELECTED = 0", null);
            DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE STRATEGY SET SELECTED = 1 WHERE ID = " + s.Id, null);
        }

        public static void ClearSelectedStrategy()
        {
            DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE STRATEGY SET SELECTED = 0", null);
        }



        public static void DeleteStrategy(Strategy s)
        {
            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STRATEGY WHERE ID = " + s.Id, null);
            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STRATEGY_FEATURE WHERE ID_STRATEGY = " + s.Id, null);
            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STRATEGY_OPTIMAL_FEATURE WHERE ID_STRATEGY = " + s.Id, null);
            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STRATEGY_FILTER WHERE ID_STRATEGY = " + s.Id, null);
            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM PORFOLIO_PARAMETERS WHERE ID = " + s.Id, null);
            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STRATEGY_DESCRIPTION WHERE ID = " + s.Id, null);
            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STRATEGY_MAILING WHERE ID_STRATEGY = " + s.Id, null);
        }

        public static void SaveStrategyDescription(int id, String description)
        {
            //save features and filters collections
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("@p1", id));
            parameters.Add(new SQLiteParameter("@p2", description));

            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM STRATEGY_DESCRIPTION WHERE ID = @p1", parameters.ToArray());
            DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO STRATEGY_DESCRIPTION (ID, DESCRIPTION) VALUES (@p1, @p2)", parameters.ToArray());
        }

        public static String GetStrategyDescription(int id)
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT DESCRIPTION FROM STRATEGY_DESCRIPTION WHERE ID = " + id);
            
            foreach (DataRow row in data.Rows)
                return row[0].ToString();

            return "";
        }

        public DateTime GetLastRebalanceDate()
        {
            //always return inception date
            return new DateTime(2005, 01, 01);

            DateTime startDate = DateTime.Now.AddYears(-1);
            startDate = new DateTime(startDate.Year, 1, 1);
            while (Utils.ConvertDateTimeToInt(startDate.AddMonths(this.PortfolioParameters.RebalanceFrequencyMonths)) <= Utils.ConvertDateTimeToInt(DateTime.Now))
            {
                startDate = startDate.AddMonths(this.PortfolioParameters.RebalanceFrequencyMonths);
            }
            //from last period go 1 year back
            startDate = startDate.AddYears(-3);

            return startDate;
        }
    }
}
