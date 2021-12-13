
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
            this.DownloadButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.FailedWizard)).BeginInit();
            this.FailedPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // FailedWizard
            // 
            this.FailedWizard.BackColor = System.Drawing.Color.White;
            this.FailedWizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FailedWizard.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FailedWizard.Location = new System.Drawing.Point(0, 0);
            this.FailedWizard.Name = "FailedWizard";
            this.FailedWizard.Pages.Add(this.FailedPage);
            this.FailedWizard.Size = new System.Drawing.Size(554, 401);
            this.FailedWizard.TabIndex = 0;
            this.FailedWizard.Text = "J-Runner with Extras";
            this.FailedWizard.Title = "J-Runner with Extras";
            this.FailedWizard.TitleIcon = ((System.Drawing.Icon)(resources.GetObject("FailedWizard.TitleIcon")));
            // 
            // FailedPage
            // 
            this.FailedPage.AllowBack = false;
            this.FailedPage.AllowCancel = false;
            this.FailedPage.Controls.Add(this.DownloadButton);
            this.FailedPage.Controls.Add(this.label1);
            this.FailedPage.Name = "FailedPage";
            this.FailedPage.Size = new System.Drawing.Size(507, 247);
            this.FailedPage.TabIndex = 0;
            this.FailedPage.Text = "Update Failed";
            // 
            // DownloadButton
            // 
            this.DownloadButton.Location = new System.Drawing.Point(6, 66);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(232, 23);
            this.DownloadButton.TabIndex = 1;
            this.DownloadButton.Text = "Download";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(361, 45);
            this.label1.TabIndex = 1;
            this.label1.Text = "J-Runner with Extras update failed for some reason\r\n\r\nCheck your network connecti" +
    "on, or download the update manually";
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
            ((System.ComponentModel.ISupportInitialize)(this.FailedWizard)).EndInit();
            this.FailedPage.ResumeLayout(false);
            this.FailedPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl FailedWizard;
        private AeroWizard.WizardPage FailedPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DownloadButton;
    }
}