using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class OpenXenium : Form
    {
        public OpenXenium()
        {
            InitializeComponent();
            OpenXeniumWizard.Cancelling += WizardCancelled;
            OpenXeniumWizard.Finished += WizardFinished;
            OpenXeniumPage.Commit += OpenXenium_Next;
        }

        private void WizardCancelled(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WizardFinished(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OpenXenium_Next(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            if (MainForm.mainForm.device == MainForm.DEVICE.XFLASHER_SPI)
            {
                MainForm.mainForm.xflasher.flashSvf(variables.rootfolder + @"\common\svf\openxenium.svf");
                this.Close();
            }
            else if (MainForm.mainForm.device == MainForm.DEVICE.XFLASHER_EMMC)
            {
                e.Cancel = true;
                MessageBox.Show("Unable to program OpenXenium in eMMC mode\n\nPlease switch to SPI mode", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                e.Cancel = true;
                MessageBox.Show("Please connect an xFlasher in SPI Mode!", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
