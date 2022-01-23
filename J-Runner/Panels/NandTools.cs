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

        public NandTools(string lptport)
        {
            txtLPTPort.Text = lptport;
        }

        public NandTools(int iterations)
        {
            numericIterations.Value = iterations;
        }

        public NandTools(string lptport, int iterations)
        {
            numericIterations.Value = iterations;
            txtLPTPort.Text = lptport;
        }

        public int getNumericIterations()
        {
            return (int)numericIterations.Value;
        }
        public void setNumericIterations(decimal value)
        {
            numericIterations.Value = value;
        }
        public string getLptPort()
        {
            return txtLPTPort.Text;
        }
        public void setLptPort(string port)
        {
            txtLPTPort.Text = port;
        }
        public bool getRbtnUSB()
        {
            return rbtnUSB.Checked;
        }
        public bool getRbtnLPT()
        {
            return rbtnLPT.Checked;
        }

        public void setbtnCreateECCEnabled(bool b)
        {
            btnCreateECC.Enabled = b;
        }
        public void setbtnCreateECC(string text)
        {
            btnCreateECC.Text = text;
        }
        public string getbtnCreateECC()
        {
            return btnCreateECC.Text;
        }
        public void setbtnWriteECC(string text)
        {
            btnWriteECC.Text = text;
        }
        public string getbtnWriteECC()
        {
            return btnWriteECC.Text;
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
        //public delegate void CheckedChanged();
        //public event CheckedChanged ChangedChecked;
        //public delegate void PortChanged();
        //public event PortChanged ChangedPort;

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

        private void rbtn_CheckedChanged(object sender, EventArgs e)
        {
            //ChangedChecked();
            txtLPTPort.Visible = (rbtnLPT.Checked);
            lblLPTPort.Visible = txtLPTPort.Visible;
        }

        private void numericIterations_ValueChanged(object sender, EventArgs e)
        {
            IterChange((int)numericIterations.Value);
        }

        private void btnCPUDB_Click(object sender, EventArgs e)
        {
            CPUDBClick();
        }

        private void txtLPTPort_TextChanged(object sender, EventArgs e)
        {
            //ChangedPort();
        }

        private int eeCount = 0;
        private void pBoxDevice_Click(object sender, EventArgs e)
        {
            if (eeCount == 5)
            {
                MessageBox.Show("Wtf are you doing!?!?!", "Confusion!", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else if (eeCount == 8)
            {
                MessageBox.Show("#%&@ Stop doing that!!!!!", "#%&@", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (eeCount == 10)
            {
                MessageBox.Show("Cut that shit out!!!!!", "You're Annoying!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (eeCount == 12)
            {
                MessageBox.Show("CLICK ME AGAIN!\nI DARE YOU!", "You Gon Get It", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (eeCount == 13)
            {
                SoundPlayer goodbye = new SoundPlayer(Properties.Resources.goodbye);
                goodbye.Play();
                Thread.Sleep(1000);
                Application.Exit();
            }
            eeCount += 1;
        }
    }
}
