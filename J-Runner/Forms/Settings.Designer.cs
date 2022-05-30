namespace JRunner.Forms
{
    partial class Settings
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.btnFolder = new System.Windows.Forms.Button();
            this.txtfolder = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.sonusDelay = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtIPStart = new System.Windows.Forms.TextBox();
            this.txtIPEnd = new System.Windows.Forms.TextBox();
            this.txtTimingLptPort = new System.Windows.Forms.TextBox();
            this.chkfiles = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkIpDefault = new System.Windows.Forms.CheckBox();
            this.AutoExtractcheckBox = new System.Windows.Forms.CheckBox();
            this.almovebut = new System.Windows.Forms.CheckBox();
            this.modderbut = new System.Windows.Forms.CheckBox();
            this.timingOnKeypressEnable = new System.Windows.Forms.CheckBox();
            this.logBackgroundCustom = new System.Windows.Forms.Button();
            this.logBackgroundBlue = new System.Windows.Forms.Button();
            this.logBackgroundBlack = new System.Windows.Forms.Button();
            this.logTextCustom = new System.Windows.Forms.Button();
            this.logTextBlack = new System.Windows.Forms.Button();
            this.logTextWhite = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.logDefault = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.SlimPreferSrgh = new System.Windows.Forms.RadioButton();
            this.SlimPreferRgh12 = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbtnTimingLpt = new System.Windows.Forms.RadioButton();
            this.rbtnTimingUsb = new System.Windows.Forms.RadioButton();
            this.btnReset = new System.Windows.Forms.Button();
            this.chkNoPatchWarnings = new System.Windows.Forms.CheckBox();
            this.chkPlaySuccess = new System.Windows.Forms.CheckBox();
            this.chkPlayError = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.sonusDelay)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFolder
            // 
            this.btnFolder.Location = new System.Drawing.Point(340, 27);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(29, 22);
            this.btnFolder.TabIndex = 22;
            this.btnFolder.Text = "...";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // txtfolder
            // 
            this.txtfolder.Location = new System.Drawing.Point(15, 28);
            this.txtfolder.Name = "txtfolder";
            this.txtfolder.Size = new System.Drawing.Size(319, 20);
            this.txtfolder.TabIndex = 21;
            // 
            // txtIP
            // 
            this.txtIP.Enabled = false;
            this.txtIP.Location = new System.Drawing.Point(99, 24);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(92, 20);
            this.txtIP.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtIP, "Set a default IP for getting CPUKey\r\nover a network.");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Output Folder:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "IP Default:";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(268, 288);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(412, 242);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Sonus360 Write Delay:";
            this.toolTip1.SetToolTip(this.label6, "If your nand write with JR-P appears to lag (should be similar speed to read) sel" +
        "ect this!");
            // 
            // sonusDelay
            // 
            this.sonusDelay.Location = new System.Drawing.Point(530, 240);
            this.sonusDelay.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.sonusDelay.Name = "sonusDelay";
            this.sonusDelay.Size = new System.Drawing.Size(64, 20);
            this.sonusDelay.TabIndex = 36;
            this.toolTip1.SetToolTip(this.sonusDelay, "If your nand write with JR-P appears to lag (should be similar speed to read) sel" +
        "ect this!");
            // 
            // txtIPStart
            // 
            this.txtIPStart.Location = new System.Drawing.Point(99, 55);
            this.txtIPStart.Name = "txtIPStart";
            this.txtIPStart.Size = new System.Drawing.Size(92, 20);
            this.txtIPStart.TabIndex = 4;
            this.toolTip1.SetToolTip(this.txtIPStart, "Set the starting IP range for\r\nScan IP range function");
            // 
            // txtIPEnd
            // 
            this.txtIPEnd.Location = new System.Drawing.Point(99, 87);
            this.txtIPEnd.Name = "txtIPEnd";
            this.txtIPEnd.Size = new System.Drawing.Size(92, 20);
            this.txtIPEnd.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtIPEnd, "Set the end of IP range for\r\nScan IP range function");
            // 
            // txtTimingLptPort
            // 
            this.txtTimingLptPort.Enabled = false;
            this.txtTimingLptPort.Location = new System.Drawing.Point(108, 17);
            this.txtTimingLptPort.MaxLength = 5;
            this.txtTimingLptPort.Name = "txtTimingLptPort";
            this.txtTimingLptPort.Size = new System.Drawing.Size(57, 20);
            this.txtTimingLptPort.TabIndex = 52;
            this.toolTip1.SetToolTip(this.txtTimingLptPort, "determines the name given to the file\r\nproduced upon creation of a Nand\r\nImage");
            this.txtTimingLptPort.TextChanged += new System.EventHandler(this.txtTimingLptPort_TextChanged);
            // 
            // chkfiles
            // 
            this.chkfiles.AutoSize = true;
            this.chkfiles.Enabled = false;
            this.chkfiles.Location = new System.Drawing.Point(624, 144);
            this.chkfiles.Name = "chkfiles";
            this.chkfiles.Size = new System.Drawing.Size(104, 17);
            this.chkfiles.TabIndex = 23;
            this.chkfiles.Text = "Delete extra files";
            this.chkfiles.UseVisualStyleBackColor = true;
            this.chkfiles.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "IP Range Start:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 90);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "IP Range Finish:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkIpDefault);
            this.groupBox2.Controls.Add(this.txtIPStart);
            this.groupBox2.Controls.Add(this.txtIP);
            this.groupBox2.Controls.Add(this.txtIPEnd);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(392, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(202, 118);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "IP Settings";
            // 
            // chkIpDefault
            // 
            this.chkIpDefault.AutoSize = true;
            this.chkIpDefault.Location = new System.Drawing.Point(78, 27);
            this.chkIpDefault.Name = "chkIpDefault";
            this.chkIpDefault.Size = new System.Drawing.Size(15, 14);
            this.chkIpDefault.TabIndex = 2;
            this.chkIpDefault.UseVisualStyleBackColor = true;
            this.chkIpDefault.CheckedChanged += new System.EventHandler(this.chkIpDefault_CheckedChanged);
            // 
            // AutoExtractcheckBox
            // 
            this.AutoExtractcheckBox.AutoSize = true;
            this.AutoExtractcheckBox.Location = new System.Drawing.Point(15, 59);
            this.AutoExtractcheckBox.Name = "AutoExtractcheckBox";
            this.AutoExtractcheckBox.Size = new System.Drawing.Size(165, 17);
            this.AutoExtractcheckBox.TabIndex = 23;
            this.AutoExtractcheckBox.Text = "Auto Extract Files from Nands";
            this.AutoExtractcheckBox.UseVisualStyleBackColor = true;
            // 
            // almovebut
            // 
            this.almovebut.AutoSize = true;
            this.almovebut.Checked = true;
            this.almovebut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.almovebut.Location = new System.Drawing.Point(15, 82);
            this.almovebut.Name = "almovebut";
            this.almovebut.Size = new System.Drawing.Size(244, 17);
            this.almovebut.TabIndex = 24;
            this.almovebut.Text = "Only move nand/files upon first CPU Key entry";
            this.almovebut.UseVisualStyleBackColor = true;
            this.almovebut.CheckedChanged += new System.EventHandler(this.almovebut_CheckedChanged);
            // 
            // modderbut
            // 
            this.modderbut.AutoSize = true;
            this.modderbut.Location = new System.Drawing.Point(15, 105);
            this.modderbut.Name = "modderbut";
            this.modderbut.Size = new System.Drawing.Size(279, 17);
            this.modderbut.TabIndex = 25;
            this.modderbut.Text = "Use unique name instead of console type in database";
            this.modderbut.UseVisualStyleBackColor = true;
            this.modderbut.CheckedChanged += new System.EventHandler(this.modderbut_CheckedChanged);
            // 
            // timingOnKeypressEnable
            // 
            this.timingOnKeypressEnable.AutoSize = true;
            this.timingOnKeypressEnable.Location = new System.Drawing.Point(15, 128);
            this.timingOnKeypressEnable.Name = "timingOnKeypressEnable";
            this.timingOnKeypressEnable.Size = new System.Drawing.Size(306, 17);
            this.timingOnKeypressEnable.TabIndex = 26;
            this.timingOnKeypressEnable.Text = "F12 key programs timings when Program Timing File is open";
            this.timingOnKeypressEnable.UseVisualStyleBackColor = true;
            this.timingOnKeypressEnable.CheckedChanged += new System.EventHandler(this.timingOnKeypressEnable_CheckedChanged);
            // 
            // logBackgroundCustom
            // 
            this.logBackgroundCustom.Location = new System.Drawing.Point(242, 222);
            this.logBackgroundCustom.Name = "logBackgroundCustom";
            this.logBackgroundCustom.Size = new System.Drawing.Size(50, 20);
            this.logBackgroundCustom.TabIndex = 31;
            this.logBackgroundCustom.TabStop = false;
            this.logBackgroundCustom.Text = "Custom";
            this.logBackgroundCustom.UseVisualStyleBackColor = true;
            this.logBackgroundCustom.Click += new System.EventHandler(this.logBackgroundCustom_Click);
            // 
            // logBackgroundBlue
            // 
            this.logBackgroundBlue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(36)))), ((int)(((byte)(86)))));
            this.logBackgroundBlue.Location = new System.Drawing.Point(216, 222);
            this.logBackgroundBlue.Name = "logBackgroundBlue";
            this.logBackgroundBlue.Size = new System.Drawing.Size(20, 20);
            this.logBackgroundBlue.TabIndex = 30;
            this.logBackgroundBlue.TabStop = false;
            this.logBackgroundBlue.UseVisualStyleBackColor = false;
            this.logBackgroundBlue.Click += new System.EventHandler(this.logBackgroundBlue_Click);
            // 
            // logBackgroundBlack
            // 
            this.logBackgroundBlack.BackColor = System.Drawing.Color.Black;
            this.logBackgroundBlack.Location = new System.Drawing.Point(190, 222);
            this.logBackgroundBlack.Name = "logBackgroundBlack";
            this.logBackgroundBlack.Size = new System.Drawing.Size(20, 20);
            this.logBackgroundBlack.TabIndex = 29;
            this.logBackgroundBlack.TabStop = false;
            this.logBackgroundBlack.UseVisualStyleBackColor = false;
            this.logBackgroundBlack.Click += new System.EventHandler(this.logBackgroundBlack_Click);
            // 
            // logTextCustom
            // 
            this.logTextCustom.Location = new System.Drawing.Point(242, 248);
            this.logTextCustom.Name = "logTextCustom";
            this.logTextCustom.Size = new System.Drawing.Size(50, 20);
            this.logTextCustom.TabIndex = 34;
            this.logTextCustom.TabStop = false;
            this.logTextCustom.Text = "Custom";
            this.logTextCustom.UseVisualStyleBackColor = true;
            this.logTextCustom.Click += new System.EventHandler(this.logTextCustom_Click);
            // 
            // logTextBlack
            // 
            this.logTextBlack.BackColor = System.Drawing.Color.Black;
            this.logTextBlack.Location = new System.Drawing.Point(216, 248);
            this.logTextBlack.Name = "logTextBlack";
            this.logTextBlack.Size = new System.Drawing.Size(20, 20);
            this.logTextBlack.TabIndex = 33;
            this.logTextBlack.TabStop = false;
            this.logTextBlack.UseVisualStyleBackColor = false;
            this.logTextBlack.Click += new System.EventHandler(this.logTextBlack_Click);
            // 
            // logTextWhite
            // 
            this.logTextWhite.BackColor = System.Drawing.Color.White;
            this.logTextWhite.Location = new System.Drawing.Point(190, 248);
            this.logTextWhite.Name = "logTextWhite";
            this.logTextWhite.Size = new System.Drawing.Size(20, 20);
            this.logTextWhite.TabIndex = 32;
            this.logTextWhite.TabStop = false;
            this.logTextWhite.UseVisualStyleBackColor = false;
            this.logTextWhite.Click += new System.EventHandler(this.logTextWhite_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 251);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 13);
            this.label11.TabIndex = 42;
            this.label11.Text = "Log Text Color: ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 225);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Log Background Color:";
            // 
            // logDefault
            // 
            this.logDefault.Location = new System.Drawing.Point(134, 222);
            this.logDefault.Name = "logDefault";
            this.logDefault.Size = new System.Drawing.Size(50, 46);
            this.logDefault.TabIndex = 28;
            this.logDefault.Text = "Default";
            this.logDefault.UseVisualStyleBackColor = true;
            this.logDefault.Click += new System.EventHandler(this.logDefault_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SlimPreferSrgh);
            this.groupBox3.Controls.Add(this.SlimPreferRgh12);
            this.groupBox3.Location = new System.Drawing.Point(420, 136);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(174, 45);
            this.groupBox3.TabIndex = 38;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Preferred Slim Method";
            // 
            // SlimPreferSrgh
            // 
            this.SlimPreferSrgh.AutoSize = true;
            this.SlimPreferSrgh.Location = new System.Drawing.Point(112, 18);
            this.SlimPreferSrgh.Name = "SlimPreferSrgh";
            this.SlimPreferSrgh.Size = new System.Drawing.Size(59, 17);
            this.SlimPreferSrgh.TabIndex = 1;
            this.SlimPreferSrgh.Text = "S-RGH";
            this.SlimPreferSrgh.UseVisualStyleBackColor = true;
            this.SlimPreferSrgh.CheckedChanged += new System.EventHandler(this.SlimPreferSrgh_CheckedChanged);
            // 
            // SlimPreferRgh12
            // 
            this.SlimPreferRgh12.AutoSize = true;
            this.SlimPreferRgh12.Checked = true;
            this.SlimPreferRgh12.Location = new System.Drawing.Point(9, 18);
            this.SlimPreferRgh12.Name = "SlimPreferRgh12";
            this.SlimPreferRgh12.Size = new System.Drawing.Size(94, 17);
            this.SlimPreferRgh12.TabIndex = 0;
            this.SlimPreferRgh12.TabStop = true;
            this.SlimPreferRgh12.Text = "RGH1.2 (Best)";
            this.SlimPreferRgh12.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtTimingLptPort);
            this.groupBox4.Controls.Add(this.rbtnTimingLpt);
            this.groupBox4.Controls.Add(this.rbtnTimingUsb);
            this.groupBox4.Location = new System.Drawing.Point(420, 187);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(174, 45);
            this.groupBox4.TabIndex = 39;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Timing File Programming";
            // 
            // rbtnTimingLpt
            // 
            this.rbtnTimingLpt.AutoSize = true;
            this.rbtnTimingLpt.Location = new System.Drawing.Point(63, 18);
            this.rbtnTimingLpt.Name = "rbtnTimingLpt";
            this.rbtnTimingLpt.Size = new System.Drawing.Size(45, 17);
            this.rbtnTimingLpt.TabIndex = 1;
            this.rbtnTimingLpt.Text = "LPT";
            this.rbtnTimingLpt.UseVisualStyleBackColor = true;
            this.rbtnTimingLpt.CheckedChanged += new System.EventHandler(this.timingRbtn_CheckedChanged);
            // 
            // rbtnTimingUsb
            // 
            this.rbtnTimingUsb.AutoSize = true;
            this.rbtnTimingUsb.Checked = true;
            this.rbtnTimingUsb.Location = new System.Drawing.Point(9, 18);
            this.rbtnTimingUsb.Name = "rbtnTimingUsb";
            this.rbtnTimingUsb.Size = new System.Drawing.Size(47, 17);
            this.rbtnTimingUsb.TabIndex = 0;
            this.rbtnTimingUsb.TabStop = true;
            this.rbtnTimingUsb.Text = "USB";
            this.rbtnTimingUsb.UseVisualStyleBackColor = true;
            this.rbtnTimingUsb.CheckedChanged += new System.EventHandler(this.timingRbtn_CheckedChanged);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.Location = new System.Drawing.Point(15, 288);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(76, 23);
            this.btnReset.TabIndex = 40;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // chkNoPatchWarnings
            // 
            this.chkNoPatchWarnings.AutoSize = true;
            this.chkNoPatchWarnings.Location = new System.Drawing.Point(15, 151);
            this.chkNoPatchWarnings.Name = "chkNoPatchWarnings";
            this.chkNoPatchWarnings.Size = new System.Drawing.Size(206, 17);
            this.chkNoPatchWarnings.TabIndex = 27;
            this.chkNoPatchWarnings.Text = "Do not show patch warning messages";
            this.chkNoPatchWarnings.UseVisualStyleBackColor = true;
            this.chkNoPatchWarnings.CheckedChanged += new System.EventHandler(this.chkNoPatchWarnings_CheckedChanged);
            this.chkNoPatchWarnings.Click += new System.EventHandler(this.chkNoPatchWarnings_Click);
            // 
            // chkPlaySuccess
            // 
            this.chkPlaySuccess.AutoSize = true;
            this.chkPlaySuccess.Checked = true;
            this.chkPlaySuccess.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPlaySuccess.Location = new System.Drawing.Point(15, 174);
            this.chkPlaySuccess.Name = "chkPlaySuccess";
            this.chkPlaySuccess.Size = new System.Drawing.Size(157, 17);
            this.chkPlaySuccess.TabIndex = 43;
            this.chkPlaySuccess.Text = "Play success sound (chime)";
            this.chkPlaySuccess.UseVisualStyleBackColor = true;
            // 
            // chkPlayError
            // 
            this.chkPlayError.AutoSize = true;
            this.chkPlayError.Checked = true;
            this.chkPlayError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPlayError.Location = new System.Drawing.Point(15, 197);
            this.chkPlayError.Name = "chkPlayError";
            this.chkPlayError.Size = new System.Drawing.Size(138, 17);
            this.chkPlayError.TabIndex = 44;
            this.chkPlayError.Text = "Play error sound (oh no)";
            this.chkPlayError.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 326);
            this.Controls.Add(this.chkPlayError);
            this.Controls.Add(this.chkPlaySuccess);
            this.Controls.Add(this.chkNoPatchWarnings);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.logDefault);
            this.Controls.Add(this.logTextCustom);
            this.Controls.Add(this.logTextBlack);
            this.Controls.Add(this.logTextWhite);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.logBackgroundCustom);
            this.Controls.Add(this.logBackgroundBlue);
            this.Controls.Add(this.logBackgroundBlack);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.timingOnKeypressEnable);
            this.Controls.Add(this.modderbut);
            this.Controls.Add(this.almovebut);
            this.Controls.Add(this.AutoExtractcheckBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chkfiles);
            this.Controls.Add(this.sonusDelay);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtfolder);
            this.Controls.Add(this.btnFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sonusDelay)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtfolder;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.FolderBrowserDialog folderDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown sonusDelay;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkfiles;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtIPStart;
        private System.Windows.Forms.TextBox txtIPEnd;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox AutoExtractcheckBox;
        private System.Windows.Forms.CheckBox almovebut;
        private System.Windows.Forms.CheckBox modderbut;
        private System.Windows.Forms.CheckBox timingOnKeypressEnable;
        private System.Windows.Forms.Button logBackgroundCustom;
        private System.Windows.Forms.Button logBackgroundBlue;
        private System.Windows.Forms.Button logBackgroundBlack;
        private System.Windows.Forms.Button logTextCustom;
        private System.Windows.Forms.Button logTextBlack;
        private System.Windows.Forms.Button logTextWhite;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button logDefault;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton SlimPreferSrgh;
        private System.Windows.Forms.RadioButton SlimPreferRgh12;
        private System.Windows.Forms.CheckBox chkIpDefault;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtTimingLptPort;
        private System.Windows.Forms.RadioButton rbtnTimingLpt;
        private System.Windows.Forms.RadioButton rbtnTimingUsb;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox chkNoPatchWarnings;
        private System.Windows.Forms.CheckBox chkPlaySuccess;
        private System.Windows.Forms.CheckBox chkPlayError;
    }
}