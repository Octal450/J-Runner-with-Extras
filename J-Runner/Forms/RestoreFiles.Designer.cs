
namespace JRunner
{
    partial class RestoreFiles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RestoreFiles));
            this.RestoreWizard = new AeroWizard.WizardControl();
            this.wizardPage1 = new AeroWizard.WizardPage();
            this.label2 = new System.Windows.Forms.Label();
            this.RestoreButton = new UI.CommandLink();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RestoreWizard)).BeginInit();
            this.wizardPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RestoreWizard
            // 
            this.RestoreWizard.Location = new System.Drawing.Point(0, 0);
            this.RestoreWizard.Name = "RestoreWizard";
            this.RestoreWizard.Pages.Add(this.wizardPage1);
            this.RestoreWizard.Size = new System.Drawing.Size(554, 401);
            this.RestoreWizard.TabIndex = 0;
            this.RestoreWizard.Title = "Restore Files";
            this.RestoreWizard.TitleIcon = ((System.Drawing.Icon)(resources.GetObject("RestoreWizard.TitleIcon")));
            // 
            // wizardPage1
            // 
            this.wizardPage1.Controls.Add(this.label2);
            this.wizardPage1.Controls.Add(this.RestoreButton);
            this.wizardPage1.Controls.Add(this.label1);
            this.wizardPage1.IsFinishPage = true;
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.ShowNext = false;
            this.wizardPage1.Size = new System.Drawing.Size(507, 247);
            this.wizardPage1.TabIndex = 0;
            this.wizardPage1.Text = "Restore Files";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(2, 193);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(478, 50);
            this.label2.TabIndex = 3;
            this.label2.Text = "An internet connection is required for this feature to operate.\r\n\r\nAll files insi" +
    "de common and xeBuild will be deleted and replaced with clean versions!";
            // 
            // RestoreButton
            // 
            this.RestoreButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.RestoreButton.Location = new System.Drawing.Point(2, 99);
            this.RestoreButton.Name = "RestoreButton";
            this.RestoreButton.Size = new System.Drawing.Size(232, 45);
            this.RestoreButton.TabIndex = 2;
            this.RestoreButton.Text = "Restore Files";
            this.RestoreButton.UseVisualStyleBackColor = true;
            this.RestoreButton.Click += new System.EventHandler(this.RestoreButton_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(478, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "Allows you to cleanup the common and xeBuild folders and restore a clean filesyst" +
    "em.\r\n\r\nPlease make sure no operations are running, as the application must resta" +
    "rt.";
            // 
            // RestoreFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 401);
            this.Controls.Add(this.RestoreWizard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RestoreFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Restore Files";
            ((System.ComponentModel.ISupportInitialize)(this.RestoreWizard)).EndInit();
            this.wizardPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl RestoreWizard;
        private AeroWizard.WizardPage wizardPage1;
        private System.Windows.Forms.Label label1;
        private UI.CommandLink RestoreButton;
        private System.Windows.Forms.Label label2;
    }
}