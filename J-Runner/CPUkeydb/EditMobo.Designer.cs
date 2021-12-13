namespace JRunner.CPUkeydb
{
    partial class Editmobo
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
        private void InitializeComponent( )
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editmobo));
            this.SerialTB = new System.Windows.Forms.TextBox();
            this.CpukeyTB = new System.Windows.Forms.TextBox();
            this.ConsTypeTB = new System.Windows.Forms.TextBox();
            this.EditOKBut = new System.Windows.Forms.Button();
            this.CancelBut = new System.Windows.Forms.Button();
            this.IdentTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SerialTB
            // 
            this.SerialTB.Enabled = false;
            this.SerialTB.ForeColor = System.Drawing.Color.Red;
            this.SerialTB.Location = new System.Drawing.Point(64, 45);
            this.SerialTB.Name = "SerialTB";
            this.SerialTB.Size = new System.Drawing.Size(157, 20);
            this.SerialTB.TabIndex = 0;
            this.SerialTB.TabStop = false;
            this.SerialTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CpukeyTB
            // 
            this.CpukeyTB.Enabled = false;
            this.CpukeyTB.ForeColor = System.Drawing.Color.Red;
            this.CpukeyTB.Location = new System.Drawing.Point(64, 78);
            this.CpukeyTB.Name = "CpukeyTB";
            this.CpukeyTB.Size = new System.Drawing.Size(213, 20);
            this.CpukeyTB.TabIndex = 1;
            this.CpukeyTB.TabStop = false;
            this.CpukeyTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ConsTypeTB
            // 
            this.ConsTypeTB.Location = new System.Drawing.Point(64, 111);
            this.ConsTypeTB.Name = "ConsTypeTB";
            this.ConsTypeTB.Size = new System.Drawing.Size(157, 20);
            this.ConsTypeTB.TabIndex = 0;
            this.ConsTypeTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // EditOKBut
            // 
            this.EditOKBut.Location = new System.Drawing.Point(63, 143);
            this.EditOKBut.Name = "EditOKBut";
            this.EditOKBut.Size = new System.Drawing.Size(75, 23);
            this.EditOKBut.TabIndex = 3;
            this.EditOKBut.Text = "Save";
            this.EditOKBut.UseVisualStyleBackColor = true;
            this.EditOKBut.Click += new System.EventHandler(this.EditOKBut_Click);
            // 
            // CancelBut
            // 
            this.CancelBut.Location = new System.Drawing.Point(153, 143);
            this.CancelBut.Name = "CancelBut";
            this.CancelBut.Size = new System.Drawing.Size(75, 23);
            this.CancelBut.TabIndex = 4;
            this.CancelBut.Text = "Cancel";
            this.CancelBut.UseVisualStyleBackColor = true;
            this.CancelBut.Click += new System.EventHandler(this.CancelBut_Click);
            // 
            // IdentTB
            // 
            this.IdentTB.Enabled = false;
            this.IdentTB.ForeColor = System.Drawing.Color.Red;
            this.IdentTB.Location = new System.Drawing.Point(64, 13);
            this.IdentTB.Name = "IdentTB";
            this.IdentTB.Size = new System.Drawing.Size(49, 20);
            this.IdentTB.TabIndex = 5;
            this.IdentTB.TabStop = false;
            this.IdentTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Serial:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "CPU Key:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Console: ";
            // 
            // Editmobo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 175);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IdentTB);
            this.Controls.Add(this.CancelBut);
            this.Controls.Add(this.EditOKBut);
            this.Controls.Add(this.ConsTypeTB);
            this.Controls.Add(this.CpukeyTB);
            this.Controls.Add(this.SerialTB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Editmobo";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Console Type";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Editmobo_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SerialTB;
        private System.Windows.Forms.TextBox CpukeyTB;
        private System.Windows.Forms.TextBox ConsTypeTB;
        private System.Windows.Forms.Button EditOKBut;
        private System.Windows.Forms.Button CancelBut;
        private System.Windows.Forms.TextBox IdentTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}