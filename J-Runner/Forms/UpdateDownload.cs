using System;
using System.Net;
using System.Windows.Forms;

namespace JRunner
{
    public partial class UpdateDownload : Form
    {
        public UpdateDownload()
        {
            InitializeComponent();
            UpdateWizard.Cancelling += WizardCancelled;
            UpdateWizard.Finished += WizardFinished;
        }

        private void WizardCancelled(object sender, EventArgs e)
        {
            Upd.cancel();
        }

        private void WizardFinished(object sender, EventArgs e)
        {
            Program.restart();
        }

        public void updateProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            updateProgressBar.BeginInvoke((Action)(() => updateProgressBar.Value = e.ProgressPercentage));
        }

        public void installMode()
        {
            updateProgressBar.BeginInvoke((Action)(() => updateProgressBar.Style = ProgressBarStyle.Marquee));
            UpdatePage.Text = "Installing Update";
            UpdatePage.AllowCancel = false;
        }
    }
}
