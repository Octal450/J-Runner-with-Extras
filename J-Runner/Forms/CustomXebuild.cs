using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class CustomXebuild : Form
    {
        public CustomXebuild()
        {
            InitializeComponent();
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        public string getString()
        {
            return txtCommand.Text;
        }

    }
}
