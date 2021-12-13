
namespace JRunner.Forms
{
    partial class xFlasherNandSel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(xFlasherNandSel));
            this.btn16 = new System.Windows.Forms.Button();
            this.btn64 = new System.Windows.Forms.Button();
            this.SmallBlockGroup = new System.Windows.Forms.GroupBox();
            this.BigBlockGroup = new System.Windows.Forms.GroupBox();
            this.btn512 = new System.Windows.Forms.Button();
            this.btn256 = new System.Windows.Forms.Button();
            this.SmallBlockGroup.SuspendLayout();
            this.BigBlockGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn16
            // 
            this.btn16.Location = new System.Drawing.Point(6, 19);
            this.btn16.Name = "btn16";
            this.btn16.Size = new System.Drawing.Size(75, 23);
            this.btn16.TabIndex = 0;
            this.btn16.Text = "16MB";
            this.btn16.UseVisualStyleBackColor = true;
            this.btn16.Click += new System.EventHandler(this.btn16_Click);
            // 
            // btn64
            // 
            this.btn64.Location = new System.Drawing.Point(6, 19);
            this.btn64.Name = "btn64";
            this.btn64.Size = new System.Drawing.Size(110, 23);
            this.btn64.TabIndex = 1;
            this.btn64.Text = "64MB (System Only)";
            this.btn64.UseVisualStyleBackColor = true;
            this.btn64.Click += new System.EventHandler(this.btn64_Click);
            // 
            // SmallBlockGroup
            // 
            this.SmallBlockGroup.Controls.Add(this.btn16);
            this.SmallBlockGroup.Location = new System.Drawing.Point(13, 12);
            this.SmallBlockGroup.Name = "SmallBlockGroup";
            this.SmallBlockGroup.Size = new System.Drawing.Size(87, 50);
            this.SmallBlockGroup.TabIndex = 2;
            this.SmallBlockGroup.TabStop = false;
            this.SmallBlockGroup.Text = "Small Block";
            // 
            // BigBlockGroup
            // 
            this.BigBlockGroup.Controls.Add(this.btn512);
            this.BigBlockGroup.Controls.Add(this.btn256);
            this.BigBlockGroup.Controls.Add(this.btn64);
            this.BigBlockGroup.Location = new System.Drawing.Point(106, 12);
            this.BigBlockGroup.Name = "BigBlockGroup";
            this.BigBlockGroup.Size = new System.Drawing.Size(285, 50);
            this.BigBlockGroup.TabIndex = 3;
            this.BigBlockGroup.TabStop = false;
            this.BigBlockGroup.Text = "Big Block - Use 64MB Unless Full Dump Needed";
            // 
            // btn512
            // 
            this.btn512.Location = new System.Drawing.Point(203, 19);
            this.btn512.Name = "btn512";
            this.btn512.Size = new System.Drawing.Size(75, 23);
            this.btn512.TabIndex = 3;
            this.btn512.Text = "512MB (Full)";
            this.btn512.UseVisualStyleBackColor = true;
            this.btn512.Click += new System.EventHandler(this.btn512_Click);
            // 
            // btn256
            // 
            this.btn256.Location = new System.Drawing.Point(122, 19);
            this.btn256.Name = "btn256";
            this.btn256.Size = new System.Drawing.Size(75, 23);
            this.btn256.TabIndex = 2;
            this.btn256.Text = "256MB (Full)";
            this.btn256.UseVisualStyleBackColor = true;
            this.btn256.Click += new System.EventHandler(this.btn256_Click);
            // 
            // xFlasherNandSel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 73);
            this.Controls.Add(this.BigBlockGroup);
            this.Controls.Add(this.SmallBlockGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "xFlasherNandSel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "xFlasher: Select Nand Size";
            this.SmallBlockGroup.ResumeLayout(false);
            this.BigBlockGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn16;
        private System.Windows.Forms.Button btn64;
        private System.Windows.Forms.GroupBox SmallBlockGroup;
        private System.Windows.Forms.GroupBox BigBlockGroup;
        private System.Windows.Forms.Button btn512;
        private System.Windows.Forms.Button btn256;
    }
}