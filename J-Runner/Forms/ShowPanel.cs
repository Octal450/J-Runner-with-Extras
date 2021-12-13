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
    public partial class ShowPanel : Form
    {
        public ShowPanel()
        {
            InitializeComponent();
        }

        public void addPanel(Control c){
            panel1.Controls.Add(c);
        }
    }
}
