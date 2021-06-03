using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace StockRanking.Benchmarks
{
    public class BlendedAdvancedBenchmark : BenchmarkBase
    {
        public static String CODE = "^BLENDED_ADV";

        CachedValuesStock asset1 = null;
        CachedValuesStock asset2 = null;
        CachedValuesStock asset3 = null;
        CachedValuesStock asset4 = null;
        Dictionary<int, bool> customRiskFileSignals = null;
        String riskFile = "";

        decimal stockPercent;
        decimal bondsPercent;
        decimal cashPercent;

        decimal lastStrategyPortofolioValue = 0;  //used to estimate strategy gains
        decimal currentAmountInStrategy = 0;
        int currentPositionsAsset1 = 0;
        int currentPositionsAsset2 = 0;
        int currentPositionsAsset3 = 0;
        int currentPositionsAsset4 = 0;

        String logFileName = "logBlendedAdv";

        public BlendedAdvancedBenchmark(PortfolioParameters portfolio, List<CachedValuesStock> cachedValuesStocks)
        {
            stockPercent = portfolio.BABStocksPercent;
            bondsPercent = portfolio.BABBondsPercent;
            cashPercent = portfolio.BABCashPercent;
            
            asset1 = cachedValuesStocks.Find(x => x.IdStock == portfolio.BABAsset1Id);
            asset2 = cachedValuesStocks.Find(x => x.IdStock == portfolio.BABAsset2Id);
            asset3 = cachedValuesStocks.Find(x => x.IdStock == portfolio.BABAsset3Id);
            asset4 = cachedValuesStocks.Find(x => x.IdStock == portfolio.BABAsset4Id);
            riskFile = portfolio.BABCustomRiskFile;
            loadRiskFile();

            if (asset1 == null || asset2 == null || asset3 == null)
                return;

            managementFee = (portfolio.AnnualFee / 100m) * (1m - (stockPercent / 100m));

            initialized = true;

            DebugDataLogger.Instance.WriteLine(logFileName, "Date\tCurrentPortfolio\tPrice1\tPrice2\tPrice3\tPrice4\tQtty1\tQtty2\tQtty3\tQtty4\tAction\tRiskSignal\tLeftoverCash\tTotalValue");
        }
        
        public override void ResetBenchmark()
        {
            if (!initialized)
                return;

            benchmarkResults = new Dictionary<int, decimal>();
            benchmarkResultsDaily = new Dictionary<int, decimal>();

            asset1.LoadPrices();
            asset2.LoadPrices();
            asset3.LoadPrices();
            asset4?.LoadPrices();

            if (asset1.PricesCount == 0)
                initialized = false;
            if (asset2.PricesCount == 0)
                initialized = false;
            if (asset3.PricesCount == 0)
                initialized = false;

            currentPositionsAsset1 = 0;
            currentPositionsAsset2 = 0;
            currentPositionsAsset3 = 0;
            currentPositionsAsset4 = 0;
        }

        protected override void buyPositions(int date, decimal currentStrategyPortfolioValue)
        {
            currentAmountInStrategy = leftoverCash * stockPercent / 100m;

            lastStrategyPortofolioValue = currentStrategyPortfolioValue;

            var customRiskFile = false;
            decimal price1 = 0;
            decimal price2 = 0;
            decimal price3 = 0;
            if (getRiskSignal(Utils.ConvertIntToDateTime(date)))
            {
                price1 = asset1.getPrice(date, 0, true);
                price2 = asset2.getPrice(date, 0, true);

                if (price1 != 0)
                    currentPositionsAsset1 = (int)Math.Floor(leftoverCash * bondsPercent / 100m / 2m / price1);
                if (price2 != 0)
                    currentPositionsAsset2 = (int)Math.Floor(leftoverCash * bondsPercent / 100m / 2m / price2);

                if (currentPositionsAsset1 < 0)
                    currentPositionsAsset1 = 0;
                if (currentPositionsAsset2 < 0)
                    currentPositionsAsset2 = 0;
            }
            else
            {
                price2 = asset2.getPrice(date, 0, true);
                price3 = asset3.getPrice(date, 0, true);

                if (price2 != 0)
                    currentPositionsAsset2 = (int)Math.Floor(leftoverCash * bondsPercent / 100m / 2m / price2);
                if (price3 != 0)
                    currentPositionsAsset3 = (int)Math.Floor(leftoverCash * bondsPercent / 100m / 2m / price3);

                if (currentPositionsAsset2 < 0)
                    currentPositionsAsset2 = 0;
                if (currentPositionsAsset3 < 0)
                    currentPositionsAsset3 = 0;
            }

            //invest finally in cash asset
            decimal price4 = 0;
            if (asset4 != null)
            {
                price4 = asset4.getPrice(date, 0, true);
                if (price4 != 0)
                {
                    currentPositionsAsset4 = (int)Math.Floor(leftoverCash * cashPercent / 100m / price4);
                    leftoverCash -= price4 * currentPositionsAsset4;
                }
            }

            leftoverCash -= currentAmountInStrategy;
            leftoverCash -= price1 * currentPositionsAsset1;
            leftoverCash -= price2 * currentPositionsAsset2;
            leftoverCash -= price3 * currentPositionsAsset3;

            lastBuyDate = date;

#if DEBUG
            DebugDataLogger.Instance.WriteLine(logFileName, date + "\t" +
                currentStrategyPortfolioValue + "\t" +
                (price1).ToString("n2") + "\t" +
                (price2).ToString("n2") + "\t" +
                (price3).ToString("n2") + "\t" +
                (price4).ToString("n2") + "\t" +
                currentPositionsAsset1 + "\t" +
                currentPositionsAsset2 + "\t" +
                currentPositionsAsset3 + "\t" +
                currentPositionsAsset4 + "\t" +
                "Buy" + "\t" +
                getRiskSignal(Utils.ConvertIntToDateTime(date)) + "\t" +
                leftoverCash.ToString("n2") + "\t" +
                lastTotalValue.ToString("n2"));
#endif
        }

        protected override void sellPositions(int date, decimal currentStrategyPortfolioValue)
        {
            //recover cash from strategy
            leftoverCash = calculateTotalValue(date, currentStrategyPortfolioValue, true);

            currentPositionsAsset1 = 0;
            currentPositionsAsset2 = 0;
            currentPositionsAsset3 = 0;
            currentPositionsAsset4 = 0;
            currentAmountInStrategy = 0;
        }

        protected override decimal calculateTotalValue(int date, decimal currentStrategyPortfolioValue, bool useLastClose = false)
        {
            //recover cash from strategy
            decimal totalValue = leftoverCash;
            if (currentAmountInStrategy != 0 && lastStrategyPortofolioValue != 0)
                totalValue += currentStrategyPortfolioValue / lastStrategyPortofolioValue * currentAmountInStrategy;

            //recover cash from assets
            var price1 = asset1.getPrice(date, 0, useLastClose);
            var price2 = asset2.getPrice(date, 0, useLastClose);
            var price3 = asset3.getPrice(date, 0, useLastClose);

            var dividends1 = asset1.getDividendSum(lastBuyDate, date);
            var dividends2 = asset2.getDividendSum(lastBuyDate, date);
            var dividends3 = asset3.getDividendSum(lastBuyDate, date);

            decimal price4 = 0;
            decimal dividends4 = 0;

            if (currentPositionsAsset4 > 0)
            {
                price4 = asset4.getPrice(date, 0, useLastClose);
                dividends4 = asset4.getDividendSum(lastBuyDate, date);
            }

            totalValue += (price1 + dividends1) * currentPositionsAsset1;
            totalValue += (price2 + dividends2) * currentPositionsAsset2;
            totalValue += (price3 + dividends3) * currentPositionsAsset3;
            totalValue += (price4 + dividends4) * currentPositionsAsset4;
#if DEBUG

            DebugDataLogger.Instance.WriteLine(logFileName, date + "\t" +
                currentStrategyPortfolioValue + "\t" +
                (price1 + dividends1).ToString("n2") + "\t" +
                (price2 + dividends2).ToString("n2") + "\t" +
                (price3 + dividends3).ToString("n2") + "\t" +
                (price4 + dividends4).ToString("n2") + "\t" +
                currentPositionsAsset1 + "\t" +
                currentPositionsAsset2 + "\t" +
                currentPositionsAsset3 + "\t" +
                currentPositionsAsset4 + "\t" +
                "Update/Sell" + "\t" +
                "-" + "\t" +
                leftoverCash.ToString("n2") + "\t" +
                totalValue.ToString("n2"));
#endif
            return totalValue;
        }

        void loadRiskFile()
        {
            if (riskFile.Trim() == "")
                return;

            if (!File.Exists(riskFile))
                return;

            customRiskFileSignals = new Dictionary<int, bool>();

            try
            {
                TextFieldParser textParser = null;
                textParser = new TextFieldParser(riskFile);
                textParser.SetDelimiters(new string[] { "," });
                textParser.HasFieldsEnclosedInQuotes = false;

                while (!textParser.EndOfData)
                {
                    String[] data = textParser.ReadFields();

                    customRiskFileSignals.Add(Convert.ToInt32(data[0]), data[1].Trim() == "1");
                }
            }
            catch (Exception e)
            {
                customRiskFileSignals = null;
            }
        }

        bool getRiskSignal(DateTime date)
        {
            try
            {
                if (customRiskFileSignals == null)
                    return false;

                //go backwards in time to get first signal
                DateTime askDate = date;
                DateTime limitDate = askDate.AddMonths(-4);
                while (askDate > limitDate)
                {
                    if (customRiskFileSignals.ContainsKey(Utils.ConvertDateTimeToInt(askDate)))
                        return customRiskFileSignals[Utils.ConvertDateTimeToInt(askDate)];

                    askDate = askDate.AddDays(-1);
                }

                return false;
            }
            catch (Exception e)
            {
            }

            return false;
        }


    }
}
