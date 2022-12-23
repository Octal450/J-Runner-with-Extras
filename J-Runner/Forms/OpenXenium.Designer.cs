
namespace JRunner.Forms
{
    partial class OpenXenium
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenXenium));
            this.OpenXeniumWizard = new AeroWizard.WizardControl();
            this.OpenXeniumPage = new AeroWizard.WizardPage();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.waitPage = new AeroWizard.WizardPage();
            ((System.ComponentModel.ISupportInitialize)(this.OpenXeniumWizard)).BeginInit();
            this.OpenXeniumPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenXeniumWizard
            // 
            this.OpenXeniumWizard.FinishButtonText = "&Program";
            this.OpenXeniumWizard.Location = new System.Drawing.Point(0, 0);
            this.OpenXeniumWizard.Name = "OpenXeniumWizard";
            this.OpenXeniumWizard.NextButtonText = "&Program";
            this.OpenXeniumWizard.Pages.Add(this.OpenXeniumPage);
            this.OpenXeniumWizard.Pages.Add(this.waitPage);
            this.OpenXeniumWizard.Size = new System.Drawing.Size(888, 401);
            this.OpenXeniumWizard.TabIndex = 0;
            this.OpenXeniumWizard.Title = "Program OpenXenium";
            this.OpenXeniumWizard.TitleIcon = ((System.Drawing.Icon)(resources.GetObject("OpenXeniumWizard.TitleIcon")));
            // 
            // OpenXeniumPage
            // 
            this.OpenXeniumPage.Controls.Add(this.label2);
            this.OpenXeniumPage.Controls.Add(this.pictureBox1);
            this.OpenXeniumPage.Controls.Add(this.label1);
            this.OpenXeniumPage.Name = "OpenXeniumPage";
            this.OpenXeniumPage.Size = new System.Drawing.Size(841, 247);
            this.OpenXeniumPage.TabIndex = 0;
            this.OpenXeniumPage.Text = "Program OpenXenium";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(392, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Click Program to proceed once the correct connections have been made.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::JRunner.Properties.Resources.openxenium;
            this.pictureBox1.Location = new System.Drawing.Point(2, 80);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(804, 143);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(586, 75);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // waitPage
            // 
            this.waitPage.AllowBack = false;
            this.waitPage.AllowCancel = false;
            this.waitPage.AllowNext = false;
            this.waitPage.IsFinishPage = true;
            this.waitPage.Name = "waitPage";
            this.waitPage.ShowCancel = false;
            this.waitPage.ShowNext = false;
            this.waitPage.Size = new System.Drawing.Size(841, 247);
            this.waitPage.TabIndex = 1;
            this.waitPage.Text = "Please Wait...";
            // 
            // OpenXenium
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 401);
            this.Controls.Add(this.OpenXeniumWizard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenXenium";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Program OpenXenium";
            ((System.ComponentModel.ISupportInitialize)(this.OpenXeniumWizard)).EndInit();
            this.OpenXeniumPage.ResumeLayout(false);
            this.OpenXeniumPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl OpenXeniumWizard;
        private AeroWizard.WizardPage OpenXeniumPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private AeroWizard.WizardPage waitPage;
    }
}