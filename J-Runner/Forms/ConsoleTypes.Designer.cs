namespace JRunner
{
    partial class ConsoleTypes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleTypes));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radiobtnTrinity = new System.Windows.Forms.RadioButton();
            this.radiobtnFalcon = new System.Windows.Forms.RadioButton();
            this.radiobtnZephyr = new System.Windows.Forms.RadioButton();
            this.radiobtnJasper256 = new System.Windows.Forms.RadioButton();
            this.radiobtnJasperSB = new System.Windows.Forms.RadioButton();
            this.radiobtnJasper = new System.Windows.Forms.RadioButton();
            this.radiobtnJasper512 = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.fulldumpbox = new System.Windows.Forms.CheckBox();
            this.linkLabelHelp = new System.Windows.Forms.LinkLabel();
            this.radiobtnXenon = new System.Windows.Forms.RadioButton();
            this.checkBox2MB = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip4 = new System.Windows.Forms.ToolTip(this.components);
            this.chkAdv = new System.Windows.Forms.CheckBox();
            this.radiobtnCorona = new System.Windows.Forms.RadioButton();
            this.radiobtnCorona4gb = new System.Windows.Forms.RadioButton();
            this.radiobtnTrinityBB = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(200, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 120);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Console Types";
            // 
            // radiobtnTrinity
            // 
            this.radiobtnTrinity.AutoSize = true;
            this.radiobtnTrinity.Location = new System.Drawing.Point(12, 146);
            this.radiobtnTrinity.Name = "radiobtnTrinity";
            this.radiobtnTrinity.Size = new System.Drawing.Size(53, 17);
            this.radiobtnTrinity.TabIndex = 0;
            this.radiobtnTrinity.Text = "Trinity";
            this.radiobtnTrinity.UseVisualStyleBackColor = true;
            // 
            // radiobtnFalcon
            // 
            this.radiobtnFalcon.AutoSize = true;
            this.radiobtnFalcon.Location = new System.Drawing.Point(12, 54);
            this.radiobtnFalcon.Name = "radiobtnFalcon";
            this.radiobtnFalcon.Size = new System.Drawing.Size(57, 17);
            this.radiobtnFalcon.TabIndex = 1;
            this.radiobtnFalcon.Text = "Falcon";
            this.radiobtnFalcon.UseVisualStyleBackColor = true;
            // 
            // radiobtnZephyr
            // 
            this.radiobtnZephyr.AutoSize = true;
            this.radiobtnZephyr.Location = new System.Drawing.Point(12, 31);
            this.radiobtnZephyr.Name = "radiobtnZephyr";
            this.radiobtnZephyr.Size = new System.Drawing.Size(58, 17);
            this.radiobtnZephyr.TabIndex = 2;
            this.radiobtnZephyr.Text = "Zephyr";
            this.radiobtnZephyr.UseVisualStyleBackColor = true;
            // 
            // radiobtnJasper256
            // 
            this.radiobtnJasper256.AutoSize = true;
            this.radiobtnJasper256.Location = new System.Drawing.Point(12, 100);
            this.radiobtnJasper256.Name = "radiobtnJasper256";
            this.radiobtnJasper256.Size = new System.Drawing.Size(93, 17);
            this.radiobtnJasper256.TabIndex = 5;
            this.radiobtnJasper256.Text = "Jasper 256MB";
            this.toolTip2.SetToolTip(this.radiobtnJasper256, "Without additional selection of boxes - this will dump first 64Mb of Nand Only!");
            this.radiobtnJasper256.UseVisualStyleBackColor = true;
            // 
            // radiobtnJasperSB
            // 
            this.radiobtnJasperSB.AutoSize = true;
            this.radiobtnJasperSB.Location = new System.Drawing.Point(12, 215);
            this.radiobtnJasperSB.Name = "radiobtnJasperSB";
            this.radiobtnJasperSB.Size = new System.Drawing.Size(187, 17);
            this.radiobtnJasperSB.TabIndex = 4;
            this.radiobtnJasperSB.Text = "Jasper 16MB (XSB / 0x01198010)";
            this.radiobtnJasperSB.UseVisualStyleBackColor = true;
            this.radiobtnJasperSB.Visible = false;
            // 
            // radiobtnJasper
            // 
            this.radiobtnJasper.AutoSize = true;
            this.radiobtnJasper.Location = new System.Drawing.Point(12, 77);
            this.radiobtnJasper.Name = "radiobtnJasper";
            this.radiobtnJasper.Size = new System.Drawing.Size(87, 17);
            this.radiobtnJasper.TabIndex = 3;
            this.radiobtnJasper.Text = "Jasper 16MB";
            this.radiobtnJasper.UseVisualStyleBackColor = true;
            // 
            // radiobtnJasper512
            // 
            this.radiobtnJasper512.AutoSize = true;
            this.radiobtnJasper512.Location = new System.Drawing.Point(12, 123);
            this.radiobtnJasper512.Name = "radiobtnJasper512";
            this.radiobtnJasper512.Size = new System.Drawing.Size(93, 17);
            this.radiobtnJasper512.TabIndex = 6;
            this.radiobtnJasper512.Text = "Jasper 512MB";
            this.toolTip2.SetToolTip(this.radiobtnJasper512, "Without additional selection of boxes - this will dump first 64Mb of Nand Only!");
            this.radiobtnJasper512.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(13, 263);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(196, 263);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(79, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // fulldumpbox
            // 
            this.fulldumpbox.AutoSize = true;
            this.fulldumpbox.Location = new System.Drawing.Point(148, 111);
            this.fulldumpbox.Name = "fulldumpbox";
            this.fulldumpbox.Size = new System.Drawing.Size(73, 17);
            this.fulldumpbox.TabIndex = 11;
            this.fulldumpbox.Text = "Full Dump";
            this.toolTip3.SetToolTip(this.fulldumpbox, "Selecting this will allow FULL dumps of the 256 or 512 nands");
            this.fulldumpbox.UseVisualStyleBackColor = true;
            this.fulldumpbox.CheckedChanged += new System.EventHandler(this.fulldumpbox_CheckedChanged);
            // 
            // linkLabelHelp
            // 
            this.linkLabelHelp.AutoSize = true;
            this.linkLabelHelp.Location = new System.Drawing.Point(183, 2);
            this.linkLabelHelp.Name = "linkLabelHelp";
            this.linkLabelHelp.Size = new System.Drawing.Size(29, 13);
            this.linkLabelHelp.TabIndex = 12;
            this.linkLabelHelp.TabStop = true;
            this.linkLabelHelp.Text = "Help";
            this.linkLabelHelp.Click += new System.EventHandler(this.linkLabel1_Click);
            // 
            // radiobtnXenon
            // 
            this.radiobtnXenon.AutoSize = true;
            this.radiobtnXenon.Location = new System.Drawing.Point(12, 8);
            this.radiobtnXenon.Name = "radiobtnXenon";
            this.radiobtnXenon.Size = new System.Drawing.Size(56, 17);
            this.radiobtnXenon.TabIndex = 13;
            this.radiobtnXenon.Text = "Xenon";
            this.radiobtnXenon.UseVisualStyleBackColor = true;
            // 
            // checkBox2MB
            // 
            this.checkBox2MB.AutoSize = true;
            this.checkBox2MB.Location = new System.Drawing.Point(148, 88);
            this.checkBox2MB.Name = "checkBox2MB";
            this.checkBox2MB.Size = new System.Drawing.Size(79, 17);
            this.checkBox2MB.TabIndex = 14;
            this.checkBox2MB.Text = "2MB Dump";
            this.toolTip1.SetToolTip(this.checkBox2MB, resources.GetString("checkBox2MB.ToolTip"));
            this.checkBox2MB.UseVisualStyleBackColor = true;
            this.checkBox2MB.Visible = false;
            this.checkBox2MB.CheckedChanged += new System.EventHandler(this.checkBox2MB_CheckedChanged);
            // 
            // chkAdv
            // 
            this.chkAdv.AutoSize = true;
            this.chkAdv.Location = new System.Drawing.Point(215, 2);
            this.chkAdv.Name = "chkAdv";
            this.chkAdv.Size = new System.Drawing.Size(75, 17);
            this.chkAdv.TabIndex = 16;
            this.chkAdv.Text = "Advanced";
            this.chkAdv.UseVisualStyleBackColor = true;
            this.chkAdv.CheckedChanged += new System.EventHandler(this.chkAdv_CheckedChanged);
            // 
            // radiobtnCorona
            // 
            this.radiobtnCorona.AutoSize = true;
            this.radiobtnCorona.Location = new System.Drawing.Point(12, 169);
            this.radiobtnCorona.Name = "radiobtnCorona";
            this.radiobtnCorona.Size = new System.Drawing.Size(90, 17);
            this.radiobtnCorona.TabIndex = 17;
            this.radiobtnCorona.Text = "Corona 16MB";
            this.radiobtnCorona.UseVisualStyleBackColor = true;
            // 
            // radiobtnCorona4gb
            // 
            this.radiobtnCorona4gb.AutoSize = true;
            this.radiobtnCorona4gb.Location = new System.Drawing.Point(12, 192);
            this.radiobtnCorona4gb.Name = "radiobtnCorona4gb";
            this.radiobtnCorona4gb.Size = new System.Drawing.Size(83, 17);
            this.radiobtnCorona4gb.TabIndex = 19;
            this.radiobtnCorona4gb.Text = "Corona 4GB";
            this.radiobtnCorona4gb.UseVisualStyleBackColor = true;
            // 
            // radiobtnTrinityBB
            // 
            this.radiobtnTrinityBB.AutoSize = true;
            this.radiobtnTrinityBB.Location = new System.Drawing.Point(12, 238);
            this.radiobtnTrinityBB.Name = "radiobtnTrinityBB";
            this.radiobtnTrinityBB.Size = new System.Drawing.Size(70, 17);
            this.radiobtnTrinityBB.TabIndex = 20;
            this.radiobtnTrinityBB.Text = "Trinity BB";
            this.radiobtnTrinityBB.UseVisualStyleBackColor = true;
            this.radiobtnTrinityBB.Visible = false;
            // 
            // ConsoleTypes
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(287, 296);
            this.ControlBox = false;
            this.Controls.Add(this.radiobtnTrinityBB);
            this.Controls.Add(this.radiobtnJasperSB);
            this.Controls.Add(this.radiobtnCorona4gb);
            this.Controls.Add(this.radiobtnCorona);
            this.Controls.Add(this.chkAdv);
            this.Controls.Add(this.checkBox2MB);
            this.Controls.Add(this.radiobtnXenon);
            this.Controls.Add(this.linkLabelHelp);
            this.Controls.Add(this.fulldumpbox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.radiobtnJasper512);
            this.Controls.Add(this.radiobtnJasper256);
            this.Controls.Add(this.radiobtnJasper);
            this.Controls.Add(this.radiobtnZephyr);
            this.Controls.Add(this.radiobtnFalcon);
            this.Controls.Add(this.radiobtnTrinity);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ConsoleTypes";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Motherboard";
            this.toolTip4.SetToolTip(this, "Choose your motherboard type. If you are unsure of your type\r\nthen use the \"Get m" +
        "b Type\" in \"Tools\" menu.\r\n (the Nand-x is required to be connected)");
            this.Load += new System.EventHandler(this.ConsoleTypes_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void linkLabel1_Click(object sender, System.EventArgs e)
        {
            PictureViewer newform = new PictureViewer(variables.xmodel);
            newform.Show();
        }
        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radiobtnTrinity;
        private System.Windows.Forms.RadioButton radiobtnFalcon;
        private System.Windows.Forms.RadioButton radiobtnZephyr;
        private System.Windows.Forms.RadioButton radiobtnJasper;
        private System.Windows.Forms.RadioButton radiobtnJasperSB;
        private System.Windows.Forms.RadioButton radiobtnJasper256;
        private System.Windows.Forms.RadioButton radiobtnJasper512;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox fulldumpbox;
        private System.Windows.Forms.LinkLabel linkLabelHelp;
        private System.Windows.Forms.RadioButton radiobtnXenon;
        private System.Windows.Forms.CheckBox checkBox2MB;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.ToolTip toolTip3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip4;
        private System.Windows.Forms.CheckBox chkAdv;
        private System.Windows.Forms.RadioButton radiobtnCorona;
        private System.Windows.Forms.RadioButton radiobtnCorona4gb;
        private System.Windows.Forms.RadioButton radiobtnTrinityBB;
    }
}