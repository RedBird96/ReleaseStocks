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
    public partial class SelectStockPopUp : Form
    {
        List<Stock> stocks = new List<Stock>();
        public Stock selectedStock = null;

        public SelectStockPopUp()
        {
            InitializeComponent();

            stocks = Stock.GetCurrentStockNames();
        }

        private void cmdAccept_Click(object sender, EventArgs e)
        {
            if (this.selectedStock == null)
            {
                MessageBox.Show("Please select a Stock Symbol");
                return;
            }

            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.selectedStock = null;
            this.Close();
        }


        private void txtTicker_TextChanged(object sender, EventArgs e)
        {
            //show stocks
            lstSymbols.Items.Clear();
            int count = 0;
            String textSearch = txtTicker.Text.Trim().ToUpper();
            selectedStock = null;
            txtCompanyName.Text = "";

            foreach (Stock st in stocks)
            {
                if (st.Symbol == textSearch)
                {
                    txtCompanyName.Text = st.CompanyName;
                    selectedStock = st;
                }

                if (count < 50 && st.Symbol.StartsWith(textSearch))
                {
                    lstSymbols.Items.Add(st.Symbol.PadRight(6, ' ') + st.CompanyName);
                    count++;
                }
            }

        }

        private void txtTicker_Enter(object sender, EventArgs e)
        {
            lstSymbols.Visible = true;
        }

        private void txtTicker_Leave(object sender, EventArgs e)
        {
            lstSymbols.Visible = false;

        }

    }
}
