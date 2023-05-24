using System;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class CustomXeBuild : Form
    {
        public CustomXeBuild()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string getString()
        {
            return txtCommand.Text;
        }

        private void txtCommand_TextChanged(object sender, EventArgs e)
        {
            if (txtCommand.TextLength > 0)
            {
                if (txtCommand.Text.Contains("-d"))
                {
                    // Cut down on dialog spam, only warn the user once
                    if (btnRun.Enabled) MessageBox.Show("You cannot specify a custom -d due to limitations of how J-Runner's XeBuild sequencing works", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    btnRun.Enabled = false;
                }
                else btnRun.Enabled = true;
            }
            else btnRun.Enabled = false;
        }
    }
}
