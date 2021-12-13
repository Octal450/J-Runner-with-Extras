using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace JRunner
{
    public partial class UpdateSuccess : Form
    {
        public UpdateSuccess()
        {
            InitializeComponent();
            SuccessWizard.Cancelling += WizardCancelled;
            SuccessWizard.Finished += WizardFinished;
        }

        private void WizardCancelled(object sender, EventArgs e)
        {
            Program.restart();
        }

        private void WizardFinished(object sender, EventArgs e)
        {
            Program.restart();
        }
    }
}
