﻿using System;
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
        List<CB> cbList;
        List<string> patches = new List<string>(new string[8]);
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
            checkDevGL();
        }

        #region delegates
        public delegate void ModeDrive();
        public event ModeDrive DriveMode;
        #endregion

        #region getters/setters
        public DataSet1 getDashDataSet()
        {
            return dashDataSet;
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
        public void setXeSettingsChecked(bool check)
        {
            if (check && !chkXeSettings.Enabled) return;
            chkXeSettings.Checked = check;
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
        public void setWBChecked(bool check)
        {
            if (check && (!chkWB.Enabled || !chkWB.Visible)) return;
            chkWB.Checked = check;
        }
        public void setCleanSMCChecked(bool check)
        {
            if (check && (!chkCleanSMC.Enabled || !chkCleanSMC.Visible)) return;
            chkCleanSMC.Checked = check;
        }
        public void setRgh3Checked(bool check)
        {
            if (check && (!chkRgh3.Enabled || !chkRgh3.Visible)) return;
            chkRgh3.Checked = check;
        }
        public void setXLUSBChecked(bool check)
        {
            if (check && !chkXLUsb.Enabled) return;
            chkXLUsb.Checked = check;
        }
        public void setXLHDDChecked(bool check)
        {
            if (check && !chkXLHdd.Enabled) return;
            chkXLHdd.Checked = check;
        }
        public void setXLBothChecked(bool check)
        {
            if (check && !chkXLBoth.Enabled) return;
            chkXLBoth.Checked = check;
        }
        public void setUsbdSecChecked(bool check)
        {
            if (check && !chkUsbdSec.Enabled) return;
            chkUsbdSec.Checked = check;
        }
        public void setCoronaKeyFixChecked(bool check)
        {
            if (check && !chkCoronaKeyFix.Enabled) return;
            chkCoronaKeyFix.Checked = check;
        }
        public void setNoFcrtChecked(bool check)
        {
            chkListBoxPatches.SetItemChecked(0, check);
        }

        public void initTabs() // Only call once after settings load
        {
            if (variables.showAdvancedTabs)
            {
                btnShowAdvanced.Text = "Hide Advanced Tabs";
            }
            else
            {
                MainTabs.TabPages.Remove(tabClient);
                MainTabs.TabPages.Remove(tabUpdate);
            }
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
            MainTabs.SelectedTab = tabXeBuild;
            for (int i = 0; i < chkListBoxPatches.Items.Count; i++)
            {
                chkListBoxPatches.SetItemChecked(i, false);
            }
            chkXeSettings.Checked = false;
        }

        public void setMBname(string txt)
        {
            txtMBname.BeginInvoke(new Action(() => {
                txtMBname.Text = txt;
                variables.boardtype = txt;
                checkAvailableHackTypes();
                checkWB(txt);
                checkBigffs(txt);
                checkDashAndConsoleSpecificPatches(txt);

                if (txt.Contains("Xenon") || txt.Contains("Winchester"))
                {
                    chkCR4.Checked = false;
                    chkCR4.Enabled = false;
                    chkSMCP.Checked = false;
                    chkSMCP.Enabled = false;
                }
                else
                {
                    chkCR4.Enabled = true;
                    chkSMCP.Enabled = true;
                }

                if (txt.Contains("Xenon"))
                {
                    chkAudClamp.Checked = false;
                    chkAudClamp.Enabled = false;
                }
                else chkAudClamp.Enabled = true;

                checkRgh3(txt);
            }));
        }
        #endregion

        #region UI

        private void rbtn_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnRetail.Checked)
            {
                if (checkDLPatches.Checked) variables.DashlaunchE = checkDLPatches.Checked;
                checkDLPatches.Enabled = chkLaunch.Enabled = false;
                if (sender.Equals(rbtnRetail)) Console.WriteLine("Retail selected");
            }
            else if (rbtnJtag.Checked)
            {
                checkDLPatches.Enabled = true;
                checkDLPatches.Checked = chkLaunch.Enabled = variables.DashlaunchE;
                if (sender.Equals(rbtnJtag)) Console.WriteLine("JTAG selected");

            }
            else if (rbtnGlitch.Checked || rbtnGlitch2.Checked || rbtnGlitch2m.Checked)
            {
                checkDLPatches.Enabled = true;
                checkDLPatches.Checked = chkLaunch.Enabled = variables.DashlaunchE;
                if (rbtnGlitch.Checked && sender.Equals(rbtnGlitch)) Console.WriteLine("Glitch selected");
                else if (rbtnGlitch2.Checked && sender.Equals(rbtnGlitch2)) Console.WriteLine("Glitch2 selected");
                else if (rbtnGlitch2m.Checked && sender.Equals(rbtnGlitch2m)) Console.WriteLine("Glitch2m selected");
            }
            else if (rbtnDevGL.Checked)
            {
                checkDLPatches.Enabled = true;
                checkDLPatches.Checked = chkLaunch.Enabled = variables.DashlaunchE;
                if (sender.Equals(rbtnDevGL)) Console.WriteLine("DEVGL selected");
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
            checkDashSpecificPatches();

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
                MainForm.mainForm.xPanel_HackChanged();
            }
            catch (Exception) { }

            updateCommand();
            setComboCB(rbtnRetail.Checked);
        }

        private void btnXeBuildOptions_Click(object sender, EventArgs e)
        {
            XBOptions xb = new XBOptions();
            xb.ShowDialog();
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            DashLaunch myNewForm3 = new DashLaunch();
            myNewForm3.ShowDialog();
        }

        private void txtMBname_Click(object sender, EventArgs e)
        {
            variables.ctype = MainForm.mainForm.callConsoleSelect(ConsoleSelect.Selected.All);
        }

        private void comboDash_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboDash.SelectedIndex > 0)
            {
                variables.preferredDash = comboDash.Text;
                variables.dashversion = Convert.ToInt32(comboDash.Text);
                lblDash.Text = comboDash.Text;
            }

            checkAvailableHackTypes();
            checkWBXdkBuild();
            checkDashSpecificPatches();
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

            if (variables.debugMode) Console.WriteLine(Path.Combine(variables.updatepath, comboDash.SelectedValue + "\\_file.ini"));
            if (!File.Exists(Path.Combine(variables.updatepath, comboDash.SelectedValue + "\\_glitch2.ini")))
            {
                rbtnGlitch2.Enabled = rbtnGlitch2.Checked = false;
            }
            else
            {
                checkGlitch2(variables.boardtype);
            }

            if (!File.Exists(Path.Combine(variables.updatepath, comboDash.SelectedValue + "\\_glitch2m.ini")))
            {
                rbtnGlitch2m.Enabled = rbtnGlitch2m.Checked = false;
            }
            else
            {
                checkGlitch2m(variables.boardtype);
            }

            if (!File.Exists(Path.Combine(variables.updatepath, comboDash.SelectedValue + "\\_glitch.ini")))
            {
                rbtnGlitch.Enabled = rbtnGlitch.Checked = false;
            }
            else
            {
                checkGlitch(variables.boardtype);
            }

            if (!File.Exists(Path.Combine(variables.updatepath, comboDash.SelectedValue + "\\_jtag.ini")))
            {
                rbtnJtag.Enabled = rbtnJtag.Checked = false;
            }
            else
            {
                checkJtag(variables.boardtype);
            }

            if (!File.Exists(Path.Combine(variables.updatepath, comboDash.SelectedValue + "\\_retail.ini")))
            {
                rbtnRetail.Checked = rbtnRetail.Enabled = false;
            }
            else
            {
                rbtnRetail.Enabled = true;
            }
            if (!File.Exists(Path.Combine(variables.updatepath, comboDash.SelectedValue + "\\_devgl.ini")))
            {
                rbtnDevGL.Enabled = rbtnDevGL.Checked = false;
            }
            else
            {
                checkDevGL();
            }
        }

        private void checkJtag(string board)
        {
            if (board == null) board = "None";
            if (board.Contains("Winchester") || board.Contains("Corona") || board.Contains("Trinity")) rbtnJtag.Enabled = rbtnJtag.Checked = false;
            else rbtnJtag.Enabled = true;
        }

        public void checkGlitch(string board)
        {
            if (board == null) board = "None";
            if (variables.rghable)
            {
                if (board.Contains("Winchester") || board.Contains("Corona") || board.Contains("Trinity") || board.Contains("Xenon")) rbtnGlitch.Enabled = rbtnGlitch.Checked = false;
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
            if (variables.dashversion == 17489 && File.Exists(variables.rootfolder + @"\xeBuild\17489\!XDKbuild Only!.txt"))
            {
                rbtnGlitch2m.Enabled = true;
            }
            else
            {
                if (board.Contains("Winchester") || board.Contains("Corona") || board.Contains("Trinity") || board.Contains("None")) rbtnGlitch2m.Enabled = true;
                else rbtnGlitch2m.Enabled = rbtnGlitch2m.Checked = false;
            }
        }

        private void checkDevGL()
        {
            if (canDevGL()) rbtnDevGL.Enabled = true;
            else rbtnDevGL.Enabled = rbtnDevGL.Checked = false;
        }

        public bool canDevGL()
        {
            if (File.Exists(Path.Combine(variables.rootfolder, @"xebuild\common\" + "sb_priv.bin"))) return true;
            else return false;
        }

        public void checkBigffs(string board)
        {
            if ((rbtnGlitch.Checked || rbtnGlitch2.Checked || rbtnGlitch2m.Checked || rbtnJtag.Checked || rbtnDevGL.Checked) && !chkXdkBuild.Checked)
            {
                if (board == null) board = "None";
                else if (board.Contains("Xenon") || board.Contains("Zephyr") || board.Contains("Falcon") || board.Contains("Jasper 16MB") || board.Contains("Jasper SB") ||
                    board.Contains("Trinity 16MB") || board.Contains("Corona 16MB") || board.Contains("Corona 4GB") || board.Contains("Winchester 16MB") ||
                    board.Contains("Winchester 4GB"))
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

        private void checkDashSpecificPatches()
        {
            if (File.Exists(Path.Combine(variables.updatepath, comboDash.SelectedValue + @"\bin\xl_usb.bin")))
            {
                if (rbtnRetail.Checked) chkXLUsb.Checked = chkXLUsb.Enabled = false;
                else chkXLUsb.Enabled = true;
            }
            else chkXLUsb.Checked = chkXLUsb.Enabled = false;

            if (File.Exists(Path.Combine(variables.updatepath, comboDash.SelectedValue + @"\bin\xl_hdd.bin")))
            {
                if (rbtnRetail.Checked) chkXLHdd.Checked = chkXLHdd.Enabled = false;
                else chkXLHdd.Enabled = true;
            }
            else chkXLHdd.Checked = chkXLHdd.Enabled = false;

            if (File.Exists(Path.Combine(variables.updatepath, comboDash.SelectedValue + @"\bin\xl_both.bin")))
            {
                if (rbtnRetail.Checked) chkXLBoth.Checked = chkXLBoth.Enabled = false;
                else chkXLBoth.Enabled = true;
            }
            else chkXLBoth.Checked = chkXLBoth.Enabled = false;

            if (File.Exists(Path.Combine(variables.updatepath, comboDash.SelectedValue + @"\bin\usbdsec.bin")))
            {
                if (rbtnRetail.Checked) chkUsbdSec.Checked = chkUsbdSec.Enabled = false;
                else chkUsbdSec.Enabled = true;
            }
            else chkUsbdSec.Checked = chkUsbdSec.Enabled = false;

            checkDashAndConsoleSpecificPatches(variables.boardtype);
        }

        private void checkDashAndConsoleSpecificPatches(string board)
        {
            if (board == null) board = "None";
            if (File.Exists(Path.Combine(variables.updatepath, comboDash.SelectedValue + @"\bin\corona_key_fix.bin")))
            {
                if (rbtnRetail.Checked) chkCoronaKeyFix.Checked = chkCoronaKeyFix.Enabled = false;
                else
                {
                    if (board.Contains("Corona") || board.Contains("Winchester") || board.Contains("None")) chkCoronaKeyFix.Enabled = true;
                    else chkCoronaKeyFix.Checked = chkCoronaKeyFix.Enabled = false;
                }
            }
            else chkCoronaKeyFix.Checked = chkCoronaKeyFix.Enabled = false;
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
            variables.DashlaunchE = checkDLPatches.Checked;
            if (!checkDLPatches.Checked || !checkDLPatches.Enabled) { chkLaunch.Enabled = false; chkLaunch.Checked = false; }
            else if (checkDLPatches.Checked && checkDLPatches.Enabled) chkLaunch.Enabled = true;
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
            if (!checkDLPatches.Enabled) chkLaunch.Enabled = false;
            else if (checkDLPatches.Checked) chkLaunch.Enabled = true;
        }

        private void chkListBoxPatches_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selected = chkListBoxPatches.SelectedIndex;
            if (selected >= 0 && selected <= 5)
            {
                if (chkListBoxPatches.GetItemChecked(selected))
                {
                    Console.WriteLine(chkListBoxPatches.Items[selected].ToString() + " selected");
                    if (selected == 0) patches[selected + 1] = "-a nofcrt";
                    else if (selected == 1) patches[selected + 1] = "-a noSShdd";
                    else if (selected == 2) patches[selected + 1] = "-a nointmu";
                    else if (selected == 3) patches[selected + 1] = "-a nohdd";
                    else if (selected == 4) patches[selected + 1] = "-a nohdmiwait";
                    else if (selected == 5) patches[selected + 1] = "-a nolan";
                }
                else
                {
                    Console.WriteLine(chkListBoxPatches.Items[selected].ToString() + " deselected");
                    patches[selected + 1] = "";
                }
            }

            updateCommand();
        }

        private void chkRJtag_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRJtag.Checked) Console.WriteLine("R-JTAG selected");
            else Console.WriteLine("R-JTAG deselected");
        }

        // Handling checkboxes allows us to only have one selected at time without extra stuff happening
        private void chkCleanSMC_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCleanSMC.Checked)
            {
                Console.WriteLine("Clean SMC selected");
                chkCR4.Checked = false;
                chkSMCP.Checked = false;
                chkRgh3.Checked = false;
            }
            else if (!chkCR4.Checked && !chkSMCP.Checked && !chkRgh3.Checked) // Don't uselessly spam the console
            {
                Console.WriteLine("Clean SMC deselected");
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
                Console.WriteLine("CR4 selected");
                chkCleanSMC.Checked = false;
                chkSMCP.Checked = false;
                chkRgh3.Checked = false;
            }
            else if (!chkCleanSMC.Checked && !chkSMCP.Checked && !chkRgh3.Checked) // Don't uselessly spam the console
            {
                Console.WriteLine("CR4 deselected");
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
                Console.WriteLine("SMC+ selected");
                chkCleanSMC.Checked = false;
                chkCR4.Checked = false;
                chkRgh3.Checked = false;
            }
            else if (!chkCleanSMC.Checked && !chkCR4.Checked && !chkRgh3.Checked) // Don't uselessly spam the console
            {
                Console.WriteLine("SMC+ deselected");
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
                Console.WriteLine("RGH3 selected");
                chkCleanSMC.Checked = false;
                chkCR4.Checked = false;
                chkSMCP.Checked = false;
            }
            else if (!chkCleanSMC.Checked && !chkCR4.Checked && !chkSMCP.Checked) // Don't uselessly spam the console
            {
                Console.WriteLine("RGH3 deselected");
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

            if (board.Contains("Xenon") || board.Contains("Zephyr") || board.Contains("Winchester"))
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
                Console.WriteLine("Winbond 2K selected");
                chkWB4G.Checked = false;
            }
            else if (!chkWB4G.Checked) // Don't uselessly spam the console
            {
                Console.WriteLine("Winbond 2K deselected");
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
                Console.WriteLine("Winbond 2K Buffer selected");
                chkWB.Checked = false;
            }
            else if (!chkWB.Checked) // Don't uselessly spam the console
            {
                Console.WriteLine("Winbond 2K Buffer deselected");
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
            if (chkBigffs.Checked) Console.WriteLine("bigffs selected");
            else Console.WriteLine("bigffs deselected");
        }

        private void chkXdkBuild_CheckedChanged(object sender, EventArgs e)
        {
            checkWB(variables.boardtype);
            checkBigffs(variables.boardtype);
            if (chkXdkBuild.Checked) Console.WriteLine("XDKbuild selected");
            else Console.WriteLine("XDKbuild deselected");
            checkRgh3(variables.boardtype);
        }

        private void chkAudClamp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAudClamp.Checked) Console.WriteLine("Aud_Clamp selected");
            else Console.WriteLine("Aud_Clamp deselected");
        }

        private void chk0Fuse_CheckedChanged(object sender, EventArgs e)
        {
            if (chk0Fuse.Checked) Console.WriteLine("0 Fuse selected");
            else Console.WriteLine("0 Fuse deselected");
        }

        private void chkXLUsb_CheckedChanged(object sender, EventArgs e)
        {
            if (chkXLUsb.Checked)
            {
                Console.WriteLine("XL USB selected");
                chkXLHdd.Checked = false;
                chkXLBoth.Checked = false;
            }
            else if (!chkXLHdd.Checked && !chkXLBoth.Checked) // Don't uselessly spam the console
            {
                Console.WriteLine("XL USB deselected");
            }
        }

        private void chkXLHdd_CheckedChanged(object sender, EventArgs e)
        {
            if (chkXLHdd.Checked)
            {
                Console.WriteLine("XL HDD selected");
                chkXLUsb.Checked = false;
                chkXLBoth.Checked = false;
            }
            else if (!chkXLUsb.Checked && !chkXLBoth.Checked) // Don't uselessly spam the console
            {
                Console.WriteLine("XL HDD deselected");
            }
        }

        private void chkXLBoth_CheckedChanged(object sender, EventArgs e)
        {
            if (chkXLBoth.Checked)
            {
                Console.WriteLine("Both XL selected");
                chkXLUsb.Checked = false;
                chkXLHdd.Checked = false;
            }
            else if (!chkXLUsb.Checked && !chkXLHdd.Checked) // Don't uselessly spam the console
            {
                Console.WriteLine("Both XL deselected");
            }
        }

        private void chkUsbdSec_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUsbdSec.Checked) Console.WriteLine("UsbdSec selected");
            else Console.WriteLine("UsbdSec deselected");
        }

        private void chkCoronaKeyFix_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCoronaKeyFix.Checked) Console.WriteLine("Corona Key Fix selected");
            else Console.WriteLine("Corona Key Fix deselected");
        }

        private void btnGetMB_Click(object sender, EventArgs e)
        {
            if ((ModifierKeys & Keys.Shift) == Keys.Shift && MainForm.mainForm.device == MainForm.DEVICE.XFLASHER_SPI) MainForm.mainForm.xflasher.getConsoleCb();
            else MainForm.mainForm.xPanel_getmb();
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
            if (!string.IsNullOrWhiteSpace(txtOffset.Text))
            {
                arguments = "-rb " + "\"" + Path.Combine(variables.outfolder, "consoleDump.bin") + "\"";
                arguments += " " + txtOffset.Text;
                if (!string.IsNullOrWhiteSpace(txtLength.Text))
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
            if (!string.IsNullOrWhiteSpace(txtOffset.Text))
            {
                arguments = "-wb " + "\"" + variables.filename1 + "\"";
                arguments += " " + txtOffset.Text;
                if (!string.IsNullOrWhiteSpace(txtLength.Text))
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
            if (!string.IsNullOrWhiteSpace(variables.filename1))
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
            if (variables.debugMode) Console.WriteLine(Path.Combine(variables.updatepath, comboDash.Text + @"\$systemupdate"));
            if (Directory.Exists(Path.Combine(variables.updatepath, comboDash.Text + @"\$systemupdate")))
            {
                Console.WriteLine("Starting, please wait!");
                string upPath = Path.Combine(variables.updatepath, comboDash.Text, @"\$systemupdate");
                //Path.Combine(upPath, @"\$systemupdate");
                // Console.WriteLine(Path.Combine(variables.update_path, comboDash.Text + @"\$systemupdate"));
                ThreadStart starter = delegate { xe_compatibilityAvatar(Path.Combine(variables.updatepath, (comboDash.Text)), "-e "); };
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
                Console.WriteLine("ForceIP selected");

            }
            else
            {
                txtIP.Enabled = txtIP2.Enabled = chkForceIP2.Checked = false;
                txtIP.Text = txtIP2.Text = "Autoscan LAN";
                Console.WriteLine("ForceIP deselected");
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
        }

        private void chkNoWrite_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNoWrite.Checked) Console.WriteLine("nowrite selected");
            else Console.WriteLine("nowrite deselected");
        }

        private void chkNoAva_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNoAva.Checked) Console.WriteLine("noava selected");
            else Console.WriteLine("noava deselected");
        }

        private void chkClean_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClean.Checked) Console.WriteLine("clean selected");
            else Console.WriteLine("clean deselected");
        }

        private void chkNoReeb_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNoReeb.Checked) Console.WriteLine("noreeb selected");
            else Console.WriteLine("noreeb deselected");
        }

        private void chkShutdown_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShutdown.Checked) Console.WriteLine("shutdown selected");
            else Console.WriteLine("shutdown deselected");
        }

        private void chkReboot_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReboot.Checked) Console.WriteLine("reboot selected");
            else Console.WriteLine("reboot deselected");
        }

        private void chkxesettings_CheckedChanged(object sender, EventArgs e)
        {
            if (chkXeSettings.Checked) Console.WriteLine("Use Edited Options selected");
            else Console.WriteLine("Use Edited Options deselected");
        }

        private void btnShowAdvanced_Click(object sender, EventArgs e)
        {
            if (btnShowAdvanced.Text == "Show Advanced Tabs")
            {
                MainTabs.TabPages.Add(tabClient);
                MainTabs.TabPages.Add(tabUpdate);
                btnShowAdvanced.Text = "Hide Advanced Tabs";
                variables.showAdvancedTabs = true;
            }
            else
            {
                MainTabs.TabPages.Remove(tabClient);
                MainTabs.TabPages.Remove(tabUpdate);
                btnShowAdvanced.Text = "Show Advanced Tabs";
                variables.showAdvancedTabs = false;
            }
        }

        #endregion

        #region code

        void xe_update()
        {
            Classes.xebuild xe = new Classes.xebuild();
            xe.Uloadvariables(variables.dashversion, (variables.hacktypes)variables.ttyp, patches, chkXeSettings.Checked, chkNoWrite.Checked, chkNoAva.Checked, chkClean.Checked,chkNoReeb.Checked, checkDLPatches.Checked,
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
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }

            Classes.xebuild.XebuildError er = xe.createxebuild();
            if (er == Classes.xebuild.XebuildError.none)
            {
                xe.xeExit += xe_xeUExit;
                xe.update();
            }
            else if (er == Classes.xebuild.XebuildError.nodash)
            {
                MessageBox.Show("No Kernel selected");
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

        public void createxebuild_v2(bool custom, Nand.PrivateN nand, bool fullDataClean)
        {
            Classes.xebuild xe = new Classes.xebuild();
            xe.loadvariables(nand._cpukey, (variables.hacktypes)variables.ttyp, variables.dashversion,
                             variables.ctype, patches, nand, chkXeSettings.Checked, checkDLPatches.Checked,
                             chkLaunch.Checked, chkAudClamp.Checked, chkRJtag.Checked, chkCleanSMC.Checked, chkCR4.Checked, chkSMCP.Checked, chkRgh3.Checked, chkBigffs.Checked,
                             chk0Fuse.Checked, chkXdkBuild.Checked, chkXLUsb.Checked, chkXLHdd.Checked, chkXLBoth.Checked, chkUsbdSec.Checked, chkCoronaKeyFix.Checked, fullDataClean);

            string ini = (variables.launchpath + @"\" + variables.dashversion + @"\_" + variables.ttyp + ".ini");

            if (variables.ctype.ID == 7 || variables.ctype.ID == 13 || variables.ctype.ID == 14)
            {
                if (MessageBox.Show("XeBuild does not support building 64MB images for Xenon, Zephyr, or Falcon\n\nContinuing will cause a 16MB image to be built\n\nDo you want to continue?", "Steep Hill Ahead", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
            }

            if (!custom)
            {
                if (string.IsNullOrWhiteSpace(variables.filename1))
                {
                    MainForm.mainForm.xPanel_loadFile(ref variables.filename1, true);
                    if (string.IsNullOrWhiteSpace(variables.filename1))
                    {
                        MessageBox.Show("No file was selected", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (!File.Exists(variables.filename1))
                {
                    MessageBox.Show("File is missing\n\nEnsure that it was not moved and the program can access it", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Path.GetExtension(variables.filename1) != ".bin")
                {
                    MessageBox.Show("You must select a .bin file", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
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
                catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
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
                bool sts = objAlphaPattern.IsMatch(variables.cpukey);
                if ((variables.cpukey.Length == 32 && sts))
                {
                    if (variables.debugMode) Console.WriteLine("Key verification");
                    long size = 0;
                    if (Nand.Nand.cpukeyverification(Oper.openfile(Path.Combine(variables.rootfolder, @"xebuild\data\kv.bin"), ref size, 0), variables.cpukey))
                    {
                        if (variables.debugMode) Console.WriteLine("CPU Key is Correct");
                        if (Nand.Nand.getfcrtflag(File.ReadAllBytes(Path.Combine(variables.rootfolder, @"xebuild\data\kv.bin")), variables.cpukey))
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
                string[] files = parse_ini.parselabel(ini, variables.ctype.Ini + "bl");
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
                    Console.WriteLine("The specified console bootloader list ({0}) is missing from the ini ({1})", variables.ctype.Ini + "bl", ini);
                    Console.WriteLine("You can either add it manually or ask for it get added if its possible");
                    return;
                case Classes.xebuild.XebuildError.wrongcpukey:
                    MessageBox.Show("Wrong CPU Key");
                    return;
                case Classes.xebuild.XebuildError.noconsole:
                    variables.ctype = MainForm.mainForm.callConsoleSelect(ConsoleSelect.Selected.All);
                    if (variables.ctype.ID == -1) return;
                    else
                    {
                        Console.WriteLine((variables.hacktypes)variables.ttyp);
                        xe.loadvariables(nand._cpukey, (variables.hacktypes)variables.ttyp, variables.dashversion,
                            variables.ctype, patches, nand, chkXeSettings.Checked, checkDLPatches.Checked,
                            chkLaunch.Checked, chkAudClamp.Checked, chkRJtag.Checked, chkCleanSMC.Checked,
                            chkCR4.Checked, chkSMCP.Checked, chkRgh3.Checked, chkBigffs.Checked, chk0Fuse.Checked,
                            chkXdkBuild.Checked, chkXLUsb.Checked, chkXLHdd.Checked, chkXLBoth.Checked, chkUsbdSec.Checked,
                            chkCoronaKeyFix.Checked, fullDataClean);
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
            MainForm.mainForm.updateProgress(100);

            try
            {
                File.Copy(Path.Combine(variables.rootfolder, @"xebuild\options.ini"), Path.Combine(variables.rootfolder, @"xebuild\data\options.ini"), true);
                chkXeSettings.Checked = false;
                File.Move(Path.Combine(variables.xefolder, variables.updflash + ".log"), Path.Combine(variables.xefolder, variables.updflash.Substring(0, variables.updflash.IndexOf(".")) + "(" + DateTime.Now.ToString("ddMMyyyyHHmm") + ").bin.log"));
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }

            try
            {
                if (File.Exists(Path.Combine(variables.rootfolder, @"build.log"))) File.Delete(Path.Combine(variables.rootfolder, @"build.log"));
            }
            catch { }

            if (variables.xefinished)
            {
                Console.WriteLine("Saved to {0}", variables.xefolder);
                Console.WriteLine("Image is Ready");
                variables.filename1 = Path.Combine(variables.xefolder, variables.updflash);
                MainForm.mainForm.xPanel_updateSource(variables.filename1);
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
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
            if (variables.debugMode) Console.WriteLine("Deleted Files Successfully");
            variables.xefinished = false;
        }

        public void copyfiles(string cpukey = "")
        {
            string targetkey = System.IO.Path.Combine(variables.xepath, variables.cpukeypath);
            string targetnand = System.IO.Path.Combine(variables.xepath, variables.nanddump);
            if (cpukey.Length > 0) File.WriteAllText(targetkey, cpukey);
            if (string.IsNullOrEmpty(variables.filename1)) return;
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
                    catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }

                }
            }
            /**/
            else /**/File.Copy(variables.filename1, targetnand, true);
        }
        private void delfiles()
        {
            if (File.Exists(variables.xepath + variables.nanddump))
            {
                try
                {
                    File.Delete(variables.xepath + variables.nanddump);
                    if (variables.debugMode) Console.WriteLine("Deleted {0}", variables.xepath + variables.nanddump);
                }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
            if (File.Exists(variables.xepath + variables.cpukeypath))
            {
                try
                { File.Delete(variables.xepath + variables.cpukeypath); if (variables.debugMode) Console.WriteLine("Deleted {0}", variables.xepath + variables.cpukeypath); }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
            if (File.Exists(variables.launchpath + @"\" + variables.dashversion + @"\launch.ini"))
            {
                try
                {
                    File.Delete(variables.launchpath + @"\" + variables.dashversion + @"\launch.ini");
                    if (variables.debugMode) Console.WriteLine("Deleted launch.ini");
                }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
            if (File.Exists(Path.Combine(variables.xepath, "SMC.bin")) && (variables.copiedSMC || variables.fullDataClean)) // Only Delete SMCs it puts there
            {
                try
                {
                    File.Delete(Path.Combine(variables.xepath, "SMC.bin"));
                    if (variables.debugMode) Console.WriteLine("Deleted SMC.bin");
                }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
            if (File.Exists(Path.Combine(variables.xepath, "KV.bin")) && variables.fullDataClean)
            {
                try
                {
                    File.Delete(Path.Combine(variables.xepath, "KV.bin"));
                    if (variables.debugMode) Console.WriteLine("Deleted KV.bin");
                }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
            if (File.Exists(Path.Combine(variables.xepath, "fcrt.bin")) && variables.fullDataClean)
            {
                try
                {
                    File.Delete(Path.Combine(variables.xepath, "fcrt.bin"));
                    if (variables.debugMode) Console.WriteLine("Deleted fcrt.bin");
                }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
            if (File.Exists(Path.Combine(variables.xepath, "smc_config.bin")) && variables.fullDataClean)
            {
                try
                {
                    File.Delete(Path.Combine(variables.xepath, "smc_config.bin"));
                    if (variables.debugMode) Console.WriteLine("Deleted KV.bin");
                }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
            if (File.Exists(Path.Combine(variables.updatepath, comboDash.SelectedValue + @"\xam.xex")) && variables.copiedXLDrive)
            {
                try
                {
                    File.Delete(Path.Combine(variables.updatepath, comboDash.SelectedValue + @"\xam.xex"));
                    if (variables.debugMode) Console.WriteLine("Deleted XL Drive xam.xex");
                    if (File.Exists(Path.Combine(variables.updatepath, comboDash.SelectedValue + @"\xam.xex.tmp")))
                    {
                        File.Move(Path.Combine(variables.updatepath, comboDash.SelectedValue + @"\xam.xex.tmp"), Path.Combine(variables.updatepath, comboDash.SelectedValue + @"\xam.xex"));
                    }
                    if (variables.debugMode) Console.WriteLine("Restored non XL Drive xam.xex");

                    string buildIni = Path.Combine(variables.updatepath, comboDash.SelectedValue + @"\_" + variables.ttyp.ToString() + ".ini");
                    if (File.Exists(buildIni + ".tmp"))
                    {
                        File.Delete(buildIni);
                        File.Move(buildIni + ".tmp", buildIni);
                    }
                    if (variables.debugMode) Console.WriteLine("Restored non XL Drive ini");
                }
                catch (System.IO.IOException e)
                { MessageBox.Show(e.Message); return; }
            }
        }

        public void xe_xeUExit(object sender, EventArgs e)
        {
            variables.changeldv = 0;
            MainForm.mainForm.updateProgress(100);

            if (variables.xefinished)
            {
                Console.WriteLine("Saved to {0}", variables.xefolder);
                Console.WriteLine("Image is Ready");
                variables.filename1 = Path.Combine(variables.xefolder, variables.updflash);
                MainForm.mainForm.xPanel_updateSource(variables.filename1);
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
            c += " -c " + variables.ctype.XeBuild;
            foreach (String patch in patches)
            {
                c += " " + patch;
            }
            c += " -f " + variables.dashversion;
            c += " -d data";
            c += " \"" + variables.xefolder + "\\" + variables.updflash + "\" ";

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            c = regex.Replace(c, @" ");
        }

        private void txtMBname_TextChanged(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(delegate { updateCommand(true); })).Start();
            new Thread(new ThreadStart(delegate { setComboCB(false, true); })).Start();
        }

        public void setComboCB(bool erase = false, bool wait = false)
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
                cbList = new List<CB>();
                if (variables.dashversion != 0)
                {
                    string ini = (variables.launchpath + @"\" + variables.dashversion + @"\_retail.ini");
                    List<string> labels = new List<string>();
                    List<string> cbs = new List<string>();
                    parse_ini.getLabelsandCBs(ini, ref labels, ref cbs);
                    string defaultCB = null;

                    foreach (string s in labels)
                    {
                        if (!s.Contains("bl")) continue;
                        if (variables.ctype.ID == -1)
                        {
                            if (s.Contains("_"))
                            {
                                cbList.Add(new CB(s.Substring(s.IndexOf("_") + 1), true));
                            }
                            else
                            {
                                string cb = cbs[labels.IndexOf(s)];
                                cbList.Add(new CB(cb, false));
                                if (defaultCB == null) defaultCB = cb; // Only once
                            }
                        }
                        else
                        {
                            if (s.Contains(variables.ctype.Ini))
                            {
                                if (s.Contains("_"))
                                {
                                    cbList.Add(new CB(s.Substring(s.IndexOf("_") + 1), true));
                                }
                                else
                                {
                                    string cb = cbs[labels.IndexOf(s)];
                                    cbList.Add(new CB(cb, false));
                                    if (defaultCB == null) defaultCB = cb; // Only once
                                }
                            }
                        }
                    }

                    cbList.Sort((a, b) => Convert.ToInt32(a.Version) - Convert.ToInt32(b.Version));

                    int defaultIndex = 0; // Fallback
                    foreach (CB cb in cbList)
                    {
                        if (cb.Version == defaultCB) defaultIndex = comboCB.Items.Count; // Before adding!
                        comboCB.Items.Add(cb);
                    }
                
                    if (comboCB.Items.Count > 0)
                    {
                        comboCB.SelectedIndex = defaultIndex; // Important, the combo becomes buggy if the default selection is not the basic (ex: xenonbl, not xenonbl_1928)
                    }
                }
            }
            catch { }
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
