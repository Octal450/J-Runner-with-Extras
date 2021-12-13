namespace JRunner
{
    partial class NandProArg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NandProArg));
            this.btnRun = new System.Windows.Forms.Button();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.txtLength = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.btnfile = new System.Windows.Forms.Button();
            this.Commandgrp = new System.Windows.Forms.GroupBox();
            this.xsvfbtn = new System.Windows.Forms.RadioButton();
            this.erasebtn = new System.Windows.Forms.RadioButton();
            this.writebtn = new System.Windows.Forms.RadioButton();
            this.ReadBtn = new System.Windows.Forms.RadioButton();
            this.sizebox = new System.Windows.Forms.GroupBox();
            this.btn512 = new System.Windows.Forms.RadioButton();
            this.btn256 = new System.Windows.Forms.RadioButton();
            this.btn64 = new System.Windows.Forms.RadioButton();
            this.btn16 = new System.Windows.Forms.RadioButton();
            this.Optionalbox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ProgramCRButton = new System.Windows.Forms.Button();
            this.Commandgrp.SuspendLayout();
            this.sizebox.SuspendLayout();
            this.Optionalbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(304, 111);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(65, 23);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtStart
            // 
            this.txtStart.Location = new System.Drawing.Point(71, 20);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(49, 20);
            this.txtStart.TabIndex = 1;
            // 
            // txtLength
            // 
            this.txtLength.Location = new System.Drawing.Point(71, 48);
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(49, 20);
            this.txtLength.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(146, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Source:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Start Block";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Length";
            // 
            // txtFilename
            // 
            this.txtFilename.AllowDrop = true;
            this.txtFilename.Location = new System.Drawing.Point(148, 28);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(220, 20);
            this.txtFilename.TabIndex = 3;
            this.txtFilename.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFilename_DragDrop);
            this.txtFilename.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtFilename_DragEnter);
            // 
            // btnfile
            // 
            this.btnfile.Location = new System.Drawing.Point(340, 53);
            this.btnfile.Name = "btnfile";
            this.btnfile.Size = new System.Drawing.Size(29, 23);
            this.btnfile.TabIndex = 4;
            this.btnfile.Text = "...";
            this.btnfile.UseVisualStyleBackColor = true;
            this.btnfile.Click += new System.EventHandler(this.btnfile_Click);
            // 
            // Commandgrp
            // 
            this.Commandgrp.Controls.Add(this.xsvfbtn);
            this.Commandgrp.Controls.Add(this.erasebtn);
            this.Commandgrp.Controls.Add(this.writebtn);
            this.Commandgrp.Controls.Add(this.ReadBtn);
            this.Commandgrp.Location = new System.Drawing.Point(13, 12);
            this.Commandgrp.Name = "Commandgrp";
            this.Commandgrp.Size = new System.Drawing.Size(69, 122);
            this.Commandgrp.TabIndex = 1;
            this.Commandgrp.TabStop = false;
            this.Commandgrp.Text = "Command";
            // 
            // xsvfbtn
            // 
            this.xsvfbtn.AutoSize = true;
            this.xsvfbtn.Location = new System.Drawing.Point(10, 92);
            this.xsvfbtn.Name = "xsvfbtn";
            this.xsvfbtn.Size = new System.Drawing.Size(52, 17);
            this.xsvfbtn.TabIndex = 3;
            this.xsvfbtn.TabStop = true;
            this.xsvfbtn.Text = "XSVF";
            this.xsvfbtn.UseVisualStyleBackColor = true;
            this.xsvfbtn.CheckedChanged += new System.EventHandler(this.xsvfbtn_CheckedChanged);
            // 
            // erasebtn
            // 
            this.erasebtn.AutoSize = true;
            this.erasebtn.Location = new System.Drawing.Point(10, 68);
            this.erasebtn.Name = "erasebtn";
            this.erasebtn.Size = new System.Drawing.Size(52, 17);
            this.erasebtn.TabIndex = 2;
            this.erasebtn.TabStop = true;
            this.erasebtn.Text = "Erase";
            this.erasebtn.UseVisualStyleBackColor = true;
            this.erasebtn.CheckedChanged += new System.EventHandler(this.erasebtn_CheckedChanged);
            // 
            // writebtn
            // 
            this.writebtn.AutoSize = true;
            this.writebtn.Location = new System.Drawing.Point(10, 44);
            this.writebtn.Name = "writebtn";
            this.writebtn.Size = new System.Drawing.Size(50, 17);
            this.writebtn.TabIndex = 1;
            this.writebtn.TabStop = true;
            this.writebtn.Text = "Write";
            this.writebtn.UseVisualStyleBackColor = true;
            this.writebtn.CheckedChanged += new System.EventHandler(this.writebtn_CheckedChanged);
            // 
            // ReadBtn
            // 
            this.ReadBtn.AutoSize = true;
            this.ReadBtn.Location = new System.Drawing.Point(10, 20);
            this.ReadBtn.Name = "ReadBtn";
            this.ReadBtn.Size = new System.Drawing.Size(51, 17);
            this.ReadBtn.TabIndex = 0;
            this.ReadBtn.TabStop = true;
            this.ReadBtn.Text = "Read";
            this.ReadBtn.UseVisualStyleBackColor = true;
            this.ReadBtn.CheckedChanged += new System.EventHandler(this.ReadBtn_CheckedChanged);
            // 
            // sizebox
            // 
            this.sizebox.Controls.Add(this.btn512);
            this.sizebox.Controls.Add(this.btn256);
            this.sizebox.Controls.Add(this.btn64);
            this.sizebox.Controls.Add(this.btn16);
            this.sizebox.Location = new System.Drawing.Point(89, 12);
            this.sizebox.Name = "sizebox";
            this.sizebox.Size = new System.Drawing.Size(52, 122);
            this.sizebox.TabIndex = 2;
            this.sizebox.TabStop = false;
            this.sizebox.Text = "Size";
            // 
            // btn512
            // 
            this.btn512.Location = new System.Drawing.Point(7, 92);
            this.btn512.Name = "btn512";
            this.btn512.Size = new System.Drawing.Size(43, 17);
            this.btn512.TabIndex = 7;
            this.btn512.TabStop = true;
            this.btn512.Text = "512";
            this.btn512.UseVisualStyleBackColor = true;
            this.btn512.CheckedChanged += new System.EventHandler(this.btn512_CheckedChanged);
            // 
            // btn256
            // 
            this.btn256.AutoSize = true;
            this.btn256.Location = new System.Drawing.Point(7, 68);
            this.btn256.Name = "btn256";
            this.btn256.Size = new System.Drawing.Size(43, 17);
            this.btn256.TabIndex = 6;
            this.btn256.TabStop = true;
            this.btn256.Text = "256";
            this.btn256.UseVisualStyleBackColor = true;
            this.btn256.CheckedChanged += new System.EventHandler(this.btn256_CheckedChanged);
            // 
            // btn64
            // 
            this.btn64.AutoSize = true;
            this.btn64.Location = new System.Drawing.Point(7, 44);
            this.btn64.Name = "btn64";
            this.btn64.Size = new System.Drawing.Size(37, 17);
            this.btn64.TabIndex = 5;
            this.btn64.TabStop = true;
            this.btn64.Text = "64";
            this.btn64.UseVisualStyleBackColor = true;
            this.btn64.CheckedChanged += new System.EventHandler(this.btn64_CheckedChanged);
            // 
            // btn16
            // 
            this.btn16.AutoSize = true;
            this.btn16.Location = new System.Drawing.Point(7, 20);
            this.btn16.Name = "btn16";
            this.btn16.Size = new System.Drawing.Size(37, 17);
            this.btn16.TabIndex = 4;
            this.btn16.TabStop = true;
            this.btn16.Text = "16";
            this.btn16.UseVisualStyleBackColor = true;
            this.btn16.CheckedChanged += new System.EventHandler(this.btn16_CheckedChanged);
            // 
            // Optionalbox
            // 
            this.Optionalbox.Controls.Add(this.label4);
            this.Optionalbox.Controls.Add(this.txtStart);
            this.Optionalbox.Controls.Add(this.txtLength);
            this.Optionalbox.Controls.Add(this.label5);
            this.Optionalbox.Location = new System.Drawing.Point(148, 55);
            this.Optionalbox.Name = "Optionalbox";
            this.Optionalbox.Size = new System.Drawing.Size(133, 79);
            this.Optionalbox.TabIndex = 5;
            this.Optionalbox.TabStop = false;
            this.Optionalbox.Text = "Optional";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "J-Runner with Extras includes RGH1, RGH1.2, S-RGH, and other timings";
            // 
            // ProgramCRButton
            // 
            this.ProgramCRButton.Location = new System.Drawing.Point(12, 167);
            this.ProgramCRButton.Name = "ProgramCRButton";
            this.ProgramCRButton.Size = new System.Drawing.Size(357, 22);
            this.ProgramCRButton.TabIndex = 10;
            this.ProgramCRButton.Text = "Open Program Timing File Menu";
            this.ProgramCRButton.UseVisualStyleBackColor = true;
            this.ProgramCRButton.Click += new System.EventHandler(this.ProgramCRButton_Click);
            // 
            // NandProArg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 199);
            this.Controls.Add(this.ProgramCRButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Optionalbox);
            this.Controls.Add(this.sizebox);
            this.Controls.Add(this.Commandgrp);
            this.Controls.Add(this.btnfile);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnRun);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "NandProArg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Custom Nand/Timing File Functions";
            this.Commandgrp.ResumeLayout(false);
            this.Commandgrp.PerformLayout();
            this.sizebox.ResumeLayout(false);
            this.sizebox.PerformLayout();
            this.Optionalbox.ResumeLayout(false);
            this.Optionalbox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.TextBox txtLength;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Button btnfile;
        private System.Windows.Forms.GroupBox Commandgrp;
        private System.Windows.Forms.RadioButton ReadBtn;
        private System.Windows.Forms.RadioButton xsvfbtn;
        private System.Windows.Forms.RadioButton erasebtn;
        private System.Windows.Forms.RadioButton writebtn;
        private System.Windows.Forms.GroupBox sizebox;
        private System.Windows.Forms.RadioButton btn512;
        private System.Windows.Forms.RadioButton btn256;
        private System.Windows.Forms.RadioButton btn64;
        private System.Windows.Forms.RadioButton btn16;
        private System.Windows.Forms.GroupBox Optionalbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ProgramCRButton;
    }
}