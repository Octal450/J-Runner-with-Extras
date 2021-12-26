using Microsoft.Win32;
using System;
using System.Data;
using System.Windows.Forms;

namespace JRunner.CPUkeydb
{
    public partial class Editmobo : Form
    {
        DataSet1 hi;

        public Editmobo(string ID, string Serial, string CPU, string Mobo, DataSet1 his)
        {
            InitializeComponent();
            this.IdentTB.Text = ID;
            this.SerialTB.Text = Serial;
            this.CpukeyTB.Text = CPU;
            this.ConsTypeTB.Text = Mobo;
            hi = his;
        }

        private void CancelBut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditOKBut_Click(object sender, EventArgs e)
        {
            DataTable cputable = hi.DataTable1;
            cputable.Rows[Convert.ToInt32(IdentTB.Text) - 1]["Comment"] = ConsTypeTB.Text;
            try
            {
                RegistryKey cpukeydb = Registry.CurrentUser.OpenSubKey("CPUKey_DB");
                RegistryKey cpukeys = cpukeydb.OpenSubKey(IdentTB.Text, true);
                cpukeys.SetValue("Mobo", ConsTypeTB.Text);
            }
            catch (Exception)
            {
            }
            this.Close();
        }

        private void Editmobo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataTable cputable = hi.DataTable1;
                cputable.Rows[Convert.ToInt32(IdentTB.Text) - 1]["Comment"] = ConsTypeTB.Text;
                try
                {
                    RegistryKey cpukeydb = Registry.CurrentUser.OpenSubKey("CPUKey_DB");
                    RegistryKey cpukeys = cpukeydb.OpenSubKey(IdentTB.Text, true);
                    cpukeys.SetValue("Mobo", ConsTypeTB.Text);
                }
                catch (Exception)
                {
                }
                this.Close();
            }
        }
    }
}
