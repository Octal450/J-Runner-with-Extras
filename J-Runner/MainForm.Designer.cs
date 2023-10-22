using System.Windows.Forms;


namespace JRunner
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnExit = new System.Windows.Forms.Button();
            this.btnCompare = new System.Windows.Forms.Button();
            this.btnLoadExtra = new System.Windows.Forms.Button();
            this.btnLoadSource = new System.Windows.Forms.Button();
            this.txtFileExtra = new System.Windows.Forms.TextBox();
            this.txtFileSource = new System.Windows.Forms.TextBox();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.txtCPUKey = new System.Windows.Forms.TextBox();
            this.lblCpuKey = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnIPGetCPU = new UI.MenuButton();
            this.getCpuKeyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.getAndSaveToWorkingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToDesktopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelIP = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnScanner = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.btnNewSession = new System.Windows.Forms.Button();
            this.btnShowWorkingFolder = new UI.MenuButton();
            this.showWorkingFolderMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showDataFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showOutputFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showRootFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnBackup = new UI.MenuButton();
            this.backupContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.backupToZIPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoBackupNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLastBackupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureBackupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnScanDevices = new System.Windows.Forms.Button();
            this.XeBuildOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusBlank = new System.Windows.Forms.ToolStripStatusLabel();
            this.VersionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.BlankSpace1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.BackupLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.BlankSpace2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.CopiedToClipboard = new System.Windows.Forms.ToolStripStatusLabel();
            this.ModeStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ModeVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.FWStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.FWVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.FlashStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.FlashVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnCheckBadBlocks = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.txtBlocks = new System.Windows.Forms.TextBox();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.pnlTools = new System.Windows.Forms.Panel();
            this.pnlExtra = new System.Windows.Forms.Panel();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rescanDevicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.pOSTMonitorRATERToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOMMonitorAdvancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soundEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.cBFuseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timingAssistantToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.mTXUSBFirmwareUtilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xboxOneHDDToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nandTimingFunctionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.corona4GBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeFusionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.convertToRGH3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkSecdataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CustomXeBuildMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.hexEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kVViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cPUKeyToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jRPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.powerOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bootloaderModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.logPostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.demoNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.powerOnToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.powerOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleNANDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.connectToUARTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getInvalidBlocksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.updateFwToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateAvailableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changelogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shortcutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.reportIssueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.newSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createDonorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decryptKeyvaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.loadGlitch2XeLLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadJTAGXeLLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.sMCConfigViewerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.patchKVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeLDVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xFlasherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installDriversToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkConsoleCBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flashOpenXeniumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nANDXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mtxUsbModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jRPBLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateJRPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox8.SuspendLayout();
            this.getCpuKeyMenu.SuspendLayout();
            this.showWorkingFolderMenu.SuspendLayout();
            this.backupContextMenu.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnExit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExit.Location = new System.Drawing.Point(567, 550);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(85, 26);
            this.btnExit.TabIndex = 298;
            this.btnExit.Text = "Exit";
            this.toolTip1.SetToolTip(this.btnExit, "Exit the application");
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(390, 42);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(65, 22);
            this.btnCompare.TabIndex = 25;
            this.btnCompare.Text = "Compare";
            this.toolTip1.SetToolTip(this.btnCompare, "Compares the source and extra nands");
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // btnLoadExtra
            // 
            this.btnLoadExtra.Location = new System.Drawing.Point(7, 42);
            this.btnLoadExtra.Name = "btnLoadExtra";
            this.btnLoadExtra.Size = new System.Drawing.Size(76, 22);
            this.btnLoadExtra.TabIndex = 23;
            this.btnLoadExtra.Text = "Load Extra";
            this.toolTip1.SetToolTip(this.btnLoadExtra, "Load an extra nand for comparison");
            this.btnLoadExtra.UseVisualStyleBackColor = true;
            this.btnLoadExtra.Click += new System.EventHandler(this.btnLoadExtra_Click);
            // 
            // btnLoadSource
            // 
            this.btnLoadSource.Location = new System.Drawing.Point(7, 13);
            this.btnLoadSource.Name = "btnLoadSource";
            this.btnLoadSource.Size = new System.Drawing.Size(76, 22);
            this.btnLoadSource.TabIndex = 20;
            this.btnLoadSource.Text = "Load Source";
            this.toolTip1.SetToolTip(this.btnLoadSource, "Load the primary working nand");
            this.btnLoadSource.UseVisualStyleBackColor = true;
            this.btnLoadSource.Click += new System.EventHandler(this.btnLoadSource_Click);
            // 
            // txtFileExtra
            // 
            this.txtFileExtra.AllowDrop = true;
            this.txtFileExtra.BackColor = System.Drawing.Color.White;
            this.txtFileExtra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFileExtra.Location = new System.Drawing.Point(89, 43);
            this.txtFileExtra.Name = "txtFileExtra";
            this.txtFileExtra.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtFileExtra.Size = new System.Drawing.Size(295, 20);
            this.txtFileExtra.TabIndex = 24;
            this.toolTip1.SetToolTip(this.txtFileExtra, "The file listed in this box is used to compare against file loaded in \"Source fil" +
        "e\" box\r\n");
            this.txtFileExtra.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFileExtra_DragDrop);
            this.txtFileExtra.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtFileExtra_DragEnter);
            this.txtFileExtra.DoubleClick += new System.EventHandler(this.txtFileExtra_DoubleClick);
            // 
            // txtFileSource
            // 
            this.txtFileSource.AllowDrop = true;
            this.txtFileSource.BackColor = System.Drawing.SystemColors.Window;
            this.txtFileSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFileSource.Location = new System.Drawing.Point(89, 14);
            this.txtFileSource.Name = "txtFileSource";
            this.txtFileSource.Size = new System.Drawing.Size(295, 20);
            this.txtFileSource.TabIndex = 21;
            this.toolTip1.SetToolTip(this.txtFileSource, "The file in this box is used for all read/write/create operations.\r\nif you use th" +
        "e functions above out of order, ensure your required\r\nfile is loaded into this b" +
        "ox first.");
            this.txtFileSource.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFileSource_DragDrop);
            this.txtFileSource.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtFileSource_DragEnter);
            this.txtFileSource.DoubleClick += new System.EventHandler(this.txtFileSource_DoubleClick);
            // 
            // txtConsole
            // 
            this.txtConsole.BackColor = System.Drawing.Color.Black;
            this.txtConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtConsole.ForeColor = System.Drawing.Color.White;
            this.txtConsole.Location = new System.Drawing.Point(12, 322);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ReadOnly = true;
            this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConsole.Size = new System.Drawing.Size(462, 253);
            this.txtConsole.TabIndex = 22;
            this.txtConsole.TabStop = false;
            this.txtConsole.DoubleClick += new System.EventHandler(this.txtConsole_DoubleClick);
            // 
            // txtCPUKey
            // 
            this.txtCPUKey.AllowDrop = true;
            this.txtCPUKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCPUKey.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCPUKey.Location = new System.Drawing.Point(89, 72);
            this.txtCPUKey.MaxLength = 32;
            this.txtCPUKey.Name = "txtCPUKey";
            this.txtCPUKey.Size = new System.Drawing.Size(295, 20);
            this.txtCPUKey.TabIndex = 26;
            this.toolTip1.SetToolTip(this.txtCPUKey, "This is where your CPU key should be entered. You can drag and drop previously sa" +
        "ved cpukey.txt or paste in your CPU Key details.");
            this.txtCPUKey.TextChanged += new System.EventHandler(this.txtCPUKey_TextChanged);
            this.txtCPUKey.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtCPUKey_DragDrop);
            this.txtCPUKey.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtCPUKey_DragEnter);
            this.txtCPUKey.DoubleClick += new System.EventHandler(this.txtCPUKey_DoubleClick);
            // 
            // lblCpuKey
            // 
            this.lblCpuKey.AutoSize = true;
            this.lblCpuKey.BackColor = System.Drawing.Color.Transparent;
            this.lblCpuKey.ForeColor = System.Drawing.Color.Black;
            this.lblCpuKey.Location = new System.Drawing.Point(19, 74);
            this.lblCpuKey.Name = "lblCpuKey";
            this.lblCpuKey.Size = new System.Drawing.Size(53, 13);
            this.lblCpuKey.TabIndex = 27;
            this.lblCpuKey.Text = "CPU Key:";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(60, 297);
            this.progressBar.MarqueeAnimationSpeed = 50;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(347, 20);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 59;
            // 
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.groupBox8.Controls.Add(this.btnIPGetCPU);
            this.groupBox8.Controls.Add(this.labelIP);
            this.groupBox8.Controls.Add(this.txtIP);
            this.groupBox8.Location = new System.Drawing.Point(656, 483);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(164, 66);
            this.groupBox8.TabIndex = 299;
            this.groupBox8.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox8, "If you connect your Xbox 360 to your PC using a network cable, \r\nOnce booted with" +
        " Xellous or Xell-Reloaded place the displayed IP address in\r\nthe box and retriev" +
        "e your CPU Key by pressing the button.");
            // 
            // btnIPGetCPU
            // 
            this.btnIPGetCPU.AutoSize = true;
            this.btnIPGetCPU.BtnImage = ((System.Drawing.Image)(resources.GetObject("btnIPGetCPU.BtnImage")));
            this.btnIPGetCPU.ContextMenuStrip = this.getCpuKeyMenu;
            this.btnIPGetCPU.DropDownContextMenu = this.getCpuKeyMenu;
            this.btnIPGetCPU.Image = ((System.Drawing.Image)(resources.GetObject("btnIPGetCPU.Image")));
            this.btnIPGetCPU.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnIPGetCPU.Location = new System.Drawing.Point(6, 36);
            this.btnIPGetCPU.Name = "btnIPGetCPU";
            this.btnIPGetCPU.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnIPGetCPU.Size = new System.Drawing.Size(152, 26);
            this.btnIPGetCPU.SplitButton = true;
            this.btnIPGetCPU.TabIndex = 11;
            this.btnIPGetCPU.Text = "Get CPU Key";
            this.toolTip1.SetToolTip(this.btnIPGetCPU, "Tries to retrieve the CPU Key and Fuses from XeLL using the IP above");
            this.btnIPGetCPU.UseVisualStyleBackColor = true;
            this.btnIPGetCPU.Click += new System.EventHandler(this.btnIPGetCPU_Click);
            // 
            // getCpuKeyMenu
            // 
            this.getCpuKeyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getAndSaveToWorkingFolderToolStripMenuItem,
            this.saveToDesktopToolStripMenuItem});
            this.getCpuKeyMenu.Name = "contextMenuStrip1";
            this.getCpuKeyMenu.Size = new System.Drawing.Size(198, 48);
            // 
            // getAndSaveToWorkingFolderToolStripMenuItem
            // 
            this.getAndSaveToWorkingFolderToolStripMenuItem.Name = "getAndSaveToWorkingFolderToolStripMenuItem";
            this.getAndSaveToWorkingFolderToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.getAndSaveToWorkingFolderToolStripMenuItem.Text = "Save To Working Folder";
            this.getAndSaveToWorkingFolderToolStripMenuItem.Click += new System.EventHandler(this.getAndSaveToWorkingFolderToolStripMenuItem_Click);
            // 
            // saveToDesktopToolStripMenuItem
            // 
            this.saveToDesktopToolStripMenuItem.Name = "saveToDesktopToolStripMenuItem";
            this.saveToDesktopToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.saveToDesktopToolStripMenuItem.Text = "Save To Desktop";
            this.saveToDesktopToolStripMenuItem.Click += new System.EventHandler(this.saveToDesktopToolStripMenuItem_Click);
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(4, 15);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(20, 13);
            this.labelIP.TabIndex = 5;
            this.labelIP.Text = "IP:";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(29, 12);
            this.txtIP.MaxLength = 15;
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(128, 20);
            this.txtIP.TabIndex = 10;
            this.txtIP.DoubleClick += new System.EventHandler(this.txtIP_DoubleClick);
            this.txtIP.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtIP_KeyUp);
            // 
            // btnScanner
            // 
            this.btnScanner.Location = new System.Drawing.Point(662, 550);
            this.btnScanner.Name = "btnScanner";
            this.btnScanner.Size = new System.Drawing.Size(152, 26);
            this.btnScanner.TabIndex = 300;
            this.btnScanner.Text = "Scan IP Range";
            this.toolTip1.SetToolTip(this.btnScanner, "Scans the IP range looking for XeLL to retrieve the CPU Key and Fuses");
            this.btnScanner.UseVisualStyleBackColor = true;
            this.btnScanner.Click += new System.EventHandler(this.btnScanner_Click);
            // 
            // btnReload
            // 
            this.btnReload.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReload.Location = new System.Drawing.Point(390, 71);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(65, 22);
            this.btnReload.TabIndex = 27;
            this.btnReload.Text = "Reload";
            this.toolTip1.SetToolTip(this.btnReload, "Reloads and initializes the nand in source box");
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnNewSession
            // 
            this.btnNewSession.Location = new System.Drawing.Point(478, 519);
            this.btnNewSession.Name = "btnNewSession";
            this.btnNewSession.Size = new System.Drawing.Size(85, 26);
            this.btnNewSession.TabIndex = 295;
            this.btnNewSession.Text = "New Session";
            this.toolTip1.SetToolTip(this.btnNewSession, "Closes the current nand and starts a new session");
            this.btnNewSession.UseVisualStyleBackColor = true;
            this.btnNewSession.Click += new System.EventHandler(this.btnNewSession_Click);
            // 
            // btnShowWorkingFolder
            // 
            this.btnShowWorkingFolder.AutoSize = true;
            this.btnShowWorkingFolder.BtnImage = ((System.Drawing.Image)(resources.GetObject("btnShowWorkingFolder.BtnImage")));
            this.btnShowWorkingFolder.ContextMenuStrip = this.showWorkingFolderMenu;
            this.btnShowWorkingFolder.DropDownContextMenu = this.showWorkingFolderMenu;
            this.btnShowWorkingFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnShowWorkingFolder.Image")));
            this.btnShowWorkingFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnShowWorkingFolder.Location = new System.Drawing.Point(478, 488);
            this.btnShowWorkingFolder.Name = "btnShowWorkingFolder";
            this.btnShowWorkingFolder.Size = new System.Drawing.Size(174, 26);
            this.btnShowWorkingFolder.SplitButton = true;
            this.btnShowWorkingFolder.TabIndex = 294;
            this.btnShowWorkingFolder.Text = "Show Working Folder";
            this.toolTip1.SetToolTip(this.btnShowWorkingFolder, "Shows the working folder in Windows Explorer");
            this.btnShowWorkingFolder.UseVisualStyleBackColor = true;
            this.btnShowWorkingFolder.Click += new System.EventHandler(this.btnShowWorkingFolder_Click);
            // 
            // showWorkingFolderMenu
            // 
            this.showWorkingFolderMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showDataFolderToolStripMenuItem,
            this.showOutputFolderToolStripMenuItem,
            this.showRootFolderToolStripMenuItem});
            this.showWorkingFolderMenu.Name = "contextMenuStrip1";
            this.showWorkingFolderMenu.Size = new System.Drawing.Size(181, 70);
            // 
            // showDataFolderToolStripMenuItem
            // 
            this.showDataFolderToolStripMenuItem.Name = "showDataFolderToolStripMenuItem";
            this.showDataFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showDataFolderToolStripMenuItem.Text = "Show Data Folder";
            this.showDataFolderToolStripMenuItem.Click += new System.EventHandler(this.showDataFolderToolStripMenuItem_Click);
            // 
            // showOutputFolderToolStripMenuItem
            // 
            this.showOutputFolderToolStripMenuItem.Name = "showOutputFolderToolStripMenuItem";
            this.showOutputFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showOutputFolderToolStripMenuItem.Text = "Show Output Folder";
            this.showOutputFolderToolStripMenuItem.Click += new System.EventHandler(this.showOutputFolderToolStripMenuItem_Click);
            // 
            // showRootFolderToolStripMenuItem
            // 
            this.showRootFolderToolStripMenuItem.Name = "showRootFolderToolStripMenuItem";
            this.showRootFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showRootFolderToolStripMenuItem.Text = "Show Root Folder";
            this.showRootFolderToolStripMenuItem.Click += new System.EventHandler(this.showRootFolderToolStripMenuItem_Click);
            // 
            // btnRestart
            // 
            this.btnRestart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnRestart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRestart.Location = new System.Drawing.Point(478, 550);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(85, 26);
            this.btnRestart.TabIndex = 297;
            this.btnRestart.Text = "Restart";
            this.toolTip1.SetToolTip(this.btnRestart, "Completely restarts the application");
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnBackup
            // 
            this.btnBackup.BtnImage = global::JRunner.Properties.Resources.arrow_dn;
            this.btnBackup.ContextMenuStrip = this.backupContextMenu;
            this.btnBackup.DropDownContextMenu = this.backupContextMenu;
            this.btnBackup.Image = global::JRunner.Properties.Resources.arrow_dn;
            this.btnBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBackup.Location = new System.Drawing.Point(390, 13);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(65, 22);
            this.btnBackup.TabIndex = 22;
            this.btnBackup.Text = "Backup";
            this.btnBackup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.btnBackup, "Displays a menu of backup options");
            this.btnBackup.UseVisualStyleBackColor = true;
            // 
            // backupContextMenu
            // 
            this.backupContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backupToZIPToolStripMenuItem,
            this.autoBackupNowToolStripMenuItem,
            this.showLastBackupToolStripMenuItem,
            this.configureBackupToolStripMenuItem});
            this.backupContextMenu.Name = "contextMenuStrip1";
            this.backupContextMenu.Size = new System.Drawing.Size(179, 92);
            // 
            // backupToZIPToolStripMenuItem
            // 
            this.backupToZIPToolStripMenuItem.Name = "backupToZIPToolStripMenuItem";
            this.backupToZIPToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.backupToZIPToolStripMenuItem.Text = "Backup to ZIP…";
            this.backupToZIPToolStripMenuItem.Click += new System.EventHandler(this.backupToZIPToolStripMenuItem_Click);
            // 
            // autoBackupNowToolStripMenuItem
            // 
            this.autoBackupNowToolStripMenuItem.Enabled = false;
            this.autoBackupNowToolStripMenuItem.Name = "autoBackupNowToolStripMenuItem";
            this.autoBackupNowToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.autoBackupNowToolStripMenuItem.Text = "Auto Backup Now";
            this.autoBackupNowToolStripMenuItem.Click += new System.EventHandler(this.autoBackupNowToolStripMenuItem_Click);
            // 
            // showLastBackupToolStripMenuItem
            // 
            this.showLastBackupToolStripMenuItem.Name = "showLastBackupToolStripMenuItem";
            this.showLastBackupToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.showLastBackupToolStripMenuItem.Text = "Show Last Backup";
            this.showLastBackupToolStripMenuItem.Click += new System.EventHandler(this.showLastBackupToolStripMenuItem_Click);
            // 
            // configureBackupToolStripMenuItem
            // 
            this.configureBackupToolStripMenuItem.Name = "configureBackupToolStripMenuItem";
            this.configureBackupToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.configureBackupToolStripMenuItem.Text = "Configure Backup…";
            this.configureBackupToolStripMenuItem.Click += new System.EventHandler(this.configureBackupToolStripMenuItem_Click);
            // 
            // btnScanDevices
            // 
            this.btnScanDevices.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnScanDevices.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnScanDevices.Location = new System.Drawing.Point(567, 519);
            this.btnScanDevices.Name = "btnScanDevices";
            this.btnScanDevices.Size = new System.Drawing.Size(85, 26);
            this.btnScanDevices.TabIndex = 296;
            this.btnScanDevices.Text = "Scan Devices";
            this.toolTip1.SetToolTip(this.btnScanDevices, "Scans USB ports for programmers and other devices");
            this.btnScanDevices.UseVisualStyleBackColor = true;
            this.btnScanDevices.Click += new System.EventHandler(this.btnScanDevices_Click);
            // 
            // XeBuildOptionsToolStripMenuItem
            // 
            this.XeBuildOptionsToolStripMenuItem.Name = "XeBuildOptionsToolStripMenuItem";
            this.XeBuildOptionsToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.XeBuildOptionsToolStripMenuItem.Text = "XeBuild Options";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.groupBox4.Controls.Add(this.btnBackup);
            this.groupBox4.Controls.Add(this.btnLoadExtra);
            this.groupBox4.Controls.Add(this.btnReload);
            this.groupBox4.Controls.Add(this.txtFileExtra);
            this.groupBox4.Controls.Add(this.txtFileSource);
            this.groupBox4.Controls.Add(this.btnLoadSource);
            this.groupBox4.Controls.Add(this.lblCpuKey);
            this.groupBox4.Controls.Add(this.btnCompare);
            this.groupBox4.Controls.Add(this.txtCPUKey);
            this.groupBox4.Location = new System.Drawing.Point(12, 192);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(462, 101);
            this.groupBox4.TabIndex = 21;
            this.groupBox4.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusBlank,
            this.VersionLabel,
            this.BlankSpace1,
            this.BackupLabel,
            this.BlankSpace2,
            this.CopiedToClipboard,
            this.ModeStatus,
            this.ModeVersion,
            this.FWStatus,
            this.FWVersion,
            this.FlashStatus,
            this.FlashVersion});
            this.statusStrip1.Location = new System.Drawing.Point(0, 578);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(832, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 75;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusBlank
            // 
            this.StatusBlank.Name = "StatusBlank";
            this.StatusBlank.Size = new System.Drawing.Size(10, 17);
            this.StatusBlank.Text = " ";
            // 
            // VersionLabel
            // 
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(45, 17);
            this.VersionLabel.Text = "Version";
            this.VersionLabel.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // BlankSpace1
            // 
            this.BlankSpace1.Name = "BlankSpace1";
            this.BlankSpace1.Size = new System.Drawing.Size(10, 17);
            this.BlankSpace1.Text = " ";
            // 
            // BackupLabel
            // 
            this.BackupLabel.Name = "BackupLabel";
            this.BackupLabel.Size = new System.Drawing.Size(98, 17);
            this.BackupLabel.Text = "Auto Backup: Off";
            this.BackupLabel.Click += new System.EventHandler(this.configureBackupToolStripMenuItem_Click);
            // 
            // BlankSpace2
            // 
            this.BlankSpace2.Name = "BlankSpace2";
            this.BlankSpace2.Size = new System.Drawing.Size(10, 17);
            this.BlankSpace2.Text = " ";
            // 
            // CopiedToClipboard
            // 
            this.CopiedToClipboard.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CopiedToClipboard.Name = "CopiedToClipboard";
            this.CopiedToClipboard.Size = new System.Drawing.Size(119, 17);
            this.CopiedToClipboard.Text = "Copied to Clipboard!";
            this.CopiedToClipboard.Visible = false;
            // 
            // ModeStatus
            // 
            this.ModeStatus.AutoSize = false;
            this.ModeStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.ModeStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.ModeStatus.Name = "ModeStatus";
            this.ModeStatus.Size = new System.Drawing.Size(418, 17);
            this.ModeStatus.Spring = true;
            this.ModeStatus.Text = "MODE: ";
            this.ModeStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ModeStatus.Visible = false;
            // 
            // ModeVersion
            // 
            this.ModeVersion.AutoSize = false;
            this.ModeVersion.Name = "ModeVersion";
            this.ModeVersion.Size = new System.Drawing.Size(70, 17);
            this.ModeVersion.Text = "NOMODE";
            this.ModeVersion.Visible = false;
            // 
            // FWStatus
            // 
            this.FWStatus.AutoSize = false;
            this.FWStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.FWStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.FWStatus.Name = "FWStatus";
            this.FWStatus.Size = new System.Drawing.Size(34, 17);
            this.FWStatus.Text = "FW: ";
            this.FWStatus.Visible = false;
            // 
            // FWVersion
            // 
            this.FWVersion.AutoSize = false;
            this.FWVersion.Name = "FWVersion";
            this.FWVersion.Size = new System.Drawing.Size(28, 17);
            this.FWVersion.Text = "0.99";
            this.FWVersion.Visible = false;
            // 
            // FlashStatus
            // 
            this.FlashStatus.AutoSize = false;
            this.FlashStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.FlashStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.FlashStatus.Name = "FlashStatus";
            this.FlashStatus.Size = new System.Drawing.Size(44, 17);
            this.FlashStatus.Text = "FLASH: ";
            this.FlashStatus.Visible = false;
            // 
            // FlashVersion
            // 
            this.FlashVersion.AutoSize = false;
            this.FlashVersion.Name = "FlashVersion";
            this.FlashVersion.Size = new System.Drawing.Size(50, 17);
            this.FlashVersion.Text = "NOFLASH";
            this.FlashVersion.Visible = false;
            // 
            // btnCheckBadBlocks
            // 
            this.btnCheckBadBlocks.Location = new System.Drawing.Point(0, 0);
            this.btnCheckBadBlocks.Name = "btnCheckBadBlocks";
            this.btnCheckBadBlocks.Size = new System.Drawing.Size(75, 23);
            this.btnCheckBadBlocks.TabIndex = 0;
            // 
            // pnlInfo
            // 
            this.pnlInfo.Location = new System.Drawing.Point(479, 186);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(342, 298);
            this.pnlInfo.TabIndex = 23;
            this.pnlInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlInfo_Paint);
            // 
            // txtBlocks
            // 
            this.txtBlocks.BackColor = System.Drawing.SystemColors.Control;
            this.txtBlocks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBlocks.Location = new System.Drawing.Point(412, 300);
            this.txtBlocks.Name = "txtBlocks";
            this.txtBlocks.ReadOnly = true;
            this.txtBlocks.Size = new System.Drawing.Size(60, 13);
            this.txtBlocks.TabIndex = 60;
            this.txtBlocks.TabStop = false;
            this.txtBlocks.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.Location = new System.Drawing.Point(9, 300);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(48, 13);
            this.ProgressLabel.TabIndex = 83;
            this.ProgressLabel.Text = "Progress";
            // 
            // pnlTools
            // 
            this.pnlTools.Location = new System.Drawing.Point(9, 25);
            this.pnlTools.Name = "pnlTools";
            this.pnlTools.Size = new System.Drawing.Size(465, 173);
            this.pnlTools.TabIndex = 20;
            // 
            // pnlExtra
            // 
            this.pnlExtra.Location = new System.Drawing.Point(479, 25);
            this.pnlExtra.Name = "pnlExtra";
            this.pnlExtra.Size = new System.Drawing.Size(342, 156);
            this.pnlExtra.TabIndex = 22;
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rescanDevicesToolStripMenuItem,
            this.toolStripMenuItem3,
            this.pOSTMonitorRATERToolStripMenuItem,
            this.cOMMonitorAdvancedToolStripMenuItem,
            this.soundEditorToolStripMenuItem,
            this.toolStripMenuItem8,
            this.cBFuseToolStripMenuItem,
            this.timingAssistantToolStripMenuItem,
            this.toolStripMenuItem12,
            this.mTXUSBFirmwareUtilityToolStripMenuItem,
            this.xboxOneHDDToolToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // rescanDevicesToolStripMenuItem
            // 
            this.rescanDevicesToolStripMenuItem.Name = "rescanDevicesToolStripMenuItem";
            this.rescanDevicesToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.rescanDevicesToolStripMenuItem.Text = "Scan Devices";
            this.rescanDevicesToolStripMenuItem.Click += new System.EventHandler(this.btnScanDevices_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(205, 6);
            // 
            // pOSTMonitorRATERToolStripMenuItem
            // 
            this.pOSTMonitorRATERToolStripMenuItem.Name = "pOSTMonitorRATERToolStripMenuItem";
            this.pOSTMonitorRATERToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.pOSTMonitorRATERToolStripMenuItem.Text = "POST Monitor/RATER";
            this.pOSTMonitorRATERToolStripMenuItem.Click += new System.EventHandler(this.pOSTMonitorRATERToolStripMenuItem_Click);
            // 
            // cOMMonitorAdvancedToolStripMenuItem
            // 
            this.cOMMonitorAdvancedToolStripMenuItem.Name = "cOMMonitorAdvancedToolStripMenuItem";
            this.cOMMonitorAdvancedToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.cOMMonitorAdvancedToolStripMenuItem.Text = "COM Monitor";
            this.cOMMonitorAdvancedToolStripMenuItem.Click += new System.EventHandler(this.btnCOM_Click);
            // 
            // soundEditorToolStripMenuItem
            // 
            this.soundEditorToolStripMenuItem.Name = "soundEditorToolStripMenuItem";
            this.soundEditorToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.soundEditorToolStripMenuItem.Text = "ISD Sound Editor";
            this.soundEditorToolStripMenuItem.Click += new System.EventHandler(this.soundEditorToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(205, 6);
            // 
            // cBFuseToolStripMenuItem
            // 
            this.cBFuseToolStripMenuItem.Name = "cBFuseToolStripMenuItem";
            this.cBFuseToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.cBFuseToolStripMenuItem.Text = "CB Fuse Table";
            this.cBFuseToolStripMenuItem.Click += new System.EventHandler(this.cBFuseToolStripMenuItem_Click);
            // 
            // timingAssistantToolStripMenuItem
            // 
            this.timingAssistantToolStripMenuItem.Name = "timingAssistantToolStripMenuItem";
            this.timingAssistantToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.timingAssistantToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.timingAssistantToolStripMenuItem.Text = "Timing Assistant";
            this.timingAssistantToolStripMenuItem.Click += new System.EventHandler(this.timingAssistantToolStripMenuItem_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(205, 6);
            // 
            // mTXUSBFirmwareUtilityToolStripMenuItem
            // 
            this.mTXUSBFirmwareUtilityToolStripMenuItem.Name = "mTXUSBFirmwareUtilityToolStripMenuItem";
            this.mTXUSBFirmwareUtilityToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.mTXUSBFirmwareUtilityToolStripMenuItem.Text = "MTX USB Firmware Utility";
            this.mTXUSBFirmwareUtilityToolStripMenuItem.Click += new System.EventHandler(this.mTXUSBFirmwareUtilityToolStripMenuItem_Click);
            // 
            // xboxOneHDDToolToolStripMenuItem
            // 
            this.xboxOneHDDToolToolStripMenuItem.Name = "xboxOneHDDToolToolStripMenuItem";
            this.xboxOneHDDToolToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.xboxOneHDDToolToolStripMenuItem.Text = "Xbox One HDD Tool";
            this.xboxOneHDDToolToolStripMenuItem.Click += new System.EventHandler(this.xboxOneHDDToolToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(197, 6);
            // 
            // advancedToolStripMenuItem
            // 
            this.advancedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nandTimingFunctionsToolStripMenuItem,
            this.corona4GBToolStripMenuItem,
            this.writeFusionToolStripMenuItem,
            this.toolStripMenuItem1,
            this.convertToRGH3ToolStripMenuItem,
            this.checkSecdataToolStripMenuItem,
            this.CustomXeBuildMenuItem,
            this.toolStripMenuItem5,
            this.hexEditorToolStripMenuItem,
            this.kVViewerToolStripMenuItem,
            this.cPUKeyToolsToolStripMenuItem});
            this.advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
            this.advancedToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.advancedToolStripMenuItem.Text = "Advanced";
            // 
            // nandTimingFunctionsToolStripMenuItem
            // 
            this.nandTimingFunctionsToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.nandTimingFunctionsToolStripMenuItem.Name = "nandTimingFunctionsToolStripMenuItem";
            this.nandTimingFunctionsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.nandTimingFunctionsToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.nandTimingFunctionsToolStripMenuItem.Text = "Nand/Timing File Functions";
            this.nandTimingFunctionsToolStripMenuItem.Click += new System.EventHandler(this.nandTimingFunctionsMenuItem_Click);
            // 
            // corona4GBToolStripMenuItem
            // 
            this.corona4GBToolStripMenuItem.Name = "corona4GBToolStripMenuItem";
            this.corona4GBToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.corona4GBToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.corona4GBToolStripMenuItem.Text = "Corona 4GB Read/Write…";
            this.corona4GBToolStripMenuItem.Click += new System.EventHandler(this.corona4GBToolStripMenuItem_Click);
            // 
            // writeFusionToolStripMenuItem
            // 
            this.writeFusionToolStripMenuItem.Name = "writeFusionToolStripMenuItem";
            this.writeFusionToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.writeFusionToolStripMenuItem.Text = "Special/Fusion Write";
            this.writeFusionToolStripMenuItem.Click += new System.EventHandler(this.writeFusionToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(237, 6);
            // 
            // convertToRGH3ToolStripMenuItem
            // 
            this.convertToRGH3ToolStripMenuItem.Name = "convertToRGH3ToolStripMenuItem";
            this.convertToRGH3ToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.convertToRGH3ToolStripMenuItem.Text = "Convert to RGH3";
            this.convertToRGH3ToolStripMenuItem.Click += new System.EventHandler(this.convertToRGH3ToolStripMenuItem_Click);
            // 
            // checkSecdataToolStripMenuItem
            // 
            this.checkSecdataToolStripMenuItem.Name = "checkSecdataToolStripMenuItem";
            this.checkSecdataToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.checkSecdataToolStripMenuItem.Text = "Check SECDATA";
            this.checkSecdataToolStripMenuItem.Click += new System.EventHandler(this.checkSecdataToolStripMenuItem_Click);
            // 
            // CustomXeBuildMenuItem
            // 
            this.CustomXeBuildMenuItem.Name = "CustomXeBuildMenuItem";
            this.CustomXeBuildMenuItem.Size = new System.Drawing.Size(240, 22);
            this.CustomXeBuildMenuItem.Text = "XeBuild Command…";
            this.CustomXeBuildMenuItem.Click += new System.EventHandler(this.CustomXeBuildMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(237, 6);
            // 
            // hexEditorToolStripMenuItem
            // 
            this.hexEditorToolStripMenuItem.Name = "hexEditorToolStripMenuItem";
            this.hexEditorToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.hexEditorToolStripMenuItem.Text = "Hex Viewer";
            this.hexEditorToolStripMenuItem.Click += new System.EventHandler(this.hexEditorToolStripMenuItem_Click);
            // 
            // kVViewerToolStripMenuItem
            // 
            this.kVViewerToolStripMenuItem.Name = "kVViewerToolStripMenuItem";
            this.kVViewerToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.kVViewerToolStripMenuItem.Text = "KV Viewer";
            this.kVViewerToolStripMenuItem.Click += new System.EventHandler(this.kVViewerToolStripMenuItem_Click);
            // 
            // cPUKeyToolsToolStripMenuItem
            // 
            this.cPUKeyToolsToolStripMenuItem.Name = "cPUKeyToolsToolStripMenuItem";
            this.cPUKeyToolsToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.cPUKeyToolsToolStripMenuItem.Text = "CPU Key Tools";
            this.cPUKeyToolsToolStripMenuItem.Click += new System.EventHandler(this.cPUKeyToolsToolStripMenuItem_Click);
            // 
            // jRPToolStripMenuItem
            // 
            this.jRPToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.powerOnToolStripMenuItem,
            this.shutdownToolStripMenuItem,
            this.bootloaderModeToolStripMenuItem,
            this.toolStripMenuItem2,
            this.logPostToolStripMenuItem});
            this.jRPToolStripMenuItem.Name = "jRPToolStripMenuItem";
            this.jRPToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.jRPToolStripMenuItem.Text = "JR-P";
            this.jRPToolStripMenuItem.Visible = false;
            // 
            // powerOnToolStripMenuItem
            // 
            this.powerOnToolStripMenuItem.Name = "powerOnToolStripMenuItem";
            this.powerOnToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.powerOnToolStripMenuItem.Text = "Power On";
            this.powerOnToolStripMenuItem.Click += new System.EventHandler(this.powerOnToolStripMenuItem_Click);
            // 
            // shutdownToolStripMenuItem
            // 
            this.shutdownToolStripMenuItem.Name = "shutdownToolStripMenuItem";
            this.shutdownToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.shutdownToolStripMenuItem.Text = "Shut Down";
            this.shutdownToolStripMenuItem.Click += new System.EventHandler(this.shutdownToolStripMenuItem_Click);
            // 
            // bootloaderModeToolStripMenuItem
            // 
            this.bootloaderModeToolStripMenuItem.Name = "bootloaderModeToolStripMenuItem";
            this.bootloaderModeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.bootloaderModeToolStripMenuItem.Text = "Bootloader Mode";
            this.bootloaderModeToolStripMenuItem.Click += new System.EventHandler(this.bootloaderModeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(163, 6);
            // 
            // logPostToolStripMenuItem
            // 
            this.logPostToolStripMenuItem.Name = "logPostToolStripMenuItem";
            this.logPostToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.logPostToolStripMenuItem.Text = "Monitor POST";
            this.logPostToolStripMenuItem.Click += new System.EventHandler(this.logPostToolStripMenuItem_Click);
            // 
            // demoNToolStripMenuItem
            // 
            this.demoNToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.powerOnToolStripMenuItem1,
            this.powerOffToolStripMenuItem,
            this.toggleNANDToolStripMenuItem,
            this.toolStripMenuItem7,
            this.connectToUARTToolStripMenuItem,
            this.getInvalidBlocksToolStripMenuItem,
            this.toolStripMenuItem6,
            this.updateFwToolStripMenuItem});
            this.demoNToolStripMenuItem.Name = "demoNToolStripMenuItem";
            this.demoNToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.demoNToolStripMenuItem.Text = "DemoN";
            this.demoNToolStripMenuItem.Visible = false;
            // 
            // powerOnToolStripMenuItem1
            // 
            this.powerOnToolStripMenuItem1.Name = "powerOnToolStripMenuItem1";
            this.powerOnToolStripMenuItem1.Size = new System.Drawing.Size(167, 22);
            this.powerOnToolStripMenuItem1.Text = "Power On";
            this.powerOnToolStripMenuItem1.Click += new System.EventHandler(this.powerOnToolStripMenuItem1_Click);
            // 
            // powerOffToolStripMenuItem
            // 
            this.powerOffToolStripMenuItem.Name = "powerOffToolStripMenuItem";
            this.powerOffToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.powerOffToolStripMenuItem.Text = "Shut Down";
            this.powerOffToolStripMenuItem.Click += new System.EventHandler(this.powerOffToolStripMenuItem_Click);
            // 
            // toggleNANDToolStripMenuItem
            // 
            this.toggleNANDToolStripMenuItem.Name = "toggleNANDToolStripMenuItem";
            this.toggleNANDToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.toggleNANDToolStripMenuItem.Text = "Toggle NAND";
            this.toggleNANDToolStripMenuItem.Click += new System.EventHandler(this.toggleNANDToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(164, 6);
            // 
            // connectToUARTToolStripMenuItem
            // 
            this.connectToUARTToolStripMenuItem.Name = "connectToUARTToolStripMenuItem";
            this.connectToUARTToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.connectToUARTToolStripMenuItem.Text = "Connect To UART";
            this.connectToUARTToolStripMenuItem.Click += new System.EventHandler(this.connectToUARTToolStripMenuItem_Click);
            // 
            // getInvalidBlocksToolStripMenuItem
            // 
            this.getInvalidBlocksToolStripMenuItem.Name = "getInvalidBlocksToolStripMenuItem";
            this.getInvalidBlocksToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.getInvalidBlocksToolStripMenuItem.Text = "Get Invalid Blocks";
            this.getInvalidBlocksToolStripMenuItem.Click += new System.EventHandler(this.getInvalidBlocksToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(164, 6);
            // 
            // updateFwToolStripMenuItem
            // 
            this.updateFwToolStripMenuItem.Name = "updateFwToolStripMenuItem";
            this.updateFwToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.updateFwToolStripMenuItem.Text = "Update DemoN…";
            this.updateFwToolStripMenuItem.Click += new System.EventHandler(this.updateFwToolStripMenuItem_Click);
            // 
            // updateAvailableToolStripMenuItem
            // 
            this.updateAvailableToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.updateAvailableToolStripMenuItem.ForeColor = System.Drawing.Color.Blue;
            this.updateAvailableToolStripMenuItem.Image = global::JRunner.Properties.Resources.update;
            this.updateAvailableToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.updateAvailableToolStripMenuItem.Name = "updateAvailableToolStripMenuItem";
            this.updateAvailableToolStripMenuItem.Size = new System.Drawing.Size(120, 20);
            this.updateAvailableToolStripMenuItem.Text = "Update Available";
            this.updateAvailableToolStripMenuItem.ToolTipText = "Click to install updates!";
            this.updateAvailableToolStripMenuItem.Visible = false;
            this.updateAvailableToolStripMenuItem.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.nandToolStripMenuItem,
            this.advancedToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.xFlasherToolStripMenuItem,
            this.nANDXToolStripMenuItem,
            this.jRPToolStripMenuItem,
            this.jRPBLToolStripMenuItem,
            this.demoNToolStripMenuItem,
            this.keyDatabaseToolStripMenuItem,
            this.updateAvailableToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(832, 24);
            this.menuStrip1.TabIndex = 64;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.changelogToolStripMenuItem,
            this.shortcutsToolStripMenuItem,
            this.toolStripMenuItem10,
            this.reportIssueToolStripMenuItem,
            this.restoreFilesToolStripMenuItem,
            this.toolStripMenuItem4,
            this.newSessionToolStripMenuItem,
            this.restartToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Image = global::JRunner.Properties.Resources.menu;
            this.fileToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.fileToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // changelogToolStripMenuItem
            // 
            this.changelogToolStripMenuItem.Name = "changelogToolStripMenuItem";
            this.changelogToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.changelogToolStripMenuItem.Text = "Changelog";
            this.changelogToolStripMenuItem.Click += new System.EventHandler(this.changelogToolStripMenuItem_Click_1);
            // 
            // shortcutsToolStripMenuItem
            // 
            this.shortcutsToolStripMenuItem.Name = "shortcutsToolStripMenuItem";
            this.shortcutsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.shortcutsToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.shortcutsToolStripMenuItem.Text = "Key Shortcuts";
            this.shortcutsToolStripMenuItem.Click += new System.EventHandler(this.shortcutsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(186, 6);
            // 
            // reportIssueToolStripMenuItem
            // 
            this.reportIssueToolStripMenuItem.Name = "reportIssueToolStripMenuItem";
            this.reportIssueToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.reportIssueToolStripMenuItem.Text = "Report Issue…";
            this.reportIssueToolStripMenuItem.Click += new System.EventHandler(this.reportIssueToolStripMenuItem_Click);
            // 
            // restoreFilesToolStripMenuItem
            // 
            this.restoreFilesToolStripMenuItem.Name = "restoreFilesToolStripMenuItem";
            this.restoreFilesToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.restoreFilesToolStripMenuItem.Text = "Restore Files…";
            this.restoreFilesToolStripMenuItem.Click += new System.EventHandler(this.restoreFilesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(186, 6);
            // 
            // newSessionToolStripMenuItem
            // 
            this.newSessionToolStripMenuItem.Name = "newSessionToolStripMenuItem";
            this.newSessionToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.newSessionToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.newSessionToolStripMenuItem.Text = "New Session";
            this.newSessionToolStripMenuItem.Click += new System.EventHandler(this.btnNewSession_Click);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.restartToolStripMenuItem.Text = "Restart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // nandToolStripMenuItem
            // 
            this.nandToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractFilesToolStripMenuItem,
            this.createDonorToolStripMenuItem,
            this.decryptKeyvaultToolStripMenuItem,
            this.toolStripMenuItem11,
            this.loadGlitch2XeLLToolStripMenuItem,
            this.loadJTAGXeLLToolStripMenuItem,
            this.toolStripMenuItem9,
            this.sMCConfigViewerToolStripMenuItem1,
            this.patchKVToolStripMenuItem,
            this.changeLDVToolStripMenuItem});
            this.nandToolStripMenuItem.Name = "nandToolStripMenuItem";
            this.nandToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.nandToolStripMenuItem.Text = "Nand";
            // 
            // extractFilesToolStripMenuItem
            // 
            this.extractFilesToolStripMenuItem.Name = "extractFilesToolStripMenuItem";
            this.extractFilesToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.extractFilesToolStripMenuItem.Text = "Extract Files";
            this.extractFilesToolStripMenuItem.Click += new System.EventHandler(this.extractFilesToolStripMenuItem_Click);
            // 
            // createDonorToolStripMenuItem
            // 
            this.createDonorToolStripMenuItem.Name = "createDonorToolStripMenuItem";
            this.createDonorToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.createDonorToolStripMenuItem.Text = "Create Donor…";
            this.createDonorToolStripMenuItem.Click += new System.EventHandler(this.createDonorToolStripMenuItem_Click);
            // 
            // decryptKeyvaultToolStripMenuItem
            // 
            this.decryptKeyvaultToolStripMenuItem.Name = "decryptKeyvaultToolStripMenuItem";
            this.decryptKeyvaultToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.decryptKeyvaultToolStripMenuItem.Text = "Decrypt Keyvault...";
            this.decryptKeyvaultToolStripMenuItem.Click += new System.EventHandler(this.decryptKeyvaultToolStripMenuItem_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(210, 6);
            // 
            // loadGlitch2XeLLToolStripMenuItem
            // 
            this.loadGlitch2XeLLToolStripMenuItem.Name = "loadGlitch2XeLLToolStripMenuItem";
            this.loadGlitch2XeLLToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.loadGlitch2XeLLToolStripMenuItem.Text = "Load Glitch2 XeLL...";
            this.loadGlitch2XeLLToolStripMenuItem.Click += new System.EventHandler(this.loadGlitch2XeLLToolStripMenuItem_Click);
            // 
            // loadJTAGXeLLToolStripMenuItem
            // 
            this.loadJTAGXeLLToolStripMenuItem.Name = "loadJTAGXeLLToolStripMenuItem";
            this.loadJTAGXeLLToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.loadJTAGXeLLToolStripMenuItem.Text = "Load JTAG XeLL...";
            this.loadJTAGXeLLToolStripMenuItem.Click += new System.EventHandler(this.loadJTAGXeLLToolStripMenuItem_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(210, 6);
            // 
            // sMCConfigViewerToolStripMenuItem1
            // 
            this.sMCConfigViewerToolStripMenuItem1.Name = "sMCConfigViewerToolStripMenuItem1";
            this.sMCConfigViewerToolStripMenuItem1.Size = new System.Drawing.Size(213, 22);
            this.sMCConfigViewerToolStripMenuItem1.Text = "SMC Config Editor";
            this.sMCConfigViewerToolStripMenuItem1.Click += new System.EventHandler(this.sMCConfigViewerToolStripMenuItem1_Click);
            // 
            // patchKVToolStripMenuItem
            // 
            this.patchKVToolStripMenuItem.Name = "patchKVToolStripMenuItem";
            this.patchKVToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.patchKVToolStripMenuItem.Text = "Patch Keyvault…";
            this.patchKVToolStripMenuItem.Click += new System.EventHandler(this.patchKVToolStripMenuItem_Click);
            // 
            // changeLDVToolStripMenuItem
            // 
            this.changeLDVToolStripMenuItem.Name = "changeLDVToolStripMenuItem";
            this.changeLDVToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.changeLDVToolStripMenuItem.Text = "Change Lockdown Value…";
            this.changeLDVToolStripMenuItem.Click += new System.EventHandler(this.changeLDVToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.settingsToolStripMenuItem.Image = global::JRunner.Properties.Resources.settings;
            this.settingsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // xFlasherToolStripMenuItem
            // 
            this.xFlasherToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.installDriversToolStripMenuItem,
            this.checkConsoleCBToolStripMenuItem,
            this.flashOpenXeniumToolStripMenuItem});
            this.xFlasherToolStripMenuItem.Name = "xFlasherToolStripMenuItem";
            this.xFlasherToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.xFlasherToolStripMenuItem.Text = "xFlasher";
            this.xFlasherToolStripMenuItem.Visible = false;
            // 
            // installDriversToolStripMenuItem
            // 
            this.installDriversToolStripMenuItem.Name = "installDriversToolStripMenuItem";
            this.installDriversToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.installDriversToolStripMenuItem.Text = "Install Drivers…";
            this.installDriversToolStripMenuItem.Click += new System.EventHandler(this.installDriversToolStripMenuItem_Click);
            // 
            // checkConsoleCBToolStripMenuItem
            // 
            this.checkConsoleCBToolStripMenuItem.Name = "checkConsoleCBToolStripMenuItem";
            this.checkConsoleCBToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.checkConsoleCBToolStripMenuItem.Text = "Check Console CB";
            this.checkConsoleCBToolStripMenuItem.Click += new System.EventHandler(this.checkConsoleCBToolStripMenuItem_Click);
            // 
            // flashOpenXeniumToolStripMenuItem
            // 
            this.flashOpenXeniumToolStripMenuItem.Name = "flashOpenXeniumToolStripMenuItem";
            this.flashOpenXeniumToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.flashOpenXeniumToolStripMenuItem.Text = "Program OpenXenium…";
            this.flashOpenXeniumToolStripMenuItem.Click += new System.EventHandler(this.flashOpenXeniumToolStripMenuItem_Click);
            // 
            // nANDXToolStripMenuItem
            // 
            this.nANDXToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mtxUsbModeToolStripMenuItem});
            this.nANDXToolStripMenuItem.Name = "nANDXToolStripMenuItem";
            this.nANDXToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.nANDXToolStripMenuItem.Text = "NAND-X";
            this.nANDXToolStripMenuItem.Visible = false;
            // 
            // mtxUsbModeToolStripMenuItem
            // 
            this.mtxUsbModeToolStripMenuItem.Name = "mtxUsbModeToolStripMenuItem";
            this.mtxUsbModeToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.mtxUsbModeToolStripMenuItem.Text = "MTX USB Mode";
            this.mtxUsbModeToolStripMenuItem.Click += new System.EventHandler(this.mtxUsbModeToolStripMenuItem_Click);
            // 
            // jRPBLToolStripMenuItem
            // 
            this.jRPBLToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateJRPToolStripMenuItem});
            this.jRPBLToolStripMenuItem.Name = "jRPBLToolStripMenuItem";
            this.jRPBLToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.jRPBLToolStripMenuItem.Text = "JR-P";
            this.jRPBLToolStripMenuItem.Visible = false;
            // 
            // updateJRPToolStripMenuItem
            // 
            this.updateJRPToolStripMenuItem.Name = "updateJRPToolStripMenuItem";
            this.updateJRPToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.updateJRPToolStripMenuItem.Text = "Update JR-P Firmware…";
            this.updateJRPToolStripMenuItem.Click += new System.EventHandler(this.updateJRPToolStripMenuItem_Click);
            // 
            // keyDatabaseToolStripMenuItem
            // 
            this.keyDatabaseToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.keyDatabaseToolStripMenuItem.Image = global::JRunner.Properties.Resources.key;
            this.keyDatabaseToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.keyDatabaseToolStripMenuItem.Name = "keyDatabaseToolStripMenuItem";
            this.keyDatabaseToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.keyDatabaseToolStripMenuItem.Size = new System.Drawing.Size(105, 20);
            this.keyDatabaseToolStripMenuItem.Text = "Key Database";
            this.keyDatabaseToolStripMenuItem.Click += new System.EventHandler(this.keyDatabaseToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(832, 600);
            this.Controls.Add(this.btnScanDevices);
            this.Controls.Add(this.btnScanner);
            this.Controls.Add(this.btnNewSession);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.btnShowWorkingFolder);
            this.Controls.Add(this.pnlExtra);
            this.Controls.Add(this.pnlTools);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.txtBlocks);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "J-Runner with Extras";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.getCpuKeyMenu.ResumeLayout(false);
            this.showWorkingFolderMenu.ResumeLayout(false);
            this.backupContextMenu.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private Button btnExit;
        private Button btnCompare;
        private Button btnLoadExtra;
        private Button btnLoadSource;
        private TextBox txtFileExtra;
        private TextBox txtFileSource;
        private TextBox txtCPUKey;
        private Label lblCpuKey;
        private TextBox txtConsole;
        private ProgressBar progressBar;
        private ToolTip toolTip1;
        private GroupBox groupBox8;
        private Label labelIP;
        private TextBox txtIP;
        private GroupBox groupBox4;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel VersionLabel;
        private ToolStripStatusLabel StatusBlank;
        private Button btnReload;
        private ToolStripMenuItem XeBuildOptionsToolStripMenuItem;
        private ToolStripStatusLabel ModeVersion;
        private ToolStripStatusLabel FWStatus;
        private ToolStripStatusLabel FWVersion;
        private ToolStripStatusLabel FlashStatus;
        private ToolStripStatusLabel FlashVersion;
        private Button btnScanner;
        private Button btnCheckBadBlocks;
        private FolderBrowserDialog folderBrowserDialog1;
        private Panel pnlInfo;
        private TextBox txtBlocks;
        private Label ProgressLabel;
        private Panel pnlTools;
        private Panel pnlExtra;
        private ToolStripStatusLabel ModeStatus;
        private Button btnRestart;
        private ToolStripStatusLabel BlankSpace1;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripMenuItem advancedToolStripMenuItem;
        private ToolStripMenuItem nandTimingFunctionsToolStripMenuItem;
        private ToolStripMenuItem CustomXeBuildMenuItem;
        private ToolStripMenuItem writeFusionToolStripMenuItem;
        private ToolStripMenuItem jRPToolStripMenuItem;
        private ToolStripMenuItem powerOnToolStripMenuItem;
        private ToolStripMenuItem shutdownToolStripMenuItem;
        private ToolStripMenuItem bootloaderModeToolStripMenuItem;
        private ToolStripMenuItem demoNToolStripMenuItem;
        private ToolStripMenuItem toggleNANDToolStripMenuItem;
        private ToolStripMenuItem powerOnToolStripMenuItem1;
        private ToolStripMenuItem powerOffToolStripMenuItem;
        private ToolStripMenuItem updateFwToolStripMenuItem;
        private ToolStripMenuItem getInvalidBlocksToolStripMenuItem;
        private ToolStripMenuItem connectToUARTToolStripMenuItem;
        private ToolStripMenuItem updateAvailableToolStripMenuItem;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem xFlasherToolStripMenuItem;
        private ToolStripMenuItem cPUKeyToolsToolStripMenuItem;
        private ToolStripMenuItem cBFuseToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem logPostToolStripMenuItem;
        private ToolStripMenuItem pOSTMonitorRATERToolStripMenuItem;
        private ToolStripMenuItem soundEditorToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem restartToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem cOMMonitorAdvancedToolStripMenuItem;
        private Button btnNewSession;
        private ToolStripMenuItem newSessionToolStripMenuItem;
        private ToolStripMenuItem shortcutsToolStripMenuItem;
        private ToolStripMenuItem changelogToolStripMenuItem;
        private ToolStripMenuItem checkSecdataToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripMenuItem timingAssistantToolStripMenuItem;
        private ToolStripMenuItem installDriversToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem8;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripMenuItem hexEditorToolStripMenuItem;
        private ToolStripMenuItem nandToolStripMenuItem;
        private ToolStripMenuItem extractFilesToolStripMenuItem;
        private ToolStripMenuItem createDonorToolStripMenuItem;
        private ToolStripMenuItem decryptKeyvaultToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem9;
        private ToolStripMenuItem sMCConfigViewerToolStripMenuItem1;
        private ToolStripMenuItem patchKVToolStripMenuItem;
        private ToolStripMenuItem changeLDVToolStripMenuItem;
        private ToolStripMenuItem mTXUSBFirmwareUtilityToolStripMenuItem;
        private ToolStripMenuItem corona4GBToolStripMenuItem;
        private ToolStripMenuItem jRPBLToolStripMenuItem;
        private ToolStripMenuItem updateJRPToolStripMenuItem;
        private ToolStripMenuItem nANDXToolStripMenuItem;
        private ToolStripMenuItem mtxUsbModeToolStripMenuItem;
        private ToolStripMenuItem reportIssueToolStripMenuItem;
        private UI.MenuButton btnIPGetCPU;
        private ContextMenuStrip getCpuKeyMenu;
        private ToolStripMenuItem getAndSaveToWorkingFolderToolStripMenuItem;
        private ToolStripMenuItem saveToDesktopToolStripMenuItem;
        private ContextMenuStrip showWorkingFolderMenu;
        private ToolStripMenuItem showRootFolderToolStripMenuItem;
        private ToolStripMenuItem showOutputFolderToolStripMenuItem;
        private UI.MenuButton btnShowWorkingFolder;
        private ToolStripMenuItem xboxOneHDDToolToolStripMenuItem;
        private ToolStripMenuItem checkConsoleCBToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem convertToRGH3ToolStripMenuItem;
        private UI.MenuButton btnBackup;
        private ContextMenuStrip backupContextMenu;
        private ToolStripMenuItem backupToZIPToolStripMenuItem;
        private ToolStripMenuItem configureBackupToolStripMenuItem;
        private ToolStripStatusLabel BackupLabel;
        private ToolStripMenuItem autoBackupNowToolStripMenuItem;
        private ToolStripMenuItem showLastBackupToolStripMenuItem;
        private ToolStripMenuItem kVViewerToolStripMenuItem;
        private ToolStripMenuItem showDataFolderToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem11;
        private ToolStripMenuItem loadGlitch2XeLLToolStripMenuItem;
        private ToolStripMenuItem loadJTAGXeLLToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem10;
        private ToolStripMenuItem restoreFilesToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem keyDatabaseToolStripMenuItem;
        private Button btnScanDevices;
        private ToolStripMenuItem rescanDevicesToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem12;
        private ToolStripMenuItem flashOpenXeniumToolStripMenuItem;
        private ToolStripStatusLabel BlankSpace2;
        private ToolStripStatusLabel CopiedToClipboard;
    }
}
