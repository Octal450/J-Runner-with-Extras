namespace JRunner
{
    partial class patch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(patch));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCF1pd = new System.Windows.Forms.TextBox();
            this.txtCF0pd = new System.Windows.Forms.TextBox();
            this.textCBpd = new System.Windows.Forms.TextBox();
            this.txtCF1ldv = new System.Windows.Forms.TextBox();
            this.txtCF0ldv = new System.Windows.Forms.TextBox();
            this.txtCBldv = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
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
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.osigBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.regionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.osigBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.regionBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtCF1pd);
            this.groupBox1.Controls.Add(this.txtCF0pd);
            this.groupBox1.Controls.Add(this.textCBpd);
            this.groupBox1.Controls.Add(this.txtCF1ldv);
            this.groupBox1.Controls.Add(this.txtCF0ldv);
            this.groupBox1.Controls.Add(this.txtCBldv);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 135);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lock Down Values";
            this.groupBox1.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(284, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "6BL [CF] Patch 1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(284, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "6BL [CF] Patch 0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(284, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "2BL [CB]";
            this.label3.Visible = false;
            // 
            // txtCF1pd
            // 
            this.txtCF1pd.Enabled = false;
            this.txtCF1pd.Location = new System.Drawing.Point(186, 99);
            this.txtCF1pd.Name = "txtCF1pd";
            this.txtCF1pd.Size = new System.Drawing.Size(78, 20);
            this.txtCF1pd.TabIndex = 8;
            this.txtCF1pd.TextChanged += new System.EventHandler(this.txtCF1pd_TextChanged);
            // 
            // txtCF0pd
            // 
            this.txtCF0pd.Enabled = false;
            this.txtCF0pd.Location = new System.Drawing.Point(186, 72);
            this.txtCF0pd.Name = "txtCF0pd";
            this.txtCF0pd.Size = new System.Drawing.Size(78, 20);
            this.txtCF0pd.TabIndex = 7;
            this.txtCF0pd.TextChanged += new System.EventHandler(this.txtCF0pd_TextChanged);
            // 
            // textCBpd
            // 
            this.textCBpd.Enabled = false;
            this.textCBpd.Location = new System.Drawing.Point(186, 45);
            this.textCBpd.Name = "textCBpd";
            this.textCBpd.Size = new System.Drawing.Size(78, 20);
            this.textCBpd.TabIndex = 6;
            this.textCBpd.Visible = false;
            this.textCBpd.TextChanged += new System.EventHandler(this.txtCBpd_TextChanged);
            // 
            // txtCF1ldv
            // 
            this.txtCF1ldv.Enabled = false;
            this.txtCF1ldv.Location = new System.Drawing.Point(145, 99);
            this.txtCF1ldv.Name = "txtCF1ldv";
            this.txtCF1ldv.Size = new System.Drawing.Size(25, 20);
            this.txtCF1ldv.TabIndex = 5;
            this.txtCF1ldv.TextChanged += new System.EventHandler(this.txtCF1ldv_TextChanged);
            // 
            // txtCF0ldv
            // 
            this.txtCF0ldv.Enabled = false;
            this.txtCF0ldv.Location = new System.Drawing.Point(145, 72);
            this.txtCF0ldv.Name = "txtCF0ldv";
            this.txtCF0ldv.Size = new System.Drawing.Size(25, 20);
            this.txtCF0ldv.TabIndex = 4;
            this.txtCF0ldv.TextChanged += new System.EventHandler(this.txtCF0ldv_TextChanged);
            // 
            // txtCBldv
            // 
            this.txtCBldv.Enabled = false;
            this.txtCBldv.Location = new System.Drawing.Point(145, 45);
            this.txtCBldv.Name = "txtCBldv";
            this.txtCBldv.Size = new System.Drawing.Size(25, 20);
            this.txtCBldv.TabIndex = 3;
            this.txtCBldv.Visible = false;
            this.txtCBldv.TextChanged += new System.EventHandler(this.txtCBldv_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(183, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Pairing Data";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(142, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "LDV";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(7, 20);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(97, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Patch Headers";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
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
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 135);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(420, 198);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Key Vault";
            // 
            // txtConsoleID
            // 
            this.txtConsoleID.Enabled = false;
            this.txtConsoleID.Location = new System.Drawing.Point(103, 136);
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
            this.label10.Location = new System.Drawing.Point(26, 139);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Console ID";
            // 
            // txtSerial
            // 
            this.txtSerial.Enabled = false;
            this.txtSerial.Location = new System.Drawing.Point(103, 110);
            this.txtSerial.MaxLength = 12;
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.Size = new System.Drawing.Size(92, 20);
            this.txtSerial.TabIndex = 8;
            this.txtSerial.TextChanged += new System.EventHandler(this.txtSerial_TextChanged);
            this.txtSerial.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSerial_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(26, 113);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Serial";
            // 
            // comboOsig
            // 
            this.comboOsig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboOsig.Enabled = false;
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
            this.comboOsig.Location = new System.Drawing.Point(103, 162);
            this.comboOsig.Name = "comboOsig";
            this.comboOsig.Size = new System.Drawing.Size(264, 21);
            this.comboOsig.TabIndex = 6;
            this.comboOsig.SelectedIndexChanged += new System.EventHandler(this.comboOsig_SelectedIndexChanged);
            // 
            // comboRegion
            // 
            this.comboRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRegion.Enabled = false;
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
            this.comboRegion.Location = new System.Drawing.Point(103, 52);
            this.comboRegion.Name = "comboRegion";
            this.comboRegion.Size = new System.Drawing.Size(145, 21);
            this.comboRegion.TabIndex = 5;
            this.comboRegion.SelectedIndexChanged += new System.EventHandler(this.comboRegion_SelectedIndexChanged);
            // 
            // txtDVDkey
            // 
            this.txtDVDkey.Enabled = false;
            this.txtDVDkey.Location = new System.Drawing.Point(103, 84);
            this.txtDVDkey.MaxLength = 32;
            this.txtDVDkey.Name = "txtDVDkey";
            this.txtDVDkey.Size = new System.Drawing.Size(264, 20);
            this.txtDVDkey.TabIndex = 4;
            this.txtDVDkey.TextChanged += new System.EventHandler(this.txtDVDkey_TextChanged);
            this.txtDVDkey.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDVDkey_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 165);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "OSIG";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 87);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "DVD Key";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Region";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(7, 20);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(102, 17);
            this.checkBox2.TabIndex = 0;
            this.checkBox2.Text = "Patch Key Vault";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
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
            this.btnOK.Location = new System.Drawing.Point(95, 350);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(258, 350);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // patch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(420, 388);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "patch";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Patch Flash Image";
            this.Load += new System.EventHandler(this.patch_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.osigBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.regionBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCF1pd;
        private System.Windows.Forms.TextBox txtCF0pd;
        private System.Windows.Forms.TextBox textCBpd;
        private System.Windows.Forms.TextBox txtCF1ldv;
        private System.Windows.Forms.TextBox txtCF0ldv;
        private System.Windows.Forms.TextBox txtCBldv;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboOsig;
        private System.Windows.Forms.ComboBox comboRegion;
        private System.Windows.Forms.TextBox txtDVDkey;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox2;
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