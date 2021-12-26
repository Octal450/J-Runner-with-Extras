using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JRunner
{
    public partial class AddCpuKey : Form
    {
        public string filename1, cerial, mobo, DVDkey, RegioN, OSIG;
        public long kv_crc;

        public AddCpuKey()
        {
            InitializeComponent();
        }


        void nandinit()
        {
            Regex objAlphaPattern = new Regex("[a-fA-F0-9]{32}$");
            bool sts = objAlphaPattern.IsMatch(txtCpuKey.Text);
            bool cpucheck = false;
            bool check = false;
            if (filename1 != null && Path.GetExtension(filename1) == ".bin")
            {
                if ((txtCpuKey.Text.Length == 32 && sts)) cpucheck = true;
                if (cpucheck) check = Nand.Nand.cpukeyverification(filename1, txtCpuKey.Text);

            }
            if (check)
            {
                kv_crc = MainForm.nand.kvcrc();
                cerial = MainForm.nand.ki.serial;
                mobo = Nand.Nand.getConsoleName(MainForm.nand, variables.flashconfig);
                DVDkey = MainForm.nand.ki.dvdkey;
                RegioN = MainForm.nand.ki.region;
                OSIG = MainForm.nand.ki.osig;
            }
        }

        public string serial()
        {
            return cerial;
        }
        public string motherboard()
        {
            return mobo;
        }

        public string cpukey()
        {
            return txtCpuKey.Text;
        }
        public long kvcrc()
        {
            return kv_crc;
        }
        public string dvdkey()
        {
            return this.DVDkey;
        }
        public string region()
        {
            return this.RegioN;
        }
        public string osig()
        {
            return this.OSIG;
        }

        /// <summary>
        /// Form functions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (Path.GetExtension(s[0]) == ".bin" || Path.GetExtension(s[0]) == ".ecc")
            {
                this.txtFilename.Text = s[0];
                filename1 = s[0];
                nandinit();
            }

        }
        void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }
        void cpukeytext_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (Path.GetExtension(s[0]) == ".txt")
            {
                Regex objAlphaPattern = new Regex("[a-fA-F0-9]{32}$");
                bool sts = objAlphaPattern.IsMatch(File.ReadAllText(s[0]));
                if (File.ReadAllText(s[0]).Length == 32 && sts)
                {
                    txtCpuKey.Text = File.ReadAllText(s[0]);
                }
            }

        }
        void cpukeytext_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "(*.bin)|*.bin|All files (*.*)|*.*";
            openFileDialog1.Title = "Select a File";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename1 = openFileDialog1.FileName;

            }
            if (filename1 != null) this.txtFilename.Text = filename1;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            nandinit();
            this.Close();
        }

        void cpukeytext_TextChanged(object sender, System.EventArgs e)
        {
            if (txtCpuKey.Text.Length == 32) nandinit();
        }
    }
}
