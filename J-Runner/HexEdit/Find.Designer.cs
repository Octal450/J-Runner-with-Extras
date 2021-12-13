namespace JRunner.HexEdit
{
    partial class FormFind
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
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.timerPercent = new System.Windows.Forms.Timer(this.components);
            this.chkMatchCase = new System.Windows.Forms.CheckBox();
            this.lblPercent = new System.Windows.Forms.Label();
            this.lblFinding = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.rbHex = new System.Windows.Forms.RadioButton();
            this.rbString = new System.Windows.Forms.RadioButton();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.hexFind = new Be.Windows.Forms.HexBox();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Interval = 50;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // timerPercent
            // 
            this.timerPercent.Tick += new System.EventHandler(this.timerPercent_Tick);
            // 
            // chkMatchCase
            // 
            this.chkMatchCase.AutoSize = true;
            this.chkMatchCase.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkMatchCase.Location = new System.Drawing.Point(239, 13);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(82, 17);
            this.chkMatchCase.TabIndex = 17;
            this.chkMatchCase.Text = "Match case";
            this.chkMatchCase.UseVisualStyleBackColor = true;
            // 
            // lblPercent
            // 
            this.lblPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPercent.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblPercent.Location = new System.Drawing.Point(20, 242);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(60, 23);
            this.lblPercent.TabIndex = 23;
            this.lblPercent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFinding
            // 
            this.lblFinding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFinding.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblFinding.ForeColor = System.Drawing.Color.Blue;
            this.lblFinding.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblFinding.Location = new System.Drawing.Point(86, 242);
            this.lblFinding.Name = "lblFinding";
            this.lblFinding.Size = new System.Drawing.Size(62, 23);
            this.lblFinding.TabIndex = 22;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(244, 242);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 33);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(164, 242);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 33);
            this.btnOK.TabIndex = 20;
            this.btnOK.Text = "&Find Next";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // rbHex
            // 
            this.rbHex.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rbHex.Location = new System.Drawing.Point(15, 75);
            this.rbHex.Name = "rbHex";
            this.rbHex.Size = new System.Drawing.Size(104, 16);
            this.rbHex.TabIndex = 18;
            this.rbHex.Text = "Hex";
            this.rbHex.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbString
            // 
            this.rbString.Checked = true;
            this.rbString.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rbString.Location = new System.Drawing.Point(15, 14);
            this.rbString.Name = "rbString";
            this.rbString.Size = new System.Drawing.Size(104, 16);
            this.rbString.TabIndex = 15;
            this.rbString.TabStop = true;
            this.rbString.Text = "Text";
            this.rbString.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // txtFind
            // 
            this.txtFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFind.Location = new System.Drawing.Point(15, 33);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(304, 20);
            this.txtFind.TabIndex = 16;
            this.txtFind.TextChanged += new System.EventHandler(this.txtString_TextChanged);
            // 
            // hexFind
            // 
            this.hexFind.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hexFind.BoldFont = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // 
            // 
            this.hexFind.BuiltInContextMenu.CopyMenuItemText = "Kopieren";
            this.hexFind.BuiltInContextMenu.CutMenuItemText = "Ausschneiden";
            this.hexFind.BuiltInContextMenu.PasteMenuItemText = "Einfügen";
            this.hexFind.BuiltInContextMenu.SelectAllMenuItemText = "Alles Markieren";
            this.hexFind.Enabled = false;
            this.hexFind.Font = new System.Drawing.Font("Courier New", 9F);
            this.hexFind.LineInfoForeColor = System.Drawing.Color.Empty;
            this.hexFind.Location = new System.Drawing.Point(15, 97);
            this.hexFind.Name = "hexFind";
            this.hexFind.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexFind.Size = new System.Drawing.Size(304, 128);
            this.hexFind.TabIndex = 19;
            // 
            // FormFind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 291);
            this.Controls.Add(this.chkMatchCase);
            this.Controls.Add(this.lblPercent);
            this.Controls.Add(this.lblFinding);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.rbHex);
            this.Controls.Add(this.rbString);
            this.Controls.Add(this.txtFind);
            this.Controls.Add(this.hexFind);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFind";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find";
            this.Activated += new System.EventHandler(this.FormFind_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer timerPercent;
        private System.Windows.Forms.CheckBox chkMatchCase;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.Label lblFinding;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton rbHex;
        private System.Windows.Forms.RadioButton rbString;
        private System.Windows.Forms.TextBox txtFind;
        private Be.Windows.Forms.HexBox hexFind;
    }
}