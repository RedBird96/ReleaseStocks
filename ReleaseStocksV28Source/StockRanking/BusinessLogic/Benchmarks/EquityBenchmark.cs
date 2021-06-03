using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking.Benchmarks
{
    public class EquityBenchmark : BenchmarkBase
    {
        public static String CODE = "^EQUITY";

        String logFileName = "logEquity";

        public EquityBenchmark()
        {
            initialized = true;

            DebugDataLogger.Instance.WriteLine(logFileName, "Date\tTotalValue");
        }

        public override void ResetBenchmark()
        {
            benchmarkResults = new Dictionary<int, decimal>();
            benchmarkResultsDaily = new Dictionary<int, decimal>();
        }

        public override void StartBenchmark(int date, decimal portfolioAmount)
        {
            base.StartBenchmark(date, portfolioAmount);

            buyPositions(date, 0);
        }

        protected override void buyPositions(int date, decimal currentStrategyPortfolioValue)
        {
            leftoverCash = currentStrategyPortfolioValue;
        }

        protected override void sellPositions(int date, decimal currentStrategyPortfolioValue)
        {
            leftoverCash = currentStrategyPortfolioValue;
        }

        protected override decimal calculateTotalValue(int date, decimal currentStrategyPortfolioValue, bool useLastClose = false)
        {
#if DEBUG
            DebugDataLogger.Instance.WriteLine(logFileName, date + "\t" +
                currentStrategyPortfolioValue.ToString("n2"));
#endif

            return currentStrategyPortfolioValue;
        }

    }
}
