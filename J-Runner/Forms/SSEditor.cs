using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JRunner
{
    public partial class SlimSounds : Form
    {
        private List<RadioButton> _radioButtonGroup = new List<RadioButton>();
        public SlimSounds()
        {
            InitializeComponent();
        }
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                
                
                foreach (RadioButton other in _radioButtonGroup)
                {
                    if (other == rb)
                    {
                        continue;
                    }
                    other.Checked = false;
                }
            }
        }
    }





}
