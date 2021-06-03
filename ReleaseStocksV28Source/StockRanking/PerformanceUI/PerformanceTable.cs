using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockRanking.Benchmarks;

namespace StockRanking
{
    public partial class PerformanceTable : Form
    {
        bool initializing = true;
        public PerformanceTable(BacktestCalculator backtestCalculator)
        {
            /*
            if (valuesList.Count == 0)
            {
                MessageBox.Show("Benchmark " + benchmarkType + " has not data, please check your portfolio parameters and retry the backtest.");
                return null;
            }

            if (valuesListBenchmark.Count == 0)
            {
                MessageBox.Show("Benchmark " + benchmarkCorrelation + " has not data, please check your portfolio parameters and retry the backtest.");
                return null;
            }
            */


            InitializeComponent();
            
            if (WindowState == FormWindowState.Normal)
            {
                if (Properties.Settings.Default.PerformanceTableSize.Width > 0)
                    Size = Properties.Settings.Default.PerformanceTableSize;
                if (Properties.Settings.Default.PerformanceTableSeparator > 50 && Properties.Settings.Default.PerformanceTableSeparator < this.Size.Width-50)
                    splitContainer1.SplitterDistance = Properties.Settings.Default.PerformanceTableSeparator;
            }
            initializing = false;

            List<PortfolioSnapshot> snapshots = backtestCalculator.MonthlySnapshots;

            //enable double buffering for the grid
            Type dgvType = grdTable.GetType();
            System.Reflection.PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            pi.SetValue(grdTable, true, null);
            pi.SetValue(grdIndicators, true, null);

            /*
            foreach(DataGridViewColumn col1 in grdIndicators.Columns)
            {
                col1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //col1.DefaultCellStyle.Format = "n2";
                col1.MinimumWidth = 60;
                col1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            */

            List<PortfolioSnapshot> monthlySnapshots = snapshots.ToList();
            colYear.MinimumWidth = 60;
            colYear.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colYear.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdTable.GridColor = Color.White;
            colYear.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridViewColumn col;
           
            for (int i = 1; i <= 12; i++)
            {
                col = new DataGridViewColumn(colYear.CellTemplate);
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                col.HeaderText = new DateTime(2018, i, 1).ToString("MMM", CultureInfo.InvariantCulture).ToUpper();
                col.DefaultCellStyle.Format = "n2";
                col.MinimumWidth = 60;
                col.HeaderCell.Style.ForeColor = Color.Green;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdTable.Columns.Add(col);
            }

            col = new DataGridViewColumn(colYear.CellTemplate);
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            col.HeaderText = "TOTAL";
            col.DefaultCellStyle.Format = "n2";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.MinimumWidth = 60;
            grdTable.Columns.Add(col);
             
            col = new DataGridViewColumn(colYear.CellTemplate);
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            col.HeaderText = "TOTAL SPY";
            col.DefaultCellStyle.Format = "n2";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.MinimumWidth = 60;
            grdTable.Columns.Add(col);
             
            col = new DataGridViewColumn(colYear.CellTemplate);
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            col.HeaderText = "ALPHA";
            col.DefaultCellStyle.Format = "n2";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.MinimumWidth = 60;
            grdTable.Columns.Add(col);


            if (monthlySnapshots.Count < 4)
            {
                this.Close();
                return;
            }

            int currentMonth = monthlySnapshots[0].Date.AddDays(1).Month;
            decimal previousValue = monthlySnapshots[0].TotalPortfolioValue;
            decimal previousValueSPY = monthlySnapshots[0].SpyBenchmark;
            int currentYear = monthlySnapshots[0].Date.AddDays(1).Year;
            var currentRow = grdTable.Rows[grdTable.Rows.Add()];
            DataGridViewTextBoxCell cell;
            decimal lastYearValue = previousValue;
            decimal lastYearValueSPY = previousValueSPY;
            monthlySnapshots.RemoveAt(0);

            List<decimal> monthlyPercent = new List<decimal>();
            List<decimal> monthlyPercentSPY = new List<decimal>();
            decimal initialPortfolio = previousValue;
            decimal initialSPY = previousValueSPY;

            //add year
            currentRow.Cells[0].Value = currentYear;

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
                        currentRow.Cells[12+1].Value = "-";
                    else
                    {
                        currentRow.Cells[12 + 1].Value = (previousValue - lastYearValue) / lastYearValue * (decimal)100;
                        paintCell((DataGridViewTextBoxCell)currentRow.Cells[12 + 1], true);

                        currentRow.Cells[12 + 2].Value = (previousValueSPY - lastYearValueSPY) / lastYearValueSPY * (decimal)100;
                        paintCell((DataGridViewTextBoxCell)currentRow.Cells[12 + 2], true);

                        currentRow.Cells[12 + 3].Value = (decimal)currentRow.Cells[12 + 1].Value - (decimal)currentRow.Cells[12 + 2].Value;
                        paintCell((DataGridViewTextBoxCell)currentRow.Cells[12 + 3], true);
                    }
                    currentMonth = 1;

                    lastYearValue = previousValue;
                    lastYearValueSPY = previousValueSPY;
                    currentRow = grdTable.Rows[grdTable.Rows.Add()];

                    //add year
                    currentRow.Cells[0].Value = currentYear++;
                }

