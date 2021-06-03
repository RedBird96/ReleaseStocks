namespace StockRanking
{
    partial class PerformanceGraph
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PerformanceGraph));
            this.chartPerformance = new LiveCharts.WinForms.CartesianChart();
            this.grdIndicators = new System.Windows.Forms.DataGridView();
            this.chkBlendedBasic = new System.Windows.Forms.CheckBox();
            this.chkFixedIncome = new System.Windows.Forms.CheckBox();
            this.chkEquity = new System.Windows.Forms.CheckBox();
            this.chkBlendedAdvanced = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRollingYears = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.lblLargestGain = new System.Windows.Forms.Label();
            this.lblLargestLoss = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblVolatility = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmdClose = new System.Windows.Forms.Button();
            this.cmdPerformanceGraph = new System.Windows.Forms.LinkLabel();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMTD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQTD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYTD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col6M = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col1Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col3Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col5Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col10Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInception = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdIndicators)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRollingYears)).BeginInit();
            this.SuspendLayout();
            // 
            // chartPerformance
            // 
            this.chartPerformance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartPerformance.BackColor = System.Drawing.Color.White;
            this.chartPerformance.Location = new System.Drawing.Point(12, 55);
            this.chartPerformance.Name = "chartPerformance";
            this.chartPerformance.Size = new System.Drawing.Size(1055, 407);
            this.chartPerformance.TabIndex = 0;
            this.chartPerformance.Text = "Performance Graph";
            // 
            // grdIndicators
            // 
            this.grdIndicators.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdIndicators.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdIndicators.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdIndicators.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colMTD,
            this.colQTD,
            this.colYTD,
            this.col6M,
            this.col1Y,
            this.col3Y,
            this.col5Y,
            this.col10Y,
            this.colInception});
            this.grdIndicators.Location = new System.Drawing.Point(12, 469);
            this.grdIndicators.Margin = new System.Windows.Forms.Padding(4);
            this.grdIndicators.Name = "grdIndicators";
            this.grdIndicators.ReadOnly = true;
            this.grdIndicators.Size = new System.Drawing.Size(1054, 176);
            this.grdIndicators.TabIndex = 3;
            // 
            // chkBlendedBasic
            // 
            this.chkBlendedBasic.AutoSize = true;
            this.chkBlendedBasic.Checked = true;
            this.chkBlendedBasic.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBlendedBasic.Location = new System.Drawing.Point(254, 18);
            this.chkBlendedBasic.Margin = new System.Windows.Forms.Padding(4);
            this.chkBlendedBasic.Name = "chkBlendedBasic";
            this.chkBlendedBasic.Size = new System.Drawing.Size(120, 21);
            this.chkBlendedBasic.TabIndex = 47;
            this.chkBlendedBasic.Text = "Blended Basic";
            this.chkBlendedBasic.UseVisualStyleBackColor = true;
            this.chkBlendedBasic.CheckedChanged += new System.EventHandler(this.chkEquity_CheckedChanged);
            // 
            // chkFixedIncome
            // 
            this.chkFixedIncome.AutoSize = true;
            this.chkFixedIncome.Checked = true;
            this.chkFixedIncome.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFixedIncome.Location = new System.Drawing.Point(112, 18);
            this.chkFixedIncome.Margin = new System.Windows.Forms.Padding(4);
            this.chkFixedIncome.Name = "chkFixedIncome";
            this.chkFixedIncome.Size = new System.Drawing.Size(112, 21);
            this.chkFixedIncome.TabIndex = 46;
            this.chkFixedIncome.Text = "Fixed Income";
            this.chkFixedIncome.UseVisualStyleBackColor = true;
            this.chkFixedIncome.CheckedChanged += new System.EventHandler(this.chkEquity_CheckedChanged);
            // 
            // chkEquity
            // 
            this.chkEquity.AutoSize = true;
            this.chkEquity.Checked = true;
            this.chkEquity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEquity.Location = new System.Drawing.Point(13, 18);
            this.chkEquity.Margin = new System.Windows.Forms.Padding(4);
            this.chkEquity.Name = "chkEquity";
            this.chkEquity.Size = new System.Drawing.Size(69, 21);
            this.chkEquity.TabIndex = 45;
            this.chkEquity.Text = "Equity";
            this.chkEquity.UseVisualStyleBackColor = true;
            this.chkEquity.CheckedChanged += new System.EventHandler(this.chkEquity_CheckedChanged);
            // 
            // chkBlendedAdvanced
            // 
            this.chkBlendedAdvanced.AutoSize = true;
            this.chkBlendedAdvanced.Checked = true;
            this.chkBlendedAdvanced.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBlendedAdvanced.Location = new System.Drawing.Point(404, 18);
            this.chkBlendedAdvanced.Margin = new System.Windows.Forms.Padding(4);
            this.chkBlendedAdvanced.Name = "chkBlendedAdvanced";
            this.chkBlendedAdvanced.Size = new System.Drawing.Size(149, 21);
            this.chkBlendedAdvanced.TabIndex = 48;
            this.chkBlendedAdvanced.Text = "Blended Advanced";
            this.chkBlendedAdvanced.UseVisualStyleBackColor = true;
            this.chkBlendedAdvanced.CheckedChanged += new System.EventHandler(this.chkEquity_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(932, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 17);
            this.label1.TabIndex = 49;
            this.label1.Text = "Rolling Years:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtRollingYears
            // 
            this.txtRollingYears.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRollingYears.Location = new System.Drawing.Point(1034, 17);
            this.txtRollingYears.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.txtRollingYears.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtRollingYears.Name = "txtRollingYears";
            this.txtRollingYears.Size = new System.Drawing.Size(33, 22);
            this.txtRollingYears.TabIndex = 50;
            this.txtRollingYears.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.txtRollingYears.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(581, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 25);
            this.label2.TabIndex = 51;
            this.label2.Text = "Largest Gain";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblLargestGain
            // 
            this.lblLargestGain.Location = new System.Drawing.Point(581, 27);
            this.lblLargestGain.Name = "lblLargestGain";
            this.lblLargestGain.Size = new System.Drawing.Size(108, 25);
            this.lblLargestGain.TabIndex = 53;
            this.lblLargestGain.Text = "   0.00 %";
            this.lblLargestGain.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblLargestLoss
            // 
            this.lblLargestLoss.Location = new System.Drawing.Point(695, 27);
            this.lblLargestLoss.Name = "lblLargestLoss";
            this.lblLargestLoss.Size = new System.Drawing.Size(108, 25);
            this.lblLargestLoss.TabIndex = 55;
            this.lblLargestLoss.Text = "   0.00 %";
            this.lblLargestLoss.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(695, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 25);
            this.label4.TabIndex = 54;
            this.label4.Text = "Largest Loss";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblVolatility
            // 
            this.lblVolatility.Location = new System.Drawing.Point(809, 27);
            this.lblVolatility.Name = "lblVolatility";
            this.lblVolatility.Size = new System.Drawing.Size(108, 25);
            this.lblVolatility.TabIndex = 57;
            this.lblVolatility.Text = "   0.00 %";
            this.lblVolatility.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(809, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 25);
            this.label7.TabIndex = 56;
            this.label7.Text = "Volatility";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.Location = new System.Drawing.Point(962, 651);
            this.cmdClose.Margin = new System.Windows.Forms.Padding(4);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(104, 28);
            this.cmdClose.TabIndex = 58;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // cmdPerformanceGraph
            // 
            this.cmdPerformanceGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPerformanceGraph.Location = new System.Drawing.Point(769, 655);
            this.cmdPerformanceGraph.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cmdPerformanceGraph.Name = "cmdPerformanceGraph";
            this.cmdPerformanceGraph.Size = new System.Drawing.Size(185, 32);
            this.cmdPerformanceGraph.TabIndex = 67;
            this.cmdPerformanceGraph.TabStop = true;
            this.cmdPerformanceGraph.Text = "Edit Benchmarks";
            this.cmdPerformanceGraph.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.cmdPerformanceGraph.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.cmdPerformanceGraph_LinkClicked);
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colMTD
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle2.Format = "n2";
            this.colMTD.DefaultCellStyle = dataGridViewCellStyle2;
            this.colMTD.HeaderText = "MTD %";
            this.colMTD.Name = "colMTD";
            this.colMTD.ReadOnly = true;
            // 
            // colQTD
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle3.Format = "n2";
            this.colQTD.DefaultCellStyle = dataGridViewCellStyle3;
            this.colQTD.HeaderText = "QTD %";
            this.colQTD.Name = "colQTD";
            this.colQTD.ReadOnly = true;
            // 
            // colYTD
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle4.Format = "n2";
            this.colYTD.DefaultCellStyle = dataGridViewCellStyle4;
            this.colYTD.HeaderText = "YTD %";
            this.colYTD.Name = "colYTD";
            this.colYTD.ReadOnly = true;
            // 
            // col6M
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle5.Format = "n2";
            this.col6M.DefaultCellStyle = dataGridViewCellStyle5;
            this.col6M.HeaderText = "6M %";
            this.col6M.Name = "col6M";
            this.col6M.ReadOnly = true;
            // 
            // col1Y
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle6.Format = "n2";
            this.col1Y.DefaultCellStyle = dataGridViewCellStyle6;
            this.col1Y.HeaderText = "1Y %";
            this.col1Y.Name = "col1Y";
            this.col1Y.ReadOnly = true;
            // 
            // col3Y
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle7.Format = "n2";
            this.col3Y.DefaultCellStyle = dataGridViewCellStyle7;
            this.col3Y.HeaderText = "3Y %";
            this.col3Y.Name = "col3Y";
            this.col3Y.ReadOnly = true;
            // 
            // col5Y
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle8.Format = "n2";
            this.col5Y.DefaultCellStyle = dataGridViewCellStyle8;
            this.col5Y.HeaderText = "5Y %";
            this.col5Y.Name = "col5Y";
            this.col5Y.ReadOnly = true;
            // 
            // col10Y
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle9.Format = "n2";
            this.col10Y.DefaultCellStyle = dataGridViewCellStyle9;
            this.col10Y.HeaderText = "10Y %";
            this.col10Y.Name = "col10Y";
            this.col10Y.ReadOnly = true;
            // 
            // colInception
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle10.Format = "n2";
            this.colInception.DefaultCellStyle = dataGridViewCellStyle10;
            this.colInception.HeaderText = "Inception%";
            this.colInception.Name = "colInception";
            this.colInception.ReadOnly = true;
            // 
            // PerformanceGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1079, 685);
            this.Controls.Add(this.cmdPerformanceGraph);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.lblVolatility);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblLargestLoss);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chartPerformance);
            this.Controls.Add(this.lblLargestGain);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRollingYears);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkBlendedAdvanced);
            this.Controls.Add(this.chkBlendedBasic);
            this.Controls.Add(this.chkFixedIncome);
            this.Controls.Add(this.chkEquity);
            this.Controls.Add(this.grdIndicators);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1090, 600);
            this.Name = "PerformanceGraph";
            this.Text = "Performance Graph";
            ((System.ComponentModel.ISupportInitialize)(this.grdIndicators)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRollingYears)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LiveCharts.WinForms.CartesianChart chartPerformance;
        private System.Windows.Forms.DataGridView grdIndicators;
        private System.Windows.Forms.CheckBox chkBlendedBasic;
        private System.Windows.Forms.CheckBox chkFixedIncome;
        private System.Windows.Forms.CheckBox chkEquity;
        private System.Windows.Forms.CheckBox chkBlendedAdvanced;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown txtRollingYears;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblLargestGain;
        private System.Windows.Forms.Label lblLargestLoss;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblVolatility;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.LinkLabel cmdPerformanceGraph;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMTD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQTD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYTD;
        private System.Windows.Forms.DataGridViewTextBoxColumn col6M;
        private System.Windows.Forms.DataGridViewTextBoxColumn col1Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn col3Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn col5Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn col10Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInception;
    }
}