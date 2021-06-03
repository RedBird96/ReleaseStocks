namespace StockRanking
{
    partial class CustomFilter
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkActivation = new JCS.ToggleSwitch();
            this.lblName = new System.Windows.Forms.Label();
            this.txtOrderNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNumber2 = new System.Windows.Forms.TextBox();
            this.lblAnd = new System.Windows.Forms.Label();
            this.txtNumber1 = new System.Windows.Forms.TextBox();
            this.cboOption = new System.Windows.Forms.ComboBox();
            this.byLabel = new System.Windows.Forms.Label();
            this.cboFilter = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // chkActivation
            // 
            this.chkActivation.Location = new System.Drawing.Point(3, 3);
            this.chkActivation.Name = "chkActivation";
            this.chkActivation.OffFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkActivation.OffText = "OFF";
            this.chkActivation.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkActivation.OnForeColor = System.Drawing.Color.White;
            this.chkActivation.OnText = "ON";
            this.chkActivation.Size = new System.Drawing.Size(50, 19);
            this.chkActivation.Style = JCS.ToggleSwitch.ToggleSwitchStyle.OSX;
            this.chkActivation.TabIndex = 50;
            this.chkActivation.CheckedChanged += new JCS.ToggleSwitch.CheckedChangedDelegate(this.chkActivation_CheckedChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(59, 6);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(68, 13);
            this.lblName.TabIndex = 51;
            this.lblName.Text = "Closing Price";
            // 
            // txtOrderNum
            // 
            this.txtOrderNum.Location = new System.Drawing.Point(509, 4);
            this.txtOrderNum.Name = "txtOrderNum";
            this.txtOrderNum.Size = new System.Drawing.Size(47, 20);
            this.txtOrderNum.TabIndex = 57;
            this.txtOrderNum.Text = "0";
            this.txtOrderNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(491, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 56;
            this.label1.Text = "#";
            // 
            // txtNumber2
            // 
            this.txtNumber2.Location = new System.Drawing.Point(442, 3);
            this.txtNumber2.Name = "txtNumber2";
            this.txtNumber2.Size = new System.Drawing.Size(44, 20);
            this.txtNumber2.TabIndex = 55;
            this.txtNumber2.Text = "0";
            this.txtNumber2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNumber2.Visible = false;
            this.txtNumber2.TextChanged += new System.EventHandler(this.txtNumber2_TextChanged);
            // 
            // lblAnd
            // 
            this.lblAnd.AutoSize = true;
            this.lblAnd.Location = new System.Drawing.Point(412, 6);
            this.lblAnd.Name = "lblAnd";
            this.lblAnd.Size = new System.Drawing.Size(25, 13);
            this.lblAnd.TabIndex = 54;
            this.lblAnd.Text = "and";
            this.lblAnd.Visible = false;
            // 
            // txtNumber1
            // 
            this.txtNumber1.Location = new System.Drawing.Point(357, 3);
            this.txtNumber1.Name = "txtNumber1";
            this.txtNumber1.Size = new System.Drawing.Size(49, 20);
            this.txtNumber1.TabIndex = 53;
            this.txtNumber1.Text = "0";
            this.txtNumber1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNumber1.TextChanged += new System.EventHandler(this.txtNumber1_TextChanged);
            // 
            // cboOption
            // 
            this.cboOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOption.FormattingEnabled = true;
            this.cboOption.Items.AddRange(new object[] {
            "Raw",
            "Deciles",
            "Quintiles",
            "Quartiles",
            "Halves"});
            this.cboOption.Location = new System.Drawing.Point(272, 2);
            this.cboOption.Name = "cboOption";
            this.cboOption.Size = new System.Drawing.Size(75, 21);
            this.cboOption.TabIndex = 58;
            // 
            // byLabel
            // 
            this.byLabel.AutoSize = true;
            this.byLabel.Location = new System.Drawing.Point(248, 7);
            this.byLabel.Name = "byLabel";
            this.byLabel.Size = new System.Drawing.Size(18, 13);
            this.byLabel.TabIndex = 59;
            this.byLabel.Text = "by";
            this.byLabel.Visible = false;
            // 
            // cboFilter
            // 
            this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilter.FormattingEnabled = true;
            this.cboFilter.Items.AddRange(new object[] {
            "More than",
            "Less than",
            "Between"});
            this.cboFilter.Location = new System.Drawing.Point(167, 3);
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.Size = new System.Drawing.Size(77, 21);
            this.cboFilter.TabIndex = 52;
            this.cboFilter.SelectedIndexChanged += new System.EventHandler(this.cboFilter_SelectedIndexChanged);
            // 
            // CustomFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.byLabel);
            this.Controls.Add(this.cboOption);
            this.Controls.Add(this.txtOrderNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNumber2);
            this.Controls.Add(this.lblAnd);
            this.Controls.Add(this.chkActivation);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.cboFilter);
            this.Controls.Add(this.txtNumber1);
            this.Name = "CustomFilter";
            this.Size = new System.Drawing.Size(565, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private JCS.ToggleSwitch chkActivation;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtOrderNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNumber2;
        private System.Windows.Forms.Label lblAnd;
        private System.Windows.Forms.TextBox txtNumber1;
        private System.Windows.Forms.ComboBox cboOption;
        private System.Windows.Forms.Label byLabel;
        private System.Windows.Forms.ComboBox cboFilter;
    }
}
