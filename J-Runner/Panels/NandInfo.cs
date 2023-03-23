using System;
using System.Globalization;
using System.Windows.Forms;

namespace JRunner.Panels
{
    public partial class NandInfo : UserControl
    {
        private Nand.PrivateN nand;
        private DateTime mfr;

        public NandInfo()
        {
            InitializeComponent();
        }

        public NandInfo(Nand.PrivateN Nand)
        {
            InitializeComponent();
            lblfcrt.Visible = false;
            lblhashed.Visible = false;
            label2bl.Visible = false;
            setNand(Nand);
        }

        public void clear()
        {
            // Nand Info
            textBox2BLa.Text = "";
            textBox2BLb.Text = "";
            textBox4BL.Text = "";
            textBox5BL.Text = "";
            textBox6BL_p0.Text = "";
            textBox6BL_p1.Text = "";
            textBox7BL_p0.Text = "";
            textBox7BL_p1.Text = "";
            textBoxldv_0.Text = "";
            textBoxldv_1.Text = "";
            textBoxpd_0.Text = "";
            textBoxpd_1.Text = "";
            textBoxldv_cb.Text = "";
            textBoxpd_cb.Text = "";
            textBoxCbType.Text = "";
            textBoxConsole.Text = "";
            textBoxSmcVer.Text = "";
            textBox2BLb.Enabled = true;
            label2blb.Visible = true;
            label2bla.Visible = true;
            label2bl.Visible = false;

            // KV Info
            btnConsoleId.Text = "View: Native";
            txtconsole.Text = "";
            textBoxConsoleId.Text = "";
            txtDvdKey.Text = "";
            txtOsig.Text = "";
            txtSerial.Text = "";
            txtkvtype.Text = "";
            txtRegion.Text = "";
            textMfrDate.Text = "";
            lblfcrt.Visible = false;
            lblhashed.Visible = false;

            // Bad Blocks
            txtBadBlocks.Text = "No Nand Loaded";

            // Reset Tab
            tabControl1.SelectedTab = tabPageNand;
        }

