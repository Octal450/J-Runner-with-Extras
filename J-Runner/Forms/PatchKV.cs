using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JRunner
{
    public partial class PatchKV : Form
    {
        static bool changed = false;

        public PatchKV()
        {
            InitializeComponent();
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
            btnOK.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
        }

        private void patch_Load(object sender, EventArgs e)
        {
            try
            {
                if (MainForm.nand.ki.region != "" || MainForm.nand.ki.osig != "")
                {
                    string osigInput = MainForm.nand.ki.osig.Substring(MainForm.nand.ki.osig.Length - 4);
                    if (osigInput == "0442") osigInput = "421C"; // Fix BenQ 0442/421C

                    foreach (string region in comboRegion.Items)
                    {
                        if (region.Contains(MainForm.nand.ki.region)) comboRegion.Text = region;
                    }

                    foreach (string osig in comboOsig.Items)
                    {
                        if (osig.EndsWith(osigInput)) comboOsig.Text = osig;
                    }

                    if (comboOsig.Text.Length == 0) comboOsig.Text = "No Drive Info/Unspoofed";
                }
                txtDVDkey.Text = MainForm.nand.ki.dvdkey;
                txtSerial.Text = MainForm.nand.ki.serial;
                txtConsoleID.Text = MainForm.nand.ki.consoleid;
                txtMfrDate.Text = MainForm.nand.ki.mfdate;
            }
            catch (ArgumentNullException) { return; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!changed) this.Close();
            else
            {
                if (variables.filename1 == null || variables.cpukey == "") this.Close();
                byte[] data = Nand.BadBlock.find_bad_blocks_b(variables.filename1, true);

                if (data.Length > 0x4500)
                {
                    int layout;
                    byte[] sparedata = new byte[0x10];

                    Buffer.BlockCopy(data, 0x4400, sparedata, 0, 0x10);

                    layout = Nand.Nand.identifylayout(sparedata);

                    data = patchKV(ref data, layout);

                    if (File.Exists(variables.filename1) && chkSaveBackup.Checked)
                    {
                        string outpath = Path.Combine(Path.GetDirectoryName(variables.filename1), Path.GetFileNameWithoutExtension(variables.filename1).Replace("_old", "") + "_old" + Path.GetExtension(variables.filename1));
                        if (File.Exists(outpath)) File.Delete(outpath);
                        File.Move(variables.filename1, outpath);
                    }

                    Oper.savefile(data, variables.filename1);
                    Console.WriteLine("Patch Successful");
                    MainForm.mainForm.nand_init();
                }
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private byte[] patchKV(ref byte[] image, int layout)
        {
            Console.WriteLine("Processing KV...");
            bool ecc;
            if (image[0x205] == 0xFF || image[0x415] == 0xFF || image[0x200] == 0xFF) ecc = true;
            else ecc = false;

            byte[] blockarray = new byte[4];
            Buffer.BlockCopy(image, 0x4400, blockarray, 0, 4);

            byte[] keyvault = Nand.Nand.decryptkv(Nand.Nand.getkv(image), Oper.StringToByteArray(variables.cpukey));
            Nand.KVInfo kinfo = new Nand.KVInfo();
            kinfo.consoleid = txtConsoleID.Text;
            kinfo.region = getregion(comboRegion.SelectedIndex);
            kinfo.dvdkey = txtDVDkey.Text;
            kinfo.osig = getosig(comboOsig.SelectedIndex);
            kinfo.serial = txtSerial.Text;
            kinfo.mfdate = txtMfrDate.Text;

            Nand.Nand.patch_kv(ref keyvault, kinfo);
            keyvault = Nand.Nand.encryptkv_hmac(keyvault, Oper.StringToByteArray(variables.cpukey));
            if (ecc)
            {
                if (variables.debugMode) Console.WriteLine("Adding ecc to keyvault");
                keyvault = Nand.Nand.addecc_v2(keyvault, true, 0x4200, layout);
                if (variables.debugMode) Console.WriteLine("Completed");
                if (variables.debugMode) Console.WriteLine("{0:X} - {1:X}", image.Length, keyvault.Length);

                image.Replace(keyvault, 0x4200, keyvault.Length);
            }
            else
            {
                image.Replace(keyvault, 0x4000, keyvault.Length);
            }

            Console.WriteLine("KV Patch Successful");
            return image;
        }
        
        string getosig(int number)
        {
            string[] osigbytes = {
                "058000321F0000005042445320202020564144363033382020202020202020203034343231432020",
                "058000321F0000005042445320202020564144363033382D36343933304320202020202020202020",
                "058000325B000000504C44532020202044472D313644325320202020202020203032353130434130",
                "058000325B000000504C44532020202044472D313644325320202020202020203734383530434130",
                "058000325B000000504C44532020202044472D313644325320202020202020203833383530434130",
                "058000325B000000504C44532020202044472D313644325320202020202020203933343530434130",
                "058000325B000000504C44532020202044472D313644345320202020202020203032323500000000",
                "058000325B000000504C44532020202044472D313644345320202020202020203032373200000000",
                "058000325B000000504C44532020202044472D313644345320202020202020203034303100000000",
                "058000325B000000504C44532020202044472D313644345320202020202020203130373100000000",
                "058000325B000000504C44532020202044472D313644345320202020202020203935303400000000",
                "058000325B000000504C44532020202044472D313644355320202020202020203131373500000000",
                "058000325B000000504C44532020202044472D313644355320202020202020203135333200000000",
                "058000325B00000054535354636F72704456442D524F4D2054532D48393433416D73323520202000",
                "058000325B00000054535354636F72704456442D524F4D2054532D48393433416D73323820202000",
                "058000325B000000484C2D44542D53544456442D524F4D20474452333132304C3030333230424D42",
                "058000325B000000484C2D44542D53544456442D524F4D20474452333132304C3030333630424D42",
                "058000325B000000484C2D44542D53544456442D524F4D20474452333132304C3030343030424D42",
                "058000325B000000484C2D44542D53544456442D524F4D20474452333132304C3030343630424D42",
                "058000325B000000484C2D44542D53544456442D524F4D20474452333132304C3030343730424D42",
                "058000325B000000484C2D44542D53544456442D524F4D20474452333132304C3030353830424D42",
                "058000325B000000484C2D44542D53544456442D524F4D20474452333132304C3030353930424D42",
                "058000325B000000484C2D44542D53544456442D524F4D20474452333132304C3030373830424D42",
                "058000325B000000484C2D44542D53544456442D524F4D20474452333132304C3030373930424D42",
                "058000326F000000484C2D44542D53544456442D524F4D20444C31304E2020203035303000000000",
                "058000326F000000484C2D44542D53544456442D524F4D20444C31304E2020203035303200000000",
                "00000000000000000000000000000000000000000000000000000000000000000000000000000000"
            };
            return osigbytes[number];
        }

        string getregion(int number)
        {
            string[] regionbytes = { "02FE", "00FF", "01FE", "01FF", "01FC", "0101", "0201", "7FFF" };
            return regionbytes[number];
        }

        #region changed

        private void comboRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            changed = true;
        }
        private void txtDVDkey_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }
        private void txtSerial_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }
        private void txtConsoleID_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }
        private void comboOsig_SelectedIndexChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        #endregion

        private void txtSerial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtConsoleID_KeyPress(object sender, KeyPressEventArgs e)
        {
            Regex objAlphaPattern = new Regex("[A-F]$");
            if (!char.IsControl(e.KeyChar)
               && !char.IsDigit(e.KeyChar)
                && !objAlphaPattern.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }
        }

        private void txtDVDkey_KeyPress(object sender, KeyPressEventArgs e)
        {
            Regex objAlphaPattern = new Regex("[0-9A-F]$");
            if (!char.IsControl(e.KeyChar)
               && !char.IsDigit(e.KeyChar)
                && !objAlphaPattern.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }
        }
    }
}
