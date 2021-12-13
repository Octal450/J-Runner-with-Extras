namespace JRunner
{
    partial class DashLaunch_Help
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashLaunch_Help));
            this.txtHelp = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtHelp
            // 
            this.txtHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHelp.Location = new System.Drawing.Point(0, 0);
            this.txtHelp.Multiline = true;
            this.txtHelp.Name = "txtHelp";
            this.txtHelp.ReadOnly = true;
            this.txtHelp.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHelp.Size = new System.Drawing.Size(539, 519);
            this.txtHelp.TabIndex = 0;
            this.txtHelp.Text = resources.GetString("txtHelp.Text");
            // 
            // DashLaunch_Help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 519);
            this.Controls.Add(this.txtHelp);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DashLaunch_Help";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "dashlaunch_help";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHelp;
    }
}