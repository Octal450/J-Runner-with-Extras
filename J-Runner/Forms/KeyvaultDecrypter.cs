using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
