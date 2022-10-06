
namespace JRunner
{
    partial class UpdateAvailable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateAvailable));
            this.SuccessWizard = new AeroWizard.WizardControl();
            this.SuccessPage = new AeroWizard.WizardPage();
            ((System.ComponentModel.ISupportInitialize)(this.SuccessWizard)).BeginInit();
            this.SuspendLayout();
            // 
            // SuccessWizard
            // 
            this.SuccessWizard.CancelButtonText = "&Skip Update";
            this.SuccessWizard.FinishButtonText = "&Download and Install";
            this.SuccessWizard.Location = new System.Drawing.Point(0, 0);
            this.SuccessWizard.Name = "SuccessWizard";
            this.SuccessWizard.NextButtonText = "&Download and Install";
            this.SuccessWizard.Pages.Add(this.SuccessPage);
            this.SuccessWizard.Size = new System.Drawing.Size(554, 401);
            this.SuccessWizard.TabIndex = 0;
            this.SuccessWizard.Title = "J-Runner with Extras";
            this.SuccessWizard.TitleIcon = ((System.Drawing.Icon)(resources.GetObject("SuccessWizard.TitleIcon")));
            // 
            // SuccessPage
            // 
            this.SuccessPage.AllowBack = false;
            this.SuccessPage.IsFinishPage = true;
            this.SuccessPage.Name = "SuccessPage";
            this.SuccessPage.Size = new System.Drawing.Size(507, 247);
            this.SuccessPage.TabIndex = 0;
            this.SuccessPage.Text = "Updates Available";
            // 
            // UpdateAvailable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 401);
            this.ControlBox = false;
            this.Controls.Add(this.SuccessWizard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateAvailable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "J-Runner with Extras";
            this.Load += new System.EventHandler(this.UpdateSuccess_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SuccessWizard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl SuccessWizard;
        private AeroWizard.WizardPage SuccessPage;
    }
}