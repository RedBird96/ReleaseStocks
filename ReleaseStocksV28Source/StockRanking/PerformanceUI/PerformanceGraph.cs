using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using StockRanking.Benchmarks;

namespace StockRanking
{
    public partial class PerformanceGraph : Form
    {
        List<PortfolioSnapshot> snapshots;
        BacktestCalculator backtestCalculator;

        public PerformanceGraph(List<PortfolioSnapshot> snapshots, BacktestCalculator backtestCalculator)
        {
            this.snapshots = snapshots;
            this.backtestCalculator = backtestCalculator;

            InitializeComponent();

            refreshGraphs();

            refreshTable();

            calcGainLoss();

            backtestCalculator.BenchmarksCalculator.ExportDebugLogs();
        }

        void calcGainLoss()
        {
            decimal largestGain = 0;
            decimal largestLoss = 0;
            decimal lastValue = 0;
            List<decimal> allValues = new List<decimal>();
            
            foreach (PortfolioSnapshot snap in snapshots)
            {
                if(lastValue != 0)
                {
                    decimal currVal = snap.TotalPortfolioValue;

                    decimal gain = (currVal - lastValue) / lastValue;
                    largestGain = Math.Max(gain, largestGain);
                    largestLoss = Math.Min(gain, largestLoss);
                    allValues.Add(gain);
                }

                lastValue = snap.TotalPortfolioValue;
            }

            var volatility = DataUpdater.CalcStdDev(allValues);

            lblLargestGain.Text = "  " + (largestGain * 100).ToString("n2") + " %";
            lblLargestLoss.Text = " " + (largestLoss * 100).ToString("n2") + " %";
            lblVolatility.Text = " " + (volatility * 100).ToString("n2") + " %";
        }

        void refreshTable()
        {
            grdIndicators.Rows.Clear();
            PortfolioSnapshot qtdSnapshot = getQuarterSnapshot();
            DateTime mtdDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            PortfolioSnapshot mtdSnapshot = snapshots.Find(x => x.Date.Year == mtdDate.Year && x.Date.Month == mtdDate.Month && x.Date.Day == mtdDate.Day);
            PortfolioSnapshot ytdSnapshot = snapshots.Find(x => x.Date.Year == DateTime.Now.Year-1 && x.Date.Month == 12 && x.Date.Day == 31);

            if (mtdSnapshot == null)
                mtdSnapshot = new PortfolioSnapshot();
            if (ytdSnapshot == null)
                ytdSnapshot = new PortfolioSnapshot();
            if (qtdSnapshot == null)
                qtdSnapshot = new PortfolioSnapshot();

            var performanceBenchmarks = PortfolioParameters.GetPerformanceGraphBenchmarkSymbols();

            addRow("Equity", mtdSnapshot.TotalPortfolioValue, qtdSnapshot.TotalPortfolioValue, ytdSnapshot.TotalPortfolioValue, backtestCalculator.Snapshot6Months.TotalPortfolioValue, backtestCalculator.Snapshot1Year.TotalPortfolioValue, backtestCalculator.Snapshot3Year.TotalPortfolioValue, backtestCalculator.Snapshot5Year.TotalPortfolioValue, backtestCalculator.Snapshot10Year.TotalPortfolioValue, snapshots[0].TotalPortfolioValue, snapshots[snapshots.Count - 1].TotalPortfolioValue);
            addRow("Blended Advanced", mtdSnapshot.BlendedAdvancedBenchmark, qtdSnapshot.BlendedAdvancedBenchmark, ytdSnapshot.BlendedAdvancedBenchmark, backtestCalculator.Snapshot6Months.BlendedAdvancedBenchmark, backtestCalculator.Snapshot1Year.BlendedAdvancedBenchmark, backtestCalculator.Snapshot3Year.BlendedAdvancedBenchmark, backtestCalculator.Snapshot5Year.BlendedAdvancedBenchmark, backtestCalculator.Snapshot10Year.BlendedAdvancedBenchmark, snapshots[0].BlendedAdvancedBenchmark, snapshots[snapshots.Count - 1].BlendedAdvancedBenchmark);
            addRow("Blended Basic", mtdSnapshot.BlendedBasicBenchmark, qtdSnapshot.BlendedBasicBenchmark, ytdSnapshot.BlendedBasicBenchmark, backtestCalculator.Snapshot6Months.BlendedBasicBenchmark, backtestCalculator.Snapshot1Year.BlendedBasicBenchmark, backtestCalculator.Snapshot3Year.BlendedBasicBenchmark, backtestCalculator.Snapshot5Year.BlendedBasicBenchmark, backtestCalculator.Snapshot10Year.BlendedBasicBenchmark, snapshots[0].BlendedBasicBenchmark, snapshots[snapshots.Count - 1].BlendedBasicBenchmark);
            addRow("Bonds Model", mtdSnapshot.BondsBenchmark, qtdSnapshot.BondsBenchmark, ytdSnapshot.BondsBenchmark, backtestCalculator.Snapshot6Months.BondsBenchmark, backtestCalculator.Snapshot1Year.BondsBenchmark, backtestCalculator.Snapshot3Year.BondsBenchmark, backtestCalculator.Snapshot5Year.BondsBenchmark, backtestCalculator.Snapshot10Year.BondsBenchmark, snapshots[0].BondsBenchmark, snapshots[snapshots.Count - 1].BondsBenchmark);
            addRow("SPY", mtdSnapshot.SpyBenchmark, qtdSnapshot.SpyBenchmark, ytdSnapshot.SpyBenchmark, backtestCalculator.Snapshot6Months.SpyBenchmark, backtestCalculator.Snapshot1Year.SpyBenchmark, backtestCalculator.Snapshot3Year.SpyBenchmark, backtestCalculator.Snapshot5Year.SpyBenchmark, backtestCalculator.Snapshot10Year.SpyBenchmark, snapshots[0].SpyBenchmark, snapshots[snapshots.Count - 1].SpyBenchmark);
            addRow("Benchmark", mtdSnapshot.BenchmarkSymbolBenchmark, qtdSnapshot.BenchmarkSymbolBenchmark, ytdSnapshot.BenchmarkSymbolBenchmark, backtestCalculator.Snapshot6Months.BenchmarkSymbolBenchmark, backtestCalculator.Snapshot1Year.BenchmarkSymbolBenchmark, backtestCalculator.Snapshot3Year.BenchmarkSymbolBenchmark, backtestCalculator.Snapshot5Year.BenchmarkSymbolBenchmark, backtestCalculator.Snapshot10Year.BenchmarkSymbolBenchmark, snapshots[0].BenchmarkSymbolBenchmark, snapshots[snapshots.Count - 1].BenchmarkSymbolBenchmark);
            foreach (var benchmarkKey in mtdSnapshot.BenchmarksList.Keys)
                if(performanceBenchmarks.Contains(benchmarkKey))
                    addRow("Benchmark " + benchmarkKey, mtdSnapshot.BenchmarksList[benchmarkKey], qtdSnapshot.BenchmarksList[benchmarkKey], ytdSnapshot.BenchmarksList[benchmarkKey], backtestCalculator.Snapshot6Months.BenchmarksList[benchmarkKey], backtestCalculator.Snapshot1Year.BenchmarksList[benchmarkKey], backtestCalculator.Snapshot3Year.BenchmarksList[benchmarkKey], backtestCalculator.Snapshot5Year.BenchmarksList[benchmarkKey], backtestCalculator.Snapshot10Year.BenchmarksList[benchmarkKey], snapshots[0].BenchmarksList[benchmarkKey], snapshots[snapshots.Count - 1].BenchmarksList[benchmarkKey]);
        }

