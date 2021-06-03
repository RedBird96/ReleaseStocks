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
    public partial class RankStocksPopup : Form
    {
        public static int SelectedIndex = 0;
        public static DateTime SelectedDate = DateTime.Now;

        public RankStocksPopup()
        {
            InitializeComponent();
            cboGroupBy.SelectedIndex = SelectedIndex;
            cboGroupBy_SelectedIndexChanged(this, null);
            dateSimulateDay.Value = SelectedDate;
        }

        private void cmdProcessFeatures_Click(object sender, EventArgs e)
        {
            SelectedIndex = cboGroupBy.SelectedIndex;
            SelectedDate = dateSimulateDay.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cboGroupBy_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
