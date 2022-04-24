using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace JRunner.Panels
{
    public partial class XeBuildPanel : UserControl
    {
        List<String> patches = new List<string>(new string[8]);
        // -a nofcrt
        // -a noSShdd
        // -a nointmu
        // -a nohdd
        // -a nohdmiwait
        // -a nolan
        // -r WB/WB4G/13182

        public XeBuildPanel()
        {
            InitializeComponent();
            checkDevGL("None");
        }

        #region delegates
        public delegate void ClickedGetMB();
        public event ClickedGetMB Getmb;
        public delegate void ChangedHack();
        public event ChangedHack HackChanged;
        public delegate void DashAdded();
        public event DashAdded AddedDash;
        public delegate void DashDeleted();
        public event DashDeleted DeletedDash;
        public delegate void CallMotherboards();
        public event CallMotherboards CallMB;
        public delegate void loadFile(ref string filename, bool erase = false);
        public event loadFile loadFil;
        public delegate void updateProgress(int progress);
        public event updateProgress UpdateProgres;
        public delegate void updateSource(string filename);
        public event updateSource updateSourc;
        public delegate void ModeDrive();
        public event ModeDrive DriveMode;
        #endregion

        #region getters/setters
        public DataSet1 getDataSet()
        {
            return dataSet1;
        }
        public ComboBox getComboDash()
        {
            return comboDash;
        }

        // Hack Getters
        public bool getRbtnRetailChecked()
        {
            return rbtnRetail.Checked;
        }
        public bool getRbtnGlitchChecked()
        {
            return rbtnGlitch.Checked;
        }
        public bool getRbtnGlitch2Checked()
        {
            return rbtnGlitch2.Checked;
        }
        public bool getRbtnGlitch2mChecked()
        {
            return rbtnGlitch2m.Checked;
        }
        public bool getRbtnJtagChecked()
        {
            return rbtnJtag.Checked;
        }
        public bool getRbtnDevGLChecked()
        {
            return rbtnDevGL.Checked;
        }
        public string getRbtnChecked()
        {
            if (rbtnRetail.Checked) return "Retail";
            else if (rbtnGlitch.Checked) return "Glitch";
            else if (rbtnGlitch2.Checked) return "Glitch2";
            else if (rbtnGlitch2m.Checked) return "Glitch2m";
            else if (rbtnJtag.Checked)
            {
                if (chkRJtag.Checked) return "R-JTAG";
                else return "JTAG";
            }
            else if (rbtnDevGL.Checked) return "DEVGL";
            else return "";
        }

        // Hack Setters - invoke is in references, so no cross thread
        public void setRbtnRetailChecked(bool check)
        {
            if (check && !rbtnRetail.Enabled) return;
            rbtnRetail.Checked = check;
        }
        public void setRbtnGlitchChecked(bool check)
        {
            if (check && !rbtnGlitch.Enabled) return;
            rbtnGlitch.Checked = check;
        }
        public void setRbtnGlitch2Checked(bool check)
        {
            if (check && !rbtnGlitch2.Enabled) return;
            rbtnGlitch2.Checked = check;
        }
        public void setRbtnGlitch2mChecked(bool check)
        {
            if (check && !rbtnGlitch2m.Enabled) return;
            rbtnGlitch2m.Checked = check;
        }
        public void setRbtnJtagChecked(bool check)
        {
            if (check && !rbtnJtag.Enabled) return;
            rbtnJtag.Checked = check;
        }
        public void setRbtnDevGLChecked(bool check)
        {
            if (check && !rbtnDevGL.Enabled) return;
            rbtnDevGL.Checked = check;
        }

        // Checkbox Getters
        public bool getCleanSMCChecked()
        {
            return chkCleanSMC.Checked;
        }
        public bool getCR4Checked()
        {
            return chkCR4.Checked;
        }
        public bool getSMCPChecked()
        {
            return chkSMCP.Checked;
        }
        public bool getRgh3Checked()
        {
            return chkRgh3.Checked;
        }
        public int getRgh3Mhz()
        {
            return int.Parse(Rgh3Mhz.Text);
        }
        public bool getAudClampChecked()
        {
            return chkAudClamp.Checked;
        }
        public bool getRJtagChecked()
        {
            return chkRJtag.Checked;
        }
        public int getWBChecked()
        {
            if (chkWB.Checked) return 1;
            else if (chkWB4G.Checked) return 2;
            else return 0;
        }

        // Checkbox Setters
        public void setCleanSMCChecked(bool check)
        {
            if (check && !chkCleanSMC.Enabled) return;
            chkCleanSMC.Checked = check;
        }
        public void setNoFcrt(bool check)
        {
            chkListBoxPatches.SetItemChecked(0, check);
        }

        public void clear()
        {
            txtMBname.Text = "None Selected";
            if (rbtnRetail.Enabled == true) rbtnRetail.Checked = true;
            else if (rbtnGlitch.Checked) rbtnGlitch.Checked = false;
            else if (rbtnGlitch2.Checked) rbtnGlitch2.Checked = false;
            else if (rbtnGlitch2m.Checked) rbtnGlitch2m.Checked = false;
            else if (rbtnJtag.Checked) rbtnJtag.Checked = false;
            else if (rbtnDevGL.Checked) rbtnDevGL.Checked = false;
            checkAvailableHackTypes();
            tabControl1.SelectedTab = Xebuild;
            for (int i = 0; i < chkListBoxPatches.Items.Count; i++)
            {
                chkListBoxPatches.SetItemChecked(i, false);
            }
            chkxesettings.Checked = false;
        }

        public void setMBname(string txt)
        {
            txtMBname.BeginInvoke(new Action(() => {
                txtMBname.Text = txt;
                variables.boardtype = txt;
                checkAvailableHackTypes();
                checkWB(txt);
                checkBigffs(txt);

                if (txt.Contains("Xenon"))
                {
                    chkCR4.Checked = false;
                    chkCR4.Enabled = false;
                    chkSMCP.Checked = false;
                    chkSMCP.Enabled = false;
                    chkAudClamp.Checked = false;
                    chkAudClamp.Enabled = false;
                }
                else
                {
                    chkCR4.Enabled = true;
                    chkSMCP.Enabled = true;
                    chkAudClamp.Enabled = true;
                }

                checkRgh3(txt);
            }));
        }
        #endregion

        #region UI

        private void rbtn_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnRetail.Checked)
            {
                if (checkDLPatches.Checked) variables.DashLaunchE = checkDLPatches.Checked;
                checkDLPatches.Enabled = chkLaunch.Visible = false;
                if (sender.Equals(rbtnRetail)) Console.WriteLine("Retail Selected");
            }
            else if (rbtnJtag.Checked)
            {
                checkDLPatches.Enabled = true;
                checkDLPatches.Checked = chkLaunch.Visible = variables.DashLaunchE;
                if (sender.Equals(rbtnJtag)) Console.WriteLine("JTAG Selected");

            }
            else if (rbtnGlitch.Checked || rbtnGlitch2.Checked || rbtnGlitch2m.Checked)
            {
                checkDLPatches.Enabled = true;
                checkDLPatches.Checked = chkLaunch.Visible = variables.DashLaunchE;
                if (rbtnGlitch.Checked && sender.Equals(rbtnGlitch)) Console.WriteLine("Glitch Selected");
                else if (rbtnGlitch2.Checked && sender.Equals(rbtnGlitch2)) Console.WriteLine("Glitch2 Selected");
                else if (rbtnGlitch2m.Checked && sender.Equals(rbtnGlitch2m)) Console.WriteLine("Glitch2m Selected");
            }
            else if (rbtnDevGL.Checked)
            {
                checkDLPatches.Enabled = true;
                checkDLPatches.Checked = chkLaunch.Visible = variables.DashLaunchE;
                if (sender.Equals(rbtnDevGL)) Console.WriteLine("DEVGL Selected");
            }

            labelCB.Visible = comboCB.Visible = rbtnRetail.Checked;
            chkCleanSMC.Visible = rbtnRetail.Checked || rbtnGlitch.Checked || rbtnGlitch2.Checked || rbtnGlitch2m.Checked || rbtnDevGL.Checked;
            chkCR4.Visible = rbtnGlitch2.Checked || rbtnGlitch2m.Checked;
            chkSMCP.Visible = rbtnGlitch2.Checked || rbtnGlitch2m.Checked;
            chkRgh3.Visible = rbtnGlitch2.Checked || rbtnGlitch2m.Checked;
            chkAudClamp.Visible = rbtnJtag.Checked;
            chkRJtag.Visible = rbtnJtag.Checked;
            chk0Fuse.Visible = rbtnDevGL.Checked;

            checkWBXdkBuild();
            checkBigffs(variables.boardtype);
            checkXLUsb();

            if (!rbtnRetail.Checked && !rbtnGlitch.Checked && !rbtnGlitch2.Checked && !rbtnGlitch2m.Checked && !rbtnDevGL.Checked) chkCleanSMC.Checked = false;

            if (!rbtnGlitch2.Checked && !rbtnGlitch2m.Checked)
            {
                chkCR4.Checked = false;
                chkSMCP.Checked = false;
                chkRgh3.Checked = false;
                chkWB.Checked = false;
                chkWB4G.Checked = false;
            }

            if (!rbtnJtag.Checked)
            {
                chkAudClamp.Checked = false;
                chkRJtag.Checked = false;
            }

            checkRgh3(variables.boardtype);

            if (!rbtnDevGL.Checked) chk0Fuse.Checked = false;

            checkWB(variables.boardtype);

            try
            {
                HackChanged();
            }
            catch (Exception) { }

            updateCommand();
            setComboCB(rbtnRetail.Checked);
        }

        private void btnXeBuildOptions_Click(object sender, EventArgs e)
        {
            XBOptions xb = new XBOptions();
            xb.ShowDialog();
            chkxesettings.Checked = true;
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            DashLaunch myNewForm3 = new DashLaunch();
            myNewForm3.ShowDialog();
        }

        private void txtMBname_Click(object sender, EventArgs e)
        {
            CallMB();
        }

        private void comboDash_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboDash.SelectedIndex == comboDash.Items.Count - 2)
            {
                add_dash();
            }
            else if (comboDash.SelectedIndex == comboDash.Items.Count - 1)
            {
                del_dash();
            }
            else if (comboDash.SelectedIndex == comboDash.Items.Count - 3)
            {
                checkAvailableHackTypes(); // will set all false
            }
            else if (comboDash.SelectedIndex >= 0)
            {
                variables.preferredDash = comboDash.Text;

                variables.dashversion = Convert.ToInt32(comboDash.Text);
                lblDash.Text = comboDash.Text;

            }

            if (comboDash.SelectedIndex < comboDash.Items.Count - 3)
            {
                checkAvailableHackTypes();
            }

            checkWBXdkBuild();
            checkXLUsb();
            updateCommand();
            setComboCB();
        }

        public void checkAvailableHackTypes()
        {
            if (comboDash.SelectedValue == null || comboDash.SelectedValue.ToString().Contains("-"))
            {
                rbtnRetail.Enabled = rbtnRetail.Checked = false;
                rbtnGlitch.Enabled = rbtnGlitch.Checked = false;
                rbtnGlitch2.Enabled = rbtnGlitch2.Checked = false;
                rbtnGlitch2m.Enabled = rbtnGlitch2m.Checked = false;
                rbtnJtag.Enabled = rbtnJtag.Checked = false;
                rbtnDevGL.Enabled = rbtnDevGL.Checked = false;
                return;
            }

            if (variables.debugme) Console.WriteLine(Path.Combine(variables.update_path, comboDash.SelectedValue + "\\_file.ini"));
            if (!File.Exists(Path.Combine(variables.update_path, comboDash.SelectedValue + "\\_glitch2.ini")))
            {
                rbtnGlitch2.Enabled = rbtnGlitch2.Checked = false;
            }
            else
            {
                checkGlitch2(variables.boardtype);
            }

            if (!File.Exists(Path.Combine(variables.update_path, comboDash.SelectedValue + "\\_glitch2m.ini")))
            {
                rbtnGlitch2m.Enabled = rbtnGlitch2m.Checked = false;
            }
            else
            {
                checkGlitch2m(variables.boardtype);
            }

            if (!File.Exists(Path.Combine(variables.update_path, comboDash.SelectedValue + "\\_glitch.ini")))
            {
                rbtnGlitch.Enabled = rbtnGlitch.Checked = false;
            }
            else
            {
                checkGlitch(variables.boardtype);
            }

            if (!File.Exists(Path.Combine(variables.update_path, comboDash.SelectedValue + "\\_jtag.ini")))
            {
                rbtnJtag.Enabled = rbtnJtag.Checked = false;
            }
            else
            {
                checkJtag(variables.boardtype);
            }

            if (!File.Exists(Path.Combine(variables.update_path, comboDash.SelectedValue + "\\_retail.ini")))
            {
                rbtnRetail.Checked = rbtnRetail.Enabled = false;
            }
            else
            {
                rbtnRetail.Enabled = true;
            }
            if (!File.Exists(Path.Combine(variables.update_path, comboDash.SelectedValue + "\\_devgl.ini")))
            {
                rbtnDevGL.Enabled = rbtnDevGL.Checked = false;
            }
            else
            {
                checkDevGL(variables.boardtype);
            }
        }

        private void checkJtag(string board)
        {
            if (board == null) board = "None";
            if (board.Contains("Corona") || board.Contains("Trinity")) rbtnJtag.Enabled = rbtnJtag.Checked = false;
            else rbtnJtag.Enabled = true;
        }

        public void checkGlitch(string board)
        {
            if (board == null) board = "None";
            if (variables.rghable)
            {
                if (board.Contains("Corona") || board.Contains("Trinity")) rbtnGlitch.Enabled = rbtnGlitch.Checked = false;
                else if (!variables.rgh1able) rbtnGlitch.Enabled = rbtnGlitch.Checked = false;
                else rbtnGlitch.Enabled = true;
            }
            else rbtnGlitch.Enabled = rbtnGlitch.Checked = false;
        }

        private void checkGlitch2(string board)
        {
            if (board == null) board = "None";
            //if (board.Contains("Xenon")) rbtnGlitch2.Enabled = rbtnGlitch2.Checked = false;
            //else
            rbtnGlitch2.Enabled = true;
        }

        private void checkGlitch2m(string board)
        {
            if (board == null) board = "None";
            if (variables.dashversion == 17489)
            {
                rbtnGlitch2m.Enabled = true;
            }
            else
            {
                if (board.Contains("Corona") || board.Contains("Trinity") || board.Contains("None")) rbtnGlitch2m.Enabled = true;
                else rbtnGlitch2m.Enabled = rbtnGlitch2m.Checked = false;
            }
        }

        private void checkDevGL(string board)
        {
            if (canDevGL(board)) rbtnDevGL.Enabled = true;
            else rbtnDevGL.Enabled = rbtnDevGL.Checked = false;
        }

        public bool canDevGL(string board)
        {
            if (board == null) board = "None";
            if (File.Exists(Path.Combine(variables.rootfolder, @"xebuild\common\" + "sb_priv.bin")))
            {
                if (board.Contains("Jasper") || board.Contains("Corona") || board.Contains("Trinity") || board.Contains("None")) return true;
                else return false;
            }
            else return false;
        }

        public void checkBigffs(string board)
        {
            if ((rbtnGlitch.Checked || rbtnGlitch2.Checked || rbtnGlitch2m.Checked || rbtnJtag.Checked || rbtnDevGL.Checked) && !chkXdkBuild.Checked)
            {
                if (board == null) board = "None";
                if (board.Contains("Trinity BB")) chkBigffs.Enabled = true;
                else if (board.Contains("Xenon") || board.Contains("Zephyr") || board.Contains("Falcon") || board.Contains("Jasper 16MB") || board.Contains("Jasper SB") || board.Contains("Trinity") || board.Contains("Corona"))
                {
                    chkBigffs.Checked = false;
                    chkBigffs.Enabled = false;
                }
                else
                {
                    chkBigffs.Enabled = true;
                }
            }
            else
            {
                chkBigffs.Checked = false;
                chkBigffs.Enabled = false;
            }
        }

        private void checkXLUsb()
        {
            if (File.Exists(Path.Combine(variables.update_path, comboDash.SelectedValue + @"\bin\xl_usb.bin")))
            {
                if (rbtnRetail.Checked) chkXLUsb.Checked = chkXLUsb.Enabled = false;
                else chkXLUsb.Enabled = true;
            }
            else chkXLUsb.Checked = chkXLUsb.Enabled = false;
        }

        bool chkWB4GVis = false;
        bool chkWB4GEn = true;
        public void checkWBXdkBuild()
        {
            if (rbtnGlitch2m.Checked && variables.dashversion == 17489 && File.Exists(variables.rootfolder + @"\xeBuild\17489\!XDKbuild Only!.txt"))
            {
                chkWB.Visible = false;
                chkWB.Checked = false;
                //chkWB4G.Visible = false;
                // chkWB4G.Checked = false;
                chkWB4GVis = false;
                chkXdkBuild.Visible = true;
                chkXdkBuild.Checked = true;
            }
            else
            {
                if (rbtnGlitch2.Checked || rbtnGlitch2m.Checked)
                {
                    chkWB.Visible = true;
                    //chkWB4G.Visible = true;
                    chkWB4GVis = true;
                }
                else
                {
                    chkWB.Visible = false;
                    chkWB.Checked = false;
                    //chkWB4G.Visible = false;
                    //chkWB4G.Checked = false;
                    chkWB4GVis = false;
                }
                chkXdkBuild.Visible = false;
                chkXdkBuild.Checked = false;
            }
            checkWB4G();
        }

        private void checkWB(string board)
        {
            if (board == null) board = "None";
            if ((board.Contains("Corona") || board.Contains("None")) && (rbtnGlitch2.Checked || rbtnGlitch2m.Checked) && !chkXdkBuild.Checked)
            {
                chkWB.Enabled = true;
                //chkWB4G.Enabled = true;
                chkWB4GEn = true;
            }
            else
            {
                chkWB.Checked = false;
                chkWB.Enabled = false;
                //chkWB4G.Checked = false;
                //chkWB4G.Enabled = false;
                chkWB4GEn = false;
            }
            checkWB4G();
        }

        private void checkWB4G()
        {
            if (chkWB4GVis && chkWB4GEn) chkWB4G.Enabled = true;
            else
            {
                chkWB4G.Checked = false;
                chkWB4G.Enabled = false;
            }
        }

        private void checkDLPatches_CheckedChanged(object sender, EventArgs e)
        {
            variables.DashLaunchE = checkDLPatches.Checked;
            if (!checkDLPatches.Checked || !checkDLPatches.Enabled) { chkLaunch.Visible = false; chkLaunch.Checked = false; }
            else if (checkDLPatches.Checked && checkDLPatches.Enabled) chkLaunch.Visible = true;
        }

        public void setDLPatches(bool checkd)
        {
            checkDLPatches.Checked = chkLaunch.Checked = checkd;
        }

        private void btnDrive_Click(object sender, EventArgs e)
        {
            try
            {
                DriveMode();
            }
            catch (Exception) { }
        }

        private void checkDLPatches_EnabledChanged(object sender, EventArgs e)
        {
            if (!checkDLPatches.Enabled) chkLaunch.Visible = false;
            else if (checkDLPatches.Checked) chkLaunch.Visible = true;
        }

        private void chkListBoxPatches_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selected = chkListBoxPatches.SelectedIndex;
            if (selected >= 0 && selected <= 5)
            {
                if (chkListBoxPatches.GetItemChecked(selected))
                {
                    Console.WriteLine(chkListBoxPatches.Items[selected].ToString() + " Enabled");
                    if (selected == 0) patches[selected + 1] = "-a nofcrt";
                    else if (selected == 1) patches[selected + 1] = "-a noSShdd";
                    else if (selected == 2) patches[selected + 1] = "-a nointmu";
                    else if (selected == 3) patches[selected + 1] = "-a nohdd";
                    else if (selected == 4) patches[selected + 1] = "-a nohdmiwait";
                    else if (selected == 5) patches[selected + 1] = "-a nolan";
                }
                else
                {
                    Console.WriteLine(chkListBoxPatches.Items[selected].ToString() + " Disabled");
                    patches[selected + 1] = "";
                }
            }

            updateCommand();
        }

        private void chkRJtag_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRJtag.Checked) Console.WriteLine("R-JTAG Selected");
            else Console.WriteLine("R-JTAG Deselected");
        }

        // Handling checkboxes allows us to only have one selected at time without extra stuff happening
        private void chkCleanSMC_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCleanSMC.Checked)
            {
                Console.WriteLine("Clean SMC Selected");
                chkCR4.Checked = false;
                chkSMCP.Checked = false;
                chkRgh3.Checked = false;
            }
            else if (!chkCR4.Checked && !chkSMCP.Checked && !chkRgh3.Checked) // Don't uselessly spam the console
            {
                Console.WriteLine("Clean SMC Deselected");
            }

            if (chkCleanSMC.Checked)
            {
                if (File.Exists(Path.Combine(variables.rootfolder, @"xebuild\data\" + "smc.bin")))
                {
                    if (MessageBox.Show("smc.bin found. Delete it?\nUnless you put it there, delete it!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        File.Delete(Path.Combine(variables.rootfolder, @"xebuild\data\" + "smc.bin"));
                    }
                }
            }
        }

        private void chkCR4_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCR4.Checked)
            {
                Console.WriteLine("CR4 Selected");
                chkCleanSMC.Checked = false;
                chkSMCP.Checked = false;
                chkRgh3.Checked = false;
            }
            else if (!chkCleanSMC.Checked && !chkSMCP.Checked && !chkRgh3.Checked) // Don't uselessly spam the console
            {
                Console.WriteLine("CR4 Deselected");
            }

            if (chkCR4.Checked)
            {
                if (File.Exists(Path.Combine(variables.rootfolder, @"xebuild\data\" + "smc.bin")))
                {
                    if (MessageBox.Show("smc.bin found. Delete it?\nUnless you put it there, delete it!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        File.Delete(Path.Combine(variables.rootfolder, @"xebuild\data\" + "smc.bin"));
                    }
                }
            }
        }

        private void chkSMCP_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSMCP.Checked)
            {
                Console.WriteLine("SMC+ Selected");
                chkCleanSMC.Checked = false;
                chkCR4.Checked = false;
                chkRgh3.Checked = false;
            }
            else if (!chkCleanSMC.Checked && !chkCR4.Checked && !chkRgh3.Checked) // Don't uselessly spam the console
            {
                Console.WriteLine("SMC+ Deselected");
            }

            if (chkSMCP.Checked)
            {
                if (File.Exists(Path.Combine(variables.rootfolder, @"xebuild\data\" + "smc.bin")))
                {
                    if (MessageBox.Show("smc.bin found. Delete it?\nUnless you put it there, delete it!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        File.Delete(Path.Combine(variables.rootfolder, @"xebuild\data\" + "smc.bin"));
                    }
                }
            }
        }

        private void chkRgh3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRgh3.Checked)
            {
                Console.WriteLine("RGH3 Selected");
                chkCleanSMC.Checked = false;
                chkCR4.Checked = false;
                chkSMCP.Checked = false;
            }
            else if (!chkCleanSMC.Checked && !chkCR4.Checked && !chkSMCP.Checked) // Don't uselessly spam the console
            {
                Console.WriteLine("RGH3 Deselected");
            }

            if (chkRgh3.Checked)
            {
                if (File.Exists(Path.Combine(variables.rootfolder, @"xebuild\data\" + "smc.bin")))
                {
                    if (MessageBox.Show("smc.bin found. Delete it?\nUnless you put it there, delete it!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        File.Delete(Path.Combine(variables.rootfolder, @"xebuild\data\" + "smc.bin"));
                    }
                }
            }
            else Rgh3Mhz.SelectedIndex = 1;

            checkRgh3(variables.boardtype);
        }

        private void checkRgh3(string board)
        {
            if (board == null) board = "None";
            if (board.Contains("Falcon") || board.Contains("Jasper"))
            {
                Rgh3Label.Visible = Rgh3Mhz.Visible = chkRgh3.Checked;
                Rgh3Label2.Visible = false;
            }
            else if (board.Contains("Trinity"))
            {
                Rgh3Label2.Text = "27";
                Rgh3Label.Visible = Rgh3Label2.Visible = chkRgh3.Checked;
                Rgh3Mhz.Visible = false;
            }
            else if (board.Contains("Corona"))
            {
                Rgh3Label2.Text = "25";
                Rgh3Label.Visible = Rgh3Label2.Visible = chkRgh3.Checked;
                Rgh3Mhz.Visible = false;
            }
            else
            {
                Rgh3Label.Visible = Rgh3Label2.Visible = Rgh3Mhz.Visible = false;
            }

            if (board.Contains("Xenon") || board.Contains("Zephyr") || board.Contains("Jasper SB") || board.Contains("Trinity BB"))
            {
                chkRgh3.Checked = false;
                chkRgh3.Enabled = false;
            }
            else chkRgh3.Enabled = true;
        }

        private void chkWB_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWB.Checked)
            {
                Console.WriteLine("Winbond 2K Selected");
                chkWB4G.Checked = false;
            }
            else if (!chkWB4G.Checked) // Don't uselessly spam the console
            {
                Console.WriteLine("Winbond 2K Deselected");
            }

            // Don't do it twice
            if (!chkWB4G.Checked && chkWB.Checked)
            {
                updateWB();
            }
            else if (!chkWB.Checked && !chkWB4G.Checked)
            {
                updateWB();
            }
        }

        private void chkWB4G_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWB4G.Checked)
            {
                MessageBox.Show("Warning: This function is for advanced users only\n\nIf you don't understand what this is for, use WB 2K on the XeBuild tab instead", "Steep Hill Ahead", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Console.WriteLine("Winbond 2K Buffer Selected");
                chkWB.Checked = false;
            }
            else if (!chkWB.Checked) // Don't uselessly spam the console
            {
                Console.WriteLine("Winbond 2K Buffer Deselected");
            }

            // Don't do it twice
            if (!chkWB.Checked && chkWB4G.Checked)
            {
                updateWB();
            }
            else if (!chkWB.Checked && !chkWB4G.Checked)
            {
                updateWB();
            }
        }

        private void updateWB()
        {
            if (chkWB.Checked) patches[7] = "-r WB";
            else if (chkWB4G.Checked) patches[7] = "-r WB4G";
            else patches[7] = "";

            updateCommand();
        }

        private void chkBigffs_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBigffs.Checked) Console.WriteLine("bigffs Selected");
            else Console.WriteLine("bigffs Deselected");
        }

        private void chkXdkBuild_CheckedChanged(object sender, EventArgs e)
        {
            checkWB(variables.boardtype);
            checkBigffs(variables.boardtype);
            if (chkXdkBuild.Checked) Console.WriteLine("XDKbuild Selected");
            else Console.WriteLine("XDKbuild Deselected");
            checkRgh3(variables.boardtype);
        }

        private void chkAudClamp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAudClamp.Checked) Console.WriteLine("Aud_Clamp Selected");
            else Console.WriteLine("Aud_Clamp Deselected");
        }

        private void chk0Fuse_CheckedChanged(object sender, EventArgs e)
        {
            if (chk0Fuse.Checked) Console.WriteLine("0 Fuse Selected");
            else Console.WriteLine("0 Fuse Deselected");
        }

        private void chkXLUsb_CheckedChanged(object sender, EventArgs e)
        {
            if (chkXLUsb.Checked)
            {
                Console.WriteLine("XL USB (BETA) Selected");
                if (DialogResult.Cancel == MessageBox.Show("XL USB requires HDDs to be formatted via FATXplorer, normal Xbox 360 storage devices will no longer work", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                {
                    chkXLUsb.Checked = false;
                }
            }
            else Console.WriteLine("XL USB (BETA) Deselected");
        }

        private void btnGetMB_Click(object sender, EventArgs e)
        {
            Getmb();
        }

        private void btnXEUpdate_Click(object sender, EventArgs e)
        {
            ThreadStart starter = delegate { xe_update(); };
            new Thread(starter).Start();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(variables.outfolder, "consoleDump.bin")))
            {
                MessageBox.Show("consoleDump.bin already exists");
                return;
            }
            string arguments;
            if (!String.IsNullOrWhiteSpace(txtOffset.Text))
            {
                arguments = "-rb " + "\"" + Path.Combine(variables.outfolder, "consoleDump.bin") + "\"";
                arguments += " " + txtOffset.Text;
                if (!String.IsNullOrWhiteSpace(txtLength.Text))
                {
                    arguments += " " + txtLength.Text;
                }
            }
            else
            {
                arguments = "-r " + "\"" + Path.Combine(variables.outfolder, "consoleDump.bin") + "\"";
            }

            if (chkShutdown.Checked) arguments += " -s";
            if (chkReboot.Checked) arguments += " -reboot";
            if (chkForceIP.Checked) arguments += " -ip " + txtIP2.Text;
            Console.WriteLine("Starting Read - please wait......");
            ThreadStart starter = delegate { xe_client(arguments); };
            new Thread(starter).Start();

        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            string arguments;
            if (!String.IsNullOrWhiteSpace(txtOffset.Text))
            {
                arguments = "-wb " + "\"" + variables.filename1 + "\"";
                arguments += " " + txtOffset.Text;
                if (!String.IsNullOrWhiteSpace(txtLength.Text))
                {
                    arguments += " " + txtLength.Text;
                }
            }
            else
            {
                arguments = "-w " + "\"" + variables.filename1 + "\"";
            }
            if (chkShutdown.Checked) arguments += " -s";
            if (chkReboot.Checked) arguments += " -reboot";
            if (chkForceIP.Checked) arguments += " -ip " + txtIP2.Text;
            Console.WriteLine("Starting Write - please wait......");
            ThreadStart starter = delegate { xe_client(arguments); };
            new Thread(starter).Start();

        }

        private void btnErase_Click(object sender, EventArgs e)
        {
            string arguments = "-eb " + txtOffset.Text;
            if (chkShutdown.Checked) arguments += " -s";
            if (chkReboot.Checked) arguments += " -reboot";
            if (chkForceIP.Checked) arguments += " -ip " + txtIP2.Text;

            ThreadStart starter = delegate { xe_client(arguments); };
            new Thread(starter).Start();
        }

        private void btnPatches_Click(object sender, EventArgs e)
        {
            string arguments = "-p";
            if (!String.IsNullOrWhiteSpace(variables.filename1))
            {
                if (MessageBox.Show("Make sure that source file is a patch file.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel) return;
                arguments += " \"" + variables.filename1 + "\"";
            }
            if (chkShutdown.Checked) arguments += " -s";
            if (chkReboot.Checked) arguments += " -reboot";
            if (chkForceIP.Checked) arguments += " -ip " + txtIP2.Text;

            ThreadStart starter = delegate { xe_client(arguments); };
            new Thread(starter).Start();
        }

        private void btnAvatar_Click(object sender, EventArgs e)
        {
            if (variables.debugme) Console.WriteLine(Path.Combine(variables.update_path, comboDash.Text + @"\$systemupdate"));
            if (Directory.Exists(Path.Combine(variables.update_path, comboDash.Text + @"\$systemupdate")))
            {
                Console.WriteLine("Starting, please wait!");
                string upPath = Path.Combine(variables.update_path, comboDash.Text, @"\$systemupdate");
                //Path.Combine(upPath, @"\$systemupdate");
                // Console.WriteLine(Path.Combine(variables.update_path, comboDash.Text + @"\$systemupdate"));
                ThreadStart starter = delegate { xe_compatibilityAvatar(Path.Combine(variables.update_path, (comboDash.Text)), "-e "); };
                new Thread(starter).Start();
            }
            else
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                if (fd.ShowDialog() == DialogResult.Cancel) return;
                Console.WriteLine("Starting, please wait!");
                ThreadStart starter = delegate { xe_compatibilityAvatar(fd.SelectedPath, "-e "); };
                new Thread(starter).Start();
            }
        }

        private void btnComp_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == DialogResult.Cancel) return;


            ThreadStart starter = delegate { xe_compatibilityAvatar(fd.SelectedPath, "-c "); };
            new Thread(starter).Start();
        }

        private void chkForceIP_CheckedChanged(object sender, EventArgs e)
        {
            if (chkForceIP.Checked)
            {
                txtIP.Enabled = txtIP2.Enabled = chkForceIP2.Checked = true;
                txtIP.Text = txtIP2.Text = "";
                Console.WriteLine("ForceIP Selected");

            }
            else
            {
                txtIP.Enabled = txtIP2.Enabled = chkForceIP2.Checked = false;
                txtIP.Text = txtIP2.Text = "Autoscan LAN";
                Console.WriteLine("ForceIP Deselected");
            }
        }

        private void txtIP2_TextChanged(object sender, EventArgs e)
        {
            txtIP.Text = txtIP2.Text;
        }

        private void chkForceIP2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkForceIP2.Checked)
            {
                txtIP.Enabled = txtIP2.Enabled = chkForceIP.Checked = true;
                txtIP.Text = txtIP2.Text = "";
            }
            else
            {
                txtIP.Enabled = txtIP2.Enabled = chkForceIP.Checked = false;
                txtIP.Text = txtIP2.Text = "Autoscan LAN";
            }
        }

        private void txtIP_TextChanged(object sender, EventArgs e)
        {
            txtIP2.Text = txtIP.Text;
        }

        private void XeBuildPanel_Load(object sender, EventArgs e)
        {
            chkCR4.Visible = false;
            chkSMCP.Visible = false;
            chkRgh3.Visible = false;
            Rgh3Label.Visible = false;
            Rgh3Label2.Visible = false;
            Rgh3Mhz.Visible = false;
            chkWB.Visible = false;
            chkWB4G.Enabled = false;
            chkXdkBuild.Visible = false;
            chkRJtag.Visible = false;
            chkAudClamp.Visible = false;
            chkBigffs.Enabled = false;
            chk0Fuse.Visible = false;

            setComboCB();
        }

        private void chkNoWrite_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNoWrite.Checked) Console.WriteLine("nowrite Selected");
            else Console.WriteLine("nowrite Deselected");
        }

        private void chkNoAva_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNoAva.Checked) Console.WriteLine("noava Selected");
            else Console.WriteLine("noava Deselected");
        }

        private void chkClean_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClean.Checked) Console.WriteLine("clean Selected");
            else Console.WriteLine("clean Deselected");
        }

        private void chkNoReeb_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNoReeb.Checked) Console.WriteLine("noreeb Selected");
            else Console.WriteLine("noreeb Deselected");
        }

        private void chkShutdown_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShutdown.Checked) Console.WriteLine("shutdown Selected");
            else Console.WriteLine("shutdown Deselected");
        }

        private void chkReboot_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReboot.Checked) Console.WriteLine("reboot Selected");
            else Console.WriteLine("reboot Deselected");
        }

        private void chkxesettings_CheckedChanged(object sender, EventArgs e)
        {
            if (chkxesettings.Checked) Console.WriteLine("Use Edited Options Selected");
            else Console.WriteLine("Use Edited Options Deselected");
        }

        #endregion

        #region code

        void xe_update()
        {
            Classes.xebuild xe = new Classes.xebuild();
            xe.Uloadvariables(variables.dashversion, (variables.hacktypes)variables.ttyp, patches, chkxesettings.Checked, chkNoWrite.Checked, chkNoAva.Checked, chkClean.Checked, chkNoReeb.Checked, checkDLPatches.Checked,
                chkLaunch.Checked);
            File.Delete(Path.Combine(variables.rootfolder, @"xebuild\data\" + "smc.bin"));
            try
            {
                string[] files = { "kv.bin", "smc.bin", "smc_config.bin", "fcrt.bin" };
                foreach (string file in files)
                {
                    if (File.Exists(Path.Combine(variables.rootfolder, @"xebuild\data\" + file)))
                    {
                        if (MessageBox.Show(file + " found. Delete it?\nUnless you put it there, delete it!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            File.Delete(Path.Combine(variables.rootfolder, @"xebuild\data\" + file));
                        }
                    }
                }
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }

            Classes.xebuild.XebuildError er = xe.createxebuild();
            if (er == Classes.xebuild.XebuildError.none)
            {
                xe.xeExit += xe_xeUExit;
                xe.update();
            }
            else if (er == Classes.xebuild.XebuildError.nodash)
            {
                MessageBox.Show("No Kernel Selected");
                return;
            }
            else
            {
                MessageBox.Show("Something Bad Happened");
                return;
            }
        }

        void xe_client(string arguments)
        {
            Classes.xebuild xe = new Classes.xebuild();
            xe.client(arguments);
        }

        void xe_compatibilityAvatar(string path, string command)
        {
            Classes.xebuild xe = new Classes.xebuild();
            string arguments = command + path + "\\";
            if (chkShutdown.Checked) arguments += " -s";
            if (chkReboot.Checked) arguments += " -reboot";
            if (chkForceIP.Checked) arguments += " -ip " + txtIP2.Text;

            xe.client(arguments);
        }

        void add_dash()
        {
            addDash newdash = new addDash();
            if (newdash.ShowDialog() == DialogResult.Cancel)
            {
                DeletedDash(); // Yes this is correct, it just refreshes the list
                return;
            }
            try
            {
                AddedDash();
            }
            catch (Exception) { }
        }
        void del_dash()
        {
            Dashes.delDash deldash = new Dashes.delDash();
            deldash.ShowDialog();
            try
            {
                DeletedDash();
            }
            catch (Exception) { }
        }

        public void createxebuild_v2(bool custom, Nand.PrivateN nand, bool fullDataClean)
        {
            Classes.xebuild xe = new Classes.xebuild();
            xe.loadvariables(nand._cpukey, (variables.hacktypes)variables.ttyp, variables.dashversion,
                             variables.ctyp, patches, nand, chkxesettings.Checked, checkDLPatches.Checked,
                             chkLaunch.Checked, chkAudClamp.Checked, chkRJtag.Checked, chkCleanSMC.Checked, chkCR4.Checked, chkSMCP.Checked, chkRgh3.Checked, chkBigffs.Checked, chk0Fuse.Checked, chkXdkBuild.Checked, chkXLUsb.Checked, fullDataClean);

            string ini = (variables.launchpath + @"\" + variables.dashversion + @"\_" + variables.ttyp + ".ini");

            if (!custom)
            {
                if (String.IsNullOrWhiteSpace(variables.filename1))
                {
                    loadFil(ref variables.filename1, true);
                    if (String.IsNullOrWhiteSpace(variables.filename1))
                    {
                        MessageBox.Show("No file was selected!", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (!File.Exists(variables.filename1))
                {
                    MessageBox.Show("File is missing. Ensure it wasn't moved and app can access it.", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Path.GetExtension(variables.filename1) != ".bin")
                {
                    MessageBox.Show("You must select a .bin file", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return;
                }
                try
                {
                    string[] files = { "kv.bin", "smc.bin", "smc_config.bin", "fcrt.bin" };
                    foreach (string file in files)
                    {
                        if (File.Exists(Path.Combine(variables.rootfolder, @"xebuild\data\" + file)))
                        {
                            if (MessageBox.Show(file + " found. Delete it?\nUnless you put it there, delete it!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            {
                                File.Delete(Path.Combine(variables.rootfolder, @"xebuild\data\" + file));
                            }
                        }
                    }
                }
                catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
                if (!nand.cpukeyverification(nand._cpukey))
                {
                    Console.WriteLine("Wrong CPU Key");
                    return;
                }
            }
            else
            {
                string[] filesa = { "kv.bin", "smc.bin", "smc_config.bin" };
                foreach (string file in filesa)
                {
                    if (file == "smc.bin" && (chkCleanSMC.Checked || chkCR4.Checked || chkSMCP.Checked || rbtnJtag.Checked)) // Options that put in an SMC
                    {
                        // Skip because we put our own SMC in
                    }
                    else
                    {
                        if (!File.Exists(Path.Combine(variables.rootfolder, @"xebuild\data\" + file)))
                        {
                            MessageBox.Show(file + " is missing", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                Regex objAlphaPattern = new Regex("[a-fA-F0-9]{32}$");
                bool sts = objAlphaPattern.IsMatch(variables.cpkey);
                if ((variables.cpkey.Length == 32 && sts))
                {
                    if (variables.debugme) Console.WriteLine("Key verification");
                    long size = 0;
                    if (Nand.Nand.cpukeyverification(Oper.openfile(Path.Combine(variables.rootfolder, @"xebuild\data\kv.bin"), ref size, 0), variables.cpkey))
                    {
                        if (variables.debugme) Console.WriteLine("CPU Key is Correct");
                        if (Nand.Nand.getfcrtflag(File.ReadAllBytes(Path.Combine(variables.rootfolder, @"xebuild\data\kv.bin")), variables.cpkey))
                        {
                            if (!File.Exists(Path.Combine(variables.rootfolder, @"xebuild\data\fcrt.bin")))
                            {
                                MessageBox.Show("fcrt.bin is missing");
                                Process.Start(Path.Combine(variables.rootfolder, @"xebuild\data"));
                                return;
                            }
                        }
                    }
                    else Console.WriteLine("Wrong CPU Key");
                }

                string cba = "", cbb = "";
                string[] files = parse_ini.parselabel(ini, variables.ctyp.Ini + "bl");
                if (files.Length >= 2)
                {
                    if (files[0].Contains("cb")) cba = files[0].Substring(files[0].IndexOf("_") + 1, files[0].IndexOf(".bin") - 4);
                    if (files[1].Contains("cbb")) cbb = files[1].Substring(files[1].IndexOf("cbb_") + 4, files[1].IndexOf(".bin") - 4);
                }

                if (variables.changeldv == 0)
                {
                    Forms.xeBuildOptions ldv = new Forms.xeBuildOptions();
                    ldv.enumeratecbs(cba, cbb);
                    ldv.ShowDialog();
                }
            }

        Start:
            switch (xe.createxebuild(custom))
            {
                case Classes.xebuild.XebuildError.nocpukey:
                    MessageBox.Show("CPU Key is Missing");
                    return;
                case Classes.xebuild.XebuildError.nodash:
                    MessageBox.Show("No Kernel Selected");
                    return;
                case Classes.xebuild.XebuildError.noinis:
                    MessageBox.Show("Ini's are Missing");
                    return;
                case Classes.xebuild.XebuildError.nobootloaders:
                    Console.WriteLine("The specified console bootloader list ({0}) is missing from the ini ({1})", variables.ctyp.Ini + "bl", ini);
                    Console.WriteLine("You can either add it manually or ask for it get added if its possible");
                    return;
                case Classes.xebuild.XebuildError.wrongcpukey:
                    MessageBox.Show("Wrong CPU Key");
                    return;
                case Classes.xebuild.XebuildError.noconsole:
                    variables.ctyp = callconsoletypes(ConsoleTypes.Selected.All);
                    if (variables.ctyp.ID == -1) return;
                    else
                    {
                        Console.WriteLine((variables.hacktypes)variables.ttyp);
                        xe.loadvariables(nand._cpukey, (variables.hacktypes)variables.ttyp, variables.dashversion,
                            variables.ctyp, patches, nand, chkxesettings.Checked, checkDLPatches.Checked,
                            chkLaunch.Checked, chkAudClamp.Checked, chkRJtag.Checked, chkCleanSMC.Checked,
                            chkCR4.Checked, chkSMCP.Checked, chkRgh3.Checked, chkBigffs.Checked, chk0Fuse.Checked, chkXdkBuild.Checked, chkXLUsb.Checked, fullDataClean);
                        goto Start;
                    }
                case Classes.xebuild.XebuildError.none:
                    copyfiles(nand._cpukey);
                    xe.xeExit += xe_xeExit;
                    xe.build();
                    break;
                default:
                    break;
            }

        }

        public void xe_xeExit(object sender, EventArgs e)
        {
            xeExitActual();
        }

        public void xeExitActual()
        {
            variables.changeldv = 0;
            UpdateProgres(100);

            try
            {
                File.Copy(Path.Combine(variables.rootfolder, @"xebuild\options.ini"), Path.Combine(variables.rootfolder, @"xebuild\data\options.ini"), true);
                chkxesettings.Checked = false;
                File.Move(Path.Combine(variables.xefolder, variables.nandflash + ".log"), Path.Combine(variables.xefolder, variables.nandflash.Substring(0, variables.nandflash.IndexOf(".")) + "(" + DateTime.Now.ToString("ddMMyyyyHHmm") + ").bin.log"));
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }

            try
            {
                if (File.Exists(Path.Combine(variables.rootfolder, @"build.log"))) File.Delete(Path.Combine(variables.rootfolder, @"build.log"));
            }
            catch { }

            if (variables.xefinished)
            {
                Console.WriteLine("Saved to {0}", variables.xefolder);
                Console.WriteLine("Image is Ready");
                variables.filename1 = Path.Combine(variables.xefolder, variables.nandflash);
                updateSourc(variables.filename1);
                //Process.Start(variables.xefolder);
            }
            else
            {
                Console.WriteLine("Failed");
            }

            try
            {
                delfiles();
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            if (variables.debugme) Console.WriteLine("Deleted Files Successfully");
            variables.xefinished = false;
        }

        private void copyfiles(string cpukey)
        {
            string targetkey = System.IO.Path.Combine(variables.xePath, variables.cpukeypath);
            string targetnand = System.IO.Path.Combine(variables.xePath, variables.nanddump);
            File.WriteAllText(targetkey, cpukey);
            if (String.IsNullOrEmpty(variables.filename1)) return;
            //
            FileInfo fi = new FileInfo(variables.filename1);
            if (fi.Length == 0xE0400000)
            {
                if (MessageBox.Show("Copy all 4GB data?", "Copy", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    JRunner.Functions.Copy c = new JRunner.Functions.Copy(variables.filename1, targetnand);
                    c.ShowDialog();
                }
                else
                {

                    try
                    {
                        FileStream fr = new FileStream(variables.filename1, FileMode.Open);
                        FileStream fw = new FileStream(targetnand, FileMode.Create);
                        int buffersize = 0x200;
                        byte[] buffer = new byte[buffersize];
                        for (int i = 0; i < 0x3000000; i += buffersize)
                        {
                            fr.Read(buffer, 0, buffersize);
                            fw.Write(buffer, 0, buffersize);
                        }
                        fr.Close();
                        fw.Close();
                    }
                    catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }

                }
            }
            /**/
            else /**/File.Copy(variables.filename1, targetnand, true);
        }
        private void delfiles()
        {
            if (File.Exists(variables.xePath + variables.nanddump))
            {
                try
                {
                    File.Delete(variables.xePath + variables.nanddump);
                    if (variables.debugme) Console.WriteLine("Deleted {0}", variables.xePath + variables.nanddump);
                }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
            if (File.Exists(variables.xePath + variables.cpukeypath))
            {
                try
                { File.Delete(variables.xePath + variables.cpukeypath); if (variables.debugme) Console.WriteLine("Deleted {0}", variables.xePath + variables.cpukeypath); }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
            if (File.Exists(variables.launchpath + @"\" + variables.dashversion + @"\launch.ini"))
            {
                try
                {
                    File.Delete(variables.launchpath + @"\" + variables.dashversion + @"\launch.ini");
                    if (variables.debugme) Console.WriteLine("Deleted launch.ini");
                }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
            if (File.Exists(Path.Combine(variables.xePath, "SMC.bin")) && (variables.copiedSMC || variables.fullDataClean)) // Only Delete SMCs it puts there
            {
                try
                {
                    File.Delete(Path.Combine(variables.xePath, "SMC.bin"));
                    if (variables.debugme) Console.WriteLine("Deleted SMC.bin");
                }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
            if (File.Exists(Path.Combine(variables.xePath, "KV.bin")) && variables.fullDataClean)
            {
                try
                {
                    File.Delete(Path.Combine(variables.xePath, "KV.bin"));
                    if (variables.debugme) Console.WriteLine("Deleted KV.bin");
                }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
            if (File.Exists(Path.Combine(variables.xePath, "fcrt.bin")) && variables.fullDataClean)
            {
                try
                {
                    File.Delete(Path.Combine(variables.xePath, "fcrt.bin"));
                    if (variables.debugme) Console.WriteLine("Deleted fcrt.bin");
                }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
            if (File.Exists(Path.Combine(variables.xePath, "smc_config.bin")) && variables.fullDataClean)
            {
                try
                {
                    File.Delete(Path.Combine(variables.xePath, "smc_config.bin"));
                    if (variables.debugme) Console.WriteLine("Deleted KV.bin");
                }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
            if (File.Exists(Path.Combine(variables.update_path, comboDash.SelectedValue + @"\xam.xex")) && variables.copiedXLUsb)
            {
                try
                {
                    File.Delete(Path.Combine(variables.update_path, comboDash.SelectedValue + @"\xam.xex"));
                    if (variables.debugme) Console.WriteLine("Deleted XL USB xam.xex");
                    if (File.Exists(Path.Combine(variables.update_path, comboDash.SelectedValue + @"\xam.xex.tmp")))
                    {
                        File.Move(Path.Combine(variables.update_path, comboDash.SelectedValue + @"\xam.xex.tmp"), Path.Combine(variables.update_path, comboDash.SelectedValue + @"\xam.xex"));
                    }
                    if (variables.debugme) Console.WriteLine("Restored non XL USB xam.xex");

                    string buildIni = Path.Combine(variables.update_path, comboDash.SelectedValue + @"\_" + variables.ttyp.ToString() + ".ini");
                    if (File.Exists(buildIni + ".tmp"))
                    {
                        File.Delete(buildIni);
                        File.Move(buildIni + ".tmp", buildIni);
                    }
                    if (variables.debugme) Console.WriteLine("Restored non XL USB ini");
                }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
        }

        consoles callconsoletypes(ConsoleTypes.Selected selec, bool twomb = false, bool full = false)
        {
            ConsoleTypes myNewForm = new ConsoleTypes();
            myNewForm.sel = selec;
            myNewForm.twombread = twomb;
            myNewForm.sfulldump = full;
            myNewForm.ShowDialog();
            if (myNewForm.DialogResult == DialogResult.Cancel) return (variables.ctypes[0]);
            if (myNewForm.heResult().ID == -1) return variables.ctypes[0];
            variables.fulldump = myNewForm.fulldump();
            variables.twombread = myNewForm.twombdump();
            if (variables.debugme) Console.WriteLine("fulldump variable = {0}", variables.fulldump);
            setMBname(myNewForm.heResult().Text);
            return (myNewForm.heResult());
        }

        public void xe_xeUExit(object sender, EventArgs e)
        {
            variables.changeldv = 0;
            UpdateProgres(100);

            if (variables.xefinished)
            {
                Console.WriteLine("Saved to {0}", variables.xefolder);
                Console.WriteLine("Image is Ready");
                variables.filename1 = Path.Combine(variables.xefolder, variables.nandflash);
                updateSourc(variables.filename1);
                //Process.Start(variables.xefolder);
            }
            else
            {
                Console.WriteLine("Failed");
            }
            variables.xefinished = false;
        }

        #endregion

        private void btnInfo_Click(object sender, EventArgs e)
        {
            ThreadStart starter = delegate { xe_compatibilityAvatar(variables.outfolder, "-i "); };
            new Thread(starter).Start();
        }

        private void updateCommand(bool wait = false)
        {
            if (wait) Thread.Sleep(100);
            string c = "";
            c = "-t " + variables.ttyp;
            c += " -c " + variables.ctyp.XeBuild;
            foreach (String patch in patches)
            {
                c += " " + patch;
            }
            c += " -f " + variables.dashversion;
            c += " -d data";
            c += " \"" + variables.xefolder + "\\" + variables.nandflash + "\" ";

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            c = regex.Replace(c, @" ");

            try
            {
                txtCommand.Text = c;
            }
            catch (Exception) { }
        }

        private void txtMBname_TextChanged(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(delegate { updateCommand(true); })).Start();
            new Thread(new ThreadStart(delegate { setComboCB(false, true); })).Start();
        }

        private void setComboCB(bool erase = false, bool wait = false)
        {
            if (erase)
            {
                patches[7] = "";
                return;
            }
            if (wait) Thread.Sleep(100);
            try
            {
                comboCB.Items.Clear();
                if (variables.dashversion != 0)
                {
                    string ini = (variables.launchpath + @"\" + variables.dashversion + @"\_retail.ini");
                    List<string> labels = parse_ini.getlabels(ini);

                    foreach (string s in labels)
                    {
                        if (!s.Contains("bl")) continue;
                        if (variables.ctyp.ID == -1)
                        {
                            if (s.Contains("_")) comboCB.Items.Add(new CB(s.Substring(s.IndexOf("_") + 1), true));
                            else
                            {
                                comboCB.Items.Add(new CB(Nand.ntable.getCBFromDash(getConsoleFromIni(s.Substring(0, s.IndexOf("bl"))), variables.dashversion), false));
                            }
                        }
                        else
                        {
                            if (s.Contains(variables.ctyp.Ini))
                            {
                                if (s.Contains("_")) comboCB.Items.Add(new CB(s.Substring(s.IndexOf("_") + 1), true));
                                else comboCB.Items.Add(new CB(Nand.ntable.getCBFromDash(getConsoleFromIni(variables.ctyp.Ini), variables.dashversion), false));
                            }
                        }
                    }

                    if (comboCB.Items.Count > 0) comboCB.SelectedIndex = 0;
                }
            }
            catch (Exception) { }
        }

        private void comboCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboCB.Items.Count > 0)
            {
                CB c = (CB)comboCB.SelectedItem;
                if (c.Patch) patches[7] = "-r " + c.Version;
                else patches[7] = "";
                updateCommand();
            }
        }

        private consoles getConsoleFromIni(string ini)
        {
            foreach (consoles c in variables.ctypes)
            {
                if (c.ID == -1) continue;
                if (ini == c.Ini) return c;
            }
            return variables.ctypes[0];
        }

        class CB
        {
            public string Version { get; set; }
            public bool Patch { get; set; }

            public CB(string v, bool p)
            {
                Version = v;
                Patch = p;
            }

            public CB(int v, bool p)
            {
                Version = v.ToString();
                Patch = p;
            }

            public override string ToString()
            {
                return Version;
            }
        }
    }
}
