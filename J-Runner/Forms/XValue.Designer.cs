namespace JRunner.Forms
{
    partial class XValue
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XValue));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblSerial = new System.Windows.Forms.Label();
            this.lblXVal = new System.Windows.Forms.Label();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.txtXVal = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(59, 62);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Decrypt";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(153, 62);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblSerial
            // 
            this.lblSerial.AutoSize = true;
            this.lblSerial.Location = new System.Drawing.Point(7, 9);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(39, 13);
            this.lblSerial.TabIndex = 2;
            this.lblSerial.Text = "Serial: ";
            // 
            // lblXVal
            // 
            this.lblXVal.AutoSize = true;
            this.lblXVal.Location = new System.Drawing.Point(7, 37);
            this.lblXVal.Name = "lblXVal";
            this.lblXVal.Size = new System.Drawing.Size(47, 13);
            this.lblXVal.TabIndex = 3;
            this.lblXVal.Text = "X Value:";
            // 
            // txtSerial
            // 
            this.txtSerial.Location = new System.Drawing.Point(60, 6);
            this.txtSerial.MaxLength = 12;
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.Size = new System.Drawing.Size(133, 20);
            this.txtSerial.TabIndex = 4;
            this.txtSerial.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSerial_KeyPress);
            // 
            // txtXVal
            // 
            this.txtXVal.Location = new System.Drawing.Point(60, 34);
            this.txtXVal.MaxLength = 16;
            this.txtXVal.Name = "txtXVal";
            this.txtXVal.Size = new System.Drawing.Size(196, 20);
            this.txtXVal.TabIndex = 5;
            this.txtXVal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtXVal_KeyPress);
            // 
            // XValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(284, 94);
            this.Controls.Add(this.txtXVal);
            this.Controls.Add(this.txtSerial);
            this.Controls.Add(this.lblXVal);
            this.Controls.Add(this.lblSerial);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "XValue";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "X Value";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblSerial;
        private System.Windows.Forms.Label lblXVal;
        private System.Windows.Forms.TextBox txtSerial;
        private System.Windows.Forms.TextBox txtXVal;
    }
}