namespace StockRanking
{
    partial class StrategyView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StrategyView));
            this.cmdProcessFeatures = new System.Windows.Forms.Button();
            this.grdFeatures = new System.Windows.Forms.DataGridView();
            this.colFeatureCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFeatureName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.cmdExport = new System.Windows.Forms.Button();
            this.cmdSwitchView = new System.Windows.Forms.LinkLabel();
            this.cmdResetFeatures = new System.Windows.Forms.LinkLabel();
            this.cmdScatterPlot = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtStrategyName = new System.Windows.Forms.TextBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdBacktest = new System.Windows.Forms.Button();
            this.cmdExportDebug = new System.Windows.Forms.Button();
            this.cmdPortfolioParameters = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.grdStocks = new System.Windows.Forms.DataGridView();
            this.colStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCompanyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSector = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosingPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYtd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlProcessingProgress = new StockRanking.ProcessingPanelControl();
            this.btnOptimize = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdFeatures)).BeginInit();
            this.pnlFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdStocks)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdProcessFeatures
            // 
            this.cmdProcessFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdProcessFeatures.Location = new System.Drawing.Point(788, 323);
            this.cmdProcessFeatures.Name = "cmdProcessFeatures";
            this.cmdProcessFeatures.Size = new System.Drawing.Size(82, 23);
            this.cmdProcessFeatures.TabIndex = 26;
            this.cmdProcessFeatures.Text = "Rank Stocks";
            this.cmdProcessFeatures.UseVisualStyleBackColor = true;
            this.cmdProcessFeatures.Click += new System.EventHandler(this.cmdProcessFeatures_Click);
            // 
            // grdFeatures
            // 
            this.grdFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdFeatures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFeatures.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFeatureCheck,
            this.colFeatureName,
            this.colWeight});
            this.grdFeatures.Location = new System.Drawing.Point(15, 54);
            this.grdFeatures.Name = "grdFeatures";
            this.grdFeatures.RowHeadersVisible = false;
            this.grdFeatures.Size = new System.Drawing.Size(430, 291);
            this.grdFeatures.TabIndex = 10;
            this.grdFeatures.TabStop = false;
            this.grdFeatures.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdFeatures_CellClick);
            this.grdFeatures.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdFeatures_CellContentClick);
            this.grdFeatures.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.grdFeatures_CellValidating);
            // 
            // colFeatureCheck
            // 
            this.colFeatureCheck.DataPropertyName = "IsEnabled";
            this.colFeatureCheck.HeaderText = "Enabled";
            this.colFeatureCheck.MinimumWidth = 50;
            this.colFeatureCheck.Name = "colFeatureCheck";
            this.colFeatureCheck.Width = 60;
            // 
            // colFeatureName
            // 
            this.colFeatureName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFeatureName.DataPropertyName = "Name";
            this.colFeatureName.HeaderText = "Feature";
            this.colFeatureName.Name = "colFeatureName";
            this.colFeatureName.ReadOnly = true;
            // 
            // colWeight
            // 
            this.colWeight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colWeight.DataPropertyName = "Weight";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle1.Format = "n2";
            this.colWeight.DefaultCellStyle = dataGridViewCellStyle1;
            this.colWeight.HeaderText = "Weight";
            this.colWeight.MinimumWidth = 70;
            this.colWeight.Name = "colWeight";
            this.colWeight.Width = 70;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Available Features";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(451, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Results Filters";
            // 
            // pnlFilters
            // 
            this.pnlFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFilters.AutoScroll = true;
            this.pnlFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilters.Controls.Add(this.pnlProcessingProgress);
            this.pnlFilters.Location = new System.Drawing.Point(451, 25);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Size = new System.Drawing.Size(700, 291);
            this.pnlFilters.TabIndex = 15;
            // 
            // cmdExport
            // 
            this.cmdExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdExport.Location = new System.Drawing.Point(7, 226);
            this.cmdExport.Name = "cmdExport";
            this.cmdExport.Size = new System.Drawing.Size(77, 23);
            this.cmdExport.TabIndex = 66;
            this.cmdExport.Text = "Export";
            this.cmdExport.UseVisualStyleBackColor = true;
            this.cmdExport.Click += new System.EventHandler(this.cmdExport_Click);
            // 
            // cmdSwitchView
            // 
            this.cmdSwitchView.Location = new System.Drawing.Point(92, 3);
            this.cmdSwitchView.Name = "cmdSwitchView";
            this.cmdSwitchView.Size = new System.Drawing.Size(172, 13);
            this.cmdSwitchView.TabIndex = 55;
            this.cmdSwitchView.TabStop = true;
            this.cmdSwitchView.Text = "Switch to Full View";
            this.cmdSwitchView.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.cmdSwitchView.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.cmdSwitchView_LinkClicked);
            // 
            // cmdResetFeatures
            // 
            this.cmdResetFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdResetFeatures.Location = new System.Drawing.Point(325, 29);
            this.cmdResetFeatures.Name = "cmdResetFeatures";
            this.cmdResetFeatures.Size = new System.Drawing.Size(120, 22);
            this.cmdResetFeatures.TabIndex = 8;
            this.cmdResetFeatures.TabStop = true;
            this.cmdResetFeatures.Text = "Reset Weights";
            this.cmdResetFeatures.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.cmdResetFeatures.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.cmdResetFeatures_LinkClicked);
            // 
            // cmdScatterPlot
            // 
            this.cmdScatterPlot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdScatterPlot.Location = new System.Drawing.Point(964, 323);
            this.cmdScatterPlot.Name = "cmdScatterPlot";
            this.cmdScatterPlot.Size = new System.Drawing.Size(82, 23);
            this.cmdScatterPlot.TabIndex = 49;
            this.cmdScatterPlot.Text = "Scatter Plot";
            this.cmdScatterPlot.UseVisualStyleBackColor = true;
            this.cmdScatterPlot.Click += new System.EventHandler(this.cmdScatterPlot_Click);
            // 
            // cmdSave
            // 
            this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSave.Location = new System.Drawing.Point(998, 226);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(61, 23);
            this.cmdSave.TabIndex = 74;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 52;
            this.label5.Text = "Strategy Name:";
            // 
            // txtStrategyName
            // 
            this.txtStrategyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStrategyName.Location = new System.Drawing.Point(98, 6);
            this.txtStrategyName.Name = "txtStrategyName";
            this.txtStrategyName.Size = new System.Drawing.Size(347, 20);
            this.txtStrategyName.TabIndex = 5;
            this.txtStrategyName.Text = "New Strategy";
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Location = new System.Drawing.Point(1065, 226);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(61, 23);
            this.cmdCancel.TabIndex = 77;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdBacktest
            // 
            this.cmdBacktest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBacktest.Location = new System.Drawing.Point(876, 323);
            this.cmdBacktest.Name = "cmdBacktest";
            this.cmdBacktest.Size = new System.Drawing.Size(82, 23);
            this.cmdBacktest.TabIndex = 38;
            this.cmdBacktest.Text = "Backtest";
            this.cmdBacktest.UseVisualStyleBackColor = true;
            this.cmdBacktest.Click += new System.EventHandler(this.cmdBacktest_Click);
            // 
            // cmdExportDebug
            // 
            this.cmdExportDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdExportDebug.Location = new System.Drawing.Point(90, 226);
            this.cmdExportDebug.Name = "cmdExportDebug";
            this.cmdExportDebug.Size = new System.Drawing.Size(121, 23);
            this.cmdExportDebug.TabIndex = 69;
            this.cmdExportDebug.Text = "Export Debug Data";
            this.cmdExportDebug.UseVisualStyleBackColor = true;
            this.cmdExportDebug.Click += new System.EventHandler(this.cmdExportDebug_Click);
            // 
            // cmdPortfolioParameters
            // 
            this.cmdPortfolioParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPortfolioParameters.Location = new System.Drawing.Point(451, 322);
            this.cmdPortfolioParameters.Name = "cmdPortfolioParameters";
            this.cmdPortfolioParameters.Size = new System.Drawing.Size(169, 23);
            this.cmdPortfolioParameters.TabIndex = 78;
            this.cmdPortfolioParameters.Text = "Portfolio Parameters";
            this.cmdPortfolioParameters.UseVisualStyleBackColor = true;
            this.cmdPortfolioParameters.Click += new System.EventHandler(this.cmdPortfolioParameters_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.Location = new System.Drawing.Point(1014, 3);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(120, 16);
            this.linkLabel1.TabIndex = 79;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Add Custom Filter";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // grdStocks
            // 
            this.grdStocks.AllowUserToAddRows = false;
            this.grdStocks.AllowUserToDeleteRows = false;
            this.grdStocks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdStocks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdStocks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStock,
            this.colCompanyName,
            this.colSector,
            this.colClosingPrice,
            this.colYtd,
            this.colVolume});
            this.grdStocks.Enabled = false;
            this.grdStocks.Location = new System.Drawing.Point(6, 21);
            this.grdStocks.Name = "grdStocks";
            this.grdStocks.ReadOnly = true;
            this.grdStocks.RowHeadersVisible = false;
            this.grdStocks.Size = new System.Drawing.Size(1119, 199);
            this.grdStocks.TabIndex = 58;
            this.grdStocks.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdStocks_CellDoubleClick);
            // 
            // colStock
            // 
            this.colStock.HeaderText = "Symbol";
            this.colStock.MinimumWidth = 50;
            this.colStock.Name = "colStock";
            this.colStock.ReadOnly = true;
            this.colStock.Width = 70;
            // 
            // colCompanyName
            // 
            this.colCompanyName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCompanyName.HeaderText = "Name";
            this.colCompanyName.MinimumWidth = 150;
            this.colCompanyName.Name = "colCompanyName";
            this.colCompanyName.ReadOnly = true;
            // 
            // colSector
            // 
            this.colSector.HeaderText = "Sector";
            this.colSector.Name = "colSector";
            this.colSector.ReadOnly = true;
            // 
            // colClosingPrice
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "n2";
            this.colClosingPrice.DefaultCellStyle = dataGridViewCellStyle2;
            this.colClosingPrice.HeaderText = "Closing Price";
            this.colClosingPrice.MinimumWidth = 50;
            this.colClosingPrice.Name = "colClosingPrice";
            this.colClosingPrice.ReadOnly = true;
            this.colClosingPrice.Width = 90;
            // 
            // colYtd
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "n4";
            this.colYtd.DefaultCellStyle = dataGridViewCellStyle3;
            this.colYtd.HeaderText = "YTD %";
            this.colYtd.MinimumWidth = 50;
            this.colYtd.Name = "colYtd";
            this.colYtd.ReadOnly = true;
            this.colYtd.Width = 90;
            // 
            // colVolume
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colVolume.DefaultCellStyle = dataGridViewCellStyle4;
            this.colVolume.HeaderText = "Volume";
            this.colVolume.MinimumWidth = 50;
            this.colVolume.Name = "colVolume";
            this.colVolume.ReadOnly = true;
            this.colVolume.Width = 90;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Process Results";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 352);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1136, 258);
            this.tableLayoutPanel1.TabIndex = 80;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.grdStocks);
            this.panel1.Controls.Add(this.cmdSwitchView);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cmdSave);
            this.panel1.Controls.Add(this.cmdExportDebug);
            this.panel1.Controls.Add(this.cmdExport);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1130, 252);
            this.panel1.TabIndex = 0;
            // 
            // pnlProcessingProgress
            // 
            this.pnlProcessingProgress.Location = new System.Drawing.Point(18, 51);
            this.pnlProcessingProgress.Margin = new System.Windows.Forms.Padding(4);
            this.pnlProcessingProgress.Name = "pnlProcessingProgress";
            this.pnlProcessingProgress.Size = new System.Drawing.Size(302, 205);
            this.pnlProcessingProgress.TabIndex = 48;
            this.pnlProcessingProgress.Visible = false;
            // 
            // btnOptimize
            // 
            this.btnOptimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOptimize.Location = new System.Drawing.Point(1054, 323);
            this.btnOptimize.Name = "btnOptimize";
            this.btnOptimize.Size = new System.Drawing.Size(82, 23);
            this.btnOptimize.TabIndex = 81;
            this.btnOptimize.Text = "Optimize";
            this.btnOptimize.UseVisualStyleBackColor = true;
            this.btnOptimize.Click += new System.EventHandler(this.btnOptimize_Click);
            // 
            // StrategyView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1146, 611);
            this.Controls.Add(this.btnOptimize);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.txtStrategyName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.grdFeatures);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmdResetFeatures);
            this.Controls.Add(this.cmdScatterPlot);
            this.Controls.Add(this.cmdBacktest);
            this.Controls.Add(this.cmdProcessFeatures);
            this.Controls.Add(this.cmdPortfolioParameters);
            this.Controls.Add(this.linkLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StrategyView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stock Ranking Analyze";
            this.Activated += new System.EventHandler(this.StrategyView_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMainUI_FormClosing);
            this.Shown += new System.EventHandler(this.frmMainUI_Shown);
            this.Resize += new System.EventHandler(this.StrategyView_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.grdFeatures)).EndInit();
            this.pnlFilters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdStocks)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdProcessFeatures;
        private System.Windows.Forms.DataGridView grdFeatures;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Button cmdExport;
        private System.Windows.Forms.LinkLabel cmdSwitchView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFeatureCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFeatureName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeight;
        private System.Windows.Forms.LinkLabel cmdResetFeatures;
        private ProcessingPanelControl pnlProcessingProgress;
        private System.Windows.Forms.Button cmdScatterPlot;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtStrategyName;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdBacktest;
        private System.Windows.Forms.Button cmdExportDebug;
        private System.Windows.Forms.Button cmdPortfolioParameters;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.DataGridView grdStocks;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCompanyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSector;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosingPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYtd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVolume;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOptimize;
    }
}

