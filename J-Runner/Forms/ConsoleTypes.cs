using System;
using System.Windows.Forms;

namespace JRunner
{
    public partial class ConsoleTypes : Form
    {
        consoles hresult = variables.cunts[0];
        public enum Selected
        {
            All,
            AllX,
            JTAG,
            BigBlock,
            Slim,
            None
        }
        public Selected sel = Selected.All;
        public bool sfulldump = false, twombread = false;

        public ConsoleTypes()
        {
            InitializeComponent();

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
        }


        public consoles heResult()
        {
            if (radiobtnTrinity.Checked)
            {
                hresult = variables.cunts[1];
            }
            else if (radiobtnFalcon.Checked)
            {
                hresult = variables.cunts[2];
            }
            else if (radiobtnZephyr.Checked)
            {
                hresult = variables.cunts[3];
            }
            else if (radiobtnJasper.Checked)
            {
                hresult = variables.cunts[4];
            }
            else if (radiobtnJasperSB.Checked)
            {
                hresult = variables.cunts[5];
            }
            else if (radiobtnJasper256.Checked)
            {
                hresult = variables.cunts[6];
            }
            else if (radiobtnJasper512.Checked)
            {
                hresult = variables.cunts[7];
            }
            else if (radiobtnXenon.Checked)
            {
                hresult = variables.cunts[8];
            }
            else if (radiobtnCorona.Checked)
            {
                hresult = variables.cunts[10];
            }
            else if (radiobtnCorona4gb.Checked)
            {
                hresult = variables.cunts[11];
            }
            else if (radiobtnTrinityBB.Checked)
            {
                hresult = variables.cunts[12];
            }
            else hresult = variables.cunts[0];

            return hresult;
        }


        public bool fulldump()
        {
            return fulldumpbox.Checked;
        }
        public bool twombdump()
        {
            return checkBox2MB.Checked;
        }

        void button1_Click(object sender, System.EventArgs e)
        {
            Console.WriteLine((this.heResult().Text + " Manually Selected"));
            this.heResult();
            this.Close();
        }
        void button2_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void load()
        {
            if (!sfulldump)
            {
                fulldumpbox.Enabled = false;
            }
            if (!twombread)
            {
                checkBox2MB.Enabled = false;
            }
            if (sel == Selected.BigBlock || sel == Selected.Slim)
            {
                radiobtnFalcon.Enabled = false;
                radiobtnJasper.Enabled = false;
                radiobtnJasperSB.Enabled = false;
                radiobtnXenon.Enabled = false;
                radiobtnZephyr.Enabled = false;
                if (sel == Selected.BigBlock)
                {
                    radiobtnTrinity.Enabled = false;
                    radiobtnCorona.Enabled = false;
                    radiobtnCorona4gb.Enabled = false;
                }
                else if (sel == Selected.Slim)
                {
                    radiobtnJasper256.Enabled = false;
                    radiobtnJasper512.Enabled = false;
                }
            }
            else if (sel == Selected.JTAG)
            {
                radiobtnTrinity.Enabled = false;
                radiobtnTrinityBB.Enabled = false;
                radiobtnCorona.Enabled = false;
                radiobtnCorona4gb.Enabled = false;
            }
            else if (sel == Selected.AllX)
            {
                radiobtnXenon.Enabled = false;
            }
        }


        private void ConsoleTypes_Load(object sender, EventArgs e)
        {
            load();
            switch (variables.ctyp.ID)
            {
                case 1:
                    radiobtnTrinity.Checked = true;
                    break;
                case 2:
                    radiobtnFalcon.Checked = true;
                    break;
                case 3:
                    radiobtnZephyr.Checked = true;
                    break;
                case 4:
                    radiobtnJasper.Checked = true;
                    break;
                case 5:
                    radiobtnJasperSB.Checked = true;
                    break;
                case 6:
                    radiobtnJasper256.Checked = true;
                    break;
                case 7:
                    radiobtnJasper512.Checked = true;
                    break;
                case 8:
                    radiobtnXenon.Checked = true;
                    break;
                case 9:
                    radiobtnFalcon.Checked = true;
                    break;
                case 10:
                    radiobtnCorona.Checked = true;
                    break;
                case 11:
                    radiobtnCorona4gb.Checked = true;
                    break;
                case 12:
                    radiobtnTrinityBB.Checked = true;
                    break;
                default:
                    break;
            }
            if (MainForm.nand.bl.CB_A != 0)
            {
                if (MainForm.nand.bl.CB_A >= 4558 && MainForm.nand.bl.CB_A <= 4580) radiobtnZephyr.Checked = true;
                else if ((MainForm.nand.bl.CB_A >= 1888 && MainForm.nand.bl.CB_A <= 1940) || MainForm.nand.bl.CB_A == 7373 || MainForm.nand.bl.CB_A == 8192) radiobtnXenon.Checked = true;
            }
        }

        private void chkAdv_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAdv.Checked)
            {
                checkBox2MB.Visible = true;
                fulldumpbox.Visible = true;
                radiobtnJasperSB.Visible = radiobtnTrinityBB.Visible = true;
            }
            else
            {
                checkBox2MB.Checked = checkBox2MB.Visible = false;
                fulldumpbox.Checked = fulldumpbox.Visible = false;
                radiobtnJasperSB.Checked = radiobtnTrinityBB.Checked = false;
                radiobtnJasperSB.Visible = radiobtnTrinityBB.Visible = false;
            }
        }
        private void checkBox2MB_CheckedChanged(object sender, EventArgs e)
        {
            if (fulldumpbox.Checked) fulldumpbox.Checked = false;
        }

        private void fulldumpbox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2MB.Checked) checkBox2MB.Checked = false;
        }
    }

}
