
namespace JRunner.Forms
{
    partial class KeyvaultDecrypter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyvaultDecrypter));
            this.DecryptWizard = new AeroWizard.WizardControl();
            this.DecryptPage = new AeroWizard.WizardPage();
            this.KvGroup = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CpuKeyBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.KvEllipse = new System.Windows.Forms.Button();
            this.KvBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.DecryptWizard)).BeginInit();
            this.DecryptPage.SuspendLayout();
            this.KvGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // DecryptWizard
            // 
            this.DecryptWizard.BackColor = System.Drawing.Color.White;
            this.DecryptWizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DecryptWizard.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DecryptWizard.Location = new System.Drawing.Point(0, 0);
            this.DecryptWizard.Name = "DecryptWizard";
            this.DecryptWizard.Pages.Add(this.DecryptPage);
            this.DecryptWizard.Size = new System.Drawing.Size(554, 401);
            this.DecryptWizard.TabIndex = 0;
            this.DecryptWizard.Text = "Decrypt Keyvault";
            this.DecryptWizard.Title = "Decrypt Keyvault";
            this.DecryptWizard.TitleIcon = ((System.Drawing.Icon)(resources.GetObject("DecryptWizard.TitleIcon")));
            // 
            // DecryptPage
            // 
            this.DecryptPage.AllowNext = false;
            this.DecryptPage.Controls.Add(this.KvGroup);
            this.DecryptPage.IsFinishPage = true;
            this.DecryptPage.Name = "DecryptPage";
            this.DecryptPage.Size = new System.Drawing.Size(507, 250);
            this.DecryptPage.TabIndex = 0;
            this.DecryptPage.Text = "Insert CPU Key and Keyvault";
            // 
            // KvGroup
            // 
            this.KvGroup.Controls.Add(this.label1);
            this.KvGroup.Controls.Add(this.CpuKeyBox);
            this.KvGroup.Controls.Add(this.label2);
            this.KvGroup.Controls.Add(this.KvEllipse);
            this.KvGroup.Controls.Add(this.KvBox);
            this.KvGroup.Location = new System.Drawing.Point(4, -5);
            this.KvGroup.Name = "KvGroup";
            this.KvGroup.Size = new System.Drawing.Size(470, 75);
            this.KvGroup.TabIndex = 3;
            this.KvGroup.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "CPU Key:";
            // 
            // CpuKeyBox
            // 
            this.CpuKeyBox.AllowDrop = true;
            this.CpuKeyBox.Location = new System.Drawing.Point(67, 15);
            this.CpuKeyBox.MaxLength = 32;
            this.CpuKeyBox.Name = "CpuKeyBox";
            this.CpuKeyBox.Size = new System.Drawing.Size(397, 23);
            this.CpuKeyBox.TabIndex = 3;
            this.CpuKeyBox.TextChanged += new System.EventHandler(this.DecryptCheckNext);
            this.CpuKeyBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.CpuKeyBox_DragDrop);
            this.CpuKeyBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.CpuKeyBox_DragEnter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "KV:";
            // 
            // KvEllipse
            // 
            this.KvEllipse.Location = new System.Drawing.Point(431, 43);
            this.KvEllipse.Name = "KvEllipse";
            this.KvEllipse.Size = new System.Drawing.Size(33, 25);
            this.KvEllipse.TabIndex = 1;
            this.KvEllipse.Text = "...";
            this.KvEllipse.UseVisualStyleBackColor = true;
            this.KvEllipse.Click += new System.EventHandler(this.KvEllipse_Click);
            // 
            // KvBox
            // 
            this.KvBox.AllowDrop = true;
            this.KvBox.Location = new System.Drawing.Point(67, 44);
            this.KvBox.Name = "KvBox";
            this.KvBox.Size = new System.Drawing.Size(358, 23);
            this.KvBox.TabIndex = 0;
            this.KvBox.TextChanged += new System.EventHandler(this.DecryptCheckNext);
            this.KvBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.KvBox_DragDrop);
            this.KvBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.KvBox_DragEnter);
            // 
            // KeyvaultDecrypter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 401);
            this.Controls.Add(this.DecryptWizard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyvaultDecrypter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Decrypt Keyvault";
            ((System.ComponentModel.ISupportInitialize)(this.DecryptWizard)).EndInit();
            this.DecryptPage.ResumeLayout(false);
            this.KvGroup.ResumeLayout(false);
            this.KvGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl DecryptWizard;
        private AeroWizard.WizardPage DecryptPage;
        private System.Windows.Forms.GroupBox KvGroup;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button KvEllipse;
        private System.Windows.Forms.TextBox KvBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox CpuKeyBox;
    }
}