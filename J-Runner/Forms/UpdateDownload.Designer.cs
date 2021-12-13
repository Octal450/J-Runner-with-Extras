
namespace JRunner
{
    partial class UpdateDownload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateDownload));
            this.UpdateWizard = new AeroWizard.WizardControl();
            this.UpdatePage = new AeroWizard.WizardPage();
            this.updateProgressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateWizard)).BeginInit();
            this.UpdatePage.SuspendLayout();
            this.SuspendLayout();
            // 
            // UpdateWizard
            // 
            this.UpdateWizard.BackColor = System.Drawing.Color.White;
            this.UpdateWizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UpdateWizard.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateWizard.Location = new System.Drawing.Point(0, 0);
            this.UpdateWizard.Name = "UpdateWizard";
            this.UpdateWizard.Pages.Add(this.UpdatePage);
            this.UpdateWizard.Size = new System.Drawing.Size(554, 401);
            this.UpdateWizard.TabIndex = 2;
            this.UpdateWizard.Text = "J-Runner with Extras";
            this.UpdateWizard.Title = "J-Runner with Extras";
            this.UpdateWizard.TitleIcon = ((System.Drawing.Icon)(resources.GetObject("UpdateWizard.TitleIcon")));
            // 
            // UpdatePage
            // 
            this.UpdatePage.AllowBack = false;
            this.UpdatePage.AllowNext = false;
            this.UpdatePage.Controls.Add(this.updateProgressBar);
            this.UpdatePage.IsFinishPage = true;
            this.UpdatePage.Name = "UpdatePage";
            this.UpdatePage.ShowNext = false;
            this.UpdatePage.Size = new System.Drawing.Size(507, 247);
            this.UpdatePage.TabIndex = 0;
            this.UpdatePage.Text = "Downloading Update...";
            // 
            // updateProgressBar
            // 
            this.updateProgressBar.Location = new System.Drawing.Point(4, 4);
            this.updateProgressBar.MarqueeAnimationSpeed = 50;
            this.updateProgressBar.Name = "updateProgressBar";
            this.updateProgressBar.Size = new System.Drawing.Size(470, 23);
            this.updateProgressBar.TabIndex = 3;
            // 
            // UpdateDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 401);
            this.ControlBox = false;
            this.Controls.Add(this.UpdateWizard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateDownload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "J-Runner with Extras";
            ((System.ComponentModel.ISupportInitialize)(this.UpdateWizard)).EndInit();
            this.UpdatePage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl UpdateWizard;
        private AeroWizard.WizardPage UpdatePage;
        private System.Windows.Forms.ProgressBar updateProgressBar;
    }
}