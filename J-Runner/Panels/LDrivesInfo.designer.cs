namespace JRunner.Panels
{
    partial class LDrivesInfo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRefresh = new System.Windows.Forms.Button();
            this.chkShowAll = new System.Windows.Forms.CheckBox();
            this.btnErase = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.chkFullDump = new System.Windows.Forms.CheckBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.iterations = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(2, 2);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(336, 181);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Drive";
            this.columnHeader6.Width = 104;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Letter";
            this.columnHeader1.Width = 39;
            // 
            // columnHeader2
            // 
            this.columnHeader2.DisplayIndex = 3;
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 42;
            // 
            // columnHeader3
            // 
            this.columnHeader3.DisplayIndex = 5;
            this.columnHeader3.Text = "Label";
            this.columnHeader3.Width = 39;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Format";
            this.columnHeader4.Width = 48;
            // 
            // columnHeader5
            // 
            this.columnHeader5.DisplayIndex = 2;
            this.columnHeader5.Text = "Size (MB)";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(133, 189);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(197, 25);
            this.btnRefresh.TabIndex = 10;
            this.btnRefresh.Text = "Refresh Drive List";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // chkShowAll
            // 
            this.chkShowAll.AutoSize = true;
            this.chkShowAll.Location = new System.Drawing.Point(117, 263);
            this.chkShowAll.Name = "chkShowAll";
            this.chkShowAll.Size = new System.Drawing.Size(109, 17);
            this.chkShowAll.TabIndex = 11;
            this.chkShowAll.Text = "Show All Devices";
            this.chkShowAll.UseVisualStyleBackColor = true;
            this.chkShowAll.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // btnErase
            // 
            this.btnErase.Location = new System.Drawing.Point(254, 220);
            this.btnErase.Name = "btnErase";
            this.btnErase.Size = new System.Drawing.Size(76, 33);
            this.btnErase.TabIndex = 16;
            this.btnErase.Text = "Erase";
            this.btnErase.UseVisualStyleBackColor = true;
            this.btnErase.Click += new System.EventHandler(this.btnErase_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(133, 220);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(115, 33);
            this.btnWrite.TabIndex = 14;
            this.btnWrite.Text = "Write";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // chkFullDump
            // 
            this.chkFullDump.AutoSize = true;
            this.chkFullDump.Location = new System.Drawing.Point(15, 263);
            this.chkFullDump.Name = "chkFullDump";
            this.chkFullDump.Size = new System.Drawing.Size(73, 17);
            this.chkFullDump.TabIndex = 15;
            this.chkFullDump.Text = "Full Dump";
            this.chkFullDump.UseVisualStyleBackColor = true;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(12, 220);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(115, 33);
            this.btnRead.TabIndex = 13;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(255, 259);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // iterations
            // 
            this.iterations.AutoSize = true;
            this.iterations.Location = new System.Drawing.Point(12, 195);
            this.iterations.Name = "iterations";
            this.iterations.Size = new System.Drawing.Size(79, 13);
            this.iterations.TabIndex = 18;
            this.iterations.Text = "Nand Reads: 2";
            // 
            // LDrivesInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.iterations);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnErase);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.chkFullDump);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.chkShowAll);
            this.Controls.Add(this.listView1);
            this.Name = "LDrivesInfo";
            this.Size = new System.Drawing.Size(342, 293);
            this.Load += new System.EventHandler(this.LDrivesInfo_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LDrives_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox chkShowAll;
        private System.Windows.Forms.Button btnErase;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.CheckBox chkFullDump;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label iterations;
    }
}
