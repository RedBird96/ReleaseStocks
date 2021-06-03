namespace StockRanking
{
    partial class CompositeYearsPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompositeYearsPopup));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cmdProcessFeatures = new System.Windows.Forms.Button();
            this.txtValue = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lblProcessing = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtValue)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(241, 131);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(82, 23);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(84, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 13);
            this.label7.TabIndex = 88;
            this.label7.Text = "Rebalancing Interval";
            // 
            // cmdProcessFeatures
            // 
            this.cmdProcessFeatures.Enabled = false;
            this.cmdProcessFeatures.Location = new System.Drawing.Point(153, 131);
            this.cmdProcessFeatures.Name = "cmdProcessFeatures";
            this.cmdProcessFeatures.Size = new System.Drawing.Size(82, 23);
            this.cmdProcessFeatures.TabIndex = 4;
            this.cmdProcessFeatures.Text = "Save Value";
            this.cmdProcessFeatures.UseVisualStyleBackColor = true;
            this.cmdProcessFeatures.Click += new System.EventHandler(this.cmdProcessFeatures_Click);
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(195, 29);
            this.txtValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(48, 20);
            this.txtValue.TabIndex = 1;
            this.txtValue.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.txtValue.ValueChanged += new System.EventHandler(this.txtValue_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(311, 56);
            this.label1.TabIndex = 89;
            this.label1.Text = "This value changes the composite calculation for the whole system. It is not Stra" +
    "tegy dependant. When setting a new value it will reprocess the precalculated fil" +
    "ters.";
            // 
            // lblProcessing
            // 
            this.lblProcessing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcessing.Location = new System.Drawing.Point(12, 109);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(108, 45);
            this.lblProcessing.TabIndex = 90;
            this.lblProcessing.Text = "Reprocessing filters, please wait a couple of minutes...";
            this.lblProcessing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProcessing.Visible = false;
            // 
            // CompositeYearsPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 165);
            this.Controls.Add(this.lblProcessing);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmdProcessFeatures);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CompositeYearsPopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Composite Years";
            ((System.ComponentModel.ISupportInitialize)(this.txtValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button cmdProcessFeatures;
        private System.Windows.Forms.NumericUpDown txtValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblProcessing;
    }
}