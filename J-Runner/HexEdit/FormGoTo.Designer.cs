namespace JRunner
{
    partial class FormGoTo
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
            this.btnOK = new System.Windows.Forms.Button();
            this.rbtnDec = new System.Windows.Forms.RadioButton();
            this.rbtnHex = new System.Windows.Forms.RadioButton();
            this.txtOffset = new System.Windows.Forms.TextBox();
            this.lblOffset = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(131, 49);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // rbtnDec
            // 
            this.rbtnDec.AutoSize = true;
            this.rbtnDec.Location = new System.Drawing.Point(15, 52);
            this.rbtnDec.Name = "rbtnDec";
            this.rbtnDec.Size = new System.Drawing.Size(45, 17);
            this.rbtnDec.TabIndex = 2;
            this.rbtnDec.Text = "Dec";
            this.rbtnDec.UseVisualStyleBackColor = true;
            this.rbtnDec.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // rbtnHex
            // 
            this.rbtnHex.AutoSize = true;
            this.rbtnHex.Checked = true;
            this.rbtnHex.Location = new System.Drawing.Point(62, 52);
            this.rbtnHex.Name = "rbtnHex";
            this.rbtnHex.Size = new System.Drawing.Size(44, 17);
            this.rbtnHex.TabIndex = 3;
            this.rbtnHex.TabStop = true;
            this.rbtnHex.Text = "Hex";
            this.rbtnHex.UseVisualStyleBackColor = true;
            this.rbtnHex.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // txtOffset
            // 
            this.txtOffset.Location = new System.Drawing.Point(62, 12);
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size(144, 20);
            this.txtOffset.TabIndex = 4;
            // 
            // lblOffset
            // 
            this.lblOffset.AutoSize = true;
            this.lblOffset.Location = new System.Drawing.Point(12, 15);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(35, 13);
            this.lblOffset.TabIndex = 5;
            this.lblOffset.Text = "Offset";
            // 
            // FormGoTo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 84);
            this.Controls.Add(this.lblOffset);
            this.Controls.Add(this.txtOffset);
            this.Controls.Add(this.rbtnHex);
            this.Controls.Add(this.rbtnDec);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGoTo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Go To";
            this.Activated += new System.EventHandler(this.FormGoTo_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton rbtnDec;
        private System.Windows.Forms.RadioButton rbtnHex;
        private System.Windows.Forms.TextBox txtOffset;
        private System.Windows.Forms.Label lblOffset;

    }
}