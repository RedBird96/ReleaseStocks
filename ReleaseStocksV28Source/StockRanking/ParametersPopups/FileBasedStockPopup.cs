using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockRanking
{
    public partial class FileBasedStockPopup : Form
    {
        private FileBasedStock stock = null;

        public FileBasedStockPopup(FileBasedStock stockToEdit)
        {
            InitializeComponent();

            if(stockToEdit != null)
            {
                txtFBSymbol.ReadOnly = true;
                txtFBName.Text = stockToEdit.Stock.CompanyName;
                txtFBSymbol.Text = stockToEdit.Stock.Symbol;
                txtFBFile.Text = stockToEdit.Path;
                chkFBKeppUpdated.Checked = stockToEdit.KeepUpdated;
            }

            stock = stockToEdit;
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            //validate values and save symbol
            var stockToSave = stock;

            if(!FileBasedStock.CheckFileFormat(txtFBFile.Text))
            {
                MessageBox.Show("File with values not found or incorrect format. Please check the file format and try again.");
                return;
            }

            var stockId = Stock.GetStockIdFromTicker(txtFBSymbol.Text.Trim());
            if (stockToSave == null && stockId != 0)
            {
                MessageBox.Show("Symbol already used by " + Stock.GetStock(stockId).CompanyName + ". Please type a different symbol.");
                return;
            }

            if(stockToSave == null)
            {
                //create new stock
                stockToSave = new FileBasedStock();
                stockToSave.Stock = new Stock();
                stockToSave.Stock.Symbol = txtFBSymbol.Text.Trim();
            }

            stockToSave.Path = txtFBFile.Text;
            stockToSave.KeepUpdated = chkFBKeppUpdated.Checked;
            stockToSave.Stock.CompanyName = txtFBName.Text;

            stockToSave.Save();

            if (!FileBasedStock.ImportFileData(stockToSave))
            {
                MessageBox.Show("There was an error importing the data file, please check the file and try again.");
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdFBSelectFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtFBFile.Text.Trim() != "")
                if (File.Exists(txtFBFile.Text))
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(txtFBFile.Text);

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            txtFBFile.Text = openFileDialog.FileName;
        }
    }
}
