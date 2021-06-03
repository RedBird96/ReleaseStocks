using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using LiveCharts;
using LiveCharts.Wpf;
using StockRanking.Benchmarks;

namespace StockRanking
{
    public partial class PerformanceReport : Form
    {
        bool initializing = true;
        BacktestCalculator backtestCalculator;
        String benchmarkSelected = EquityBenchmark.CODE;
        String modelSelected = EquityBenchmark.CODE;
        Dictionary<String, List<ChartData>> chartsDataToExport = new Dictionary<string, List<ChartData>>();

        public PerformanceReport(BacktestCalculator backtestCalculator)
        {
            InitializeComponent();

            txtManagementFee.Text = backtestCalculator.Portfolio.AnnualFee.ToString("n2") + "%";
            lblEquityTitle.Text = lblEquityTitle.Text.Replace("##", backtestCalculator.Portfolio.AnnualFee.ToString("n2") + "%");
            lblMonthlyTitle.Text = lblMonthlyTitle.Text.Replace("##", backtestCalculator.Portfolio.AnnualFee.ToString("n2") + "%");
            txtModelDescription.Text = Strategy.GetStrategyDescription(backtestCalculator.Portfolio.Id);

            this.backtestCalculator = backtestCalculator;

            var modelsList = new List<KeyValuePair<int, string>>();
            modelsList.Add(new KeyValuePair<int, string>(0, "Equity"));
            modelsList.Add(new KeyValuePair<int, string>(1, "Bond Model"));
            modelsList.Add(new KeyValuePair<int, string>(2, "Blended Advanced with Bond Switching"));
            modelsList.Add(new KeyValuePair<int, string>(3, "Blended Advanced with Bond Model"));
            cboChangeModel.DataSource = modelsList;
            cboChangeModel.SelectedIndex = 0;

            refreshModels();

        }

        void refreshModels()
        {
            switch(cboChangeModel.SelectedValue)
            {
                case 0:
                    modelSelected = EquityBenchmark.CODE;
                    benchmarkSelected = SymbolBenchmark.BENCHMARK_CODE;
                    break;
                case 1:
                    modelSelected = BondsModelBenchmark.CODE;
                    benchmarkSelected = Stock.GetStock(backtestCalculator.Portfolio.GetBondsRiskModelAssetId()).Symbol;
                    break;
                case 2:
                    modelSelected = BlendedAdvancedBenchmark.CODE;
                    benchmarkSelected = BlendedConfigurableBenchmark.CODE;
                    break;
                case 3:
                    modelSelected = EquityBondsModelBenchmark.CODE;
                    benchmarkSelected = BlendedConfigurableBenchmark.CODE;
                    break;
            }

            chartsDataToExport.Clear();

            drawEquityGraph();
            drawDistribution();
            drawRORGraph();
            drawVolatilityGraph();
            drawMonthlyReturns();
            fillGeneralGrid();
            fillReturnStatistics();
            fillRiskStatistics();
            fillReturnReport();
            fillDDReport();


            disableSort(grdDrawdownReport);
            disableSort(grdMonthlyPerformance);
            disableSort(grdReturnReport);
            disableSort(grdReturnStatistics);
            disableSort(grdRiskStats);
            disableSort(gridBasicData);
        }

        void disableSort(DataGridView grid)
        {
            for (int i = 0; i < grid.Columns.Count; i++)
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            grid.ClearSelection();
        }

        void drawDistribution()
        {
            LiveCharts.SeriesCollection seriesToPlot = new LiveCharts.SeriesCollection();

            LiveCharts.Wpf.ColumnSeries newLine1 = new ColumnSeries();
            newLine1.Title = "Model";
            newLine1.Values = new ChartValues<int> { };
            newLine1.PointGeometry = null;

            LiveCharts.Wpf.ColumnSeries newLine2 = new ColumnSeries();
            newLine2.Title = "Benchmark";
            newLine2.Values = new ChartValues<int> { };
            newLine2.PointGeometry = null;

            newLine1.Fill = System.Windows.Media.Brushes.Blue;
            newLine2.Fill = System.Windows.Media.Brushes.Red;

            List<ChartData> chartDist = new List<ChartData>();
            chartsDataToExport.Add("Distribution Chart", chartDist);

            List<String> labels = new List<String>();
            labels.Add("< -10%");
            for (int i = -10; i < 10; i++)
                labels.Add(i + " to " + (i+1) + "%");
            labels.Add("> 10%");

            var values = PortfolioSnapshot.GetSnapshotVariationValues(backtestCalculator.MonthlySnapshots, modelSelected);
            var valuesList = values.Select(x => x.Value).ToList();

            var valuesBenchmark = PortfolioSnapshot.GetSnapshotVariationValues(backtestCalculator.MonthlySnapshots, benchmarkSelected);
            var valuesListBenchmark = valuesBenchmark.Select(x => x.Value).ToList();

            var histogram1 = createHistogram(valuesList);
            var histogram2 = createHistogram(valuesListBenchmark);

            for(int i = 0; i < histogram1.Length; i++)
            {
                newLine1.Values.Add(histogram1[i]);
                newLine2.Values.Add(histogram2[i]);

                chartDist.Add(new ChartData(labels[i], histogram1[i], histogram2[i]));
            }

            seriesToPlot.Add(newLine1);
            seriesToPlot.Add(newLine2);

            graphDistribution.Series = seriesToPlot;

            graphDistribution.AxisX.Clear();
            graphDistribution.AxisX.Add(new LiveCharts.Wpf.Axis { Labels = labels });
            graphDistribution.AxisX[0].Separator.Step = 1;
            graphDistribution.AxisX[0].Separator.StrokeThickness = 0;
            graphDistribution.AxisX[0].LabelsRotation = -90;
            
            graphDistribution.LegendLocation = LegendLocation.None;
        }

