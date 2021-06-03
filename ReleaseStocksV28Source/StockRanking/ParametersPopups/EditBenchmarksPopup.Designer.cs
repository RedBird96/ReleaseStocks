namespace StockRanking
{
    partial class EditBenchmarksPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditBenchmarksPopup));
            this.lstSelected = new System.Windows.Forms.ListView();
            this.lstAll = new System.Windows.Forms.ListView();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.cmdRemove = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstSelected
            // 
            this.lstSelected.FullRowSelect = true;
            this.lstSelected.HideSelection = false;
            this.lstSelected.Location = new System.Drawing.Point(13, 33);
            this.lstSelected.Margin = new System.Windows.Forms.Padding(4);
            this.lstSelected.Name = "lstSelected";
            this.lstSelected.Size = new System.Drawing.Size(179, 157);
            this.lstSelected.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstSelected.TabIndex = 108;
            this.lstSelected.UseCompatibleStateImageBehavior = false;
            this.lstSelected.View = System.Windows.Forms.View.List;
            // 
            // lstAll
            // 
            this.lstAll.FullRowSelect = true;
            this.lstAll.HideSelection = false;
            this.lstAll.Location = new System.Drawing.Point(235, 34);
            this.lstAll.Margin = new System.Windows.Forms.Padding(4);
            this.lstAll.Name = "lstAll";
            this.lstAll.Size = new System.Drawing.Size(179, 157);
            this.lstAll.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstAll.TabIndex = 111;
            this.lstAll.UseCompatibleStateImageBehavior = false;
            this.lstAll.View = System.Windows.Forms.View.List;
            // 
            // cmdAdd
            // 
            this.cmdAdd.Location = new System.Drawing.Point(195, 75);
            this.cmdAdd.Margin = new System.Windows.Forms.Padding(4);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(37, 28);
            this.cmdAdd.TabIndex = 109;
            this.cmdAdd.Text = "<";
            this.cmdAdd.UseVisualStyleBackColor = true;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // cmdRemove
            // 
            this.cmdRemove.Location = new System.Drawing.Point(195, 111);
            this.cmdRemove.Margin = new System.Windows.Forms.Padding(4);
            this.cmdRemove.Name = "cmdRemove";
            this.cmdRemove.Size = new System.Drawing.Size(37, 28);
            this.cmdRemove.TabIndex = 110;
            this.cmdRemove.Text = ">";
            this.cmdRemove.UseVisualStyleBackColor = true;
            this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(232, 13);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(147, 17);
            this.label8.TabIndex = 106;
            this.label8.Text = "Available Benchmarks";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 13);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(140, 17);
            this.label9.TabIndex = 107;
            this.label9.Text = "Benchmarks to Show";
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(229, 198);
            this.cmdSave.Margin = new System.Windows.Forms.Padding(4);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(89, 28);
            this.cmdSave.TabIndex = 121;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(326, 198);
            this.cmdClose.Margin = new System.Windows.Forms.Padding(4);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(89, 28);
            this.cmdClose.TabIndex = 122;
            this.cmdClose.Text = "Cancel";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // EditBenchmarksPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 239);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.lstSelected);
            this.Controls.Add(this.lstAll);
            this.Controls.Add(this.cmdAdd);
            this.Controls.Add(this.cmdRemove);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditBenchmarksPopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Benchmarks";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstSelected;
        private System.Windows.Forms.ListView lstAll;
        private System.Windows.Forms.Button cmdAdd;
        private System.Windows.Forms.Button cmdRemove;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Button cmdClose;
    }
}