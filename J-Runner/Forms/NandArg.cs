using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace JRunner
{
    public partial class NandProArg : Form
    {
        public delegate void ClickedRun(string function, string filename, int size, int startblock, int length);
        public event ClickedRun RunClick;

        public string ComFunc = "";
        public string SizeFunc = "";
        
        public NandProArg()
        {
            InitializeComponent();
            this.AcceptButton = btnRun;
            btnRun.DialogResult = System.Windows.Forms.DialogResult.OK;
            txtFilename.Text = variables.filename1;
            txtFilename.Select(txtFilename.Text.Length, 0);
            MainForm.mainForm.updateDevice += UpdateDevice;
            UpdateDevice();
        }

        public NandProArg(string filename, string length, string start, string size, string function)
        {
            InitializeComponent();
            this.AcceptButton = btnRun;
            btnRun.DialogResult = System.Windows.Forms.DialogResult.OK;
            txtFilename.Text = filename;
            txtFilename.Select(txtFilename.Text.Length, 0);
            txtLength.Text = length;
            txtStart.Text = start;
            if (size == "16") btn16.Checked = true;
            else if (size == "64") btn64.Checked = true;
            else if (size == "256") btn256.Checked = true;
            else if (size == "512") btn512.Checked = true;

            if (function == "Read") ReadBtn.Checked = true;
            else if (function == "Write") writebtn.Checked = true;
            else if (function == "Erase") erasebtn.Checked = true;
            else if (function == "Xsvf") xsvfbtn.Checked = true;
            MainForm.mainForm.updateDevice += UpdateDevice;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.getfilename();
            this.getlength();
            this.getStart();
            this.getSize();
            this.getfunction();
            RunClick(getfunction(), getfilename(), getSize(), getStart(), getlength());
        }
        public string getfilename()
        {
            return txtFilename.Text;
        }
        public int getlength()
        {
            try
            {
                int temp = Convert.ToInt32(txtLength.Text, 16);
                return temp;
            }
            catch (Exception) { }
            return 0;
        }
        public int getStart()
        {
            try
            {
                int temp = Convert.ToInt32(txtStart.Text, 16);
                return temp;
            }
            catch (Exception) { }
            return 0;
        }
        public string getfunction()
        {
            return ComFunc;
        }
        public int getSize()
        {
            try
            {
                int temp = Convert.ToInt32(SizeFunc);
                return temp;
            }
            catch (Exception) { }
            return 0;
        }

        //private void cboxCommand_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cboxCommand.Text == "Read")
        //    {
        //        txtFilename.Enabled = true;
        //        txtStart.Enabled = true;
        //        txtLength.Enabled = true;
        //        cBoxSize.Enabled = true;
        //    }
        //    else if (cboxCommand.Text == "Write")
        //    {
        //        txtFilename.Enabled = true;
        //        txtStart.Enabled = true;
        //        txtLength.Enabled = true;
        //        cBoxSize.Enabled = true;
        //    }
        //    if (cboxCommand.Text == "Erase")
        //    {
        //        txtFilename.Enabled = false;
        //        txtStart.Enabled = true;
        //        txtLength.Enabled = true;
        //        cBoxSize.Enabled = true;
        //    }
        //    if (cboxCommand.Text == "Xsvf")
        //    {
        //        txtFilename.Enabled = true;
        //        txtStart.Enabled = false;
        //        txtLength.Enabled = false;
        //        cBoxSize.Enabled = false;
        //    }
        //}

        private void btnfile_Click(object sender, EventArgs e)
        {
            if (ReadBtn.Checked)
            {
                string filename1 = "";
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Nand files (*.bin)|*.bin";
                saveFileDialog1.Title = "Save to File";
                //saveFileDialog1.InitialDirectory = variables.currentdir;
                saveFileDialog1.RestoreDirectory = false;
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filename1 = saveFileDialog1.FileName;
                    variables.currentdir = filename1;
                }
                if (filename1 != "") this.txtFilename.Text = filename1;
            }
            else
            {
                string filename1 = "";
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                if ((MainForm.mainForm.device == 3 || MainForm.mainForm.device == 4) && xsvfbtn.Checked)
                {
                    openFileDialog1.Filter = "SVF files (*.svf)|*.svf";
                }
                else if (xsvfbtn.Checked)
                {
                    openFileDialog1.Filter = "XSVF files (*.xsvf)|*.xsvf";
                }
                else
                {
                    openFileDialog1.Filter = "Nand files (*.bin;*.ecc)|*.bin;*.ecc";
                }
                openFileDialog1.Title = "Select a File";
                //openFileDialog1.InitialDirectory = variables.currentdir;
                openFileDialog1.RestoreDirectory = false;
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filename1 = openFileDialog1.FileName;
                    variables.currentdir = filename1;
                }
                if (filename1 != "") this.txtFilename.Text = filename1;
            }
        }

        private void txtFilename_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            this.txtFilename.Text = s[0];
        }

        private void txtFilename_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ReadBtn_CheckedChanged(object sender, EventArgs e)
        {
            txtFilename.Enabled = true;
            txtStart.Enabled = true;
            txtLength.Enabled = true;
            sizebox.Enabled = true;
            btnfile.Enabled = true;
            ComFunc = "Read";
        }

        private void writebtn_CheckedChanged(object sender, EventArgs e)
        {
            txtFilename.Enabled = true;
            txtStart.Enabled = true;
            txtLength.Enabled = true;
            sizebox.Enabled = true;
            btnfile.Enabled = true;
            ComFunc = "Write";
        }

        private void erasebtn_CheckedChanged(object sender, EventArgs e)
        {
            txtFilename.Enabled = false;
            txtStart.Enabled = true;
            txtLength.Enabled = true;
            sizebox.Enabled = true;
            btnfile.Enabled = false;
            ComFunc = "Erase";
        }

        private void xsvfbtn_CheckedChanged(object sender, EventArgs e)
        {
            txtFilename.Enabled = true;
            txtStart.Enabled = false;
            txtLength.Enabled = false;
            btnfile.Enabled = true;
            ComFunc = "Xsvf";
            btn16.Checked = false;
            btn64.Checked = false;
            btn256.Checked = false;
            btn512.Checked = false;
            sizebox.Enabled = false;
        }

        private void btn16_CheckedChanged(object sender, EventArgs e)
        {
            SizeFunc = "16";
        }

        private void btn64_CheckedChanged(object sender, EventArgs e)
        {
            SizeFunc = "64";
        }

        private void btn256_CheckedChanged(object sender, EventArgs e)
        {
            SizeFunc = "256";
        }

        private void btn512_CheckedChanged(object sender, EventArgs e)
        {
            SizeFunc = "512";
        }

        private void ProgramCRButton_Click(object sender, EventArgs e)
        {
            MainForm.mainForm.openXsvfInfo();
            NandProArg.ActiveForm.Hide();
        }

        void UpdateDevice()
        {
            if (MainForm.mainForm.device == 3 || MainForm.mainForm.device == 4)
            {
                Optionalbox.Enabled = false;
                xsvfbtn.Text = "SVF";
            }
            else
            {
                Optionalbox.Enabled = true;
                xsvfbtn.Text = "XSVF";
            }
        }
    }
}
