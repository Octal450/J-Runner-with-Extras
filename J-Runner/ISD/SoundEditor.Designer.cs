namespace JRunner.Forms
{
    partial class SoundEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoundEditor));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnHalo4 = new System.Windows.Forms.RadioButton();
            this.rbtnOther = new System.Windows.Forms.RadioButton();
            this.rbtnStandard = new System.Windows.Forms.RadioButton();
            this.rbtnHalo = new System.Windows.Forms.RadioButton();
            this.rbtnStarWars = new System.Windows.Forms.RadioButton();
            this.rbtnMW3 = new System.Windows.Forms.RadioButton();
            this.rbtnGOW3 = new System.Windows.Forms.RadioButton();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnFile = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnPlayPower = new System.Windows.Forms.Button();
            this.btnPlayEject = new System.Windows.Forms.Button();
            this.veraftreadchk = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCustom1 = new System.Windows.Forms.Button();
            this.btnCustom3 = new System.Windows.Forms.Button();
            this.btnCustom2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnShutdownPlay = new System.Windows.Forms.Button();
            this.btnEjectPlay = new System.Windows.Forms.Button();
            this.btnStartupPlay = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.btnPowerOpen = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.txtFinalbin = new System.Windows.Forms.TextBox();
            this.txtPower = new System.Windows.Forms.TextBox();
            this.btnEjectOpen = new System.Windows.Forms.Button();
            this.btnStartOpen = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtEject = new System.Windows.Forms.TextBox();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtnHalo4);
            this.groupBox1.Controls.Add(this.rbtnOther);
            this.groupBox1.Controls.Add(this.rbtnStandard);
            this.groupBox1.Controls.Add(this.rbtnHalo);
            this.groupBox1.Controls.Add(this.rbtnStarWars);
            this.groupBox1.Controls.Add(this.rbtnMW3);
            this.groupBox1.Controls.Add(this.rbtnGOW3);
            this.groupBox1.Location = new System.Drawing.Point(284, 75);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(120, 253);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Files";
            // 
            // rbtnHalo4
            // 
            this.rbtnHalo4.AutoSize = true;
            this.rbtnHalo4.Location = new System.Drawing.Point(6, 111);
            this.rbtnHalo4.Name = "rbtnHalo4";
            this.rbtnHalo4.Size = new System.Drawing.Size(56, 17);
            this.rbtnHalo4.TabIndex = 6;
            this.rbtnHalo4.TabStop = true;
            this.rbtnHalo4.Text = "Halo 4";
            this.toolTip1.SetToolTip(this.rbtnHalo4, "Use any of these to write the original soundfiles to your console");
            this.rbtnHalo4.UseVisualStyleBackColor = true;
            this.rbtnHalo4.CheckedChanged += new System.EventHandler(this.rbtnFile_CheckedChanged);
            // 
            // rbtnOther
            // 
            this.rbtnOther.AutoSize = true;
            this.rbtnOther.Checked = true;
            this.rbtnOther.Location = new System.Drawing.Point(6, 157);
            this.rbtnOther.Name = "rbtnOther";
            this.rbtnOther.Size = new System.Drawing.Size(60, 17);
            this.rbtnOther.TabIndex = 5;
            this.rbtnOther.TabStop = true;
            this.rbtnOther.Text = "Custom";
            this.toolTip1.SetToolTip(this.rbtnOther, "Custom selected when reading or writing an external/created file");
            this.rbtnOther.UseVisualStyleBackColor = true;
            this.rbtnOther.CheckedChanged += new System.EventHandler(this.rbtnOther_CheckedChanged);
            // 
            // rbtnStandard
            // 
            this.rbtnStandard.AutoSize = true;
            this.rbtnStandard.Location = new System.Drawing.Point(6, 134);
            this.rbtnStandard.Name = "rbtnStandard";
            this.rbtnStandard.Size = new System.Drawing.Size(90, 17);
            this.rbtnStandard.TabIndex = 4;
            this.rbtnStandard.Text = "Standard Slim";
            this.toolTip1.SetToolTip(this.rbtnStandard, "Use any of these to write the original soundfiles to your console");
            this.rbtnStandard.UseVisualStyleBackColor = true;
            this.rbtnStandard.CheckedChanged += new System.EventHandler(this.rbtnFile_CheckedChanged);
            // 
            // rbtnHalo
            // 
            this.rbtnHalo.AutoSize = true;
            this.rbtnHalo.Location = new System.Drawing.Point(6, 88);
            this.rbtnHalo.Name = "rbtnHalo";
            this.rbtnHalo.Size = new System.Drawing.Size(82, 17);
            this.rbtnHalo.TabIndex = 3;
            this.rbtnHalo.Text = "Halo Reach";
            this.toolTip1.SetToolTip(this.rbtnHalo, "Use any of these to write the original soundfiles to your console");
            this.rbtnHalo.UseVisualStyleBackColor = true;
            this.rbtnHalo.CheckedChanged += new System.EventHandler(this.rbtnFile_CheckedChanged);
            // 
            // rbtnStarWars
            // 
            this.rbtnStarWars.AutoSize = true;
            this.rbtnStarWars.Location = new System.Drawing.Point(6, 65);
            this.rbtnStarWars.Name = "rbtnStarWars";
            this.rbtnStarWars.Size = new System.Drawing.Size(72, 17);
            this.rbtnStarWars.TabIndex = 2;
            this.rbtnStarWars.Text = "Star Wars";
            this.toolTip1.SetToolTip(this.rbtnStarWars, "Use any of these to write the original soundfiles to your console");
            this.rbtnStarWars.UseVisualStyleBackColor = true;
            this.rbtnStarWars.CheckedChanged += new System.EventHandler(this.rbtnFile_CheckedChanged);
            // 
            // rbtnMW3
            // 
            this.rbtnMW3.AutoSize = true;
            this.rbtnMW3.Location = new System.Drawing.Point(6, 42);
            this.rbtnMW3.Name = "rbtnMW3";
            this.rbtnMW3.Size = new System.Drawing.Size(111, 17);
            this.rbtnMW3.TabIndex = 1;
            this.rbtnMW3.Text = "Modern Warfare 3";
            this.toolTip1.SetToolTip(this.rbtnMW3, "Use any of these to write the original soundfiles to your console");
            this.rbtnMW3.UseVisualStyleBackColor = true;
            this.rbtnMW3.CheckedChanged += new System.EventHandler(this.rbtnFile_CheckedChanged);
            // 
            // rbtnGOW3
            // 
            this.rbtnGOW3.AutoSize = true;
            this.rbtnGOW3.Location = new System.Drawing.Point(6, 19);
            this.rbtnGOW3.Name = "rbtnGOW3";
            this.rbtnGOW3.Size = new System.Drawing.Size(97, 17);
            this.rbtnGOW3.TabIndex = 0;
            this.rbtnGOW3.Text = "Gears of War 3";
            this.toolTip1.SetToolTip(this.rbtnGOW3, "Use any of these to write the original soundfiles to your console");
            this.rbtnGOW3.UseVisualStyleBackColor = true;
            this.rbtnGOW3.CheckedChanged += new System.EventHandler(this.rbtnFile_CheckedChanged);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(11, 28);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(141, 40);
            this.btnRead.TabIndex = 1;
            this.btnRead.Text = "Read Sound from Console";
            this.toolTip1.SetToolTip(this.btnRead, "Read the sound file from the console\r\n");
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            this.btnRead.Enabled = false;
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(11, 128);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(141, 38);
            this.btnWrite.TabIndex = 2;
            this.btnWrite.Text = "Write Sound to Console";
            this.toolTip1.SetToolTip(this.btnWrite, "Writes the file shown in the box below to the console");
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            this.btnWrite.Enabled = false;
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(11, 80);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(141, 39);
            this.btnVerify.TabIndex = 3;
            this.btnVerify.Text = "Verify Sound against Console";
            this.toolTip1.SetToolTip(this.btnVerify, "This checks the file on the console\r\n against the file on your PC");
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            this.btnVerify.Enabled = false;
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(239, 206);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(28, 23);
            this.btnFile.TabIndex = 4;
            this.btnFile.Text = "...";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // txtFile
            // 
            this.txtFile.AllowDrop = true;
            this.txtFile.Location = new System.Drawing.Point(6, 208);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(229, 20);
            this.txtFile.TabIndex = 5;
            this.txtFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.txtFile, "the file that will be read from / written to \r\nyour console");
            this.txtFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFile_DragDrop);
            this.txtFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtFile_DragEnter);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 235);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(261, 11);
            this.progressBar1.TabIndex = 6;
            this.toolTip1.SetToolTip(this.progressBar1, "Progress bar for read / write / verify");
            // 
            // btnPlayPower
            // 
            this.btnPlayPower.Location = new System.Drawing.Point(8, 16);
            this.btnPlayPower.Name = "btnPlayPower";
            this.btnPlayPower.Size = new System.Drawing.Size(81, 23);
            this.btnPlayPower.TabIndex = 7;
            this.btnPlayPower.Text = "Play Power";
            this.toolTip1.SetToolTip(this.btnPlayPower, "Play Sound on your console\r\nJR-P must be connected and console\r\nin standby!");
            this.btnPlayPower.UseVisualStyleBackColor = true;
            this.btnPlayPower.Click += new System.EventHandler(this.btnPlayPower_Click);
            // 
            // btnPlayEject
            // 
            this.btnPlayEject.Location = new System.Drawing.Point(9, 45);
            this.btnPlayEject.Name = "btnPlayEject";
            this.btnPlayEject.Size = new System.Drawing.Size(80, 23);
            this.btnPlayEject.TabIndex = 8;
            this.btnPlayEject.Text = "Play Eject";
            this.toolTip1.SetToolTip(this.btnPlayEject, "Play Sound on your console\r\nJR-P must be connected and console\r\nin standby!");
            this.btnPlayEject.UseVisualStyleBackColor = true;
            this.btnPlayEject.Click += new System.EventHandler(this.btnPlayEject_Click);
            // 
            // veraftreadchk
            // 
            this.veraftreadchk.AutoSize = true;
            this.veraftreadchk.Location = new System.Drawing.Point(11, 174);
            this.veraftreadchk.Name = "veraftreadchk";
            this.veraftreadchk.Size = new System.Drawing.Size(77, 17);
            this.veraftreadchk.TabIndex = 9;
            this.veraftreadchk.Text = "Auto-Verify";
            this.toolTip1.SetToolTip(this.veraftreadchk, "will auto verify after a write");
            this.veraftreadchk.UseVisualStyleBackColor = true;
            this.veraftreadchk.CheckedChanged += new System.EventHandler(this.veraftreadchk_CheckedChanged);
            this.veraftreadchk.Enabled = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Enabled = false;
            this.pictureBox1.Location = new System.Drawing.Point(29, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(355, 63);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // btnCustom1
            // 
            this.btnCustom1.Location = new System.Drawing.Point(8, 19);
            this.btnCustom1.Name = "btnCustom1";
            this.btnCustom1.Size = new System.Drawing.Size(81, 23);
            this.btnCustom1.TabIndex = 11;
            this.btnCustom1.Text = "Play Custom1";
            this.toolTip1.SetToolTip(this.btnCustom1, "Play Sound on your console\r\nJR-P must be connected and console\r\nin standby!");
            this.btnCustom1.UseVisualStyleBackColor = true;
            this.btnCustom1.Click += new System.EventHandler(this.btnCustom1_Click);
            // 
            // btnCustom3
            // 
            this.btnCustom3.Location = new System.Drawing.Point(8, 72);
            this.btnCustom3.Name = "btnCustom3";
            this.btnCustom3.Size = new System.Drawing.Size(81, 23);
            this.btnCustom3.TabIndex = 12;
            this.btnCustom3.Text = "Play Custom3";
            this.toolTip1.SetToolTip(this.btnCustom3, "Play Sound on your console\r\nJR-P must be connected and console\r\nin standby!");
            this.btnCustom3.UseVisualStyleBackColor = true;
            this.btnCustom3.Click += new System.EventHandler(this.btnCustom3_Click);
            // 
            // btnCustom2
            // 
            this.btnCustom2.Location = new System.Drawing.Point(8, 45);
            this.btnCustom2.Name = "btnCustom2";
            this.btnCustom2.Size = new System.Drawing.Size(81, 23);
            this.btnCustom2.TabIndex = 13;
            this.btnCustom2.Text = "Play Custom2";
            this.toolTip1.SetToolTip(this.btnCustom2, "Play Sound on your console\r\nJR-P must be connected and console\r\nin standby!");
            this.btnCustom2.UseVisualStyleBackColor = true;
            this.btnCustom2.Click += new System.EventHandler(this.btnCustom2_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnShutdownPlay);
            this.groupBox2.Controls.Add(this.btnEjectPlay);
            this.groupBox2.Controls.Add(this.btnStartupPlay);
            this.groupBox2.Controls.Add(this.btnConvert);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.btnPowerOpen);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtOutput);
            this.groupBox2.Controls.Add(this.txtFinalbin);
            this.groupBox2.Controls.Add(this.txtPower);
            this.groupBox2.Controls.Add(this.btnEjectOpen);
            this.groupBox2.Controls.Add(this.btnStartOpen);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnStart);
            this.groupBox2.Controls.Add(this.txtEject);
            this.groupBox2.Controls.Add(this.txtStart);
            this.groupBox2.Location = new System.Drawing.Point(428, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(399, 320);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Create Custom File";
            // 
            // btnShutdownPlay
            // 
            this.btnShutdownPlay.Image = ((System.Drawing.Image)(resources.GetObject("btnShutdownPlay.Image")));
            this.btnShutdownPlay.Location = new System.Drawing.Point(366, 103);
            this.btnShutdownPlay.Name = "btnShutdownPlay";
            this.btnShutdownPlay.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnShutdownPlay.Size = new System.Drawing.Size(27, 30);
            this.btnShutdownPlay.TabIndex = 42;
            this.btnShutdownPlay.UseVisualStyleBackColor = true;
            this.btnShutdownPlay.Visible = false;
            this.btnShutdownPlay.Click += new System.EventHandler(this.btnShutdownPlay_Click);
            // 
            // btnEjectPlay
            // 
            this.btnEjectPlay.Image = ((System.Drawing.Image)(resources.GetObject("btnEjectPlay.Image")));
            this.btnEjectPlay.Location = new System.Drawing.Point(366, 63);
            this.btnEjectPlay.Name = "btnEjectPlay";
            this.btnEjectPlay.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnEjectPlay.Size = new System.Drawing.Size(27, 30);
            this.btnEjectPlay.TabIndex = 41;
            this.btnEjectPlay.UseVisualStyleBackColor = true;
            this.btnEjectPlay.Visible = false;
            this.btnEjectPlay.Click += new System.EventHandler(this.btnEjectPlay_Click);
            // 
            // btnStartupPlay
            // 
            this.btnStartupPlay.Image = ((System.Drawing.Image)(resources.GetObject("btnStartupPlay.Image")));
            this.btnStartupPlay.Location = new System.Drawing.Point(366, 24);
            this.btnStartupPlay.Name = "btnStartupPlay";
            this.btnStartupPlay.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnStartupPlay.Size = new System.Drawing.Size(27, 30);
            this.btnStartupPlay.TabIndex = 40;
            this.btnStartupPlay.UseVisualStyleBackColor = true;
            this.btnStartupPlay.Visible = false;
            this.btnStartupPlay.Click += new System.EventHandler(this.btnStartupPlay_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.Enabled = false;
            this.btnConvert.Location = new System.Drawing.Point(10, 262);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(88, 23);
            this.btnConvert.TabIndex = 39;
            this.btnConvert.Text = "Convert .wav";
            this.toolTip1.SetToolTip(this.btnConvert, "converts wav files so they are ready to be included in the final .bin");
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton1);
            this.groupBox3.Controls.Add(this.radioButton2);
            this.groupBox3.Controls.Add(this.radioButton3);
            this.groupBox3.Location = new System.Drawing.Point(10, 164);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(89, 92);
            this.groupBox3.TabIndex = 35;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sound Format";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(70, 17);
            this.radioButton1.TabIndex = 9;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "2 Sounds";
            this.toolTip1.SetToolTip(this.radioButton1, "Standard 2 sound file");
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 42);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(82, 17);
            this.radioButton2.TabIndex = 10;
            this.radioButton2.Text = "+ Shutdown";
            this.toolTip1.SetToolTip(this.radioButton2, "3 sounds file\r\nStartup, Eject, Shutdown");
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(6, 65);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(61, 17);
            this.radioButton3.TabIndex = 11;
            this.radioButton3.Text = "+ Glitch";
            this.toolTip1.SetToolTip(this.radioButton3, "3 sounds file\r\nStartup, Eject, Glitch");
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // btnPowerOpen
            // 
            this.btnPowerOpen.Enabled = false;
            this.btnPowerOpen.Location = new System.Drawing.Point(332, 107);
            this.btnPowerOpen.Name = "btnPowerOpen";
            this.btnPowerOpen.Size = new System.Drawing.Size(28, 23);
            this.btnPowerOpen.TabIndex = 34;
            this.btnPowerOpen.Text = "...";
            this.btnPowerOpen.UseVisualStyleBackColor = true;
            this.btnPowerOpen.Click += new System.EventHandler(this.btnPowerOpen_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "Final Output.bin name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(3, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 33;
            this.label4.Text = "Shutdown sound ";
            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.Color.Black;
            this.txtOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOutput.ForeColor = System.Drawing.Color.White;
            this.txtOutput.Location = new System.Drawing.Point(103, 164);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(276, 149);
            this.txtOutput.TabIndex = 31;
            this.toolTip1.SetToolTip(this.txtOutput, "Log window to see whats going on");
            // 
            // txtFinalbin
            // 
            this.txtFinalbin.Location = new System.Drawing.Point(122, 138);
            this.txtFinalbin.Name = "txtFinalbin";
            this.txtFinalbin.Size = new System.Drawing.Size(100, 20);
            this.txtFinalbin.TabIndex = 37;
            this.txtFinalbin.Text = "Custom";
            this.toolTip1.SetToolTip(this.txtFinalbin, "The name given to your .bin file upon creation");
            // 
            // txtPower
            // 
            this.txtPower.AllowDrop = true;
            this.txtPower.Enabled = false;
            this.txtPower.Location = new System.Drawing.Point(6, 109);
            this.txtPower.Name = "txtPower";
            this.txtPower.Size = new System.Drawing.Size(320, 20);
            this.txtPower.TabIndex = 32;
            this.toolTip1.SetToolTip(this.txtPower, "The .bin file for Shutdown/Glitch Sound");
            this.txtPower.TextChanged += new System.EventHandler(this.txtPower_TextChanged);
            this.txtPower.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtPower_DragDrop);
            this.txtPower.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtPower_DragEnter);
            // 
            // btnEjectOpen
            // 
            this.btnEjectOpen.Location = new System.Drawing.Point(332, 67);
            this.btnEjectOpen.Name = "btnEjectOpen";
            this.btnEjectOpen.Size = new System.Drawing.Size(28, 23);
            this.btnEjectOpen.TabIndex = 30;
            this.btnEjectOpen.Text = "...";
            this.btnEjectOpen.UseVisualStyleBackColor = true;
            this.btnEjectOpen.Click += new System.EventHandler(this.btnEjectOpen_Click);
            // 
            // btnStartOpen
            // 
            this.btnStartOpen.Location = new System.Drawing.Point(332, 28);
            this.btnStartOpen.Name = "btnStartOpen";
            this.btnStartOpen.Size = new System.Drawing.Size(28, 23);
            this.btnStartOpen.TabIndex = 29;
            this.btnStartOpen.Text = "...";
            this.btnStartOpen.UseVisualStyleBackColor = true;
            this.btnStartOpen.Click += new System.EventHandler(this.btnStartOpen_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Eject Sound ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Startup sound ";
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(10, 291);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(88, 23);
            this.btnStart.TabIndex = 26;
            this.btnStart.Text = "Create  .bin";
            this.toolTip1.SetToolTip(this.btnStart, "creates a final sound.bin file ready \r\nfor writing to the console");
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtEject
            // 
            this.txtEject.AllowDrop = true;
            this.txtEject.Location = new System.Drawing.Point(6, 69);
            this.txtEject.Name = "txtEject";
            this.txtEject.Size = new System.Drawing.Size(320, 20);
            this.txtEject.TabIndex = 25;
            this.toolTip1.SetToolTip(this.txtEject, "The .bin file for Eject Sound");
            this.txtEject.TextChanged += new System.EventHandler(this.txtEject_TextChanged);
            this.txtEject.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtEject_DragDrop);
            this.txtEject.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtEject_DragEnter);
            // 
            // txtStart
            // 
            this.txtStart.AllowDrop = true;
            this.txtStart.Location = new System.Drawing.Point(6, 30);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(320, 20);
            this.txtStart.TabIndex = 24;
            this.toolTip1.SetToolTip(this.txtStart, "The .bin file for Startup Sound");
            this.txtStart.TextChanged += new System.EventHandler(this.txtStart_TextChanged);
            this.txtStart.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtStart_DragDrop);
            this.txtStart.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtStart_DragEnter);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnPlayPower);
            this.groupBox4.Controls.Add(this.btnPlayEject);
            this.groupBox4.Location = new System.Drawing.Point(166, 14);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(94, 78);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Dual Sound";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnCustom2);
            this.groupBox5.Controls.Add(this.btnCustom1);
            this.groupBox5.Controls.Add(this.btnCustom3);
            this.groupBox5.Enabled = false;
            this.groupBox5.Location = new System.Drawing.Point(166, 98);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(94, 104);
            this.groupBox5.TabIndex = 39;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Triple Sound";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnWrite);
            this.groupBox6.Controls.Add(this.groupBox5);
            this.groupBox6.Controls.Add(this.btnRead);
            this.groupBox6.Controls.Add(this.progressBar1);
            this.groupBox6.Controls.Add(this.groupBox4);
            this.groupBox6.Controls.Add(this.btnFile);
            this.groupBox6.Controls.Add(this.txtFile);
            this.groupBox6.Controls.Add(this.btnVerify);
            this.groupBox6.Controls.Add(this.veraftreadchk);
            this.groupBox6.Location = new System.Drawing.Point(5, 75);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(273, 253);
            this.groupBox6.TabIndex = 40;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Console Controls";
            // 
            // SoundEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 331);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SoundEditor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SoundEditor";
            this.Load += new System.EventHandler(this.SoundEditor_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SoundEditor_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtnOther;
        private System.Windows.Forms.RadioButton rbtnStandard;
        private System.Windows.Forms.RadioButton rbtnHalo;
        private System.Windows.Forms.RadioButton rbtnStarWars;
        private System.Windows.Forms.RadioButton rbtnMW3;
        private System.Windows.Forms.RadioButton rbtnGOW3;
        private System.Windows.Forms.RadioButton rbtnHalo4;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnPlayPower;
        private System.Windows.Forms.Button btnPlayEject;
        private System.Windows.Forms.CheckBox veraftreadchk;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnCustom1;
        private System.Windows.Forms.Button btnCustom3;
        private System.Windows.Forms.Button btnCustom2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Button btnPowerOpen;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFinalbin;
        private System.Windows.Forms.TextBox txtPower;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnEjectOpen;
        private System.Windows.Forms.Button btnStartOpen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtEject;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnStartupPlay;
        private System.Windows.Forms.Button btnShutdownPlay;
        private System.Windows.Forms.Button btnEjectPlay;
    }
}
