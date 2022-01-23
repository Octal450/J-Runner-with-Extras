namespace JRunner
{
    partial class CpuKeyDB
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CpuKeyDB));
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataTable1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet1 = new JRunner.DataSet1();
            this.lblSerial = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblNumber = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditMobotoolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.rbtnSerial = new System.Windows.Forms.RadioButton();
            this.rbtnCPU = new System.Windows.Forms.RadioButton();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnreverse = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.txtregion = new System.Windows.Forms.TextBox();
            this.labelregion = new System.Windows.Forms.Label();
            this.txtdvdkey = new System.Windows.Forms.TextBox();
            this.txtosig = new System.Windows.Forms.TextBox();
            this.labelosig = new System.Windows.Forms.Label();
            this.labeldvdkey = new System.Windows.Forms.Label();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serialDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPUKeyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRCKVDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Comment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.SystemColors.Window;
            this.txtSearch.Location = new System.Drawing.Point(13, 12);
            this.txtSearch.MaxLength = 32;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(210, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView1.AutoGenerateColumns = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.serialDataGridViewTextBoxColumn,
            this.cPUKeyDataGridViewTextBoxColumn,
            this.cRCKVDataGridViewTextBoxColumn,
            this.Comment});
            this.dataGridView1.DataSource = this.dataTable1BindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.Location = new System.Drawing.Point(13, 40);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(583, 210);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            this.dataGridView1.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView1_UserDeletingRow);
            this.dataGridView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseUp);
            // 
            // dataTable1BindingSource
            // 
            this.dataTable1BindingSource.DataMember = "DataTable1";
            this.dataTable1BindingSource.DataSource = this.dataSet1;
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "DataSet1";
            this.dataSet1.Locale = new System.Globalization.CultureInfo("");
            this.dataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // lblSerial
            // 
            this.lblSerial.AutoSize = true;
            this.lblSerial.Location = new System.Drawing.Point(229, 15);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(59, 13);
            this.lblSerial.TabIndex = 6;
            this.lblSerial.Text = "Search By:";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(419, 15);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(87, 13);
            this.lblCount.TabIndex = 7;
            this.lblCount.Text = "Number of keys: ";
            // 
            // lblNumber
            // 
            this.lblNumber.AutoSize = true;
            this.lblNumber.Location = new System.Drawing.Point(505, 15);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(13, 13);
            this.lblNumber.TabIndex = 8;
            this.lblNumber.Text = "0";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(611, 40);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(98, 23);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "Add Value";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(611, 156);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(98, 23);
            this.btnExport.TabIndex = 11;
            this.btnExport.Text = "Export DB";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(611, 227);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(98, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.EditMobotoolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(172, 70);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.copyToolStripMenuItem.Text = "Copy to Clipboard";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // EditMobotoolStripMenuItem1
            // 
            this.EditMobotoolStripMenuItem1.Name = "EditMobotoolStripMenuItem1";
            this.EditMobotoolStripMenuItem1.Size = new System.Drawing.Size(171, 22);
            this.EditMobotoolStripMenuItem1.Text = "Edit Mobo Entry";
            this.EditMobotoolStripMenuItem1.Click += new System.EventHandler(this.EditMobotoolStripMenuItem1_Click);
            // 
            // rbtnSerial
            // 
            this.rbtnSerial.AutoSize = true;
            this.rbtnSerial.Location = new System.Drawing.Point(294, 13);
            this.rbtnSerial.Name = "rbtnSerial";
            this.rbtnSerial.Size = new System.Drawing.Size(51, 17);
            this.rbtnSerial.TabIndex = 13;
            this.rbtnSerial.Text = "Serial";
            this.rbtnSerial.UseVisualStyleBackColor = true;
            this.rbtnSerial.CheckedChanged += new System.EventHandler(this.rbtnSerial_CheckedChanged);
            // 
            // rbtnCPU
            // 
            this.rbtnCPU.AutoSize = true;
            this.rbtnCPU.Checked = true;
            this.rbtnCPU.Location = new System.Drawing.Point(345, 13);
            this.rbtnCPU.Name = "rbtnCPU";
            this.rbtnCPU.Size = new System.Drawing.Size(68, 17);
            this.rbtnCPU.TabIndex = 14;
            this.rbtnCPU.TabStop = true;
            this.rbtnCPU.Text = "CPU Key";
            this.rbtnCPU.UseVisualStyleBackColor = true;
            this.rbtnCPU.CheckedChanged += new System.EventHandler(this.rbtnCPU_CheckedChanged);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(611, 69);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(98, 23);
            this.btnScan.TabIndex = 16;
            this.btnScan.Text = "Scan Folders";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnreverse
            // 
            this.btnreverse.Location = new System.Drawing.Point(611, 98);
            this.btnreverse.Name = "btnreverse";
            this.btnreverse.Size = new System.Drawing.Size(98, 23);
            this.btnreverse.TabIndex = 17;
            this.btnreverse.Text = "Reverse Scan";
            this.btnreverse.UseVisualStyleBackColor = true;
            this.btnreverse.Click += new System.EventHandler(this.btnreverse_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(611, 127);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(98, 23);
            this.btnImport.TabIndex = 18;
            this.btnImport.Text = "Import DB";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // txtregion
            // 
            this.txtregion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtregion.Location = new System.Drawing.Point(336, 265);
            this.txtregion.Name = "txtregion";
            this.txtregion.ReadOnly = true;
            this.txtregion.Size = new System.Drawing.Size(129, 20);
            this.txtregion.TabIndex = 46;
            this.txtregion.TabStop = false;
            // 
            // labelregion
            // 
            this.labelregion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelregion.AutoSize = true;
            this.labelregion.Location = new System.Drawing.Point(292, 268);
            this.labelregion.Name = "labelregion";
            this.labelregion.Size = new System.Drawing.Size(44, 13);
            this.labelregion.TabIndex = 45;
            this.labelregion.Text = "Region:";
            // 
            // txtdvdkey
            // 
            this.txtdvdkey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtdvdkey.Location = new System.Drawing.Point(65, 265);
            this.txtdvdkey.Name = "txtdvdkey";
            this.txtdvdkey.ReadOnly = true;
            this.txtdvdkey.Size = new System.Drawing.Size(219, 20);
            this.txtdvdkey.TabIndex = 44;
            this.txtdvdkey.TabStop = false;
            // 
            // txtosig
            // 
            this.txtosig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtosig.Location = new System.Drawing.Point(510, 265);
            this.txtosig.Name = "txtosig";
            this.txtosig.ReadOnly = true;
            this.txtosig.Size = new System.Drawing.Size(199, 20);
            this.txtosig.TabIndex = 43;
            this.txtosig.TabStop = false;
            // 
            // labelosig
            // 
            this.labelosig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelosig.AutoSize = true;
            this.labelosig.Location = new System.Drawing.Point(474, 268);
            this.labelosig.Name = "labelosig";
            this.labelosig.Size = new System.Drawing.Size(36, 13);
            this.labelosig.TabIndex = 41;
            this.labelosig.Text = "OSIG:";
            // 
            // labeldvdkey
            // 
            this.labeldvdkey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labeldvdkey.AutoSize = true;
            this.labeldvdkey.Location = new System.Drawing.Point(11, 268);
            this.labeldvdkey.Name = "labeldvdkey";
            this.labeldvdkey.Size = new System.Drawing.Size(54, 13);
            this.labeldvdkey.TabIndex = 42;
            this.labeldvdkey.Text = "DVD Key:";
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.FillWeight = 31.46342F;
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            this.iDDataGridViewTextBoxColumn.ReadOnly = true;
            this.iDDataGridViewTextBoxColumn.Width = 30;
            // 
            // serialDataGridViewTextBoxColumn
            // 
            this.serialDataGridViewTextBoxColumn.DataPropertyName = "Serial";
            this.serialDataGridViewTextBoxColumn.FillWeight = 93.33911F;
            this.serialDataGridViewTextBoxColumn.HeaderText = "Serial";
            this.serialDataGridViewTextBoxColumn.Name = "serialDataGridViewTextBoxColumn";
            this.serialDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cPUKeyDataGridViewTextBoxColumn
            // 
            this.cPUKeyDataGridViewTextBoxColumn.DataPropertyName = "CPU Key";
            this.cPUKeyDataGridViewTextBoxColumn.FillWeight = 175.1975F;
            this.cPUKeyDataGridViewTextBoxColumn.HeaderText = "CPU Key";
            this.cPUKeyDataGridViewTextBoxColumn.MaxInputLength = 32;
            this.cPUKeyDataGridViewTextBoxColumn.Name = "cPUKeyDataGridViewTextBoxColumn";
            this.cPUKeyDataGridViewTextBoxColumn.ReadOnly = true;
            this.cPUKeyDataGridViewTextBoxColumn.Width = 250;
            // 
            // cRCKVDataGridViewTextBoxColumn
            // 
            this.cRCKVDataGridViewTextBoxColumn.DataPropertyName = "CRC KV";
            this.cRCKVDataGridViewTextBoxColumn.HeaderText = "CRC KV";
            this.cRCKVDataGridViewTextBoxColumn.Name = "cRCKVDataGridViewTextBoxColumn";
            this.cRCKVDataGridViewTextBoxColumn.ReadOnly = true;
            this.cRCKVDataGridViewTextBoxColumn.Visible = false;
            // 
            // Comment
            // 
            this.Comment.DataPropertyName = "Comment";
            this.Comment.HeaderText = "Console Type";
            this.Comment.Name = "Comment";
            this.Comment.ReadOnly = true;
            this.Comment.Width = 145;
            // 
            // CpuKeyDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 301);
            this.Controls.Add(this.txtregion);
            this.Controls.Add(this.labelregion);
            this.Controls.Add(this.txtdvdkey);
            this.Controls.Add(this.txtosig);
            this.Controls.Add(this.labelosig);
            this.Controls.Add(this.labeldvdkey);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnreverse);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.rbtnCPU);
            this.Controls.Add(this.rbtnSerial);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblNumber);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblSerial);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(740, 2160);
            this.MinimumSize = new System.Drawing.Size(740, 337);
            this.Name = "CpuKeyDB";
            this.Text = "CPU Key Database";
            this.Load += new System.EventHandler(this.cpukeydb_Load);
            this.Resize += new System.EventHandler(this.CpuKeyDB_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource dataTable1BindingSource;
        private DataSet1 dataSet1;
        private System.Windows.Forms.Label lblSerial;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblNumber;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.RadioButton rbtnSerial;
        private System.Windows.Forms.RadioButton rbtnCPU;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnreverse;
        private System.Windows.Forms.ToolStripMenuItem EditMobotoolStripMenuItem1;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TextBox txtregion;
        private System.Windows.Forms.Label labelregion;
        private System.Windows.Forms.TextBox txtdvdkey;
        private System.Windows.Forms.TextBox txtosig;
        private System.Windows.Forms.Label labelosig;
        private System.Windows.Forms.Label labeldvdkey;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn serialDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPUKeyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRCKVDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Comment;
    }
}