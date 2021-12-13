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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoundEditor));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtnOther);
            this.groupBox1.Controls.Add(this.rbtnStandard);
            this.groupBox1.Controls.Add(this.rbtnHalo);
            this.groupBox1.Controls.Add(this.rbtnStarWars);
            this.groupBox1.Controls.Add(this.rbtnMW3);
            this.groupBox1.Controls.Add(this.rbtnGOW3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(137, 178);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Files";
            // 
            // rbtnOther
            // 
            this.rbtnOther.AutoSize = true;
            this.rbtnOther.Checked = true;
            this.rbtnOther.Location = new System.Drawing.Point(6, 134);
            this.rbtnOther.Name = "rbtnOther";
            this.rbtnOther.Size = new System.Drawing.Size(60, 17);
            this.rbtnOther.TabIndex = 5;
            this.rbtnOther.TabStop = true;
            this.rbtnOther.Text = "Custom";
            this.rbtnOther.UseVisualStyleBackColor = true;
            this.rbtnOther.CheckedChanged += new System.EventHandler(this.rbtnOther_CheckedChanged);
            // 
            // rbtnStandard
            // 
            this.rbtnStandard.AutoSize = true;
            this.rbtnStandard.Location = new System.Drawing.Point(6, 111);
            this.rbtnStandard.Name = "rbtnStandard";
            this.rbtnStandard.Size = new System.Drawing.Size(90, 17);
            this.rbtnStandard.TabIndex = 4;
            this.rbtnStandard.Text = "Standard Slim";
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
            this.rbtnGOW3.UseVisualStyleBackColor = true;
            this.rbtnGOW3.CheckedChanged += new System.EventHandler(this.rbtnFile_CheckedChanged);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(155, 12);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(141, 63);
            this.btnRead.TabIndex = 1;
            this.btnRead.Text = "Read Sound from Console";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(155, 150);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(141, 63);
            this.btnWrite.TabIndex = 2;
            this.btnWrite.Text = "Write Sound to Console";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(155, 81);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(141, 63);
            this.btnVerify.TabIndex = 3;
            this.btnVerify.Text = "Verify Sound against Console";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(268, 245);
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
            this.txtFile.Location = new System.Drawing.Point(12, 248);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(249, 20);
            this.txtFile.TabIndex = 5;
            this.txtFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFile_DragDrop);
            this.txtFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtFile_DragEnter);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 275);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(284, 10);
            this.progressBar1.TabIndex = 6;
            // 
            // btnPlayPower
            // 
            this.btnPlayPower.Location = new System.Drawing.Point(12, 219);
            this.btnPlayPower.Name = "btnPlayPower";
            this.btnPlayPower.Size = new System.Drawing.Size(137, 23);
            this.btnPlayPower.TabIndex = 7;
            this.btnPlayPower.Text = "Play Power Sound";
            this.btnPlayPower.UseVisualStyleBackColor = true;
            this.btnPlayPower.Click += new System.EventHandler(this.btnPlayPower_Click);
            // 
            // btnPlayEject
            // 
            this.btnPlayEject.Location = new System.Drawing.Point(155, 219);
            this.btnPlayEject.Name = "btnPlayEject";
            this.btnPlayEject.Size = new System.Drawing.Size(141, 23);
            this.btnPlayEject.TabIndex = 8;
            this.btnPlayEject.Text = "Play Eject Sound";
            this.btnPlayEject.UseVisualStyleBackColor = true;
            this.btnPlayEject.Click += new System.EventHandler(this.btnPlayEject_Click);
            // 
            // veraftreadchk
            // 
            this.veraftreadchk.AutoSize = true;
            this.veraftreadchk.Location = new System.Drawing.Point(18, 196);
            this.veraftreadchk.Name = "veraftreadchk";
            this.veraftreadchk.Size = new System.Drawing.Size(77, 17);
            this.veraftreadchk.TabIndex = 9;
            this.veraftreadchk.Text = "Auto-Verify";
            this.veraftreadchk.UseVisualStyleBackColor = true;
            this.veraftreadchk.CheckedChanged += new System.EventHandler(this.veraftreadchk_CheckedChanged);
            // 
            // SoundEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 293);
            this.Controls.Add(this.veraftreadchk);
            this.Controls.Add(this.btnPlayEject);
            this.Controls.Add(this.btnPlayPower);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SoundEditor";
            this.ShowInTaskbar = false;
            this.Text = "Xecuter Sonus 360 Editor";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SoundEditor_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtnOther;
        private System.Windows.Forms.RadioButton rbtnStandard;
        private System.Windows.Forms.RadioButton rbtnHalo;
        private System.Windows.Forms.RadioButton rbtnStarWars;
        private System.Windows.Forms.RadioButton rbtnMW3;
        private System.Windows.Forms.RadioButton rbtnGOW3;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnPlayPower;
        private System.Windows.Forms.Button btnPlayEject;
        private System.Windows.Forms.CheckBox veraftreadchk;
    }
}