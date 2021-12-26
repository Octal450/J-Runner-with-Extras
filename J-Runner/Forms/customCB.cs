using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class customCB : Form
    {
        List<int> CBs;
        public int selected;
        public customCB(List<int> CBs)
        {
            InitializeComponent();
            this.CBs = CBs;
        }

        private void customCB_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.RadioButton[] radioButtons =
         new System.Windows.Forms.RadioButton[CBs.Count];

            for (int i = 0; i < CBs.Count; ++i)
            {
                radioButtons[i] = new RadioButton();
                radioButtons[i].Text = CBs[i].ToString();
                radioButtons[i].CheckedChanged += customCB_CheckedChanged;
                radioButtons[i].Location = new System.Drawing.Point(
                    10, 10 + i * 20);
                this.Controls.Add(radioButtons[i]);
            }

            if (CBs.Count >= 1) radioButtons[0].Checked = true;
        }

        void customCB_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked) int.TryParse(((RadioButton)sender).Text, out selected);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
