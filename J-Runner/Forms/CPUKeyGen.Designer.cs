
namespace JRunner.Forms
{
    partial class CPUKeyGen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CPUKeyGen));
            this.txtGenKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGenKey = new System.Windows.Forms.Button();
            this.btnFun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtGenKey
            // 
            this.txtGenKey.Location = new System.Drawing.Point(67, 8);
            this.txtGenKey.Margin = new System.Windows.Forms.Padding(2);
            this.txtGenKey.Name = "txtGenKey";
            this.txtGenKey.Size = new System.Drawing.Size(219, 20);
            this.txtGenKey.TabIndex = 0;
            this.txtGenKey.TextChanged += new System.EventHandler(this.txtGenKey_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "CPU Key:";
            // 
            // btnGenKey
            // 
            this.btnGenKey.Location = new System.Drawing.Point(67, 34);
            this.btnGenKey.Margin = new System.Windows.Forms.Padding(2);
            this.btnGenKey.Name = "btnGenKey";
            this.btnGenKey.Size = new System.Drawing.Size(161, 38);
            this.btnGenKey.TabIndex = 2;
            this.btnGenKey.Text = "Generate CPU Key";
            this.btnGenKey.UseVisualStyleBackColor = true;
            this.btnGenKey.Click += new System.EventHandler(this.btnGenKey_Click);
            // 
            // btnFun
            // 
            this.btnFun.Location = new System.Drawing.Point(232, 34);
            this.btnFun.Margin = new System.Windows.Forms.Padding(2);
            this.btnFun.Name = "btnFun";
            this.btnFun.Size = new System.Drawing.Size(54, 38);
            this.btnFun.TabIndex = 3;
            this.btnFun.Text = "Fun Keys";
            this.btnFun.UseVisualStyleBackColor = true;
            this.btnFun.Visible = false;
            this.btnFun.Click += new System.EventHandler(this.btnFun_Click);
            // 
            // CPUKeyGen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 79);
            this.Controls.Add(this.btnFun);
            this.Controls.Add(this.btnGenKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtGenKey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CPUKeyGen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CPU Key Generation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtGenKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGenKey;
        private System.Windows.Forms.Button btnFun;
    }
}