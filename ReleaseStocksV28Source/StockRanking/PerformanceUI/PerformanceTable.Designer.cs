namespace StockRanking
{
    partial class PerformanceTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PerformanceTable));
            this.grdTable = new System.Windows.Forms.DataGridView();
            this.colYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmdClose = new System.Windows.Forms.Button();
            this.grdIndicators = new System.Windows.Forms.DataGridView();
            this.colAlpha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSharpe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBeta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStdev = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaxDD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHighWater = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutpCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnderCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWorst = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grdRiskStatistics = new System.Windows.Forms.DataGridView();
            this.colRiskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRisk1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRisk2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.grdTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdIndicators)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdRiskStatistics)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdTable
            // 
            this.grdTable.AllowUserToAddRows = false;
            this.grdTable.AllowUserToDeleteRows = false;
            this.grdTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colYear});
            this.grdTable.Location = new System.Drawing.Point(4, 4);
            this.grdTable.Margin = new System.Windows.Forms.Padding(4);
            this.grdTable.Name = "grdTable";
            this.grdTable.ReadOnly = true;
            this.grdTable.RowHeadersVisible = false;
            this.grdTable.Size = new System.Drawing.Size(899, 499);
            this.grdTable.TabIndex = 0;
            // 
            // colYear
            // 
            this.colYear.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colYear.DefaultCellStyle = dataGridViewCellStyle1;
            this.colYear.HeaderText = "YEAR";
            this.colYear.Name = "colYear";
            this.colYear.ReadOnly = true;
            this.colYear.Width = 74;
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.Location = new System.Drawing.Point(1238, 637);
            this.cmdClose.Margin = new System.Windows.Forms.Padding(4);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(93, 31);
            this.cmdClose.TabIndex = 1;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // grdIndicators
            // 
            this.grdIndicators.AllowUserToAddRows = false;
            this.grdIndicators.AllowUserToDeleteRows = false;
            this.grdIndicators.AllowUserToResizeRows = false;
            this.grdIndicators.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdIndicators.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdIndicators.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAlpha,
            this.colSharpe,
            this.colProfit,
            this.colBeta,
            this.colStdev,
            this.colMaxDD,
            this.colHighWater,
            this.colOutpCount,
            this.colUnderCount,
            this.colBest,
            this.colWorst});
            this.grdIndicators.Location = new System.Drawing.Point(4, 510);
            this.grdIndicators.Margin = new System.Windows.Forms.Padding(4);
            this.grdIndicators.Name = "grdIndicators";
            this.grdIndicators.ReadOnly = true;
            this.grdIndicators.RowHeadersVisible = false;
            this.grdIndicators.Size = new System.Drawing.Size(899, 104);
            this.grdIndicators.TabIndex = 2;
            // 
            // colAlpha
            // 
            this.colAlpha.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle2.Format = "n2";
            this.colAlpha.DefaultCellStyle = dataGridViewCellStyle2;
            this.colAlpha.HeaderText = "ALPHA";
            this.colAlpha.MinimumWidth = 70;
            this.colAlpha.Name = "colAlpha";
            this.colAlpha.ReadOnly = true;
            // 
            // colSharpe
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle3.Format = "n2";
            this.colSharpe.DefaultCellStyle = dataGridViewCellStyle3;
            this.colSharpe.HeaderText = "SHARPE";
            this.colSharpe.Name = "colSharpe";
            this.colSharpe.ReadOnly = true;
            this.colSharpe.Width = 70;
            // 
            // colProfit
            // 
            dataGridViewCellStyle4.Format = "n2";
            this.colProfit.DefaultCellStyle = dataGridViewCellStyle4;
            this.colProfit.HeaderText = "PROFIT FACTOR";
            this.colProfit.Name = "colProfit";
            this.colProfit.ReadOnly = true;
            this.colProfit.Width = 70;
            // 
            // colBeta
            // 
            dataGridViewCellStyle5.Format = "n2";
            this.colBeta.DefaultCellStyle = dataGridViewCellStyle5;
            this.colBeta.HeaderText = "BETA";
            this.colBeta.Name = "colBeta";
            this.colBeta.ReadOnly = true;
            this.colBeta.Width = 70;
            // 
            // colStdev
            // 
            dataGridViewCellStyle6.Format = "n2";
            this.colStdev.DefaultCellStyle = dataGridViewCellStyle6;
            this.colStdev.HeaderText = "STDEV";
            this.colStdev.Name = "colStdev";
            this.colStdev.ReadOnly = true;
            this.colStdev.Width = 70;
            // 
            // colMaxDD
            // 
            dataGridViewCellStyle7.Format = "0.00 %";
            this.colMaxDD.DefaultCellStyle = dataGridViewCellStyle7;
            this.colMaxDD.HeaderText = "MAX DD";
            this.colMaxDD.Name = "colMaxDD";
            this.colMaxDD.ReadOnly = true;
            this.colMaxDD.Width = 70;
            // 
            // colHighWater
            // 
            dataGridViewCellStyle8.Format = "0.00 %";
            this.colHighWater.DefaultCellStyle = dataGridViewCellStyle8;
            this.colHighWater.HeaderText = "HIGH WATER";
            this.colHighWater.Name = "colHighWater";
            this.colHighWater.ReadOnly = true;
            this.colHighWater.Width = 70;
            // 
            // colOutpCount
            // 
            dataGridViewCellStyle9.Format = "n0";
            this.colOutpCount.DefaultCellStyle = dataGridViewCellStyle9;
            this.colOutpCount.HeaderText = "OUTPERFORM MONTHS";
            this.colOutpCount.MinimumWidth = 130;
            this.colOutpCount.Name = "colOutpCount";
            this.colOutpCount.ReadOnly = true;
            this.colOutpCount.Width = 130;
            // 
            // colUnderCount
            // 
            dataGridViewCellStyle10.Format = "n0";
            this.colUnderCount.DefaultCellStyle = dataGridViewCellStyle10;
            this.colUnderCount.HeaderText = "UNDERPERF. MONTHS";
            this.colUnderCount.MinimumWidth = 130;
            this.colUnderCount.Name = "colUnderCount";
            this.colUnderCount.ReadOnly = true;
            this.colUnderCount.Width = 130;
            // 
            // colBest
            // 
            dataGridViewCellStyle11.Format = "n0";
            this.colBest.DefaultCellStyle = dataGridViewCellStyle11;
            this.colBest.HeaderText = "BEST STREAK";
            this.colBest.Name = "colBest";
            this.colBest.ReadOnly = true;
            this.colBest.Width = 70;
            // 
            // colWorst
            // 
            dataGridViewCellStyle12.Format = "n0";
            this.colWorst.DefaultCellStyle = dataGridViewCellStyle12;
            this.colWorst.HeaderText = "WORST STREAK";
            this.colWorst.Name = "colWorst";
            this.colWorst.ReadOnly = true;
            this.colWorst.Width = 70;
            // 
            // grdRiskStatistics
            // 
            this.grdRiskStatistics.AllowUserToAddRows = false;
            this.grdRiskStatistics.AllowUserToDeleteRows = false;
            this.grdRiskStatistics.AllowUserToResizeRows = false;
            this.grdRiskStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdRiskStatistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdRiskStatistics.ColumnHeadersVisible = false;
            this.grdRiskStatistics.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRiskName,
            this.colRisk1,
            this.colRisk2});
            this.grdRiskStatistics.Location = new System.Drawing.Point(4, 4);
            this.grdRiskStatistics.Margin = new System.Windows.Forms.Padding(4);
            this.grdRiskStatistics.Name = "grdRiskStatistics";
            this.grdRiskStatistics.ReadOnly = true;
            this.grdRiskStatistics.RowHeadersVisible = false;
            this.grdRiskStatistics.Size = new System.Drawing.Size(400, 610);
            this.grdRiskStatistics.TabIndex = 3;
            // 
            // colRiskName
            // 
            this.colRiskName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colRiskName.HeaderText = "Name";
            this.colRiskName.MinimumWidth = 50;
            this.colRiskName.Name = "colRiskName";
            this.colRiskName.ReadOnly = true;
            // 
            // colRisk1
            // 
            this.colRisk1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle13.Format = "n4";
            this.colRisk1.DefaultCellStyle = dataGridViewCellStyle13;
            this.colRisk1.HeaderText = "Value1";
            this.colRisk1.MinimumWidth = 50;
            this.colRisk1.Name = "colRisk1";
            this.colRisk1.ReadOnly = true;
            this.colRisk1.Width = 50;
            // 
            // colRisk2
            // 
            this.colRisk2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle14.Format = "n4";
            this.colRisk2.DefaultCellStyle = dataGridViewCellStyle14;
            this.colRisk2.HeaderText = "Value2";
            this.colRisk2.MinimumWidth = 50;
            this.colRisk2.Name = "colRisk2";
            this.colRisk2.ReadOnly = true;
            this.colRisk2.Width = 50;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grdTable);
            this.splitContainer1.Panel1.Controls.Add(this.grdIndicators);
            this.splitContainer1.Panel1MinSize = 400;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdRiskStatistics);
            this.splitContainer1.Panel2MinSize = 100;
            this.splitContainer1.Size = new System.Drawing.Size(1319, 618);
            this.splitContainer1.SplitterDistance = 907;
            this.splitContainer1.TabIndex = 4;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // PerformanceTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1347, 675);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.cmdClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(700, 600);
            this.Name = "PerformanceTable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Performance Table";
            this.Resize += new System.EventHandler(this.PerformanceTable_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.grdTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdIndicators)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdRiskStatistics)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYear;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.DataGridView grdIndicators;
        private System.Windows.Forms.DataGridView grdRiskStatistics;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRiskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRisk1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRisk2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAlpha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSharpe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProfit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBeta;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStdev;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaxDD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHighWater;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutpCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnderCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBest;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWorst;
    }
}