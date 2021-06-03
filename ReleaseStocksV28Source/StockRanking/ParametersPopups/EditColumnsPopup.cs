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
    public partial class EditColumnsPopup : Form
    {
        List<DataGridViewColumn> columnsCurr;
        List<DataGridViewColumn> columnsExp;

        public EditColumnsPopup(List<DataGridViewColumn> curr, List<DataGridViewColumn> exp)
        {
            InitializeComponent();

            this.DialogResult = DialogResult.Cancel;
            columnsCurr = curr;
            columnsExp = exp;

            foreach (DataGridViewColumn col in columnsCurr)
                lstCurrentColumns.Items.Add(col.HeaderText, col.Visible);

            foreach (DataGridViewColumn col in columnsExp)
                lstExpectedColumns.Items.Add(col.HeaderText, col.Visible);
        }

        private void lstColumns_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmdAccept_Click(object sender, EventArgs e)
        {
            int index = 0;
            foreach (DataGridViewColumn col in columnsCurr)
                col.Visible = lstCurrentColumns.GetItemChecked(index++);

            index = 0;
            foreach (DataGridViewColumn col in columnsExp)
                col.Visible = lstExpectedColumns.GetItemChecked(index++);
             
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
