using System;
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
            System.Diagnostics.Process.Start("https://github.com/Octal450/J-Runner-with-Extras/releases/latest");
        }
    }
}
