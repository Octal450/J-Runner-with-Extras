using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JRunner
{
    public partial class patch : Form
    {
        static bool changed = false, changedCF0ldv = false, changedCF1ldv = false, changedCF0pd = false, changedCF1pd = false;
        public MainForm frm1;
        public patch()
        {
            InitializeComponent();
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
            btnOK.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
#if Dev
            groupBox1.Visible = true;
#else
            groupBox1.Visible = false;
#endif
        }

        private void patch_Load(object sender, EventArgs e)
        {
            try
            {
                txtCBldv.Text = MainForm.nand.uf.ldv_cb.ToString();
                txtCF0ldv.Text = MainForm.nand.uf.ldv_p0.ToString();
                txtCF1ldv.Text = MainForm.nand.uf.ldv_p1.ToString();
                textCBpd.Text = MainForm.nand.uf.pd_cb.ToString();
                txtCF0pd.Text = MainForm.nand.uf.pd_0.ToString();
                txtCF1pd.Text = MainForm.nand.uf.pd_1.ToString();
                if (MainForm.nand.ki.region != "" || MainForm.nand.ki.osig != "")
                {
                    foreach (string region in comboRegion.Items)
                    {
                        if (region.Contains(MainForm.nand.ki.region)) comboRegion.Text = region;
                    }
                    foreach (string osig in comboOsig.Items)
                    {
                        if (osig.EndsWith(MainForm.nand.ki.osig.Substring(MainForm.nand.ki.osig.Length - 4))) comboOsig.Text = osig;
                    }
                }
                txtDVDkey.Text = MainForm.nand.ki.dvdkey;
                txtSerial.Text = MainForm.nand.ki.serial;
                txtConsoleID.Text = MainForm.nand.ki.consoleid;
            }
            catch (System.ArgumentNullException) { return; }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            comboRegion.Enabled = comboOsig.Enabled = txtDVDkey.Enabled = txtConsoleID.Enabled = txtSerial.Enabled = checkBox2.Checked;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtCBldv.Enabled = true;
                txtCF0ldv.Enabled = true;
                txtCF1ldv.Enabled = true;
                textCBpd.Enabled = true;
                txtCF0pd.Enabled = true;
                txtCF1pd.Enabled = true;
            }
            else
            {
                txtCBldv.Enabled = false;
                txtCF0ldv.Enabled = false;
                txtCF1ldv.Enabled = false;
                textCBpd.Enabled = false;
                txtCF0pd.Enabled = false;
                txtCF1pd.Enabled = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!changed) this.Close();
            else
            {
                if (variables.filename1 == null || variables.cpkey == "") this.Close();
                byte[] data = Nand.BadBlock.find_bad_blocks_b(variables.filename1, true);

                if (data.Length > 0x4500)
                {
                    int layout;
                    byte[] sparedata = new byte[0x10];

                    Buffer.BlockCopy(data, 0x4400, sparedata, 0, 0x10);

                    layout = Nand.Nand.identifylayout(sparedata);

                    if (checkBox2.Checked)
                    {
                        data = KV_Stuff(ref data, layout);
                    }
                    if (checkBox1.Checked)
                    {
                        data = Bootloader_Stuff(ref data, layout);
                    }

                    if (File.Exists(variables.filename1))
                    {
                        string outpath = Path.Combine(Path.GetDirectoryName(variables.filename1), Path.GetFileNameWithoutExtension(variables.filename1) + "_old" + Path.GetExtension(variables.filename1));
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

        private byte[] KV_Stuff(ref byte[] image, int layout)
        {
            Console.WriteLine("Processing KV...");
            bool ecc;
            if (image[0x205] == 0xFF || image[0x415] == 0xFF || image[0x200] == 0xFF) ecc = true;
            else ecc = false;

            byte[] blockarray = new byte[4];
            Buffer.BlockCopy(image, 0x4400, blockarray, 0, 4);

            byte[] keyvault = Nand.Nand.decryptkv(Nand.Nand.getkv(image), Oper.StringToByteArray(variables.cpkey));
            Nand.KVInfo kinfo = new Nand.KVInfo();
            kinfo.consoleid = txtConsoleID.Text;
            kinfo.region = getregion(comboRegion.SelectedIndex);
            kinfo.dvdkey = txtDVDkey.Text;
            kinfo.osig = getosig(comboOsig.SelectedIndex);
            kinfo.serial = txtSerial.Text;

            Nand.Nand.patch_kv(ref keyvault, kinfo);
            keyvault = Nand.Nand.encryptkv_hmac(keyvault, Oper.StringToByteArray(variables.cpkey));
            if (ecc)
            {
                if (variables.debugme) Console.WriteLine("Adding ecc to keyvault");
                keyvault = Nand.Nand.addecc_v2(keyvault, true, 0x4200, layout);
                if (variables.debugme) Console.WriteLine("Completed");
                if (variables.debugme) Console.WriteLine("{0:X} - {1:X}", image.Length, keyvault.Length);

                image.Replace(keyvault, 0x4200, keyvault.Length);
            }
            else
            {
                image.Replace(keyvault, 0x4000, keyvault.Length);
            }

            Console.WriteLine("KV Patch Successful");
            return image;
        }

        private byte[] Bootloader_Stuff(ref byte[] data, int layout)
        {
            Console.WriteLine("Processing CF...");
            byte[] CF0 = null, CF1 = null;
            byte[] CF0_dec = null, CF1_dec = null;
            int CF0offset = 0, CF1offset = 0;

            if (data[0] == 0xFF && data[1] == 0x4F)
            {
                Nand.Nand.getCF(data, layout == 2 ? true : false, out CF0, out CF0offset, out CF1, out CF1offset);
                if (variables.debugme) Console.WriteLine("CF0 offset: {0:X} - CF0 size: {1:X}", CF0offset, CF0.Length);
                if (variables.debugme) Console.WriteLine("CF1 offset: {0:X} - CF1 size: {1:X}", CF1offset, CF1.Length);
            }
            if (CF0 == null && CF1 == null)
            {
                Console.WriteLine("Failed");
                this.Close();
            }
            bool ready0 = false, ready1 = false;
            if (CF0 != null && (changedCF0ldv || changedCF0pd))
            {
                if (variables.debugme) Console.WriteLine("Performing operations on CF0");
                CF0_dec = Nand.Nand.decrypt_CF(CF0);
                CF0_dec[0x21F] = Convert.ToByte(txtCF0ldv.Text, 10);
                CF0_dec[0x21C] = Convert.ToByte(txtCF0pd.Text.Substring(6, 2), 16);
                CF0_dec[0x21D] = Convert.ToByte(txtCF0pd.Text.Substring(4, 2), 16);
                CF0_dec[0x21E] = Convert.ToByte(txtCF0pd.Text.Substring(2, 2), 16);
                CF0 = Nand.Nand.encrypt_CF(CF0_dec, CF0, Oper.StringToByteArray(variables.cpkey));
                ready0 = true;
            }
            if (CF1 != null && (changedCF1ldv || changedCF1pd))
            {
                if (variables.debugme) Console.WriteLine("Performing operations on CF1");
                CF1_dec = Nand.Nand.decrypt_CF(CF1);
                CF1_dec[0x21F] = Convert.ToByte(txtCF1ldv.Text, 10);
                CF1_dec[0x21C] = Convert.ToByte(txtCF1pd.Text.Substring(6, 2), 16);
                CF1_dec[0x21D] = Convert.ToByte(txtCF1pd.Text.Substring(4, 2), 16);
                CF1_dec[0x21E] = Convert.ToByte(txtCF1pd.Text.Substring(2, 2), 16);
                CF1 = Nand.Nand.encrypt_CF(CF1_dec, CF1, Oper.StringToByteArray(variables.cpkey));
                ready1 = true;
            }
            if (ready0)
            {
                if (variables.debugme) Console.WriteLine("Adding ecc on CF0");
                CF0 = Nand.Nand.addecc_v2(CF0, true, (CF0offset / 0x200) * 0x210, layout);
                if (variables.debugme) Console.WriteLine("Adding CF0 to nand");
                Buffer.BlockCopy(CF0, 0, data, (CF0offset / 0x200) * 0x210, CF0.Length);
            }
            if (ready1)
            {
                if (variables.debugme) Console.WriteLine("Adding ecc on CF1");
                CF1 = Nand.Nand.addecc_v2(CF1, true, (CF1offset / 0x200) * 0x210, layout);
                if (variables.debugme) Console.WriteLine("Adding CF1 to nand");
                Buffer.BlockCopy(CF1, 0, data, (CF1offset / 0x200) * 0x210, CF1.Length);
            }
            Console.WriteLine("Bootloader Patch Successful");
            return data;
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
            string[] regionbytes = { "02FE",
                    "00FF",
                     "01FE",
                     "01FF",
                     "01FC",
                     "0101",
                    "0201",
                     "7FFF"
                                             };
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
        private void txtCBldv_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }
        private void txtCBpd_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }
        private void txtCF0ldv_TextChanged(object sender, EventArgs e)
        {
            changed = true;
            changedCF0ldv = true;
        }
        private void txtCF0pd_TextChanged(object sender, EventArgs e)
        {
            changed = true;
            changedCF0pd = true;
        }
        private void txtCF1ldv_TextChanged(object sender, EventArgs e)
        {
            changed = true;
            changedCF1ldv = true;
        }
        private void txtCF1pd_TextChanged(object sender, EventArgs e)
        {
            changed = true;
            changedCF1pd = true;
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
