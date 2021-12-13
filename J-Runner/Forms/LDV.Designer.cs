namespace JRunner.Forms
{
    partial class xeBuildOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(xeBuildOptions));
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.chkAdvanced = new System.Windows.Forms.CheckBox();
            this.txtCBA = new System.Windows.Forms.TextBox();
            this.txtCBB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(197, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose LDV:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(88, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(52, 20);
            this.textBox1.TabIndex = 2;
            // 
            // chkAdvanced
            // 
            this.chkAdvanced.AutoSize = true;
            this.chkAdvanced.Location = new System.Drawing.Point(197, 39);
            this.chkAdvanced.Name = "chkAdvanced";
            this.chkAdvanced.Size = new System.Drawing.Size(75, 17);
            this.chkAdvanced.TabIndex = 3;
            this.chkAdvanced.Text = "Advanced";
            this.chkAdvanced.UseVisualStyleBackColor = true;
            this.chkAdvanced.CheckedChanged += new System.EventHandler(this.chkAdvanced_CheckedChanged);
            // 
            // txtCBA
            // 
            this.txtCBA.Location = new System.Drawing.Point(88, 36);
            this.txtCBA.Name = "txtCBA";
            this.txtCBA.Size = new System.Drawing.Size(52, 20);
            this.txtCBA.TabIndex = 4;
            this.txtCBA.Visible = false;
            // 
            // txtCBB
            // 
            this.txtCBB.Location = new System.Drawing.Point(53, 62);
            this.txtCBB.Name = "txtCBB";
            this.txtCBB.Size = new System.Drawing.Size(52, 20);
            this.txtCBB.TabIndex = 5;
            this.txtCBB.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "CB_A:";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "CB_B";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(111, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Leave blank if not split cb";
            this.label4.Visible = false;
            // 
            // xeBuildOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 62);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCBB);
            this.Controls.Add(this.txtCBA);
            this.Controls.Add(this.chkAdvanced);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "xeBuildOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "XeBuild Options";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox chkAdvanced;
        private System.Windows.Forms.TextBox txtCBA;
        private System.Windows.Forms.TextBox txtCBB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}