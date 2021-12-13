namespace JRunner
{
    partial class addDash
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
            this.checkedDashes = new System.Windows.Forms.CheckedListBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkAdvanced = new System.Windows.Forms.CheckBox();
            this.textAdvAdd = new System.Windows.Forms.TextBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkedDashes
            // 
            this.checkedDashes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedDashes.FormattingEnabled = true;
            this.checkedDashes.Location = new System.Drawing.Point(8, 33);
            this.checkedDashes.Name = "checkedDashes";
            this.checkedDashes.Size = new System.Drawing.Size(196, 139);
            this.checkedDashes.Sorted = true;
            this.checkedDashes.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.Location = new System.Drawing.Point(26, 178);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(164, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "Add Dashes";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkAdvanced
            // 
            this.checkAdvanced.AutoSize = true;
            this.checkAdvanced.Location = new System.Drawing.Point(134, 12);
            this.checkAdvanced.Name = "checkAdvanced";
            this.checkAdvanced.Size = new System.Drawing.Size(75, 17);
            this.checkAdvanced.TabIndex = 2;
            this.checkAdvanced.Text = "Advanced";
            this.checkAdvanced.UseVisualStyleBackColor = true;
            this.checkAdvanced.CheckedChanged += new System.EventHandler(this.checkAdvanced_CheckedChanged);
            // 
            // textAdvAdd
            // 
            this.textAdvAdd.Location = new System.Drawing.Point(12, 10);
            this.textAdvAdd.MaxLength = 5;
            this.textAdvAdd.Name = "textAdvAdd";
            this.textAdvAdd.Size = new System.Drawing.Size(67, 20);
            this.textAdvAdd.TabIndex = 3;
            this.textAdvAdd.Visible = false;
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(71, 207);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(85, 8);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(26, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // addDash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(216, 239);
            this.ControlBox = false;
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.textAdvAdd);
            this.Controls.Add(this.checkAdvanced);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.checkedDashes);
            this.Name = "addDash";
            this.ShowInTaskbar = false;
            this.Text = "Add Dash";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedDashes;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkAdvanced;
        private System.Windows.Forms.TextBox textAdvAdd;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button btnAdd;
    }
}