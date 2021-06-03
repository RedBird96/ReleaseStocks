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
    public partial class SelectETFSymbolPopup : Form
    {
        List<ETFStockInfo> etfStocks = new List<ETFStockInfo>();
        public List<ETFStockInfo> SelectedETFs = new List<ETFStockInfo>();

        public SelectETFSymbolPopup()
        {
            InitializeComponent();
            etfStocks = ETFStockInfo.GetAvailableETFSymbols();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            filterResults(txtFilter.Text.Trim());
        }

        void filterResults(String text)
        {
            lstETFSymbols.Items.Clear();

            if (text.Length < 2)
                return;

            foreach(var etf in etfStocks.Where(x => x.Symbol.ToLower().Contains(text.ToLower()) || x.CompanyName.ToLower().Contains(text.ToLower())).OrderBy(x => x.Symbol))
            {
                var item = lstETFSymbols.Items.Add(etf.Symbol);
                item.Tag = etf;
                item.SubItems.Add(etf.CompanyName);
                item.SubItems.Add(etf.Category);
                item.SubItems.Add(etf.Exchange);
                item.SubItems.Add(Utils.ConvertIntToDateTime(etf.FirstDate).ToString("MM/dd/yyyy"));
                item.SubItems.Add(Utils.ConvertIntToDateTime(etf.LastDate).ToString("MM/dd/yyyy"));
            }
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            if(lstETFSymbols.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one ETF to add.");
                return;
            }

            foreach(ListViewItem item in lstETFSymbols.SelectedItems)
            {
                ((ETFStockInfo)item.Tag).SaveAsNewStock();
            }

            this.DialogResult = DialogResult.OK;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
