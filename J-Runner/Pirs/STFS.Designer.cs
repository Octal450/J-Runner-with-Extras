using System.Drawing;
using System.Windows.Forms;
namespace JRunner.Pirs
{
    partial class STFS
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(STFS));
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCluster = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDateModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnCRC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.extractFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyCRCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderSize,
            this.columnHeaderCluster,
            this.columnHeaderDateModified,
            this.columnCRC});
            this.listView.ContextMenuStrip = this.contextMenuStrip1;
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Name = "listView";
            this.listView.ShowItemToolTips = true;
            this.listView.Size = new System.Drawing.Size(693, 411);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseClick);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 200;
            // 
            // columnHeaderSize
            // 
            this.columnHeaderSize.Text = "Size";
            this.columnHeaderSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // columnHeaderCluster
            // 
            this.columnHeaderCluster.Text = "Cluster";
            this.columnHeaderCluster.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // columnHeaderDateModified
            // 
            this.columnHeaderDateModified.Text = "Date modified";
            this.columnHeaderDateModified.Width = 120;
            // 
            // columnCRC
            // 
            this.columnCRC.Text = "CRC";
            this.columnCRC.Width = 84;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractFileToolStripMenuItem,
            this.extractAllToolStripMenuItem,
            this.extractAllToolStripMenuItem1,
            this.copyCRCToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(158, 92);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // extractFileToolStripMenuItem
            // 
            this.extractFileToolStripMenuItem.Name = "extractFileToolStripMenuItem";
            this.extractFileToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.extractFileToolStripMenuItem.Text = "Extract File";
            this.extractFileToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // extractAllToolStripMenuItem
            // 
            this.extractAllToolStripMenuItem.Name = "extractAllToolStripMenuItem";
            this.extractAllToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.extractAllToolStripMenuItem.Text = "Extract Selected";
            this.extractAllToolStripMenuItem.Click += new System.EventHandler(this.extractAllToolStripMenuItem_Click);
            // 
            // extractAllToolStripMenuItem1
            // 
            this.extractAllToolStripMenuItem1.Name = "extractAllToolStripMenuItem1";
            this.extractAllToolStripMenuItem1.Size = new System.Drawing.Size(157, 22);
            this.extractAllToolStripMenuItem1.Text = "Extract All";
            this.extractAllToolStripMenuItem1.Click += new System.EventHandler(this.extractAllToolStripMenuItem1_Click);
            // 
            // copyCRCToolStripMenuItem
            // 
            this.copyCRCToolStripMenuItem.Name = "copyCRCToolStripMenuItem";
            this.copyCRCToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.copyCRCToolStripMenuItem.Text = "Copy CRC";
            this.copyCRCToolStripMenuItem.Click += new System.EventHandler(this.copyCRCToolStripMenuItem_Click);
            // 
            // STFS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 411);
            this.Controls.Add(this.listView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "STFS";
            this.Text = "STFS";
            this.TopMost = true;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.STFS_KeyUp);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private ColumnHeader columnHeaderName;
        private ColumnHeader columnHeaderSize;
        private ColumnHeader columnHeaderCluster;
        private ColumnHeader columnHeaderDateModified;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem extractFileToolStripMenuItem;
        private ToolStripMenuItem extractAllToolStripMenuItem;
        private ToolStripMenuItem extractAllToolStripMenuItem1;
        private ColumnHeader columnCRC;
        private ToolStripMenuItem copyCRCToolStripMenuItem;
    }
}