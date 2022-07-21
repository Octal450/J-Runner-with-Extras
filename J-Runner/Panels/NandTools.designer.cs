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
            this.btnExtractFiles = new System.Windows.Forms.Button();
            this.btnCreateDonor = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericIterations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxDevice)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCPUDB
            // 
            this.btnCPUDB.Location = new System.Drawing.Point(208, 106);
            this.btnCPUDB.Name = "btnCPUDB";
            this.btnCPUDB.Size = new System.Drawing.Size(65, 51);
            this.btnCPUDB.TabIndex = 29;
            this.btnCPUDB.Text = "CPU Key\r\nDatabase";
            this.toolTip1.SetToolTip(this.btnCPUDB, "Database of CPU Keys and Serial Numbers");
            this.btnCPUDB.UseVisualStyleBackColor = true;
            this.btnCPUDB.Click += new System.EventHandler(this.btnCPUDB_Click);
            // 
            // btnProgramCR
            // 
            this.btnProgramCR.Location = new System.Drawing.Point(9, 22);
            this.btnProgramCR.Name = "btnProgramCR";
            this.btnProgramCR.Size = new System.Drawing.Size(66, 51);
            this.btnProgramCR.TabIndex = 26;
            this.btnProgramCR.Text = "Program\r\nTiming File";
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
            this.groupBox3.Size = new System.Drawing.Size(373, 86);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Nand";
            // 
            // btnCreateECC
            // 
            this.btnCreateECC.Location = new System.Drawing.Point(82, 22);
            this.btnCreateECC.Name = "btnCreateECC";
            this.btnCreateECC.Size = new System.Drawing.Size(65, 51);
            this.btnCreateECC.TabIndex = 21;
            this.btnCreateECC.Text = "Create\r\nXeLL";
            this.toolTip1.SetToolTip(this.btnCreateECC, "Creates a XeLL image from the source file, and the settings selected on the XeBui" +
        "ld panel");
            this.btnCreateECC.UseVisualStyleBackColor = true;
            this.btnCreateECC.Click += new System.EventHandler(this.btnCreateECC_Click);
            // 
            // btnXeBuild
            // 
            this.btnXeBuild.Location = new System.Drawing.Point(226, 22);
            this.btnXeBuild.Name = "btnXeBuild";
            this.btnXeBuild.Size = new System.Drawing.Size(65, 51);
            this.btnXeBuild.TabIndex = 24;
            this.btnXeBuild.Text = "Create\r\nXeBuild";
            this.toolTip1.SetToolTip(this.btnXeBuild, "Creates a Freeboot XeBuild image from the source file, and the settings selected " +
        "on the XeBuild panel");
            this.btnXeBuild.UseVisualStyleBackColor = true;
            this.btnXeBuild.Click += new System.EventHandler(this.btnXeBuild_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(298, 22);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(65, 51);
            this.btnWrite.TabIndex = 25;
            this.btnWrite.Text = "Write\r\nNand";
            this.toolTip1.SetToolTip(this.btnWrite, "Writes the source to the nand");
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(10, 22);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(65, 51);
            this.btnRead.TabIndex = 20;
            this.btnRead.Text = "Read\r\nNand";
            this.toolTip1.SetToolTip(this.btnRead, "Reads the nand the amount of times selected below");
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnWriteECC
            // 
            this.btnWriteECC.Location = new System.Drawing.Point(154, 22);
            this.btnWriteECC.Name = "btnWriteECC";
            this.btnWriteECC.Size = new System.Drawing.Size(65, 51);
            this.btnWriteECC.TabIndex = 23;
            this.btnWriteECC.Text = "Write\r\nXeLL";
            this.toolTip1.SetToolTip(this.btnWriteECC, "Writes a XeLL image to the nand");
            this.btnWriteECC.UseVisualStyleBackColor = true;
            this.btnWriteECC.Click += new System.EventHandler(this.btnWriteECC_Click);
            // 
            // lblNReads
            // 
            this.lblNReads.AutoSize = true;
            this.lblNReads.Location = new System.Drawing.Point(6, 14);
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
            this.numericIterations.Location = new System.Drawing.Point(8, 31);
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
            this.numericIterations.TabIndex = 27;
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
            this.pBoxDevice.Location = new System.Drawing.Point(281, 92);
            this.pBoxDevice.Name = "pBoxDevice";
            this.pBoxDevice.Size = new System.Drawing.Size(180, 76);
            this.pBoxDevice.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pBoxDevice.TabIndex = 83;
            this.pBoxDevice.TabStop = false;
            this.pBoxDevice.Click += new System.EventHandler(this.pBoxDevice_Click);
            // 
            // btnExtractFiles
            // 
            this.btnExtractFiles.Location = new System.Drawing.Point(136, 106);
            this.btnExtractFiles.Name = "btnExtractFiles";
            this.btnExtractFiles.Size = new System.Drawing.Size(65, 51);
            this.btnExtractFiles.TabIndex = 28;
            this.btnExtractFiles.Text = "Extract\r\nFiles";
            this.toolTip1.SetToolTip(this.btnExtractFiles, "Extracts the files from the source nand");
            this.btnExtractFiles.UseVisualStyleBackColor = true;
            this.btnExtractFiles.Click += new System.EventHandler(this.btnExtractFiles_Click);
            // 
            // btnCreateDonor
            // 
            this.btnCreateDonor.Location = new System.Drawing.Point(64, 106);
            this.btnCreateDonor.Name = "btnCreateDonor";
            this.btnCreateDonor.Size = new System.Drawing.Size(65, 51);
            this.btnCreateDonor.TabIndex = 30;
            this.btnCreateDonor.Text = "Create\r\nDonor";
            this.toolTip1.SetToolTip(this.btnCreateDonor, "Launches the Create Donor Nand Wizard");
            this.btnCreateDonor.UseVisualStyleBackColor = true;
            this.btnCreateDonor.Click += new System.EventHandler(this.btnCreateDonor_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblNReads);
            this.groupBox1.Controls.Add(this.numericIterations);
            this.groupBox1.Location = new System.Drawing.Point(3, 101);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(53, 56);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Nand";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnProgramCR);
            this.groupBox2.Location = new System.Drawing.Point(381, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(84, 86);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Glitch Chip";
            // 
            // NandTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnExtractFiles);
            this.Controls.Add(this.btnCreateDonor);
            this.Controls.Add(this.btnCPUDB);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pBoxDevice);
            this.Controls.Add(this.groupBox3);
            this.Name = "NandTools";
            this.Size = new System.Drawing.Size(465, 173);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericIterations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxDevice)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pBoxDevice;
        private System.Windows.Forms.Button btnCPUDB;
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
        private System.Windows.Forms.Button btnExtractFiles;
        private System.Windows.Forms.Button btnCreateDonor;
    }
}
