using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace JRunner.Nand
{
    public partial class VNandForm : Form
    {
        public string filename;
        public List<int> BadBlocks = new List<int>();
        public string flashconfig;
        public consoles console;
        private int blockselected;

        public VNandForm()
        {
            InitializeComponent();
            this.AcceptButton = btnCancel;
        }

        private void VNandForm_Load(object sender, EventArgs e)
        {
            foreach (consoles c in variables.cunts)
            {
                if (c.ID == -1 || c.ID == 11) continue;
                listBoxConsoles.Items.Add(c.Text);
            }
            foreach (string config in variables.flashconfigs)
            {
                listBoxConfigs.Items.Add(config);
            }
        }

        private void btnAddBadBlock_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtBadBlock.Text))
            {
                int result = 0;
                if (int.TryParse(txtBadBlock.Text, System.Globalization.NumberStyles.HexNumber, new CultureInfo("en-US"), out result))
                {
                    listBoxBadBlocks.Items.Add(txtBadBlock.Text);
                    BadBlocks.Add(result);
                }
            }
        }

        private void btnRemoveBadBlock_Click(object sender, EventArgs e)
        {
            int result = 0;
            if (int.TryParse(listBoxBadBlocks.Items[blockselected].ToString(), System.Globalization.NumberStyles.HexNumber, new CultureInfo("en-US"), out result)) if (BadBlocks.Contains(result)) BadBlocks.Remove(result);
            listBoxBadBlocks.Items.RemoveAt(blockselected);
        }

        private void btnSaveTo_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = sf.FileName;
                filename = sf.FileName;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void listBoxConsoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxConfigs.Items.Clear();
            foreach (consoles c in variables.cunts)
            {
                if (c.ID == -1) continue;
                if (listBoxConsoles.Items[listBoxConsoles.SelectedIndex].ToString() == c.Text) console = c;
            }

            if (console.ID == 1 || console.ID == 4) listBoxConfigs.Items.Add("00023010");
            else if (console.ID == 10) listBoxConfigs.Items.Add("00043000");
            else if (console.ID == 6 || console.ID == 7)
            {
                listBoxConfigs.Items.Add("008A3020");
                listBoxConfigs.Items.Add("00AA3020");
            }
            else listBoxConfigs.Items.Add("01198010");


        }

        private void listBoxConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {
            flashconfig = listBoxConfigs.Items[listBoxConfigs.SelectedIndex].ToString();
        }

        private void listBoxBadBlocks_SelectedIndexChanged(object sender, EventArgs e)
        {
            blockselected = listBoxBadBlocks.SelectedIndex;
        }
    }
}
