using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class KeyvaultDecrypter : Form
    {
        public KeyvaultDecrypter()
        {
            InitializeComponent();
            DecryptWizard.Cancelling += WizardCancelled;
            DecryptWizard.Finished += WizardFinished;
            DecryptPage.Commit += DecryptNext;
        }

        private byte[] kv;
        private string kvPath;

        private void WizardCancelled(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WizardFinished(object sender, EventArgs e)
        {
            MainForm.mainForm.startKvDecrypt(kvPath, CpuKeyBox.Text);
            this.Close();
        }

        private void DecryptCheckNext(object sender, EventArgs e)
        {
            if (CpuKeyBox.TextLength == 32 && KvBox.TextLength > 0 && KvBox.Text.Contains(".bin")) DecryptPage.AllowNext = true;
            else DecryptPage.AllowNext = false;
        }

        private void DecryptNext(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            if (!Nand.Nand.VerifyKey(Oper.StringToByteArray(CpuKeyBox.Text)))
            {
                e.Cancel = true;
                MessageBox.Show("CPU Key is wrong", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!checkKv()) // Error message handled by checkKv()
            {
                e.Cancel = true;
                return;
            }
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

        private bool checkKv()
        {
            try
            {
                kvPath = Path.GetFullPath(KvBox.Text);
            }
            catch
            {
                MessageBox.Show("Illegal path to keyvault\n\nYou need to supply a valid decrypted keyvault", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (File.Exists(kvPath))
            {
                if (new FileInfo(kvPath).Length == 16384)
                {
                    kv = File.ReadAllBytes(kvPath);
                    if (Nand.Nand.kvFcrtEncrypted(kv))
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Keyvault is already decrypted\n\nNo need to run this tool", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Keyvault is invalid or corrupt\n\nYou need to supply a valid decrypted keyvault", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Keyvault is missing\n\nYou need to supply a valid decrypted keyvault", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
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
            openDialog.Filter = "Encrypted Keyvault (*.bin)|*.bin|All Files (*.*)|*.*";
            openDialog.Title = "Select Keyvault";
            openDialog.InitialDirectory = Oper.FilePickerInitialPath(KvBox.Text);
            openDialog.RestoreDirectory = false;
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                KvBox.Text = openDialog.FileName;
            }
        }
    }
}
