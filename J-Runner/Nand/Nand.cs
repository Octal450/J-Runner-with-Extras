using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace JRunner.Nand
{
    public struct Bootloaders
    {
        // BL Versions
        public int CB_A;
        public int CB_B;
        public int CB_X;
        public int SC;
        public int CD;
        public int CE;
        public int CF_0;
        public int CG_0;
        public int CF_1;
        public int CG_1;

        // BL Magic Strings (e.g. CB, SC, CD)
        public string _2BL_magic;
        public string _3BL_magic;
        public string _4BL_magic;
        public string _5BL_magic;
    }
    public struct SMCInfo
    {
        public string smcver;
    }
    public struct KVInfo
    {
        public string osig;
        public string dvdkey;
        public string serial;
        public string consoleid;
        public string region;
        public string kvtype;
        public string mfdate;
        public bool fcrtflag;
    }
    public struct SMCConfig
    {
        public SMCConfig(bool what)
        {
            this.ok = false;
            this.powervcs = null;
            this.ana = null;
            this.anabackup = null;
            this.bit = null;
            this.boardgain = null;
            this.boardoff = null;
            this.checksum = null;
            this.clock = null;
            this.config = null;
            this.cpufanspeed = null;
            this.cpugain = null;
            this.cpuoff = null;
            this.dramgain = null;
            this.dramoff = null;
            this.dvdregion = null;
            this.flags = null;
            this.gainoff = null;
            this.gameregion = null;
            this.gpufanspeed = null;
            this.gpugain = null;
            this.gpuoff = null;
            this.mac = null;
            this.net = null;
            this.pwrmode = null;
            this.reserve0 = null;
            this.reserve1 = null;
            this.reserve2 = null;
            this.reserve3 = null;
            this.reserve4 = null;
            this.reserve5 = null;
            this.reset = null;
            this.structure = null;
            this.thermal = null;
            this.version = null;
            this.videoregion = null;
        }

        public bool ok;
        public byte[] checksum;
        public byte[] structure;
        public byte[] config;
        public byte[] bit;
        public byte[] mac;
        public byte[] cpugain;
        public byte[] cpuoff;
        public byte[] gpugain;
        public byte[] gpuoff;
        public byte[] dramgain;
        public byte[] dramoff;
        public byte[] boardgain;
        public byte[] boardoff;
        public byte[] ana;
        public byte[] anabackup;
        public byte[] clock;
        public byte[] flags;
        public byte[] version;
        public byte[] net;
        public byte[] reset;
        public byte[] thermal;
        public byte[] gainoff;
        public byte[] dvdregion;
        public byte[] gameregion;
        public byte[] videoregion;
        public byte[] pwrmode;
        public byte[] powervcs;
        public byte[] reserve0;
        public byte[] reserve1;
        public byte[] reserve2;
        public byte[] reserve3;
        public byte[] reserve4;
        public byte[] reserve5;
        public byte[] cpufanspeed;
        public byte[] gpufanspeed;
    }
    public struct Useful
    {
        public string pd_0;
        public string pd_1;
        public string pd_cb;
        public int ldv_p0;
        public int ldv_p1;
        public int ldv_cb;
    }
    public class FSFile
    {
        string filename;
        int length;
        int block;

        public FSFile(string filename, int block, int length)
        {
            this.filename = filename;
            this.block = block;
            this.length = length;
        }

        public FSFile()
        {
            this.filename = "";
            this.block = 0;
            this.length = 0;
        }

        public string getFilename() { return filename; }
        public int getLength() { return length; }
        public int getBlock() { return block; }
    }
    public class PrivateN
    {
        public bool ok = false;
        public Bootloaders bl;
        public SMCInfo si;
        public KVInfo ki;
        public Useful uf;
        public string _cpukey = "", _filename;
        private int _currentFS = 0;
        public bool noecc = false, bigblock = false, bigflash = false;
        public byte[] _rawkv, _smc, _smc_config;
        public List<int> bad_blocks = new List<int>(), remapped_blocks = new List<int>();
        private List<FSFile> Files = new List<FSFile>();

        private void erasev()
        {
            _cpukey = "";
            _filename = "";
            _currentFS = 0;
            ok = noecc = bigblock = bigflash = false;
            bad_blocks = new List<int>();
            remapped_blocks = new List<int>();
            Files = new List<FSFile>();
            bl.CB_A = 0;
            bl.CB_B = 0;
            si.smcver = "";
            ki.osig = "";
            ki.serial = "";
            ki.region = "";
            ki.dvdkey = "";
            ki.consoleid = "";
            ki.kvtype = "";
            ki.mfdate = "";
        }

        public PrivateN()
        {
            erasev();
        }
        public PrivateN(string filename, string cpukey = "")
        {
            erasev();
            Encoding ascii = Encoding.ASCII;
            _filename = filename;
            _cpukey = cpukey;
            FileInfo f = new FileInfo(filename);
            long s1 = f.Length;
            if (s1 == 0x40000) return;
            byte[] data = BadBlock.find_bad_blocks_X(filename, 0x50);
            //
            if (s1 >= 0x4200000 && s1 <= 0x21000000)
            {
                bigflash = true;
                if (data[0x205] == 0xFF) bigblock = false;
                else bigblock = true;
            }
            //
            byte[] temp = new byte[0x210];
            Buffer.BlockCopy(data, 0, temp, 0, data.Length > temp.Length ? temp.Length : data.Length);
            if (!ascii.GetString(temp).Contains("Microsoft"))
            {
                if (variables.debugMode) Console.WriteLine(ascii.GetString(temp));
                if (temp[0] == 0x46 && temp[1] == 0x57 && temp[2] == 0x41 && temp[3] == 0x00) Console.WriteLine("DemoN Firmware");
            }
            //

            // Early NAND images begin with 0x0F3F or 0x0F4F
            // Regular NAND images begin with 0xFF4F
            if( (data[0] == 0xFF || data[0] == 0x0F) &&
                (data[1] == 0x3F || data[1] == 0x4F) )
            {
                unpack_base_image(data, bigblock);

                if (cpukeyverification(_cpukey)) { }
                else if (cpukeyverification(Oper.ByteArrayToString(Oper.returnportion(data, 0x99AA0, 0x10)))) { }

                data = null;
                ok = true;
            }
            else
            {
                return;
            }
        }

        private void unpack_cbb_data(byte[] cb_dec)
        {
            // Encrypted CB_Bs introduce problems parsing this data
            if (cb_dec[0xA0] == 0 && cb_dec[0xA7] == 0 && cb_dec[0xAF] == 0)
            {
                if (cb_dec[0x02] == 0x3C && cb_dec[0x03] == 0x48) uf.ldv_cb = 0;
                else if (cb_dec[0x3B1] <= 16) uf.ldv_cb = cb_dec[0x3B1];

                if (variables.debugMode) Console.WriteLine("LDV CB: {0}", uf.ldv_cb.ToString());

                byte[] temppd = (Oper.returnportion(cb_dec, 0x20, 3));
                Array.Reverse(temppd);
                uf.pd_cb = "0x" + Oper.ByteArrayToString(temppd);
                if (variables.debugMode) Console.WriteLine("-Pairing Data: " + uf.pd_cb);
            }
        }

        void unpack_base_image(byte[] image, bool bigblock)
        {
            byte[] data, cb_dec = { }, sc_dec = { }, cd_dec = { }, ce_dec = { };
            byte[] CB_A = null, CB_B = null, CB_X = null, SC = null, CD = null, CE = null;
            bl.CB_A = 0; bl.CB_B = 0; bl.CB_X = 0;  bl.CD = 0; bl.CE = 0; bl.CF_0 = 0; bl.CG_0 = 0; bl.CF_1 = 0; bl.CG_1 = 0;
            bl._2BL_magic = "";bl._3BL_magic = "";bl._4BL_magic = "";bl._5BL_magic = "";
            uf.ldv_p0 = 0; uf.ldv_p1 = 0; uf.ldv_cb = 0; uf.pd_cb = ""; uf.pd_0 = ""; uf.pd_1 = "";

            if (Nand.rawecc(image)) Console.WriteLine("Image is raw");
            if (Nand.hasecc_v2(ref image)) Nand.unecc(ref image, false);
            else noecc = true;

            if (variables.debugMode) Console.WriteLine("Has ecc? !{0}", noecc);

            byte[] block_offset = new byte[4];
            block_offset = Oper.returnportion(image, 0x8, 4);
            variables.smcmbtype = 0;
            try
            {
                byte[] SMC = null, Keyvault = null;
                byte[] smc_len = new byte[4], smc_start = new byte[4];
                Buffer.BlockCopy(image, 0x78, smc_len, 0, 4);
                Buffer.BlockCopy(image, 0x7C, smc_start, 0, 4);
                SMC = new byte[Oper.ByteArrayToInt(smc_len)];
                Buffer.BlockCopy(image, Oper.ByteArrayToInt(smc_start), SMC, 0, Oper.ByteArrayToInt(smc_len));
                if (variables.extractfiles) Oper.savefile(SMC, "output\\SMC_en.bin");
                SMC = Nand.decrypt_SMC(SMC);
                if (variables.extractfiles) Oper.savefile(SMC, "output\\SMC_dec.bin");
                si.smcver = SMC[0x101] + "." + SMC[0x102].ToString("D2");
                variables.smcmbtype = SMC[0x100] >> 4 & 15;
                _smc = SMC;
                SMC = null;

                #region keyvault
                Keyvault = new byte[0x4000];
                Buffer.BlockCopy(image, 0x4000, Keyvault, 0, 0x4000);
                _rawkv = Keyvault;
                if (variables.extractfiles) Oper.savefile(Keyvault, "output\\KV_en.bin");
                Keyvault = null;
                #endregion
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }

            #region blocks
            try
            {
                int block = 0, block_size, id;
                byte block_id;
                bool isDevBl = false;
                string blIdString = "";
                int block_build;
                byte[] block_build_b = new byte[2], block_size_b = new byte[4];
                int block_offset_b = Convert.ToInt32(Oper.ByteArrayToString(block_offset), 16);

                int _2bl_idx = 0;

                for (block = 0; block < 30; block++)
                {
                    // Dev BLs start with characters other than 0x43 (C)
                    isDevBl = (image[block_offset_b] != 0x43);

                    // Get the ID string of the BL (CB/CD/SB/SD/etc)
                    blIdString = Encoding.ASCII.GetString(image.Skip(block_offset_b).Take(2).ToArray());

                    // If the first character of the ID string is garbage, it's an old dev bootloader
                    // Replace it with an S character
                    if (Path.GetInvalidFileNameChars().Contains(blIdString[0]))
                    {
                        blIdString = "S" + blIdString.Substring(1);
                    }

                    block_id = image[block_offset_b + 1];
                    Buffer.BlockCopy(image, block_offset_b + 2, block_build_b, 0, 2);
                    //block_build_b = returnportion(image, block_offset_b + 2, 2);
                    Buffer.BlockCopy(image, block_offset_b + 12, block_size_b, 0, 4);
                    //block_size_b = returnportion(image, block_offset_b + 12, 4);
                    block_size = Convert.ToInt32(Oper.ByteArrayToString(block_size_b), 16);
                    block_build = Convert.ToInt32(Oper.ByteArrayToString(block_build_b), 16);
                    block_size += 0xF;
                    block_size &= ~0xF;
                    id = block_id & 0xF;

                    if (id == 0 || block_size == 0)
                    {
                        break;
                    }

                    if (variables.debugMode) Console.WriteLine("Found {0} {1}BL (build {2}) at {3}", isDevBl ? "dev" : "retail", id, block_build, Convert.ToString(block_offset_b, 16));
                    data = new byte[block_size];
                    //data = returnportion(image, block_offset_b, block_size);
                    if (block_offset_b + block_size <= image.Length) Buffer.BlockCopy(image, block_offset_b, data, 0, block_size);
                    if (id == 2)
                    {
                        bl._2BL_magic = blIdString;

                        if (_2bl_idx == 0)
                        {
                            // First time through this function we've got a single CB or a CB_A
                            bl.CB_A = block_build;
                            CB_A = data;
                        }
                        else if (_2bl_idx == 1)
                        {
                            bl.CB_B = block_build;
                            CB_B = data;
                        }
                        else if(_2bl_idx == 2)
                        {
                            // If we're seeing this function three times, that means the
                            // previous CB_B was actually an RGH3 CB_X or RGH 1.3 CB_Y
                            // Anything more is a bug... if we ever get in to a scenario
                            // where there are 4 or more CB stages then you'll need to
                            // update JRunner and pray for whoever created CB_4
                            bl.CB_X = bl.CB_B;
                            CB_X = CB_B;

                            bl.CB_B = block_build;
                            CB_B = data;
                        }

                        _2bl_idx++;
                    }
                    else if (id == 3)
                    {
                        bl._3BL_magic = blIdString;
                        bl.SC = block_build;
                        SC = data;
                    }
                    else if (id == 4)
                    {
                        bl._4BL_magic = blIdString;
                        bl.CD = block_build;
                        CD = data;
                    }
                    else if (id == 5)
                    {
                        bl.CE = block_build;
                        bl._5BL_magic = blIdString;
                        CE = data;
                    }
                    block_offset_b += block_size;
                    if (id == 5) break;
                }

                // We're done scanning the first few blocks. Now we can figure out the CB_A/CB_X/CB_B situation
                if (bl.CB_A > 0)
                {
                    if (bl._2BL_magic == "S2")
                    {
                        // DD1 images use a different 1BL key than DD2+
                        cb_dec = Nand.decrypt_S2(CB_A);
                    }
                    else
                    {
                        cb_dec = Nand.decrypt_CB(CB_A);
                    }

                    if (variables.extractfiles) Oper.savefile(CB_A, "output\\" + bl._2BL_magic + "_A.bin");
                    if (variables.extractfiles) Oper.savefile(cb_dec, "output\\" + bl._2BL_magic + "_A_dec.bin");

                    if (cb_dec[0x3B1] <= 16) uf.ldv_cb = cb_dec[0x3B1];

                    if (variables.debugMode) Console.WriteLine("LDV CB: {0}", uf.ldv_cb.ToString());
                    byte[] temppd = (Oper.returnportion(cb_dec, 0x20, 3));
                    Array.Reverse(temppd);
                    uf.pd_cb = "0x" + Oper.ByteArrayToString(temppd);
                    if (variables.debugMode) Console.WriteLine("-Pairing Data: " + uf.pd_cb);
                }

                if (bl.CB_X > 0)
                {
                    if (variables.extractfiles) Oper.savefile(CB_X, "output\\CB_X.bin");
                    if (string.IsNullOrEmpty(variables.cpukey)) cb_dec = Nand.decrypt_CB_cpukey(CB_X, Nand.decrypt_CB(CB_A), Oper.StringToByteArray("00000000000000000000000000000000")); // It just needs something, doesn't matter that its not valid
                    else cb_dec = Nand.decrypt_CB_cpukey(CB_X, Nand.decrypt_CB(CB_A), Oper.StringToByteArray(variables.cpukey));
                    if (variables.extractfiles) Oper.savefile(cb_dec, "output\\CB_X_dec.bin");
                }

                if (bl.CB_B > 0)
                {
                    // If we've got a CB_X (RGH3 or RGH 1.3), then the CB_B is
                    // stored in plaintext. No need to decrypt it first
                    if (bl.CB_X > 0)
                    {
                        cb_dec = CB_B;
                        if (variables.extractfiles) Oper.savefile(cb_dec, "output\\" + bl._2BL_magic + "_B_dec.bin");
                        unpack_cbb_data(cb_dec);
                    }
                    else
                    {
                        if (variables.extractfiles) Oper.savefile(CB_B, "output\\" + bl._2BL_magic + "_B.bin");
                        if (string.IsNullOrEmpty(variables.cpukey)) cb_dec = Nand.decrypt_CB_cpukey(CB_B, Nand.decrypt_CB(CB_A), Oper.StringToByteArray("00000000000000000000000000000000")); // It just needs something, doesn't matter that its not valid
                        else cb_dec = Nand.decrypt_CB_cpukey(CB_B, Nand.decrypt_CB(CB_A), Oper.StringToByteArray(variables.cpukey));
                        if (variables.extractfiles) Oper.savefile(cb_dec, "output\\" + bl._2BL_magic + "_B_dec.bin");

                        // Encrypted CB_Bs introduce problems parsing this data
                        unpack_cbb_data(cb_dec);
                    }
                }

                if (bl.SC > 0)
                {
                    if (variables.extractfiles) Oper.savefile(SC, "output\\" + bl._3BL_magic + ".bin");
                    sc_dec = Nand.decrypt_SC(SC);
                    if (variables.extractfiles) Oper.savefile(sc_dec, "output\\" + bl._3BL_magic + "_dec.bin");
                }

                if (bl._4BL_magic != "")
                {

                    if (variables.extractfiles) Oper.savefile(CD, "output\\" + bl._4BL_magic + ".bin");

                    // If there was a 3BL, the 4BL encryption is derived
                    // from it rather than the 2BL
                    if (bl.SC > 0)
                    {
                        // In case someone tries to open an XDKbuild image that hasn't been
                        // patched, only try to decrypt the SD if the SC was decrypted OK
                        if (sc_dec.Length > 0)
                        {
                            cd_dec = Nand.decrypt_SD(CD, sc_dec);
                            if (variables.extractfiles) Oper.savefile(cd_dec, "output\\" + bl._4BL_magic + "_dec.bin");
                        }
                    }
                    else
                    {
                        cd_dec = Nand.decrypt_CD_cpukey(CD, cb_dec, Oper.StringToByteArray(variables.cpukey));
                        if (variables.extractfiles) Oper.savefile(cd_dec, "output\\" + bl._4BL_magic + "_dec.bin");
                    }
                }

                if (bl._5BL_magic != "")
                {
                    if (variables.extractfiles) Oper.savefile(CE, "output\\" + blIdString + ".bin");
                    ce_dec = Nand.decrypt_CE(CE, cd_dec);
                    if (variables.extractfiles) Oper.savefile(ce_dec, "output\\" + blIdString + "_dec.bin");
                }
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
            #endregion

            #region Patch Parsing



            #endregion

            try
            {
                unpack_update(ref image, bigblock);
            }
            catch (System.IndexOutOfRangeException ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
            catch (System.OutOfMemoryException ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }

        }
        void unpack_update(ref byte[] image, bool bigblock)
        {
            byte[] CF0 = new byte[0x100], CF1 = new byte[0x100];
            if (variables.extractfiles) Oper.savefile(image, "image.bin");
            int size = image.Length;
            int blocksize, patch_offset = 0;
            if (bigblock) blocksize = 0x20000;
            else blocksize = 0x4000;
            int block = 0, block_size, id;
            byte block_id;
            int block_build;
            byte[] block_build_b = new byte[2], block_size_b = new byte[4];
            int block_offset_b = 0;
            int patch = 0;
            try
            {
                for (block = 0; block < 10; block++)
                {
                    if (block_offset_b + 1 >= image.Length || block_offset_b + 1 < 0) break;
                    block_id = image[block_offset_b + 1];
                    //if (variables.debugme) Console.WriteLine("Block ID: {0} | Block offset: {1:X}", block_id, block_offset_b);
                    int temp_block_offset = block_offset_b;
                    //block_build_b = returnportion(image, block_offset_b + 2, 2);
                    Buffer.BlockCopy(image, block_offset_b + 2, block_build_b, 0, 2);
                    //block_size_b = returnportion(image, block_offset_b + 12, 4);
                    Buffer.BlockCopy(image, block_offset_b + 12, block_size_b, 0, 4);
                    block_size = Convert.ToInt32(Oper.ByteArrayToString(block_size_b), 16);
                    block_build = Convert.ToInt32(Oper.ByteArrayToString(block_build_b), 16);
                    //if (variables.debugme) Console.WriteLine("Block Build {0} : Block Size {1:X}", block_build, block_size);
                    block_size += 0xF;
                    block_size &= ~0xF;
                    id = block_id & 0xF;
                    if (block_size > image.Length) break;
                    byte[] data = new byte[block_size];
                    //byte[] data = returnportion(image, block_offset_b, block_size);
                    if (block_size + block_offset_b <= image.Length)
                    {
                        //if (variables.debugme) Console.WriteLine("Copying to buffer..");
                        Buffer.BlockCopy(image, block_offset_b, data, 0, block_size);
                    }
                    else
                    {
                        if (variables.debugMode) Console.WriteLine("block size: 0x{0:X} - offset: 0x{1:X} - image: 0x{2:X}", block_size, block_offset_b, image.Length);
                    }

                    if (id == 6 || id == 7)
                    {
                        if (variables.debugMode) Console.WriteLine("-Found {0}BL Patch {3} (build {1}) at {2:X}", id, block_build, block_offset_b, patch);
                        if (id == 6)
                        {
                            patch_offset = block_offset_b;

                            if (patch == 0)
                            {
                                CF0 = Nand.decrypt_CF(data);
                                bl.CF_0 = block_build;
                                uf.ldv_p0 = Nand.decrypt_CF(data)[0x21F];
                                if (variables.debugMode) Console.WriteLine("-LDV Patch {0}: {1}", patch, uf.ldv_p0);
                                byte[] temppd = (Oper.returnportion(Nand.decrypt_CF(data), 0x21C, 3));
                                Array.Reverse(temppd);
                                uf.pd_0 = "0x" + Oper.ByteArrayToString(temppd);
                                if (variables.debugMode) Console.WriteLine("-Pairing Data 0: {0:X}", uf.pd_0);
                            }
                            else
                            {
                                CF1 = Nand.decrypt_CF(data);
                                bl.CF_1 = block_build;
                                uf.ldv_p1 = Nand.decrypt_CF(data)[0x21F];
                                if (variables.debugMode) Console.WriteLine("-LDV Patch {0}: {1}", patch, uf.ldv_p1);
                                byte[] temppd = (Oper.returnportion(Nand.decrypt_CF(data), 0x21C, 3));
                                Array.Reverse(temppd);
                                uf.pd_1 = "0x" + Oper.ByteArrayToString(temppd);
                                if (variables.debugMode) Console.WriteLine("-Pairing Data 1: {0:X}", uf.pd_1);
                            }

                            if (variables.extractfiles)
                            {
                                Oper.savefile(data, "output\\CF" + patch + ".bin");
                                Oper.savefile(Nand.decrypt_CF(data), "output\\CF" + patch + "_dec.bin");
                            }
                        }
                        else if (id == 7)
                        {
                            if (variables.extractfiles)
                            {
                                Oper.savefile(data, "output\\CG" + patch + ".bin");
                                Oper.savefile(Nand.decrypt_CG(data, patch == 0 ? CF0 : CF1), "output\\CG" + patch + "_dec.bin");
                            }
                            if (patch == 0)
                            {
                                bl.CG_0 = block_build;
                                //block_offset_b += 0xBBB0;
                                patch = 1;
                            }
                            else
                            {
                                bl.CG_1 = block_build;
                                break;
                            }
                        }
                    }
                    if ((patch_offset + blocksize + 1 > image.Length) || (patch_offset + 0x10001 > image.Length) || (block_offset_b + block_size + 1 > image.Length)) break;
                    int tem0 = image[patch_offset + blocksize];
                    int temo = image[patch_offset + blocksize + 1];
                    int tem2 = image[patch_offset + 0x10001];
                    int tem1 = image[patch_offset + 0x10000];
                    int tem3 = image[block_offset_b + 0x10000];
                    int tem4 = image[block_offset_b + 0x10001];
                    int tem5 = image[block_offset_b + block_size];
                    int tem6 = image[block_offset_b + block_size + 1];
                    if (patch == 1 && block_offset_b < 0x80000 && tem2 == 0x46 && tem1 == 0x43)
                    {
                        if (variables.debugMode) Console.WriteLine("2 - {0:X}", block_offset_b);
                        block_offset_b = patch_offset + 0x10000;
                        continue;
                    }
                    else if (temo == 0x46 && tem0 == 0x43 && patch == 1)
                    {
                        if (variables.debugMode) Console.WriteLine("1 - {0:X}", block_offset_b);
                        block_offset_b = patch_offset + blocksize;
                        continue;
                    }
                    else if (patch == 0 && tem3 == 0x43 && tem4 == 0x46 && tem5 != 0x43 && tem6 != 0x47)
                    {
                        if (variables.debugMode) Console.WriteLine("4 - {0:X}", block_offset_b);
                        block_offset_b += 0x10000;
                        patch = 1;
                        continue;
                    }

                    else if (patch == 0 && block_offset_b > 0x80000 && patch_offset < 0x80000)
                    {
                        if (variables.debugMode) Console.WriteLine("3 - {0:X}", block_offset_b);
                        patch = 1;
                        block_offset_b = 0x80000;
                        continue;
                    }
                    if (block_size == 0x10) { block_size = 0x20000; patch = 1; }
                    block_offset_b += block_size;
                    if (variables.debugMode) Console.WriteLine("5 - {0:X}", block_offset_b);
                    if (temp_block_offset == block_offset_b) break;
                    if (block_offset_b > size) break;
                }
            }
            catch (System.OverflowException) { return; }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }

        }
        public static byte[] fill(int startoffset, int length, byte fil)
        {
            byte[] bi = new byte[length];
            int num2 = length - 1;
            for (int i = 0; i <= num2; i++)
            {
                bi[startoffset + i] = fil;
            }
            return bi;
        }
        public void updatekvval()
        {
            Encoding ascii = Encoding.ASCII;
            byte[] Keyvault;
            if (Oper.allsame(Oper.returnportion(_rawkv, 0x40, 0x20), 0x00)) Keyvault = _rawkv;
            else Keyvault = Nand.decryptkv(_rawkv, Oper.StringToByteArray(_cpukey));

            if (!Oper.allsame(Oper.returnportion(Keyvault, 0x40, 0x20), 0x00)) Keyvault = fill(0, 0x4000, 0x20);

            ki.serial = ascii.GetString(Oper.returnportion(Keyvault, 0xB0, 12));
            ki.dvdkey = Oper.ByteArrayToString(Oper.returnportion(Keyvault, 0x100, 16));
            ki.osig = ascii.GetString(Oper.returnportion(Keyvault, 0xC92, 28));
            ki.consoleid = Oper.ByteArrayToString(Oper.returnportion(Keyvault, 0x9CA, 5));
            ki.region = Oper.ByteArrayToString(Oper.returnportion(Keyvault, 0xC8, 2));
            if (Oper.allsame(Oper.returnportion(Keyvault, 0x1df8, 8), 0xFF) || Oper.allsame(Oper.returnportion(Keyvault, 0x1df8, 8), 0x00)) ki.kvtype = "1";
            else ki.kvtype = "2";
            //variables.kvtype = ByteArrayToString(returnportion(Keyvault, 0x9E0, 4));
            ki.mfdate = ascii.GetString(Oper.returnportion(Keyvault, 0x9E4, 8));
            //ki.fcrtflag = (Keyvault[0x1D] & 0xF0) == 0xF0;
            ki.fcrtflag = (BitConverter.ToUInt16(new byte[2] { Keyvault[0x1D], Keyvault[0x1C] }, 0) & 0x120) != 0;
            //Console.WriteLine(Oper.ByteArrayToString(Oper.returnportion(Keyvault, 0x1c, 2)));
        }

        public bool cpukeyverification(string cpukey)
        {
            if (string.IsNullOrWhiteSpace(cpukey)) return false;
            byte[] key = Oper.StringToByteArray(cpukey);
            if (Oper.allsame(Oper.returnportion(_rawkv, 0x40, 0x20), 0x00)) return true;
            if (Oper.allsame(Oper.returnportion(Nand.decryptkv(_rawkv, key), 0x40, 0x20), 0x00))
            {
                if (variables.debugMode) Console.WriteLine("cpukey verified - {0}", cpukey);
                _cpukey = cpukey;
                updatekvval();
                return true;
            }
            else return false;
        }

        public byte[] CalculateSMCHash()
        {
            byte[] SMCen = Nand.encrypt_SMC(_smc);
            ulong s0 = 0;
            ulong s1 = 0;
            for (int i = 0; i < SMCen.Length / 4; i++)
            {
                byte[] tmp = new byte[4];
                Buffer.BlockCopy(SMCen, i * 4, tmp, 0, 4);
                uint tmp2 = BitConverter.ToUInt32(Oper.endianness(tmp), 0);

                s0 += tmp2;
                s1 -= tmp2;
                s0 = (s0 << 29) | ((s0 & 0xFFFFFFF800000000) >> 35); // poor man's rotate left 29
                s1 = (s1 << 31) | ((s1 & 0xFFFFFFFE00000000) >> 33); // poor man's rotate left 31
            }

            byte[] csum = new byte[0x10];
            Buffer.BlockCopy(Oper.StringToByteArray(s0.ToString("X")), 0, csum, 0, 0x8);
            Buffer.BlockCopy(Oper.StringToByteArray(s1.ToString("X")), 0, csum, 8, 0x8);
            return csum;
        }
        public bool checkifhackedSMC()
        {
            //byte[] SMC = Nand.decrypt_SMC(_smc);
            if (Oper.allsame(Oper.returnportion(_smc, 0x2db0, 0x10), 0x00)) { Console.WriteLine("Clean SMC detected"); return false; }
            else { Console.WriteLine("Hacked SMC detected"); return true; }
        }
        public void getsmcconfig()
        {
            _smc_config = null;

            FileStream infile = new FileStream(_filename, FileMode.Open, FileAccess.Read);
            BinaryReader file = new BinaryReader(infile);

            int smc_config_offset, smc_config_length;
            if (!bigblock)
            {
                if (bigflash) smc_config_offset = 0x3FDF800;
                else smc_config_offset = 0xFEB800;
                smc_config_length = 0x4200 * 4;
                _smc_config = new byte[smc_config_length];
            }
            else
            {
                smc_config_offset = 0x3D5C000;
                smc_config_length = 0x21000 * 4;
                _smc_config = new byte[smc_config_length];
            }
            if (noecc)
            {
                smc_config_offset = 0x2ff0000;
                smc_config_length = 0x4000 * 4;
                _smc_config = new byte[smc_config_length];
            }
            file.BaseStream.Seek(smc_config_offset, SeekOrigin.Begin);
            file.Read(_smc_config, 0, smc_config_length);

            file.Close();
            infile.Close();

            if (!noecc)
            {
                _smc_config = Nand.unecc(_smc_config);
            }
        }
        public long kvcrc()
        {
            crc32 crc = new crc32();
            long hashData = crc.CRC(_rawkv);
            return hashData;
        }

        public void getbadblocks()
        {
            if (noecc) return;
            bad_blocks = new List<int>();
            long imgsize = 0;
            byte[] image;
            int blocksize, reservedoffset;
            bool bad_block_in_xell = false;

            if (bigblock)
            {
                image = Oper.openfile(_filename, ref imgsize, 0x4200000);
                blocksize = 0x21000;
                reservedoffset = 0x1E0;
            }
            else
            {
                image = Oper.openfile(_filename, ref imgsize, 0);
                blocksize = 0x4200;
                reservedoffset = 0x3E0;
            }

            if (image[0x205] != 0xFF && image[0x415] != 0xFF && image[0x200] != 0xFF) return;

            if (variables.debugMode) Console.WriteLine("-R-Image Size: 0x{0:X} | imagesize: 0x{1:X}", image.Length, blocksize);

            int counter;
            for (counter = 0; counter < image.Length / blocksize; counter++)
            {
                byte[] block = new byte[blocksize];
                Buffer.BlockCopy(image, counter * blocksize, block, 0, blocksize);
                if (JRunner.Nand.BadBlock.checkifbadblock(block, counter, bigblock, true))
                {
                    bad_blocks.Add(counter);
                    if (counter < 0x50) bad_block_in_xell = true;
                }
                if (bad_blocks.Count >= 0x20)
                {
                    bad_blocks = new List<int>();
                    return;
                }
            }

            if (bad_block_in_xell) MessageBox.Show("Bad block detected in XeLL image area (0x00-0x50)\n\nThis may cause the console to not boot properly\n\nIf this occurs, replace the NAND TSOP chip and if needed, rebuild the image using the Donor Nand Creator", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (bad_blocks.Count == 0)
            {
                return;
            }

            int reserveblockpos;
            if (blocksize == 0x4200) reserveblockpos = 0x3FF;
            else reserveblockpos = 0x1FF;

            int reservestartpos = reserveblockpos - 0x20;
            byte[] reserved = Oper.returnportion(image, reservedoffset * blocksize, 0x20 * blocksize);
            if (variables.debugMode) Oper.savefile(reserved, "reservedarea.bin");
            image = null;

            remapped_blocks = JRunner.Nand.BadBlock.checkifremapped(reserved, bad_blocks, bigblock, true);

            return;
        }
        public byte[] exctractfile(string file)
        {
            //long size = 0;
            //byte[] searched = Oper.openfile(_filename, ref size, 0x4200000);
            //byte[] searched = BadBlock.find_bad_blocks_X(_filename, 0x400);
            byte[] searched = BadBlock.openRemappedImage(_filename, 0x4200000, bad_blocks, remapped_blocks, bigblock, !noecc);
            System.Text.ASCIIEncoding ASCII = new System.Text.ASCIIEncoding();
            byte[] find = ASCII.GetBytes(file);
            int start = 0;
            int found = -1;
            bool matched = false;
            bool corona = false;
            int block_length = 0x4200;
            if (searched[0x205] == 0xFF || searched[0x415] == 0xFF || searched[0x200] == 0xFF) corona = false;
            else corona = true;
            //only look at this if we have a populated search array and search bytes with a sensible start
            if (searched.Length > 0 && find.Length > 0 && start <= (searched.Length - find.Length) && searched.Length >= find.Length)
            {
                //iterate through the array to be searched
                for (int i = start; i <= searched.Length - find.Length; i++)
                {
                    //if the start bytes match we will start comparing all other bytes
                    if (searched[i] == find[0])
                    {
                        if (searched.Length > 1)
                        {
                            //multiple bytes to be searched we have to compare byte by byte
                            matched = true;
                            for (int y = 1; y <= find.Length - 1; y++)
                            {
                                if (searched[i + y] != find[y])
                                {
                                    matched = false;
                                    break;
                                }
                            }
                            //everything matched up
                            if (matched)
                            {
                                found = i;
                                break;
                            }

                        }
                        else
                        {
                            //search byte is only one bit nothing else to do
                            found = i;
                            break; //stop the loop
                        }

                    }
                }

            }
            if (found == -1) { return null; }
            if (corona) block_length = 0x4000;
            int offset = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(searched, found + 0x10, 8)), 16) * block_length;
            int length = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(searched, found + 0x18, 4)), 16);
            if (variables.debugMode) Console.WriteLine("Offset: {0:X} - Length {1:X} - corona: {2}", offset, length, corona);
            if (corona)
            {
                byte[] res = new byte[length];
                Buffer.BlockCopy(searched, offset, res, 0, length);
                searched = res;
            }
            else
            {
                #region unecc
                int counter;
                byte[] res = { };
                for (counter = offset; counter < offset + length + 0x200; counter += 0x210)
                {
                    res = Oper.concatByteArrays(res, Oper.returnportion(searched, counter, 0x200), res.Length, 0x200);
                }
                //res = concatByteArrays(res, returnportion(searched, counter, 496), res.Length, 496);
                searched = res;
                #endregion
            }
            return searched;
        }

        private void getFS1(ref byte[] image)
        {
            byte[] anchor1 = new byte[0x200];
            byte[] anchor2 = new byte[0x200];

            Buffer.BlockCopy(image, 0x2FE8000, anchor1, 0, 0x200);
            Buffer.BlockCopy(image, 0x2FEC000, anchor2, 0, 0x200);

            SHA1 sha1 = SHA1.Create();
            byte[] hash1 = sha1.ComputeHash(Oper.returnportion(anchor1, 0x14, 0x1EC));
            byte[] hash2 = sha1.ComputeHash(Oper.returnportion(anchor2, 0x14, 0x1EC));

            int version = 0;

            if (Oper.ByteArrayCompare(hash1, Oper.returnportion(anchor1, 0, 0x14)))
            {
                version = anchor1[0x1B];
                _currentFS = Oper.ByteArrayToInt(Oper.returnportion(anchor1, 0x1C, 2));
            }
            if (Oper.ByteArrayCompare(hash2, Oper.returnportion(anchor2, 0, 0x14)))
            {
                if (version < anchor2[0x1B]) _currentFS = Oper.ByteArrayToInt(Oper.returnportion(anchor2, 0x1C, 2));
            }
        }

        private void getFS(ref byte[] image)
        {
            if (_currentFS != 0) return;
            byte[] fsSequence = new byte[4];
            byte blocktype;
            int position;

            int blocksize;
            int fullsize;
            int block_type;

            if (variables.debugMode) Console.WriteLine("bigblock: {0}", bigblock);
            if (bigblock)
            {
                blocksize = 0x21000;
                fullsize = 0x200;
                block_type = 0x2C;
            }
            else
            {
                blocksize = 0x4200;
                fullsize = 0x3ff;
                block_type = 0x30;
            }



            int newfilesystem = 0;
            for (int i = 0; i < fullsize; i++)
            {
                for (int j = 0; j < 0x20; j++)
                {
                    position = (blocksize * i) + 0x200 + (j * 0x210);
                    blocktype = image[position + 0xC];
                    int fsseq;
                    if (bigblock)
                    {
                        fsSequence[1] = image[position + 3];
                        fsSequence[2] = image[position + 4];
                        fsSequence[3] = image[position + 5];
                        fsseq = (fsSequence[2] << 8) | (fsSequence[3]);
                    }
                    else
                    {
                        fsSequence[0] = image[position + 0];
                        fsSequence[1] = image[position + 3];
                        fsSequence[2] = image[position + 4];
                        fsSequence[3] = image[position + 6];
                        fsseq = (fsSequence[2] << 16) + (fsSequence[1] << 8) + fsSequence[0];
                    }

                    if (fsseq != 0 && (blocktype & 0x3F) == block_type)
                    {
                        if (variables.debugMode) Console.WriteLine(fsseq);
                        if (fsseq > newfilesystem)
                        {
                            newfilesystem = fsseq;
                            _currentFS = i;
                        }
                        break;
                    }
                }
            }
            return;
        }
        private void getFileList(ref byte[] image)
        {
            int blocksize = 0;
            int pagesize = 0;
            if (noecc)
            {
                blocksize = 0x4000;
                pagesize = 0x200;
            }
            else if (bigblock)
            {
                blocksize = 0x21000;
                pagesize = 0x210;
            }
            else
            {
                blocksize = 0x4200;
                pagesize = 0x210;
            }

            if (Files.Count != 0 || _currentFS == 0) return;
            int startpage = (_currentFS * blocksize) / pagesize;
            List<int> blockMapPages = new List<int>();
            List<int> fileNamePages = new List<int>();
            for (int i = 0; i < 0x20; i++)
                if (i % 2 == 0)
                    blockMapPages.Add(startpage + i);
                else
                    fileNamePages.Add(startpage + i);
            bool breakk = false;

            foreach (int page in fileNamePages)
            {
                if (breakk)
                    break;

                int entrycount = 0x20;
                for (int i = 0; i < entrycount; i += 2)
                {
                    byte[] name = new byte[0x16];
                    byte[] len = new byte[0x4];
                    byte[] blok = new byte[0x2];

                    Buffer.BlockCopy(image, (page * pagesize) + (i * 0x10), name, 0, 0x16);

                    string filename = Encoding.ASCII.GetString(name).Trim('\0');
                    if (string.IsNullOrEmpty(filename))
                    {
                        breakk = true;
                        break;
                    }
                    int length = 0;
                    int block = 0;
                    try
                    {
                        Buffer.BlockCopy(image, (page * pagesize) + (i * 0x10) + 0x18, len, 0, 0x4);
                        Buffer.BlockCopy(image, (page * pagesize) + (i * 0x10) + 0x16, blok, 0, 0x2);

                        length = Oper.ByteArrayToInt(len);
                        block = Oper.ByteArrayToInt(blok);

                        if (bigblock)
                        {
                            byte[] sparedata = new byte[0x10];
                            Buffer.BlockCopy(image, ((page * pagesize) + 0x200), sparedata, 0, 0x10);
                            block = getBlockOffset(block, sparedata);
                        }
                    }
                    catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }

                    if (image[(page * pagesize) + (i * 0x10)] != 0x05) { Files.Add(new FSFile(filename, block, length)); }
                }
            }
        }

        private int getBlockOffset(int blockoffset, byte[] sparedata)
        {
            int smallblocksInBigBlocks = 8;
            int blocksInNand = 0x200;
            int remapReserveSize = 0x20;
            int sizeOfFlashFileSystem = sparedata[8];
            int blocksReservedConfigInfo = sparedata[9];

            int totalSize = blocksInNand * (smallblocksInBigBlocks);
            int endOfConfigArea = totalSize - (remapReserveSize * smallblocksInBigBlocks);
            int endOfFileSystem = endOfConfigArea - (blocksReservedConfigInfo * smallblocksInBigBlocks);
            int startOfFileSystem = endOfFileSystem - (sizeOfFlashFileSystem << 5);


            return ((startOfFileSystem * 0x4200) + (blockoffset * 0x4200));
        }
        public byte[] exctractFSfile(string file)
        {
            //long size = 0;
            byte[] image = BadBlock.openRemappedImage(_filename, 0x4200000, bad_blocks, remapped_blocks, bigblock, !noecc);
            //byte[] image = Oper.openfile(_filename, ref size, 0x4200000);
            if (noecc) getFS1(ref image);
            else getFS(ref image);
            getFileList(ref image);
            FSFile fil = new FSFile();
            foreach (FSFile f in Files)
            {
                if (file == f.getFilename())
                {
                    fil = f;
                    break;
                }
            }
            if (string.IsNullOrEmpty(fil.getFilename())) return null;
            if (variables.debugMode) Console.WriteLine("{0:X} : {1:X}", fil.getBlock(), fil.getLength());
            byte[] searched = new byte[fil.getLength()];

            if (noecc)
            {
                byte[] res = new byte[fil.getLength()];
                Buffer.BlockCopy(image, fil.getBlock() * 0x4000, res, 0, fil.getLength());
                searched = res;
            }
            else
            {

                searched = Nand.unecc(Oper.returnportion(image, bigblock ? fil.getBlock() : fil.getBlock() * 0x4200, (fil.getLength() / 0x200) * 0x210));
                //int counter;
                //byte[] res = { };
                //for (counter = fil.getBlock() * 0x4200; counter < fil.getBlock() * 0x4200 + fil.getLength(); counter += 0x210)
                //{
                //    res = Oper.concatByteArrays(res, Oper.returnportion(image, counter, 0x200), res.Length, 0x200);
                //}
                //searched = res;
            }
            return searched;
        }

    }

    public static class Nand
    {
        /// <summary>
        /// variables
        /// </summary>
        #region variables
        static byte[] secret_1bl = { 0xDD, 0x88, 0xAD, 0x0C, 0x9E, 0xD6, 0x69, 0xE7, 0xB5, 0x67, 0x94, 0xFB, 0x68, 0x56, 0x3E, 0xFA };
        static byte[] random = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        //static string signature = "D6A582FC";
        //static string signature1 = "52A92D9";
        static Encoding enc8 = Encoding.UTF8;
        static Encoding ascii = Encoding.ASCII;
        #endregion


        /// <summary>
        /// keyvault
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="key_s"></param>
        #region keyvault

        public static byte[] getkv(byte[] data)
        {
            byte[] image = Oper.returnportion(ref data, 0, 0x4200 * 3);
            byte[] Keyvault = null;
            if (hasecc(ref image)) unecc(ref image);
            Keyvault = new byte[0x4000];
            if (image.Length > 0x8000) Buffer.BlockCopy(image, 0x4000, Keyvault, 0, 0x4000);
            return Keyvault;
        }


        public static long kvcrc(string filename, bool nobbcheck = false)
        {
            byte[] Keyvault = null;
            byte[] data;
            long size = 0;
            if (nobbcheck) data = Oper.openfile(filename, ref size, 40 * 1024);
            else data = Oper.returnportion(BadBlock.find_bad_blocks_X(filename, 2), 0, 40 * 1024);

            if (data[0] == 0xFF && data[1] == 0x4F)
            {
                int counter;
                if (data[0x205] == 0xFF || data[0x415] == 0xFF || data[0x200] == 0xFF)
                {
                    byte[] res = { };
                    for (counter = 0; counter + 496 < data.Length; counter += 0x210)
                    {
                        res = Oper.concatByteArrays(res, Oper.returnportion(data, counter, 0x200), res.Length, 0x200);
                    }
                    data = res;
                }
                Keyvault = new byte[0x4000];
                Keyvault = Oper.returnportion(data, 0x4000, 0x4000);
                if (variables.extractfiles) Oper.savefile(Keyvault, "output\\KV_en.bin");

                crc32 crc = new crc32();
                long hashData = crc.CRC(Oper.returnportion(Keyvault, 0x0, Keyvault.Length));
                return hashData;
            }
            else
            {
                if (variables.debugMode) Console.WriteLine("* unknown image found !");
                return 0;
            }
        }

        public static byte[] getrawkv(string filename)
        {
            long size = 0;
            byte[] data = BadBlock.find_bad_blocks_X(Oper.openfile(filename, ref size, 0), 5);
            if (variables.debugMode) Console.WriteLine("data: {0:X}", data.Length);
            byte[] Keyraw = new byte[0x4200];
            Keyraw = Oper.returnportion(data, 0x4200, 0x4200);
            if (variables.extractfiles) Oper.savefile(Keyraw, "output\\KV_raw.bin");
            if (variables.debugMode) Console.WriteLine("Keyraw: {0:X}", Keyraw.Length);
            return Keyraw;
        }

        public static bool cpukeyverification(string filename, string key_s, bool fast = false)
        {
            byte[] Keyvault = null;

            long size = 0;
            byte[] data;
            if (fast) data = Oper.openfile(filename, ref size, 0x4200 * 3);
            else data = Oper.returnportion(BadBlock.find_bad_blocks_X(filename, 2), 0, 40 * 1024); //2 * 0x4200);
            byte[] key = Oper.StringToByteArray(key_s);

            if (data[0] == 0xFF && data[1] == 0x4F)
            {
                if (hasecc_v2(ref data)) unecc(ref data);

                Keyvault = new byte[0x4000];
                if (data.Length > 0x8000) Buffer.BlockCopy(data, 0x4000, Keyvault, 0, 0x4000);
                //Keyvault = Oper.returnportion(data, 0x4000, 0x4000);

                if (variables.extractfiles) Oper.savefile(Keyvault, "output\\KV_en.bin");
                Keyvault = decryptkv(Keyvault, key);
                if (Keyvault == null) return false;
                if (variables.extractfiles) Oper.savefile(Keyvault, "output\\KV_dec.bin");
                MainForm.nand.ki.serial = ascii.GetString(Oper.returnportion(Keyvault, 0xB0, 12));
                if (Oper.allsame(Oper.returnportion(Keyvault, 0x40, 0x20), 0x00)) return true;
                else return false;
                /*
                crc32 crc = new crc32();
                //savefile(returnportion(Keyvault, 0x2600, 0x1640), "signature.bin");
                long hashData = crc.CRC(returnportion(Keyvault, 0x2600, 0x1640));
                if (variables.debugme) Console.WriteLine("HashData {0:X}", hashData.ToString());
                if (variables.debugme) Console.WriteLine("Signature {0}", signature);
                if (hashData.ToString("X") == signature || hashData.ToString("X") == signature1) return true;
                else return false;
                */
            }
            else
            {
                // Console.WriteLine("* unknown image found !");
                return false;
            }
        }
        public static bool cpukeyverification(byte[] kv, string key_s)
        {
            byte[] key = Oper.StringToByteArray(key_s);
            if (Oper.allsame(Oper.returnportion(kv, 0x40, 0x20), 0x00)) return true;
            else kv = decryptkv(kv, key);
            if (Oper.allsame(Oper.returnportion(kv, 0x40, 0x20), 0x00)) return true;
            else return false;
            /*
            crc32 crc = new crc32();
            long hashData = crc.CRC(returnportion(kv, 0x2600, 0x1640));
            if (hashData.ToString("X") == signature || hashData.ToString("X") == signature1) return true;
            else return false;
             * */
        }

        public static bool getfcrtflag(byte[] kv, string key_s)
        {
            byte[] key = Oper.StringToByteArray(key_s);
            if (!Oper.allsame(Oper.returnportion(kv, 0x40, 0x20), 0x00)) kv = decryptkv(kv, key);
            //return ((kv[0x1D] & 0xF0) == 0xF0);
            return (BitConverter.ToUInt16(new byte[2] { kv[0x1D], kv[0x1C] }, 0) & 0x120) != 0;
        }

        public static string bruteforce(byte[] kv)
        {
            byte[] key = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                             0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
            byte[] end_key = { 0xFF, 0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
                                 0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF};
            int i = 0;
            byte[] message = new byte[16];
            message = Oper.returnportion(kv, 0, 0x10);
            byte[] restofkv = Oper.returnportion(kv, 0x10, kv.Length - 0x10);
            byte[] ten = new byte[0x10];
            byte[] twenty = new byte[0x20];
            while (key != end_key && !variables.escapeloop)
            {
                if (i % 0x100 == 0) Console.WriteLine("{0:X}", i);
                byte[] RC4_key = Oper.HMAC_SHA1(key, message);
                Buffer.BlockCopy(RC4_key, 0, ten, 0, 0x10);
                Oper.RC4_v(ref restofkv, ten);
                Buffer.BlockCopy(restofkv, 0x30, twenty, 0x0, 0x20);
                if (Oper.allsame(twenty, 0x00))
                {
                    Console.WriteLine(Oper.ByteArrayToString(key));
                    return Oper.ByteArrayToString(key);
                }
                incrementAtIndex(ref key, 31);
                i++;
            }
            Console.WriteLine("{0:X}", i);
            variables.escapeloop = false;
            return null;
        }

        public static void incrementAtIndex(ref byte[] array, int index)
        {
            if (array[index] == byte.MaxValue)
            {
                array[index] = 0;
                if (index > 0)
                    incrementAtIndex(ref array, index - 1);
            }
            else
            {
                array[index]++;
            }
        }

        // can be used for secdata as well
        public static byte[] decryptkv(byte[] kv, byte[] key)
        {
            try
            {
                if (kv == null || key == null) return null;
                byte[] message = new byte[16];
                message = Oper.returnportion(kv, 0, 0x10);
                byte[] RC4_key = Oper.HMAC_SHA1(key, message);
                if (RC4_key == null) return null;
                byte[] restofkv = Oper.returnportion(kv, 0x10, kv.Length - 0x10);
                Oper.RC4_v(ref restofkv, Oper.returnportion(RC4_key, 0, 0x10));
                byte[] finalimage = new byte[message.Length + restofkv.Length];
                for (int i = 0; i < message.Length + restofkv.Length; i++)
                {
                    if (i < message.Length) finalimage[i] = message[i];
                    else finalimage[i] = restofkv[i - message.Length];
                }
                return finalimage;
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
            return null;
        }

        public static byte[] encryptkv(byte[] kv, byte[] key)
        {
            if (variables.debugMode) Console.WriteLine(key.Length);
            byte[] message = new byte[16];
            message = Oper.returnportion(kv, 0, 0x10);
            byte[] RC4_key = Oper.HMAC_SHA1(key, message);
            byte[] restofkv = Oper.returnportion(kv, 0x10, kv.Length - 0x10);
            Oper.RC4_v(ref restofkv, Oper.returnportion(RC4_key, 0, 0x10));
            byte[] finalimage = new byte[message.Length + restofkv.Length];
            for (int i = 0; i < message.Length + restofkv.Length; i++)
            {
                if (i < message.Length) finalimage[i] = message[i];
                else finalimage[i] = restofkv[i - message.Length];
            }

            return finalimage;
        }

        public static byte[] encryptkv_hmac(byte[] kv, byte[] key)
        {
            byte[] secret = { 0x07, 0x12 };
            byte[] message = new byte[16];
            message = Oper.returnportion(kv, 0x10, kv.Length - 0x10);
            message = Oper.addtoflash_v2(message, secret);
            byte[] RC4_key = Oper.HMAC_SHA1(key, message);
            byte[] restofkv = Oper.returnportion(kv, 0x10, kv.Length - 0x10);
            //byte[] restofkv = kv;
            byte[] RC4_k = Oper.HMAC_SHA1(key, Oper.returnportion(RC4_key, 0, 0x10));
            Oper.RC4_v(ref restofkv, Oper.returnportion(RC4_k, 0, 0x10));
            byte[] finalimage = new byte[0x4000];
            for (int i = 0; i < 0x4000; i++)
            {
                if (i < 0x10) finalimage[i] = RC4_key[i];
                else finalimage[i] = restofkv[i - 0x10];
            }

            return finalimage;
        }

        public static void injectRawKV(string filename, byte[] rawKV)
        {
            if (filename == null) return;
            if (!File.Exists(filename)) return;
            FileInfo f = new FileInfo(filename);

            FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            BinaryWriter fileb = new BinaryWriter(infile);

            if (variables.debugMode) Console.WriteLine("kv length: {0:X}", rawKV.Length);

            fileb.BaseStream.Seek(rawKV.Length, SeekOrigin.Begin);
            fileb.Write(rawKV);

            infile.Close();
            return;
        }

        public static void injectEncryptedKV(string flashFilePath, string kvFilePath, byte[] cpukey)
        {
            byte[] flashFileData = File.ReadAllBytes(flashFilePath);
            bool flashHasEcc = false;
            int blockType = 0;

            byte[] kvFileData = File.ReadAllBytes(kvFilePath);

            // KV size and KV offset are stored in the first page of NAND
            int flashKvSize = BitConverter.ToInt32(flashFileData.Skip(0x60).Take(0x4).Reverse().ToArray(),0);
            int flashKvOffset = BitConverter.ToInt32(flashFileData.Skip(0x6c).Take(0x4).Reverse().ToArray(),0);

            if ( kvFileData.Length % 0x200 != 0 )
            {
                Console.WriteLine("Error: KV size is not a multiple of 0x200, decrypted KV might be corrupt.");
                return;
            }

            if( flashKvOffset % 0x200 != 0 )
            {
                Console.WriteLine("Error: KV is not stored on a page boundary. NAND image may be corrupt.");
                return;
            }

            if (flashKvSize == 0)
            {
                Console.WriteLine("Warning: KV size set to 0 in this flash image. KV size will not be validated.");
            }
            else if (kvFileData.Length != flashKvSize)
            {
                Console.WriteLine("Error: can't inject a different length KV in to an existing NAND");
                return;
            }

            // Determine whether this image has ECC
            if (flashFileData.Length == 17301504 || flashFileData.Length == 69206016)
            {
                flashHasEcc = true;
            }
            else if (flashFileData.Length == 50331648)
            {
                flashHasEcc = false;
            }
            else
            {
                Console.WriteLine("Couldn't inject KV: Invalid flash image size");
                return;
            }

            // Encrypt the KV with the CPU key
            byte[] kvenc = encryptkv_hmac(kvFileData, cpukey);

            if (flashHasEcc)
            {
                // If the flash has ECC data, determine the block type so ECC data can be recalculated
                byte[] sparedata = flashFileData.Skip(0x4400).Take(0x10).ToArray();

                // Block Types
                // 0 = Small block NAND (XSB)
                // 1 = Small block NAND on BB controller (PSB/KSB)
                // 2 = Big block NAND on BB controller (PSB/KSB)
                blockType = identifylayout(sparedata);

                int flashKvOffsetPhys = (flashKvOffset / 0x200) * 0x210;

                kvenc = addecc_v2(kvenc, true, flashKvOffsetPhys, blockType);

                Buffer.BlockCopy(kvenc, 0, flashFileData, flashKvOffsetPhys, kvenc.Length);
            }
            else
            {
                Buffer.BlockCopy(kvenc, 0, flashFileData, flashKvOffset, flashKvSize);
            }

            File.WriteAllBytes(flashFilePath, flashFileData);

            Console.WriteLine("Success!");
            return;
        }

        #endregion


        /// <summary>
        /// getcb_build - encrypt - decrypt
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        #region bootloaders

        public static int getcb_build(byte[] image)
        {
            if (variables.extractfiles) Oper.savefile(image, "conf.bin");
            if (variables.debugMode) Console.WriteLine("Getting CB");
            int counter;
            if (image[0x205] == 0xFF || image[0x415] == 0xFF || image[0x200] == 0xFF)
            {
                byte[] res = { };
                for (counter = 0; counter < image.Length; counter += 0x210)
                {
                    res = Oper.concatByteArrays(res, Oper.returnportion(image, counter, 0x200), res.Length, 0x200);
                }
                image = res;
            }
            if (variables.debugMode) Console.WriteLine("Unecc'd Conf");
            byte block_id;
            int block_build = 0;
            byte[] block_build_b = new byte[2], block_size_b = new byte[4], SMC;
            int block_offset_b = (Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(image, 0x8, 4)), 16));
            block_id = image[block_offset_b + 1];
            block_build_b = Oper.returnportion(image, block_offset_b + 2, 2);
            int id = block_id & 0xF;
            if (id == 2) block_build = Convert.ToInt32(Oper.ByteArrayToString(block_build_b), 16);
            if (variables.debugMode) Console.WriteLine("Block Build: {0}", block_build);
            if (variables.debugMode) Console.WriteLine("Checking SMC");
            byte[] smc_len = new byte[4], smc_start = new byte[4];
            smc_len = Oper.returnportion(image, 0x78, 4);
            smc_start = Oper.returnportion(image, 0x7C, 4);
            SMC = new byte[Convert.ToInt32(Oper.ByteArrayToString(smc_len), 16)];
            SMC = Oper.returnportion(image, Convert.ToInt32(Oper.ByteArrayToString(smc_start), 16), Convert.ToInt32(Oper.ByteArrayToString(smc_len), 16));
            SMC = decrypt_SMC(SMC);
            variables.smcmbtype = SMC[0x100] >> 4 & 15;
            if (variables.debugMode) Console.WriteLine("SMC Type: {0}", variables.smcmbtype);
            SMC = null;
            return block_build;
        }

        public static void getCF(byte[] image, bool bigblock, out byte[] CF0, out int CF0offset, out byte[] CF1, out int CF1offset)
        {
            CF0 = null;
            CF1 = null;
            CF0offset = 0;
            CF1offset = 0;
            if (Oper.allsame(Oper.returnportion(image, 0x202, 3), 0x00))
            {
                int counter = 0;
                try
                {
                    if (image[0x205] == 0xFF || image[0x415] == 0xFF || image[0x200] == 0xFF)
                    {
                        byte[] res = new byte[(image.Length / 0x210) * 0x200];
                        for (counter = 0; counter < res.Length; counter += 0x200)
                        {
                            if (((counter / 0x200) * 0x210) + 0x200 <= image.Length) Buffer.BlockCopy(image, (counter / 0x200) * 0x210, res, counter, 0x200);
                        }
                        image = res;
                        res = null;
                    }
                }
                catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }

                try
                {
                    int size = image.Length;
                    int blocksize, patch_offset = 0;
                    if (bigblock) blocksize = 0x20000;
                    else blocksize = 0x4000;
                    int block = 0, block_size, id;
                    byte block_id;
                    int block_build;
                    byte[] block_build_b = new byte[2], block_size_b = new byte[4];
                    int block_offset_b = 0;
                    int patch = 0;
                    try
                    {
                        for (block = 0; block < 10; block++)
                        {
                            block_id = image[block_offset_b + 1];
                            if (variables.debugMode) Console.WriteLine("Block ID: {0} | Block offset: {1:X}", block_id, block_offset_b);
                            int temp_block_offset = block_offset_b;
                            //block_build_b = returnportion(image, block_offset_b + 2, 2);
                            Buffer.BlockCopy(image, block_offset_b + 2, block_build_b, 0, 2);
                            //block_size_b = returnportion(image, block_offset_b + 12, 4);
                            Buffer.BlockCopy(image, block_offset_b + 12, block_size_b, 0, 4);
                            block_size = Convert.ToInt32(Oper.ByteArrayToString(block_size_b), 16);
                            block_build = Convert.ToInt32(Oper.ByteArrayToString(block_build_b), 16);
                            if (variables.debugMode) Console.WriteLine("Block Build {0} : Block Size {1:X}", block_build, block_size);
                            block_size += 0xF;
                            block_size &= ~0xF;
                            id = block_id & 0xF;
                            byte[] data = new byte[block_size];
                            //byte[] data = returnportion(image, block_offset_b, block_size);
                            if (block_size + block_offset_b <= image.Length) Buffer.BlockCopy(image, block_offset_b, data, 0, block_size);

                            if (id == 6 || id == 7)
                            {
                                if (variables.debugMode) Console.WriteLine("Found {0}BL Patch {3} (build {1}) at {2:X}", id, block_build, block_offset_b, patch);
                                if (id == 6)
                                {
                                    patch_offset = block_offset_b;
                                    if (patch == 0)
                                    {
                                        CF0 = data;
                                        CF0offset = block_offset_b;
                                    }
                                    else
                                    {
                                        CF1 = data;
                                        CF1offset = block_offset_b;
                                    }

                                    if (variables.extractfiles)
                                    {
                                        Oper.savefile(data, "CF" + patch + ".bin");
                                        Oper.savefile(decrypt_CF(data), "CF" + patch + "_dec.bin");
                                    }
                                }
                                else if (id == 7)
                                {
                                    if (variables.extractfiles) Oper.savefile(data, "CG" + patch + ".bin");

                                    if (patch == 0)
                                    {
                                        MainForm.nand.bl.CG_0 = block_build;
                                        //block_offset_b += 0xBBB0;
                                        patch = 1;
                                    }
                                    else
                                    {
                                        MainForm.nand.bl.CG_1 = block_build;
                                        break;
                                    }
                                }
                            }
                            int tem0 = image[patch_offset + blocksize];
                            int temo = image[patch_offset + blocksize + 1];
                            int tem2 = image[patch_offset + 0x10001];
                            int tem1 = image[patch_offset + 0x10000];
                            int tem3 = image[block_offset_b + 0x10000];
                            int tem4 = image[block_offset_b + 0x10001];
                            int tem5 = image[block_offset_b + block_size];
                            int tem6 = image[block_offset_b + block_size + 1];
                            if (patch == 1 && block_offset_b < 0x80000 && tem2 == 0x46 && tem1 == 0x43)
                            {
                                if (variables.debugMode) Console.WriteLine("2 - {0:X}", block_offset_b);
                                block_offset_b = patch_offset + 0x10000;
                                continue;
                            }
                            else if (temo == 0x46 && tem0 == 0x43 && patch == 1)
                            {
                                if (variables.debugMode) Console.WriteLine("1 - {0:X}", block_offset_b);
                                block_offset_b = patch_offset + blocksize;
                                continue;
                            }
                            else if (patch == 0 && tem3 == 0x43 && tem4 == 0x46 && tem5 != 0x43 && tem6 != 0x47)
                            {
                                if (variables.debugMode) Console.WriteLine("4 - {0:X}", block_offset_b);
                                block_offset_b += 0x10000;
                                patch = 1;
                                continue;
                            }

                            else if (patch == 0 && block_offset_b > 0x80000 && patch_offset < 0x80000)
                            {
                                if (variables.debugMode) Console.WriteLine("3 - {0:X}", block_offset_b);
                                patch = 1;
                                block_offset_b = 0x80000;
                                continue;
                            }
                            if (block_size == 0x10) { block_size = 0x20000; patch = 1; }
                            block_offset_b += block_size;
                            if (variables.debugMode) Console.WriteLine("5 - {0:X}", block_offset_b);
                            if (temp_block_offset == block_offset_b) break;
                            if (block_offset_b > size) break;
                        }
                    }
                    catch (System.OverflowException) { return; }
                    catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
                }
                catch (System.IndexOutOfRangeException ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
                catch (System.OutOfMemoryException ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }
            }
        }

        public static byte[] decrypt_CB_internal(byte[] image, byte[] cpu1blKey)
        {

            byte[] message = Oper.returnportion(image, 0x10, 0x10);
            byte[] RC4_key = Oper.HMAC_SHA1(cpu1blKey, message);
            byte[] imfordec = Oper.returnportion(image, 0x20, image.Length - 0x20);
            Oper.RC4_v(ref imfordec, Oper.returnportion(RC4_key, 0, 0x10));
            byte[] finalimage = new byte[image.Length];
            for (int i = 0; i < image.Length; i++)
            {
                if (i < 0x10) finalimage[i] = image[i];
                else if (i < 0x20) finalimage[i] = RC4_key[i - 0x10];
                else finalimage[i] = imfordec[i - 0x20];
            }

            return finalimage;
        }

        // CB, SB, effectively any 2BL that runs on DD2+ uses the same crypto 
        public static byte[] decrypt_CB(byte[] image)
        {
            if (variables.debugMode) Console.WriteLine(" * decrypting CB...");
            return decrypt_CB_internal(image, secret_1bl);
        }

        // "S2" is the 2BL that runs on DD1. Crypto is the same, except the 1BL
        // key is all zeros
        public static byte[] decrypt_S2(byte[] image)
        {
            if (variables.debugMode) Console.WriteLine(" * decrypting S2...");
            return decrypt_CB_internal(image, keyZero);
        }

        public static byte[] decrypt_CB_cpukey(byte[] CB_B, byte[] CB_A, byte[] cpukey)
        {
            byte[] secret = Oper.returnportion(CB_A, 0x10, 0x10);
            byte[] temp = Oper.returnportion(CB_B, 0x10, 0x10);
            byte[] message = { };

            if (CB_A[0x7] != 0)
            {
                // The MFG flag is set. Decrypt the CB_B with the zero key instead
                // of the CPU key that was passed in
                message = Oper.concatByteArrays(temp, keyZero, 0x10, 0x10);
            }
            else
            {
                message = Oper.concatByteArrays(temp, cpukey, 0x10, 0x10);
            }

            if ((Oper.ByteArrayToInt(Oper.returnportion(CB_A, 0x6, 2)) & 0x1000) != 0)
            {
                if (variables.debugMode) Console.WriteLine("CB - Using new encryption scheme");
                temp = Oper.returnportion(CB_A, 0, 0x10);
                temp[0x6] = 0x00;
                temp[0x7] = 0x00;
                message = Oper.concatByteArrays(message, temp, message.Length, 0x10);
            }

            byte[] RC4_key = Oper.HMAC_SHA1(secret, message);
            byte[] imfordec = Oper.returnportion(CB_B, 0x20, CB_B.Length - 0x20);
            Oper.RC4_v(ref imfordec, Oper.returnportion(RC4_key, 0, 0x10));


            byte[] finalimage = new byte[CB_B.Length];
            for (int i = 0; i < CB_B.Length; i++)
            {
                if (i < 0x10) finalimage[i] = CB_B[i];
                else if (i < 0x20) finalimage[i] = RC4_key[i - 0x10];
                else finalimage[i] = imfordec[i - 0x20];
            }
            return finalimage;
        }

        public static byte[] decrypt_CD(byte[] CD, byte[] CB_B)
        {
            return decrypt_CD_cpukey(CD, CB_B, null);
        }

        public static byte[] decrypt_CD_cpukey(byte[] CD, byte[] CB_B, byte[] cpukey)
        {
            byte[] secret = Oper.returnportion(CB_B, 0x10, 0x10);
            byte[] message = Oper.returnportion(CD, 0x10, 0x10);

            byte[] RC4_key = Oper.returnportion(Oper.HMAC_SHA1(secret, message), 0, 0x10);

            byte[] imfordec = Oper.returnportion(CD, 0x20, CD.Length - 0x20);
            Oper.RC4_v(ref imfordec, RC4_key);

            // Check to see if we need decrypted the CD properly. If not, this is probably a
            // non-zeropaired single cb image that needs a CPU key to decrypt the CD. Borrowed
            // this check from the RGBuild image editor
            if (!( (imfordec[0x20] == 0 && imfordec[0x21] == 0 && imfordec[0x22] == 0 && imfordec[0x23] == 0) ||
                   (imfordec[0x210] == 0 && imfordec[0x211] == 0 && imfordec[0x212] == 0 && imfordec[0x213] == 0) ))
            {
                // We didn't decrypt the CD properly. Oops! We've got to try again with the CPU key if available
                if (cpukey != null)
                {
                    if (variables.debugMode) Console.WriteLine("CD wasn't decrypted properly, trying again with the CPU key");

                    // The nonce/message is the previously calculated RC4 key, the secret is the CPU key
                    secret = cpukey;
                    message = RC4_key;

                    // Regenerate the decryption key
                    RC4_key = Oper.returnportion(Oper.HMAC_SHA1(secret, message), 0, 0x10);

                    // Let's try this again... hopefully it works this time
                    imfordec = Oper.returnportion(CD, 0x20, CD.Length - 0x20);
                    Oper.RC4_v(ref imfordec, RC4_key);
                }
            }

            byte[] finalimage = new byte[CD.Length];

            for (int i = 0; i < CD.Length; i++)
            {
                if (i < 0x10) finalimage[i] = CD[i];
                else if (i < 0x20) finalimage[i] = RC4_key[i - 0x10];
                else finalimage[i] = imfordec[i - 0x20];
            }
            return finalimage;
        }

        public static byte[] decrypt_CE(byte[] CE, byte[] CD)
        {
            // CE is encrypted the exact same way the CD is
            return decrypt_CD(CE, CD);
        }

        public static byte[] decrypt_SC(byte[] SC)
        {
            // This is decrypted the same way as CD, but with a zero key
            // as input. So we don't have to reinvent the wheel, pass
            // a blank byte array in to decrypt_CD
            byte[] ZERO_KEY_SC = new byte[0x20];

            return decrypt_CD(SC, ZERO_KEY_SC);
        }

        public static byte[] decrypt_SD(byte[] SD, byte[] SC)
        {
            // SD is decrypted the same way as CD, but with SC as input
            return decrypt_CD(SD, SC);
        }

        public static byte[] getCbbRc4Key(byte[] CB_A_key, bool CB_A_new_crypto, byte[] CB_B_nonce, byte[] cpukey)
        {
            byte[] secret = CB_A_key;
            byte[] message = Oper.concatByteArrays(CB_B_nonce, cpukey, 0x10, 0x10);
            
            // New CB_A versions use a different method to generate the CB_B RC4 key, as seen below.
            // CB_A flags. WORD at 0x6 in the CB_A binary will have bit 0x1000 set if this is the case.
            if (CB_A_new_crypto)
            {
                Console.WriteLine("Using new encryption scheme to generate CB_B RC4 key");
                CB_A_key[0x6] = 0x00;
                CB_A_key[0x7] = 0x00;
                message = Oper.concatByteArrays(message, CB_A_key, message.Length, 0x10);
            }

            return Oper.HMAC_SHA1(secret, message);
        }

        public static byte[] encrypt_CB_cpukey(byte[] image, byte[] CB_A_key, bool CB_A_new_crypto, byte[] cpukey)
        {
            if (variables.debugMode) Console.WriteLine(cpukey.Length);

            byte[] cbb_nonce = Oper.returnportion(image, 0x10, 0x10);

            byte[] RC4_key = getCbbRc4Key(CB_A_key, CB_A_new_crypto, cbb_nonce, cpukey);
            byte[] imfordec = Oper.returnportion(image, 0x20, image.Length - 0x20);
            if (variables.debugMode) Console.WriteLine(Oper.ByteArrayToString(RC4_key));
            Oper.RC4_v(ref imfordec, Oper.returnportion(RC4_key, 0, 0x10));


            byte[] finalimage = new byte[image.Length];
            for (int i = 0; i < image.Length; i++)
            {
                if (i < 0x10) finalimage[i] = image[i];
                else if (i < 0x20) finalimage[i] = cbb_nonce[i - 0x10];
                else finalimage[i] = imfordec[i - 0x20];
            }
            return finalimage;
        }

        public static byte[] encrypt_CB(byte[] image, byte[] random, ref byte[] key)
        {
            // Dummy variable so encrypt_CB_A is happy
            bool CB_A_new_crypto = false;

            return encrypt_CB_A(image, random, ref key, ref CB_A_new_crypto);
        }

        public static byte[] encrypt_CB_A(byte[] image, byte[] random, ref byte[] key, ref bool CB_A_new_crypto)
        {
            byte[] finalimage = new byte[image.Length];
            try
            {
                // Split CB_A that have the bit 0x1000 set in their flags structure use
                // a different method for generating the CB_B encryption key.
                if (0 != (BitConverter.ToInt16(image.Skip(0x6).Take(0x2).Reverse().ToArray(), 0) & 0x1000))
                {
                    if (variables.debugMode) Console.WriteLine("CB_A uses new encryption scheme...");

                    CB_A_new_crypto = true;
                }
                else
                {
                    CB_A_new_crypto = false;
                }

                if (variables.debugMode) Console.WriteLine("Encrypting CB...");
                byte[] RC4_key = Oper.HMAC_SHA1(secret_1bl, random);
                //byte[] RC4_key = returnportion(image, 0x10, 0x10);
                byte[] imfordec = Oper.returnportion(image, 0x20, image.Length - 0x20);
                if (variables.debugMode) Console.WriteLine(" CB Stage 1");
                key = Oper.returnportion(RC4_key, 0, 0x10);
                Oper.RC4_v(ref imfordec, Oper.returnportion(RC4_key, 0, 0x10));
                if (variables.debugMode) Console.WriteLine(" CB Stage 2");

                for (int i = 0; i < image.Length; i++)
                {
                    if (i < 0x10) finalimage[i] = image[i];
                    else if (i < 0x20) finalimage[i] = random[i - 0x10];
                    else finalimage[i] = imfordec[i - 0x20];
                }
                if (variables.debugMode) Console.WriteLine("Encrypted CB...");

            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
            return finalimage;
        }

        public static byte[] encrypt_CD(byte[] image, byte[] random, byte[] CB_B_key)
        {
            if (variables.debugMode) Console.WriteLine("Encrypting CD...");
            byte[] RC4_key = Oper.HMAC_SHA1(CB_B_key, random);
            byte[] imfordec = Oper.returnportion(image, 0x20, image.Length - 0x20);
            Oper.RC4_v(ref imfordec, Oper.returnportion(RC4_key, 0, 0x10));
            byte[] finalimage = new byte[image.Length];
            for (int i = 0; i < image.Length; i++)
            {
                if (i < 0x10) finalimage[i] = image[i];
                else if (i < 0x20) finalimage[i] = random[i - 0x10];
                else finalimage[i] = imfordec[i - 0x20];
            }
            if (variables.debugMode) Console.WriteLine("Encrypted CD...");
            return finalimage;
        }

        public static byte[] decrypt_CF(byte[] image)
        {
            if (variables.debugMode) Console.WriteLine("Decrypting CF...");
            byte[] message = Oper.returnportion(image, 0x20, 0x10);
            byte[] RC4_key = Oper.HMAC_SHA1(secret_1bl, message);
            byte[] imfordec = Oper.returnportion(image, 0x30, image.Length - 0x30);
            Oper.RC4_v(ref imfordec, Oper.returnportion(RC4_key, 0, 0x10));
            byte[] finalimage = new byte[image.Length];
            for (int i = 0; i < image.Length; i++)
            {
                if (i < 0x20) finalimage[i] = image[i];
                else if (i < 0x30) finalimage[i] = RC4_key[i - 0x20];
                else finalimage[i] = imfordec[i - 0x30];
            }

            return finalimage;
        }

        public static byte[] decrypt_CG(byte[] image, byte[] CF)
        {
            if (variables.debugMode) Console.WriteLine("Decrypting CG...");
            byte[] secret = Oper.returnportion(CF, 0x330, 0x10);
            byte[] message = Oper.returnportion(image, 0x10, 0x10);
            byte[] RC4_key = Oper.HMAC_SHA1(secret, message);
            byte[] imfordec = Oper.returnportion(image, 0x20, image.Length - 0x20);
            Oper.RC4_v(ref imfordec, Oper.returnportion(RC4_key, 0, 0x10));
            byte[] finalimage = new byte[image.Length];
            for (int i = 0; i < image.Length; i++)
            {
                if (i < 0x10) finalimage[i] = image[i];
                else if (i < 0x20) finalimage[i] = RC4_key[i - 0x10];
                else finalimage[i] = imfordec[i - 0x20];
            }

            return finalimage;
        }

        public static byte[] encrypt_CF(byte[] CF_dec, byte[] encryptedCF, byte[] cpukey)
        {
            if (variables.debugMode) Console.WriteLine("Encrypting...");
            byte[] message = random;
            //byte[] RC4_key = HMAC_SHA1(secret_1bl, message);
            byte[] RC4_key = Oper.returnportion(CF_dec, 0x20, 0x10);
            byte[] imfordec = Oper.returnportion(CF_dec, 0x30, CF_dec.Length - 0x30);
            Oper.RC4_v(ref imfordec, Oper.returnportion(RC4_key, 0, 0x10));
            byte[] hash = calcCFhash(CF_dec, cpukey);
            RC4_key = Oper.HMAC_SHA1(secret_1bl, Oper.returnportion(CF_dec, 0x20, 0x10));
            if (variables.debugMode) Console.WriteLine(Oper.ByteArrayToString(RC4_key));
            byte[] finalimage = new byte[CF_dec.Length];
            for (int i = 0; i < CF_dec.Length; i++)
            {
                if (i < 0x20) finalimage[i] = CF_dec[i];
                else if (i < 0x30) finalimage[i] = encryptedCF[i];
                else finalimage[i] = imfordec[i - 0x30];
            }
            Buffer.BlockCopy(hash, 0x0, finalimage, 0x220, 0x10);
            return finalimage;
        }

        private static byte[] calcCFhash(byte[] CF, byte[] cpukey)
        {
            byte[] secret = secret_1bl;
            byte[] key = Oper.HMAC_SHA1(secret, Oper.returnportion(CF, 0x20, 0x10));
            Array.Resize(ref key, 0x10);
            byte[] imfordec = CF;
            Buffer.BlockCopy(key, 0, imfordec, 0x20, 0x10);

            byte[] hash = Oper.HMAC_SHA1(cpukey, Oper.returnportion(imfordec, 0x0, 0x220));
            Array.Resize(ref hash, 0x10);

            return hash;
        }

        #endregion

        #region smc_config

        public static byte[] calcConfigSum(byte[] data)
        {
            int i, len, sum = 0;
            for (i = 0x10, len = 252 + 0x10; i < len; i++)
                sum += data[i] & 0xFF;
            sum = (~sum) & 0xFFFF;
            return new byte[] { (byte)((sum & 0xFF00) >> 8), (byte)((sum & 0xFF)) };
        }

        public static SMCConfig getConfigValues(byte[] data, int blockoffset)
        {
            SMCConfig val = new SMCConfig(false);
            if (data != null)
            {
                val.checksum = Oper.returnportion(data, 0 + blockoffset, 2);
                val.structure = Oper.returnportion(data, 0xE + blockoffset, 1);
                val.config = Oper.returnportion(data, 0xF + blockoffset, 1);
                val.bit = Oper.returnportion(data, 0x14 + blockoffset, 1);
                val.mac = Oper.returnportion(data, 0x220 + blockoffset, 6);
                val.cpugain = Oper.returnportion(data, 0x18 + blockoffset, 2);
                val.cpuoff = Oper.returnportion(data, 0x1A + blockoffset, 2);
                val.gpugain = Oper.returnportion(data, 0x1C + blockoffset, 2);
                val.gpuoff = Oper.returnportion(data, 0x1E + blockoffset, 2);
                val.dramgain = Oper.returnportion(data, 0x20 + blockoffset, 2);
                val.dramoff = Oper.returnportion(data, 0x22 + blockoffset, 2);
                val.boardgain = Oper.returnportion(data, 0x24 + blockoffset, 2);
                val.boardoff = Oper.returnportion(data, 0x26 + blockoffset, 2);
                val.ana = Oper.returnportion(data, 0x28 + blockoffset, 1);
                val.anabackup = Oper.returnportion(data, 0x102 + blockoffset, 1);
                val.clock = Oper.returnportion(data, 0x10 + blockoffset, 1);
                val.flags = Oper.returnportion(data, 0x23C + blockoffset, 4);
                val.version = Oper.returnportion(data, 0x204 + blockoffset, 4);
                val.net = Oper.returnportion(data, 0x208 + blockoffset, 4);

                val.reset = Oper.returnportion(data, 0x238 + blockoffset, 4);
                val.thermal = Oper.returnportion(data, 0x29 + blockoffset, 6);
                val.gainoff = Oper.returnportion(data, 0xF2 + blockoffset, 16);

                val.dvdregion = Oper.returnportion(data, 0x237 + blockoffset, 1);
                val.gameregion = Oper.returnportion(data, 0x22C + blockoffset, 1);
                val.videoregion = Oper.returnportion(data, 0x22D + blockoffset, 1);

                val.pwrmode = Oper.returnportion(data, 0x240 + blockoffset, 2);
                val.powervcs = Oper.returnportion(data, 0x242 + blockoffset, 2);

                val.cpufanspeed = Oper.returnportion(data, 0x11 + blockoffset, 1);
                val.gpufanspeed = Oper.returnportion(data, 0x12 + blockoffset, 1);

                val.reserve0 = Oper.returnportion(data, 0x22E + blockoffset, 6);
                val.reserve1 = Oper.returnportion(data, 0x226 + blockoffset, 2);
                val.reserve2 = Oper.returnportion(data, 0x214 + blockoffset, 12);
                val.reserve3 = Oper.returnportion(data, 0x4 + blockoffset, 4);
                val.reserve4 = Oper.returnportion(data, 0x8 + blockoffset, 5);
                val.reserve5 = Oper.returnportion(data, 0x20C + blockoffset, 8);
                val.ok = true;
            }
            return val;
        }

        public static byte[] editConfigValues(string filename, SMCConfig val)
        {
            int block_offset = 0;
            byte[] data = getsmcconfig(filename, out block_offset);
            if (data == null) return null;
            if (variables.debugMode) Console.WriteLine("{0:X} - {1:X}", data.Length, block_offset);
            data.Replace(val.structure, block_offset + 0xE, 1);
            data.Replace(val.config, block_offset + 0xF, 1);
            data.Replace(val.bit, block_offset + 0x14, 1);
            data.Replace(val.mac, block_offset + 0x220, 6);
            data.Replace(val.cpugain, block_offset + 0x18, 2);
            data.Replace(val.cpuoff, block_offset + 0x1A, 2);
            data.Replace(val.gpugain, block_offset + 0x1C, 2);
            data.Replace(val.gpuoff, block_offset + 0x1E, 2);
            data.Replace(val.dramgain, block_offset + 0x20, 2);
            data.Replace(val.dramoff, block_offset + 0x22, 2);
            data.Replace(val.boardgain, block_offset + 0x24, 2);
            data.Replace(val.boardoff, block_offset + 0x26, 2);
            data.Replace(val.ana, block_offset + 0x28, 1);
            data.Replace(val.anabackup, block_offset + 0x102, 1);
            data.Replace(val.clock, block_offset + 0x10, 1);
            data.Replace(val.flags, block_offset + 0x23C, 4);
            data.Replace(val.version, block_offset + 0x204, 4);
            data.Replace(val.net, block_offset + 0x208, 4);

            data.Replace(val.reset, block_offset + 0x238, 4);
            data.Replace(val.thermal, block_offset + 0x29, 6);
            data.Replace(val.gainoff, block_offset + 0xF2, 16);

            data.Replace(val.dvdregion, block_offset + 0x237, 1);
            data.Replace(val.gameregion, block_offset + 0x22C, 1);
            data.Replace(val.videoregion, block_offset + 0x22D, 1);

            data.Replace(val.pwrmode, block_offset + 0x240, 2);
            data.Replace(val.powervcs, block_offset + 0x242, 2);

            data.Replace(val.cpufanspeed, block_offset + 0x11, 1);
            data.Replace(val.gpufanspeed, block_offset + 0x12, 1);

            data.Replace(val.reserve0, block_offset + 0x22E, 6);
            data.Replace(val.reserve1, block_offset + 0x226, 2);
            data.Replace(val.reserve2, block_offset + 0x214, 12);
            data.Replace(val.reserve3, block_offset + 0x4, 4);
            data.Replace(val.reserve4, block_offset + 0x8, 5);
            data.Replace(val.reserve5, block_offset + 0x20C, 8);

            byte[] sum = calcConfigSum(Oper.returnportion(data, block_offset, 0x200));
            Array.Reverse(sum);
            data.Replace(sum, block_offset, 2);
            return data;
        }

        public static void injectSMC(string filename, byte[] SMCdec)
        {
            bool bigblock, corona4g, bigflash;
            int layout;
            if (filename == null) return;
            if (!File.Exists(filename)) return;
            FileInfo f = new FileInfo(filename);
            long s1 = f.Length;
            long imgsize = 0;
            byte[] image = Oper.openfile(filename, ref imgsize, 0x4200000);
            if (s1 >= 0x4200000)
            {
                bigflash = true;
                if (image[0x205] == 0xFF) bigblock = false;
                else bigblock = true;
            }
            else bigblock = bigflash = false;
            corona4g = true;

            FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader file = new BinaryReader(infile);
            BinaryWriter fileb = new BinaryWriter(infile);

            byte[] temp = new byte[0x420];
            byte[] sparedata = new byte[0x10];
            file.Read(temp, 0, 0x420);

            file.BaseStream.Seek(0x4400, SeekOrigin.Begin);
            file.Read(sparedata, 0, 0x10);
            //file.Close();
            layout = identifylayout(sparedata);

            if (hasecc(temp)) corona4g = false;
            if (variables.debugMode) Console.WriteLine("bigblock: {0} - corona4g: {1} - bigflash: {2} - layout: {3}", bigblock, corona4g, bigflash, layout);

            int smc_offset, smc_length;
            byte[] smc_len = new byte[4], smc_start = new byte[4];
            Buffer.BlockCopy(temp, 0x78, smc_len, 0, 4);
            Buffer.BlockCopy(temp, 0x7C, smc_start, 0, 4);
            smc_length = Oper.ByteArrayToInt(smc_len);
            smc_offset = Oper.ByteArrayToInt(smc_start);

            SMCdec = encrypt_SMC(SMCdec);
            if (!corona4g)
            {
                SMCdec = addecc_v2(SMCdec, true, 0, layout);
                smc_offset = (smc_offset / 0x200) * 0x210;
                smc_length = (smc_length / 0x200) * 0x210;
            }
            fileb.BaseStream.Seek(smc_offset, SeekOrigin.Begin);
            fileb.Write(SMCdec);


            infile.Close();
            return;
        }

        public static void injectSMCConf(string filename, byte[] data)
        {
            bool bigblock, corona4g, bigflash;
            int layout;
            if (filename == null) return;
            if (!File.Exists(filename)) return;
            FileInfo f = new FileInfo(filename);
            long s1 = f.Length;
            long imgsize = 0;
            byte[] image = Oper.openfile(filename, ref imgsize, 0x4200000);
            if (s1 >= 0x4200000)
            {
                bigflash = true;
                if (image[0x205] == 0xFF) bigblock = false;
                else bigblock = true;
            }
            else bigblock = bigflash = false;
            corona4g = true;
            byte[] smc_config = null;
            byte[] temp = BadBlock.find_bad_blocks_X(filename, 0x50);
            FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader file = new BinaryReader(infile);
            BinaryWriter fileb = new BinaryWriter(infile);

            //byte[] temp = (BadBlock.find_bad_blocks_X(filename, 0x50));
            byte[] sparedata = new byte[0x10];
            file.Read(temp, 0, 0x420);

            file.BaseStream.Seek(0x4400, SeekOrigin.Begin);
            file.Read(sparedata, 0, 0x10);
            layout = identifylayout(sparedata);

            if (hasecc(temp)) corona4g = false;
            if (variables.debugMode) Console.WriteLine("bigblock:{0} - corona4g: {1} - bigflash {2} - layout: {3}", bigblock, corona4g, bigflash, layout);

            int smc_config_offset, smc_config_length;
            if (!bigblock)
            {
                if (bigflash) smc_config_offset = 0x3FDF800;
                else smc_config_offset = 0xFEB800;
                smc_config_length = 0x4200 * 4;
                smc_config = new byte[smc_config_length];
            }
            else
            {
                smc_config_offset = 0x3D5C000;
                smc_config_length = 0x21000 * 4;
                smc_config = new byte[smc_config_length];
            }
            if (corona4g)
            {
                smc_config_offset = 0x2ff0000;
                smc_config_length = 0x4000 * 4;
                smc_config = new byte[smc_config_length];
            }

            fileb.BaseStream.Seek(smc_config_offset, SeekOrigin.Begin);
            if (!corona4g)
            {
                data = addecc_v2(data, true, smc_config_offset, layout);
            }
            fileb.Write(data);


            infile.Close();
            return;
        }

        public static byte[] getsmcconfig(string filename, out int block_offset)
        {
            bool bigblock, corona4g, bigflash;
            block_offset = 0;
            if (filename == null) return null;
            if (!File.Exists(filename)) return null;
            FileInfo f = new FileInfo(filename);
            long s1 = f.Length;
            long imgsize = 0;
            byte[] image = Oper.openfile(filename, ref imgsize, 0x4200000);
            if (s1 >= 0x4200000)
            {
                bigflash = true;
                if (image[0x205] == 0xFF) bigblock = false;
                else bigblock = true;
            }
            else bigblock = bigflash = false;
            corona4g = true;
            byte[] smc_config = null;

            FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader file = new BinaryReader(infile);

            byte[] temp = BadBlock.find_bad_blocks_X(filename, 0x50);
            if (hasecc(temp)) corona4g = false;

            if (variables.debugMode) Console.WriteLine("bigblock:{0} - corona4g: {1} - bigflash: {2}", bigblock, corona4g, bigflash);

            int smc_config_offset, smc_config_length;
            if (!bigblock)
            {
                block_offset = 0xC000;
                if (bigflash) smc_config_offset = 0x3FDF800;
                else smc_config_offset = 0xFEB800;
                smc_config_length = 0x4200 * 4;
                smc_config = new byte[smc_config_length];
            }
            else
            {
                block_offset = 0x60000;
                smc_config_offset = 0x3D5C000;
                smc_config_length = 0x21000 * 4;
                smc_config = new byte[smc_config_length];
            }
            if (corona4g)
            {
                block_offset = 0xC000;
                smc_config_offset = 0x2ff0000;
                smc_config_length = 0x4000 * 4;
                smc_config = new byte[smc_config_length];
            }

            file.BaseStream.Seek(smc_config_offset, SeekOrigin.Begin);
            file.Read(smc_config, 0, smc_config_length);

            file.Close();
            infile.Close();

            if (variables.debugMode) Console.WriteLine("length: {0:X} - offset {1:X}", smc_config_length, smc_config_offset);

            if (!corona4g)
            {
                unecc(ref smc_config);
            }
            return smc_config;
        }

        #endregion

        public static string getConsoleName(PrivateN nand, string flashconfig = "")
        {
            if (variables.debugMode) Console.WriteLine("Identifying Console");
            int[] cons = identifyConsole(nand, flashconfig);

            int max = -1;
            int howmany = 0;
            int consl = 0;
            for (int i = 0; i < 18; i++)
            {
                if (max < cons[i])
                {
                    max = cons[i];
                    consl = i;
                }
                else if (max == cons[i]) howmany++;
            }

            return variables.ctypes[consl].Text;
        }
        public static consoles getConsole(PrivateN nand, string flashconfig = "")
        {
            if (variables.debugMode) Console.WriteLine("Getting consoles...");
            int[] cons = identifyConsole(nand, flashconfig);
            // do Stuff
            int max = -1;
            int howmany = 0;
            int consl = 0;
            for (int i = 0; i < 18; i++)
            {
                if (max < cons[i])
                {
                    max = cons[i];
                    consl = i;
                }
                else if (max == cons[i]) howmany++;
            }
            
            return variables.ctypes[consl];
        }
        public static int[] identifyConsole(PrivateN nand, string flashconfig = "")
        {
            int[] cons = new int[18];
            int testCB;

            if (nand.bl.CB_X > 0) // Must check CB_B instead for RGH3
            {
                testCB = nand.bl.CB_B;
            }
            else
            {
                testCB = nand.bl.CB_A;
            }

            // CB check
            if (testCB >= 9188 && testCB <= 9250)
            {
                cons[1] += 3;
                cons[12] += 3;
            }
            else if (testCB >= 16000)
            {
                if (nand.noecc) cons[16] += 3;
                else
                {
                    cons[15] += 3;
                    cons[17] += 3;
                }
            }
            else if (testCB >= 13121 && testCB <= 13200)
            {
                if (nand.noecc) cons[11] += 3;
                else
                {
                    cons[9] += 3;
                    cons[10] += 3;
                }
            }
            else if (testCB >= 6712 && testCB <= 6780)
            {
                cons[4] += 3;
                cons[5] += 3;
                cons[6] += 3;
            }
            else if (testCB >= 4558 && testCB <= 4590)
            {
                cons[3] += 3;
                cons[13] += 3;
            }
            else if ((testCB >= 1888 && testCB <= 1960) || (testCB >= 7373 && testCB <= 7378) || testCB == 8192)
            {
                cons[7] += 3;
                cons[8] += 3;
            }
            else if (testCB >= 5761 && testCB <= 5780)
            {
                if (nand.bl.CB_B >= 7373 && nand.bl.CB_B <= 7378)
                {
                    cons[7] += 3;
                    cons[8] += 3;
                }
                else
                {
                    cons[2] += 3;
                    cons[14] += 3;
                }
            }

            // smc check
            int smctype = 0;
            if(nand._smc != null) smctype = nand._smc[0x100] >> 4 & 15;

            if (smctype < variables.console_types.Length && smctype >= 0)
            {
                if (smctype == 1) // Xenon SMC doesn't work on any other consoles, so we need higher bias here for Xenon's with Falcon flash
                {
                    cons[7] += 5;
                    cons[8] += 5;
                }
                else if (smctype == 2)
                {
                    cons[3] += 2;
                    cons[13] += 2;
                }
                else if (smctype == 3)
                {
                    cons[2] += 2;
                    cons[14] += 2;
                }
                else if (smctype == 4)
                {
                    cons[4] += 2;
                    cons[5] += 2;
                    cons[6] += 2;
                }
                else if (smctype == 5)
                {
                    cons[1] += 2;
                    cons[12] += 2;
                }
                else if (smctype == 6)
                {
                    cons[9] += 2;
                    cons[10] += 2;
                    cons[11] += 2;
                }
                else if (smctype == 7)
                {
                    cons[15] += 2;
                    cons[16] += 2;
                    cons[17] += 2;
                }
            }

            // Flash config check
            if (!string.IsNullOrWhiteSpace(flashconfig))
            {
                if (flashconfig == "008A3020" || flashconfig == "00AA3020")
                {
                    cons[6]++;
                    cons[12]++;
                }
                else if (flashconfig == "008C3020" || flashconfig == "00AC3020")
                {
                    cons[9]++;
                    cons[17]++;
                }
                else if (flashconfig == "C0462002")
                {
                    cons[11]++;
                    cons[16]++;
                }
                else if (flashconfig == "01198010")
                {
                    cons[2]++;
                    cons[3]++;
                    cons[5]++;
                    cons[8]++;
                }
                else if (flashconfig == "01198030")
                {
                    cons[7]++;
                    cons[13]++;
                    cons[14]++;
                }
                else if (flashconfig == "00023010")
                {
                    cons[1]++;
                    cons[4]++;
                }
                else if (flashconfig == "00043000")
                {
                    cons[10]++;
                    cons[15]++;
                }
            }

            // File length
            if (File.Exists(nand._filename))
            {
                FileInfo fl = new FileInfo(nand._filename);
                long length = fl.Length;

                if (length == 17301504)
                {
                    cons[1]++;
                    cons[2]++;
                    cons[3]++;
                    cons[4]++;
                    cons[5]++;
                    cons[8]++;
                    cons[10]++;
                    cons[15]++;
                }
                else if (length == 69206016 || length == 276824064 || length == 553648128)
                {
                    cons[6] += 2;
                    cons[7] += 2;
                    cons[9] += 2;
                    cons[12] += 2;
                    cons[13] += 2;
                    cons[14] += 2;
                    cons[17] += 2;
                }
                else
                {
                    cons[11]++;
                    cons[16]++;
                }
            }

            // Spare data check
            if (nand.noecc)
            {
                cons[11]++;
                cons[16]++;
            }
            else
            {
                //IMAGE_LAYOUT_0: XSB
                //IMAGE_LAYOUT_1: PSB/KSB 16MB
                //IMAGE_LAYOUT_2: PSB/KSB 256/512MB
                int layout = -1;
                List<int> layouts = new List<int>();
                byte[] file = BadBlock.find_bad_blocks_X(nand._filename, 50);
                layouts.Add(identifylayout(Oper.returnportion(ref file, 0x14C00, 0x10)));
                layouts.Add(identifylayout(Oper.returnportion(ref file, 0x41F0, 0x10)));
                layouts.Add(identifylayout(Oper.returnportion(ref file, 0x83F0, 0x10)));
                layouts.Add(identifylayout(Oper.returnportion(ref file, 0xC800, 0x10)));
                layouts.Add(identifylayout(Oper.returnportion(ref file, 0xE2D0, 0x10)));
                layout = (int)layouts.Median();

                if (layout == 0)
                {
                    cons[2]++;
                    cons[3]++;
                    cons[5]++;
                    cons[7]++;
                    cons[8]++;
                    cons[13]++;
                    cons[14]++;
                }
                else if (layout == 1)
                {
                    cons[1]++;
                    cons[4]++;
                    cons[10]++;
                    cons[15]++;
                }
                else if (layout == 2)
                {
                    cons[6]++;
                    cons[9]++;
                    cons[12]++;
                    cons[17]++;
                }
            }

            return cons;
        }

        public static byte[] CalculateSMCHash(byte[] SMCen)
        {
            ulong s0 = 0;
            ulong s1 = 0;
            for (int i = 0; i < SMCen.Length / 4; i++)
            {
                byte[] tmp = new byte[4];
                Buffer.BlockCopy(SMCen, i * 4, tmp, 0, 4);
                uint tmp2 = BitConverter.ToUInt32(Oper.endianness(tmp), 0);

                s0 += tmp2;
                s1 -= tmp2;
                s0 = (s0 << 29) | ((s0 & 0xFFFFFFF800000000) >> 35); // poor man's rotate left 29
                s1 = (s1 << 31) | ((s1 & 0xFFFFFFFE00000000) >> 33); // poor man's rotate left 31
            }

            byte[] csum = new byte[0x10];
            Buffer.BlockCopy(Oper.StringToByteArray(s0.ToString("X")), 0, csum, 0, 0x8);
            Buffer.BlockCopy(Oper.StringToByteArray(s1.ToString("X")), 0, csum, 8, 0x8);
            return csum;
        }
        public static byte[] FixPerBoxDigest(byte[] SMC_en, byte[] CB_dec, byte[] CB_nonce, byte[] CB_A_key, bool CB_A_new_crypto, byte[] cpukey)
        {
            
            byte[] RC4_key = { };

            if (null == CB_A_key)
            {
                // For a single CB machine, there's no CB_A key
                // and as such the RC4 key is simple to calculate
                RC4_key = Oper.HMAC_SHA1(secret_1bl, CB_nonce);
            }
            else
            {
                RC4_key = getCbbRc4Key(CB_A_key, CB_A_new_crypto, CB_nonce, cpukey);
            }

            byte[] reserved = Oper.returnportion(CB_dec, 0x24, 0xC);
            byte[] pairingdata = Oper.returnportion(CB_dec, 0x20, 3);

            byte[] digest = new byte[0x30];
            byte[] SMC_HASH = CalculateSMCHash(SMC_en);

            // The per-box digest/SMC auth hash/etc, aka what they
            // were messing with for the timing attack is made up
            // of the following (CB == CB_B):
            //
            // 1) CB RC4 key (calculated)
            // 2) CB Pairing Data (0x20 - 0x22)
            // 3) CB LDV (0x23)
            // 4) CB Reserved data (0x24 - 0x2f)
            // 5) SMC Hash (of the encrypted SMC)
            //
            // Then, do an HMAC SHA1 with all of this as the message
            // and the CPU key as the key
            //
            // Or, if you're RGH, you can just patch out the check
            // and not need to recalculate anything.
            // 
            // Patch:
            //     0x48 0x00 0x00 0x14
            //
            // Location:
            //     CB_B 5772: 0x6B2C
            //     CB_B 6752: 0x6B74

            Buffer.BlockCopy(RC4_key, 0, digest, 0x0, 0x10);
            Buffer.BlockCopy(pairingdata, 0, digest, 0x10, 0x3);
            digest[0x13] = CB_dec[0x23];
            Buffer.BlockCopy(reserved, 0, digest, 0x14, 0xC);
            Buffer.BlockCopy(SMC_HASH, 0, digest, 0x20, 0x10);

            return Oper.returnportion(Oper.HMAC_SHA1(cpukey, digest), 0, 0x10);
        }

        public static void extract(string filename, string outfolder, string cpukey = "")
        {
            if (filename == null) return;
            if (!File.Exists(filename)) return;
            long size = 0;
            byte[] image = Oper.openfile(filename, ref size, 1024 * 1024);
            //
            bool bigblock = false;
            bool corona = false;
            bool bigflash = false;
            FileInfo f = new FileInfo(filename);
            long s1 = f.Length;
            if (s1 >= 0x4200000)
            {
                bigflash = true;
                if (image[0x205] == 0xFF) bigblock = false;
                else bigblock = true;
            }
            if (image[0] == 0xFF && image[1] == 0x4F)
            {
                byte[] SMC = null, Keyvault = null, smc_config = null;
                if (hasecc(image)) unecc(ref image);
                else corona = true;
                if (variables.extractfiles) Oper.savefile(image, Path.Combine(outfolder, "image.bin"));
                byte[] data, cb_dec = { };
                byte[] CB_A = null, CB_B = null;
                byte[] block_offset = new byte[4];
                Buffer.BlockCopy(image, 0x8, block_offset, 0, 4);
                //block_offset = returnportion(image, 0x8, 4);
                byte[] smc_len = new byte[4], smc_start = new byte[4];
                Buffer.BlockCopy(image, 0x78, smc_len, 0, 4);
                Buffer.BlockCopy(image, 0x7C, smc_start, 0, 4);
                //smc_len = returnportion(image, 0x78, 4);
                //smc_start = returnportion(image, 0x7C, 4);
                SMC = new byte[Convert.ToInt32(Oper.ByteArrayToString(smc_len), 16)];
                //SMC = returnportion(image, Convert.ToInt32(ByteArrayToString(smc_start), 16), Convert.ToInt32(ByteArrayToString(smc_len), 16));
                Buffer.BlockCopy(image, Oper.ByteArrayToInt(smc_start), SMC, 0x00, Oper.ByteArrayToInt(smc_len));
                Oper.savefile(SMC, Path.Combine(outfolder, "SMC_en.bin"));
                SMC = decrypt_SMC(SMC);
                Oper.savefile(SMC, Path.Combine(outfolder, "SMC_dec.bin"));
                SMC = null;

                #region keyvault
                Keyvault = new byte[0x4000];
                Keyvault = Oper.returnportion(image, 0x4000, 0x4000);
                Oper.savefile(Keyvault, Path.Combine(outfolder, "KV_en.bin"));
                if (cpukey != "")
                {
                    Keyvault = decryptkv(Keyvault, Oper.StringToByteArray(cpukey));
                    if (Oper.allsame(Oper.returnportion(Keyvault, 0x40, 0x20), 0x00)) Oper.savefile(Keyvault, Path.Combine(outfolder, "KV_dec.bin"));
                }
                Keyvault = null;
                #endregion
                #region blocks

                int block = 0, block_size, id;
                byte block_id;
                byte[] block_build_b = new byte[2], block_size_b = new byte[4];
                int block_offset_b = Convert.ToInt32(Oper.ByteArrayToString(block_offset), 16);
                int semi = 0;
                for (block = 0; block < 10; block++)
                {

                    block_id = image[block_offset_b + 1];
                    //block_size_b = returnportion(image, block_offset_b + 12, 4);
                    Buffer.BlockCopy(image, block_offset_b + 12, block_size_b, 0x00, 4);
                    block_size = Convert.ToInt32(Oper.ByteArrayToString(block_size_b), 16);
                    block_size += 0xF;
                    block_size &= ~0xF;
                    id = block_id & 0xF;
                    if (variables.debugMode) Console.WriteLine("Found {0}BL at {1}", id, block_offset_b);
                    data = new byte[block_size];
                    //data = returnportion(image, block_offset_b, block_size);
                    Buffer.BlockCopy(image, block_offset_b, data, 0x00, block_size);

                    if (id == 2)
                    {
                        if (semi == 0)
                        {
                            CB_A = data;
                            cb_dec = decrypt_CB(CB_A);
                            Oper.savefile(data, Path.Combine(outfolder, "CB_A.bin"));
                            Oper.savefile(cb_dec, Path.Combine(outfolder, "CB_A_dec.bin"));
                            semi = 1;
                        }
                        else if (semi == 1)
                        {
                            CB_B = data;
                            Oper.savefile(data, Path.Combine(outfolder, "CB_B.bin"));
                            if (variables.cpukey != "")
                            {
                                cb_dec = decrypt_CB_cpukey(CB_B, decrypt_CB(CB_A), Oper.StringToByteArray(variables.cpukey));
                                Oper.savefile(cb_dec, Path.Combine(outfolder, "CB_B_dec.bin"));
                            }
                            semi = 0;
                        }

                    }
                    block_offset_b += block_size;
                    if (id == 4) break;
                }
                #endregion
                image = null;
                FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read);
                BinaryReader file = new BinaryReader(infile);

                int smc_config_offset, smc_config_length;
                if (!bigblock)
                {
                    if (bigflash) smc_config_offset = 0x3FDF800;
                    else smc_config_offset = 0xFEB800;
                    smc_config_length = 0x4200 * 4;
                    smc_config = new byte[smc_config_length];
                }
                else
                {
                    smc_config_offset = 0x3D5C000;
                    smc_config_length = 0x21000 * 4;
                    smc_config = new byte[smc_config_length];
                }
                if (corona)
                {
                    smc_config_offset = 0x2ff0000;
                    smc_config_length = 0x4000 * 4;
                    smc_config = new byte[smc_config_length];
                }
                file.BaseStream.Seek(smc_config_offset, SeekOrigin.Begin);
                file.Read(smc_config, 0, smc_config_length);

                file.Close();
                infile.Close();
                //smc_config = returnportion(image, smc_config_offset, 0x400);

                if (!corona)
                {
                    byte[] res_smc = { };
                    for (int i = 0; i < smc_config.Length; i += 0x210)
                    {
                        res_smc = Oper.concatByteArrays(res_smc, Oper.returnportion(smc_config, i, 0x200), res_smc.Length, 0x200);
                    }
                    smc_config = res_smc;
                    res_smc = null;
                }
                Oper.savefile(smc_config, Path.Combine(outfolder, "smc_config.bin"));
                smc_config = null;
                if (!bigblock || corona) extractfcrt(filename, outfolder, cpukey);
                Console.WriteLine("Files saved at {0}", outfolder);
            }
            else
            {
                Console.WriteLine("unknown image found !");
                return;
            }
        }

        public static bool imageknown(string filename, bool print = true)
        {
            long size = 0;
            byte[] data = Oper.openfile(filename, ref size, 50);
            //byte[] data = find_bad_blocks_X(filename, 1);
            //checkifbadblock(returnportion(data, 0, 0x4200), 0);
            if (!ascii.GetString(data).Contains("Microsoft") && print)
            {
                if (variables.debugMode) Console.WriteLine(ascii.GetString(data));
                if (data[0] == 0x46 && data[1] == 0x57 && data[2] == 0x41 && data[3] == 0x00) Console.WriteLine("DemoN FW");
            }
            if (data[0] == 0xFF && data[1] == 0x4F)
            {
                return true;
            }
            else return false;
        }

        // DaCukiMonsta 09 Nov 2021
        public static string consoleID_KV_to_friendly(string KVencoded)
        {
            // take KV encoded console ID, and convert to friendly console ID
            // KVencoded must be 10 characters hex string

            // contains no validation that this is true, proceed at your own risk
            // or add validation and exceptions

            // convert the first 9 characters from hex to decimal
            UInt64 first_part = Convert.ToUInt64(KVencoded.Substring(0, 9), 16); // uint64 because more than 4 bytes

            // add last digit from original encoding, and left pad with zeros
            string friendly_encoding = (first_part.ToString() + KVencoded.Substring(9)).PadLeft(12, '0');
            return friendly_encoding;
        }

        // DaCukiMonsta 09 Nov 2021
        public static string ConsoleID_friendly_to_KV(string friendly_encoded)
        {
            // take friendly encoded console ID, and convert to friendly KV console ID
            // friendly_encoded must be 12 characters, first 11 decimal, last one can be hex

            // contains no validation that this is true, proceed at your own risk
            // or add validation and exceptions

            // convert the first 11 characters from decimal to hex
            UInt64 first_part = Convert.ToUInt64(friendly_encoded.Substring(0, 11)); // uint64 because more than 4 bytes

            // add last digit from original encoding, and left pad with zeros
            string KVencoded = (first_part.ToString("X") + friendly_encoded.Substring(11)).PadLeft(10, '0');
            return KVencoded;
        }

        public static void patch_kv(ref byte[] keyvault, KVInfo k)
        {
            byte[] dvdkey_b = Oper.StringToByteArray_v2(k.dvdkey);
            byte[] region_b = Oper.StringToByteArray_v2(k.region);
            byte[] osig_b = Oper.StringToByteArray_v2(k.osig);
            byte[] cid_b = Oper.StringToByteArray_v2(k.consoleid);
            byte[] serial_b = Encoding.ASCII.GetBytes(k.serial);
            byte[] mfdate_b = Encoding.ASCII.GetBytes(k.mfdate);

            keyvault.Replace(dvdkey_b, 0x100, 0x10);
            keyvault[0xC8] = region_b[0];
            keyvault[0xC9] = region_b[1];
            keyvault.Replace(osig_b, 0xC8A, 40);
            keyvault.Replace(cid_b, 0x9CA, 5);
            keyvault.Replace(serial_b, 0xB0, 12);
            keyvault.Replace(mfdate_b, 0x9E4, 8);
        }

        public static void decrypt_fcrt(byte[] fcrt, byte[] cpukey)
        {
            if (fcrt == null) return;
            try
            {
                if (variables.debugMode) Console.WriteLine(cpukey.Length);
                if (variables.debugMode) Console.WriteLine(fcrt.Length);
                if (fcrt.Length != 0x4000) { Console.WriteLine("Wrong fcrt.bin size"); return; }
                if (cpukey.Length != 0x10) { Console.WriteLine("Wrong CPU Key size"); return; }
                Console.WriteLine(Environment.NewLine + "Decrypting fcrt.bin");
                int size, offset;
                offset = Oper.ByteArrayToInt(Oper.returnportion(fcrt, 0x11C, 4));
                size = Oper.ByteArrayToInt(Oper.returnportion(fcrt, 0x118, 4));
                if (variables.debugMode) Console.WriteLine("offset: {0:X} - size: {1:X}", offset, size);
                byte[] toEncryptArray = Oper.returnportion(fcrt, offset, size); // here here
                if (variables.debugMode) Console.WriteLine(toEncryptArray.Length);
                RijndaelManaged rDel = new RijndaelManaged();
                rDel.IV = Oper.returnportion(fcrt, 0x100, 0x10);
                rDel.Key = cpukey;
                rDel.Mode = CipherMode.CBC; // http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
                rDel.Padding = PaddingMode.None; // better lang support
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                if (variables.debugMode) Console.WriteLine(resultArray.Length);
                Console.WriteLine("Checking hash");
                SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                byte[] Hash1 = sha.ComputeHash(resultArray);
                if (variables.debugMode) Console.WriteLine("{0} - {1}", Oper.ByteArrayToString(Hash1), Hash1.Length);
                if (variables.debugMode) Console.WriteLine(Oper.ByteArrayToString(Oper.returnportion(fcrt, 0x12C, 0x14)));
                if (Oper.ByteArrayCompare(Hash1, Oper.returnportion(fcrt, 0x12C, 0x14), 0x14)) Console.WriteLine("Decrypted Successfully");
                else Console.WriteLine("Failed");
                Oper.savefile(Oper.concatByteArrays(Oper.returnportion(fcrt, 0, offset), resultArray, offset, resultArray.Length), Path.Combine(variables.outfolder, "fcrt_dec.bin"));
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }
        }
        public static bool decrypt_fcrt(byte[] fcrt, byte[] cpukey, out byte[] fcrtd)
        {
            fcrtd = null;
            if (fcrt == null) return false;
            try
            {
                if (variables.debugMode) Console.WriteLine(cpukey.Length);
                if (variables.debugMode) Console.WriteLine(fcrt.Length);
                if (fcrt.Length != 0x4000) { return false; }
                if (cpukey.Length != 0x10) { return false; }
                int size, offset;
                offset = Oper.ByteArrayToInt(Oper.returnportion(fcrt, 0x11C, 4));
                //size = Oper.ByteArrayToInt(Oper.returnportion(fcrt, 0x118, 4));
                size = 0x3EC0;
                byte[] toEncryptArray = Oper.returnportion(fcrt, offset, size); // here here
                RijndaelManaged rDel = new RijndaelManaged();
                rDel.IV = Oper.returnportion(fcrt, 0x100, 0x10);
                rDel.Key = cpukey;
                rDel.Mode = CipherMode.CBC; // http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
                rDel.Padding = PaddingMode.None; // better lang support
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                byte[] Hash1 = sha.ComputeHash(resultArray);
                if (!Oper.ByteArrayCompare(Hash1, Oper.returnportion(fcrt, 0x12C, 0x14), 0x14)) return false;
                fcrtd = (Oper.concatByteArrays(Oper.returnportion(fcrt, 0, offset), resultArray, offset, resultArray.Length));
                return true;
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }
            return false;
        }

        public static void injectXell(string flashFilePath, string xellFilePath)
        {
            int[] xellOffsets = { 0x70000,    // Glitch, Glitch2, Glitch2m, DevGL: xell-gggggg
                                  0x95060,    // JTAG: xell-2f
                                  0x100000,   // XeLL-Only Image (Main XeLL)
                                  0xC0000,    // XeLL-Only Image (Backup XeLL)
                                  0xE0000,    // Unknown, but listed in libxenon updxell function
                                  0xF0000,    // XeLL in the flashfs of XDKBuild and RGLoader images
                                  0xF4000,    // XeLL in the flashfs of 64mb Devkit images
                                  0xB80000 }; // XeLL in the flashfs of BB Jasper and BB Trinity XDKBuild images

            int blockType = 0;
            bool flashHasEcc = false;

            int pagesz = 0x200;
            int pagesz_phys = 0x210;

            int xellOffset = 0;
            int xellOffsetPhys = 0;

            int xellFirstPageOffsetPhys = 0;
            int xellPageNumber = 0;
            int xellOffsetInPage = 0;
            int xellPageCount = 0;

            // Read in flash data
            byte[] flashData = File.ReadAllBytes(flashFilePath);
            byte[] xellData = File.ReadAllBytes(xellFilePath);

            // XeLL should be an even multiple of the page size
            xellPageCount = xellData.Length / pagesz;

            // Determine whether this image has ECC
            if (flashData.Length == 17301504 || flashData.Length == 69206016 || flashData.Length == 1351680 )
            {
                flashHasEcc = true;
            }
            else if (flashData.Length == 50331648 || flashData.Length == 1310720 )
            {
                flashHasEcc = false;
            }
            else
            {
                Console.WriteLine("Couldn't inject XeLL: Invalid flash image size");
                return;
            }

            // XeLL binaries should always be 256kb. If not, either they've made it
            // larger and this check needs to change, or something has gone wrong.
            if (xellData.Length != 262144)
            {
                Console.WriteLine("Couldn't inject XeLL: Invalid XeLL binary size");
                return;
            }

            Console.WriteLine("Injecting " + Path.GetFileName(xellFilePath) + " into " + Path.GetFileName(flashFilePath));

            // If the flash has ECC data, determine the block type so ECC data can be recalculated
            if (flashHasEcc)
            {
                byte[] sparedata = flashData.Skip(0x4400).Take(0x10).ToArray();

                // Block Types
                // 0 = Small block NAND (XSB)
                // 1 = Small block NAND on BB controller (PSB/KSB)
                // 2 = Big block NAND on BB controller (PSB/KSB)
                blockType = identifylayout(sparedata);

            }

            // Determine where in the world XeLL lives in this image
            foreach (int testXellOffset in xellOffsets)
            {
                if(flashHasEcc)
                {
                    // Calculate WHERE in the physical image we should be able to find XeLL,
                    // and calculate a few other values that will help us later
                    xellOffsetInPage = testXellOffset % pagesz;
                    xellPageNumber = testXellOffset / pagesz;
                    xellFirstPageOffsetPhys = xellPageNumber * pagesz_phys;
                    xellOffsetPhys = xellFirstPageOffsetPhys + xellOffsetInPage;
                }
                else
                {
                    // For a non-ECC flash image, the offset is the physical offset as there
                    // is no ECC data to take in to account
                    xellOffsetPhys = testXellOffset;
                }

                // Look for the XeLL header to see if we're at the right spot
                if (Oper.ByteArrayCompare(flashData, Oper.StringToByteArray("48000020480000EC4800000048000000"), xellOffsetPhys, 0, 0x10))
                {
                    xellOffset = testXellOffset;
                    Console.WriteLine("XeLL found at offset 0x" + xellOffset.ToString("x"));

                    if (0 != xellOffsetInPage)
                    {
                        // If XeLL is not stored on a page boundary (thank you JTAG)
                        // then we need to read one more page from the flash data
                        xellPageCount += 1;
                    }

                    if (flashHasEcc)
                    {
                        // Get the physical pages from the flash image that we need to modify
                        byte[] xellFlashPages = flashData.Skip(xellFirstPageOffsetPhys).Take(xellPageCount * pagesz_phys).ToArray();

                        // Strip the ECC data
                        xellFlashPages = unecc(xellFlashPages);

                        // Copy the xell data into the pages
                        Buffer.BlockCopy(xellData, 0, xellFlashPages, xellOffsetInPage, xellData.Length);

                        // Re-add ECC data
                        xellFlashPages = addecc_v2(xellFlashPages, true, xellPageNumber * pagesz_phys, blockType);

                        // Copy the ECC'ed pages back to the NAND image
                        Buffer.BlockCopy(xellFlashPages, 0, flashData, xellFirstPageOffsetPhys, xellFlashPages.Length);
                    }
                    else
                    {
                        // We can just do a plain copy if there's no ECC data
                        Buffer.BlockCopy(xellData, 0, flashData, xellOffset, xellData.Length);
                    }

                    // Do a final sanity check to make sure something didn't go wrong
                    if (!Oper.ByteArrayCompare(flashData, Oper.StringToByteArray("48000020480000EC4800000048000000"), xellOffsetPhys, 0, 0x10))
                    {
                        Console.WriteLine("Couldn't inject XeLL: couldn't detect XeLL in the resulting flash image");
                        return;
                    }
                }
            }

            if( 0 == xellOffset )
            {
                Console.WriteLine("Couldn't inject XeLL: did not find XeLL in this flash image");
                return;
            }

            // So we've updated the flashData, write it back to disk!
            File.WriteAllBytes(flashFilePath, flashData);

            Console.WriteLine("Successfully injected XeLL");
        }

        /// <summary>
        /// Fixes the various bugs that XeBuild has when generating images for XSB consoles
        /// - For Falcon and Jasper XSB, the patch slot size is set to 0x00000000 rather than the
        /// expected 0x00010000. This causes the 2BL and 4BL to panic.
        /// - For Xenon, the KV offset is not set causing XeLL to be unable to show the DVD key, console serial, etc.
        /// </summary>
        /// <param name="flashFilePath">Flash image to be patched, result will be written back to the same file</param>
        public static void fixBuggyXeBuildImage(string flashFilePath)
        {
            byte[] flashData = { };

            // Logical page size is always 0x200
            // and the physical page size (for ECC images) is always 0x210
            int pagesz = 0x200;
            int pagesz_phys = 0x210;

            int blockType = 0;
            bool flashHasEcc = false;

            Console.WriteLine("Patching image to resolve xeBuild bugs...");

            // Read in the flash image
            try
            {
                flashData = File.ReadAllBytes(flashFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Image patch error: couldn't read input flash image");
                if (variables.debugMode) Console.WriteLine(ex.ToString());
                return;
            }

            // Determine whether this image has ECC
            if (flashData.Length == 17301504 || flashData.Length == 69206016)
            {
                flashHasEcc = true;
            }
            else if (flashData.Length == 50331648)
            {
                // Flash data doesn't have ECC, pagesz_phys = pagesz
                flashHasEcc = false;
                pagesz_phys = pagesz;
            }
            else
            {
                Console.WriteLine("Image patch error: invalid image size");
                return;
            }

            // If the flash has ECC data, determine the block type so ECC data can be recalculated
            if (flashHasEcc)
            {
                byte[] sparedata = flashData.Skip(0x4400).Take(0x10).ToArray();

                // Block Types
                // 0 = Small block NAND (XSB)
                // 1 = Small block NAND on BB controller (PSB/KSB)
                // 2 = Big block NAND on BB controller (PSB/KSB)
                blockType = identifylayout(sparedata);
            }

            
            // The patch slot size that we need to fix for Falcon images is always
            // in the first NAND page, so we can just take the first page of bytes
            byte[] nandPatchPages = flashData.Take(pagesz_phys).ToArray();

            if (flashHasEcc)
            {
                // remove the ECC so we can copy our patch data to the logical addresses
                nandPatchPages = unecc(nandPatchPages);
            }

            // If the patch slot size is unset, set the patch slot size to 0x00010000
            // which is the size for all common image types
            if( 0 == BitConverter.ToInt32(nandPatchPages.Skip(0x70).Take(0x4).ToArray(),0) )
            {
                Console.WriteLine("Fixing patch slot size set to zero...");

                nandPatchPages[0x70] = 0x00;
                nandPatchPages[0x71] = 0x01;
                nandPatchPages[0x72] = 0x00;
                nandPatchPages[0x73] = 0x00;
            }


            // If the KV size is unset, set the KV size to 0x00004000 (this is assuming a retail KV)
            if (0 == BitConverter.ToInt32(nandPatchPages.Skip(0x60).Take(0x4).ToArray(), 0))
            {
                Console.WriteLine("Fixing KV size set to zero...");

                nandPatchPages[0x60] = 0x00;
                nandPatchPages[0x61] = 0x00;
                nandPatchPages[0x62] = 0x40;
                nandPatchPages[0x63] = 0x00;
            }

            // Re-add ECC data and copy it over to the flash data buffer
            if (flashHasEcc)
            {
                nandPatchPages = addecc_v2(nandPatchPages, true, 0, blockType);
            }
            Buffer.BlockCopy(nandPatchPages, 0, flashData, 0, nandPatchPages.Length);

            try
            {
                // So we've updated the flashData, write it back to disk!
                File.WriteAllBytes(flashFilePath, flashData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Image patch error: couldn't write output flash image");
                if (variables.debugMode) Console.WriteLine(ex.ToString());
                return;
            }

            Console.WriteLine("Successfully patched image!");
            Console.WriteLine("");
        }


        /// <summary>
        /// Zero-pairs the SB of a devkit image, the final step in generating a 64mb DevGL image
        /// </summary>
        /// <param name="flashFilePath">Flash image to be patched, result will be written back to the same file</param>
        /// <param name="sequenced">True if this is part of a xeBuild operation, false otherwise</param>
        public static void zeroPairDevkitSb(string flashFilePath, bool sequenced)
        {
            byte[] flashData = { };

            // Logical page size is always 0x200
            // and the physical page size (for ECC images) is always 0x210
            int pagesz = 0x200;
            int pagesz_phys = 0x210;

            int blockType = 0;
            bool flashHasEcc = false;

            // Read in the flash image
            try
            {
                flashData = File.ReadAllBytes(flashFilePath);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Zero pair SB error: couldn't read input flash image");
                if (variables.debugMode) Console.WriteLine(ex.ToString());

                if (sequenced)
                {
                    variables.xefinished = true;
                    MainForm.mainForm.xPanel.xeExitActual(false);
                }
                return;
            }

            // Determine whether this image has ECC
            if (flashData.Length == 17301504 || flashData.Length == 69206016)
            {
                flashHasEcc = true;
            }
            else if (flashData.Length == 50331648)
            {
                // Flash data doesn't have ECC, pagesz_phys = pagesz
                flashHasEcc = false;
                pagesz_phys = pagesz;
            }
            else
            {
                Console.WriteLine("Zero pair SB error: Invalid flash image size");
                if (sequenced)
                {
                    variables.xefinished = true;
                    MainForm.mainForm.xPanel.xeExitActual(false);
                }
                return;
            }

            // If the flash has ECC data, determine the block type so ECC data can be recalculated
            if (flashHasEcc)
            {
                byte[] sparedata = flashData.Skip(0x4400).Take(0x10).ToArray();

                // Block Types
                // 0 = Small block NAND (XSB)
                // 1 = Small block NAND on BB controller (PSB/KSB)
                // 2 = Big block NAND on BB controller (PSB/KSB)
                blockType = identifylayout(sparedata);
            }

            // Encryption of the SC and later stages are different compared
            // to retail CB/CD encryption- SC uses a zero key and nonce
            // and the SD depends on the SC key. e.g.
            //
            // sb_key = XeCryptHmacSha(XECRYPT_1BL_KEY, sb_nonce)
            // sc_key = XeCryptHmacSha(ZERO_KEY, sc_nonce)
            // sd_key = XeCryptHmacSha(sc_key, sd_nonce)
            // sd_key = XeCryptHmacSha(sd_key, se_nonce)
            // 
            // So, we can decrypt and zeropair the SB without touching
            // later stages. Isn't that convenient!

            // Determine the logical SB offset by looking at 0x8 in NAND
            // Then calculate the physical offset and offset in page
            int logicalSbOffset = BitConverter.ToInt32(flashData.Skip(0x8).Take(4).Reverse().ToArray(), 0);
            int sbOffsetInPage = logicalSbOffset % pagesz;

            // Calculate the offset of the first page containing the SB
            int physicalSbPageOffset = (logicalSbOffset / pagesz) * pagesz_phys;

            // The size of the SB is stored in its header, which is 0xC in to the SB binary
            int sbSize = BitConverter.ToInt32(flashData.Skip(physicalSbPageOffset + 0xC).Take(4).Reverse().ToArray(), 0);

            // Get the length of data to read from NAND, which is the (SB size / page size) + 1
            // in case the SB doesn't start on a page boundary or the size of the SB isn't an 
            // exact multiple of the page size.
            int patchDataLength = ((sbSize / pagesz) + 1) * pagesz_phys;

            // Get the pages of the SB that we need to patch
            byte[] nandPatchPages = flashData.Skip(physicalSbPageOffset).Take(patchDataLength).ToArray();

            if (flashHasEcc)
            {
                // remove the ECC so we can copy our patch data to the logical addresses
                nandPatchPages = unecc(nandPatchPages);
            }

            // Extract the encrypted SB data
            byte[] sb_crypt = nandPatchPages.Skip(sbOffsetInPage).Take(sbSize).ToArray();

            // Check that we actually read an SB by checking the magic bytes
            // at 0x0 and 0x1, they should be 0x53 (S) and 0x42 (B).
            // This function does not support zeropairing CB or CF/CG
            if(sb_crypt[0] != 0x53 || sb_crypt[1] != 0x42)
            {
                Console.WriteLine("Zero pair SB error: BL at offset " + logicalSbOffset.ToString("X") + " is not an SB.");

                if (sequenced)
                {
                    variables.xefinished = true;
                    MainForm.mainForm.xPanel.xeExitActual(false);
                }
                return;
            }

            // Decrypt the SB (it's encrypted the same way as a retail single CB or split CB_A)
            byte[] sb_decrypt = Nand.decrypt_CB(sb_crypt);

            // Blow away all the pairing data, LDV, auth hash, etc
            // for a zero paired image and then re-encrypt everything
            // - Pairing Data: 0x20-0x22
            // - LDV: 0x23
            // - CB auth hash/per-box digest: 0x30-0x3f
            for (int i = 0x20; i<= 0x3F; i++)
            {
                sb_decrypt[i] = 0x0;
            }

            // Use the same nonce from the encrypted SB
            // sb_key is just to make encrypt_CB happy,
            // we don't need it for any later stages
            byte[] sb_nonce = sb_crypt.Skip(0x10).Take(0x10).ToArray();
            byte[] sb_key = { };

            // Re-encrypt the SB and place it back in the patch data
            sb_crypt = encrypt_CB(sb_decrypt, sb_nonce, ref sb_key);
            Buffer.BlockCopy(sb_crypt, 0, nandPatchPages, sbOffsetInPage, sbSize);

            // Re-add ECC data and copy it over to the flash data buffer
            if(flashHasEcc)
            {
                nandPatchPages = addecc_v2(nandPatchPages, true, physicalSbPageOffset, blockType);
            }
            Buffer.BlockCopy(nandPatchPages, 0, flashData, physicalSbPageOffset, nandPatchPages.Length);

            try
            {
                // So we've updated the flashData, write it back to disk!
                File.WriteAllBytes(flashFilePath, flashData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Zero pair SB error: couldn't write modified flash image");
                if (variables.debugMode) Console.WriteLine(ex.ToString());

                if (sequenced)
                {
                    variables.xefinished = true;
                    MainForm.mainForm.xPanel.xeExitActual(false);
                }
                return;
            }

            Console.WriteLine("Successfully zero paired SB");
            Console.WriteLine("");

            if(sequenced)
            {
                variables.xefinished = true;
                MainForm.mainForm.xPanel.xeExitActual();
            }
            else
            {
                MainForm.mainForm.nand_init();
            }
        }

        public static string extend16mbTo64mb(string flashFilePath)
        {
            string flashFileResultPath = flashFilePath + "_aligned.bin";

            byte[] flashData = File.ReadAllBytes(flashFilePath);
            
            int blockType = 0;

            if (flashData.Length != 17301504)
            {
                Console.WriteLine("Error: input NAND image is not 16mb with ECC");
                return flashFilePath;
            }

            Console.WriteLine("Aligning 16mb NAND image to 64mb...");

            // Determine what kind of ECC is in this image... should only
            // ever be 0 or 1, 2 would be unexpected
            byte[] sparedata = flashData.Skip(0x4400).Take(0x10).ToArray();

            // Block Types
            // 0 = Small block NAND (XSB)
            // 1 = Small block NAND on BB controller (PSB/KSB)
            // 2 = Big block NAND on BB controller (PSB/KSB)
            blockType = identifylayout(sparedata);

            // extend the buffer to 69206016 bytes (64mb w/ ECC)
            Array.Resize(ref flashData, 69206016);

            // Step 2: fill it with 48mb worth of zero pages w/ valid ECC data
            // we don't reeeeeeeallly need a bunch of buffers for this but 
            // addeccv2 doesn't have a "start at this offset" option (yet)
            byte[] blankPages = new byte[0x18000 * 0x200];
            blankPages = addecc_v2(blankPages, true, 17301504, blockType);
            Buffer.BlockCopy(blankPages,0,flashData, 17301504, blankPages.Length);

            // Copy the SMC config to the new location
            // 64mb: 0x03dfc000 (0x3FEBE00 physical), len 0x400 (two logical pages)
            // 16mb: 0x00f7c000 (0xFF7E00 physical), len 0x400 (two logical pages)
            // logical page size: 0x200
            // physical page size: 0x210

            // Get the old SMC config bytes and re-add ECC data for the new loc
            byte[] smcConfigBytes = flashData.Skip(0xFF7E00).Take(0x420).ToArray();
            smcConfigBytes = unecc(smcConfigBytes);
            smcConfigBytes = addecc_v2(smcConfigBytes, true, 0x3FEBE00, blockType);
            //smcConfigBytes *should* be 0x420 now
            Buffer.BlockCopy(smcConfigBytes,0,flashData,0x3FEBE00, smcConfigBytes.Length);

            //zero out the old location
            byte[] blankSmcConfigPages = new byte[0x400];
            blankSmcConfigPages = addecc_v2(blankSmcConfigPages, true, 0xFF7E00, blockType);
            Buffer.BlockCopy(blankSmcConfigPages, 0, flashData, 0xFF7E00, blankSmcConfigPages.Length);

            File.WriteAllBytes(flashFileResultPath, flashData);

            Console.WriteLine("Done. Image written to " + flashFileResultPath);

            return flashFileResultPath;
        }

        public static bool doesNandContainVfuses(string flashFilePath)
        {
            byte[] cpukeyArr = { };
            return getVirtualCPUKey(flashFilePath, ref cpukeyArr);
        }

        public static bool getVirtualCPUKey(string flashFilePath, ref byte[] cpukey)
        {
            byte[] flashData = { };
            byte[] fuseline0 = { 0xC0, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            bool flashHasEcc = true;
            

            try
            {
                flashData = File.ReadAllBytes(flashFilePath);
            }
            catch
            {
                // Couldn't read image, we'll return to the caller
                // such that it prompts the user for a CPU key
                return true;
            }

            // Images with vfuses (other than JTAG) store them at the beginning of the patch slots
            // This is the same thing XeLL does when searching for the virtual CPU key
            int patchSlotOffset = BitConverter.ToInt32(flashData.Skip(0x64).Take(0x4).Reverse().ToArray(), 0);
            int patchSlotCount = BitConverter.ToInt16(flashData.Skip(0x68).Take(0x2).Reverse().ToArray(), 0);
            int patchSlotSize = BitConverter.ToInt32(flashData.Skip(0x70).Take(0x4).Reverse().ToArray(), 0);

            // Determine whether this image has ECC
            if (flashData.Length == 17301504 || flashData.Length == 69206016)
            {
                flashHasEcc = true;
            }
            else if (flashData.Length == 50331648)
            {
                // Flash data doesn't have ECC
                flashHasEcc = false;
            }
            else
            {
                // Invalid image type, we'll return to the caller
                // such that it prompts the user for a CPU key
                return true;
            }

            for (int i = 0; i < patchSlotCount; i++)
            {
                int patchSlotAddress = patchSlotOffset + (i * patchSlotSize);
                int patchSlotAddressPhys = 0;

                if (flashHasEcc)
                {
                    // Addresses stored in NAND are logical (no SPARE)
                    // Calculate the page number and offset in page so
                    // we can translate to a physical offset
                    // Logical page size = 0x200
                    // Physical page size = 0x210
                    int patchSlotPage = patchSlotAddress / 0x200;
                    int patchSlotOffsetInPage = patchSlotAddress % 0x200;
                    patchSlotAddressPhys = (patchSlotPage * 0x210) + patchSlotOffsetInPage;
                }
                else
                {
                    patchSlotAddressPhys = patchSlotAddress;
                }

                if (Oper.ByteArrayCompare(fuseline0, flashData.Skip(patchSlotAddressPhys).Take(0x8).ToArray(), 0x8))
                {
                    // ByteArrayCompare returns true if the buffers are equal
                    cpukey = flashData.Skip(patchSlotAddressPhys + 0x20).Take(0x10).ToArray();
                    return true;
                }
            }

            // If we didn't find virtual fuses in the regular locations, 
            // try the JTAG location (0x95000 logical, 0x99A80 physical)
            if (Oper.ByteArrayCompare(fuseline0, flashData.Skip(0x99A80).Take(0x8).ToArray(), 0x8))
            {
                // ByteArrayCompare returns true if the buffers are equal
                cpukey = flashData.Skip(0x99A80 + 0x20).Take(0x10).ToArray();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Messes with an RGH3 image so that it can boot old dashboards correctly.
        /// Tested all the way back to 1888 on the FFFFalcon
        /// </summary>
        /// <param name="flashFilePath">Path to the NAND image we want to patch</param>
        /// <param name="cpukey_phys">The physical CPU key of the machine (not the virtual CPU key!!!!)</param>
        public static void g3fix(string flashFilePath, byte[] cpukey_phys)
        {
            byte[] flashData = { };
            int blockType = 0;
            bool flashHasEcc = false;

            Console.WriteLine("g3fix Physical CPU Key: " + Oper.ByteArrayToString(cpukey_phys));

            // Read in the flash image
            try
            {
                flashData = File.ReadAllBytes(flashFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("g3fix error: couldn't read input flash image");
                if (variables.debugMode) Console.WriteLine(ex.ToString());
                return;
            }

            // Determine whether this image has ECC
            if (flashData.Length == 17301504 || flashData.Length == 69206016)
            {
                flashHasEcc = true;
            }
            else if (flashData.Length == 50331648)
            {
                // Flash data doesn't have ECC
                flashHasEcc = false;
            }
            else
            {
                Console.WriteLine("g3fix error: Invalid flash image size");
                return;
            }

            // If the flash has ECC data, determine the block type so ECC data can be recalculated
            if (flashHasEcc)
            {
                byte[] sparedata = flashData.Skip(0x4400).Take(0x10).ToArray();

                // Block Types
                // 0 = Small block NAND (XSB)
                // 1 = Small block NAND on BB controller (PSB/KSB)
                // 2 = Big block NAND on BB controller (PSB/KSB)
                blockType = identifylayout(sparedata);
            }

            // Take the first 0x21000 bytes (one block on 256/512mb BB machines)
            // It's more than enough to capture the CB_A, CB_X, and CB_B. We can
            // un-ecc it and won't need to monkey around with logical/physical
            // address calculations
            byte[] nandPatchPages = flashData.Take(0x21000).ToArray();

            if (flashHasEcc)
            {
                // remove the ECC so we can copy our patch data to the logical addresses
                nandPatchPages = unecc(nandPatchPages);
            }

            int cbaOffset = BitConverter.ToInt32(nandPatchPages.Skip(0x8).Take(4).Reverse().ToArray(), 0);
            int cbaSize = BitConverter.ToInt32(nandPatchPages.Skip(cbaOffset + 0xC).Take(4).Reverse().ToArray(), 0);
            byte[] cba_nonce = nandPatchPages.Skip(cbaOffset + 0x10).Take(0x10).ToArray();

            int cbxOffset = cbaOffset + cbaSize;
            int cbxSize = BitConverter.ToInt32(nandPatchPages.Skip(cbxOffset + 0xC).Take(4).Reverse().ToArray(), 0);
            byte[] cbx_nonce = nandPatchPages.Skip(cbxOffset + 0x10).Take(0x10).ToArray();

            int cbbOffset = cbxOffset + cbxSize;
            int cbbSize = BitConverter.ToInt32(nandPatchPages.Skip(cbbOffset + 0xC).Take(4).Reverse().ToArray(), 0);

            //byte[] cbb_nonce = nandPatchPages.Skip(cbbOffset + 0x10).Take(0x10).ToArray();
            byte[] cbb_dec = nandPatchPages.Skip(cbbOffset).Take(cbbSize).ToArray();

            int cbaVersion = BitConverter.ToInt16(nandPatchPages.Skip(cbaOffset + 2).Take(2).Reverse().ToArray(), 0);
            int cbxVersion = BitConverter.ToInt16(nandPatchPages.Skip(cbxOffset + 2).Take(2).Reverse().ToArray(), 0);
            int cbbVersion = BitConverter.ToInt16(nandPatchPages.Skip(cbbOffset + 2).Take(2).Reverse().ToArray(), 0);

            // Do some checking on the CB_A and CB_X we've decrypted.
            // RGH3 images that use CB_A 10918 and CB_X 15432 are what we're looking to patch
            if (cbaVersion != 10918 || cbxVersion != 15432)
            {
                Console.WriteLine("g3fix error: invalid bootloaders. Image is not RGH3, has already been g3fixed, or is corrupt.");
                Console.WriteLine("CB_A version: " + cbaVersion.ToString());
                Console.WriteLine("CB_X version: " + cbxVersion.ToString());
                return;
            }

            //
            // Step 1: prepare the new CB_A and patch it in to the NAND image
            //

            byte[] newcba = { };

            // The new CB_A is going to be 5772. It doesn't really matter,
            // since all the CB_As are pretty much the same, but g3fix.py
            // uses it and that seems to work so we'll do it here too.
            try
            {
                newcba = File.ReadAllBytes(Path.Combine(variables.rootfolder, "common\\CB\\CB_A.5772.bin"));
            }
            catch(Exception ex)
            {
                Console.WriteLine("g3fix error: couldn't read replacement CB_A");
                Console.WriteLine(ex.ToString());
                return;
            }

            if (newcba.Length > cbaSize)
            {
                Console.WriteLine("g3fix error: replacement CB_A is somehow larger than original CB_A");
                return;
            }

            // re-encrypt the cba and copy it to the flash image
            byte[] cba_key = { };
            newcba = encrypt_CB(newcba, cba_nonce, ref cba_key);
            Buffer.BlockCopy(newcba, 0 , nandPatchPages, cbaOffset, newcba.Length);

            //
            // Step 2: Load the pre-patched CB_X
            //
            // Credits to wurthless-elektroniks- this CB_X is based on
            // the "new" RGH3 ECCs. Old dashboards don't get along with
            // CB_X so we use the RGH3 V2 CB_X and patch it slightly
            //
            // 0x3C0: mov r4,r31 (avoid r31 being trashed by cbb_jump)
            //      : byte[] cbx_mov = { 0x7F, 0xE4, 0xFB, 0x78 };
            //
            // 0x3C4: b 0xB4 (jump to the CB_A "jump to CB_B" function)
            //      : byte[] cba_jump = { 0x48, 0x00, 0x00, 0xB4 };
            //
            byte[] newcbx = { };

            try
            {
                newcbx = File.ReadAllBytes(Path.Combine(variables.rootfolder, "common\\CB\\CB_X_g3fix.bin"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("g3fix error: couldn't read replacement CB_X");
                Console.WriteLine(ex.ToString());
                return;
            }

            // Set the nonce in the new CB_X
            Buffer.BlockCopy(cbx_nonce, 0, newcbx, 0x10, 0x10);

            // Re-encrypt the CB_X. CB_A doesn't have vfuses, so this
            // MUST be the *physical* CPU key if it differs on a glitch2m image
            // We're always going to use a CB_A that uses the "old" crypto scheme,
            // there's really no reason to use one of the newer CB_A binaries
            newcbx = encrypt_CB_cpukey(newcbx, cba_key, false, cpukey_phys);

            //
            // Step 3: Fiddle with the unencrypted CB_B
            //
            // SMC sum patching logic based on modern-loadfare:
            //
            // https://github.com/wurthless-elektroniks/modern-loadfare/blob/main/newcbpatcher.py
            // https://github.com/wurthless-elektroniks/modern-loadfare/blob/main/oldcbpatcher.py
            //

            // Need to pad the CB_B to make up the remaining space
            int bootBlockSize = cbaSize + cbxSize + cbbSize;
            byte[] newcbb = cbb_dec;
            Array.Resize(ref newcbb, bootBlockSize - (newcba.Length + newcbx.Length));
            
            // Set the new size of the CB_B in its header
            byte[] newcbbSizeBytes = BitConverter.GetBytes(newcbb.Length).Reverse().ToArray();
            Buffer.BlockCopy(newcbbSizeBytes, 0, newcbb, 0xC, 0x4);

            // Patch CB_B to branch past the SMC hash check
            // After RGH dropped, microsoft removed a lot of the POST codes
            // from the CB_B. To handle the code differences, there are two
            // different patterns and two different patches to apply depending
            // on which pattern is found in the CB_B
            byte?[] oldCbbSmcHashCheckPattern = new byte?[] {
                0x2F, 0x03, 0x00, 0x00,
                0x40, 0x9A, 0x00, 0x14,
                0x38, 0x80, 0x00, 0xA4
            };
            int oldCbbPatternSearchResult = Oper.ByteArrayFindPattern(cbb_dec, oldCbbSmcHashCheckPattern);

            byte?[] newCbbSmcHashCheckPattern = new byte?[] {
                0x48, null, null, null,
                0x2F, 0x03, 0x00, 0x00,
                0x40, 0x9A, 0x00, 0x08,
                0x00, 0x00, 0x00, 0x00
            };
            int newCbbPatternSearchResult = Oper.ByteArrayFindPattern(cbb_dec, newCbbSmcHashCheckPattern);

            if (variables.debugMode)
            {
                Console.WriteLine("SMC hash check pattern search results:");
                Console.WriteLine("Old CBB pattern: " + oldCbbPatternSearchResult.ToString("x"));
                Console.WriteLine("New CBB pattern: " + newCbbPatternSearchResult.ToString("x"));
            }

            if ( (-1 == oldCbbPatternSearchResult && -1 == newCbbPatternSearchResult ) ||
                 (-1 != oldCbbPatternSearchResult && -1 != newCbbPatternSearchResult) )
            {
                // Odd, either the hash check sequence wasn't found at all or it was
                // found with both the new and old style patterns. Skip this patch
                // because something has obviously gone wrong or the CB_B is prepatched
                Console.WriteLine("g3fix: Skipping CB_B SMC hash check patch");
            }
            else if (oldCbbPatternSearchResult != -1)
            {
                byte[] old_cbb_jump = { 0x48, 0x00, 0x00, 0x14 }; // b +0x14
                int oldPatchLocation = oldCbbPatternSearchResult + 0x4;
                Console.WriteLine("g3fix: patching old-style CB_B at location 0x" + oldPatchLocation.ToString("x"));
                Buffer.BlockCopy(old_cbb_jump, 0, newcbb, oldPatchLocation, 0x4);
            }
            else
            {
                byte[] new_cbb_jump = { 0x48, 0x00, 0x00, 0x08 }; // b +0x8
                int newPatchLocation = newCbbPatternSearchResult + 0xC;
                Console.WriteLine("g3fix: patching new-style CB_B at location 0x" + newPatchLocation.ToString("x"));
                Buffer.BlockCopy(new_cbb_jump, 0, newcbb, newPatchLocation, 0x4);
            }

            // Copy everything over to the NAND patch pages
            // Note: we DON'T need to encrypt the CB_B, that's
            // just the way the RGH3 boot chain works
            int newbootblkSize = newcba.Length + newcbx.Length + newcbb.Length;

            if (newbootblkSize != bootBlockSize)
            {
                Console.WriteLine("g3fix error: new boot block size not the same size as the old boot block!");
                return;
            }

            byte[] newbootblk = new byte[newbootblkSize];

            // Build the new CB_A/CB_X/CB_B block
            Buffer.BlockCopy(newcba, 0, newbootblk, 0, newcba.Length);
            Buffer.BlockCopy(newcbx, 0, newbootblk, newcba.Length, newcbx.Length);
            Buffer.BlockCopy(newcbb, 0, newbootblk, newcba.Length + newcbx.Length, newcbb.Length);

            // Copy it to the NAND image
            Buffer.BlockCopy(newbootblk, 0, nandPatchPages, cbaOffset, newbootblkSize);

            // Re-add ECC data and copy it over to the flash data buffer
            if (flashHasEcc)
            {
                nandPatchPages = addecc_v2(nandPatchPages, true, 0, blockType);
            }
            Buffer.BlockCopy(nandPatchPages, 0, flashData, 0, nandPatchPages.Length);

            try
            {
                // So we've updated the flashData, write it back to disk!
                File.WriteAllBytes(flashFilePath, flashData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("g3fix error: couldn't write modified flash image");
                if (variables.debugMode) Console.WriteLine(ex.ToString());
                return;
            }

            Console.WriteLine("g3fix: successfully replaced CB_A and CB_X");
            Console.WriteLine("");

            MainForm.mainForm.nand_init();
        }

        private static byte[] CalculateCPUKeyECD(byte[] key)
        {
            byte[] ecd = new byte[0x10];
            Buffer.BlockCopy(key, 0, ecd, 0, 0x10);

            uint acc1 = 0, acc2 = 0;
            for (var cnt = 0; cnt < 0x80; cnt++, acc1 >>= 1)
            {
                var bTmp = ecd[cnt >> 3];
                var dwTmp = (uint)((bTmp >> (cnt & 7)) & 1);
                if (cnt < 0x6A)
                {
                    acc1 = dwTmp ^ acc1;
                    if ((acc1 & 1) > 0)
                        acc1 = acc1 ^ 0x360325;
                    acc2 = dwTmp ^ acc2;
                }
                else if (cnt < 0x7F)
                {
                    if (dwTmp != (acc1 & 1))
                        ecd[(cnt >> 3)] = (byte)((1 << (cnt & 7)) ^ (bTmp & 0xFF));
                    acc2 = (acc1 & 1) ^ acc2;
                }
                else if (dwTmp != acc2)
                    ecd[0xF] = (byte)((0x80 ^ bTmp) & 0xFF);
            }
            return ecd;
        }

        public static byte[] keyZero = {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        public static bool VerifyKey(byte[] key)
        {
            if (key == null || key.Length != 0x10) return false;

            if (variables.allowZeroPaired)
            {
                if (key.SequenceEqual(keyZero)) return true; // Allow 0 paired key
            }

            int hamming = 0;
            byte[] hammingArray = new byte[13];
            Buffer.BlockCopy(key, 0, hammingArray, 0, 13);
            BitArray bitArray = new BitArray(hammingArray);

            foreach (bool s in bitArray) if (s) hamming++;
            if (key[13].getBit(0)) hamming++;
            if (key[13].getBit(1)) hamming++;

            if (hamming != 53) return false;
            byte[] key2 = CalculateCPUKeyECD(key);
            if (!Oper.ByteArrayCompare(key, key2)) return false;
            else return true;
        }


        #region secdata

        public static void DecryptSecData(byte[] secdata, byte[] cpukey)
        {
            if (secdata == null) return;
            byte[] decrypted = decryptkv(secdata, cpukey);
            byte[] decrypted_data = Oper.returnportion(decrypted, 0x28, 8);

            Console.WriteLine("Security Activated: {0}", decrypted[0x18]);
            Console.WriteLine("CF/CG LDV: {0}", decrypted[0x19]);
            Console.WriteLine("Filetime: {0}", Oper.ByteArrayToString(Oper.returnportion(decrypted, 0x20, 8)));
            Console.WriteLine("Security Detected: {0}", Oper.ByteArrayToString(decrypted_data));
            byte[] array = Oper.returnportion(decrypted, 0x40, 8);
            if (BitConverter.IsLittleEndian) Array.Reverse(array);
            Console.WriteLine("Lock System Update Counter: {0}", BitConverter.ToInt64(array, 0));
            array = Oper.returnportion(decrypted, 0x30, 8);
            if (BitConverter.IsLittleEndian) Array.Reverse(array);
            Console.WriteLine("Security Activated: {0}", BitConverter.ToInt64(array, 0));
            array = Oper.returnportion(decrypted, 0x38, 8);
            if (BitConverter.IsLittleEndian) Array.Reverse(array);
            Console.WriteLine("No DVD Connected Counter: {0}", BitConverter.ToInt64(array, 0));
            DisplayResults(decrypted_data);
        }

        public static void DecryptXVal(string console_serial, string console_xval)
        {
            byte[] xval = null;
            xval = Oper.StringToByteArray(console_xval.Replace("-", ""));
            if (xval == null) return;
            if (console_serial.Length != 0xC) return;

            HMACSHA1 hmac = new HMACSHA1(Encoding.ASCII.GetBytes(console_serial));
            hmac.Initialize();
            byte[] des_key = Oper.returnportion(hmac.ComputeHash(Encoding.ASCII.GetBytes("XBOX360SSB")), 0, 8);
            Console.WriteLine("{0}", Oper.ByteArrayToString(des_key));

            if (des_key.Length != 8) return;
            if (xval.Length != 8) return;

            byte[] decrypted_data = DecryptDES(xval, des_key);
            Console.WriteLine("Data: {0}", Oper.ByteArrayToString(decrypted_data));
            DisplayResults(decrypted_data);
        }

        public static byte[] DecryptDES(byte[] clearData, byte[] key)
        {
            DES desDecrypt = new DESCryptoServiceProvider();
            desDecrypt.Mode = CipherMode.ECB;
            desDecrypt.Key = key;
            desDecrypt.Padding = PaddingMode.None;
            ICryptoTransform transForm = desDecrypt.CreateDecryptor();
            MemoryStream decryptedStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(decryptedStream, transForm, CryptoStreamMode.Write);
            cryptoStream.Write(clearData, 0, clearData.Length);
            byte[] encryptedData = decryptedStream.ToArray();
            return encryptedData;
        }


        private static void DisplayResults(byte[] xval)
        {
            //int FLAG_SSB_NONE = 0x0000;
            int FLAG_SSB_AUTH_EX_FAILURE = 0x0001;
            int FLAG_SSB_AUTH_EX_NO_TABLE = 0x0002;
            int FLAG_SSB_AUTH_EX_RESERVED = 0x0004;
            int FLAG_SSB_INVALID_DVD_GEOMETRY = 0x0008;
            int FLAG_SSB_INVALID_DVD_DMI = 0x0010;
            int FLAG_SSB_DVD_KEYVAULT_PAIR_MISMATCH = 0x0020;
            int FLAG_SSB_CRL_DATA_INVALID = 0x0040;
            int FLAG_SSB_CRL_CERTIFICATE_REVOKED = 0x0080;
            int FLAG_SSB_UNAUTHORIZED_INSTALL = 0x0100;
            int FLAG_SSB_KEYVAULT_POLICY_VIOLATION = 0x0200;
            int FLAG_SSB_CONSOLE_BANNED = 0x0400;
            int FLAG_SSB_ODD_VIOLATION = 0x0800;

            int xval_h = BitConverter.ToInt32(xval, 0);
            int xval_l = BitConverter.ToInt32(xval, 4);
            if (xval_h == 0 && xval_l == 0) Console.WriteLine("SecData is clean");
            else if (xval_h == 0xFFFF && xval_l == 0xFFFF) Console.WriteLine("Secdata is invalid");
            else if (xval_h != 0 && xval_l != 0) Console.WriteLine("SecData decryption error");
            else
            {
                if ((xval_l & FLAG_SSB_AUTH_EX_FAILURE) != 0)
                    Console.WriteLine("AuthEx Challenge Failure");
                if ((xval_l & FLAG_SSB_AUTH_EX_NO_TABLE) != 0)
                    Console.WriteLine("AuthEx Table missing");
                if ((xval_l & FLAG_SSB_AUTH_EX_RESERVED) != 0)
                    Console.WriteLine("AuthEx Reserved Flag");
                if ((xval_l & FLAG_SSB_INVALID_DVD_GEOMETRY) != 0)
                    Console.WriteLine("Invalid DVD Geometry");
                if ((xval_l & FLAG_SSB_INVALID_DVD_DMI) != 0)
                    Console.WriteLine("Invalid DVD DMI");
                if ((xval_l & FLAG_SSB_DVD_KEYVAULT_PAIR_MISMATCH) != 0)
                    Console.WriteLine("DVD Keyvault Pair Mismatch");
                if ((xval_l & FLAG_SSB_CRL_DATA_INVALID) != 0)
                    Console.WriteLine("Invalid CRL Data");
                if ((xval_l & FLAG_SSB_CRL_CERTIFICATE_REVOKED) != 0)
                    Console.WriteLine("CRL Certificate Revoked");
                if ((xval_l & FLAG_SSB_UNAUTHORIZED_INSTALL) != 0)
                    Console.WriteLine("Unauthorized Install");
                if ((xval_l & FLAG_SSB_KEYVAULT_POLICY_VIOLATION) != 0)
                    Console.WriteLine("Keyvault Policy Violation");
                if ((xval_l & FLAG_SSB_CONSOLE_BANNED) != 0)
                    Console.WriteLine("Console Banned");
                if ((xval_l & FLAG_SSB_ODD_VIOLATION) != 0)
                    Console.WriteLine("ODD Violation");
                if ((xval_l & 0xFFFFF000) != 0)
                    Console.WriteLine("Unknown Violation(s)");
            }
            Console.WriteLine("");
            return;
        }

        #endregion

        /// <summary>
        /// patch, encrypt, decrypt SMC
        /// </summary>
        /// <param name="SMC"></param>
        /// <returns></returns>
        #region SMC

        public static bool checkifhackedSMC(byte[] SMC)
        {
            if (!IndexOfSequence(SMC, Encoding.ASCII.GetBytes("Microsoft"), 0, 0x150))
            {
                if (variables.debugMode) Console.WriteLine("decrypting smc");
                SMC = Nand.decrypt_SMC(SMC);
            }
            if (Oper.allsame(Oper.returnportion(SMC, 0x2db0, 0x10), 0x00)) return false;
            else return true;
        }
        public static bool IndexOfSequence(byte[] buffer, byte[] pattern, int startIndex, int endIndex = 0)
        {
            int i = Array.IndexOf<byte>(buffer, pattern[0], startIndex);
            while (i >= 0 && i <= buffer.Length - pattern.Length)
            {
                if (i >= endIndex) return false;
                byte[] segment = new byte[pattern.Length];
                Buffer.BlockCopy(buffer, i, segment, 0, pattern.Length);
                if (segment.SequenceEqual<byte>(pattern)) return true;
                i = Array.IndexOf<byte>(buffer, pattern[0], i + pattern.Length);
            }
            return false;
        }

        public static byte[] patch_SMC(byte[] SMC)
        {
            string[] console_types = { "none/unk", "Xenon", "Zephyr", "Falcon", "Jasper", "Trinity", "Corona", "Winchester" };
            bool found = false;
            int smctype = (SMC[0x100] >> 4) & 0xF;

            for (int i = 0; i < SMC.Length - 8; i++)
            {
                if (SMC[i] == 0x05)
                {
                    if ((SMC[i + 2] == 0xE5) && (SMC[i + 4] == 0xb4) && (SMC[i + 5] == 0x05))
                    {
                        found = true;
                        Console.WriteLine("Patching {0} version {1}.{2} SMC at offset 0x{3:X}", console_types[smctype], SMC[0x101], SMC[0x102], i);
                        SMC[i] = 0x00; SMC[i + 1] = 0x00;
                    }
                }
            }
            if (!found)
            {
                Console.WriteLine(" ! Warning: can't patch this {0} type SMC!", console_types[smctype]);
            }
            return SMC;
        }

        public static byte[] decrypt_SMC(byte[] SMC)
        {

            byte[] Key = { 0x42, 0x75, 0x4e, 0x79 };
            int[] Keys = { 0x42, 0x75, 0x4E, 0x79 };
            int i = 0;
            int mod;
            byte[] res = new byte[SMC.Length];
            for (i = 0; i < SMC.Length; i++)
            {
                mod = (SMC[i] * 0xFB);
                res[i] = (byte)(SMC[i] ^ (Keys[i & 3] & 0xFF));
                Keys[(i + 1) & 3] += mod;
                Keys[(i + 2) & 3] += (mod >> 8);
            }
            return res;
        }

        public static byte[] encrypt_SMC(byte[] SMC)
        {

            byte[] Key = { 0x42, 0x75, 0x4e, 0x79 };
            int[] Keys = { 0x42, 0x75, 0x4E, 0x79 };
            int i = 0;
            int mod;
            byte[] res = new byte[SMC.Length];
            for (i = 0; i < SMC.Length; i++)
            {
                mod = (SMC[i] ^ (Keys[i & 3] & 0xFF)) * 0xFB;
                res[i] = (byte)(SMC[i] ^ (Keys[i & 3] & 0xFF));
                Keys[(i + 1) & 3] += mod;
                Keys[(i + 2) & 3] += (mod >> 8);
            }
            return res;
        }

        public static bool checkifhacked(string filename)
        {
            byte[] SMC;
            long size = 0;
            byte[] image = Oper.openfile(filename, ref size, 40 * 1024);
            int counter;
            if (image[0x205] == 0xFF || image[0x415] == 0xFF || image[0x200] == 0xFF)
            {
                byte[] res = { };
                for (counter = 0; counter + 496 < image.Length; counter += 0x210)
                {
                    res = Oper.concatByteArrays(res, Oper.returnportion(image, counter, 0x200), res.Length, 0x200);
                }
                image = res;
                res = null;
            }
            SMC = Oper.returnportion(image, Oper.ByteArrayToInt(Oper.returnportion(image, 0x7C, 4)), 0x4000 - Oper.ByteArrayToInt(Oper.returnportion(image, 0x7C, 4)));
            SMC = decrypt_SMC(SMC);
            if (Oper.allsame(Oper.returnportion(SMC, 0x2db0, 0x10), 0x00)) { Console.WriteLine("Clean SMC detected"); return false; }
            else { Console.WriteLine("Hacked SMC detected"); return true; }
        }

        #endregion

        #region Nand FileSystem

        public static void extractfcrt(string filename, string outputfolder, string cpukey = "")
        {
            long size = 0;
            byte[] searched = Oper.openfile(filename, ref size, 0x4200000);
            System.Text.ASCIIEncoding ASCII = new System.Text.ASCIIEncoding();
            byte[] find = ASCII.GetBytes("fcrt.bin");
            int start = 0;
            int found = -1;
            bool matched = false;
            bool corona = false;
            int block_length = 0x4200;
            if (hasecc(searched)) corona = false;
            else corona = true;
            //only look at this if we have a populated search array and search bytes with a sensible start
            if (searched.Length > 0 && find.Length > 0 && start <= (searched.Length - find.Length) && searched.Length >= find.Length)
            {
                //iterate through the array to be searched
                for (int i = start; i <= searched.Length - find.Length; i++)
                {
                    //if the start bytes match we will start comparing all other bytes
                    if (searched[i] == find[0])
                    {
                        if (searched.Length > 1)
                        {
                            //multiple bytes to be searched we have to compare byte by byte
                            matched = true;
                            for (int y = 1; y <= find.Length - 1; y++)
                            {
                                if (searched[i + y] != find[y])
                                {
                                    matched = false;
                                    break;
                                }
                            }
                            //everything matched up
                            if (matched)
                            {
                                found = i;
                                break;
                            }

                        }
                        else
                        {
                            //search byte is only one bit nothing else to do
                            found = i;
                            break; //stop the loop
                        }

                    }
                }

            }
            if (found == -1) { Console.WriteLine("No fcrt.bin was found"); return; }
            if (corona) block_length = 0x4000;
            int fcrt_offset = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(searched, found + 0x10, 8)), 16) * block_length;
            int fcrt_length = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(searched, found + 0x18, 4)), 16);
            if (variables.debugMode) Console.WriteLine("Offset: {0:X} - Length {1:X} - corona: {2}", fcrt_offset, fcrt_length, corona);
            if (corona)
            {
                byte[] res = new byte[fcrt_length];
                Buffer.BlockCopy(searched, fcrt_offset, res, 0, fcrt_length);
                searched = res;
            }
            else
            {
                #region unecc
                int counter;
                byte[] res = { };
                for (counter = fcrt_offset; counter < fcrt_offset + fcrt_length + 0x200; counter += 0x210)
                {
                    res = Oper.concatByteArrays(res, Oper.returnportion(searched, counter, 0x200), res.Length, 0x200);
                }
                //res = concatByteArrays(res, returnportion(searched, counter, 496), res.Length, 496);
                searched = res;
                #endregion
            }
            Oper.savefile(searched, Path.Combine(outputfolder, "fcrt_enc.bin"));
            Console.WriteLine("fcrt.bin extracted successfully");
            if (!string.IsNullOrEmpty(cpukey)) decrypt_fcrt(searched, Oper.StringToByteArray(cpukey));
        }

        public static byte[] getsecdata(string filename)
        {
            long size = 0;
            int counter = 1;
            byte[] searched = Oper.openfile(filename, ref size, 0);
            System.Text.ASCIIEncoding ASCII = new System.Text.ASCIIEncoding();
            byte[] find = ASCII.GetBytes("secdata.bin");
            int start = 0;
            int found = -1;
            bool matched = false;
            //only look at this if we have a populated search array and search bytes with a sensible start
            if (searched.Length > 0 && find.Length > 0 && start <= (searched.Length - find.Length) && searched.Length >= find.Length)
            {
                //iterate through the array to be searched
                for (int i = start; i <= searched.Length - find.Length; i++)
                {
                    //if the start bytes match we will start comparing all other bytes
                    if (searched[i] == find[0])
                    {
                        //multiple bytes to be searched we have to compare byte by byte
                        matched = true;
                        for (int y = 1; y <= find.Length - 1; y++)
                        {
                            if (searched[i + y] != find[y])
                            {
                                matched = false;
                                break;
                            }
                        }
                        //everything matched up
                        if (matched)
                        {
                            found = i;
                            Console.WriteLine("0x{0:X}  -  0x{1:X}  -  0x{2:X}  -  0x{3:X}", found, found / 0x4200, (found / 0x4200 - counter) * 0x4200, Oper.ByteArrayToString(Oper.returnportion(searched, found + 0x45, 1)));
                            counter++;
                        }
                    }
                }

            }
            if (found == -1) { Console.WriteLine("No secdata.bin was found"); return null; }
            int secdata_offset = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(searched, found + 0x10, 8)), 16) * 0x4200;
            int secdata_length = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(searched, found + 0x18, 4)), 16);
            if (variables.debugMode) Console.WriteLine("Offset: {0:X} - Length {1:X}", secdata_offset, secdata_length);
            #region unecc
            int counter1;
            byte[] res = { };
            for (counter1 = secdata_offset; counter1 < secdata_offset + secdata_length; counter1 += 0x210)
            {
                res = Oper.concatByteArrays(res, Oper.returnportion(searched, counter1, 0x200), res.Length, 0x200);
            }
            //res = concatByteArrays(res, returnportion(searched, counter, 496), res.Length, 496);
            searched = res;
            #endregion
            Oper.savefile(searched, "sec.bin");
            return searched;
        }

        public static void findfs()
        {
            Console.WriteLine("Started");
            long size = 0;
            byte[] image = Oper.openfile(variables.filename1, ref size, 0);
            List<int> fs = new List<int>();
            List<string> filenames = new List<string>();
            byte[] fsSequence = new byte[4];
            byte blocktype;
            int position;


            int blocksize = 0x4200;
            int fullsize = 0x3ff;
            int pagesize = 0x210;

            int newfilesystem = 0, currentfs = 0;
            for (int i = 0; i < fullsize; i++)
            {
                bool badblock = false;
                for (int j = 0; j < 0x20; j++)
                {
                    position = (blocksize * i) + 0x200 + (j * pagesize);
                    fsSequence[0] = image[position + 0];
                    fsSequence[1] = image[position + 3];
                    fsSequence[2] = image[position + 4];
                    fsSequence[3] = image[position + 6];
                    blocktype = image[position + 0xC];
                    if (image[position + 5] != 0xFF)
                    {
                        if (!badblock)
                        {
                            Console.WriteLine("Bad Block {0:X}:{1:X}-{2:X}", i, j, position - 0x200);
                            badblock = true;
                        }
                    }
                    int fsseq = (fsSequence[2] << 16) + (fsSequence[1] << 8) + fsSequence[0];
                    if (fsseq != 0 && ((blocktype & 0x3F) == 0x30 || (blocktype & 0x3F) == 0x2C))
                    {
                        Console.WriteLine("* Found filesystem Version {3:X}  - {0:X}:{1:X}-{2:X}", i, j, position - 0x200, fsseq);
                        fs.Add(i);
                        if (fsseq > newfilesystem)
                        {
                            newfilesystem = fsseq;
                            currentfs = i;
                        }
                        break;
                    }
                }
            }
            Console.WriteLine("Current filesystem is Block 0x{0:X} - Version 0x{1:X}", currentfs, newfilesystem);
            //Console.WriteLine("{0:X}", cr);
            int startpage = (currentfs * 0x4200) / 0x210;
            List<int> blockMapPages = new List<int>();
            List<int> fileNamePages = new List<int>();
            for (int i = 0; i < 0x20; i++)
                if (i % 2 == 0)
                    blockMapPages.Add(startpage + i);
                else
                    fileNamePages.Add(startpage + i);
            bool breakk = false;
            foreach (int page in fileNamePages)
            {
                if (breakk)
                    break;
                //Console.WriteLine("{0:X}",page * 0x210);
                int entrycount = 0x20;
                for (int i = 0; i < entrycount; i += 2)
                {
                    //Console.WriteLine("{0:X}", (page * 0x210) + (i * 0x10));
                    string filename = ascii.GetString(Oper.returnportion(image, (page * 0x210) + (i * 0x10), 0x16)).Trim('\0');
                    if (string.IsNullOrEmpty(filename))
                    {
                        breakk = true;
                        break;
                    }
                    //Console.WriteLine("{0}", filename);
                    if (filename == "fcrt.bin" || filename == "secdata.bin" || true)
                    {
                        try
                        {
                            int length = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(image, (page * 0x210) + (i * 0x10) + 0x18, 0x4)), 16);
                            int block = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(image, (page * 0x210) + (i * 0x10) + 0x16, 0x2)), 16);
                            Console.WriteLine("{0} - {1:X} - {2:X} - {3:X}", filename, currentfs, block, length);
                            Oper.savefile(Oper.returnportion_ecc(image, block * 0x4200, length), filename);
                        }
                        catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
                    }
                    if (image[(page * 0x210) + (i * 0x10)] != 0x05) { /*if (!filenames.Contains(filename)) */ filenames.Add(filename); }
                    //Console.WriteLine("{0:X}", page + (0x10 * i) + 0x20F);
                }
            }
            Console.WriteLine("");
            foreach (string fl in filenames)
            {
                Console.WriteLine(fl);
            }
            Console.WriteLine("Finished");
        }
        public static void findfsBB()
        {
            Console.WriteLine("Started");
            long size = 0;
            byte[] image = Oper.openfile(variables.filename1, ref size, 0);
            List<int> fs = new List<int>();
            List<string> filenames = new List<string>();
            byte[] fsSequence = new byte[4];
            byte blocktype;
            int position;


            int blocksize = 0x21000;
            int fullsize = 0x200;
            int pagesize = 0x210;

            int newfilesystem = 0, currentfs = 0;
            for (int i = 0; i < fullsize; i++)
            {
                for (int j = 0; j < 0x40; j++)
                {
                    position = (blocksize * i) + (4 * j * pagesize) + (pagesize - 0x10);
                    //Console.WriteLine("{0:X}", position);
                    //fsSequence[0] = image[position + 0];
                    fsSequence[1] = image[position + 3];
                    fsSequence[2] = image[position + 4];
                    fsSequence[3] = image[position + 5];
                    blocktype = image[position + 0xC];

                    int fsseq = (fsSequence[2] << 8) | (fsSequence[3]);
                    if (fsseq != 0 && ((blocktype & 0x3F) == 0x2C))
                    {
                        Console.WriteLine("* Found filesystem Version {3:X}  - {0:X}:{1:X}-{2:X}", i, j, position - (pagesize - 0x10), fsseq);
                        fs.Add(i);
                        if (fsseq > newfilesystem)
                        {
                            newfilesystem = fsseq;
                            currentfs = i;
                        }
                        break;
                    }
                }
            }
            Console.WriteLine("Current filesystem is Block 0x{0:X} - Version 0x{1:X}", currentfs, newfilesystem);
            //Console.WriteLine("{0:X}", cr);

            int startpage = (currentfs * blocksize) / pagesize;
            List<int> blockMapPages = new List<int>();
            List<int> fileNamePages = new List<int>();
            for (int i = 0; i < 0x20; i++)
                if (i % 2 == 0)
                    blockMapPages.Add(startpage + i);
                else
                    fileNamePages.Add(startpage + i);
            bool breakk = false;
            foreach (int page in fileNamePages)
            {
                if (breakk)
                    break;
                //Console.WriteLine("{0:X}",page * 0x210);
                int entrycount = 0x20;
                for (int i = 0; i < entrycount; i += 2)
                {
                    //Console.WriteLine("{0:X}", (page * 0x210) + (i * 0x10));
                    string filename = ascii.GetString(Oper.returnportion(image, (page * pagesize) + (i * 0x10), 0x16)).Trim('\0');
                    if (string.IsNullOrEmpty(filename))
                    {
                        breakk = true;
                        break;
                    }
                    Console.WriteLine("{0}", filename);
                    if (filename == "fcrt.bin" || filename == "secdata.bin" || true)
                    {
                        try
                        {
                            int length = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(image, (page * pagesize) + (i * 0x10) + 0x18, 0x4)), 16);
                            int block = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(image, (page * pagesize) + (i * 0x10) + 0x16, 0x2)), 16);
                            Console.WriteLine("{0} - {1:X} - {2:X} - {3:X}", filename, currentfs, block, length);
                            Oper.savefile(Oper.returnportion_ecc(image, block * blocksize, length), filename);
                        }
                        catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
                    }
                    if (image[(page * 0x210) + (i * 0x10)] != 0x05) { /*if (!filenames.Contains(filename)) */ filenames.Add(filename); }
                    //Console.WriteLine("{0:X}", page + (0x10 * i) + 0x20F);
                }
            }
            Console.WriteLine("");
            foreach (string fl in filenames)
            {
                Console.WriteLine(fl);
            }
            Console.WriteLine("Finished");
        }
        #endregion

        #region sparedata

        public static void rawtoimage(string oldfile, string newfile)
        {
            if (!File.Exists(oldfile)) return;
            FileStream fs = new FileStream(oldfile, FileMode.Open);
            FileStream fw = new FileStream(newfile, FileMode.Create);
            byte[] page = new byte[0x21000];
            //byte[] towrite = new byte[0x840];
            int i = 0;
            while (i < fs.Length)
            {
                fs.Read(page, 0, page.Length);

                sparedatatonormal(ref page);
                /*
                for (int j = 0; j < 4; j++)
                {
                    Buffer.BlockCopy(page, j * 0x200, towrite, j * 0x210, 0x200);
                    Buffer.BlockCopy(page, 0x800 + (j * 0x10), towrite, ((j + 1) * 0x210) - 0x10 , 0x10);
                }
                fw.Write(towrite, 0, towrite.Length);
                */
                fw.Write(page, 0, page.Length);
                i += page.Length;
            }

            fs.Close();
            fw.Close();
        }
        public static void imagetoraw(string oldfile, string newfile)
        {
            if (!File.Exists(oldfile)) return;
            FileStream fs = new FileStream(oldfile, FileMode.Open);
            FileStream fw = new FileStream(newfile, FileMode.Create);
            byte[] page = new byte[0x840];
            //byte[] towrite = new byte[0x840];
            int i = 0;
            while (i < fs.Length)
            {
                fs.Read(page, 0, page.Length);
                /*
                for (int j = 0; j < 4; j++)
                {
                    byte[] data = new byte[0x210];
                    Buffer.BlockCopy(page, j * 0x210, data, 0, 0x200);
                    byte[] sparedata = new byte[0x10];
                    Buffer.BlockCopy(page, ((j + 1) * 0x210) - 0x10, sparedata, 0, 0x10);

                    if (!Oper.allsame(sparedata, 0x00) &&
                        !Oper.allsame(sparedata, 0xFF))
                    {
                        data = addecc_v2(data, true, i, true);
                    }
                    if (data.Length != 0x210) continue;

                    if (Oper.allsame(sparedata, 0xFF))
                    {
                        sparedata.Fill(0xFF);
                        Buffer.BlockCopy(sparedata, 0, data, 0x200, 0x10);
                    }
                    Buffer.BlockCopy(data, 0, towrite, j * 0x200, 0x200);
                    Buffer.BlockCopy(data, 0x200, towrite, 0x800 + (j * 0x10), 0x10);
                }
                fw.Write(towrite, 0, towrite.Length);
                */
                sparedatatoraw(ref page);
                fw.Write(page, 0, page.Length);

                i += page.Length;
            }
            fs.Close();
            fw.Close();
        }
        public static void imagetoraw1(string oldfile, string newfile)
        {
            if (!File.Exists(oldfile)) return;
            FileStream fs = new FileStream(oldfile, FileMode.Open);
            FileStream fw = new FileStream(newfile, FileMode.Create);
            byte[] page = new byte[0x840];
            byte[] towrite = new byte[0x840];
            int i = 0;
            while (i < fs.Length)
            {
                fs.Read(page, 0, page.Length);
                for (int j = 0; j < 4; j++)
                {
                    byte[] data = new byte[0x210];
                    Buffer.BlockCopy(page, j * 0x210, data, 0, 0x200);
                    byte[] sparedata = new byte[0x10];
                    Buffer.BlockCopy(page, ((j + 1) * 0x210) - 0x10, sparedata, 0, 0x10);

                    if (!Oper.allsame(sparedata, 0x00) &&
                        !Oper.allsame(sparedata, 0xFF))
                    {
                        data = addecc_v2(data, true, i, 2);
                    }
                    if (data.Length != 0x210) continue;

                    if (Oper.allsame(sparedata, 0xFF))
                    {
                        sparedata.Fill(0xFF);
                        Buffer.BlockCopy(sparedata, 0, data, 0x200, 0x10);
                    }
                    Buffer.BlockCopy(data, 0, towrite, j * 0x200, 0x200);
                    Buffer.BlockCopy(data, 0x200, towrite, 0x800 + (j * 0x10), 0x10);
                }
                fw.Write(towrite, 0, towrite.Length);
                i += 0x840;
            }
            fs.Close();
            fw.Close();
        }

        public static void sparedatatoraw(ref byte[] data)
        {
            try
            {
                int i = 0;
                byte[] page = new byte[0x840];

                while (i < data.Length)
                {
                    page = new byte[0x840];
                    for (int j = 0; j < 4; j++)
                    {
                        Buffer.BlockCopy(data, i + (j * 0x210), page, j * 0x200, 0x200);
                        Buffer.BlockCopy(data, i + (((j + 1) * 0x210) - 0x10), page, 0x800 + (j * 0x10), 0x10);
                    }
                    Buffer.BlockCopy(page, 0, data, i, 0x840);

                    i += 0x840;
                }
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
            return;
        }
        public static void sparedatatonormal(ref byte[] data)
        {
            try
            {
                int i = 0;
                byte[] page = new byte[0x840];
                byte[] sparedata = new byte[0x40];

                while (i < data.Length)
                {
                    page = new byte[0x840];
                    sparedata = new byte[0x40];
                    Buffer.BlockCopy(data, i + 0x800, sparedata, 0, 0x40);

                    for (int j = 0; j < 4; j++)
                    {
                        Buffer.BlockCopy(data, i + (j * 0x200), page, j * 0x210, 0x200);
                        Buffer.BlockCopy(sparedata, j * 0x10, page, (((j + 1) * 0x210) - 0x10), 0x10);
                    }
                    Buffer.BlockCopy(page, 0, data, i, 0x840);

                    i += 0x840;
                }
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
            return;
        }

        public static int identifylayout(byte[] sparedata)
        {
            if (sparedata[5] == 0x00 && sparedata[0] == 0xFF)
            {
                return 2;
            }
            else if (sparedata[5] == 0xFF)
            {
                if (sparedata[0] == 0x00) return 1;
                else return 0;
            }
            return 1;
        }

        public static byte[] addecc(byte[] image, bool bigblock, ref ProgressBar pb)
        {
            if (variables.extractfiles) Oper.savefile(image, "ecc.bin");
            int datalen = image.Length;
            byte[] d = new byte[0x200], result = new byte[(image.Length / 0x200) * 0x210];
            byte[] tempbyte = { 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            for (int i = 0; i < datalen / 0x200; i++)
            {
                //pb.Value = (pb.Maximum / 2) + ((i * 50) / (datalen / 0x200));

                d = new byte[0x200];
                Buffer.BlockCopy(image, i * 0x200, d, 0, 0x200);

                byte[] blockarray = { 0x00, 0x00, 0x00, 0x00 };
                if (bigblock == true)
                {
                    blockarray[0] = 0xFF;
                    blockarray[1] = (byte)(i / 256);
                }
                else
                {
                    blockarray[1] = (byte)((i / 32) - ((i / 32) / 0x100));
                    blockarray[2] = (byte)((i / 32) / 0x100);
                }
                if (blockarray.Length < 4)
                {
                    Array.Reverse(blockarray);
                    blockarray = Oper.padto(blockarray, 0x00, 4);
                }
                //Console.Write("\r{0}%     ", ((((i * 100) / (datalen / 0x200)))));
                d = Oper.addtoflash_v2(d, Oper.addtoflash_v2(blockarray, tempbyte));
                try
                {
                    d = calcecc(d);
                }
                catch (System.IndexOutOfRangeException) { Oper.ByteArrayToString(d); }
                Buffer.BlockCopy(d, 0, result, i * 0x210, 0x210);

            }
            //Console.WriteLine("\r100%");
            return result;
        }
        public static byte[] addecc_v2(byte[] image,bool addecc,int blockstart,int layout)
        {
            return addecc_v2_internal(image, addecc, blockstart, layout, null);
        }

        public static byte[] addecc_v2(byte[] image,bool addecc,int blockstart,int layout,IProgress<int> progress)
        {
            return addecc_v2_internal(image, addecc, blockstart, layout, progress);
        }
        public static byte[] addecc_v2_internal(byte[] image, bool addecc, int blockstart, int layout, IProgress<int> progress)
        {
            if (variables.extractfiles)
                Oper.savefile(image, "test.bin");

            if (variables.debugMode)
                Console.WriteLine("blockstart: {0:X}, layout: {1}", blockstart / 0x4200, layout);

            if (!addecc)
            {
                if (hasecc(image))
                    unecc(ref image);
            }

            int pageSize = 0x200;
            int spareSize = 0x10;
            int pageWithSpareSize = 0x210;

            int totalPages = (int)Math.Ceiling(image.Length / (double)pageSize);
            byte[] result = new byte[totalPages * pageWithSpareSize];

            byte[] sparedata = new byte[spareSize];
            int blockNumberBase = blockstart / 0x4200;

            int readOffset = 0;
            int writeOffset = 0;

            for (int i = 0; i < totalPages; i++)
            {
                // extract next 0x200 bytes
                byte[] dataBlock;
                int bytesRemaining = image.Length - readOffset;

                if (bytesRemaining > 0)
                {
                    int bytesToCopy = Math.Min(pageSize, bytesRemaining);
                    dataBlock = Oper.padto(
                        Oper.returnportion(ref image, readOffset, bytesToCopy),
                        0x00,
                        pageSize
                    );
                    readOffset += bytesToCopy;
                }
                else
                {
                    dataBlock = Oper.padto(new byte[0], 0x00, pageSize);
                }

                Array.Clear(sparedata, 0, spareSize);

                switch (layout)
                {
                    case 0:
                        sparedata[5] = 0xFF;
                        sparedata[0] = (byte)(((i / 32) + blockNumberBase) & 0xFF);
                        sparedata[1] = (byte)(((i / 32) + blockNumberBase) / 0x100);
                        break;
                    case 1:
                        sparedata[5] = 0xFF;
                        sparedata[1] = (byte)(((i / 32) + blockNumberBase) & 0xFF);
                        sparedata[2] = (byte)(((i / 32) + blockNumberBase) / 0x100);
                        break;
                    case 2:
                        sparedata[0] = 0xFF;
                        sparedata[1] = (byte)(((i / 0x100) + (blockstart / 0x21000)) & 0xFF);
                        sparedata[2] = (byte)((((i / 0x100) + (blockstart / 0x21000)) & 0xFF00) >> 8);
                        break;
                }

                // Combine data + spare
                byte[] pagePlusSpare = new byte[pageWithSpareSize];
                Buffer.BlockCopy(dataBlock, 0, pagePlusSpare, 0, pageSize);
                Buffer.BlockCopy(sparedata, 0, pagePlusSpare, pageSize, spareSize);

                // ECC
                byte[] pageWithECC;
                try
                {
                    pageWithECC = calcecc(pagePlusSpare);
                }
                catch (IndexOutOfRangeException)
                {
                    Oper.ByteArrayToString(pagePlusSpare);
                    throw;
                }

                Buffer.BlockCopy(pageWithECC, 0, result, writeOffset, pageWithECC.Length);
                writeOffset += pageWithECC.Length;
            }

            return result;
        }


        private static byte[] calcecc(byte[] data)
        {
            if (data.Length != 0x210) Console.WriteLine("Bad data length");
            int val = 0;
            int i = 0;
            int v = 0;
            for (i = 0; i < 0x1066; i++)
            {
                if ((i & 31) == 0)
                {
                    byte[] tempbyte = Oper.returnportion(data, i / 8, 4);
                    v = ~BitConverter.ToInt32(tempbyte, 0);
                }
                val ^= v & 1;
                v >>= 1;
                if ((val & 1) != 0) val ^= 0x6954559;
                val >>= 1;
            }
            val = ~val;
            byte[] temp = Oper.StringToByteArray(((val << 6) & 0xFFFFFFFF).ToString("X"));
            Array.Reverse(temp);
            for (int j = data.Length - 4; j != data.Length; j++) data[j] = temp[j - data.Length + 4];
            return data;
        }

        public static bool rawecc(byte[] data)
        {
            int i = 0x800, counter = 0;
            while (i < data.Length && counter <= 0x100)
            {
                byte[] sparedata = new byte[0x40];
                Buffer.BlockCopy(data, i, sparedata, 0, 0x40);

                i += 0x40;
                if (sparedata[0] == 0xFF && sparedata[0x10] == 0xFF &&
                    sparedata[0x20] == 0xFF && sparedata[0x30] == 0xFF &&
                    !Oper.allsame(sparedata, 0xFF) && sparedata[3] == 0x00 && sparedata[4] == 0x00
                    && sparedata[0x13] == 0x00 && sparedata[0x14] == 0x00
                    && sparedata[0x23] == 0x00 && sparedata[0x24] == 0x00
                    && sparedata[0x33] == 0x00 && sparedata[0x34] == 0x00)
                {
                    return true;
                }

                i += 0x800;
                if (i % 4200 == 0) counter++;
            }
            return false;
        }
        public static bool hasecc(byte[] data)
        {
            int i = 0x200, counter = 0;
            while (i < data.Length && counter <= 0x100)
            {
                byte[] sparedata = new byte[0x40];

                switch (i % 800)
                {
                    case 0:
                        Buffer.BlockCopy(data, i, sparedata, 0, 0x40);
                        i += 0x40;
                        if (sparedata[0] == 0xFF && sparedata[0x10] == 0xFF &&
                            sparedata[0x20] == 0xFF && sparedata[0x30] == 0xFF &&
                            !Oper.allsame(sparedata, 0xFF) && sparedata[3] == 0x00 && sparedata[4] == 0x00)
                        {
                            if (variables.debugMode) Console.WriteLine("Sparer {0:X}", i);
                            return true;
                        }
                        break;
                    default:
                        Buffer.BlockCopy(data, i, sparedata, 0, 0x10);
                        i += 0x10;
                        if ((sparedata[0] == 0xFF || sparedata[5] == 0xFF) && !Oper.allsame(Oper.returnportion(sparedata, 0xC, 0x4), 0xFF)
                            && !Oper.allsame(Oper.returnportion(sparedata, 0xC, 0x4), 0x00) && sparedata[3] == 0x00 && sparedata[4] == 0x00)
                        {
                            if (variables.debugMode) Console.WriteLine("Spare {0:X}", i);
                            return true;
                        }
                        break;
                }
                i += 0x200;
                if (i % 4200 == 0) counter++;
            }
            return false;
        }
        public static bool hasecc(ref byte[] data)
        {
            int i = 0x200, counter = 0;
            while (i < data.Length && counter <= 0x100)
            {
                byte[] sparedata = new byte[0x40];

                switch (i % 800)
                {
                    case 0:
                        Buffer.BlockCopy(data, i, sparedata, 0, 0x40);
                        i += 0x40;
                        if (sparedata[0] == 0xFF && sparedata[0x10] == 0xFF &&
                            sparedata[0x20] == 0xFF && sparedata[0x30] == 0xFF &&
                            !Oper.allsame(sparedata, 0xFF) && sparedata[3] == 0x00 && sparedata[4] == 0x00)
                        {
                            if (variables.debugMode) Console.WriteLine("Sparer {0:X}", i);
                            return true;
                        }
                        break;
                    default:
                        Buffer.BlockCopy(data, i, sparedata, 0, 0x10);
                        i += 0x10;
                        if ((sparedata[0] == 0xFF || sparedata[5] == 0xFF) && !Oper.allsame(Oper.returnportion(sparedata, 0xC, 0x4), 0xFF)
                            && !Oper.allsame(Oper.returnportion(sparedata, 0xC, 0x4), 0x00) && sparedata[3] == 0x00 && sparedata[4] == 0x00)
                        {
                            if (variables.debugMode) Console.WriteLine("Spare {0:X}", i);
                            return true;
                        }
                        break;
                }
                i += 0x200;
                if (i % 4200 == 0) counter++;
            }
            return false;
        }
        public static bool hasecc_v2(ref byte[] data)
        {
            int block_offset_b = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(data, 0x8, 4)), 16);
            if (data.Length < block_offset_b + 2) return hasecc(ref data);
            else
            {
                // I do not know why, but with my viper dual nand v2 on a trinity, when on NAND 1 (flash 1), readin the nands and comparing them leads to a "Index is out of bounds". Exception RIGHT here.
                // block_offset_b is negative.
                /* HOW TO REPLICATE: 
                 * Get Viper V2
                 * Dont solder in properly
                 * Read Nand and let them compare
                 * IndexOutOfBoundsException
                 */

                try
                {
                    if ((data[block_offset_b] == 0x43 && data[block_offset_b + 1] == 0x42) || (data[block_offset_b] == 0x53 && data[block_offset_b + 1] == 0x42)) // Check for text 'CB' or 'SB'
                    {
                        int length = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(data, block_offset_b + 0xC, 4)), 16);
                        if (data.Length < block_offset_b + length || length < 0) return hasecc(ref data);
                        else
                        {
                            block_offset_b = block_offset_b + length;
                            if (data[block_offset_b] == 0x43 && (data[block_offset_b + 1] == 0x42 || data[block_offset_b + 1] == 0x44)) return false; // Retail: Cx
                            else if (data[block_offset_b] == 0x53 && (data[block_offset_b + 1] == 0x42 || data[block_offset_b + 1] == 0x43 || data[block_offset_b + 1] == 0x44)) return false; // Dev: Sx
                            else return true;
                        }
                    }
                    else return true;
                }
                catch(IndexOutOfRangeException ex)
                {
                    // Throw again.
                    throw new Exception("There was a critical error analyzing a NAND. This is likely due to a highly corrupted NAND. Please check your NAND dumps.");
                }
            }
        }

        public static byte[] unecc(byte[] data, bool print = false)
        {
            int counter = 0;
            try
            {
                if (data[0x205] == 0xFF || data[0x415] == 0xFF || data[0x200] == 0xFF)
                {
                    if (print) Console.WriteLine("ECC'ed - will unecc.");
                    byte[] res = new byte[(data.Length / 0x210) * 0x200];
                    for (counter = 0; counter < res.Length; counter += 0x200)
                    {
                        if (((counter / 0x200) * 0x210) + 0x200 <= data.Length) Buffer.BlockCopy(data, (counter / 0x200) * 0x210, res, counter, 0x200);
                    }
                    data = res;
                    res = null;
                }
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }

            return data;
        }
        public static void unecc(ref byte[] data, bool print = false)
        {
            if (variables.debugMode) Console.WriteLine("On unecc");
            int counter = 0;
            try
            {
                if (data[0x205] == 0xFF || data[0x415] == 0xFF || data[0x200] == 0xFF)
                {
                    if (print || variables.debugMode) Console.WriteLine("ECC'ed - will unecc.");
                    byte[] res = new byte[(data.Length / 0x210) * 0x200];
                    for (counter = 0; counter < res.Length; counter += 0x200)
                    {
                        if (((counter / 0x200) * 0x210) + 0x200 <= data.Length) Buffer.BlockCopy(data, (counter / 0x200) * 0x210, res, counter, 0x200);
                    }
                    data = res;
                    res = null;
                }
                else if (data[0x800] == 0xFF && data[0x810] == 0xFF && data[0x820] == 0xFF)
                {
                    if (print || variables.debugMode) Console.WriteLine("ECC'ed BB - will unecc.");
                    byte[] res = new byte[(data.Length / 0x840) * 0x800];
                    for (counter = 0; counter < res.Length; counter += 0x800)
                    {
                        if (((counter / 0x800) * 0x840) + 0x800 <= data.Length) Buffer.BlockCopy(data, (counter / 0x800) * 0x840, res, counter, 0x800);
                    }
                    data = res;
                    res = null;
                }
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }
        }
        public static void unecc(ref byte[] data, ref ProgressBar pb, bool print = false)
        {
            int counter = 0;
            try
            {
                if (data[0x205] == 0xFF || data[0x415] == 0xFF || data[0x200] == 0xFF)
                {
                    if (print || variables.debugMode) Console.WriteLine("ECC'ed - will unecc.");
                    byte[] res = new byte[(data.Length / 0x210) * 0x200];
                    for (counter = 0; counter < res.Length; counter += 0x200)
                    {
                        if (((counter / 0x200) * 0x210) + 0x200 <= data.Length) Buffer.BlockCopy(data, (counter / 0x200) * 0x210, res, counter, 0x200);
                        pb.Value = (counter * 50 / data.Length);
                    }
                    data = res;
                    res = null;
                }
                else if (data[0x800] == 0xFF && data[0x810] == 0xFF && data[0x820] == 0xFF)
                {
                    if (print || variables.debugMode) Console.WriteLine("ECC'ed BB - will unecc.");
                    byte[] res = new byte[(data.Length / 0x840) * 0x800];
                    for (counter = 0; counter < res.Length; counter += 0x800)
                    {
                        if (((counter / 0x800) * 0x840) + 0x800 <= data.Length) Buffer.BlockCopy(data, (counter / 0x800) * 0x840, res, counter, 0x800);
                        pb.Value = (counter * 50 / data.Length);
                    }
                    data = res;
                    res = null;
                }
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }
        }

        public static bool checkecc(byte[] image, int blockstart, int blocklength)
        {
            int datalen;
            if (blocklength == 0) datalen = image.Length;
            else datalen = blocklength * 0x4200;

            byte[] d;
            bool check = true;
            for (int i = blockstart * 0x4200; i < datalen / 0x210; i++)
            {
                d = new byte[0x210];
                Buffer.BlockCopy(image, i * 0x210, d, 0, 0x210);
                byte[] ecc = Oper.returnportion(ref d, 0x210 - 4, 4);
                try
                {
                    d = calcecc(d);
                    byte[] f = Oper.returnportion(ref d, 0x210 - 4, 4);
                    if (!Oper.ByteArrayCompare(ecc, f, 4))
                    {
                        Console.WriteLine("ECD error @ {0:X}", i * 0x210);
                        Console.WriteLine("Wanted: {0} - Got: {1}", Oper.ByteArrayToString(f), Oper.ByteArrayToString(ecc));
                        check = false;
                    }
                }
                catch (System.IndexOutOfRangeException) { Oper.ByteArrayToString(d); }
            }
            return check;
        }
        public static void fixecc(string filename, int blockstart, int blocklength)
        {
            FileStream fr = new FileStream(filename, FileMode.Open);
            FileStream fw = new FileStream(Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename) + "_fixed" + Path.GetExtension(filename)), FileMode.Create);
            long datalen;
            if (blocklength == 0) datalen = fr.Length;
            else datalen = blocklength * 0x4200;

            byte[] d;

            for (int i = blockstart * 0x4200; i < datalen / 0x210; i++)
            {
                d = new byte[0x210];
                fr.Read(d, i * 0x210, 0x210);
                byte[] ecc = Oper.returnportion(ref d, 0x210 - 4, 4);
                try
                {
                    d = calcecc(d);
                    byte[] f = Oper.returnportion(ref d, 0x210 - 4, 4);

                }
                catch (System.IndexOutOfRangeException) { Oper.ByteArrayToString(d); }
                fw.Write(d, i * 0x4200, 0x210);
            }

            fr.Close();
            fw.Close();
        }

        public static bool kvNeedFcrt(byte[] kv)
        {
            //return (kv[0x1D] & 0xF0) == 0xF0;
            return (BitConverter.ToUInt16(new byte[2] { kv[0x1D], kv[0x1C] }, 0) & 0x120) != 0;
        }

        public static bool kvFcrtEncrypted(byte[] kv)
        {
            return !Oper.allsame(Oper.returnportion(kv, 0x40, 0x20), 0x00);
        }

        //private static byte[] ChangeLDVs(ref byte[] data, int layout)
        //{
        //    Console.WriteLine("Processing CF...");
        //    byte[] CF0 = null, CF1 = null;
        //    byte[] CF0_dec = null, CF1_dec = null;
        //    int CF0offset = 0, CF1offset = 0;
        //
        //    if (data[0] == 0xFF && data[1] == 0x4F)
        //    {
        //        Nand.Nand.getCF(data, layout == 2 ? true : false, out CF0, out CF0offset, out CF1, out CF1offset);
        //        if (variables.debugMode) Console.WriteLine("CF0 offset: {0:X} - CF0 size: {1:X}", CF0offset, CF0.Length);
        //        if (variables.debugMode) Console.WriteLine("CF1 offset: {0:X} - CF1 size: {1:X}", CF1offset, CF1.Length);
        //    }
        //    if (CF0 == null && CF1 == null)
        //    {
        //        Console.WriteLine("Failed");
        //        this.Close();
        //    }
        //    bool ready0 = false, ready1 = false;
        //    if (CF0 != null && (changedCF0ldv || changedCF0pd))
        //    {
        //        if (variables.debugMode) Console.WriteLine("Performing operations on CF0");
        //        CF0_dec = Nand.Nand.decrypt_CF(CF0);
        //        CF0_dec[0x21F] = Convert.ToByte(txtCF0ldv.Text, 10);
        //        CF0_dec[0x21C] = Convert.ToByte(txtCF0pd.Text.Substring(6, 2), 16);
        //        CF0_dec[0x21D] = Convert.ToByte(txtCF0pd.Text.Substring(4, 2), 16);
        //        CF0_dec[0x21E] = Convert.ToByte(txtCF0pd.Text.Substring(2, 2), 16);
        //        CF0 = Nand.Nand.encrypt_CF(CF0_dec, CF0, Oper.StringToByteArray(variables.cpukey));
        //        ready0 = true;
        //    }
        //    if (CF1 != null && (changedCF1ldv || changedCF1pd))
        //    {
        //        if (variables.debugMode) Console.WriteLine("Performing operations on CF1");
        //        CF1_dec = Nand.Nand.decrypt_CF(CF1);
        //        CF1_dec[0x21F] = Convert.ToByte(txtCF1ldv.Text, 10);
        //        CF1_dec[0x21C] = Convert.ToByte(txtCF1pd.Text.Substring(6, 2), 16);
        //        CF1_dec[0x21D] = Convert.ToByte(txtCF1pd.Text.Substring(4, 2), 16);
        //        CF1_dec[0x21E] = Convert.ToByte(txtCF1pd.Text.Substring(2, 2), 16);
        //        CF1 = Nand.Nand.encrypt_CF(CF1_dec, CF1, Oper.StringToByteArray(variables.cpukey));
        //        ready1 = true;
        //    }
        //    if (ready0)
        //    {
        //        if (variables.debugMode) Console.WriteLine("Adding ecc on CF0");
        //        CF0 = Nand.Nand.addecc_v2(CF0, true, (CF0offset / 0x200) * 0x210, layout);
        //        if (variables.debugMode) Console.WriteLine("Adding CF0 to nand");
        //        Buffer.BlockCopy(CF0, 0, data, (CF0offset / 0x200) * 0x210, CF0.Length);
        //    }
        //    if (ready1)
        //    {
        //        if (variables.debugMode) Console.WriteLine("Adding ecc on CF1");
        //        CF1 = Nand.Nand.addecc_v2(CF1, true, (CF1offset / 0x200) * 0x210, layout);
        //        if (variables.debugMode) Console.WriteLine("Adding CF1 to nand");
        //        Buffer.BlockCopy(CF1, 0, data, (CF1offset / 0x200) * 0x210, CF1.Length);
        //    }
        //    Console.WriteLine("Bootloader Patch Successful");
        //    return data;
        //}

        #endregion

    }
}

