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

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
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
            chkUnused2.Checked = variables.timingonkeypress;
            chkNoPatchWarnings.Checked = variables.noPatchWarnings;
            chkAllMove.Checked = !variables.allmove;
            if (variables.slimprefersrgh) SlimPreferSrgh.Checked = true;
            if (variables.LPTtiming) rbtnTimingLpt.Checked = true;
            txtTimingLptPort.Text = variables.LPTport;

            if (!string.IsNullOrWhiteSpace(variables.overrideRootPath))
            {
                txtRootOverride.Text = variables.overrideRootPath;
                chkRootOverride.Checked = true;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            bool ok = true;

            try
            {
                if (chkIPDefault.Checked)
                {
                    variables.ipPrefix = txtIP.Text;
                }
                else variables.ipPrefix = "";
                MainForm.mainForm.setIP();
                IP.initaddresses();

                variables.playSuccess = chkPlaySuccess.Checked;
                variables.playError = chkPlayError.Checked;
                variables.autoExtract = chkAutoExtract.Checked;
                variables.allmove = !chkAllMove.Checked;
                variables.autoDelXeLL = chkAutoDelXeLL.Checked;
                variables.LPTtiming = rbtnTimingLpt.Checked;
                if (!string.IsNullOrWhiteSpace(txtTimingLptPort.Text)) variables.LPTport = txtTimingLptPort.Text;
                else variables.LPTport = "378";

                if (!chkRootOverride.Checked || string.IsNullOrWhiteSpace(txtRootOverride.Text))
                {
                    variables.overrideRootPath = "";
                    variables.outfolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "output");
                }
                else
                {
                    try
                    {
                        string overridePath = Path.GetFullPath(txtRootOverride.Text);
                        variables.overrideRootPath = overridePath;
                        variables.outfolder = Path.Combine(overridePath, "output");
                    }
                    catch
                    {
                        ok = false;
                        MessageBox.Show("Illegal path to output folder", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (ok) MainForm.mainForm.savesettings();
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

            if (ok) this.Close();
            else this.DialogResult = DialogResult.None;
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
            if (File.Exists(variables.settingsfile)) File.Delete(variables.settingsfile);
            if (DialogResult.Yes == MessageBox.Show("Application must be restarted in order to restore settings to defaults\n\nDo you want to restart now?", "Restore Defaults", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                Program.restart(); // Restart without running on exit tasks, prevents settings from being put back
            }
        }

        private void chkAllMove_CheckedChanged(object sender, EventArgs e)
        {
            variables.allmove = !chkAllMove.Checked;
        }

        private void timingOnKeypressEnable_CheckedChanged(object sender, EventArgs e)
        {
            variables.timingonkeypress = chkUnused2.Checked;
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

        private void chkNoPatchWarnings_CheckedChanged(object sender, EventArgs e)
        {
            variables.noPatchWarnings = chkNoPatchWarnings.Checked;
        }

        private void chkNoPatchWarnings_Click(object sender, EventArgs e)
        {
            if (chkNoPatchWarnings.Checked) MessageBox.Show("Warnings or messages about patches will not be displayed as pop-ups\n\nConsole log messages will continue to show", "Steep Hill Ahead", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

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
    }
}
