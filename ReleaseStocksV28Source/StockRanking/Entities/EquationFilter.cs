using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking.Entities
{
    public class EquationFilter
    {
        public int Id = -1;
        public String FilterName = "";
        public int FilterType = 0;
        public int FilterOption = 0;
        public decimal Value1 = 0;
        public decimal Value2 = 0;
        public String Equation1 = "";
        public String Equation2 = "";
        public int EquCompare;

        
        public EquationFilter()
        {

        }

        public EquationFilter(DataRow row)
        {
            Id = Convert.ToInt32(row["ID"]);
            FilterName = Convert.ToString(row["FILTERNAME"]);
            FilterType = Convert.ToInt32(row["FILTERTYPE"]);
            FilterOption = Convert.ToInt32(row["FILTEROPTION"]);
            Value1 = Convert.ToDecimal(row["VALUE1"]);
            Value2 = Convert.ToDecimal(row["VALUE2"]);
            Equation1 = Convert.ToString(row["EQUATION1"]);
            Equation2 = Convert.ToString(row["EQUATION2"]);
            EquCompare = Convert.ToInt32(row["EQUCOMPARE"]);
        }

            
        public void Save()
        {
            if (this.Id == -1)
            {
                int newId;
                try
                {
                    newId = Convert.ToInt32(DatabaseSingleton.Instance.GetData("SELECT IFNULL(MAX(ID), 0) FROM EQUATIONFILTER").Rows[0][0]) + 1;
                } catch (Exception e)
                {
                    newId = 1;
                }

                this.Id = newId;
                List<SQLiteParameter> parameters = new List<SQLiteParameter>();

                parameters.Add(new SQLiteParameter("@p1", this.Id));
                parameters.Add(new SQLiteParameter("@p2", FilterName));
                parameters.Add(new SQLiteParameter("@p3", FilterType));
                parameters.Add(new SQLiteParameter("@p4", FilterOption));
                parameters.Add(new SQLiteParameter("@p5", Value1));
                parameters.Add(new SQLiteParameter("@p6", Value2));
                parameters.Add(new SQLiteParameter("@p7", Equation1));
                parameters.Add(new SQLiteParameter("@p8", Equation2));
                parameters.Add(new SQLiteParameter("@p9", EquCompare));

                DatabaseSingleton.Instance.ExecuteNonQuery("INSERT INTO EQUATIONFILTER (ID, FILTERNAME, FILTERTYPE, FILTEROPTION, VALUE1, VALUE2, EQUATION1, EQUATION2, EQUCOMPARE) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)", parameters.ToArray());

            }
            else
            {
                List<SQLiteParameter> parameters = new List<SQLiteParameter>();
                parameters.Add(new SQLiteParameter("@p1", this.Id));
                parameters.Add(new SQLiteParameter("@p2", FilterName));
                parameters.Add(new SQLiteParameter("@p3", FilterType));
                parameters.Add(new SQLiteParameter("@p4", FilterOption));
                parameters.Add(new SQLiteParameter("@p5", Value1));
                parameters.Add(new SQLiteParameter("@p6", Value2));
                parameters.Add(new SQLiteParameter("@p7", Equation1));
                parameters.Add(new SQLiteParameter("@p8", Equation2));
                parameters.Add(new SQLiteParameter("@p9", EquCompare));
                
                DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE EQUATIONFILTER SET FILTERNAME = @p2, FILTERTYPE = @p3, " +
                    "FILTEROPTION = @p4, VALUE1 = @p5, VALUE2 = @p6, EQUATION1 = @p7, EQUATION2 = @p8, EQUCOMPARE = @p9 WHERE ID = @p1", parameters.ToArray());
            }
        }

        public String getString()
        {
            String[] filterOptions = { " ", "Deciles", "Quintiles", "Quartiles", "Halves" };
            if (FilterType < 3)
            {
                String left = Equation1;
                
                if (FilterType == 0)
                {
                    String value = Value1.ToString();
                    if (FilterOption != 0)
                    {
                        value += " " + filterOptions[FilterOption];
                    }
                    return left + " > " + value;
                }
                else if (FilterType == 1)
                {
                    String value = Value1.ToString();
                    if (FilterOption != 0)
                    {
                        value += " " + filterOptions[FilterOption];
                    }
                    return left + " < " + value;
                }
                else 
                {
                    String value = Value1.ToString();
                    String value1 = Value2.ToString();
                    if (FilterOption != 0)
                    {
                        value += " " + filterOptions[FilterOption];
                        value1 += " " + filterOptions[FilterOption];
                    }
                    return value + " < " + left + " < " + value1;
                }
            }
            else
            {
                String left = Equation1;
                String right = Equation2;
                if (EquCompare == 0)
                {
                    return left + " > " + right;
                }
                else
                {
                    return left + " < " + right;
                }
            }
        }
        public static void Delete(int id)
        {
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("@p1", id));
            
            DatabaseSingleton.Instance.ExecuteNonQuery("UPDATE EQUATIONFILTER SET FILTEROPTION = -1 WHERE ID = @p1", parameters.ToArray());
        }

        public static List<EquationFilter> GetAllFilters()
        {
            List<EquationFilter> result = new List<EquationFilter>();
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT * FROM EQUATIONFILTER");

            foreach (DataRow row in data.Rows)
            {
                EquationFilter newFilter = new EquationFilter(row);
                if (newFilter.FilterOption >= 0)
                {
                    result.Add(newFilter);
                }
            }

            return result;
        }

        public static EquationFilter GetFilter(int id)
        {
            DataTable data = DatabaseSingleton.Instance.GetData("SELECT * FROM EQUATIONFILTER WHERE ID = " + id.ToString());
            if (data.Rows.Count == 0) return null;
            return new EquationFilter(data.Rows[0]);
        }
    }
}
