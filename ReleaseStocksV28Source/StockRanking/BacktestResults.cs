using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts.Wpf;
using LiveCharts.WinForms;
using LiveCharts;
using LiveCharts.Defaults;
using StockRanking.Benchmarks;
using StockRanking.Entities;

namespace StockRanking
{
    public partial class BacktestResults : Form
    {
        public static bool ShowPerformanceGraphOnNextRun = false;

        BacktestCalculator backtestCalculator;
        List<PortfolioSnapshot> snapshots = null;
        List<PortfolioSnapshot> snapshotTransactions = null;
        
        bool chkEquity = true;
        bool chkAlpha = false;
        bool chkBenchmark = true;
        bool chkPercent = true;
        bool chkBlendedAdv = false;
        bool chkBlendedBasic = false;
        bool chkBondsModel = false;
        bool initializing = true;

        bool isShowHistory = false;
        bool isFilterOn = false;
        DateTime filterDate;
        List<Trade> tradelist = new List<Trade>();
        List<TradeHistory> historylist = new List<TradeHistory>();

        bool showPerformanceGraph = false;

        bool isFromOptimal = false;

        decimal maxLabelsValue = 0;

        public BacktestResults(BacktestCalculator calculator, bool isOptimal = false)
        {
            InitializeComponent();

            showPerformanceGraph = ShowPerformanceGraphOnNextRun;
            ShowPerformanceGraphOnNextRun = false;

            this.backtestCalculator = calculator;

            if (isOptimal == false) pnlProcessingProgress.CancelProcess += this.PnlProcessing_CancelProcess;
            
            Type dgvType = grdTrades.GetType();
            System.Reflection.PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            pi.SetValue(grdTrades, true, null);

            Type dghType = dataGridHistory.GetType();
            System.Reflection.PropertyInfo pih = dgvType.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            pih.SetValue(dataGridHistory, true, null);

            if (WindowState == FormWindowState.Normal)
            {
                if (Properties.Settings.Default.FormSizeBacktest.Width > 0)
                    Size = Properties.Settings.Default.FormSizeBacktest;
            }

            grdTrades.AutoGenerateColumns = false;
            dataGridHistory.AutoGenerateColumns = false;

            //default checked items
            lstSelectedLines.SetItemChecked(0, true);
            lstSelectedLines.SetItemChecked(5, true);

            initializing = false;

            isFromOptimal = isOptimal;
        }

