using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace StockRanking
{
    public partial class EditPortfolio : Form
    {
        public static String allSectorsStr = "ALL SECTORS";
        public bool DataFullUpdate = false;
        public bool updating = false;
        String customRiskFile = "";
        String customRiskFileBlendedBenchmark = "";
        bool saveIntoDB = true;
        PortfolioParameters portfolio = null;
        public bool ReprocessVRHistory = false;
        public bool ReprocessETFSymbols = false;
        int strategyId = 0;
        bool editingStrategy = false;
        List<Stock> etfStocks = new List<Stock>();
        List<FileBasedStock> fileBasedStocks = new List<FileBasedStock>();
        
        public EditPortfolio(PortfolioParameters portfolio, bool showFullReprocess, bool saveIntoDB, int pstrategyId, bool editingStrategy)
        {
            InitializeComponent();
            this.portfolio = portfolio;
            this.saveIntoDB = saveIntoDB;
            this.DialogResult = DialogResult.Cancel;
            this.strategyId = pstrategyId;
            this.editingStrategy = editingStrategy;

            cmdFullUpdate.Visible = showFullReprocess;

            updating = true;

            lblCompositeYears.Text = portfolio.GetCompositeRollingMedianYears().ToString() + " years";

            //load portfolio parameters from input object
            txtAccountSize.Value = portfolio.AccountSize;
            txtBenchmark.Value = (decimal)portfolio.BenchmarkPercent;
            txtCommission.Value = (decimal)portfolio.CommisionPerShare;
            txtMaxPosPerIndustry.Value = portfolio.MaxPositionsPerIndustry;
            txtMaxWeight.Value = portfolio.MaxWeightPerPosition * 100;
            txtPositions.Value = portfolio.Positions;
            txtShortPositions.Value = portfolio.ShortPositions;
            if (portfolio.LongSelling == 0)
            {
                chkLong.Checked = false;
                cboLongAt.SelectedIndex = -1;
            } else
            {
                chkLong.Checked = true;
                cboLongAt.SelectedIndex = portfolio.LongSelling - 1;
            }
            if (portfolio.ShortSelling == 0)
            {
                chkShort.Checked = false;
                cboShortAt.SelectedIndex = -1;
            }
            else
            {
                chkShort.Checked = true;
                cboShortAt.SelectedIndex = portfolio.ShortSelling - 1;
            }
            chkFilterByIndustry.Checked = portfolio.IndustriesIncluded.Count > 0;
            chkMaxPorPerInd.Checked = portfolio.MaxPositionsPerIndustry > 0;
            chkUseRiskModel.Checked = portfolio.UseRiskModel > 0;
            txtYOYCape.Value = portfolio.CapeYOY;
            txtMOMCape.Value = portfolio.CapeMOM;
            txtHiLoAVG.Value = portfolio.HiLoDays;
            txtHiLoPercent.Value = portfolio.HiLoPercent;
            txtAVGDays1.Value = portfolio.SPYAvgDays1;
            txtAVGDays2.Value = portfolio.SPYAvgDays2;
            txtSPXUPercent.Value = portfolio.SPXUPercent;
            chkCapeModel.Checked = portfolio.UseCapeModel > 0;
            chkMovingAVGModel.Checked = portfolio.UseAVGModel > 0;
            chkHiLoModel.Checked = portfolio.UseHiLoModel > 0;
            chkCustomRiskFile.Checked = portfolio.UseCustomFileModel > 0;
            chkBuyLeftoverMoney.Checked = portfolio.BuyBenchmarkLeftoverCash > 0;
            chkReBuyOption.Checked = portfolio.ReBuyAfterStop > 0;
            txtCustomRiskFile.Text = Path.GetFileName(portfolio.CustomRiskFile);
            txtAnnualFee.Value = portfolio.AnnualFee;
            customRiskFile = portfolio.CustomRiskFile;

            cboMACD.Items.Add("greater than");
            cboMACD.Items.Add("less than");
            cboRSI.Items.Add("greater than");
            cboRSI.Items.Add("less than");
            cboStochastic.Items.Add("greater than");
            cboStochastic.Items.Add("less than");

            chkMACD.Checked = portfolio.UseMACDModel > 0;
            txtMACDLoopback.Value = portfolio.MACDLoopback;
            txtMACDLoopback1.Value = portfolio.MACDLoopback1;
            txtMACDLoopback2.Value = portfolio.MACDLoopback2;
            cboMACD.SelectedIndex = portfolio.MACDCompare;
            txtMACD.Value = portfolio.MACDThreshold;

            chkRSI.Checked = portfolio.UseRSIModel > 0;
            txtRSILoopback.Value = portfolio.RSILoopback;
            cboRSI.SelectedIndex = portfolio.RSICompare;
            txtRSI.Value = portfolio.RSIThreshold;

            chkStochastic.Checked = portfolio.UseStochasticModel > 0;
            txtStochasticLoopback.Value = portfolio.StochasticLoopback;
            cboStochastic.SelectedIndex = portfolio.StochasticCompare;
            txtStochastic.Value = portfolio.StochasticThreshold;

            chkOnlyInitial.Checked = portfolio.LossOnlyInitial > 0;
            txtDelayEntry.Value = portfolio.MonthDelayEntry;

            txtCompositePB.Value = portfolio.CompositePBWeight;
            txtCompositePS.Value = portfolio.CompositePSWeight;
            txtCompositePE.Value = portfolio.CompositePEWeight;
            txtCompositePFCF.Value = portfolio.CompositePFCFWeight;
            txtVRHistoryYears.Value = portfolio.VRHistoryYears;

            txtFilterComissionPercent.Value = portfolio.FilterComissionsPerc;
            txtFilterVolume.Value = portfolio.FilterMinVolume;

            //load industries
            cboRebalance.ValueMember = "Key";
            cboRebalance.DisplayMember = "Value";
            cboRebalance.Items.Add(new KeyValuePair<int, string>(1, "Montly"));
            cboRebalance.Items.Add(new KeyValuePair<int, string>(3, "Quarterly"));
            cboRebalance.Items.Add(new KeyValuePair<int, string>(6, "Six Months"));
            cboRebalance.Items.Add(new KeyValuePair<int, string>(12, "Yearly"));

            if (portfolio.BABCustomRiskFile != "")
            {
                chkBACustomRiskFile.Checked = true;
                txtBACustomRiskFile.Text = Path.GetFileName(portfolio.BABCustomRiskFile);
                customRiskFileBlendedBenchmark = portfolio.BABCustomRiskFile;
            }

            int index = 0;
            foreach (KeyValuePair<int, string> pair in cboRebalance.Items)
            {
                if (pair.Key == portfolio.RebalanceFrequencyMonths)
                    cboRebalance.SelectedIndex = index;
                index++;
            }

            //load industries
            lstIndustries.Items.Clear();
            lstIndustriesSelected.Items.Clear();

            foreach (String ind in Stock.getAllSectors())
                lstIndustries.Items.Add(new ListViewItem(ind));

            foreach (String ind in portfolio.IndustriesIncluded)
            {
                foreach (ListViewItem item in lstIndustries.Items)
                {
                    if (item.Text.Trim().ToUpper() == ind.Trim().ToUpper())
                    {
                        lstIndustries.Items.Remove(item);
                        lstIndustriesSelected.Items.Add(item);
                    }
                }
            }

            if (lstIndustriesSelected.Items.Count == 0)
            {
                lstIndustriesSelected.Items.Add(new ListViewItem(allSectorsStr));
            }

            //load stop loss
            chkUseStopLoss.Checked = false;
            if (portfolio.EntryStopLoss != 0)
            {
                chkUseStopLoss.Checked = true;
                txtStopInitial.Value = portfolio.EntryStopLoss;
                txtStopTrailing.Value = portfolio.TrailingStopLoss;
            }

            updating = false;

            refreshCheckboxes();

            refreshFileBasedStocks(false);

            refreshETFStocks(false);

            refreshSymbolsComboboxes();


            //select current combobox items
            cboSymbolBenchmark.SelectedValue = portfolio.IdSymbolBenchmark;
            cboSymbolInverse.SelectedValue = portfolio.IdSymbolInverse;

            cboBAAsset1.SelectedValue = portfolio.BABAsset1Id;
            cboBAAsset2.SelectedValue = portfolio.BABAsset2Id;
            cboBAAsset3.SelectedValue = portfolio.BABAsset3Id;
            cboBAAsset4.SelectedValue = portfolio.BABAsset4Id;
            cboBARiskAsset1.SelectedValue = portfolio.BABRiskAssetConfigurable1;
            cboBARiskAsset2.SelectedValue = portfolio.BABRiskAssetConfigurable2;

            txtBAStock.Value = portfolio.BABStocksPercent;
            txtBABonds.Value = portfolio.BABBondsPercent;
            txtBACash.Value = portfolio.BABCashPercent;

            cboBondsAsset1.SelectedValue = portfolio.BondsAsset1Id;
            cboBondsAsset2.SelectedValue = portfolio.BondsAsset2Id;
            cboBondsAsset3.SelectedValue = portfolio.BondsAsset3Id;
            cboBondsAsset4.SelectedValue = portfolio.BondsAsset4Id;
            cboBondsRiskAsset.SelectedValue = portfolio.BondsRiskModelAssetId;

            txtBondsMomentumWindow.Value = portfolio.BondsMomentumWindow;
            txtBondsMovingAvg1.Value = portfolio.BondsMovingAvg1;
            txtBondsMovingAvg2.Value = portfolio.BondsMovingAvg2;

            //disable certain tabs if editing strategy parameters
            if (editingStrategy)
            {
                loadMailingParameters(portfolio.MailingParameters);

                foreach (Control control in tabBlendedAdvanced.Controls)
                    control.Enabled = false;
                foreach (Control control in tabFileBased.Controls)
                    control.Enabled = false;
                foreach (Control control in tabETF.Controls)
                    control.Enabled = false;
            }
            else
            {
                foreach (Control control in tabMailing.Controls)
                    control.Enabled = false;
                lnkMailConfigureSmtp.Enabled = true;
            }
        }

        void refreshCheckboxes()
        {
            if (updating)
                return;

            txtMaxPosPerIndustry.Enabled = chkMaxPorPerInd.Checked;
            lstIndustries.Enabled = chkFilterByIndustry.Checked;
            lstIndustriesSelected.Enabled = chkFilterByIndustry.Checked;
            cmdAdd.Enabled = chkFilterByIndustry.Checked;
            cmdRemove.Enabled = chkFilterByIndustry.Checked;

            if (!txtMaxPosPerIndustry.Enabled)
                txtMaxPosPerIndustry.Value = 0;

            txtStopInitial.Enabled = chkUseStopLoss.Checked;
            txtStopTrailing.Enabled = chkUseStopLoss.Checked && !chkOnlyInitial.Checked;

            if (chkUseStopLoss.Checked && txtStopInitial.Value == 0)
            {
                txtStopInitial.Value = 30;
                txtStopTrailing.Value = 60;
            }

            if(chkUseRiskModel.Checked)
            {
                txtYOYCape.Enabled = true;
                txtMOMCape.Enabled = true;
                txtHiLoAVG.Enabled = true;
                txtHiLoPercent.Enabled = true;
                txtAVGDays1.Enabled = true;
                txtAVGDays2.Enabled = true;
                txtSPXUPercent.Enabled = true;
                cboSymbolInverse.Enabled = true;
                chkCapeModel.Enabled = true;
                chkHiLoModel.Enabled = true;
                chkMovingAVGModel.Enabled = true;
                chkCustomRiskFile.Enabled = true;
                cmdSelectCustomFile.Enabled = true;
                txtCustomRiskFile.Enabled = true;
            }
            else
            {
                txtYOYCape.Enabled = false;
                txtMOMCape.Enabled = false;
                txtHiLoAVG.Enabled = false;
                txtHiLoPercent.Enabled = false;
                txtAVGDays1.Enabled = false;
                txtAVGDays2.Enabled = false;
                txtSPXUPercent.Enabled = false;
                cboSymbolInverse.Enabled = false;
                chkCapeModel.Enabled = false;
                chkHiLoModel.Enabled = false;
                chkMovingAVGModel.Enabled = false;
                chkCustomRiskFile.Enabled = false;
                cmdSelectCustomFile.Enabled = false;
                txtCustomRiskFile.Enabled = false;
            }

            chkRSI.Enabled = chkUseRiskModel.Checked;
            chkMACD.Enabled = chkUseRiskModel.Checked;
            chkStochastic.Enabled = chkUseRiskModel.Checked;
            cboRSI.Enabled = chkRSI.Enabled && chkRSI.Checked;
            txtRSI.Enabled = chkRSI.Enabled && chkRSI.Checked;
            cboMACD.Enabled = chkMACD.Enabled && chkMACD.Checked;
            txtMACD.Enabled = chkMACD.Enabled && chkMACD.Checked;
            cboStochastic.Enabled = chkStochastic.Enabled && chkStochastic.Checked;
            txtStochastic.Enabled = chkStochastic.Enabled && chkStochastic.Checked;
            txtMACDLoopback.Enabled = chkMACD.Enabled && chkMACD.Checked;
            txtMACDLoopback1.Enabled = chkMACD.Enabled && chkMACD.Checked;
            txtMACDLoopback2.Enabled = chkMACD.Enabled && chkMACD.Checked;
            txtRSILoopback.Enabled = chkRSI.Enabled && chkRSI.Checked;
            txtStochasticLoopback.Enabled = chkStochastic.Enabled && chkStochastic.Checked;

            if (!chkMovingAVGModel.Checked)
            {
                txtAVGDays1.Enabled = false;
                txtAVGDays2.Enabled = false;
            }

            if (!chkCapeModel.Checked)
            {
                txtYOYCape.Enabled = false;
                txtMOMCape.Enabled = false;
            }

            if (!chkHiLoModel.Checked)
            {
                txtHiLoAVG.Enabled = false;
                txtHiLoPercent.Enabled = false;
            }

            if (!chkCustomRiskFile.Checked)
            {
                txtCustomRiskFile.Enabled = false;
                cmdSelectCustomFile.Enabled = false;
            }

            txtBACustomRiskFile.Enabled = false;
            cmdBACustomRiskFile.Enabled = false;
            if (chkBACustomRiskFile.Checked)
            {
                txtBACustomRiskFile.Enabled = true;
                cmdBACustomRiskFile.Enabled = true;
            }

            if (chkLong.Checked)
            {
                cboLongAt.Enabled = true;
                txtPositions.Enabled = true;
            } else
            {
                cboLongAt.Enabled = false;
                txtPositions.Enabled = false;
            }

            if (chkShort.Checked)
            {
                cboShortAt.Enabled = true;
                txtShortPositions.Enabled = true;
            }
            else
            {
                cboShortAt.Enabled = false;
                txtShortPositions.Enabled = false;
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            if (chkLong.Checked == false && chkShort.Checked == false)
            {
                MessageBox.Show("Please select at least one of long or short selling.");
                return;
            }
            if ((chkLong.Checked == true && cboLongAt.SelectedIndex == -1) || (chkShort.Checked == true && cboShortAt.SelectedIndex == -1) || 
                (chkLong.Checked == true && chkShort.Checked == true && cboLongAt.SelectedIndex == cboShortAt.SelectedIndex))
            {
                MessageBox.Show("Please select valid positions of short or long selling.");
                return;
            }
            if (chkUseRiskModel.Checked)
            {
                int checkcount = 0;
                checkcount += chkCapeModel.Checked ? 1 : 0;
                checkcount += chkHiLoModel.Checked ? 1 : 0;
                checkcount += chkCustomRiskFile.Checked ? 1 : 0;
                checkcount += chkMovingAVGModel.Checked ? 1 : 0;
                checkcount += chkMACD.Checked ? 1 : 0;
                checkcount += chkRSI.Checked ? 1 : 0;
                checkcount += chkStochastic.Checked ? 1 : 0;

                if (checkcount > 3)
                {
                    MessageBox.Show("Only 3 risk models selected at the same time are allowed.");
                    return;
                }
            }

            if(cboSymbolBenchmark.Text.Contains("MTUM"))
            {
                if(!DataUpdater.CheckMTUMData())
                {
                    MessageBox.Show("Please upload a Custom MTUM file before using MTUM or Custom MTUM benchmark symbol.");
                    return;
                }
            }

            if(saveIntoDB)
            {
                if(txtBABonds.Value + txtBAStock.Value + txtBACash.Value != 100)
                {
                    MessageBox.Show("Blended Advanced Backtest percentages should add up to 100%, please review the values.");
                    return;
                }
            }

            //check is VR history values changed
            bool vrHistoryChanged = false;
            if (portfolio.CompositePBWeight != txtCompositePB.Value ||
               portfolio.CompositePEWeight != txtCompositePE.Value ||
               portfolio.CompositePSWeight != txtCompositePS.Value ||
               portfolio.CompositePFCFWeight != txtCompositePFCF.Value ||
               portfolio.VRHistoryYears != txtVRHistoryYears.Value)
            {
                vrHistoryChanged = true;
            }

            PortfolioParameters newParameters = portfolio;
            if (saveIntoDB)
                newParameters = new PortfolioParameters();

            newParameters.AccountSize = (int)txtAccountSize.Value;
            newParameters.BenchmarkPercent = (float)txtBenchmark.Value;
            newParameters.CommisionPerShare = (float)txtCommission.Value;
            newParameters.MaxPositionsPerIndustry = (int)txtMaxPosPerIndustry.Value;
            newParameters.MaxWeightPerPosition = txtMaxWeight.Value / 100;
            newParameters.Positions = (int)txtPositions.Value;
            if (chkLong.Checked == false)
            {
                newParameters.LongSelling = 0;
            } else
            {
                newParameters.LongSelling = cboLongAt.SelectedIndex + 1;
            }
            newParameters.ShortPositions = (int)txtShortPositions.Value;
            if (chkShort.Checked == false)
            {
                newParameters.ShortSelling = 0;
            } else
            {
                newParameters.ShortSelling = cboShortAt.SelectedIndex + 1;
            }
            newParameters.RebalanceFrequencyMonths = ((KeyValuePair<int, string>)cboRebalance.SelectedItem).Key;
            newParameters.IdSymbolBenchmark = (int)cboSymbolBenchmark.SelectedValue;
            newParameters.IdSymbolInverse = (int)cboSymbolInverse.SelectedValue;

            newParameters.IndustriesIncluded.Clear();
            if (chkFilterByIndustry.Checked)
                foreach (ListViewItem item in lstIndustriesSelected.Items)
                    if(item.Text != allSectorsStr)
                        newParameters.IndustriesIncluded.Add(item.Text);

            newParameters.TrailingStopLoss = 0;
            newParameters.EntryStopLoss = 0;
            if (chkUseStopLoss.Checked)
            {
                newParameters.EntryStopLoss = txtStopInitial.Value;
                newParameters.TrailingStopLoss = txtStopTrailing.Value;
            }

            newParameters.UseRiskModel = chkUseRiskModel.Checked ? 1 : 0;
            newParameters.UseCapeModel = chkCapeModel.Checked ? 1 : 0;
            newParameters.UseAVGModel = chkMovingAVGModel.Checked ? 1 : 0;
            newParameters.UseHiLoModel = chkHiLoModel.Checked ? 1 : 0;
           
            newParameters.CapeYOY = txtYOYCape.Value;
            newParameters.CapeMOM = txtMOMCape.Value;
            newParameters.HiLoDays = txtHiLoAVG.Value;
            newParameters.HiLoPercent = txtHiLoPercent.Value;
            newParameters.SPYAvgDays1 = Convert.ToInt32(txtAVGDays1.Value);
            newParameters.SPYAvgDays2 = Convert.ToInt32(txtAVGDays2.Value);
            newParameters.SPXUPercent = txtSPXUPercent.Value;

            newParameters.UseCustomFileModel = chkCustomRiskFile.Checked ? 1 : 0;
            newParameters.BuyBenchmarkLeftoverCash = chkBuyLeftoverMoney.Checked ? 1 : 0;
            newParameters.ReBuyAfterStop = chkReBuyOption.Checked ? 1 : 0;
            newParameters.CustomRiskFile = customRiskFile;
            newParameters.AnnualFee = txtAnnualFee.Value;

            newParameters.CompositePFCFWeight = txtCompositePFCF.Value;
            newParameters.CompositePSWeight = txtCompositePS.Value;
            newParameters.CompositePEWeight = txtCompositePE.Value;
            newParameters.CompositePBWeight = txtCompositePB.Value;
            newParameters.VRHistoryYears = (int)txtVRHistoryYears.Value;

            newParameters.FilterMinVolume = (int)txtFilterVolume.Value;
            newParameters.FilterComissionsPerc = txtFilterComissionPercent.Value;
            newParameters.MaxSharesPennyStocks = (long)txtMaxSharesPennyStock.Value;

            newParameters.UseMACDModel = chkMACD.Checked ? 1 : 0;
            newParameters.MACDLoopback = txtMACDLoopback.Value;
            newParameters.MACDLoopback1 = txtMACDLoopback1.Value;
            newParameters.MACDLoopback2 = txtMACDLoopback2.Value;
            newParameters.MACDCompare = cboMACD.SelectedIndex;
            newParameters.MACDThreshold = txtMACD.Value;

            newParameters.UseRSIModel = chkRSI.Checked ? 1 : 0;
            newParameters.RSILoopback = txtRSILoopback.Value;
            newParameters.RSICompare = cboRSI.SelectedIndex;
            newParameters.RSIThreshold = txtRSI.Value;

            newParameters.UseStochasticModel = chkStochastic.Checked ? 1 : 0;
            newParameters.StochasticLoopback = txtStochasticLoopback.Value;
            newParameters.StochasticCompare = cboStochastic.SelectedIndex;
            newParameters.StochasticThreshold = txtStochastic.Value;

            newParameters.LossOnlyInitial = chkOnlyInitial.Checked ? 1 : 0;
            newParameters.MonthDelayEntry = txtDelayEntry.Value;

            if (saveIntoDB)
            {
                if(cboBAAsset1.SelectedItem != null)
                    newParameters.BABAsset1Id = (int)cboBAAsset1.SelectedValue;
                if (cboBAAsset1.SelectedItem != null)
                    newParameters.BABAsset2Id = (int)cboBAAsset2.SelectedValue;
                if (cboBAAsset1.SelectedItem != null)
                    newParameters.BABAsset3Id = (int)cboBAAsset3.SelectedValue;
                if (cboBAAsset1.SelectedItem != null)
                    newParameters.BABAsset4Id = (int)cboBAAsset4.SelectedValue;
                if (cboBARiskAsset1.SelectedItem != null)
                    newParameters.BABRiskAssetConfigurable1 = (int)cboBARiskAsset1.SelectedValue;
                if (cboBARiskAsset2.SelectedItem != null)
                    newParameters.BABRiskAssetConfigurable2 = (int)cboBARiskAsset2.SelectedValue;

                newParameters.BABStocksPercent = txtBAStock.Value;
                newParameters.BABBondsPercent = txtBABonds.Value;
                newParameters.BABCashPercent = txtBACash.Value;

                newParameters.BABCustomRiskFile = customRiskFileBlendedBenchmark;
                if (!chkBACustomRiskFile.Checked)
                    newParameters.BABCustomRiskFile = "";

                if (cboBondsAsset1.SelectedItem != null)
                    newParameters.BondsAsset1Id = (int)cboBondsAsset1.SelectedValue;
                if (cboBondsAsset2.SelectedItem != null)
                    newParameters.BondsAsset2Id = (int)cboBondsAsset2.SelectedValue;
                if (cboBondsAsset3.SelectedItem != null)
                    newParameters.BondsAsset3Id = (int)cboBondsAsset3.SelectedValue;
                if (cboBondsAsset4.SelectedItem != null)
                    newParameters.BondsAsset4Id = (int)cboBondsAsset4.SelectedValue;
                if (cboBondsRiskAsset.SelectedItem != null)
                    newParameters.BondsRiskModelAssetId = (int)cboBondsRiskAsset.SelectedValue;

                newParameters.BondsMomentumWindow = (int)txtBondsMomentumWindow.Value;
                newParameters.BondsMovingAvg1 = (int)txtBondsMovingAvg1.Value;
                newParameters.BondsMovingAvg2 = (int)txtBondsMovingAvg2.Value;
            }


            if (saveIntoDB)
            {
                newParameters.Save();
            }

            if (strategyId > 0)
            {
                newParameters.Id = strategyId;
                newParameters.Save();
            }

            if (editingStrategy)
            {
                newParameters.MailingParameters = saveMailingParameters(strategyId);
            }

            if (vrHistoryChanged)
            {
                //check if there is a history ID for this logic
                bool newRecord = false;
                int historyId = DataUpdater.GenerateVRHistoryId(newParameters, out newRecord);
                ReprocessVRHistory = newRecord;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private MailingParameters saveMailingParameters(int strategyId)
        {
            MailingParameters mailingParams = new MailingParameters();
            mailingParams.IdStrategy = strategyId;
            mailingParams.Destinations = txtMailRecipients.Text.Split(',').ToList();
            mailingParams.IncludeBondsModel = chkMailBond.Checked;
            mailingParams.IncludePerformanceTable = chkMailPerformance.Checked;
            mailingParams.IncludePortfolio = chkMailFullPortfolio.Checked;
            mailingParams.IncludeRiskModel = chkMailRisk.Checked;
            mailingParams.IncludeTopStocks = (int)txtMailingTopStocks.Value;

            String days = (chkMailMonday.Checked ? "1" : "") +
                (chkMailTuesday.Checked ? "2" : "") +
                (chkMailWednesday.Checked ? "3" : "") +
                (chkMailThursday.Checked ? "4" : "") +
                (chkMailFriday.Checked ? "5" : "");
            mailingParams.SendWeekdays = days;
            mailingParams.SendRebalance = chkMailSendRebalance.Checked;
            mailingParams.SendSells = chkMailSells.Checked;

            if(strategyId != 0)
                mailingParams.Save();

            return mailingParams;
        }

        private void loadMailingParameters(MailingParameters mailingParams)
        {
            txtMailRecipients.Text = String.Join(",", mailingParams.Destinations);
            chkMailBond.Checked = mailingParams.IncludeBondsModel;
            chkMailPerformance.Checked = mailingParams.IncludePerformanceTable;
            chkMailFullPortfolio.Checked = mailingParams.IncludePortfolio;
            chkMailRisk.Checked = mailingParams.IncludeRiskModel;
            txtMailingTopStocks.Value = mailingParams.IncludeTopStocks;

            chkMailMonday.Checked = mailingParams.SendWeekdays.Contains("1");
            chkMailTuesday.Checked = mailingParams.SendWeekdays.Contains("2");
            chkMailWednesday.Checked = mailingParams.SendWeekdays.Contains("3");
            chkMailThursday.Checked = mailingParams.SendWeekdays.Contains("4");
            chkMailFriday.Checked = mailingParams.SendWeekdays.Contains("5");

            chkMailSendRebalance.Checked = mailingParams.SendRebalance;
            chkMailSells.Checked = mailingParams.SendSells;
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkFilterByIndustry_CheckedChanged(object sender, EventArgs e)
        {
            refreshCheckboxes();
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            if (lstIndustries.SelectedItems.Count == 0)
                return;
             
            List<ListViewItem> items = new List<ListViewItem>();
            foreach (ListViewItem item in lstIndustries.SelectedItems)
                items.Add(item);

            foreach (ListViewItem item2 in lstIndustriesSelected.Items)
                if (item2.Text == allSectorsStr)
                    lstIndustriesSelected.Items.Remove(item2);

            foreach (ListViewItem item in items)
            {
                lstIndustries.Items.Remove(item);
                lstIndustriesSelected.Items.Add(item);
            }
        }

        private void cmdRemove_Click(object sender, EventArgs e)
        {
            if (lstIndustriesSelected.SelectedItems.Count == 0)
                return;

            List<ListViewItem> items = new List<ListViewItem>();
            foreach (ListViewItem item in lstIndustriesSelected.SelectedItems)
                items.Add(item);

            foreach (ListViewItem item in items)
            {
                if (item.Text != allSectorsStr)
                {
                    lstIndustriesSelected.Items.Remove(item);
                    lstIndustries.Items.Add(item);
                }
            }

            if(lstIndustriesSelected.Items.Count == 0)
            {
                lstIndustriesSelected.Items.Add(new ListViewItem(allSectorsStr));
            }
        }

        private void chkUseStopLoss_CheckedChanged(object sender, EventArgs e)
        {
            refreshCheckboxes();
        }

        private void EditPortfolio_Load(object sender, EventArgs e)
        {

        }

        private void chkUseRiskModel_CheckedChanged(object sender, EventArgs e)
        {
            refreshCheckboxes();
        }

        private void cmdFullUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to run a full Data Update. This process will recreate all the downloaded Stocks and Metrics data. It will take some time to process. Strategies and other parameters will not be lost.", "Full Data Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                != DialogResult.Yes)
                return;

            DatabaseSingleton.Instance.ExecuteNonQuery("DELETE FROM SYSTEM_PARAMETERS WHERE ID IN (1,2)", null);

            DataFullUpdate = true;

            this.Close();
        }

        private void chkCapeModel_CheckedChanged(object sender, EventArgs e)
        {
            refreshCheckboxes();
        }

        private void chkMovingAVGModel_CheckedChanged(object sender, EventArgs e)
        {
            refreshCheckboxes();
        }

        private void chkHiLoModel_CheckedChanged(object sender, EventArgs e)
        {
            refreshCheckboxes();
        }

        private void chkCustomRiskFile_CheckedChanged(object sender, EventArgs e)
        {
            refreshCheckboxes();
        }

        private void cmdSelectCustomFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(customRiskFile.Trim() != "")
                if(File.Exists(customRiskFile))
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(customRiskFile);

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            customRiskFile = openFileDialog.FileName;

            txtCustomRiskFile.Text = Path.GetFileName(customRiskFile);
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cmdCompositeYears_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            CompositeYearsPopup popup = new CompositeYearsPopup();
            popup.ShowDialog();
            lblCompositeYears.Text = portfolio.GetCompositeRollingMedianYears().ToString() + " years";

        }

        private void cmdUploadMTUMValues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog.FileName = "";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            this.Cursor = Cursors.WaitCursor;

            String mtumFile = openFileDialog.FileName;

            //load MTUM data
            try
            {
                DataUpdater.ImportCustomMTUMData(mtumFile);

                ZachsSourceReader.CancelProcess = false;
                StockSourcesReader.CancelProcess = false;
                var a = DataUpdater.UpdateBenchmarkSymbolValues(null, Stock.GetStock(-9), true).Result;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                if(ex.InnerException != null)
                    MessageBox.Show("Error importing MTUM Data: " + ex.InnerException.Message);
                else
                    MessageBox.Show("Error importing MTUM Data: " + ex.Message);

                return;
            }

            MessageBox.Show("MTUM data correctly imported");

            this.Cursor = Cursors.Arrow;
        }

        private void cmdFBEdit_Click(object sender, EventArgs e)
        {
            if (lstFileBasedSymbols.SelectedItems.Count != 1)
            {
                MessageBox.Show("Please select a Symbol to edit");
                return;
            }

            var item = (FileBasedStock)lstFileBasedSymbols.SelectedItems[0].Tag;

            FileBasedStockPopup editPopup = new FileBasedStockPopup(item);
            if (editPopup.ShowDialog() == DialogResult.OK)
                refreshFileBasedStocks();
        }

        private void cmdFBAdd_Click(object sender, EventArgs e)
        {
            FileBasedStockPopup editPopup = new FileBasedStockPopup(null);
            if(editPopup.ShowDialog() == DialogResult.OK)
                refreshFileBasedStocks();
        }

        private void cmdFBDelete_Click(object sender, EventArgs e)
        {
            if(lstFileBasedSymbols.SelectedItems.Count != 1)
            {
                MessageBox.Show("Please select a Symbol to delete");
                return;
            }

            var item = (FileBasedStock)lstFileBasedSymbols.SelectedItems[0].Tag;

            if (MessageBox.Show("Are you sure you want to delete the Symbol " + item.Stock.Symbol, "Confirm Deletion", MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;

            if(!FileBasedStock.Delete(item))
            {
                MessageBox.Show("There was an error deleting the Symbol, please try again.");
            }

            refreshFileBasedStocks();
        }

        private void refreshFileBasedStocks(bool refreshCombos = true)
        {
            lstFileBasedSymbols.Items.Clear();

            fileBasedStocks = FileBasedStock.GetAllFileBasedStocks();
            foreach (var stock in fileBasedStocks)
            {
                var item = lstFileBasedSymbols.Items.Add(stock.Stock.Symbol);
                item.Tag = stock;
                item.SubItems.Add(stock.Stock.CompanyName);
                item.SubItems.Add(stock.KeepUpdated == true ? "Yes" : "No");
                if(stock.LastPriceDate == 0)
                    item.SubItems.Add("");
                else
                    item.SubItems.Add(Utils.ConvertIntToDateTime(stock.LastPriceDate).ToString("MM/dd/yyyy"));
                item.SubItems.Add(stock.Path);
            }

            if(refreshCombos)
                refreshSymbolsComboboxes();
        }

        private void refreshSymbolsComboboxes()
        {
            List<ComboBox> combos = new List<ComboBox>();
            combos.Add(cboBAAsset1);
            combos.Add(cboBAAsset2);
            combos.Add(cboBAAsset3);
            combos.Add(cboBAAsset4);
            combos.Add(cboBondsAsset1);
            combos.Add(cboBondsAsset2);
            combos.Add(cboBondsAsset3);
            combos.Add(cboBondsAsset4);
            combos.Add(cboBondsRiskAsset);
            combos.Add(cboBARiskAsset1);
            combos.Add(cboBARiskAsset2);

            var dataSourceBenchmarks = new List<IStockInfo>();

            dataSourceBenchmarks.Insert(0, new SimpleIdNameStock(-1, "SPY"));
            dataSourceBenchmarks.Insert(1, new SimpleIdNameStock(-3, "QQQ"));
            dataSourceBenchmarks.Insert(2, new SimpleIdNameStock(-4, "DIA"));
            dataSourceBenchmarks.Insert(3, new SimpleIdNameStock(-5, "XLK"));
            dataSourceBenchmarks.Insert(4, new SimpleIdNameStock(-6, "XLV"));
            dataSourceBenchmarks.Insert(5, new SimpleIdNameStock(-9, "MTUM"));
            dataSourceBenchmarks.Insert(6, new SimpleIdNameStock(-10, "Custom MTUM"));
            dataSourceBenchmarks.Insert(7, new SimpleIdNameStock(-11, "AGG"));

            var dataSource = dataSourceBenchmarks.Concat(fileBasedStocks.Cast<IStockInfo>().ToArray().Concat(etfStocks.Cast<IStockInfo>())).OrderBy(x => x.StockSymbol).ToList();
            
            foreach (var combo in combos)
            {
                var item = combo.SelectedValue;
                if (combo == cboBAAsset4 || combo.Name.Contains("Bonds"))
                {
                    var newDS = dataSource.ToList();
                    newDS.Insert(0, (IStockInfo)new FileBasedStock());
                    combo.DataSource = newDS;
                }
                else
                {
                    combo.DataSource = dataSource.ToList();
                }

                if (item != null)
                    combo.SelectedValue = item;
            }

            var dataSourceInverse = etfStocks.Cast<IStockInfo>().OrderBy(x => x.StockSymbol).ToList();
            
            dataSourceBenchmarks.AddRange(etfStocks.Cast<IStockInfo>().OrderBy(x => x.StockSymbol).ToList());

            var itemInverse = cboSymbolInverse.SelectedValue;
            var itemBenchmark = cboSymbolBenchmark.SelectedValue;

            cboSymbolInverse.ValueMember = "IdStock";
            cboSymbolInverse.DisplayMember = "StockSymbol";

            dataSourceInverse.Insert(0, new SimpleIdNameStock(-2, "SPXU"));
            dataSourceInverse.Insert(1, new SimpleIdNameStock(-7, "IEF"));
            dataSourceInverse.Insert(2, new SimpleIdNameStock(-8, "TLT"));

            cboSymbolInverse.DataSource = dataSourceInverse.ToList();

            if (itemInverse != null && dataSourceBenchmarks.Any(x => x.IdStock == (int)itemInverse))
                cboSymbolInverse.SelectedValue = itemInverse;
            else
                cboSymbolInverse.SelectedIndex = 0;

            cboSymbolBenchmark.ValueMember = "IdStock";
            cboSymbolBenchmark.DisplayMember = "StockSymbol";

            cboSymbolBenchmark.DataSource = dataSourceBenchmarks.ToList();

            if (itemBenchmark != null && dataSourceBenchmarks.Any(x => x.IdStock == (int)itemBenchmark))
                cboSymbolBenchmark.SelectedValue = itemBenchmark;
            else
                cboSymbolBenchmark.SelectedIndex = 0;
        }

        private void chkBACustomRiskFile_CheckedChanged(object sender, EventArgs e)
        {
            refreshCheckboxes();
        }

        private void cmdBACustomRiskFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (customRiskFileBlendedBenchmark.Trim() != "")
                if (File.Exists(customRiskFileBlendedBenchmark))
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(customRiskFileBlendedBenchmark);

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            customRiskFileBlendedBenchmark = openFileDialog.FileName;

            txtBACustomRiskFile.Text = Path.GetFileName(customRiskFileBlendedBenchmark);
        }

        private void refreshETFStocks(bool refreshComboboxes = true)
        {
            lstETFSymbols.Items.Clear();

            etfStocks = Stock.GetAllETFStocks();
            foreach (var stock in etfStocks)
            {
                var item = lstETFSymbols.Items.Add(stock.Symbol);
                item.Tag = stock;
                item.SubItems.Add(stock.CompanyName);
                item.SubItems.Add(stock.Exchange);

                if (stock.FirstValuesDate == 0)
                    item.SubItems.Add("");
                else
                    item.SubItems.Add(Utils.ConvertIntToDateTime(stock.FirstValuesDate).ToString("MM/dd/yyyy"));

                if (stock.LastValuesDate == 0)
                    item.SubItems.Add("");
                else
                    item.SubItems.Add(Utils.ConvertIntToDateTime(stock.LastValuesDate).ToString("MM/dd/yyyy"));
            }

            if (refreshComboboxes)
                refreshSymbolsComboboxes();
        }

        private void cmdETFAdd_Click(object sender, EventArgs e)
        {
            var editPopup = new SelectETFSymbolPopup();

            if (editPopup.ShowDialog() == DialogResult.OK)
            {
                ReprocessETFSymbols = true;
            }

            refreshETFStocks();
        }

        private void cmdETFRemove_Click(object sender, EventArgs e)
        {
            if(lstETFSymbols.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a symbol to remove.");
                return;
            }

            if(MessageBox.Show("Are you sure you want to remove " + lstETFSymbols.SelectedItems[0].Name + "?", "Remove ETF", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            Stock.DeleteETFStock((Stock)lstETFSymbols.SelectedItems[0].Tag);

            lstETFSymbols.Items.Remove(lstETFSymbols.SelectedItems[0]);

            refreshETFStocks();
        }

        private void lnkMailConfigureSmtp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ConfigureSMTPPopup config = new ConfigureSMTPPopup();
            config.ShowDialog();
        }

        private void macdCheck_CheckedChanged(object sender, EventArgs e)
        {
            refreshCheckboxes();
        }

        private void rsiCheck_CheckedChanged(object sender, EventArgs e)
        {
            refreshCheckboxes();
        }

        private void stochasticCheck_CheckedChanged(object sender, EventArgs e)
        {
            refreshCheckboxes();
        }

        private void chkOnlyInitial_CheckedChanged(object sender, EventArgs e)
        {
            refreshCheckboxes();
        }

        private void chkLong_CheckedChanged(object sender, EventArgs e)
        {
            refreshCheckboxes();
        }

        private void chkShort_CheckedChanged(object sender, EventArgs e)
        {
            refreshCheckboxes();
        }
    }
}
