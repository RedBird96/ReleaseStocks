using StockRanking.Entities;
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
    public partial class MainPanel : Form
    {
        PortfolioParameters currentPortfolioParameters = new PortfolioParameters();
        List<Position> DesiredPortfolio = null;
        decimal DesiredPortfolioCash = 0;
        decimal DesiredPortfolio1YearBack = 0;
        BacktestCalculator BacktestCalculatorCurrent = null;
        List<PortfolioSnapshot> DesiredPortfolioMonthlySnapshots = new List<PortfolioSnapshot>();
        bool DisableUpdates = false;
        Strategy selectedStrategy = null;
        List<Position> CurrentPortfolio = null;
        public static bool SaveMemoryMode = false;

        private bool ReprocessVRHistory = false;
        private bool ReprocessETFSymbols = false;
        frmLicense checkform = new frmLicense();

        public MainPanel()
        {
            DisableUpdates = true;

            InitializeComponent();
            
            pnlProcessing.CancelProcess += this.PnlProcessing_CancelProcess;

            RefreshStrategies();

            RefreshPortfolioParameters();

            DisableUpdates = false;

        }

        private void PnlProcessing_CancelProcess(object sender, EventArgs e)
        {
            ZachsSourceReader.CancelProcess = true;
            StockSourcesReader.CancelProcess = true;
            StrategyView.CancelResultsProcessing = true;
            BacktestCalculator.CancelProcess = true;
        }

        void RefreshPortfolioParameters()
        {
            currentPortfolioParameters = PortfolioParameters.Load(0);
            CustomRiskModelSingleton.Instance.SetFile(currentPortfolioParameters.CustomRiskFile);

            lstIndustries.Items.Clear();
            if (currentPortfolioParameters.IndustriesIncluded.Count > 0)
                foreach (String ind in currentPortfolioParameters.IndustriesIncluded)
                    lstIndustries.Items.Add(new ListViewItem(ind));
            else
                lstIndustries.Items.Add(new ListViewItem(EditPortfolio.allSectorsStr));

            lstPortfolioParameters.Items.Clear();

            ListViewItem lstItem = new ListViewItem("Account Size");
            lstItem.SubItems.Add(currentPortfolioParameters.AccountSize.ToString("n0"));
            lstPortfolioParameters.Items.Add(lstItem);

            lstItem = new ListViewItem("Rebalance Frequency");
            lstItem.SubItems.Add(currentPortfolioParameters.RebalanceFrequencyMonths.ToString("n0"));
            lstPortfolioParameters.Items.Add(lstItem);

            lstItem = new ListViewItem("Benchmark %");
            lstItem.SubItems.Add(currentPortfolioParameters.BenchmarkPercent.ToString("n2"));
            lstPortfolioParameters.Items.Add(lstItem);

            lstItem = new ListViewItem("Commission per Share");
            lstItem.SubItems.Add(currentPortfolioParameters.CommisionPerShare.ToString("n4"));
            lstPortfolioParameters.Items.Add(lstItem);

            lstItem = new ListViewItem("Positions");
            String posStr = "";
            if (currentPortfolioParameters.LongSelling > 0)
            {
                posStr = posStr + "Long " + ((currentPortfolioParameters.LongSelling == 1) ? "Top " : "Bottom ")  + currentPortfolioParameters.Positions.ToString("n0");
            }
            if (currentPortfolioParameters.ShortSelling > 0)
            {
                if (posStr.Length > 0) posStr = posStr + ", ";
                posStr = posStr + "Short " + ((currentPortfolioParameters.ShortSelling == 1) ? "Top " : "Bottom ") + currentPortfolioParameters.ShortPositions.ToString("n0");
            }
            lstItem.SubItems.Add(posStr);
            lstPortfolioParameters.Items.Add(lstItem);

            lstItem = new ListViewItem("Max Weight x Pos");
            lstItem.SubItems.Add(currentPortfolioParameters.MaxWeightPerPosition.ToString("n2"));
            lstPortfolioParameters.Items.Add(lstItem);

            lstItem = new ListViewItem("Max Pos x Sector");
            lstItem.SubItems.Add(currentPortfolioParameters.MaxPositionsPerIndustry.ToString("n2"));
            lstPortfolioParameters.Items.Add(lstItem);
        }

        void RefreshStrategies()
        {
            List<Strategy> strategies = Strategy.GetStrategies();
            selectedStrategy = null;
            lstStrategies.Items.Clear();

            foreach(Strategy strategy in strategies)
            {
                ListViewItem newItem = new ListViewItem(strategy.Name);
                newItem.Tag = strategy;
                newItem.SubItems.Add(strategy.PortfolioParameters.GetFrequencyText());
                newItem.SubItems.Add(strategy.PortfolioParameters.GetRiskModelText());
                newItem.SubItems.Add(strategy.GetFeaturesText());
                newItem.SubItems.Add(strategy.GetFiltersText());

                if (strategy.Selected)
                {
                    newItem.UseItemStyleForSubItems = true;
                    newItem.Font = new Font(newItem.Font, FontStyle.Bold);
                    selectedStrategy = strategy;
                }

                lstStrategies.Items.Add(newItem);
            }
        }

        private void cmdCreateStrategy_Click(object sender, EventArgs e)
        {
            disableLowMemMode();

            StrategyView newView = new StrategyView(null);
            var result = newView.ShowDialog();

            RefreshStrategies();

            if (result == DialogResult.OK)
                refreshDesiredPortfolio();
        }

        private void cmdUpdateData_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private async Task<bool> UpdateData()
        {
            if (DataUpdater.UpdatingDatabase)
                return false;

            //check if full download is needed
            if (DatabaseSingleton.Instance.GetData("SELECT VALUE FROM SYSTEM_PARAMETERS WHERE ID = " + (int)SystemParameters.FullDownloadCompleted).Rows.Count > 0
                && DatabaseSingleton.Instance.GetData("SELECT VALUE FROM SYSTEM_PARAMETERS WHERE VALUE > " + Utils.ConvertDateTimeToInt(DateTime.Now.AddMonths(-1)).ToString() + " AND ID = " + (int)SystemParameters.LastUpdateDate).Rows.Count > 0)
            {
                //only update if needed
                int realUpdate = DatabaseSingleton.Instance.GetData("SELECT VALUE FROM SYSTEM_PARAMETERS WHERE VALUE < " + Utils.ConvertDateTimeToInt(DateTime.Now).ToString() + " AND ID = " + (int)SystemParameters.LastUpdateDate).Rows.Count;

                if (realUpdate > 0)
                {

                    disableLowMemMode();

                    var a = await DataUpdater.DownloadNewData(pnlProcessing);

                    //make sure no transaction is opened
                    if (DatabaseSingleton.Instance.currentTransaction != null)
                    {
                        DatabaseSingleton.Instance.RollbackTransaction();
                    }

                    //send mail alerts
                    if (a != null && !StockSourcesReader.CancelProcess)
                    {
                        var mailGenerator = new MailingGenerator();
                        mailGenerator.ProcessMails(0, pnlProcessing);
                    }

                    return true;
                }
            }
            else
            {
                disableLowMemMode();

                var a = await DataUpdater.DownloadFullData(pnlProcessing);

                //make sure no transaction is opened
                if (DatabaseSingleton.Instance.currentTransaction != null)
                {
                    DatabaseSingleton.Instance.RollbackTransaction();
                }

                if (a != null)
                {
                    MessageBox.Show("Update completed, please close this message and run the Application again.");
                    Application.Exit();
                    return false;
                }
                
                tmrCheckUpdates.Enabled = false;

                return false;
            }

            var returnValue = false;

            //check if reprocessing VR History is needed
            if(ReprocessVRHistory)
            {
                var a = await DataUpdater.ProcessVRHistory(pnlProcessing);
                ReprocessVRHistory = false;
                returnValue = true;
            }

            //check if reprocessing VR History is needed
            if (ReprocessETFSymbols)
            {
                var a = await DataUpdater.ProcessETFPrices(pnlProcessing);
                ReprocessETFSymbols = false;
                returnValue = true;
            }

            return returnValue;
        }

        private void disableLowMemMode()
        {
            if (MainPanel.SaveMemoryMode)
            {
                MainPanel.SaveMemoryMode = false;
                chkSaveMemory.Checked = false;
            }
        }

        private void lstStrategies_DoubleClick(object sender, EventArgs e)
        {
            if (lstStrategies.SelectedIndices.Count < 1)
                return;

            disableLowMemMode();

            StrategyView newView = new StrategyView((Strategy)lstStrategies.SelectedItems[0].Tag);
            newView.ShowDialog();
            RefreshStrategies();
        }

        private void cmdBacktest_Click(object sender, EventArgs e)
        {
            if (lstStrategies.SelectedIndices.Count < 1)
                return;

            disableLowMemMode();

            Strategy strategy = (Strategy)lstStrategies.SelectedItems[0].Tag;
            List<FeatureWeight> featuresW = Feature.generateFeatureWeightsList();
            
            for (int i = 0; i < featuresW.Count; i++)
            {
                FeatureWeight fw = featuresW[i];

                fw.IsEnabled = false;
                fw.Weight = 0;

                try
                {
                    Feature featureFound = strategy.Features.Find(x => x.FeatureId == i);

                    if (featureFound == null)
                        continue;

                    fw.Weight = (float)featureFound.FeatureWeight;

                    if (fw.Weight != 0)
                        fw.IsEnabled = true;
                }
                catch (Exception)
                { }
            }
             
            BacktestCalculator backtest = new BacktestCalculator(strategy.PortfolioParameters, featuresW, strategy.Filters);
            BacktestResults newView = new BacktestResults(backtest);
            newView.ShowDialog();
             
        }


        void ShowProcessing(String cmdText)
        {
            pnlProcessing.StartProcess();
            pnlProcessing.SetTitle("Starting Process...");
            pnlProcessing.SetCommand("Stop Process");
            pnlProcessing.SetMaxValue(100);
        }

        void HideProcessing()
        {
            pnlProcessing.StopProcess();
        }

        private void cmdViewCurrentStrategy_Click(object sender, EventArgs e)
        {
            if(selectedStrategy == null)
            {
                MessageBox.Show("Please select one Strategy in order to generate the desired portfolio.");
                return;
            }
            
            RealtimeView realtime = new RealtimeView(DesiredPortfolio, CurrentPortfolio, BacktestCalculatorCurrent, DesiredPortfolioCash, selectedStrategy.PortfolioParameters);
            realtime.Show();
        }

        private void cmdSelectStrategy_Click(object sender, EventArgs e)
        {
            if (lstStrategies.SelectedItems.Count == 0)
                return;

            Strategy.SetSelectedStrategy((Strategy)lstStrategies.SelectedItems[0].Tag);

            RefreshStrategies();

            refreshDesiredPortfolio();
        }

        private void refreshDesiredPortfolio()
        {
            if (Stock.CachedStocks == null)
            {
                pnlProcessing.StartProcess();
                Stock.ReadStockSymbolsThreads(true, pnlProcessing);
                HideProcessing();
            }


            //get current portfolio and get symbol and name
            CurrentPortfolio = PortfolioParameters.GetCurrentPortfolio();
            var stocks = Stock.GetCurrentStockNames();
            foreach (Position pos in CurrentPortfolio)
            {
                Stock stock = stocks.Find(x => x.Id == pos.IdStock);
                if (stock != null)
                {
                    pos.AuxName = stock.CompanyName;
                    pos.AuxSymbol = stock.Symbol;
                    pos.AuxSector = stock.getStockSector();
                }
            }

            refreshCurrentPortfolio();



            //refresh desired portfolio
            lstDesiredPortfolio.Items.Clear();
            if (selectedStrategy == null)
                return;


            //get last rebalancing date
            DateTime startDate = selectedStrategy.GetLastRebalanceDate();
            Strategy strategy = selectedStrategy;
            List<FeatureWeight> featuresW = Feature.generateFeatureWeightsList(strategy);
            
            BacktestCalculator backtest = new BacktestCalculator(selectedStrategy.PortfolioParameters, featuresW, strategy.Filters);

            BacktestResult outCash = new BacktestResult();
            backtest.RunBacktest(pnlProcessing, startDate, out outCash);

            HideProcessing();

            DesiredPortfolioMonthlySnapshots = backtest.MonthlySnapshots;
            DesiredPortfolio = backtest.LastDayPositions;
            DesiredPortfolioCash = backtest.LastDayCash;
            DesiredPortfolio1YearBack = backtest.YearBackPortfolioValue;
            BacktestCalculatorCurrent = backtest;

            List<int> alreadyShown = new List<int>();
#if TESTDEV
            Console.WriteLine("Desired Portfolios");
#endif
            foreach (Position pos in DesiredPortfolio)
            {
                Stock stock = Stock.GetStock(pos.IdStock);
#if TESTDEV
                Console.WriteLine(pos.AuxSymbol + ", " + pos.DateEntered + ", " + pos.EntryPrice + ", " + pos.Shares + ", " + pos.SellingStyle);
#endif
                if (alreadyShown.Contains(pos.IdStock)) continue;
                ListViewItem newItem = new ListViewItem("NOT FOUND");
                if (stock != null)
                {
                    int count, firstdate;
                    
                    alreadyShown.Add(pos.IdStock);
                    newItem.Text = stock.Symbol;
                    
                    decimal avgprice = getAveragePrice(pos.IdStock, out count, out firstdate);
                    newItem.SubItems.Add(Utils.ConvertIntToDateTime(firstdate).ToString("MM-dd-yyyy"));
                    if (count == 1)
                    {
                        newItem.SubItems.Add(avgprice.ToString("n2"));
                    } else
                    {
                        newItem.SubItems.Add(avgprice.ToString("n2") + "*");
                    }
                    
                    newItem.SubItems.Add(pos.SellingStyle.ToString());
                    newItem.SubItems.Add(stock.CompanyName);
                    newItem.SubItems.Add(stock.getStockSector());
                }

                lstDesiredPortfolio.Items.Add(newItem);
            }
        }

        decimal getAveragePrice(int idStock, out int count, out int firstdate)
        {
            int totalShare = 0;
            decimal totalprice = 0;
            count = 0;
            firstdate = 0;
            foreach (Position pos in DesiredPortfolio)
            {
                if (pos.IdStock == idStock)
                {
                    if (count == 0) firstdate = pos.DateEntered;
                    else if (firstdate > pos.DateEntered) firstdate = pos.DateEntered;
                    count++;
                    totalShare += pos.Shares;
                    totalprice += pos.EntryPrice * pos.Shares;
                }
            }
            if (totalShare == 0) return 0;
            return totalprice / totalShare;
        }

        void refreshCurrentPortfolio()
        {
            lstCurrentPortfolio.Items.Clear();

            foreach (Position pos in CurrentPortfolio)
            {
                ListViewItem newItem = new ListViewItem(pos.AuxSymbol);
                newItem.SubItems.Add(Utils.ConvertIntToDateTime(pos.DateEntered).ToString("MM-dd-yyyy"));
                newItem.SubItems.Add(pos.Shares.ToString("n0"));
                newItem.SubItems.Add(pos.EntryPrice.ToString("n2"));
                newItem.SubItems.Add(pos.AuxName);
                newItem.SubItems.Add(pos.AuxSector);

                lstCurrentPortfolio.Items.Add(newItem);
            }
             
        }

        private async void MainPanel_Shown(object sender, EventArgs e)
        {
            //todo change if needed to reactivate
            if (!checkform.isPassed)
            {
                this.Hide();
                if (checkform.ShowDialog(this) == DialogResult.OK)
                {
                    this.Show();
                }
                else
                {
                    Environment.Exit(Environment.ExitCode);
                }
            }

            if (DataUpdater.UpdatingDatabase)
                return;

            DisableUpdates = true;

            bool result = await UpdateData();

            if (!pnlProcessing.IsShowingError)
            {
                refreshDesiredPortfolio();
            }

            DisableUpdates = false;
        }

        private async void tmrCheckUpdates_Tick(object sender, EventArgs e)
        {
            if (DisableUpdates)
                return;

            bool result = await UpdateData();
            
            if (result)
            {
                refreshDesiredPortfolio();

                if (MainPanel.SaveMemoryMode)
                    Stock.ClearCache();
            }

        }

        private void cmdEditPositions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EditPositions editPositions = new EditPositions(CurrentPortfolio);
            if (editPositions.ShowDialog() != DialogResult.OK)
                return;

            CurrentPortfolio = editPositions.CurrentPortfolio;

            refreshCurrentPortfolio();
            
            if (MainPanel.SaveMemoryMode)
                Stock.ClearCache();
        }

        private void cmdEditPortfolio_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EditPortfolio frm = new EditPortfolio(PortfolioParameters.Load(0), true, true, 0,false);
            var dialogResult = frm.ShowDialog();

            RefreshPortfolioParameters();
            
            if (frm.DataFullUpdate)
            {
                tmrCheckUpdates_Tick(null, null);
                frm.DataFullUpdate = false;
                return;
            }

            if (frm.ReprocessVRHistory)
            {
                ReprocessVRHistory = true;
                tmrCheckUpdates_Tick(null, null);
                frm.ReprocessVRHistory = false;
            }

            if (frm.ReprocessETFSymbols)
            {
                ReprocessETFSymbols = true;
                tmrCheckUpdates_Tick(null, null);
                frm.ReprocessETFSymbols = false;
            }

            if (dialogResult == DialogResult.OK)
            {
                RefreshStrategies();
                refreshDesiredPortfolio();
            }
        }


        private void cmdDeleteStrategy_Click(object sender, EventArgs e)
        {
            if (lstStrategies.SelectedItems.Count == 0)
                return;

            string text = Microsoft.VisualBasic.Interaction.InputBox(
                "Are you sure you want to delete the strategy: " + ((Strategy)lstStrategies.SelectedItems[0].Tag).Name + ". Type DELETE in Uppercase in order to continue",
                "Delete Strategy",
                "");

            if (text.Trim() != "DELETE")
                return;
            
            Strategy.DeleteStrategy((Strategy)lstStrategies.SelectedItems[0].Tag);

            RefreshStrategies();

            refreshDesiredPortfolio();
            
            if (MainPanel.SaveMemoryMode)
                Stock.ClearCache();
        }

        private void chkSaveMemory_CheckedChanged(object sender, EventArgs e)
        {
            SaveMemoryMode = chkSaveMemory.Checked;

            if(MainPanel.SaveMemoryMode)
                Stock.ClearCache();
        }

        private void MainPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Environment.Exit(Environment.ExitCode);
            }
            catch (Exception ex)
            { }
        }
        
        private void cmdCompositeGraph_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            System.Windows.Forms.Application.DoEvents();

            if (selectedStrategy != null)
                (new CompositeGraph(Stock.GetStockIdFromTicker("AAPL"), selectedStrategy.PortfolioParameters)).ShowDialog();
            else
                (new CompositeGraph(Stock.GetStockIdFromTicker("AAPL"), PortfolioParameters.Load(0))).ShowDialog();

            this.Cursor = Cursors.Arrow;
        }

        private void cmdUnselectStrategy_Click(object sender, EventArgs e)
        {
            Strategy.ClearSelectedStrategy();

            RefreshStrategies();

            refreshDesiredPortfolio();
        }

        private void cmdPerformance_Click(object sender, EventArgs e)
        {
            if (selectedStrategy == null)
                MessageBox.Show("Please select a default strategy in order to show the performance graph.");
           
            BacktestResults.ShowPerformanceGraphOnNextRun = true;

            foreach (ListViewItem item in lstStrategies.Items)
            {
                if (item.Tag == selectedStrategy)
                    item.Selected = true;
                else
                    item.Selected = false;
            }

            cmdBacktest_Click(this, null);
        }

        private void cmdSendMailAlerts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var mailGenerator = new MailingGenerator();
            mailGenerator.ProcessMails(0, pnlProcessing);
        }
    }
}
