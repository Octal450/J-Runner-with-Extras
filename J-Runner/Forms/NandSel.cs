using System;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class NandSel : Form
    {
        public NandSel()
        {
            InitializeComponent();
        }

        private void btn16_Click(object sender, EventArgs e)
        {
            variables.fulldump = false;
            variables.read1p28mb = false;
            this.Close();
        }

        private void btn64_Click(object sender, EventArgs e)
        {
            variables.fulldump = false;
            variables.read1p28mb = false;
            this.Close();
        }

        private void btn256_Click(object sender, EventArgs e)
        {
            variables.fulldump = true;
            variables.read1p28mb = false;
            this.Close();
        }

        private void btn512_Click(object sender, EventArgs e)
        {
            variables.fulldump = true;
            variables.read1p28mb = false;
            this.Close();
        }

        private void btn1p28_Click(object sender, EventArgs e)
        {
            variables.fulldump = false;
            variables.read1p28mb = true;
            this.Close();
        }

        public void setGroups(int bb)
        {
            if (bb > 0) SmallBlockGroup.Enabled = false;
            else SmallBlockGroup.Enabled = true;

            if (bb == 2) btn512.Enabled = false;
            else btn512.Enabled = true;

            if (bb == 3) btn256.Enabled = false;
            else btn256.Enabled = true;
        }
    }
}
