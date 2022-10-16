
namespace JRunner
{
    partial class UpdUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdUI));
            this.UpdateWizard = new AeroWizard.WizardControl();
            this.UpdatePage = new AeroWizard.WizardPage();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.updateProgressBar = new System.Windows.Forms.ProgressBar();
            this.FailedPage = new AeroWizard.WizardPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.FailedReason = new System.Windows.Forms.Label();
            this.DownloadButton = new UI.CommandLink();
            this.label1 = new System.Windows.Forms.Label();
            this.SuccessPage = new AeroWizard.WizardPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateWizard)).BeginInit();
            this.UpdatePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.FailedPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuccessPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // UpdateWizard
            // 
            this.UpdateWizard.Location = new System.Drawing.Point(0, 0);
            this.UpdateWizard.Name = "UpdateWizard";
            this.UpdateWizard.Pages.Add(this.UpdatePage);
            this.UpdateWizard.Pages.Add(this.FailedPage);
            this.UpdateWizard.Pages.Add(this.SuccessPage);
            this.UpdateWizard.Size = new System.Drawing.Size(649, 471);
            this.UpdateWizard.TabIndex = 2;
            this.UpdateWizard.Title = "J-Runner with Extras";
            this.UpdateWizard.TitleIcon = ((System.Drawing.Icon)(resources.GetObject("UpdateWizard.TitleIcon")));
            // 
            // UpdatePage
            // 
            this.UpdatePage.AllowBack = false;
            this.UpdatePage.AllowNext = false;
            this.UpdatePage.Controls.Add(this.pictureBox3);
            this.UpdatePage.Controls.Add(this.updateProgressBar);
            this.UpdatePage.IsFinishPage = true;
            this.UpdatePage.Name = "UpdatePage";
            this.UpdatePage.ShowNext = false;
            this.UpdatePage.Size = new System.Drawing.Size(602, 317);
            this.UpdatePage.TabIndex = 0;
            this.UpdatePage.Text = "Downloading Update...";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::JRunner.Properties.Resources.JR2;
            this.pictureBox3.Location = new System.Drawing.Point(2, 33);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(569, 281);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox3.TabIndex = 10;
            this.pictureBox3.TabStop = false;
            // 
            // updateProgressBar
            // 
            this.updateProgressBar.Location = new System.Drawing.Point(4, 4);
            this.updateProgressBar.MarqueeAnimationSpeed = 50;
            this.updateProgressBar.Name = "updateProgressBar";
            this.updateProgressBar.Size = new System.Drawing.Size(565, 23);
            this.updateProgressBar.TabIndex = 3;
            // 
            // FailedPage
            // 
            this.FailedPage.AllowBack = false;
            this.FailedPage.Controls.Add(this.pictureBox2);
            this.FailedPage.Controls.Add(this.FailedReason);
            this.FailedPage.Controls.Add(this.DownloadButton);
            this.FailedPage.Controls.Add(this.label1);
            this.FailedPage.IsFinishPage = true;
            this.FailedPage.Name = "FailedPage";
            this.FailedPage.ShowCancel = false;
            this.FailedPage.Size = new System.Drawing.Size(602, 317);
            this.FailedPage.TabIndex = 1;
            this.FailedPage.Text = "Update Failed";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::JRunner.Properties.Resources.rederror;
            this.pictureBox2.Location = new System.Drawing.Point(2, 148);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(569, 166);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // FailedReason
            // 
            this.FailedReason.AutoSize = true;
            this.FailedReason.Location = new System.Drawing.Point(2, 32);
            this.FailedReason.Name = "FailedReason";
            this.FailedReason.Size = new System.Drawing.Size(165, 15);
            this.FailedReason.TabIndex = 4;
            this.FailedReason.Text = "Error retrieving error message.";
            // 
            // DownloadButton
            // 
            this.DownloadButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.DownloadButton.Location = new System.Drawing.Point(4, 97);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(232, 45);
            this.DownloadButton.TabIndex = 2;
            this.DownloadButton.Text = "Download Manually";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(364, 75);
            this.label1.TabIndex = 3;
            this.label1.Text = "J-Runner with Extras could not be updated.\r\n\r\n\r\n\r\nCheck your network connection, " +
    "or download the update manually.";
            // 
            // SuccessPage
            // 
            this.SuccessPage.AllowBack = false;
            this.SuccessPage.Controls.Add(this.pictureBox1);
            this.SuccessPage.Controls.Add(this.label3);
            this.SuccessPage.IsFinishPage = true;
            this.SuccessPage.Name = "SuccessPage";
            this.SuccessPage.ShowCancel = false;
            this.SuccessPage.Size = new System.Drawing.Size(602, 317);
            this.SuccessPage.TabIndex = 2;
            this.SuccessPage.Text = "Update Successful";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::JRunner.Properties.Resources.greencheck;
            this.pictureBox1.Location = new System.Drawing.Point(2, 50);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(569, 264);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(280, 45);
            this.label3.TabIndex = 8;
            this.label3.Text = "J-Runner with Extras has been updated successfully!\r\n\r\nClick Finish to restart an" +
    "d launch the new version.";
            // 
            // UpdUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 471);
            this.ControlBox = false;
            this.Controls.Add(this.UpdateWizard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "J-Runner with Extras";
            ((System.ComponentModel.ISupportInitialize)(this.UpdateWizard)).EndInit();
            this.UpdatePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.FailedPage.ResumeLayout(false);
            this.FailedPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.SuccessPage.ResumeLayout(false);
            this.SuccessPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl UpdateWizard;
        private AeroWizard.WizardPage UpdatePage;
        private System.Windows.Forms.ProgressBar updateProgressBar;
        private AeroWizard.WizardPage FailedPage;
        private UI.CommandLink DownloadButton;
        private System.Windows.Forms.Label label1;
        private AeroWizard.WizardPage SuccessPage;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label FailedReason;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}