namespace JRunner
{
    partial class IP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IP));
            this.btnGetCpu = new System.Windows.Forms.Button();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGetCpu
            // 
            this.btnGetCpu.Location = new System.Drawing.Point(26, 60);
            this.btnGetCpu.Name = "btnGetCpu";
            this.btnGetCpu.Size = new System.Drawing.Size(75, 23);
            this.btnGetCpu.TabIndex = 0;
            this.btnGetCpu.Text = "Get CPU Key";
            this.btnGetCpu.UseVisualStyleBackColor = true;
            this.btnGetCpu.Click += new System.EventHandler(this.btnGetCpu_Click);
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(12, 25);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(109, 20);
            this.txtIP.TabIndex = 1;
            this.txtIP.Text = "192.168.1.";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(12, 9);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(23, 13);
            this.lblIP.TabIndex = 2;
            this.lblIP.Text = "IP :";
            // 
            // IP
            // 
            this.AcceptButton = this.btnGetCpu;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(133, 99);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.btnGetCpu);
            
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IP";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "IP Address";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetCpu;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label lblIP;
    }
}