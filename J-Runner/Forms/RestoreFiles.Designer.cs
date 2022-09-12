
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
            this.RestorePage = new AeroWizard.WizardPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RestoreWizard)).BeginInit();
            this.RestorePage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RestoreWizard
            // 
            this.RestoreWizard.FinishButtonText = "&Next";
            this.RestoreWizard.Location = new System.Drawing.Point(0, 0);
            this.RestoreWizard.Name = "RestoreWizard";
            this.RestoreWizard.Pages.Add(this.RestorePage);
            this.RestoreWizard.Size = new System.Drawing.Size(554, 401);
            this.RestoreWizard.TabIndex = 0;
            this.RestoreWizard.Title = "Restore Files";
            this.RestoreWizard.TitleIcon = ((System.Drawing.Icon)(resources.GetObject("RestoreWizard.TitleIcon")));
            // 
            // RestorePage
            // 
            this.RestorePage.Controls.Add(this.groupBox1);
            this.RestorePage.Controls.Add(this.label2);
            this.RestorePage.Controls.Add(this.label1);
            this.RestorePage.Name = "RestorePage";
            this.RestorePage.Size = new System.Drawing.Size(507, 247);
            this.RestorePage.TabIndex = 0;
            this.RestorePage.Text = "Restore Files";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(151, 159);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(176, 43);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Save Downloaded Package";
            this.groupBox1.Visible = false;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(132, 17);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(41, 19);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "No";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(9, 17);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(42, 19);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "Yes";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(2, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(478, 50);
            this.label2.TabIndex = 3;
            this.label2.Text = "An internet connection is required for this feature to operate.\r\n\r\nAll files insi" +
    "de common and xeBuild will be deleted and replaced with clean versions!";
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
            this.RestorePage.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl RestoreWizard;
        private AeroWizard.WizardPage RestorePage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}