        int[] createHistogram(List<decimal> values)
        {
            int[] results = new int[22];

            foreach(var value in values)
            {
                if (value < -0.1m)
                {
                    results[0]++;
                    continue;
                }

                if (value > 0.1m)
                {
                    results[21]++;
                    continue;
                }

                int iIndex = (int)Math.Floor(value * 100.0m);
                iIndex = iIndex + 11;

                results[iIndex]++;
            }

            return results;
        }

        void drawMonthlyReturns()
        {
            List<PortfolioSnapshot> monthlySnapshots = backtestCalculator.MonthlySnapshots.ToList();

            if (grdMonthlyPerformance.Columns.Count < 10)
            {
                colGrdMonthlyYear.MinimumWidth = 60;
                colGrdMonthlyYear.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                colGrdMonthlyYear.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdMonthlyPerformance.GridColor = Color.White;
                colGrdMonthlyYear.DefaultCellStyle.Format = "";
                colGrdMonthlyYear.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colGrdMonthlyYear.HeaderText = "";
                DataGridViewColumn col;

                for (int i = 1; i <= 12; i++)
                {
                    col = new DataGridViewColumn(colGrdMonthlyYear.CellTemplate);
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    col.HeaderText = new DateTime(2018, i, 1).ToString("MMM", CultureInfo.InvariantCulture).ToUpper();
                    col.DefaultCellStyle.Format = "n2";
                    col.MinimumWidth = 60;
                    col.HeaderCell.Style.ForeColor = Color.Green;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    grdMonthlyPerformance.Columns.Add(col);
                }

                col = new DataGridViewColumn(colGrdMonthlyYear.CellTemplate);
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                col.HeaderText = "YEAR";
                col.DefaultCellStyle.Format = "n2";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.MinimumWidth = 60;
                grdMonthlyPerformance.Columns.Add(col);
            }

            grdMonthlyPerformance.Rows.Clear();

            int currentMonth = monthlySnapshots[0].Date.AddDays(1).Month;
            decimal previousValue = PortfolioSnapshot.GetPortfolioValue(monthlySnapshots[0], modelSelected);
            int currentYear = monthlySnapshots[0].Date.AddDays(1).Year;
            var currentRow = grdMonthlyPerformance.Rows[grdMonthlyPerformance.Rows.Add()];
            DataGridViewTextBoxCell cell;
            decimal lastYearValue = previousValue;
            monthlySnapshots.RemoveAt(0);

            List<decimal> monthlyPercent = new List<decimal>();
            decimal initialPortfolio = previousValue;

            //add year
            currentRow.Cells[0].Value = currentYear.ToString();

            if (currentMonth > 1)
            {
                for (int i = 1; i <= currentMonth; i++)
                {
                    cell = new DataGridViewTextBoxCell();
                    currentRow.Cells[currentMonth].Value = "-";
                }
            }

            currentMonth--;
            currentYear++;

            foreach (PortfolioSnapshot snap in monthlySnapshots)
            {
                currentMonth++;
                if (currentMonth > 12)
                {
                    //add totals
                    if (previousValue == 0)
                        currentRow.Cells[12 + 1].Value = "-";
                    else
                    {
                        currentRow.Cells[12 + 1].Value = (previousValue - lastYearValue) / lastYearValue * (decimal)100;
                        paintCell((DataGridViewTextBoxCell)currentRow.Cells[12 + 1], true);
                    }
                    currentMonth = 1;

                    lastYearValue = previousValue;
                    currentRow = grdMonthlyPerformance.Rows[grdMonthlyPerformance.Rows.Add()];

                    //add year
                    currentRow.Cells[0].Value = currentYear++;
                }

                //add % value
                var snapValue = PortfolioSnapshot.GetPortfolioValue(snap, modelSelected);
                if (previousValue == 0)
                    currentRow.Cells[currentMonth].Value = 0;
                else
                {
                    currentRow.Cells[currentMonth].Value = (snapValue - previousValue) / previousValue * (decimal)100;

                    paintCell((DataGridViewTextBoxCell)currentRow.Cells[currentMonth], false);
                }

                monthlyPercent.Add((snapValue - previousValue) / previousValue * (decimal)100);
                previousValue = snapValue;
            }

            //last year total value
            if (previousValue == 0)
                currentRow.Cells[12 + 1].Value = "-";
            else
            {
                currentRow.Cells[12 + 1].Value = (previousValue - lastYearValue) / lastYearValue * (decimal)100;
                paintCell((DataGridViewTextBoxCell)currentRow.Cells[12 + 1], true);
            }

            for (int auxMonth = currentMonth + 1; auxMonth <= 12; auxMonth++)
            {
                currentRow.Cells[auxMonth].Style.BackColor = Color.FromArgb(243, 243, 243);
                currentRow.Cells[auxMonth].Style.ForeColor = Color.FromArgb(109, 109, 109);
                currentRow.Cells[auxMonth].Value = "-";
            }

            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            rows.AddRange(grdMonthlyPerformance.Rows.Cast<DataGridViewRow>());
            rows.Reverse();
            grdMonthlyPerformance.Rows.Clear();
            grdMonthlyPerformance.Rows.AddRange(rows.ToArray());
        }

