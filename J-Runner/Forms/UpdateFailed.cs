using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JRunner
{
    public partial class UpdateFailed : Form
    {
        public UpdateFailed()
        {
            InitializeComponent();
            FailedWizard.Cancelling += WizardCancelled;
            FailedWizard.Finished += WizardFinished;
        }

        private void UpdateFailed_Load(object sender, EventArgs e)
        {
            FailedReason.Text = Upd.failedReason;
        }

        private void WizardCancelled(object sender, EventArgs e)
        {
            Application.ExitThread();
            Application.Exit();
        }

        private void WizardFinished(object sender, EventArgs e)
        {
            Application.ExitThread();
            Application.Exit();
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://cdn.octalsconsoleshop.com/J-Runner%20with%20Extras.zip");
        }
    }
}
