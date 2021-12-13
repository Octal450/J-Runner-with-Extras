namespace JRunner
{
    partial class POST
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POST));
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtRate = new System.Windows.Forms.TextBox();
            this.btnNudge = new System.Windows.Forms.Button();
            this.numericCap = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericIter = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClearOutput = new System.Windows.Forms.Button();
            this.btnClearRater = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.numericCool = new System.Windows.Forms.NumericUpDown();
            this.txtProgress = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.RaterRes = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.SystemTxtbox = new System.Windows.Forms.TextBox();
            this.SetupDetailBtn = new System.Windows.Forms.Button();
            this.ScreenshotBTN = new System.Windows.Forms.Button();
            this.RaterSettings = new System.Windows.Forms.GroupBox();
            this.cr4but = new System.Windows.Forms.RadioButton();
            this.PhatFBut = new System.Windows.Forms.RadioButton();
            this.CorBut = new System.Windows.Forms.RadioButton();
            this.SlimBut = new System.Windows.Forms.RadioButton();
            this.PhatBut = new System.Windows.Forms.RadioButton();
            this.numericSDPause = new System.Windows.Forms.NumericUpDown();
            this.txtShow = new System.Windows.Forms.TextBox();
            this.PostOutButton = new System.Windows.Forms.Button();
            this.ResultsClipBtn = new System.Windows.Forms.Button();
            this.CycleClipBtn = new System.Windows.Forms.Button();
            this.RaterPIC = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericCap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericIter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericCool)).BeginInit();
            this.RaterSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSDPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RaterPIC)).BeginInit();
            this.SuspendLayout();
            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.Color.Black;
            this.txtOutput.ForeColor = System.Drawing.Color.Lime;
            this.txtOutput.Location = new System.Drawing.Point(10, 27);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(286, 443);
            this.txtOutput.TabIndex = 0;
            this.txtOutput.TabStop = false;
            this.toolTip1.SetToolTip(this.txtOutput, "Log window of POST Results \r\nShow exactly whats happening during boot cycle.\r\n");
            this.txtOutput.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtOutput_Enter);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(312, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.TabStop = false;
            this.btnStart.Text = "Start";
            this.toolTip1.SetToolTip(this.btnStart, "Starts Rating process - your console will power up!");
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(393, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.TabStop = false;
            this.btnStop.Text = "Stop";
            this.toolTip1.SetToolTip(this.btnStop, "Stops Rating process ");
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtRate
            // 
            this.txtRate.BackColor = System.Drawing.Color.Black;
            this.txtRate.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtRate.ForeColor = System.Drawing.Color.Lime;
            this.txtRate.Location = new System.Drawing.Point(315, 274);
            this.txtRate.Multiline = true;
            this.txtRate.Name = "txtRate";
            this.txtRate.ReadOnly = true;
            this.txtRate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRate.Size = new System.Drawing.Size(143, 196);
            this.txtRate.TabIndex = 4;
            this.txtRate.TabStop = false;
            this.toolTip1.SetToolTip(this.txtRate, "running log of the number of glitches per boot.\r\n");
            this.txtRate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtRate_Enter);
            // 
            // btnNudge
            // 
            this.btnNudge.Enabled = false;
            this.btnNudge.Location = new System.Drawing.Point(474, 12);
            this.btnNudge.Name = "btnNudge";
            this.btnNudge.Size = new System.Drawing.Size(75, 23);
            this.btnNudge.TabIndex = 5;
            this.btnNudge.TabStop = false;
            this.btnNudge.Text = "[Nudge]";
            this.toolTip1.SetToolTip(this.btnNudge, "This will power down the console and reboot (value will not count towards rating!" +
        ")");
            this.btnNudge.UseVisualStyleBackColor = true;
            this.btnNudge.Click += new System.EventHandler(this.btnNudge_Click);
            // 
            // numericCap
            // 
            this.numericCap.Location = new System.Drawing.Point(118, 138);
            this.numericCap.Name = "numericCap";
            this.numericCap.Size = new System.Drawing.Size(42, 20);
            this.numericCap.TabIndex = 4;
            this.numericCap.ValueChanged += new System.EventHandler(this.numericCap_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Cycle Cap: ";
            this.toolTip1.SetToolTip(this.label1, "Sets a maximum number of Cycles to attempt to Glitch\r\nbefore powering down and tr" +
        "ying again. (0 sets No Limit)");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "No. of Boots\r\n";
            this.toolTip1.SetToolTip(this.label2, "Choose how many Boot cycles you wish to perform (0 sets it to infinite)");
            // 
            // numericIter
            // 
            this.numericIter.Location = new System.Drawing.Point(118, 109);
            this.numericIter.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericIter.Name = "numericIter";
            this.numericIter.Size = new System.Drawing.Size(42, 20);
            this.numericIter.TabIndex = 3;
            this.numericIter.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(69, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "[0 = Off]";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(70, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "[0 = inf]";
            // 
            // btnClearOutput
            // 
            this.btnClearOutput.Enabled = false;
            this.btnClearOutput.Location = new System.Drawing.Point(307, 232);
            this.btnClearOutput.Name = "btnClearOutput";
            this.btnClearOutput.Size = new System.Drawing.Size(75, 23);
            this.btnClearOutput.TabIndex = 13;
            this.btnClearOutput.TabStop = false;
            this.btnClearOutput.Text = "Clear POST";
            this.toolTip1.SetToolTip(this.btnClearOutput, "Clears the POST Window");
            this.btnClearOutput.UseVisualStyleBackColor = true;
            this.btnClearOutput.Click += new System.EventHandler(this.btnClearOutput_Click);
            // 
            // btnClearRater
            // 
            this.btnClearRater.Enabled = false;
            this.btnClearRater.Location = new System.Drawing.Point(388, 232);
            this.btnClearRater.Name = "btnClearRater";
            this.btnClearRater.Size = new System.Drawing.Size(75, 23);
            this.btnClearRater.TabIndex = 14;
            this.btnClearRater.TabStop = false;
            this.btnClearRater.Text = "Clear Rater";
            this.toolTip1.SetToolTip(this.btnClearRater, "Clears the Glitches per Boot and Rating Window");
            this.btnClearRater.UseVisualStyleBackColor = true;
            this.btnClearRater.Click += new System.EventHandler(this.btnClearRater_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "txt";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Cooldown (Seconds)";
            this.toolTip1.SetToolTip(this.label5, "This is the number of Seconds in between boot cycles");
            // 
            // numericCool
            // 
            this.numericCool.Location = new System.Drawing.Point(118, 50);
            this.numericCool.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericCool.Name = "numericCool";
            this.numericCool.Size = new System.Drawing.Size(42, 20);
            this.numericCool.TabIndex = 1;
            this.numericCool.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // txtProgress
            // 
            this.txtProgress.BackColor = System.Drawing.Color.Black;
            this.txtProgress.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtProgress.ForeColor = System.Drawing.Color.Lime;
            this.txtProgress.Location = new System.Drawing.Point(487, 133);
            this.txtProgress.Multiline = true;
            this.txtProgress.Name = "txtProgress";
            this.txtProgress.ReadOnly = true;
            this.txtProgress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProgress.Size = new System.Drawing.Size(131, 337);
            this.txtProgress.TabIndex = 19;
            this.txtProgress.TabStop = false;
            this.toolTip1.SetToolTip(this.txtProgress, "shows current results of this Rating since\r\nStart button was pressed.\r\n");
            this.txtProgress.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtProgress_Enter);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(21, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "POST Output";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(323, 258);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Cycles to Glitch";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(493, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Results:";
            // 
            // RaterRes
            // 
            this.RaterRes.AutoSize = true;
            this.RaterRes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RaterRes.Location = new System.Drawing.Point(538, 116);
            this.RaterRes.Name = "RaterRes";
            this.RaterRes.Size = new System.Drawing.Size(80, 13);
            this.RaterRes.TabIndex = 24;
            this.RaterRes.Text = "More Data Req";
            this.RaterRes.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Shutdown Delay: ";
            this.toolTip1.SetToolTip(this.label9, "Sets a number of seconds after a boot is registered\r\nbefore powering down to allo" +
        "w viewing of xell on screen\r\n");
            // 
            // SystemTxtbox
            // 
            this.SystemTxtbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SystemTxtbox.Location = new System.Drawing.Point(24, 41);
            this.SystemTxtbox.Multiline = true;
            this.SystemTxtbox.Name = "SystemTxtbox";
            this.SystemTxtbox.Size = new System.Drawing.Size(242, 177);
            this.SystemTxtbox.TabIndex = 30;
            this.SystemTxtbox.Text = "Mobo:\r\nDips set on:\r\nCPU_RST:\r\nJTAG Wiring: \r\nAdditional info:";
            this.toolTip1.SetToolTip(this.SystemTxtbox, "Enter your setup details: dips set, wiring etc");
            this.SystemTxtbox.Visible = false;
            // 
            // SetupDetailBtn
            // 
            this.SetupDetailBtn.Location = new System.Drawing.Point(556, 12);
            this.SetupDetailBtn.Name = "SetupDetailBtn";
            this.SetupDetailBtn.Size = new System.Drawing.Size(16, 23);
            this.SetupDetailBtn.TabIndex = 31;
            this.SetupDetailBtn.Text = "!";
            this.toolTip1.SetToolTip(this.SetupDetailBtn, "Show / Hide Freetext box for entering your Setup Details");
            this.SetupDetailBtn.UseVisualStyleBackColor = true;
            this.SetupDetailBtn.Click += new System.EventHandler(this.SetupDetailBtn_Click);
            // 
            // ScreenshotBTN
            // 
            this.ScreenshotBTN.Image = global::JRunner.Properties.Resources.cam;
            this.ScreenshotBTN.Location = new System.Drawing.Point(578, 8);
            this.ScreenshotBTN.Name = "ScreenshotBTN";
            this.ScreenshotBTN.Size = new System.Drawing.Size(43, 31);
            this.ScreenshotBTN.TabIndex = 29;
            this.toolTip1.SetToolTip(this.ScreenshotBTN, "Take a screenshot for uploading to forum");
            this.ScreenshotBTN.UseVisualStyleBackColor = true;
            this.ScreenshotBTN.Click += new System.EventHandler(this.ScreenshotBTN_Click);
            // 
            // RaterSettings
            // 
            this.RaterSettings.Controls.Add(this.cr4but);
            this.RaterSettings.Controls.Add(this.PhatFBut);
            this.RaterSettings.Controls.Add(this.CorBut);
            this.RaterSettings.Controls.Add(this.SlimBut);
            this.RaterSettings.Controls.Add(this.PhatBut);
            this.RaterSettings.Controls.Add(this.numericSDPause);
            this.RaterSettings.Controls.Add(this.label4);
            this.RaterSettings.Controls.Add(this.label9);
            this.RaterSettings.Controls.Add(this.numericCap);
            this.RaterSettings.Controls.Add(this.label1);
            this.RaterSettings.Controls.Add(this.label2);
            this.RaterSettings.Controls.Add(this.numericIter);
            this.RaterSettings.Controls.Add(this.label3);
            this.RaterSettings.Controls.Add(this.label5);
            this.RaterSettings.Controls.Add(this.numericCool);
            this.RaterSettings.Location = new System.Drawing.Point(305, 60);
            this.RaterSettings.Name = "RaterSettings";
            this.RaterSettings.Size = new System.Drawing.Size(168, 166);
            this.RaterSettings.TabIndex = 25;
            this.RaterSettings.TabStop = false;
            this.RaterSettings.Text = "Settings";
            this.RaterSettings.Enter += new System.EventHandler(this.RaterSettings_Enter);
            // 
            // cr4but
            // 
            this.cr4but.AutoSize = true;
            this.cr4but.Location = new System.Drawing.Point(59, 32);
            this.cr4but.Name = "cr4but";
            this.cr4but.Size = new System.Drawing.Size(46, 17);
            this.cr4but.TabIndex = 21;
            this.cr4but.TabStop = true;
            this.cr4but.Text = "CR4";
            this.cr4but.UseVisualStyleBackColor = true;
            // 
            // PhatFBut
            // 
            this.PhatFBut.AutoSize = true;
            this.PhatFBut.Location = new System.Drawing.Point(12, 16);
            this.PhatFBut.Name = "PhatFBut";
            this.PhatFBut.Size = new System.Drawing.Size(47, 17);
            this.PhatFBut.TabIndex = 20;
            this.PhatFBut.TabStop = true;
            this.PhatFBut.Text = "Phat";
            this.PhatFBut.UseVisualStyleBackColor = true;
            this.PhatFBut.CheckedChanged += new System.EventHandler(this.PhatFBut_CheckedChanged);
            // 
            // CorBut
            // 
            this.CorBut.AutoSize = true;
            this.CorBut.Location = new System.Drawing.Point(100, 16);
            this.CorBut.Name = "CorBut";
            this.CorBut.Size = new System.Drawing.Size(56, 17);
            this.CorBut.TabIndex = 19;
            this.CorBut.TabStop = true;
            this.CorBut.Text = "Cor 3+";
            this.CorBut.UseVisualStyleBackColor = true;
            this.CorBut.CheckedChanged += new System.EventHandler(this.CorBut_CheckedChanged);
            // 
            // SlimBut
            // 
            this.SlimBut.AutoSize = true;
            this.SlimBut.Checked = true;
            this.SlimBut.Location = new System.Drawing.Point(59, 16);
            this.SlimBut.Name = "SlimBut";
            this.SlimBut.Size = new System.Drawing.Size(44, 17);
            this.SlimBut.TabIndex = 1;
            this.SlimBut.TabStop = true;
            this.SlimBut.Text = "Slim";
            this.SlimBut.UseVisualStyleBackColor = true;
            this.SlimBut.CheckedChanged += new System.EventHandler(this.SlimBut_CheckedChanged);
            // 
            // PhatBut
            // 
            this.PhatBut.AutoSize = true;
            this.PhatBut.Enabled = false;
            this.PhatBut.Location = new System.Drawing.Point(112, 0);
            this.PhatBut.Name = "PhatBut";
            this.PhatBut.Size = new System.Drawing.Size(56, 17);
            this.PhatBut.TabIndex = 0;
            this.PhatBut.Text = "PFake";
            this.PhatBut.UseVisualStyleBackColor = true;
            this.PhatBut.Visible = false;
            this.PhatBut.CheckedChanged += new System.EventHandler(this.PhatBut_CheckedChanged);
            // 
            // numericSDPause
            // 
            this.numericSDPause.Location = new System.Drawing.Point(118, 78);
            this.numericSDPause.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericSDPause.Name = "numericSDPause";
            this.numericSDPause.Size = new System.Drawing.Size(42, 20);
            this.numericSDPause.TabIndex = 2;
            this.numericSDPause.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // txtShow
            // 
            this.txtShow.BackColor = System.Drawing.SystemColors.Control;
            this.txtShow.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtShow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtShow.ForeColor = System.Drawing.Color.MediumBlue;
            this.txtShow.Location = new System.Drawing.Point(350, 41);
            this.txtShow.Name = "txtShow";
            this.txtShow.ReadOnly = true;
            this.txtShow.Size = new System.Drawing.Size(80, 13);
            this.txtShow.TabIndex = 26;
            this.txtShow.TabStop = false;
            this.txtShow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PostOutButton
            // 
            this.PostOutButton.Image = global::JRunner.Properties.Resources.save_icon;
            this.PostOutButton.Location = new System.Drawing.Point(251, 445);
            this.PostOutButton.Name = "PostOutButton";
            this.PostOutButton.Size = new System.Drawing.Size(27, 25);
            this.PostOutButton.TabIndex = 32;
            this.PostOutButton.UseVisualStyleBackColor = true;
            this.PostOutButton.Visible = false;
            this.PostOutButton.Click += new System.EventHandler(this.PostOutButton_Click);
            // 
            // ResultsClipBtn
            // 
            this.ResultsClipBtn.BackgroundImage = global::JRunner.Properties.Resources.clipboard;
            this.ResultsClipBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ResultsClipBtn.Location = new System.Drawing.Point(571, 445);
            this.ResultsClipBtn.Name = "ResultsClipBtn";
            this.ResultsClipBtn.Size = new System.Drawing.Size(28, 25);
            this.ResultsClipBtn.TabIndex = 28;
            this.ResultsClipBtn.TabStop = false;
            this.ResultsClipBtn.UseVisualStyleBackColor = true;
            this.ResultsClipBtn.Visible = false;
            this.ResultsClipBtn.Click += new System.EventHandler(this.ResultsClipBtn_Click);
            // 
            // CycleClipBtn
            // 
            this.CycleClipBtn.BackgroundImage = global::JRunner.Properties.Resources.clipboard;
            this.CycleClipBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CycleClipBtn.Location = new System.Drawing.Point(411, 445);
            this.CycleClipBtn.Name = "CycleClipBtn";
            this.CycleClipBtn.Size = new System.Drawing.Size(28, 25);
            this.CycleClipBtn.TabIndex = 27;
            this.CycleClipBtn.TabStop = false;
            this.CycleClipBtn.UseVisualStyleBackColor = true;
            this.CycleClipBtn.Visible = false;
            this.CycleClipBtn.Click += new System.EventHandler(this.CycleClipBtn_Click);
            // 
            // RaterPIC
            // 
            this.RaterPIC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RaterPIC.InitialImage = null;
            this.RaterPIC.Location = new System.Drawing.Point(521, 54);
            this.RaterPIC.Name = "RaterPIC";
            this.RaterPIC.Size = new System.Drawing.Size(50, 44);
            this.RaterPIC.TabIndex = 23;
            this.RaterPIC.TabStop = false;
            // 
            // POST
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 482);
            this.Controls.Add(this.PostOutButton);
            this.Controls.Add(this.SetupDetailBtn);
            this.Controls.Add(this.SystemTxtbox);
            this.Controls.Add(this.ScreenshotBTN);
            this.Controls.Add(this.ResultsClipBtn);
            this.Controls.Add(this.CycleClipBtn);
            this.Controls.Add(this.txtShow);
            this.Controls.Add(this.RaterSettings);
            this.Controls.Add(this.RaterRes);
            this.Controls.Add(this.RaterPIC);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtProgress);
            this.Controls.Add(this.btnClearRater);
            this.Controls.Add(this.btnClearOutput);
            this.Controls.Add(this.btnNudge);
            this.Controls.Add(this.txtRate);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtOutput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "POST";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "POST Monitor/RATER";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.POST_FormClosing);
            this.Load += new System.EventHandler(this.POST_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericCap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericIter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericCool)).EndInit();
            this.RaterSettings.ResumeLayout(false);
            this.RaterSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSDPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RaterPIC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox txtRate;
        private System.Windows.Forms.Button btnNudge;
        private System.Windows.Forms.NumericUpDown numericCap;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericIter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClearOutput;
        private System.Windows.Forms.Button btnClearRater;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericCool;
        private System.Windows.Forms.TextBox txtProgress;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox RaterPIC;
        private System.Windows.Forms.Label RaterRes;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox RaterSettings;
        private System.Windows.Forms.TextBox txtShow;
        private System.Windows.Forms.Button CycleClipBtn;
        private System.Windows.Forms.Button ResultsClipBtn;
        private System.Windows.Forms.NumericUpDown numericSDPause;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton SlimBut;
        private System.Windows.Forms.RadioButton PhatBut;
        private System.Windows.Forms.RadioButton CorBut;
        private System.Windows.Forms.RadioButton PhatFBut;
        private System.Windows.Forms.Button ScreenshotBTN;
        private System.Windows.Forms.TextBox SystemTxtbox;
        private System.Windows.Forms.Button SetupDetailBtn;
        private System.Windows.Forms.Button PostOutButton;
        private System.Windows.Forms.RadioButton cr4but;
    }
}
