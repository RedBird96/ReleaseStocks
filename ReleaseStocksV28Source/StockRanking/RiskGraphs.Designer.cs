namespace StockRanking
{
    partial class RiskGraphs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RiskGraphs));
            this.chartCAPE = new LiveCharts.WinForms.CartesianChart();
            this.chkCape = new System.Windows.Forms.CheckBox();
            this.chkSPY = new System.Windows.Forms.CheckBox();
            this.chkHiLo = new System.Windows.Forms.CheckBox();
            this.filterBar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.filterBar)).BeginInit();
            this.SuspendLayout();
            // 
            // chartCAPE
            // 
            this.chartCAPE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartCAPE.BackColor = System.Drawing.Color.White;
            this.chartCAPE.Location = new System.Drawing.Point(9, 42);
            this.chartCAPE.Margin = new System.Windows.Forms.Padding(0);
            this.chartCAPE.Name = "chartCAPE";
            this.chartCAPE.Size = new System.Drawing.Size(773, 435);
            this.chartCAPE.TabIndex = 41;
            this.chartCAPE.Text = "Cumulative Sum";
            this.chartCAPE.DataClick += new LiveCharts.Events.DataClickHandler(this.chartCAPE_DataClick);
            // 
            // chkCape
            // 
            this.chkCape.AutoSize = true;
            this.chkCape.Checked = true;
            this.chkCape.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCape.Location = new System.Drawing.Point(9, 12);
            this.chkCape.Name = "chkCape";
            this.chkCape.Size = new System.Drawing.Size(54, 17);
            this.chkCape.TabIndex = 42;
            this.chkCape.Text = "CAPE";
            this.chkCape.UseVisualStyleBackColor = true;
            this.chkCape.CheckedChanged += new System.EventHandler(this.chkCape_CheckedChanged);
            // 
            // chkSPY
            // 
            this.chkSPY.AutoSize = true;
            this.chkSPY.Checked = true;
            this.chkSPY.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSPY.Location = new System.Drawing.Point(105, 12);
            this.chkSPY.Name = "chkSPY";
            this.chkSPY.Size = new System.Drawing.Size(72, 17);
            this.chkSPY.TabIndex = 43;
            this.chkSPY.Text = "SPY AVG";
            this.chkSPY.UseVisualStyleBackColor = true;
            this.chkSPY.CheckedChanged += new System.EventHandler(this.chkSPY_CheckedChanged);
            // 
            // chkHiLo
            // 
            this.chkHiLo.AutoSize = true;
            this.chkHiLo.Checked = true;
            this.chkHiLo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHiLo.Location = new System.Drawing.Point(219, 12);
            this.chkHiLo.Name = "chkHiLo";
            this.chkHiLo.Size = new System.Drawing.Size(51, 17);
            this.chkHiLo.TabIndex = 44;
            this.chkHiLo.Text = "Hi Lo";
            this.chkHiLo.UseVisualStyleBackColor = true;
            this.chkHiLo.CheckedChanged += new System.EventHandler(this.chkHiLo_CheckedChanged);
            // 
            // filterBar
            // 
            this.filterBar.LargeChange = 2;
            this.filterBar.Location = new System.Drawing.Point(365, 10);
            this.filterBar.Minimum = 1;
            this.filterBar.Name = "filterBar";
            this.filterBar.Size = new System.Drawing.Size(208, 45);
            this.filterBar.TabIndex = 49;
            this.filterBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.filterBar.Value = 1;
            this.filterBar.Scroll += new System.EventHandler(this.filterBar_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(324, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 50;
            this.label1.Text = "Zoom";
            // 
            // RiskGraphs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 486);
            this.Controls.Add(this.chartCAPE);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.filterBar);
            this.Controls.Add(this.chkHiLo);
            this.Controls.Add(this.chkSPY);
            this.Controls.Add(this.chkCape);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RiskGraphs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Risk Model";
            ((System.ComponentModel.ISupportInitialize)(this.filterBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LiveCharts.WinForms.CartesianChart chartCAPE;
        private System.Windows.Forms.CheckBox chkCape;
        private System.Windows.Forms.CheckBox chkSPY;
        private System.Windows.Forms.CheckBox chkHiLo;
        private System.Windows.Forms.TrackBar filterBar;
        private System.Windows.Forms.Label label1;
    }
}