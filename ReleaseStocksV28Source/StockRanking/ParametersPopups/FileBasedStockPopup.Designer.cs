namespace StockRanking
{
    partial class FileBasedStockPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileBasedStockPopup));
            this.cmdFBSelectFile = new System.Windows.Forms.LinkLabel();
            this.txtFBFile = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.txtFBName = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.chkFBKeppUpdated = new System.Windows.Forms.CheckBox();
            this.txtFBSymbol = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // cmdFBSelectFile
            // 
            this.cmdFBSelectFile.AutoSize = true;
            this.cmdFBSelectFile.Location = new System.Drawing.Point(447, 101);
            this.cmdFBSelectFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cmdFBSelectFile.Name = "cmdFBSelectFile";
            this.cmdFBSelectFile.Size = new System.Drawing.Size(67, 17);
            this.cmdFBSelectFile.TabIndex = 11;
            this.cmdFBSelectFile.TabStop = true;
            this.cmdFBSelectFile.Text = "select file";
            this.cmdFBSelectFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.cmdFBSelectFile_LinkClicked);
            // 
            // txtFBFile
            // 
            this.txtFBFile.Location = new System.Drawing.Point(121, 97);
            this.txtFBFile.Margin = new System.Windows.Forms.Padding(4);
            this.txtFBFile.Name = "txtFBFile";
            this.txtFBFile.ReadOnly = true;
            this.txtFBFile.Size = new System.Drawing.Size(316, 22);
            this.txtFBFile.TabIndex = 7;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(51, 101);
            this.label36.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(64, 17);
            this.label36.TabIndex = 123;
            this.label36.Text = "Data File";
            // 
            // txtFBName
            // 
            this.txtFBName.Location = new System.Drawing.Point(121, 65);
            this.txtFBName.Margin = new System.Windows.Forms.Padding(4);
            this.txtFBName.Name = "txtFBName";
            this.txtFBName.Size = new System.Drawing.Size(316, 22);
            this.txtFBName.TabIndex = 4;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(51, 69);
            this.label35.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(45, 17);
            this.label35.TabIndex = 121;
            this.label35.Text = "Name";
            // 
            // chkFBKeppUpdated
            // 
            this.chkFBKeppUpdated.AutoSize = true;
            this.chkFBKeppUpdated.Checked = true;
            this.chkFBKeppUpdated.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFBKeppUpdated.Location = new System.Drawing.Point(121, 129);
            this.chkFBKeppUpdated.Margin = new System.Windows.Forms.Padding(4);
            this.chkFBKeppUpdated.Name = "chkFBKeppUpdated";
            this.chkFBKeppUpdated.Size = new System.Drawing.Size(121, 21);
            this.chkFBKeppUpdated.TabIndex = 14;
            this.chkFBKeppUpdated.Text = "Keep Updated";
            this.chkFBKeppUpdated.UseVisualStyleBackColor = true;
            // 
            // txtFBSymbol
            // 
            this.txtFBSymbol.Location = new System.Drawing.Point(121, 33);
            this.txtFBSymbol.Margin = new System.Windows.Forms.Padding(4);
            this.txtFBSymbol.Name = "txtFBSymbol";
            this.txtFBSymbol.Size = new System.Drawing.Size(115, 22);
            this.txtFBSymbol.TabIndex = 1;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(51, 37);
            this.label34.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(54, 17);
            this.label34.TabIndex = 118;
            this.label34.Text = "Symbol";
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(339, 175);
            this.cmdSave.Margin = new System.Windows.Forms.Padding(4);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(89, 28);
            this.cmdSave.TabIndex = 18;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(436, 175);
            this.cmdClose.Margin = new System.Windows.Forms.Padding(4);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(89, 28);
            this.cmdClose.TabIndex = 22;
            this.cmdClose.Text = "Cancel";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "Asset File.csv";
            this.openFileDialog.Filter = "csv files (*.csv)|*.csv";
            // 
            // FileBasedStockPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 217);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdFBSelectFile);
            this.Controls.Add(this.txtFBFile);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.txtFBName);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.chkFBKeppUpdated);
            this.Controls.Add(this.txtFBSymbol);
            this.Controls.Add(this.label34);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FileBasedStockPopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "File Based Stock";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel cmdFBSelectFile;
        private System.Windows.Forms.TextBox txtFBFile;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TextBox txtFBName;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.CheckBox chkFBKeppUpdated;
        private System.Windows.Forms.TextBox txtFBSymbol;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}