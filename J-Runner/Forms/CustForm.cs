using System;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class custform : Form
    {
        public custform()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            variables.custname = textBox1.Text;
            this.Close();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                variables.custname = textBox1.Text;
                this.Close();
            }
        }
    }
}
