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
            this.btnsuccom = new System.Windows.Forms.Button();
            this.txtsuccom = new System.Windows.Forms.TextBox();
            this.txtsuccess = new System.Windows.Forms.TextBox();
            this.btnsuccess = new System.Windows.Forms.Button();
            this.txterror = new System.Windows.Forms.TextBox();
            this.btnerror = new System.Windows.Forms.Button();
            this.btnfolder = new System.Windows.Forms.Button();
            this.txtfolder = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chksuccom = new System.Windows.Forms.CheckBox();
            this.chksuccess = new System.Windows.Forms.CheckBox();
            this.chkerror = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtNandflash = new System.Windows.Forms.TextBox();
            this.txtIPStart = new System.Windows.Forms.TextBox();
            this.txtIPEnd = new System.Windows.Forms.TextBox();
            this.chkfiles = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkIpDefault = new System.Windows.Forms.CheckBox();
            this.AutoExtractcheckBox = new System.Windows.Forms.CheckBox();
            this.almovebut = new System.Windows.Forms.CheckBox();
            this.modderbut = new System.Windows.Forms.CheckBox();
            this.discordRPCEnable = new System.Windows.Forms.CheckBox();
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
            this.minimizeToSystemTray = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.SlimPreferSrgh = new System.Windows.Forms.RadioButton();
            this.SlimPreferRgh12 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnsuccom
            // 
            this.btnsuccom.Location = new System.Drawing.Point(331, 31);
            this.btnsuccom.Name = "btnsuccom";
            this.btnsuccom.Size = new System.Drawing.Size(29, 23);
            this.btnsuccom.TabIndex = 0;
            this.btnsuccom.Text = "...";
            this.btnsuccom.UseVisualStyleBackColor = true;
            this.btnsuccom.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtsuccom
            // 
            this.txtsuccom.Location = new System.Drawing.Point(36, 32);
            this.txtsuccom.Name = "txtsuccom";
            this.txtsuccom.Size = new System.Drawing.Size(289, 20);
            this.txtsuccom.TabIndex = 2;
            // 
            // txtsuccess
            // 
            this.txtsuccess.Location = new System.Drawing.Point(36, 71);
            this.txtsuccess.Name = "txtsuccess";
            this.txtsuccess.Size = new System.Drawing.Size(289, 20);
            this.txtsuccess.TabIndex = 4;
            // 
            // btnsuccess
            // 
            this.btnsuccess.Location = new System.Drawing.Point(331, 70);
            this.btnsuccess.Name = "btnsuccess";
            this.btnsuccess.Size = new System.Drawing.Size(29, 23);
            this.btnsuccess.TabIndex = 5;
            this.btnsuccess.Text = "...";
            this.btnsuccess.UseVisualStyleBackColor = true;
            this.btnsuccess.Click += new System.EventHandler(this.button2_Click);
            // 
            // txterror
            // 
            this.txterror.Location = new System.Drawing.Point(36, 110);
            this.txterror.Name = "txterror";
            this.txterror.Size = new System.Drawing.Size(289, 20);
            this.txterror.TabIndex = 7;
            // 
            // btnerror
            // 
            this.btnerror.Location = new System.Drawing.Point(331, 109);
            this.btnerror.Name = "btnerror";
            this.btnerror.Size = new System.Drawing.Size(29, 23);
            this.btnerror.TabIndex = 8;
            this.btnerror.Text = "...";
            this.btnerror.UseVisualStyleBackColor = true;
            this.btnerror.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnfolder
            // 
            this.btnfolder.Location = new System.Drawing.Point(334, 183);
            this.btnfolder.Name = "btnfolder";
            this.btnfolder.Size = new System.Drawing.Size(29, 23);
            this.btnfolder.TabIndex = 9;
            this.btnfolder.Text = "...";
            this.btnfolder.UseVisualStyleBackColor = true;
            this.btnfolder.Click += new System.EventHandler(this.button4_Click);
            // 
            // txtfolder
            // 
            this.txtfolder.Location = new System.Drawing.Point(18, 184);
            this.txtfolder.Name = "txtfolder";
            this.txtfolder.Size = new System.Drawing.Size(310, 20);
            this.txtfolder.TabIndex = 10;
            // 
            // txtIP
            // 
            this.txtIP.Enabled = false;
            this.txtIP.Location = new System.Drawing.Point(99, 24);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(92, 20);
            this.txtIP.TabIndex = 11;
            this.toolTip1.SetToolTip(this.txtIP, "Set a default IP for getting CPUKey\r\nover a network.");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Success on Compare:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Success:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Error/Failure:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 168);
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
            this.label5.TabIndex = 16;
            this.label5.Text = "IP Default:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(268, 415);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 23);
            this.btnOK.TabIndex = 17;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtsuccom);
            this.groupBox1.Controls.Add(this.btnsuccom);
            this.groupBox1.Controls.Add(this.chksuccom);
            this.groupBox1.Controls.Add(this.chksuccess);
            this.groupBox1.Controls.Add(this.txtsuccess);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnsuccess);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chkerror);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txterror);
            this.groupBox1.Controls.Add(this.btnerror);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(375, 149);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sounds";
            // 
            // chksuccom
            // 
            this.chksuccom.AutoSize = true;
            this.chksuccom.Location = new System.Drawing.Point(15, 35);
            this.chksuccom.Name = "chksuccom";
            this.chksuccom.Size = new System.Drawing.Size(15, 14);
            this.chksuccom.TabIndex = 1;
            this.chksuccom.UseVisualStyleBackColor = true;
            // 
            // chksuccess
            // 
            this.chksuccess.AutoSize = true;
            this.chksuccess.Location = new System.Drawing.Point(15, 74);
            this.chksuccess.Name = "chksuccess";
            this.chksuccess.Size = new System.Drawing.Size(15, 14);
            this.chksuccess.TabIndex = 3;
            this.chksuccess.UseVisualStyleBackColor = true;
            // 
            // chkerror
            // 
            this.chkerror.AutoSize = true;
            this.chkerror.Location = new System.Drawing.Point(15, 113);
            this.chkerror.Name = "chkerror";
            this.chkerror.Size = new System.Drawing.Size(15, 14);
            this.chkerror.TabIndex = 6;
            this.chkerror.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(413, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Sonus360 Write Delay:";
            this.toolTip1.SetToolTip(this.label6, "If your nand write with JR-P appears to lag (should be similar speed to read) sel" +
        "ect this!");
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(531, 146);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(64, 20);
            this.numericUpDown1.TabIndex = 22;
            this.toolTip1.SetToolTip(this.numericUpDown1, "If your nand write with JR-P appears to lag (should be similar speed to read) sel" +
        "ect this!");
            // 
            // txtNandflash
            // 
            this.txtNandflash.Location = new System.Drawing.Point(492, 179);
            this.txtNandflash.Name = "txtNandflash";
            this.txtNandflash.Size = new System.Drawing.Size(103, 20);
            this.txtNandflash.TabIndex = 24;
            this.toolTip1.SetToolTip(this.txtNandflash, "determines the name given to the file\r\nproduced upon creation of a Nand\r\nImage");
            // 
            // txtIPStart
            // 
            this.txtIPStart.Location = new System.Drawing.Point(99, 55);
            this.txtIPStart.Name = "txtIPStart";
            this.txtIPStart.Size = new System.Drawing.Size(92, 20);
            this.txtIPStart.TabIndex = 28;
            this.toolTip1.SetToolTip(this.txtIPStart, "Set the starting IP range for\r\nScan IP range function");
            // 
            // txtIPEnd
            // 
            this.txtIPEnd.Location = new System.Drawing.Point(99, 87);
            this.txtIPEnd.Name = "txtIPEnd";
            this.txtIPEnd.Size = new System.Drawing.Size(92, 20);
            this.txtIPEnd.TabIndex = 29;
            this.toolTip1.SetToolTip(this.txtIPEnd, "Set the end of IP range for\r\nScan IP range function");
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(382, 182);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "XeBuild Image Name:";
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
            this.groupBox2.Location = new System.Drawing.Point(393, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(202, 116);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "IP Settings";
            // 
            // chkIpDefault
            // 
            this.chkIpDefault.AutoSize = true;
            this.chkIpDefault.Location = new System.Drawing.Point(78, 27);
            this.chkIpDefault.Name = "chkIpDefault";
            this.chkIpDefault.Size = new System.Drawing.Size(15, 14);
            this.chkIpDefault.TabIndex = 30;
            this.chkIpDefault.UseVisualStyleBackColor = true;
            this.chkIpDefault.CheckedChanged += new System.EventHandler(this.chkIpDefault_CheckedChanged);
            // 
            // AutoExtractcheckBox
            // 
            this.AutoExtractcheckBox.AutoSize = true;
            this.AutoExtractcheckBox.Location = new System.Drawing.Point(18, 238);
            this.AutoExtractcheckBox.Name = "AutoExtractcheckBox";
            this.AutoExtractcheckBox.Size = new System.Drawing.Size(165, 17);
            this.AutoExtractcheckBox.TabIndex = 33;
            this.AutoExtractcheckBox.Text = "Auto Extract Files from Nands";
            this.AutoExtractcheckBox.UseVisualStyleBackColor = true;
            // 
            // almovebut
            // 
            this.almovebut.AutoSize = true;
            this.almovebut.Checked = true;
            this.almovebut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.almovebut.Location = new System.Drawing.Point(18, 261);
            this.almovebut.Name = "almovebut";
            this.almovebut.Size = new System.Drawing.Size(244, 17);
            this.almovebut.TabIndex = 34;
            this.almovebut.Text = "Only move nand/files upon first CPU Key entry";
            this.almovebut.UseVisualStyleBackColor = true;
            this.almovebut.CheckedChanged += new System.EventHandler(this.almovebut_CheckedChanged);
            // 
            // modderbut
            // 
            this.modderbut.AutoSize = true;
            this.modderbut.Location = new System.Drawing.Point(18, 284);
            this.modderbut.Name = "modderbut";
            this.modderbut.Size = new System.Drawing.Size(280, 17);
            this.modderbut.TabIndex = 35;
            this.modderbut.Text = "Use unique name instead of Console type in database";
            this.modderbut.UseVisualStyleBackColor = true;
            this.modderbut.CheckedChanged += new System.EventHandler(this.modderbut_CheckedChanged);
            // 
            // discordRPCEnable
            // 
            this.discordRPCEnable.AutoSize = true;
            this.discordRPCEnable.Checked = true;
            this.discordRPCEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.discordRPCEnable.Location = new System.Drawing.Point(18, 215);
            this.discordRPCEnable.Name = "discordRPCEnable";
            this.discordRPCEnable.Size = new System.Drawing.Size(120, 17);
            this.discordRPCEnable.TabIndex = 36;
            this.discordRPCEnable.Text = "Enable DiscordRPC";
            this.discordRPCEnable.UseVisualStyleBackColor = true;
            this.discordRPCEnable.CheckedChanged += new System.EventHandler(this.discordRPCEnable_CheckedChanged);
            this.discordRPCEnable.Click += new System.EventHandler(this.discordRPCEnable_Click);
            // 
            // timingOnKeypressEnable
            // 
            this.timingOnKeypressEnable.AutoSize = true;
            this.timingOnKeypressEnable.Location = new System.Drawing.Point(18, 307);
            this.timingOnKeypressEnable.Name = "timingOnKeypressEnable";
            this.timingOnKeypressEnable.Size = new System.Drawing.Size(306, 17);
            this.timingOnKeypressEnable.TabIndex = 37;
            this.timingOnKeypressEnable.Text = "F12 key programs timings when Program Timing File is open";
            this.timingOnKeypressEnable.UseVisualStyleBackColor = true;
            this.timingOnKeypressEnable.CheckedChanged += new System.EventHandler(this.timingOnKeypressEnable_CheckedChanged);
            // 
            // logBackgroundCustom
            // 
            this.logBackgroundCustom.Location = new System.Drawing.Point(241, 353);
            this.logBackgroundCustom.Name = "logBackgroundCustom";
            this.logBackgroundCustom.Size = new System.Drawing.Size(50, 20);
            this.logBackgroundCustom.TabIndex = 41;
            this.logBackgroundCustom.Text = "Custom";
            this.logBackgroundCustom.UseVisualStyleBackColor = true;
            this.logBackgroundCustom.Click += new System.EventHandler(this.logBackgroundCustom_Click);
            // 
            // logBackgroundBlue
            // 
            this.logBackgroundBlue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(36)))), ((int)(((byte)(86)))));
            this.logBackgroundBlue.Location = new System.Drawing.Point(215, 353);
            this.logBackgroundBlue.Name = "logBackgroundBlue";
            this.logBackgroundBlue.Size = new System.Drawing.Size(20, 20);
            this.logBackgroundBlue.TabIndex = 40;
            this.logBackgroundBlue.UseVisualStyleBackColor = false;
            this.logBackgroundBlue.Click += new System.EventHandler(this.logBackgroundBlue_Click);
            // 
            // logBackgroundBlack
            // 
            this.logBackgroundBlack.BackColor = System.Drawing.Color.Black;
            this.logBackgroundBlack.Location = new System.Drawing.Point(189, 353);
            this.logBackgroundBlack.Name = "logBackgroundBlack";
            this.logBackgroundBlack.Size = new System.Drawing.Size(20, 20);
            this.logBackgroundBlack.TabIndex = 39;
            this.logBackgroundBlack.UseVisualStyleBackColor = false;
            this.logBackgroundBlack.Click += new System.EventHandler(this.logBackgroundBlack_Click);
            // 
            // logTextCustom
            // 
            this.logTextCustom.Location = new System.Drawing.Point(241, 379);
            this.logTextCustom.Name = "logTextCustom";
            this.logTextCustom.Size = new System.Drawing.Size(50, 20);
            this.logTextCustom.TabIndex = 45;
            this.logTextCustom.Text = "Custom";
            this.logTextCustom.UseVisualStyleBackColor = true;
            this.logTextCustom.Click += new System.EventHandler(this.logTextCustom_Click);
            // 
            // logTextBlack
            // 
            this.logTextBlack.BackColor = System.Drawing.Color.Black;
            this.logTextBlack.Location = new System.Drawing.Point(215, 379);
            this.logTextBlack.Name = "logTextBlack";
            this.logTextBlack.Size = new System.Drawing.Size(20, 20);
            this.logTextBlack.TabIndex = 44;
            this.logTextBlack.UseVisualStyleBackColor = false;
            this.logTextBlack.Click += new System.EventHandler(this.logTextBlack_Click);
            // 
            // logTextWhite
            // 
            this.logTextWhite.BackColor = System.Drawing.Color.White;
            this.logTextWhite.Location = new System.Drawing.Point(189, 379);
            this.logTextWhite.Name = "logTextWhite";
            this.logTextWhite.Size = new System.Drawing.Size(20, 20);
            this.logTextWhite.TabIndex = 43;
            this.logTextWhite.UseVisualStyleBackColor = false;
            this.logTextWhite.Click += new System.EventHandler(this.logTextWhite_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 382);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 13);
            this.label11.TabIndex = 42;
            this.label11.Text = "Log Text Color: ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 356);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Log Background Color:";
            // 
            // logDefault
            // 
            this.logDefault.Location = new System.Drawing.Point(133, 353);
            this.logDefault.Name = "logDefault";
            this.logDefault.Size = new System.Drawing.Size(50, 46);
            this.logDefault.TabIndex = 46;
            this.logDefault.Text = "Default";
            this.logDefault.UseVisualStyleBackColor = true;
            this.logDefault.Click += new System.EventHandler(this.logDefault_Click);
            // 
            // minimizeToSystemTray
            // 
            this.minimizeToSystemTray.AutoSize = true;
            this.minimizeToSystemTray.Location = new System.Drawing.Point(18, 330);
            this.minimizeToSystemTray.Name = "minimizeToSystemTray";
            this.minimizeToSystemTray.Size = new System.Drawing.Size(133, 17);
            this.minimizeToSystemTray.TabIndex = 48;
            this.minimizeToSystemTray.Text = "Minimize to system tray";
            this.minimizeToSystemTray.UseVisualStyleBackColor = true;
            this.minimizeToSystemTray.CheckedChanged += new System.EventHandler(this.minimizeToSystemTray_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SlimPreferSrgh);
            this.groupBox3.Controls.Add(this.SlimPreferRgh12);
            this.groupBox3.Location = new System.Drawing.Point(421, 215);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(174, 45);
            this.groupBox3.TabIndex = 50;
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
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 455);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.minimizeToSystemTray);
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
            this.Controls.Add(this.discordRPCEnable);
            this.Controls.Add(this.modderbut);
            this.Controls.Add(this.almovebut);
            this.Controls.Add(this.AutoExtractcheckBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtNandflash);
            this.Controls.Add(this.chkfiles);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtfolder);
            this.Controls.Add(this.btnfolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Settings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Settings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnsuccom;
        private System.Windows.Forms.TextBox txtsuccom;
        private System.Windows.Forms.TextBox txtsuccess;
        private System.Windows.Forms.Button btnsuccess;
        private System.Windows.Forms.TextBox txterror;
        private System.Windows.Forms.Button btnerror;
        private System.Windows.Forms.TextBox txtfolder;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnfolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkfiles;
        private System.Windows.Forms.TextBox txtNandflash;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtIPStart;
        private System.Windows.Forms.TextBox txtIPEnd;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox AutoExtractcheckBox;
        private System.Windows.Forms.CheckBox almovebut;
        private System.Windows.Forms.CheckBox modderbut;
        private System.Windows.Forms.CheckBox discordRPCEnable;
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
        private System.Windows.Forms.CheckBox chksuccom;
        private System.Windows.Forms.CheckBox chksuccess;
        private System.Windows.Forms.CheckBox chkerror;
        private System.Windows.Forms.CheckBox minimizeToSystemTray;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton SlimPreferSrgh;
        private System.Windows.Forms.RadioButton SlimPreferRgh12;
        private System.Windows.Forms.CheckBox chkIpDefault;
    }
}