        void fillGeneralGrid()
        {
            gridBasicData.Rows.Clear();

            int row = gridBasicData.Rows.Add();
            gridBasicData.Rows[row].Cells[0].Value = "Model";
            gridBasicData.Rows[row].Cells[1].Value = backtestCalculator.MetricsCalculator.GetPerformance3Months(modelSelected);
            gridBasicData.Rows[row].Cells[2].Value = backtestCalculator.MetricsCalculator.GetPerformanceYTD(modelSelected);
            gridBasicData.Rows[row].Cells[3].Value = backtestCalculator.MetricsCalculator.GetPerformance1Y(modelSelected);
            gridBasicData.Rows[row].Cells[4].Value = backtestCalculator.MetricsCalculator.GetPerformance3Y(modelSelected);
            gridBasicData.Rows[row].Cells[5].Value = backtestCalculator.MetricsCalculator.GetPerformanceInception(modelSelected);

            row = gridBasicData.Rows.Add();
            gridBasicData.Rows[row].Cells[0].Value = "Benchmark";
            gridBasicData.Rows[row].Cells[1].Value = backtestCalculator.MetricsCalculator.GetPerformance3Months(benchmarkSelected);
            gridBasicData.Rows[row].Cells[2].Value = backtestCalculator.MetricsCalculator.GetPerformanceYTD(benchmarkSelected);
            gridBasicData.Rows[row].Cells[3].Value = backtestCalculator.MetricsCalculator.GetPerformance1Y(benchmarkSelected);
            gridBasicData.Rows[row].Cells[4].Value = backtestCalculator.MetricsCalculator.GetPerformance3Y(benchmarkSelected);
            gridBasicData.Rows[row].Cells[5].Value = backtestCalculator.MetricsCalculator.GetPerformanceInception(benchmarkSelected);

            txt1Year.Text = (backtestCalculator.MetricsCalculator.GetPerformance1Y(modelSelected) * 100).ToString("0.00") + "%";
            txtYTD.Text = (backtestCalculator.MetricsCalculator.GetPerformanceYTD(modelSelected) * 100).ToString("0.00") + "%";
            txtLastMonth.Text = (backtestCalculator.MetricsCalculator.GetPerformanceLastMonth(modelSelected) * 100).ToString("0.00") + "%";
            txtSinceInception.Text = (backtestCalculator.MetricsCalculator.GetPerformanceInception(modelSelected) * 100).ToString("0.00") + "%";
        }

        void fillReturnStatistics()
        {
            grdReturnStatistics.Rows.Clear();

            addRow(grdReturnStatistics, "Last Month", backtestCalculator.MetricsCalculator.GetPerformanceLastMonth(modelSelected)*100m);
            addRow(grdReturnStatistics, "Year To Date", backtestCalculator.MetricsCalculator.GetPerformanceYTD(modelSelected) * 100m);
            addRow(grdReturnStatistics, "3 Month ROR", backtestCalculator.MetricsCalculator.GetPerformance3Months(modelSelected) * 100m);
            addRow(grdReturnStatistics, "12 Month ROR", backtestCalculator.MetricsCalculator.GetPerformanceLastYear(modelSelected) * 100m);
            addRow(grdReturnStatistics, "36 Month ROR", backtestCalculator.MetricsCalculator.GetPerformance3Y(modelSelected) * 100m);
            addRow(grdReturnStatistics, "Total Return", backtestCalculator.MetricsCalculator.GetPerformanceInception(modelSelected) * 100m);
            double totalYears = PortfolioSnapshot.GetSnapshotVariationValues(backtestCalculator.MonthlySnapshots, modelSelected).Count() / 12;
            var cagr = Math.Pow((double)backtestCalculator.MetricsCalculator.GetPerformanceInception(modelSelected), 1f / totalYears) - 1d;
            addRow(grdReturnStatistics, "Compound ROR", (decimal)cagr*100m);
            addRow(grdReturnStatistics, "Winning Months (%)", backtestCalculator.MetricsCalculator.GetWinningMonths(modelSelected) * 100m);
            addRow(grdReturnStatistics, "Average Winning Month", backtestCalculator.MetricsCalculator.GetAverageWinningMonth(modelSelected) * 100m);
        }

        void fillRiskStatistics()
        {
            var metrics = MetricsCalculator.CalcMetrics(backtestCalculator, modelSelected, benchmarkSelected);
            grdRiskStats.Rows.Clear();

            addRow(grdRiskStats, "Sharpe Ratio", ((decimal)metrics[RiskMetrics.SharpeRatio]).ToString("n2"));
            addRow(grdRiskStats, "Sortino Ratio", ((decimal)metrics[RiskMetrics.SortinoRatio]).ToString("n2"));
            addRow(grdRiskStats, "Maximum Drawdown", -(decimal)metrics[RiskMetrics.DD1Plain] * 100m);
            addRow(grdRiskStats, "Correlation Vs Benchmark", ((decimal)metrics[RiskMetrics.BenchmarkCorrelation]).ToString("n2"));
            addRow(grdRiskStats, "Standard Deviation (monthly)", ((decimal)metrics[RiskMetrics.Volatility]).ToString("n2"));
            addRow(grdRiskStats, "Downside Deviation", ((decimal)metrics[RiskMetrics.DownsideDeviationMonthly]).ToString("n2"));
            addRow(grdRiskStats, "Beta", ((decimal)metrics[RiskMetrics.BetaLifetime]).ToString("n2"));
            addRow(grdRiskStats, "VaR Historical", Math.Abs(((decimal)metrics[RiskMetrics.VARHistorical])).ToString("n2"));
            addRow(grdRiskStats, "Average Losing Month", backtestCalculator.MetricsCalculator.GetAverageLosingMonth(modelSelected) * 100m);
        }

