using System;
using System.Windows.Forms;

namespace JRunner
{
    public partial class FormGoTo : Form
    {
        public FormGoTo()
        {
            InitializeComponent();
        }

        public void SetDefaultValue(long byteIndex)
        {
            txtOffset.Text = (byteIndex).ToString("X");
        }

        public long GetByteIndex()
        {
            if (rbtnHex.Checked) return Convert.ToInt64(txtOffset.Text, 16);
            else return Convert.ToInt64(txtOffset.Text);
        }

        private void FormGoTo_Activated(object sender, System.EventArgs e)
        {
            txtOffset.Focus();
            txtOffset.Select(0, txtOffset.Text.Length);
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void rbtn_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbtnDec.Checked)
                {
                    txtOffset.Text = Convert.ToInt64(txtOffset.Text, 16).ToString();
                }
                else
                {
                    txtOffset.Text = Convert.ToInt64(txtOffset.Text).ToString("X");
                }
            }
            catch (Exception) { }
        }
    }
}
