namespace JRunner.Forms
{
    partial class Comport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Comport));
            this.cboPort = new System.Windows.Forms.ComboBox();
            this.cboBaud = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BaudRate = new System.Windows.Forms.Label();
            this.DataBits = new System.Windows.Forms.Label();
            this.Parity = new System.Windows.Forms.Label();
            this.StopBits = new System.Windows.Forms.Label();
            this.rbtnHex = new System.Windows.Forms.RadioButton();
            this.rbtnText = new System.Windows.Forms.RadioButton();
            this.cboParity = new System.Windows.Forms.ComboBox();
            this.cboData = new System.Windows.Forms.ComboBox();
            this.cboStop = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cmdRefresh = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closePortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedChk = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboPort
            // 
            this.cboPort.FormattingEnabled = true;
            this.cboPort.Location = new System.Drawing.Point(42, 26);
            this.cboPort.Name = "cboPort";
            this.cboPort.Size = new System.Drawing.Size(80, 21);
            this.cboPort.TabIndex = 2;
            this.cboPort.SelectedIndexChanged += new System.EventHandler(this.cboPort_SelectedIndexChanged);
            // 
            // cboBaud
            // 
            this.cboBaud.FormattingEnabled = true;
            this.cboBaud.Items.AddRange(new object[] {
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "28800",
            "38400",
            "57600",
            "115000"});
            this.cboBaud.Location = new System.Drawing.Point(245, 26);
            this.cboBaud.Name = "cboBaud";
            this.cboBaud.Size = new System.Drawing.Size(76, 21);
            this.cboBaud.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Port:";
            // 
            // BaudRate
            // 
            this.BaudRate.AutoSize = true;
            this.BaudRate.Location = new System.Drawing.Point(183, 29);
            this.BaudRate.Name = "BaudRate";
            this.BaudRate.Size = new System.Drawing.Size(61, 13);
            this.BaudRate.TabIndex = 5;
            this.BaudRate.Text = "Baud Rate:";
            // 
            // DataBits
            // 
            this.DataBits.AutoSize = true;
            this.DataBits.Location = new System.Drawing.Point(327, 29);
            this.DataBits.Name = "DataBits";
            this.DataBits.Size = new System.Drawing.Size(53, 13);
            this.DataBits.TabIndex = 7;
            this.DataBits.Text = "Data Bits:";
            // 
            // Parity
            // 
            this.Parity.AutoSize = true;
            this.Parity.Location = new System.Drawing.Point(577, 29);
            this.Parity.Name = "Parity";
            this.Parity.Size = new System.Drawing.Size(36, 13);
            this.Parity.TabIndex = 8;
            this.Parity.Text = "Parity:";
            // 
            // StopBits
            // 
            this.StopBits.AutoSize = true;
            this.StopBits.Location = new System.Drawing.Point(463, 29);
            this.StopBits.Name = "StopBits";
            this.StopBits.Size = new System.Drawing.Size(52, 13);
            this.StopBits.TabIndex = 11;
            this.StopBits.Text = "Stop Bits:";
            // 
            // rbtnHex
            // 
            this.rbtnHex.AutoSize = true;
            this.rbtnHex.Location = new System.Drawing.Point(316, 3);
            this.rbtnHex.Name = "rbtnHex";
            this.rbtnHex.Size = new System.Drawing.Size(44, 17);
            this.rbtnHex.TabIndex = 12;
            this.rbtnHex.Text = "Hex";
            this.rbtnHex.UseVisualStyleBackColor = true;
            // 
            // rbtnText
            // 
            this.rbtnText.AutoSize = true;
            this.rbtnText.Checked = true;
            this.rbtnText.Location = new System.Drawing.Point(267, 3);
            this.rbtnText.Name = "rbtnText";
            this.rbtnText.Size = new System.Drawing.Size(46, 17);
            this.rbtnText.TabIndex = 13;
            this.rbtnText.TabStop = true;
            this.rbtnText.Text = "Text";
            this.rbtnText.UseVisualStyleBackColor = true;
            // 
            // cboParity
            // 
            this.cboParity.FormattingEnabled = true;
            this.cboParity.Location = new System.Drawing.Point(614, 26);
            this.cboParity.Name = "cboParity";
            this.cboParity.Size = new System.Drawing.Size(55, 21);
            this.cboParity.TabIndex = 14;
            // 
            // cboData
            // 
            this.cboData.FormattingEnabled = true;
            this.cboData.Items.AddRange(new object[] {
            "7",
            "8",
            "9"});
            this.cboData.Location = new System.Drawing.Point(381, 26);
            this.cboData.Name = "cboData";
            this.cboData.Size = new System.Drawing.Size(76, 21);
            this.cboData.TabIndex = 15;
            // 
            // cboStop
            // 
            this.cboStop.FormattingEnabled = true;
            this.cboStop.Location = new System.Drawing.Point(516, 26);
            this.cboStop.Name = "cboStop";
            this.cboStop.Size = new System.Drawing.Size(55, 21);
            this.cboStop.TabIndex = 16;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.Black;
            this.textBox1.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(15, 53);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(654, 401);
            this.textBox1.TabIndex = 17;
            // 
            // cmdRefresh
            // 
            this.cmdRefresh.Location = new System.Drawing.Point(125, 25);
            this.cmdRefresh.Name = "cmdRefresh";
            this.cmdRefresh.Size = new System.Drawing.Size(52, 23);
            this.cmdRefresh.TabIndex = 19;
            this.cmdRefresh.Text = "Refresh";
            this.cmdRefresh.UseVisualStyleBackColor = true;
            this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.openPortToolStripMenuItem,
            this.closePortToolStripMenuItem,
            this.clearLogToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(681, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.exportToolStripMenuItem.Text = "Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // openPortToolStripMenuItem
            // 
            this.openPortToolStripMenuItem.Name = "openPortToolStripMenuItem";
            this.openPortToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.openPortToolStripMenuItem.Text = "Open Port";
            this.openPortToolStripMenuItem.Click += new System.EventHandler(this.openPortToolStripMenuItem_Click);
            // 
            // closePortToolStripMenuItem
            // 
            this.closePortToolStripMenuItem.Name = "closePortToolStripMenuItem";
            this.closePortToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.closePortToolStripMenuItem.Text = "Close Port";
            this.closePortToolStripMenuItem.Click += new System.EventHandler(this.closePortToolStripMenuItem_Click);
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.clearLogToolStripMenuItem.Text = "Clear Log";
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // advancedChk
            // 
            this.advancedChk.AutoSize = true;
            this.advancedChk.Location = new System.Drawing.Point(373, 4);
            this.advancedChk.Name = "advancedChk";
            this.advancedChk.Size = new System.Drawing.Size(144, 17);
            this.advancedChk.TabIndex = 21;
            this.advancedChk.Text = "Show Advanced Options";
            this.advancedChk.UseVisualStyleBackColor = true;
            this.advancedChk.CheckedChanged += new System.EventHandler(this.advancedChk_CheckedChanged);
            // 
            // Comport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 464);
            this.Controls.Add(this.advancedChk);
            this.Controls.Add(this.cmdRefresh);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cboStop);
            this.Controls.Add(this.cboData);
            this.Controls.Add(this.cboParity);
            this.Controls.Add(this.rbtnText);
            this.Controls.Add(this.rbtnHex);
            this.Controls.Add(this.StopBits);
            this.Controls.Add(this.Parity);
            this.Controls.Add(this.DataBits);
            this.Controls.Add(this.BaudRate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboBaud);
            this.Controls.Add(this.cboPort);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(697, 500);
            this.Name = "Comport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "COM Port Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Comport_FormClosing);
            this.Load += new System.EventHandler(this.Comport_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Comport_KeyUp);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cboPort;
        private System.Windows.Forms.ComboBox cboBaud;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label BaudRate;
        private System.Windows.Forms.Label DataBits;
        private System.Windows.Forms.Label Parity;
        private System.Windows.Forms.Label StopBits;
        private System.Windows.Forms.RadioButton rbtnHex;
        private System.Windows.Forms.RadioButton rbtnText;
        private System.Windows.Forms.ComboBox cboParity;
        private System.Windows.Forms.ComboBox cboData;
        private System.Windows.Forms.ComboBox cboStop;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button cmdRefresh;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openPortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closePortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.CheckBox advancedChk;
    }
}