/*
if (layout == IMAGE_LAYOUT_0) {
    self->marker_ofs = 517;
    self->id_ofs = 512;
} else if (layout == IMAGE_LAYOUT_1) {
    self->marker_ofs = 517;
    self->id_ofs = 513;
} else {
    self->marker_ofs = 512;
    self->id_ofs = 513;
}
IMAGE_LAYOUT_0: xenon, zephyr, falcon
IMAGE_LAYOUT_1: jasper 16, slims
IMAGE_LAYOUT_2: jasper 256/512
*/

#region structs

//typedef struct _BLDR_FLASH{
//        u16 Magic; // 0xFF4f
//        u16 Build; // 0x2110
//        u16 Qfe; // 0x8000
//        u16 Flags; // 0x0
//        u32 Entry; // 0x8000
//        u32 Size; // 0x70000
//        char achCopyright[64];
//        u8 abReserved[16]; // 0x0 filled
//        u32 dwKeyVaultSize; // 0x4000
//        u32 dwSysUpdateAddr; // 0x70000 <-- offset to first cf
//        u16 wSysUpdateCount; // 2
//        u16 wKeyVaultVersion; // 0x0712
//        u32 dwKeyVaultAddr; // 0x4000
//        u32 dwSysUpdateSize; // if 0 = 0x10000, otherwise it's as-is patch slot size
//        u32 dwSmcConfigAddr; // 0x0
//        u32 dwSmcBootSize; // 0x3000
//        u32 dwSmcBootAddr; // 0x1000
//} BLDR_FLASH, *PBLDR_FLASH;

