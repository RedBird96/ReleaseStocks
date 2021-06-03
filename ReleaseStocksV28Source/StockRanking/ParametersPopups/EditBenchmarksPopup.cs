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
    public partial class EditBenchmarksPopup : Form
    {
        List<CachedValuesStock> allStocks;

        public EditBenchmarksPopup()
        {
            InitializeComponent();

            allStocks = new List<CachedValuesStock>();
            var fileBasedStocks = FileBasedStock.GetAllFileBasedStocks();
            var etfStocks = Stock.GetAllETFStocks();
            var defaultEtfStocks = Stock.GetDefaultETFStocks();

            allStocks.AddRange(fileBasedStocks);
            allStocks.AddRange(etfStocks);
            allStocks.AddRange(defaultEtfStocks);

            //load stocks into the lists
            lstAll.Items.Clear();
            lstSelected.Items.Clear();
            var selected = PortfolioParameters.GetPerformanceGraphBenchmarkSymbols();
            foreach (var stock in allStocks)
            {
                if (selected.Any(x => x == stock.StockSymbol))
                    lstSelected.Items.Add(stock.StockSymbol);
                else
                    lstAll.Items.Add(stock.StockSymbol);
            }
        }

        private void cmdRemove_Click(object sender, EventArgs e)
        {
            if (lstSelected.SelectedItems.Count == 0)
                return;

            List<ListViewItem> items = new List<ListViewItem>();
            foreach (ListViewItem item in lstSelected.SelectedItems)
                items.Add(item);

            foreach (ListViewItem item in items)
            {
                lstSelected.Items.Remove(item);
                lstAll.Items.Add(item);
            }
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            if (lstAll.SelectedItems.Count == 0)
                return;

            List<ListViewItem> items = new List<ListViewItem>();
            foreach (ListViewItem item in lstAll.SelectedItems)
                items.Add(item);

            foreach (ListViewItem item in items)
            {
                lstAll.Items.Remove(item);
                lstSelected.Items.Add(item);
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            var list = new List<String>();
            foreach(ListViewItem item in lstSelected.Items)
            {
                list.Add(item.Text);
            }

            PortfolioParameters.SavePerformanceGraphBenchmarkSymbols(list.ToArray());

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
