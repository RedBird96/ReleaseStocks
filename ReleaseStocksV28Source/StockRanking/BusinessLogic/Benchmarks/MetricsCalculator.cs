using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking.Benchmarks
{
    public class MetricsCalculator
    {
        BenchmarksCalculator benchmarksCalculator;
        BacktestCalculator backtestCalculator;

        //dictionarykey=metrictype, keyvaluepair key=dddays value=ddvalue
        Dictionary<String, KeyValuePair<int, decimal>> drawDown1 = new Dictionary<String, KeyValuePair<int, decimal>>();
        Dictionary<String, KeyValuePair<int, decimal>> drawDown2 = new Dictionary<String, KeyValuePair<int, decimal>>();
        Dictionary<String, KeyValuePair<int, decimal>> drawDown3 = new Dictionary<String, KeyValuePair<int, decimal>>();
        Dictionary<String, KeyValuePair<int, decimal>> drawDown3Years = new Dictionary<String, KeyValuePair<int, decimal>>();
        Dictionary<String, decimal> highWater = new Dictionary<String, decimal>();
        Dictionary<String, decimal> performanceYTD = new Dictionary<String, decimal>();
        Dictionary<String, decimal> performance3M = new Dictionary<String, decimal>();
        Dictionary<String, decimal> performance1Y = new Dictionary<String, decimal>();
        Dictionary<String, decimal> performance3Y = new Dictionary<String, decimal>();
        Dictionary<String, decimal> performance5Y = new Dictionary<String, decimal>();
        Dictionary<String, decimal> performance10Y = new Dictionary<String, decimal>();
        Dictionary<String, decimal> performance6M = new Dictionary<String, decimal>();
        Dictionary<String, decimal> performanceInception = new Dictionary<String, decimal>();
        Dictionary<String, decimal> performanceLastMonth = new Dictionary<String, decimal>();
        Dictionary<String, decimal> performanceLastYear = new Dictionary<String, decimal>();
        Dictionary<String, decimal> winningMonths = new Dictionary<String, decimal>();
        Dictionary<String, decimal> averageWinningMonth = new Dictionary<String, decimal>();
        Dictionary<String, decimal> averageLosingMonth = new Dictionary<String, decimal>();
        Dictionary<String, List<DrawDown>> allDrawDowns = new Dictionary<String, List<DrawDown>>();

        public MetricsCalculator(BenchmarksCalculator benchmarksCalculator, BacktestCalculator backtestCalculator)
        {
            this.benchmarksCalculator = benchmarksCalculator;
            this.backtestCalculator = backtestCalculator;
        }

        public List<DrawDown> GetAllDrawDowns(String benchmarkType)
        {
            if (!allDrawDowns.ContainsKey(benchmarkType))
            {
                calculateDrawDowns(benchmarkType);
            }

            return allDrawDowns[benchmarkType];
        }

        public KeyValuePair<int, decimal> GetDrawDown1(String benchmarkType)
        {
            if(!drawDown1.ContainsKey(benchmarkType))
            {
                calculateDrawDowns(benchmarkType);
            }

            return drawDown1[benchmarkType];
        }

        public KeyValuePair<int, decimal> GetDrawDown2(String benchmarkType)
        {
            if (!drawDown2.ContainsKey(benchmarkType))
            {
                calculateDrawDowns(benchmarkType);
            }

            return drawDown2[benchmarkType];
        }

        public KeyValuePair<int, decimal> GetDrawDown3(String benchmarkType)
        {
            if (!drawDown3.ContainsKey(benchmarkType))
            {
                calculateDrawDowns(benchmarkType);
            }

            return drawDown3[benchmarkType];
        }

        public KeyValuePair<int, decimal> GetDrawDown3Years(String benchmarkType)
        {
            if (!drawDown3Years.ContainsKey(benchmarkType))
            {
                calculateDrawDowns(benchmarkType);
            }

            return drawDown3Years[benchmarkType];
        }

        public decimal GetHighWater(String benchmarkType)
        {
            if (!highWater.ContainsKey(benchmarkType))
            {
                calculateDrawDowns(benchmarkType);
            }

            return highWater[benchmarkType];
        }

        public decimal GetPerformance1Y(String benchmarkType)
        {
            if (!performance1Y.ContainsKey(benchmarkType))
            {
                calculatePerformance(benchmarkType);
            }

            return performance1Y[benchmarkType];
        }

        public decimal GetPerformance3Y(String benchmarkType)
        {
            if (!performance3Y.ContainsKey(benchmarkType))
            {
                calculatePerformance(benchmarkType);
            }

            return performance3Y[benchmarkType];
        }

        public decimal GetPerformance5Y(String benchmarkType)
        {
            if (!performance5Y.ContainsKey(benchmarkType))
            {
                calculatePerformance(benchmarkType);
            }

            return performance5Y[benchmarkType];
        }

        public decimal GetPerformance10Y(String benchmarkType)
        {
            if (!performance10Y.ContainsKey(benchmarkType))
            {
                calculatePerformance(benchmarkType);
            }

            return performance10Y[benchmarkType];
        }

        public decimal GetPerformance3Months(String benchmarkType)
        {
            if (!performance3M.ContainsKey(benchmarkType))
            {
                calculatePerformance(benchmarkType);
            }

            return performance3M[benchmarkType];
        }

        public decimal GetPerformance6Months(String benchmarkType)
        {
            if (!performance6M.ContainsKey(benchmarkType))
            {
                calculatePerformance(benchmarkType);
            }

            return performance6M[benchmarkType];
        }

        public decimal GetPerformanceYTD(String benchmarkType)
        {
            if (!performanceYTD.ContainsKey(benchmarkType))
            {
                calculatePerformance(benchmarkType);
            }

            return performanceYTD[benchmarkType];
        }

        public decimal GetPerformanceInception(String benchmarkType)
        {
            if (!performanceInception.ContainsKey(benchmarkType))
            {
                calculatePerformance(benchmarkType);
            }

            return performanceInception[benchmarkType];
        }

        public decimal GetPerformanceLastYear(String benchmarkType)
        {
            if (!performanceLastYear.ContainsKey(benchmarkType))
            {
                calculatePerformance(benchmarkType);
            }

            return performanceLastYear[benchmarkType];
        }

        public decimal GetPerformanceLastMonth(String benchmarkType)
        {
            if (!performanceLastMonth.ContainsKey(benchmarkType))
            {
                calculatePerformance(benchmarkType);
            }

            return performanceLastMonth[benchmarkType];
        }
        
        public decimal GetWinningMonths(String benchmarkType)
        {
            if (!winningMonths.ContainsKey(benchmarkType))
            {
                calculatePerformance(benchmarkType);
            }

            return winningMonths[benchmarkType];
        }
        
        public decimal GetAverageWinningMonth(String benchmarkType)
        {
            if (!averageWinningMonth.ContainsKey(benchmarkType))
            {
                calculatePerformance(benchmarkType);
            }

            return averageWinningMonth[benchmarkType];
        }

        public decimal GetAverageLosingMonth(String benchmarkType)
        {
            if (!averageLosingMonth.ContainsKey(benchmarkType))
            {
                calculatePerformance(benchmarkType);
            }

            return averageLosingMonth[benchmarkType];
        }

        public List<SnapshotVariation> GetRevenueGroup(String benchmarkType, int monthsGroup)
        {
            var values = PortfolioSnapshot.GetSnapshotVariationValues(backtestCalculator.MonthlySnapshots, benchmarkType);

            if (monthsGroup == 1)
                return values;

            var results = new List<SnapshotVariation>();
            for(int i = 0; i < values.Count - monthsGroup; i++)
            {
                decimal newVariation = 1m;
                for(int indexValues = 0; indexValues < monthsGroup; indexValues++)
                    newVariation *= (values[i+indexValues].Value + 1m);
                newVariation -= 1m;

                var newVal = new SnapshotVariation(values[i].Date, newVariation);
                newVal.Months = monthsGroup;
                results.Add(newVal);
            }

            return results;
        }

        void calculatePerformance(String benchmarkType)
        {
            DateTime mtdDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            DateTime qtdDate = Utils.GetFirstQuarterDay(DateTime.Now).AddDays(-1);
            var portfolioMTD = backtestCalculator.MonthlySnapshots.Find(x => x.Date.Year == mtdDate.Year && x.Date.Month == mtdDate.Month && x.Date.Day == mtdDate.Day);
            var portfolioYTD = backtestCalculator.MonthlySnapshots.Find(x => x.Date.Year == DateTime.Now.Year - 1 && x.Date.Month == 12 && x.Date.Day == 31);
            var portfolioQTD = backtestCalculator.MonthlySnapshots.Find(x => x.Date.Year == qtdDate.Year && x.Date.Month == qtdDate.Month && x.Date.Day == qtdDate.Day);
            var portfolioLast = backtestCalculator.LastSnapshot;
            var portfolio1Month = backtestCalculator.Snapshot1Month;
            var portfolioLastYear = backtestCalculator.Snapshot1Year;
            if (portfolioQTD == null)
                portfolioQTD = new PortfolioSnapshot();
            if (portfolioYTD == null)
                portfolioYTD = new PortfolioSnapshot();

            this.performance10Y.Add(benchmarkType, getGains(portfolioLast, backtestCalculator.Snapshot10Year, benchmarkType));
            this.performance1Y.Add(benchmarkType, getGains(portfolioLast, backtestCalculator.Snapshot1Year, benchmarkType));
            this.performance3Y.Add(benchmarkType, getGains(portfolioLast, backtestCalculator.Snapshot3Year, benchmarkType));
            this.performance5Y.Add(benchmarkType, getGains(portfolioLast, backtestCalculator.Snapshot5Year, benchmarkType));
            this.performance3M.Add(benchmarkType, getGains(portfolioLast, backtestCalculator.Snapshot3Months, benchmarkType));
            this.performance6M.Add(benchmarkType, getGains(portfolioLast, backtestCalculator.Snapshot6Months, benchmarkType));
            this.performanceInception.Add(benchmarkType, getGains(portfolioLast, backtestCalculator.Portfolio.AccountSize, benchmarkType));
            this.performanceYTD.Add(benchmarkType, getGains(portfolioLast, portfolioYTD, benchmarkType));
            if (portfolioMTD == null)
                portfolioMTD = new PortfolioSnapshot();
            this.performanceLastMonth.Add(benchmarkType, getGains(portfolioMTD, portfolio1Month, benchmarkType));
            this.performanceLastYear.Add(benchmarkType, getGains(portfolioLast, portfolioLastYear, benchmarkType));

            var values = PortfolioSnapshot.GetSnapshotVariationValues(backtestCalculator.MonthlySnapshots, benchmarkType);
            var valuesList = values.Select(x => x.Value);
            var valuesUp = valuesList.Where(x => x > 0).ToList();
            var valuesDown = valuesList.Where(x => x < 0).ToList();
            int upMonths = valuesList.Where(x => x > 0).Count();
            int downMonths = valuesList.Where(x => x < 0).Count();

            this.winningMonths.Add(benchmarkType, (decimal)upMonths / (decimal)valuesList.Count());
            this.averageWinningMonth.Add(benchmarkType, (decimal)valuesUp.Average());
            this.averageLosingMonth.Add(benchmarkType, (decimal)valuesDown.Average());
        }

        decimal getGains(PortfolioSnapshot end, PortfolioSnapshot initial, String benchmarkType)
        {
            var initialVal = PortfolioSnapshot.GetSnapshotValue(initial, benchmarkType).Value;
            var endVal = PortfolioSnapshot.GetSnapshotValue(end, benchmarkType).Value;

            if (initialVal == 0)
                return 0;

            return (endVal - initialVal) / initialVal;
        }

        decimal getGains(PortfolioSnapshot end, decimal initialVal, String benchmarkType)
        {
            var endVal = PortfolioSnapshot.GetSnapshotValue(end, benchmarkType).Value;

            if (initialVal == 0)
                return 0;

            return (endVal - initialVal) / initialVal;
        }

        void calculateDrawDowns(String benchmarkType)
        {
            List<DrawDown> drawDownsList = new List<DrawDown>();
            var dailyValues = benchmarksCalculator.GetDailySnapshots(benchmarkType);

            if (dailyValues == null || dailyValues.Count == 0)
            {
                drawDown1.Add(benchmarkType, new KeyValuePair<int, decimal>(0, 0));
                drawDown2.Add(benchmarkType, new KeyValuePair<int, decimal>(0, 0));
                drawDown3.Add(benchmarkType, new KeyValuePair<int, decimal>(0, 0));
                drawDown3Years.Add(benchmarkType, new KeyValuePair<int, decimal>(0, 0));
                allDrawDowns.Add(benchmarkType, new List<DrawDown>());
                this.highWater.Add(benchmarkType, 0);
                return;
            }
            var drawDowns = new List<KeyValuePair<int, decimal>>();

            decimal highWater = 0;
            decimal currentDD = 0;
            DateTime currentDDStartDate = DateTime.Now;
            DateTime currentDDLowestDate = DateTime.Now;
            decimal currentMax = 0;
            var orderedList = dailyValues.Keys.OrderBy(x => x).ToList();
            decimal startingValue = dailyValues[orderedList[0]];
            decimal dd3Years = 0;
            decimal currentMax3Years = 0;
            int dd3YearsDate = Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-3));
            decimal maxDD3Years = 0;
            decimal value = 0;
            DrawDown lastDD = null;
            foreach (int date in orderedList)
            {
                value = dailyValues[date];

                if(value >= currentMax)
                {
                    if(currentDD != 0)
                    {
                        drawDowns.Add(new KeyValuePair<int, decimal>((int)(currentDDLowestDate-currentDDStartDate).TotalDays, currentDD));
                        lastDD = new DrawDown()
                        {
                            Depth = currentDD,
                            Months = (int)((currentDDLowestDate - currentDDStartDate).TotalDays / 30d),
                            StartDate = Utils.ConvertDateTimeToInt(currentDDStartDate),
                            EndDate = Utils.ConvertDateTimeToInt(currentDDLowestDate),
                            Recovery = (int)((Utils.ConvertIntToDateTime(date) - currentDDLowestDate).TotalDays / 30d)
                        };
                        drawDownsList.Add(lastDD);
                    }

                    //reset DD
                    currentDD = 0;
                    currentDDStartDate = Utils.ConvertIntToDateTime(date);
                    currentDDLowestDate = Utils.ConvertIntToDateTime(date);

                    currentMax = Math.Max(value, currentMax);
                    highWater = Math.Max(Math.Round(currentMax / startingValue, 4), highWater);
                }
                else
                {
                    var newDD = Math.Round((value - currentMax) / currentMax, 4);
                    if (newDD <= currentDD)
                    {
                        currentDD = newDD;
                        currentDDLowestDate = Utils.ConvertIntToDateTime(date);
                    }
                }


                //DD 3 years
                if (date >= dd3YearsDate)
                {
                    if (value >= currentMax3Years)
                    {
                        currentMax3Years = Math.Max(value, currentMax3Years);
                    }
                    else
                    {
                        var newDD = currentMax3Years == 0 ? 0 : Math.Round((value - currentMax3Years) / currentMax3Years, 4);
                        if (newDD <= dd3Years)
                            dd3Years = newDD;
                    }
                }

            }

            //if drawdowns continue on the last day
            if (currentDD != 0)
            {
                drawDowns.Add(new KeyValuePair<int, decimal>((int)(currentDDLowestDate - currentDDStartDate).TotalDays, currentDD));
                lastDD = new DrawDown()
                {
                    Depth = currentDD,
                    Months = (int)((currentDDLowestDate - currentDDStartDate).TotalDays / 30d),
                    StartDate = Utils.ConvertDateTimeToInt(currentDDStartDate),
                    EndDate = Utils.ConvertDateTimeToInt(currentDDLowestDate),
                    Recovery = 0
                };
                drawDownsList.Add(lastDD);
            }
            
            var orderedDD = drawDowns.OrderBy(x => x.Value).ToList();

            var drawDown1Value = new KeyValuePair<int, decimal>(0, 0);
            var drawDown2Value = new KeyValuePair<int, decimal>(0, 0);
            var drawDown3Value = new KeyValuePair<int, decimal>(0, 0);

            if (orderedDD.Count >= 1)
                drawDown1Value = orderedDD[0];
            if (orderedDD.Count >= 2)
                drawDown2Value = orderedDD[1];
            if (orderedDD.Count >= 3)
                drawDown3Value = orderedDD[2];

            drawDown1.Add(benchmarkType, drawDown1Value);
            drawDown2.Add(benchmarkType, drawDown2Value);
            drawDown3.Add(benchmarkType, drawDown3Value);
            drawDown3Years.Add(benchmarkType, new KeyValuePair<int, decimal>(0, dd3Years));
            allDrawDowns.Add(benchmarkType, drawDownsList.OrderBy(x => x.Depth).ToList());
            this.highWater.Add(benchmarkType, highWater);
        }

        public static Dictionary<RiskMetrics, object> CalcMetrics(BacktestCalculator backtestCalculator, String benchmarkType, String benchmarkCorrelation)
        {
            var resultMetrics = new Dictionary<RiskMetrics, object>();

            var values = PortfolioSnapshot.GetSnapshotVariationValues(backtestCalculator.MonthlySnapshots, benchmarkType);
            var valuesList = values.Select(x => x.Value).ToList();

            var valuesBenchmark = PortfolioSnapshot.GetSnapshotVariationValues(backtestCalculator.MonthlySnapshots, benchmarkCorrelation);
            var valuesListBenchmark = valuesBenchmark.Select(x => x.Value).ToList();

            var valuesYearly = PortfolioSnapshot.GetSnapshotVariationValuesYearly(backtestCalculator.MonthlySnapshots, benchmarkType);
            var valuesYearlyBenchmark = PortfolioSnapshot.GetSnapshotVariationValuesYearly(backtestCalculator.MonthlySnapshots, benchmarkCorrelation);

            resultMetrics.Add(RiskMetrics.Volatility, DataUpdater.CalcStdDev(valuesList));
            resultMetrics.Add(RiskMetrics.VolatilityAnnualized, DataUpdater.CalcStdDev(valuesYearly.Select(x => x.Value).ToList()));
            resultMetrics.Add(RiskMetrics.DownsideDeviationMonthly, DataUpdater.CalcStdDev(values.Where(x => x.Value < 0).Select(x => x.Value).ToList()));

            var DD = backtestCalculator.MetricsCalculator.GetDrawDown1(benchmarkType);
            resultMetrics.Add(RiskMetrics.DD1, (DD.Value * 100).ToString("n2") + "% (" + DD.Key + "d)");
            resultMetrics.Add(RiskMetrics.DD1Plain, DD.Value);
            DD = backtestCalculator.MetricsCalculator.GetDrawDown2(benchmarkType);
            resultMetrics.Add(RiskMetrics.DD2, DD.Value != 0 ? (DD.Value * 100).ToString("n2") + "% (" + DD.Key + "d)" : "0");
            DD = backtestCalculator.MetricsCalculator.GetDrawDown3(benchmarkType);
            resultMetrics.Add(RiskMetrics.DD3, DD.Value != 0 ? (DD.Value * 100).ToString("n2") + "% (" + DD.Key + "d)" : "0");


            //Betas
            int last3YearsDate = Utils.ConvertDateTimeToInt(DateTime.Now.AddYears(-3));
            var values3Years = values.Where(x => x.Date >= last3YearsDate).OrderBy(x => x.Date).Select(x => x.Value).ToList();
            var values3YearsBenchmark = valuesBenchmark.Where(x => x.Date >= last3YearsDate).OrderBy(x => x.Date).Select(x => x.Value).ToList();
            var lifetimeBeta = StatisticsMath.CalcBeta(valuesList, valuesListBenchmark);

            resultMetrics.Add(RiskMetrics.BetaLifetime, lifetimeBeta);
            resultMetrics.Add(RiskMetrics.RollingBeta3Years, StatisticsMath.CalcBeta(values3Years, values3YearsBenchmark));


            //calc yearly alpha
            List<decimal> avgAlpha = new List<decimal>();
            for (int i = 0; i < valuesYearly.Count; i++)
                avgAlpha.Add(valuesYearly[i].Value - valuesYearlyBenchmark[i].Value);
            resultMetrics.Add(RiskMetrics.AvgAnnualAlpha, avgAlpha.Count == 0 ? 0 : avgAlpha.Average());


            //last year alpha
            var portfolioLastYear = PortfolioSnapshot.GetSnapshotValue(backtestCalculator.Snapshot1Year, benchmarkType).Value;
            var benchmarkLastYear = PortfolioSnapshot.GetSnapshotValue(backtestCalculator.Snapshot1Year, benchmarkCorrelation).Value;
            var portfolioCurrent = PortfolioSnapshot.GetSnapshotValue(backtestCalculator.LastSnapshot, benchmarkType).Value;
            var benchmarkCurrent = PortfolioSnapshot.GetSnapshotValue(backtestCalculator.LastSnapshot, benchmarkCorrelation).Value;
            var variationPortfolio = portfolioLastYear == 0 ? 0 : (portfolioCurrent - portfolioLastYear) / portfolioLastYear * 100;
            var variationBenchmark = benchmarkLastYear == 0 ? 0 : (benchmarkCurrent - benchmarkLastYear) / benchmarkLastYear * 100;
            resultMetrics.Add(RiskMetrics.AlphaAnnualized, variationPortfolio - variationBenchmark);


            int iCount = 0;
            double r2 = StatisticsMath.GenerateR2(valuesList.Select(x => new PointF(iCount++, (float)x)).ToList());
            resultMetrics.Add(RiskMetrics.R2, r2);

            decimal average = valuesList.Count() == 0 ? 0 : valuesList.Average();
            decimal stdDev = DataUpdater.CalcStdDev(valuesList, average);
            decimal semivariance = DataUpdater.CalcSemivariance(valuesList, 0);

            decimal sharpe = stdDev == 0 ? 0 : (average / stdDev) * Convert.ToDecimal(Math.Sqrt(valuesList.Count()));
            decimal sortino = semivariance == 0 ? 0 : (average / semivariance) * Convert.ToDecimal(Math.Sqrt(valuesList.Count()));

            resultMetrics.Add(RiskMetrics.SharpeRatio, sharpe);
            resultMetrics.Add(RiskMetrics.SortinoRatio, sortino);

            decimal totalReturn = 0;
            if (values.Count() > 0)
                if (PortfolioSnapshot.GetSnapshotValue(backtestCalculator.LastSnapshot, benchmarkType).Value != 0)
                    totalReturn = (PortfolioSnapshot.GetSnapshotValue(backtestCalculator.LastSnapshot, benchmarkType).Value - backtestCalculator.Portfolio.AccountSize) / backtestCalculator.Portfolio.AccountSize;
            decimal treynor = lifetimeBeta == 0 ? 0 : (totalReturn - 0.01m) / lifetimeBeta;

            resultMetrics.Add(RiskMetrics.TreynorRatio, treynor);

            //calmar
            decimal[] years = new decimal[4];
            var dd3Years = backtestCalculator.MetricsCalculator.GetDrawDown3Years(benchmarkType).Value;
            years[3] = PortfolioSnapshot.GetSnapshotValue(backtestCalculator.LastSnapshot, benchmarkType).Value;
            years[2] = PortfolioSnapshot.GetSnapshotValue(backtestCalculator.Snapshot1Year, benchmarkType).Value;
            years[1] = PortfolioSnapshot.GetSnapshotValue(backtestCalculator.Snapshot2Year, benchmarkType).Value;
            years[0] = PortfolioSnapshot.GetSnapshotValue(backtestCalculator.Snapshot3Year, benchmarkType).Value;
            var returnsCalmar = new List<decimal>();
            for (int i = 0; i < years.Length - 1; i++)
                if (years[i] != 0)
                    returnsCalmar.Add((years[i + 1] - years[i]) / years[i]);
            var calmar = dd3Years == 0 ? 0 : returnsCalmar.Average() / -dd3Years;
            resultMetrics.Add(RiskMetrics.Calmar, calmar);

            var informationRatio = stdDev == 0 ? 0 : average / stdDev;
            resultMetrics.Add(RiskMetrics.InformationRatio, informationRatio);

            var upResults = new List<decimal>();
            var downResults = new List<decimal>();
            var upResultsSpy = new List<decimal>();
            var downResultsSpy = new List<decimal>();
            int maxWinStreak = 0;
            int maxLossStreak = 0;
            int winStreak = 0;
            int lossStreak = 0;
            for (int iPeriod = 0; iPeriod < valuesList.Count; iPeriod++)
            {
                if (valuesListBenchmark[iPeriod] > 0)
                {
                    upResultsSpy.Add(valuesListBenchmark[iPeriod]);
                    upResults.Add(valuesList[iPeriod]);
                }

                if (valuesListBenchmark[iPeriod] < 0)
                {
                    downResultsSpy.Add(valuesListBenchmark[iPeriod]);
                    downResults.Add(valuesList[iPeriod]);
                }

                decimal alpha = valuesList[iPeriod] - valuesListBenchmark[iPeriod];
                if (alpha > 0)
                {
                    winStreak++;
                    maxWinStreak = Math.Max(winStreak, maxWinStreak);
                    lossStreak = 0;
                }

                if (alpha < 0)
                {
                    lossStreak++;
                    maxLossStreak = Math.Max(lossStreak, maxLossStreak);
                    winStreak = 0;
                }
            }

            decimal spyUpTotal = upResultsSpy.Aggregate(1m, (x, y) => x * (1m + y)) - 1;
            decimal spyDownTotal = downResultsSpy.Aggregate(1m, (x, y) => x * (1m + y)) - 1;
            decimal upCapture = spyUpTotal == 0 ? 0 : (upResults.Aggregate(1m, (x, y) => x * (1m + y)) - 1) / spyUpTotal;
            decimal downCapture = spyDownTotal == 0 ? 0 : (downResults.Aggregate(1m, (x, y) => x * (1m + y)) - 1) / spyDownTotal;

            resultMetrics.Add(RiskMetrics.UpsideCapture, upCapture);
            resultMetrics.Add(RiskMetrics.DownsideCapture, downCapture);

            resultMetrics.Add(RiskMetrics.WinStreak, maxWinStreak);
            resultMetrics.Add(RiskMetrics.LoseStreak, maxLossStreak);

            decimal initialPortfolio = backtestCalculator.Portfolio.AccountSize;
            decimal lastValue = backtestCalculator.Portfolio.AccountSize;
            decimal gainsSum = 0;
            decimal lossSum = 0;
            foreach (var snap in backtestCalculator.MonthlySnapshots)
            {
                decimal newVal = PortfolioSnapshot.GetSnapshotValue(snap, benchmarkType).Value;
                if (newVal != lastValue)
                {
                    decimal variation = newVal - lastValue;
                    if (variation > 0)
                        gainsSum += variation;
                    else
                        lossSum -= variation;

                    lastValue = newVal;
                }
            }
            decimal profitFactor = lossSum == 0 ? 100 : gainsSum / lossSum;
            resultMetrics.Add(RiskMetrics.ProfitFactor, profitFactor);

            int upMonths = valuesList.Where(x => x > 0).Count();
            int downMonths = valuesList.Where(x => x < 0).Count();

            resultMetrics.Add(RiskMetrics.PercPositiveMonths, (decimal)upMonths / (decimal)valuesList.Count() * 100m);
            resultMetrics.Add(RiskMetrics.PercNegativeMonths, (decimal)downMonths / (decimal)valuesList.Count() * 100m);

            var correlation = StatisticsMath.CalcCorrelation(valuesList, valuesListBenchmark);
            resultMetrics.Add(RiskMetrics.BenchmarkCorrelation, correlation);

            //calc VaR Historical
            var valuesYearlyCompleteYears = PortfolioSnapshot.GetSnapshotVariationValuesYearly(backtestCalculator.MonthlySnapshots, benchmarkType, true);
            decimal varHist = 0;
            if (valuesYearlyCompleteYears.Count > 0)
            {
                var list = valuesYearlyCompleteYears.Select(x => x.Value).ToList();
                varHist = Math.Abs((list.Average() - 1.96m * DataUpdater.CalcStdDev(list)) * PortfolioSnapshot.GetPortfolioValue(backtestCalculator.LastSnapshot, benchmarkType));
            }
            resultMetrics.Add(RiskMetrics.VARHistorical, varHist);

            return resultMetrics;
        }

    }
}