        void fillReturnReport()
        {
            int[] groups = new int[] { 1, 3, 6, 12, 12*3, 12*5, 12*10 };
            String[] groupNames = new String[] { "1 Month", "3 Months", "6 Months", "1 Year", "3 Years", "5 Years", "10 Years" };
            grdReturnReport.Rows.Clear();

            for (int groupIndex = 0; groupIndex < groups.Length; groupIndex++)
            {
                var revenues = backtestCalculator.MetricsCalculator.GetRevenueGroup(modelSelected, groups[groupIndex]).Select(x => x.Value).ToList();

                int row = grdReturnReport.Rows.Add();
                grdReturnReport.Rows[row].Cells[0].Value = groupNames[groupIndex];

                if (revenues.Count == 0)
                {
                    grdReturnReport.Rows[row].Cells[1].Value = "0.00";
                    grdReturnReport.Rows[row].Cells[2].Value = "0.00";
                    grdReturnReport.Rows[row].Cells[3].Value = "0.00";
                    grdReturnReport.Rows[row].Cells[4].Value = "0.00";
                    grdReturnReport.Rows[row].Cells[5].Value = "0.00";
                }
                else
                {
                    grdReturnReport.Rows[row].Cells[1].Value = Math.Round(revenues.Max() * 100, 2).ToString("n2");
                    grdReturnReport.Rows[row].Cells[2].Value = Math.Round(revenues.Min() * 100, 2).ToString("n2");
                    grdReturnReport.Rows[row].Cells[3].Value = Math.Round(revenues.Average() * 100, 2).ToString("n2");
                    grdReturnReport.Rows[row].Cells[4].Value = Math.Round(Feature.calcMedian(revenues) * 100, 2).ToString("n2");
                    grdReturnReport.Rows[row].Cells[5].Value = Math.Round(revenues.Last() * 100, 2).ToString("n2");
                }
            }
        }

        void fillDDReport()
        {
            var allDD = backtestCalculator.MetricsCalculator.GetAllDrawDowns(modelSelected);
            var allDDBenchmark = backtestCalculator.MetricsCalculator.GetAllDrawDowns(benchmarkSelected);
            grdDrawdownReport.Rows.Clear();
            colDDBenchmarkDepth.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colDDBenchmarkLength.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colDDDepth.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colDDEndDate.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colDDLenght.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colDDRecovery.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colDDStartDate.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            colDDBenchmarkDepth.FillWeight = 12;
            colDDBenchmarkLength.FillWeight = 12;
            colDDDepth.FillWeight = 12;
            colDDEndDate.FillWeight = 16;
            colDDStartDate.FillWeight = 16;
            colDDLenght.FillWeight = 12;
            colDDRecovery.FillWeight = 12;
            colDDNro.FillWeight = 8;

            for (int i = 0; i < 5; i++)
            {
                int row = grdDrawdownReport.Rows.Add();
                grdDrawdownReport.Rows[row].Cells[0].Value = i + 1;

                if (allDD.Count > i)
                {
                    grdDrawdownReport.Rows[row].Cells[1].Value = formatPercent(allDD[i].Depth);
                    grdDrawdownReport.Rows[row].Cells[2].Value = allDD[i].Months;
                    grdDrawdownReport.Rows[row].Cells[3].Value = allDD[i].Recovery;
                    grdDrawdownReport.Rows[row].Cells[4].Value = Utils.ConvertIntToDateTime(allDD[i].StartDate).ToString("MM/yyyy");
                    grdDrawdownReport.Rows[row].Cells[5].Value = Utils.ConvertIntToDateTime(allDD[i].EndDate).ToString("MM/yyyy");
                }
                else
                {
                    grdDrawdownReport.Rows[row].Cells[1].Value = "0";
                    grdDrawdownReport.Rows[row].Cells[2].Value = "0";
                    grdDrawdownReport.Rows[row].Cells[3].Value = "0";
                    grdDrawdownReport.Rows[row].Cells[4].Value = "";
                    grdDrawdownReport.Rows[row].Cells[5].Value = "";
                }

                if (allDDBenchmark.Count > i)
                {
                    grdDrawdownReport.Rows[row].Cells[6].Value = formatPercent(allDDBenchmark[i].Depth);
                    grdDrawdownReport.Rows[row].Cells[7].Value = allDDBenchmark[i].Months;
                }
                else
                {
                    grdDrawdownReport.Rows[row].Cells[6].Value = "0";
                    grdDrawdownReport.Rows[row].Cells[7].Value = "0";
                }
            }
        }

        void addRow(DataGridView grid, String name, decimal value)
        {
            int row = grid.Rows.Add();
            grid.Rows[row].Cells[0].Value = name;
            grid.Rows[row].Cells[1].Value = Math.Round(value, 2).ToString("n2") + "%";
        }

