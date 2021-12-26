using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace JRunner.Pirs
{
    public partial class STFS : Form
    {
        Pirs p;
        string filename2;
        public STFS(string filename, string ini)
        {
            InitializeComponent();
            p = new Pirs(filename, true);
            foreach (PirsEntry a in p.getList())
            {
                ListViewItem item = new ListViewItem
                {
                    Text = a.Filename
                };
                item.SubItems.Add(a.Size.ToString());
                item.SubItems.Add(a.Cluster.ToString());
                item.SubItems.Add(a.AccessTime.ToString());
                item.SubItems.Add(a.CRC.ToString("X"));
                this.listView.Items.Add(item);
            }

            filename2 = ini;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = listView.SelectedItems[0].Text;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                long cluster = Convert.ToInt64(listView.SelectedItems[0].SubItems[this.columnHeaderCluster.Index].Text);
                long size = Convert.ToInt64(listView.SelectedItems[0].SubItems[this.columnHeaderSize.Index].Text);
                p.extractFile(cluster, size, saveFileDialog.FileName);
            }
        }

        private void extractAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < listView.SelectedItems.Count; i++)
                {
                    if (p.isFolder(listView.SelectedItems[i].Text))
                    {
                        Console.WriteLine(string.Format("Extracting folder"));
                        p.extractFolder(0xffff, "", folderBrowserDialog.SelectedPath);
                    }
                    else
                    {
                        long cluster = Convert.ToInt64(listView.SelectedItems[i].SubItems[this.columnHeaderCluster.Index].Text);
                        long size = Convert.ToInt64(listView.SelectedItems[i].SubItems[this.columnHeaderSize.Index].Text);
                        p.extractFile(cluster, size, folderBrowserDialog.SelectedPath + @"\" + listView.SelectedItems[i].Text);
                    }
                }
                Console.WriteLine("Done");
            }
        }

        private void listView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = listView.PointToScreen(e.Location);
                contextMenuStrip1.Show(pt);
            }
        }

        private void extractAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine(string.Format("Extracting files"));
                p.extractFolder(0xffff, "", folderBrowserDialog.SelectedPath);
                Console.WriteLine("Done");
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (listView.SelectedItems.Count >= 1)
            {
            }
            else e.Cancel = true;
        }

        private void copyCRCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listView.SelectedItems[0].SubItems[this.columnCRC.Index].Text.ToLower());
        }

        private void STFS_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                if (listView.SelectedItems.Count >= 1 && listView.SelectedItems[0].Text == "xboxupd.bin")
                {
                    long cluster = Convert.ToInt64(listView.SelectedItems[0].SubItems[this.columnHeaderCluster.Index].Text);
                    long size = Convert.ToInt64(listView.SelectedItems[0].SubItems[this.columnHeaderSize.Index].Text);
                    byte[] CF, CG;
                    extractUpd(p.extractFile(cluster, size), out CF, out CG);
                }
            }
            else if (e.KeyCode == Keys.F9)
            {
                if (File.Exists(filename2)) update_ini(filename2);
            }

        }

        private void extractUpd(byte[] upd, out byte[] cf, out byte[] cg)
        {
            int CFlength = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(upd, 0xC, 4)), 16);
            int CGlength = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(upd, CFlength + 0xC, 4)), 16);
            cf = Oper.returnportion(upd, 0, CFlength);
            cg = Oper.returnportion(upd, CFlength, CGlength);

            cf = Nand.Nand.decrypt_CF(cf);
            cg = Nand.Nand.decrypt_CG(cg, cf);

            crc32 crc = new crc32();
            int dash = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(cf, 0x2, 2)), 16);
            Console.WriteLine("cf_" + dash + ".bin," + crc.CRC(editbl(cf)).ToString("X8").ToLower());
            crc = new crc32();
            dash = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(cg, 0x2, 2)), 16);
            Console.WriteLine("cg_" + dash + ".bin," + crc.CRC(editbl(cg)).ToString("X8").ToLower());
        }
        private byte[] editbl(byte[] bl)
        {
            int length = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(bl, 0xC, 4)), 16);
            if (bl[0] == 0x43 && bl[1] == 0x42)
            {
                for (int i = 0x10; i < 0x40; i++) bl[i] = 0x0;
            }
            else if (bl[0] == 0x43 && bl[1] == 0x44)
            {
                for (int i = 0x10; i < 0x20; i++) bl[i] = 0x0;
            }
            else if (bl[0] == 0x43 && bl[1] == 0x45)
            {
                for (int i = 0x10; i < 0x20; i++) bl[i] = 0x0;
            }
            else if (bl[0] == 0x43 && bl[1] == 0x46)
            {
                for (int i = 0x20; i < 0x230; i++) bl[i] = 0x0;
            }
            else if (bl[0] == 0x43 && bl[1] == 0x47)
            {
                for (int i = 0x10; i < 0x20; i++) bl[i] = 0x0;
            }
            else length = bl.Length;
            return Oper.returnportion(bl, 0, length);
        }

        private void update_ini(string filename)
        {
            string[] lines = File.ReadAllLines(filename);

            string old_version = "", new_version = "";
            string cf_checksum = "", cg_checksum = "";

            foreach (string s in lines) if (s.Contains("cf_"))
                {
                    old_version = s.Substring(3, 5);
                    break;
                }

            foreach (PirsEntry a in p.getList())
            {
                if (a.Filename == "xboxupd.bin")
                {
                    byte[] upd = p.extractFile(a.Cluster, a.Size);

                    int CFlength = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(upd, 0xC, 4)), 16);
                    int CGlength = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(upd, CFlength + 0xC, 4)), 16);
                    byte[] cf = Oper.returnportion(upd, 0, CFlength);
                    byte[] cg = Oper.returnportion(upd, CFlength, CGlength);

                    cf = Nand.Nand.decrypt_CF(cf);
                    cg = Nand.Nand.decrypt_CG(cg, cf);

                    crc32 crc = new crc32();
                    new_version = Convert.ToInt32(Oper.ByteArrayToString(Oper.returnportion(cf, 0x2, 2)), 16).ToString();
                    cf_checksum = crc.CRC(editbl(cf)).ToString("X8").ToLower();
                    crc = new crc32();
                    cg_checksum = crc.CRC(editbl(cg)).ToString("X8").ToLower();

                    break;
                }
            }
            bool test = false;
            Console.WriteLine(old_version);
            Console.WriteLine(new_version);
            Console.WriteLine(cf_checksum);
            Console.WriteLine(cg_checksum);
            for (int i = 0; i < lines.Length; i++)
            {
                if (String.IsNullOrWhiteSpace(lines[i])) continue;
                string line = lines[i];
                if (line.Contains(old_version))
                {
                    lines[i] = line.Replace(old_version, new_version);
                    line = lines[i];
                }
                if (line.Contains("cf_")) lines[i] = line.Substring(0, line.IndexOf(',')) + "," + cf_checksum;
                if (line.Contains("cg_")) lines[i] = line.Substring(0, line.IndexOf(',')) + "," + cg_checksum;

                if (line.Contains("[flashfs]"))
                {
                    test = true;
                    continue;
                }

                if (test)
                {
                    if (!line.Contains(",")) continue;
                    string name = line.Substring(0, line.IndexOf(','));
                    foreach (PirsEntry a in p.getList())
                    {
                        if ("$flash_" + name == a.Filename)
                        {
                            lines[i] = name + "," + a.CRC.ToString("X8").ToLower();
                            break;
                        }
                    }
                }
            }

            File.WriteAllLines(Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename) + "_new" + Path.GetExtension(filename)), lines);

        }

    }
}
