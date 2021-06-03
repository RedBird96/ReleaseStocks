using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking.Benchmarks
{
    public class EquityBondsModelBenchmark : BenchmarkBase
    {
        public static String CODE = "^EQUITY_BONDS";

        decimal stockPercent;
        decimal bondsPercent;

        decimal lastStrategyPortofolioValue = 0;  //used to estimate strategy gains
        decimal currentAmountInStrategy = 0;
        decimal lastBondsPortofolioValue = 0;  //used to estimate bonds gains
        decimal currentAmountInBonds = 0;

        CachedValuesStock asset1 = null;
        CachedValuesStock benchmarkSymbol = null;

        BondsModelBenchmark bondsModel;

        String logFileName = "logEquityBonds";

        public EquityBondsModelBenchmark(PortfolioParameters portfolio, List<CachedValuesStock> cachedStocks, BondsModelBenchmark bondsModel)
        {
            stockPercent = portfolio.BABStocksPercent;
            bondsPercent = portfolio.BABBondsPercent + portfolio.BABCashPercent;
            this.bondsModel = bondsModel;

            initialized = true;
        }

        public override void ResetBenchmark()
        {
            if (!initialized)
                return;

            benchmarkResults = new Dictionary<int, decimal>();
            benchmarkResultsDaily = new Dictionary<int, decimal>();
        }

        protected override void buyPositions(int date, decimal currentStrategyPortfolioValue)
        {
            currentAmountInStrategy = leftoverCash * stockPercent / 100m;
            currentAmountInBonds = leftoverCash - currentAmountInStrategy;

            lastStrategyPortofolioValue = currentStrategyPortfolioValue;
            lastBondsPortofolioValue = bondsModel.CurrentValue;

            leftoverCash = 0;

            lastBuyDate = date;

        }

        protected override void sellPositions(int date, decimal currentStrategyPortfolioValue)
        {
            //recover cash from strategy
            leftoverCash = calculateTotalValue(date, currentStrategyPortfolioValue);
            currentAmountInStrategy = 0;
            currentAmountInBonds = 0;
        }

        protected override decimal calculateTotalValue(int date, decimal currentStrategyPortfolioValue, bool useLastClose = false)
        {
            //recover cash from strategy
            decimal totalValue = leftoverCash;
            if (currentAmountInStrategy != 0 && lastStrategyPortofolioValue != 0)
                totalValue += currentStrategyPortfolioValue / lastStrategyPortofolioValue * currentAmountInStrategy;
            if (currentAmountInBonds != 0 && lastBondsPortofolioValue != 0)
                totalValue += bondsModel.CurrentValue / lastBondsPortofolioValue * currentAmountInBonds;
            
            return totalValue;
        }
        
    }
}
