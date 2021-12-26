using System;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class xFlasherNandSel : Form
    {

        public delegate void ClickedSize(int size);
        public event ClickedSize SizeClick;

        public xFlasherNandSel()
        {
            InitializeComponent();
        }

        private void btn16_Click(object sender, EventArgs e)
        {
            SizeClick(16);
            this.Close();
        }

        private void btn64_Click(object sender, EventArgs e)
        {
            SizeClick(64);
            this.Close();
        }

        private void btn256_Click(object sender, EventArgs e)
        {
            SizeClick(256);
            this.Close();
        }

        private void btn512_Click(object sender, EventArgs e)
        {
            SizeClick(512);
            this.Close();
        }

        public void BigBlock(bool bb)
        {
            if (bb) SmallBlockGroup.Enabled = false;
            else SmallBlockGroup.Enabled = true;
        }
    }
}
