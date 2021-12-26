using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JRunner
{
    public class ECC
    {
        struct eccs
        {
            public byte[] Header;
            public byte[] SMC;
            public byte[] CB_A;
            public byte[] CB_B;
            public byte[] CD;
            public byte[] CE;
            public byte[] CD_plain;
            public byte[] CB_A_crypted;
            public byte[] Xell;
            public byte[] Keyvault;
        }

        #region variables
        private bool donor = false;
        private bool hasecc = true;
        //static byte[] secret_1bl = { 0xDD, 0x88, 0xAD, 0x0C, 0x9E, 0xD6, 0x69, 0xE7, 0xB5, 0x67, 0x94, 0xFB, 0x68, 0x56, 0x3E, 0xFA };
        //static byte[] random = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        //static string signature = "D6A582FC";
        //static string signature1 = "52A92D9";
        private int XELL_BASE_FLASH = 0xc0000;
        private string CODE_BASE = "0x1c000000";
        private Regex objAlphaPattern = new Regex("[a-fA-F0-9]{32}$");
        //private static byte[] copyright = { 0x3C, 0x43, 0x6F, 0x70, 0x79, 0x72, 0x69, 0x67, 0x68, 0x74, 0x20, 0x32, 0x30, 0x30, 0x31, 0x2D };
        int lastloc = 0;
        //static Encoding enc8 = Encoding.UTF8;
        Encoding ascii = Encoding.ASCII;
        #endregion

        /// <summary>
        /// Ecc Creation
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="outputfolder"></param>
        /// <param name="pb"></param>
        /// <returns></returns>
        #region ecc creation

        private void creatergh2eccinit(ref eccs dt)
        {
            ///
            /// Paths
            ///
            string pathforit = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string xellfile = Path.Combine(pathforit, @"common/xell/xell-gggggg.bin");
            string cdfile = Path.Combine(pathforit, @"common/cdxell/CD");

            long size = 0;
            // cd file
            {
                dt.CD_plain = Oper.openfile(cdfile, ref size, 1 * 1024 * 1024);
                Console.WriteLine("* found decrypted CD");
            }
            // xell file
            {
                byte[] data = Oper.openfile(xellfile, ref size, 1 * 1024 * 1024);
                Console.WriteLine("* found XeLL binary, must be linked to {0}", CODE_BASE);
                dt.Xell = Oper.padto(data, 0x00, 256 * 1024);
            }
        }
        private void createrghheader(ref eccs dt, string c)
        {
            int base_size = 0x8000 + dt.CB_A.Length + dt.CD.Length + dt.CE.Length;
            if (dt.CB_B != null) base_size += dt.CB_B.Length;
            base_size += 16383;
            base_size &= ~16383;

            int patch_offset = base_size;

            Console.WriteLine(" * base size: {0}", base_size.ToString("X"));

            byte[] cbyt = ascii.GetBytes(c);
            byte[] base_size_array = Oper.StringToByteArray(base_size.ToString("X"));
            byte[] patch_offset_array = Oper.StringToByteArray(patch_offset.ToString("X"));
            byte[] smc_offset_array = Oper.StringToByteArray_v2((0x4000 - dt.SMC.Length).ToString("X"), 4);
            byte[] smc_size_array = Oper.StringToByteArray_v2(dt.SMC.Length.ToString("X"), 4);
            byte[] header = new byte[0x1000];
            ///
            /// Fucking Header
            /// 
            header[0x00] = 0xFF; header[0x01] = 0x4F; header[0x02] = 0x07; header[0x03] = 0x60;
            for (int bytes = 0x04; bytes < 0x08; bytes++) header[bytes] = 0x00;
            header[0x08] = 0x00; header[0x09] = 0x00; header[0x0A] = 0x80; header[0x0B] = 0x00;
            for (int bytes = 0x0C; bytes < 0x10; bytes++) header[bytes] = base_size_array[bytes - 0x0c];
            for (int bytes = 0x10; bytes < 0x50; bytes++)
            {
                if (bytes - 0x10 >= cbyt.Length) header[bytes] = 0x00;
                else header[bytes] = cbyt[bytes - 0x10];
            }
            for (int bytes = 0x50; bytes < 0x62; bytes++) header[bytes] = 0x00;
            header[0x62] = 0x40; header[0x63] = 0x00;
            for (int bytes = 0x64; bytes < 0x68; bytes++) header[bytes] = patch_offset_array[bytes - 0x64];
            header[0x68] = 0x00; header[0x69] = 0x02; header[0x6A] = 0x07; header[0x6B] = 0x12;
            header[0x6C] = 0x00; header[0x6D] = 0x00; header[0x6E] = 0x40; header[0x6F] = 0x00;
            for (int bytes = 0x70; bytes < 0x78; bytes++) header[bytes] = 0x00;
            for (int bytes = 0x78; bytes < 0x7C; bytes++) header[bytes] = smc_size_array[bytes - 0x78];
            for (int bytes = 0x7C; bytes < 0x80; bytes++) header[bytes] = smc_offset_array[bytes - 0x7C];
            header = Oper.padto(header, 0x00, 0x1000);
            dt.Header = header;
            ///
            ///
            cbyt = null; base_size_array = null; patch_offset_array = null;
        }

        public int creatergh2ecc(string filename, string outputfolder, ref ProgressBar pb, string cpukey)
        {
            eccs dt = new eccs();
            byte[] data;
            int layout;
            bool rgh2 = false;

            bool sts = objAlphaPattern.IsMatch(cpukey);
            if (variables.rgh2 && !String.IsNullOrEmpty(cpukey) && sts) rgh2 = true;

            long size = 0;
            string imagefile = filename;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            if (variables.debugme) Console.WriteLine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            string pathforit = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string cdrgh2file = Path.Combine(pathforit, @"common/cdxell/CDRGH2");
            string cdfile = Path.Combine(pathforit, @"common/cdxell/CD");
            string cdjasperfile = Path.Combine(pathforit, @"common/cdxell/CDjasper");

            /// nand image
            {
                byte[] bbimage = Nand.BadBlock.find_bad_blocks_X(imagefile, 0x50);
                data = Oper.returnportion(ref bbimage, 0, 1 * 1024 * 1024);
                bbimage = null;

                layout = Nand.Nand.getConsole(new Nand.PrivateN(filename)).Layout;

                if (Oper.allsame(Oper.returnportion(ref data, 0x210, 0x200), 0xFF))
                {
                    Console.WriteLine("Invalid Image");
                    return -1;
                }
                Console.WriteLine("* unpacking flash image, ....");
                unpack_base_image_ecc(ref data, ref pb, ref dt);
                dt.CB_A_crypted = dt.CB_A;
                dt.SMC = decrypt_SMC(dt.SMC);
            }

            creatergh2eccinit(ref dt);
            ///
            ///Finished Loading images
            ///
            data = null;
            if (dt.CD_plain == null) return -1;
            if (dt.SMC == null) return -1;
            ///
            Console.WriteLine(" * we found the following parts: ");
            Console.WriteLine("SMC: {0}.{1}", (dt.SMC[0x101].ToString()), (dt.SMC[0x102]).ToString());
            ///
            if (dt.CB_A != null) Console.WriteLine("CB_A: {0}", Oper.ByteArrayToInt(build(dt.CB_A))); else Console.WriteLine("CB_A: missing");
            if (dt.CB_B != null) Console.WriteLine("CB_B: {0}", Oper.ByteArrayToInt(build(dt.CB_B))); else Console.WriteLine("CB_B: missing");

            ///
            ///
            int[] rghcbs = { 6753, 6752, 5773, 4578, 4577, 5772, 9230 };
            int[] rgh1cbs = { 9188 };
            //if (rghcbs.Contains(bytearray_to_int(build(CB_A)))) rgh2 = true;
            if (dt.CB_B != null && !rgh1cbs.Contains(Oper.ByteArrayToInt(build(dt.CB_B)))) rgh2 = true;
            /// 
            ///
            string c;

            if (rgh2)
            {
                dt.CD_plain = Oper.openfile(cdrgh2file, ref size, 1 * 1024 * 1024);
                if (dt.CD_plain == null) return -1;
                if (dt.CD != null) Console.WriteLine("CD (image): {0}", Oper.ByteArrayToInt(build(dt.CD))); else Console.WriteLine("CD (image): missing");
                if (dt.CD_plain != null) Console.WriteLine("CD (decrypted): {0}", Oper.ByteArrayToInt(build(dt.CD_plain))); else Console.WriteLine("CD (decrypted): missing");
                byte[] CB_A_img_RAND = { };
                CB_A_img_RAND = Oper.returnportion(ref dt.CB_A_crypted, 0x10, 0x10);
                ///
                byte[] CB_A_img = Nand.Nand.decrypt_CB(dt.CB_A_crypted);
                Console.WriteLine(" * checking required versions...");

                int[] zephyr_builds = { 4578, 4577, 4575, 4560, 4576 };
                int[] falcon_builds = { 5771, 5772, 5773 };
                int[] jasper_builds = { 6750, 6752, 6753, 6754 };
                int[] trinity_builds = { 9188, 9230, 9231 };
                int[] corona_builds = { 13121, 13180 };
                int[] xor_hack_builds = { 9230, 9231, 5773, 6753, 6754, 4575, 4560, 13180 };
                int[] patch_builds = { 5772, 6752, 9188, 13121 };

                if (!zephyr_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A))) &&
                    !falcon_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A))) &&
                    !jasper_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A))) &&
                    !corona_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A))) &&
                    !trinity_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A)))) return -3;
                if (!xor_hack_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A))) &&
                    !patch_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A))) && String.IsNullOrEmpty(cpukey)) return -4;

                Console.WriteLine("ok");

                Console.WriteLine(" * patching SMC...");
                dt.SMC = patch_SMC(dt.SMC);

                Console.WriteLine(" * Replacing CD...");
                dt.CD = dt.CD_plain;
                dt.CD_plain = null;

                string cbapath = "", cbbpath = "";

                if (falcon_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A))))
                {
                    cbapath = Path.Combine(pathforit, "common\\CB\\CB_A.5772.bin");
                    cbbpath = Path.Combine(pathforit, "common\\CB\\CB_B.5772.bin");
                }
                else if (jasper_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A))))
                {
                    cbapath = Path.Combine(pathforit, "common\\CB\\CB_A.6752.bin");
                    cbbpath = Path.Combine(pathforit, "common\\CB\\CB_B.6752.bin");
                }
                else if (trinity_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A))))
                {
                    cbapath = Path.Combine(pathforit, "common\\CB\\CB_A.9188.bin");
                    cbbpath = Path.Combine(pathforit, "common\\CB\\CB_B.9188.bin");
                }
                else if (zephyr_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A))))
                {
                    cbapath = Path.Combine(pathforit, "common\\CB\\CB_A.4577.bin");
                    cbbpath = Path.Combine(pathforit, "common\\CB\\CB_B.4577.bin");
                }
                else if (corona_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A))))
                {
                    cbapath = Path.Combine(pathforit, "common\\CB\\CB_A.13121.bin");
                    cbbpath = Path.Combine(pathforit, "common\\CB\\CB_B.13121.bin");
                }

                c = "RGH2 2stage CB img";
                cpukey = "";
            wtf:
                if (String.IsNullOrEmpty(cpukey))
                {
                    if (patch_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_B))))
                    {
                        Console.WriteLine(" * patching CB_B...");
                        dt.CB_B = patch_CB(dt.CB_B);
                        dt.CB_A = dt.CB_A_crypted;
                    }
                    else if (xor_hack_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_B))))
                    {
                        Console.WriteLine();
                        Console.WriteLine("* XOR HACK NEEDED FOR CB {0}", Oper.ByteArrayToInt(build(dt.CB_B)));

                        byte[] CB_B_plain = Oper.openfile(Path.Combine(variables.pathforit, @"common\CB\CB_B." + Oper.ByteArrayToInt(build(dt.CB_B)) + ".bin"), ref size, 0);
                        if (CB_B_plain == null) { Console.WriteLine("Failed to open CB_B.{0}.bin", Oper.ByteArrayToInt(build(dt.CB_B))); return 5; }

                        byte[] CB_B_patched = Oper.openfile(cbbpath, ref size, 0);
                        if (CB_B_patched == null) { Console.WriteLine("Failed to open {0}", cbbpath); return 5; }
                        Console.WriteLine(" * patching CB_B...");

                        CB_B_patched = patch_CB(CB_B_patched);
                        if (CB_B_patched == null) { Console.WriteLine("Failed to patch the CB_B"); return 5; }
                        Console.WriteLine(" * Applying XOR Hack to CB_B {0}", Oper.ByteArrayToInt(build(dt.CB_B)));

                        dt.CB_B = xor_hack(dt.CB_B, CB_B_plain, CB_B_patched);
                        Console.WriteLine(" * Replacing CB_A {0} with {1}", Oper.ByteArrayToInt(build(dt.CB_A)), Oper.ByteArrayToInt(build(CB_B_patched)));

                        dt.CB_A = Oper.openfile(cbapath, ref size, 0);
                        if (dt.CB_A == null) { Console.WriteLine("Failed to open {0}", cbapath); return 5; }
                        byte[] CB_A_key = { };
                        dt.CB_A = Nand.Nand.encrypt_CB(dt.CB_A, CB_A_img_RAND, ref CB_A_key);
                    }
                }
                else
                {

                    if (MessageBox.Show("Are you sure you want to build a new bootloader chain?", "Bootloader Chain", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        cpukey = "";
                        goto wtf;
                    }
                    Console.WriteLine("\n * Building new bootloader chain using cpu_key: {0}", cpukey);

                    dt.CB_A = Oper.openfile(cbapath, ref size, 0);
                    dt.CB_B = Oper.openfile(cbbpath, ref size, 0);

                    dt.CB_B = patch_CB(dt.CB_B);

                    byte[] CB_A_key = { };
                    dt.CB_A = Nand.Nand.encrypt_CB(dt.CB_A, CB_A_img_RAND, ref CB_A_key);
                    dt.CB_B = Nand.Nand.encrypt_CB_cpukey(dt.CB_B, CB_A_key, Oper.StringToByteArray(cpukey));

                }
                ///
                ///
                ///
                Console.WriteLine(" * constructing new image...");

                c = c + ", CB=" + Oper.ByteArrayToInt(build(dt.CB_A));
            }
            else
            {
                if (dt.CD != null) Console.WriteLine("CD (image): {0}", Oper.ByteArrayToInt(build(dt.CD))); else Console.WriteLine("CD (image): missing");
                if (dt.CD_plain != null) Console.WriteLine("CD (decrypted): {0}", Oper.ByteArrayToInt(build(dt.CD_plain))); else Console.WriteLine("CD (decrypted): missing");
                int[] jasper_builds = { 6750, 6752, 6753 };
                if (jasper_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A)))) dt.CD_plain = Oper.openfile(cdjasperfile, ref size, 1 * 1024 * 1024);
                if (dt.CD_plain == null) return -1;
                byte[] CB_A_img_RAND = { };
                if (!donor)
                {
                    CB_A_img_RAND = Oper.returnportion(ref dt.CB_A_crypted, 0x10, 0x10);
                }
                ///
                if (Oper.ByteArrayToInt(build(dt.CB_A)) >= 1888 && Oper.ByteArrayToInt(build(dt.CB_A)) <= 1940 || Oper.ByteArrayToInt(build(dt.CB_A)) == 7373 || Oper.ByteArrayToInt(build(dt.CB_A)) == 8192)
                {
                    Console.WriteLine(" * using donor CB");
                    dt.CB_A_crypted = null;
                    if (MessageBox.Show("There have been various reports that using a different bootloader improves the glitch speeds on xenon. Click Yes to use the 7375 Bootloader or Click No to use the 1940 one.", "Choose CB", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        dt.CB_A = Oper.openfile(Path.Combine(variables.pathforit, @"common\CB\CB_A.7375.bin"), ref size, 0);
                    }
                    else
                    {
                        dt.CB_A = Oper.openfile(Path.Combine(variables.pathforit, @"common\CB\CB_A.1940.bin"), ref size, 0);
                    }
                    if (dt.CB_A == null) { Console.WriteLine("Failed to open CB"); return 5; }
                    dt.CB_B = null;
                    dt.CD_plain = Oper.openfile(cdfile, ref size, 1 * 1024 * 1024);
                    if (dt.CD_plain == null) return -1;
                }
                ///
                ///
                ///
                int[] xenon_builds = { 1923, 7375 };
                int[] zephyr_builds = { 4578, 4579, 4575 };
                int[] falcon_builds = { 5771, 5772, 5773 };
                //int[] jasper_builds = { 6750, 6752, 6753 };
                int[] trinity_builds = { 9188, 9230 };
                int[] corona_builds = { 13121, 13180 };
                int[] slim_builds = { 9188, 9230, 13121 };
                int[] xor_hack_builds = { 9230, 5773, 6753, 4575 };

                if (!donor)
                {
                    if ((dt.CB_A_crypted != null) && (Oper.ByteArrayToInt(build(dt.CB_A)) != 9230))
                    {
                        dt.CB_A = Nand.Nand.decrypt_CB(dt.CB_A_crypted);
                        if (dt.CB_A == null) Console.WriteLine("Error");
                    }
                }
                if (dt.CB_B != null && !slim_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A))))
                {
                    Console.WriteLine("Not supported yet");
                    return 5;
                }


                if (trinity_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A)))) Console.WriteLine(" * this image will be valid *only* for: trinity (slim)");
                else if (corona_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A)))) Console.WriteLine(" * this image will be valid *only* for: corona");
                else if (zephyr_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A)))) Console.WriteLine(" * this image will be valid *only* for: zephyr");
                else if (falcon_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A)))) Console.WriteLine(" * this image will be valid *only* for: falcon");
                else if (jasper_builds.Contains(Oper.ByteArrayToInt(build(dt.CB_A))))
                {
                    if (donor) Console.WriteLine(" * this image will be valid *only* for: jasper (CB_6751)");
                    else Console.WriteLine(" * this image will be valid *only* for: jasper (CB_6750)");
                }
                else Console.WriteLine(" * this image will be valid *only* for: xenon");

                Console.WriteLine(" * patching SMC...");
                dt.SMC = patch_SMC(dt.SMC);

                dt.CD = dt.CD_plain;
                dt.CD_plain = null;

                if (dt.CB_B != null)
                {
                    Console.WriteLine(" * patching CB_B...");
                    dt.CB_B = patch_CB(dt.CB_B);
                    c = "patched CB img";
                }
                else
                {
                    //Nand.savefile(CB_A, "CB_f.bin");
                    Console.WriteLine(" * zero-pairing...");
                    for (int bytes = 0x20; bytes < 0x40; bytes++) dt.CB_A[bytes] = 0x00;
                    c = "zeropair image";
                }
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "output"))) Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "output"));
                ///
                ///
                ///
                Console.WriteLine(" * constructing new image...");
                byte[] random = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                c = c + ", version=01, CB=" + Oper.ByteArrayToInt(build(dt.CB_A));

                if (dt.CB_B == null)
                {
                    byte[] CB_A_key = { };
                    //Nand.savefile(CB_A, "CB_d.bin");
                    dt.CB_A = Nand.Nand.encrypt_CB(dt.CB_A, random, ref CB_A_key);
                    //Nand.savefile(CB_A, "CB_e.bin");
                    //Console.WriteLine(ByteArrayToString(CB_A_key));
                    dt.CD = Nand.Nand.encrypt_CD(dt.CD, random, CB_A_key);
                }
            }

            createrghheader(ref dt, c);
            ///
            dt.SMC = encrypt_SMC(dt.SMC);
            ///
            ///
            byte[] newXell = dt.Xell;
            if (dt.Xell.Length <= 256 * 1024)
            {
                Console.WriteLine(" * No separate recovery Xell available!");
                newXell = Oper.concatByteArrays(dt.Xell, dt.Xell, dt.Xell.Length, dt.Xell.Length);
            }
            dt.Xell = null;
            ///
            /// Start of image creation
            ///
            byte[] Final = { };
            Console.WriteLine(" * Flash Layout:");
            Final = addtoflash(Oper.returnportion(ref dt.Header, 0x00, 0x200), Final, "Header", 0x00, 0x200);
            Final = padto_v2(Final, 0x4000 - dt.SMC.Length);
            ///
            Final = addtoflash(dt.SMC, Final, "SMC", Final.Length, dt.SMC.Length);
            dt.SMC = null;
            Final = addtoflash(dt.Keyvault, Final, "Keyvault", Final.Length, dt.Keyvault.Length);
            dt.Keyvault = null;
            ///
            if (dt.CB_B != null)
            {
                if (!rgh2) Final = addtoflash(dt.CB_A_crypted, Final, "CB_A " + Oper.ByteArrayToInt(build(dt.CB_A)), Final.Length, dt.CB_A_crypted.Length);
                else Final = addtoflash(dt.CB_A, Final, "CB_A " + Oper.ByteArrayToInt(build(dt.CB_A)), Final.Length, dt.CB_A.Length);
                Final = addtoflash(dt.CB_B, Final, "CB_B " + Oper.ByteArrayToInt(build(dt.CB_B)), Final.Length, dt.CB_B.Length);
            }
            else Final = addtoflash(dt.CB_A, Final, "CB_A " + Oper.ByteArrayToInt(build(dt.CB_A)), Final.Length, dt.CB_A.Length);
            dt.CB_A = null; dt.CB_B = null; dt.CB_A_crypted = null;
            ///
            Final = addtoflash(dt.CD, Final, "CD " + Oper.ByteArrayToInt(build(dt.CD)), Final.Length, dt.CD.Length);
            dt.CD = null;
            Final = padto_v2(Final, XELL_BASE_FLASH);
            ///
            Final = addtoflash(Oper.returnportion(ref newXell, 0, 256 * 1024), Final, "Xell (backup)", Final.Length, 256 * 1024);
            Final = addtoflash(Oper.returnportion(ref newXell, 256 * 1024, newXell.Length - (256 * 1024)), Final, "Xell (main)", Final.Length, newXell.Length - (256 * 1024));
            newXell = null;
            ///
            if (variables.extractfiles || variables.debugme) Oper.savefile(Final, Path.Combine(outputfolder, "image_no.ecc"));

            if (hasecc)
            {
                Console.Write(" * Encoding ECC...");
                Final = Nand.Nand.addecc_v2(Final, true, 0, layout);
            }
            else
            {
                Console.WriteLine("NOT adding Spare Data");
            }

            Console.WriteLine("Done");
            ///
            Oper.savefile(Final, Path.Combine(outputfolder, "glitch.ecc"));
            DirectoryInfo dinfo = new DirectoryInfo(outputfolder);
            Console.WriteLine("------------- Written into {0}\n", Path.Combine(dinfo.Name, "glitch.ecc"));
            Console.WriteLine("");
            pb.Value = pb.Maximum;
            return 1;
        }

        void unpack_base_image_ecc(ref byte[] image, ref ProgressBar pb, ref eccs dt)
        {
            byte[] data;

            if (Nand.Nand.hasecc(image))
            {
                hasecc = true;
                Console.WriteLine("Spare Data found, will remove.");
                Nand.Nand.unecc(ref image, ref pb);
                Console.WriteLine("Removed");
            }
            else
            {
                hasecc = false;
                Console.WriteLine("Spare data NOT found");
            }

            try
            {
                byte[] block_offset = new byte[4], smc_len = new byte[4], smc_start = new byte[4];
                //block_offset = Nand.returnportion(image, 0x8, 4);
                Buffer.BlockCopy(image, 0x8, block_offset, 0, 4);
                //smc_len = Nand.returnportion(image, 0x78, 4);
                Buffer.BlockCopy(image, 0x78, smc_len, 0, 4);
                //smc_start = Nand.returnportion(image, 0x7C, 4);
                Buffer.BlockCopy(image, 0x7C, smc_start, 0, 4);
                dt.SMC = new byte[Convert.ToInt32(Oper.ByteArrayToString(smc_len), 16)];
                //SMC = Nand.returnportion(image, Convert.ToInt32(Nand.ByteArrayToString(smc_start), 16), Convert.ToInt32(Nand.ByteArrayToString(smc_len), 16));
                Buffer.BlockCopy(image, Convert.ToInt32(Oper.ByteArrayToString(smc_start), 16), dt.SMC, 0, Convert.ToInt32(Oper.ByteArrayToString(smc_len), 16));
                if (variables.extractfiles) Oper.savefile(dt.SMC, "output\\SMC_en.bin");
                #region keyvault
                dt.Keyvault = new byte[0x4000];
                Buffer.BlockCopy(image, 0x4000, dt.Keyvault, 0, 0x4000);
                //Keyvault = Nand.returnportion(image, 0x4000, 0x4000);
                if (variables.extractfiles) Oper.savefile(dt.Keyvault, "output\\KV_en.bin");
                #endregion


                #region blocks

                int block = 0, block_size, id;
                byte block_id;
                int block_build;
                byte[] block_build_b = new byte[2], block_size_b = new byte[4];
                int block_offset_b = Convert.ToInt32(Oper.ByteArrayToString(block_offset), 16);
                int semi = 0;
                for (block = 0; block < 30; block++)
                {
                    block_id = image[block_offset_b + 1];
                    //block_build_b = Nand.returnportion(image, block_offset_b + 2, 2);
                    Buffer.BlockCopy(image, block_offset_b + 2, block_build_b, 0, 2);
                    //block_size_b = Nand.returnportion(image, block_offset_b + 12, 4);
                    Buffer.BlockCopy(image, block_offset_b + 12, block_size_b, 0, 4);
                    block_size = Convert.ToInt32(Oper.ByteArrayToString(block_size_b), 16);
                    block_build = Convert.ToInt32(Oper.ByteArrayToString(block_build_b), 16);
                    block_size += 0xF;
                    block_size &= ~0xF;
                    id = block_id & 0xF;

                    if (variables.debugme) Console.WriteLine("Found {0}BL (build {1}) at {2}", id, block_build, Convert.ToString(block_offset_b, 16));
                    data = null;
                    try
                    {
                        data = new byte[block_size];
                        //data = Nand.returnportion(image, block_offset_b, block_size);
                        Buffer.BlockCopy(image, block_offset_b, data, 0, block_size);
                    }
                    catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
                    if (id == 2)
                    {
                        if (semi == 0)
                        {
                            if (block_build == 6751)
                            {
                                Console.WriteLine("A donor version will be used with this CB");
                                long csize = 0;
                                dt.CB_A = Oper.openfile(Path.Combine(variables.pathforit, "common/CB/cb_6750.bin"), ref csize, 0);
                                if (dt.CB_A == null) Console.WriteLine("CB_A 6750 file is missing!!!");
                                donor = true;
                            }
                            else if (block_build == 5771)
                            {
                                if (variables.debugme) Console.WriteLine("This CB version is for a Falcon");
                                dt.CB_A = data;
                            }
                            else dt.CB_A = data;
                            semi = 1;
                        }
                        else if (semi == 1)
                        {
                            dt.CB_B = data;
                            semi = 0;
                        }
                    }
                    else if (id == 4)
                    {
                        dt.CD = data;
                    }
                    else if (id == 5)
                    {
                        dt.CE = data;
                    }

                    block_offset_b += block_size;
                    if (id == 5) break;
                }
                #endregion
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
        }
        #endregion

        /// <summary>
        /// addtoflash - padto
        /// </summary>
        /// <param name="image"></param>
        /// <param name="padding"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        #region addtoflash - padto

        private byte[] padto_v2(byte[] image, int length)
        {
            byte[] padding = new byte[length];
            for (int i = 0; i < length; i++) padding[i] = 0xFF;
            byte[] newimage = addtoflash(padding, image, "Padding", image.Length, (length - image.Length));
            return newimage;

        }

        private byte[] addtoflash(byte[] data, byte[] image, string what, int offset, int length)
        {
            byte[] tempimage = new byte[image.Length + length];
            int i;
            Console.WriteLine("0x{0}..0x{1} (0x{2} bytes) {3}", (lastloc + offset).ToString("X"), (offset + lastloc + length - 1).ToString("X"), (length).ToString("X"), what);
            for (i = 0; i < image.Length; i++)
            {
                tempimage[i] = image[i];
            }
            for (; i < offset + length; i++)
            {
                tempimage[i] = data[i - image.Length];
            }
            return tempimage;
        }

        #endregion

        #region ecc CB operations

        private byte[] patch_CB(byte[] CB)
        {
            if (CB == null) return null;
            int[] patchsets = { 9188, 6752, 5772, 4577, 13121 };

            int[] CB_patches_offsets_9188 = { 0x4f08, 0x5618, 0x5678, 0x4d10 };
            string[][] CB_patches_9188 = { new string[] { "409a0010", "60000000" }, new string[] { "480018e1", "60000000" }, new string[] { "480000b9", "60000000" }, new string[] { "7BEB0620", "48000168" } };
            int[] CB_patches_offsets_6752 = { 0x6AA0, 0x71B0, 0x7200, 0x68A8 };
            string[][] CB_patches_6752 = { new string[] { "409a0010", "60000000" }, new string[] { "480018D9", "60000000" }, new string[] { "419A0014", "48000014" }, new string[] { "7BEB0620", "48000168" } };
            int[] CB_patches_offsets_5772 = { 0x6A58, 0x7168, 0x71b8, 0x6860 };
            string[][] CB_patches_5772 = { new string[] { "409a0010", "60000000" }, new string[] { "480018e1", "60000000" }, new string[] { "419A0014", "48000014" }, new string[] { "7BEB0620", "48000168" } };
            int[] CB_patches_offsets_4577 = { 0x53C0, 0x55B8, 0x5D54, 0x5DB0 };
            string[][] CB_patches_4577 = { new string[] { "7beb0620", "48000168" }, new string[] { "409a0010", "60000000" }, new string[] { "480018c5", "60000000" }, new string[] { "480000a1", "60000000" } };
            int[] CB_patches_offsets_13121 = { 0x5240, 0x5938, 0x59B8, 0x5048, 0x58AC };
            string[][] CB_patches_13121 = { new string[] { "409A0010", "60000000" }, new string[] { "48001859", "60000000" }, new string[] { "480000b9", "60000000" }, new string[] { "7BEB0620", "48000168" }, new string[] { "48000195", "60000000" } };
            //[13121,[[0x5240,0x409A0010,0x60000000],[0x5938,0x48001859,0x60000000],[0x59B8,0x480000b9,0x60000000], [0x5048, 0x7BEB0620, 0x48000168], [0x58AC, 0x48000195, 0x60000000]]]
            int[] CB_patches_offsets = { };
            string[][] CB_patches = { new string[] { } };
            /*
             *int[] CB_patches_offsets = { 0x4ecc, 0x4e40, 0x4df4, 0x4f08, 0x5618, 0x5678  };
             *string[][] CB_patches_9188 = { new string[] { "409a0010", "60000000" }, new string[] { "409A0038", "48000038" },new string[] { "409a0010", "60000000" }, 
             *                              new string[] { "409a0010", "60000000" }, new string[] { "480018e1", "60000000" }, new string[] { "480000b9", "60000000" } };
             */
            bool found = false;

            foreach (int patchCB in patchsets)
            {
                if (Oper.ByteArrayToInt(build(CB)) == patchCB)
                {
                    if (patchCB == 9188)
                    {
                        CB_patches_offsets = CB_patches_offsets_9188;
                        CB_patches = CB_patches_9188;
                    }
                    else if (patchCB == 6752)
                    {
                        CB_patches_offsets = CB_patches_offsets_6752;
                        CB_patches = CB_patches_6752;
                    }
                    else if (patchCB == 5772)
                    {
                        CB_patches_offsets = CB_patches_offsets_5772;
                        CB_patches = CB_patches_5772;
                    }
                    else if (patchCB == 4577)
                    {
                        CB_patches_offsets = CB_patches_offsets_4577;
                        CB_patches = CB_patches_4577;
                    }
                    else if (patchCB == 13121)
                    {
                        CB_patches_offsets = CB_patches_offsets_13121;
                        CB_patches = CB_patches_13121;
                    }
                    if (variables.debugme) Console.WriteLine("{0} patches selected", patchCB);

                    Console.WriteLine("patchset for {0} found, {1} patch(es)", patchCB, CB_patches_offsets.Length);
                    found = true;
                    int keystream, patched;

                    for (int j = 0; j < CB_patches_offsets.Length; j++)
                    {

                        for (int i = 0; i < 4; i++)
                        {
                            keystream = Oper.StringToByteArray(CB_patches[j][0])[i] ^ (CB[i + CB_patches_offsets[j]]);
                            patched = (keystream ^ (Oper.StringToByteArray(CB_patches[j][1])[i]));
                            CB[CB_patches_offsets[j] + i] = (byte)patched;
                        }
                    }
                }
            }
            if (!found)
            {
                Console.WriteLine("can't patch that CB");
                //Exit
            }
            return CB;
        }

        private byte[] xor_hack(byte[] CB_B, byte[] CB_B_plain, byte[] CB_B_patched)
        {
            int headerlen = 0x40;
            int offset = headerlen;

            int keystream, patched;

            Console.WriteLine(" *** Re-encrypting CB_B with xor keystream");
            int j = 0;
            for (j = 0; j < (CB_B_patched.Length - headerlen) / 4; j++)
            {
                byte[] plain = Oper.returnportion(ref CB_B_plain, offset, 4);
                byte[] patch = Oper.returnportion(ref CB_B_patched, offset, 4);

                for (int i = 0; i < 4; i++)
                {
                    keystream = plain[i] ^ (CB_B[i + offset]);
                    patched = (keystream ^ (patch[i]));
                    CB_B[offset + i] = (byte)patched;
                }

                offset += 4;
            }
            Console.WriteLine(" *** Fixing entrypoint");
            CB_B = Oper.concatByteArrays(CB_B_patched, Oper.returnportion(ref CB_B, 0xC, CB_B.Length - 0xC), 0xC, CB_B.Length - 0xC);
            return CB_B;
        }

        private byte[] build(byte[] data)
        {
            byte[] returnval = Oper.returnportion(ref data, 2, 2);
            return returnval;
        }

        #endregion

        /// <summary>
        /// patch, encrypt, decrypt SMC
        /// </summary>
        /// <param name="SMC"></param>
        /// <returns></returns>
        #region SMC

        private byte[] patch_SMC_old(byte[] SMC)
        {
            bool found = false;
            crc32 crc = new crc32(); // equivalent to new CRC32(CRC32.DefaultPolynomial);
            byte[] newsmc = Oper.returnportion(ref SMC, 4, SMC.Length - 4);

            long hashData = crc.CRC(newsmc);
            Console.WriteLine("CRC32: {0}", hashData.ToString("X"));

            uint[] SMC_patches_crc = { 0x87E726B7, 0xf9c96639, 0x5b3aed00, 0x9ad5b7ee, 0x7e5bc217, 0x1d0c613e };
            string[] SMC_patches_version = { "Trinity, version 3.1", "Trinity, version 3.1", "Jasper, version 2.3", "Zephyr, version 1.10", "Zephyr, version 1.13", "Falcon, version 1.6" };
            int[] SMC_patches_patch = { 0x13b3, 0x13b3, 0x12ba, 0x1257, 0x12a3, 0x12a3 };
            byte[] patched_SMC = new byte[SMC.Length];
            for (int i = 0; i < SMC_patches_crc.Length; i++)
            {
                if (hashData == SMC_patches_crc[i])
                {
                    Console.WriteLine("patchset \"{0}\" matches, 1 patch(es)", SMC_patches_version[i]); //attention
                    found = true;

                    byte[] SMC_A = Oper.returnportion(ref SMC, 0, SMC_patches_patch[i]);
                    byte[] SMC_B = Oper.returnportion(ref SMC, SMC_patches_patch[i] + 2, SMC.Length - SMC_patches_patch[i] - 2);
                    for (int j = 0; j < SMC.Length; j++)
                    {
                        if (j == SMC_patches_patch[i] || j == SMC_patches_patch[i] + 1) patched_SMC[j] = 0x00;
                        else if (j < SMC_patches_patch[i]) patched_SMC[j] = SMC_A[j];
                        else patched_SMC[j] = SMC_B[j - SMC_patches_patch[i] - 2];
                    }

                }
            }
            if (!found)
            {
                Console.WriteLine(" ! Warning: can't patch that SMC, here are the current supported versions:");
                for (int i = 0; i < SMC_patches_version.Length; i++)
                {
                    Console.WriteLine("  - {0}", SMC_patches_version[i]);
                }
            }
            return patched_SMC;
        }

        private byte[] patch_SMC(byte[] SMC)
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

        private byte[] decrypt_SMC(byte[] SMC)
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

        private byte[] encrypt_SMC(byte[] SMC)
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

        #endregion
    }
}
