using CommPort;
using System;
using System.IO;
using System.Windows.Forms;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace JRunner.Forms
{
    public partial class Comport : Form
    {
        CommunicationManager comm = new CommunicationManager();

        public Comport()
        {
            InitializeComponent();
        }

        private void Comport_Load(object sender, EventArgs e)
        {
            LoadValues();
            SetDefaults();
            SetControlState();
            updateLogColor();
        }

        private void SetDefaults()
        {
            try
            {
                cboPort.SelectedIndex = 0;
                cboBaud.Items.Add("115200");
                cboBaud.SelectedText = "115200";
                cboParity.SelectedIndex = 0;
                cboStop.SelectedIndex = 1;
                cboData.SelectedIndex = 1;
            }
            catch (System.ArgumentOutOfRangeException) { MessageBox.Show("No COM ports were found"); }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        private void LoadValues()
        {
            try
            {
                comm.SetPortNameValues(cboPort);
                //if (variables.COMPort != "" && cboPort.Items.Count > 0) cboPort.SelectedText = variables.COMPort;
                if (cboPort.Items.Count > 0) cboPort.SelectedIndex = 0;
                comm.SetParityValues(cboParity);
                comm.SetStopBitValues(cboStop);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        private void SetControlState()
        {
            closePortToolStripMenuItem.Enabled = false;
            BaudRate.Visible = false;
            DataBits.Visible = false;
            StopBits.Visible = false;
            Parity.Visible = false;
            cboBaud.Visible = false;
            cboData.Visible = false;
            cboStop.Visible = false;
            cboParity.Visible = false;
        }

        private void Comport_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                comm.ClosePort();
            }
            catch (Exception ex) { Console.WriteLine(ex.InnerException); }
        }

        private void cboPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            variables.COMPort = cboPort.SelectedItem.ToString();
        }

        private void Comport_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            cboPort.Items.Clear();
            LoadValues();
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rbtnHex.Checked) comm.CurrentTransmissionType = CommPort.CommunicationManager.TransmissionType.Hex;
            else comm.CurrentTransmissionType = CommPort.CommunicationManager.TransmissionType.Text;
            comm.PortName = cboPort.Text;
            comm.Parity = cboParity.Text;
            comm.StopBits = cboStop.Text;
            comm.DataBits = cboData.Text;
            comm.BaudRate = cboBaud.Text;
            comm.DisplayWindow = textBox1;
            if (cboPort.Text == "") return;
            try
            {
                comm.OpenPort();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            openPortToolStripMenuItem.Enabled = false;
            closePortToolStripMenuItem.Enabled = true;
        }

        private void closePortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                closePortToolStripMenuItem.Enabled = false;
                openPortToolStripMenuItem.Enabled = true;
                comm.ClosePort();
            }
            catch (Exception ex) { Console.WriteLine(ex.InnerException); }
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void advancedChk_CheckedChanged(object sender, EventArgs e)
        {
            if (advancedChk.Checked)
            {
                BaudRate.Visible = true;
                DataBits.Visible = true;
                StopBits.Visible = true;
                Parity.Visible = true;
                cboBaud.Visible = true;
                cboData.Visible = true;
                cboStop.Visible = true;
                cboParity.Visible = true;
            }
            else
            {
                BaudRate.Visible = false;
                DataBits.Visible = false;
                StopBits.Visible = false;
                Parity.Visible = false;
                cboBaud.Visible = false;
                cboData.Visible = false;
                cboStop.Visible = false;
                cboParity.Visible = false;
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Log Files|*.log|Text Documents|*.txt|All Files|*.*";
            saveDialog.Title = "Export COM Log";
            saveDialog.ShowDialog();

            if (saveDialog.FileName != "")
            {
                File.WriteAllText(saveDialog.FileName, textBox1.Text);
            }
        }

        public void updateLogColor()
        {
            textBox1.BackColor = variables.logbackground;
            textBox1.ForeColor = variables.logtext;
        }
    }
}
