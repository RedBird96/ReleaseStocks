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
    public partial class ScatterPlotPopup : Form
    {
        public static int RebalancingInterval = 1;
        public static int SelectedIndex = 0;

        public ScatterPlotPopup()
        {
            InitializeComponent();

            cboPeriod.Items.Add(new KeyValuePair<int, string>(1, "Monthly"));
            cboPeriod.Items.Add(new KeyValuePair<int, string>(3, "Quarter"));
            cboPeriod.Items.Add(new KeyValuePair<int, string>(6, "Six Months"));
            cboPeriod.Items.Add(new KeyValuePair<int, string>(12, "Annually"));

            cboPeriod.ValueMember = "Key";
            cboPeriod.DisplayMember = "Value";
            cboPeriod.SelectedIndex = 2;

            cboGroupBy.SelectedIndex = SelectedIndex;

            foreach (KeyValuePair<int, string> value in cboPeriod.Items)
                if (value.Key == RebalancingInterval)
                    cboPeriod.SelectedItem = value;

        }

        private void cmdProcessFeatures_Click(object sender, EventArgs e)
        {
            RebalancingInterval = (int)((KeyValuePair<int, string>)(cboPeriod.SelectedItem)).Key;
            SelectedIndex = cboGroupBy.SelectedIndex;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
