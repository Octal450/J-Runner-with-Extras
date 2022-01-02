
namespace JRunner.Forms
{
    partial class CreateDonorNand
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateDonorNand));
            this.DonorWizard = new AeroWizard.WizardControl();
            this.PrereqPage = new AeroWizard.WizardPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.CpuKvPage = new AeroWizard.WizardPage();
            this.DonorKvText = new System.Windows.Forms.Label();
            this.RetailKvWarn = new System.Windows.Forms.Label();
            this.CpuKeyGroup = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.CpuKeyBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.KvGroup = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.KvEllipse = new System.Windows.Forms.Button();
            this.KvBox = new System.Windows.Forms.TextBox();
            this.DonorKv = new System.Windows.Forms.CheckBox();
            this.FcrtPage = new AeroWizard.WizardPage();
            this.NoFcrtText = new System.Windows.Forms.Label();
            this.RetailFcrtWarn = new System.Windows.Forms.Label();
            this.NoFcrt = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.DonorFcrt = new System.Windows.Forms.CheckBox();
            this.FcrtGroup = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.FcrtEllipse = new System.Windows.Forms.Button();
            this.FcrtBox = new System.Windows.Forms.TextBox();
            this.DonorFcrtText = new System.Windows.Forms.Label();
            this.LdvSmcConfPage = new AeroWizard.WizardPage();
            this.SmcConfigGroup = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.SmcConfigEllipse = new System.Windows.Forms.Button();
            this.SmcConfigBox = new System.Windows.Forms.TextBox();
            this.DonorSmcConfig = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.LdvGroup = new System.Windows.Forms.GroupBox();
            this.LdvBox = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.FinishPage = new AeroWizard.WizardPage();
            this.RevLdv = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.RevKernel = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.RevSmc = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.RevHack = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.RevConsole = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DonorWizard)).BeginInit();
            this.PrereqPage.SuspendLayout();
            this.CpuKvPage.SuspendLayout();
            this.CpuKeyGroup.SuspendLayout();
            this.KvGroup.SuspendLayout();
            this.FcrtPage.SuspendLayout();
            this.FcrtGroup.SuspendLayout();
            this.LdvSmcConfPage.SuspendLayout();
            this.SmcConfigGroup.SuspendLayout();
            this.LdvGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LdvBox)).BeginInit();
            this.FinishPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // DonorWizard
            // 
            this.DonorWizard.BackColor = System.Drawing.Color.White;
            this.DonorWizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DonorWizard.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DonorWizard.Location = new System.Drawing.Point(0, 0);
            this.DonorWizard.Name = "DonorWizard";
            this.DonorWizard.Pages.Add(this.PrereqPage);
            this.DonorWizard.Pages.Add(this.CpuKvPage);
            this.DonorWizard.Pages.Add(this.FcrtPage);
            this.DonorWizard.Pages.Add(this.LdvSmcConfPage);
            this.DonorWizard.Pages.Add(this.FinishPage);
            this.DonorWizard.Size = new System.Drawing.Size(554, 401);
            this.DonorWizard.TabIndex = 0;
            this.DonorWizard.Text = "Create Donor Nand";
            this.DonorWizard.Title = "Create Donor Nand";
            this.DonorWizard.TitleIcon = ((System.Drawing.Icon)(resources.GetObject("DonorWizard.TitleIcon")));
            // 
            // PrereqPage
            // 
            this.PrereqPage.Controls.Add(this.label1);
            this.PrereqPage.Controls.Add(this.label8);
            this.PrereqPage.Name = "PrereqPage";
            this.PrereqPage.Size = new System.Drawing.Size(507, 250);
            this.PrereqPage.TabIndex = 4;
            this.PrereqPage.Text = "Prerequisites";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 226);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Click Next when you are ready to continue";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(2, 2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(478, 150);
            this.label8.TabIndex = 0;
            this.label8.Text = resources.GetString("label8.Text");
            // 
            // CpuKvPage
            // 
            this.CpuKvPage.AllowNext = false;
            this.CpuKvPage.Controls.Add(this.DonorKvText);
            this.CpuKvPage.Controls.Add(this.RetailKvWarn);
            this.CpuKvPage.Controls.Add(this.CpuKeyGroup);
            this.CpuKvPage.Controls.Add(this.label3);
            this.CpuKvPage.Controls.Add(this.KvGroup);
            this.CpuKvPage.Controls.Add(this.DonorKv);
            this.CpuKvPage.Name = "CpuKvPage";
            this.CpuKvPage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CpuKvPage.Size = new System.Drawing.Size(507, 250);
            this.CpuKvPage.TabIndex = 1;
            this.CpuKvPage.Text = "CPU Key and Keyvault";
            // 
            // DonorKvText
            // 
            this.DonorKvText.AutoSize = true;
            this.DonorKvText.Location = new System.Drawing.Point(0, 213);
            this.DonorKvText.Name = "DonorKvText";
            this.DonorKvText.Size = new System.Drawing.Size(308, 15);
            this.DonorKvText.TabIndex = 1;
            this.DonorKvText.Text = "If you don\'t have a keyvault to use, check Donor Keyvault";
            // 
            // RetailKvWarn
            // 
            this.RetailKvWarn.AutoSize = true;
            this.RetailKvWarn.ForeColor = System.Drawing.Color.Firebrick;
            this.RetailKvWarn.Location = new System.Drawing.Point(207, 48);
            this.RetailKvWarn.Name = "RetailKvWarn";
            this.RetailKvWarn.Size = new System.Drawing.Size(271, 15);
            this.RetailKvWarn.TabIndex = 4;
            this.RetailKvWarn.Text = "Retail Nands require the KV to match the CPU key!";
            // 
            // CpuKeyGroup
            // 
            this.CpuKeyGroup.Controls.Add(this.label13);
            this.CpuKeyGroup.Controls.Add(this.CpuKeyBox);
            this.CpuKeyGroup.Location = new System.Drawing.Point(4, -5);
            this.CpuKeyGroup.Name = "CpuKeyGroup";
            this.CpuKeyGroup.Size = new System.Drawing.Size(470, 45);
            this.CpuKeyGroup.TabIndex = 3;
            this.CpuKeyGroup.TabStop = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 15);
            this.label13.TabIndex = 2;
            this.label13.Text = "CPU Key:";
            // 
            // CpuKeyBox
            // 
            this.CpuKeyBox.AllowDrop = true;
            this.CpuKeyBox.Location = new System.Drawing.Point(67, 15);
            this.CpuKeyBox.MaxLength = 32;
            this.CpuKeyBox.Name = "CpuKeyBox";
            this.CpuKeyBox.Size = new System.Drawing.Size(397, 23);
            this.CpuKeyBox.TabIndex = 0;
            this.CpuKeyBox.TextChanged += new System.EventHandler(this.CpuKeyBox_TextChanged);
            this.CpuKeyBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.CpuKeyBox_DragDrop);
            this.CpuKeyBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.CpuKeyBox_DragEnter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 228);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(390, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "If your keyvault requires an FCRT, you will be prompted on the next page";
            // 
            // KvGroup
            // 
            this.KvGroup.Controls.Add(this.label2);
            this.KvGroup.Controls.Add(this.KvEllipse);
            this.KvGroup.Controls.Add(this.KvBox);
            this.KvGroup.Location = new System.Drawing.Point(4, 64);
            this.KvGroup.Name = "KvGroup";
            this.KvGroup.Size = new System.Drawing.Size(470, 45);
            this.KvGroup.TabIndex = 1;
            this.KvGroup.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "KV:";
            // 
            // KvEllipse
            // 
            this.KvEllipse.Location = new System.Drawing.Point(431, 14);
            this.KvEllipse.Name = "KvEllipse";
            this.KvEllipse.Size = new System.Drawing.Size(33, 25);
            this.KvEllipse.TabIndex = 1;
            this.KvEllipse.Text = "...";
            this.KvEllipse.UseVisualStyleBackColor = true;
            this.KvEllipse.Click += new System.EventHandler(this.KvEllipse_Click);
            // 
            // KvBox
            // 
            this.KvBox.AllowDrop = true;
            this.KvBox.Location = new System.Drawing.Point(67, 15);
            this.KvBox.Name = "KvBox";
            this.KvBox.Size = new System.Drawing.Size(358, 23);
            this.KvBox.TabIndex = 0;
            this.KvBox.TextChanged += new System.EventHandler(this.KvBox_TextChanged);
            this.KvBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.KvBox_DragDrop);
            this.KvBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.KvBox_DragEnter);
            // 
            // DonorKv
            // 
            this.DonorKv.AutoSize = true;
            this.DonorKv.Location = new System.Drawing.Point(4, 47);
            this.DonorKv.Name = "DonorKv";
            this.DonorKv.Size = new System.Drawing.Size(107, 19);
            this.DonorKv.TabIndex = 0;
            this.DonorKv.Text = "Donor Keyvault";
            this.DonorKv.UseVisualStyleBackColor = true;
            this.DonorKv.CheckedChanged += new System.EventHandler(this.DonorKv_CheckedChanged);
            // 
            // FcrtPage
            // 
            this.FcrtPage.AllowNext = false;
            this.FcrtPage.Controls.Add(this.NoFcrtText);
            this.FcrtPage.Controls.Add(this.RetailFcrtWarn);
            this.FcrtPage.Controls.Add(this.NoFcrt);
            this.FcrtPage.Controls.Add(this.label7);
            this.FcrtPage.Controls.Add(this.DonorFcrt);
            this.FcrtPage.Controls.Add(this.FcrtGroup);
            this.FcrtPage.Controls.Add(this.DonorFcrtText);
            this.FcrtPage.Name = "FcrtPage";
            this.FcrtPage.Size = new System.Drawing.Size(507, 250);
            this.FcrtPage.TabIndex = 2;
            this.FcrtPage.Text = "FCRT";
            // 
            // NoFcrtText
            // 
            this.NoFcrtText.Location = new System.Drawing.Point(2, 118);
            this.NoFcrtText.Name = "NoFcrtText";
            this.NoFcrtText.Size = new System.Drawing.Size(476, 65);
            this.NoFcrtText.TabIndex = 9;
            this.NoFcrtText.Text = resources.GetString("NoFcrtText.Text");
            // 
            // RetailFcrtWarn
            // 
            this.RetailFcrtWarn.AutoSize = true;
            this.RetailFcrtWarn.ForeColor = System.Drawing.Color.Firebrick;
            this.RetailFcrtWarn.Location = new System.Drawing.Point(225, 5);
            this.RetailFcrtWarn.Name = "RetailFcrtWarn";
            this.RetailFcrtWarn.Size = new System.Drawing.Size(255, 15);
            this.RetailFcrtWarn.TabIndex = 8;
            this.RetailFcrtWarn.Text = "Retail Nands require the FCRT to match the KV!";
            // 
            // NoFcrt
            // 
            this.NoFcrt.AutoSize = true;
            this.NoFcrt.Checked = true;
            this.NoFcrt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NoFcrt.Location = new System.Drawing.Point(4, 194);
            this.NoFcrt.Name = "NoFcrt";
            this.NoFcrt.Size = new System.Drawing.Size(150, 19);
            this.NoFcrt.TabIndex = 7;
            this.NoFcrt.Text = "nofcrt (Recommended)";
            this.NoFcrt.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(2, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(476, 33);
            this.label7.TabIndex = 6;
            this.label7.Text = "It is not possible for this tool to check your supplied fcrt for validity, please" +
    " ensure you provide a valid decrypted FCRT";
            // 
            // DonorFcrt
            // 
            this.DonorFcrt.AutoSize = true;
            this.DonorFcrt.Location = new System.Drawing.Point(4, 4);
            this.DonorFcrt.Name = "DonorFcrt";
            this.DonorFcrt.Size = new System.Drawing.Size(90, 19);
            this.DonorFcrt.TabIndex = 3;
            this.DonorFcrt.Text = "Donor FCRT";
            this.DonorFcrt.UseVisualStyleBackColor = true;
            this.DonorFcrt.CheckedChanged += new System.EventHandler(this.DonorFcrt_CheckedChanged);
            // 
            // FcrtGroup
            // 
            this.FcrtGroup.Controls.Add(this.label4);
            this.FcrtGroup.Controls.Add(this.FcrtEllipse);
            this.FcrtGroup.Controls.Add(this.FcrtBox);
            this.FcrtGroup.Location = new System.Drawing.Point(4, 21);
            this.FcrtGroup.Name = "FcrtGroup";
            this.FcrtGroup.Size = new System.Drawing.Size(470, 46);
            this.FcrtGroup.TabIndex = 4;
            this.FcrtGroup.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "FCRT:";
            // 
            // FcrtEllipse
            // 
            this.FcrtEllipse.Location = new System.Drawing.Point(431, 14);
            this.FcrtEllipse.Name = "FcrtEllipse";
            this.FcrtEllipse.Size = new System.Drawing.Size(33, 25);
            this.FcrtEllipse.TabIndex = 1;
            this.FcrtEllipse.Text = "...";
            this.FcrtEllipse.UseVisualStyleBackColor = true;
            this.FcrtEllipse.Click += new System.EventHandler(this.FcrtEllipse_Click);
            // 
            // FcrtBox
            // 
            this.FcrtBox.AllowDrop = true;
            this.FcrtBox.Location = new System.Drawing.Point(48, 15);
            this.FcrtBox.Name = "FcrtBox";
            this.FcrtBox.Size = new System.Drawing.Size(377, 23);
            this.FcrtBox.TabIndex = 0;
            this.FcrtBox.TextChanged += new System.EventHandler(this.FcrtBox_TextChanged);
            this.FcrtBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.FcrtBox_DragDrop);
            this.FcrtBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.FcrtBox_DragEnter);
            // 
            // DonorFcrtText
            // 
            this.DonorFcrtText.AutoSize = true;
            this.DonorFcrtText.Location = new System.Drawing.Point(2, 228);
            this.DonorFcrtText.Name = "DonorFcrtText";
            this.DonorFcrtText.Size = new System.Drawing.Size(282, 15);
            this.DonorFcrtText.TabIndex = 5;
            this.DonorFcrtText.Text = "If you don\'t have an FCRT to use, check Donor FCRT";
            // 
            // LdvSmcConfPage
            // 
            this.LdvSmcConfPage.Controls.Add(this.SmcConfigGroup);
            this.LdvSmcConfPage.Controls.Add(this.DonorSmcConfig);
            this.LdvSmcConfPage.Controls.Add(this.label14);
            this.LdvSmcConfPage.Controls.Add(this.LdvGroup);
            this.LdvSmcConfPage.Name = "LdvSmcConfPage";
            this.LdvSmcConfPage.Size = new System.Drawing.Size(507, 250);
            this.LdvSmcConfPage.TabIndex = 5;
            this.LdvSmcConfPage.Text = "Lock Down Value and SMC Config";
            // 
            // SmcConfigGroup
            // 
            this.SmcConfigGroup.Controls.Add(this.label16);
            this.SmcConfigGroup.Controls.Add(this.SmcConfigEllipse);
            this.SmcConfigGroup.Controls.Add(this.SmcConfigBox);
            this.SmcConfigGroup.Enabled = false;
            this.SmcConfigGroup.Location = new System.Drawing.Point(4, 202);
            this.SmcConfigGroup.Name = "SmcConfigGroup";
            this.SmcConfigGroup.Size = new System.Drawing.Size(470, 45);
            this.SmcConfigGroup.TabIndex = 7;
            this.SmcConfigGroup.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 18);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(74, 15);
            this.label16.TabIndex = 2;
            this.label16.Text = "SMC Config:";
            // 
            // SmcConfigEllipse
            // 
            this.SmcConfigEllipse.Location = new System.Drawing.Point(431, 14);
            this.SmcConfigEllipse.Name = "SmcConfigEllipse";
            this.SmcConfigEllipse.Size = new System.Drawing.Size(33, 25);
            this.SmcConfigEllipse.TabIndex = 1;
            this.SmcConfigEllipse.Text = "...";
            this.SmcConfigEllipse.UseVisualStyleBackColor = true;
            this.SmcConfigEllipse.Click += new System.EventHandler(this.SmcConfigEllipse_Click);
            // 
            // SmcConfigBox
            // 
            this.SmcConfigBox.AllowDrop = true;
            this.SmcConfigBox.Location = new System.Drawing.Point(86, 15);
            this.SmcConfigBox.Name = "SmcConfigBox";
            this.SmcConfigBox.Size = new System.Drawing.Size(339, 23);
            this.SmcConfigBox.TabIndex = 0;
            this.SmcConfigBox.TextChanged += new System.EventHandler(this.SmcConfigBox_TextChanged);
            this.SmcConfigBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.SmcConfigBox_DragDrop);
            this.SmcConfigBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.SmcConfigBox_DragEnter);
            // 
            // DonorSmcConfig
            // 
            this.DonorSmcConfig.AutoSize = true;
            this.DonorSmcConfig.Checked = true;
            this.DonorSmcConfig.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DonorSmcConfig.Location = new System.Drawing.Point(4, 185);
            this.DonorSmcConfig.Name = "DonorSmcConfig";
            this.DonorSmcConfig.Size = new System.Drawing.Size(126, 19);
            this.DonorSmcConfig.TabIndex = 6;
            this.DonorSmcConfig.Text = "Donor SMC Config";
            this.DonorSmcConfig.UseVisualStyleBackColor = true;
            this.DonorSmcConfig.CheckedChanged += new System.EventHandler(this.DonorSmcConfig_CheckedChanged);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(2, 2);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(476, 137);
            this.label14.TabIndex = 5;
            this.label14.Text = resources.GetString("label14.Text");
            // 
            // LdvGroup
            // 
            this.LdvGroup.Controls.Add(this.LdvBox);
            this.LdvGroup.Controls.Add(this.label5);
            this.LdvGroup.Location = new System.Drawing.Point(4, 132);
            this.LdvGroup.Name = "LdvGroup";
            this.LdvGroup.Size = new System.Drawing.Size(90, 46);
            this.LdvGroup.TabIndex = 4;
            this.LdvGroup.TabStop = false;
            // 
            // LdvBox
            // 
            this.LdvBox.BackColor = System.Drawing.Color.Gainsboro;
            this.LdvBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LdvBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.LdvBox.ForeColor = System.Drawing.Color.Black;
            this.LdvBox.Location = new System.Drawing.Point(43, 18);
            this.LdvBox.Maximum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.LdvBox.Name = "LdvBox";
            this.LdvBox.Size = new System.Drawing.Size(37, 17);
            this.LdvBox.TabIndex = 21;
            this.LdvBox.TabStop = false;
            this.LdvBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "LDV:";
            // 
            // FinishPage
            // 
            this.FinishPage.Controls.Add(this.RevLdv);
            this.FinishPage.Controls.Add(this.label15);
            this.FinishPage.Controls.Add(this.RevKernel);
            this.FinishPage.Controls.Add(this.label12);
            this.FinishPage.Controls.Add(this.RevSmc);
            this.FinishPage.Controls.Add(this.label11);
            this.FinishPage.Controls.Add(this.RevHack);
            this.FinishPage.Controls.Add(this.label10);
            this.FinishPage.Controls.Add(this.RevConsole);
            this.FinishPage.Controls.Add(this.label9);
            this.FinishPage.Controls.Add(this.label6);
            this.FinishPage.IsFinishPage = true;
            this.FinishPage.Name = "FinishPage";
            this.FinishPage.Size = new System.Drawing.Size(507, 250);
            this.FinishPage.TabIndex = 3;
            this.FinishPage.Text = "All Set?";
            // 
            // RevLdv
            // 
            this.RevLdv.Location = new System.Drawing.Point(70, 183);
            this.RevLdv.Name = "RevLdv";
            this.RevLdv.ReadOnly = true;
            this.RevLdv.Size = new System.Drawing.Size(80, 23);
            this.RevLdv.TabIndex = 10;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(2, 186);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(34, 15);
            this.label15.TabIndex = 9;
            this.label15.Text = "LDV: ";
            // 
            // RevKernel
            // 
            this.RevKernel.Location = new System.Drawing.Point(70, 154);
            this.RevKernel.Name = "RevKernel";
            this.RevKernel.ReadOnly = true;
            this.RevKernel.Size = new System.Drawing.Size(80, 23);
            this.RevKernel.TabIndex = 8;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(2, 157);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 15);
            this.label12.TabIndex = 7;
            this.label12.Text = "Kernel: ";
            // 
            // RevSmc
            // 
            this.RevSmc.Location = new System.Drawing.Point(70, 125);
            this.RevSmc.Name = "RevSmc";
            this.RevSmc.ReadOnly = true;
            this.RevSmc.Size = new System.Drawing.Size(80, 23);
            this.RevSmc.TabIndex = 6;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(2, 128);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 15);
            this.label11.TabIndex = 5;
            this.label11.Text = "SMC: ";
            // 
            // RevHack
            // 
            this.RevHack.Location = new System.Drawing.Point(70, 96);
            this.RevHack.Name = "RevHack";
            this.RevHack.ReadOnly = true;
            this.RevHack.Size = new System.Drawing.Size(80, 23);
            this.RevHack.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(2, 99);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 15);
            this.label10.TabIndex = 3;
            this.label10.Text = "Hack Type: ";
            // 
            // RevConsole
            // 
            this.RevConsole.Location = new System.Drawing.Point(70, 67);
            this.RevConsole.Name = "RevConsole";
            this.RevConsole.ReadOnly = true;
            this.RevConsole.Size = new System.Drawing.Size(80, 23);
            this.RevConsole.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(2, 70);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 15);
            this.label9.TabIndex = 1;
            this.label9.Text = "Console: ";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(2, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(476, 33);
            this.label6.TabIndex = 0;
            this.label6.Text = "The wizard is ready to create your Donor Nand. Please review the information belo" +
    "w to ensure everything is correct, then click Finish to build the image";
            // 
            // CreateDonorNand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 401);
            this.Controls.Add(this.DonorWizard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateDonorNand";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Donor Nand";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.DonorWizard)).EndInit();
            this.PrereqPage.ResumeLayout(false);
            this.PrereqPage.PerformLayout();
            this.CpuKvPage.ResumeLayout(false);
            this.CpuKvPage.PerformLayout();
            this.CpuKeyGroup.ResumeLayout(false);
            this.CpuKeyGroup.PerformLayout();
            this.KvGroup.ResumeLayout(false);
            this.KvGroup.PerformLayout();
            this.FcrtPage.ResumeLayout(false);
            this.FcrtPage.PerformLayout();
            this.FcrtGroup.ResumeLayout(false);
            this.FcrtGroup.PerformLayout();
            this.LdvSmcConfPage.ResumeLayout(false);
            this.LdvSmcConfPage.PerformLayout();
            this.SmcConfigGroup.ResumeLayout(false);
            this.SmcConfigGroup.PerformLayout();
            this.LdvGroup.ResumeLayout(false);
            this.LdvGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LdvBox)).EndInit();
            this.FinishPage.ResumeLayout(false);
            this.FinishPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl DonorWizard;
        private AeroWizard.WizardPage CpuKvPage;
        private System.Windows.Forms.GroupBox KvGroup;
        private System.Windows.Forms.Button KvEllipse;
        private System.Windows.Forms.TextBox KvBox;
        private System.Windows.Forms.CheckBox DonorKv;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private AeroWizard.WizardPage FcrtPage;
        private System.Windows.Forms.CheckBox DonorFcrt;
        private System.Windows.Forms.GroupBox FcrtGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button FcrtEllipse;
        private System.Windows.Forms.TextBox FcrtBox;
        private System.Windows.Forms.Label DonorFcrtText;
        private AeroWizard.WizardPage FinishPage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private AeroWizard.WizardPage PrereqPage;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox RevConsole;
        private System.Windows.Forms.TextBox RevHack;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox RevKernel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox RevSmc;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox CpuKeyGroup;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox CpuKeyBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox NoFcrt;
        private System.Windows.Forms.Label RetailKvWarn;
        private System.Windows.Forms.Label RetailFcrtWarn;
        private System.Windows.Forms.Label NoFcrtText;
        private System.Windows.Forms.Label DonorKvText;
        private AeroWizard.WizardPage LdvSmcConfPage;
        private System.Windows.Forms.GroupBox LdvGroup;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown LdvBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox RevLdv;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox SmcConfigGroup;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button SmcConfigEllipse;
        private System.Windows.Forms.TextBox SmcConfigBox;
        private System.Windows.Forms.CheckBox DonorSmcConfig;
    }
}