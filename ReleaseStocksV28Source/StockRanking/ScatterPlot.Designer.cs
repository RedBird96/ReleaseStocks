namespace StockRanking
{
    partial class ScatterPlot
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScatterPlot));
            this.label2 = new System.Windows.Forms.Label();
            this.dateFrom = new System.Windows.Forms.DateTimePicker();
            this.dateTo = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdClose = new System.Windows.Forms.Button();
            this.cmdExport = new System.Windows.Forms.Button();
            this.charScatterPlot = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.filterBar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.lblFilterValues = new System.Windows.Forms.Label();
            this.lblSlope = new System.Windows.Forms.Label();
            this.txtSlope = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.charScatterPlot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterBar)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 41;
            this.label2.Text = "Filter Dates between:";
            // 
            // dateFrom
            // 
            this.dateFrom.CustomFormat = "MM-dd-yyyy";
            this.dateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateFrom.Location = new System.Drawing.Point(131, 11);
            this.dateFrom.MinDate = new System.DateTime(2004, 1, 1, 16, 25, 0, 0);
            this.dateFrom.Name = "dateFrom";
            this.dateFrom.Size = new System.Drawing.Size(107, 20);
            this.dateFrom.TabIndex = 42;
            this.dateFrom.Value = new System.DateTime(2004, 1, 1, 16, 25, 0, 0);
            this.dateFrom.ValueChanged += new System.EventHandler(this.dateFrom_ValueChanged);
            // 
            // dateTo
            // 
            this.dateTo.CustomFormat = "MM-dd-yyyy";
            this.dateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTo.Location = new System.Drawing.Point(276, 11);
            this.dateTo.Name = "dateTo";
            this.dateTo.Size = new System.Drawing.Size(107, 20);
            this.dateTo.TabIndex = 43;
            this.dateTo.ValueChanged += new System.EventHandler(this.dateTo_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(244, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 44;
            this.label1.Text = "And";
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.Location = new System.Drawing.Point(749, 430);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 45;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // cmdExport
            // 
            this.cmdExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdExport.Location = new System.Drawing.Point(668, 430);
            this.cmdExport.Name = "cmdExport";
            this.cmdExport.Size = new System.Drawing.Size(75, 23);
            this.cmdExport.TabIndex = 46;
            this.cmdExport.Text = "Export";
            this.cmdExport.UseVisualStyleBackColor = true;
            this.cmdExport.Click += new System.EventHandler(this.cmdExport_Click);
            // 
            // charScatterPlot
            // 
            this.charScatterPlot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.Name = "ChartArea1";
            this.charScatterPlot.ChartAreas.Add(chartArea1);
            this.charScatterPlot.IsSoftShadows = false;
            legend1.Name = "Legend1";
            this.charScatterPlot.Legends.Add(legend1);
            this.charScatterPlot.Location = new System.Drawing.Point(12, 43);
            this.charScatterPlot.Name = "charScatterPlot";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            series1.MarkerSize = 2;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series2.IsVisibleInLegend = false;
            series2.Legend = "Legend1";
            series2.MarkerColor = System.Drawing.Color.Blue;
            series2.Name = "Series2";
            this.charScatterPlot.Series.Add(series1);
            this.charScatterPlot.Series.Add(series2);
            this.charScatterPlot.Size = new System.Drawing.Size(812, 381);
            this.charScatterPlot.TabIndex = 47;
            this.charScatterPlot.AxisViewChanged += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ViewEventArgs>(this.charScatterPlot_AxisViewChanged);
            // 
            // filterBar
            // 
            this.filterBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterBar.LargeChange = 100;
            this.filterBar.Location = new System.Drawing.Point(569, 10);
            this.filterBar.Maximum = 2000;
            this.filterBar.Minimum = 200;
            this.filterBar.Name = "filterBar";
            this.filterBar.Size = new System.Drawing.Size(255, 45);
            this.filterBar.SmallChange = 10;
            this.filterBar.TabIndex = 48;
            this.filterBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.filterBar.Value = 200;
            this.filterBar.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(423, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 49;
            this.label3.Text = "Ignore values over:";
            // 
            // lblFilterValues
            // 
            this.lblFilterValues.AutoSize = true;
            this.lblFilterValues.Location = new System.Drawing.Point(521, 15);
            this.lblFilterValues.Name = "lblFilterValues";
            this.lblFilterValues.Size = new System.Drawing.Size(36, 13);
            this.lblFilterValues.TabIndex = 50;
            this.lblFilterValues.Text = "200 %";
            // 
            // lblSlope
            // 
            this.lblSlope.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSlope.AutoSize = true;
            this.lblSlope.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSlope.Location = new System.Drawing.Point(9, 435);
            this.lblSlope.Name = "lblSlope";
            this.lblSlope.Size = new System.Drawing.Size(93, 13);
            this.lblSlope.TabIndex = 51;
            this.lblSlope.Text = "Linear Best Fit:";
            // 
            // txtSlope
            // 
            this.txtSlope.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSlope.AutoSize = true;
            this.txtSlope.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSlope.Location = new System.Drawing.Point(108, 435);
            this.txtSlope.Name = "txtSlope";
            this.txtSlope.Size = new System.Drawing.Size(70, 13);
            this.txtSlope.TabIndex = 52;
            this.txtSlope.Text = "Y = mX + B";
            // 
            // ScatterPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 460);
            this.Controls.Add(this.txtSlope);
            this.Controls.Add(this.lblSlope);
            this.Controls.Add(this.lblFilterValues);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.charScatterPlot);
            this.Controls.Add(this.cmdExport);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTo);
            this.Controls.Add(this.dateFrom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.filterBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ScatterPlot";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScatterPlot";
            this.Load += new System.EventHandler(this.ScatterPlot_Load);
            this.Resize += new System.EventHandler(this.ScatterPlot_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.charScatterPlot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateFrom;
        private System.Windows.Forms.DateTimePicker dateTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Button cmdExport;
        private System.Windows.Forms.DataVisualization.Charting.Chart charScatterPlot;
        private System.Windows.Forms.TrackBar filterBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblFilterValues;
        private System.Windows.Forms.Label lblSlope;
        private System.Windows.Forms.Label txtSlope;
    }
}