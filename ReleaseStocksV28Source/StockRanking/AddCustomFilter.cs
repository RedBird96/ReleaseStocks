using StockRanking.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockRanking
{
    public partial class AddCustomFilter : Form
    {
        public EquationFilter filter;
        private Control focusedControl = null;
        private bool isValid = false;
        public AddCustomFilter()
        {
            InitializeComponent();

            cboFilter.SelectedIndex = 0;
            cboCompare.SelectedIndex = 0;
            cboOption.SelectedIndex = 0;

            listIndicators.View = View.Details;
            listIndicators.GridLines = true;
            listIndicators.FullRowSelect = true;

            listIndicators.Columns.Add("Indicator");
            listIndicators.Columns.Add("Description");
            listIndicators.HeaderStyle = ColumnHeaderStyle.Nonclickable;

            foreach (Indicators indicator in Enum.GetValues(typeof(Indicators)))
            {
                var displayname = indicator.GetDisplayName();

                String[] str = new String[3];
                str[0] = Enum.GetName(typeof(Indicators), indicator);
                str[1] = displayname;
                ListViewItem item = new ListViewItem(str);

                listIndicators.Items.Add(item);
            }
            listIndicators.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            listIndicators.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);

            filter = new EquationFilter();
            foreach (Control control in this.Controls)
            {
                if (control.Controls.Count == 0)
                {
                    control.GotFocus += onFocus;
                }
                else
                {
                    foreach (Control control1 in control.Controls)
                    {
                        control1.GotFocus += onFocus;
                    }
                }
            }
        }

        public AddCustomFilter(EquationFilter _filter)
        {
            InitializeComponent();

            cboFilter.SelectedIndex = 0;
            cboCompare.SelectedIndex = 0;
            cboOption.SelectedIndex = 0;

            listIndicators.View = View.Details;
            listIndicators.GridLines = true;
            listIndicators.FullRowSelect = true;

            listIndicators.Columns.Add("Indicator");
            listIndicators.Columns.Add("Description");
            listIndicators.HeaderStyle = ColumnHeaderStyle.Nonclickable;

            foreach (Indicators indicator in Enum.GetValues(typeof(Indicators)))
            {
                var displayname = indicator.GetDisplayName();

                String[] str = new String[3];
                str[0] = Enum.GetName(typeof(Indicators), indicator);
                str[1] = displayname;
                ListViewItem item = new ListViewItem(str);

                listIndicators.Items.Add(item);
            }
            listIndicators.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            listIndicators.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);


            filter = _filter;
            
            foreach (Control control in this.Controls)
            {
                if (control.Controls.Count == 0)
                {
                    control.GotFocus += onFocus;
                }
                else
                {
                    foreach (Control control1 in control.Controls)
                    {
                        control1.GotFocus += onFocus;
                    }
                }
            }

            txtFilterName.Text = filter.FilterName;
            cboFilter.SelectedIndex = filter.FilterType;
            cboOption.SelectedIndex = filter.FilterOption;
            if (filter.FilterType == 3)
            {
                txtEqu1.Text = filter.Equation1;
                txtEqu2.Text = filter.Equation2;
            } else
            {
                txtEqu.Text = filter.Equation1;
            }
            
            txtNumber1.Text = filter.Value1.ToString();
            txtNumber2.Text = filter.Value2.ToString();
            cboCompare.SelectedIndex = filter.EquCompare;
            isValid = true;
        }

        private void onFocus(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            String name = control.Name;

            if (name != "listIndicators")
            {
                focusedControl = control;
                if (name == "txtEqu" || name == "txtEqu1" || name == "txtEqu2")
                {
                    this.listIndicators.Enabled = true;
                } else
                {
                    this.listIndicators.Enabled = false;
                }
            }
        }
        void btnSave_Click(object sender, EventArgs e)
        {
            if (txtFilterName.Text.Trim() == "")
            {
                MessageBox.Show("Please input filter name.");
                return;
            }
            if (isValid == false)
            {
                MessageBox.Show("Invalid equation.");
                return;
            }
            filter.FilterName = txtFilterName.Text;
            filter.FilterType = cboFilter.SelectedIndex;
            filter.FilterOption = cboOption.SelectedIndex;
            
            if (filter.FilterType == 3)
            {
                filter.Equation1 = txtEqu1.Text;
                filter.Equation2 = txtEqu2.Text;
            } else
            {
                filter.Equation1 = txtEqu.Text;
            }
            filter.Value1 = Convert.ToDecimal(txtNumber1.Text);
            filter.Value2 = Convert.ToDecimal(txtNumber2.Text);
            filter.EquCompare = cboCompare.SelectedIndex;
            this.filter.Save();
            this.Close();
        }

        private void cboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFilter.SelectedIndex == 3)
            {
                panelComplex.Visible = true;
                panelSimple.Visible = false;
            } else
            {
                panelComplex.Visible = false;
                panelSimple.Visible = true;
                if (cboFilter.SelectedIndex != 2)
                {
                    andlabel.Visible = false;
                    number2label.Visible = false;
                    txtNumber2.Visible = false;
                } else
                {
                    andlabel.Visible = true;
                    number2label.Visible = true;
                    txtNumber2.Visible = true;
                }
            }
            refreshEquation();
        }

        private void form_shown(object sender, EventArgs e)
        {
            
        }

        private void listIndicators_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listIndicators.SelectedItems.Count > 0)
            {
                String text = this.listIndicators.SelectedItems[0].SubItems[0].Text;
                if (focusedControl.Name == "txtEqu" || focusedControl.Name == "txtEqu1" || focusedControl.Name == "txtEqu2")
                {
                    TextBox txtBox = (TextBox)focusedControl;
                    focusedControl.Focus();
                    int start = txtBox.SelectionStart;
                    focusedControl.Text = focusedControl.Text.Insert(txtBox.SelectionStart, text);
                    txtBox.SelectionStart = start + text.Length;
                }
            }
        }

        private void cboCompare_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshEquation();
        }

        private void txtFilterName_TextChanged(object sender, EventArgs e)
        {
            refreshEquation();
        }

        private void txtEqu1_TextChanged(object sender, EventArgs e)
        {
            refreshEquation();
        }

        private void txtEqu2_TextChanged(object sender, EventArgs e)
        {
            refreshEquation();
        }

        private void cboOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshEquation();
        }

        private void txtEqu_TextChanged(object sender, EventArgs e)
        {
            refreshEquation();
        }

        private void txtNumber1_TextChanged(object sender, EventArgs e)
        {
            refreshEquation();
        }

        private void txtNumber2_TextChanged(object sender, EventArgs e)
        {
            refreshEquation();
        }

        private int findBracket(String str, int i)
        {
            int r = 0;
            while(i >= 0)
            {
                if (str[i] == ')') r++;
                if (str[i] == '(') r--;
                if (r == 0) return i;
                i--;
            }
            return i;
        }
        private Boolean notValid(String str, int st, int en)
        {
            while (en > st && str[en - 1] == ' ') en--;
            while (en > st && str[st] == ' ') st++;

            if (st >= en) return true;
            Console.WriteLine("Valid " + str.Substring(st, en - st));
            String op = "+-*/";
            int p = en - 1;
            if (str[p] == ')')
            {
                p = findBracket(str, p);
                if (p < st) return true;
                if (p == st) return notValid(str, st + 1, en - 1);
                p--;
                while (p >= st && str[p] == ' ') p--;
                if (p < st) return true;
                if (op.Contains(str[p]) == false)
                {
                    return true;
                }
                return notValid(str, st, p) || notValid(str, p + 1, en);
            }

            p = en - 1;
            while (p >= st)
            {
                if (str[p] == ')') p = findBracket(str, p);
                if (p < st)
                {
                    return true;
                }

                if (str[p] == '+' || str[p] == '-')
                {
                    if (notValid(str, st, p)) return true;
                    if (notValid(str, p + 1, en)) return true;
                    return false;
                }
                p--;
            }

            p = en - 1;
            while (p >= st)
            {
                if (str[p] == ')') p = findBracket(str, p);
                if (p < st)
                {
                    return true;
                }

                if (str[p] == '*' || str[p] == '/')
                {
                    if (notValid(str, st, p)) return true;
                    if (notValid(str, p+1, en)) return true;
                    return false;
                }
                p--;
            }

            try
            {
                decimal value = Convert.ToDecimal(str.Substring(st, en - st));
            } catch (Exception e)
            {
                foreach (Indicators indicator in Enum.GetValues(typeof(Indicators)))
                {
                    String indstr= Enum.GetName(typeof(Indicators), indicator);
                    if (indstr == str.Substring(st, en-st))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        private void refreshEquation()
        {
            isValid = true;
            if (cboFilter.SelectedIndex < 3)
            {
                String left = txtEqu.Text.Trim();
                if (left == "")
                {
                    left = "Equation";
                    isValid = false;
                }
                else if (notValid(left, 0, left.Length))
                {
                    txtEquation.Text = "Equation is not valid.";
                    isValid = false;
                    return;
                }
                if (cboFilter.SelectedIndex == 0)
                {
                    try
                    {
                        decimal right = Convert.ToDecimal(txtNumber1.Text.Trim());
                        String value = right.ToString();
                        if (cboOption.SelectedIndex != 0)
                        {
                            value += " " + cboOption.SelectedItem.ToString();
                        }

                        txtEquation.Text = left + " > " + value;
                    } catch (Exception e)
                    {
                        txtEquation.Text = "Invalid value.";
                        isValid = false;
                    }
                    
                } else if (cboFilter.SelectedIndex == 1)
                {
                    try
                    {
                        decimal right = Convert.ToDecimal(txtNumber1.Text.Trim());
                        String value = right.ToString();
                        if (cboOption.SelectedIndex != 0)
                        {
                            value += " " + cboOption.SelectedItem.ToString();
                        }

                        txtEquation.Text = left + " < " + value;
                    }
                    catch (Exception e)
                    {
                        txtEquation.Text = "Invalid value.";
                        isValid = false;
                    }

                } else if (cboFilter.SelectedIndex == 2)
                {
                    try
                    {
                        decimal right = Convert.ToDecimal(txtNumber1.Text.Trim());
                        decimal right1 = Convert.ToDecimal(txtNumber2.Text.Trim());
                        String value = right.ToString();
                        String value1 = right1.ToString();
                        if (cboOption.SelectedIndex != 0)
                        {
                            value += " " + cboOption.SelectedItem.ToString();
                            value1 += " " + cboOption.SelectedItem.ToString();
                        }
                        txtEquation.Text = value + " < " + left + " < " + value1;
                    } catch (Exception e)
                    {
                        txtEquation.Text = "Invalid value.";
                        isValid = false;
                    }
                }
            } else
            {
                String left = txtEqu1.Text.Trim();
                String right = txtEqu2.Text.Trim();
                if (left == "")
                {
                    left = "Equation 1";
                    isValid = false;
                }
                else if (notValid(left, 0, left.Length))
                {
                    txtEquation.Text = "Equation 1 is not valid.";
                    isValid = false;
                    return;
                }
                if (right == "")
                {
                    right = "Equation 2";
                    isValid = false;
                }
                else if (notValid(right, 0, right.Length))
                {
                    txtEquation.Text = "Equation 2 is not valid.";
                    isValid = false;
                    return;
                }
                if (cboCompare.SelectedIndex == 0)
                {
                    txtEquation.Text = left + " > " + right;
                } else
                {
                    txtEquation.Text = left + " < " + right;
                }
            }
        }
    }
}
