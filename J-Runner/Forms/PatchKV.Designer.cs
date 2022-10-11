namespace JRunner
{
    partial class PatchKV
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatchKV));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtConsoleID = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboOsig = new System.Windows.Forms.ComboBox();
            this.comboRegion = new System.Windows.Forms.ComboBox();
            this.txtDVDkey = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.osigBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.regionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.osigBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.regionBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtConsoleID);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtSerial);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.comboOsig);
            this.groupBox2.Controls.Add(this.comboRegion);
            this.groupBox2.Controls.Add(this.txtDVDkey);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(13, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(353, 158);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Key Vault";
            // 
            // txtConsoleID
            // 
            this.txtConsoleID.Location = new System.Drawing.Point(76, 18);
            this.txtConsoleID.MaxLength = 10;
            this.txtConsoleID.Name = "txtConsoleID";
            this.txtConsoleID.Size = new System.Drawing.Size(92, 20);
            this.txtConsoleID.TabIndex = 10;
            this.txtConsoleID.TextChanged += new System.EventHandler(this.txtConsoleID_TextChanged);
            this.txtConsoleID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtConsoleID_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Console ID:";
            // 
            // txtSerial
            // 
            this.txtSerial.Location = new System.Drawing.Point(76, 44);
            this.txtSerial.MaxLength = 12;
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.Size = new System.Drawing.Size(92, 20);
            this.txtSerial.TabIndex = 11;
            this.txtSerial.TextChanged += new System.EventHandler(this.txtSerial_TextChanged);
            this.txtSerial.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSerial_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Serial:";
            // 
            // comboOsig
            // 
            this.comboOsig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboOsig.FormattingEnabled = true;
            this.comboOsig.Items.AddRange(new object[] {
            "PBDS    VAD6038         04421C",
            "PBDS    VAD6038         64930C      ",
            "PLDS    DG-16D2S        0251",
            "PLDS    DG-16D2S        7485",
            "PLDS    DG-16D2S        8385",
            "PLDS    DG-16D2S        9345",
            "PLDS    DG-16D4S        0225",
            "PLDS    DG-16D4S        0272",
            "PLDS    DG-16D4S        0401",
            "PLDS    DG-16D4S        1071",
            "PLDS    DG-16D4S        9504   ",
            "PLDS    DG-16D5S        1175",
            "PLDS    DG-16D5S        1532",
            "TSSTcorp TS-H943A   ms25",
            "TSSTcorp TS-H943A   ms28",
            "HL-DT-ST GDR-3120L 0032",
            "HL-DT-ST GDR-3120L 0036",
            "HL-DT-ST GDR-3120L 0040",
            "HL-DT-ST GDR-3120L 0046",
            "HL-DT-ST GDR-3120L 0047",
            "HL-DT-ST GDR-3120L 0058",
            "HL-DT-ST GDR-3120L 0059",
            "HL-DT-ST GDR-3120L 0078",
            "HL-DT-ST GDR-3120L 0079",
            "HL-DT-ST  DL10N   0500",
            "HL-DT-ST  DL10N   0502",
            "No Drive Info/Unspoofed"});
            this.comboOsig.Location = new System.Drawing.Point(76, 123);
            this.comboOsig.Name = "comboOsig";
            this.comboOsig.Size = new System.Drawing.Size(264, 21);
            this.comboOsig.TabIndex = 14;
            this.comboOsig.SelectedIndexChanged += new System.EventHandler(this.comboOsig_SelectedIndexChanged);
            // 
            // comboRegion
            // 
            this.comboRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRegion.FormattingEnabled = true;
            this.comboRegion.Items.AddRange(new object[] {
            "0x02FE   |   PAL/EU",
            "0x00FF   |   NTSC/US",
            "0x01FE   |   NTSC/JAP",
            "0x01FF   |   NTSC/JAP",
            "0x01FC  |   NTSC/KOR",
            "0x0101   |   NTSC/HK",
            "0x0201   |   PAL/AUS",
            "0x7FFF   |   DEVKIT"});
            this.comboRegion.Location = new System.Drawing.Point(76, 70);
            this.comboRegion.Name = "comboRegion";
            this.comboRegion.Size = new System.Drawing.Size(145, 21);
            this.comboRegion.TabIndex = 12;
            this.comboRegion.SelectedIndexChanged += new System.EventHandler(this.comboRegion_SelectedIndexChanged);
            // 
            // txtDVDkey
            // 
            this.txtDVDkey.Location = new System.Drawing.Point(76, 97);
            this.txtDVDkey.MaxLength = 32;
            this.txtDVDkey.Name = "txtDVDkey";
            this.txtDVDkey.Size = new System.Drawing.Size(264, 20);
            this.txtDVDkey.TabIndex = 13;
            this.txtDVDkey.TextChanged += new System.EventHandler(this.txtDVDkey_TextChanged);
            this.txtDVDkey.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDVDkey_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "OSIG:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "DVD Key:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Region:";
            // 
            // osigBindingSource
            // 
            this.osigBindingSource.DataMember = "osig";
            // 
            // regionBindingSource
            // 
            this.regionBindingSource.DataMember = "region";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(111, 176);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(192, 176);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 70;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // PatchKV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(379, 211);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PatchKV";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Patch KV";
            this.Load += new System.EventHandler(this.patch_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.osigBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.regionBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboOsig;
        private System.Windows.Forms.ComboBox comboRegion;
        private System.Windows.Forms.TextBox txtDVDkey;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.BindingSource regionBindingSource;
        private System.Windows.Forms.BindingSource osigBindingSource;
        private System.Windows.Forms.TextBox txtConsoleID;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSerial;
        private System.Windows.Forms.Label label9;
    }
}