//typedef struct _FS_ENT{
//        char fileName[22];
//        u16 startCluster; //u8 startCluster[2];
//        u32 clusterSz; //u8 clusterSz[4];
//        u32 typeTime;
//} FS_ENT, *PFS_ENT;

//typedef struct _FS_SORTED{
//        u16 indirections[0x100*0x10];
//        FS_ENT fsent[0x10*0x10];
//} FS_SORTED, *PFS_SORTED;

//typedef struct _METADATA_SMALLBLOCK{
//        unsigned char BlockID1; // lba/id = (((BlockID0<<8)&0xF)+(BlockID1&0xFF))
//        unsigned char BlockID0 : 4;
//        unsigned char FsUnused0 : 4;
//        unsigned char FsSequence0; // oddly these aren't reversed
//        unsigned char FsSequence1;
//        unsigned char FsSequence2;
//        unsigned char BadBlock;
//        unsigned char FsSequence3;
//        unsigned char FsSize1; // (((FsSize0<<8)&0xFF)+(FsSize1&0xFF)) = cert size
//        unsigned char FsSize0;
//        unsigned char FsPageCount; // free pages left in block (ie: if 3 pages are used by cert then this would be 29:0x1d)
//        unsigned char FsUnused1[2];
//        unsigned char FsBlockType : 6;
//        unsigned char ECC3 : 2;
//        unsigned char ECC2; // 26 bit ECD
//        unsigned char ECC1;
//        unsigned char ECC0;
//} SMALLBLOCK;

