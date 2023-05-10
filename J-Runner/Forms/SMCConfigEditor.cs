using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class SMCConfigEditor : Form
    {
        Nand.SMCConfig smc_config;
        int block_offset;
        Encoding ascii = Encoding.ASCII;
        bool save = false;
        bool loaded = false;

        enum VideoRegion
        {
            PAL,
            NTSC
        }
        enum GameRegion
        {
            USA,
            JAP,
            KOR,
            HK,
            EU,
            AUS
        }

        public SMCConfigEditor()
        {
            InitializeComponent();
            try
            {
                smc_config = Nand.Nand.getConfigValues(Nand.Nand.getsmcconfig(variables.filename1, out block_offset), block_offset);
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
        }

        private void SMCConfig_Load(object sender, EventArgs e)
        {
            loaded = false;
            try
            {
                if (smc_config.ok)
                {
                    btnEdit.Enabled = true;

                    txtchecksum.Text = Oper.ByteArrayToString(smc_config.checksum);
                    txtstructure.Text = Oper.ByteArrayToString(smc_config.structure);
                    txtconfig.Text = Oper.ByteArrayToString(smc_config.config);
                    txtbit.Text = Oper.ByteArrayToString(smc_config.bit);
                    txtmac.Text = Regex.Replace(Oper.ByteArrayToString(smc_config.mac), @"(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})", @"$1 - $2 - $3 - $4 - $5 - $6");
                    txtcpugain.Text = Oper.ByteArrayToString(smc_config.cpugain);
                    txtcpuoff.Text = Oper.ByteArrayToString(smc_config.cpuoff);
                    txtgpugain.Text = Oper.ByteArrayToString(smc_config.gpugain);
                    txtgpuoff.Text = Oper.ByteArrayToString(smc_config.gpuoff);
                    txtedramgain.Text = Oper.ByteArrayToString(smc_config.dramgain);
                    txtedramoff.Text = Oper.ByteArrayToString(smc_config.dramoff);
                    txtboardgain.Text = Oper.ByteArrayToString(smc_config.boardgain);
                    txtboardoff.Text = Oper.ByteArrayToString(smc_config.boardoff);
                    txtana.Text = Oper.ByteArrayToString(smc_config.ana);
                    txtanabackup.Text = Oper.ByteArrayToString(smc_config.anabackup);
                    txtclock.Text = Oper.ByteArrayToString(smc_config.clock);
                    txtflags.Text = Oper.ByteArrayToString(smc_config.flags);
                    txtversion.Text = Oper.ByteArrayToString(smc_config.version);
                    txtnet.Text = Oper.ByteArrayToString(smc_config.net);

                    txtreset.Text = ascii.GetString(smc_config.reset);
                    updateResetCode(txtreset.Text);

                    populateTemps();

                    txtgainoff.Text = Regex.Replace(Oper.ByteArrayToString(smc_config.gainoff), @"(.{4})(.{4})(.{4})(.{4})(.{4})(.{4})(.{4})(.{4})", @"$1;$2;$3;$4;$5;$6;$7;$8");

                    //txtdvdregion.Text = Oper.ByteArrayToString(smc_config.dvdregion);
                    //txtgameregion.Text = Oper.ByteArrayToString(smc_config.gameregion);
                    //txtvideoregion.Text = Oper.ByteArrayToString(smc_config.videoregion);
                    switch (smc_config.dvdregion[0])
                    {
                        case 0x01:
                            comboDVD.SelectedIndex = 0;
                            break;
                        case 0x02:
                            comboDVD.SelectedIndex = 1;
                            break;
                        case 0x03:
                            comboDVD.SelectedIndex = 2;
                            break;
                        case 0x04:
                            comboDVD.SelectedIndex = 3;
                            break;
                        case 0x05:
                            comboDVD.SelectedIndex = 4;
                            break;
                        case 0x06:
                            comboDVD.SelectedIndex = 5;
                            break;
                        case 0x07:
                            comboDVD.SelectedIndex = 6;
                            break;
                        case 0x08:
                            comboDVD.SelectedIndex = 7;
                            break;
                        case 0xFF:
                            comboDVD.SelectedIndex = 8;
                            break;
                    }

                    switch (smc_config.gameregion[0])
                    {
                        case 0x00:
                            comboVideo.SelectedIndex = (int)VideoRegion.NTSC;
                            switch (smc_config.videoregion[0])
                            {
                                case 0xFF:
                                    comboGame.SelectedIndex = (int)GameRegion.USA;
                                    break;
                            }
                            break;
                        case 0x01:
                            comboVideo.SelectedIndex = (int)VideoRegion.NTSC;
                            switch (smc_config.videoregion[0])
                            {
                                case 0x01:
                                    comboGame.SelectedIndex = (int)GameRegion.HK;
                                    break;
                                case 0xFC:
                                    comboGame.SelectedIndex = (int)GameRegion.KOR;
                                    break;
                                case 0xFE:
                                case 0xFF:
                                    comboGame.SelectedIndex = (int)GameRegion.JAP;
                                    break;
                            }
                            break;
                        case 0x02:
                            comboVideo.SelectedIndex = (int)VideoRegion.PAL;
                            switch (smc_config.videoregion[0])
                            {
                                case 0x01:
                                    comboGame.SelectedIndex = (int)GameRegion.AUS;
                                    break;
                                case 0xFE:
                                    comboGame.SelectedIndex = (int)GameRegion.EU;
                                    break;
                            }
                            break;
                    }

                    txtpwrmode.Text = Oper.ByteArrayToString(smc_config.pwrmode);
                    txtpowervcs.Text = Oper.ByteArrayToString(smc_config.powervcs);

                    txtreserve0.Text = Oper.ByteArrayToString(smc_config.reserve0);
                    txtreserve1.Text = Oper.ByteArrayToString(smc_config.reserve1);
                    txtreserve2.Text = Oper.ByteArrayToString(smc_config.reserve2);
                    txtreserve3.Text = Oper.ByteArrayToString(smc_config.reserve3);
                    txtreserve4.Text = Oper.ByteArrayToString(smc_config.reserve4);
                    txtreserve5.Text = Oper.ByteArrayToString(smc_config.reserve5);

                    if (Oper.ByteArrayToString(smc_config.cpufanspeed) == "7F") chkcpufanspeed.Checked = true;
                    else if ((smc_config.cpufanspeed[0]) >= 0x80)
                    {
                        chkcpufanspeed.Checked = false;
                        chkcpufanspeed.ForeColor = Color.Red;
                        chkcpufanspeed.Text = "CPU Fan Speed        [Manual Mode]";
                        trackCPU.Visible = true;
                        trackCPU.Value = smc_config.cpufanspeed[0] - 0x80;
                    }
                    if (Oper.ByteArrayToString(smc_config.gpufanspeed) == "7F") chkgpufanspeed.Checked = true;
                    else if ((smc_config.gpufanspeed[0]) >= 0x80)
                    {
                        chkgpufanspeed.Checked = false;
                        chkcpufanspeed.ForeColor = Color.Red;
                        chkgpufanspeed.Text = "GPU Fan Speed        [Manual Mode]";
                        trackGPU.Enabled = true;
                        trackGPU.Value = smc_config.gpufanspeed[0] - 0x80;
                    }

                    loaded = true;
                }
                else
                {
                    resetLt.Visible = false;
                    resetRt.Visible = false;
                    resetX.Visible = false;
                    resetY.Visible = false;
                    resetLb.Visible = false;
                    resetRb.Visible = false;
                    reset1.Visible = false;
                    reset2.Visible = false;
                    reset3.Visible = false;
                    reset4.Visible = false;
                }
            }
            catch (Exception ex)
            {
                if (variables.debugMode) Console.WriteLine(ex.ToString());
                MessageBox.Show("SMC Config could not be loaded\n\nIt might be corrupt\n\n" + ex.GetType(), "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void populateTemps(bool overwrite = true)
        {
            string thr = Oper.ByteArrayToString(smc_config.thermal);
            if (txtTempTargetCPU.Text.Length == 0 || overwrite) txtTempTargetCPU.Text = Convert.ToInt32(thr.Substring(0, 2), 16).ToString();
            if (txtTempTargetGPU.Text.Length == 0 || overwrite) txtTempTargetGPU.Text = Convert.ToInt32(thr.Substring(2, 2), 16).ToString();
            if (txtTempTargetEDRAM.Text.Length == 0 || overwrite) txtTempTargetEDRAM.Text = Convert.ToInt32(thr.Substring(4, 2), 16).ToString();
            if (txtTempCriticalCPU.Text.Length == 0 || overwrite) txtTempCriticalCPU.Text = Convert.ToInt32(thr.Substring(6, 2), 16).ToString();
            if (txtTempCriticalGPU.Text.Length == 0 || overwrite) txtTempCriticalGPU.Text = Convert.ToInt32(thr.Substring(8, 2), 16).ToString();
            if (txtTempCriticalEDRAM.Text.Length == 0 || overwrite) txtTempCriticalEDRAM.Text = Convert.ToInt32(thr.Substring(10, 2), 16).ToString();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            save = !save;
            if (save) btnEdit.Text = "Save Config";
            else
            {
                btnEdit.Text = "Edit Config";
                config_edit();
            }
            enable(save);
        }

        private void config_edit()
        {
            try
            {
                if (File.Exists(variables.filename1) && chkSaveBackup.Checked)
                {
                    string outpath = Path.Combine(Path.GetDirectoryName(variables.filename1), Path.GetFileNameWithoutExtension(variables.filename1).Replace("_old", "") + "_old" + Path.GetExtension(variables.filename1));
                    if (File.Exists(outpath)) File.Delete(outpath);
                    File.Copy(variables.filename1, outpath);
                }

                string newfile = variables.filename1;
                Nand.SMCConfig val = new Nand.SMCConfig(false);

                val.structure = Oper.StringToByteArray_v2(txtstructure.Text);
                val.config = Oper.StringToByteArray_v2(txtconfig.Text);
                val.bit = Oper.StringToByteArray_v2(txtbit.Text);
                val.mac = Oper.StringToByteArray_v2(txtmac.Text.Replace("-", "").Replace(" ", ""));
                val.cpugain = Oper.StringToByteArray_v2(txtcpugain.Text);
                val.cpuoff = Oper.StringToByteArray_v2(txtcpuoff.Text);
                val.gpugain = Oper.StringToByteArray_v2(txtgpugain.Text);
                val.gpuoff = Oper.StringToByteArray_v2(txtgpuoff.Text);
                val.dramgain = Oper.StringToByteArray_v2(txtedramgain.Text);
                val.dramoff = Oper.StringToByteArray_v2(txtedramoff.Text);
                val.boardgain = Oper.StringToByteArray_v2(txtboardgain.Text);
                val.boardoff = Oper.StringToByteArray_v2(txtboardoff.Text);
                val.ana = Oper.StringToByteArray_v2(txtana.Text);
                val.anabackup = Oper.StringToByteArray_v2(txtanabackup.Text);
                val.clock = Oper.StringToByteArray_v2(txtclock.Text);
                val.flags = Oper.StringToByteArray_v2(txtflags.Text);
                val.version = Oper.StringToByteArray_v2(txtversion.Text);
                val.net = Oper.StringToByteArray_v2(txtnet.Text);

                val.reset = ascii.GetBytes(txtreset.Text);
                updateResetCode(txtreset.Text);
                string thr = Oper.ByteArrayToString(smc_config.thermal);

                populateTemps(false); // That way if any are blank, we set the value from before
                val.thermal = new byte[6];
                try
                {
                    byte.TryParse(txtTempTargetCPU.Text, out val.thermal[0]);
                    byte.TryParse(txtTempTargetGPU.Text, out val.thermal[1]);
                    byte.TryParse(txtTempTargetEDRAM.Text, out val.thermal[2]);
                    byte.TryParse(txtTempCriticalCPU.Text, out val.thermal[3]);
                    byte.TryParse(txtTempCriticalGPU.Text, out val.thermal[4]);
                    byte.TryParse(txtTempCriticalEDRAM.Text, out val.thermal[5]);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                val.gainoff = Oper.StringToByteArray_v2(txtgainoff.Text.Replace(";", ""));
                val.dvdregion = new byte[1];
                val.dvdregion[0] = comboDVD.SelectedIndex != 8 ? (byte)(comboDVD.SelectedIndex + 1) : (byte)0xFF;
                val.gameregion = new byte[1];
                val.videoregion = new byte[1];

                if (comboVideo.SelectedIndex == (int)VideoRegion.PAL)
                    val.gameregion[0] = 0x02;

                switch (comboGame.SelectedIndex)
                {
                    case (int)GameRegion.USA:
                        val.videoregion[0] = 0xFF;
                        break;
                    case (int)GameRegion.HK:
                        val.videoregion[0] = 0x01;
                        val.gameregion[0] = 0x01;
                        break;
                    case (int)GameRegion.KOR:
                        val.videoregion[0] = 0xFC;
                        val.gameregion[0] = 0x01;
                        break;
                    case (int)GameRegion.JAP:
                        val.videoregion[0] = 0xFF;
                        val.gameregion[0] = 0x01;
                        break;
                    case (int)GameRegion.AUS:
                        val.videoregion[0] = 0x01;
                        break;
                    case (int)GameRegion.EU:
                        val.videoregion[0] = 0xFE;
                        break;
                }

                val.pwrmode = Oper.StringToByteArray_v2(txtpwrmode.Text);
                val.powervcs = Oper.StringToByteArray_v2(txtpowervcs.Text);

                val.cpufanspeed = new byte[1];
                if (chkcpufanspeed.Checked) val.cpufanspeed[0] = 0x7F;
                else
                {
                    val.cpufanspeed[0] = (byte)(trackCPU.Value + 0x80);
                }
                val.gpufanspeed = new byte[1];
                if (chkgpufanspeed.Checked) val.gpufanspeed[0] = 0x7F;
                else
                {
                    val.gpufanspeed[0] = (byte)(trackGPU.Value + 0x80);
                }

                val.reserve0 = Oper.StringToByteArray_v2(txtreserve0.Text);
                val.reserve1 = Oper.StringToByteArray_v2(txtreserve1.Text);
                val.reserve2 = Oper.StringToByteArray_v2(txtreserve2.Text);
                val.reserve3 = Oper.StringToByteArray_v2(txtreserve3.Text);
                val.reserve4 = Oper.StringToByteArray_v2(txtreserve4.Text);
                val.reserve5 = Oper.StringToByteArray_v2(txtreserve5.Text);

                if (variables.debugMode) Console.WriteLine("editing");
                byte[] smc_conf = Nand.Nand.editConfigValues(newfile, val);
                if (variables.debugMode) Console.WriteLine("injecting");
                Nand.Nand.injectSMCConf(newfile, smc_conf);

                Console.WriteLine("SMC Config Patch Successful");
                MainForm.mainForm.nand_init();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (variables.debugMode) Console.WriteLine(ex.ToString());
                MessageBox.Show("SMC Config could not be saved\n\nSome values might be set incorrectly", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void enable(bool en)
        {
            if (smc_config.ok)
            {
                var d = GetAll(this, typeof(TextBox));
                foreach (TextBox txt in d)
                {
                    if (txt != txtchecksum) txt.Enabled = en;
                }
                d = GetAll(this, typeof(ComboBox));
                foreach (ComboBox cm in d)
                {
                    cm.Enabled = en;
                }
                btnDefaultTemp.Enabled = en;
                trackCPU.Enabled = en;
                trackGPU.Enabled = en;
                chkcpufanspeed.Enabled = en;
                chkgpufanspeed.Enabled = en;
            }
        }

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
        }

        private void chkcpufanspeed_CheckedChanged(object sender, EventArgs e)
        {
            if (loaded && !chkcpufanspeed.Checked)
            {
                if (DialogResult.No == MessageBox.Show("Setting a fixed fan speed will prevent the SMC from being able to adjust the fan speed\n\nThis may cause SERIOUS DAMAGE to the Xbox\n\nIf you absolutely must, it is STRONGLY suggested to simply lower the fan targets (below) instead\n\nAre you sure you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    chkcpufanspeed.Checked = true;
                    return;
                }
            }

            trackCPU.Visible = !chkcpufanspeed.Checked;

            if (!chkcpufanspeed.Checked)
            {
                chkcpufanspeed.ForeColor = Color.Red;
                chkcpufanspeed.Text = "CPU Fan Speed        [Manual Mode]";
            }
            else
            {
                chkcpufanspeed.ForeColor = Color.Black;
                chkcpufanspeed.Text = "CPU Fan Speed        [Auto Mode]";
            }
        }
        private void chkgpufanspeed_CheckedChanged(object sender, EventArgs e)
        {
            if (loaded && !chkgpufanspeed.Checked)
            {
                if (DialogResult.No == MessageBox.Show("Setting a fixed fan speed will prevent the SMC from being able to adjust the fan speed\n\nThis may cause SERIOUS DAMAGE to the Xbox\n\nIf you absolutely must, it is STRONGLY suggested to simply lower the fan targets (below) instead\n\nAre you sure you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    chkgpufanspeed.Checked = true;
                    return;
                }
            }

            trackGPU.Visible = !chkgpufanspeed.Checked;

            if (!chkgpufanspeed.Checked)
            {
                chkgpufanspeed.ForeColor = Color.Red;
                chkgpufanspeed.Text = "GPU Fan Speed        [Manual Mode]";
            }
            else
            {
                chkgpufanspeed.ForeColor = Color.Black;
                chkgpufanspeed.Text = "GPU Fan Speed        [Auto Mode]";
            }
        }

        private void trackCPU_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackCPU, trackCPU.Value.ToString());
        }
        private void trackGPU_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackGPU, trackGPU.Value.ToString());
        }

        private void updateResetCode(string code)
        {
            if (code.Length > 0 && code.Length <= 4)
            {
                char[] chars = code.ToCharArray();

                string box1 = "key_" + chars[0];
                string box2 = "key_" + chars[1];
                string box3 = "key_" + chars[2];
                string box4 = "key_" + chars[3];

                reset1.Image = (Image)Properties.Resources.ResourceManager.GetObject(box1);
                reset2.Image = (Image)Properties.Resources.ResourceManager.GetObject(box2);
                reset3.Image = (Image)Properties.Resources.ResourceManager.GetObject(box3);
                reset4.Image = (Image)Properties.Resources.ResourceManager.GetObject(box4);

                resetLt.Visible = true;
                resetRt.Visible = true;
                resetX.Visible = true;
                resetY.Visible = true;
                resetLb.Visible = true;
                resetRb.Visible = true;
                reset1.Visible = true;
                reset2.Visible = true;
                reset3.Visible = true;
                reset4.Visible = true;
                resetNo.Visible = false;
            }
            else
            {
                resetLt.Visible = false;
                resetRt.Visible = false;
                resetX.Visible = false;
                resetY.Visible = false;
                resetLb.Visible = false;
                resetRb.Visible = false;
                reset1.Visible = false;
                reset2.Visible = false;
                reset3.Visible = false;
                reset4.Visible = false;
                resetNo.Visible = true;
            }
        }

        private void txtreset_Leave(object sender, EventArgs e)
        {
            txtreset.Text = txtreset.Text.ToUpper();
        }

        private bool checkTempValid(string s)
        {
            int.TryParse(s, out int val);

            if (val > 127)
            {
                MessageBox.Show("Temp value must be less than 128", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else return true;
        }

        private void txtTempTargetCPU_TextChanged(object sender, EventArgs e)
        {
            if (!save) return;

            if (!checkTempValid(txtTempTargetCPU.Text))
            {
                txtTempTargetCPU.Text = "";
                populateTemps(false);
            }
        }

        private void txtTempTargetGPU_TextChanged(object sender, EventArgs e)
        {
            if (!save) return;

            if (!checkTempValid(txtTempTargetGPU.Text))
            {
                txtTempTargetGPU.Text = "";
                populateTemps(false);
            }
        }

        private void txtTempTargetEDRAM_TextChanged(object sender, EventArgs e)
        {
            if (!save) return;

            if (!checkTempValid(txtTempTargetEDRAM.Text))
            {
                txtTempTargetEDRAM.Text = "";
                populateTemps(false);
            }
        }

        private void txtTempCriticalCPU_TextChanged(object sender, EventArgs e)
        {
            if (!save) return;

            if (!checkTempValid(txtTempCriticalCPU.Text))
            {
                txtTempCriticalCPU.Text = "";
                populateTemps(false);
            }
        }

        private void txtTempCriticalGPU_TextChanged(object sender, EventArgs e)
        {
            if (!save) return;

            if (!checkTempValid(txtTempCriticalGPU.Text))
            {
                txtTempCriticalGPU.Text = "";
                populateTemps(false);
            }
        }

        private void txtTempCriticalEDRAM_TextChanged(object sender, EventArgs e)
        {
            if (!save) return;

            if (!checkTempValid(txtTempCriticalEDRAM.Text))
            {
                txtTempCriticalEDRAM.Text = "";
                populateTemps(false);
            }
        }

        #region Default Temps Menu

        private void setDefaultTempTargets(Nand.ntable._temptable entry)
        {
            txtTempTargetCPU.Text = entry.targetCPU.ToString();
            txtTempTargetGPU.Text = entry.targetGPU.ToString();
            txtTempTargetEDRAM.Text = entry.targetEDRAM.ToString();
            txtTempCriticalCPU.Text = entry.criticalCPU.ToString();
            txtTempCriticalGPU.Text = entry.criticalGPU.ToString();
            txtTempCriticalEDRAM.Text = entry.criticalEDRAM.ToString();
        }

        private void x1DefaultTemp_Click(object sender, EventArgs e)
        {
            setDefaultTempTargets(Nand.ntable.defaultTempTable[0]);
        }
        private void x2DefaultTemp_Click(object sender, EventArgs e)
        {
            setDefaultTempTargets(Nand.ntable.defaultTempTable[1]);
        }
        private void zDefaultTemp_Click(object sender, EventArgs e)
        {
            setDefaultTempTargets(Nand.ntable.defaultTempTable[2]);
        }
        private void fDefaultTemp_Click(object sender, EventArgs e)
        {
            setDefaultTempTargets(Nand.ntable.defaultTempTable[3]);
        }
        private void jDefaultTemp_Click(object sender, EventArgs e)
        {
            setDefaultTempTargets(Nand.ntable.defaultTempTable[4]);
        }
        private void jtDefaultTemp_Click(object sender, EventArgs e)
        {
            setDefaultTempTargets(Nand.ntable.defaultTempTable[5]);
        }
        private void tDefaultTemp_Click(object sender, EventArgs e)
        {
            setDefaultTempTargets(Nand.ntable.defaultTempTable[6]);
        }
        private void cDefaultTemp_Click(object sender, EventArgs e)
        {
            setDefaultTempTargets(Nand.ntable.defaultTempTable[7]);
        }
        private void wDefaultTemp_Click(object sender, EventArgs e)
        {
            setDefaultTempTargets(Nand.ntable.defaultTempTable[8]);
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (save)
            {
                DialogResult mbr = MessageBox.Show("Do you want to save changes to the SMC Config?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (mbr == DialogResult.Yes)
                {
                    save = false;
                    config_edit();
                    enable(false);
                    this.Close();
                }
                else
                {
                    this.Close();
                }
            }
            else this.Close();
        }
    }
}

/*
 *  * SMC CONFIG
 * 
 * 0-400
+C000 small block
 * 
+60000 big block
 * 
 * 
 * 
0x00-0x01 checksum
0x0E structure version
0x0F config source
0x14 bit field

0xF2-0xF3 cpu gain 0x18-0x19
0xF4-0xF5 cpu offset 0x1A-0x1B
0xF6-0xF7 gpu gain 0x1C-0x1D
0xF8-0xF9 gpu offset 0x1E-0x1F
0xFA-0xFB edram gain 0x20-0x21
0xFC-0xFD edram offset 0x22-0x23
0xFE-0xFF Board gain 0x24-0x25
0x100-0x101 board offset 0x26-0x27
ana fuse value 0x102


dvd region 0x207? (0x237?) 0x22A?
game region 0x22C
video region 0x22D

0x220-0x225 mac address
0x238-0x23B Reset code

0x240-0x241 POWER MODE
*/
