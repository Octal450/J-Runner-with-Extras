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
        public void setNumericIterations(int value)
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
            MainForm.mainForm.openCpuKeyDb();
        }

        private int funCount = 0;
        private void pBoxDevice_Click(object sender, EventArgs e)
        {
            if (funCount == 5)
            {
                MessageBox.Show("WTF are you DOING?!?!?", "Confusion!", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else if (funCount == 8)
            {
                MessageBox.Show("Stop doing that!!!!!", "You're annoying!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (funCount == 10)
            {
                MessageBox.Show("Cut that #$@!% out!!!!!", "Stop Plz!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (funCount == 12)
            {
                MessageBox.Show("CLICK ME AGAIN!\nI DARE YOU!", "You Gon Get It!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (funCount == 13)
            {
                if (variables.reading || variables.writing)
                {
                    MessageBox.Show("The easter egg rudely tried to interrupt your nand read/write and needs a timeout\n\nTry again later", "Hypervisor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    funCount = 0;
                }
                else
                {
                    SoundPlayer goodbye = new SoundPlayer(Properties.Resources.goodbye);
                    goodbye.Play();
                    Thread goodbyeThread = new Thread(() =>
                    {
                        Thread.Sleep(1000);
                        Application.Exit();
                    });
                    goodbyeThread.Start();
                    MessageBox.Show("I warned ya!", ":/", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            funCount += 1;
        }
    }
}
