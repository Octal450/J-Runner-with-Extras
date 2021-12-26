using Be.Windows.Forms;
using System;
using System.Windows.Forms;

namespace JRunner.HexEdit
{
    partial class FormFind : Form
    {
        public FormFind()
        {
            InitializeComponent();
        }

        private FindOptions _findOptions;
        public FindOptions FindOptions
        {
            get
            {
                return _findOptions;
            }
            set
            {
                _findOptions = value;
                Reinitialize();
            }
        }

        public HexBox HexBox { get; set; }

        private void Reinitialize()
        {
            rbString.Checked = _findOptions.Type == FindType.Text;
            txtFind.Text = _findOptions.Text;
            chkMatchCase.Checked = _findOptions.MatchCase;

            rbHex.Checked = _findOptions.Type == FindType.Hex;

            if (hexFind.ByteProvider != null)
                hexFind.ByteProvider.Changed -= new EventHandler(ByteProvider_Changed);

            var hex = this._findOptions.Hex != null ? _findOptions.Hex : new byte[0];
            hexFind.ByteProvider = new DynamicByteProvider(hex);
            hexFind.ByteProvider.Changed += new EventHandler(ByteProvider_Changed);
        }

        void ByteProvider_Changed(object sender, EventArgs e)
        {
            ValidateFind();
        }

        private void rb_CheckedChanged(object sender, System.EventArgs e)
        {
            txtFind.Enabled = rbString.Checked;
            hexFind.Enabled = !txtFind.Enabled;

            if (txtFind.Enabled)
                txtFind.Focus();
            else
                hexFind.Focus();
        }

        private void rbString_Enter(object sender, EventArgs e)
        {
            txtFind.Focus();
        }

        private void rbHex_Enter(object sender, EventArgs e)
        {
            hexFind.Focus();
        }

        private void FormFind_Activated(object sender, System.EventArgs e)
        {
            if (rbString.Checked)
                txtFind.Focus();
            else
                hexFind.Focus();
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            _findOptions.MatchCase = chkMatchCase.Checked;

            var provider = this.hexFind.ByteProvider as DynamicByteProvider;
            _findOptions.Hex = provider.Bytes.ToArray();
            _findOptions.Text = txtFind.Text;
            _findOptions.Type = rbHex.Checked ? FindType.Hex : FindType.Text;
            _findOptions.MatchCase = chkMatchCase.Checked;
            _findOptions.IsValid = true;

            FindNext();
        }

        bool _finding;

        public void FindNext()
        {
            if (!_findOptions.IsValid)
                return;

            UpdateUIToFindingState();

            // start find process
            long res = HexBox.Find(_findOptions);

            UpdateUIToNormalState();

            Application.DoEvents();

            if (res == -1) // -1 = no match
            {
                MessageBox.Show("Find reached end of file!", "EOF",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (res == -2) // -2 = find was aborted
            {
                return;
            }
            else // something was found
            {
                this.Close();

                Application.DoEvents();

                if (!HexBox.Focused)
                    HexBox.Focus();
            }
        }

        private void UpdateUIToNormalState()
        {
            timer.Stop();
            timerPercent.Stop();
            _finding = false;
            txtFind.Enabled = chkMatchCase.Enabled = rbHex.Enabled = rbString.Enabled
                = hexFind.Enabled = btnOK.Enabled = true;
        }

        private void UpdateUIToFindingState()
        {
            _finding = true;
            timer.Start();
            timerPercent.Start();
            txtFind.Enabled = chkMatchCase.Enabled = rbHex.Enabled = rbString.Enabled
                = hexFind.Enabled = btnOK.Enabled = false;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            if (_finding)
                this.HexBox.AbortFind();
            else
                this.Close();
        }

        private void txtString_TextChanged(object sender, EventArgs e)
        {
            ValidateFind();
        }

        private void ValidateFind()
        {
            var isValid = false;
            if (rbString.Checked && txtFind.Text.Length > 0)
                isValid = true;
            if (rbHex.Checked && hexFind.ByteProvider.Length > 0)
                isValid = true;
            this.btnOK.Enabled = isValid;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (lblFinding.Text.Length == 13)
                lblFinding.Text = "";

            lblFinding.Text += ".";
        }

        private void timerPercent_Tick(object sender, EventArgs e)
        {
            long pos = this.HexBox.CurrentFindingPosition;
            long length = HexBox.ByteProvider.Length;
            double percent = pos / (double)length * 100;

            System.Globalization.NumberFormatInfo nfi =
                new System.Globalization.CultureInfo("en-US").NumberFormat;

            string text = percent.ToString("0.00", nfi) + " %";
            lblPercent.Text = text;
        }
    }
}
