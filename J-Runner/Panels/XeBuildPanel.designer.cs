﻿namespace JRunner.Panels
{
    partial class XeBuildPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.MainTabs = new System.Windows.Forms.TabControl();
            this.tabXeBuild = new System.Windows.Forms.TabPage();
            this.Rgh3Label2 = new System.Windows.Forms.Label();
            this.Rgh3Label = new System.Windows.Forms.Label();
            this.Rgh3Mhz = new System.Windows.Forms.ComboBox();
            this.chkRgh3 = new System.Windows.Forms.CheckBox();
            this.chkWB = new System.Windows.Forms.CheckBox();
            this.chkCR4 = new System.Windows.Forms.CheckBox();
            this.chkCleanSMC = new System.Windows.Forms.CheckBox();
            this.chkXdkBuild = new System.Windows.Forms.CheckBox();
            this.chk0Fuse = new System.Windows.Forms.CheckBox();
            this.rbtnDevGL = new System.Windows.Forms.RadioButton();
            this.chkSMCP = new System.Windows.Forms.CheckBox();
            this.labelCB = new System.Windows.Forms.Label();
            this.comboCB = new System.Windows.Forms.ComboBox();
            this.rbtnGlitch2m = new System.Windows.Forms.RadioButton();
            this.btnGetMB = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDashVersion = new System.Windows.Forms.Label();
            this.chkAudClamp = new System.Windows.Forms.CheckBox();
            this.comboDash = new System.Windows.Forms.ComboBox();
            this.dashBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dashDataSet = new JRunner.DataSet1();
            this.chkRJtag = new System.Windows.Forms.CheckBox();
            this.txtMBname = new System.Windows.Forms.TextBox();
            this.rbtnRetail = new System.Windows.Forms.RadioButton();
            this.rbtnJtag = new System.Windows.Forms.RadioButton();
            this.rbtnGlitch2 = new System.Windows.Forms.RadioButton();
            this.rbtnGlitch = new System.Windows.Forms.RadioButton();
            this.tabPatches = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkCoronaKeyFix = new System.Windows.Forms.CheckBox();
            this.chkUsbdSec = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkXLBoth = new System.Windows.Forms.CheckBox();
            this.chkXLHdd = new System.Windows.Forms.CheckBox();
            this.chkXLUsb = new System.Windows.Forms.CheckBox();
            this.chkWB4G = new System.Windows.Forms.CheckBox();
            this.chkListBoxPatches = new System.Windows.Forms.CheckedListBox();
            this.tabOptions = new System.Windows.Forms.TabPage();
            this.btnShowAdvanced = new System.Windows.Forms.Button();
            this.btnXeBuildOptions = new System.Windows.Forms.Button();
            this.chkBigffs = new System.Windows.Forms.CheckBox();
            this.checkDLPatches = new System.Windows.Forms.CheckBox();
            this.btnLaunch = new System.Windows.Forms.Button();
            this.chkXeSettings = new System.Windows.Forms.CheckBox();
            this.chkLaunch = new System.Windows.Forms.CheckBox();
            this.tabClient = new System.Windows.Forms.TabPage();
            this.btnInfo = new System.Windows.Forms.Button();
            this.chkForceIP2 = new System.Windows.Forms.CheckBox();
            this.txtIP2 = new System.Windows.Forms.TextBox();
            this.lblLength = new System.Windows.Forms.Label();
            this.lblOffset = new System.Windows.Forms.Label();
            this.btnErase = new System.Windows.Forms.Button();
            this.txtOffset = new System.Windows.Forms.TextBox();
            this.txtLength = new System.Windows.Forms.TextBox();
            this.btnPatches = new System.Windows.Forms.Button();
            this.btnComp = new System.Windows.Forms.Button();
            this.btnAvatar = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.chkReboot = new System.Windows.Forms.CheckBox();
            this.chkShutdown = new System.Windows.Forms.CheckBox();
            this.tabUpdate = new System.Windows.Forms.TabPage();
            this.lblDash = new System.Windows.Forms.Label();
            this.lblD = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.chkForceIP = new System.Windows.Forms.CheckBox();
            this.chkNoReeb = new System.Windows.Forms.CheckBox();
            this.chkClean = new System.Windows.Forms.CheckBox();
            this.chkNoAva = new System.Windows.Forms.CheckBox();
            this.chkNoWrite = new System.Windows.Forms.CheckBox();
            this.btnXEUpdate = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox7.SuspendLayout();
            this.MainTabs.SuspendLayout();
            this.tabXeBuild.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dashBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dashDataSet)).BeginInit();
            this.tabPatches.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.tabClient.SuspendLayout();
            this.tabUpdate.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.MainTabs);
            this.groupBox7.Location = new System.Drawing.Point(0, 1);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(341, 156);
            this.groupBox7.TabIndex = 78;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "XeBuild";
            // 
            // MainTabs
            // 
            this.MainTabs.Controls.Add(this.tabXeBuild);
            this.MainTabs.Controls.Add(this.tabPatches);
            this.MainTabs.Controls.Add(this.tabOptions);
            this.MainTabs.Controls.Add(this.tabClient);
            this.MainTabs.Controls.Add(this.tabUpdate);
            this.MainTabs.Location = new System.Drawing.Point(6, 14);
            this.MainTabs.Name = "MainTabs";
            this.MainTabs.SelectedIndex = 0;
            this.MainTabs.Size = new System.Drawing.Size(331, 136);
            this.MainTabs.TabIndex = 0;
            // 
            // tabXeBuild
            // 
            this.tabXeBuild.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tabXeBuild.Controls.Add(this.Rgh3Label2);
            this.tabXeBuild.Controls.Add(this.Rgh3Label);
            this.tabXeBuild.Controls.Add(this.Rgh3Mhz);
            this.tabXeBuild.Controls.Add(this.chkRgh3);
            this.tabXeBuild.Controls.Add(this.chkWB);
            this.tabXeBuild.Controls.Add(this.chkCR4);
            this.tabXeBuild.Controls.Add(this.chkCleanSMC);
            this.tabXeBuild.Controls.Add(this.chkXdkBuild);
            this.tabXeBuild.Controls.Add(this.chk0Fuse);
            this.tabXeBuild.Controls.Add(this.rbtnDevGL);
            this.tabXeBuild.Controls.Add(this.chkSMCP);
            this.tabXeBuild.Controls.Add(this.labelCB);
            this.tabXeBuild.Controls.Add(this.comboCB);
            this.tabXeBuild.Controls.Add(this.rbtnGlitch2m);
            this.tabXeBuild.Controls.Add(this.btnGetMB);
            this.tabXeBuild.Controls.Add(this.label2);
            this.tabXeBuild.Controls.Add(this.lblDashVersion);
            this.tabXeBuild.Controls.Add(this.chkAudClamp);
            this.tabXeBuild.Controls.Add(this.comboDash);
            this.tabXeBuild.Controls.Add(this.chkRJtag);
            this.tabXeBuild.Controls.Add(this.txtMBname);
            this.tabXeBuild.Controls.Add(this.rbtnRetail);
            this.tabXeBuild.Controls.Add(this.rbtnJtag);
            this.tabXeBuild.Controls.Add(this.rbtnGlitch2);
            this.tabXeBuild.Controls.Add(this.rbtnGlitch);
            this.tabXeBuild.Location = new System.Drawing.Point(4, 22);
            this.tabXeBuild.Name = "tabXeBuild";
            this.tabXeBuild.Padding = new System.Windows.Forms.Padding(3);
            this.tabXeBuild.Size = new System.Drawing.Size(323, 110);
            this.tabXeBuild.TabIndex = 0;
            this.tabXeBuild.Text = "Home";
            // 
            // Rgh3Label2
            // 
            this.Rgh3Label2.AutoSize = true;
            this.Rgh3Label2.Location = new System.Drawing.Point(286, 64);
            this.Rgh3Label2.Name = "Rgh3Label2";
            this.Rgh3Label2.Size = new System.Drawing.Size(19, 13);
            this.Rgh3Label2.TabIndex = 21;
            this.Rgh3Label2.Text = "27";
            this.Rgh3Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Rgh3Label
            // 
            this.Rgh3Label.AutoSize = true;
            this.Rgh3Label.Location = new System.Drawing.Point(281, 47);
            this.Rgh3Label.Name = "Rgh3Label";
            this.Rgh3Label.Size = new System.Drawing.Size(32, 13);
            this.Rgh3Label.TabIndex = 99;
            this.Rgh3Label.Text = "MHz:";
            // 
            // Rgh3Mhz
            // 
            this.Rgh3Mhz.FormattingEnabled = true;
            this.Rgh3Mhz.Items.AddRange(new object[] {
            "10",
            "27"});
            this.Rgh3Mhz.Location = new System.Drawing.Point(284, 61);
            this.Rgh3Mhz.Name = "Rgh3Mhz";
            this.Rgh3Mhz.Size = new System.Drawing.Size(36, 21);
            this.Rgh3Mhz.TabIndex = 20;
            this.Rgh3Mhz.Text = "27";
            this.toolTip1.SetToolTip(this.Rgh3Mhz, "Sets the I2C slowdown type");
            // 
            // chkRgh3
            // 
            this.chkRgh3.AutoSize = true;
            this.chkRgh3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkRgh3.Location = new System.Drawing.Point(229, 63);
            this.chkRgh3.Name = "chkRgh3";
            this.chkRgh3.Size = new System.Drawing.Size(56, 17);
            this.chkRgh3.TabIndex = 19;
            this.chkRgh3.Text = "RGH3";
            this.toolTip1.SetToolTip(this.chkRgh3, "Uses the SMC to glitch the console");
            this.chkRgh3.UseVisualStyleBackColor = true;
            this.chkRgh3.CheckedChanged += new System.EventHandler(this.chkRgh3_CheckedChanged);
            // 
            // chkWB
            // 
            this.chkWB.AutoSize = true;
            this.chkWB.BackColor = System.Drawing.Color.Transparent;
            this.chkWB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkWB.Location = new System.Drawing.Point(229, 5);
            this.chkWB.Name = "chkWB";
            this.chkWB.Size = new System.Drawing.Size(60, 17);
            this.chkWB.TabIndex = 14;
            this.chkWB.Text = "WB 2K";
            this.toolTip1.SetToolTip(this.chkWB, "Corona only, use 13182 CBB for Winbond 2K consoles");
            this.chkWB.UseVisualStyleBackColor = false;
            this.chkWB.CheckedChanged += new System.EventHandler(this.chkWB_CheckedChanged);
            // 
            // chkCR4
            // 
            this.chkCR4.AutoSize = true;
            this.chkCR4.Location = new System.Drawing.Point(229, 31);
            this.chkCR4.Name = "chkCR4";
            this.chkCR4.Size = new System.Drawing.Size(47, 17);
            this.chkCR4.TabIndex = 17;
            this.chkCR4.Text = "CR4";
            this.toolTip1.SetToolTip(this.chkCR4, "Speeds up glitch timouts and allows use of the TX CR4 RGH2+ method\r\n");
            this.chkCR4.UseVisualStyleBackColor = true;
            this.chkCR4.CheckedChanged += new System.EventHandler(this.chkCR4_CheckedChanged);
            // 
            // chkCleanSMC
            // 
            this.chkCleanSMC.AutoSize = true;
            this.chkCleanSMC.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkCleanSMC.Location = new System.Drawing.Point(229, 90);
            this.chkCleanSMC.Name = "chkCleanSMC";
            this.chkCleanSMC.Size = new System.Drawing.Size(79, 17);
            this.chkCleanSMC.TabIndex = 24;
            this.chkCleanSMC.Text = "Clean SMC";
            this.toolTip1.SetToolTip(this.chkCleanSMC, "Use a clean retail SMC");
            this.chkCleanSMC.UseVisualStyleBackColor = true;
            this.chkCleanSMC.CheckedChanged += new System.EventHandler(this.chkCleanSMC_CheckedChanged);
            // 
            // chkXdkBuild
            // 
            this.chkXdkBuild.AutoSize = true;
            this.chkXdkBuild.Enabled = false;
            this.chkXdkBuild.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkXdkBuild.Location = new System.Drawing.Point(229, 5);
            this.chkXdkBuild.Name = "chkXdkBuild";
            this.chkXdkBuild.Size = new System.Drawing.Size(70, 17);
            this.chkXdkBuild.TabIndex = 15;
            this.chkXdkBuild.Text = "XDKbuild";
            this.toolTip1.SetToolTip(this.chkXdkBuild, "Builds XDK filesystem for booting on an RGH, requires custom timing files located" +
        " in the common folder");
            this.chkXdkBuild.UseVisualStyleBackColor = true;
            this.chkXdkBuild.CheckedChanged += new System.EventHandler(this.chkXdkBuild_CheckedChanged);
            // 
            // chk0Fuse
            // 
            this.chk0Fuse.AutoSize = true;
            this.chk0Fuse.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chk0Fuse.Location = new System.Drawing.Point(229, 74);
            this.chk0Fuse.Name = "chk0Fuse";
            this.chk0Fuse.Size = new System.Drawing.Size(58, 17);
            this.chk0Fuse.TabIndex = 22;
            this.chk0Fuse.Text = "0 Fuse";
            this.toolTip1.SetToolTip(this.chk0Fuse, "Check if building for a 0 Fuse console");
            this.chk0Fuse.UseVisualStyleBackColor = true;
            this.chk0Fuse.CheckedChanged += new System.EventHandler(this.chk0Fuse_CheckedChanged);
            // 
            // rbtnDevGL
            // 
            this.rbtnDevGL.AutoSize = true;
            this.rbtnDevGL.BackColor = System.Drawing.Color.Transparent;
            this.rbtnDevGL.ForeColor = System.Drawing.Color.Blue;
            this.rbtnDevGL.Location = new System.Drawing.Point(154, 89);
            this.rbtnDevGL.Name = "rbtnDevGL";
            this.rbtnDevGL.Size = new System.Drawing.Size(61, 17);
            this.rbtnDevGL.TabIndex = 13;
            this.rbtnDevGL.Text = "DEVGL";
            this.rbtnDevGL.UseVisualStyleBackColor = false;
            this.rbtnDevGL.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // chkSMCP
            // 
            this.chkSMCP.AutoSize = true;
            this.chkSMCP.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkSMCP.Location = new System.Drawing.Point(229, 47);
            this.chkSMCP.Name = "chkSMCP";
            this.chkSMCP.Size = new System.Drawing.Size(55, 17);
            this.chkSMCP.TabIndex = 18;
            this.chkSMCP.Text = "SMC+";
            this.toolTip1.SetToolTip(this.chkSMCP, "Increases the glitch timeout to the maximum possible, recommended for RGH1.2 and " +
        "S-RGH");
            this.chkSMCP.UseVisualStyleBackColor = true;
            this.chkSMCP.CheckedChanged += new System.EventHandler(this.chkSMCP_CheckedChanged);
            // 
            // labelCB
            // 
            this.labelCB.AutoSize = true;
            this.labelCB.Location = new System.Drawing.Point(226, 7);
            this.labelCB.Name = "labelCB";
            this.labelCB.Size = new System.Drawing.Size(21, 13);
            this.labelCB.TabIndex = 88;
            this.labelCB.Text = "CB";
            // 
            // comboCB
            // 
            this.comboCB.FormattingEnabled = true;
            this.comboCB.Location = new System.Drawing.Point(247, 3);
            this.comboCB.Name = "comboCB";
            this.comboCB.Size = new System.Drawing.Size(73, 21);
            this.comboCB.TabIndex = 16;
            this.comboCB.SelectedIndexChanged += new System.EventHandler(this.comboCB_SelectedIndexChanged);
            // 
            // rbtnGlitch2m
            // 
            this.rbtnGlitch2m.AutoSize = true;
            this.rbtnGlitch2m.BackColor = System.Drawing.Color.Transparent;
            this.rbtnGlitch2m.ForeColor = System.Drawing.Color.Blue;
            this.rbtnGlitch2m.Location = new System.Drawing.Point(154, 55);
            this.rbtnGlitch2m.Name = "rbtnGlitch2m";
            this.rbtnGlitch2m.Size = new System.Drawing.Size(66, 17);
            this.rbtnGlitch2m.TabIndex = 11;
            this.rbtnGlitch2m.Text = "Glitch2m";
            this.rbtnGlitch2m.UseVisualStyleBackColor = false;
            this.rbtnGlitch2m.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // btnGetMB
            // 
            this.btnGetMB.Location = new System.Drawing.Point(115, 73);
            this.btnGetMB.Name = "btnGetMB";
            this.btnGetMB.Size = new System.Drawing.Size(25, 22);
            this.btnGetMB.TabIndex = 7;
            this.btnGetMB.Text = "?";
            this.toolTip1.SetToolTip(this.btnGetMB, "Query the console to get the type");
            this.btnGetMB.UseVisualStyleBackColor = true;
            this.btnGetMB.Click += new System.EventHandler(this.btnGetMB_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 72;
            this.label2.Text = "Console Type";
            // 
            // lblDashVersion
            // 
            this.lblDashVersion.AutoSize = true;
            this.lblDashVersion.Location = new System.Drawing.Point(13, 13);
            this.lblDashVersion.Name = "lblDashVersion";
            this.lblDashVersion.Size = new System.Drawing.Size(75, 13);
            this.lblDashVersion.TabIndex = 64;
            this.lblDashVersion.Text = "Kernel Version";
            // 
            // chkAudClamp
            // 
            this.chkAudClamp.AutoSize = true;
            this.chkAudClamp.BackColor = System.Drawing.Color.Transparent;
            this.chkAudClamp.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkAudClamp.Location = new System.Drawing.Point(229, 65);
            this.chkAudClamp.Name = "chkAudClamp";
            this.chkAudClamp.Size = new System.Drawing.Size(80, 17);
            this.chkAudClamp.TabIndex = 21;
            this.chkAudClamp.Text = "Aud_Clamp";
            this.toolTip1.SetToolTip(this.chkAudClamp, "Check if using Aud_Clamp wiring on HDMI Phats");
            this.chkAudClamp.UseVisualStyleBackColor = false;
            this.chkAudClamp.CheckedChanged += new System.EventHandler(this.chkAudClamp_CheckedChanged);
            // 
            // comboDash
            // 
            this.comboDash.DataSource = this.dashBindingSource;
            this.comboDash.DisplayMember = "Dash";
            this.comboDash.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboDash.FormattingEnabled = true;
            this.comboDash.Location = new System.Drawing.Point(16, 28);
            this.comboDash.Name = "comboDash";
            this.comboDash.Size = new System.Drawing.Size(71, 21);
            this.comboDash.TabIndex = 5;
            this.toolTip1.SetToolTip(this.comboDash, "Select the kernel/dashboard version used for XeBuild");
            this.comboDash.ValueMember = "Dash";
            this.comboDash.SelectedIndexChanged += new System.EventHandler(this.comboDash_SelectedIndexChanged);
            // 
            // dashBindingSource
            // 
            this.dashBindingSource.DataMember = "DataTable2";
            this.dashBindingSource.DataSource = this.dashDataSet;
            // 
            // dashDataSet
            // 
            this.dashDataSet.DataSetName = "DashDataSet";
            this.dashDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // chkRJtag
            // 
            this.chkRJtag.AutoSize = true;
            this.chkRJtag.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkRJtag.Location = new System.Drawing.Point(229, 81);
            this.chkRJtag.Name = "chkRJtag";
            this.chkRJtag.Size = new System.Drawing.Size(64, 17);
            this.chkRJtag.TabIndex = 23;
            this.chkRJtag.Text = "R-JTAG";
            this.toolTip1.SetToolTip(this.chkRJtag, "Check if using R-JTAG/R-JTOP methods");
            this.chkRJtag.UseVisualStyleBackColor = true;
            this.chkRJtag.CheckedChanged += new System.EventHandler(this.chkRJtag_CheckedChanged);
            // 
            // txtMBname
            // 
            this.txtMBname.BackColor = System.Drawing.SystemColors.Control;
            this.txtMBname.Location = new System.Drawing.Point(16, 74);
            this.txtMBname.Name = "txtMBname";
            this.txtMBname.ReadOnly = true;
            this.txtMBname.Size = new System.Drawing.Size(96, 20);
            this.txtMBname.TabIndex = 6;
            this.txtMBname.Text = "None Selected";
            this.txtMBname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtMBname, "Click to set the console type for XeBuild");
            this.txtMBname.Click += new System.EventHandler(this.txtMBname_Click);
            this.txtMBname.TextChanged += new System.EventHandler(this.txtMBname_TextChanged);
            // 
            // rbtnRetail
            // 
            this.rbtnRetail.AutoSize = true;
            this.rbtnRetail.BackColor = System.Drawing.Color.Transparent;
            this.rbtnRetail.Checked = true;
            this.rbtnRetail.ForeColor = System.Drawing.Color.Blue;
            this.rbtnRetail.Location = new System.Drawing.Point(154, 4);
            this.rbtnRetail.Name = "rbtnRetail";
            this.rbtnRetail.Size = new System.Drawing.Size(52, 17);
            this.rbtnRetail.TabIndex = 8;
            this.rbtnRetail.TabStop = true;
            this.rbtnRetail.Text = "Retail";
            this.rbtnRetail.UseVisualStyleBackColor = false;
            this.rbtnRetail.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // rbtnJtag
            // 
            this.rbtnJtag.AutoSize = true;
            this.rbtnJtag.BackColor = System.Drawing.Color.Transparent;
            this.rbtnJtag.ForeColor = System.Drawing.Color.Blue;
            this.rbtnJtag.Location = new System.Drawing.Point(154, 72);
            this.rbtnJtag.Name = "rbtnJtag";
            this.rbtnJtag.Size = new System.Drawing.Size(52, 17);
            this.rbtnJtag.TabIndex = 12;
            this.rbtnJtag.Text = "JTAG";
            this.rbtnJtag.UseVisualStyleBackColor = false;
            this.rbtnJtag.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // rbtnGlitch2
            // 
            this.rbtnGlitch2.AutoSize = true;
            this.rbtnGlitch2.BackColor = System.Drawing.Color.Transparent;
            this.rbtnGlitch2.ForeColor = System.Drawing.Color.Blue;
            this.rbtnGlitch2.Location = new System.Drawing.Point(154, 38);
            this.rbtnGlitch2.Name = "rbtnGlitch2";
            this.rbtnGlitch2.Size = new System.Drawing.Size(58, 17);
            this.rbtnGlitch2.TabIndex = 10;
            this.rbtnGlitch2.Text = "Glitch2";
            this.rbtnGlitch2.UseVisualStyleBackColor = false;
            this.rbtnGlitch2.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // rbtnGlitch
            // 
            this.rbtnGlitch.AutoSize = true;
            this.rbtnGlitch.BackColor = System.Drawing.Color.Transparent;
            this.rbtnGlitch.ForeColor = System.Drawing.Color.Blue;
            this.rbtnGlitch.Location = new System.Drawing.Point(154, 21);
            this.rbtnGlitch.Name = "rbtnGlitch";
            this.rbtnGlitch.Size = new System.Drawing.Size(52, 17);
            this.rbtnGlitch.TabIndex = 9;
            this.rbtnGlitch.Text = "Glitch";
            this.rbtnGlitch.UseVisualStyleBackColor = false;
            this.rbtnGlitch.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // tabPatches
            // 
            this.tabPatches.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tabPatches.Controls.Add(this.groupBox2);
            this.tabPatches.Controls.Add(this.groupBox1);
            this.tabPatches.Controls.Add(this.chkWB4G);
            this.tabPatches.Controls.Add(this.chkListBoxPatches);
            this.tabPatches.Location = new System.Drawing.Point(4, 22);
            this.tabPatches.Name = "tabPatches";
            this.tabPatches.Padding = new System.Windows.Forms.Padding(3);
            this.tabPatches.Size = new System.Drawing.Size(323, 110);
            this.tabPatches.TabIndex = 3;
            this.tabPatches.Text = "Patches";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkCoronaKeyFix);
            this.groupBox2.Controls.Add(this.chkUsbdSec);
            this.groupBox2.Location = new System.Drawing.Point(210, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(104, 61);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Other Patches";
            // 
            // chkCoronaKeyFix
            // 
            this.chkCoronaKeyFix.AutoSize = true;
            this.chkCoronaKeyFix.Enabled = false;
            this.chkCoronaKeyFix.Location = new System.Drawing.Point(21, 37);
            this.chkCoronaKeyFix.Name = "chkCoronaKeyFix";
            this.chkCoronaKeyFix.Size = new System.Drawing.Size(60, 17);
            this.chkCoronaKeyFix.TabIndex = 1;
            this.chkCoronaKeyFix.Text = "Key Fix";
            this.toolTip1.SetToolTip(this.chkCoronaKeyFix, "Patch Freeboot to fix an issue on Corona that may affect certain games");
            this.chkCoronaKeyFix.UseVisualStyleBackColor = true;
            this.chkCoronaKeyFix.CheckedChanged += new System.EventHandler(this.chkCoronaKeyFix_CheckedChanged);
            // 
            // chkUsbdSec
            // 
            this.chkUsbdSec.AutoSize = true;
            this.chkUsbdSec.Enabled = false;
            this.chkUsbdSec.Location = new System.Drawing.Point(21, 18);
            this.chkUsbdSec.Name = "chkUsbdSec";
            this.chkUsbdSec.Size = new System.Drawing.Size(70, 17);
            this.chkUsbdSec.TabIndex = 0;
            this.chkUsbdSec.Text = "UsbdSec";
            this.toolTip1.SetToolTip(this.chkUsbdSec, "Patch Freeboot to allow use of custom USB peripherals, like controllers");
            this.chkUsbdSec.UseVisualStyleBackColor = true;
            this.chkUsbdSec.CheckedChanged += new System.EventHandler(this.chkUsbdSec_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkXLBoth);
            this.groupBox1.Controls.Add(this.chkXLHdd);
            this.groupBox1.Controls.Add(this.chkXLUsb);
            this.groupBox1.Location = new System.Drawing.Point(97, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(107, 80);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Drive Patches";
            // 
            // chkXLBoth
            // 
            this.chkXLBoth.AutoSize = true;
            this.chkXLBoth.Enabled = false;
            this.chkXLBoth.Location = new System.Drawing.Point(24, 56);
            this.chkXLBoth.Name = "chkXLBoth";
            this.chkXLBoth.Size = new System.Drawing.Size(64, 17);
            this.chkXLBoth.TabIndex = 2;
            this.chkXLBoth.Text = "Both XL";
            this.toolTip1.SetToolTip(this.chkXLBoth, "Patch Freeboot to allow use of both USB and internal hard drives over 2TB in size" +
        " \r\n");
            this.chkXLBoth.UseVisualStyleBackColor = true;
            this.chkXLBoth.CheckedChanged += new System.EventHandler(this.chkXLBoth_CheckedChanged);
            // 
            // chkXLHdd
            // 
            this.chkXLHdd.AutoSize = true;
            this.chkXLHdd.Enabled = false;
            this.chkXLHdd.Location = new System.Drawing.Point(24, 37);
            this.chkXLHdd.Name = "chkXLHdd";
            this.chkXLHdd.Size = new System.Drawing.Size(66, 17);
            this.chkXLHdd.TabIndex = 1;
            this.chkXLHdd.Text = "XL HDD";
            this.toolTip1.SetToolTip(this.chkXLHdd, "Patch Freeboot to allow use of internal hard drives over 2TB in size \r\n");
            this.chkXLHdd.UseVisualStyleBackColor = true;
            this.chkXLHdd.CheckedChanged += new System.EventHandler(this.chkXLHdd_CheckedChanged);
            // 
            // chkXLUsb
            // 
            this.chkXLUsb.AutoSize = true;
            this.chkXLUsb.Enabled = false;
            this.chkXLUsb.Location = new System.Drawing.Point(24, 18);
            this.chkXLUsb.Name = "chkXLUsb";
            this.chkXLUsb.Size = new System.Drawing.Size(64, 17);
            this.chkXLUsb.TabIndex = 0;
            this.chkXLUsb.Text = "XL USB";
            this.toolTip1.SetToolTip(this.chkXLUsb, "Patch Freeboot to allow use of USB hard drives over 2TB in size ");
            this.chkXLUsb.UseVisualStyleBackColor = true;
            this.chkXLUsb.CheckedChanged += new System.EventHandler(this.chkXLUsb_CheckedChanged);
            // 
            // chkWB4G
            // 
            this.chkWB4G.AutoSize = true;
            this.chkWB4G.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkWB4G.Location = new System.Drawing.Point(97, 89);
            this.chkWB4G.Name = "chkWB4G";
            this.chkWB4G.Size = new System.Drawing.Size(114, 17);
            this.chkWB4G.TabIndex = 7;
            this.chkWB4G.Text = "WB 2K Buffer (4G)";
            this.toolTip1.SetToolTip(this.chkWB4G, "Corona only, use 13182 CBB with a buffer for Winbond 2K consoles\r\nLegacy option t" +
        "hat should not be used except in rare cases");
            this.chkWB4G.UseVisualStyleBackColor = true;
            this.chkWB4G.CheckedChanged += new System.EventHandler(this.chkWB4G_CheckedChanged);
            // 
            // chkListBoxPatches
            // 
            this.chkListBoxPatches.BackColor = System.Drawing.SystemColors.Control;
            this.chkListBoxPatches.CheckOnClick = true;
            this.chkListBoxPatches.FormattingEnabled = true;
            this.chkListBoxPatches.Items.AddRange(new object[] {
            "nofcrt",
            "noSShdd",
            "nointmu",
            "nohdd",
            "nohdmiwait",
            "nolan"});
            this.chkListBoxPatches.Location = new System.Drawing.Point(8, 9);
            this.chkListBoxPatches.Name = "chkListBoxPatches";
            this.chkListBoxPatches.Size = new System.Drawing.Size(83, 94);
            this.chkListBoxPatches.TabIndex = 5;
            this.toolTip1.SetToolTip(this.chkListBoxPatches, "XeBuild Patches which can be selected");
            this.chkListBoxPatches.SelectedIndexChanged += new System.EventHandler(this.chkListBoxPatches_SelectedIndexChanged);
            // 
            // tabOptions
            // 
            this.tabOptions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tabOptions.Controls.Add(this.btnShowAdvanced);
            this.tabOptions.Controls.Add(this.btnXeBuildOptions);
            this.tabOptions.Controls.Add(this.chkBigffs);
            this.tabOptions.Controls.Add(this.checkDLPatches);
            this.tabOptions.Controls.Add(this.btnLaunch);
            this.tabOptions.Controls.Add(this.chkXeSettings);
            this.tabOptions.Controls.Add(this.chkLaunch);
            this.tabOptions.Location = new System.Drawing.Point(4, 22);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptions.Size = new System.Drawing.Size(323, 110);
            this.tabOptions.TabIndex = 2;
            this.tabOptions.Text = "Options";
            // 
            // btnShowAdvanced
            // 
            this.btnShowAdvanced.Location = new System.Drawing.Point(3, 84);
            this.btnShowAdvanced.Name = "btnShowAdvanced";
            this.btnShowAdvanced.Size = new System.Drawing.Size(317, 23);
            this.btnShowAdvanced.TabIndex = 16;
            this.btnShowAdvanced.Text = "Show Advanced Tabs";
            this.toolTip1.SetToolTip(this.btnShowAdvanced, "Show the Client and Update tabs (legacy)");
            this.btnShowAdvanced.UseVisualStyleBackColor = true;
            this.btnShowAdvanced.Click += new System.EventHandler(this.btnShowAdvanced_Click);
            // 
            // btnXeBuildOptions
            // 
            this.btnXeBuildOptions.Location = new System.Drawing.Point(3, 3);
            this.btnXeBuildOptions.Name = "btnXeBuildOptions";
            this.btnXeBuildOptions.Size = new System.Drawing.Size(179, 26);
            this.btnXeBuildOptions.TabIndex = 10;
            this.btnXeBuildOptions.Text = "Advanced XeBuild Options";
            this.toolTip1.SetToolTip(this.btnXeBuildOptions, "Advanced Users Only\r\nAllows you to set many options for XeBuild");
            this.btnXeBuildOptions.UseVisualStyleBackColor = true;
            this.btnXeBuildOptions.Click += new System.EventHandler(this.btnXeBuildOptions_Click);
            // 
            // chkBigffs
            // 
            this.chkBigffs.AutoSize = true;
            this.chkBigffs.Location = new System.Drawing.Point(4, 61);
            this.chkBigffs.Name = "chkBigffs";
            this.chkBigffs.Size = new System.Drawing.Size(83, 17);
            this.chkBigffs.TabIndex = 13;
            this.chkBigffs.Text = "bigffs Image";
            this.toolTip1.SetToolTip(this.chkBigffs, "Only for 64MB and large NAND sizes");
            this.chkBigffs.UseVisualStyleBackColor = true;
            this.chkBigffs.CheckedChanged += new System.EventHandler(this.chkBigffs_CheckedChanged);
            // 
            // checkDLPatches
            // 
            this.checkDLPatches.AutoSize = true;
            this.checkDLPatches.BackColor = System.Drawing.Color.Transparent;
            this.checkDLPatches.Enabled = false;
            this.checkDLPatches.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkDLPatches.Location = new System.Drawing.Point(189, 37);
            this.checkDLPatches.Name = "checkDLPatches";
            this.checkDLPatches.Size = new System.Drawing.Size(121, 17);
            this.checkDLPatches.TabIndex = 14;
            this.checkDLPatches.Text = "Custom Dashlaunch";
            this.checkDLPatches.UseVisualStyleBackColor = false;
            this.checkDLPatches.CheckedChanged += new System.EventHandler(this.checkDLPatches_CheckedChanged);
            this.checkDLPatches.EnabledChanged += new System.EventHandler(this.checkDLPatches_EnabledChanged);
            // 
            // btnLaunch
            // 
            this.btnLaunch.Location = new System.Drawing.Point(188, 3);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(132, 26);
            this.btnLaunch.TabIndex = 11;
            this.btnLaunch.Text = "Launch.ini Options";
            this.toolTip1.SetToolTip(this.btnLaunch, "Advanced Users Only\r\nAllows you to create a launch.ini\r\n");
            this.btnLaunch.UseVisualStyleBackColor = true;
            this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
            // 
            // chkXeSettings
            // 
            this.chkXeSettings.AutoSize = true;
            this.chkXeSettings.Location = new System.Drawing.Point(4, 37);
            this.chkXeSettings.Name = "chkXeSettings";
            this.chkXeSettings.Size = new System.Drawing.Size(117, 17);
            this.chkXeSettings.TabIndex = 12;
            this.chkXeSettings.Text = "Use Edited Options";
            this.chkXeSettings.UseVisualStyleBackColor = true;
            this.chkXeSettings.CheckedChanged += new System.EventHandler(this.chkxesettings_CheckedChanged);
            // 
            // chkLaunch
            // 
            this.chkLaunch.AutoSize = true;
            this.chkLaunch.BackColor = System.Drawing.Color.Transparent;
            this.chkLaunch.Enabled = false;
            this.chkLaunch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkLaunch.Location = new System.Drawing.Point(189, 61);
            this.chkLaunch.Name = "chkLaunch";
            this.chkLaunch.Size = new System.Drawing.Size(109, 17);
            this.chkLaunch.TabIndex = 15;
            this.chkLaunch.Text = "Custom launch.ini";
            this.chkLaunch.UseVisualStyleBackColor = false;
            // 
            // tabClient
            // 
            this.tabClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tabClient.Controls.Add(this.btnInfo);
            this.tabClient.Controls.Add(this.chkForceIP2);
            this.tabClient.Controls.Add(this.txtIP2);
            this.tabClient.Controls.Add(this.lblLength);
            this.tabClient.Controls.Add(this.lblOffset);
            this.tabClient.Controls.Add(this.btnErase);
            this.tabClient.Controls.Add(this.txtOffset);
            this.tabClient.Controls.Add(this.txtLength);
            this.tabClient.Controls.Add(this.btnPatches);
            this.tabClient.Controls.Add(this.btnComp);
            this.tabClient.Controls.Add(this.btnAvatar);
            this.tabClient.Controls.Add(this.btnWrite);
            this.tabClient.Controls.Add(this.btnRead);
            this.tabClient.Controls.Add(this.chkReboot);
            this.tabClient.Controls.Add(this.chkShutdown);
            this.tabClient.Location = new System.Drawing.Point(4, 22);
            this.tabClient.Name = "tabClient";
            this.tabClient.Padding = new System.Windows.Forms.Padding(3);
            this.tabClient.Size = new System.Drawing.Size(323, 110);
            this.tabClient.TabIndex = 5;
            this.tabClient.Text = "Client";
            // 
            // btnInfo
            // 
            this.btnInfo.Location = new System.Drawing.Point(114, 56);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(57, 22);
            this.btnInfo.TabIndex = 13;
            this.btnInfo.Text = "Get Info";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // chkForceIP2
            // 
            this.chkForceIP2.AutoSize = true;
            this.chkForceIP2.Location = new System.Drawing.Point(176, 60);
            this.chkForceIP2.Name = "chkForceIP2";
            this.chkForceIP2.Size = new System.Drawing.Size(66, 17);
            this.chkForceIP2.TabIndex = 14;
            this.chkForceIP2.Text = "Force IP";
            this.chkForceIP2.UseVisualStyleBackColor = true;
            this.chkForceIP2.CheckedChanged += new System.EventHandler(this.chkForceIP2_CheckedChanged);
            // 
            // txtIP2
            // 
            this.txtIP2.Enabled = false;
            this.txtIP2.Location = new System.Drawing.Point(242, 57);
            this.txtIP2.Name = "txtIP2";
            this.txtIP2.Size = new System.Drawing.Size(75, 20);
            this.txtIP2.TabIndex = 15;
            this.txtIP2.Text = "Autoscan LAN";
            this.txtIP2.TextChanged += new System.EventHandler(this.txtIP2_TextChanged);
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point(219, 32);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(57, 13);
            this.lblLength.TabIndex = 11;
            this.lblLength.Text = "Length: 0x";
            // 
            // lblOffset
            // 
            this.lblOffset.AutoSize = true;
            this.lblOffset.Location = new System.Drawing.Point(221, 11);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(55, 13);
            this.lblOffset.TabIndex = 10;
            this.lblOffset.Text = " Offset: 0x";
            // 
            // btnErase
            // 
            this.btnErase.Location = new System.Drawing.Point(123, 5);
            this.btnErase.Name = "btnErase";
            this.btnErase.Size = new System.Drawing.Size(75, 22);
            this.btnErase.TabIndex = 7;
            this.btnErase.Text = "Erase Block";
            this.btnErase.UseVisualStyleBackColor = true;
            this.btnErase.Click += new System.EventHandler(this.btnErase_Click);
            // 
            // txtOffset
            // 
            this.txtOffset.Location = new System.Drawing.Point(282, 6);
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size(35, 20);
            this.txtOffset.TabIndex = 8;
            // 
            // txtLength
            // 
            this.txtLength.Location = new System.Drawing.Point(282, 29);
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(35, 20);
            this.txtLength.TabIndex = 11;
            // 
            // btnPatches
            // 
            this.btnPatches.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnPatches.Location = new System.Drawing.Point(5, 56);
            this.btnPatches.Name = "btnPatches";
            this.btnPatches.Size = new System.Drawing.Size(105, 22);
            this.btnPatches.TabIndex = 12;
            this.btnPatches.Text = "Update Patch(es)";
            this.btnPatches.UseVisualStyleBackColor = true;
            this.btnPatches.Click += new System.EventHandler(this.btnPatches_Click);
            // 
            // btnComp
            // 
            this.btnComp.Location = new System.Drawing.Point(149, 83);
            this.btnComp.Name = "btnComp";
            this.btnComp.Size = new System.Drawing.Size(169, 22);
            this.btnComp.TabIndex = 17;
            this.btnComp.Text = "Send Xbox Compatibility Data";
            this.btnComp.UseVisualStyleBackColor = true;
            this.btnComp.Click += new System.EventHandler(this.btnComp_Click);
            // 
            // btnAvatar
            // 
            this.btnAvatar.Location = new System.Drawing.Point(5, 83);
            this.btnAvatar.Name = "btnAvatar";
            this.btnAvatar.Size = new System.Drawing.Size(140, 22);
            this.btnAvatar.TabIndex = 16;
            this.btnAvatar.Text = "Send Avatar/Kinect Data";
            this.btnAvatar.UseVisualStyleBackColor = true;
            this.btnAvatar.Click += new System.EventHandler(this.btnAvatar_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(65, 5);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(52, 22);
            this.btnWrite.TabIndex = 6;
            this.btnWrite.Text = "Write";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(5, 5);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(54, 22);
            this.btnRead.TabIndex = 5;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // chkReboot
            // 
            this.chkReboot.AutoSize = true;
            this.chkReboot.Location = new System.Drawing.Point(86, 34);
            this.chkReboot.Name = "chkReboot";
            this.chkReboot.Size = new System.Drawing.Size(61, 17);
            this.chkReboot.TabIndex = 10;
            this.chkReboot.Text = "Reboot";
            this.chkReboot.UseVisualStyleBackColor = true;
            this.chkReboot.CheckedChanged += new System.EventHandler(this.chkReboot_CheckedChanged);
            // 
            // chkShutdown
            // 
            this.chkShutdown.AutoSize = true;
            this.chkShutdown.Location = new System.Drawing.Point(6, 34);
            this.chkShutdown.Name = "chkShutdown";
            this.chkShutdown.Size = new System.Drawing.Size(74, 17);
            this.chkShutdown.TabIndex = 9;
            this.chkShutdown.Text = "Shutdown";
            this.chkShutdown.UseVisualStyleBackColor = true;
            this.chkShutdown.CheckedChanged += new System.EventHandler(this.chkShutdown_CheckedChanged);
            // 
            // tabUpdate
            // 
            this.tabUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tabUpdate.Controls.Add(this.lblDash);
            this.tabUpdate.Controls.Add(this.lblD);
            this.tabUpdate.Controls.Add(this.txtIP);
            this.tabUpdate.Controls.Add(this.chkForceIP);
            this.tabUpdate.Controls.Add(this.chkNoReeb);
            this.tabUpdate.Controls.Add(this.chkClean);
            this.tabUpdate.Controls.Add(this.chkNoAva);
            this.tabUpdate.Controls.Add(this.chkNoWrite);
            this.tabUpdate.Controls.Add(this.btnXEUpdate);
            this.tabUpdate.Location = new System.Drawing.Point(4, 22);
            this.tabUpdate.Name = "tabUpdate";
            this.tabUpdate.Padding = new System.Windows.Forms.Padding(3);
            this.tabUpdate.Size = new System.Drawing.Size(323, 110);
            this.tabUpdate.TabIndex = 4;
            this.tabUpdate.Text = "Update";
            // 
            // lblDash
            // 
            this.lblDash.AutoSize = true;
            this.lblDash.Location = new System.Drawing.Point(154, 28);
            this.lblDash.Name = "lblDash";
            this.lblDash.Size = new System.Drawing.Size(0, 13);
            this.lblDash.TabIndex = 10;
            // 
            // lblD
            // 
            this.lblD.AutoSize = true;
            this.lblD.Location = new System.Drawing.Point(116, 28);
            this.lblD.Name = "lblD";
            this.lblD.Size = new System.Drawing.Size(0, 13);
            this.lblD.TabIndex = 9;
            // 
            // txtIP
            // 
            this.txtIP.Enabled = false;
            this.txtIP.Location = new System.Drawing.Point(230, 68);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(75, 20);
            this.txtIP.TabIndex = 11;
            this.txtIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtIP.TextChanged += new System.EventHandler(this.txtIP_TextChanged);
            // 
            // chkForceIP
            // 
            this.chkForceIP.AutoSize = true;
            this.chkForceIP.Location = new System.Drawing.Point(238, 46);
            this.chkForceIP.Name = "chkForceIP";
            this.chkForceIP.Size = new System.Drawing.Size(66, 17);
            this.chkForceIP.TabIndex = 10;
            this.chkForceIP.Text = "Force IP";
            this.chkForceIP.UseVisualStyleBackColor = true;
            this.chkForceIP.CheckedChanged += new System.EventHandler(this.chkForceIP_CheckedChanged);
            // 
            // chkNoReeb
            // 
            this.chkNoReeb.AutoSize = true;
            this.chkNoReeb.Location = new System.Drawing.Point(18, 57);
            this.chkNoReeb.Name = "chkNoReeb";
            this.chkNoReeb.Size = new System.Drawing.Size(78, 17);
            this.chkNoReeb.TabIndex = 8;
            this.chkNoReeb.Text = "No Reboot";
            this.toolTip1.SetToolTip(this.chkNoReeb, "\r\n");
            this.chkNoReeb.UseVisualStyleBackColor = true;
            this.chkNoReeb.CheckedChanged += new System.EventHandler(this.chkNoReeb_CheckedChanged);
            // 
            // chkClean
            // 
            this.chkClean.AutoSize = true;
            this.chkClean.Location = new System.Drawing.Point(18, 76);
            this.chkClean.Name = "chkClean";
            this.chkClean.Size = new System.Drawing.Size(53, 17);
            this.chkClean.TabIndex = 7;
            this.chkClean.Text = "Clean";
            this.chkClean.UseVisualStyleBackColor = true;
            this.chkClean.CheckedChanged += new System.EventHandler(this.chkClean_CheckedChanged);
            // 
            // chkNoAva
            // 
            this.chkNoAva.AutoSize = true;
            this.chkNoAva.Location = new System.Drawing.Point(18, 37);
            this.chkNoAva.Name = "chkNoAva";
            this.chkNoAva.Size = new System.Drawing.Size(74, 17);
            this.chkNoAva.TabIndex = 6;
            this.chkNoAva.Text = "No Avatar";
            this.chkNoAva.UseVisualStyleBackColor = true;
            this.chkNoAva.CheckedChanged += new System.EventHandler(this.chkNoAva_CheckedChanged);
            // 
            // chkNoWrite
            // 
            this.chkNoWrite.AutoSize = true;
            this.chkNoWrite.Location = new System.Drawing.Point(18, 17);
            this.chkNoWrite.Name = "chkNoWrite";
            this.chkNoWrite.Size = new System.Drawing.Size(68, 17);
            this.chkNoWrite.TabIndex = 5;
            this.chkNoWrite.Text = "No Write";
            this.chkNoWrite.UseVisualStyleBackColor = true;
            this.chkNoWrite.CheckedChanged += new System.EventHandler(this.chkNoWrite_CheckedChanged);
            // 
            // btnXEUpdate
            // 
            this.btnXEUpdate.Location = new System.Drawing.Point(106, 65);
            this.btnXEUpdate.Name = "btnXEUpdate";
            this.btnXEUpdate.Size = new System.Drawing.Size(100, 27);
            this.btnXEUpdate.TabIndex = 9;
            this.btnXEUpdate.Text = "Update";
            this.btnXEUpdate.UseVisualStyleBackColor = true;
            this.btnXEUpdate.Click += new System.EventHandler(this.btnXEUpdate_Click);
            // 
            // XeBuildPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox7);
            this.Name = "XeBuildPanel";
            this.Size = new System.Drawing.Size(344, 156);
            this.Load += new System.EventHandler(this.XeBuildPanel_Load);
            this.groupBox7.ResumeLayout(false);
            this.MainTabs.ResumeLayout(false);
            this.tabXeBuild.ResumeLayout(false);
            this.tabXeBuild.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dashBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dashDataSet)).EndInit();
            this.tabPatches.ResumeLayout(false);
            this.tabPatches.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabOptions.ResumeLayout(false);
            this.tabOptions.PerformLayout();
            this.tabClient.ResumeLayout(false);
            this.tabClient.PerformLayout();
            this.tabUpdate.ResumeLayout(false);
            this.tabUpdate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TabControl MainTabs;
        private System.Windows.Forms.TabPage tabXeBuild;
        private System.Windows.Forms.CheckBox chkRJtag;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDashVersion;
        private System.Windows.Forms.RadioButton rbtnGlitch2;
        private System.Windows.Forms.CheckBox chkAudClamp;
        private System.Windows.Forms.ComboBox comboDash;
        private System.Windows.Forms.RadioButton rbtnJtag;
        private System.Windows.Forms.RadioButton rbtnRetail;
        private System.Windows.Forms.RadioButton rbtnGlitch;
        private System.Windows.Forms.TextBox txtMBname;
        private System.Windows.Forms.TabPage tabOptions;
        private System.Windows.Forms.Button btnXeBuildOptions;
        private System.Windows.Forms.CheckBox chkXeSettings;
        private System.Windows.Forms.CheckedListBox chkListBoxPatches;
        private System.Windows.Forms.TabPage tabPatches;
        private System.Windows.Forms.Button btnLaunch;
        private System.Windows.Forms.CheckBox checkDLPatches;
        private System.Windows.Forms.BindingSource dashBindingSource;
        private DataSet1 dashDataSet;
        private System.Windows.Forms.CheckBox chkLaunch;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnGetMB;
        private System.Windows.Forms.TabPage tabClient;
        private System.Windows.Forms.TextBox txtOffset;
        private System.Windows.Forms.TextBox txtLength;
        private System.Windows.Forms.Button btnPatches;
        private System.Windows.Forms.Button btnComp;
        private System.Windows.Forms.Button btnAvatar;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.CheckBox chkReboot;
        private System.Windows.Forms.CheckBox chkShutdown;
        private System.Windows.Forms.Button btnErase;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.Label lblOffset;
        private System.Windows.Forms.TextBox txtIP2;
        private System.Windows.Forms.CheckBox chkForceIP2;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.RadioButton rbtnGlitch2m;
        private System.Windows.Forms.Label labelCB;
        private System.Windows.Forms.ComboBox comboCB;
        private System.Windows.Forms.CheckBox chkSMCP;
        private System.Windows.Forms.RadioButton rbtnDevGL;
        private System.Windows.Forms.CheckBox chk0Fuse;
        private System.Windows.Forms.CheckBox chkXdkBuild;
        private System.Windows.Forms.CheckBox chkCleanSMC;
        private System.Windows.Forms.CheckBox chkCR4;
        private System.Windows.Forms.CheckBox chkWB;
        private System.Windows.Forms.CheckBox chkWB4G;
        private System.Windows.Forms.CheckBox chkBigffs;
        private System.Windows.Forms.CheckBox chkRgh3;
        private System.Windows.Forms.Label Rgh3Label;
        private System.Windows.Forms.ComboBox Rgh3Mhz;
        private System.Windows.Forms.Label Rgh3Label2;
        private System.Windows.Forms.CheckBox chkXLUsb;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkXLHdd;
        private System.Windows.Forms.Button btnShowAdvanced;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkUsbdSec;
        private System.Windows.Forms.TabPage tabUpdate;
        private System.Windows.Forms.Label lblDash;
        private System.Windows.Forms.Label lblD;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.CheckBox chkForceIP;
        private System.Windows.Forms.CheckBox chkNoReeb;
        private System.Windows.Forms.CheckBox chkClean;
        private System.Windows.Forms.CheckBox chkNoAva;
        private System.Windows.Forms.CheckBox chkNoWrite;
        private System.Windows.Forms.Button btnXEUpdate;
        private System.Windows.Forms.CheckBox chkXLBoth;
        private System.Windows.Forms.CheckBox chkCoronaKeyFix;
    }
}