        String formatPercent(decimal value)
        {
            return Math.Round((value * 100m), 2).ToString("n2") + "%";
        }

        void addRow(DataGridView grid, String name, String value)
        {
            int row = grid.Rows.Add();
            grid.Rows[row].Cells[0].Value = name;
            grid.Rows[row].Cells[1].Value = value;
        }

        void drawEquityGraph()
        {
            chartPerformance.Series.Clear();
            chartPerformance.AxisX.Clear();
            chartPerformance.DisableAnimations = true;

            graphDrawdown.Series.Clear();
            graphDrawdown.AxisX.Clear();
            graphDrawdown.DisableAnimations = true;

            LiveCharts.SeriesCollection seriesToPlot = new LiveCharts.SeriesCollection();
            LiveCharts.SeriesCollection seriesToPlotDD = new LiveCharts.SeriesCollection();

            LineSeries newLine = new LineSeries();
            newLine.Title = "Model";
            newLine.Values = new ChartValues<decimal> { };
            newLine.PointGeometry = null;
            newLine.Stroke = System.Windows.Media.Brushes.Blue;
            newLine.Stroke.Freeze();

            //add the SPY line
            LiveCharts.Wpf.LineSeries spyLine = new LineSeries();
            spyLine.Title = "Benchmark";
            spyLine.Values = new ChartValues<decimal> { };
            spyLine.PointGeometry = null;
            spyLine.Stroke = System.Windows.Media.Brushes.Red;
            spyLine.Stroke.Freeze();

            LineSeries newLineDD = new LineSeries();
            newLineDD.Title = "Model";
            newLineDD.Values = new ChartValues<decimal> { };
            newLineDD.PointGeometry = null;
            newLineDD.Stroke = System.Windows.Media.Brushes.Blue;
            newLineDD.Stroke.Freeze();

            LineSeries spyLineDD = new LineSeries();
            spyLineDD.Title = "Benchmark";
            spyLineDD.Values = new ChartValues<decimal> { };
            spyLineDD.PointGeometry = null;
            spyLineDD.Stroke = System.Windows.Media.Brushes.Red;
            spyLineDD.Stroke.Freeze();

            newLine.LineSmoothness = 0;
            spyLine.LineSmoothness = 0;
            newLineDD.LineSmoothness = 0;
            spyLineDD.LineSmoothness = 0;

            var snapshots = backtestCalculator.MonthlySnapshots;
            if (snapshots.Count == 0)
                return;

            List<String> labels = new List<string>();

            List<ChartData> chartEquity = new List<ChartData>();
            List<ChartData> chartDD = new List<ChartData>();
            chartsDataToExport.Add("Equity Chart", chartEquity);
            chartsDataToExport.Add("Drawdown Chart", chartDD);

            decimal startingValue = 0;
            if (snapshots.Count > 0)
                startingValue = PortfolioSnapshot.GetPortfolioValue(snapshots[0], modelSelected);

            foreach (PortfolioSnapshot snapshot in snapshots)
            {
                decimal result = PortfolioSnapshot.GetPortfolioValue(snapshot, modelSelected);
                decimal spyValue = Math.Round(PortfolioSnapshot.GetPortfolioValue(snapshot, benchmarkSelected), 0);
                int decimals = 2;

                result = result / startingValue * (decimal)100 - 100;
                spyValue = spyValue / startingValue * (decimal)100 - 100;

                newLine.Values.Add(Math.Round(result, decimals));
                spyLine.Values.Add(Math.Round(spyValue, decimals));

                chartEquity.Add(new ChartData(snapshot.Date.ToString("MM/dd/yyyy"), Math.Round(result, decimals), Math.Round(spyValue, decimals)));
                
                labels.Add(snapshot.Date.Month == 1 ? snapshot.Date.ToString("yyyy") : "");
            }


            var snapshotsDD = backtestCalculator.BenchmarksCalculator.GetDailySnapshots(modelSelected);
            var snapshotsSpyDD = backtestCalculator.BenchmarksCalculator.GetDailySnapshots(benchmarkSelected);
            List<String> labelsDD = new List<string>();
            List<int> keyDates = snapshotsDD.Keys.OrderBy(x => x).ToList();
            if (snapshots.Count > 0)
                startingValue = snapshotsDD[keyDates[0]];
            decimal newMax = 0;
            decimal newMaxSpy = 0;
            int group = 0;
            var lastKey = keyDates.Last();
            decimal ddMin = 0;
            decimal ddMinSpy = 0;

            foreach (int key in keyDates)
            {
                group++;
                decimal result = snapshotsDD[key];
                decimal spyValue = Math.Round(snapshotsSpyDD[key], 0);
                int decimals = 2;

                result = result / startingValue * (decimal)100;
                spyValue = spyValue / startingValue * (decimal)100;

                newMax = Math.Max(newMax, result);
                newMaxSpy = Math.Max(newMaxSpy, spyValue);

                ddMin = Math.Min(ddMin, Math.Round((result - newMax) > 0 || newMax == 0 ? 0 : (result - newMax) / newMax * (decimal)100, decimals));
                ddMinSpy = Math.Min(ddMinSpy, Math.Round((spyValue - newMaxSpy) > 0 || newMaxSpy == 0 ? 0 : (spyValue - newMaxSpy) / newMaxSpy * (decimal)100, decimals));

                chartDD.Add(new ChartData(Utils.ConvertIntToDateTime(key).ToString("MM/dd/yyyy"), ddMin, ddMinSpy));

                if (group > 20 || key == lastKey)
                {
                    group = 0;

                    newLineDD.Values.Add(ddMin);
                    spyLineDD.Values.Add(ddMinSpy);

                    labelsDD.Add(key.ToString().Substring(0, 4));

                    ddMin = 0;
                    ddMinSpy = 0;
                }
            }


            newLine.Fill = System.Windows.Media.Brushes.Transparent;
            newLine.Fill.Freeze();
            newLine.DataLabels = false;

            spyLine.Fill = System.Windows.Media.Brushes.Transparent;
            spyLine.Fill.Freeze();
            spyLine.DataLabels = false;

            newLineDD.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(40, 0, 0, 255));
            newLineDD.Fill.Freeze();
            newLineDD.DataLabels = false;
            newLineDD.AreaLimit = 0;

