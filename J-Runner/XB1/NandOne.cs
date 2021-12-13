using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRunner.XB1
{
    class NandOne
    {
        #region Constants

        const long NAND_SIZE = 0x13C000000;
        const int NAND_SPLIT = (int)(NAND_SIZE / 0x20);
        const int LOG_BLOCK_SZ = 0x1000;

        const int MAGIC_SIZE = 0x4;

        private byte[] SFBX_MAGIC = Oper.StringToByteArray_v3("SFBX");
        private byte[] GFCX_MAGIC = Oper.StringToByteArray_v3("GFCX");
        private byte[] GFCU_MAGIC = Oper.StringToByteArray_v3("GFCU");
        private byte[] XVD_MAGIC = Oper.StringToByteArray_v3("msft-xvd");

        const int XVD_MAGIC_START = 0x200;

        int[] SFBX_START = { 0x810000, 0x10000 };
        const int SFBX_MAGIC_START = 0x0;

        const int SFBX_ENTS_START = 0x10;

        const int SFBX_BLOB_START = 0x3C0;
        const int SFBX_BLOB_SIZE = 0x40;

        const int SFBX_EN_SIZE = 0x10;
        const int SFBX_EN_LBA_START = 0x0;
        const int SFBX_EN_SZ_START = 0x4;

        const int SFBX_ENTS_MAXCNT_FIX = 0x3B;

        const int GFCU_MAGIC_START = 0x28;
        const int GFCU_ENTS_START = 0xF0;

        const int GFCU_BLOB_START = 0xAD8;
        const int GFCU_BLOB_SIZE = 0x64;

        const int GFCU_EN_SIZE = 0x4C;
        const int GFCU_EN_FN_START = 0x0;
        const int GFCU_EN_FN_LENGTH = 0x40;
        const int GFCU_EN_SIZE_START = 0x40;
        const int GFCU_EN_BLOCK_START = 0x44;
        const int GFCU_EN_UNKNOWN_START = 0x48;
        const int GFCU_EN_EOF_START = 0x4;
        const int GFCU_EN_EOF_MAGIC = 0x3FF;

        const int GFCU_ENTS_MAXCNT = 0x100;

        const int KRNL_VER_START = 0x30;
        const int KRNL_VER_LENGTH = 0x70;

        #endregion

        struct SFBX
        {
            public int POSITION;
            public int LBA;
            public int SIZE;
            public byte[] MAGIC;

            public SFBX(int total_pos, int lba, int sz, byte[] magic)
            {
                this.POSITION = total_pos;
                this.LBA = lba;
                this.SIZE = sz;
                this.MAGIC = magic;
            }
        }

        struct GFCU
        {
            public int POSITION;
            public string FILENAME;
            public int SIZE;
            public int BLOCK;
            public int UNKNOWN;

            public GFCU(int pos, string fn, int sz, int blk, int un)
            {
                this.POSITION = pos;
                this.SIZE = sz;
                this.FILENAME = fn;
                this.BLOCK = blk;
                this.UNKNOWN = un;
            }
        }

        class TOTAL_ADDR_SZ
        {
            public int total_addr;
            public int total_size;

            public TOTAL_ADDR_SZ(int total_addr, int total_size)
            {
                this.total_addr = total_addr;
                this.total_size = total_size;
            }
        }

        List<SFBX> sfbx_arr = new List<SFBX>();
        List<GFCU> gfcu_arr = new List<GFCU>();

        byte[] ReadFile(string fn, int pos, int len)
        {
            byte[] bytes = new byte[len];
            FileStream fs = new FileStream(fn, FileMode.Open);
            fs.Seek(pos, SeekOrigin.Begin);
            fs.Read(bytes, 0, len);
            fs.Close();
            return bytes;
        }

        int ReadUInt32_LE(byte[] data, int pos)
        {
            return (int)Convert.ToUInt32(Oper.ByteArrayToString(data), 16);
        }

        bool CheckMagic(byte[] indata, int pos, byte[] magic)
        {
            return Oper.ByteArrayCompare(indata, magic, pos, 0, magic.Length);
        }

        int ScanForSFBX(string fn)
        {
            int sfbx_addr = 0;
            FileStream fs = new FileStream(fn, FileMode.Open);
            for (int i = 0; i < NAND_SIZE / LOG_BLOCK_SZ; i += LOG_BLOCK_SZ)
            {
                fs.Seek(LOG_BLOCK_SZ, SeekOrigin.Current);
                byte[] sfbx_magic = new byte[SFBX_MAGIC.Length];
                fs.Read(sfbx_magic, 0, sfbx_magic.Length);
                if (CheckMagic(sfbx_magic, SFBX_MAGIC_START, SFBX_MAGIC))
                {
                    sfbx_addr = i * NAND_SPLIT;
                    break;
                }
            }
            fs.Close();
            return sfbx_addr;
        }

        int GetSizeSFBX(string fn, int addr)
        {
            if (addr == 0) return 0;
            int total_pos, sz, lba;
            byte[] entry;
            for (int i = 0; i < SFBX_ENTS_MAXCNT_FIX; i++)
            {
                total_pos = addr + i * SFBX_EN_SIZE;
                entry = ReadFile(fn, total_pos, SFBX_EN_SIZE);

                lba = ReadUInt32_LE(entry, SFBX_EN_LBA_START);
                sz = ReadUInt32_LE(entry, SFBX_EN_SZ_START);
                if ((lba * LOG_BLOCK_SZ) == addr) return (sz * LOG_BLOCK_SZ) / 0x10; // Needs division by 0x10
            }
            return 0;
        }

        int DumpSFBX(string fn)
        {
            int sfbxaddr = 0, sfbxsize;
            for (int i = 0; i < SFBX_START.Length; i++)
            {
                byte[] sfbx_magic = ReadFile(fn, SFBX_START[i], SFBX_MAGIC.Length);
                if (CheckMagic(sfbx_magic, SFBX_MAGIC_START, SFBX_MAGIC))
                {
                    sfbxaddr = SFBX_START[i];
                    break;
                }
                if (i == SFBX_START.Length - 1)
                {
                    Console.WriteLine("SFBX data wasn't found. Scanning for it!");
                    sfbxaddr = ScanForSFBX(fn);
                    break;
                }
            }
            sfbxsize = GetSizeSFBX(fn, sfbxaddr);
            sfbxaddr = sfbxaddr + SFBX_ENTS_START; // Don't want the header

            if (sfbxsize == 0)
            {
                Console.WriteLine("Size of SFBX wasn't found in Adresstable");
                return 0;
            }

            int sfbx_ents_size = sfbxsize - SFBX_BLOB_SIZE - SFBX_ENTS_START;
            int sfbx_ents_maxcnt = sfbx_ents_size / SFBX_EN_SIZE;
            int j = 0;
            //Read adresses in array		
            for (; j < sfbx_ents_maxcnt; j++)
            {
                int total_pos = sfbxaddr + j * SFBX_EN_SIZE;
                byte[] entry = ReadFile(fn, total_pos, SFBX_EN_SIZE);

                int lba = ReadUInt32_LE(entry, SFBX_EN_LBA_START);
                int sz = ReadUInt32_LE(entry, SFBX_EN_SZ_START);

                int fileaddr = lba * LOG_BLOCK_SZ;

                // msft-xvd magic doesnt start at 0x0!
                byte[] xvd = ReadFile(fn, fileaddr + XVD_MAGIC_START, XVD_MAGIC.Length);
                byte[] magic;
                if (Oper.ByteArrayCompare(xvd, XVD_MAGIC))
                {
                    magic = Oper.StringToByteArray_v3("XVD");
                }
                else
                {
                    magic = ReadFile(fn, fileaddr, MAGIC_SIZE);
                }

                SFBX s = new SFBX(total_pos, lba, sz, magic);
                sfbx_arr.Add(s);
            }
            return j - 1;
        }

        void ExtractSFBXData(string fn)
        {
            //infile = open(fn, 'r+b')
            //foldername = os.path.basename(fn).replace('.','_')
            //MakeDir(foldername)
            foreach (SFBX sfbx in sfbx_arr)
            {
                if (sfbx.SIZE != 0)
                {
                    int addr = sfbx.LBA * LOG_BLOCK_SZ;
                    int size = sfbx.SIZE * LOG_BLOCK_SZ;
                    byte[] magic = sfbx.MAGIC;
                    string ext;
                    if (magic != null) ext = Oper.ByteArrayToString_v2(magic);
                    else ext = "BIN";

                    //string fn_out = '{:X}_{:X}.{}'.format(addr,addr+size,ext);

                    //        path_out = os.path.join(foldername, fn_out)

                    //        outfile = open(path_out, 'w+b')
                    //Console.WriteLine("Extracting @ {0:#08x}, size: {1}kb to '{2}'", addr, size / 1024, fn_out);

                    //        infile.seek(addr)
                    //        outfile.write(infile.read(size))	
                    //        outfile.close()
                }
            }
            //infile.close()
        }

        void PrintSFBX()
        {
            Console.WriteLine("SFBX Entries");
            Console.WriteLine("-----------\n");
            int i = 0;
            foreach (SFBX sfbx in sfbx_arr)
            {
                if (sfbx.SIZE == 0) continue;
                Console.WriteLine("'Entry 0x{0:02X} : found @ pos: {1:08X}", i, sfbx.POSITION);
                Console.WriteLine("\tLBA: {0:08X} (addr {1:09X})", sfbx.LBA, sfbx.LBA * LOG_BLOCK_SZ);
                Console.WriteLine("\tSize: {0:08X} ({0} Bytes, {1}kB, {2}MB)", sfbx.SIZE * LOG_BLOCK_SZ, (sfbx.SIZE * LOG_BLOCK_SZ) / 1024, (sfbx.SIZE * LOG_BLOCK_SZ) / 1024 / 1024);
                if (sfbx.MAGIC != null) Console.WriteLine("*** MAGIC: {0} ***", Oper.ByteArrayToString_v2(sfbx.MAGIC));
            }
        }

        // Returns: total addr, total_size 		
        TOTAL_ADDR_SZ GetEntryByMagic(byte[] magic)
        {
            foreach (SFBX sfbx in sfbx_arr)
            {
                if (Oper.ByteArrayCompare(sfbx.MAGIC, magic))
                {
                    return new TOTAL_ADDR_SZ(sfbx.LBA * LOG_BLOCK_SZ, sfbx.SIZE * LOG_BLOCK_SZ);
                }
            }
            return null;
        }

        // Returns: total_size
        int GetEntryByAddr(int addr)
        {
            foreach (SFBX sfbx in sfbx_arr)
            {
                if ((sfbx.LBA * LOG_BLOCK_SZ) == addr) return (sfbx.SIZE * LOG_BLOCK_SZ);
            }
            return 0;
        }

        string DumpKernelVer(byte[] data)
        {
            return Oper.ByteArrayToString_v2(data, KRNL_VER_START, KRNL_VER_LENGTH);
        }

        int DumpGFCU(byte[] data, int startaddr)
        {
            int i = 0;
            for (; i < GFCU_ENTS_MAXCNT; i++)
            {
                int pos = startaddr + i * GFCU_EN_SIZE;
                byte[] entry = Oper.returnportion(data, pos, GFCU_EN_SIZE);

                int eof = ReadUInt32_LE(entry, GFCU_EN_EOF_START);
                if (eof == GFCU_EN_EOF_MAGIC) return i - 1;

                string fn = Oper.ByteArrayToString_v2(entry, GFCU_EN_FN_START, GFCU_EN_FN_LENGTH);
                int sz = ReadUInt32_LE(entry, GFCU_EN_SIZE_START);
                int blk = ReadUInt32_LE(entry, GFCU_EN_BLOCK_START);
                int un = ReadUInt32_LE(entry, GFCU_EN_UNKNOWN_START);

                GFCU gfcu = new GFCU(pos, fn, sz, blk, un);
                gfcu_arr.Add(gfcu);
            }
            return i - 1;
        }

        void PrintGFCU()
        {
            Console.WriteLine("GFCU Entries");
            Console.WriteLine("-----------");
            int i = 0;
            foreach (GFCU gfcu in gfcu_arr)
            {
                Console.WriteLine("Entry 0x{0:02X} : found @ pos: {1:08X}", i, gfcu.POSITION);
                Console.WriteLine("\tfilename: {0} (size: {1:09X})", gfcu.FILENAME, gfcu.SIZE * LOG_BLOCK_SZ);
                Console.WriteLine("\tBlock: {0:08X} Unknown: {0:08X}", gfcu.BLOCK * LOG_BLOCK_SZ, gfcu.UNKNOWN * LOG_BLOCK_SZ);
                i++;
            }
        }

        int FindGFCU(string filename)
        {
            int gfcx_addr, gfcx_size;
            TOTAL_ADDR_SZ t = GetEntryByMagic(GFCX_MAGIC);
            gfcx_addr = t.total_addr;
            gfcx_size = t.total_size;
            if (gfcx_addr == 0)
            {
                Console.WriteLine("GFCX MAGIC not found. Exiting!");
                return -1;
            }

            int gfcu_addr = gfcx_addr + gfcx_size;
            int gfcu_size = GetEntryByAddr(gfcu_addr);

            if (gfcu_size == 0)
            {
                Console.WriteLine("GFCU Entry not found. Exiting!");
                return -2;
            }

            byte[] gfcu = ReadFile(filename, gfcu_addr, gfcu_size);
            if (!CheckMagic(gfcu, GFCU_MAGIC_START, GFCU_MAGIC))
            {
                Console.WriteLine("GFCU MAGIC not found. Exiting!");
                return -3;
            }

            Console.WriteLine("Parsing GFCU Entries... ");
            int gfcu_len = DumpGFCU(gfcu, GFCU_ENTS_START);
            Console.WriteLine("Found {0} Entries", gfcu_len);
            Console.WriteLine("Xbox ONE Kernel-Version: {0}", DumpKernelVer(gfcu));
            return gfcu_len;
        }

        public NandOne(string filename)
        {
            Console.WriteLine("Opening '{0}'", filename);

            FileInfo fi = new FileInfo(filename);
            if (fi.Length != NAND_SIZE)
            {
                Console.WriteLine("Invalid filesize. Aborting!");
                return;
            }

            Console.WriteLine("Parsing SFBX Entries... ");
            int sfbx_len = DumpSFBX(filename);
            if (sfbx_len == 0)
            {
                Console.WriteLine("SFBX not found! Aborting!\n");
                return;
            }
            Console.WriteLine("Found {0} Entries", sfbx_len);


            PrintSFBX();
            if (FindGFCU(filename) <= 0) return;
            PrintGFCU();
        }

    }
}