        PortfolioSnapshot getQuarterSnapshot()
        {
            int quarterStart = 1;

            switch(DateTime.Now.Month)
            {
                case 1:
                case 2:
                case 3:
                    quarterStart = 1;
                    break;
                case 4:
                case 5:
                case 6:
                    quarterStart = 4;
                    break;
                case 7:
                case 8:
                case 9:
                    quarterStart = 7;
                    break;
                case 10:
                case 11:
                case 12:
                    quarterStart = 10;
                    break;
            }

            DateTime quarter = new DateTime(DateTime.Now.Year, quarterStart, 1).AddDays(-1);
            return snapshots.Find(x => x.Date.Year == quarter.Year && x.Date.Month == quarter.Month && x.Date.Day == quarter.Day);
        }
        
        void addRow(string name, decimal mtd, decimal qtd, decimal ytd, decimal m6, decimal y1, decimal y3, decimal y5, decimal y10, decimal start, decimal last)
        {  
            int n = grdIndicators.Rows.Add();

            grdIndicators.Rows[n].Cells[0].Value = name;
            grdIndicators.Rows[n].Cells[1].Value = mtd == 0 ? 0 : (last - mtd) / mtd * 100;
            grdIndicators.Rows[n].Cells[2].Value = qtd == 0 ? 0 : (last - qtd) / qtd * 100;
            grdIndicators.Rows[n].Cells[3].Value = ytd == 0 ? 0 : (last - ytd) / ytd * 100;
            grdIndicators.Rows[n].Cells[4].Value = m6 == 0 ? 0 : (last - m6) / m6 * 100;
            grdIndicators.Rows[n].Cells[5].Value = y1 == 0 ? 0 : (last - y1) / y1 * 100;
            grdIndicators.Rows[n].Cells[6].Value = y3 == 0 ? 0 : (last - y3) / y3 * 100;
            grdIndicators.Rows[n].Cells[7].Value = y5 == 0 ? 0 : (last - y5) / y5 * 100;
            grdIndicators.Rows[n].Cells[8].Value = y10 == 0 ? 0 : (last - y10) / y10 * 100;
            grdIndicators.Rows[n].Cells[9].Value = start == 0 ? 0 : (last - start) / start * 100;
        }

