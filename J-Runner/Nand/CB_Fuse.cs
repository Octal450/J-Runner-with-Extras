using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JRunner.Nand
{
    public partial class CB_Fuse : Form
    {
        public CB_Fuse()
        {
            InitializeComponent();
        }
        List<ListViewGroup> lgroups = new List<ListViewGroup>();
        List<ListViewItem> litems = new List<ListViewItem>();

        private void CB_Fuse_Load(object sender, EventArgs e)
        {
            this.CenterToParent();

            //Given that Microsoft isn't adding new console types anymore, lets sort this manually so its more pleasing to the user
            foreach (string s in variables.sortedConsoleNames)
            {
                cbListView.Groups.Add(s, s);
            }

            //foreach (consoles c in variables.ctypes)
            //{
            //    if (c.ID == -1 || c.ID == 9 || c.ID == 11 || c.ID == 5 || c.ID == 6 || c.ID == 7) continue;
            //    ListViewGroup lvg = new ListViewGroup(c.Text, c.Text);
            //    if (c.ID == 2) lvg = new ListViewGroup("Falcon", "Falcon");
            //    if (c.ID == 4) lvg = new ListViewGroup("Jasper", "Jasper");
            //    if (c.ID == 10) lvg = new ListViewGroup("Corona", "Corona");
            //
            //    lgroups.Add(lvg);
            //    cbListView.Groups.Add(lvg);
            //}

            foreach (ntable._nand n in ntable.Table)
            {
                if (n.csequence == 0) continue;

                string group = n.MotherBoard;
                string[] text = new string[4];

                text[0] = n.CB.ToString();
                text[1] = n.maxDashVersion.ToString();
                text[2] = n.csequence.ToString();

                ListViewItem lvi = new ListViewItem(text, cbListView.Groups[group]);
                litems.Add(lvi);
                cbListView.Items.Add(lvi);
            }
        }

        private void refresh()
        {
            cbListView.Clear();
            foreach (ListViewGroup lvg in lgroups) cbListView.Groups.Add(lvg);
            foreach (ListViewItem lvi in litems) cbListView.Items.Add(lvi);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}