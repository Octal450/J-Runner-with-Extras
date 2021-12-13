namespace JRunner.HexEdit
{
    partial class KVViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KVViewer));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.fileSizeToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.hexBox1 = new Be.Windows.Forms.HexBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(424, 331);
            this.treeView1.TabIndex = 2;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripStatusLabel.Size = new System.Drawing.Size(802, 17);
            this.toolStripStatusLabel.Spring = true;
            // 
            // fileSizeToolStripStatusLabel
            // 
            this.fileSizeToolStripStatusLabel.Name = "fileSizeToolStripStatusLabel";
            this.fileSizeToolStripStatusLabel.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fileSizeToolStripStatusLabel.Size = new System.Drawing.Size(8, 17);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel,
            this.fileSizeToolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 332);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size(1063, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(238, 17);
            this.toolStripStatusLabel1.Text = "                                                                             ";
            // 
            // hexBox1
            // 
            this.hexBox1.AllowDrop = true;
            this.hexBox1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.hexBox1.BoldFont = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            // 
            // 
            // 
            this.hexBox1.BuiltInContextMenu.CopyMenuItemText = "Copy";
            this.hexBox1.BuiltInContextMenu.CutMenuItemText = "Cut";
            this.hexBox1.BuiltInContextMenu.PasteMenuItemText = "Paste";
            this.hexBox1.BuiltInContextMenu.SelectAllMenuItemText = "Select All";
            this.hexBox1.ColumnInfoVisible = true;
            this.hexBox1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexBox1.LineInfoForeColor = System.Drawing.Color.Empty;
            this.hexBox1.LineInfoVisible = true;
            this.hexBox1.Location = new System.Drawing.Point(430, 0);
            this.hexBox1.Name = "hexBox1";
            this.hexBox1.ReadOnly = true;
            this.hexBox1.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexBox1.Size = new System.Drawing.Size(633, 331);
            this.hexBox1.StringViewVisible = true;
            this.hexBox1.TabIndex = 0;
            this.hexBox1.UseFixedBytesPerLine = true;
            this.hexBox1.VScrollBarVisible = true;
            this.hexBox1.CurrentLineChanged += new System.EventHandler(this.Position_Changed);
            this.hexBox1.CurrentPositionInLineChanged += new System.EventHandler(this.Position_Changed);
            // 
            // KVViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 354);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.hexBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1079, 393);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1079, 393);
            this.Name = "KVViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "KV Viewer";
            this.Load += new System.EventHandler(this.HexViewer_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HexViewer_KeyUp);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Be.Windows.Forms.HexBox hexBox1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel fileSizeToolStripStatusLabel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;



    }
}