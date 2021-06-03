namespace StockRanking
{
    partial class MainPanel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPanel));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdCreateStrategy = new System.Windows.Forms.Button();
            this.lstStrategies = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFrequency = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRiskModel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFeatures = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFilter = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstDesiredPortfolio = new System.Windows.Forms.ListView();
            this.colDesiredSymbol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEntryDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEntryPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDesiredName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSector2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.cmdBacktest = new System.Windows.Forms.Button();
            this.lstPortfolioParameters = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label5 = new System.Windows.Forms.Label();
            this.lstIndustries = new System.Windows.Forms.ListView();
            this.cmdViewCurrentStrategy = new System.Windows.Forms.Button();
            this.cmdSelectStrategy = new System.Windows.Forms.Button();
            this.tmrCheckUpdates = new System.Windows.Forms.Timer(this.components);
            this.lstCurrentPortfolio = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCurrShares = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSector = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmdEditPortfolio = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cmdEditPositions = new System.Windows.Forms.LinkLabel();
            this.cmdDeleteStrategy = new System.Windows.Forms.Button();
            this.chkSaveMemory = new JCS.ToggleSwitch();
            this.label6 = new System.Windows.Forms.Label();
            this.cmdCompositeGraph = new System.Windows.Forms.Button();
            this.cmdUnselectStrategy = new System.Windows.Forms.Button();
            this.pnlProcessing = new StockRanking.ProcessingPanelControl();
            this.cmdPerformance = new System.Windows.Forms.Button();
            this.cmdSendMailAlerts = new System.Windows.Forms.LinkLabel();
            this.colStyle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Default Portfolio Parameters";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 177);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Saved Strategies";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Current Portfolio";
            // 
            // cmdCreateStrategy
            // 
            this.cmdCreateStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCreateStrategy.Location = new System.Drawing.Point(760, 286);
            this.cmdCreateStrategy.Name = "cmdCreateStrategy";
            this.cmdCreateStrategy.Size = new System.Drawing.Size(106, 23);
            this.cmdCreateStrategy.TabIndex = 38;
            this.cmdCreateStrategy.Text = "New Strategy";
            this.cmdCreateStrategy.UseVisualStyleBackColor = true;
            this.cmdCreateStrategy.Click += new System.EventHandler(this.cmdCreateStrategy_Click);
            // 
            // lstStrategies
            // 
            this.lstStrategies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstStrategies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colFrequency,
            this.colRiskModel,
            this.colFeatures,
            this.colFilter});
            this.lstStrategies.FullRowSelect = true;
            this.lstStrategies.HideSelection = false;
            this.lstStrategies.Location = new System.Drawing.Point(12, 145);
            this.lstStrategies.Name = "lstStrategies";
            this.lstStrategies.Size = new System.Drawing.Size(855, 135);
            this.lstStrategies.TabIndex = 20;
            this.lstStrategies.UseCompatibleStateImageBehavior = false;
            this.lstStrategies.View = System.Windows.Forms.View.Details;
            this.lstStrategies.DoubleClick += new System.EventHandler(this.lstStrategies_DoubleClick);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 140;
            // 
            // colFrequency
            // 
            this.colFrequency.Text = "Frequency";
            this.colFrequency.Width = 80;
            // 
            // colRiskModel
            // 
            this.colRiskModel.Text = "Risk Model";
            this.colRiskModel.Width = 100;
            // 
            // colFeatures
            // 
            this.colFeatures.Text = "Features";
            this.colFeatures.Width = 250;
            // 
            // colFilter
            // 
            this.colFilter.Text = "Filters";
            this.colFilter.Width = 250;
            // 
            // lstDesiredPortfolio
            // 
            this.lstDesiredPortfolio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDesiredPortfolio.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDesiredSymbol,
            this.colEntryDate,
            this.colEntryPrice,
            this.colStyle,
            this.colDesiredName,
            this.colSector2});
            this.lstDesiredPortfolio.HideSelection = false;
            this.lstDesiredPortfolio.Location = new System.Drawing.Point(6, 20);
            this.lstDesiredPortfolio.Name = "lstDesiredPortfolio";
            this.lstDesiredPortfolio.Size = new System.Drawing.Size(358, 159);
            this.lstDesiredPortfolio.TabIndex = 49;
            this.lstDesiredPortfolio.UseCompatibleStateImageBehavior = false;
            this.lstDesiredPortfolio.View = System.Windows.Forms.View.Details;
            // 
            // colDesiredSymbol
            // 
            this.colDesiredSymbol.Text = "Ticker";
            // 
            // colEntryDate
            // 
            this.colEntryDate.Text = "Entry Date";
            this.colEntryDate.Width = 100;
            // 
            // colEntryPrice
            // 
            this.colEntryPrice.Text = "Entry Price";
            this.colEntryPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colEntryPrice.Width = 100;
            // 
            // colDesiredName
            // 
            this.colDesiredName.Text = "Name";
            this.colDesiredName.Width = 250;
            // 
            // colSector2
            // 
            this.colSector2.Text = "Sector";
            this.colSector2.Width = 100;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Expected Portfolio";
            // 
            // cmdBacktest
            // 
            this.cmdBacktest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBacktest.Location = new System.Drawing.Point(536, 286);
            this.cmdBacktest.Name = "cmdBacktest";
            this.cmdBacktest.Size = new System.Drawing.Size(106, 23);
            this.cmdBacktest.TabIndex = 30;
            this.cmdBacktest.Text = "Run Backtest";
            this.cmdBacktest.UseVisualStyleBackColor = true;
            this.cmdBacktest.Click += new System.EventHandler(this.cmdBacktest_Click);
            // 
            // lstPortfolioParameters
            // 
            this.lstPortfolioParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPortfolioParameters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstPortfolioParameters.HideSelection = false;
            this.lstPortfolioParameters.Location = new System.Drawing.Point(3, 16);
            this.lstPortfolioParameters.Name = "lstPortfolioParameters";
            this.lstPortfolioParameters.Size = new System.Drawing.Size(421, 104);
            this.lstPortfolioParameters.TabIndex = 5;
            this.lstPortfolioParameters.UseCompatibleStateImageBehavior = false;
            this.lstPortfolioParameters.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Parameter Name";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader2.Width = 180;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Sectors Allowed";
            // 
            // lstIndustries
            // 
            this.lstIndustries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstIndustries.HideSelection = false;
            this.lstIndustries.Location = new System.Drawing.Point(6, 16);
            this.lstIndustries.Name = "lstIndustries";
            this.lstIndustries.Size = new System.Drawing.Size(418, 104);
            this.lstIndustries.TabIndex = 15;
            this.lstIndustries.UseCompatibleStateImageBehavior = false;
            this.lstIndustries.View = System.Windows.Forms.View.List;
            // 
            // cmdViewCurrentStrategy
            // 
            this.cmdViewCurrentStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdViewCurrentStrategy.Location = new System.Drawing.Point(760, 470);
            this.cmdViewCurrentStrategy.Name = "cmdViewCurrentStrategy";
            this.cmdViewCurrentStrategy.Size = new System.Drawing.Size(106, 23);
            this.cmdViewCurrentStrategy.TabIndex = 70;
            this.cmdViewCurrentStrategy.Text = "Realtime View";
            this.cmdViewCurrentStrategy.UseVisualStyleBackColor = true;
            this.cmdViewCurrentStrategy.Click += new System.EventHandler(this.cmdViewCurrentStrategy_Click);
            // 
            // cmdSelectStrategy
            // 
            this.cmdSelectStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdSelectStrategy.Location = new System.Drawing.Point(12, 283);
            this.cmdSelectStrategy.Name = "cmdSelectStrategy";
            this.cmdSelectStrategy.Size = new System.Drawing.Size(106, 23);
            this.cmdSelectStrategy.TabIndex = 23;
            this.cmdSelectStrategy.Text = "Select Strategy";
            this.cmdSelectStrategy.UseVisualStyleBackColor = true;
            this.cmdSelectStrategy.Click += new System.EventHandler(this.cmdSelectStrategy_Click);
            // 
            // tmrCheckUpdates
            // 
            this.tmrCheckUpdates.Enabled = true;
            this.tmrCheckUpdates.Interval = 600000;
            this.tmrCheckUpdates.Tick += new System.EventHandler(this.tmrCheckUpdates_Tick);
            // 
            // lstCurrentPortfolio
            // 
            this.lstCurrentPortfolio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstCurrentPortfolio.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.colCurrShares,
            this.columnHeader5,
            this.columnHeader6,
            this.colSector});
            this.lstCurrentPortfolio.HideSelection = false;
            this.lstCurrentPortfolio.Location = new System.Drawing.Point(3, 20);
            this.lstCurrentPortfolio.Name = "lstCurrentPortfolio";
            this.lstCurrentPortfolio.Size = new System.Drawing.Size(360, 159);
            this.lstCurrentPortfolio.TabIndex = 46;
            this.lstCurrentPortfolio.UseCompatibleStateImageBehavior = false;
            this.lstCurrentPortfolio.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Ticker";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Entry Date";
            this.columnHeader4.Width = 100;
            // 
            // colCurrShares
            // 
            this.colCurrShares.Text = "Shares";
            this.colCurrShares.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colCurrShares.Width = 70;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Entry Price";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Name";
            this.columnHeader6.Width = 250;
            // 
            // colSector
            // 
            this.colSector.Text = "Sector";
            this.colSector.Width = 100;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(867, 129);
            this.tableLayoutPanel1.TabIndex = 26;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.lstIndustries);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(436, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(428, 123);
            this.panel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmdEditPortfolio);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lstPortfolioParameters);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(427, 123);
            this.panel1.TabIndex = 0;
            // 
            // cmdEditPortfolio
            // 
            this.cmdEditPortfolio.AutoSize = true;
            this.cmdEditPortfolio.Location = new System.Drawing.Point(179, 0);
            this.cmdEditPortfolio.Name = "cmdEditPortfolio";
            this.cmdEditPortfolio.Size = new System.Drawing.Size(24, 13);
            this.cmdEditPortfolio.TabIndex = 10;
            this.cmdEditPortfolio.TabStop = true;
            this.cmdEditPortfolio.Text = "edit";
            this.cmdEditPortfolio.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.cmdEditPortfolio_LinkClicked);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel4, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 312);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(745, 187);
            this.tableLayoutPanel2.TabIndex = 27;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.lstDesiredPortfolio);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(375, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(367, 181);
            this.panel3.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cmdEditPositions);
            this.panel4.Controls.Add(this.lstCurrentPortfolio);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(366, 181);
            this.panel4.TabIndex = 0;
            // 
            // cmdEditPositions
            // 
            this.cmdEditPositions.AutoSize = true;
            this.cmdEditPositions.Location = new System.Drawing.Point(88, 4);
            this.cmdEditPositions.Name = "cmdEditPositions";
            this.cmdEditPositions.Size = new System.Drawing.Size(24, 13);
            this.cmdEditPositions.TabIndex = 42;
            this.cmdEditPositions.TabStop = true;
            this.cmdEditPositions.Text = "edit";
            this.cmdEditPositions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.cmdEditPositions_LinkClicked);
            // 
            // cmdDeleteStrategy
            // 
            this.cmdDeleteStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDeleteStrategy.Location = new System.Drawing.Point(648, 286);
            this.cmdDeleteStrategy.Name = "cmdDeleteStrategy";
            this.cmdDeleteStrategy.Size = new System.Drawing.Size(106, 23);
            this.cmdDeleteStrategy.TabIndex = 35;
            this.cmdDeleteStrategy.Text = "Delete Strategy";
            this.cmdDeleteStrategy.UseVisualStyleBackColor = true;
            this.cmdDeleteStrategy.Click += new System.EventHandler(this.cmdDeleteStrategy_Click);
            // 
            // chkSaveMemory
            // 
            this.chkSaveMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSaveMemory.Location = new System.Drawing.Point(791, 351);
            this.chkSaveMemory.Name = "chkSaveMemory";
            this.chkSaveMemory.OffFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSaveMemory.OffText = "OFF";
            this.chkSaveMemory.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSaveMemory.OnText = "ON";
            this.chkSaveMemory.Size = new System.Drawing.Size(50, 19);
            this.chkSaveMemory.Style = JCS.ToggleSwitch.ToggleSwitchStyle.OSX;
            this.chkSaveMemory.TabIndex = 55;
            this.chkSaveMemory.CheckedChanged += new JCS.ToggleSwitch.CheckedChangedDelegate(this.chkSaveMemory_CheckedChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(764, 335);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 13);
            this.label6.TabIndex = 53;
            this.label6.Text = "Save Memory Mode";
            // 
            // cmdCompositeGraph
            // 
            this.cmdCompositeGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCompositeGraph.Location = new System.Drawing.Point(760, 441);
            this.cmdCompositeGraph.Name = "cmdCompositeGraph";
            this.cmdCompositeGraph.Size = new System.Drawing.Size(106, 23);
            this.cmdCompositeGraph.TabIndex = 60;
            this.cmdCompositeGraph.Text = "Composite Graph";
            this.cmdCompositeGraph.UseVisualStyleBackColor = true;
            this.cmdCompositeGraph.Click += new System.EventHandler(this.cmdCompositeGraph_Click);
            // 
            // cmdUnselectStrategy
            // 
            this.cmdUnselectStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdUnselectStrategy.Location = new System.Drawing.Point(124, 283);
            this.cmdUnselectStrategy.Name = "cmdUnselectStrategy";
            this.cmdUnselectStrategy.Size = new System.Drawing.Size(106, 23);
            this.cmdUnselectStrategy.TabIndex = 24;
            this.cmdUnselectStrategy.Text = "Unselect Strategy";
            this.cmdUnselectStrategy.UseVisualStyleBackColor = true;
            this.cmdUnselectStrategy.Click += new System.EventHandler(this.cmdUnselectStrategy_Click);
            // 
            // pnlProcessing
            // 
            this.pnlProcessing.Location = new System.Drawing.Point(760, 332);
            this.pnlProcessing.Margin = new System.Windows.Forms.Padding(4);
            this.pnlProcessing.Name = "pnlProcessing";
            this.pnlProcessing.Size = new System.Drawing.Size(134, 54);
            this.pnlProcessing.TabIndex = 16;
            this.pnlProcessing.Visible = false;
            // 
            // cmdPerformance
            // 
            this.cmdPerformance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPerformance.Location = new System.Drawing.Point(760, 412);
            this.cmdPerformance.Name = "cmdPerformance";
            this.cmdPerformance.Size = new System.Drawing.Size(106, 23);
            this.cmdPerformance.TabIndex = 59;
            this.cmdPerformance.Text = "Performance";
            this.cmdPerformance.UseVisualStyleBackColor = true;
            this.cmdPerformance.Click += new System.EventHandler(this.cmdPerformance_Click);
            // 
            // cmdSendMailAlerts
            // 
            this.cmdSendMailAlerts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSendMailAlerts.Location = new System.Drawing.Point(759, 382);
            this.cmdSendMailAlerts.Name = "cmdSendMailAlerts";
            this.cmdSendMailAlerts.Size = new System.Drawing.Size(105, 14);
            this.cmdSendMailAlerts.TabIndex = 71;
            this.cmdSendMailAlerts.TabStop = true;
            this.cmdSendMailAlerts.Text = "Send Mail Alerts";
            this.cmdSendMailAlerts.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cmdSendMailAlerts.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.cmdSendMailAlerts_LinkClicked);
            // 
            // colStyle
            // 
            this.colStyle.Text = "Sell Style";
            this.colStyle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colStyle.Width = 70;
            // 
            // MainPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 511);
            this.Controls.Add(this.pnlProcessing);
            this.Controls.Add(this.cmdDeleteStrategy);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.cmdBacktest);
            this.Controls.Add(this.lstStrategies);
            this.Controls.Add(this.cmdCreateStrategy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdSelectStrategy);
            this.Controls.Add(this.cmdViewCurrentStrategy);
            this.Controls.Add(this.chkSaveMemory);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmdCompositeGraph);
            this.Controls.Add(this.cmdUnselectStrategy);
            this.Controls.Add(this.cmdPerformance);
            this.Controls.Add(this.cmdSendMailAlerts);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 548);
            this.Name = "MainPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Factor Analysis";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainPanel_FormClosing);
            this.Shown += new System.EventHandler(this.MainPanel_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cmdCreateStrategy;
        private System.Windows.Forms.ListView lstStrategies;
        private System.Windows.Forms.ListView lstDesiredPortfolio;
        private System.Windows.Forms.Label label4;
        private ProcessingPanelControl pnlProcessing;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colFeatures;
        private System.Windows.Forms.ColumnHeader colFilter;
        private System.Windows.Forms.Button cmdBacktest;
        private System.Windows.Forms.ListView lstPortfolioParameters;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListView lstIndustries;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button cmdViewCurrentStrategy;
        private System.Windows.Forms.Button cmdSelectStrategy;
        private System.Windows.Forms.Timer tmrCheckUpdates;
        private System.Windows.Forms.ColumnHeader colDesiredSymbol;
        private System.Windows.Forms.ColumnHeader colDesiredName;
        private System.Windows.Forms.ColumnHeader colEntryDate;
        private System.Windows.Forms.ColumnHeader colEntryPrice;
        private System.Windows.Forms.ListView lstCurrentPortfolio;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.LinkLabel cmdEditPositions;
        private System.Windows.Forms.LinkLabel cmdEditPortfolio;
        private System.Windows.Forms.Button cmdDeleteStrategy;
        private System.Windows.Forms.ColumnHeader colCurrShares;
        private JCS.ToggleSwitch chkSaveMemory;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ColumnHeader colFrequency;
        private System.Windows.Forms.ColumnHeader colRiskModel;
        private System.Windows.Forms.ColumnHeader colSector2;
        private System.Windows.Forms.ColumnHeader colSector;
        private System.Windows.Forms.Button cmdCompositeGraph;
        private System.Windows.Forms.Button cmdUnselectStrategy;
        private System.Windows.Forms.Button cmdPerformance;
        private System.Windows.Forms.LinkLabel cmdSendMailAlerts;
        private System.Windows.Forms.ColumnHeader colStyle;
    }
}