using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockRanking
{
    public partial class RealtimeView : Form
    {
        private List<Position> DesiredPortfolio = null;
        private List<Position> orgPositions = null;
        private List<Position> CurrentPortfolio = null;
        private List<PortfolioSnapshot> MonthlySnapshots = new List<PortfolioSnapshot>();
        private decimal currentPortfolioCash = 0;
        private decimal portfolio1YBack = 0;
        private decimal portfolioMTD = 0;
        private decimal portfolioQTD = 0;
        private decimal portfolioYTD = 0;
        private PortfolioParameters portfolioParameters;
        private BacktestCalculator backtestCalculator = null;

        public RealtimeView(List<Position> portfolio, List<Position> currentPortfolio, BacktestCalculator backtestCalculator, decimal desiredPortfolioCash, PortfolioParameters portfolioParams)
        {
            InitializeComponent();

            portfolioParameters = portfolioParams;
            this.backtestCalculator = backtestCalculator;

            //preload columns hidden state
            if (Properties.Settings.Default.ExpectedPortfolioCols.Trim() != "")
            {
                foreach (DataGridViewColumn col in grdPositions.Columns)
                    if (!Properties.Settings.Default.ExpectedPortfolioCols.Contains("," + col.Index + ","))
                        col.Visible = false;
            }

            if (Properties.Settings.Default.CurrentPortfolioCols.Trim() != "")
            {
                foreach (DataGridViewColumn col in grdCurrent.Columns)
                    if (!Properties.Settings.Default.CurrentPortfolioCols.Contains("," + col.Index + ","))
                        col.Visible = false;
            }

            grdCurrent.BackgroundColor = System.Drawing.SystemColors.Control;
            grdPositions.BackgroundColor = System.Drawing.SystemColors.Control;

            orgPositions = portfolio.ToList();
            getDesiredPositions();
            CurrentPortfolio = currentPortfolio.ToList();

            if (WindowState == FormWindowState.Normal)
            {
                if (Properties.Settings.Default.FormSizeRealtimeView.Width > 0)
                    Size = Properties.Settings.Default.FormSizeRealtimeView;
            }

            trackRefresh.Value = Properties.Settings.Default.RefreshInterval;

            grdPositions.AutoGenerateColumns = false;
            grdPositions.DataSource = DesiredPortfolio;

            grdCurrent.AutoGenerateColumns = false;
            grdCurrent.DataSource = CurrentPortfolio;

            //search historic portfolio value for each required period
            currentPortfolioCash = desiredPortfolioCash;
            MonthlySnapshots = MonthlySnapshots;
            portfolio1YBack = backtestCalculator.YearBackPortfolioValue;

            DateTime mtdDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            DateTime qtdDate = Utils.GetFirstQuarterDay(DateTime.Now).AddDays(-1);
            portfolioMTD = backtestCalculator.MonthlySnapshots.Find(x => x.Date.Year == mtdDate.Year && x.Date.Month == mtdDate.Month && x.Date.Day == mtdDate.Day).TotalPortfolioValue;
            portfolioYTD = backtestCalculator.MonthlySnapshots.Find(x => x.Date.Year == DateTime.Now.Year - 1 && x.Date.Month == 12 && x.Date.Day == 31).TotalPortfolioValue;
            portfolioQTD = backtestCalculator.MonthlySnapshots.Find(x => x.Date.Year == qtdDate.Year && x.Date.Month == qtdDate.Month && x.Date.Day == qtdDate.Day).TotalPortfolioValue;
        }

        private void getDesiredPositions()
        {
            List<int> alreadyShown = new List<int>();

            if (DesiredPortfolio == null) DesiredPortfolio = new List<Position>();
            else DesiredPortfolio.Clear();

            foreach (Position pos in orgPositions)
            {
                int count = 0, totalshare = 0, firstdate = 0;
                decimal totalentryprice = 0;
                
                if (alreadyShown.Contains(pos.IdStock)) continue;
                alreadyShown.Add(pos.IdStock);
                
                Position newPos = new Position(pos);

                orgPositions.ForEach(x =>
                {
                    if (x.IdStock == pos.IdStock)
                    {
                        count++;
                        totalshare += x.Shares;
                        if (count == 1) firstdate = x.DateEntered;
                        else if (firstdate > x.DateEntered) firstdate = x.DateEntered;
                        totalentryprice += x.Shares * x.EntryPrice;
                    }
                    
                });
                if (count > 0) newPos.AuxSymbol = newPos.AuxSymbol + "*";
                newPos.EntryPrice = totalentryprice / totalshare;
                newPos.Shares = totalshare;

                DesiredPortfolio.Add(newPos);
            }
        }

        private async void tmrRealtime_Tick(object sender, EventArgs e)
        {
            DateTime LastRefresh = DateTime.MinValue;
            decimal totalChange = 0;
            

            foreach (Position pos in DesiredPortfolio)
            {
                Application.DoEvents();
                //update prices
                StockValue price = await StockSourcesReader.QueryRealtimePrice(pos.AuxSymbol);

                if (price == null)
                    continue;

                decimal close1st = Stock.GetPrice1StJan(pos.IdStock);
                price.AuxYtdChange = (price.Close - close1st) / close1st;

                //update values for the stocks
                pos.AuxCurrentPrice = price.Close;
                pos.AuxDailyChange = price.ChangePercent * 100;
                pos.AuxYTD = price.AuxYtdChange * 100;

                totalChange += pos.AuxDailyChange;

                LastRefresh = DateTime.Now;
            }

            grdPositions.Invalidate();
            grdPositions.Refresh();

            if (LastRefresh != DateTime.MinValue)
            {
                txtLastRefresh.Text = "Last Refresh: " + LastRefresh.ToString();

                StockValue spyPrice = await StockSourcesReader.QueryRealtimePrice("SPY");
                if (spyPrice != null)
                {
                    decimal alpha = totalChange - spyPrice.ChangePercent * 100;
                    lblTodaysAlpha.Text = alpha.ToString("n2") + " %";
                }
            }

            foreach (Position pos in CurrentPortfolio)
            {
                Application.DoEvents();
                //update prices
                StockValue price = await StockSourcesReader.QueryRealtimePrice(pos.AuxSymbol);

                if (price == null)
                    continue;

                decimal close1st = Stock.GetPrice1StJan(pos.IdStock);
                price.AuxYtdChange = (price.Close - close1st) / close1st;

                //update values for the stocks
                pos.AuxCurrentPrice = price.Close;
                pos.AuxDailyChange = price.ChangePercent * 100;
                pos.AuxYTD = price.AuxYtdChange * 100;

                this.backtestCalculator.calcStopLossPrice(pos);
            }

            grdCurrent.Invalidate();
            grdCurrent.Refresh();

            //refresh risk model
            decimal avg50, avg200;
            bool spyRes = this.backtestCalculator.checkSPYAvgSignal(DateTime.Now, out avg50, out avg200);

            decimal hiloValue;
            bool hiloRes = this.backtestCalculator.checkHiLoSignal(DateTime.Now, out hiloValue);

            decimal cape1, cape2;
            bool capeRes = this.backtestCalculator.checkCapeSignal(DateTime.Now, out cape1, out cape2);

            bool customRes = CustomRiskModelSingleton.Instance.GetSignal(DateTime.Now);

            lstRiskModel.Clear();
            lstRiskModel.Items.Add("SPY AVG:     " + (spyRes ? "OFF" : "ON ") +  "  MM1  : " + avg50.ToString("n2") + "  MM2  : " + avg200.ToString("n2"));
            lstRiskModel.Items.Add("HI LO:       " + (hiloRes ? "OFF" : "ON ") + "  HiLo%: " + hiloValue.ToString("n2"));
            lstRiskModel.Items.Add("CAPE:        " + (capeRes ? "OFF" : "ON ") + "  CAPE1: " + cape1.ToString("n2") + "  CAPE2: " + cape2.ToString("n2"));
            lstRiskModel.Items.Add("FILE:        " + (customRes ? "OFF" : "ON "));
            lstRiskModel.Items.Add("BONDS ASSET: " + this.backtestCalculator.BenchmarksCalculator.GetBondsSymbol());

            //calculate performance indicators
            decimal currentPorfolioValue = currentPortfolioCash;
            foreach (Position pos in DesiredPortfolio)
                currentPorfolioValue += pos.AuxCurrentPrice * pos.Shares;

            if (portfolio1YBack != 0)
                txtP1Y.Text = ((currentPorfolioValue - portfolio1YBack) / portfolio1YBack * 100).ToString("n2") + " %";
            if (portfolioMTD != 0)
                txtPMTD.Text = ((currentPorfolioValue - portfolioMTD) / portfolioMTD * 100).ToString("n2") + " %";
            if (portfolioYTD != 0)
                txtPYTD.Text = ((currentPorfolioValue - portfolioYTD) / portfolioYTD * 100).ToString("n2") + " %";
            if (portfolioQTD != 0)
                txtPQTD.Text = ((currentPorfolioValue - portfolioQTD) / portfolioQTD * 100).ToString("n2") + " %";
        }

        private void RealtimeView_Shown(object sender, EventArgs e)
        {
            tmrRealtime_Tick(null, null);

            trackRefresh_Scroll(null, null);
        }

        private void RealtimeView_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.FormSizeRealtimeView = Size;
                Properties.Settings.Default.Save();
            }
        }

        private void trackRefresh_Scroll(object sender, EventArgs e)
        {
            int refresh = 1000;

            switch (trackRefresh.Value)
            {
                case 1:
                    refresh *= 15;
                    txtRefresh.Text = "every 15 seconds";
                    break;

                case 2:
                    refresh *= 30;
                    txtRefresh.Text = "every 30 seconds";
                    break;

                case 3:
                    refresh *= 60;
                    txtRefresh.Text = "every minute";
                    break;

                case 4:
                    refresh *= 60 * 5;
                    txtRefresh.Text = "every 5 minutes";
                    break;

                case 5:
                    refresh *= 15 * 60;
                    txtRefresh.Text = "every 15 minutes";
                    break;
            }

            tmrRealtime.Interval = refresh;

            Properties.Settings.Default.RefreshInterval = trackRefresh.Value;
            Properties.Settings.Default.Save();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (grdPositions.Columns[e.ColumnIndex].Name == "colDailyChange" ||
               grdPositions.Columns[e.ColumnIndex].Name == "colYTD" ||
               grdPositions.Columns[e.ColumnIndex].Name == "colPortGain")
            {
                if (Convert.ToDecimal(grdPositions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) >= 0)
                {
                    grdPositions.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Green, BackColor = Color.White, SelectionForeColor = Color.Black };
                }
                else
                {
                    grdPositions.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Red, BackColor = Color.White, SelectionForeColor = Color.Black };
                }
            }

            if (grdPositions.Columns[e.ColumnIndex].Name == "colSTOP")
            {
                if (Convert.ToDecimal(grdPositions.Rows[e.RowIndex].Cells[colCurrentPrice.Index].Value) != 0)
                {
                    if (Convert.ToDecimal(grdPositions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) <=
                        Convert.ToDecimal(grdPositions.Rows[e.RowIndex].Cells[colCurrentPrice.Index].Value))
                    {
                        if (grdPositions.Rows[e.RowIndex].Cells[colStyle.Index].Value.ToString() == "Long")
                            grdPositions.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.White, SelectionForeColor = Color.Black };
                        else
                            grdPositions.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.Red, SelectionForeColor = Color.Black };
                    }
                    else
                    {
                        if (grdPositions.Rows[e.RowIndex].Cells[colStyle.Index].Value.ToString() == "Long")
                            grdPositions.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.Red, SelectionForeColor = Color.Black };
                        else
                            grdPositions.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.White, SelectionForeColor = Color.Black };
                    }
                }
            }
        }

        private void cellFormattingCurrent(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (grdCurrent.Columns[e.ColumnIndex].Name == "colCDailyChange" ||
               grdCurrent.Columns[e.ColumnIndex].Name == "colCYTD" ||
               grdCurrent.Columns[e.ColumnIndex].Name == "colCPortGain")
            {
                if (Convert.ToDecimal(grdCurrent.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) >= 0)
                {
                    grdCurrent.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Green, BackColor = Color.White, SelectionForeColor = Color.Green };
                }
                else
                {
                    grdCurrent.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Red, BackColor = Color.White, SelectionForeColor = Color.Red };
                }
            }

            if (grdCurrent.Columns[e.ColumnIndex].Name == "colCStop")
            {
                if (Convert.ToDecimal(grdCurrent.Rows[e.RowIndex].Cells[colCCurrentPrice.Index].Value) != 0)
                {
                    if (Convert.ToDecimal(grdCurrent.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) <=
                        Convert.ToDecimal(grdCurrent.Rows[e.RowIndex].Cells[colCCurrentPrice.Index].Value))
                    {
                        grdCurrent.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.White, SelectionForeColor = Color.Black };
                    }
                    else
                    {
                        grdCurrent.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.Red, SelectionForeColor = Color.Black };
                    }
                }
            }
        }

        private void cmdRiskGraphs_Click(object sender, EventArgs e)
        {
            RiskGraphs graphs = new RiskGraphs(portfolioParameters);
            graphs.Show();
        }

        private void lnkEditColumns_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            List<DataGridViewColumn> columnsCurr = new List<DataGridViewColumn>();
            foreach (DataGridViewColumn col in grdCurrent.Columns)
                columnsCurr.Add(col);

            List<DataGridViewColumn> columnsExp = new List<DataGridViewColumn>();
            foreach (DataGridViewColumn col in grdPositions.Columns)
                columnsExp.Add(col);

            EditColumnsPopup frmCols = new EditColumnsPopup(columnsCurr, columnsExp);

            if (frmCols.ShowDialog() == DialogResult.OK)
            {
                //save columns hidden state
                String colsStr = "";
                foreach (DataGridViewColumn col in grdPositions.Columns)
                    if (col.Visible)
                        colsStr += "," + col.Index + ",";
                Properties.Settings.Default.ExpectedPortfolioCols = colsStr;

                colsStr = "";
                foreach (DataGridViewColumn col in grdCurrent.Columns)
                    if (col.Visible)
                        colsStr += "," + col.Index + ",";
                Properties.Settings.Default.CurrentPortfolioCols = colsStr;

                Properties.Settings.Default.Save();
            }
        }

        private void RealtimeView_Load(object sender, EventArgs e)
        {
        }

        private void label12_Click(object sender, EventArgs e)
        {
        }

        private void label13_Click(object sender, EventArgs e)
        {
        }

        private void grdPositions_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void grdCurrent_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                (new CompositeGraph(((Position)grdCurrent.Rows[e.RowIndex].DataBoundItem).IdStock, portfolioParameters)).ShowDialog();
            }
            catch (Exception ex)
            { }
        }

        private void grdPositions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                (new CompositeGraph(((Position)grdPositions.Rows[e.RowIndex].DataBoundItem).IdStock, portfolioParameters)).ShowDialog();
            }
            catch (Exception ex)
            { }
        }
    }
}