        bool generatingGraph = false;
        void generateGraph(bool refreshData)
        {
            if (generatingGraph)
                return;

            generatingGraph = true;

            //calc backtest
            BacktestCalculator.CancelProcess = false;
            StrategyView.CancelResultsProcessing = false;
            maxLabelsValue = 0;

            if (snapshots == null || refreshData)
            {
                pnlProcessingProgress.StartProcess();

                BacktestResult outCach = new BacktestResult();

                if (snapshots == null && isFromOptimal)
                {
                    snapshotTransactions = backtestCalculator.Snapshots;
                    snapshots = backtestCalculator.MonthlySnapshots;
                    historylist = backtestCalculator.TradeHistories;
                } else
                {
                    snapshotTransactions = backtestCalculator.RunBacktest(pnlProcessingProgress, dateFrom.Value, out outCach);
                    snapshots = backtestCalculator.MonthlySnapshots;
                    historylist = backtestCalculator.TradeHistories;
                }

                pnlProcessingProgress.StopProcess();
            }


            if (snapshots == null || snapshotTransactions == null)
            {
                if (StrategyView.CancelResultsProcessing)
                {
                    this.Close();
                }

                generatingGraph = false;
                return;
            }


            tradelist.Clear();
            foreach (PortfolioSnapshot snap in snapshotTransactions)
            {
                tradelist.AddRange(snap.Trades);
            }

            tradelist = tradelist.OrderBy(x => x.ExitDate).ToList();
            grdTrades.DataSource = new SortedBindingList<Trade>(tradelist);

            historylist.Sort(delegate (TradeHistory x, TradeHistory y)
            {
                if (x.Date != y.Date) return x.Date.CompareTo(y.Date);
                return y.IsExit.CompareTo(x.IsExit);
            });
            dataGridHistory.DataSource = new SortedBindingList<TradeHistory>(historylist);

            //render graphs
            LiveCharts.SeriesCollection seriesToPlot = new LiveCharts.SeriesCollection();

            //drawdown graphs
            chartDrawdown.Series.Clear();
            chartDrawdown.AxisX.Clear();
            chartDrawdown.DisableAnimations = true;
            try
            {
                chartDrawdown.AxisY[0].MaxValue = 0;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            chartEquity.Series.Clear();
            chartEquity.AxisX.Clear();
            chartEquity.DisableAnimations = true;

            try
            {
                if (chkAlpha)
                    chartEquity.AxisY[0].MinValue = double.NaN;
                else
                    chartEquity.AxisY[0].MinValue = 0;
            } catch (Exception ex)
            {
                
            }
            

            LiveCharts.SeriesCollection seriesToPlotDD = new LiveCharts.SeriesCollection();
            
            List<String> labels = new List<string>();

            if (chkBondsModel)
                labels = plotLine("Bond Model", BondsModelBenchmark.CODE, System.Windows.Media.Brushes.Violet, seriesToPlot, seriesToPlotDD);
            if (chkBlendedBasic)
                labels = plotLine("Blended Basic", BlendedBasicBenchmark.CODE, System.Windows.Media.Brushes.Orange, seriesToPlot, seriesToPlotDD);
            if (chkBlendedAdv)
                labels = plotLine("Blended Adv", BlendedAdvancedBenchmark.CODE, System.Windows.Media.Brushes.Yellow, seriesToPlot, seriesToPlotDD);
            if (chkBenchmark)
                labels = plotLine("Buy & Hold", SymbolBenchmark.BENCHMARK_CODE, System.Windows.Media.Brushes.Red, seriesToPlot, seriesToPlotDD);
            if (chkEquity)
                labels = plotLine("Strategy", EquityBenchmark.CODE, System.Windows.Media.Brushes.Blue, seriesToPlot, seriesToPlotDD);
            
            chartEquity.Series = seriesToPlot;

            chartEquity.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Portfolio",
                Labels = labels
            });
            chartEquity.AxisX[0].Separator.Step = 8;
            chartEquity.AxisX[0].LabelsRotation = 45;
            
            chartEquity.LegendLocation = LegendLocation.None;
            
