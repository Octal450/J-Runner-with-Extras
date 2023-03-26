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
        private static string oldOutFolder;

        public Settings()
        {
            InitializeComponent();
            btnOK.DialogResult = DialogResult.OK;
        }

        public void setTab(string tab)
        {
            if (tab == "backup")
            {
                tabCSettings.SelectedTab = tabBackup;
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            oldOutFolder = variables.outfolder;
            if (variables.deletefiles) chkfiles.Checked = true;
            txtIP.Text = variables.ipPrefix;

            if (variables.ipPrefix.Length == 0)
            {
                chkIPDefault.Checked = txtIP.Enabled = false;
                txtIP.Text = "Automatic";
            }
            else chkIPDefault.Checked = txtIP.Enabled = true;

            chkAutoExtract.Checked = variables.autoExtract;
            chkPlaySuccess.Checked = variables.playSuccess;
            chkPlayError.Checked = variables.playError;
            chkAutoDelXeLL.Checked = variables.autoDelXeLL;
            chkNoPatchWarnings.Checked = variables.noPatchWarnings;
            chkAllMove.Checked = !variables.allmove;
            chkAllowZeroPaired.Checked = variables.allowZeroPaired;

            if (variables.LPTtiming) rbtnTimingLpt.Checked = true;
            txtTimingLptPort.Text = variables.LPTport;

            if (!string.IsNullOrWhiteSpace(variables.overrideRootPath))
            {
                txtRootOverride.Text = variables.overrideRootPath;
                chkRootOverride.Checked = true;
            }

            chkBackupEn.Checked = variables.backupEn; // Will enable group boxes
            txtBackupRoot.Text = variables.backupRoot;
            if (variables.backupType == 1) rbtnFolder.Checked = true;
            if (variables.backupNaming == 1) rbtnCtypeSnDate.Checked = true;
            else if (variables.backupNaming == 2) rbtnSnOnly.Checked = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            bool ok = true;

            // Check paths before applying anything

            if (chkRootOverride.Checked && !string.IsNullOrWhiteSpace(txtRootOverride.Text))
            {
                try
                {
                    string overridePath = Path.GetFullPath(txtRootOverride.Text);
                }
                catch
                {
                    ok = false;
                    MessageBox.Show("Illegal path to output folder", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (chkBackupEn.Checked && !string.IsNullOrWhiteSpace(txtBackupRoot.Text))
            {
                string backupRootPath = "";
                try
                {
                    backupRootPath = Path.GetFullPath(txtBackupRoot.Text);
                }
                catch
                {
                    ok = false;
                    MessageBox.Show("Illegal path to backup root folder", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (backupRootPath.ToLower().Contains(variables.rootfolder.ToLower()))
                {
                    ok = false;
                    MessageBox.Show("You can't backup to a folder that is inside the application root folder", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBackupRoot.Text = "";
                }
            }

            if (!ok) // Abort and do not save settings
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            try
            {
                if (chkIPDefault.Checked)
                {
                    variables.ipPrefix = txtIP.Text;
                }
                else variables.ipPrefix = "";
                MainForm.mainForm.setIP();
                IP.initaddresses();

                variables.noPatchWarnings = chkNoPatchWarnings.Checked;
                variables.playSuccess = chkPlaySuccess.Checked;
                variables.playError = chkPlayError.Checked;
                variables.autoExtract = chkAutoExtract.Checked;
                variables.allmove = !chkAllMove.Checked;
                variables.autoDelXeLL = chkAutoDelXeLL.Checked;
                variables.LPTtiming = rbtnTimingLpt.Checked;
                variables.allowZeroPaired = chkAllowZeroPaired.Checked;

                if (!string.IsNullOrWhiteSpace(txtTimingLptPort.Text)) variables.LPTport = txtTimingLptPort.Text;
                else variables.LPTport = "378";

                if (!chkRootOverride.Checked || string.IsNullOrWhiteSpace(txtRootOverride.Text))
                {
                    variables.overrideRootPath = "";
                    variables.outfolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "output");
                }
                else
                {
                    string overridePath = Path.GetFullPath(txtRootOverride.Text);
                    variables.overrideRootPath = overridePath;
                    variables.outfolder = Path.Combine(overridePath, "output");
                }

                if (string.IsNullOrWhiteSpace(txtBackupRoot.Text))
                {
                    variables.backupEn = false;
                    variables.backupRoot = "";
                }
                else
                {
                    variables.backupEn = chkBackupEn.Checked;
                    variables.backupRoot = Path.GetFullPath(txtBackupRoot.Text);
                }

                if (rbtnFolder.Checked) variables.backupType = 1;
                else variables.backupType = 0;

                if (rbtnCtypeSnDate.Checked) variables.backupNaming = 1;
                else if (rbtnSnOnly.Checked) variables.backupNaming = 2;
                else variables.backupNaming = 0;

                MainForm.mainForm.setBackupLabel();
                MainForm.mainForm.savesettings();
            }
            catch (Exception ex)
            {
                if (variables.debugMode) Console.WriteLine(ex.ToString());
                MessageBox.Show("A critical error has occured while trying to apply settings\n\nThe application is now in an invalid state and needs to restart", "Critical", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Program.restart(); // Restart without running on exit tasks, so that we do not save settings
            }

            if (oldOutFolder != variables.outfolder)
            {
                MessageBox.Show("Application must be restarted in order to change the folder paths", "Change Folders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }

            this.Close();
        }

        #region General

        private void chkRootOverride_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRootOverride.Checked)
            {
                txtRootOverride.Enabled = true;
                btnRootOverride.Enabled = true;
            }
            else
            {
                txtRootOverride.Enabled = false;
                txtRootOverride.Text = "";
                btnRootOverride.Enabled = false;
            }
        }

        private void btnRootOverride_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog openDialog = new CommonOpenFileDialog();
            openDialog.InitialDirectory = Oper.FilePickerInitialPath(txtRootOverride.Text);
            openDialog.RestoreDirectory = false;
            openDialog.IsFolderPicker = true;

            if (openDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                txtRootOverride.Text = openDialog.FileName;
                string pathOutput = Path.Combine(openDialog.FileName, "output");
                if (!Directory.Exists(pathOutput))
                {
                    Directory.CreateDirectory(pathOutput);
                }
            }
        }

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Application must be restarted in order to restore settings to defaults\n\nDo you want to reset and restart now?", "Restore Defaults", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                if (File.Exists(variables.settingsfile)) File.Delete(variables.settingsfile);
                Program.restart(); // Restart without running on exit tasks, prevents settings from being put back
            }
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

        private void chkIPDefault_CheckedChanged(object sender, EventArgs e)
        {
            txtIP.Enabled = chkIPDefault.Checked;
            if (!chkIPDefault.Checked) txtIP.Text = "Automatic";
            else if (txtIP.Text == "Automatic")
            {
                string localIP = IP.getGatewayIp();
                txtIP.Text = localIP.Remove(localIP.LastIndexOf('.'));
            }
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

        private void chkNoPatchWarnings_Click(object sender, EventArgs e)
        {
            if (chkNoPatchWarnings.Checked) MessageBox.Show("Warnings or messages about patches will not be displayed as pop-ups\n\nConsole log messages will continue to show", "Steep Hill Ahead", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        #endregion

        #region Backup

        private void chkBackupEn_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBackupEn.Checked)
            {
                txtBackupRoot.Enabled = true;
                btnBackupRoot.Enabled = true;
                groupBackupType.Enabled = true;
            }
            else
            {
                txtBackupRoot.Enabled = false;
                txtBackupRoot.Text = "";
                btnBackupRoot.Enabled = false;
                groupBackupType.Enabled = false;
            }
        }

        private void btnBackupRoot_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog openDialog = new CommonOpenFileDialog();
            openDialog.InitialDirectory = Oper.FilePickerInitialPath(txtRootOverride.Text);
            openDialog.RestoreDirectory = false;
            openDialog.IsFolderPicker = true;

            if (openDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (openDialog.FileName.ToLower().Contains(variables.rootfolder.ToLower()))
                {
                    MessageBox.Show("You can't backup to a folder that is inside the application root folder", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBackupRoot.Text = "";
                }
                else txtBackupRoot.Text = openDialog.FileName;
            }
        }

        #endregion
    }
}
