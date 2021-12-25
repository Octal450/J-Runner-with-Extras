using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JRunner
{
    public partial class Issues : Form
    {
        public Issues()
        {
            InitializeComponent();
            IssueWizard.Cancelling += WizardCancelled;
            IssueWizard.Finished += WizardFinished;
        }

        private void WizardCancelled(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WizardFinished(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Octal450/J-Runner-with-Extras/issues");
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Octal450/J-Runner-with-Extras/issues/new/choose");
        }
    }
}
