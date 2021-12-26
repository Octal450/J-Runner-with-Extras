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

        protected override void WndProc(ref Message message)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            switch (message.Msg)
            {
                case WM_SYSCOMMAND:
                    int command = message.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        return;
                    break;
            }

            base.WndProc(ref message);
        }

        private void CB_Fuse_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            foreach (JRunner.consoles c in JRunner.variables.cunts)
            {
                if (c.ID == -1 || c.ID == 9 || c.ID == 11 || c.ID == 5 || c.ID == 6 || c.ID == 7) continue;
                ListViewGroup lvg = new ListViewGroup(c.Text, c.Text);
                if (c.ID == 2) lvg = new ListViewGroup("Falcon", "Falcon");
                if (c.ID == 4) lvg = new ListViewGroup("Jasper", "Jasper");

                lgroups.Add(lvg);
                listView1.Groups.Add(lvg);
            }
            foreach (ntable._nand n in ntable.Table)
            {
                string group = n.Cunt.Text;
                string[] text = new string[4];
                if (group == "Jasper 16MB") group = "Jasper";

                text[0] = n.CB.ToString();
                text[1] = n.minDashVersion.ToString();
                text[2] = n.maxDashVersion.ToString();
                string cseq = "";
                if (n.minCsequence == n.maxCsequence) cseq = n.minCsequence.ToString();
                else cseq = n.minCsequence.ToString() + "-" + n.maxCsequence.ToString();
                text[3] = cseq;

                ListViewItem lvi = new ListViewItem(text, listView1.Groups[group]);
                litems.Add(lvi);
                listView1.Items.Add(lvi);
            }
        }

        private void refresh()
        {
            listView1.Clear();
            foreach (ListViewGroup lvg in lgroups) listView1.Groups.Add(lvg);
            foreach (ListViewItem lvi in litems) listView1.Items.Add(lvi);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}