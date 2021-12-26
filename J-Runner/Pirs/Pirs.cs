using System;
using System.Collections.Generic;
using System.IO;

namespace JRunner.Pirs
{
    class Pirs
    {
        static int MAGIC_CON_ = 0x434f4e20;
        static int MAGIC_LIVE = 0x4c495645;
        static int MAGIC_PIRS = 0x50495253;
        static long PIRS_BASE = 0xb000L;
        long pirs_offset;
        long pirs_start;
        static long PIRS_TYPE1 = 0x1000L;
        static long PIRS_TYPE2 = 0x2000L;
        string _filename;
        BinaryReader br;
        FileStream fs;
        List<PirsEntry> list;
        bool stealth = false;

        public Pirs(string filename)
        {
            _filename = filename;
            stealth = false;
            list = new List<PirsEntry>();
            openfile();
            print();
            Console.WriteLine("Done");
        }
        public Pirs(string filename, bool stealth)
        {
            this.stealth = stealth;
            _filename = filename;
            list = new List<PirsEntry>();
            openfile();
            if (!stealth)
            {
                print();
                Console.WriteLine("Done");
            }
        }

        ~Pirs()
        {
            if (br != null) br.Close();
        }

        void print()
        {
            foreach (PirsEntry p in list)
            {
                Console.WriteLine("{0} - cluster {1} - size {2:X}", p.Filename, p.Cluster, p.Size);
            }
        }
        public List<PirsEntry> getList()
        {
            return list;
        }

        DateTime dosDateTime(int datetime)
        {
            return dosDateTime((short)(datetime >> 0x10), (short)(datetime - ((datetime >> 0x10) << 0x10)));
        }
        DateTime dosDateTime(short date, short time)
        {
            if ((date == 0) && (time == 0))
            {
                return DateTime.Now;
            }
            int year = ((date & 0xfe00) >> 9) + 0x7bc;
            int month = (date & 480) >> 5;
            int day = date & 0x1f;
            int hour = (time & 0xf800) >> 11;
            int minute = (time & 0x7e0) >> 5;
            return new DateTime(year, month, day, hour, minute, (time & 0x1f) * 2);
        }

