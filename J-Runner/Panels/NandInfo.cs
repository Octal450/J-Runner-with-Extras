using System;
using System.Globalization;
using System.Windows.Forms;

namespace JRunner.Panels
{
    public partial class NandInfo : UserControl
    {
        private Nand.PrivateN nand;
        public delegate void DragDropC(string filename);
        public event DragDropC DragDropChanged;
        private DateTime mfr;

        public NandInfo()
        {
            InitializeComponent();
        }

        public NandInfo(Nand.PrivateN Nand)
        {
            InitializeComponent();
            lblfcrt.Visible = false;
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
            textBox2BLb.Enabled = true;
            label2blb.Visible = true;
            label2bla.Visible = true;
            label2bl.Visible = false;

            // KV Info
            btnConsoleId.Text = "View: Native";
            txtconsole.Text = "";
            textBoxconsoleid.Text = "";
            txtdvdkey.Text = "";
            txtosig.Text = "";
            txtSerial.Text = "";
            txtkvtype.Text = "";
            txtregion.Text = "";
            textMFRdate.Text = "";
            lblfcrt.Visible = false;

            // Bad Blocks
            txtBadBlocks.Text = "";

            // Reset Tab
            tabControl1.SelectedTab = tabPage1;
        }

        public void populateInfo()
        {
            if (nand.ok)
            {
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
                if (nand.bl.CB_A > 0 || nand.bl.CB_B > 0) textBoxldv_cb.Text = nand.uf.ldv_cb.ToString();
                else textBoxldv_cb.Text = "";
                textBoxpd_cb.Text = nand.uf.pd_cb;

                if (nand.bl.CB_B != 0)
                {
                    textBox2BLb.Text = nand.bl.CB_B.ToString();
                    textBoxCbType.Text = "Split CB";
                    textBox2BLb.Enabled = true;
                    label2blb.Visible = true;
                    label2bla.Visible = true;
                    label2bl.Visible = false;
                }
                else
                {
                    textBox2BLb.Enabled = false;
                    textBoxCbType.Text = "Single CB";
                    label2blb.Visible = false;
                    label2bla.Visible = false;
                    label2bl.Visible = true;
                }

                string name = Nand.Nand.getConsoleName(nand, variables.flashconfig);
                textBoxConsole.Text = name;

                // KV Info
                txtconsole.Text = name;

                if (!String.IsNullOrWhiteSpace(nand._cpukey) && nand.ki.serial.Length > 0)
                {
                    string mfrraw = nand.ki.mfdate;
                    try
                    {
                        DateTime.TryParseExact(mfrraw, "MM-dd-yy", null, DateTimeStyles.None, out mfr);
                        textMFRdate.Text = mfr.Date.ToString("MM/dd/yyyy");
                    }
                    catch
                    {
                        textMFRdate.Text = mfrraw;
                    }
                    try
                    {
                        if (btnConsoleId.Text == "View: Native") textBoxconsoleid.Text = Nand.Nand.consoleID_KV_to_friendly(nand.ki.consoleid);
                        else textBoxconsoleid.Text = nand.ki.consoleid;
                    }
                    catch
                    {
                        textBoxconsoleid.Text = "";
                    }
                    txtdvdkey.Text = nand.ki.dvdkey;
                    txtosig.Text = nand.ki.osig;
                    txtSerial.Text = nand.ki.serial;
                    txtkvtype.Text = nand.ki.kvtype.Replace("0", " ");
                    txtregion.Text = "0x" + nand.ki.region + "   |   ";
                    if (nand.ki.region == "02FE") txtregion.Text += "PAL/EU";
                    else if (nand.ki.region == "00FF") txtregion.Text += "NTSC/US";
                    else if (nand.ki.region == "01FE") txtregion.Text += "NTSC/JAP";
                    else if (nand.ki.region == "01FF") txtregion.Text += "NTSC/JAP";
                    else if (nand.ki.region == "01FC") txtregion.Text += "NTSC/KOR";
                    else if (nand.ki.region == "0101") txtregion.Text += "NTSC/HK";
                    else if (nand.ki.region == "0201") txtregion.Text += "PAL/AUS";
                    else if (nand.ki.region == "7FFF") txtregion.Text += "DEVKIT";
                    lblfcrt.Visible = nand.ki.fcrtflag;
                }
                else
                {
                    textBoxconsoleid.Text = "";
                    txtdvdkey.Text = "";
                    txtosig.Text = "";
                    txtSerial.Text = "";
                    txtkvtype.Text = "";
                    txtregion.Text = "";
                    textMFRdate.Text = "";
                    lblfcrt.Visible = false;
                }
                Console.WriteLine(name);

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
            this.BeginInvoke(new Action(() => populateInfo()));
        }

        delegate void ShowCpuKeyTab();
        public void show_cpukey_tab()
        {
            if (tabControl1.InvokeRequired)
            {
                ShowCpuKeyTab s = new ShowCpuKeyTab(show_cpukey_tab);
                this.Invoke(s);
            }
            else
            {
                this.tabControl1.SelectedTab = this.tabPage2;
            }
        }

        public void change_tab()
        {
            this.tabControl1.BeginInvoke((Action)(() => tabControl1.SelectedTab = tabPage1));
            this.tabControl1.BeginInvoke((Action)(() => tabControl1.Refresh()));
        }

        private void NandInfo_Load(object sender, EventArgs e)
        {
            lblfcrt.Visible = false;
            label2bl.Visible = false;
        }

        private void NandInfo_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            DragDropChanged(s[0]);
        }

        private void NandInfo_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            if (nand != null && nand.ok)
            {
                HexEdit.KVViewer k = new HexEdit.KVViewer(Nand.Nand.decryptkv(nand._rawkv, Oper.StringToByteArray(nand._cpukey)));
                k.ShowDialog();
            }
        }

        private void btnConsoleId_Click(object sender, EventArgs e)
        {
            if (btnConsoleId.Text != "View: Native")
            {
                btnConsoleId.Text = "View: Native";
                if (textBoxconsoleid.Text != "") textBoxconsoleid.Text = Nand.Nand.consoleID_KV_to_friendly(nand.ki.consoleid);
            }
            else
            {
                btnConsoleId.Text = "View: Raw";
                if (textBoxconsoleid.Text != "") textBoxconsoleid.Text = nand.ki.consoleid;
            }
        }
    }
}
