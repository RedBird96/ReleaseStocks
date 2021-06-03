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
    public partial class CompositeYearsPopup : Form
    {
        public CompositeYearsPopup()
        {
            InitializeComponent();

            txtValue.Value = PortfolioParameters.GetCompositeRollingMedianYearsStatic();
        }

        private void cmdProcessFeatures_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            int newYears = (int)txtValue.Value;
            cmdCancel.Enabled = false;
            cmdProcessFeatures.Enabled = false;
            txtValue.Enabled = false;
            lblProcessing.Dock = DockStyle.Fill;
            lblProcessing.Visible = true;

            Application.DoEvents();

            try
            {
                DataUpdater.ReprocessRollingMedianYears(newYears);
            }
            catch(Exception ee)
            {
                MessageBox.Show("Error reprocessing data, please try again. Error: " + ee.Message);
            }

            this.Cursor = Cursors.Arrow;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtValue_ValueChanged(object sender, EventArgs e)
        {
            if (PortfolioParameters.GetCompositeRollingMedianYearsStatic() != (int)txtValue.Value)
                cmdProcessFeatures.Enabled = true;
            else
                cmdProcessFeatures.Enabled = false;
        }
    }
}