        public void populateInfo()
        {
            if (nand.ok)
            {
                textBox2BLa.BeginInvoke(new Action(() => {
                    // Nand Info
                    if (nand.bl.CB_A > 0) textBox2BLa.Text = nand.bl.CB_A.ToString();
                    else textBox2BLa.Text = "";
                    if (nand.bl.CB_B > 0) textBox2BLb.Text = nand.bl.CB_B.ToString();
                    else textBox2BLb.Text = "";
                    if (nand.bl.CD > 0) textBox4BL.Text = nand.bl.CD.ToString();
                    else textBox4BL.Text = "";
                    if (nand.bl.CE > 0) textBox5BL.Text = nand.bl.CE.ToString();
                    else textBox5BL.Text = "";
                    if (nand.bl.CF_0 > 0) textBox6BL_p0.Text = nand.bl.CF_0.ToString();
                    else textBox6BL_p0.Text = "";
                    if (nand.bl.CF_1 > 0) textBox6BL_p1.Text = nand.bl.CF_1.ToString();
                    else textBox6BL_p1.Text = "";
                    if (nand.bl.CG_0 > 0) textBox7BL_p0.Text = nand.bl.CG_0.ToString();
                    else textBox7BL_p0.Text = "";
                    if (nand.bl.CG_1 > 0) textBox7BL_p1.Text = nand.bl.CG_1.ToString();
                    else textBox7BL_p1.Text = "";
                    if (nand.bl.CF_0 > 0 || nand.bl.CG_0 > 0) textBoxldv_0.Text = nand.uf.ldv_p0.ToString();
                    else textBoxldv_0.Text = "";
                    if (nand.bl.CF_1 > 0 || nand.bl.CG_1 > 0) textBoxldv_1.Text = nand.uf.ldv_p1.ToString();
                    else textBoxldv_1.Text = "";
                    textBoxpd_0.Text = nand.uf.pd_0;
                    textBoxpd_1.Text = nand.uf.pd_1;

                    if (nand.bl.CB_B != 0)
                    {
                        textBox2BLb.Text = nand.bl.CB_B.ToString();
                        if (textBox2BLb.Text == "15432") textBoxCbType.Text = "RGH3";
                        else textBoxCbType.Text = "Split";
                        textBox2BLb.Enabled = true;
                        label2blb.Visible = true;
                        label2bla.Visible = true;
                        label2bl.Visible = false;
                    }
                    else
                    {
                        textBox2BLb.Enabled = false;
                        textBoxCbType.Text = "Single";
                        label2blb.Visible = false;
                        label2bla.Visible = false;
                        label2bl.Visible = true;
                    }

                    if (textBox2BLb.Text == "15432") // It's not currently possible to properly parse the triple CB setup in RGH3
                    {
                        textBoxldv_cb.Text = textBoxpd_cb.Text = "";
                    }
                    else
                    {
                        if (nand.bl.CB_A > 0 || nand.bl.CB_B > 0) textBoxldv_cb.Text = nand.uf.ldv_cb.ToString();
                        else textBoxldv_cb.Text = "";

                        if (nand.uf.pd_cb == "0x000000") textBoxpd_cb.Text = "";
                        else textBoxpd_cb.Text = nand.uf.pd_cb;
                    }

                    string name = Nand.Nand.getConsoleName(nand, variables.flashconfig);
                    textBoxConsole.Text = name;

                    if (nand.si.smcver.Length > 0) textBoxSmcVer.Text = nand.si.smcver;
                    else textBoxSmcVer.Text = "";

                    // KV Info
                    txtconsole.Text = name;

                    if (!string.IsNullOrWhiteSpace(nand._cpukey) && nand.ki.serial.Length > 0)
                    {
                        string mfrraw = nand.ki.mfdate;
                        try
                        {
                            DateTime.TryParseExact(mfrraw, "MM-dd-yy", null, DateTimeStyles.None, out mfr);
                            textMfrDate.Text = mfr.Date.ToString("MM/dd/yyyy");
                        }
                        catch
                        {
                            textMfrDate.Text = mfrraw;
                        }
                        try
                        {
                            if (btnConsoleId.Text == "View: Native") textBoxConsoleId.Text = Nand.Nand.consoleID_KV_to_friendly(nand.ki.consoleid);
                            else textBoxConsoleId.Text = nand.ki.consoleid;
                        }
                        catch
                        {
                            textBoxConsoleId.Text = "";
                        }
                        txtDvdKey.Text = nand.ki.dvdkey;
                        txtOsig.Text = nand.ki.osig;
                        txtSerial.Text = nand.ki.serial;
                        txtkvtype.Text = nand.ki.kvtype.Replace("0", " ");
                        lblhashed.Visible = txtkvtype.Text == "2";
                        txtRegion.Text = "0x" + nand.ki.region + "   |   ";
                        if (nand.ki.region == "02FE") txtRegion.Text += "PAL/EU";
                        else if (nand.ki.region == "00FF") txtRegion.Text += "NTSC/US";
                        else if (nand.ki.region == "01FE") txtRegion.Text += "NTSC/JAP";
                        else if (nand.ki.region == "01FF") txtRegion.Text += "NTSC/JAP";
                        else if (nand.ki.region == "01FC") txtRegion.Text += "NTSC/KOR";
                        else if (nand.ki.region == "0101") txtRegion.Text += "NTSC/HK";
                        else if (nand.ki.region == "0201") txtRegion.Text += "PAL/AUS";
                        else if (nand.ki.region == "7FFF") txtRegion.Text += "DEVKIT";
                        lblfcrt.Visible = nand.ki.fcrtflag;
                    }
                    else
                    {
                        textBoxConsoleId.Text = "";
                        txtDvdKey.Text = "";
                        txtOsig.Text = "";
                        txtSerial.Text = "";
                        txtkvtype.Text = "";
                        txtRegion.Text = "";
                        textMfrDate.Text = "";
                        lblfcrt.Visible = false;
                        lblhashed.Visible = false;
                    }
                }));

                // Bad Blocks
                nand.getbadblocks();
                if (nand.bad_blocks.Count != 0)
                {
                    string text = "";
                    int blocksize = nand.bigblock ? 0x21000 : 0x4200;
                    int reservestartpos = nand.bigblock ? 0x1E0 : 0x3E0;
                    foreach (int bblock in nand.bad_blocks)
                    {
                        text += ("• Bad Block ID @ 0x" + bblock.ToString("X") + " [Offset: 0x" + ((bblock) * blocksize).ToString("X") + "]");
                        text += Environment.NewLine;
                    }
                    if (nand.remapped_blocks.Count != 0)
                    {
                        text += Environment.NewLine;
                        text += Environment.NewLine;
                        int i = 0;
                        foreach (int bblock in nand.remapped_blocks)
                        {
                            if (bblock != -1)
                            {
                                text += ("• Bad Block ID @ 0x" + nand.bad_blocks[i].ToString("X") + " Found @ 0x" + (reservestartpos + bblock).ToString("X") + "[Offset: 0x" + (blocksize * (reservestartpos + bblock)).ToString("X") + "]");
                                text += Environment.NewLine;
                            }
                            i++;
                        }
                    }
                    else text += ("Remapped Blocks Don't Exist");
                    add_badblocks_tab(text);
                }
                else add_badblocks_tab("No Bad Blocks");
            }
        }

