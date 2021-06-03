namespace StockRanking
{
    partial class SelectETFSymbolPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectETFSymbolPopup));
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lstETFSymbols = new System.Windows.Forms.ListView();
            this.colETFSymbol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colETFName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colETFCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colETFExchange = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colETFDataSince = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colETFDataTo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 17);
            this.label1.TabIndex = 103;
            this.label1.Text = "Select one or more ETF to Add";
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(61, 34);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(154, 22);
            this.txtFilter.TabIndex = 4;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // cmdAdd
            // 
            this.cmdAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdAdd.Location = new System.Drawing.Point(573, 368);
            this.cmdAdd.Margin = new System.Windows.Forms.Padding(4);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(89, 28);
            this.cmdAdd.TabIndex = 22;
            this.cmdAdd.Text = "Add";
            this.cmdAdd.UseVisualStyleBackColor = true;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Location = new System.Drawing.Point(670, 368);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(4);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(89, 28);
            this.cmdCancel.TabIndex = 33;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 17);
            this.label2.TabIndex = 107;
            this.label2.Text = "Filter:";
            // 
            // lstETFSymbols
            // 
            this.lstETFSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstETFSymbols.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colETFSymbol,
            this.colETFName,
            this.colETFCategory,
            this.colETFExchange,
            this.colETFDataSince,
            this.colETFDataTo});
            this.lstETFSymbols.FullRowSelect = true;
            this.lstETFSymbols.Location = new System.Drawing.Point(15, 63);
            this.lstETFSymbols.Margin = new System.Windows.Forms.Padding(4);
            this.lstETFSymbols.Name = "lstETFSymbols";
            this.lstETFSymbols.Size = new System.Drawing.Size(745, 297);
            this.lstETFSymbols.TabIndex = 108;
            this.lstETFSymbols.UseCompatibleStateImageBehavior = false;
            this.lstETFSymbols.View = System.Windows.Forms.View.Details;
            // 
            // colETFSymbol
            // 
            this.colETFSymbol.Text = "Symbol";
            this.colETFSymbol.Width = 100;
            // 
            // colETFName
            // 
            this.colETFName.Text = "Name";
            this.colETFName.Width = 200;
            // 
            // colETFCategory
            // 
            this.colETFCategory.Text = "Category";
            this.colETFCategory.Width = 100;
            // 
            // colETFExchange
            // 
            this.colETFExchange.Text = "Exchange";
            this.colETFExchange.Width = 100;
            // 
            // colETFDataSince
            // 
            this.colETFDataSince.Text = "First Date";
            this.colETFDataSince.Width = 100;
            // 
            // colETFDataTo
            // 
            this.colETFDataTo.Text = "Last Date";
            this.colETFDataTo.Width = 100;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(221, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(247, 17);
            this.label3.TabIndex = 109;
            this.label3.Text = "(enter at least 2 characters to search)";
            // 
            // SelectETFSymbolPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 409);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstETFSymbols);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdAdd);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectETFSymbolPopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add ETF Symbol";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Button cmdAdd;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lstETFSymbols;
        private System.Windows.Forms.ColumnHeader colETFSymbol;
        private System.Windows.Forms.ColumnHeader colETFName;
        private System.Windows.Forms.ColumnHeader colETFCategory;
        private System.Windows.Forms.ColumnHeader colETFExchange;
        private System.Windows.Forms.ColumnHeader colETFDataSince;
        private System.Windows.Forms.ColumnHeader colETFDataTo;
        private System.Windows.Forms.Label label3;
    }
}