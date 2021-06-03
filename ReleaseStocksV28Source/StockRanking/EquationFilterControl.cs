using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockRanking.Entities;

namespace StockRanking
{
    public partial class EquationFilterControl : UserControl
    {
        public FilterConditions filter;
        public EquationFilter equFilter;

        public event EventHandler OnChangedHandler;

        public EquationFilterControl()
        {
            InitializeComponent();
        }

        public EquationFilterControl(FilterConditions _filter)
        {
            InitializeComponent();
            filter = _filter;

            equFilter = EquationFilter.GetFilter(filter.isCustom);

            txtEquation.Text = equFilter.getString();
            lblName.Text = equFilter.FilterName;
        }

        public EquationFilterControl(EquationFilter _filter)
        {
            InitializeComponent();
            
            equFilter = EquationFilter.GetFilter(_filter.Id);
            txtEquation.Text = equFilter.getString();
            lblName.Text = equFilter.FilterName;
        }

        private void linkEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AddCustomFilter addCustomDialog = new AddCustomFilter(equFilter);
            DialogResult result = addCustomDialog.ShowDialog();

            equFilter = addCustomDialog.filter;
            txtEquation.Text = equFilter.getString();
            lblName.Text = equFilter.FilterName;

            if (OnChangedHandler != null)
            {
                OnChangedHandler(sender, e);
            }
        }

        private void linkDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EquationFilter.Delete(equFilter.Id);
            if (OnChangedHandler != null)
            {
                OnChangedHandler(sender, e);
            }
        }

        private void chkActivation_CheckedChanged(object sender, EventArgs e)
        {
            if (chkActivation.Checked == true)
            {
                txtOrderNum.Enabled = true;
                //linkDelete.Enabled = true;
                //linkEdit.Enabled = true;
            } else
            {
                txtOrderNum.Enabled = false;
                //linkDelete.Enabled = false;
                //linkEdit.Enabled = false;
            }
        }

        public bool IsActive
        {
            get { return chkActivation.Checked; }
        }

        public void SetDisabled()
        {
            chkActivation.Checked = false;
        }

        public void FillConditions(FilterConditions _filter)
        {
            filter = _filter;

            equFilter = EquationFilter.GetFilter(filter.isCustom);

            txtEquation.Text = equFilter.getString();
            lblName.Text = equFilter.FilterName;
            txtOrderNum.Text = filter.Order.ToString();
            chkActivation.Checked = true;
        }

        public FilterConditions GetFilterConditions()
        {
            FilterConditions filterCond = new FilterConditions()
            {
                isCustom = equFilter.Id,
                Order = Convert.ToInt32(txtOrderNum.Text),
                Option = equFilter.FilterOption,
                FilterRange = (FilterRanges) equFilter.FilterType,
                Value1 = Convert.ToDouble(equFilter.Value1),
                Value2 = Convert.ToDouble(equFilter.Value2),
                equation1 = equFilter.Equation1,
                equation2 = equFilter.Equation2,
                equCompare = equFilter.EquCompare
            };
            return filterCond;
        }
    }
}
