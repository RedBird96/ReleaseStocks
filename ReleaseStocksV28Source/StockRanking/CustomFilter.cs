using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockRanking
{
    public partial class CustomFilter : UserControl
    {
        private String filterName = "";
        private FilterTypes filterType = FilterTypes.ClosingPrice;
        public event EventHandler FilterChanged;

        public CustomFilter()
        {
            InitializeComponent();

            cboFilter.SelectedIndex = 0;
            cboOption.SelectedIndex = 0;

            RefreshStatus();
        }

        public string FilterName { get => filterName;
            set
            {
                filterName = value;
                lblName.Text = filterName;
            }
        }

        public FilterTypes FilterType { get => filterType;
            set {
                filterType = value;
                if (filterType == FilterTypes.MovingAverage)
                {
                    cboFilter.Items.RemoveAt(2);
                } else if (filterType == FilterTypes.RoCVSBenchmark || filterType == FilterTypes.RoC)
                {
                    cboFilter.Items.RemoveAt(2);
                }
            }
        }

        private void chkActivation_CheckedChanged(object sender, EventArgs e)
        {
            RefreshStatus();
        }
        
        public bool IsActive
        {
            get { return chkActivation.Checked; }
        }

        public void SetDisabled()
        {
            chkActivation.Checked = false;
            RefreshStatus();
        }

        public void FillConditions(FilterConditions filter)
        {
            chkActivation.Checked = true;
            switch(filter.FilterRange)
            {
                case FilterRanges.Between:
                    cboFilter.SelectedIndex = 2;
                    break;
                case FilterRanges.LessThan:
                    cboFilter.SelectedIndex = 1;
                    break;
                case FilterRanges.MoreThan:
                    cboFilter.SelectedIndex = 0;
                    break;
            }

            txtNumber1.Text = filter.Value1.ToString();
            txtNumber2.Text = filter.Value2.ToString();
            txtOrderNum.Text = filter.Order.ToString();
            cboOption.SelectedIndex = filter.Option;
            RefreshStatus();
        }

        void RefreshStatus()
        {
            if (chkActivation.Checked)
            {
                txtNumber1.Enabled = true;
                txtOrderNum.Enabled = true;
                cboOption.Enabled = true;
                cboFilter.Enabled = true;
                
                if (cboFilter.SelectedIndex == 2 || filterType == FilterTypes.RoC)
                {
                    txtNumber2.Enabled = true;
                    txtNumber2.Visible = true;
                    lblAnd.Visible = true;
                }
                else
                {
                    txtNumber2.Enabled = false;
                    txtNumber2.Visible = false;
                    lblAnd.Visible = false;
                }

                if (filterType == FilterTypes.RoC || filterType == FilterTypes.MovingAverage || filterType == FilterTypes.RoCVSBenchmark)
                {
                    cboOption.Enabled = false;
                    lblAnd.Visible = true;
                    lblAnd.Text = (filterType == FilterTypes.RoC) ? "len:" : ":len";
                } 
            }
            else
            {
                txtNumber1.Enabled = false;
                txtNumber2.Enabled = false;
                txtNumber2.Visible = false;
                cboOption.Enabled = false;
                lblAnd.Visible = false;
                cboFilter.Enabled = false;
                txtOrderNum.Enabled = false;
            }

            FilterChanged?.Invoke(this, new EventArgs());
        }

        private void cboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshStatus();
        }

        private void txtNumber1_TextChanged(object sender, EventArgs e)
        {
            RefreshStatus();
        }

        private void txtNumber2_TextChanged(object sender, EventArgs e)
        {
            RefreshStatus();
        }

        public FilterConditions GetFilterConditions()
        {

            FilterConditions filterCond = new FilterConditions()
            {
                isCustom = 0,
                FilterRange = (FilterRanges)cboFilter.SelectedIndex,
                FilterType = filterType,
                Value1 = Utils.ConvertToDouble(txtNumber1.Text),
                Value2 = Utils.ConvertToDouble(txtNumber2.Text),
                Order = Convert.ToInt32(txtOrderNum.Text),
                Option = cboOption.SelectedIndex
            };

            return filterCond;
        }

    }
}
