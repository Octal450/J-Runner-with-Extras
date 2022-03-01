
namespace JRunner
{
    partial class UpdateFailed
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateFailed));
            this.FailedWizard = new AeroWizard.WizardControl();
            this.FailedPage = new AeroWizard.WizardPage();
            this.FailedReason = new System.Windows.Forms.Label();
            this.DownloadButton = new UI.CommandLink();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.FailedWizard)).BeginInit();
            this.FailedPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // FailedWizard
            // 
            this.FailedWizard.Location = new System.Drawing.Point(0, 0);
            this.FailedWizard.Name = "FailedWizard";
            this.FailedWizard.Pages.Add(this.FailedPage);
            this.FailedWizard.Size = new System.Drawing.Size(554, 401);
            this.FailedWizard.TabIndex = 0;
            this.FailedWizard.Title = "J-Runner with Extras";
            this.FailedWizard.TitleIcon = ((System.Drawing.Icon)(resources.GetObject("FailedWizard.TitleIcon")));
            // 
            // FailedPage
            // 
            this.FailedPage.AllowBack = false;
            this.FailedPage.AllowCancel = false;
            this.FailedPage.Controls.Add(this.FailedReason);
            this.FailedPage.Controls.Add(this.DownloadButton);
            this.FailedPage.Controls.Add(this.label1);
            this.FailedPage.Name = "FailedPage";
            this.FailedPage.ShowCancel = false;
            this.FailedPage.Size = new System.Drawing.Size(507, 247);
            this.FailedPage.TabIndex = 0;
            this.FailedPage.Text = "Update Failed";
            // 
            // FailedReason
            // 
            this.FailedReason.AutoSize = true;
            this.FailedReason.Location = new System.Drawing.Point(2, 34);
            this.FailedReason.Name = "FailedReason";
            this.FailedReason.Size = new System.Drawing.Size(76, 15);
            this.FailedReason.TabIndex = 2;
            this.FailedReason.Text = "FailedReason";
            // 
            // DownloadButton
            // 
            this.DownloadButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.DownloadButton.Location = new System.Drawing.Point(4, 97);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(232, 45);
            this.DownloadButton.TabIndex = 1;
            this.DownloadButton.Text = "Download Manually";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(361, 75);
            this.label1.TabIndex = 1;
            this.label1.Text = "J-Runner with Extras could not be updated\r\n\r\n\r\n\r\nCheck your network connection, o" +
    "r download the update manually";
            // 
            // UpdateFailed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 401);
            this.ControlBox = false;
            this.Controls.Add(this.FailedWizard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateFailed";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "J-Runner with Extras";
            this.Load += new System.EventHandler(this.UpdateFailed_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FailedWizard)).EndInit();
            this.FailedPage.ResumeLayout(false);
            this.FailedPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl FailedWizard;
        private AeroWizard.WizardPage FailedPage;
        private System.Windows.Forms.Label label1;
        private UI.CommandLink DownloadButton;
        private System.Windows.Forms.Label FailedReason;
    }
}