using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace JRunner
{
    public partial class ConsoleSelect : Form
    {
        consoles hresult = variables.ctypes[0];
        public enum Selected
        {
            All,
            AllX,
            BigBlock,
            Slim,
            None
        }
        public Selected sel = Selected.All;
        private string type = "";

        public ConsoleSelect()
        {
            InitializeComponent();

            btnXenon.DialogResult = DialogResult.OK;
            btnXenon64.DialogResult = DialogResult.OK;
            btnZephyr.DialogResult = DialogResult.OK;
            btnZephyr64.DialogResult = DialogResult.OK;
            btnFalcon.DialogResult = DialogResult.OK;
            btnFalcon64.DialogResult = DialogResult.OK;
            btnJasper.DialogResult = DialogResult.OK;
            btnJasperXsb.DialogResult = DialogResult.OK;
            btnJasperBb.DialogResult = DialogResult.OK;
            btnTrinity.DialogResult = DialogResult.OK;
            btnTrinityBb.DialogResult = DialogResult.OK;
            btnCorona.DialogResult = DialogResult.OK;
            btnCoronaBb.DialogResult = DialogResult.OK;
            btnCorona4g.DialogResult = DialogResult.OK;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void ConsoleSelect_Load(object sender, EventArgs e)
        {
            if (sel == Selected.BigBlock || sel == Selected.Slim)
            {
                btnFalcon.Enabled = false;
                btnJasper.Enabled = false;
                btnJasperXsb.Enabled = false;
                btnXenon.Enabled = false;
                btnZephyr.Enabled = false;
                if (sel == Selected.BigBlock)
                {
                    btnTrinity.Enabled = false;
                    btnCorona.Enabled = false;
                    btnCorona4g.Enabled = false;
                }
                else if (sel == Selected.Slim)
                {
                    btnJasperBb.Enabled = false;
                }
            }
            else if (sel == Selected.AllX)
            {
                btnXenon.Enabled = false;
            }
        }

        public consoles heResult()
        {
            if (type == "trinity")
            {
                hresult = variables.ctypes[1];
            }
            else if (type == "falcon")
            {
                hresult = variables.ctypes[2];
            }
            else if (type == "zephyr")
            {
                hresult = variables.ctypes[3];
            }
            else if (type == "jasper")
            {
                hresult = variables.ctypes[4];
            }
            else if (type == "jasperxsb")
            {
                hresult = variables.ctypes[5];
            }
            else if (type == "jasperbb")
            {
                hresult = variables.ctypes[6];
            }
            else if (type == "xenon64")
            {
                hresult = variables.ctypes[7];
            }
            else if (type == "xenon")
            {
                hresult = variables.ctypes[8];
            }
            else if (type == "coronabb")
            {
                if ((ModifierKeys & Keys.Control) == Keys.Control && (ModifierKeys & Keys.Shift) == Keys.Shift) hresult = variables.ctypes[17];
                else hresult = variables.ctypes[9];
            }
            else if (type == "corona")
            {
                if ((ModifierKeys & Keys.Control) == Keys.Control && (ModifierKeys & Keys.Shift) == Keys.Shift) hresult = variables.ctypes[15];
                else hresult = variables.ctypes[10];
            }
            else if (type == "corona4g")
            {
                if ((ModifierKeys & Keys.Control) == Keys.Control && (ModifierKeys & Keys.Shift) == Keys.Shift) hresult = variables.ctypes[16];
                else hresult = variables.ctypes[11];
            }
            else if (type == "trinitybb")
            {
                hresult = variables.ctypes[12];
            }
            else if (type == "zephyr64")
            {
                hresult = variables.ctypes[13];
            }
            else if (type == "falcon64")
            {
                hresult = variables.ctypes[14];
            }
            else hresult = variables.ctypes[0];

            return hresult;
        }

        private void setType()
        {
            string ct = this.heResult().Text;
            if (!string.IsNullOrWhiteSpace(ct)) Console.WriteLine(ct + " Manually Selected");
            this.heResult();
            this.Close();
        }

        private void btnXenon_Click(object sender, EventArgs e)
        {
            type = "xenon";
            setType();
        }

        private void btnXenon64_Click(object sender, EventArgs e)
        {
            type = "xenon64";
            setType();
        }

        private void btnZephyr_Click(object sender, EventArgs e)
        {
            type = "zephyr";
            setType();
        }

        private void btnZephyr64_Click(object sender, EventArgs e)
        {
            type = "zephyr64";
            setType();
        }

        private void btnFalcon_Click(object sender, EventArgs e)
        {
            type = "falcon";
            setType();
        }

        private void btnFalcon64_Click(object sender, EventArgs e)
        {
            type = "falcon64";
            setType();
        }

        private void btnJasper_Click(object sender, EventArgs e)
        {
            type = "jasper";
            setType();
        }

        private void btnJasperXsb_Click(object sender, EventArgs e)
        {
            type = "jasperxsb";
            setType();
        }

        private void btnJasperBb_Click(object sender, EventArgs e)
        {
            type = "jasperbb";
            setType();
        }

        private void btnTrinity_Click(object sender, EventArgs e)
        {
            type = "trinity";
            setType();
        }

        private void btnTrinityBb_Click(object sender, EventArgs e)
        {
            type = "trinitybb";
            setType();
        }

        private void btnCorona_Click(object sender, EventArgs e)
        {
            type = "corona";
            setType();
        }

        private void btnCoronaBb_Click(object sender, EventArgs e)
        {
            type = "coronabb";
            setType();
        }

        private void btnCorona4G_Click(object sender, EventArgs e)
        {
            type = "corona4g";
            setType();
        }

        private void advancedChk_CheckedChanged(object sender, EventArgs e)
        {
            float dpi = Program.getScalingFactor();
            if (advancedChk.Checked)
            {
                this.Height += Convert.ToInt32(163 * dpi);
                AdvancedBox.Visible = true;
            }
            else
            {
                this.Height -= Convert.ToInt32(163 * dpi);
                AdvancedBox.Visible = false;
            }
        }
    }
}
