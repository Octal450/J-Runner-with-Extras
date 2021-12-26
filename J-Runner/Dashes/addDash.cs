using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JRunner
{
    public partial class addDash : Form
    {
        public addDash()
        {
            InitializeComponent();
            btnAdd.Visible = false;
            this.CancelButton = Cancel;
            this.AcceptButton = buttonOK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (variables.dashes_all == null)
            {
                variables.dashes_all = new List<string>();
            }
            foreach (string value in checkedDashes.CheckedItems)
            {
                if (!variables.dashes_all.Contains(value)) variables.dashes_all.Add(value);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkAdvanced.Checked = true;
            btnAdd.Visible = true;
            textAdvAdd.Visible = true;

            foreach (string valueName in variables.dashes_all)
            {
                for (int i = 0; i < checkedDashes.Items.Count; i++)
                {
                    if (valueName == (string)checkedDashes.Items[i])
                    {
                        checkedDashes.SetItemChecked(i, true);
                    }
                }
            }
        }

        private void checkAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            btnAdd.Visible = checkAdvanced.Checked;
            textAdvAdd.Visible = checkAdvanced.Checked;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Regex regex = new Regex("^[0-9]+$");
            if (regex.IsMatch(textAdvAdd.Text))
            {
                checkedDashes.Items.Add(textAdvAdd.Text, true);
                Application.DoEvents();
            }
        }
    }
}
