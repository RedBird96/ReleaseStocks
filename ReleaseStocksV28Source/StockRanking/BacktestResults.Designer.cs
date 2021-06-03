namespace StockRanking
{
    partial class BacktestResults
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BacktestResults));
            this.grdTrades = new System.Windows.Forms.DataGridView();
            this.colEntryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSymbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSector = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SellingStyle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEntryAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEntryPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExitDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExitAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProf2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCommission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNetProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStopLossSell = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStopLossBuy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPanicToCash = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.chartEquity = new LiveCharts.WinForms.CartesianChart();
            this.chartDrawdown = new LiveCharts.WinForms.CartesianChart();
            this.cmdExport = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.tblPanel = new System.Windows.Forms.TableLayoutPanel();
            this.dateFrom = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdRefresh = new System.Windows.Forms.Button();
            this.cmdPerformanceTable = new System.Windows.Forms.LinkLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.lstSelectedLines = new System.Windows.Forms.CheckedListBox();
            this.cmdPerformanceGraph = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkShowPercent = new JCS.ToggleSwitch();
            this.cmdPerformanceReport = new System.Windows.Forms.LinkLabel();
            this.linkLabelShowHistory = new System.Windows.Forms.LinkLabel();
            this.dateTimeFilter = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.toggleFilter = new JCS.ToggleSwitch();
            this.dataGridHistory = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlProcessingProgress = new StockRanking.ProcessingPanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.grdTrades)).BeginInit();
            this.tblPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // grdTrades
            // 
            this.grdTrades.AllowUserToAddRows = false;
            this.grdTrades.AllowUserToDeleteRows = false;
            this.grdTrades.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdTrades.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.grdTrades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdTrades.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEntryDate,
            this.colSymbol,
            this.colSector,
            this.SellingStyle,
            this.colEntryAmount,
            this.colEntryPrice,
            this.colExitDate,
            this.colExitAmount,
            this.colExitPrice,
            this.colProf2,
            this.colCommission,
            this.colNetProfit,
            this.colStopLossSell,
            this.colStopLossBuy,
            this.colPanicToCash});
            this.grdTrades.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdTrades.Location = new System.Drawing.Point(13, 340);
            this.grdTrades.Name = "grdTrades";
            this.grdTrades.ReadOnly = true;
            this.grdTrades.Size = new System.Drawing.Size(924, 142);
            this.grdTrades.TabIndex = 0;
            // 
            // colEntryDate
            // 
            this.colEntryDate.DataPropertyName = "EntryDateDT";
            dataGridViewCellStyle1.Format = "MM/dd/yyyy";
            this.colEntryDate.DefaultCellStyle = dataGridViewCellStyle1;
            this.colEntryDate.Frozen = true;
            this.colEntryDate.HeaderText = "Entry Date";
            this.colEntryDate.Name = "colEntryDate";
            this.colEntryDate.ReadOnly = true;
            // 
            // colSymbol
            // 
            this.colSymbol.DataPropertyName = "Symbol";
            this.colSymbol.Frozen = true;
            this.colSymbol.HeaderText = "Symbol";
            this.colSymbol.Name = "colSymbol";
            this.colSymbol.ReadOnly = true;
            // 
            // colSector
            // 
            this.colSector.DataPropertyName = "AuxSector";
            this.colSector.Frozen = true;
            this.colSector.HeaderText = "Sector";
            this.colSector.Name = "colSector";
            this.colSector.ReadOnly = true;
            // 
            // SellingStyle
            // 
            this.SellingStyle.DataPropertyName = "SellType";
            this.SellingStyle.Frozen = true;
            this.SellingStyle.HeaderText = "Direction";
            this.SellingStyle.Name = "SellingStyle";
            this.SellingStyle.ReadOnly = true;
            // 
            // colEntryAmount
            // 
            this.colEntryAmount.DataPropertyName = "Shares";
            dataGridViewCellStyle2.Format = "n0";
            this.colEntryAmount.DefaultCellStyle = dataGridViewCellStyle2;
            this.colEntryAmount.HeaderText = "Entry Amount";
            this.colEntryAmount.Name = "colEntryAmount";
            this.colEntryAmount.ReadOnly = true;
            // 
            // colEntryPrice
            // 
            this.colEntryPrice.DataPropertyName = "EntryPrice";
            dataGridViewCellStyle3.Format = "n2";
            this.colEntryPrice.DefaultCellStyle = dataGridViewCellStyle3;
            this.colEntryPrice.HeaderText = "Entry Price";
            this.colEntryPrice.Name = "colEntryPrice";
            this.colEntryPrice.ReadOnly = true;
            // 
            // colExitDate
            // 
            this.colExitDate.DataPropertyName = "ExitDateDT";
            dataGridViewCellStyle4.Format = "MM/dd/yyyy";
            this.colExitDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.colExitDate.HeaderText = "Exit Date";
            this.colExitDate.Name = "colExitDate";
            this.colExitDate.ReadOnly = true;
            // 
            // colExitAmount
            // 
            this.colExitAmount.DataPropertyName = "Shares";
            dataGridViewCellStyle5.Format = "n0";
            this.colExitAmount.DefaultCellStyle = dataGridViewCellStyle5;
            this.colExitAmount.HeaderText = "Exit Amount";
            this.colExitAmount.Name = "colExitAmount";
            this.colExitAmount.ReadOnly = true;
            // 
            // colExitPrice
            // 
            this.colExitPrice.DataPropertyName = "ExitPrice";
            dataGridViewCellStyle6.Format = "n2";
            this.colExitPrice.DefaultCellStyle = dataGridViewCellStyle6;
            this.colExitPrice.HeaderText = "Exit Price";
            this.colExitPrice.Name = "colExitPrice";
            this.colExitPrice.ReadOnly = true;
            // 
            // colProf2
            // 
            this.colProf2.DataPropertyName = "Profit";
            dataGridViewCellStyle7.Format = "n2";
            this.colProf2.DefaultCellStyle = dataGridViewCellStyle7;
            this.colProf2.HeaderText = "Profit";
            this.colProf2.Name = "colProf2";
            this.colProf2.ReadOnly = true;
            // 
            // colCommission
            // 
            this.colCommission.DataPropertyName = "Commission";
            dataGridViewCellStyle8.Format = "n2";
            this.colCommission.DefaultCellStyle = dataGridViewCellStyle8;
            this.colCommission.HeaderText = "Commission";
            this.colCommission.Name = "colCommission";
            this.colCommission.ReadOnly = true;
            // 
            // colNetProfit
            // 
            this.colNetProfit.DataPropertyName = "NetProfit";
            dataGridViewCellStyle9.Format = "n2";
            this.colNetProfit.DefaultCellStyle = dataGridViewCellStyle9;
            this.colNetProfit.HeaderText = "Net Profit";
            this.colNetProfit.Name = "colNetProfit";
            this.colNetProfit.ReadOnly = true;
            // 
            // colStopLossSell
            // 
            this.colStopLossSell.DataPropertyName = "StopLossSell";
            this.colStopLossSell.HeaderText = "Stop Loss Sell";
            this.colStopLossSell.Name = "colStopLossSell";
            this.colStopLossSell.ReadOnly = true;
            // 
            // colStopLossBuy
            // 
            this.colStopLossBuy.DataPropertyName = "BuyAfterStopLoss";
            this.colStopLossBuy.HeaderText = "Stop Loss Buy";
            this.colStopLossBuy.Name = "colStopLossBuy";
            this.colStopLossBuy.ReadOnly = true;
            // 
            // colPanicToCash
            // 
            this.colPanicToCash.DataPropertyName = "RiskModelName";
            this.colPanicToCash.HeaderText = "Risk Model";
            this.colPanicToCash.Name = "colPanicToCash";
            this.colPanicToCash.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 319);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Transactions:";
            // 
            // chartEquity
            // 
            this.chartEquity.BackColor = System.Drawing.Color.White;
            this.chartEquity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartEquity.Location = new System.Drawing.Point(0, 0);
            this.chartEquity.Margin = new System.Windows.Forms.Padding(0);
            this.chartEquity.Name = "chartEquity";
            this.chartEquity.Size = new System.Drawing.Size(921, 207);
            this.chartEquity.TabIndex = 40;
            this.chartEquity.Text = "Cumulative Sum";
            // 
            // chartDrawdown
            // 
            this.chartDrawdown.BackColor = System.Drawing.Color.White;
            this.chartDrawdown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartDrawdown.Location = new System.Drawing.Point(0, 207);
            this.chartDrawdown.Margin = new System.Windows.Forms.Padding(0);
            this.chartDrawdown.Name = "chartDrawdown";
            this.chartDrawdown.Size = new System.Drawing.Size(921, 89);
            this.chartDrawdown.TabIndex = 41;
            this.chartDrawdown.Text = "Draw Down";
            // 
            // cmdExport
            // 
            this.cmdExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdExport.Location = new System.Drawing.Point(774, 529);
            this.cmdExport.Name = "cmdExport";
            this.cmdExport.Size = new System.Drawing.Size(78, 23);
            this.cmdExport.TabIndex = 2;
            this.cmdExport.Text = "Export";
            this.cmdExport.UseVisualStyleBackColor = true;
            this.cmdExport.Click += new System.EventHandler(this.cmdExport_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.Location = new System.Drawing.Point(858, 529);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(78, 23);
            this.cmdClose.TabIndex = 4;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // tblPanel
            // 
            this.tblPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblPanel.ColumnCount = 1;
            this.tblPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblPanel.Controls.Add(this.chartDrawdown, 0, 1);
            this.tblPanel.Controls.Add(this.chartEquity, 0, 0);
            this.tblPanel.Location = new System.Drawing.Point(12, 12);
            this.tblPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tblPanel.Name = "tblPanel";
            this.tblPanel.RowCount = 2;
            this.tblPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tblPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tblPanel.Size = new System.Drawing.Size(921, 296);
            this.tblPanel.TabIndex = 42;
            // 
            // dateFrom
            // 
            this.dateFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dateFrom.CustomFormat = "MM-dd-yyyy";
            this.dateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateFrom.Location = new System.Drawing.Point(124, 487);
            this.dateFrom.MinDate = new System.DateTime(2005, 1, 1, 16, 25, 0, 0);
            this.dateFrom.Name = "dateFrom";
            this.dateFrom.Size = new System.Drawing.Size(107, 20);
            this.dateFrom.TabIndex = 44;
            this.dateFrom.Value = new System.DateTime(2005, 1, 1, 16, 25, 0, 0);
            this.dateFrom.ValueChanged += new System.EventHandler(this.dateFrom_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 490);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 43;
            this.label2.Text = "Back Test Start Date:";
            // 
            // cmdRefresh
            // 
            this.cmdRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdRefresh.Location = new System.Drawing.Point(236, 485);
            this.cmdRefresh.Name = "cmdRefresh";
            this.cmdRefresh.Size = new System.Drawing.Size(78, 23);
            this.cmdRefresh.TabIndex = 50;
            this.cmdRefresh.Text = "Refresh";
            this.cmdRefresh.UseVisualStyleBackColor = true;
            this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
            // 
            // cmdPerformanceTable
            // 
            this.cmdPerformanceTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdPerformanceTable.AutoSize = true;
            this.cmdPerformanceTable.Location = new System.Drawing.Point(122, 507);
            this.cmdPerformanceTable.Name = "cmdPerformanceTable";
            this.cmdPerformanceTable.Size = new System.Drawing.Size(64, 13);
            this.cmdPerformanceTable.TabIndex = 53;
            this.cmdPerformanceTable.TabStop = true;
            this.cmdPerformanceTable.Text = "Show Table";
            this.cmdPerformanceTable.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.cmdPerformanceTable_LinkClicked);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(332, 490);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 56;
            this.label5.Text = "Data to Plot:";
            // 
            // lstSelectedLines
            // 
            this.lstSelectedLines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lstSelectedLines.FormattingEnabled = true;
            this.lstSelectedLines.Items.AddRange(new object[] {
            "Show Equity",
            "Show Alpha",
            "Blended Adv",
            "Blended Basic",
            "Bonds Model",
            "Show Buy & Hold"});
            this.lstSelectedLines.Location = new System.Drawing.Point(402, 487);
            this.lstSelectedLines.Margin = new System.Windows.Forms.Padding(2);
            this.lstSelectedLines.MultiColumn = true;
            this.lstSelectedLines.Name = "lstSelectedLines";
            this.lstSelectedLines.Size = new System.Drawing.Size(377, 19);
            this.lstSelectedLines.TabIndex = 65;
            this.lstSelectedLines.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstSelectedLines_ItemCheck);
            // 
            // cmdPerformanceGraph
            // 
            this.cmdPerformanceGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdPerformanceGraph.AutoSize = true;
            this.cmdPerformanceGraph.Location = new System.Drawing.Point(122, 524);
            this.cmdPerformanceGraph.Name = "cmdPerformanceGraph";
            this.cmdPerformanceGraph.Size = new System.Drawing.Size(66, 13);
            this.cmdPerformanceGraph.TabIndex = 66;
            this.cmdPerformanceGraph.TabStop = true;
            this.cmdPerformanceGraph.Text = "Show Graph";
            this.cmdPerformanceGraph.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.cmdPerformanceGraph_LinkClicked);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 524);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 67;
            this.label3.Text = "Performance:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(332, 529);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 68;
            this.label4.Text = "Show Y Axis Percent";
            // 
            // chkShowPercent
            // 
            this.chkShowPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowPercent.Checked = true;
            this.chkShowPercent.Location = new System.Drawing.Point(441, 526);
            this.chkShowPercent.Name = "chkShowPercent";
            this.chkShowPercent.OffFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowPercent.OffText = "OFF";
            this.chkShowPercent.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowPercent.OnText = "ON";
            this.chkShowPercent.Size = new System.Drawing.Size(50, 19);
            this.chkShowPercent.Style = JCS.ToggleSwitch.ToggleSwitchStyle.OSX;
            this.chkShowPercent.TabIndex = 69;
            this.chkShowPercent.CheckedChanged += new JCS.ToggleSwitch.CheckedChangedDelegate(this.chkShowPercent_CheckedChanged);
            // 
            // cmdPerformanceReport
            // 
            this.cmdPerformanceReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdPerformanceReport.AutoSize = true;
            this.cmdPerformanceReport.Location = new System.Drawing.Point(122, 541);
            this.cmdPerformanceReport.Name = "cmdPerformanceReport";
            this.cmdPerformanceReport.Size = new System.Drawing.Size(69, 13);
            this.cmdPerformanceReport.TabIndex = 70;
            this.cmdPerformanceReport.TabStop = true;
            this.cmdPerformanceReport.Text = "Show Report";
            this.cmdPerformanceReport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.cmdPerformanceReport_LinkClicked);
            // 
            // linkLabelShowHistory
            // 
            this.linkLabelShowHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabelShowHistory.AutoSize = true;
            this.linkLabelShowHistory.Location = new System.Drawing.Point(88, 319);
            this.linkLabelShowHistory.Name = "linkLabelShowHistory";
            this.linkLabelShowHistory.Size = new System.Drawing.Size(69, 13);
            this.linkLabelShowHistory.TabIndex = 71;
            this.linkLabelShowHistory.TabStop = true;
            this.linkLabelShowHistory.Text = "Show History";
            this.linkLabelShowHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelShowHistory_LinkClicked);
            // 
            // dateTimeFilter
            // 
            this.dateTimeFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimeFilter.CustomFormat = "MM-dd-yyyy";
            this.dateTimeFilter.Enabled = false;
            this.dateTimeFilter.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimeFilter.Location = new System.Drawing.Point(826, 314);
            this.dateTimeFilter.MinDate = new System.DateTime(2005, 1, 1, 16, 25, 0, 0);
            this.dateTimeFilter.Name = "dateTimeFilter";
            this.dateTimeFilter.Size = new System.Drawing.Size(107, 20);
            this.dateTimeFilter.TabIndex = 73;
            this.dateTimeFilter.Value = new System.DateTime(2005, 1, 1, 16, 25, 0, 0);
            this.dateTimeFilter.ValueChanged += new System.EventHandler(this.dateTimeFilter_ValueChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(693, 317);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 72;
            this.label6.Text = "Filter Date:";
            // 
            // toggleFilter
            // 
            this.toggleFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.toggleFilter.Location = new System.Drawing.Point(760, 314);
            this.toggleFilter.Name = "toggleFilter";
            this.toggleFilter.OffFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toggleFilter.OffText = "OFF";
            this.toggleFilter.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toggleFilter.OnText = "ON";
            this.toggleFilter.Size = new System.Drawing.Size(50, 19);
            this.toggleFilter.Style = JCS.ToggleSwitch.ToggleSwitchStyle.OSX;
            this.toggleFilter.TabIndex = 74;
            this.toggleFilter.CheckedChanged += new JCS.ToggleSwitch.CheckedChangedDelegate(this.toggleFilter_CheckedChanged);
            // 
            // dataGridHistory
            // 
            this.dataGridHistory.AllowUserToAddRows = false;
            this.dataGridHistory.AllowUserToDeleteRows = false;
            this.dataGridHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridHistory.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn5,
            this.colPrice,
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12});
            this.dataGridHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridHistory.Location = new System.Drawing.Point(13, 340);
            this.dataGridHistory.Name = "dataGridHistory";
            this.dataGridHistory.ReadOnly = true;
            this.dataGridHistory.Size = new System.Drawing.Size(924, 142);
            this.dataGridHistory.TabIndex = 75;
            this.dataGridHistory.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "DateDT";
            dataGridViewCellStyle10.Format = "MM/dd/yyyy";
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "Date";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Symbol";
            this.dataGridViewTextBoxColumn2.Frozen = true;
            this.dataGridViewTextBoxColumn2.HeaderText = "Symbol";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "SellType";
            this.dataGridViewTextBoxColumn4.Frozen = true;
            this.dataGridViewTextBoxColumn4.HeaderText = "Direction";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Action";
            this.dataGridViewTextBoxColumn6.Frozen = true;
            this.dataGridViewTextBoxColumn6.HeaderText = "Action";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Shares";
            dataGridViewCellStyle11.Format = "n0";
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewTextBoxColumn5.Frozen = true;
            this.dataGridViewTextBoxColumn5.HeaderText = "Amount";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // colPrice
            // 
            this.colPrice.DataPropertyName = "Price";
            dataGridViewCellStyle12.Format = "n2";
            dataGridViewCellStyle12.NullValue = "0";
            this.colPrice.DefaultCellStyle = dataGridViewCellStyle12;
            this.colPrice.HeaderText = "Price";
            this.colPrice.Name = "colPrice";
            this.colPrice.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "Profit";
            dataGridViewCellStyle13.Format = "n2";
            this.dataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridViewTextBoxColumn10.HeaderText = "Profit";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "Commission";
            dataGridViewCellStyle14.Format = "n2";
            this.dataGridViewTextBoxColumn11.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridViewTextBoxColumn11.HeaderText = "Commission";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "NetProfit";
            dataGridViewCellStyle15.Format = "n2";
            this.dataGridViewTextBoxColumn12.DefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridViewTextBoxColumn12.HeaderText = "Net Profit";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // pnlProcessingProgress
            // 
            this.pnlProcessingProgress.Location = new System.Drawing.Point(364, 201);
            this.pnlProcessingProgress.Margin = new System.Windows.Forms.Padding(4);
            this.pnlProcessingProgress.Name = "pnlProcessingProgress";
            this.pnlProcessingProgress.Size = new System.Drawing.Size(222, 161);
            this.pnlProcessingProgress.TabIndex = 49;
            this.pnlProcessingProgress.Visible = false;
            // 
            // BacktestResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 557);
            this.Controls.Add(this.pnlProcessingProgress);
            this.Controls.Add(this.tblPanel);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdExport);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grdTrades);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmdPerformanceGraph);
            this.Controls.Add(this.lstSelectedLines);
            this.Controls.Add(this.dateFrom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmdRefresh);
            this.Controls.Add(this.cmdPerformanceTable);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkShowPercent);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmdPerformanceReport);
            this.Controls.Add(this.dataGridHistory);
            this.Controls.Add(this.toggleFilter);
            this.Controls.Add(this.dateTimeFilter);
            this.Controls.Add(this.linkLabelShowHistory);
            this.Controls.Add(this.label6);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(754, 576);
            this.Name = "BacktestResults";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StockRanking BT Results";
            this.Shown += new System.EventHandler(this.BacktestResults_Shown);
            this.Resize += new System.EventHandler(this.BacktestResults_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.grdTrades)).EndInit();
            this.tblPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridHistory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdTrades;
        private System.Windows.Forms.Label label1;
        private LiveCharts.WinForms.CartesianChart chartEquity;
        private LiveCharts.WinForms.CartesianChart chartDrawdown;
        private System.Windows.Forms.Button cmdExport;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.TableLayoutPanel tblPanel;
        private System.Windows.Forms.DateTimePicker dateFrom;
        private System.Windows.Forms.Label label2;
        private ProcessingPanelControl pnlProcessingProgress;
        private System.Windows.Forms.Button cmdRefresh;
        private System.Windows.Forms.LinkLabel cmdPerformanceTable;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckedListBox lstSelectedLines;
        private System.Windows.Forms.LinkLabel cmdPerformanceGraph;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private JCS.ToggleSwitch chkShowPercent;
        private System.Windows.Forms.LinkLabel cmdPerformanceReport;
        private System.Windows.Forms.LinkLabel linkLabelShowHistory;
        private System.Windows.Forms.DateTimePicker dateTimeFilter;
        private System.Windows.Forms.Label label6;
        private JCS.ToggleSwitch toggleFilter;
        private System.Windows.Forms.DataGridView dataGridHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEntryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSymbol;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSector;
        private System.Windows.Forms.DataGridViewTextBoxColumn SellingStyle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEntryAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEntryPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExitDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExitAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProf2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCommission;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNetProfit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStopLossSell;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStopLossBuy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPanicToCash;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
    }
}