        delegate void AddBadBlockTab(string text);
        private void add_badblocks_tab(string text)
        {
            if (txtBadBlocks.InvokeRequired)
            {
                AddBadBlockTab s = new AddBadBlockTab(add_badblocks_tab);
                this.Invoke(s, new object[] { text });
            }
            else
            {
                txtBadBlocks.Text = text;
            }
        }

        public void setNand(Nand.PrivateN Nand)
        {
            this.nand = Nand;
            populateInfo();
        }

        private void NandInfo_Load(object sender, EventArgs e)
        {
            lblfcrt.Visible = false;
            lblhashed.Visible = false;
            label2bl.Visible = false;
        }

        private void NandInfo_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            MainForm.mainForm.txtFileSource_DragName(s[0]);
        }

        private void NandInfo_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void btnConsoleId_Click(object sender, EventArgs e)
        {
            if (btnConsoleId.Text != "View: Native")
            {
                btnConsoleId.Text = "View: Native";
                if (textBoxConsoleId.Text != "") textBoxConsoleId.Text = Nand.Nand.consoleID_KV_to_friendly(nand.ki.consoleid);
            }
            else
            {
                btnConsoleId.Text = "View: Raw";
                if (textBoxConsoleId.Text != "") textBoxConsoleId.Text = nand.ki.consoleid;
            }
        }

        #region Double Clicks

        private void textBox2BLa_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(textBox2BLa.Text);
        }

        private void textBox2BLb_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(textBox2BLb.Text);
        }

        private void textBox4BL_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(textBox4BL.Text);
        }

        private void textBox5BL_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(textBox5BL.Text);
        }

        private void textBox6BL_p0_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(textBox6BL_p0.Text);
        }

        private void textBox7BL_p0_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(textBox7BL_p0.Text);
        }

        private void textBoxpd_0_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(textBoxpd_0.Text);
        }

        private void textBoxSmcVer_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(textBoxSmcVer.Text);
        }

        private void textBoxpd_cb_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(textBoxpd_cb.Text);
        }

        private void textBox6BL_p1_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(textBox6BL_p1.Text);
        }

        private void textBox7BL_p1_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(textBox7BL_p1.Text);
        }

        private void textBoxpd_1_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(textBoxpd_1.Text);
        }

        private void txtMfrDate_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(textMfrDate.Text);
        }

        private void textBoxConsoleId_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(textBoxConsoleId.Text);
        }

        private void txtSerial_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(txtSerial.Text);
        }

        private void txtRegion_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(txtRegion.Text);
        }

        private void txtOsig_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(txtOsig.Text);
        }

        private void txtDvdKey_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(txtDvdKey.Text);
        }

        private void txtBadBlocks_DoubleClick(object sender, EventArgs e)
        {
            MainForm.mainForm.copyToClipboard(txtBadBlocks.Text);
        }

        #endregion
    }
}
