namespace JRunner.Panels
{
    partial class NandTools
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
            this.btnCPUDB = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.txtLPTPort = new System.Windows.Forms.TextBox();
            this.lblLPTPort = new System.Windows.Forms.Label();
            this.rbtnLPT = new System.Windows.Forms.RadioButton();
            this.rbtnUSB = new System.Windows.Forms.RadioButton();
            this.btnProgramCR = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnCreateECC = new System.Windows.Forms.Button();
            this.btnXeBuild = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnWriteECC = new System.Windows.Forms.Button();
            this.lblNReads = new System.Windows.Forms.Label();
            this.numericIterations = new System.Windows.Forms.NumericUpDown();
            this.pBoxDevice = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox9.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericIterations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxDevice)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCPUDB
            // 
            this.btnCPUDB.Location = new System.Drawing.Point(206, 107);
            this.btnCPUDB.Name = "btnCPUDB";
            this.btnCPUDB.Size = new System.Drawing.Size(65, 51);
            this.btnCPUDB.TabIndex = 80;
            this.btnCPUDB.TabStop = false;
            this.btnCPUDB.Text = "CPU Key Database";
            this.toolTip1.SetToolTip(this.btnCPUDB, "Database of CPU Keys and Serial Numbers");
            this.btnCPUDB.UseVisualStyleBackColor = true;
            this.btnCPUDB.Click += new System.EventHandler(this.btnCPUDB_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.txtLPTPort);
            this.groupBox9.Controls.Add(this.lblLPTPort);
            this.groupBox9.Controls.Add(this.rbtnLPT);
            this.groupBox9.Controls.Add(this.rbtnUSB);
            this.groupBox9.Location = new System.Drawing.Point(63, 99);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(137, 62);
            this.groupBox9.TabIndex = 82;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Glitch Chip Programming";
            // 
            // txtLPTPort
            // 
            this.txtLPTPort.Location = new System.Drawing.Point(66, 36);
            this.txtLPTPort.MaxLength = 4;
            this.txtLPTPort.Name = "txtLPTPort";
            this.txtLPTPort.Size = new System.Drawing.Size(46, 20);
            this.txtLPTPort.TabIndex = 3;
            this.txtLPTPort.TabStop = false;
            this.txtLPTPort.Text = "378";
            this.txtLPTPort.Visible = false;
            this.txtLPTPort.TextChanged += new System.EventHandler(this.txtLPTPort_TextChanged);
            // 
            // lblLPTPort
            // 
            this.lblLPTPort.AutoSize = true;
            this.lblLPTPort.Location = new System.Drawing.Point(63, 19);
            this.lblLPTPort.Name = "lblLPTPort";
            this.lblLPTPort.Size = new System.Drawing.Size(26, 13);
            this.lblLPTPort.TabIndex = 2;
            this.lblLPTPort.Text = "Port";
            this.lblLPTPort.Visible = false;
            // 
            // rbtnLPT
            // 
            this.rbtnLPT.AutoSize = true;
            this.rbtnLPT.Location = new System.Drawing.Point(10, 37);
            this.rbtnLPT.Name = "rbtnLPT";
            this.rbtnLPT.Size = new System.Drawing.Size(45, 17);
            this.rbtnLPT.TabIndex = 1;
            this.rbtnLPT.TabStop = true;
            this.rbtnLPT.Text = "LPT";
            this.rbtnLPT.UseVisualStyleBackColor = true;
            this.rbtnLPT.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // rbtnUSB
            // 
            this.rbtnUSB.AutoSize = true;
            this.rbtnUSB.Checked = true;
            this.rbtnUSB.Location = new System.Drawing.Point(10, 18);
            this.rbtnUSB.Name = "rbtnUSB";
            this.rbtnUSB.Size = new System.Drawing.Size(47, 17);
            this.rbtnUSB.TabIndex = 10;
            this.rbtnUSB.TabStop = true;
            this.rbtnUSB.Text = "USB";
            this.rbtnUSB.UseVisualStyleBackColor = true;
            this.rbtnUSB.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // btnProgramCR
            // 
            this.btnProgramCR.Location = new System.Drawing.Point(7, 22);
            this.btnProgramCR.Name = "btnProgramCR";
            this.btnProgramCR.Size = new System.Drawing.Size(66, 51);
            this.btnProgramCR.TabIndex = 6;
            this.btnProgramCR.TabStop = false;
            this.btnProgramCR.Text = "Program Timing File";
            this.toolTip1.SetToolTip(this.btnProgramCR, "Program one of the built in timing files to a glitch chip");
            this.btnProgramCR.UseVisualStyleBackColor = true;
            this.btnProgramCR.Click += new System.EventHandler(this.btnProgramCR_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.groupBox3.Controls.Add(this.btnCreateECC);
            this.groupBox3.Controls.Add(this.btnXeBuild);
            this.groupBox3.Controls.Add(this.btnWrite);
            this.groupBox3.Controls.Add(this.btnRead);
            this.groupBox3.Controls.Add(this.btnWriteECC);
            this.groupBox3.Location = new System.Drawing.Point(3, 1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(373, 87);
            this.groupBox3.TabIndex = 81;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Nand";
            // 
            // btnCreateECC
            // 
            this.btnCreateECC.Location = new System.Drawing.Point(82, 22);
            this.btnCreateECC.Name = "btnCreateECC";
            this.btnCreateECC.Size = new System.Drawing.Size(65, 51);
            this.btnCreateECC.TabIndex = 64;
            this.btnCreateECC.TabStop = false;
            this.btnCreateECC.Text = "Create ECC";
            this.toolTip1.SetToolTip(this.btnCreateECC, "Creates an ECC or XeLL from the source file, and the settings selected on the XeB" +
        "uild panel");
            this.btnCreateECC.UseVisualStyleBackColor = true;
            this.btnCreateECC.Click += new System.EventHandler(this.btnCreateECC_Click);
            // 
            // btnXeBuild
            // 
            this.btnXeBuild.Location = new System.Drawing.Point(226, 22);
            this.btnXeBuild.Name = "btnXeBuild";
            this.btnXeBuild.Size = new System.Drawing.Size(65, 51);
            this.btnXeBuild.TabIndex = 5;
            this.btnXeBuild.TabStop = false;
            this.btnXeBuild.Text = "Create XeBuild Image";
            this.toolTip1.SetToolTip(this.btnXeBuild, "Creates an XeBuild image from the source file, and the settings selected on the X" +
        "eBuild panel");
            this.btnXeBuild.UseVisualStyleBackColor = true;
            this.btnXeBuild.Click += new System.EventHandler(this.btnXeBuild_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(298, 22);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(65, 51);
            this.btnWrite.TabIndex = 56;
            this.btnWrite.TabStop = false;
            this.btnWrite.Text = "Write Nand";
            this.toolTip1.SetToolTip(this.btnWrite, "Writes the source to the nand");
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(10, 22);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(65, 51);
            this.btnRead.TabIndex = 8;
            this.btnRead.TabStop = false;
            this.btnRead.Text = "Read Nand";
            this.toolTip1.SetToolTip(this.btnRead, "Reads the nand the amount of times selected below");
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnWriteECC
            // 
            this.btnWriteECC.Location = new System.Drawing.Point(154, 22);
            this.btnWriteECC.Name = "btnWriteECC";
            this.btnWriteECC.Size = new System.Drawing.Size(65, 51);
            this.btnWriteECC.TabIndex = 9;
            this.btnWriteECC.TabStop = false;
            this.btnWriteECC.Text = "Write\r\nECC";
            this.toolTip1.SetToolTip(this.btnWriteECC, "Writes ECC or XeLL to the nand");
            this.btnWriteECC.UseVisualStyleBackColor = true;
            this.btnWriteECC.Click += new System.EventHandler(this.btnWriteECC_Click);
            // 
            // lblNReads
            // 
            this.lblNReads.AutoSize = true;
            this.lblNReads.Location = new System.Drawing.Point(6, 15);
            this.lblNReads.Name = "lblNReads";
            this.lblNReads.Size = new System.Drawing.Size(38, 13);
            this.lblNReads.TabIndex = 63;
            this.lblNReads.Text = "Reads";
            // 
            // numericIterations
            // 
            this.numericIterations.BackColor = System.Drawing.Color.Gainsboro;
            this.numericIterations.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericIterations.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.numericIterations.ForeColor = System.Drawing.Color.Black;
            this.numericIterations.Location = new System.Drawing.Point(9, 33);
            this.numericIterations.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericIterations.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericIterations.Name = "numericIterations";
            this.numericIterations.Size = new System.Drawing.Size(37, 17);
            this.numericIterations.TabIndex = 20;
            this.numericIterations.TabStop = false;
            this.numericIterations.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numericIterations, "This is the number of reads that will be performed on Read Nand");
            this.numericIterations.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericIterations.ValueChanged += new System.EventHandler(this.numericIterations_ValueChanged);
            // 
            // pBoxDevice
            // 
            this.pBoxDevice.ErrorImage = null;
            this.pBoxDevice.InitialImage = null;
            this.pBoxDevice.Location = new System.Drawing.Point(323, 106);
            this.pBoxDevice.Name = "pBoxDevice";
            this.pBoxDevice.Size = new System.Drawing.Size(53, 43);
            this.pBoxDevice.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pBoxDevice.TabIndex = 83;
            this.pBoxDevice.TabStop = false;
            this.pBoxDevice.Click += new System.EventHandler(this.pBoxDevice_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblNReads);
            this.groupBox1.Controls.Add(this.numericIterations);
            this.groupBox1.Location = new System.Drawing.Point(3, 99);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(53, 62);
            this.groupBox1.TabIndex = 85;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Nand";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnProgramCR);
            this.groupBox2.Location = new System.Drawing.Point(383, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(80, 87);
            this.groupBox2.TabIndex = 86;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Glitch Chip";
            // 
            // NandTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pBoxDevice);
            this.Controls.Add(this.btnCPUDB);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox3);
            this.Name = "NandTools";
            this.Size = new System.Drawing.Size(463, 175);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericIterations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxDevice)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pBoxDevice;
        private System.Windows.Forms.Button btnCPUDB;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.TextBox txtLPTPort;
        private System.Windows.Forms.Label lblLPTPort;
        private System.Windows.Forms.RadioButton rbtnLPT;
        private System.Windows.Forms.RadioButton rbtnUSB;
        private System.Windows.Forms.Button btnProgramCR;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnCreateECC;
        private System.Windows.Forms.Label lblNReads;
        private System.Windows.Forms.Button btnXeBuild;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.NumericUpDown numericIterations;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnWriteECC;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}
