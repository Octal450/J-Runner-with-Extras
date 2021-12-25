
namespace JRunner
{
    partial class UpdateSuccess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateSuccess));
            this.SuccessWizard = new AeroWizard.WizardControl();
            this.SuccessPage = new AeroWizard.WizardPage();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SuccessWizard)).BeginInit();
            this.SuccessPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // SuccessWizard
            // 
            this.SuccessWizard.BackColor = System.Drawing.Color.White;
            this.SuccessWizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SuccessWizard.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SuccessWizard.Location = new System.Drawing.Point(0, 0);
            this.SuccessWizard.Name = "SuccessWizard";
            this.SuccessWizard.Pages.Add(this.SuccessPage);
            this.SuccessWizard.Size = new System.Drawing.Size(554, 401);
            this.SuccessWizard.TabIndex = 0;
            this.SuccessWizard.Text = "J-Runner with Extras";
            this.SuccessWizard.Title = "J-Runner with Extras";
            this.SuccessWizard.TitleIcon = ((System.Drawing.Icon)(resources.GetObject("SuccessWizard.TitleIcon")));
            // 
            // SuccessPage
            // 
            this.SuccessPage.AllowBack = false;
            this.SuccessPage.AllowCancel = false;
            this.SuccessPage.Controls.Add(this.label1);
            this.SuccessPage.IsFinishPage = true;
            this.SuccessPage.Name = "SuccessPage";
            this.SuccessPage.Size = new System.Drawing.Size(507, 247);
            this.SuccessPage.TabIndex = 0;
            this.SuccessPage.Text = "Update Successful";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(280, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "J-Runner with Extras has been updated successfully!";
            // 
            // UpdateSuccess
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
            this.Name = "UpdateSuccess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "J-Runner with Extras";
            ((System.ComponentModel.ISupportInitialize)(this.SuccessWizard)).EndInit();
            this.SuccessPage.ResumeLayout(false);
            this.SuccessPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl SuccessWizard;
        private AeroWizard.WizardPage SuccessPage;
        private System.Windows.Forms.Label label1;
    }
}