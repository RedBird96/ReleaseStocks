using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking.Benchmarks
{
    public class BondsModelBenchmark : BenchmarkBase
    {
        public static String CODE = "^BONDS";

        int momentumWindow;
        int movingAvg1;
        int movingAvg2;

        int[] currentPositionsAsset = new int[4];

        CachedValuesStock[] assets = new CachedValuesStock[4];

        String logFileName = "logBonds";
        public Trade LastSell = null;
        public int LastBuyPositionDate = 0;
        
        public BondsModelBenchmark(PortfolioParameters portfolio, List<CachedValuesStock> cachedStocks)
        {
            momentumWindow = portfolio.BondsMomentumWindow;
            movingAvg1 = portfolio.BondsMovingAvg1;
            movingAvg2 = portfolio.BondsMovingAvg2;

            assets[0] = cachedStocks.Find(x => x.IdStock == portfolio.BondsAsset1Id);
            assets[1] = cachedStocks.Find(x => x.IdStock == portfolio.BondsAsset2Id);
            assets[2] = cachedStocks.Find(x => x.IdStock == portfolio.BondsAsset3Id);
            assets[3] = cachedStocks.Find(x => x.IdStock == portfolio.BondsAsset4Id);

            if (assets.All(x => x == null))
                return;

            managementFee = portfolio.AnnualFee / 100;

            initialized = true;

            DebugDataLogger.Instance.WriteLine(logFileName, "Date\tPrice1\tPrice2\tPrice3\tPrice4\tQtty1\tQtty2\tQtty3\tQtty4\tAction\tLeftoverCash\tTotalValue");
        }

        public override void ResetBenchmark()
        {
            if (!initialized)
                return;
            
            benchmarkResults = new Dictionary<int, decimal>();
            benchmarkResultsDaily = new Dictionary<int, decimal>();

            assets[0]?.LoadPrices();
            assets[1]?.LoadPrices();
            assets[2]?.LoadPrices();
            assets[3]?.LoadPrices();

            currentPositionsAsset[0] = 0;
            currentPositionsAsset[1] = 0;
            currentPositionsAsset[2] = 0;
            currentPositionsAsset[3] = 0;
        }
        
        protected override void buyPositions(int date, decimal currentStrategyPortfolioValue)
        {
            decimal[] prices = new decimal[3];
            LastBuyPositionDate = date;

            for (int i = 0; i < prices.Length; i++)
            {
                prices[i] = assets[i] == null ? 0 : assets[i].getPrice(date);
            }
            var price4 = assets[3] == null ? 0 : assets[3].getPrice(date);

            decimal maxChange = decimal.MinValue;
            int currentMaxAsset = -1;
            for (int i = 0; i < prices.Length; i++)
            {
                if (prices[i] > 0)
                {
                    var assetChange = getChangeFromWindow(assets[i], date);
                    if (assetChange > maxChange)
                    {
                        maxChange = assetChange;
                        currentMaxAsset = i;
                    }
                }
            }

            if(currentMaxAsset >= 0)
            {
                var movAvg1 = getMovingAverage(assets[currentMaxAsset], this.movingAvg1, date);
                var movAvg2 = getMovingAverage(assets[currentMaxAsset], this.movingAvg2, date);

                if(maxChange > movAvg1 && maxChange > movAvg2)
                {
                    //buy asset
                    currentPositionsAsset[currentMaxAsset] = (int)Math.Floor(leftoverCash / prices[currentMaxAsset]);
                    leftoverCash -= currentPositionsAsset[currentMaxAsset] * prices[currentMaxAsset];


#if DEBUG
                    DebugDataLogger.Instance.WriteLine(logFileName, date + "\t" +
                        (prices[0]).ToString("n2") + "\t" +
                        (prices[1]).ToString("n2") + "\t" +
                        (prices[2]).ToString("n2") + "\t" +
                        (price4).ToString("n2") + "\t" +
                        currentPositionsAsset[0] + "\t" +
                        currentPositionsAsset[1] + "\t" +
                        currentPositionsAsset[2] + "\t" +
                        currentPositionsAsset[3] + "\t" +
                        "Buy" + "\t" +
                        leftoverCash.ToString("n2") + "\t" +
                        lastTotalValue.ToString("n2"));
#endif

                    return;
                }
            }

            //finally buy asset 4 if it exists
            if(price4 > 0)
            {
                currentPositionsAsset[3] = (int)Math.Floor(leftoverCash / price4);
                leftoverCash -= currentPositionsAsset[3] * price4;
            }

            lastBuyDate = date;

#if DEBUG
            DebugDataLogger.Instance.WriteLine(logFileName, date + "\t" +
                (prices[0]).ToString("n2") + "\t" +
                (prices[1]).ToString("n2") + "\t" +
                (prices[2]).ToString("n2") + "\t" +
                (price4).ToString("n2") + "\t" +
                currentPositionsAsset[0] + "\t" +
                currentPositionsAsset[1] + "\t" +
                currentPositionsAsset[2] + "\t" +
                currentPositionsAsset[3] + "\t" +
                "Buy" + "\t" +
                leftoverCash.ToString("n2") + "\t" +
                lastTotalValue.ToString("n2"));
#endif
        }

        protected override void sellPositions(int date, decimal currentStrategyPortfolioValue)
        {
            LastSell = null;
            Console.WriteLine("Sell Position = " + date.ToString());
            if(GetCurrentAssetId() != 0)
            {
                var asset = GetCurrentAsset();
                LastSell = new Trade()
                {
                    RealEntryDate = asset.getPriceDate(LastBuyPositionDate),
                    RealExitDate = asset.getPriceDate(date),
                    EntryDate = LastBuyPositionDate,
                    ExitDate = date,
                    EntryPrice = GetCurrentAsset().getPrice(lastBuyDate),
                    ExitPrice = GetCurrentAsset().getPrice(date),
                    IdStock = asset.IdStock,
                    Symbol = asset.StockSymbol
                };
            }

            //recover cash from strategy
            leftoverCash = calculateTotalValue(date, 0);

            currentPositionsAsset[0] = 0;
            currentPositionsAsset[1] = 0;
            currentPositionsAsset[2] = 0;
            currentPositionsAsset[3] = 0;
        }

        protected override decimal calculateTotalValue(int date, decimal currentStrategyPortfolioValue, bool useLastClose = false)
        {
            //recover leftover cash
            decimal totalValue = leftoverCash;
            decimal[] prices = new decimal[4];

            for(int i = 0; i < assets.Length; i++)
            {
                decimal dividends = 0;
                decimal price = 0;
                if (assets[i] != null)
                {
                    price = assets[i].getPrice(date);
                    prices[i] = price;
                    dividends = assets[i].getDividendSum(lastBuyDate, date);
                    prices[i] += dividends;
                }
                totalValue += (price + dividends) * currentPositionsAsset[i];
            }

#if DEBUG

            DebugDataLogger.Instance.WriteLine(logFileName, date + "\t" +
                (prices[0]).ToString("n2") + "\t" +
                (prices[1]).ToString("n2") + "\t" +
                (prices[2]).ToString("n2") + "\t" +
                (prices[3]).ToString("n2") + "\t" +
                currentPositionsAsset[0] + "\t" +
                currentPositionsAsset[1] + "\t" +
                currentPositionsAsset[2] + "\t" +
                currentPositionsAsset[3] + "\t" +
                "Update/Sell" + "\t" +
                leftoverCash.ToString("n2") + "\t" +
                totalValue.ToString("n2"));
#endif

            return totalValue;
        }

        decimal getChangeFromWindow(CachedValuesStock asset, int date)
        {
            //(t - (t - 3)) / (t - 3)
            decimal prevValue = asset.getPrice(date, -momentumWindow);
            decimal currentValue = asset.getPrice(date);

            if (prevValue == 0)
                return -10000;

            return (currentValue - prevValue) / prevValue;
        }

        decimal getMovingAverage(CachedValuesStock asset, int range, int date)
        {
            decimal sum = 0;
            int count = 1;
            decimal currPrice = asset.getPrice(date);
            for (int i = 0; i < range; i++)
            {
                decimal prevPrice = asset.getPrice(date, -(i+1));

                if (prevPrice != 0 && currPrice != 0)
                {
                    count++;
                    sum += (currPrice - prevPrice) / prevPrice;
                }
                currPrice = prevPrice;
            }

            return sum / count;
        }

        public int GetCurrentAssetId()
        {
            for (int i = 0; i < currentPositionsAsset.Length; i++)
            {
                if (currentPositionsAsset[i] > 0)
                    return assets[i].IdStock;
            }

            return 0;
        }

        public CachedValuesStock GetCurrentAsset()
        {
            for (int i = 0; i < currentPositionsAsset.Length; i++)
            {
                if (currentPositionsAsset[i] > 0)
                    return assets[i];
            }

            return null;
        }

        public String GetCurrentAssetSymbol()
        {
            for (int i = 0; i < currentPositionsAsset.Length; i++)
            {
                if (currentPositionsAsset[i] > 0)
                    return assets[i].StockSymbol;
            }

            return "";
        }

    }
}
