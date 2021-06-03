namespace StockRanking
{
    partial class AddCustomFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddCustomFilter));
            this.label1 = new System.Windows.Forms.Label();
            this.txtEquation = new System.Windows.Forms.TextBox();
            this.byLabel = new System.Windows.Forms.Label();
            this.cboOption = new System.Windows.Forms.ComboBox();
            this.andlabel = new System.Windows.Forms.Label();
            this.txtNumber2 = new System.Windows.Forms.TextBox();
            this.lblAnd = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.cboFilter = new System.Windows.Forms.ComboBox();
            this.txtNumber1 = new System.Windows.Forms.TextBox();
            this.txtFilterName = new System.Windows.Forms.TextBox();
            this.panelSimple = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.txtEqu = new System.Windows.Forms.TextBox();
            this.number2label = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelComplex = new System.Windows.Forms.Panel();
            this.cboCompare = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtEqu1 = new System.Windows.Forms.TextBox();
            this.txtEqu2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.listIndicators = new System.Windows.Forms.ListView();
            this.panelSimple.SuspendLayout();
            this.panelComplex.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Equation Result :";
            // 
            // txtEquation
            // 
            this.txtEquation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEquation.Location = new System.Drawing.Point(13, 35);
            this.txtEquation.Name = "txtEquation";
            this.txtEquation.ReadOnly = true;
            this.txtEquation.Size = new System.Drawing.Size(812, 23);
            this.txtEquation.TabIndex = 1;
            // 
            // byLabel
            // 
            this.byLabel.AutoSize = true;
            this.byLabel.Location = new System.Drawing.Point(171, 67);
            this.byLabel.Name = "byLabel";
            this.byLabel.Size = new System.Drawing.Size(52, 13);
            this.byLabel.TabIndex = 68;
            this.byLabel.Text = "Filter type";
            this.byLabel.Visible = false;
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
            this.cboOption.Location = new System.Drawing.Point(6, 26);
            this.cboOption.Name = "cboOption";
            this.cboOption.Size = new System.Drawing.Size(85, 21);
            this.cboOption.TabIndex = 67;
            this.cboOption.SelectedIndexChanged += new System.EventHandler(this.cboOption_SelectedIndexChanged);
            // 
            // andlabel
            // 
            this.andlabel.AutoSize = true;
            this.andlabel.Location = new System.Drawing.Point(439, 30);
            this.andlabel.Name = "andlabel";
            this.andlabel.Size = new System.Drawing.Size(25, 13);
            this.andlabel.TabIndex = 65;
            this.andlabel.Text = "and";
            // 
            // txtNumber2
            // 
            this.txtNumber2.Location = new System.Drawing.Point(470, 27);
            this.txtNumber2.Name = "txtNumber2";
            this.txtNumber2.Size = new System.Drawing.Size(61, 20);
            this.txtNumber2.TabIndex = 64;
            this.txtNumber2.Text = "0";
            this.txtNumber2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNumber2.TextChanged += new System.EventHandler(this.txtNumber2_TextChanged);
            // 
            // lblAnd
            // 
            this.lblAnd.AutoSize = true;
            this.lblAnd.Location = new System.Drawing.Point(3, 7);
            this.lblAnd.Name = "lblAnd";
            this.lblAnd.Size = new System.Drawing.Size(63, 13);
            this.lblAnd.TabIndex = 63;
            this.lblAnd.Text = "Filter Option";
            this.lblAnd.Visible = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(13, 67);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(60, 13);
            this.lblName.TabIndex = 60;
            this.lblName.Text = "Filter Name";
            // 
            // cboFilter
            // 
            this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilter.FormattingEnabled = true;
            this.cboFilter.Items.AddRange(new object[] {
            "More than",
            "Less than",
            "Between",
            "Compare equation"});
            this.cboFilter.Location = new System.Drawing.Point(174, 86);
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.Size = new System.Drawing.Size(117, 21);
            this.cboFilter.TabIndex = 61;
            this.cboFilter.SelectedIndexChanged += new System.EventHandler(this.cboFilter_SelectedIndexChanged);
            // 
            // txtNumber1
            // 
            this.txtNumber1.Location = new System.Drawing.Point(362, 27);
            this.txtNumber1.Name = "txtNumber1";
            this.txtNumber1.Size = new System.Drawing.Size(68, 20);
            this.txtNumber1.TabIndex = 62;
            this.txtNumber1.Text = "0";
            this.txtNumber1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNumber1.TextChanged += new System.EventHandler(this.txtNumber1_TextChanged);
            // 
            // txtFilterName
            // 
            this.txtFilterName.Location = new System.Drawing.Point(15, 86);
            this.txtFilterName.Name = "txtFilterName";
            this.txtFilterName.Size = new System.Drawing.Size(148, 20);
            this.txtFilterName.TabIndex = 69;
            this.txtFilterName.TextChanged += new System.EventHandler(this.txtFilterName_TextChanged);
            // 
            // panelSimple
            // 
            this.panelSimple.Controls.Add(this.label8);
            this.panelSimple.Controls.Add(this.txtEqu);
            this.panelSimple.Controls.Add(this.number2label);
            this.panelSimple.Controls.Add(this.label3);
            this.panelSimple.Controls.Add(this.lblAnd);
            this.panelSimple.Controls.Add(this.txtNumber1);
            this.panelSimple.Controls.Add(this.txtNumber2);
            this.panelSimple.Controls.Add(this.cboOption);
            this.panelSimple.Controls.Add(this.andlabel);
            this.panelSimple.Location = new System.Drawing.Point(292, 60);
            this.panelSimple.Name = "panelSimple";
            this.panelSimple.Size = new System.Drawing.Size(533, 53);
            this.panelSimple.TabIndex = 70;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(95, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 77;
            this.label8.Text = "Equation";
            // 
            // txtEqu
            // 
            this.txtEqu.Location = new System.Drawing.Point(97, 27);
            this.txtEqu.Name = "txtEqu";
            this.txtEqu.Size = new System.Drawing.Size(259, 20);
            this.txtEqu.TabIndex = 76;
            this.txtEqu.TextChanged += new System.EventHandler(this.txtEqu_TextChanged);
            // 
            // number2label
            // 
            this.number2label.AutoSize = true;
            this.number2label.Location = new System.Drawing.Point(466, 8);
            this.number2label.Name = "number2label";
            this.number2label.Size = new System.Drawing.Size(40, 13);
            this.number2label.TabIndex = 69;
            this.number2label.Text = "Value2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(358, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 68;
            this.label3.Text = "Value1";
            // 
            // panelComplex
            // 
            this.panelComplex.Controls.Add(this.cboCompare);
            this.panelComplex.Controls.Add(this.label5);
            this.panelComplex.Controls.Add(this.label6);
            this.panelComplex.Controls.Add(this.txtEqu1);
            this.panelComplex.Controls.Add(this.txtEqu2);
            this.panelComplex.Location = new System.Drawing.Point(292, 60);
            this.panelComplex.Name = "panelComplex";
            this.panelComplex.Size = new System.Drawing.Size(532, 51);
            this.panelComplex.TabIndex = 71;
            // 
            // cboCompare
            // 
            this.cboCompare.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCompare.FormattingEnabled = true;
            this.cboCompare.Items.AddRange(new object[] {
            "More than",
            "Less than"});
            this.cboCompare.Location = new System.Drawing.Point(224, 25);
            this.cboCompare.Name = "cboCompare";
            this.cboCompare.Size = new System.Drawing.Size(89, 21);
            this.cboCompare.TabIndex = 75;
            this.cboCompare.SelectedIndexChanged += new System.EventHandler(this.cboCompare_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(323, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 74;
            this.label5.Text = "Equation 2";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 73;
            this.label6.Text = "Equation1";
            // 
            // txtEqu1
            // 
            this.txtEqu1.Location = new System.Drawing.Point(5, 25);
            this.txtEqu1.Name = "txtEqu1";
            this.txtEqu1.Size = new System.Drawing.Size(213, 20);
            this.txtEqu1.TabIndex = 70;
            this.txtEqu1.TextChanged += new System.EventHandler(this.txtEqu1_TextChanged);
            // 
            // txtEqu2
            // 
            this.txtEqu2.Location = new System.Drawing.Point(319, 25);
            this.txtEqu2.Name = "txtEqu2";
            this.txtEqu2.Size = new System.Drawing.Size(210, 20);
            this.txtEqu2.TabIndex = 71;
            this.txtEqu2.TextChanged += new System.EventHandler(this.txtEqu2_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(231, 13);
            this.label7.TabIndex = 73;
            this.label7.Text = "Indicators ( Double click to insert into equation )";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(751, 424);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 74;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // listIndicators
            // 
            this.listIndicators.HideSelection = false;
            this.listIndicators.Location = new System.Drawing.Point(16, 145);
            this.listIndicators.MultiSelect = false;
            this.listIndicators.Name = "listIndicators";
            this.listIndicators.Size = new System.Drawing.Size(458, 302);
            this.listIndicators.TabIndex = 75;
            this.listIndicators.UseCompatibleStateImageBehavior = false;
            this.listIndicators.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listIndicators_MouseDoubleClick);
            // 
            // AddCustomFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 455);
            this.Controls.Add(this.listIndicators);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panelComplex);
            this.Controls.Add(this.panelSimple);
            this.Controls.Add(this.txtFilterName);
            this.Controls.Add(this.byLabel);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.cboFilter);
            this.Controls.Add(this.txtEquation);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddCustomFilter";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddCustomFilter";
            this.Shown += new System.EventHandler(this.form_shown);
            this.panelSimple.ResumeLayout(false);
            this.panelSimple.PerformLayout();
            this.panelComplex.ResumeLayout(false);
            this.panelComplex.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEquation;
        private System.Windows.Forms.Label byLabel;
        private System.Windows.Forms.ComboBox cboOption;
        private System.Windows.Forms.Label andlabel;
        private System.Windows.Forms.TextBox txtNumber2;
        private System.Windows.Forms.Label lblAnd;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox cboFilter;
        private System.Windows.Forms.TextBox txtNumber1;
        private System.Windows.Forms.TextBox txtFilterName;
        private System.Windows.Forms.Panel panelSimple;
        private System.Windows.Forms.Label number2label;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelComplex;
        private System.Windows.Forms.ComboBox cboCompare;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtEqu1;
        private System.Windows.Forms.TextBox txtEqu2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtEqu;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListView listIndicators;
    }
}