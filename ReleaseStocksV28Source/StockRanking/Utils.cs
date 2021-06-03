using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public static class Utils
    {
        public static List<String> QuarterDates = new List<String>() { "0101", "0701", "0401", "1001" };
        public static List<String> QuarterEndDates = new List<String>() { "1231", "0630", "0331", "0930" };

        public static int MinProcessDate = 20010101;

        public static DateTime ConvertIntToDateTime(int date)
        {
            String tempDate = date.ToString();

            if (tempDate == "0")
                return DateTime.MinValue;

            int year = Convert.ToInt32(tempDate.Substring(0, 4));
            int month = Convert.ToInt32(tempDate.Substring(4, 2));
            int day = Convert.ToInt32(tempDate.Substring(6));

            DateTime result = new DateTime(year, month, day);
            return result;
        }

        public static int AddDays(int date, int days)
        {
            return Utils.ConvertDateTimeToInt(Utils.ConvertIntToDateTime(date).AddDays(days));
        }

        public static int AddMonths(int date, int months)
        {
            return Utils.ConvertDateTimeToInt(Utils.ConvertIntToDateTime(date).AddMonths(months));
        }

        public static int GetLastValidDate()
        {
            DateTime date = DateTime.Now.AddDays(-1);
            if (date.DayOfWeek == DayOfWeek.Sunday)
                date = date.AddDays(-2);
            if (date.DayOfWeek == DayOfWeek.Saturday)
                date = date.AddDays(-1);

            return Convert.ToInt32(date.ToString("yyyyMMdd"));
        }

        public static String ConvertDateIntToString(int date)
        {
            return date.ToString().Substring(4, 2) + "/" + date.ToString().Substring(6, 2) + "/" + date.ToString().Substring(0, 4);
        }

        public static int ConvertStringDateToInt(String date)
        {
            return Convert.ToInt32(date.Split('/')[2] + date.Split('/')[1] + date.Split('/')[0]);
        }

        public static double ConvertToDouble(String number)
        {
            try
            {
                return Convert.ToDouble(number);
            }
            catch(Exception)
            {
                return 0;
            }
        }

        public static int GetCorrespondingQuarter(int date)
        {
            DateTime estDate = new DateTime(Convert.ToInt32(date.ToString().Substring(0, 4)), Convert.ToInt32(date.ToString().Substring(4, 2)), Convert.ToInt32(date.ToString().Substring(6, 2)));

            while(true)
            {
                String dateToCompare = estDate.ToString("MMdd");
                foreach(String dateCompared in QuarterEndDates)
                {
                    if(dateCompared == dateToCompare)
                    {
                        return Convert.ToInt32(estDate.ToString("yyyyMMdd"));
                    }
                }

                estDate = estDate.AddDays(-1);
            }

        }

        public static DateTime GetFirstQuarterDay(DateTime date)
        {
            if (date.Month < 4)
                return new DateTime(date.Year, 1, 1);

            if (date.Month < 7)
                return new DateTime(date.Year, 4, 1);

            if (date.Month < 10)
                return new DateTime(date.Year, 7, 1);

            return new DateTime(date.Year, 10, 1);
        }


        public static int ConvertDateTimeToInt(DateTime date)
        {
            return Convert.ToInt32(date.ToString("yyyyMMdd"));
        }

        public static String AddCastFromDB(String field)
        {
            return "cast(IFNULL(" + field + ", 0)as double) " + field;
        }


        public static String AddCastFromDBWIthNull(String field)
        {
            return "cast(" + field + " as double) " + field;
        }

        public static decimal Covariance(this IEnumerable<decimal> source, IEnumerable<decimal> other)
        {
            int len = source.Count();

            decimal avgSource = source.Average();
            decimal avgOther = other.Average();
            decimal covariance = 0;

            for (int i = 0; i < len; i++)
                covariance += (source.ElementAt(i) - avgSource) * (other.ElementAt(i) - avgOther);

            return covariance / len;
        }

        public static decimal Variance(this IEnumerable<decimal> source)
        {
            if (source.Count() == 0)
                return 0;

            int n = 0;
            decimal mean = 0;
            decimal M2 = 0;

            foreach (decimal x in source)
            {
                n = n + 1;
                decimal delta = x - mean;
                mean = mean + delta / n;
                M2 += delta * (x - mean);
            }
            return M2 / (n - 1);
        }

        public static bool ConvertDBIntToBoolean(object value)
        {
            return Convert.ToInt32(value) == 1 ? true : false;
        }
        
        public static int ConvertBooleanToDBInt(object value)
        {
            return Convert.ToBoolean(value) ? 1 : 0;
        }
        
        public static String FormatDateTimeToString(DateTime date)
        {
            return date.ToString("MM/dd/yyyy");
        }

    }
}
