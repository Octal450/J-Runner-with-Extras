
namespace JRunner
{
    partial class Issues
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Issues));
            this.IssueWizard = new AeroWizard.WizardControl();
            this.wizardPage1 = new AeroWizard.WizardPage();
            this.CreateButton = new UI.CommandLink();
            this.ViewButton = new UI.CommandLink();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.IssueWizard)).BeginInit();
            this.wizardPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // IssueWizard
            // 
            this.IssueWizard.FinishButtonText = "&Close";
            this.IssueWizard.Location = new System.Drawing.Point(0, 0);
            this.IssueWizard.Name = "IssueWizard";
            this.IssueWizard.Pages.Add(this.wizardPage1);
            this.IssueWizard.Size = new System.Drawing.Size(554, 401);
            this.IssueWizard.TabIndex = 0;
            this.IssueWizard.Title = "Report Issue";
            this.IssueWizard.TitleIcon = ((System.Drawing.Icon)(resources.GetObject("IssueWizard.TitleIcon")));
            // 
            // wizardPage1
            // 
            this.wizardPage1.Controls.Add(this.CreateButton);
            this.wizardPage1.Controls.Add(this.ViewButton);
            this.wizardPage1.Controls.Add(this.label1);
            this.wizardPage1.IsFinishPage = true;
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.ShowCancel = false;
            this.wizardPage1.Size = new System.Drawing.Size(507, 247);
            this.wizardPage1.TabIndex = 0;
            this.wizardPage1.Text = "Find a bug? Have a suggestion?";
            // 
            // CreateButton
            // 
            this.CreateButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CreateButton.Location = new System.Drawing.Point(2, 132);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(232, 45);
            this.CreateButton.TabIndex = 3;
            this.CreateButton.Text = "Create New Issue";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // ViewButton
            // 
            this.ViewButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ViewButton.Location = new System.Drawing.Point(2, 79);
            this.ViewButton.Name = "ViewButton";
            this.ViewButton.Size = new System.Drawing.Size(232, 45);
            this.ViewButton.TabIndex = 2;
            this.ViewButton.Text = "View Open Issues";
            this.ViewButton.UseVisualStyleBackColor = true;
            this.ViewButton.Click += new System.EventHandler(this.ViewButton_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(478, 65);
            this.label1.TabIndex = 0;
            this.label1.Text = "We\'d love to hear from you in order to make J-Runner with Extras better!\r\n\r\nPleas" +
    "e make sure you check open issues first before creating an issue, as duplicates " +
    "will be closed.";
            // 
            // Issues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 401);
            this.Controls.Add(this.IssueWizard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Issues";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Report Issue";
            ((System.ComponentModel.ISupportInitialize)(this.IssueWizard)).EndInit();
            this.wizardPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl IssueWizard;
        private AeroWizard.WizardPage wizardPage1;
        private System.Windows.Forms.Label label1;
        private UI.CommandLink ViewButton;
        private UI.CommandLink CreateButton;
    }
}