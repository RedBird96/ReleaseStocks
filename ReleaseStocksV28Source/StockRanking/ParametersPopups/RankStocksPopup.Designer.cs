namespace StockRanking
{
    partial class RankStocksPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RankStocksPopup));
            this.cboGroupBy = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dateSimulateDay = new System.Windows.Forms.DateTimePicker();
            this.cmdProcessFeatures = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVRHistoryYears = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.txtVRHistoryYears)).BeginInit();
            this.SuspendLayout();
            // 
            // cboGroupBy
            // 
            this.cboGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGroupBy.FormattingEnabled = true;
            this.cboGroupBy.Items.AddRange(new object[] {
            "Stocks",
            "Industries",
            "Sectors"});
            this.cboGroupBy.Location = new System.Drawing.Point(114, 55);
            this.cboGroupBy.Name = "cboGroupBy";
            this.cboGroupBy.Size = new System.Drawing.Size(141, 21);
            this.cboGroupBy.TabIndex = 7;
            this.cboGroupBy.SelectedIndexChanged += new System.EventHandler(this.cboGroupBy_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(57, 58);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 84;
            this.label7.Text = "Group By";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(57, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 13);
            this.label6.TabIndex = 83;
            this.label6.Text = "Rank Stocks for Date:";
            // 
            // dateSimulateDay
            // 
            this.dateSimulateDay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateSimulateDay.CustomFormat = "MM/dd/yyyy";
            this.dateSimulateDay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateSimulateDay.Location = new System.Drawing.Point(173, 29);
            this.dateSimulateDay.MinDate = new System.DateTime(2005, 1, 1, 0, 0, 0, 0);
            this.dateSimulateDay.Name = "dateSimulateDay";
            this.dateSimulateDay.Size = new System.Drawing.Size(82, 20);
            this.dateSimulateDay.TabIndex = 3;
            // 
            // cmdProcessFeatures
            // 
            this.cmdProcessFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdProcessFeatures.Location = new System.Drawing.Point(134, 110);
            this.cmdProcessFeatures.Name = "cmdProcessFeatures";
            this.cmdProcessFeatures.Size = new System.Drawing.Size(82, 23);
            this.cmdProcessFeatures.TabIndex = 20;
            this.cmdProcessFeatures.Text = "Run Ranking";
            this.cmdProcessFeatures.UseVisualStyleBackColor = true;
            this.cmdProcessFeatures.Click += new System.EventHandler(this.cmdProcessFeatures_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Location = new System.Drawing.Point(222, 110);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(82, 23);
            this.cmdCancel.TabIndex = 25;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 85;
            this.label1.Text = "VR History Years:";
            this.label1.Visible = false;
            // 
            // txtVRHistoryYears
            // 
            this.txtVRHistoryYears.Location = new System.Drawing.Point(173, 82);
            this.txtVRHistoryYears.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.txtVRHistoryYears.Name = "txtVRHistoryYears";
            this.txtVRHistoryYears.Size = new System.Drawing.Size(82, 20);
            this.txtVRHistoryYears.TabIndex = 12;
            this.txtVRHistoryYears.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.txtVRHistoryYears.Visible = false;
            // 
            // RankStocksPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 141);
            this.Controls.Add(this.txtVRHistoryYears);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cboGroupBy);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dateSimulateDay);
            this.Controls.Add(this.cmdProcessFeatures);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RankStocksPopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rank Stocks";
            ((System.ComponentModel.ISupportInitialize)(this.txtVRHistoryYears)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboGroupBy;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateSimulateDay;
        private System.Windows.Forms.Button cmdProcessFeatures;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown txtVRHistoryYears;
    }
}