            chartDrawdown.Series = seriesToPlotDD;
            chartDrawdown.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Portfolio",
                Labels = labels
            });
            chartDrawdown.LegendLocation = LegendLocation.None;
            chartDrawdown.AxisX[0].Separator.Step = 8;
            chartDrawdown.AxisX[0].LabelsRotation = 45;

            chartDrawdown.AxisX[0].IsMerged = false;
            chartEquity.AxisX[0].IsMerged = false;

            try
            {
                chartDrawdown.AxisY[0].IsMerged = false;
                chartEquity.AxisY[0].IsMerged = false;

                chartDrawdown.AxisY[0].FontFamily = new System.Windows.Media.FontFamily("Courier New");
                chartEquity.AxisY[0].FontFamily = new System.Windows.Media.FontFamily("Courier New");
            } catch (Exception ex)
            {
            }
            

            int totalChars = Math.Round(maxLabelsValue, 0).ToString().Length;
            

            generatingGraph = false;
        }

        private List<String> plotLine(String name, String benchmarkType, System.Windows.Media.Brush color, LiveCharts.SeriesCollection seriesToPlot, LiveCharts.SeriesCollection seriesToPlotDD)
        {
            LiveCharts.Wpf.LineSeries newLine = new LineSeries();
            newLine.Title = name;
            newLine.Values = new ChartValues<decimal> { };
            newLine.PointGeometry = null;
            newLine.Stroke = color;
            newLine.Stroke.Freeze();
            newLine.LineSmoothness = 0;

            LineSeries newLineDD = new LineSeries();
            newLineDD.Title = "";
            newLineDD.Values = new ChartValues<decimal> { };
            newLineDD.PointGeometry = null;
            newLineDD.LineSmoothness = 0;
            newLineDD.Fill = System.Windows.Media.Brushes.Transparent;
            newLineDD.Fill.Freeze();
            newLineDD.DataLabels = false;
            newLineDD.Stroke = color;
            newLineDD.Stroke.Freeze();

            LiveCharts.Wpf.LineSeries newLineAlpha = null;
            ScatterSeries maxSeries = null;
            ScatterSeries maxSeriesDD = null;

            if (benchmarkType == EquityBenchmark.CODE)
            {
                newLineAlpha = new LineSeries();
                newLineAlpha.Title = "Alpha";
                newLineAlpha.Values = new ChartValues<decimal> { };
                newLineAlpha.PointGeometry = null;
                newLineAlpha.Stroke = System.Windows.Media.Brushes.Green;
                newLineAlpha.Stroke.Freeze();
                newLineAlpha.LineSmoothness = 0;

                maxSeries = new ScatterSeries();
                maxSeries.Title = "Strategy Max";
                maxSeries.Values = new ChartValues<ObservablePoint>();
                maxSeries.PointGeometry = DefaultGeometries.Diamond;
                maxSeries.Stroke = System.Windows.Media.Brushes.Blue;
                maxSeries.PointGeometry.Freeze();
                
                maxSeriesDD = new ScatterSeries();
                maxSeriesDD.Title = "";
                maxSeriesDD.DataLabels = false;
                maxSeriesDD.Values = new ChartValues<ObservablePoint>();
                maxSeriesDD.PointGeometry = DefaultGeometries.Diamond;
                maxSeriesDD.Stroke = System.Windows.Media.Brushes.Blue;
                maxSeriesDD.PointGeometry.Freeze();
            }

            int xIndex = 0;
            decimal newMax = 0;
            decimal minDDLine = 0;
            decimal newMaxSpy = 0;
            decimal newMaxMoney = 0;
            decimal newMaxSpyMoney = 0;
            decimal valueMoney = 0;
            decimal spyValueMoney = 0;

            List<String> labels = new List<string>();

            decimal startingValue = 0;
            if (snapshots.Count > 0)
                startingValue = snapshots[0].TotalPortfolioValue;

            var dailyValues = backtestCalculator.BenchmarksCalculator.GetDailySnapshots(benchmarkType);
            Dictionary<int, decimal> dailyValuesSpy = null;
            if (benchmarkType == EquityBenchmark.CODE)
                dailyValuesSpy = backtestCalculator.BenchmarksCalculator.GetDailySnapshots(SymbolBenchmark.BENCHMARK_CODE);

            List<int> keyDates = dailyValues.Keys.OrderBy(x => x).ToList();
            var lastKey = keyDates.Last();
            var firstKey = keyDates[0];
            decimal minVal = decimal.MaxValue;
            decimal minDDAll = 0;
            decimal valSpy = 0;
            decimal maxVal = 0;
            int group = 0;
            bool newMaxInGroup = false;
            decimal lastGroupDD = 0;
            decimal spyValueMax = 0;
            decimal spyValueMin = decimal.MaxValue;
            int startingMax = 0;

            foreach (int key in keyDates)
            {
                group++;
                decimal result = dailyValues[key];
                valueMoney = result;
                decimal spyValue = 0;
                if (dailyValuesSpy != null)
                    spyValue = Math.Round(dailyValuesSpy[key], 0);
                spyValueMoney = spyValue;
                int decimals = 0;

                //if percentuals selected, transform using startingValue
                if (chkPercent)
                {
                    decimals = 2;
                    result = result / startingValue * (decimal)100;
                    spyValue = spyValue / startingValue * (decimal)100;
                }
                
                if (newMax < result)
                {
                    newMaxInGroup = true;
                    newMax = result;
                }

                maxVal = Math.Max(maxVal, result);
                spyValueMax = Math.Max(spyValueMax, spyValue);
                decimal newMinVal = 0;
                if (chkPercent)
                    newMinVal = Math.Round((result - newMax) > 0 || newMax == 0 ? 0 : (result - newMax) / newMax * (decimal)100, decimals);
                else
                    newMinVal = Math.Round(result - newMax, decimals);
                
                if(newMinVal < minDDLine)
                {
                    minDDLine = newMinVal;
                    minDDAll = newMinVal;
                    minVal = Math.Min(minVal, result);
                    spyValueMin = Math.Min(spyValueMin, spyValue);
                }

                if (key == firstKey || key.ToString().Substring(6,2) == "01" || key.ToString().Substring(6, 2) == "15" || key == lastKey)
                {
                    group = 0;
                    var valueToPlot = maxVal;
                    var spyToPlot = spyValueMax;
                    if (minDDLine < lastGroupDD && !newMaxInGroup)
                    {
                        valueToPlot = minVal;
                        spyToPlot = spyValueMin;
                    }

                    if (newMaxInGroup && maxSeries != null)
                        maxSeries.Values.Add(new ObservablePoint(xIndex, (double)Math.Round(newMax, decimals)));

                    newLine.Values.Add(Math.Round(valueToPlot, decimals));
                    if (newLineAlpha != null)
                        newLineAlpha.Values.Add(Math.Round(valueToPlot - spyToPlot, decimals));

                    labels.Add(key.ToString().Substring(4, 2) + "/" + key.ToString().Substring(0,4));

                    newLineDD.Values.Add(minDDLine);

                    maxLabelsValue = Math.Max(maxLabelsValue, valueToPlot);
                    lastGroupDD = minDDLine;
                    maxVal = 0;
                    spyValueMin = decimal.MaxValue;
                    spyValueMax = 0;
                    newMaxInGroup = false;
                    minDDLine = 0;
                    minVal = decimal.MaxValue;
                    xIndex++;
                }
            }

            newLine.Fill = System.Windows.Media.Brushes.Transparent;
            newLine.Fill.Freeze();
            newLine.DataLabels = false;

            if (newLineAlpha != null)
            {
                newLineAlpha.Fill = System.Windows.Media.Brushes.Transparent;
                newLineAlpha.Fill.Freeze();
                newLineAlpha.DataLabels = false;
            }

            seriesToPlot.Add(newLine);
            if(maxSeries != null)
                seriesToPlot.Add(maxSeries);
            if (maxSeriesDD != null)
                seriesToPlotDD.Add(maxSeriesDD);
            if (newLineAlpha != null && chkAlpha)
                seriesToPlot.Add(newLineAlpha);
            
            seriesToPlotDD.Add(newLineDD);

            return labels;
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdExport_Click(object sender, EventArgs e)
        { 
            grdTrades.SelectAll();
            Clipboard.SetDataObject(grdTrades.GetClipboardContent());
        }

        private void BacktestResults_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
               Properties.Settings.Default.FormSizeBacktest = Size;
               Properties.Settings.Default.Save();
            }
        }

        private void dateFrom_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void BacktestResults_Shown(object sender, EventArgs e)
        {
            if (isFromOptimal == false) generateGraph(true);
            else
            {
                generateGraph(false);
            }

            if (showPerformanceGraph)
            {
                cmdPerformanceGraph_LinkClicked(this, null);
            }
        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            if (DataUpdater.CheckDatabaseBloqued())
                return;

            if(dateFrom.Value > DateTime.Now.AddMonths(14))
            {
                MessageBox.Show("Backtest cannot be less than 14 months.");
                return;
            }

            generateGraph(true);
        }
        
        private void PnlProcessing_CancelProcess(object sender, EventArgs e)
        {
            StrategyView.CancelResultsProcessing = true;
            BacktestCalculator.CancelProcess = true;
            showPerformanceGraph = false;
        }

        private void lstSelectedLines_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (initializing)
                return;

            if (DataUpdater.CheckDatabaseBloqued())
                return;

            //update checks
            chkEquity = e.Index == 0 ? e.NewValue == CheckState.Checked : lstSelectedLines.GetItemChecked(0);
            chkAlpha = e.Index == 1 ? e.NewValue == CheckState.Checked : lstSelectedLines.GetItemChecked(1);
            chkBlendedAdv = e.Index == 2 ? e.NewValue == CheckState.Checked : lstSelectedLines.GetItemChecked(2);
            chkBlendedBasic = e.Index == 3 ? e.NewValue == CheckState.Checked : lstSelectedLines.GetItemChecked(3);
            chkBondsModel = e.Index == 4 ? e.NewValue == CheckState.Checked : lstSelectedLines.GetItemChecked(4);
            chkBenchmark = e.Index == 5 ? e.NewValue == CheckState.Checked : lstSelectedLines.GetItemChecked(5);

            generateGraph(false);
        }

        private void cmdPerformanceTable_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PerformanceTable frmTable = new PerformanceTable(backtestCalculator);
            frmTable.ShowDialog();
        }
        
        private void cmdPerformanceGraph_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PerformanceGraph frmTable = new PerformanceGraph(backtestCalculator.MonthlySnapshots, backtestCalculator);
            frmTable.ShowDialog();
        }

        private void chkShowPercent_CheckedChanged(object sender, EventArgs e)
        {
            if (initializing)
                return;

            if (DataUpdater.CheckDatabaseBloqued())
                return;

            chkPercent = chkShowPercent.Checked;

            generateGraph(false);
        }

        private void cmdPerformanceReport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PerformanceReport frmTable = new PerformanceReport(backtestCalculator);
            frmTable.ShowDialog();
        }

        private void linkLabelShowHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.isShowHistory = !this.isShowHistory;
            if (this.isShowHistory)
            {
                linkLabelShowHistory.Text = "Show Transaction";
            } else
            {
                linkLabelShowHistory.Text = "Show History";
            }
            reloadGridView();
        }

        private void toggleFilter_CheckedChanged(object sender, EventArgs e)
        {
            this.isFilterOn = toggleFilter.Checked;
            this.filterDate = dateTimeFilter.Value;
            if (this.isFilterOn) dateTimeFilter.Enabled = true;
            else dateTimeFilter.Enabled = false;
            reloadGridView();
        }

        private void reloadGridView()
        {
            if (this.isShowHistory)
            {
                dataGridHistory.Visible = true;
                grdTrades.Visible = false;
            } else
            {
                dataGridHistory.Visible = false;
                grdTrades.Visible = true;
            }
            if (isFilterOn)
            {
                grdTrades.DataSource = new SortedBindingList<Trade>(tradelist.FindAll(x=>x.EntryDateDT <= this.filterDate && x.ExitDateDT > this.filterDate));
                dataGridHistory.DataSource = new SortedBindingList<TradeHistory>(historylist.FindAll(x=>x.Date == Utils.ConvertDateTimeToInt(this.filterDate)));
            } else
            {
                grdTrades.DataSource = new SortedBindingList<Trade>(tradelist);
                dataGridHistory.DataSource = new SortedBindingList<TradeHistory>(historylist);
            }

            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        private void dateTimeFilter_ValueChanged(object sender, EventArgs e)
        {
            this.filterDate = dateTimeFilter.Value;
            reloadGridView();
        }
    }
}
