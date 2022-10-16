
namespace JRunner
{
    partial class UpdChangelog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdChangelog));
            this.UpdateWizard = new AeroWizard.WizardControl();
            this.UpdatePage = new AeroWizard.WizardPage();
            this.label2 = new System.Windows.Forms.Label();
            this.txtChangeLog = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateWizard)).BeginInit();
            this.UpdatePage.SuspendLayout();
            this.SuspendLayout();
            // 
            // UpdateWizard
            // 
            this.UpdateWizard.CancelButtonText = "&Remind Me Later";
            this.UpdateWizard.FinishButtonText = "&Download and Install";
            this.UpdateWizard.Location = new System.Drawing.Point(0, 0);
            this.UpdateWizard.Name = "UpdateWizard";
            this.UpdateWizard.NextButtonText = "&Download and Install";
            this.UpdateWizard.Pages.Add(this.UpdatePage);
            this.UpdateWizard.Size = new System.Drawing.Size(649, 471);
            this.UpdateWizard.TabIndex = 2;
            this.UpdateWizard.Title = "J-Runner with Extras";
            this.UpdateWizard.TitleIcon = ((System.Drawing.Icon)(resources.GetObject("UpdateWizard.TitleIcon")));
            // 
            // UpdatePage
            // 
            this.UpdatePage.AllowBack = false;
            this.UpdatePage.Controls.Add(this.label2);
            this.UpdatePage.Controls.Add(this.txtChangeLog);
            this.UpdatePage.IsFinishPage = true;
            this.UpdatePage.Name = "UpdatePage";
            this.UpdatePage.Size = new System.Drawing.Size(602, 317);
            this.UpdatePage.TabIndex = 0;
            this.UpdatePage.Text = "Update Available";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "Changelog:";
            // 
            // txtChangeLog
            // 
            this.txtChangeLog.Location = new System.Drawing.Point(3, 20);
            this.txtChangeLog.Multiline = true;
            this.txtChangeLog.Name = "txtChangeLog";
            this.txtChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtChangeLog.Size = new System.Drawing.Size(569, 294);
            this.txtChangeLog.TabIndex = 3;
            // 
            // UpdChangelog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 471);
            this.Controls.Add(this.UpdateWizard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdChangelog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "J-Runner with Extras";
            ((System.ComponentModel.ISupportInitialize)(this.UpdateWizard)).EndInit();
            this.UpdatePage.ResumeLayout(false);
            this.UpdatePage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl UpdateWizard;
        private AeroWizard.WizardPage UpdatePage;
        private System.Windows.Forms.TextBox txtChangeLog;
        private System.Windows.Forms.Label label2;
    }
}