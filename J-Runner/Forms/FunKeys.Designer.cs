
namespace JRunner.Forms
{
    partial class FunKeys
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FunKeys));
            this.txtCPUBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtCPUBox
            // 
            this.txtCPUBox.BackColor = System.Drawing.Color.Black;
            this.txtCPUBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtCPUBox.ForeColor = System.Drawing.Color.White;
            this.txtCPUBox.Location = new System.Drawing.Point(10, 11);
            this.txtCPUBox.Multiline = true;
            this.txtCPUBox.Name = "txtCPUBox";
            this.txtCPUBox.ReadOnly = true;
            this.txtCPUBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCPUBox.Size = new System.Drawing.Size(358, 309);
            this.txtCPUBox.TabIndex = 12;
            this.txtCPUBox.TabStop = false;
            // 
            // FunKeys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 329);
            this.Controls.Add(this.txtCPUBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FunKeys";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fun Keys";
            this.Load += new System.EventHandler(this.FunKeys_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCPUBox;
    }
}