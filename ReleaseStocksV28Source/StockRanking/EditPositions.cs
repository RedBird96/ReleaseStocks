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
    public partial class EditPositions : Form
    {
        List<Stock> stocks = new List<Stock>();
        Stock selectedStock = null;
        public List<Position> CurrentPortfolio = new List<Position>();

        public EditPositions(List<Position> portfolio)
        {
            InitializeComponent();

            CurrentPortfolio = portfolio.ToList();
            this.DialogResult = DialogResult.Cancel;

            stocks = Stock.GetCurrentStockNames();


            foreach (Position newPos in portfolio)
            {
                ListViewItem item = new ListViewItem(newPos.AuxSymbol);
                item.SubItems.Add(Utils.ConvertIntToDateTime(newPos.DateEntered).ToShortDateString());
                item.SubItems.Add(newPos.Shares.ToString());
                item.SubItems.Add(newPos.EntryPrice.ToString("n2"));
                item.SubItems.Add(newPos.AuxName);
                item.Tag = newPos;

                lstPositions.Items.Add(item);
            }
        }

        private void cmdRemove_Click(object sender, EventArgs e)
        {
            if (lstPositions.SelectedItems.Count == 0)
                return;

            lstPositions.Items.Remove(lstPositions.SelectedItems[0]);
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            List<Position> positions = new List<Position>();
            foreach(ListViewItem item in lstPositions.Items)
            {
                positions.Add((Position)item.Tag);
            }

            PortfolioParameters.SaveCurrentPortfolio(positions);

            CurrentPortfolio = positions;

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {

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

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            if(selectedStock == null)
            {
                MessageBox.Show("Please select a stock");
                return;
            }

            if(Utils.ConvertToDouble(txtPrice.Text) <= 0)
            {
                MessageBox.Show("Please enter a valid entry price");
                return;
            }

            if (Utils.ConvertToDouble(txtShares.Text) <= 0)
            {
                MessageBox.Show("Please enter a valid shares quantity");
                return;
            }

            if (Utils.ConvertDateTimeToInt(DateTime.Now) < Utils.ConvertDateTimeToInt(dateEntry.Value))
            {
                MessageBox.Show("Please enter a valid entry date");
                return;
            }

            Position newPos = new Position(selectedStock.Id, (int)Utils.ConvertToDouble(txtShares.Text), Utils.ConvertDateTimeToInt(dateEntry.Value), (decimal)Utils.ConvertToDouble(txtPrice.Text), 0, txtTicker.Text, txtCompanyName.Text, "");
            ListViewItem item = new ListViewItem(newPos.AuxSymbol.ToUpper());
            item.SubItems.Add(dateEntry.Value.ToShortDateString());
            item.SubItems.Add(newPos.Shares.ToString());
            item.SubItems.Add(newPos.EntryPrice.ToString("n2"));
            item.SubItems.Add(txtCompanyName.Text);
            item.Tag = newPos;

            lstPositions.Items.Add(item);
                
        }
    }
}
