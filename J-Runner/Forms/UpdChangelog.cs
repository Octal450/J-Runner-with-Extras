using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

namespace JRunner
{
    public partial class UpdChangelog : Form
    {
        public UpdChangelog()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.UpdChangelog_FormClosing);
            UpdateWizard.Cancelling += WizardCancelled;
            UpdateWizard.Finished += WizardFinished;
        }

        private void UpdChangelog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
            Application.Exit();
        }

        private void WizardCancelled(object sender, EventArgs e)
        {
            this.FormClosing -= new FormClosingEventHandler(this.UpdChangelog_FormClosing);

            MainForm.mainForm.startMainForm(true);
            Upd.changelog = ""; // Clear it
            this.Close();
        }

        private void WizardFinished(object sender, EventArgs e)
        {
            this.FormClosing -= new FormClosingEventHandler(this.UpdChangelog_FormClosing);

            if (variables.revision >= Upd.minDeltaRevision) // Delta
            {
                Upd.startDelta();
            }
            else // Full
            {
                Upd.startFull();
            }

            Upd.changelog = ""; // Clear it
            this.Close();
        }

        public void showChangelog(string changelog)
        {
            txtChangeLog.Text = changelog.Replace("\n", "\r\n").Trim();
        }
    }
}
