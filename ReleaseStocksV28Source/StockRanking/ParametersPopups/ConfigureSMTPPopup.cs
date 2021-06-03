using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace StockRanking
{
    public partial class ConfigureSMTPPopup : Form
    {
        public ConfigureSMTPPopup()
        {
            InitializeComponent();

            var mailConfig = MailConfiguration.Load();
            if (mailConfig != null)
            {
                txtFrom.Text = mailConfig.From;
                txtPassword.Text = mailConfig.Password;
                txtPort.Text = mailConfig.Port.ToString();
                txtSmtp.Text = mailConfig.Smtp;
                txtUsername.Text = mailConfig.Username;
            }
        }

        private void cmdAccept_Click(object sender, EventArgs e)
        {
            if (!validate())
                return;

            var config = loadMailConfig();
            if (config == null)
                return;

            config.Save();

            this.Close();
        }

        private bool validate()
        {
            var port = 0;
            try
            {
                port = Convert.ToInt32(txtPort.Text);
            }    
            catch(Exception e)
            {
                port = 0;
            }

            if (port == 0)
            {
                MessageBox.Show("Please type a valid Port parameter.");
                return false;
            }

            if(txtFrom.Text.Trim() == "" ||
                txtPassword.Text.Trim() == "" ||
                txtSmtp.Text.Trim() == "" ||
                txtUsername.Text.Trim() == "")
            {
                MessageBox.Show("Please fill all fields.");
                return false;
            }

            return true;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdTestMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            String result = Interaction.InputBox("Destination for Sample Email: ", "Email Configuration", txtFrom.Text);
            if (result != "")
            {
                var mailConf = loadMailConfig();
                if (mailConf == null)
                    return;

                String mailError = MailingGenerator.SendTestMail(mailConf, result);

                if(mailError == "")
                    MessageBox.Show("Mail Sent, please check your inbox and spam folders.");
                else
                    MessageBox.Show("There was an error sending the email, please check your configuration. " + mailError);
            }
        }

        MailConfiguration loadMailConfig()
        {
            if (!validate())
                return null;

            MailConfiguration config = new MailConfiguration();
            config.From = txtFrom.Text;
            config.Port = Convert.ToInt32(txtPort.Text);
            config.Username = txtUsername.Text;
            config.Password = txtPassword.Text;
            config.Smtp = txtSmtp.Text;

            return config;
        }
    }
}
