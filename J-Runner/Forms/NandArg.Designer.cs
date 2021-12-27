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
            this.components = new System.ComponentModel.Container();
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
            this.readbtn = new System.Windows.Forms.RadioButton();
            this.sizebox = new System.Windows.Forms.GroupBox();
            this.btn512 = new System.Windows.Forms.RadioButton();
            this.btn256 = new System.Windows.Forms.RadioButton();
            this.btn64 = new System.Windows.Forms.RadioButton();
            this.btn16 = new System.Windows.Forms.RadioButton();
            this.optionalbox = new System.Windows.Forms.GroupBox();
            this.chkOptional = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Commandgrp.SuspendLayout();
            this.sizebox.SuspendLayout();
            this.optionalbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(299, 110);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(109, 24);
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
            this.txtStart.TextChanged += new System.EventHandler(this.txtStartLength_TextChanged);
            // 
            // txtLength
            // 
            this.txtLength.Location = new System.Drawing.Point(71, 48);
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(49, 20);
            this.txtLength.TabIndex = 2;
            this.txtLength.TextChanged += new System.EventHandler(this.txtStartLength_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(151, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "File:";
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
            this.txtFilename.Location = new System.Drawing.Point(154, 28);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(219, 20);
            this.txtFilename.TabIndex = 3;
            this.txtFilename.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFilename_DragDrop);
            this.txtFilename.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtFilename_DragEnter);
            // 
            // btnfile
            // 
            this.btnfile.Location = new System.Drawing.Point(379, 27);
            this.btnfile.Name = "btnfile";
            this.btnfile.Size = new System.Drawing.Size(29, 22);
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
            this.Commandgrp.Controls.Add(this.readbtn);
            this.Commandgrp.Location = new System.Drawing.Point(13, 12);
            this.Commandgrp.Name = "Commandgrp";
            this.Commandgrp.Size = new System.Drawing.Size(68, 122);
            this.Commandgrp.TabIndex = 1;
            this.Commandgrp.TabStop = false;
            this.Commandgrp.Text = "Command";
            // 
            // xsvfbtn
            // 
            this.xsvfbtn.AutoSize = true;
            this.xsvfbtn.Location = new System.Drawing.Point(11, 95);
            this.xsvfbtn.Name = "xsvfbtn";
            this.xsvfbtn.Size = new System.Drawing.Size(45, 17);
            this.xsvfbtn.TabIndex = 3;
            this.xsvfbtn.Text = "SVF";
            this.xsvfbtn.UseVisualStyleBackColor = true;
            this.xsvfbtn.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // erasebtn
            // 
            this.erasebtn.AutoSize = true;
            this.erasebtn.Location = new System.Drawing.Point(11, 70);
            this.erasebtn.Name = "erasebtn";
            this.erasebtn.Size = new System.Drawing.Size(52, 17);
            this.erasebtn.TabIndex = 2;
            this.erasebtn.Text = "Erase";
            this.erasebtn.UseVisualStyleBackColor = true;
            this.erasebtn.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // writebtn
            // 
            this.writebtn.AutoSize = true;
            this.writebtn.Location = new System.Drawing.Point(11, 45);
            this.writebtn.Name = "writebtn";
            this.writebtn.Size = new System.Drawing.Size(50, 17);
            this.writebtn.TabIndex = 1;
            this.writebtn.Text = "Write";
            this.writebtn.UseVisualStyleBackColor = true;
            this.writebtn.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // readbtn
            // 
            this.readbtn.AutoSize = true;
            this.readbtn.Checked = true;
            this.readbtn.Location = new System.Drawing.Point(11, 20);
            this.readbtn.Name = "readbtn";
            this.readbtn.Size = new System.Drawing.Size(51, 17);
            this.readbtn.TabIndex = 0;
            this.readbtn.TabStop = true;
            this.readbtn.Text = "Read";
            this.readbtn.UseVisualStyleBackColor = true;
            this.readbtn.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // sizebox
            // 
            this.sizebox.Controls.Add(this.btn512);
            this.sizebox.Controls.Add(this.btn256);
            this.sizebox.Controls.Add(this.btn64);
            this.sizebox.Controls.Add(this.btn16);
            this.sizebox.Location = new System.Drawing.Point(88, 12);
            this.sizebox.Name = "sizebox";
            this.sizebox.Size = new System.Drawing.Size(59, 122);
            this.sizebox.TabIndex = 2;
            this.sizebox.TabStop = false;
            this.sizebox.Text = "Size";
            // 
            // btn512
            // 
            this.btn512.Location = new System.Drawing.Point(11, 95);
            this.btn512.Name = "btn512";
            this.btn512.Size = new System.Drawing.Size(43, 17);
            this.btn512.TabIndex = 7;
            this.btn512.Text = "512";
            this.btn512.UseVisualStyleBackColor = true;
            this.btn512.CheckedChanged += new System.EventHandler(this.btn512_CheckedChanged);
            // 
            // btn256
            // 
            this.btn256.AutoSize = true;
            this.btn256.Location = new System.Drawing.Point(11, 70);
            this.btn256.Name = "btn256";
            this.btn256.Size = new System.Drawing.Size(43, 17);
            this.btn256.TabIndex = 6;
            this.btn256.Text = "256";
            this.btn256.UseVisualStyleBackColor = true;
            this.btn256.CheckedChanged += new System.EventHandler(this.btn256_CheckedChanged);
            // 
            // btn64
            // 
            this.btn64.AutoSize = true;
            this.btn64.Location = new System.Drawing.Point(11, 45);
            this.btn64.Name = "btn64";
            this.btn64.Size = new System.Drawing.Size(37, 17);
            this.btn64.TabIndex = 5;
            this.btn64.Text = "64";
            this.btn64.UseVisualStyleBackColor = true;
            this.btn64.CheckedChanged += new System.EventHandler(this.btn64_CheckedChanged);
            // 
            // btn16
            // 
            this.btn16.AutoSize = true;
            this.btn16.Checked = true;
            this.btn16.Location = new System.Drawing.Point(11, 20);
            this.btn16.Name = "btn16";
            this.btn16.Size = new System.Drawing.Size(37, 17);
            this.btn16.TabIndex = 4;
            this.btn16.TabStop = true;
            this.btn16.Text = "16";
            this.btn16.UseVisualStyleBackColor = true;
            this.btn16.CheckedChanged += new System.EventHandler(this.btn16_CheckedChanged);
            // 
            // optionalbox
            // 
            this.optionalbox.Controls.Add(this.chkOptional);
            this.optionalbox.Controls.Add(this.label4);
            this.optionalbox.Controls.Add(this.txtStart);
            this.optionalbox.Controls.Add(this.txtLength);
            this.optionalbox.Controls.Add(this.label5);
            this.optionalbox.Location = new System.Drawing.Point(154, 55);
            this.optionalbox.Name = "optionalbox";
            this.optionalbox.Size = new System.Drawing.Size(133, 79);
            this.optionalbox.TabIndex = 5;
            this.optionalbox.TabStop = false;
            this.optionalbox.Text = "      Optional";
            // 
            // chkOptional
            // 
            this.chkOptional.AutoSize = true;
            this.chkOptional.Location = new System.Drawing.Point(9, 0);
            this.chkOptional.Name = "chkOptional";
            this.chkOptional.Size = new System.Drawing.Size(15, 14);
            this.chkOptional.TabIndex = 11;
            this.chkOptional.UseVisualStyleBackColor = true;
            this.chkOptional.CheckedChanged += new System.EventHandler(this.chkOptional_CheckedChanged);
            // 
            // NandProArg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 146);
            this.Controls.Add(this.optionalbox);
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
            this.Text = "Nand/Timing File Functions";
            this.Commandgrp.ResumeLayout(false);
            this.Commandgrp.PerformLayout();
            this.sizebox.ResumeLayout(false);
            this.sizebox.PerformLayout();
            this.optionalbox.ResumeLayout(false);
            this.optionalbox.PerformLayout();
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
        private System.Windows.Forms.RadioButton readbtn;
        private System.Windows.Forms.RadioButton xsvfbtn;
        private System.Windows.Forms.RadioButton erasebtn;
        private System.Windows.Forms.RadioButton writebtn;
        private System.Windows.Forms.GroupBox sizebox;
        private System.Windows.Forms.RadioButton btn512;
        private System.Windows.Forms.RadioButton btn256;
        private System.Windows.Forms.RadioButton btn64;
        private System.Windows.Forms.RadioButton btn16;
        private System.Windows.Forms.GroupBox optionalbox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkOptional;
    }
}
