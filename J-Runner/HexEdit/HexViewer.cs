using Be.Windows.Forms;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace JRunner.HexEdit
{
    public partial class HexViewer : Form
    {
        FormGoTo _formGoto = new FormGoTo();
        FormFind _formFind;
        FindOptions _findOptions = new FindOptions();
        private bool file = false;

        public HexViewer(string filename)
        {
            file = true;
            InitializeComponent();
            if (File.Exists(filename))
            {
                DynamicFileByteProvider dynamicFileByteProvider;
                try
                {
                    dynamicFileByteProvider = new DynamicFileByteProvider(filename);
                    hexBox1.ByteProvider = dynamicFileByteProvider;
                }
                catch (IOException) // write mode failed
                {
                    try
                    {
                        // try to open in read-only mode
                        dynamicFileByteProvider = new DynamicFileByteProvider(filename, true);
                        Console.WriteLine("Opened in read-only mode.");
                    }
                    catch (IOException) // read-only also failed
                    {
                        // file cannot be opened
                        Console.WriteLine("Open Failed");
                        return;
                    }
                }
            }
        }
        public HexViewer(byte[] bytes)
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

        private void hexBox1_DragDrop(object sender, DragEventArgs e)
        {
            object oFileNames = e.Data.GetData(DataFormats.FileDrop);
            string[] fileNames = (string[])oFileNames;
            if (fileNames.Length == 1)
            {
                if (File.Exists(fileNames[0]))
                {
                    DynamicFileByteProvider dynamicFileByteProvider;
                    try
                    {
                        dynamicFileByteProvider = new DynamicFileByteProvider(fileNames[0]);
                        hexBox1.ByteProvider = dynamicFileByteProvider;
                    }
                    catch (IOException) // write mode failed
                    {
                        try
                        {
                            // try to open in read-only mode
                            dynamicFileByteProvider = new DynamicFileByteProvider(fileNames[0], true);
                            Console.WriteLine("Opened in read-only mode.");
                        }
                        catch (IOException) // read-only also failed
                        {
                            // file cannot be opened
                            Console.WriteLine("Open Failed");
                            return;
                        }
                    }
                }
            }
        }
        private void hexBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }
        private void HexViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hexBox1.ByteProvider != null && hexBox1.ByteProvider.HasChanges())
            {
                DialogResult res = MessageBox.Show("Do you want to save changes?",
                    "Save",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    if (file) SaveFile();
                }
            }

            if (file) CleanUp();
        }
        private void HexViewer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F2)
            {
                this.Text += " (Edit Mode)";
                hexBox1.BackColor = Color.LimeGreen;
                hexBox1.ReadOnly = false;
            }
            else if (e.Control && e.KeyCode == Keys.G)
            {
                Goto();
            }
            else if (e.Control && e.KeyCode == Keys.F)
            {
                Find();
            }
        }
        public void getBytes(ref byte[] bytes)
        {
            if (hexBox1.ByteProvider == null)
                return;
            try
            {
                DynamicByteProvider dynamicByteProvider = hexBox1.ByteProvider as DynamicByteProvider;
                bytes = dynamicByteProvider.Bytes.ToArray();
            }
            catch (Exception ex1)
            {
                Console.WriteLine(ex1.Message);
                if (variables.debugme) Console.WriteLine(ex1.ToString());
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
                _formFind.Location = new Point(Location.X + (Width - _formFind.Width) / 2, Location.Y + (Height - _formFind.Height) / 2);
            }
            else
            {
                _formFind.Focus();
            }
        }

    }
}
