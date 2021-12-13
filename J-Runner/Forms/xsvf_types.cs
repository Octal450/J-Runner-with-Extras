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
    public partial class xsvf_types : Form
    {
        private List<RadioButton> _radioButtonGroup = new List<RadioButton>();

        public xsvf_types()
        {
            InitializeComponent();
            this.CancelButton = btnCancel;
            btnCancel.DialogResult = DialogResult.Cancel;
        }
        
        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