                //add % value
                if (previousValue == 0)
                    currentRow.Cells[currentMonth].Value = 0;
                else
                {
                    currentRow.Cells[currentMonth].Value = (snap.TotalPortfolioValue - previousValue) / previousValue * (decimal)100;

                    paintCell((DataGridViewTextBoxCell)currentRow.Cells[currentMonth], false);
                }

                monthlyPercent.Add((snap.TotalPortfolioValue - previousValue) / previousValue * (decimal)100);
                monthlyPercentSPY.Add((snap.SpyBenchmark - previousValueSPY) / previousValueSPY * (decimal)100);

                previousValue = snap.TotalPortfolioValue;
                previousValueSPY = snap.SpyBenchmark;
            }

            //last year total value
            if (previousValue == 0)
                currentRow.Cells[12 + 1].Value = "-";
            else
            {
                currentRow.Cells[12 + 1].Value = (previousValue - lastYearValue) / lastYearValue * (decimal)100;
                paintCell((DataGridViewTextBoxCell)currentRow.Cells[12 + 1], true);

                currentRow.Cells[12 + 2].Value = (previousValueSPY - lastYearValueSPY) / lastYearValueSPY * (decimal)100;
                paintCell((DataGridViewTextBoxCell)currentRow.Cells[12 + 2], true);

                currentRow.Cells[12 + 3].Value = (decimal)currentRow.Cells[12 + 1].Value - (decimal)currentRow.Cells[12 + 2].Value;
                paintCell((DataGridViewTextBoxCell)currentRow.Cells[12 + 3], true);
            }


            for (int auxMonth = currentMonth + 1; auxMonth <= 12; auxMonth++)
            {
                currentRow.Cells[auxMonth].Style.BackColor = Color.FromArgb(243, 243, 243);
                currentRow.Cells[auxMonth].Style.ForeColor = Color.FromArgb(109, 109, 109);
                currentRow.Cells[auxMonth].Value = "-";
            }

            //fill indicators grid
            decimal alpha = 0, beta = 0, sharpe = 0, profit = 0, stdev = 0, maxdd = 0, water = 0, outperform = 0, underperform = 0, goodStreak = 0, badStreak = 0;

            decimal alphaDiffPortfolio = (previousValue - initialPortfolio) / initialPortfolio;
            decimal alphaDiffSPY = (previousValueSPY - initialSPY) / initialSPY;
            alpha = (alphaDiffPortfolio - alphaDiffSPY);

            stdev = DataUpdater.CalcStdDev(monthlyPercent);
            if(stdev != 0)
                sharpe = monthlyPercent.Average() / stdev * (decimal)Math.Sqrt(12f);

            decimal totalPositiveMonths = 0;
            decimal totalNegativeMonths = 0;
            foreach (decimal percent in monthlyPercent)
                if (percent > 0)
                    totalPositiveMonths += percent;
                else
                    totalNegativeMonths += percent;
            if(totalNegativeMonths != 0)
                profit = totalPositiveMonths / Math.Abs(totalNegativeMonths);

            decimal variance = Utils.Variance(monthlyPercentSPY);
            if(variance != 0)
                beta = Utils.Covariance(monthlyPercent, monthlyPercentSPY) / variance;

            decimal worstDD = backtestCalculator.MetricsCalculator.GetDrawDown1(EquityBenchmark.CODE).Value;
            decimal highWater = backtestCalculator.MetricsCalculator.GetHighWater(EquityBenchmark.CODE);
            
