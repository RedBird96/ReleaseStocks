using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class RankingResult
    {
        public DateTime Date;
        public int IdStock;
        public decimal RankPosition;
        public decimal ForwardGain = decimal.MinValue;
        public decimal TotalWeight;

        public RankingResult(DateTime date, int idStock, decimal weight)
        {
            this.Date = date;
            this.IdStock = idStock;
            this.TotalWeight = weight;
        }


        public static void RankRankingResults(List<RankingResult> rankResults)
        {
            rankResults.Sort(delegate (RankingResult x, RankingResult y)
            {
                return x.TotalWeight.CompareTo(y.TotalWeight);
            });

            int totalRanks = rankResults.Count;
            decimal lastRankValue = decimal.MinValue;
            int lastCurrentRank = 0;
            for (int currentRank = 0; currentRank < totalRanks; currentRank++)
            {
                if (lastRankValue != rankResults[currentRank].TotalWeight)
                {
                    lastRankValue = rankResults[currentRank].TotalWeight;
                    lastCurrentRank = currentRank;
                }

                rankResults[currentRank].RankPosition = (decimal)lastCurrentRank / (decimal)totalRanks;
            }

        }

    }
}
