using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockRanking.Benchmarks;
using StockRanking.Entities;

namespace StockRanking
{
    [DebuggerDisplay("{Date} Total: {TotalPortfolioValue} SPY: {SpyBenchmark}")]
    public class PortfolioSnapshot
    {
        public List<Trade> Trades = new List<Trade>();
        public decimal TotalPortfolioValue = 0;
        public decimal TotalPositionsValue = 0;
        public decimal TotalCash = 0;
        public int RealDate = 0;
        public DateTime Date = DateTime.Now;

        public decimal BlendedBasicBenchmark = 0;
        public decimal BlendedAdvancedBenchmark = 0;
        public decimal BondsBenchmark = 0;
        public decimal SpyBenchmark = 0;
        public decimal BenchmarkSymbolBenchmark = 0;
        public decimal BlendedConfigurableBenchmark = 0;
        public decimal EquityBondsModelBenchmark = 0;

        public Dictionary<String, decimal> BenchmarksList = new Dictionary<string, decimal>();

        public PortfolioSnapshot()
        {
        }

        public PortfolioSnapshot(BenchmarksCalculator benchmarksCalculator)
        {
            this.BlendedAdvancedBenchmark = benchmarksCalculator.GetBlendedAdvancedValue(false);
            this.BlendedBasicBenchmark = benchmarksCalculator.GetBlendedBasicValue(false);
            this.BondsBenchmark = benchmarksCalculator.GetBondsValue(false);
            this.SpyBenchmark = benchmarksCalculator.GetSPYValue(false);
            this.BenchmarkSymbolBenchmark = benchmarksCalculator.GetBenchmarkSymbolValue(false);
            this.BenchmarksList = benchmarksCalculator.GetBenchmarkListValues(false);
            this.BlendedConfigurableBenchmark = benchmarksCalculator.GetBlendedConfigurableBenchmarkValues(false);
            this.EquityBondsModelBenchmark = benchmarksCalculator.GetEquityBondsModelBenchmarkValues(false);
        }

        public PortfolioSnapshot(DateTime date, decimal totalMoney, BenchmarksCalculator benchmarksCalculator, bool monthly = false)
        {
            this.Date = date;
            this.RealDate = benchmarksCalculator.GetRealDate(date);
            this.TotalPortfolioValue = totalMoney;
            this.BlendedAdvancedBenchmark = benchmarksCalculator.GetBlendedAdvancedValue(monthly);
            this.BlendedBasicBenchmark = benchmarksCalculator.GetBlendedBasicValue(monthly);
            this.BondsBenchmark = benchmarksCalculator.GetBondsValue(monthly);
            this.SpyBenchmark = benchmarksCalculator.GetSPYValue(monthly);
            this.BenchmarkSymbolBenchmark = benchmarksCalculator.GetBenchmarkSymbolValue(monthly);
            this.BenchmarksList = benchmarksCalculator.GetBenchmarkListValues(monthly);
            this.BlendedConfigurableBenchmark = benchmarksCalculator.GetBlendedConfigurableBenchmarkValues(monthly);
            this.EquityBondsModelBenchmark = benchmarksCalculator.GetEquityBondsModelBenchmarkValues(monthly);
        }

        internal static List<SnapshotVariation> GetSnapshotVariationValues(List<PortfolioSnapshot> snapshots, String benchmarkCode)
        {
            var values = new List<SnapshotVariation>();
            decimal lastValue = 0;

            foreach (var snapshot in snapshots)
            {
                decimal value = GetPortfolioValue(snapshot, benchmarkCode);
                
                decimal variation = 0;
                if (lastValue != 0)
                {
                    variation = (value - lastValue) / lastValue;
                }
                lastValue = value;

                values.Add(new SnapshotVariation(Utils.ConvertDateTimeToInt(snapshot.Date), variation));
            }

            return values;
        }
        
        public static decimal GetPortfolioValue(PortfolioSnapshot snapshot, String benchmarkCode)
        {
            if (benchmarkCode == EquityBenchmark.CODE)
                return snapshot.TotalPortfolioValue;

            if (benchmarkCode == Benchmarks.BlendedAdvancedBenchmark.CODE)
                return snapshot.BlendedAdvancedBenchmark;

            if (benchmarkCode == Benchmarks.BlendedBasicBenchmark.CODE)
                return snapshot.BlendedBasicBenchmark;

            if (benchmarkCode == BondsModelBenchmark.CODE)
                return snapshot.BondsBenchmark;

            if (benchmarkCode == Benchmarks.SymbolBenchmark.SPY_CODE)
                return snapshot.SpyBenchmark;

            if (benchmarkCode == Benchmarks.SymbolBenchmark.BENCHMARK_CODE)
                return snapshot.BenchmarkSymbolBenchmark;

            if (benchmarkCode == Benchmarks.BlendedConfigurableBenchmark.CODE)
                return snapshot.BlendedConfigurableBenchmark;

            if (benchmarkCode == Benchmarks.EquityBondsModelBenchmark.CODE)
                return snapshot.EquityBondsModelBenchmark;

            foreach (var symbol in snapshot.BenchmarksList.Keys)
            {
                if (symbol == benchmarkCode)
                    return snapshot.BenchmarksList[symbol];
            }

            return 0;
        }
        
        internal static List<SnapshotVariation> GetSnapshotVariationValuesYearly(List<PortfolioSnapshot> snapshots, String benchmarkCode, bool justCompleteYears = false)
        {
            var results = new List<SnapshotVariation>();

            if (snapshots.Count == 0)
                return results;

            decimal lastValue = GetPortfolioValue(snapshots[0], benchmarkCode);
            int monthsUsed = 0;

            for (int i = 1; i < snapshots.Count; i++)
            {
                monthsUsed++;
                var snapshot = snapshots[i];
                if (snapshot.Date.Month == 12 && snapshot.Date.Day == 31
                    || i == snapshots.Count - 1)
                {

                    decimal value = GetPortfolioValue(snapshot, benchmarkCode);

                    decimal variation = 0;
                    if (lastValue != 0)
                    {
                        variation = (value - lastValue) / lastValue;
                    }
                    lastValue = value;

                    var newVal = new SnapshotVariation(Utils.ConvertDateTimeToInt(snapshot.Date), variation);
                    newVal.Months = 12;

                    if (!justCompleteYears || monthsUsed >= 11)
                        results.Add(newVal);

                    monthsUsed = 0;
                }
            }

            return results;
        }


        internal static KeyValuePair<int, decimal> GetSnapshotValue(PortfolioSnapshot snapshot, String benchmarkCode)
        {
            decimal value = GetPortfolioValue(snapshot, benchmarkCode);

            return new KeyValuePair<int, decimal>(Utils.ConvertDateTimeToInt(snapshot.Date), value);
        }
    }
}
