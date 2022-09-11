using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace JRunner
{
    public partial class RestoreFiles : Form
    {
        private readonly CheckBox resetSettings;

        public RestoreFiles()
        {
            InitializeComponent();
            resetSettings = new CheckBox { Checked = true, Text = "Reset Settings" };
            RestoreWizard.AddCommandControl(resetSettings);

            RestoreWizard.Cancelling += WizardCancelled;
            RestoreWizard.Finished += WizardFinished;
        }

        private void WizardCancelled(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WizardFinished(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure that you want to restore files?\n\nAll files inside common and xeBuild will be deleted and replaced with clean versions!", "Steep Hill Ahead", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                this.Close();
            }
            else
            {
                this.Enabled = false;

                if (resetSettings.Checked)
                {
                    if (File.Exists(variables.settingsfile)) File.Delete(variables.settingsfile);
                }

                Upd.restoreFiles();
                this.Close();
            }
        }
    }
}
