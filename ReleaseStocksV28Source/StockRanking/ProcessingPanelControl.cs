using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockRanking
{
    public partial class ProcessingPanelControl : UserControl
    {
        public event EventHandler CancelProcess;
        public bool IsShowingError = false;

        public ProcessingPanelControl()
        {
            InitializeComponent();
        }

        private void cmdErrorDetails_Click(object sender, EventArgs e)
        {
            txtErrorDetails.Visible = true;
        }

        private void cmdCopyError_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtErrorDetails.Text);
            MessageBox.Show("Error information copied to the Clipboard.");
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            StopProcess();
            IsShowingError = false;
        }

        private void cmdStopUpdate_Click(object sender, EventArgs e)
        {
            if (CancelProcess != null)
                CancelProcess(this, new EventArgs());
        }

        public void StopProcess()
        {
            pnlProcessing.Visible = false;
            cmdStopUpdate.Visible = false;
            this.Visible = false;
        }

        public void ShowError(String errorText)
        {
            txtErrorDetails.Text = errorText;
            txtErrorDetails.Visible = false;
            cmdErrorDetails.Visible = true;
            cmdCopyError.Visible = true;
            cmdClose.Visible = true;
            IsShowingError = true;
        }

        public void StartProcess()
        {
            IsShowingError = false;
            txtErrorDetails.Visible = false;
            cmdErrorDetails.Visible = false;
            cmdCopyError.Visible = false;
            cmdClose.Visible = false;
            cmdStopUpdate.Visible = true;
            pnlProcessing.Visible = true;
            pnlProcessing.Dock = DockStyle.Fill;
            this.Dock = DockStyle.Fill;
            this.Visible = true;

            cmdStopUpdate.Focus();
        }

        public void SetTitle(String title)
        {
            lblProcessStep.Text = title;
            cmdStopUpdate.Focus();
        }

        public void PerformStep()
        {
            progressProcessing.PerformStep();
            Application.DoEvents();
            cmdStopUpdate.Focus();
        }

        public void SetCommand(String cmdText)
        {
            cmdStopUpdate.Text = cmdText;
        }

        public void SetMaxValue(int maxVal)
        {
            progressProcessing.Value = 0;
            progressProcessing.Step = 1;
            progressProcessing.Maximum = maxVal;
        }


        private void pnlProcessing_Resize(object sender, EventArgs e)
        {
            try
            {
                pnlProgressCenter.Left = pnlProcessing.Width / 2 - pnlProgressCenter.Width / 2;
                pnlProgressCenter.Top = pnlProcessing.Height / 2 - pnlProgressCenter.Height / 2;
            }
            catch (Exception)
            {

            }
        }

        private void cmdStopUpdate_Leave(object sender, EventArgs e)
        {
            cmdStopUpdate.Focus();
        }
    }
}