            maxdd = worstDD;
            water = highWater;
            int maxGoodStreak = 0;
            int maxBadStreak = 0;
            for (int i = 0; i < monthlyPercent.Count; i++)
            {
                if (monthlyPercent[i] > monthlyPercentSPY[i])
                {
                    goodStreak++;
                    outperform++;
                }
                else
                {
                    maxGoodStreak = Math.Max((int)maxGoodStreak, (int)goodStreak);
                    goodStreak = 0;
                }

                if (monthlyPercent[i] < monthlyPercentSPY[i])
                {
                    underperform++;
                    badStreak++;
                }
                else
                {
                    maxBadStreak = Math.Max((int)maxBadStreak, (int)badStreak);
                    badStreak = 0;
                }
            }

            goodStreak = Math.Max((int)maxGoodStreak, (int)goodStreak);
            badStreak = Math.Max((int)maxBadStreak, (int)badStreak);

            grdIndicators.Rows.Clear();
            currentRow = grdIndicators.Rows[grdIndicators.Rows.Add()];

            currentRow.Cells[0].Value = alpha;
            currentRow.Cells[1].Value = sharpe;
            currentRow.Cells[2].Value = profit;
            currentRow.Cells[3].Value = beta;
            currentRow.Cells[4].Value = stdev;
            currentRow.Cells[5].Value = maxdd;
            currentRow.Cells[6].Value = water;
            currentRow.Cells[7].Value = outperform;
            currentRow.Cells[8].Value = underperform;
            currentRow.Cells[9].Value = goodStreak;
            currentRow.Cells[10].Value = badStreak;

