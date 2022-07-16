using System;
using System.Drawing;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace JRunner.Panels
{
    public partial class NandTools : UserControl
    {
        public NandTools()
        {
            InitializeComponent();
        }

        public int getNumericIterations()
        {
            return (int)numericIterations.Value;
        }
        public void setNumericIterations(decimal value)
        {
            numericIterations.Value = value;
        }

        public void setImage(Image m)
        {
            pBoxDevice.Image = m;
        }

        public delegate void ClickedRead();
        public event ClickedRead ReadClick;
        public delegate void ClickedCreateECC();
        public event ClickedCreateECC CreateEccClick;
        public delegate void ClickedWriteECC();
        public event ClickedWriteECC WriteEccClick;
        public delegate void ClickedXeBuild();
        public event ClickedXeBuild XeBuildClick;
        public delegate void ClickedWrite();
        public event ClickedWrite WriteClick;
        public delegate void ClickedProgramCR();
        public event ClickedProgramCR ProgramCRClick;
        public delegate void ClickedCPUDB();
        public event ClickedCPUDB CPUDBClick;
        public delegate void ChangedIter(int iter);
        public event ChangedIter IterChange;

        private void btnRead_Click(object sender, EventArgs e)
        {
            ReadClick();
        }

        private void btnCreateECC_Click(object sender, EventArgs e)
        {
            CreateEccClick();
        }

        private void btnWriteECC_Click(object sender, EventArgs e)
        {
            WriteEccClick();
        }

        private void btnXeBuild_Click(object sender, EventArgs e)
        {
            XeBuildClick();
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            WriteClick();
        }

        private void btnProgramCR_Click(object sender, EventArgs e)
        {
            ProgramCRClick();
        }

        private void numericIterations_ValueChanged(object sender, EventArgs e)
        {
            IterChange((int)numericIterations.Value);
        }

        private void btnExtractFiles_Click(object sender, EventArgs e)
        {
            MainForm.mainForm.extractFilesFromNand();
        }

        private void btnCreateDonor_Click(object sender, EventArgs e)
        {
            MainForm.mainForm.createDonorNand();
        }

        private void btnCPUDB_Click(object sender, EventArgs e)
        {
            CPUDBClick();
        }

        private int funCount = 0;
        private void pBoxDevice_Click(object sender, EventArgs e)
        {
            if (funCount == 5)
            {
                MessageBox.Show("Wtf are you doing!?!?!", "Confusion!", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else if (funCount == 8)
            {
                MessageBox.Show("#%&@ Stop doing that!!!!!", "#%&@", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (funCount == 10)
            {
                MessageBox.Show("Cut that shit out!!!!!", "You're Annoying!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (funCount == 12)
            {
                MessageBox.Show("CLICK ME AGAIN!\nI DARE YOU!", "You Gon Get It", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (funCount == 13)
            {
                SoundPlayer goodbye = new SoundPlayer(Properties.Resources.goodbye);
                goodbye.Play();
                Thread.Sleep(1000);
                Application.Exit();
            }
            funCount += 1;
        }
    }
}
