namespace StockRanking
{
    partial class CompositeGraph
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.chartComposite = new LiveCharts.WinForms.CartesianChart();
            this.grdMetrics = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colMin = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colMax = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colMedian = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colRollingMedian = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.grdData = new System.Windows.Forms.DataGridView();
            this.colDataMetric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataCurrent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataAverage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataMedian = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataPercAbove = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataZScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtStock = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStockSymbol = new System.Windows.Forms.TextBox();
            this.lstSymbols = new System.Windows.Forms.ListBox();
            this.cmdPrint = new System.Windows.Forms.PictureBox();
            this.cmdConfiguration = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cmdClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdMetrics)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdPrint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdConfiguration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chartComposite
            // 
            this.chartComposite.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartComposite.BackColor = System.Drawing.Color.White;
            this.chartComposite.Location = new System.Drawing.Point(0, 0);
            this.chartComposite.Margin = new System.Windows.Forms.Padding(0);
            this.chartComposite.Name = "chartComposite";
            this.chartComposite.Size = new System.Drawing.Size(773, 165);
            this.chartComposite.TabIndex = 42;
            this.chartComposite.Text = "Cumulative Sum";
            // 
            // grdMetrics
            // 
            this.grdMetrics.AllowUserToAddRows = false;
            this.grdMetrics.AllowUserToDeleteRows = false;
            this.grdMetrics.AllowUserToResizeColumns = false;
            this.grdMetrics.AllowUserToResizeRows = false;
            this.grdMetrics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdMetrics.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdMetrics.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.grdMetrics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdMetrics.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colValue,
            this.colMin,
            this.colMax,
            this.colMedian,
            this.colRollingMedian});
            this.grdMetrics.Location = new System.Drawing.Point(0, 168);
            this.grdMetrics.Name = "grdMetrics";
            this.grdMetrics.Size = new System.Drawing.Size(774, 117);
            this.grdMetrics.TabIndex = 43;
            this.grdMetrics.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdMetrics_CellContentClick);
            this.grdMetrics.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdMetrics_CellValueChanged);
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Metric";
            this.colName.Name = "colName";
            // 
            // colValue
            // 
            this.colValue.DataPropertyName = "Value";
            this.colValue.FalseValue = false;
            this.colValue.HeaderText = "Value";
            this.colValue.Name = "colValue";
            this.colValue.TrueValue = true;
            this.colValue.Width = 70;
            // 
            // colMin
            // 
            this.colMin.DataPropertyName = "Min";
            this.colMin.FalseValue = false;
            this.colMin.HeaderText = "Min";
            this.colMin.Name = "colMin";
            this.colMin.TrueValue = true;
            this.colMin.Width = 70;
            // 
            // colMax
            // 
            this.colMax.DataPropertyName = "Max";
            this.colMax.FalseValue = false;
            this.colMax.HeaderText = "Max";
            this.colMax.Name = "colMax";
            this.colMax.TrueValue = true;
            this.colMax.Width = 70;
            // 
            // colMedian
            // 
            this.colMedian.DataPropertyName = "Median";
            this.colMedian.FalseValue = false;
            this.colMedian.HeaderText = "Median";
            this.colMedian.Name = "colMedian";
            this.colMedian.TrueValue = true;
            this.colMedian.Width = 70;
            // 
            // colRollingMedian
            // 
            this.colRollingMedian.DataPropertyName = "RollingMedian";
            this.colRollingMedian.FalseValue = false;
            this.colRollingMedian.HeaderText = "Rolling Median";
            this.colRollingMedian.Name = "colRollingMedian";
            this.colRollingMedian.TrueValue = true;
            this.colRollingMedian.Width = 90;
            // 
            // grdData
            // 
            this.grdData.AllowUserToAddRows = false;
            this.grdData.AllowUserToDeleteRows = false;
            this.grdData.AllowUserToResizeColumns = false;
            this.grdData.AllowUserToResizeRows = false;
            this.grdData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdData.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDataMetric,
            this.colDataCurrent,
            this.colDataMin,
            this.colDataMax,
            this.colDataAverage,
            this.colDataMedian,
            this.colDataPercAbove,
            this.colDataZScore});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.Format = "n2";
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdData.DefaultCellStyle = dataGridViewCellStyle8;
            this.grdData.Location = new System.Drawing.Point(0, 3);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(774, 150);
            this.grdData.TabIndex = 44;
            // 
            // colDataMetric
            // 
            this.colDataMetric.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colDataMetric.DefaultCellStyle = dataGridViewCellStyle7;
            this.colDataMetric.HeaderText = "Metric";
            this.colDataMetric.Name = "colDataMetric";
            // 
            // colDataCurrent
            // 
            this.colDataCurrent.HeaderText = "Current";
            this.colDataCurrent.Name = "colDataCurrent";
            this.colDataCurrent.ReadOnly = true;
            this.colDataCurrent.Width = 90;
            // 
            // colDataMin
            // 
            this.colDataMin.HeaderText = "Min";
            this.colDataMin.Name = "colDataMin";
            this.colDataMin.ReadOnly = true;
            this.colDataMin.Width = 90;
            // 
            // colDataMax
            // 
            this.colDataMax.HeaderText = "Max";
            this.colDataMax.Name = "colDataMax";
            this.colDataMax.ReadOnly = true;
            this.colDataMax.Width = 90;
            // 
            // colDataAverage
            // 
            this.colDataAverage.HeaderText = "Average";
            this.colDataAverage.Name = "colDataAverage";
            this.colDataAverage.ReadOnly = true;
            this.colDataAverage.Width = 90;
            // 
            // colDataMedian
            // 
            this.colDataMedian.HeaderText = "Median";
            this.colDataMedian.Name = "colDataMedian";
            this.colDataMedian.ReadOnly = true;
            this.colDataMedian.Width = 90;
            // 
            // colDataPercAbove
            // 
            this.colDataPercAbove.HeaderText = "% Above";
            this.colDataPercAbove.Name = "colDataPercAbove";
            this.colDataPercAbove.ReadOnly = true;
            this.colDataPercAbove.Width = 90;
            // 
            // colDataZScore
            // 
            this.colDataZScore.HeaderText = "Z Score";
            this.colDataZScore.Name = "colDataZScore";
            this.colDataZScore.ReadOnly = true;
            this.colDataZScore.Width = 90;
            // 
            // txtStock
            // 
            this.txtStock.AutoSize = true;
            this.txtStock.Location = new System.Drawing.Point(8, 9);
            this.txtStock.Name = "txtStock";
            this.txtStock.Size = new System.Drawing.Size(47, 17);
            this.txtStock.TabIndex = 46;
            this.txtStock.Text = "Stock:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(232, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(326, 17);
            this.label1.TabIndex = 48;
            this.label1.Text = "(use mouse wheel to scroll and drag graph to pan)";
            // 
            // txtStockSymbol
            // 
            this.txtStockSymbol.Location = new System.Drawing.Point(52, 6);
            this.txtStockSymbol.Name = "txtStockSymbol";
            this.txtStockSymbol.Size = new System.Drawing.Size(92, 22);
            this.txtStockSymbol.TabIndex = 49;
            this.txtStockSymbol.TextChanged += new System.EventHandler(this.txtStockSymbol_TextChanged);
            this.txtStockSymbol.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStockSymbol_KeyPress);
            this.txtStockSymbol.Leave += new System.EventHandler(this.txtStockSymbol_Leave);
            // 
            // lstSymbols
            // 
            this.lstSymbols.FormattingEnabled = true;
            this.lstSymbols.ItemHeight = 16;
            this.lstSymbols.Location = new System.Drawing.Point(150, 6);
            this.lstSymbols.Name = "lstSymbols";
            this.lstSymbols.Size = new System.Drawing.Size(339, 68);
            this.lstSymbols.TabIndex = 76;
            this.lstSymbols.TabStop = false;
            this.lstSymbols.Visible = false;
            // 
            // cmdPrint
            // 
            this.cmdPrint.Image = global::StockRanking.Properties.Resources.printer;
            this.cmdPrint.Location = new System.Drawing.Point(27, 0);
            this.cmdPrint.Name = "cmdPrint";
            this.cmdPrint.Size = new System.Drawing.Size(18, 18);
            this.cmdPrint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.cmdPrint.TabIndex = 77;
            this.cmdPrint.TabStop = false;
            this.cmdPrint.Click += new System.EventHandler(this.cmdPrint_Click);
            // 
            // cmdConfiguration
            // 
            this.cmdConfiguration.Image = global::StockRanking.Properties.Resources.settingsicon1;
            this.cmdConfiguration.Location = new System.Drawing.Point(3, 0);
            this.cmdConfiguration.Name = "cmdConfiguration";
            this.cmdConfiguration.Size = new System.Drawing.Size(18, 18);
            this.cmdConfiguration.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.cmdConfiguration.TabIndex = 45;
            this.cmdConfiguration.TabStop = false;
            this.cmdConfiguration.Click += new System.EventHandler(this.cmdConfiguration_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(10, 29);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cmdPrint);
            this.splitContainer1.Panel1.Controls.Add(this.cmdConfiguration);
            this.splitContainer1.Panel1.Controls.Add(this.chartComposite);
            this.splitContainer1.Panel1.Controls.Add(this.grdMetrics);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdData);
            this.splitContainer1.Panel2MinSize = 100;
            this.splitContainer1.Size = new System.Drawing.Size(774, 448);
            this.splitContainer1.SplitterDistance = 288;
            this.splitContainer1.TabIndex = 78;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.Location = new System.Drawing.Point(708, 481);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 79;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // CompositeGraph
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(795, 513);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.lstSymbols);
            this.Controls.Add(this.txtStockSymbol);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtStock);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CompositeGraph";
            this.Text = "CompositeGraph";
            this.Resize += new System.EventHandler(this.CompositeGraph_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.grdMetrics)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdPrint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdConfiguration)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LiveCharts.WinForms.CartesianChart chartComposite;
        private System.Windows.Forms.DataGridView grdMetrics;
        private System.Windows.Forms.DataGridView grdData;
        private System.Windows.Forms.PictureBox cmdConfiguration;
        private System.Windows.Forms.Label txtStock;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colValue;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colMin;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colMax;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colMedian;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colRollingMedian;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataMetric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataCurrent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataAverage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataMedian;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataPercAbove;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataZScore;
        private System.Windows.Forms.TextBox txtStockSymbol;
        private System.Windows.Forms.ListBox lstSymbols;
        private System.Windows.Forms.PictureBox cmdPrint;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button cmdClose;
    }
}