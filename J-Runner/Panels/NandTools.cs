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

        private void btnRead_Click(object sender, EventArgs e)
        {
            MainForm.mainForm.btnReadClick();
        }

        private void btnCreateECC_Click(object sender, EventArgs e)
        {
            MainForm.mainForm.btnCreateECCClick();
        }

        private void btnWriteECC_Click(object sender, EventArgs e)
        {
            MainForm.mainForm.btnWriteECCClick();
        }

        private void btnXeBuild_Click(object sender, EventArgs e)
        {
            MainForm.mainForm.btnXeBuildClick();
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            MainForm.mainForm.btnWriteClick();
        }

        private void btnProgramCR_Click(object sender, EventArgs e)
        {
            MainForm.mainForm.openXsvfChoice(true);
        }

        private void numericIterations_ValueChanged(object sender, EventArgs e)
        {
            MainForm.mainForm.nTools_IterChange((int)numericIterations.Value);
        }

        private void btnExtractFiles_Click(object sender, EventArgs e)
        {
            MainForm.mainForm.extractFilesFromNand();
        }

        private void btnCreateDonor_Click(object sender, EventArgs e)
        {
            MainForm.mainForm.createDonorNand();
        }

        private void btnPatchKv_Click(object sender, EventArgs e)
        {
            MainForm.mainForm.openPatchKv();
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
