namespace StockRanking
{
    partial class EditPositions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditPositions));
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.lstPositions = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colShares = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmdRemove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dateEntry = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.Label();
            this.txtTicker = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.lstSymbols = new System.Windows.Forms.ListBox();
            this.txtShares = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmdSave
            // 
            this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSave.Location = new System.Drawing.Point(369, 281);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(67, 23);
            this.cmdSave.TabIndex = 35;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.Location = new System.Drawing.Point(442, 281);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(67, 23);
            this.cmdClose.TabIndex = 40;
            this.cmdClose.Text = "Cancel";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // lstPositions
            // 
            this.lstPositions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPositions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.colShares,
            this.columnHeader5,
            this.columnHeader6});
            this.lstPositions.FullRowSelect = true;
            this.lstPositions.Location = new System.Drawing.Point(12, 128);
            this.lstPositions.Name = "lstPositions";
            this.lstPositions.Size = new System.Drawing.Size(497, 144);
            this.lstPositions.TabIndex = 25;
            this.lstPositions.UseCompatibleStateImageBehavior = false;
            this.lstPositions.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Ticker";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Entry Date";
            this.columnHeader4.Width = 100;
            // 
            // colShares
            // 
            this.colShares.Text = "Shares";
            this.colShares.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colShares.Width = 80;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Entry Price";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Name";
            this.columnHeader6.Width = 250;
            // 
            // cmdRemove
            // 
            this.cmdRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdRemove.Location = new System.Drawing.Point(12, 281);
            this.cmdRemove.Name = "cmdRemove";
            this.cmdRemove.Size = new System.Drawing.Size(67, 23);
            this.cmdRemove.TabIndex = 30;
            this.cmdRemove.Text = "Remove";
            this.cmdRemove.UseVisualStyleBackColor = true;
            this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "Ticker";
            // 
            // dateEntry
            // 
            this.dateEntry.CustomFormat = "MM-dd-yyyy";
            this.dateEntry.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateEntry.Location = new System.Drawing.Point(85, 48);
            this.dateEntry.MinDate = new System.DateTime(2004, 1, 1, 16, 25, 0, 0);
            this.dateEntry.Name = "dateEntry";
            this.dateEntry.Size = new System.Drawing.Size(95, 20);
            this.dateEntry.TabIndex = 5;
            this.dateEntry.Value = new System.DateTime(2004, 1, 1, 16, 25, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "Entry Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 63;
            this.label3.Text = "Entry Price";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 64;
            this.label4.Text = "Name";
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.AutoSize = true;
            this.txtCompanyName.Location = new System.Drawing.Point(82, 30);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(16, 13);
            this.txtCompanyName.TabIndex = 65;
            this.txtCompanyName.Text = "...";
            this.txtCompanyName.Click += new System.EventHandler(this.label5_Click);
            // 
            // txtTicker
            // 
            this.txtTicker.Location = new System.Drawing.Point(85, 6);
            this.txtTicker.Name = "txtTicker";
            this.txtTicker.Size = new System.Drawing.Size(95, 20);
            this.txtTicker.TabIndex = 1;
            this.txtTicker.TextChanged += new System.EventHandler(this.txtTicker_TextChanged);
            this.txtTicker.Enter += new System.EventHandler(this.txtTicker_Enter);
            this.txtTicker.Leave += new System.EventHandler(this.txtTicker_Leave);
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(85, 69);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(95, 20);
            this.txtPrice.TabIndex = 10;
            // 
            // cmdAdd
            // 
            this.cmdAdd.Location = new System.Drawing.Point(186, 89);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(67, 23);
            this.cmdAdd.TabIndex = 20;
            this.cmdAdd.Text = "Add";
            this.cmdAdd.UseVisualStyleBackColor = true;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // lstSymbols
            // 
            this.lstSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSymbols.FormattingEnabled = true;
            this.lstSymbols.Location = new System.Drawing.Point(293, 6);
            this.lstSymbols.Name = "lstSymbols";
            this.lstSymbols.Size = new System.Drawing.Size(216, 82);
            this.lstSymbols.TabIndex = 70;
            this.lstSymbols.TabStop = false;
            this.lstSymbols.Visible = false;
            // 
            // txtShares
            // 
            this.txtShares.Location = new System.Drawing.Point(85, 91);
            this.txtShares.Name = "txtShares";
            this.txtShares.Size = new System.Drawing.Size(95, 20);
            this.txtShares.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 71;
            this.label5.Text = "Shares";
            // 
            // EditPositions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 313);
            this.Controls.Add(this.txtShares);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lstSymbols);
            this.Controls.Add(this.cmdAdd);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.txtTicker);
            this.Controls.Add(this.txtCompanyName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateEntry);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdRemove);
            this.Controls.Add(this.lstPositions);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.cmdClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditPositions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Current Portfolio";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.ListView lstPositions;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button cmdRemove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateEntry;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label txtCompanyName;
        private System.Windows.Forms.TextBox txtTicker;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Button cmdAdd;
        private System.Windows.Forms.ListBox lstSymbols;
        private System.Windows.Forms.ColumnHeader colShares;
        private System.Windows.Forms.TextBox txtShares;
        private System.Windows.Forms.Label label5;
    }
}