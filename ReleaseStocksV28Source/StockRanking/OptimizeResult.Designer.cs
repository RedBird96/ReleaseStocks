namespace StockRanking
{
    partial class OptimizeResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptimizeResult));
            this.lblProcessStep = new System.Windows.Forms.Label();
            this.progressBar0 = new System.Windows.Forms.ProgressBar();
            this.grdFeatures = new System.Windows.Forms.DataGridView();
            this.colFeatureCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFeatureName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.progressBar3 = new System.Windows.Forms.ProgressBar();
            this.gridResults = new System.Windows.Forms.DataGridView();
            this.PNL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColRatio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProfitMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdFeatures)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridResults)).BeginInit();
            this.SuspendLayout();
            // 
            // lblProcessStep
            // 
            this.lblProcessStep.Location = new System.Drawing.Point(9, 8);
            this.lblProcessStep.Name = "lblProcessStep";
            this.lblProcessStep.Size = new System.Drawing.Size(212, 17);
            this.lblProcessStep.TabIndex = 46;
            this.lblProcessStep.Text = "Generating New Result...";
            this.lblProcessStep.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // progressBar0
            // 
            this.progressBar0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar0.Location = new System.Drawing.Point(13, 30);
            this.progressBar0.Name = "progressBar0";
            this.progressBar0.Size = new System.Drawing.Size(537, 17);
            this.progressBar0.Step = 1;
            this.progressBar0.TabIndex = 45;
            // 
            // grdFeatures
            // 
            this.grdFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdFeatures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFeatures.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFeatureCheck,
            this.colFeatureName,
            this.colWeight});
            this.grdFeatures.Location = new System.Drawing.Point(278, 53);
            this.grdFeatures.Name = "grdFeatures";
            this.grdFeatures.RowHeadersVisible = false;
            this.grdFeatures.Size = new System.Drawing.Size(272, 506);
            this.grdFeatures.TabIndex = 48;
            this.grdFeatures.TabStop = false;
            // 
            // colFeatureCheck
            // 
            this.colFeatureCheck.DataPropertyName = "IsEnabled";
            this.colFeatureCheck.HeaderText = "Enabled";
            this.colFeatureCheck.MinimumWidth = 50;
            this.colFeatureCheck.Name = "colFeatureCheck";
            this.colFeatureCheck.ReadOnly = true;
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
            this.colWeight.ReadOnly = true;
            this.colWeight.Width = 70;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(12, 567);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(132, 23);
            this.btnSave.TabIndex = 50;
            this.btnSave.Text = "Save to Strategy";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(13, 53);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(537, 17);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 51;
            this.progressBar1.Visible = false;
            // 
            // progressBar2
            // 
            this.progressBar2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar2.Location = new System.Drawing.Point(13, 76);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(537, 17);
            this.progressBar2.Step = 1;
            this.progressBar2.TabIndex = 52;
            this.progressBar2.Visible = false;
            // 
            // progressBar3
            // 
            this.progressBar3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar3.Location = new System.Drawing.Point(13, 99);
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Size = new System.Drawing.Size(537, 17);
            this.progressBar3.Step = 1;
            this.progressBar3.TabIndex = 53;
            this.progressBar3.Visible = false;
            // 
            // gridResults
            // 
            this.gridResults.AllowUserToAddRows = false;
            this.gridResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gridResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PNL,
            this.DD,
            this.ColRatio,
            this.ProfitMonth});
            this.gridResults.Location = new System.Drawing.Point(12, 53);
            this.gridResults.MaximumSize = new System.Drawing.Size(260, 2000);
            this.gridResults.MultiSelect = false;
            this.gridResults.Name = "gridResults";
            this.gridResults.RowHeadersVisible = false;
            this.gridResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridResults.Size = new System.Drawing.Size(260, 506);
            this.gridResults.TabIndex = 54;
            this.gridResults.TabStop = false;
            this.gridResults.SelectionChanged += new System.EventHandler(this.gridReults_SelectionChanged);
            this.gridResults.DoubleClick += new System.EventHandler(this.gridReults_DoubleClick);
            // 
            // PNL
            // 
            this.PNL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle2.Format = "N3";
            dataGridViewCellStyle2.NullValue = null;
            this.PNL.DefaultCellStyle = dataGridViewCellStyle2;
            this.PNL.Frozen = true;
            this.PNL.HeaderText = "PNL";
            this.PNL.MinimumWidth = 60;
            this.PNL.Name = "PNL";
            this.PNL.ReadOnly = true;
            this.PNL.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PNL.Width = 60;
            // 
            // DD
            // 
            this.DD.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle3.Format = "N3";
            dataGridViewCellStyle3.NullValue = null;
            this.DD.DefaultCellStyle = dataGridViewCellStyle3;
            this.DD.Frozen = true;
            this.DD.HeaderText = "DD";
            this.DD.MinimumWidth = 55;
            this.DD.Name = "DD";
            this.DD.ReadOnly = true;
            this.DD.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DD.Width = 55;
            // 
            // ColRatio
            // 
            this.ColRatio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle4.Format = "N3";
            dataGridViewCellStyle4.NullValue = null;
            this.ColRatio.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColRatio.Frozen = true;
            this.ColRatio.HeaderText = "PNL/DD";
            this.ColRatio.MinimumWidth = 60;
            this.ColRatio.Name = "ColRatio";
            this.ColRatio.ReadOnly = true;
            this.ColRatio.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColRatio.Width = 60;
            // 
            // ProfitMonth
            // 
            this.ProfitMonth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            this.ProfitMonth.DefaultCellStyle = dataGridViewCellStyle5;
            this.ProfitMonth.HeaderText = "# Profit Month";
            this.ProfitMonth.MinimumWidth = 65;
            this.ProfitMonth.Name = "ProfitMonth";
            this.ProfitMonth.ReadOnly = true;
            this.ProfitMonth.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // OptimizeResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 598);
            this.Controls.Add(this.gridResults);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grdFeatures);
            this.Controls.Add(this.lblProcessStep);
            this.Controls.Add(this.progressBar0);
            this.Controls.Add(this.progressBar3);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 39);
            this.Name = "OptimizeResult";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Optimizing Result";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptimizeResult_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OptimizeResult_FormClosed);
            this.Load += new System.EventHandler(this.OptimizeResult_Load);
            this.Shown += new System.EventHandler(this.OptimizeResult_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.grdFeatures)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblProcessStep;
        private System.Windows.Forms.ProgressBar progressBar0;
        private System.Windows.Forms.DataGridView grdFeatures;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.ProgressBar progressBar3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFeatureCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFeatureName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeight;
        private System.Windows.Forms.DataGridView gridResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn PNL;
        private System.Windows.Forms.DataGridViewTextBoxColumn DD;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColRatio;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProfitMonth;
    }
}