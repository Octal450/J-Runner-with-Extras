namespace JRunner.Nand
{
    partial class VNandForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VNandForm));
            this.listBoxConsoles = new System.Windows.Forms.ListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.listBoxConfigs = new System.Windows.Forms.ListBox();
            this.btnSaveTo = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listBoxBadBlocks = new System.Windows.Forms.ListBox();
            this.btnAddBadBlock = new System.Windows.Forms.Button();
            this.btnRemoveBadBlock = new System.Windows.Forms.Button();
            this.txtBadBlock = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxConsoles
            // 
            this.listBoxConsoles.FormattingEnabled = true;
            this.listBoxConsoles.Location = new System.Drawing.Point(12, 12);
            this.listBoxConsoles.Name = "listBoxConsoles";
            this.listBoxConsoles.Size = new System.Drawing.Size(148, 225);
            this.listBoxConsoles.TabIndex = 0;
            this.listBoxConsoles.SelectedIndexChanged += new System.EventHandler(this.listBoxConsoles_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(418, 215);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 22);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // listBoxConfigs
            // 
            this.listBoxConfigs.FormattingEnabled = true;
            this.listBoxConfigs.Location = new System.Drawing.Point(166, 12);
            this.listBoxConfigs.Name = "listBoxConfigs";
            this.listBoxConfigs.Size = new System.Drawing.Size(148, 82);
            this.listBoxConfigs.TabIndex = 2;
            this.listBoxConfigs.SelectedIndexChanged += new System.EventHandler(this.listBoxConfigs_SelectedIndexChanged);
            // 
            // btnSaveTo
            // 
            this.btnSaveTo.Location = new System.Drawing.Point(460, 100);
            this.btnSaveTo.Name = "btnSaveTo";
            this.btnSaveTo.Size = new System.Drawing.Size(33, 22);
            this.btnSaveTo.TabIndex = 3;
            this.btnSaveTo.Text = "...";
            this.btnSaveTo.UseVisualStyleBackColor = true;
            this.btnSaveTo.Click += new System.EventHandler(this.btnSaveTo_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(166, 101);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(288, 20);
            this.textBox1.TabIndex = 4;
            // 
            // listBoxBadBlocks
            // 
            this.listBoxBadBlocks.FormattingEnabled = true;
            this.listBoxBadBlocks.Location = new System.Drawing.Point(166, 129);
            this.listBoxBadBlocks.Name = "listBoxBadBlocks";
            this.listBoxBadBlocks.Size = new System.Drawing.Size(61, 108);
            this.listBoxBadBlocks.TabIndex = 5;
            this.listBoxBadBlocks.SelectedIndexChanged += new System.EventHandler(this.listBoxBadBlocks_SelectedIndexChanged);
            // 
            // btnAddBadBlock
            // 
            this.btnAddBadBlock.Location = new System.Drawing.Point(283, 128);
            this.btnAddBadBlock.Name = "btnAddBadBlock";
            this.btnAddBadBlock.Size = new System.Drawing.Size(93, 22);
            this.btnAddBadBlock.TabIndex = 6;
            this.btnAddBadBlock.Text = "Add Bad Block";
            this.btnAddBadBlock.UseVisualStyleBackColor = true;
            this.btnAddBadBlock.Click += new System.EventHandler(this.btnAddBadBlock_Click);
            // 
            // btnRemoveBadBlock
            // 
            this.btnRemoveBadBlock.Location = new System.Drawing.Point(382, 128);
            this.btnRemoveBadBlock.Name = "btnRemoveBadBlock";
            this.btnRemoveBadBlock.Size = new System.Drawing.Size(111, 22);
            this.btnRemoveBadBlock.TabIndex = 7;
            this.btnRemoveBadBlock.Text = "Remove Bad Block";
            this.btnRemoveBadBlock.UseVisualStyleBackColor = true;
            this.btnRemoveBadBlock.Click += new System.EventHandler(this.btnRemoveBadBlock_Click);
            // 
            // txtBadBlock
            // 
            this.txtBadBlock.Location = new System.Drawing.Point(234, 129);
            this.txtBadBlock.Name = "txtBadBlock";
            this.txtBadBlock.Size = new System.Drawing.Size(43, 20);
            this.txtBadBlock.TabIndex = 8;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(337, 215);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 22);
            this.buttonOK.TabIndex = 9;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // VNandForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 262);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.txtBadBlock);
            this.Controls.Add(this.btnRemoveBadBlock);
            this.Controls.Add(this.btnAddBadBlock);
            this.Controls.Add(this.listBoxBadBlocks);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnSaveTo);
            this.Controls.Add(this.listBoxConfigs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.listBoxConsoles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VNandForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "VNand";
            this.Load += new System.EventHandler(this.VNandForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxConsoles;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox listBoxConfigs;
        private System.Windows.Forms.Button btnSaveTo;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox listBoxBadBlocks;
        private System.Windows.Forms.Button btnAddBadBlock;
        private System.Windows.Forms.Button btnRemoveBadBlock;
        private System.Windows.Forms.TextBox txtBadBlock;
        private System.Windows.Forms.Button buttonOK;
    }
}