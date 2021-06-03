using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Security.Cryptography;



namespace StockRanking
{
    public partial class frmLicense : Form
    {
        public bool isPassed = false;

        public frmLicense()
        {
            this.DialogResult = DialogResult.Cancel;

            InitializeComponent();

            Security sec = new Security();

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\license.dat"))
            {

                var text = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\license.dat");
                if (text.Length > 0 && sec.checkActivation(text))
                {
                    isPassed = true;
                }
            }

            txtRequest.Text = sec.getRequestCode();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
           
        private void btnActivate_Click(object sender, EventArgs e)
        {
            Security sec = new Security();
             
            if (sec.checkActivation(txtActivate.Text))
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\license.dat", txtActivate.Text);
                MessageBox.Show("Activation completed.");
                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
            else
            {
                MessageBox.Show("Activation failed, please check the request and activation codes.");
            }
        }
    }
}
