using System;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class NandSel : Form
    {
        //public delegate void ClickedSize(int size);
        //public event ClickedSize SizeClick;

        public NandSel()
        {
            InitializeComponent();
        }

        private void btn16_Click(object sender, EventArgs e)
        {
            //SizeClick(16);
            //this.Close();
        }

        private void btn64_Click(object sender, EventArgs e)
        {
            //SizeClick(64);
            //this.Close();
        }

        private void btn256_Click(object sender, EventArgs e)
        {
            //SizeClick(256);
            //this.Close();
        }

        private void btn512_Click(object sender, EventArgs e)
        {
            //SizeClick(512);
            //this.Close();
        }

        private void btn1p28_Click(object sender, EventArgs e)
        {
            //SizeClick(1);
            //this.Close();
        }

        public void setGroups(int seltype, bool bb)
        {
            //if (bb) AdvancedGroup.Enabled = SmallBlockGroup.Enabled = false;
            //else
            //{
            //    if (seltype == 2) AdvancedGroup.Enabled = false;
            //    else AdvancedGroup.Enabled = true;

            //    SmallBlockGroup.Enabled = true;
            //}
        }
    }
}
