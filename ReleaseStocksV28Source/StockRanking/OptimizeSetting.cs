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
    public partial class OptimizeSetting : Form
    {
        Strategy strategy;
        List<FeatureWeight> featureWeights;

        public OptimizeSetting()
        {
            InitializeComponent();
        }

        public OptimizeSetting(Strategy _strategy)
        {
            InitializeComponent();
            strategy = _strategy;
        }

        private void cmdStart_Click(object sender, EventArgs e)
        {
            int selectedfunc = cboFunction.SelectedIndex;
            OptimizeResult resultDlg = new OptimizeResult(strategy, featureWeights, selectedfunc);
            this.Hide();
            resultDlg.ShowDialog();
            this.Close();
        }

        private void OptimizeSetting_Load(object sender, EventArgs e)
        {
            featureWeights = Feature.generateFeatureIntervalList();

            grdFeatures.AutoGenerateColumns = false;
            grdFeatures.DataSource = featureWeights;

            for (int i = 0; i < featureWeights.Count; i++)
            {
                FeatureWeight fw = featureWeights[i];

                fw.IsEnabled = false;
                fw.Weight = 0;

                try
                {
                    Feature featureFound = strategy.Features.Find(x => x.FeatureId == i);

                    if (featureFound == null)
                        continue;

                    fw.Weight = (float)featureFound.FeatureWeight;
                    fw.From = (float) 0;
                    fw.To = (float)(10 * featureFound.FeatureWeight);
                    fw.Step = (float)0.1;

                    if (fw.Weight != 0)
                        fw.IsEnabled = true;
                }
                catch (Exception)
                { }
            }
            grdFeatures.Refresh();

            cboFunction.SelectedIndex = 0;
            RefreshFeaturesGrid();
        }

        private void grdFeatures_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            grdFeatures.RefreshEdit();
        }

        private void grdFeatures_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!(grdFeatures.CurrentCell is DataGridViewCheckBoxCell))
                return;

            DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)grdFeatures.CurrentCell;

            if (cell != null && !cell.ReadOnly)
            {
                cell.Value = cell.Value == null || !((bool)cell.Value);
                grdFeatures.RefreshEdit();
                grdFeatures.NotifyCurrentCellDirty(true);
            }

            RefreshFeaturesGrid();
        }

        void RefreshFeaturesGrid()
        {
            foreach (DataGridViewRow row in grdFeatures.Rows)
            {
                if ((bool)row.Cells[0].Value == true)
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.Cells[2].ReadOnly = false;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.ForeColor = Color.Gray;
                    row.Cells[2].ReadOnly = true;
                }
            }
        }

        private void grdFeatures_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4)
            {
                try
                {
                    decimal val = Convert.ToDecimal(e.FormattedValue);
                    if (e.ColumnIndex == 4 && val == 0)
                    {
                        MessageBox.Show("Please write a numeric value greater than zero", "Conversion error");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Please write a numeric value", "Conversion error");
                    e.Cancel = true;
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < featureWeights.Count; i++)
            {
                FeatureWeight fw = featureWeights[i];

                fw.IsEnabled = true;
                fw.From = 0;
                fw.To = 10;
                fw.Step = (float)0.1;
            }
            grdFeatures.Refresh();
            RefreshFeaturesGrid();
        }
    }
}
