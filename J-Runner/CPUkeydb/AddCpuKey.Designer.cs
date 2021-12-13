namespace JRunner
{
    partial class AddCpuKey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddCpuKey));
            this.lblNand = new System.Windows.Forms.Label();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.ldlCpukey = new System.Windows.Forms.Label();
            this.btnFile = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.txtCpuKey = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblNand
            // 
            this.lblNand.AutoSize = true;
            this.lblNand.Location = new System.Drawing.Point(24, 16);
            this.lblNand.Name = "lblNand";
            this.lblNand.Size = new System.Drawing.Size(33, 13);
            this.lblNand.TabIndex = 0;
            this.lblNand.Text = "Nand";
            // 
            // txtFilename
            // 
            this.txtFilename.AllowDrop = true;
            this.txtFilename.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtFilename.Location = new System.Drawing.Point(27, 32);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.ReadOnly = true;
            this.txtFilename.Size = new System.Drawing.Size(200, 20);
            this.txtFilename.TabIndex = 1;
            this.txtFilename.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFilename.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            this.txtFilename.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBox1_DragEnter);
            // 
            // ldlCpukey
            // 
            this.ldlCpukey.AutoSize = true;
            this.ldlCpukey.Location = new System.Drawing.Point(24, 68);
            this.ldlCpukey.Name = "ldlCpukey";
            this.ldlCpukey.Size = new System.Drawing.Size(50, 13);
            this.ldlCpukey.TabIndex = 2;
            this.ldlCpukey.Text = "CPU Key";
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(239, 30);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(28, 23);
            this.btnFile.TabIndex = 4;
            this.btnFile.Text = "...";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(99, 119);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtCpuKey
            // 
            this.txtCpuKey.AllowDrop = true;
            this.txtCpuKey.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCpuKey.Location = new System.Drawing.Point(27, 84);
            this.txtCpuKey.MaxLength = 32;
            this.txtCpuKey.Name = "txtCpuKey";
            this.txtCpuKey.Size = new System.Drawing.Size(234, 20);
            this.txtCpuKey.TabIndex = 24;
            this.txtCpuKey.TextChanged += new System.EventHandler(this.cpukeytext_TextChanged);
            this.txtCpuKey.DragDrop += new System.Windows.Forms.DragEventHandler(this.cpukeytext_DragDrop);
            this.txtCpuKey.DragEnter += new System.Windows.Forms.DragEventHandler(this.cpukeytext_DragEnter);
            // 
            // AddCpuKey
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 154);
            this.Controls.Add(this.txtCpuKey);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.ldlCpukey);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.lblNand);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddCpuKey";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Entry";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNand;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Label ldlCpukey;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtCpuKey;
    }
}