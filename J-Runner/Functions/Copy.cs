using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace JRunner.Functions
{
    public partial class Copy : Form
    {
        private bool cancel = false;
        private string _source, _destination;
        public Copy(string source, string destination)
        {
            InitializeComponent();
            _source = source;
            _destination = destination;
        }

        public delegate void ProgressChangeDelegate(double Persentage, ref bool Cancel);
        public delegate void Completedelegate();

        class CustomFileCopier
        {
            bool delete = false;
            public CustomFileCopier(string Source, string Dest)
            {
                this.SourceFilePath = Source;
                this.DestFilePath = Dest;

                OnProgressChanged += delegate { };
                OnComplete += delegate { };
            }

            public void Copy()
            {
                byte[] buffer = new byte[1024 * 1024]; // 1MB buffer
                bool cancelFlag = false;

                using (FileStream source = new FileStream(SourceFilePath, FileMode.Open, FileAccess.Read))
                {
                    long fileLength = source.Length;
                    using (FileStream dest = new FileStream(DestFilePath, FileMode.Create, FileAccess.Write))
                    {
                        long totalBytes = 0;
                        int currentBlockSize = 0;

                        while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            totalBytes += currentBlockSize;
                            double persentage = totalBytes * 100.0 / fileLength;

                            dest.Write(buffer, 0, currentBlockSize);

                            cancelFlag = false;
                            OnProgressChanged(persentage, ref cancelFlag);

                            if (cancelFlag == true)
                            {
                                delete = true;
                                break;
                            }
                        }
                    }
                }
                if (delete)
                {
                    try
                    {
                        File.Delete(DestFilePath);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
                OnComplete();
            }

            public string SourceFilePath { get; set; }
            public string DestFilePath { get; set; }

            public event ProgressChangeDelegate OnProgressChanged;
            public event Completedelegate OnComplete;
        }

        private void Copy_Load(object sender, EventArgs e)
        {
            CustomFileCopier c = new CustomFileCopier(_source, _destination);
            c.OnProgressChanged += c_OnProgressChanged;
            c.OnComplete += c_OnComplete;
            new Thread(c.Copy).Start();
        }

        void c_OnComplete()
        {
            this.Close();
        }

        void c_OnProgressChanged(double Persentage, ref bool Cancel)
        {
            progressBar1.Value = (int)Persentage > progressBar1.Maximum ? 100 : (int)Persentage;
            Cancel = cancel;
        }

        private void Copy_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cancel = true;
        }
    }
}