            spyLineDD.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(40, 255, 0, 0));
            spyLineDD.Fill.Freeze();
            spyLineDD.DataLabels = false;
            spyLineDD.AreaLimit = 0;

            seriesToPlot.Add(spyLine);
            seriesToPlot.Add(newLine);

            seriesToPlotDD.Add(spyLineDD);
            seriesToPlotDD.Add(newLineDD);

            chartPerformance.Series = seriesToPlot;

            chartPerformance.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "",
                Labels = labels,
            });

            chartPerformance.AxisX[0].LabelsRotation = 45;
            chartPerformance.AxisX[0].Separator.Step = 1;
            chartPerformance.AxisX[0].Separator.StrokeThickness = 0;
            chartPerformance.LegendLocation = LegendLocation.Bottom;
            chartPerformance.AxisY.Clear();
            chartPerformance.AxisY.Add(new Axis() { LabelFormatter = value => value.ToString("n0") + "%" });

            graphDrawdown.Series = seriesToPlotDD;

            graphDrawdown.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "",
                Labels = labelsDD,
            });
            
            graphDrawdown.AxisX[0].Separator.StrokeThickness = 0;
            graphDrawdown.LegendLocation = LegendLocation.Bottom;
            graphDrawdown.AxisY.Clear();
            graphDrawdown.AxisY.Add(new Axis() { LabelFormatter = value => value.ToString("n0") + "%" });
        }

        void drawRORGraph()
        {
            LiveCharts.SeriesCollection seriesToPlot = new LiveCharts.SeriesCollection();

            graphROR.Series.Clear();

            var labels = new List<String>();

            var values2 = getValuesROR(benchmarkSelected);
            labels = plotGraphROR(1, values2, seriesToPlot, false);
            var values1 = getValuesROR(modelSelected);
            labels = plotGraphROR(0, values1, seriesToPlot, false);

            AddValues("ROR Chart", labels, values1, values2);

            graphROR.AxisX.Clear();
            graphROR.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "",
                Labels = labels
            });

            graphROR.LegendLocation = LegendLocation.Bottom;
            graphROR.Series = seriesToPlot;
            graphROR.AxisX[0].Separator.StrokeThickness = 0;
            graphROR.DisableAnimations = true;
            graphROR.AxisY.Clear();
            graphROR.AxisY.Add(new Axis() { LabelFormatter = value => value.ToString("n0") + "%" });
        }

        void AddValues(String name, List<String> labels, Dictionary<int, decimal> values1, Dictionary<int, decimal> values2)
        {
            List<ChartData> chartData = new List<ChartData>();
            chartsDataToExport.Add(name, chartData);

            var keys = values1.Keys.OrderBy(x => x);
            foreach (var key in keys)
            {
                chartData.Add(new ChartData(Utils.ConvertIntToDateTime(key).AddDays(1).ToString("MM/dd/yyyy"), values1[key], values2[key]));
            }
        }

        void drawVolatilityGraph()
        {
            LiveCharts.SeriesCollection seriesToPlot = new LiveCharts.SeriesCollection();
            graphVolatility.Series.Clear();

            var labels = new List<String>();

            var values2 = getVolatilityROR(benchmarkSelected);
            labels = plotGraphROR(1, values2, seriesToPlot, true);
            var values1 = getVolatilityROR(modelSelected);
            labels = plotGraphROR(0, values1, seriesToPlot, true);

            AddValues("Volatility Chart", labels, values1, values2);

            graphVolatility.AxisX.Clear();
            graphVolatility.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "",
                Labels = labels
            });

            graphVolatility.LegendLocation = LegendLocation.Bottom;
            graphVolatility.AxisX[0].Separator.StrokeThickness = 0;
            graphVolatility.Series = seriesToPlot;
            graphVolatility.DisableAnimations = true;
        }

        List<String> plotGraphROR(int selectedMetric, Dictionary<int, decimal> values, LiveCharts.SeriesCollection seriesToPlot, bool transparent)
        {
            LiveCharts.Wpf.LineSeries newLine = new LineSeries();
            newLine.Values = new ChartValues<decimal> { };
            newLine.PointGeometry = null;
            newLine.LineSmoothness = 0;
            newLine.Fill = System.Windows.Media.Brushes.Transparent;
            newLine.DataLabels = false;
            newLine.AreaLimit = 0;

            switch (selectedMetric)
            {
                case 0:
                    newLine.Stroke = System.Windows.Media.Brushes.Blue;
                    if(!transparent)
                        newLine.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(40, 0, 0, 255));
                    newLine.Title = "Model";
                    break;
                case 1:
                    newLine.Stroke = System.Windows.Media.Brushes.Red;
                    if (!transparent)
                        newLine.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(40, 255, 0, 0));
                    newLine.Title = "Benchmark";
                    break;
            }
            newLine.Stroke.Freeze();
            newLine.Fill.Freeze();

            List<String> labels = new List<string>();

            var keys = values.Keys.OrderBy(x => x);
            foreach (var key in keys)
            {
                var value = values[key];
                newLine.Values.Add(Math.Round(value, 2));
                if (keys.Last() == key)
                    labels.Add(Utils.ConvertIntToDateTime(key).AddDays(1).ToString("yyyy"));
                else
                    labels.Add(Utils.ConvertIntToDateTime(key).AddDays(1).ToString("yyyy"));
            }

            seriesToPlot.Add(newLine);

            return labels;
        }

        Dictionary<int, decimal> getValuesROR(String selectedMetric)
        {
            var values = PortfolioSnapshot.GetSnapshotVariationValues(this.backtestCalculator.MonthlySnapshots, selectedMetric);
            var rollingValues = new Dictionary<int, decimal>();

            //calc rolling values
            foreach (var date in values)
            {
                //get en of month of start date
                var datetimevalue = Utils.ConvertIntToDateTime(date.Date);
                var startDate = Utils.ConvertDateTimeToInt(new DateTime(datetimevalue.Year - 1, datetimevalue.Month, DateTime.DaysInMonth(datetimevalue.Year - 1, datetimevalue.Month)));

                var rollingVariation = values.Where(x => x.Date > startDate && x.Date <= date.Date).Aggregate(1m, (x, y) => x * (1m + y.Value)) - 1;

                rollingValues.Add(date.Date, rollingVariation * 100);
            }

            return rollingValues;
        }

        Dictionary<int, decimal> getVolatilityROR(String selectedMetric)
        {
            var values = PortfolioSnapshot.GetSnapshotVariationValues(this.backtestCalculator.MonthlySnapshots, selectedMetric);
            var rollingValues = new Dictionary<int, decimal>();

            //calc rolling values
            foreach (var date in values)
            {
                //get en of month of start date
                var datetimevalue = Utils.ConvertIntToDateTime(date.Date);
                var startDate = Utils.ConvertDateTimeToInt(new DateTime(datetimevalue.Year - 1, datetimevalue.Month, DateTime.DaysInMonth(datetimevalue.Year - 1, datetimevalue.Month)));

                var rollingVariation = DataUpdater.CalcStdDev(values.Where(x => x.Date > startDate && x.Date <= date.Date).Select(x => x.Value));

                rollingValues.Add(date.Date, rollingVariation * 100);
            }

            return rollingValues;
        }

        void paintCell(DataGridViewTextBoxCell cell, Boolean totals)
        {
            if ((decimal)cell.Value > 0)
            {
                cell.Style.BackColor = Color.FromArgb(220, 241, 202);
                cell.Style.ForeColor = Color.FromArgb(0, 128, 0);

                if (totals)
                {
                    //cell.Style.BackColor = Color.FromArgb(194,225,166);
                    cell.Style.Font = new Font(grdMonthlyPerformance.Font, FontStyle.Bold);
                }
                return;
            }

            if ((decimal)cell.Value < 0)
            {
                cell.Style.BackColor = Color.FromArgb(237, 216, 216);
                cell.Style.ForeColor = Color.FromArgb(255, 0, 0);

                if (totals)
                {
                    //cell.Style.BackColor = Color.FromArgb(220,173,173);
                    cell.Style.Font = new Font(grdMonthlyPerformance.Font, FontStyle.Bold);
                }
                return;
            }

            cell.Style.BackColor = Color.FromArgb(243, 243, 243);
            cell.Style.ForeColor = Color.FromArgb(109, 109, 109);

            if (totals)
            {
                //cell.Style.BackColor = Color.FromArgb(225,225,225);
                cell.Style.Font = new Font(grdMonthlyPerformance.Font, FontStyle.Bold);
            }
        }

        private void PerformanceReport_Load(object sender, EventArgs e)
        {

        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridBasicData_Leave(object sender, EventArgs e)
        {
            ((DataGridView)sender).ClearSelection();
        }

        private void txtModelDescription_TextChanged(object sender, EventArgs e)
        {
            tmrSaveDescription.Stop();
            tmrSaveDescription.Start();
        }

        private void tmrSaveDescription_Tick(object sender, EventArgs e)
        {
            tmrSaveDescription.Stop();
            Strategy.SaveStrategyDescription(this.backtestCalculator.Portfolio.Id, txtModelDescription.Text);
        }

        private void cboChangeModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshModels();
        }

        private void cmdExcelExport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Excel Files|*.xlsx";
            saveFile.AddExtension = true;

            if(saveFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(saveFile.FileName, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    // Add a WorkbookPart to the document.
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                    DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());

                    uint sheetId = 1;
                    exportGrid("Basic Data", gridBasicData, workbookPart, sheets, sheetId++);
                    exportGrid("Return Statistics", grdReturnStatistics, workbookPart, sheets, sheetId++);
                    exportGrid("Risk Statistics", grdRiskStats, workbookPart, sheets, sheetId++);
                    exportGrid("Monthly Performance", grdMonthlyPerformance, workbookPart, sheets, sheetId++);
                    exportGrid("Return Report", grdReturnReport, workbookPart, sheets, sheetId++);
                    exportGrid("Drawdow Report", grdDrawdownReport, workbookPart, sheets, sheetId++);

                    exportGraphsData(workbookPart, sheets, sheetId++);

                    workbookPart.Workbook.Save();
                }

                MessageBox.Show("File generation completed successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error on the file generation. " + ex.Message);
            }
        }

        void exportGrid(String name, DataGridView grid, WorkbookPart workbookPart, DocumentFormat.OpenXml.Spreadsheet.Sheets sheets, uint sheetId)
        {
            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

            DocumentFormat.OpenXml.Spreadsheet.Worksheet workSheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet();
            DocumentFormat.OpenXml.Spreadsheet.SheetData sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
            
            DocumentFormat.OpenXml.Spreadsheet.Row rowHeader = new DocumentFormat.OpenXml.Spreadsheet.Row();
            foreach (DataGridViewColumn col in grid.Columns)
                rowHeader.Append(ConstructCell(col.HeaderText, DocumentFormat.OpenXml.Spreadsheet.CellValues.String));

            sheetData.AppendChild(rowHeader);

            foreach (DataGridViewRow row in grid.Rows)
            {
                DocumentFormat.OpenXml.Spreadsheet.Row rowData = new DocumentFormat.OpenXml.Spreadsheet.Row();

                for(int i = 0; i < grid.Columns.Count; i++)
                {
                    if (i == 0 || grid.Columns[i] == colDDStartDate || grid.Columns[i] == colDDEndDate)
                    {
                        rowData.Append(ConstructCell(row.Cells[i].Value == null ? "" : row.Cells[i].Value.ToString(), DocumentFormat.OpenXml.Spreadsheet.CellValues.String));
                    }
                    else
                        rowData.Append(ConstructCell(row.Cells[i].Value == null ? "0" : row.Cells[i].Value.ToString().Replace("%", ""), DocumentFormat.OpenXml.Spreadsheet.CellValues.Number));
                }

                sheetData.AppendChild(rowData);
            }

            workSheet.AppendChild(sheetData);
            worksheetPart.Worksheet = workSheet;

            DocumentFormat.OpenXml.Spreadsheet.Sheet sheet1 = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = new DocumentFormat.OpenXml.UInt32Value(sheetId),
                Name = name
            };
            sheets.Append(sheet1);
        }

        void exportGraphsData(WorkbookPart workbookPart, DocumentFormat.OpenXml.Spreadsheet.Sheets sheets, uint sheetId)
        {
            foreach (String key in chartsDataToExport.Keys)
            {
                // Add a WorksheetPart to the WorkbookPart.
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

                DocumentFormat.OpenXml.Spreadsheet.Worksheet workSheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet();
                DocumentFormat.OpenXml.Spreadsheet.SheetData sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();

                DocumentFormat.OpenXml.Spreadsheet.Row rowHeader = new DocumentFormat.OpenXml.Spreadsheet.Row();
                rowHeader.Append(ConstructCell("Labels", DocumentFormat.OpenXml.Spreadsheet.CellValues.String));
                rowHeader.Append(ConstructCell("Model", DocumentFormat.OpenXml.Spreadsheet.CellValues.String));
                rowHeader.Append(ConstructCell("Benchmark", DocumentFormat.OpenXml.Spreadsheet.CellValues.String));

                sheetData.AppendChild(rowHeader);


                foreach (var data in chartsDataToExport[key])
                {
                    DocumentFormat.OpenXml.Spreadsheet.Row rowData = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    rowData.Append(ConstructCell(data.Label, DocumentFormat.OpenXml.Spreadsheet.CellValues.String));
                    rowData.Append(ConstructCell(data.Data1.ToString(), DocumentFormat.OpenXml.Spreadsheet.CellValues.Number));
                    rowData.Append(ConstructCell(data.Data2.ToString(), DocumentFormat.OpenXml.Spreadsheet.CellValues.Number));

                    sheetData.AppendChild(rowData);
                }

                workSheet.AppendChild(sheetData);
                worksheetPart.Worksheet = workSheet;

                DocumentFormat.OpenXml.Spreadsheet.Sheet sheet1 = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = new DocumentFormat.OpenXml.UInt32Value(sheetId++),
                    Name = key
                };

                sheets.Append(sheet1);
            }
        }
        
        private DocumentFormat.OpenXml.Spreadsheet.Cell ConstructCell(string value, DocumentFormat.OpenXml.Spreadsheet.CellValues dataType)
        {
            if(dataType == DocumentFormat.OpenXml.Spreadsheet.CellValues.Number)
                value = value.Replace(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator, "");

            return new DocumentFormat.OpenXml.Spreadsheet.Cell()
            {
                CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(value),
                DataType = new DocumentFormat.OpenXml.EnumValue<DocumentFormat.OpenXml.Spreadsheet.CellValues>(dataType)
            };
        }

        private class ChartData
        {
            public String Label;
            public decimal Data1;
            public decimal Data2;

            public ChartData(String label, decimal data1, decimal data2)
            {
                this.Label = label;
                this.Data1 = data1;
                this.Data2 = data2;
            }
        }


    }
}
