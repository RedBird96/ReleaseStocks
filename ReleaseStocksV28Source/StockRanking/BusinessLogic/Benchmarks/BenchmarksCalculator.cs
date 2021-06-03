using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking.Benchmarks
{
    public class BenchmarksCalculator
    {
        PortfolioParameters portfolioParameters;
        BlendedAdvancedBenchmark blendedAdvancedBenchmark;
        BlendedBasicBenchmark blendedBasicBenchmark;
        BondsModelBenchmark bondsModelBenchmark;
        EquityBondsModelBenchmark equityBondsModelBenchmark;
        EquityBenchmark equityBenchmark;
        SymbolBenchmark spySymbolBenchmark;
        SymbolBenchmark benchmarkSymbolBenchmark;
        BlendedConfigurableBenchmark blendedConfigurableBenchmark;
        List<SymbolBenchmark> benchmarkSymbolsList;
        CachedValuesStock spyStock = null;

        public BondsModelBenchmark BondsModelBenchmark => bondsModelBenchmark;

        public BenchmarksCalculator(PortfolioParameters portfolio)
        {
            this.portfolioParameters = portfolio;
            var fileBasedStocks = FileBasedStock.GetAllFileBasedStocks();
            var etfStocks = Stock.GetAllETFStocks();
            var defaultEtfStocks = Stock.GetDefaultETFStocks();
            var allStocks = new List<CachedValuesStock>();
            allStocks.AddRange(fileBasedStocks);
            allStocks.AddRange(etfStocks);
            allStocks.AddRange(defaultEtfStocks);

            blendedAdvancedBenchmark = new BlendedAdvancedBenchmark(portfolio, allStocks);
            blendedBasicBenchmark = new BlendedBasicBenchmark(portfolio, allStocks);
            bondsModelBenchmark = new BondsModelBenchmark(portfolio, allStocks);
            spySymbolBenchmark = new SymbolBenchmark(allStocks.Find(x => x.IdStock == -1), true);
            benchmarkSymbolBenchmark = new SymbolBenchmark(allStocks.Find(x => x.IdStock == portfolio.IdSymbolBenchmark), false);
            benchmarkSymbolsList = new List<SymbolBenchmark>();
            equityBenchmark = new EquityBenchmark();
            blendedConfigurableBenchmark = new BlendedConfigurableBenchmark(portfolio, allStocks);
            equityBondsModelBenchmark = new EquityBondsModelBenchmark(portfolio, allStocks, bondsModelBenchmark);

            var performanceBenchmarks = PortfolioParameters.GetPerformanceGraphBenchmarkSymbols();
            spyStock = allStocks.Find(x => x.IdStock == -1);
            String spySymbol = spyStock.StockSymbol;
            String portfolioSymbol = allStocks.Find(x => x.IdStock == portfolio.IdSymbolBenchmark).StockSymbol;
            foreach (var symbol in performanceBenchmarks)
            {
                if (symbol == spySymbol || symbol == portfolioSymbol)
                    continue;

                benchmarkSymbolsList.Add(new SymbolBenchmark(allStocks.Find(x => x.StockSymbol == symbol), true));
            }

            var otherStocksToAdd = new List<CachedValuesStock>();
            otherStocksToAdd.Add(allStocks.FirstOrDefault(x => x.IdStock == portfolio.BABAsset1Id));
            otherStocksToAdd.Add(allStocks.FirstOrDefault(x => x.IdStock == portfolio.BABAsset2Id));
            otherStocksToAdd.Add(allStocks.FirstOrDefault(x => x.IdStock == portfolio.BABAsset3Id));
            otherStocksToAdd.Add(allStocks.FirstOrDefault(x => x.IdStock == portfolio.BABAsset4Id));
            otherStocksToAdd.Add(allStocks.FirstOrDefault(x => x.IdStock == portfolio.BondsAsset1Id));
            otherStocksToAdd.Add(allStocks.FirstOrDefault(x => x.IdStock == portfolio.BondsAsset2Id));
            otherStocksToAdd.Add(allStocks.FirstOrDefault(x => x.IdStock == portfolio.BondsAsset3Id));
            otherStocksToAdd.Add(allStocks.FirstOrDefault(x => x.IdStock == portfolio.BondsAsset4Id));
            otherStocksToAdd.Add(allStocks.FirstOrDefault(x => x.IdStock == portfolio.GetBondsRiskModelAssetId()));

            foreach (var stock in otherStocksToAdd.Distinct())
            {
                if (stock == null)
                    continue;

                if (performanceBenchmarks.Contains(stock.StockSymbol))
                    continue;

                if (stock.StockSymbol == spySymbol || stock.StockSymbol == portfolioSymbol)
                    continue;

                benchmarkSymbolsList.Add(new SymbolBenchmark(stock, true));
            }
        }

        public int GetRealDateByInt(int dateInt)
        {
            try
            {
                return spyStock.getPriceDate(dateInt);
            }
            catch (Exception e)
            {
                return dateInt;
            }
        }

        public int GetRealDate(DateTime date)
        {
            try
            {
                return spyStock.getPriceDate(Utils.ConvertDateTimeToInt(date));
            }
            catch(Exception e)
            {
                return Utils.ConvertDateTimeToInt(date);
            }
        }

        public int GetRealDate(int date)
        {
            try
            {
                return spyStock.getPriceDate(date);
            }
            catch (Exception e)
            {
                return date;
            }
        }

        public void ResetBenchmarks()
        {
            blendedAdvancedBenchmark.ResetBenchmark();
            blendedBasicBenchmark.ResetBenchmark();
            blendedConfigurableBenchmark.ResetBenchmark();
            bondsModelBenchmark.ResetBenchmark();
            spySymbolBenchmark.ResetBenchmark();
            benchmarkSymbolBenchmark.ResetBenchmark();
            equityBenchmark.ResetBenchmark();
            equityBondsModelBenchmark.ResetBenchmark();
            foreach (var benchmark in benchmarkSymbolsList)
                benchmark.ResetBenchmark();
        }

        public void StartBenchmarks(int date, decimal portfolioAmount)
        {
            blendedAdvancedBenchmark.StartBenchmark(date, portfolioAmount);
            blendedBasicBenchmark.StartBenchmark(date, portfolioAmount);
            blendedConfigurableBenchmark.StartBenchmark(date, portfolioAmount);
            bondsModelBenchmark.StartBenchmark(date, portfolioAmount);
            spySymbolBenchmark.StartBenchmark(date, portfolioAmount);
            benchmarkSymbolBenchmark.StartBenchmark(date, portfolioAmount);
            equityBenchmark.StartBenchmark(date, portfolioAmount);
            equityBondsModelBenchmark.StartBenchmark(date, portfolioAmount);
            foreach (var benchmark in benchmarkSymbolsList)
                benchmark.StartBenchmark(date, portfolioAmount);
        }

        public void UpdateBenchmarks(int date, decimal portfolioAmount)
        {
            blendedAdvancedBenchmark.UpdateBenchmark(date, portfolioAmount);
            blendedBasicBenchmark.UpdateBenchmark(date, portfolioAmount);
            blendedConfigurableBenchmark.UpdateBenchmark(date, portfolioAmount);
            bondsModelBenchmark.UpdateBenchmark(date, portfolioAmount);
            spySymbolBenchmark.UpdateBenchmark(date, portfolioAmount);
            equityBenchmark.UpdateBenchmark(date, portfolioAmount);
            benchmarkSymbolBenchmark.UpdateBenchmark(date, portfolioAmount);
            equityBondsModelBenchmark.UpdateBenchmark(date, portfolioAmount);
            foreach (var benchmark in benchmarkSymbolsList)
                benchmark.UpdateBenchmark(date, portfolioAmount);
        }

        public void UpdateBenchmarksDaily(int date, decimal portfolioAmount)
        {
            blendedAdvancedBenchmark.UpdateBenchmarkDaily(date, portfolioAmount);
            blendedBasicBenchmark.UpdateBenchmarkDaily(date, portfolioAmount);
            blendedConfigurableBenchmark.UpdateBenchmarkDaily(date, portfolioAmount);
            bondsModelBenchmark.UpdateBenchmarkDaily(date, portfolioAmount);
            equityBenchmark.UpdateBenchmarkDaily(date, portfolioAmount);
            spySymbolBenchmark.UpdateBenchmarkDaily(date, portfolioAmount);
            benchmarkSymbolBenchmark.UpdateBenchmarkDaily(date, portfolioAmount);
            equityBondsModelBenchmark.UpdateBenchmarkDaily(date, portfolioAmount);
            foreach (var benchmark in benchmarkSymbolsList)
                benchmark.UpdateBenchmarkDaily(date, portfolioAmount);
        }

        public void RebalanceBenchmarks(int date, decimal portfolioAmount)
        {
            blendedAdvancedBenchmark.RebalanceBenchmark(date, portfolioAmount);
            blendedBasicBenchmark.RebalanceBenchmark(date, portfolioAmount);
            blendedConfigurableBenchmark.RebalanceBenchmark(date, portfolioAmount);
            bondsModelBenchmark.RebalanceBenchmark(date, portfolioAmount);
            equityBenchmark.RebalanceBenchmark(date, portfolioAmount);
            spySymbolBenchmark.RebalanceBenchmark(date, portfolioAmount);
            benchmarkSymbolBenchmark.RebalanceBenchmark(date, portfolioAmount);
            equityBondsModelBenchmark.RebalanceBenchmark(date, portfolioAmount);
            foreach (var benchmark in benchmarkSymbolsList)
                benchmark.RebalanceBenchmark(date, portfolioAmount);
        }

        public decimal GetBlendedAdvancedValue(bool monthly)
        {
            if (!monthly)
                return blendedAdvancedBenchmark.CurrentValue;
            else
                return blendedAdvancedBenchmark.CurrentMonthlyValue;
        }

        public decimal GetBlendedBasicValue(bool monthly)
        {
            if (!monthly)
                return blendedBasicBenchmark.CurrentValue;
            else
                return blendedBasicBenchmark.CurrentMonthlyValue;
        }
        
        public decimal GetBondsValue(bool monthly)
        {
            if (!monthly)
                return bondsModelBenchmark.CurrentValue;
            else
                return bondsModelBenchmark.CurrentMonthlyValue;
        }

        public decimal GetBenchmarkSymbolValue(bool monthly)
        {
            if (!monthly)
                return benchmarkSymbolBenchmark.CurrentValue;
            else
                return benchmarkSymbolBenchmark.CurrentMonthlyValue;
        }

        public decimal GetSPYValue(bool monthly)
        {
            if (!monthly)
                return spySymbolBenchmark.CurrentValue;
            else
                return spySymbolBenchmark.CurrentMonthlyValue;
        }

        public decimal GetBlendedConfigurableBenchmarkValues(bool monthly)
        {
            if (!monthly)
                return blendedConfigurableBenchmark.CurrentValue;
            else
                return blendedConfigurableBenchmark.CurrentMonthlyValue;
        }

        public decimal GetEquityBondsModelBenchmarkValues(bool monthly)
        {
            if (!monthly)
                return equityBondsModelBenchmark.CurrentValue;
            else
                return equityBondsModelBenchmark.CurrentMonthlyValue;
        }

        public void ExportDebugLogs()
        {
#if DEBUG
            blendedAdvancedBenchmark.ExportLogData("BlendedA");
            blendedBasicBenchmark.ExportLogData("BlendedB");
            blendedConfigurableBenchmark.ExportLogData("BlendedC");
            bondsModelBenchmark.ExportLogData("Bonds");
            equityBenchmark.ExportLogData("Equity");
            spySymbolBenchmark.ExportLogData("SPY");
            benchmarkSymbolBenchmark.ExportLogData("Benchmark");
            equityBondsModelBenchmark.ExportLogData("EquityBonds");
            foreach (var benchmark in benchmarkSymbolsList)
                benchmark.ExportLogData("s" + benchmark.Symbol);
#endif
        }

        public Dictionary<String, decimal> GetBenchmarkListValues(bool monthly)
        {
            var results = new Dictionary<string, decimal>();
            foreach (var benchmark in benchmarkSymbolsList)
                if(benchmark.Symbol != "")
                    results.Add(benchmark.Symbol, monthly ? benchmark.CurrentMonthlyValue : benchmark.CurrentValue);

            return results;
        }

        BenchmarkBase GetBenchmark(String benchmarkCode)
        {
            if (benchmarkCode == EquityBenchmark.CODE)
                return equityBenchmark;

            if (benchmarkCode == Benchmarks.BlendedAdvancedBenchmark.CODE)
                return blendedAdvancedBenchmark;

            if (benchmarkCode == Benchmarks.BlendedBasicBenchmark.CODE)
                return blendedBasicBenchmark;

            if (benchmarkCode == BondsModelBenchmark.CODE)
                return bondsModelBenchmark;

            if (benchmarkCode == Benchmarks.SymbolBenchmark.SPY_CODE)
                return spySymbolBenchmark;

            if (benchmarkCode == Benchmarks.SymbolBenchmark.BENCHMARK_CODE)
                return benchmarkSymbolBenchmark;

            if (benchmarkCode == Benchmarks.BlendedConfigurableBenchmark.CODE)
                return blendedConfigurableBenchmark;

            if (benchmarkCode == Benchmarks.EquityBondsModelBenchmark.CODE)
                return equityBondsModelBenchmark;

            foreach (var benchmark in benchmarkSymbolsList)
            {
                if (benchmark.Symbol == benchmarkCode)
                    return benchmark;
            }

            return null;
        }

        public Dictionary<int, decimal> GetDailySnapshots(String benchmarkType)
        {
            var benchmark = GetBenchmark(benchmarkType);

            if (benchmark != null)
                return benchmark.DailyValues;

            return new Dictionary<int, decimal>();
        }

        public String GetBondsSymbol()
        {
            return bondsModelBenchmark.GetCurrentAssetSymbol();
        }
    }
}
