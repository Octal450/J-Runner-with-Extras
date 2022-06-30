using System;
using System.Windows.Forms;

namespace JRunner
{
    public partial class NandProArg : Form
    {
        public delegate void ClickedRun(string function, string filename, int size, int startblock, int length, bool recalcEcc);
        public event ClickedRun RunClick;

        public string ComFunc = "Read";
        public string SizeFunc = "16";
        private int timingType = 0;

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

        private void btnRun_Click(object sender, EventArgs e)
        {
            this.getFilename();
            this.getLength();
            this.getStart();
            this.getSize();
            this.getFunction();
            RunClick(getFunction(), getFilename(), getSize(), getStart(), getLength(), getRecalcEcc());
        }
        public string getFilename()
        {
            return txtFilename.Text;
        }
        public int getLength()
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
        public string getFunction()
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
        public bool getRecalcEcc()
        {
            return chkRecalcEcc.Checked;
        }

        private void btnfile_Click(object sender, EventArgs e)
        {
            if (readbtn.Checked)
            {
                string file = "";
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Nand files (*.bin)|*.bin";
                saveFileDialog.Title = "Save to File";
                saveFileDialog.RestoreDirectory = false;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    file = saveFileDialog.FileName;
                    variables.currentdir = file;
                }
                if (!String.IsNullOrWhiteSpace(file)) this.txtFilename.Text = file;
            }
            else
            {
                string file = "";
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (xsvfbtn.Checked && timingType == 2) openFileDialog.Filter = "SVF files (*.svf)|*.svf";
                else if (xsvfbtn.Checked && timingType == 1) openFileDialog.Filter = "XSVF files (*.xsvf)|*.xsvf";
                else if (xsvfbtn.Checked) openFileDialog.Filter = "XSVF/SVF files (*.xsvf;*.svf)|*.xsvf;*.svf";
                else openFileDialog.Filter = "Nand files (*.bin;*.ecc)|*.bin;*.ecc";
                openFileDialog.Title = "Select a File";
                openFileDialog.RestoreDirectory = false;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    file = openFileDialog.FileName;
                    variables.currentdir = file;
                }
                if (!String.IsNullOrWhiteSpace(file)) this.txtFilename.Text = file;
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

        private void rbtn_CheckedChanged(object sender, EventArgs e)
        {
            if (readbtn.Checked)
            {
                ComFunc = "Read";
            }
            else if (writebtn.Checked)
            {
                ComFunc = "Write";
            }
            else if (erasebtn.Checked)
            {
                ComFunc = "Erase";
            }
            else if (xsvfbtn.Checked)
            {
                ComFunc = "Xsvf";
            }

            if (!writebtn.Checked) chkRecalcEcc.Checked = false;
            chkRecalcEcc.Visible = writebtn.Checked;

            if (erasebtn.Checked)
            {
                txtFilename.Enabled = false;
                btnfile.Enabled = false;
                txtFilename.Text = "";
            }
            else
            {
                txtFilename.Enabled = true;
                btnfile.Enabled = true;
            }

            if (xsvfbtn.Checked)
            {
                btn16.Checked = false;
                btn64.Checked = false;
                btn256.Checked = false;
                btn512.Checked = false;
                optionalbox.Enabled = false;
                sizebox.Enabled = false;
                txtLength.Text = "";
                txtStart.Text = "";
            }
            else
            {
                optionalbox.Enabled = true;
                sizebox.Enabled = true;
            }
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

        public void UpdateDevice()
        {
            timingType = MainForm.mainForm.getTimingType();

            if (timingType == 1)
            {
                timingType = 1;
                xsvfbtn.Text = "XSVF";
            }
            else if (timingType == 2)
            {
                xsvfbtn.Text = "SVF";
            }
            else
            {
                xsvfbtn.Text = "XSVF";
            }
        }

        private void txtStartLength_TextChanged(object sender, EventArgs e)
        {
            if (txtStart.TextLength > 0 || txtLength.TextLength > 0)
            {
                if (!chkOptional.Checked) chkOptional.Checked = true;
            }
            else
            {
                if (chkOptional.Checked) chkOptional.Checked = false;
            }
        }

        private void chkOptional_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkOptional.Checked)
            {
                if (txtLength.TextLength > 0) txtLength.Text = "";
                if (txtStart.TextLength > 0) txtStart.Text = "";
            }
        }
    }
}
