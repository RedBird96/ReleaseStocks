using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking.Benchmarks
{
    public abstract class BenchmarkBase
    {
        protected bool initialized = false;
        protected decimal leftoverCash = 0;
        protected decimal lastTotalValue = 0;
        protected int lastBuyDate = 0;
        protected DateTime startingDate;
        protected decimal managementFee = 0;
        protected int lastYearFee = 0;

        protected Dictionary<int, decimal> benchmarkResults = new Dictionary<int, decimal>();
        protected Dictionary<int, decimal> benchmarkResultsDaily = new Dictionary<int, decimal>();

        public decimal CurrentValue => lastTotalValue;
        public Dictionary<int, decimal> DailyValues => benchmarkResultsDaily;
        public decimal CurrentMonthlyValue => benchmarkResults.Count == 0 ? 0 : benchmarkResults.Last().Value;

        public abstract void ResetBenchmark();
        
        public virtual void StartBenchmark(int date, decimal portfolioAmount)
        {
            if (!initialized)
                return;

            startingDate = Utils.ConvertIntToDateTime(date);
            lastTotalValue = portfolioAmount;
            leftoverCash = portfolioAmount;
            lastYearFee = startingDate.Year;

            if (!benchmarkResults.ContainsKey(date))
                benchmarkResults.Add(Utils.ConvertDateTimeToInt(Utils.ConvertIntToDateTime(date).AddDays(-1)), lastTotalValue);
        }

        public void UpdateBenchmark(int date, decimal currentStrategyPortfolioValue)
        {
            if (!initialized)
                return;

            if (benchmarkResults.ContainsKey(Utils.ConvertDateTimeToInt(Utils.ConvertIntToDateTime(date).AddDays(-1))))
                return;

            lastTotalValue = calculateTotalValue(date, currentStrategyPortfolioValue);
            benchmarkResults.Add(Utils.ConvertDateTimeToInt(Utils.ConvertIntToDateTime(date).AddDays(-1)), lastTotalValue);
            if (!benchmarkResultsDaily.ContainsKey(date))
                benchmarkResultsDaily.Add(date, lastTotalValue);
        }
        
        public void UpdateBenchmarkDaily(int date, decimal currentStrategyPortfolioValue)
        {
            //used to store daily data
            if (!initialized)
                return;

            if (benchmarkResultsDaily.ContainsKey(date))
                return;

            var dailyValue = calculateTotalValue(date, currentStrategyPortfolioValue);
            lastTotalValue = dailyValue;
            benchmarkResultsDaily.Add(date, dailyValue);
        }
        
        public void RebalanceBenchmark(int date, decimal currentStrategyPortfolioValue)
        {
            if (!initialized)
                return;

            sellPositions(date, currentStrategyPortfolioValue);

            //discount annual fee
            if (managementFee > 0)
            {
                DateTime currentDate = Utils.ConvertIntToDateTime(date);
                if (lastYearFee != currentDate.Year && currentDate.Month == startingDate.Month && startingDate.Year != currentDate.Year && currentDate.Day == 1)
                {
                    leftoverCash -= leftoverCash * managementFee;
                    lastYearFee = currentDate.Year;
                }
            }

            lastTotalValue = leftoverCash;

            if (!benchmarkResults.ContainsKey(Utils.ConvertDateTimeToInt(Utils.ConvertIntToDateTime(date).AddDays(-1))))
                benchmarkResults.Add(Utils.ConvertDateTimeToInt(Utils.ConvertIntToDateTime(date).AddDays(-1)), lastTotalValue);
            
            buyPositions(date, currentStrategyPortfolioValue);

            UpdateBenchmarkDaily(date, currentStrategyPortfolioValue);
        }

        public void ExportLogData(String fileName)
        {
            if (!initialized)
                return;

            var file1 = "D_" + fileName;
            var file2 = "M_" + fileName;

            DebugDataLogger.Instance.WriteLine(file1, "Date\tValue");
            DebugDataLogger.Instance.WriteLine(file2, "Date\tValue");
            foreach (var key in benchmarkResults.Keys.OrderBy(x => x))
                DebugDataLogger.Instance.WriteLine(file2, key + "\t" + benchmarkResults[key].ToString("n2"));
            foreach (var key in benchmarkResultsDaily.Keys.OrderBy(x => x))
                DebugDataLogger.Instance.WriteLine(file1, key + "\t" + benchmarkResultsDaily[key].ToString("n2"));
        }

        protected abstract decimal calculateTotalValue(int date, decimal currentStrategyPortfolioValue, bool useLastClose = false);

        protected abstract void buyPositions(int date, decimal currentStrategyPortfolioValue);

        protected abstract void sellPositions(int date, decimal currentStrategyPortfolioValue);
    }
}
