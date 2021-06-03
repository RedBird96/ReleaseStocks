namespace StockRanking
{
    partial class EquationFilterControl
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
            this.txtOrderNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkActivation = new JCS.ToggleSwitch();
            this.lblName = new System.Windows.Forms.Label();
            this.txtEquation = new System.Windows.Forms.TextBox();
            this.linkEdit = new System.Windows.Forms.LinkLabel();
            this.linkDelete = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // txtOrderNum
            // 
            this.txtOrderNum.Enabled = false;
            this.txtOrderNum.Location = new System.Drawing.Point(509, 4);
            this.txtOrderNum.Name = "txtOrderNum";
            this.txtOrderNum.Size = new System.Drawing.Size(47, 20);
            this.txtOrderNum.TabIndex = 61;
            this.txtOrderNum.Text = "0";
            this.txtOrderNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(491, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "#";
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
            this.chkActivation.TabIndex = 58;
            this.chkActivation.CheckedChanged += new JCS.ToggleSwitch.CheckedChangedDelegate(this.chkActivation_CheckedChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(59, 6);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(68, 13);
            this.lblName.TabIndex = 59;
            this.lblName.Text = "Closing Price";
            // 
            // txtEquation
            // 
            this.txtEquation.Enabled = false;
            this.txtEquation.Location = new System.Drawing.Point(166, 3);
            this.txtEquation.Multiline = true;
            this.txtEquation.Name = "txtEquation";
            this.txtEquation.Size = new System.Drawing.Size(319, 42);
            this.txtEquation.TabIndex = 62;
            // 
            // linkEdit
            // 
            this.linkEdit.AutoSize = true;
            this.linkEdit.Location = new System.Drawing.Point(59, 30);
            this.linkEdit.Name = "linkEdit";
            this.linkEdit.Size = new System.Drawing.Size(25, 13);
            this.linkEdit.TabIndex = 63;
            this.linkEdit.TabStop = true;
            this.linkEdit.Text = "Edit";
            this.linkEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkEdit_LinkClicked);
            // 
            // linkDelete
            // 
            this.linkDelete.AutoSize = true;
            this.linkDelete.Location = new System.Drawing.Point(119, 30);
            this.linkDelete.Name = "linkDelete";
            this.linkDelete.Size = new System.Drawing.Size(38, 13);
            this.linkDelete.TabIndex = 64;
            this.linkDelete.TabStop = true;
            this.linkDelete.Text = "Delete";
            this.linkDelete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDelete_LinkClicked);
            // 
            // EquationFilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkDelete);
            this.Controls.Add(this.linkEdit);
            this.Controls.Add(this.txtEquation);
            this.Controls.Add(this.txtOrderNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkActivation);
            this.Controls.Add(this.lblName);
            this.Name = "EquationFilterControl";
            this.Size = new System.Drawing.Size(565, 48);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOrderNum;
        private System.Windows.Forms.Label label1;
        private JCS.ToggleSwitch chkActivation;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtEquation;
        private System.Windows.Forms.LinkLabel linkEdit;
        private System.Windows.Forms.LinkLabel linkDelete;
    }
}