            fillRiskMetrics(backtestCalculator);
        }

        void fillRiskMetrics(BacktestCalculator backtestCalculator)
        {
            addRiskGroup(backtestCalculator, "Equity Only vs Benchmark", EquityBenchmark.CODE, SymbolBenchmark.BENCHMARK_CODE);
            if(Stock.GetStock(backtestCalculator.Portfolio.GetBondsRiskModelAssetId()) != null && backtestCalculator.Portfolio.GetBondsRiskModelAssetId() != 0)
                addRiskGroup(backtestCalculator, "Bond Model vs " + Stock.GetStock(backtestCalculator.Portfolio.GetBondsRiskModelAssetId()).Symbol, BondsModelBenchmark.CODE, Stock.GetStock(backtestCalculator.Portfolio.GetBondsRiskModelAssetId()).Symbol);
            addRiskGroup(backtestCalculator, "Blended Advanced with Bond Switching", EquityBenchmark.CODE, BlendedConfigurableBenchmark.CODE);
            addRiskGroup(backtestCalculator, "Blended Advanced with Bond Model", EquityBondsModelBenchmark.CODE, BlendedConfigurableBenchmark.CODE);
        }

        void addRiskGroup(BacktestCalculator backtestCalculator, String title, String code1, String code2)
        { 
            var rowIndex = grdRiskStatistics.Rows.Add();
            var row = grdRiskStatistics.Rows[rowIndex];
            row.DefaultCellStyle = new DataGridViewCellStyle(grdRiskStatistics.DefaultCellStyle);
            row.DefaultCellStyle.Font = new Font(row.DefaultCellStyle.Font, FontStyle.Bold);
            row.Cells[0].Value = title;
            
            var metricsCol1 = MetricsCalculator.CalcMetrics(backtestCalculator, code1, code2);
            var metricsCol2 = MetricsCalculator.CalcMetrics(backtestCalculator, code2, code1);

            if (metricsCol1 == null)
                return;
            if (metricsCol2 == null)
                return;

            addRow("Volatility (monthly)", metricsCol1[RiskMetrics.Volatility], metricsCol2[RiskMetrics.Volatility]);
            addRow("Volatility (annualized)", metricsCol1[RiskMetrics.VolatilityAnnualized], metricsCol2[RiskMetrics.VolatilityAnnualized]);
            addRow("Downside Deviation Monthly", metricsCol1[RiskMetrics.DownsideDeviationMonthly], metricsCol2[RiskMetrics.DownsideDeviationMonthly]);
            addRow("Max Drawdown 1", metricsCol1[RiskMetrics.DD1], metricsCol2[RiskMetrics.DD1]);
            addRow("Max Drawdown 2", metricsCol1[RiskMetrics.DD2], metricsCol2[RiskMetrics.DD2]);
            addRow("Max Drawdown 3", metricsCol1[RiskMetrics.DD3], metricsCol2[RiskMetrics.DD3]);
            addRow("Benchmark Correlation", metricsCol1[RiskMetrics.BenchmarkCorrelation], "");
            addRow("Beta Lifetime", metricsCol1[RiskMetrics.BetaLifetime], "");
            addRow("Rolling Beta 3 years", metricsCol1[RiskMetrics.RollingBeta3Years], "");
            addRow("Last Year's Alpha Change", metricsCol1[RiskMetrics.AlphaAnnualized], "");
            addRow("Avg Annual Alpha", metricsCol1[RiskMetrics.AvgAnnualAlpha], "");
            addRow("R^2", metricsCol1[RiskMetrics.R2], metricsCol2[RiskMetrics.R2]);
            addRow("Sharpe Ratio", metricsCol1[RiskMetrics.SharpeRatio], metricsCol2[RiskMetrics.SharpeRatio]);
            addRow("Sortino Ratio", metricsCol1[RiskMetrics.SortinoRatio], metricsCol2[RiskMetrics.SortinoRatio]);
            addRow("Treynor Ratio", metricsCol1[RiskMetrics.TreynorRatio], metricsCol2[RiskMetrics.TreynorRatio]);
            addRow("Calmar", metricsCol1[RiskMetrics.Calmar], metricsCol2[RiskMetrics.Calmar]);
            addRow("Information Ratio", metricsCol1[RiskMetrics.InformationRatio], metricsCol2[RiskMetrics.InformationRatio]);
            addRow("Upside Capture", metricsCol1[RiskMetrics.UpsideCapture], "");
            addRow("Downside Capture", metricsCol1[RiskMetrics.DownsideCapture], "");
            addRow("Profit Factor", metricsCol1[RiskMetrics.ProfitFactor], metricsCol2[RiskMetrics.ProfitFactor]);
            addRow("% Positive Months", metricsCol1[RiskMetrics.PercPositiveMonths], metricsCol2[RiskMetrics.PercPositiveMonths]);
            addRow("% Negative Months", metricsCol1[RiskMetrics.PercNegativeMonths], metricsCol2[RiskMetrics.PercNegativeMonths]);
            addRow("Win Streak", metricsCol1[RiskMetrics.WinStreak], metricsCol2[RiskMetrics.WinStreak]);
            addRow("Lose Streak", metricsCol1[RiskMetrics.LoseStreak], metricsCol2[RiskMetrics.LoseStreak]);

            addRow(" ", " ", " ");
        }

        
        void addRow(String name, decimal value)
        {
            var rowIndex = grdRiskStatistics.Rows.Add();
            var row = grdRiskStatistics.Rows[rowIndex];
            row.Cells[0].Value = name;
            row.Cells[1].Value = value;
        }

        void addRow(String name, object value1, object value2)
        {
            var rowIndex = grdRiskStatistics.Rows.Add();
            var row = grdRiskStatistics.Rows[rowIndex];
            row.Cells[0].Value = name;
            row.Cells[1].Value = value1;
            row.Cells[2].Value = value2;
        }

        void paintCell(DataGridViewTextBoxCell cell, Boolean totals)
        {
            if ((decimal)cell.Value > 0)
            {
                cell.Style.BackColor = Color.FromArgb(220, 241, 202);
                cell.Style.ForeColor = Color.FromArgb(0,128,0);

                if(totals)
                {
                    //cell.Style.BackColor = Color.FromArgb(194,225,166);
                    cell.Style.Font = new Font(grdTable.Font, FontStyle.Bold);
                }
                return;
            }

            if ((decimal)cell.Value < 0)
            {
                cell.Style.BackColor = Color.FromArgb(237,216,216);
                cell.Style.ForeColor = Color.FromArgb(255,0,0);

                if (totals)
                {
                    //cell.Style.BackColor = Color.FromArgb(220,173,173);
                    cell.Style.Font = new Font(grdTable.Font, FontStyle.Bold);
                }
                return;
            }

            cell.Style.BackColor = Color.FromArgb(243, 243, 243);
            cell.Style.ForeColor = Color.FromArgb(109, 109, 109);

            if (totals)
            {
                //cell.Style.BackColor = Color.FromArgb(225,225,225);
                cell.Style.Font = new Font(grdTable.Font, FontStyle.Bold);
            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PerformanceTable_Resize(object sender, EventArgs e)
        {
            if (initializing)
                return;

            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.PerformanceTableSize = Size;
                Properties.Settings.Default.PerformanceTableSeparator = splitContainer1.SplitterDistance;
                Properties.Settings.Default.Save();
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (initializing)
                return;

            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.PerformanceTableSeparator = splitContainer1.SplitterDistance;
                Properties.Settings.Default.Save();
            }
        }
    }
}
