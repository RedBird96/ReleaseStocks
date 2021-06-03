using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class FilterScore
    {
        public int IdStock = 0;
        
        public decimal RoicScore;
        public decimal CroicScore;
        public decimal FcfScore;
        public decimal EbitdaScore;
        public decimal SalesScore;
        public decimal RoicScoreAux;
        public decimal CroicScoreAux;
        public decimal FcfScoreAux;
        public decimal EbitdaScoreAux;
        public decimal SalesScoreAux;

        public int Date { get; set; }
        

        public FilterScore()
        {

        }


        public FilterScore(DataRow row)
        {
            Date = Convert.ToInt32(row[0]);
            IdStock = Convert.ToInt32(row[1]);

            RoicScore = decimal.MinValue;
            CroicScore = decimal.MinValue;
            FcfScore = decimal.MinValue;
            EbitdaScore = decimal.MinValue;
            SalesScore = decimal.MinValue;

            RoicScoreAux = Convert.ToDecimal(row[2]);
            CroicScoreAux = Convert.ToDecimal(row[3]);
            FcfScoreAux = Convert.ToDecimal(row[4]);
            EbitdaScoreAux = Convert.ToDecimal(row[5]);
            SalesScoreAux = Convert.ToDecimal(row[6]);
        }
    }
}
