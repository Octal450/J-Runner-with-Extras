using System;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class XValue : Form
    {
        public XValue()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Nand.Nand.DecryptXVal(txtSerial.Text, txtXVal.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSerial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtXVal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !System.Uri.IsHexDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
