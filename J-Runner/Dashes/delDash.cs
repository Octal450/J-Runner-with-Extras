using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace JRunner.Dashes
{
    public partial class delDash : Form
    {
        public delDash()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (string value in checkedListBox1.CheckedItems)
            {
                try
                {
                    variables.dashes_all.Remove(value);
                    string dashfolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "xeBuild/" + value);
                    string dashfolder_2 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"xeBuild/xeBuild/" + value);
                    Directory.Delete(dashfolder, true);
                    Directory.Delete(dashfolder_2, true);
                    Console.WriteLine("Deleted " + value);
                }
                catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            }
            this.Close();
        }

        private void delDash_Load(object sender, EventArgs e)
        {
            string[] dashes_all = new String[0];
            try
            {
                dashes_all = variables.dashes_all.ToArray();
            }
            catch (NullReferenceException) { }
            foreach (string valueName in dashes_all)
            {
                try
                {
                    checkedListBox1.Items.Add(valueName);
                }
                catch (Exception ex)
                {
                    if (variables.debugme) Console.WriteLine(ex.ToString());
                    continue;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
