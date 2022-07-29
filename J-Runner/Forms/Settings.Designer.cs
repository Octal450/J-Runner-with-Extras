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
            this.btnRootOverride = new System.Windows.Forms.Button();
            this.txtRootOverride = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtTimingLptPort = new System.Windows.Forms.TextBox();
            this.chkRootOverride = new System.Windows.Forms.CheckBox();
            this.chkIPDefault = new System.Windows.Forms.CheckBox();
            this.chkfiles = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkAutoExtract = new System.Windows.Forms.CheckBox();
            this.chkAllMove = new System.Windows.Forms.CheckBox();
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
            this.btnDefaults = new System.Windows.Forms.Button();
            this.chkNoPatchWarnings = new System.Windows.Forms.CheckBox();
            this.chkPlaySuccess = new System.Windows.Forms.CheckBox();
            this.chkPlayError = new System.Windows.Forms.CheckBox();
            this.chkAutoDelXeLL = new System.Windows.Forms.CheckBox();
            this.tabCSettings = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.tabBackup = new System.Windows.Forms.TabPage();
            this.groupBackupNaming = new System.Windows.Forms.GroupBox();
            this.rbtnSnOnly = new System.Windows.Forms.RadioButton();
            this.rbtnCtypeSn = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBackupRoot = new System.Windows.Forms.Button();
            this.txtBackupRoot = new System.Windows.Forms.TextBox();
            this.chkBackupEn = new System.Windows.Forms.CheckBox();
            this.groupBackupType = new System.Windows.Forms.GroupBox();
            this.rbtnFolder = new System.Windows.Forms.RadioButton();
            this.rbtnZip = new System.Windows.Forms.RadioButton();
            this.rbtnCtypeSnDate = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabCSettings.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabBackup.SuspendLayout();
            this.groupBackupNaming.SuspendLayout();
            this.groupBackupType.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRootOverride
            // 
            this.btnRootOverride.Enabled = false;
            this.btnRootOverride.Location = new System.Drawing.Point(331, 24);
            this.btnRootOverride.Name = "btnRootOverride";
            this.btnRootOverride.Size = new System.Drawing.Size(29, 22);
            this.btnRootOverride.TabIndex = 22;
            this.btnRootOverride.Text = "...";
            this.btnRootOverride.UseVisualStyleBackColor = true;
            this.btnRootOverride.Click += new System.EventHandler(this.btnRootOverride_Click);
            // 
            // txtRootOverride
            // 
            this.txtRootOverride.Enabled = false;
            this.txtRootOverride.Location = new System.Drawing.Point(6, 25);
            this.txtRootOverride.Name = "txtRootOverride";
            this.txtRootOverride.Size = new System.Drawing.Size(319, 20);
            this.txtRootOverride.TabIndex = 21;
            // 
            // txtIP
            // 
            this.txtIP.Enabled = false;
            this.txtIP.Location = new System.Drawing.Point(80, 17);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(96, 20);
            this.txtIP.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtIP, "Set a default IP for getting CPUKey\r\nover a network.");
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(203, 290);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Save and Apply";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtTimingLptPort
            // 
            this.txtTimingLptPort.Enabled = false;
            this.txtTimingLptPort.Location = new System.Drawing.Point(121, 17);
            this.txtTimingLptPort.MaxLength = 5;
            this.txtTimingLptPort.Name = "txtTimingLptPort";
            this.txtTimingLptPort.Size = new System.Drawing.Size(55, 20);
            this.txtTimingLptPort.TabIndex = 52;
            this.toolTip1.SetToolTip(this.txtTimingLptPort, "determines the name given to the file\r\nproduced upon creation of a Nand\r\nImage");
            this.txtTimingLptPort.TextChanged += new System.EventHandler(this.txtTimingLptPort_TextChanged);
            // 
            // chkRootOverride
            // 
            this.chkRootOverride.AutoSize = true;
            this.chkRootOverride.Location = new System.Drawing.Point(6, 7);
            this.chkRootOverride.Name = "chkRootOverride";
            this.chkRootOverride.Size = new System.Drawing.Size(127, 17);
            this.chkRootOverride.TabIndex = 20;
            this.chkRootOverride.Text = "Root Folder Override:";
            this.toolTip1.SetToolTip(this.chkRootOverride, "Override the location of the output and XeBuild folders instead of the default (a" +
        "pplication root)\r\nDo not set unless you have a specific reason to redirect folde" +
        "rs");
            this.chkRootOverride.UseVisualStyleBackColor = true;
            this.chkRootOverride.CheckedChanged += new System.EventHandler(this.chkRootOverride_CheckedChanged);
            // 
            // chkIPDefault
            // 
            this.chkIPDefault.AutoSize = true;
            this.chkIPDefault.Location = new System.Drawing.Point(11, 19);
            this.chkIPDefault.Name = "chkIPDefault";
            this.chkIPDefault.Size = new System.Drawing.Size(69, 17);
            this.chkIPDefault.TabIndex = 2;
            this.chkIPDefault.Text = "Override:";
            this.toolTip1.SetToolTip(this.chkIPDefault, "When unchecked (default), this is fetched automatically\r\nManually set the prefix " +
        "that appears by default in the lower right of the application");
            this.chkIPDefault.UseVisualStyleBackColor = true;
            this.chkIPDefault.CheckedChanged += new System.EventHandler(this.chkIPDefault_CheckedChanged);
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkIPDefault);
            this.groupBox2.Controls.Add(this.txtIP);
            this.groupBox2.Location = new System.Drawing.Point(402, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(183, 45);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "XeLL IP Prefix";
            // 
            // chkAutoExtract
            // 
            this.chkAutoExtract.AutoSize = true;
            this.chkAutoExtract.Location = new System.Drawing.Point(6, 54);
            this.chkAutoExtract.Name = "chkAutoExtract";
            this.chkAutoExtract.Size = new System.Drawing.Size(213, 17);
            this.chkAutoExtract.TabIndex = 23;
            this.chkAutoExtract.Text = "Extract files from nand at CPU key entry";
            this.chkAutoExtract.UseVisualStyleBackColor = true;
            // 
            // chkAllMove
            // 
            this.chkAllMove.AutoSize = true;
            this.chkAllMove.Checked = true;
            this.chkAllMove.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllMove.Location = new System.Drawing.Point(6, 77);
            this.chkAllMove.Name = "chkAllMove";
            this.chkAllMove.Size = new System.Drawing.Size(274, 17);
            this.chkAllMove.TabIndex = 24;
            this.chkAllMove.Text = "Only move to XeBuild folder upon first CPU Key entry";
            this.chkAllMove.UseVisualStyleBackColor = true;
            // 
            // logBackgroundCustom
            // 
            this.logBackgroundCustom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.logBackgroundCustom.Location = new System.Drawing.Point(233, 192);
            this.logBackgroundCustom.Name = "logBackgroundCustom";
            this.logBackgroundCustom.Size = new System.Drawing.Size(50, 20);
            this.logBackgroundCustom.TabIndex = 34;
            this.logBackgroundCustom.TabStop = false;
            this.logBackgroundCustom.Text = "Custom";
            this.logBackgroundCustom.UseVisualStyleBackColor = true;
            this.logBackgroundCustom.Click += new System.EventHandler(this.logBackgroundCustom_Click);
            // 
            // logBackgroundBlue
            // 
            this.logBackgroundBlue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.logBackgroundBlue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(36)))), ((int)(((byte)(86)))));
            this.logBackgroundBlue.Location = new System.Drawing.Point(207, 192);
            this.logBackgroundBlue.Name = "logBackgroundBlue";
            this.logBackgroundBlue.Size = new System.Drawing.Size(20, 20);
            this.logBackgroundBlue.TabIndex = 33;
            this.logBackgroundBlue.TabStop = false;
            this.logBackgroundBlue.UseVisualStyleBackColor = false;
            this.logBackgroundBlue.Click += new System.EventHandler(this.logBackgroundBlue_Click);
            // 
            // logBackgroundBlack
            // 
            this.logBackgroundBlack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.logBackgroundBlack.BackColor = System.Drawing.Color.Black;
            this.logBackgroundBlack.Location = new System.Drawing.Point(181, 192);
            this.logBackgroundBlack.Name = "logBackgroundBlack";
            this.logBackgroundBlack.Size = new System.Drawing.Size(20, 20);
            this.logBackgroundBlack.TabIndex = 32;
            this.logBackgroundBlack.TabStop = false;
            this.logBackgroundBlack.UseVisualStyleBackColor = false;
            this.logBackgroundBlack.Click += new System.EventHandler(this.logBackgroundBlack_Click);
            // 
            // logTextCustom
            // 
            this.logTextCustom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.logTextCustom.Location = new System.Drawing.Point(233, 218);
            this.logTextCustom.Name = "logTextCustom";
            this.logTextCustom.Size = new System.Drawing.Size(50, 20);
            this.logTextCustom.TabIndex = 37;
            this.logTextCustom.TabStop = false;
            this.logTextCustom.Text = "Custom";
            this.logTextCustom.UseVisualStyleBackColor = true;
            this.logTextCustom.Click += new System.EventHandler(this.logTextCustom_Click);
            // 
            // logTextBlack
            // 
            this.logTextBlack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.logTextBlack.BackColor = System.Drawing.Color.Black;
            this.logTextBlack.Location = new System.Drawing.Point(207, 218);
            this.logTextBlack.Name = "logTextBlack";
            this.logTextBlack.Size = new System.Drawing.Size(20, 20);
            this.logTextBlack.TabIndex = 36;
            this.logTextBlack.TabStop = false;
            this.logTextBlack.UseVisualStyleBackColor = false;
            this.logTextBlack.Click += new System.EventHandler(this.logTextBlack_Click);
            // 
            // logTextWhite
            // 
            this.logTextWhite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.logTextWhite.BackColor = System.Drawing.Color.White;
            this.logTextWhite.Location = new System.Drawing.Point(181, 218);
            this.logTextWhite.Name = "logTextWhite";
            this.logTextWhite.Size = new System.Drawing.Size(20, 20);
            this.logTextWhite.TabIndex = 35;
            this.logTextWhite.TabStop = false;
            this.logTextWhite.UseVisualStyleBackColor = false;
            this.logTextWhite.Click += new System.EventHandler(this.logTextWhite_Click);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 221);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 13);
            this.label11.TabIndex = 42;
            this.label11.Text = "Log Text Color: ";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 195);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Log Background Color:";
            // 
            // logDefault
            // 
            this.logDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.logDefault.Location = new System.Drawing.Point(125, 192);
            this.logDefault.Name = "logDefault";
            this.logDefault.Size = new System.Drawing.Size(50, 46);
            this.logDefault.TabIndex = 31;
            this.logDefault.Text = "Default";
            this.logDefault.UseVisualStyleBackColor = true;
            this.logDefault.Click += new System.EventHandler(this.logDefault_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SlimPreferSrgh);
            this.groupBox3.Controls.Add(this.SlimPreferRgh12);
            this.groupBox3.Location = new System.Drawing.Point(402, 59);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(183, 45);
            this.groupBox3.TabIndex = 39;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Preferred Slim Method";
            // 
            // SlimPreferSrgh
            // 
            this.SlimPreferSrgh.AutoSize = true;
            this.SlimPreferSrgh.Location = new System.Drawing.Point(121, 18);
            this.SlimPreferSrgh.Name = "SlimPreferSrgh";
            this.SlimPreferSrgh.Size = new System.Drawing.Size(59, 17);
            this.SlimPreferSrgh.TabIndex = 1;
            this.SlimPreferSrgh.Text = "S-RGH";
            this.SlimPreferSrgh.UseVisualStyleBackColor = true;
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
            this.groupBox4.Location = new System.Drawing.Point(402, 111);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(183, 45);
            this.groupBox4.TabIndex = 40;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Timing File Programming";
            // 
            // rbtnTimingLpt
            // 
            this.rbtnTimingLpt.AutoSize = true;
            this.rbtnTimingLpt.Location = new System.Drawing.Point(72, 18);
            this.rbtnTimingLpt.Name = "rbtnTimingLpt";
            this.rbtnTimingLpt.Size = new System.Drawing.Size(48, 17);
            this.rbtnTimingLpt.TabIndex = 1;
            this.rbtnTimingLpt.Text = "LPT:";
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
            // btnDefaults
            // 
            this.btnDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDefaults.Location = new System.Drawing.Point(309, 290);
            this.btnDefaults.Name = "btnDefaults";
            this.btnDefaults.Size = new System.Drawing.Size(100, 23);
            this.btnDefaults.TabIndex = 40;
            this.btnDefaults.Text = "Restore Defaults";
            this.btnDefaults.UseVisualStyleBackColor = true;
            this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
            // 
            // chkNoPatchWarnings
            // 
            this.chkNoPatchWarnings.AutoSize = true;
            this.chkNoPatchWarnings.Location = new System.Drawing.Point(6, 123);
            this.chkNoPatchWarnings.Name = "chkNoPatchWarnings";
            this.chkNoPatchWarnings.Size = new System.Drawing.Size(206, 17);
            this.chkNoPatchWarnings.TabIndex = 27;
            this.chkNoPatchWarnings.Text = "Do not show patch warning messages";
            this.chkNoPatchWarnings.UseVisualStyleBackColor = true;
            this.chkNoPatchWarnings.Click += new System.EventHandler(this.chkNoPatchWarnings_Click);
            // 
            // chkPlaySuccess
            // 
            this.chkPlaySuccess.AutoSize = true;
            this.chkPlaySuccess.Checked = true;
            this.chkPlaySuccess.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPlaySuccess.Location = new System.Drawing.Point(6, 146);
            this.chkPlaySuccess.Name = "chkPlaySuccess";
            this.chkPlaySuccess.Size = new System.Drawing.Size(157, 17);
            this.chkPlaySuccess.TabIndex = 29;
            this.chkPlaySuccess.Text = "Play success sound (chime)";
            this.chkPlaySuccess.UseVisualStyleBackColor = true;
            // 
            // chkPlayError
            // 
            this.chkPlayError.AutoSize = true;
            this.chkPlayError.Checked = true;
            this.chkPlayError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPlayError.Location = new System.Drawing.Point(6, 169);
            this.chkPlayError.Name = "chkPlayError";
            this.chkPlayError.Size = new System.Drawing.Size(138, 17);
            this.chkPlayError.TabIndex = 30;
            this.chkPlayError.Text = "Play error sound (oh no)";
            this.chkPlayError.UseVisualStyleBackColor = true;
            // 
            // chkAutoDelXeLL
            // 
            this.chkAutoDelXeLL.AutoSize = true;
            this.chkAutoDelXeLL.Checked = true;
            this.chkAutoDelXeLL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoDelXeLL.Location = new System.Drawing.Point(6, 100);
            this.chkAutoDelXeLL.Name = "chkAutoDelXeLL";
            this.chkAutoDelXeLL.Size = new System.Drawing.Size(226, 17);
            this.chkAutoDelXeLL.TabIndex = 28;
            this.chkAutoDelXeLL.Text = "Auto delete XeLL file after successful write";
            this.chkAutoDelXeLL.UseVisualStyleBackColor = true;
            // 
            // tabCSettings
            // 
            this.tabCSettings.Controls.Add(this.tabGeneral);
            this.tabCSettings.Controls.Add(this.tabBackup);
            this.tabCSettings.Location = new System.Drawing.Point(5, 5);
            this.tabCSettings.Name = "tabCSettings";
            this.tabCSettings.SelectedIndex = 0;
            this.tabCSettings.Size = new System.Drawing.Size(604, 271);
            this.tabCSettings.TabIndex = 2;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.chkRootOverride);
            this.tabGeneral.Controls.Add(this.btnRootOverride);
            this.tabGeneral.Controls.Add(this.chkAutoDelXeLL);
            this.tabGeneral.Controls.Add(this.txtRootOverride);
            this.tabGeneral.Controls.Add(this.chkPlayError);
            this.tabGeneral.Controls.Add(this.chkPlaySuccess);
            this.tabGeneral.Controls.Add(this.chkNoPatchWarnings);
            this.tabGeneral.Controls.Add(this.groupBox2);
            this.tabGeneral.Controls.Add(this.chkAutoExtract);
            this.tabGeneral.Controls.Add(this.groupBox4);
            this.tabGeneral.Controls.Add(this.chkAllMove);
            this.tabGeneral.Controls.Add(this.groupBox3);
            this.tabGeneral.Controls.Add(this.logDefault);
            this.tabGeneral.Controls.Add(this.logTextCustom);
            this.tabGeneral.Controls.Add(this.label10);
            this.tabGeneral.Controls.Add(this.logTextBlack);
            this.tabGeneral.Controls.Add(this.logBackgroundBlack);
            this.tabGeneral.Controls.Add(this.logTextWhite);
            this.tabGeneral.Controls.Add(this.logBackgroundBlue);
            this.tabGeneral.Controls.Add(this.label11);
            this.tabGeneral.Controls.Add(this.logBackgroundCustom);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(596, 245);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // tabBackup
            // 
            this.tabBackup.Controls.Add(this.groupBackupNaming);
            this.tabBackup.Controls.Add(this.label1);
            this.tabBackup.Controls.Add(this.btnBackupRoot);
            this.tabBackup.Controls.Add(this.txtBackupRoot);
            this.tabBackup.Controls.Add(this.chkBackupEn);
            this.tabBackup.Controls.Add(this.groupBackupType);
            this.tabBackup.Location = new System.Drawing.Point(4, 22);
            this.tabBackup.Name = "tabBackup";
            this.tabBackup.Padding = new System.Windows.Forms.Padding(3);
            this.tabBackup.Size = new System.Drawing.Size(596, 245);
            this.tabBackup.TabIndex = 1;
            this.tabBackup.Text = "Backup";
            this.tabBackup.UseVisualStyleBackColor = true;
            // 
            // groupBackupNaming
            // 
            this.groupBackupNaming.Controls.Add(this.rbtnCtypeSnDate);
            this.groupBackupNaming.Controls.Add(this.rbtnSnOnly);
            this.groupBackupNaming.Controls.Add(this.rbtnCtypeSn);
            this.groupBackupNaming.Location = new System.Drawing.Point(6, 146);
            this.groupBackupNaming.Name = "groupBackupNaming";
            this.groupBackupNaming.Size = new System.Drawing.Size(354, 88);
            this.groupBackupNaming.TabIndex = 2;
            this.groupBackupNaming.TabStop = false;
            this.groupBackupNaming.Text = "Naming Scheme";
            // 
            // rbtnSnOnly
            // 
            this.rbtnSnOnly.AutoSize = true;
            this.rbtnSnOnly.Location = new System.Drawing.Point(9, 63);
            this.rbtnSnOnly.Name = "rbtnSnOnly";
            this.rbtnSnOnly.Size = new System.Drawing.Size(115, 17);
            this.rbtnSnOnly.TabIndex = 1;
            this.rbtnSnOnly.Text = "Serial Number Only";
            this.rbtnSnOnly.UseVisualStyleBackColor = true;
            // 
            // rbtnCtypeSn
            // 
            this.rbtnCtypeSn.AutoSize = true;
            this.rbtnCtypeSn.Checked = true;
            this.rbtnCtypeSn.Location = new System.Drawing.Point(9, 17);
            this.rbtnCtypeSn.Name = "rbtnCtypeSn";
            this.rbtnCtypeSn.Size = new System.Drawing.Size(180, 17);
            this.rbtnCtypeSn.TabIndex = 0;
            this.rbtnCtypeSn.Text = "Console Type and Serial Number";
            this.rbtnCtypeSn.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Backup Root Location:";
            // 
            // btnBackupRoot
            // 
            this.btnBackupRoot.Enabled = false;
            this.btnBackupRoot.Location = new System.Drawing.Point(331, 45);
            this.btnBackupRoot.Name = "btnBackupRoot";
            this.btnBackupRoot.Size = new System.Drawing.Size(29, 22);
            this.btnBackupRoot.TabIndex = 24;
            this.btnBackupRoot.Text = "...";
            this.btnBackupRoot.UseVisualStyleBackColor = true;
            this.btnBackupRoot.Click += new System.EventHandler(this.btnBackupRoot_Click);
            // 
            // txtBackupRoot
            // 
            this.txtBackupRoot.Enabled = false;
            this.txtBackupRoot.Location = new System.Drawing.Point(6, 46);
            this.txtBackupRoot.Name = "txtBackupRoot";
            this.txtBackupRoot.Size = new System.Drawing.Size(319, 20);
            this.txtBackupRoot.TabIndex = 23;
            // 
            // chkBackupEn
            // 
            this.chkBackupEn.AutoSize = true;
            this.chkBackupEn.Location = new System.Drawing.Point(6, 7);
            this.chkBackupEn.Name = "chkBackupEn";
            this.chkBackupEn.Size = new System.Drawing.Size(246, 17);
            this.chkBackupEn.TabIndex = 1;
            this.chkBackupEn.Text = "Automatically back up files after CPU key entry";
            this.chkBackupEn.UseVisualStyleBackColor = true;
            this.chkBackupEn.CheckedChanged += new System.EventHandler(this.chkBackupEn_CheckedChanged);
            // 
            // groupBackupType
            // 
            this.groupBackupType.Controls.Add(this.rbtnFolder);
            this.groupBackupType.Controls.Add(this.rbtnZip);
            this.groupBackupType.Enabled = false;
            this.groupBackupType.Location = new System.Drawing.Point(6, 74);
            this.groupBackupType.Name = "groupBackupType";
            this.groupBackupType.Size = new System.Drawing.Size(354, 65);
            this.groupBackupType.TabIndex = 0;
            this.groupBackupType.TabStop = false;
            this.groupBackupType.Text = "Backup Type";
            // 
            // rbtnFolder
            // 
            this.rbtnFolder.AutoSize = true;
            this.rbtnFolder.Location = new System.Drawing.Point(9, 40);
            this.rbtnFolder.Name = "rbtnFolder";
            this.rbtnFolder.Size = new System.Drawing.Size(251, 17);
            this.rbtnFolder.TabIndex = 1;
            this.rbtnFolder.Text = "Folder Copy - Copy top level files to a new folder\r\n";
            this.rbtnFolder.UseVisualStyleBackColor = true;
            // 
            // rbtnZip
            // 
            this.rbtnZip.AutoSize = true;
            this.rbtnZip.Checked = true;
            this.rbtnZip.Location = new System.Drawing.Point(9, 17);
            this.rbtnZip.Name = "rbtnZip";
            this.rbtnZip.Size = new System.Drawing.Size(293, 17);
            this.rbtnZip.TabIndex = 0;
            this.rbtnZip.TabStop = true;
            this.rbtnZip.Text = "ZIP Archive - Compresses all files and folders to a ZIP file";
            this.rbtnZip.UseVisualStyleBackColor = true;
            // 
            // rbtnCtypeSnDate
            // 
            this.rbtnCtypeSnDate.AutoSize = true;
            this.rbtnCtypeSnDate.Location = new System.Drawing.Point(9, 40);
            this.rbtnCtypeSnDate.Name = "rbtnCtypeSnDate";
            this.rbtnCtypeSnDate.Size = new System.Drawing.Size(212, 17);
            this.rbtnCtypeSnDate.TabIndex = 2;
            this.rbtnCtypeSnDate.Text = "Console Type, Serial Number, and Date";
            this.rbtnCtypeSnDate.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 328);
            this.Controls.Add(this.tabCSettings);
            this.Controls.Add(this.btnDefaults);
            this.Controls.Add(this.chkfiles);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabCSettings.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.tabBackup.ResumeLayout(false);
            this.tabBackup.PerformLayout();
            this.groupBackupNaming.ResumeLayout(false);
            this.groupBackupNaming.PerformLayout();
            this.groupBackupType.ResumeLayout(false);
            this.groupBackupType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtRootOverride;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnRootOverride;
        private System.Windows.Forms.FolderBrowserDialog folderDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkfiles;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkAutoExtract;
        private System.Windows.Forms.CheckBox chkAllMove;
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
        private System.Windows.Forms.CheckBox chkIPDefault;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtTimingLptPort;
        private System.Windows.Forms.RadioButton rbtnTimingLpt;
        private System.Windows.Forms.RadioButton rbtnTimingUsb;
        private System.Windows.Forms.Button btnDefaults;
        private System.Windows.Forms.CheckBox chkNoPatchWarnings;
        private System.Windows.Forms.CheckBox chkPlaySuccess;
        private System.Windows.Forms.CheckBox chkPlayError;
        private System.Windows.Forms.CheckBox chkAutoDelXeLL;
        private System.Windows.Forms.CheckBox chkRootOverride;
        private System.Windows.Forms.TabControl tabCSettings;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabBackup;
        private System.Windows.Forms.GroupBox groupBackupType;
        private System.Windows.Forms.CheckBox chkBackupEn;
        private System.Windows.Forms.RadioButton rbtnZip;
        private System.Windows.Forms.RadioButton rbtnFolder;
        private System.Windows.Forms.Button btnBackupRoot;
        private System.Windows.Forms.TextBox txtBackupRoot;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBackupNaming;
        private System.Windows.Forms.RadioButton rbtnSnOnly;
        private System.Windows.Forms.RadioButton rbtnCtypeSn;
        private System.Windows.Forms.RadioButton rbtnCtypeSnDate;
    }
}