//typedef struct _METADATA_BIGONSMALL{
//        unsigned char FsSequence0;
//        unsigned char BlockID1; // lba/id = (((BlockID0<<8)&0xF)+(BlockID1&0xFF))
//        unsigned char BlockID0 : 4;
//        unsigned char FsUnused0 : 4;
//        unsigned char FsSequence1;
//        unsigned char FsSequence2;
//        unsigned char BadBlock;
//        unsigned char FsSequence3;
//        unsigned char FsSize1; // (((FsSize0<<8)&0xFF)+(FsSize1&0xFF)) = cert size
//        unsigned char FsSize0;
//        unsigned char FsPageCount; // free pages left in block (ie: if 3 pages are used by cert then this would be 29:0x1d)
//        unsigned char FsUnused1[2];
//        unsigned char FsBlockType : 6;
//        unsigned char ECC3 : 2;
//        unsigned char ECC2; // 26 bit ECD
//        unsigned char ECC1;
//        unsigned char ECC0;
//} BIGONSMALL;

//typedef struct _METADATA_BIGBLOCK{
//        unsigned char BadBlock;
//        unsigned char BlockID1; // lba/id = (((BlockID0<<8)&0xF)+(BlockID1&0xFF))
//        unsigned char BlockID0 : 4;
//        unsigned char FsUnused0 : 4;
//        unsigned char FsSequence2; // oddly, compared to before these are reversed...?
//        unsigned char FsSequence1;
//        unsigned char FsSequence0;
//        unsigned char FsUnused1;
//        unsigned char FsSize1; // FS: 06 (system reserve block number) else ((FsSize0<<16)+(FsSize1<<8)) = cert size
//        unsigned char FsSize0; // FS: 20 (size of flash filesys in smallblocks >>5)
//        unsigned char FsPageCount; // FS: 04 (system config reserve) free pages left in block (multiples of 4 pages, ie if 3f then 3f*4 pages are free after)
//        unsigned char FsUnused2[0x2];
//        unsigned char FsBlockType : 6; // FS: 2a bitmap: 2c (both use FS: vals for size), mobiles
//        unsigned char ECC3 : 2;
//        unsigned char ECC2; // 26 bit ECD
//        unsigned char ECC1;
//        unsigned char ECC0;
//} BIGBLOCK;

//typedef struct _METADATA{
//        union{
//                SMALLBLOCK sm;
//                BIGBLOCK bg;
//                BIGONSMALL bos;
//        };
//} METADATA, *PMETADATA;

//typedef struct _PAGEDATA{
//        unsigned char user[512];
//        METADATA meta;
//} PAGEDATA, *PPAGEDATA;

#endregion