using System;
using System.Windows.Forms;

namespace JRunner
{
    public partial class UpdateAvailable : Form
    {
        public UpdateAvailable()
        {
            InitializeComponent();
            SuccessWizard.Cancelling += WizardCancelled;
            SuccessWizard.Finished += WizardFinished;
        }

        private void UpdateSuccess_Load(object sender, EventArgs e)
        {
            // Make sure we're on top
            bool top = TopMost;
            TopMost = true; // Bring to front
            TopMost = top; // Set it back
            Activate();
        }

        private void WizardCancelled(object sender, EventArgs e) // No
        {
            Upd.allowUpdate = false;
            Upd.cancelSource.Cancel();
        }

        private void WizardFinished(object sender, EventArgs e) // Yes
        {
            Upd.allowUpdate = true;
            Upd.cancelSource.Cancel();
            this.Close();
        }
    }
}
