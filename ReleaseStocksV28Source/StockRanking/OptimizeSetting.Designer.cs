namespace StockRanking
{
    partial class OptimizeSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptimizeSetting));
            this.grdFeatures = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdStart = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboFunction = new System.Windows.Forms.ComboBox();
            this.colFeatureCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFeatureName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdFeatures)).BeginInit();
            this.SuspendLayout();
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
            this.colFrom,
            this.colTo,
            this.colStep});
            this.grdFeatures.Location = new System.Drawing.Point(12, 38);
            this.grdFeatures.Name = "grdFeatures";
            this.grdFeatures.RowHeadersVisible = false;
            this.grdFeatures.Size = new System.Drawing.Size(408, 524);
            this.grdFeatures.TabIndex = 11;
            this.grdFeatures.TabStop = false;
            this.grdFeatures.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdFeatures_CellClick);
            this.grdFeatures.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdFeatures_CellContentClick);
            this.grdFeatures.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.grdFeatures_CellValidating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Feature Weights Interval:";
            // 
            // cmdStart
            // 
            this.cmdStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdStart.Location = new System.Drawing.Point(11, 572);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(77, 23);
            this.cmdStart.TabIndex = 67;
            this.cmdStart.Text = "Start";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(343, 572);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(77, 23);
            this.btnReset.TabIndex = 68;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(221, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 69;
            this.label1.Text = "Optimize Function:";
            // 
            // cboFunction
            // 
            this.cboFunction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFunction.FormattingEnabled = true;
            this.cboFunction.Items.AddRange(new object[] {
            "PNL",
            "DD",
            "PNL/DD",
            "# Profitable Months"});
            this.cboFunction.Location = new System.Drawing.Point(316, 10);
            this.cboFunction.Name = "cboFunction";
            this.cboFunction.Size = new System.Drawing.Size(103, 21);
            this.cboFunction.TabIndex = 70;
            // 
            // colFeatureCheck
            // 
            this.colFeatureCheck.DataPropertyName = "IsEnabled";
            this.colFeatureCheck.Frozen = true;
            this.colFeatureCheck.HeaderText = "Enabled";
            this.colFeatureCheck.MinimumWidth = 60;
            this.colFeatureCheck.Name = "colFeatureCheck";
            this.colFeatureCheck.Resizable = System.Windows.Forms.DataGridViewTriState.False;
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
            // colFrom
            // 
            this.colFrom.DataPropertyName = "From";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle1.Format = "n2";
            this.colFrom.DefaultCellStyle = dataGridViewCellStyle1;
            this.colFrom.HeaderText = "From";
            this.colFrom.MinimumWidth = 70;
            this.colFrom.Name = "colFrom";
            this.colFrom.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colFrom.Width = 70;
            // 
            // colTo
            // 
            this.colTo.DataPropertyName = "To";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = "0";
            this.colTo.DefaultCellStyle = dataGridViewCellStyle2;
            this.colTo.HeaderText = "To";
            this.colTo.MinimumWidth = 70;
            this.colTo.Name = "colTo";
            this.colTo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colTo.Width = 70;
            // 
            // colStep
            // 
            this.colStep.DataPropertyName = "Step";
            this.colStep.HeaderText = "Step";
            this.colStep.MinimumWidth = 70;
            this.colStep.Name = "colStep";
            this.colStep.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colStep.Width = 70;
            // 
            // OptimizeSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 603);
            this.Controls.Add(this.cboFunction);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.cmdStart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.grdFeatures);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(380, 0);
            this.Name = "OptimizeSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Optimizing Setting";
            this.Load += new System.EventHandler(this.OptimizeSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdFeatures)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdFeatures;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmdStart;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboFunction;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFeatureCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFeatureName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStep;
    }
}