        void openfile()
        {
            if (this.br != null)
            {
                this.br.Close();
            }
            if (this.fs != null)
            {
                this.fs.Dispose();
            }
            if (!File.Exists(_filename)) return;
            this.fs = new FileStream(_filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            br = new BinaryReader(fs);
            getDescription();
            br.BaseStream.Seek(0L, SeekOrigin.Begin);
            int num = Oper.ByteArrayToInt(br.ReadBytes(4));
            if (((num != MAGIC_PIRS) && (num != MAGIC_LIVE)) && (num != MAGIC_CON_))
            {
                Console.WriteLine("Not a PIRS/LIVE file!\r\n");
            }
            else
            {
                br.BaseStream.Seek(0xc030L, SeekOrigin.Begin);
                int num2 = Oper.ByteArrayToInt(br.ReadBytes(4));
                if (num == MAGIC_CON_)
                {
                    pirs_offset = PIRS_TYPE2;
                    pirs_start = 0xc000L;
                }
                else if (num2 == 0xffff)
                {
                    pirs_offset = PIRS_TYPE1;
                    pirs_start = PIRS_BASE + pirs_offset;
                }
                else
                {
                    pirs_offset = PIRS_TYPE2;
                    pirs_start = PIRS_BASE + pirs_offset;
                }
                if (variables.debugme) Console.WriteLine("offset: {0:X} - start: {1:X}", pirs_offset, pirs_start);
                parse();
            }
            //br.Close();
        }
        private void getDescription()
        {
            try
            {
                byte[] buffer = new byte[0x100];
                br.BaseStream.Seek(0x410L, SeekOrigin.Begin);
                buffer = br.ReadBytes(0x100);
                if (!stealth) Console.WriteLine("Title : " + unicodeToStr(buffer, 2) + "\r\n");
                br.BaseStream.Seek(0xd10L, SeekOrigin.Begin);
                buffer = br.ReadBytes(0x100);
                if (!stealth) Console.WriteLine("Description : " + unicodeToStr(buffer, 2) + "\r\n");
                br.BaseStream.Seek(0x1610L, SeekOrigin.Begin);
                buffer = br.ReadBytes(0x100);
                if (!stealth) Console.WriteLine("Publisher : " + unicodeToStr(buffer, 2) + "\r\n");
            }
            catch (System.ArgumentException ex) { Console.WriteLine(ex.Message); }
        }
        void parse()
        {
            getFiles();
        }
        void getFiles()
        {
            int num = 0;
            if (!stealth) Console.WriteLine("Getting Files..");

            while (true)
            {
                br.BaseStream.Seek(pirs_start + (num * 0x40), SeekOrigin.Begin);
                PirsEntry entry = new PirsEntry();
                entry = getEntry();
                if (String.IsNullOrWhiteSpace(entry.Filename.Trim()))
                {
                    return;
                }
                if ((entry.Cluster != 0) || ((entry.Cluster == 0) && (entry.Size == 0)))
                {
                    list.Add(entry);
                }
                num++;
            }
        }

        bool getBit(byte a, int bitNumber)
        {
            return (a & (1 << bitNumber)) != 0;
        }
        PirsEntry getEntry()
        {
            PirsEntry entry = new PirsEntry();
            entry.Filename = readString(0x27);
            //Console.WriteLine(entry.FileName.Length);
            if (!String.IsNullOrWhiteSpace(entry.Filename.Trim()))
            {
                entry.Flags = br.ReadByte();
                if (getBit(entry.Flags, 7)) entry.Folder = true;
                else entry.Folder = false;
                br.ReadBytes(0x3);
                entry.Blocklen = Oper.ByteArrayToInt(br.ReadBytes(0x3));
                entry.Cluster = br.ReadInt32() >> 8;
                entry.Parent = (ushort)Convert.ToUInt32(Oper.ByteArrayToString(br.ReadBytes(2)), 16);
                entry.Size = Oper.ByteArrayToInt(br.ReadBytes(0x4));
                entry.UpdateTime = dosDateTime(Oper.ByteArrayToInt(br.ReadBytes(0x4)));
                entry.AccessTime = dosDateTime(Oper.ByteArrayToInt(br.ReadBytes(0x4)));
                long pos = br.BaseStream.Position;
                entry.CRC = getCRC(br, entry.Cluster, entry.Size);
                br.BaseStream.Seek(pos, SeekOrigin.Begin);
            }
            return entry;
        }
        private bool isFolder(PirsEntry p)
        {
            return p.Folder;
        }
        public bool isFolder(string name)
        {
            foreach (PirsEntry p in list)
            {
                if (p.Filename == name) return p.Folder;
            }
            return false;
        }

        long getOffset(long cluster)
        {
            long num = pirs_start + (cluster * 0x1000L);
            long num2 = cluster / 170L;
            long num3 = num2 / 170L;
            if (num2 > 0L)
            {
                num += (num2 + 1L) * pirs_offset;
            }
            if (num3 > 0L)
            {
                num += (num3 + 1L) * pirs_offset;
            }
            return num;
        }
        string unicodeToStr(byte[] data)
        {
            return this.unicodeToStr(data, 0, data.Length);
        }
        string unicodeToStr(byte[] data, int start)
        {
            return this.unicodeToStr(data, start, data.Length);
        }
        string unicodeToStr(byte[] data, int start, int length)
        {
            string str = "";
            for (int i = start; i < data.Length; i += 2)
            {
                if (data[i] == 0)
                {
                    return str;
                }
                str = str + Convert.ToChar(data[i]);
            }
            return str;
        }
        string readString(uint nbchar)
        {
            string str = "";
            for (uint i = 0; i < nbchar; i++)
            {
                char ch = br.ReadChar();
                if (ch != '\0')
                {
                    str = str + Convert.ToString(ch);
                }
            }
            return str;
        }


        public long getCRC(BinaryReader br, long cluster, long size)
        {
            long num;
            byte[] data = new byte[size];
            int pos = 0;
            long num2 = size >> 12;
            long num3 = size - (num2 << 12);
            for (long i = cluster; i < (cluster + num2); i += 1L)
            {
                num = getOffset(i);
                br.BaseStream.Seek(num, SeekOrigin.Begin);
                Buffer.BlockCopy(br.ReadBytes(0x1000), 0, data, pos, 0x1000);
                pos += 0x1000;
            }
            num = getOffset(cluster + num2);
            br.BaseStream.Seek(num, SeekOrigin.Begin);
            Buffer.BlockCopy(br.ReadBytes((int)num3), 0, data, pos, (int)num3);
            crc32 cr = new crc32();
            return cr.CRC(data);

        }
        public void extractFile(long cluster, long size, string filename)
        {
            long num;
            FileStream stream;
            BinaryWriter writer;
            //if (br == null) br = new BinaryReader(new FileStream(_filename, FileMode.Open));
            try
            {
                stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                writer = new BinaryWriter(stream);
            }
            catch (IOException exception)
            {
                Console.WriteLine(string.Format("Error : {0}\r\n", exception));
                return;
            }
            long num2 = size >> 12;
            long num3 = size - (num2 << 12);
            for (long i = cluster; i < (cluster + num2); i += 1L)
            {
                num = getOffset(i);
                br.BaseStream.Seek(num, SeekOrigin.Begin);
                writer.Write(br.ReadBytes(0x1000));
            }
            num = getOffset(cluster + num2);
            br.BaseStream.Seek(num, SeekOrigin.Begin);
            writer.Write(br.ReadBytes((int)num3));
            //br.Close();
            writer.Close();
            stream.Dispose();
        }
        public byte[] extractFile(long cluster, long size)
        {
            byte[] file = new byte[size];
            long num, i;
            //if (br == null) br = new BinaryReader(new FileStream(_filename, FileMode.Open));
            long num2 = size >> 12;
            long num3 = size - (num2 << 12);
            for (i = cluster; i < (cluster + num2); i += 1L)
            {
                num = getOffset(i);
                br.BaseStream.Seek(num, SeekOrigin.Begin);
                Buffer.BlockCopy(br.ReadBytes(0x1000), 0, file, (int)(i - cluster) * 0x1000, 0x1000);
            }
            num = getOffset(cluster + num2);
            br.BaseStream.Seek(num, SeekOrigin.Begin);
            Buffer.BlockCopy(br.ReadBytes((int)num3), 0, file, (int)(i - cluster) * 0x1000, (int)num3);

            return file;
        }

        public void extractFolder(ushort tag, string foldername, string pathname)
        {
            //if (br == null) br = new BinaryReader(new FileStream(_filename, FileMode.Open));
            ushort num = 0;
            while (true)
            {
                br.BaseStream.Seek((PIRS_BASE + pirs_offset) + (num * 0x40), SeekOrigin.Begin);
                PirsEntry entry = new PirsEntry();
                entry = getEntry();
                if (String.IsNullOrWhiteSpace(entry.Filename.Trim()))
                {
                    //br.Close();
                    return;
                }
                if (((entry.Cluster == 0) && (entry.Size == 0)) && (entry.Parent == tag))
                {
                    extractFolder(num, entry.Filename, pathname + @"\" + foldername);
                }
                else if ((entry.Cluster != 0) && (entry.Parent == tag))
                {
                    extractFile(entry.Cluster, entry.Size, pathname + @"\" + foldername + @"\" + entry.Filename);
                }
                num = (ushort)(num + 1);
            }
        }
    }

    public class PirsEntry
    {
        string _filename;
        byte _Flags;
        int _BlockLen;
        int _Cluster;
        ushort _Parent;
        int _Size;
        bool _folder;
        long _CRC;
        DateTime _UpdateTime;
        DateTime _AccessTime;

        public string Filename
        {
            get
            {
                return _filename;
            }
            set
            {
                _filename = value;
            }
        }
        public byte Flags
        {
            get { return _Flags; }
            set { _Flags = value; }
        }
        public int Blocklen
        {
            get { return _BlockLen; }
            set { _BlockLen = value; }
        }
        public int Cluster
        {
            get { return _Cluster; }
            set { _Cluster = value; }
        }
        public int Size
        {
            get { return _Size; }
            set { _Size = value; }
        }
        public long CRC
        {
            get { return _CRC; }
            set { _CRC = value; }
        }
        public ushort Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }
        public DateTime UpdateTime
        {
            get { return _UpdateTime; }
            set { _UpdateTime = value; }
        }
        public DateTime AccessTime
        {
            get { return _AccessTime; }
            set { _AccessTime = value; }
        }
        public bool Folder
        {
            get { return _folder; }
            set { _folder = value; }
        }
    }
}
