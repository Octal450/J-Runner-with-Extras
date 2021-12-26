using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace JRunner.Forms
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            this.AcceptButton = btnOK;
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
            txtsuccom.Text = variables.soundcompare;
            txtsuccess.Text = variables.soundsuccess;
            txterror.Text = variables.sounderror;
            numericUpDown1.Value = variables.delay;
            txtNandflash.Text = variables.nandflash;

            AutoExtractcheckBox.Checked = variables.autoExtract;
            modderbut.Checked = variables.modder;
            discordRPCEnable.Checked = variables.discordrpc;
            timingOnKeypressEnable.Checked = variables.timingonkeypress;
            minimizeToSystemTray.Checked = variables.minimizetotray;
            almovebut.Checked = !variables.allmove;
            //if (MainForm.mainForm.device == 4 && variables.boardtype == null) buttonDeviceGroup.Visible = true;
            if (variables.soundcompare != "") chksuccom.Checked = true;
            if (variables.sounderror != "") chkerror.Checked = true;
            if (variables.soundsuccess != "") chksuccess.Checked = true;
            if (variables.slimprefersrgh) SlimPreferSrgh.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "(*.wav)|*.wav";
            openFileDialog1.Title = "Select a File";
            //openFileDialog1.InitialDirectory = variables.currentdir;
            openFileDialog1.RestoreDirectory = false;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtsuccom.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "(*.wav)|*.wav";
            openFileDialog1.Title = "Select a File";
            //openFileDialog1.InitialDirectory = variables.currentdir;
            openFileDialog1.RestoreDirectory = false;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtsuccess.Text = openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "(*.wav)|*.wav";
            openFileDialog1.Title = "Select a File";
            //openFileDialog1.InitialDirectory = variables.currentdir;
            openFileDialog1.RestoreDirectory = false;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txterror.Text = openFileDialog1.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.ShowNewFolderButton = true;
            this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;



            DialogResult outres = folderBrowserDialog1.ShowDialog();
            if (outres == DialogResult.OK)
            {
                string path;
                if (string.Equals(Path.GetFileName(folderBrowserDialog1.SelectedPath), "output", StringComparison.OrdinalIgnoreCase)) path = folderBrowserDialog1.SelectedPath;
                else path = Path.Combine(folderBrowserDialog1.SelectedPath, "output");
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
                if (!chksuccess.Checked) txtsuccess.Text = "";
                if (!chksuccom.Checked) txtsuccom.Text = "";
                if (!chkerror.Checked) txterror.Text = "";
                variables.soundcompare = txtsuccom.Text;
                variables.sounderror = txterror.Text;
                variables.soundsuccess = txtsuccess.Text;
                variables.IPstart = txtIPStart.Text;
                variables.IPend = txtIPEnd.Text;
                variables.delay = (int)numericUpDown1.Value;
                if (variables.debugme) Console.WriteLine("outfolderchanged = true\noutfolder = {0}", variables.outfolder);
                variables.ip = txtIP.Text;

                variables.nandflash = txtNandflash.Text;
                variables.autoExtract = AutoExtractcheckBox.Checked;
                variables.modder = modderbut.Checked;
                variables.allmove = !almovebut.Checked;

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
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void almovebut_CheckedChanged(object sender, EventArgs e)
        {
            variables.allmove = !almovebut.Checked;
        }

        private void modderbut_CheckedChanged(object sender, EventArgs e)
        {
            variables.modder = modderbut.Checked;
        }

        private void discordRPCEnable_CheckedChanged(object sender, EventArgs e)
        {
            variables.discordrpc = discordRPCEnable.Checked;
        }

        private void discordRPCEnable_Click(object sender, EventArgs e)
        {
            DialogResult d;
            if (variables.discordrpc)
            {
                d = MessageBox.Show("J-Runner must be restarted to enable DiscordRPC!" + Environment.NewLine + Environment.NewLine + "Do you want to restart J-Runner now?", "DiscordRPC", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else
            {
                d = MessageBox.Show("J-Runner must be restarted to disable DiscordRPC!" + Environment.NewLine + Environment.NewLine + "Do you want to restart J-Runner now?", "DiscordRPC", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            if (d == DialogResult.Yes)
            {
                Application.Restart();
            }
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

        private void minimizeToSystemTray_CheckedChanged(object sender, EventArgs e)
        {
            variables.minimizetotray = minimizeToSystemTray.Checked;
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
    }
}
