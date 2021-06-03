using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking.Benchmarks
{
    public class BlendedBasicBenchmark : BenchmarkBase
    {
        public static String CODE = "^BLENDED_BASIC";

        decimal benchmarkPercent;
        decimal asset1Percent;

        int currentPositionsBenchmark = 0;
        int currentPositionsAsset1 = 0;

        CachedValuesStock asset1 = null;
        CachedValuesStock benchmarkSymbol = null;

        String logFileName = "logBlendedBasic";

        public BlendedBasicBenchmark(PortfolioParameters portfolio, List<CachedValuesStock> cachedStocks)
        {
            benchmarkPercent = portfolio.BABStocksPercent;
            asset1Percent = portfolio.BABBondsPercent + portfolio.BABCashPercent;

            asset1 = cachedStocks.Find(x => x.IdStock == portfolio.BABAsset1Id);
            benchmarkSymbol = cachedStocks.Find(x => x.IdStock == portfolio.IdSymbolBenchmark);

            if (asset1 == null || benchmarkSymbol == null)
                return;

            initialized = true;

            DebugDataLogger.Instance.WriteLine(logFileName, "Date\tPriceBenchmark\tPrice1\tQttyBenchmark\tQtty1\tAction\tLeftoverCash\tTotalValue");
        }

        public override void ResetBenchmark()
        {
            if (!initialized)
                return;

            benchmarkResults = new Dictionary<int, decimal>();
            benchmarkResultsDaily = new Dictionary<int, decimal>();

            asset1.LoadPrices();

            if (asset1.PricesCount == 0)
                initialized = false;

            currentPositionsBenchmark = 0;
            currentPositionsAsset1 = 0;
        }
        
        protected override void buyPositions(int date, decimal currentStrategyPortfolioValue)
        {
            var price1 = asset1.getPrice(date);
            var benchmarkSymbolPrice = benchmarkSymbol.getPrice(date);

            if (price1 != 0)
                currentPositionsAsset1 = (int)Math.Floor(leftoverCash * asset1Percent / 100m / price1);
            if (benchmarkSymbolPrice != 0)
                currentPositionsBenchmark = (int)Math.Floor(leftoverCash * benchmarkPercent / 100m / benchmarkSymbolPrice);

            leftoverCash -= price1 * currentPositionsAsset1;
            leftoverCash -= benchmarkSymbolPrice * currentPositionsBenchmark;

            lastBuyDate = date;

#if DEBUG
            DebugDataLogger.Instance.WriteLine(logFileName, date + "\t" +
                (benchmarkSymbolPrice).ToString("n2") + "\t" +
                (price1).ToString("n2") + "\t" +
                (currentPositionsBenchmark) + "\t" +
                (currentPositionsAsset1) + "\t" +
                "Buy" + "\t" +
                leftoverCash.ToString("n2") + "\t" +
                lastTotalValue.ToString("n2"));
#endif
        }

        protected override void sellPositions(int date, decimal currentStrategyPortfolioValue)
        {
            //recover cash from strategy
            leftoverCash = calculateTotalValue(date, currentStrategyPortfolioValue);

            currentPositionsAsset1 = 0;
            currentPositionsBenchmark = 0;
        }

        protected override decimal calculateTotalValue(int date, decimal currentStrategyPortfolioValue, bool useLastClose = false)
        {
            //recover cash from assets
            var price1 = asset1.getPrice(date);
            var benchmarkSymbolPrice = benchmarkSymbol.getPrice(date);

            decimal totalValue = leftoverCash;

            var dividendsBenchmark = benchmarkSymbol.getDividendSum(lastBuyDate, date);
            var dividends1 = asset1.getDividendSum(lastBuyDate, date);

            totalValue += (price1 + dividends1) * currentPositionsAsset1;
            totalValue += (benchmarkSymbolPrice + dividendsBenchmark) * currentPositionsBenchmark;

#if DEBUG
            DebugDataLogger.Instance.WriteLine(logFileName, date + "\t" +
                (benchmarkSymbolPrice+dividendsBenchmark).ToString("n2") + "\t" +
                (price1+dividends1).ToString("n2") + "\t" +
                (currentPositionsBenchmark) + "\t" +
                (currentPositionsAsset1) + "\t" +
                "Update/Sell" + "\t" +
                leftoverCash.ToString("n2") + "\t" +
                totalValue.ToString("n2"));
#endif            
            return totalValue;
        }
    }
}
