using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace JRunner.Forms
{
    public partial class xeBuildOptions : Form
    {
        public xeBuildOptions()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Regex objAlphaPattern = new Regex("^[0-9]*$");
            try
            {
                variables.highldv = Convert.ToInt32(textBox1.Text);
                variables.changeldv = 2;
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            if(objAlphaPattern.IsMatch(textBox1.Text)) this.Close();
        }
        public void disableAdv()
        {
            chkAdvanced.Enabled = false;
        }

        private void chkAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAdvanced.Checked)
            {
                this.Height = 126;
            }
            else
            {
                this.Height = 100;
            }
            txtCBA.Visible = chkAdvanced.Checked;
            txtCBB.Visible = chkAdvanced.Checked;
            label2.Visible = chkAdvanced.Checked;
            label3.Visible = chkAdvanced.Checked;
            label4.Visible = chkAdvanced.Checked;
        }

        public bool getcbstate()
        {
            return chkAdvanced.Checked;
        }
        public void enumeratecbs(string cba = "", string cbb = "")
        {
            txtCBA.Text = cba;
            txtCBB.Text = cbb;
        }
        public void getcbs(out string cba, out string cbb)
        {
            cba = txtCBA.Text;
            cbb = txtCBB.Text;
        }
    }
}
