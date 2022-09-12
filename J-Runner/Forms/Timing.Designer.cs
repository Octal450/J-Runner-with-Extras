namespace JRunner.Forms
{
    partial class Timing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Timing));
            this.btn1 = new System.Windows.Forms.Button();
            this.topLabel = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.btn3 = new System.Windows.Forms.Button();
            this.btn4 = new System.Windows.Forms.Button();
            this.btn5 = new System.Windows.Forms.Button();
            this.doneLabel = new System.Windows.Forms.Label();
            this.btnA = new System.Windows.Forms.Button();
            this.btnB = new System.Windows.Forms.Button();
            this.btnC = new System.Windows.Forms.Button();
            this.btnD = new System.Windows.Forms.Button();
            this.btnE = new System.Windows.Forms.Button();
            this.btnF = new System.Windows.Forms.Button();
            this.instructionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn1
            // 
            this.btn1.Location = new System.Drawing.Point(9, 94);
            this.btn1.Margin = new System.Windows.Forms.Padding(2);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(66, 50);
            this.btn1.TabIndex = 1;
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // topLabel
            // 
            this.topLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.topLabel.Location = new System.Drawing.Point(0, 0);
            this.topLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.topLabel.Name = "topLabel";
            this.topLabel.Size = new System.Drawing.Size(364, 56);
            this.topLabel.TabIndex = 3;
            this.topLabel.Text = "Select Board Type";
            this.topLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(11, 195);
            this.btnReset.Margin = new System.Windows.Forms.Padding(2);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(45, 24);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Visible = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btn2
            // 
            this.btn2.Location = new System.Drawing.Point(79, 94);
            this.btn2.Margin = new System.Windows.Forms.Padding(2);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(66, 50);
            this.btn2.TabIndex = 6;
            this.btn2.UseVisualStyleBackColor = true;
            this.btn2.Click += new System.EventHandler(this.btn2_Click);
            // 
            // btn3
            // 
            this.btn3.Location = new System.Drawing.Point(149, 94);
            this.btn3.Margin = new System.Windows.Forms.Padding(2);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(66, 50);
            this.btn3.TabIndex = 7;
            this.btn3.UseVisualStyleBackColor = true;
            this.btn3.Click += new System.EventHandler(this.btn3_Click);
            // 
            // btn4
            // 
            this.btn4.Location = new System.Drawing.Point(219, 94);
            this.btn4.Margin = new System.Windows.Forms.Padding(2);
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(66, 50);
            this.btn4.TabIndex = 8;
            this.btn4.UseVisualStyleBackColor = true;
            this.btn4.Click += new System.EventHandler(this.btn4_Click);
            // 
            // btn5
            // 
            this.btn5.Location = new System.Drawing.Point(289, 94);
            this.btn5.Margin = new System.Windows.Forms.Padding(2);
            this.btn5.Name = "btn5";
            this.btn5.Size = new System.Drawing.Size(66, 50);
            this.btn5.TabIndex = 9;
            this.btn5.UseVisualStyleBackColor = true;
            this.btn5.Click += new System.EventHandler(this.btn5_Click);
            // 
            // doneLabel
            // 
            this.doneLabel.AutoSize = true;
            this.doneLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doneLabel.Location = new System.Drawing.Point(63, 199);
            this.doneLabel.Name = "doneLabel";
            this.doneLabel.Size = new System.Drawing.Size(286, 16);
            this.doneLabel.TabIndex = 11;
            this.doneLabel.Text = "First timing has been auto-selected to program.";
            this.doneLabel.Visible = false;
            // 
            // btnA
            // 
            this.btnA.Location = new System.Drawing.Point(10, 94);
            this.btnA.Margin = new System.Windows.Forms.Padding(2);
            this.btnA.Name = "btnA";
            this.btnA.Size = new System.Drawing.Size(54, 50);
            this.btnA.TabIndex = 12;
            this.btnA.Text = "Xenon";
            this.btnA.UseVisualStyleBackColor = true;
            this.btnA.Click += new System.EventHandler(this.btnA_Click);
            // 
            // btnB
            // 
            this.btnB.Location = new System.Drawing.Point(68, 94);
            this.btnB.Margin = new System.Windows.Forms.Padding(2);
            this.btnB.Name = "btnB";
            this.btnB.Size = new System.Drawing.Size(54, 50);
            this.btnB.TabIndex = 13;
            this.btnB.Text = "Zephyr";
            this.btnB.UseVisualStyleBackColor = true;
            this.btnB.Click += new System.EventHandler(this.btnB_Click);
            // 
            // btnC
            // 
            this.btnC.Location = new System.Drawing.Point(126, 94);
            this.btnC.Margin = new System.Windows.Forms.Padding(2);
            this.btnC.Name = "btnC";
            this.btnC.Size = new System.Drawing.Size(54, 50);
            this.btnC.TabIndex = 14;
            this.btnC.Text = "Falcon";
            this.btnC.UseVisualStyleBackColor = true;
            this.btnC.Click += new System.EventHandler(this.btnC_Click);
            // 
            // btnD
            // 
            this.btnD.Location = new System.Drawing.Point(184, 94);
            this.btnD.Margin = new System.Windows.Forms.Padding(2);
            this.btnD.Name = "btnD";
            this.btnD.Size = new System.Drawing.Size(54, 50);
            this.btnD.TabIndex = 15;
            this.btnD.Text = "Jasper";
            this.btnD.UseVisualStyleBackColor = true;
            this.btnD.Click += new System.EventHandler(this.btnD_Click);
            // 
            // btnE
            // 
            this.btnE.Location = new System.Drawing.Point(242, 94);
            this.btnE.Margin = new System.Windows.Forms.Padding(2);
            this.btnE.Name = "btnE";
            this.btnE.Size = new System.Drawing.Size(54, 50);
            this.btnE.TabIndex = 16;
            this.btnE.Text = "Trinity";
            this.btnE.UseVisualStyleBackColor = true;
            this.btnE.Click += new System.EventHandler(this.btnE_Click);
            // 
            // btnF
            // 
            this.btnF.Location = new System.Drawing.Point(300, 94);
            this.btnF.Margin = new System.Windows.Forms.Padding(2);
            this.btnF.Name = "btnF";
            this.btnF.Size = new System.Drawing.Size(54, 50);
            this.btnF.TabIndex = 17;
            this.btnF.Text = "Corona";
            this.btnF.UseVisualStyleBackColor = true;
            this.btnF.Click += new System.EventHandler(this.btnF_Click);
            // 
            // instructionLabel
            // 
            this.instructionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.instructionLabel.Location = new System.Drawing.Point(0, 56);
            this.instructionLabel.Name = "instructionLabel";
            this.instructionLabel.Size = new System.Drawing.Size(364, 137);
            this.instructionLabel.TabIndex = 18;
            this.instructionLabel.Text = "text";
            this.instructionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Timing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 229);
            this.Controls.Add(this.instructionLabel);
            this.Controls.Add(this.btnF);
            this.Controls.Add(this.btnE);
            this.Controls.Add(this.btnD);
            this.Controls.Add(this.btnC);
            this.Controls.Add(this.btnB);
            this.Controls.Add(this.btnA);
            this.Controls.Add(this.doneLabel);
            this.Controls.Add(this.btn5);
            this.Controls.Add(this.btn4);
            this.Controls.Add(this.btn3);
            this.Controls.Add(this.btn2);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.topLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Timing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Timing Assistant";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label topLabel;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn3;
        private System.Windows.Forms.Button btn4;
        private System.Windows.Forms.Button btn5;
        private System.Windows.Forms.Label doneLabel;
        private System.Windows.Forms.Button btnA;
        private System.Windows.Forms.Button btnB;
        private System.Windows.Forms.Button btnC;
        private System.Windows.Forms.Button btnD;
        private System.Windows.Forms.Button btnE;
        private System.Windows.Forms.Button btnF;
        private System.Windows.Forms.Label instructionLabel;
    }
}