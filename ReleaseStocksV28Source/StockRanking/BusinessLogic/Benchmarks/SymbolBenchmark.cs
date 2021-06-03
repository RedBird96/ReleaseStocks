using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking.Benchmarks
{
    public class SymbolBenchmark : BenchmarkBase
    {
        public static String SPY_CODE = "^SPY_BENCHMARK";
        public static String BENCHMARK_CODE = "^BENCHMARK_BENCHMARK";

        Int64 currentPositions = 0;
        CachedValuesStock stock = null;

        public String Symbol => initialized == false ? "" : stock.StockSymbol;

        String logFileName = "logBenchmark";
        bool DisableLogs = false;

        public SymbolBenchmark(CachedValuesStock stock, bool disableLogs)
        {
            this.stock = stock;
            this.DisableLogs = disableLogs;

            if (stock == null)
                return;

            initialized = true;

            if(!disableLogs)
                DebugDataLogger.Instance.WriteLine(logFileName + "_" + Symbol, "Date\tPrice\tQtty\tAction\tLeftoverCash\tTotalValue");
        }

        public override void ResetBenchmark()
        {
            if (!initialized)
                return;

            benchmarkResults = new Dictionary<int, decimal>();
            benchmarkResultsDaily = new Dictionary<int, decimal>();

            this.stock?.LoadPrices();

            currentPositions = 0;
        }

        public override void StartBenchmark(int date, decimal portfolioAmount)
        {
            base.StartBenchmark(date, portfolioAmount);

            buyPositions(date, 0);
        }
        
        protected override void buyPositions(int date, decimal currentStrategyPortfolioValue)
        {
            decimal price = stock == null ? 0 : stock.getPrice(date, 0, true);

            if (price > 0)
            {
                currentPositions = (Int64)Math.Floor(leftoverCash / price);
                leftoverCash -= currentPositions * price;
            }

            lastBuyDate = date;


#if DEBUG
            if (!DisableLogs)
                DebugDataLogger.Instance.WriteLine(logFileName + "_" + Symbol, date + "\t" +
                (price).ToString("n2") + "\t" +
                (currentPositions) + "\t" +
                "Buy" + "\t" +
                leftoverCash.ToString("n2") + "\t" +
                lastTotalValue.ToString("n2"));
#endif
        }

        protected override void sellPositions(int date, decimal currentStrategyPortfolioValue)
        {
            //recover cash from strategy
            leftoverCash = calculateTotalValue(date, currentStrategyPortfolioValue, true);
            
            currentPositions = 0;
        }

        protected override decimal calculateTotalValue(int date, decimal currentStrategyPortfolioValue, bool useLastClose = false)
        {
            //recover leftover cash
            decimal totalValue = leftoverCash;

            decimal price = 0;
            decimal dividends = 0;
            if (stock != null)
            {
                price = stock.getPrice(date, 0, useLastClose);

                dividends = stock.getDividendSum(lastBuyDate, date);
            }

            totalValue += (price + dividends) * currentPositions;


#if DEBUG
            if (useLastClose)
            {
                if (!DisableLogs)
                    DebugDataLogger.Instance.WriteLine(logFileName + "_" + Symbol, date + "\t" +
                    (price).ToString("n2") + "\t" +
                    (currentPositions) + "\t" +
                    "Sell" + "\t" +
                    leftoverCash.ToString("n2") + "\t" +
                    totalValue.ToString("n2"));
            }
#endif

            return totalValue;
        }

    }
}
