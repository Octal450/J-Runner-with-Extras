using System;
using System.Windows.Forms;

namespace JRunner.Forms
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
            btnZephyr.DialogResult = DialogResult.OK;
            btnFalcon.DialogResult = DialogResult.OK;
            btnJasper.DialogResult = DialogResult.OK;
            btnJasperSb.DialogResult = DialogResult.OK;
            btnJasper256.DialogResult = DialogResult.OK;
            btnJasper512.DialogResult = DialogResult.OK;
            btnTrinity.DialogResult = DialogResult.OK;
            btnTrinityBb.DialogResult = DialogResult.OK;
            btnCorona.DialogResult = DialogResult.OK;
            btnCorona4G.DialogResult = DialogResult.OK;
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
            Console.WriteLine(this.Height);
            if (sel == Selected.BigBlock || sel == Selected.Slim)
            {
                btnFalcon.Enabled = false;
                btnJasper.Enabled = false;
                btnJasperSb.Enabled = false;
                btnXenon.Enabled = false;
                btnZephyr.Enabled = false;
                if (sel == Selected.BigBlock)
                {
                    btnTrinity.Enabled = false;
                    btnCorona.Enabled = false;
                    btnCorona4G.Enabled = false;
                }
                else if (sel == Selected.Slim)
                {
                    btnJasper256.Enabled = false;
                    btnJasper512.Enabled = false;
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
            else if (type == "jaspersb")
            {
                hresult = variables.ctypes[5];
            }
            else if (type == "jasper256")
            {
                hresult = variables.ctypes[6];
            }
            else if (type == "jasper512")
            {
                hresult = variables.ctypes[7];
            }
            else if (type == "xenon")
            {
                hresult = variables.ctypes[8];
            }
            else if (type == "corona")
            {
                hresult = variables.ctypes[10];
            }
            else if (type == "corona4g")
            {
                hresult = variables.ctypes[11];
            }
            else if (type == "trinitybb")
            {
                hresult = variables.ctypes[12];
            }
            else hresult = variables.ctypes[0];

            return hresult;
        }

        private void setType()
        {
            string ct = this.heResult().Text;
            if (!String.IsNullOrWhiteSpace(ct)) Console.WriteLine(ct + " Manually Selected");
            this.heResult();
            this.Close();
        }

        private void btnXenon_Click(object sender, EventArgs e)
        {
            type = "xenon";
            setType();
        }

        private void btnZephyr_Click(object sender, EventArgs e)
        {
            type = "zephyr";
            setType();
        }

        private void btnFalcon_Click(object sender, EventArgs e)
        {
            type = "falcon";
            setType();
        }

        private void btnJasper_Click(object sender, EventArgs e)
        {
            type = "jasper";
            setType();
        }

        private void btnJasperSb_Click(object sender, EventArgs e)
        {
            type = "jaspersb";
            setType();
        }

        private void btnJasper256_Click(object sender, EventArgs e)
        {
            type = "jasper256";
            setType();
        }

        private void btnJasper512_Click(object sender, EventArgs e)
        {
            type = "jasper512";
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

        private void btnCorona4G_Click(object sender, EventArgs e)
        {
            type = "corona4g";
            setType();
        }

        private void advancedChk_CheckedChanged(object sender, EventArgs e)
        {
            if (advancedChk.Checked)
            {
                this.Height += 55;
                AdvancedBox.Visible = true;
            }
            else
            {
                this.Height -= 55;
                AdvancedBox.Visible = false;
            }
        }
    }
}
