using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            btnOK.DialogResult = DialogResult.OK;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            if (variables.deletefiles) chkfiles.Checked = true;
            if (String.IsNullOrEmpty(variables.IPend) || String.IsNullOrEmpty(variables.IPstart)) IP.initaddresses();
            txtIPEnd.Text = variables.IPend;
            txtIPStart.Text = variables.IPstart;
            txtfolder.Text = variables.outfolder;
            txtIP.Text = variables.ip;
            if (variables.ip.Length == 0) chkIpDefault.Checked = txtIP.Enabled = false;
            else chkIpDefault.Checked = txtIP.Enabled = true;
            sonusDelay.Value = variables.delay;
            AutoExtractcheckBox.Checked = variables.autoExtract;
            modderbut.Checked = variables.modder;
            chkPlaySuccess.Checked = variables.playSuccess;
            chkPlayError.Checked = variables.playError;
            chkAutoDelEcc.Checked = variables.autoDelEcc;
            timingOnKeypressEnable.Checked = variables.timingonkeypress;
            chkNoPatchWarnings.Checked = variables.noPatchWarnings;
            almovebut.Checked = !variables.allmove;
            if (variables.slimprefersrgh) SlimPreferSrgh.Checked = true;
            if (variables.LPTtiming) rbtnTimingLpt.Checked = true;
            txtTimingLptPort.Text = variables.LPTport;
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog openDialog = new CommonOpenFileDialog();
            openDialog.InitialDirectory = Oper.FilePickerInitialPath(txtfolder.Text);
            openDialog.RestoreDirectory = false;
            openDialog.IsFolderPicker = true;

            if (openDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string path;
                if (string.Equals(Path.GetFileName(openDialog.FileName), "output", StringComparison.OrdinalIgnoreCase)) path = openDialog.FileName;
                else path = Path.Combine(openDialog.FileName, "output");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                txtfolder.Text = path;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                variables.IPstart = txtIPStart.Text;
                variables.IPend = txtIPEnd.Text;
                variables.delay = (int)sonusDelay.Value;
                if (variables.debugMode) Console.WriteLine("outfolderchanged = true\noutfolder = {0}", variables.outfolder);
                variables.ip = txtIP.Text;
                variables.playSuccess = chkPlaySuccess.Checked;
                variables.playError = chkPlayError.Checked;
                variables.autoExtract = AutoExtractcheckBox.Checked;
                variables.modder = modderbut.Checked;
                variables.allmove = !almovebut.Checked;
                variables.autoDelEcc = chkAutoDelEcc.Checked;
                variables.LPTtiming = rbtnTimingLpt.Checked;
                if (!String.IsNullOrWhiteSpace(txtTimingLptPort.Text)) variables.LPTport = txtTimingLptPort.Text;
                else variables.LPTport = "378";

                if (txtfolder.Text == "")
                {
                    variables.outfolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "output");
                }
                else
                {
                    variables.outfolder = txtfolder.Text;
                }

                MainForm.mainForm.savesettings();
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (File.Exists(variables.settingsfile)) File.Delete(variables.settingsfile);
            if (DialogResult.Yes == MessageBox.Show("Settings will be reset when the application restarts\n\nDo you want to restart now?", "Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                Program.restart(); // Restart without running on exit tasks, prevents setting from being put back
            }
        }

        private void almovebut_CheckedChanged(object sender, EventArgs e)
        {
            variables.allmove = !almovebut.Checked;
        }

        private void modderbut_CheckedChanged(object sender, EventArgs e)
        {
            variables.modder = modderbut.Checked;
        }

        private void timingOnKeypressEnable_CheckedChanged(object sender, EventArgs e)
        {
            variables.timingonkeypress = timingOnKeypressEnable.Checked;
        }

        private void logDefault_Click(object sender, EventArgs e)
        {
            variables.logbackground = Color.Black;
            variables.logtext = Color.White;
            MainForm.mainForm.updateLogColor();
        }

        private void logBackgroundBlack_Click(object sender, EventArgs e)
        {
            variables.logbackground = Color.Black;
            MainForm.mainForm.updateLogColor();
        }

        private void logBackgroundBlue_Click(object sender, EventArgs e)
        {
            variables.logbackground = Color.FromArgb(1, 36, 86);
            MainForm.mainForm.updateLogColor();
        }

        private void logBackgroundCustom_Click(object sender, EventArgs e)
        {
            ColorDialog backgroundDlg = new ColorDialog();
            backgroundDlg.FullOpen = true;
            if (backgroundDlg.ShowDialog() == DialogResult.OK)
            {
                variables.logbackground = backgroundDlg.Color;
                MainForm.mainForm.updateLogColor();
            }
        }

        private void logTextWhite_Click(object sender, EventArgs e)
        {
            variables.logtext = Color.White;
            MainForm.mainForm.updateLogColor();
        }

        private void logTextBlack_Click(object sender, EventArgs e)
        {
            variables.logtext = Color.Black;
            MainForm.mainForm.updateLogColor();
        }

        private void logTextCustom_Click(object sender, EventArgs e)
        {
            ColorDialog textDlg = new ColorDialog();
            textDlg.FullOpen = true;
            if (textDlg.ShowDialog() == DialogResult.OK)
            {
                variables.logtext = textDlg.Color;
                MainForm.mainForm.updateLogColor();
            }
        }

        private void SlimPreferSrgh_CheckedChanged(object sender, EventArgs e)
        {
            variables.slimprefersrgh = SlimPreferSrgh.Checked;
        }

        private void chkIpDefault_CheckedChanged(object sender, EventArgs e)
        {
            txtIP.Enabled = chkIpDefault.Checked;
            if (!chkIpDefault.Checked) txtIP.Text = "";
        }

        private void timingRbtn_CheckedChanged(object sender, EventArgs e)
        {
            txtTimingLptPort.Enabled = rbtnTimingLpt.Checked;
        }

        private void txtTimingLptPort_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(txtTimingLptPort.Text, @"^\d+$"))
            {
                MessageBox.Show("Port is not valid", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTimingLptPort.Text = variables.LPTport;
            }
        }

        private void chkNoPatchWarnings_CheckedChanged(object sender, EventArgs e)
        {
            variables.noPatchWarnings = chkNoPatchWarnings.Checked;
        }

        private void chkNoPatchWarnings_Click(object sender, EventArgs e)
        {
            if (chkNoPatchWarnings.Checked) MessageBox.Show("Warnings or messages about patches will not be displayed as pop-ups\n\nConsole log messages will continue to show", "Steep Hill Ahead", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