        void refreshGraphs()
        {
            LiveCharts.SeriesCollection seriesToPlot = new LiveCharts.SeriesCollection();

            var labels = new List<String>();

            if (chkEquity.Checked)
            {
                var values1 = getValues(EquityBenchmark.CODE);
                labels = plotGraph(0, values1, seriesToPlot);
            }
            if (chkBlendedAdvanced.Checked)
            {
                var values2 = getValues(BlendedAdvancedBenchmark.CODE);
                labels = plotGraph(1, values2, seriesToPlot);
            }
            if (chkBlendedBasic.Checked)
            {
                var values3 = getValues(BlendedBasicBenchmark.CODE);
                labels = plotGraph(2, values3, seriesToPlot);
            }
            if (chkFixedIncome.Checked)
            {
                var values4 = getValues(BondsModelBenchmark.CODE);
                labels = plotGraph(3, values4, seriesToPlot);
            }

            chartPerformance.AxisX.Clear();
            chartPerformance.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Portfolio",
                Labels = labels
            });
            /*chartPerformance.AxisX[0].Separator.Step = 1;
            if (backtestCalculator.getMonthsInterval() == 1)
                chartPerformance.AxisX[0].Separator.Step = 3;
            */
            chartPerformance.AxisX[0].LabelsRotation = 45;
            chartPerformance.LegendLocation = LegendLocation.None;

            chartPerformance.Series = seriesToPlot;
        }

        List<String> plotGraph(int selectedMetric, Dictionary<int, decimal> values, LiveCharts.SeriesCollection seriesToPlot)
        {
            LiveCharts.Wpf.LineSeries newLine = new LineSeries();
            newLine.Values = new ChartValues<decimal> { };
            newLine.PointGeometry = null;
            newLine.LineSmoothness = 0;
            newLine.Fill = System.Windows.Media.Brushes.Transparent;
            newLine.Fill.Freeze();
            newLine.DataLabels = false;

            switch (selectedMetric)
            {
                case 0:
                    newLine.Stroke = System.Windows.Media.Brushes.Blue;
                    newLine.Title = "Equity";
                    break;
                case 1:
                    newLine.Stroke = System.Windows.Media.Brushes.Yellow;
                    newLine.Title = "Blended Advanced";
                    break;
                case 2:
                    newLine.Stroke = System.Windows.Media.Brushes.Orange;
                    newLine.Title = "Blended Basic";
                    break;
                case 3:
                    newLine.Stroke = System.Windows.Media.Brushes.Violet;
                    newLine.Title = "Bonds Model";
                    break;
            }
            newLine.Stroke.Freeze();

            List<String> labels = new List<string>();

            var keys = values.Keys.OrderBy(x => x);
            foreach (var key in keys)
            {
                var value = values[key];
                newLine.Values.Add(Math.Round(value, 2));
                if (keys.Last() == key)
                    labels.Add(Utils.ConvertIntToDateTime(key).AddDays(1).ToString("dd/MM/yyyy"));
                else
                    labels.Add(Utils.ConvertIntToDateTime(key).AddDays(1).ToString("MM/yyyy"));
            }

            seriesToPlot.Add(newLine);

            return labels;
        }

        Dictionary<int, decimal> getValues(String selectedMetric)
        {
            var values = PortfolioSnapshot.GetSnapshotVariationValues(snapshots, selectedMetric);
            var rollingValues = new Dictionary<int, decimal>();

            //calc rolling values
            foreach (var date in values)
            {
                //get en of month of start date
                var datetimevalue = Utils.ConvertIntToDateTime(date.Date);
                var startDate = Utils.ConvertDateTimeToInt(new DateTime(datetimevalue.Year - (int)txtRollingYears.Value, datetimevalue.Month, DateTime.DaysInMonth(datetimevalue.Year- (int)txtRollingYears.Value, datetimevalue.Month)));

                var rollingVariation = values.Where(x => x.Date > startDate && x.Date <= date.Date).Aggregate(1m, (x, y) => x * (1m + y.Value))-1;

                rollingValues.Add(date.Date, rollingVariation * 100);
            }
            
            return rollingValues;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            refreshGraphs();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void chkEquity_CheckedChanged(object sender, EventArgs e)
        {
            refreshGraphs();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdPerformanceGraph_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EditBenchmarksPopup popup = new EditBenchmarksPopup();
            if(popup.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Please run this backtest again in order to update benchmarks.");
            }
        }
    }
}
