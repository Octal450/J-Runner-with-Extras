namespace JRunner
{

    public struct _FDEVI
    {
        private readonly string name; // name of device
        private readonly byte id; // device id, byte[1] of silicon ID
        private readonly long pagesize; // size of each page
        private readonly long sparesize; // size of spare for each page
        private readonly long chipsize; // total size of user memory
        private readonly long pagePB; // pages per block
        private readonly bool bigblock; // whether the chip uses big block dump style

        public _FDEVI(string name, byte id, long pagesize, long sparesize, long chipsize, long pagePB, bool bigblock)
        {
            this.name = name;
            this.id = id;
            this.pagesize = pagesize;
            this.sparesize = sparesize;
            this.chipsize = chipsize;
            this.pagePB = pagePB;
            this.bigblock = bigblock;
        }

        public string Name { get { return name; } }
        public byte ID { get { return id; } }
        public long Pagesize { get { return pagesize; } }
        public long Sparesize { get { return sparesize; } }
        public long Chipsize { get { return chipsize; } }
        public long PagePB { get { return pagePB; } }
        public bool Bigblock { get { return bigblock; } }
    }
    public struct _FMANU
    {
        private readonly string name; // manufacturer name
        private readonly byte id; // manufacturer ID, byte[0] of silicon ID

        public _FMANU(string name, byte id)
        {
            this.name = name;
            this.id = id;
        }

        public string Name { get { return name; } }
        public byte ID { get { return id; } }
    }

    public class Nands
    {

        public static _FDEVI[] Fdevi = new _FDEVI[]
        {       //	    {"name",	ID[1],	pagesz,	sparesz,chipsz,	ppb,	bigblock},	
                // small block chips
                new _FDEVI("8MiB",  0xd6,   512,    16,     8,      16,     false),
                new _FDEVI("8MiB",  0xe6,   512,    16,     8,      16,     false),
                new _FDEVI("16MiB", 0x73,   512,    16,     16,     32,     false),
                new _FDEVI("32MiB", 0x75,   512,    16,     32,     32,     false),
                new _FDEVI("64MiB", 0x76,   512,    16,     64,     32,     false),
                new _FDEVI("128MiB",0x79,   512,    16,     128,    32,     false),
                new _FDEVI("256MiB",0x71,   512,    16,     256,    32,     false),
                new _FDEVI("4GB"    ,0xD7,  8192,   448,    4096,   256,    false),
                // big block chips
                new _FDEVI("128MiB",0xF1,   2048,   64,     128,    64,     true),
                new _FDEVI("64MiB", 0xF2,   512,    16,     64,     32,     true),
                new _FDEVI("256MiB",0xDA,   2048,   64,     256,    64,     true),
                new _FDEVI("512MiB",0xDC,   2048,   64,     512,    64,     true),
                new _FDEVI("UNKNOWN",0x00,  0,      0,      0,      0,      false)
        };
        public static _FMANU[] Fman = new _FMANU[]
        {
            new _FMANU("Toshiba",       0x98),
            new _FMANU("Samsung",       0xec),
            new _FMANU("Fujitsu",       0x04),
            new _FMANU("National",      0x8f),
            new _FMANU("Renesas",       0x07),
            new _FMANU("ST Micro",      0x20),
            new _FMANU("Hynix",         0xad),
            new _FMANU("Micron",        0x2c),
            new _FMANU("Amd",           0x01),
            new _FMANU("Macronix",      0xc2),
            new _FMANU("Unknown",       0x00)
        };
    }
}