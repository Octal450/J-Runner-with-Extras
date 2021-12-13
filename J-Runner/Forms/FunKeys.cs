using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class FunKeys : Form
    {
        public FunKeys()
        {
            InitializeComponent();
        }

        private void FunKeys_Load(object sender, EventArgs e)
        {
            txtCPUBox.Font = new Font("Courier New", 12);
            txtCPUBox.AppendText("B00B1E5B00B1E5B00B1E5B0FFFF346A5" + "\n" +
                                 "A55A55A55A55E5B00B1E5B1A5537E1FF" + "\n" + 
                                 "1337C0DE1337C0DE1337C0DE0F712824" + "\n" +
                                 "DEADBEEF0DEADBEEFF0000000031D984" + "\n" +
                                 "BAAAAAAD0BAAAAAADFBAAAAA00F0FC10" + "\n" +
                                 "0C7A10C7A10C7A10C7A10DFFFF57A395");
        }
    }
}
