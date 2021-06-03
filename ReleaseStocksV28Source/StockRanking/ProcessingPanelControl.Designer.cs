namespace StockRanking
{
    partial class ProcessingPanelControl
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
            this.pnlProcessing = new System.Windows.Forms.Panel();
            this.pnlProgressCenter = new System.Windows.Forms.Panel();
            this.lblProcessStep = new System.Windows.Forms.Label();
            this.cmdStopUpdate = new System.Windows.Forms.Button();
            this.progressProcessing = new System.Windows.Forms.ProgressBar();
            this.cmdClose = new System.Windows.Forms.Button();
            this.cmdErrorDetails = new System.Windows.Forms.Button();
            this.cmdCopyError = new System.Windows.Forms.Button();
            this.txtErrorDetails = new System.Windows.Forms.TextBox();
            this.pnlProcessing.SuspendLayout();
            this.pnlProgressCenter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlProcessing
            // 
            this.pnlProcessing.Controls.Add(this.pnlProgressCenter);
            this.pnlProcessing.Location = new System.Drawing.Point(0, 0);
            this.pnlProcessing.Name = "pnlProcessing";
            this.pnlProcessing.Size = new System.Drawing.Size(666, 428);
            this.pnlProcessing.TabIndex = 44;
            this.pnlProcessing.Visible = false;
            this.pnlProcessing.Resize += new System.EventHandler(this.pnlProcessing_Resize);
            // 
            // pnlProgressCenter
            // 
            this.pnlProgressCenter.Controls.Add(this.lblProcessStep);
            this.pnlProgressCenter.Controls.Add(this.cmdStopUpdate);
            this.pnlProgressCenter.Controls.Add(this.progressProcessing);
            this.pnlProgressCenter.Controls.Add(this.cmdClose);
            this.pnlProgressCenter.Controls.Add(this.cmdErrorDetails);
            this.pnlProgressCenter.Controls.Add(this.cmdCopyError);
            this.pnlProgressCenter.Controls.Add(this.txtErrorDetails);
            this.pnlProgressCenter.Location = new System.Drawing.Point(145, 108);
            this.pnlProgressCenter.Name = "pnlProgressCenter";
            this.pnlProgressCenter.Size = new System.Drawing.Size(451, 252);
            this.pnlProgressCenter.TabIndex = 50;
            // 
            // lblProcessStep
            // 
            this.lblProcessStep.Location = new System.Drawing.Point(19, 8);
            this.lblProcessStep.Name = "lblProcessStep";
            this.lblProcessStep.Size = new System.Drawing.Size(413, 27);
            this.lblProcessStep.TabIndex = 44;
            this.lblProcessStep.Text = "Update process initialization...";
            this.lblProcessStep.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // cmdStopUpdate
            // 
            this.cmdStopUpdate.Location = new System.Drawing.Point(338, 67);
            this.cmdStopUpdate.Name = "cmdStopUpdate";
            this.cmdStopUpdate.Size = new System.Drawing.Size(94, 27);
            this.cmdStopUpdate.TabIndex = 49;
            this.cmdStopUpdate.Text = "Stop Update";
            this.cmdStopUpdate.UseVisualStyleBackColor = true;
            this.cmdStopUpdate.Visible = false;
            this.cmdStopUpdate.Click += new System.EventHandler(this.cmdStopUpdate_Click);
            this.cmdStopUpdate.Leave += new System.EventHandler(this.cmdStopUpdate_Leave);
            // 
            // progressProcessing
            // 
            this.progressProcessing.Location = new System.Drawing.Point(22, 38);
            this.progressProcessing.Name = "progressProcessing";
            this.progressProcessing.Size = new System.Drawing.Size(410, 23);
            this.progressProcessing.Step = 1;
            this.progressProcessing.TabIndex = 0;
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(198, 67);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(82, 27);
            this.cmdClose.TabIndex = 48;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Visible = false;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // cmdErrorDetails
            // 
            this.cmdErrorDetails.Location = new System.Drawing.Point(22, 67);
            this.cmdErrorDetails.Name = "cmdErrorDetails";
            this.cmdErrorDetails.Size = new System.Drawing.Size(82, 27);
            this.cmdErrorDetails.TabIndex = 45;
            this.cmdErrorDetails.Text = "Error Details...";
            this.cmdErrorDetails.UseVisualStyleBackColor = true;
            this.cmdErrorDetails.Visible = false;
            this.cmdErrorDetails.Click += new System.EventHandler(this.cmdErrorDetails_Click);
            // 
            // cmdCopyError
            // 
            this.cmdCopyError.Location = new System.Drawing.Point(110, 67);
            this.cmdCopyError.Name = "cmdCopyError";
            this.cmdCopyError.Size = new System.Drawing.Size(82, 27);
            this.cmdCopyError.TabIndex = 47;
            this.cmdCopyError.Text = "Copy Error";
            this.cmdCopyError.UseVisualStyleBackColor = true;
            this.cmdCopyError.Visible = false;
            this.cmdCopyError.Click += new System.EventHandler(this.cmdCopyError_Click);
            // 
            // txtErrorDetails
            // 
            this.txtErrorDetails.Location = new System.Drawing.Point(22, 100);
            this.txtErrorDetails.Multiline = true;
            this.txtErrorDetails.Name = "txtErrorDetails";
            this.txtErrorDetails.Size = new System.Drawing.Size(410, 125);
            this.txtErrorDetails.TabIndex = 46;
            this.txtErrorDetails.Visible = false;
            // 
            // ProcessingPanelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlProcessing);
            this.Name = "ProcessingPanelControl";
            this.Size = new System.Drawing.Size(666, 428);
            this.pnlProcessing.ResumeLayout(false);
            this.pnlProgressCenter.ResumeLayout(false);
            this.pnlProgressCenter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlProcessing;
        private System.Windows.Forms.Panel pnlProgressCenter;
        private System.Windows.Forms.Label lblProcessStep;
        private System.Windows.Forms.Button cmdStopUpdate;
        private System.Windows.Forms.ProgressBar progressProcessing;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Button cmdErrorDetails;
        private System.Windows.Forms.Button cmdCopyError;
        private System.Windows.Forms.TextBox txtErrorDetails;
    }
}
