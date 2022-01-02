using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class CreateDonorNand : Form
    {
        public CreateDonorNand()
        {
            InitializeComponent();
            this.FormClosing += FormClose;
            DonorWizard.Cancelling += WizardCancelled;
            DonorWizard.Finished += WizardFinished;
            PrereqPage.Commit += PrereqNext;
            CpuKvPage.Rollback += CpuKvBack;
            CpuKvPage.Commit += CpuKvNext;
            FcrtPage.Commit += FcrtNext;
            LdvSmcConfPage.Commit += LdvSmcConfNext;
        }

        private byte[] kv;
        private string kvPath;
        private bool kvValid = false;
        private string fcrtPath;
        private bool fcrtValid = false;
        private bool fcrtNeeded = false;
        private string smcConfPath;
        private bool smcConfValid = false;
        private string console = "";
        private string hack = "";
        private string kernelStr = "";
        private string smc = "";
        private int ldv = 0;

        private void FormClose(object sender, EventArgs e)
        {
            forceFocus(false);
        }

        private void forceFocus(bool on) // Prevent interaction with MainForm and keep on top
        {
            if (on)
            {
                MainForm.mainForm.Enabled = false;
                //this.TopMost = true;
            }
            else
            {
                MainForm.mainForm.Enabled = true;
                //this.TopMost = false;
            }
        }

        private void WizardCancelled(object sender, EventArgs e)
        {
            forceFocus(false);
            this.Close();
        }

        private void WizardFinished(object sender, EventArgs e)
        {
            if (!fcrtNeeded) fcrtPath = "unneeded";
            else if (DonorFcrt.Checked || DonorKv.Checked) fcrtPath = "donor";
            if (DonorKv.Checked) kvPath = "donor";
            if (DonorSmcConfig.Checked) smcConfPath = "donor";

            forceFocus(false); // Must be before CreateDonor()
            MainForm.mainForm.createDonor(console, hack, smc, CpuKeyBox.Text, kvPath, fcrtPath, smcConfPath, ldv, NoFcrt.Checked);
            this.Close();
        }

        // Prerequisites Page
        private void PrereqNext(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            pullXeBuildVal();

            if (console.Length == 0 || hack.Length == 0 || kernelStr.Length == 0)
            {
                e.Cancel = true;
                MessageBox.Show("Console Type, Hack Type, or Dashboard Version not set\n\nPlease make sure the XeBuild panel is set correctly", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                forceFocus(true);

                string mainKey = MainForm.mainForm.getTxtCpuKey();
                if (mainKey.Length > 0) CpuKeyBox.Text = mainKey;

                PopulateFinish();
            }
        }

        private void pullXeBuildVal()
        {
            if (variables.boardtype != null)
            {
                console = variables.boardtype;
            }
            else console = "";

            hack = MainForm.mainForm.xPanel.getRbtnChecked();
            kernelStr = MainForm.mainForm.xPanel.getComboDash().Text;

            if (hack == "Retail") smc = "Clean";
            else if (hack == "Glitch") smc = "Glitch";
            else if (hack == "Glitch2" || hack == "Glitch2m")
            {
                if (MainForm.mainForm.xPanel.getSMCPChecked()) smc = "SMC+";
                else if (MainForm.mainForm.xPanel.getCR4Checked()) smc = "CR4";
                else if (MainForm.mainForm.xPanel.getRgh3Checked()) smc = "RGH3";
                else smc = "Glitch";
            }
            else if (hack == "JTAG" || hack == "R-JTAG")
            {
                if (MainForm.mainForm.xPanel.getAudClampChecked()) smc = "Aud Clamp";
                else if (console != "Xenon") smc = "Argon Data";
                else smc = "JTAG";
            }
            else if (hack == "DEVGL") smc = "Clean";

            if (hack == "Retail")
            {
                DonorFcrt.Checked = DonorKv.Checked = false;
                DonorFcrt.Enabled = DonorKv.Enabled = false;
                DonorFcrtText.Visible = DonorKvText.Visible = NoFcrt.Checked = NoFcrt.Visible = NoFcrtText.Visible = false;
                RetailFcrtWarn.Visible = RetailKvWarn.Visible = true;
            }
            else
            {
                DonorFcrt.Enabled = DonorKv.Enabled = true;
                DonorFcrtText.Visible = DonorKvText.Visible = NoFcrt.Checked = NoFcrt.Visible = NoFcrtText.Visible = true;
                RetailFcrtWarn.Visible = RetailKvWarn.Visible = false;
            }
        }

        // CPU Key and KV Page
        private void CpuKvBack(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            forceFocus(false);
        }

        private void CpuKvNext(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            if (!Nand.Nand.VerifyKey(Oper.StringToByteArray(CpuKeyBox.Text)))
            {
                e.Cancel = true;
                MessageBox.Show("CPU Key is wrong", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!DonorKv.Checked)
            {
                checkKv();
                if (!kvValid) // Error message handled by checkKv()
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (fcrtNeeded) CpuKvPage.NextPage = FcrtPage;
            else CpuKvPage.NextPage = LdvSmcConfPage;
        }

        private void CpuKvCheckNext()
        {
            if (CpuKeyBox.TextLength == 32 && (DonorKv.Checked || (KvBox.TextLength > 0 && KvBox.Text.Contains(".bin")))) CpuKvPage.AllowNext = true;
            else CpuKvPage.AllowNext = false;
        }

        private void CpuKeyBox_TextChanged(object sender, EventArgs e)
        {
            CpuKvCheckNext();
        }

        private void CpuKeyBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (File.Exists(s[0]))
            {
                FileInfo f = new FileInfo(s[0]);
                if (f.Length == 16) CpuKeyBox.Text = Oper.ByteArrayToString(File.ReadAllBytes(s[0]));
            }
            if (Path.GetExtension(s[0]) == ".txt")
            {
                Regex objAlphaPattern = new Regex("[a-fA-F0-9]{32}$");
                string[] cpu = File.ReadAllLines(s[0]);
                string cpukey = "";
                bool check = false;
                int i = 0;
                foreach (string line in cpu)
                {
                    if (objAlphaPattern.Match(line).Success) i++;
                    if (i > 1) check = true;
                }
                foreach (string line in cpu)
                {
                    if (check)
                    {
                        if (line.ToUpper().Contains("CPU"))
                        {
                            cpukey = objAlphaPattern.Match(line).Value;
                        }
                    }
                    else
                    {
                        cpukey = objAlphaPattern.Match(line).Value;
                        break;
                    }
                    if (variables.debugme) Console.WriteLine(objAlphaPattern.Match(line).Value);
                }
                CpuKeyBox.Text = cpukey;
            }
        }
        private void CpuKeyBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void DonorKv_CheckedChanged(object sender, EventArgs e)
        {
            if (DonorKv.Checked)
            {
                KvBox.Text = "";
                kvValid = false;
                fcrtNeeded = false;
            }

            KvGroup.Enabled = !DonorKv.Checked;
            CpuKvCheckNext();
        }

        private void KvBox_TextChanged(object sender, EventArgs e)
        {
            CpuKvCheckNext();
        }

        private void KvBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            KvBox.Text = s[0];
        }
        private void KvBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void KvEllipse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Decrypted Keyvault (*.bin)|*.bin|All Files (*.*)|*.*";
            openDialog.Title = "Select Keyvault";
            openDialog.InitialDirectory = Oper.FilePickerInitialPath(KvBox.Text);
            openDialog.RestoreDirectory = false;
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                KvBox.Text = openDialog.FileName;
            }
        }

        private void checkKv()
        {
            try
            {
                kvPath = Path.GetFullPath(KvBox.Text);
            }
            catch
            {
                kvValid = false;
                MessageBox.Show("Illegal path to keyvault\n\nYou need to supply a valid decrypted keyvault", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (File.Exists(kvPath))
            {
                if (new FileInfo(kvPath).Length == 16384)
                {
                    kv = File.ReadAllBytes(kvPath);
                    if (!Nand.Nand.kvFcrtEncrypted(kv))
                    {
                        kvValid = true;
                        checkFcrtNeeded(kv);
                    }
                    else
                    {
                        kvValid = false;
                        MessageBox.Show("Keyvault is encrypted\n\nYou can decrypt it using the Decrypt Keyvault option in the Tools menu", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    kvValid = false;
                    MessageBox.Show("Keyvault is invalid or corrupt\n\nYou need to supply a valid decrypted keyvault", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                kvValid = false;
                MessageBox.Show("Keyvault is missing\n\nYou need to supply a valid decrypted keyvault", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkFcrtNeeded(byte[] input)
        {
            if (!DonorKv.Checked)
            {
                if (Nand.Nand.kvNeedFcrt(input))
                {
                    fcrtNeeded = true;
                }
                else
                {
                    fcrtNeeded = false;
                }
            }
            else fcrtNeeded = false;
        }

        // FCRT Page
        private void FcrtNext(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            if (!DonorFcrt.Checked)
            {
                checkFcrt();
                if (!fcrtValid)
                {
                    e.Cancel = true;
                    return;
                }
            }

            PopulateFinish();
        }

        private void FcrtCheckNext()
        {
            if (DonorFcrt.Checked || (FcrtBox.TextLength > 0 && FcrtBox.Text.Contains(".bin"))) FcrtPage.AllowNext = true;
            else FcrtPage.AllowNext = false;
        }

        private void DonorFcrt_CheckedChanged(object sender, EventArgs e)
        {
            if (DonorFcrt.Checked)
            {
                FcrtBox.Text = "";
                NoFcrt.Checked = true;
            }

            NoFcrt.Enabled = !DonorFcrt.Checked;
            FcrtGroup.Enabled = !DonorFcrt.Checked;
            FcrtCheckNext();
        }

        private void FcrtBox_TextChanged(object sender, EventArgs e)
        {
            FcrtCheckNext();
        }

        private void FcrtBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            FcrtBox.Text = s[0];
        }
        private void FcrtBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void FcrtEllipse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Decrypted FCRT (*.bin)|*.bin|All Files (*.*)|*.*";
            openDialog.Title = "Select FCRT";
            openDialog.InitialDirectory = Oper.FilePickerInitialPath(FcrtBox.Text);
            openDialog.RestoreDirectory = false;
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                FcrtBox.Text = openDialog.FileName;
            }
        }

        private void checkFcrt()
        {
            try
            {
                fcrtPath = Path.GetFullPath(FcrtBox.Text);
            }
            catch
            {
                fcrtValid = false;
                MessageBox.Show("Illegal path to FCRT\n\nYou need to supply a valid decrypted FCRT", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (File.Exists(fcrtPath))
            {
                if (new FileInfo(fcrtPath).Length == 16384)
                {
                    fcrtValid = true;
                }
                else
                {
                    fcrtValid = false;
                    MessageBox.Show("FCRT is invalid or corrupt\n\nYou need to supply a valid decrypted FCRT", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                fcrtValid = false;
                MessageBox.Show("FCRT is missing\n\nYou need to supply a valid decrypted FCRT", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // LDV and SMC Config Page
        private void LdvSmcConfNext(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            ldv = Convert.ToInt32(Math.Round(LdvBox.Value, 0));

            if (!DonorSmcConfig.Checked)
            {
                checkSmcConfig();
                if (!smcConfValid)
                {
                    e.Cancel = true;
                    return;
                }
            }

            PopulateFinish();
        }

        private void LdvSmcConfCheckNext()
        {
            if (DonorSmcConfig.Checked || (SmcConfigBox.TextLength > 0 && SmcConfigBox.Text.Contains(".bin"))) LdvSmcConfPage.AllowNext = true;
            else LdvSmcConfPage.AllowNext = false;
        }

        private void DonorSmcConfig_CheckedChanged(object sender, EventArgs e)
        {
            if (DonorSmcConfig.Checked) SmcConfigBox.Text = "";
            SmcConfigGroup.Enabled = !DonorSmcConfig.Checked;
            LdvSmcConfCheckNext();
        }

        private void SmcConfigBox_TextChanged(object sender, EventArgs e)
        {
            LdvSmcConfCheckNext();
        }

        private void SmcConfigBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            SmcConfigBox.Text = s[0];
        }
        private void SmcConfigBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void SmcConfigEllipse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "SMC Config (*.bin)|*.bin|All Files (*.*)|*.*";
            openDialog.Title = "Select SMC Config";
            openDialog.InitialDirectory = Oper.FilePickerInitialPath(SmcConfigBox.Text);
            openDialog.RestoreDirectory = false;
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                SmcConfigBox.Text = openDialog.FileName;
            }
        }

        private void checkSmcConfig()
        {
            try
            {
                smcConfPath = Path.GetFullPath(SmcConfigBox.Text);
            }
            catch
            {
                smcConfValid = false;
                MessageBox.Show("Illegal path to SMC Config\n\nYou need to supply a valid SMC Config", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (File.Exists(smcConfPath))
            {
                if (new FileInfo(smcConfPath).Length == 65536)
                {
                    smcConfValid = true;
                }
                else
                {
                    smcConfValid = false;
                    MessageBox.Show("SMC Config is invalid or corrupt\n\nYou need to supply a valid SMC Config", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                smcConfValid = false;
                MessageBox.Show("SMC Config is missing\n\nYou need to supply a valid SMC Config", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Finish Page
        private void PopulateFinish()
        {
            RevConsole.Text = console;
            RevHack.Text = hack;
            RevSmc.Text = smc;
            RevKernel.Text = kernelStr;
            RevLdv.Text = ldv.ToString();
        }
    }
}
