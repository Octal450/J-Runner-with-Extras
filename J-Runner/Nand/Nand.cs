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
        public int CB_A;
        public int CB_B;
        public int CD;
        public int CE;
        public int CF_0;
        public int CG_0;
        public int CF_1;
        public int CG_1;
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
        public KVInfo ki;
        public Useful uf;
        public string _cpukey = "", _filename;
        private int _currentFS = 0;
        public bool noecc = false, bigblock = false;
        public byte[] _rawkv, _smc, _smc_config;
        public List<int> bad_blocks = new List<int>(), remapped_blocks = new List<int>();
        private List<FSFile> Files = new List<FSFile>();

        private void erasev()
        {
            _cpukey = "";
            _filename = "";
            _currentFS = 0;
            ok = noecc = bigblock = false;
            bad_blocks = new List<int>();
            remapped_blocks = new List<int>();
            Files = new List<FSFile>();
            bl.CB_A = 0;
            bl.CB_B = 0;
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
            if (s1 >= 0x4200000 && s1 <= 0x21000000) bigblock = true;
            else bigblock = false;
            //
            byte[] temp = new byte[0x210];
            Buffer.BlockCopy(data, 0, temp, 0, data.Length > temp.Length ? temp.Length : data.Length);
            if (!ascii.GetString(temp).Contains("Microsoft"))
            {
                if (variables.debugme) Console.WriteLine(ascii.GetString(temp));
                if (temp[0] == 0x46 && temp[1] == 0x57 && temp[2] == 0x41 && temp[3] == 0x00) Console.WriteLine("DemoN FW");
                else if (s1 != 0x40000) Console.WriteLine("Header is wrong");
            }
            //

            if (data[0] == 0xFF && data[1] == 0x4F)
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

        void unpack_base_image(byte[] image, bool bigblock)
        {
            byte[] data, cb_dec = { }, cd_dec = { };
            byte[] CB_A = null, CB_B = null; //SMC = null, CD = null, CE = null, Keyvault = null;
            bl.CB_A = 0; bl.CB_B = 0; bl.CD = 0; bl.CE = 0; bl.CF_0 = 0; bl.CG_0 = 0; bl.CF_1 = 0; bl.CG_1 = 0;
            uf.ldv_p0 = 0; uf.ldv_p1 = 0; uf.ldv_cb = 0; uf.pd_cb = ""; uf.pd_0 = ""; uf.pd_1 = "";

            if (Nand.rawecc(image)) Console.WriteLine("Image is raw. F11 to convert");
            if (Nand.hasecc_v2(ref image)) Nand.unecc(ref image, false);
            else noecc = true;

            if (variables.debugme) Console.WriteLine("Has ecc? !{0}", noecc);

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
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }


            #region blocks
            try
            {
                int block = 0, block_size, id;
                byte block_id;
                int block_build;
                byte[] block_build_b = new byte[2], block_size_b = new byte[4];
                int block_offset_b = Convert.ToInt32(Oper.ByteArrayToString(block_offset), 16);
                int semi = 0;
                for (block = 0; block < 30; block++)
                {
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
                    if (variables.debugme) Console.WriteLine("Found {0}BL (build {1}) at {2}", id, block_build, Convert.ToString(block_offset_b, 16));
                    data = new byte[block_size];
                    //data = returnportion(image, block_offset_b, block_size);
                    if (block_offset_b + block_size <= image.Length) Buffer.BlockCopy(image, block_offset_b, data, 0, block_size);
                    if (id == 2)
                    {
                        if (semi == 0)
                        {
                            bl.CB_A = block_build;
                            CB_A = data;
                            semi = 1;
                        }
                        else if (semi == 1)
                        {
                            bl.CB_B = block_build;
                            CB_B = data;
                            semi = 0;
                        }

                        if (semi == 0)
                        {
                            if (variables.extractfiles) Oper.savefile(data, "output\\CB_B.bin");
                            if (string.IsNullOrEmpty(variables.cpkey)) cb_dec = Nand.decrypt_CB_cpukey(CB_B, Nand.decrypt_CB(CB_A), Oper.StringToByteArray("00000000000000000000000000000000")); // It just needs something, doesn't matter that its not valid
                            else cb_dec = Nand.decrypt_CB_cpukey(CB_B, Nand.decrypt_CB(CB_A), Oper.StringToByteArray(variables.cpkey));
                            if (variables.extractfiles) Oper.savefile(cb_dec, "output\\CB_B_dec.bin");
                            uf.ldv_cb = cb_dec[0x192]; // needs fixing
                            if (variables.debugme) Console.WriteLine("LDV CB: {0}", uf.ldv_cb.ToString());
                            byte[] temppd = (Oper.returnportion(cb_dec, 0x20, 3));
                            Array.Reverse(temppd);
                            uf.pd_cb = "0x" + Oper.ByteArrayToString(temppd);
                            //if (variables.debugme) Console.WriteLine(uf.pd_cb);
                        }
                        else
                        {
                            cb_dec = Nand.decrypt_CB(CB_A);
                            if (variables.extractfiles) Oper.savefile(data, "output\\CB_A.bin");
                            if (variables.extractfiles) Oper.savefile(cb_dec, "output\\CB_A_dec.bin");
                            uf.ldv_cb = cb_dec[0x192]; // needs fixing
                            if (variables.debugme) Console.WriteLine("LDV CB: {0}", uf.ldv_cb.ToString());
                            byte[] temppd = (Oper.returnportion(cb_dec, 0x20, 3));
                            Array.Reverse(temppd);
                            uf.pd_cb = "0x" + Oper.ByteArrayToString(temppd);
                            //if (variables.debugme) Console.WriteLine(uf.pd_cb);
                        }

                    }
                    else if (id == 4)
                    {
                        bl.CD = block_build;
                        if (variables.extractfiles) Oper.savefile(data, "output\\CD.bin");
                        cd_dec = Nand.decrypt_CD(data, cb_dec);
                        if (variables.extractfiles) Oper.savefile(cd_dec, "output\\CD_dec.bin");
                        //CD = data;
                    }
                    else if (id == 5)
                    {
                        bl.CE = block_build;
                        if (variables.extractfiles) Oper.savefile(data, "output\\CE.bin");
                        //CE = data;
                    }
                    block_offset_b += block_size;
                    if (id == 5) break;
                }
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            #endregion

            try
            {
                unpack_update(ref image, bigblock);
            }
            catch (System.IndexOutOfRangeException ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            catch (System.OutOfMemoryException ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }

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
                        if (variables.debugme) Console.WriteLine("block size: 0x{0:X} - offset: 0x{1:X} - image: 0x{2:X}", block_size, block_offset_b, image.Length);
                    }

                    if (id == 6 || id == 7)
                    {
                        if (variables.debugme) Console.WriteLine("-Found {0}BL Patch {3} (build {1}) at {2:X}", id, block_build, block_offset_b, patch);
                        if (id == 6)
                        {
                            patch_offset = block_offset_b;

                            if (patch == 0)
                            {
                                CF0 = Nand.decrypt_CF(data);
                                bl.CF_0 = block_build;
                                uf.ldv_p0 = Nand.decrypt_CF(data)[0x21F];
                                if (variables.debugme) Console.WriteLine("-LDV Patch {0}: {1}", patch, uf.ldv_p0);
                                byte[] temppd = (Oper.returnportion(Nand.decrypt_CF(data), 0x21C, 3));
                                Array.Reverse(temppd);
                                uf.pd_0 = "0x" + Oper.ByteArrayToString(temppd);
                                if (variables.debugme) Console.WriteLine("-Pairing Data 0: {0:X}", uf.pd_0);
                            }
                            else
                            {
                                CF1 = Nand.decrypt_CF(data);
                                bl.CF_1 = block_build;
                                uf.ldv_p1 = Nand.decrypt_CF(data)[0x21F];
                                if (variables.debugme) Console.WriteLine("-LDV Patch {0}: {1}", patch, uf.ldv_p1);
                                byte[] temppd = (Oper.returnportion(Nand.decrypt_CF(data), 0x21C, 3));
                                Array.Reverse(temppd);
                                uf.pd_1 = "0x" + Oper.ByteArrayToString(temppd);
                                if (variables.debugme) Console.WriteLine("-Pairing Data 1: {0:X}", uf.pd_1);
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
                        if (variables.debugme) Console.WriteLine("2 - {0:X}", block_offset_b);
                        block_offset_b = patch_offset + 0x10000;
                        continue;
                    }
                    else if (temo == 0x46 && tem0 == 0x43 && patch == 1)
                    {
                        if (variables.debugme) Console.WriteLine("1 - {0:X}", block_offset_b);
                        block_offset_b = patch_offset + blocksize;
                        continue;
                    }
                    else if (patch == 0 && tem3 == 0x43 && tem4 == 0x46 && tem5 != 0x43 && tem6 != 0x47)
                    {
                        if (variables.debugme) Console.WriteLine("4 - {0:X}", block_offset_b);
                        block_offset_b += 0x10000;
                        patch = 1;
                        continue;
                    }

                    else if (patch == 0 && block_offset_b > 0x80000 && patch_offset < 0x80000)
                    {
                        if (variables.debugme) Console.WriteLine("3 - {0:X}", block_offset_b);
                        patch = 1;
                        block_offset_b = 0x80000;
                        continue;
                    }
                    if (block_size == 0x10) { block_size = 0x20000; patch = 1; }
                    block_offset_b += block_size;
                    if (variables.debugme) Console.WriteLine("5 - {0:X}", block_offset_b);
                    if (temp_block_offset == block_offset_b) break;
                    if (block_offset_b > size) break;
                }
            }
            catch (System.OverflowException) { return; }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }

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
            if (String.IsNullOrWhiteSpace(cpukey)) return false;
            byte[] key = Oper.StringToByteArray(cpukey);
            if (Oper.allsame(Oper.returnportion(_rawkv, 0x40, 0x20), 0x00)) return true;
            if (Oper.allsame(Oper.returnportion(Nand.decryptkv(_rawkv, key), 0x40, 0x20), 0x00))
            {
                if (variables.debugme) Console.WriteLine("cpukey verified - {0}", cpukey);
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
                smc_config_offset = 0xFEB800;
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

            if (variables.debugme) Console.WriteLine("-R-Image Size: 0x{0:X} | imagesize: 0x{1:X}", image.Length, blocksize);

            int counter;
            for (counter = 0; counter < image.Length / blocksize; counter++)
            {
                byte[] block = new byte[blocksize];
                Buffer.BlockCopy(image, counter * blocksize, block, 0, blocksize);
                if (JRunner.Nand.BadBlock.checkifbadblock(block, counter, bigblock, true))
                {
                    bad_blocks.Add(counter);
                }
                if (bad_blocks.Count >= 0x20)
                {
                    bad_blocks = new List<int>();
                    return;
                }
            }


            if (bad_blocks.Count == 0)
            {
                return;
            }

            int reserveblockpos;
            if (blocksize == 0x4200) reserveblockpos = 0x3FF;
            else reserveblockpos = 0x1FF;

            int reservestartpos = reserveblockpos - 0x20;
            byte[] reserved = Oper.returnportion(image, reservedoffset * blocksize, 0x20 * blocksize);
            if (variables.debugme) Oper.savefile(reserved, "reservedarea.bin");
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
            if (variables.debugme) Console.WriteLine("Offset: {0:X} - Length {1:X} - corona: {2}", offset, length, corona);
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

            if (variables.debugme) Console.WriteLine("bigblock: {0}", bigblock);
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
                        if (variables.debugme) Console.WriteLine(fsseq);
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
                    if (String.IsNullOrEmpty(filename))
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
                    catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }

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
            if (String.IsNullOrEmpty(fil.getFilename())) return null;
            if (variables.debugme) Console.WriteLine("{0:X} : {1:X}", fil.getBlock(), fil.getLength());
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

        public static long kvcrc(string filename)
        {
            byte[] Keyvault = null;
            //long size = 0;
            //byte[] data = openfile(filename, ref size, 40 * 1024);
            byte[] data = Oper.returnportion(BadBlock.find_bad_blocks_X(filename, 2), 0, 40 * 1024);

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
                //Console.WriteLine("* unknown image found !");
                return 0;
            }
        }

        public static byte[] getrawkv(string filename)
        {
            long size = 0;
            byte[] data = BadBlock.find_bad_blocks_X(Oper.openfile(filename, ref size, 0), 5);
            if (variables.debugme) Console.WriteLine("data: {0:X}", data.Length);
            byte[] Keyraw = new byte[0x4200];
            Keyraw = Oper.returnportion(data, 0x4200, 0x4200);
            if (variables.extractfiles) Oper.savefile(Keyraw, "output\\KV_raw.bin");
            if (variables.debugme) Console.WriteLine("Keyraw: {0:X}", Keyraw.Length);
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
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            return null;
        }

        public static byte[] encryptkv(byte[] kv, byte[] key)
        {
            if (variables.debugme) Console.WriteLine(key.Length);
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

            if (variables.debugme) Console.WriteLine("kv length: {0:X}", rawKV.Length);

            fileb.BaseStream.Seek(rawKV.Length, SeekOrigin.Begin);
            fileb.Write(rawKV);

            infile.Close();
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
            if (variables.debugme) Console.WriteLine("Getting CB");
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
            if (variables.debugme) Console.WriteLine("Unecc'd Conf");
            byte block_id;
            int block_build = 0;
            byte[] block_build_b = new byte[2], block_size_b = new byte[4], SMC;
            int block_offset_b = (Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(image, 0x8, 4)), 16));
            block_id = image[block_offset_b + 1];
            block_build_b = Oper.returnportion(image, block_offset_b + 2, 2);
            int id = block_id & 0xF;
            if (id == 2) block_build = Convert.ToInt32(Oper.ByteArrayToString(block_build_b), 16);
            if (variables.debugme) Console.WriteLine("Block Build: {0}", block_build);
            if (variables.debugme) Console.WriteLine("Checking SMC");
            byte[] smc_len = new byte[4], smc_start = new byte[4];
            smc_len = Oper.returnportion(image, 0x78, 4);
            smc_start = Oper.returnportion(image, 0x7C, 4);
            SMC = new byte[Convert.ToInt32(Oper.ByteArrayToString(smc_len), 16)];
            SMC = Oper.returnportion(image, Convert.ToInt32(Oper.ByteArrayToString(smc_start), 16), Convert.ToInt32(Oper.ByteArrayToString(smc_len), 16));
            SMC = decrypt_SMC(SMC);
            variables.smcmbtype = SMC[0x100] >> 4 & 15;
            if (variables.debugme) Console.WriteLine("SMC Type: {0}", variables.smcmbtype);
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
                catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }

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
                            if (variables.debugme) Console.WriteLine("Block ID: {0} | Block offset: {1:X}", block_id, block_offset_b);
                            int temp_block_offset = block_offset_b;
                            //block_build_b = returnportion(image, block_offset_b + 2, 2);
                            Buffer.BlockCopy(image, block_offset_b + 2, block_build_b, 0, 2);
                            //block_size_b = returnportion(image, block_offset_b + 12, 4);
                            Buffer.BlockCopy(image, block_offset_b + 12, block_size_b, 0, 4);
                            block_size = Convert.ToInt32(Oper.ByteArrayToString(block_size_b), 16);
                            block_build = Convert.ToInt32(Oper.ByteArrayToString(block_build_b), 16);
                            if (variables.debugme) Console.WriteLine("Block Build {0} : Block Size {1:X}", block_build, block_size);
                            block_size += 0xF;
                            block_size &= ~0xF;
                            id = block_id & 0xF;
                            byte[] data = new byte[block_size];
                            //byte[] data = returnportion(image, block_offset_b, block_size);
                            if (block_size + block_offset_b <= image.Length) Buffer.BlockCopy(image, block_offset_b, data, 0, block_size);

                            if (id == 6 || id == 7)
                            {
                                if (variables.debugme) Console.WriteLine("Found {0}BL Patch {3} (build {1}) at {2:X}", id, block_build, block_offset_b, patch);
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
                                if (variables.debugme) Console.WriteLine("2 - {0:X}", block_offset_b);
                                block_offset_b = patch_offset + 0x10000;
                                continue;
                            }
                            else if (temo == 0x46 && tem0 == 0x43 && patch == 1)
                            {
                                if (variables.debugme) Console.WriteLine("1 - {0:X}", block_offset_b);
                                block_offset_b = patch_offset + blocksize;
                                continue;
                            }
                            else if (patch == 0 && tem3 == 0x43 && tem4 == 0x46 && tem5 != 0x43 && tem6 != 0x47)
                            {
                                if (variables.debugme) Console.WriteLine("4 - {0:X}", block_offset_b);
                                block_offset_b += 0x10000;
                                patch = 1;
                                continue;
                            }

                            else if (patch == 0 && block_offset_b > 0x80000 && patch_offset < 0x80000)
                            {
                                if (variables.debugme) Console.WriteLine("3 - {0:X}", block_offset_b);
                                patch = 1;
                                block_offset_b = 0x80000;
                                continue;
                            }
                            if (block_size == 0x10) { block_size = 0x20000; patch = 1; }
                            block_offset_b += block_size;
                            if (variables.debugme) Console.WriteLine("5 - {0:X}", block_offset_b);
                            if (temp_block_offset == block_offset_b) break;
                            if (block_offset_b > size) break;
                        }
                    }
                    catch (System.OverflowException) { return; }
                    catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
                }
                catch (System.IndexOutOfRangeException ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
                catch (System.OutOfMemoryException ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }
            }
        }

        public static byte[] decrypt_CB(byte[] image)
        {
            if (variables.debugme) Console.WriteLine(" * decrypting CB...");
            byte[] message = Oper.returnportion(image, 0x10, 0x10);
            byte[] RC4_key = Oper.HMAC_SHA1(secret_1bl, message);
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

        public static byte[] decrypt_CB_cpukey(byte[] CB_B, byte[] CB_A, byte[] cpukey)
        {
            byte[] secret = Oper.returnportion(CB_A, 0x10, 0x10);
            byte[] temp = Oper.returnportion(CB_B, 0x10, 0x10);
            byte[] message = Oper.concatByteArrays(temp, cpukey, 0x10, 0x10);

            if ((Oper.ByteArrayToInt(Oper.returnportion(CB_A, 0x6, 2)) & 0x1000) != 0)
            {
                if (variables.debugme) Console.WriteLine("CB - Using new encryption scheme");
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
            byte[] secret = Oper.returnportion(CB_B, 0x10, 0x10);
            byte[] message = Oper.returnportion(CD, 0x10, 0x10);
            byte[] RC4_key = Oper.HMAC_SHA1(secret, message);

            //RC4_key = HMAC_SHA1(cpukey, returnportion(RC4_key,0, 0x10));
            byte[] imfordec = Oper.returnportion(CD, 0x20, CD.Length - 0x20);
            Oper.RC4_v(ref imfordec, Oper.returnportion(RC4_key, 0, 0x10));


            byte[] finalimage = new byte[CD.Length];
            for (int i = 0; i < CD.Length; i++)
            {
                if (i < 0x10) finalimage[i] = CD[i];
                else if (i < 0x20) finalimage[i] = RC4_key[i - 0x10];
                else finalimage[i] = imfordec[i - 0x20];
            }
            return finalimage;
        }

        public static byte[] encrypt_CB_cpukey(byte[] image, byte[] CB_A_key, byte[] cpukey)
        {
            if (variables.debugme) Console.WriteLine(cpukey.Length);

            byte[] secret = CB_A_key;
            byte[] crypto = Oper.returnportion(image, 0x10, 0x10);
            byte[] message = Oper.concatByteArrays(crypto, cpukey, 0x10, 0x10);

            if ((Oper.ByteArrayToInt(Oper.returnportion(CB_A_key, 0x6, 2)) & 0x1000) != 0)
            {
                if (variables.debugme) Console.WriteLine("Using new encryption scheme");
                CB_A_key[0x6] = 0x00;
                CB_A_key[0x7] = 0x00;
                message = Oper.concatByteArrays(message, CB_A_key, message.Length, 0x10);
            }

            byte[] RC4_key = Oper.HMAC_SHA1(secret, message);
            byte[] imfordec = Oper.returnportion(image, 0x20, image.Length - 0x20);
            if (variables.debugme) Console.WriteLine(Oper.ByteArrayToString(RC4_key));
            Oper.RC4_v(ref imfordec, Oper.returnportion(RC4_key, 0, 0x10));


            byte[] finalimage = new byte[image.Length];
            for (int i = 0; i < image.Length; i++)
            {
                if (i < 0x10) finalimage[i] = image[i];
                else if (i < 0x20) finalimage[i] = crypto[i - 0x10];
                else finalimage[i] = imfordec[i - 0x20];
            }
            return finalimage;
        }

        public static byte[] encrypt_CB(byte[] image, byte[] random, ref byte[] key)
        {
            byte[] finalimage = new byte[image.Length];
            try
            {
                Console.WriteLine(" * encrypting CB...");
                byte[] RC4_key = Oper.HMAC_SHA1(secret_1bl, random);
                //byte[] RC4_key = returnportion(image, 0x10, 0x10);
                byte[] imfordec = Oper.returnportion(image, 0x20, image.Length - 0x20);
                if (variables.debugme) Console.WriteLine(" CB Stage 1");
                key = Oper.returnportion(RC4_key, 0, 0x10);
                Oper.RC4_v(ref imfordec, Oper.returnportion(RC4_key, 0, 0x10));
                if (variables.debugme) Console.WriteLine(" CB Stage 2");

                for (int i = 0; i < image.Length; i++)
                {
                    if (i < 0x10) finalimage[i] = image[i];
                    else if (i < 0x20) finalimage[i] = random[i - 0x10];
                    else finalimage[i] = imfordec[i - 0x20];
                }
                if (variables.debugme) Console.WriteLine(" * encrypted CB...");

            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            return finalimage;
        }

        public static byte[] encrypt_CD(byte[] image, byte[] random, byte[] CB_B_key)
        {
            Console.WriteLine(" * encrypting CD...");
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
            if (variables.debugme) Console.WriteLine(" * encrypted CD...");
            return finalimage;
        }

        public static byte[] decrypt_CF(byte[] image)
        {
            if (variables.debugme) Console.WriteLine(" * decrypting CF...");
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
            if (variables.debugme) Console.WriteLine(" * decrypting CG...");
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
            if (variables.debugme) Console.WriteLine(" * encrypting...");
            byte[] message = random;
            //byte[] RC4_key = HMAC_SHA1(secret_1bl, message);
            byte[] RC4_key = Oper.returnportion(CF_dec, 0x20, 0x10);
            byte[] imfordec = Oper.returnportion(CF_dec, 0x30, CF_dec.Length - 0x30);
            Oper.RC4_v(ref imfordec, Oper.returnportion(RC4_key, 0, 0x10));
            byte[] hash = calcCFhash(CF_dec, cpukey);
            RC4_key = Oper.HMAC_SHA1(secret_1bl, Oper.returnportion(CF_dec, 0x20, 0x10));
            if (variables.debugme) Console.WriteLine(Oper.ByteArrayToString(RC4_key));
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
            if (variables.debugme) Console.WriteLine("{0:X} - {1:X}", data.Length, block_offset);
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
            bool bigblock, corona;
            int layout;
            if (filename == null) return;
            if (!File.Exists(filename)) return;
            FileInfo f = new FileInfo(filename);
            long s1 = f.Length;
            if (s1 >= 0x4200000) bigblock = true;
            else bigblock = false;
            corona = true;

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

            if (hasecc(temp)) corona = false;
            if (variables.debugme) Console.WriteLine("bigblock:{0} - corona: {1} - layout: {2}", bigblock, corona, layout);

            int smc_offset, smc_length;
            byte[] smc_len = new byte[4], smc_start = new byte[4];
            Buffer.BlockCopy(temp, 0x78, smc_len, 0, 4);
            Buffer.BlockCopy(temp, 0x7C, smc_start, 0, 4);
            smc_length = Oper.ByteArrayToInt(smc_len);
            smc_offset = Oper.ByteArrayToInt(smc_start);

            SMCdec = encrypt_SMC(SMCdec);
            if (!corona)
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
            bool bigblock, corona;
            int layout;
            if (filename == null) return;
            if (!File.Exists(filename)) return;
            FileInfo f = new FileInfo(filename);
            long s1 = f.Length;
            if (s1 >= 0x4200000) bigblock = true;
            else bigblock = false;
            corona = true;
            byte[] smc_config = null;
            byte[] temp = (BadBlock.find_bad_blocks_X(filename, 0x50));
            FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader file = new BinaryReader(infile);
            BinaryWriter fileb = new BinaryWriter(infile);

            //byte[] temp = (BadBlock.find_bad_blocks_X(filename, 0x50));
            byte[] sparedata = new byte[0x10];
            file.Read(temp, 0, 0x420);

            file.BaseStream.Seek(0x4400, SeekOrigin.Begin);
            file.Read(sparedata, 0, 0x10);
            layout = identifylayout(sparedata);

            if (hasecc(temp)) corona = false;
            if (variables.debugme) Console.WriteLine("bigblock:{0} - corona: {1} - layout: {2}", bigblock, corona, layout);

            int smc_config_offset, smc_config_length;
            if (!bigblock)
            {
                smc_config_offset = 0xFEB800;
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

            fileb.BaseStream.Seek(smc_config_offset, SeekOrigin.Begin);
            if (!corona)
            {
                data = addecc_v2(data, true, smc_config_offset, layout);
            }
            fileb.Write(data);


            infile.Close();
            return;
        }

        public static byte[] getsmcconfig(string filename, out int block_offset)
        {
            bool bigblock, corona;
            block_offset = 0;
            if (filename == null) return null;
            if (!File.Exists(filename)) return null;
            FileInfo f = new FileInfo(filename);
            long s1 = f.Length;
            if (s1 >= 0x4200000) bigblock = true;
            else bigblock = false;
            corona = true;
            byte[] smc_config = null;

            FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader file = new BinaryReader(infile);

            byte[] temp = (BadBlock.find_bad_blocks_X(filename, 0x50));
            if (hasecc(temp)) corona = false;

            if (variables.debugme) Console.WriteLine("bigblock:{0} - corona: {1}", bigblock, corona);

            int smc_config_offset, smc_config_length;
            if (!bigblock)
            {
                block_offset = 0xC000;
                smc_config_offset = 0xFEB800;
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
            if (corona)
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

            if (variables.debugme) Console.WriteLine("length: {0:X} - offset {1:X}", smc_config_length, smc_config_offset);

            if (!corona)
            {
                unecc(ref smc_config);
            }
            return smc_config;
        }

        #endregion

        public static string getConsoleName(PrivateN nand, string flashconfig = "")
        {
            if (variables.debugme) Console.WriteLine("Identifying Console");
            int[] cons = identifyConsole(nand, flashconfig);

            int max = -1;
            int howmany = 0;
            int consl = 0;
            for (int i = 0; i < 13; i++)
            {
                if (max < cons[i])
                {
                    max = cons[i];
                    consl = i;
                }
                else if (max == cons[i]) howmany++;
            }

            if (cons[2] == cons[9] && consl == 2) return "Falcon";
            if (cons[6] == cons[7] && consl == 6) return "Jasper BB";
            if (cons[4] == cons[6] && cons[4] == cons[7] && consl == 4) return "Jasper";

            return variables.cunts[consl].Text;
        }
        public static consoles getConsole(PrivateN nand, string flashconfig = "")
        {
            if (variables.debugme) Console.WriteLine("Getting cunts");
            int[] cons = identifyConsole(nand, flashconfig);
            // do Stuff
            int max = -1;
            int howmany = 0;
            int consl = 0;
            for (int i = 0; i < 13; i++)
            {
                if (max < cons[i])
                {
                    max = cons[i];
                    consl = i;
                }
                else if (max == cons[i]) howmany++;
            }
            
            if (cons[2] == cons[9] && consl == 2) return variables.cunts[2];
            if (cons[6] == cons[7] && consl == 6) return variables.cunts[6];
            if (cons[4] == cons[5] && cons[4] == cons[6] && cons[4] == cons[7] && consl == 4) return variables.cunts[0];
            
            return variables.cunts[consl];
        }
        public static int[] identifyConsole(PrivateN nand, string flashconfig = "")
        {
            int[] cons = new int[13];

            // CB check
            if (nand.bl.CB_A >= 9188 && nand.bl.CB_A <= 9250)
            {
                cons[1] += 3;
                cons[12] += 3;
            }
            else if (nand.bl.CB_A >= 13121 && nand.bl.CB_A <= 13200)
            {
                if (nand.noecc) cons[11] += 3;
                else cons[10] += 3;
            }
            else if (nand.bl.CB_A >= 6712 && nand.bl.CB_A <= 6780)
            {
                cons[4] += 3;
                cons[5] += 3;
                cons[6] += 3;
                cons[7] += 3;
            }
            else if (nand.bl.CB_A >= 4558 && nand.bl.CB_A <= 4590) cons[3] += 3;
            else if ((nand.bl.CB_A >= 1888 && nand.bl.CB_A <= 1960) || (nand.bl.CB_A >= 7373 && nand.bl.CB_A <= 7378) || nand.bl.CB_A == 8192) cons[8] += 3;
            else if (nand.bl.CB_A >= 5761 && nand.bl.CB_A <= 5780)
            {
                cons[2] += 3;
                cons[9] += 3;
            }

            // smc check
            //console_types = { "none/unk", "Xenon", "Zephyr", "Falcon", "Jasper", "Trinity", "Corona", "Winchester" };
            int smctype = nand._smc[0x100] >> 4 & 15;
            if (smctype < variables.console_types.Length && smctype >= 0)
            {
                if (smctype == 1) cons[8] += 2;
                else if (smctype == 2) cons[3] += 2;
                else if (smctype == 3)
                {
                    cons[2] += 2;
                    cons[9] += 2;
                }
                else if (smctype == 4)
                {
                    cons[4] += 2;
                    cons[5] += 2;
                    cons[6] += 2;
                    cons[7] += 2;
                }
                else if (smctype == 5)
                {
                    cons[1] += 2;
                    cons[12] += 2;
                }
                else if (smctype == 6)
                {
                    cons[10] += 2;
                    cons[11] += 2;
                }
            }
            //flashconfig check
            if (!String.IsNullOrWhiteSpace(flashconfig))
            {
                if (flashconfig == "008A3020")
                {
                    cons[6]++;
                    cons[12]++;
                }
                else if (flashconfig == "00AA3020")
                {
                    cons[7]++;
                    cons[12]++;
                }
                else if (flashconfig == "C0462002") cons[11]++;
                else if (flashconfig == "01198010")
                {
                    cons[2]++;
                    cons[3]++;
                    cons[5]++;
                    cons[8]++;
                    cons[9]++;
                }
                else if (flashconfig == "00023010")
                {
                    cons[1]++;
                    cons[4]++;
                }
                else if (flashconfig == "00043000")
                {
                    cons[10]++;
                }
            }
            //file length
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
                    cons[9]++;
                    cons[10]++;
                }
                else if (length == 69206016)
                {
                    cons[6] += 2;
                    cons[7] += 2;
                    cons[12] += 2;
                }
                else if (length == 276824064)
                {
                    cons[6] += 2;
                    cons[12] += 2;
                }
                else if (length == 553648128)
                {
                    cons[7] += 2;
                    cons[12] += 2;
                }
                else cons[11]++;
            }
            //spare data check
            if (nand.noecc)
            {
                cons[11]++;
            }
            else
            {
                //IMAGE_LAYOUT_0: xenon, zephyr, falcon
                //IMAGE_LAYOUT_1: jasper 16, slims
                //IMAGE_LAYOUT_2: jasper/trinity 256/512
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
                    cons[8]++;
                    cons[9]++;
                }
                else if (layout == 1)
                {
                    cons[1]++;
                    cons[4]++;
                    cons[10]++;
                }
                else if (layout == 2)
                {
                    cons[6]++;
                    cons[7]++;
                    cons[12]++;
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
        public static byte[] FixPerBoxDigest(byte[] SMC_en, byte[] CB_en, byte[] cpukey)
        {
            byte[] RC4_key = Oper.HMAC_SHA1(secret_1bl, Oper.returnportion(CB_en, 0x10, 0x10));

            byte[] CB_dec = decrypt_CB(CB_en);
            byte[] reserved = Oper.returnportion(CB_dec, 0x24, 0xC);
            byte[] pairingdata = Oper.returnportion(CB_dec, 0x20, 3);

            byte[] digest = new byte[0x30];
            byte[] SMC_HASH = CalculateSMCHash(SMC_en);

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
            bool bigblock;
            bool corona = false;
            FileInfo f = new FileInfo(filename);
            long s1 = f.Length;
            if (s1 >= 0x4200000) bigblock = true;
            else bigblock = false;
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
                    if (variables.debugme) Console.WriteLine("Found {0}BL at {1}", id, block_offset_b);
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
                            if (variables.cpkey != "")
                            {
                                cb_dec = decrypt_CB_cpukey(CB_B, decrypt_CB(CB_A), Oper.StringToByteArray(variables.cpkey));
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
                    smc_config_offset = 0xFEB800;
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
                if (variables.debugme) Console.WriteLine(ascii.GetString(data));
                if (data[0] == 0x46 && data[1] == 0x57 && data[2] == 0x41 && data[3] == 0x00) Console.WriteLine("DemoN fw");
                else if (size != 0x40000) Console.WriteLine("Header is wrong..");
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

            keyvault.Replace(dvdkey_b, 0x100, 0x10);
            keyvault[0xC8] = region_b[0];
            keyvault[0xC9] = region_b[1];
            keyvault.Replace(osig_b, 0xC8A, 40);
            keyvault.Replace(cid_b, 0x9CA, 5);
            keyvault.Replace(serial_b, 0xB0, 12);
        }

        public static void decrypt_fcrt(byte[] fcrt, byte[] cpukey)
        {
            if (fcrt == null) return;
            try
            {
                if (variables.debugme) Console.WriteLine(cpukey.Length);
                if (variables.debugme) Console.WriteLine(fcrt.Length);
                if (fcrt.Length != 0x4000) { Console.WriteLine("Wrong fcrt.bin size"); return; }
                if (cpukey.Length != 0x10) { Console.WriteLine("Wrong CPU Key size"); return; }
                Console.WriteLine(Environment.NewLine + "Decrypting fcrt.bin");
                int size, offset;
                offset = Oper.ByteArrayToInt(Oper.returnportion(fcrt, 0x11C, 4));
                size = Oper.ByteArrayToInt(Oper.returnportion(fcrt, 0x118, 4));
                if (variables.debugme) Console.WriteLine("offset: {0:X} - size: {1:X}", offset, size);
                byte[] toEncryptArray = Oper.returnportion(fcrt, offset, size); // here here
                if (variables.debugme) Console.WriteLine(toEncryptArray.Length);
                RijndaelManaged rDel = new RijndaelManaged();
                rDel.IV = Oper.returnportion(fcrt, 0x100, 0x10);
                rDel.Key = cpukey;
                rDel.Mode = CipherMode.CBC; // http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
                rDel.Padding = PaddingMode.None; // better lang support
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                if (variables.debugme) Console.WriteLine(resultArray.Length);
                Console.WriteLine("Checking hash");
                SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                byte[] Hash1 = sha.ComputeHash(resultArray);
                if (variables.debugme) Console.WriteLine("{0} - {1}", Oper.ByteArrayToString(Hash1), Hash1.Length);
                if (variables.debugme) Console.WriteLine(Oper.ByteArrayToString(Oper.returnportion(fcrt, 0x12C, 0x14)));
                if (Oper.ByteArrayCompare(Hash1, Oper.returnportion(fcrt, 0x12C, 0x14), 0x14)) Console.WriteLine("Decrypted Successfully");
                else Console.WriteLine("Failed");
                Oper.savefile(Oper.concatByteArrays(Oper.returnportion(fcrt, 0, offset), resultArray, offset, resultArray.Length), Path.Combine(variables.outfolder, "fcrt_dec.bin"));
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }
        }
        public static bool decrypt_fcrt(byte[] fcrt, byte[] cpukey, out byte[] fcrtd)
        {
            fcrtd = null;
            if (fcrt == null) return false;
            try
            {
                if (variables.debugme) Console.WriteLine(cpukey.Length);
                if (variables.debugme) Console.WriteLine(fcrt.Length);
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
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }
            return false;
        }

        public static void injectXell(string filename, byte[] xell, bool ecc)
        {
            int[] xelloffsets = {0x70000, // ggBoot main xell-gggggg
								0x95060, // FreeBOOT Single-NAND main xell-2f
								0x100000, // XeLL-Only Image
								0xC0000,
                                0xE0000,
                                0xB80000};

            byte[] doublexell = new byte[xell.Length * 2];
            Buffer.BlockCopy(xell, 0, doublexell, 0, xell.Length);
            Buffer.BlockCopy(xell, 0, doublexell, xell.Length, xell.Length);
            int blocksize = 0x4000;
            int startblock = 0x30;
            if (ecc)
            {
                blocksize = 0x4200;
                doublexell = addecc_v2(doublexell, true, startblock * blocksize, 1);
            }
            BinaryWriter bw = new BinaryWriter(new FileStream(filename, FileMode.Open, FileAccess.ReadWrite));

            bw.Seek(startblock * blocksize, SeekOrigin.Begin);
            bw.Write(doublexell);
            bw.Close();
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
        public static bool VerifyKey(byte[] key)
        {
            if (key == null || key.Length != 0x10) return false;

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
            if (xval_h == 0 && xval_l == 0) Console.WriteLine("Secdata is clean");
            else if (xval_h == 0xFFFF && xval_l == 0xFFFF) Console.WriteLine("Secdata is invalid");
            else if (xval_h != 0 && xval_l != 0) Console.WriteLine("Secdata decryption error");
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
                if (variables.debugme) Console.WriteLine("decrypting smc");
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
            if (variables.debugme) Console.WriteLine("Offset: {0:X} - Length {1:X} - corona: {2}", fcrt_offset, fcrt_length, corona);
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
            if (!String.IsNullOrEmpty(cpukey)) decrypt_fcrt(searched, Oper.StringToByteArray(cpukey));
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
            if (variables.debugme) Console.WriteLine("Offset: {0:X} - Length {1:X}", secdata_offset, secdata_length);
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
                    if (String.IsNullOrEmpty(filename))
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
                        catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
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
                    if (String.IsNullOrEmpty(filename))
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
                        catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
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
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
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
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
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
        public static byte[] addecc_v2(byte[] image, bool addecc, int blockstart, int layout)
        {
            //int counter = 0;
            if (variables.extractfiles) Oper.savefile(image, "test.bin");
            if (variables.debugme) Console.WriteLine("blockstart: {0:X}, layout: {1}", blockstart / 0x4200, layout);
            if (!addecc)
            {
                if (hasecc(image)) unecc(ref image);
            }
            int datalen = image.Length;
            byte[] d, data = image, result = { };
            for (int i = 0; i < datalen / 0x200; i++)
            {
                byte[] sparedata = new byte[0x10];
                d = Oper.returnportion(Oper.padto(data, 0x00, 0x200), 0, 0x200);
                data = Oper.returnportion(data, 0x200, data.Length - 0x200);
                switch (layout)
                {
                    case 0:
                        sparedata[5] = 0xFF;
                        sparedata[0] = (byte)(((i / 32) + (blockstart / 0x4200)) & 0xFF);
                        sparedata[1] = (byte)(((i / 32) + (blockstart / 0x4200)) / 0x100);
                        break;
                    case 1:
                        sparedata[5] = 0xFF;
                        sparedata[1] = (byte)(((i / 32) + (blockstart / 0x4200)) & 0xFF);
                        sparedata[2] = (byte)(((i / 32) + (blockstart / 0x4200)) / 0x100);
                        break;
                    case 2:
                        sparedata[0] = 0xFF;
                        sparedata[1] = (byte)(((i / 0x100) + (blockstart / 0x21000)) & 0xFF);
                        sparedata[2] = (byte)((((i / 0x100) + (blockstart / 0x21000)) & 0xFF00) >> 8);
                        break;
                    default:
                        break;
                }

                d = Oper.addtoflash_v2(d, sparedata);
                try
                {
                    d = calcecc(d);
                }
                catch (System.IndexOutOfRangeException) { Oper.ByteArrayToString(d); }
                result = Oper.addtoflash_v2(result, d);
            }
            return result;
        }

        private static byte[] calcecc(byte[] data)
        {
            if (data.Length != 0x210) Console.WriteLine("Fuck");
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
                            if (variables.debugme) Console.WriteLine("Sparer {0:X}", i);
                            return true;
                        }
                        break;
                    default:
                        Buffer.BlockCopy(data, i, sparedata, 0, 0x10);
                        i += 0x10;
                        if ((sparedata[0] == 0xFF || sparedata[5] == 0xFF) && !Oper.allsame(Oper.returnportion(sparedata, 0xC, 0x4), 0xFF)
                            && !Oper.allsame(Oper.returnportion(sparedata, 0xC, 0x4), 0x00) && sparedata[3] == 0x00 && sparedata[4] == 0x00)
                        {
                            if (variables.debugme) Console.WriteLine("Spare {0:X}", i);
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
                            if (variables.debugme) Console.WriteLine("Sparer {0:X}", i);
                            return true;
                        }
                        break;
                    default:
                        Buffer.BlockCopy(data, i, sparedata, 0, 0x10);
                        i += 0x10;
                        if ((sparedata[0] == 0xFF || sparedata[5] == 0xFF) && !Oper.allsame(Oper.returnportion(sparedata, 0xC, 0x4), 0xFF)
                            && !Oper.allsame(Oper.returnportion(sparedata, 0xC, 0x4), 0x00) && sparedata[3] == 0x00 && sparedata[4] == 0x00)
                        {
                            if (variables.debugme) Console.WriteLine("Spare {0:X}", i);
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
                if (data[block_offset_b] == 0x43 && data[block_offset_b + 1] == 0x42)
                {
                    int length = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(data, block_offset_b + 0xC, 4)), 16);
                    if (data.Length < block_offset_b + length || length < 0) return hasecc(ref data);
                    else
                    {
                        block_offset_b = block_offset_b + length;
                        if (data[block_offset_b] == 0x43 && (data[block_offset_b + 1] == 0x42 || data[block_offset_b + 1] == 0x44)) return false;
                        else return true;
                    }
                }
                else return true;
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
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }

            return data;
        }
        public static void unecc(ref byte[] data, bool print = false)
        {
            if (variables.debugme) Console.WriteLine("On unecc");
            int counter = 0;
            try
            {
                if (data[0x205] == 0xFF || data[0x415] == 0xFF || data[0x200] == 0xFF)
                {
                    if (print || variables.debugme) Console.WriteLine("ECC'ed - will unecc.");
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
                    if (print || variables.debugme) Console.WriteLine("ECC'ed BB - will unecc.");
                    byte[] res = new byte[(data.Length / 0x840) * 0x800];
                    for (counter = 0; counter < res.Length; counter += 0x800)
                    {
                        if (((counter / 0x800) * 0x840) + 0x800 <= data.Length) Buffer.BlockCopy(data, (counter / 0x800) * 0x840, res, counter, 0x800);
                    }
                    data = res;
                    res = null;
                }
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }
        }
        public static void unecc(ref byte[] data, ref ProgressBar pb, bool print = false)
        {
            int counter = 0;
            try
            {
                if (data[0x205] == 0xFF || data[0x415] == 0xFF || data[0x200] == 0xFF)
                {
                    if (print || variables.debugme) Console.WriteLine("ECC'ed - will unecc.");
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
                    if (print || variables.debugme) Console.WriteLine("ECC'ed BB - will unecc.");
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
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); else Console.WriteLine(ex.Message); }
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