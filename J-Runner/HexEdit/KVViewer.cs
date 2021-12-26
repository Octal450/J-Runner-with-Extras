using Be.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace JRunner.HexEdit
{
    public partial class KVViewer : Form
    {
        FormGoTo _formGoto = new FormGoTo();
        FormFind _formFind;
        FindOptions _findOptions = new FindOptions();

        public KVViewer(byte[] bytes)
        {
            InitializeComponent();
            if (bytes != null)
            {
                try
                {
                    DynamicByteProvider dynamicByteProvider = new DynamicByteProvider(bytes);
                    hexBox1.ByteProvider = dynamicByteProvider;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            buildTree();
        }

        private void HexViewer_Load(object sender, EventArgs e)
        {
            UpdateFileSizeStatus();
        }

        void UpdateFileSizeStatus()
        {
            if (this.hexBox1.ByteProvider == null)
                this.fileSizeToolStripStatusLabel.Text = string.Empty;
            else
                this.fileSizeToolStripStatusLabel.Text = GetDisplayBytes(this.hexBox1.ByteProvider.Length);
        }
        void SaveFile()
        {
            if (hexBox1.ByteProvider == null)
                return;

            try
            {
                DynamicFileByteProvider dynamicFileByteProvider = hexBox1.ByteProvider as DynamicFileByteProvider;
                dynamicFileByteProvider.ApplyChanges();
            }
            catch (Exception ex1)
            {
                Console.WriteLine(ex1.Message);
                if (variables.debugme) Console.WriteLine(ex1.ToString());
            }
        }

        public static string GetDisplayBytes(long size)
        {
            const long multi = 1024;
            long kb = multi;
            long mb = kb * multi;
            long gb = mb * multi;
            long tb = gb * multi;

            const string BYTES = "Bytes";
            const string KB = "KB";
            const string MB = "MB";
            const string GB = "GB";
            const string TB = "TB";

            string result;
            if (size < kb)
                result = string.Format("{0} {1}", size, BYTES);
            else if (size < mb)
                result = string.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, kb), KB, ConvertBytesDisplay(size));
            else if (size < gb)
                result = string.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, mb), MB, ConvertBytesDisplay(size));
            else if (size < tb)
                result = string.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, gb), GB, ConvertBytesDisplay(size));
            else
                result = string.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, tb), TB, ConvertBytesDisplay(size));

            return result;
        }
        static string ConvertBytesDisplay(long size)
        {
            return size.ToString("###,###,###,###,###", CultureInfo.CurrentCulture);
        }
        static string ConvertToOneDigit(long size, long quan)
        {
            double quotient = size / (double)quan;
            string result = quotient.ToString("0.#", CultureInfo.CurrentCulture);
            return result;
        }

        void Position_Changed(object sender, EventArgs e)
        {
            this.toolStripStatusLabel.Text = string.Format("Ln {0}    Col {1}",
                hexBox1.CurrentLine, hexBox1.CurrentPositionInLine);
        }

        private void HexViewer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.G)
            {
                Goto();
            }
            else if (e.Control && e.KeyCode == Keys.F)
            {
                Find();
            }
        }

        public void CleanUp()
        {
            if (hexBox1.ByteProvider != null)
            {
                IDisposable byteProvider = hexBox1.ByteProvider as IDisposable;
                if (byteProvider != null)
                    byteProvider.Dispose();
                hexBox1.ByteProvider = null;
            }
        }

        void Goto()
        {
            _formGoto.SetDefaultValue(hexBox1.SelectionStart);
            if (_formGoto.ShowDialog() == DialogResult.OK)
            {
                hexBox1.SelectionStart = _formGoto.GetByteIndex();
                hexBox1.SelectionLength = 1;
                hexBox1.Focus();
            }
        }
        void Find()
        {
            if (_formFind == null || _formFind.IsDisposed)
            {
                _formFind = new FormFind();
                _formFind.HexBox = this.hexBox1;
                _formFind.FindOptions = _findOptions;
                _formFind.Show(this);
            }
            else
            {
                _formFind.Focus();
            }
        }


        class leaf
        {
            public string fatherName;
            public bool lastChild;
            public string name;
            public bool isRoot;
            public bool hasChildren;
            public int offset;
            public int size;

            public leaf(string Name, bool hasRoot, bool isChildren, int off, int length, string father = "Key Vault", bool lChild = false)
            {
                name = Name;
                isRoot = hasRoot;
                hasChildren = isChildren;
                offset = off;
                size = length;
                fatherName = father;
                lastChild = lChild;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return (name == ((leaf)obj).name &&
                    fatherName == ((leaf)obj).fatherName);
            }
        }
        List<leaf> list = new List<leaf>();

        private void fillTree()
        {
            list.Add(new leaf("Key Vault", true, true, 0, 0x0));
            list.Add(new leaf("HMAC SHA1", false, false, 0, 0x10));
            list.Add(new leaf("KEY 0x00 - XEKEY_MANUFACTURING_MODE", false, false, 0x18, 0x01));
            list.Add(new leaf("KEY 0x01 - XEKEY_ALTERNATE_KEY_VAULT", false, false, 0x19, 0x01));
            list.Add(new leaf("KEY 0x02 - XEKEY_RESTRICTED_PRIVILEGES_FLAGS", false, false, 0x1A, 0x01));
            list.Add(new leaf("KEY 0x03 - XEKEY_RESERVED_BYTE3", false, false, 0x1B, 0x01));
            list.Add(new leaf("KEY 0x04 - XEKEY_ODD_FEATURES", false, false, 0x1C, 0x02));
            list.Add(new leaf("KEY 0x05 - XEKEY_RESERVED_WORD2", false, false, 0x1E, 0x02));
            list.Add(new leaf("KEY 0x06 - XEKEY_RESTRICTED_HVEXT_LOADER", false, false, 0x20, 0x04));
            list.Add(new leaf("KEY 0x07 - XEKEY_POLICY_FLASH_FILE", false, false, 0x24, 0x4));
            list.Add(new leaf("KEY 0x08 - XEKEY_POLICY_BUILTIN_USBMU_SIZE", false, false, 0x28, 0x4));
            list.Add(new leaf("KEY 0x09 - XEKEY_RESERVED_DWORD4", false, false, 0xC, 0x4));
            list.Add(new leaf("KEY 0x0A - RESTRICTED_PRIVILEGES", false, false, 0x30, 0x8));
            list.Add(new leaf("KEY 0x0B - XEKEY_RESERVED_QWORD2", false, false, 0x38, 0x8));
            list.Add(new leaf("KEY 0x0C - XEKEY_RESERVED_QWORD3", false, false, 0x40, 0x8));
            list.Add(new leaf("KEY 0x0D - XEKEY_RESERVED_QWORD4", false, false, 0x48, 0x8));
            list.Add(new leaf("KEY 0x0E - XEKEY_RESERVED_KEY1", false, false, 0x50, 0x10));
            list.Add(new leaf("KEY 0x0F - XEKEY_RESERVED_KEY2", false, false, 0x60, 0x10));
            list.Add(new leaf("KEY 0x10 - XEKEY_RESERVED_KEY3", false, false, 0x70, 0x10));
            list.Add(new leaf("KEY 0x11 - XEKEY_RESERVED_KEY4", false, false, 0x80, 0x10));
            list.Add(new leaf("KEY 0x12 - XEKEY_RESERVED_RANDOM_KEY1", false, false, 0x90, 0x10));
            list.Add(new leaf("KEY 0x13 - XEKEY_RESERVED_RANDOM_KEY2", false, false, 0xA0, 0x10));
            list.Add(new leaf("KEY 0x14 - XEKEY_CONSOLE_SERIAL_NUMBER", false, false, 0xB0, 0xC));
            list.Add(new leaf("PADDING1", false, false, 0xBC, 0x4));
            list.Add(new leaf("KEY 0x15 - XEKEY_MAINBOARD_SERIAL_NUMBER", false, false, 0xC0, 0x8));
            list.Add(new leaf("KEY 0x16 - XEKEY_GAME_REGION", false, false, 0xC8, 0x2));
            list.Add(new leaf("PADDING2", false, false, 0xCA, 0x6));
            list.Add(new leaf("KEY 0x17 - XEKEY_CONSOLE_OBFUSCATION_KEY", false, false, 0xD0, 0x10));
            list.Add(new leaf("KEY 0x18 - XEKEY_OBFUSCATION_KEY", false, false, 0xE0, 0x10));
            list.Add(new leaf("KEY 0x19 - XEKEY_ROAMABLE_OBFUSCATION_KEY (DVD Auth. Key)", false, false, 0xF0, 0x10));
            list.Add(new leaf("KEY 0x1A - XEKEY_DVD_KEY", false, false, 0x100, 0x10));
            list.Add(new leaf("KEY 0x1B - XEKEY_PRIMARY_ACTIVATION_KEY", false, false, 0x110, 0x18));
            list.Add(new leaf("KEY 0x1C - XEKEY_SECONDARY_ACTIVATION_KEY", false, false, 0x128, 0x10));
            list.Add(new leaf("KEY 0x1D - XEKEY_GLOBAL_DEVICE_2DES_KEY1", false, false, 0x138, 0x10));
            list.Add(new leaf("KEY 0x1E - XEKEY_GLOBAL_DEVICE_2DES_KEY2", false, false, 0x148, 0x10));
            list.Add(new leaf("KEY 0x1F - XEKEY_WIRELESS_CONTROLLER_MS_2DES_KEY1", false, false, 0x158, 0x10));
            list.Add(new leaf("KEY 0x20 - XEKEY_WIRELESS_CONTROLLER_MS_2DES_KEY2", false, false, 0x168, 0x10));
            list.Add(new leaf("KEY 0x21 - XEKEY_WIRED_WEBCAM_MS_2DES_KEY1", false, false, 0x178, 0x10));
            list.Add(new leaf("KEY 0x22 - XEKEY_WIRED_WEBCAM_MS_2DES_KEY2", false, false, 0x188, 0x10));
            list.Add(new leaf("KEY 0x23 - XEKEY_WIRED_CONTROLLER_MS_2DES_KEY1", false, false, 0x198, 0x10));
            list.Add(new leaf("KEY 0x24 - XEKEY_WIRED_CONTROLLER_MS_2DES_KEY2", false, false, 0x1A8, 0x10));
            list.Add(new leaf("KEY 0x25 - XEKEY_MEMORY_UNIT_MS_2DES_KEY1", false, false, 0x1B8, 0x10));
            list.Add(new leaf("KEY 0x26 - XEKEY_MEMORY_UNIT_MS_2DES_KEY2", false, false, 0x1C8, 0x10));
            list.Add(new leaf("KEY 0x27 - XEKEY_OTHER_XSM3_DEVICE_MS_2DES_KEY1", false, false, 0x1D8, 0x10));
            list.Add(new leaf("KEY 0x28 - XEKEY_OTHER_XSM3_DEVICE_MS_2DES_KEY2", false, false, 0x1E8, 0x10));
            list.Add(new leaf("KEY 0x29 - XEKEY_WIRELESS_CONTROLLER_3P_2DES_KEY1", false, false, 0x1F8, 0x10));
            list.Add(new leaf("KEY 0x2A - XEKEY_WIRELESS_CONTROLLER_3P_2DES_KEY2 ", false, false, 0x208, 0x10));
            list.Add(new leaf("KEY 0x2B - XEKEY_WIRED_WEBCAM_3P_2DES_KEY1", false, false, 0x218, 0x10));
            list.Add(new leaf("KEY 0x2C - XEKEY_WIRED_WEBCAM_3P_2DES_KEY2", false, false, 0x228, 0x10));
            list.Add(new leaf("KEY 0x2D - XEKEY_WIRED_CONTROLLER_3P_2DES_KEY1", false, false, 0x238, 0x10));
            list.Add(new leaf("KEY 0x2E - XEKEY_WIRED_CONTROLLER_3P_2DES_KEY2", false, false, 0x248, 0x10));
            list.Add(new leaf("KEY 0x2F - XEKEY_MEMORY_UNIT_3P_2DES_KEY1", false, false, 0x258, 0x10));
            list.Add(new leaf("KEY 0x30 - XEKEY_MEMORY_UNIT_3P_2DES_KEY2", false, false, 0x268, 0x10));
            list.Add(new leaf("KEY 0x31 - XEKEY_OTHER_XSM3_DEVICE_3P_2DES_KEY1", false, false, 0x278, 0x10));
            list.Add(new leaf("KEY 0x32 - XEKEY_OTHER_XSM3_DEVICE_3P_2DES_KEY2", false, false, 0x288, 0x10));
            list.Add(new leaf("KEY 0x33 - XEKEY_CONSOLE_PRIVATE_KEY", false, true, 0x298, 0x1d0));
            list.Add(new leaf("Console.Private.Key.CQW", false, false, 0x298, 0x4, "KEY 0x33 - XEKEY_CONSOLE_PRIVATE_KEY"));
            list.Add(new leaf("Console.Private.Key.EXPONENT", false, false, 0x29C, 0x4, "KEY 0x33 - XEKEY_CONSOLE_PRIVATE_KEY"));
            list.Add(new leaf("Console.Private.Key.QWRESERVED", false, false, 0x2A0, 0x8, "KEY 0x33 - XEKEY_CONSOLE_PRIVATE_KEY"));
            list.Add(new leaf("Console.Private.Key.MODULUS", false, false, 0x2A8, 0x80, "KEY 0x33 - XEKEY_CONSOLE_PRIVATE_KEY"));
            list.Add(new leaf("Console.Private.Key.P", false, false, 0x328, 0x40, "KEY 0x33 - XEKEY_CONSOLE_PRIVATE_KEY"));
            list.Add(new leaf("Console.Private.Key.Q", false, false, 0x368, 0x40, "KEY 0x33 - XEKEY_CONSOLE_PRIVATE_KEY"));
            list.Add(new leaf("Console.Private.Key.DP", false, false, 0x3A8, 0x40, "KEY 0x33 - XEKEY_CONSOLE_PRIVATE_KEY"));
            list.Add(new leaf("Console.Private.Key.DQ", false, false, 0x3E8, 0x40, "KEY 0x33 - XEKEY_CONSOLE_PRIVATE_KEY"));
            list.Add(new leaf("Console.Private.Key.CR", false, false, 0x428, 0x40, "KEY 0x33 - XEKEY_CONSOLE_PRIVATE_KEY", true));
            list.Add(new leaf("KEY 0x34 - XEKEY_XEIKA_PRIVATE_KEY", false, true, 0x468, 0x390));
            list.Add(new leaf("Xeika.Private.Key.CQW", false, false, 0x468, 0x4, "KEY 0x34 - XEKEY_XEIKA_PRIVATE_KEY"));
            list.Add(new leaf("Xeika.Private.Key.EXPONENT", false, false, 0x46c, 0x4, "KEY 0x34 - XEKEY_XEIKA_PRIVATE_KEY"));
            list.Add(new leaf("Xeika.Private.Key.QWRESERVED", false, false, 0x470, 0x8, "KEY 0x34 - XEKEY_XEIKA_PRIVATE_KEY"));
            list.Add(new leaf("Xeika.Private.Key.MODULUS", false, false, 0x478, 0x100, "KEY 0x34 - XEKEY_XEIKA_PRIVATE_KEY"));
            list.Add(new leaf("Xeika.Private.Key.P", false, false, 0x578, 0x80, "KEY 0x34 - XEKEY_XEIKA_PRIVATE_KEY"));
            list.Add(new leaf("Xeika.Private.Key.Q", false, false, 0x5F8, 0x80, "KEY 0x34 - XEKEY_XEIKA_PRIVATE_KEY"));
            list.Add(new leaf("Xeika.Private.Key.DP", false, false, 0x678, 0x80, "KEY 0x34 - XEKEY_XEIKA_PRIVATE_KEY"));
            list.Add(new leaf("Xeika.Private.Key.DQ", false, false, 0x6f8, 0x80, "KEY 0x34 - XEKEY_XEIKA_PRIVATE_KEY"));
            list.Add(new leaf("Xeika.Private.Key.CR", false, false, 0x778, 0x80, "KEY 0x34 - XEKEY_XEIKA_PRIVATE_KEY", true));
            list.Add(new leaf("KEY 0x35 - XEKEY_CARDAE_PRIVATE_KEY", false, true, 0x7F8, 0x1D0));
            list.Add(new leaf("Cardae.Private.Key.CQW", false, false, 0x7F8, 0x4, "KEY 0x35 - XEKEY_CARDAE_PRIVATE_KEY"));
            list.Add(new leaf("Cardae.Private.Key.EXPONENT", false, false, 0x7FC, 0x4, "KEY 0x35 - XEKEY_CARDAE_PRIVATE_KEY"));
            list.Add(new leaf("Cardae.Private.Key.QWRESERVED", false, false, 0x800, 0x8, "KEY 0x35 - XEKEY_CARDAE_PRIVATE_KEY"));
            list.Add(new leaf("Cardae.Private.Key.MODULUS", false, false, 0x808, 0x80, "KEY 0x35 - XEKEY_CARDAE_PRIVATE_KEY"));
            list.Add(new leaf("Cardae.Private.Key.P", false, false, 0x888, 0x40, "KEY 0x35 - XEKEY_CARDAE_PRIVATE_KEY"));
            list.Add(new leaf("Cardae.Private.Key.Q", false, false, 0x8C8, 0x40, "KEY 0x35 - XEKEY_CARDAE_PRIVATE_KEY"));
            list.Add(new leaf("Cardae.Private.Key.DP", false, false, 0x908, 0x40, "KEY 0x35 - XEKEY_CARDAE_PRIVATE_KEY"));
            list.Add(new leaf("Cardae.Private.Key.DQ", false, false, 0x948, 0x40, "KEY 0x35 - XEKEY_CARDAE_PRIVATE_KEY"));
            list.Add(new leaf("Cardae.Private.Key.CR", false, false, 0x988, 0x40, "KEY 0x35 - XEKEY_CARDAE_PRIVATE_KEY", true));
            list.Add(new leaf("KEY 0x36 - XEKEY_CONSOLE_CERTIFICATE", false, true, 0x9C8, 0x1A8));
            list.Add(new leaf("Console.Certificate.CERT_SIZE", false, false, 0x9C8, 0x2, "KEY 0x36 - XEKEY_CONSOLE_CERTIFICATE"));
            list.Add(new leaf("Console.Certificate.CONSOLE_ID", false, false, 0x9CA, 0x5, "KEY 0x36 - XEKEY_CONSOLE_CERTIFICATE"));
            list.Add(new leaf("Console.Certificate.CONSOLE_PART_NUMBER", false, false, 0x9CF, 0xB, "KEY 0x36 - XEKEY_CONSOLE_CERTIFICATE"));
            list.Add(new leaf("Console.Certificate.RESERVED_DWORD", false, false, 0x9DA, 0x4, "KEY 0x36 - XEKEY_CONSOLE_CERTIFICATE"));
            list.Add(new leaf("Console.Certificate.PRIVILEGES", false, false, 0x9DE, 0x2, "KEY 0x36 - XEKEY_CONSOLE_CERTIFICATE"));
            list.Add(new leaf("Console.Certificate.CONSOLE_TYPE", false, false, 0x9E0, 0x4, "KEY 0x36 - XEKEY_CONSOLE_CERTIFICATE"));
            list.Add(new leaf("Console.Certificate.MANUFACTURING_DATE", false, false, 0x9E4, 0x8, "KEY 0x36 - XEKEY_CONSOLE_CERTIFICATE"));
            list.Add(new leaf("Console.Certificate.EXPONENT", false, false, 0x9EC, 0x4, "KEY 0x36 - XEKEY_CONSOLE_CERTIFICATE"));
            list.Add(new leaf("Console.Certificate.CONSOLE_PUBLIC_KEY", false, false, 0x9EC, 0x84, "KEY 0x36 - XEKEY_CONSOLE_CERTIFICATE"));
            list.Add(new leaf("Console.Certificate.MODULUS", false, false, 0x9F0, 0x80, "KEY 0x36 - XEKEY_CONSOLE_CERTIFICATE"));
            list.Add(new leaf("Console.Certificate.SIGNATURE", false, false, 0xA70, 0x100, "KEY 0x36 - XEKEY_CONSOLE_CERTIFICATE", true));
            list.Add(new leaf("Xeika.Certificate.CERT_SIZE", false, false, 0xb70, 0x2));
            list.Add(new leaf("KEY 0x37 - XEKEY_XEIKA_CERTIFICATE", false, true, 0xB72, 0x140));
            list.Add(new leaf("Xeika.Certificate.PUBLIC_KEY", false, true, 0xB72, 0x110, "KEY 0x37 - XEKEY_XEIKA_CERTIFICATE"));
            list.Add(new leaf("Xeika.Public.Key.CQW", false, false, 0xB72, 0x4, "Xeika.Certificate.PUBLIC_KEY"));
            list.Add(new leaf("Xeika.Public.Key.EXPONENT", false, false, 0xB76, 0x4, "Xeika.Certificate.PUBLIC_KEY"));
            list.Add(new leaf("Xeika.Public.Key.QWRESERVED", false, false, 0xB7A, 0x8, "Xeika.Certificate.PUBLIC_KEY"));
            list.Add(new leaf("Xeika.Public.Key.MODULUS", false, false, 0xB82, 0x100, "Xeika.Certificate.PUBLIC_KEY", true));
            list.Add(new leaf("Xeika.Certificate.OVERLAY_SIGNATURE", false, false, 0xC82, 0x4, "KEY 0x37 - XEKEY_XEIKA_CERTIFICATE"));
            list.Add(new leaf("Xeika.Certificate.OVERLAY_VERSION", false, false, 0xC86, 0x2, "KEY 0x37 - XEKEY_XEIKA_CERTIFICATE"));
            list.Add(new leaf("Xeika.Certificate.ODD_DATA_VERSION", false, false, 0xC88, 0x1, "KEY 0x37 - XEKEY_XEIKA_CERTIFICATE"));
            list.Add(new leaf("Xeika.Certificate.DRIVE_PHASE_LEVEL", false, false, 0xC89, 0x1, "KEY 0x37 - XEKEY_XEIKA_CERTIFICATE"));
            list.Add(new leaf("Xeika.Certificate.DRIVE_INQUIRY", false, false, 0xC8A, 0x28, "KEY 0x37 - XEKEY_XEIKA_CERTIFICATE"));
            list.Add(new leaf("Xeika.Certificate.RESERVED", false, false, 0xCB2, 0x1146, "KEY 0x37 - XEKEY_XEIKA_CERTIFICATE", true));
            list.Add(new leaf("KEY 0x44 - XEKEY_SPECIAL_KEYVAULT_SIGNATURE", false, false, 0x1DF8, 0x100));
            list.Add(new leaf("KEY 0x38 - XEKEY_CARDAE_CERTIFICATE", false, false, 0x1ef8, 0x2108, "Key Vault", true));
        }

        enum STATES
        {
            NONE,
            ROOT,
            LEAF,
            PARENT,
            LAST_CHILD
        }

        private void buildTree()
        {
            fillTree();
            TreeNode treeroot = new TreeNode();

            List<TreeNode> temp = new List<TreeNode>();
            STATES state = STATES.NONE;
            int i = 0;
            for (i = 0; i < list.Count; i++)
            {
                leaf l = list[i];
                if (l.isRoot)
                {
                    state = STATES.ROOT;
                    treeroot = new TreeNode(l.name);
                }
                else if (l.hasChildren)
                {
                    state = STATES.PARENT;
                    temp.Add(new TreeNode(l.name));
                    continue;
                }
                else if (l.lastChild)
                {
                    STATES tempstate = state;
                    state = STATES.LAST_CHILD;
                    if (temp.Count > 0)
                    {
                        temp[temp.Count - 1].Nodes.Add(l.name);
                        if (getLeaf(l.fatherName).fatherName == "Key Vault" || l.fatherName == "Key Vault") treeroot.Nodes.Add(temp[temp.Count - 1]);
                        else temp[temp.Count - 2].Nodes.Add(temp[temp.Count - 1]);
                        temp.RemoveAt(temp.Count - 1);
                    }
                    if (temp.Count > 0) state = tempstate;
                    else state = STATES.LEAF;
                    continue;
                }

                if (state != STATES.PARENT)
                {
                    treeroot.Nodes.Add(l.name);
                }
                if (state == STATES.PARENT)
                {
                    temp[temp.Count - 1].Nodes.Add(l.name);
                }

            }
            treeroot.Nodes.Add(list[i - 1].name);
            this.treeView1.Nodes.AddRange(new TreeNode[] { treeroot });
            //treeView1.ExpandAll();
        }

        private leaf getLeaf(string name)
        {
            foreach (leaf l in list)
            {
                if (l.name == name) return l;
            }
            return null;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            leaf l = getLeaf(e.Node.Text);
            hexBox1.Select(l.offset, l.size);
            toolStripStatusLabel1.Text = string.Format("Offset: 0x{0:X}    Size: 0x{1:X}",
                l.offset, l.size);
        }

    }
}
