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
            this.comparebutton = new System.Windows.Forms.Button();
            this.btnLoadFile2 = new System.Windows.Forms.Button();
            this.btnLoadFile1 = new System.Windows.Forms.Button();
            this.txtFilePath2 = new System.Windows.Forms.TextBox();
            this.txtFilePath1 = new System.Windows.Forms.TextBox();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.txtCPUKey = new System.Windows.Forms.TextBox();
            this.lblCpuKey = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnIPGetCPU = new UI.SplitButton();
            this.getCpuKeyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.getAndSaveToWorkingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToDesktopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelIP = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnScanner = new System.Windows.Forms.Button();
            this.btnInit = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnNewSession = new System.Windows.Forms.Button();
            this.btnShowWorkingFolder = new UI.SplitButton();
            this.showWorkingFolderMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showRootFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showOutputFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRestart = new System.Windows.Forms.Button();
            this.XeBuildOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusBlank = new System.Windows.Forms.ToolStripStatusLabel();
            this.XeBuildLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.XeBuildVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.BlankSpace = new System.Windows.Forms.ToolStripStatusLabel();
            this.DashlaunchLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.DashlaunchVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.ModeStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ModeVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.FWStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.FWVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.FlashStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.FlashVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.btnCheckBadBlocks = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.txtBlocks = new System.Windows.Forms.TextBox();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.pnlTools = new System.Windows.Forms.Panel();
            this.pnlExtra = new System.Windows.Forms.Panel();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pOSTMonitorRATERToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOMMonitorAdvancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sonus360EditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.rescanDevicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mTXUSBFirmwareUtilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xboxOneHDDToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.timingAssistantToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cBFuseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customNandProCommandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.corona4GBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CustomXeBuildMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeFusionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripHexEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.dEVGLCPUKeyToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.checkSecdataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.reportIssueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shortcutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.newSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createDonorNandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decryptKeyvaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.sMCConfigViewerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.patchNandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeLDVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xFlasherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installDriversToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flashOpenXeniumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nANDXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mtxUsbModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jRPBLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox8.SuspendLayout();
            this.getCpuKeyContextMenu.SuspendLayout();
            this.showWorkingFolderMenu.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnExit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExit.Location = new System.Drawing.Point(568, 547);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 26);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.toolTip1.SetToolTip(this.btnExit, "Exit the application");
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // comparebutton
            // 
            this.comparebutton.Location = new System.Drawing.Point(387, 14);
            this.comparebutton.Name = "comparebutton";
            this.comparebutton.Size = new System.Drawing.Size(66, 51);
            this.comparebutton.TabIndex = 19;
            this.comparebutton.TabStop = false;
            this.comparebutton.Text = "Nand Compare";
            this.toolTip1.SetToolTip(this.comparebutton, "Compares the source and extra nands");
            this.comparebutton.UseVisualStyleBackColor = true;
            this.comparebutton.Click += new System.EventHandler(this.comparebutton_Click);
            // 
            // btnLoadFile2
            // 
            this.btnLoadFile2.Location = new System.Drawing.Point(6, 43);
            this.btnLoadFile2.Name = "btnLoadFile2";
            this.btnLoadFile2.Size = new System.Drawing.Size(76, 22);
            this.btnLoadFile2.TabIndex = 16;
            this.btnLoadFile2.TabStop = false;
            this.btnLoadFile2.Text = "Load Extra";
            this.toolTip1.SetToolTip(this.btnLoadFile2, "Load an extra nand for comparison");
            this.btnLoadFile2.UseVisualStyleBackColor = true;
            this.btnLoadFile2.Click += new System.EventHandler(this.btnLoadFile2_Click);
            // 
            // btnLoadFile1
            // 
            this.btnLoadFile1.Location = new System.Drawing.Point(6, 14);
            this.btnLoadFile1.Name = "btnLoadFile1";
            this.btnLoadFile1.Size = new System.Drawing.Size(76, 22);
            this.btnLoadFile1.TabIndex = 15;
            this.btnLoadFile1.TabStop = false;
            this.btnLoadFile1.Text = "Load Source";
            this.toolTip1.SetToolTip(this.btnLoadFile1, "Load the primary working nand");
            this.btnLoadFile1.UseVisualStyleBackColor = true;
            this.btnLoadFile1.Click += new System.EventHandler(this.btnLoadFile1_Click);
            // 
            // txtFilePath2
            // 
            this.txtFilePath2.AllowDrop = true;
            this.txtFilePath2.BackColor = System.Drawing.Color.White;
            this.txtFilePath2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilePath2.Location = new System.Drawing.Point(88, 44);
            this.txtFilePath2.Name = "txtFilePath2";
            this.txtFilePath2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtFilePath2.Size = new System.Drawing.Size(293, 20);
            this.txtFilePath2.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtFilePath2, "The file listed in this box is used to compare against file loaded in \"Source fil" +
        "e\" box\r\n");
            this.txtFilePath2.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFilePath2_DragDrop);
            this.txtFilePath2.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtFilePath2_DragEnter);
            // 
            // txtFilePath1
            // 
            this.txtFilePath1.AllowDrop = true;
            this.txtFilePath1.BackColor = System.Drawing.SystemColors.Window;
            this.txtFilePath1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilePath1.Location = new System.Drawing.Point(88, 15);
            this.txtFilePath1.Name = "txtFilePath1";
            this.txtFilePath1.Size = new System.Drawing.Size(293, 20);
            this.txtFilePath1.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtFilePath1, "The file in this box is used for all read/write/create operations.\r\nif you use th" +
        "e functions above out of order, ensure your required\r\nfile is loaded into this b" +
        "ox first.");
            this.txtFilePath1.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFilePath1_DragDrop);
            this.txtFilePath1.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtFilePath1_DragEnter);
            // 
            // txtConsole
            // 
            this.txtConsole.BackColor = System.Drawing.Color.Black;
            this.txtConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtConsole.ForeColor = System.Drawing.Color.White;
            this.txtConsole.Location = new System.Drawing.Point(12, 321);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ReadOnly = true;
            this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConsole.Size = new System.Drawing.Size(462, 253);
            this.txtConsole.TabIndex = 11;
            this.txtConsole.TabStop = false;
            this.txtConsole.DoubleClick += new System.EventHandler(this.txtConsole_DoubleClick);
            // 
            // txtCPUKey
            // 
            this.txtCPUKey.AllowDrop = true;
            this.txtCPUKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCPUKey.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCPUKey.Location = new System.Drawing.Point(88, 73);
            this.txtCPUKey.MaxLength = 32;
            this.txtCPUKey.Name = "txtCPUKey";
            this.txtCPUKey.Size = new System.Drawing.Size(293, 20);
            this.txtCPUKey.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtCPUKey, "This is where your CPU key should be entered. You can drag and drop previously sa" +
        "ved cpukey.txt or paste in your CPU Key details.");
            this.txtCPUKey.TextChanged += new System.EventHandler(this.txtCPUKey_TextChanged);
            this.txtCPUKey.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtCPUKey_DragDrop);
            this.txtCPUKey.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtCPUKey_DragEnter);
            // 
            // lblCpuKey
            // 
            this.lblCpuKey.AutoSize = true;
            this.lblCpuKey.BackColor = System.Drawing.Color.Transparent;
            this.lblCpuKey.ForeColor = System.Drawing.Color.Black;
            this.lblCpuKey.Location = new System.Drawing.Point(18, 75);
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
            this.groupBox8.Location = new System.Drawing.Point(656, 480);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(165, 66);
            this.groupBox8.TabIndex = 73;
            this.groupBox8.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox8, "If you connect your Xbox 360 to your PC using a network cable, \r\nOnce booted with" +
        " Xellous or Xell-Reloaded place the displayed IP address in\r\nthe box and retriev" +
        "e your CPU Key by pressing the button.");
            // 
            // btnIPGetCPU
            // 
            this.btnIPGetCPU.AutoSize = true;
            this.btnIPGetCPU.BtnImage = ((System.Drawing.Image)(resources.GetObject("btnIPGetCPU.BtnImage")));
            this.btnIPGetCPU.ContextMenuStrip = this.getCpuKeyContextMenu;
            this.btnIPGetCPU.DropDownContextMenu = this.getCpuKeyContextMenu;
            this.btnIPGetCPU.Image = ((System.Drawing.Image)(resources.GetObject("btnIPGetCPU.Image")));
            this.btnIPGetCPU.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnIPGetCPU.Location = new System.Drawing.Point(6, 36);
            this.btnIPGetCPU.Name = "btnIPGetCPU";
            this.btnIPGetCPU.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnIPGetCPU.Size = new System.Drawing.Size(153, 26);
            this.btnIPGetCPU.TabIndex = 6;
            this.btnIPGetCPU.Text = "Get CPU Key";
            this.toolTip1.SetToolTip(this.btnIPGetCPU, "Tries to retrieve the CPU Key and Fuses from XeLL using the IP above");
            this.btnIPGetCPU.UseVisualStyleBackColor = true;
            this.btnIPGetCPU.Click += new System.EventHandler(this.btnIPGetCPU_Click);
            // 
            // getCpuKeyContextMenu
            // 
            this.getCpuKeyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getAndSaveToWorkingFolderToolStripMenuItem,
            this.saveToDesktopToolStripMenuItem});
            this.getCpuKeyContextMenu.Name = "contextMenuStrip1";
            this.getCpuKeyContextMenu.Size = new System.Drawing.Size(200, 48);
            // 
            // getAndSaveToWorkingFolderToolStripMenuItem
            // 
            this.getAndSaveToWorkingFolderToolStripMenuItem.Name = "getAndSaveToWorkingFolderToolStripMenuItem";
            this.getAndSaveToWorkingFolderToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.getAndSaveToWorkingFolderToolStripMenuItem.Text = "Save To Working Folder";
            this.getAndSaveToWorkingFolderToolStripMenuItem.Click += new System.EventHandler(this.getAndSaveToWorkingFolderToolStripMenuItem_Click);
            // 
            // saveToDesktopToolStripMenuItem
            // 
            this.saveToDesktopToolStripMenuItem.Name = "saveToDesktopToolStripMenuItem";
            this.saveToDesktopToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.saveToDesktopToolStripMenuItem.Text = "Save To Desktop";
            this.saveToDesktopToolStripMenuItem.Click += new System.EventHandler(this.saveToDesktopToolStripMenuItem_Click);
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(8, 14);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(20, 13);
            this.labelIP.TabIndex = 5;
            this.labelIP.Text = "IP:";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(34, 11);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(124, 20);
            this.txtIP.TabIndex = 5;
            this.txtIP.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtIP_KeyUp);
            // 
            // btnScanner
            // 
            this.btnScanner.Location = new System.Drawing.Point(662, 547);
            this.btnScanner.Name = "btnScanner";
            this.btnScanner.Size = new System.Drawing.Size(153, 26);
            this.btnScanner.TabIndex = 6;
            this.btnScanner.TabStop = false;
            this.btnScanner.Text = "Scan IP";
            this.toolTip1.SetToolTip(this.btnScanner, "Scans the IP range looking for XeLL to retrieve the CPU Key and Fuses");
            this.btnScanner.UseVisualStyleBackColor = true;
            this.btnScanner.Click += new System.EventHandler(this.btnScanner_Click);
            // 
            // btnInit
            // 
            this.btnInit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInit.Location = new System.Drawing.Point(387, 72);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(66, 22);
            this.btnInit.TabIndex = 61;
            this.btnInit.TabStop = false;
            this.btnInit.Text = "Reload";
            this.toolTip1.SetToolTip(this.btnInit, "Reloads and initializes the nand in source box");
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(567, 516);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(83, 26);
            this.btnSettings.TabIndex = 80;
            this.btnSettings.TabStop = false;
            this.btnSettings.Text = "Settings";
            this.toolTip1.SetToolTip(this.btnSettings, "Edit settings and behavior");
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnNewSession
            // 
            this.btnNewSession.Location = new System.Drawing.Point(480, 516);
            this.btnNewSession.Name = "btnNewSession";
            this.btnNewSession.Size = new System.Drawing.Size(83, 26);
            this.btnNewSession.TabIndex = 92;
            this.btnNewSession.TabStop = false;
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
            this.btnShowWorkingFolder.Location = new System.Drawing.Point(480, 485);
            this.btnShowWorkingFolder.Name = "btnShowWorkingFolder";
            this.btnShowWorkingFolder.Size = new System.Drawing.Size(170, 26);
            this.btnShowWorkingFolder.TabIndex = 90;
            this.btnShowWorkingFolder.Text = "Show Working Folder";
            this.toolTip1.SetToolTip(this.btnShowWorkingFolder, "Shows the working folder in Windows Explorer");
            this.btnShowWorkingFolder.UseVisualStyleBackColor = true;
            this.btnShowWorkingFolder.Click += new System.EventHandler(this.btnShowWorkingFolder_Click);
            // 
            // showWorkingFolderMenu
            // 
            this.showWorkingFolderMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showRootFolderToolStripMenuItem,
            this.showOutputFolderToolStripMenuItem});
            this.showWorkingFolderMenu.Name = "contextMenuStrip1";
            this.showWorkingFolderMenu.Size = new System.Drawing.Size(181, 48);
            // 
            // showRootFolderToolStripMenuItem
            // 
            this.showRootFolderToolStripMenuItem.Name = "showRootFolderToolStripMenuItem";
            this.showRootFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showRootFolderToolStripMenuItem.Text = "Show Root Folder";
            this.showRootFolderToolStripMenuItem.Click += new System.EventHandler(this.showRootFolderToolStripMenuItem_Click);
            // 
            // showOutputFolderToolStripMenuItem
            // 
            this.showOutputFolderToolStripMenuItem.Name = "showOutputFolderToolStripMenuItem";
            this.showOutputFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showOutputFolderToolStripMenuItem.Text = "Show Output Folder";
            this.showOutputFolderToolStripMenuItem.Click += new System.EventHandler(this.showOutputFolderToolStripMenuItem_Click);
            // 
            // btnRestart
            // 
            this.btnRestart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnRestart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRestart.Location = new System.Drawing.Point(480, 547);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(83, 26);
            this.btnRestart.TabIndex = 91;
            this.btnRestart.Text = "Restart";
            this.toolTip1.SetToolTip(this.btnRestart, "Completely restarts the application");
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
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
            this.groupBox4.Controls.Add(this.btnLoadFile2);
            this.groupBox4.Controls.Add(this.btnInit);
            this.groupBox4.Controls.Add(this.txtFilePath2);
            this.groupBox4.Controls.Add(this.txtFilePath1);
            this.groupBox4.Controls.Add(this.btnLoadFile1);
            this.groupBox4.Controls.Add(this.lblCpuKey);
            this.groupBox4.Controls.Add(this.comparebutton);
            this.groupBox4.Controls.Add(this.txtCPUKey);
            this.groupBox4.Location = new System.Drawing.Point(12, 192);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(460, 101);
            this.groupBox4.TabIndex = 67;
            this.groupBox4.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusBlank,
            this.XeBuildLabel,
            this.XeBuildVersion,
            this.BlankSpace,
            this.DashlaunchLabel,
            this.DashlaunchVersion,
            this.ModeStatus,
            this.ModeVersion,
            this.FWStatus,
            this.FWVersion,
            this.FlashStatus,
            this.FlashVersion});
            this.statusStrip1.Location = new System.Drawing.Point(0, 581);
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
            // XeBuildLabel
            // 
            this.XeBuildLabel.Name = "XeBuildLabel";
            this.XeBuildLabel.Size = new System.Drawing.Size(50, 17);
            this.XeBuildLabel.Text = "XeBuild:";
            // 
            // XeBuildVersion
            // 
            this.XeBuildVersion.Name = "XeBuildVersion";
            this.XeBuildVersion.Size = new System.Drawing.Size(0, 17);
            // 
            // BlankSpace
            // 
            this.BlankSpace.Name = "BlankSpace";
            this.BlankSpace.Size = new System.Drawing.Size(10, 17);
            this.BlankSpace.Text = " ";
            // 
            // DashlaunchLabel
            // 
            this.DashlaunchLabel.Name = "DashlaunchLabel";
            this.DashlaunchLabel.Size = new System.Drawing.Size(72, 17);
            this.DashlaunchLabel.Text = "Dashlaunch:";
            // 
            // DashlaunchVersion
            // 
            this.DashlaunchVersion.Name = "DashlaunchVersion";
            this.DashlaunchVersion.Size = new System.Drawing.Size(0, 17);
            // 
            // ModeStatus
            // 
            this.ModeStatus.AutoSize = false;
            this.ModeStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.ModeStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.ModeStatus.Name = "ModeStatus";
            this.ModeStatus.Size = new System.Drawing.Size(449, 17);
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
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Enabled = false;
            this.splitter1.Location = new System.Drawing.Point(0, 578);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(832, 3);
            this.splitter1.TabIndex = 78;
            this.splitter1.TabStop = false;
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
            this.pnlInfo.Size = new System.Drawing.Size(342, 294);
            this.pnlInfo.TabIndex = 79;
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
            this.ProgressLabel.Location = new System.Drawing.Point(10, 300);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(48, 13);
            this.ProgressLabel.TabIndex = 83;
            this.ProgressLabel.Text = "Progress";
            // 
            // pnlTools
            // 
            this.pnlTools.Location = new System.Drawing.Point(9, 24);
            this.pnlTools.Name = "pnlTools";
            this.pnlTools.Size = new System.Drawing.Size(463, 174);
            this.pnlTools.TabIndex = 84;
            // 
            // pnlExtra
            // 
            this.pnlExtra.Location = new System.Drawing.Point(479, 25);
            this.pnlExtra.Name = "pnlExtra";
            this.pnlExtra.Size = new System.Drawing.Size(342, 156);
            this.pnlExtra.TabIndex = 85;
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pOSTMonitorRATERToolStripMenuItem,
            this.cOMMonitorAdvancedToolStripMenuItem,
            this.sonus360EditorToolStripMenuItem,
            this.toolStripMenuItem3,
            this.rescanDevicesToolStripMenuItem,
            this.mTXUSBFirmwareUtilityToolStripMenuItem,
            this.xboxOneHDDToolToolStripMenuItem,
            this.toolStripMenuItem8,
            this.timingAssistantToolStripMenuItem,
            this.cBFuseToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // pOSTMonitorRATERToolStripMenuItem
            // 
            this.pOSTMonitorRATERToolStripMenuItem.Name = "pOSTMonitorRATERToolStripMenuItem";
            this.pOSTMonitorRATERToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.pOSTMonitorRATERToolStripMenuItem.Text = "POST Monitor/RATER";
            this.pOSTMonitorRATERToolStripMenuItem.Click += new System.EventHandler(this.pOSTMonitorRATERToolStripMenuItem_Click);
            // 
            // cOMMonitorAdvancedToolStripMenuItem
            // 
            this.cOMMonitorAdvancedToolStripMenuItem.Name = "cOMMonitorAdvancedToolStripMenuItem";
            this.cOMMonitorAdvancedToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.cOMMonitorAdvancedToolStripMenuItem.Text = "COM Monitor";
            this.cOMMonitorAdvancedToolStripMenuItem.Click += new System.EventHandler(this.btnCOM_Click);
            // 
            // sonus360EditorToolStripMenuItem
            // 
            this.sonus360EditorToolStripMenuItem.Name = "sonus360EditorToolStripMenuItem";
            this.sonus360EditorToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.sonus360EditorToolStripMenuItem.Text = "Sonus360";
            this.sonus360EditorToolStripMenuItem.Click += new System.EventHandler(this.sonus360EditorToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(206, 6);
            // 
            // rescanDevicesToolStripMenuItem
            // 
            this.rescanDevicesToolStripMenuItem.Name = "rescanDevicesToolStripMenuItem";
            this.rescanDevicesToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.rescanDevicesToolStripMenuItem.Text = "Re-Scan Devices";
            this.rescanDevicesToolStripMenuItem.Click += new System.EventHandler(this.rescanDevicesToolStripMenuItem_Click);
            // 
            // mTXUSBFirmwareUtilityToolStripMenuItem
            // 
            this.mTXUSBFirmwareUtilityToolStripMenuItem.Name = "mTXUSBFirmwareUtilityToolStripMenuItem";
            this.mTXUSBFirmwareUtilityToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.mTXUSBFirmwareUtilityToolStripMenuItem.Text = "MTX USB Firmware Utility";
            this.mTXUSBFirmwareUtilityToolStripMenuItem.Click += new System.EventHandler(this.mTXUSBFirmwareUtilityToolStripMenuItem_Click);
            // 
            // xboxOneHDDToolToolStripMenuItem
            // 
            this.xboxOneHDDToolToolStripMenuItem.Name = "xboxOneHDDToolToolStripMenuItem";
            this.xboxOneHDDToolToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.xboxOneHDDToolToolStripMenuItem.Text = "Xbox One HDD Tool";
            this.xboxOneHDDToolToolStripMenuItem.Click += new System.EventHandler(this.xboxOneHDDToolToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(206, 6);
            // 
            // timingAssistantToolStripMenuItem
            // 
            this.timingAssistantToolStripMenuItem.Name = "timingAssistantToolStripMenuItem";
            this.timingAssistantToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.timingAssistantToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.timingAssistantToolStripMenuItem.Text = "Timing Assistant";
            this.timingAssistantToolStripMenuItem.Click += new System.EventHandler(this.timingAssistantToolStripMenuItem_Click);
            // 
            // cBFuseToolStripMenuItem
            // 
            this.cBFuseToolStripMenuItem.Name = "cBFuseToolStripMenuItem";
            this.cBFuseToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.cBFuseToolStripMenuItem.Text = "CB Fuse Table";
            this.cBFuseToolStripMenuItem.Click += new System.EventHandler(this.cBFuseToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(197, 6);
            // 
            // advancedToolStripMenuItem
            // 
            this.advancedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customNandProCommandToolStripMenuItem,
            this.corona4GBToolStripMenuItem,
            this.CustomXeBuildMenuItem,
            this.writeFusionToolStripMenuItem,
            this.toolStripMenuItem5,
            this.toolStripHexEditor,
            this.dEVGLCPUKeyToolsToolStripMenuItem,
            this.toolStripSeparator15,
            this.checkSecdataToolStripMenuItem,
            this.xValueToolStripMenuItem});
            this.advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
            this.advancedToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.advancedToolStripMenuItem.Text = "Advanced";
            // 
            // customNandProCommandToolStripMenuItem
            // 
            this.customNandProCommandToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.customNandProCommandToolStripMenuItem.Name = "customNandProCommandToolStripMenuItem";
            this.customNandProCommandToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.customNandProCommandToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.customNandProCommandToolStripMenuItem.Text = "Nand/Timing File Functions";
            this.customNandProCommandToolStripMenuItem.Click += new System.EventHandler(this.customNandProCommandToolStripMenuItem_Click);
            // 
            // corona4GBToolStripMenuItem
            // 
            this.corona4GBToolStripMenuItem.Name = "corona4GBToolStripMenuItem";
            this.corona4GBToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.corona4GBToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.corona4GBToolStripMenuItem.Text = "Corona 4GB Read/Write…";
            this.corona4GBToolStripMenuItem.Click += new System.EventHandler(this.corona4GBToolStripMenuItem_Click);
            // 
            // CustomXeBuildMenuItem
            // 
            this.CustomXeBuildMenuItem.Name = "CustomXeBuildMenuItem";
            this.CustomXeBuildMenuItem.Size = new System.Drawing.Size(241, 22);
            this.CustomXeBuildMenuItem.Text = "XeBuild Command…";
            this.CustomXeBuildMenuItem.Click += new System.EventHandler(this.CustomXeBuildMenuItem_Click);
            // 
            // writeFusionToolStripMenuItem
            // 
            this.writeFusionToolStripMenuItem.Name = "writeFusionToolStripMenuItem";
            this.writeFusionToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.writeFusionToolStripMenuItem.Text = "Special/Fusion Write";
            this.writeFusionToolStripMenuItem.Click += new System.EventHandler(this.writeFusionToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(238, 6);
            // 
            // toolStripHexEditor
            // 
            this.toolStripHexEditor.Name = "toolStripHexEditor";
            this.toolStripHexEditor.Size = new System.Drawing.Size(241, 22);
            this.toolStripHexEditor.Text = "Hex Viewer";
            this.toolStripHexEditor.Click += new System.EventHandler(this.toolStripHexEditor_Click);
            // 
            // dEVGLCPUKeyToolsToolStripMenuItem
            // 
            this.dEVGLCPUKeyToolsToolStripMenuItem.Name = "dEVGLCPUKeyToolsToolStripMenuItem";
            this.dEVGLCPUKeyToolsToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.dEVGLCPUKeyToolsToolStripMenuItem.Text = "DEVGL CPU Key Tools";
            this.dEVGLCPUKeyToolsToolStripMenuItem.Visible = false;
            this.dEVGLCPUKeyToolsToolStripMenuItem.Click += new System.EventHandler(this.dEVGLCPUKeyToolsToolStripMenuItem_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(238, 6);
            // 
            // checkSecdataToolStripMenuItem
            // 
            this.checkSecdataToolStripMenuItem.Name = "checkSecdataToolStripMenuItem";
            this.checkSecdataToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.checkSecdataToolStripMenuItem.Text = "Check SECDATA";
            this.checkSecdataToolStripMenuItem.Click += new System.EventHandler(this.checkSecdataToolStripMenuItem_Click);
            // 
            // xValueToolStripMenuItem
            // 
            this.xValueToolStripMenuItem.Name = "xValueToolStripMenuItem";
            this.xValueToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.xValueToolStripMenuItem.Text = "Decrypt X Value…";
            this.xValueToolStripMenuItem.Click += new System.EventHandler(this.xValueToolStripMenuItem_Click);
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
            this.powerOnToolStripMenuItem1.Size = new System.Drawing.Size(169, 22);
            this.powerOnToolStripMenuItem1.Text = "Power On";
            this.powerOnToolStripMenuItem1.Click += new System.EventHandler(this.powerOnToolStripMenuItem1_Click);
            // 
            // powerOffToolStripMenuItem
            // 
            this.powerOffToolStripMenuItem.Name = "powerOffToolStripMenuItem";
            this.powerOffToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.powerOffToolStripMenuItem.Text = "Shut Down";
            this.powerOffToolStripMenuItem.Click += new System.EventHandler(this.powerOffToolStripMenuItem_Click);
            // 
            // toggleNANDToolStripMenuItem
            // 
            this.toggleNANDToolStripMenuItem.Name = "toggleNANDToolStripMenuItem";
            this.toggleNANDToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.toggleNANDToolStripMenuItem.Text = "Toggle NAND";
            this.toggleNANDToolStripMenuItem.Click += new System.EventHandler(this.toggleNANDToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(166, 6);
            // 
            // connectToUARTToolStripMenuItem
            // 
            this.connectToUARTToolStripMenuItem.Name = "connectToUARTToolStripMenuItem";
            this.connectToUARTToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.connectToUARTToolStripMenuItem.Text = "Connect To UART";
            this.connectToUARTToolStripMenuItem.Click += new System.EventHandler(this.connectToUARTToolStripMenuItem_Click);
            // 
            // getInvalidBlocksToolStripMenuItem
            // 
            this.getInvalidBlocksToolStripMenuItem.Name = "getInvalidBlocksToolStripMenuItem";
            this.getInvalidBlocksToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.getInvalidBlocksToolStripMenuItem.Text = "Get Invalid Blocks";
            this.getInvalidBlocksToolStripMenuItem.Click += new System.EventHandler(this.getInvalidBlocksToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(166, 6);
            // 
            // updateFwToolStripMenuItem
            // 
            this.updateFwToolStripMenuItem.Name = "updateFwToolStripMenuItem";
            this.updateFwToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.updateFwToolStripMenuItem.Text = "Update DemoN…";
            this.updateFwToolStripMenuItem.Click += new System.EventHandler(this.updateFwToolStripMenuItem_Click);
            // 
            // updateAvailableToolStripMenuItem
            // 
            this.updateAvailableToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.updateAvailableToolStripMenuItem.ForeColor = System.Drawing.Color.Blue;
            this.updateAvailableToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("updateAvailableToolStripMenuItem.Image")));
            this.updateAvailableToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.updateAvailableToolStripMenuItem.Name = "updateAvailableToolStripMenuItem";
            this.updateAvailableToolStripMenuItem.Size = new System.Drawing.Size(159, 20);
            this.updateAvailableToolStripMenuItem.Text = "Update Ready to Install!";
            this.updateAvailableToolStripMenuItem.ToolTipText = "Click to install updates!";
            this.updateAvailableToolStripMenuItem.Visible = false;
            this.updateAvailableToolStripMenuItem.Click += new System.EventHandler(this.updateAvailableToolStripMenuItem_Click);
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
            this.xFlasherToolStripMenuItem,
            this.nANDXToolStripMenuItem,
            this.jRPToolStripMenuItem,
            this.jRPBLToolStripMenuItem,
            this.demoNToolStripMenuItem,
            this.versionToolStripMenuItem,
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
            this.reportIssueToolStripMenuItem,
            this.shortcutsToolStripMenuItem,
            this.toolStripMenuItem4,
            this.newSessionToolStripMenuItem,
            this.restartToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fileToolStripMenuItem.Image")));
            this.fileToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.fileToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // changelogToolStripMenuItem
            // 
            this.changelogToolStripMenuItem.Name = "changelogToolStripMenuItem";
            this.changelogToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.changelogToolStripMenuItem.Text = "Changelog";
            this.changelogToolStripMenuItem.Click += new System.EventHandler(this.changelogToolStripMenuItem_Click_1);
            // 
            // reportIssueToolStripMenuItem
            // 
            this.reportIssueToolStripMenuItem.Name = "reportIssueToolStripMenuItem";
            this.reportIssueToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.reportIssueToolStripMenuItem.Text = "Report Issue…";
            this.reportIssueToolStripMenuItem.Click += new System.EventHandler(this.reportIssueToolStripMenuItem_Click);
            // 
            // shortcutsToolStripMenuItem
            // 
            this.shortcutsToolStripMenuItem.Name = "shortcutsToolStripMenuItem";
            this.shortcutsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.shortcutsToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.shortcutsToolStripMenuItem.Text = "Shortcuts";
            this.shortcutsToolStripMenuItem.Click += new System.EventHandler(this.shortcutsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(164, 6);
            // 
            // newSessionToolStripMenuItem
            // 
            this.newSessionToolStripMenuItem.Name = "newSessionToolStripMenuItem";
            this.newSessionToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.newSessionToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.newSessionToolStripMenuItem.Text = "New Session";
            this.newSessionToolStripMenuItem.Click += new System.EventHandler(this.btnNewSession_Click);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.restartToolStripMenuItem.Text = "Restart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // nandToolStripMenuItem
            // 
            this.nandToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractFilesToolStripMenuItem,
            this.createDonorNandToolStripMenuItem,
            this.decryptKeyvaultToolStripMenuItem,
            this.toolStripMenuItem9,
            this.sMCConfigViewerToolStripMenuItem1,
            this.patchNandToolStripMenuItem,
            this.changeLDVToolStripMenuItem});
            this.nandToolStripMenuItem.Name = "nandToolStripMenuItem";
            this.nandToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.nandToolStripMenuItem.Text = "Nand";
            // 
            // extractFilesToolStripMenuItem
            // 
            this.extractFilesToolStripMenuItem.Name = "extractFilesToolStripMenuItem";
            this.extractFilesToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.extractFilesToolStripMenuItem.Text = "Extract Nand Files";
            this.extractFilesToolStripMenuItem.Click += new System.EventHandler(this.extractFilesToolStripMenuItem_Click);
            // 
            // createDonorNandToolStripMenuItem
            // 
            this.createDonorNandToolStripMenuItem.Name = "createDonorNandToolStripMenuItem";
            this.createDonorNandToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.createDonorNandToolStripMenuItem.Text = "Create Donor Nand…";
            this.createDonorNandToolStripMenuItem.Click += new System.EventHandler(this.createDonorNandToolStripMenuItem_Click);
            // 
            // decryptKeyvaultToolStripMenuItem
            // 
            this.decryptKeyvaultToolStripMenuItem.Name = "decryptKeyvaultToolStripMenuItem";
            this.decryptKeyvaultToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.decryptKeyvaultToolStripMenuItem.Text = "Decrypt Keyvault...";
            this.decryptKeyvaultToolStripMenuItem.Click += new System.EventHandler(this.decryptKeyvaultToolStripMenuItem_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(215, 6);
            // 
            // sMCConfigViewerToolStripMenuItem1
            // 
            this.sMCConfigViewerToolStripMenuItem1.Name = "sMCConfigViewerToolStripMenuItem1";
            this.sMCConfigViewerToolStripMenuItem1.Size = new System.Drawing.Size(218, 22);
            this.sMCConfigViewerToolStripMenuItem1.Text = "SMC Config Editor";
            this.sMCConfigViewerToolStripMenuItem1.Click += new System.EventHandler(this.sMCConfigViewerToolStripMenuItem1_Click);
            // 
            // patchNandToolStripMenuItem
            // 
            this.patchNandToolStripMenuItem.Name = "patchNandToolStripMenuItem";
            this.patchNandToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.patchNandToolStripMenuItem.Text = "Patch KV/Nand…";
            this.patchNandToolStripMenuItem.Click += new System.EventHandler(this.patchNandToolStripMenuItem_Click);
            // 
            // changeLDVToolStripMenuItem
            // 
            this.changeLDVToolStripMenuItem.Name = "changeLDVToolStripMenuItem";
            this.changeLDVToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.changeLDVToolStripMenuItem.Text = "Change Lock Down Value…";
            this.changeLDVToolStripMenuItem.Click += new System.EventHandler(this.changeLDVToolStripMenuItem_Click);
            // 
            // xFlasherToolStripMenuItem
            // 
            this.xFlasherToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.installDriversToolStripMenuItem,
            this.flashOpenXeniumToolStripMenuItem});
            this.xFlasherToolStripMenuItem.Name = "xFlasherToolStripMenuItem";
            this.xFlasherToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.xFlasherToolStripMenuItem.Text = "xFlasher";
            this.xFlasherToolStripMenuItem.Visible = false;
            // 
            // installDriversToolStripMenuItem
            // 
            this.installDriversToolStripMenuItem.Name = "installDriversToolStripMenuItem";
            this.installDriversToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.installDriversToolStripMenuItem.Text = "Install Drivers…";
            this.installDriversToolStripMenuItem.Click += new System.EventHandler(this.installDriversToolStripMenuItem_Click);
            // 
            // flashOpenXeniumToolStripMenuItem
            // 
            this.flashOpenXeniumToolStripMenuItem.Name = "flashOpenXeniumToolStripMenuItem";
            this.flashOpenXeniumToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.flashOpenXeniumToolStripMenuItem.Text = "Program OpenXenium";
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
            this.mtxUsbModeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.mtxUsbModeToolStripMenuItem.Text = "MTX USB Mode";
            this.mtxUsbModeToolStripMenuItem.Click += new System.EventHandler(this.mtxUsbModeToolStripMenuItem_Click);
            // 
            // jRPBLToolStripMenuItem
            // 
            this.jRPBLToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateToolStripMenuItem});
            this.jRPBLToolStripMenuItem.Name = "jRPBLToolStripMenuItem";
            this.jRPBLToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.jRPBLToolStripMenuItem.Text = "JR-P";
            this.jRPBLToolStripMenuItem.Visible = false;
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.updateToolStripMenuItem.Text = "Update JR-P Firmware…";
            this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            // 
            // versionToolStripMenuItem
            // 
            this.versionToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.versionToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.versionToolStripMenuItem.Name = "versionToolStripMenuItem";
            this.versionToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.versionToolStripMenuItem.Text = "Version";
            this.versionToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(832, 603);
            this.Controls.Add(this.btnScanner);
            this.Controls.Add(this.btnNewSession);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.btnShowWorkingFolder);
            this.Controls.Add(this.pnlExtra);
            this.Controls.Add(this.pnlTools);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.txtBlocks);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(848, 639);
            this.MinimumSize = new System.Drawing.Size(848, 639);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "J-Runner with Extras";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.getCpuKeyContextMenu.ResumeLayout(false);
            this.showWorkingFolderMenu.ResumeLayout(false);
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
        private Button comparebutton;
        private Button btnLoadFile2;
        private Button btnLoadFile1;
        private TextBox txtFilePath2;
        private TextBox txtFilePath1;
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
        private ToolStripStatusLabel XeBuildVersion;
        private ToolStripStatusLabel DashlaunchVersion;
        private ToolStripStatusLabel StatusBlank;
        private Button btnInit;
        private ToolStripMenuItem XeBuildOptionsToolStripMenuItem;
        private ToolStripStatusLabel ModeVersion;
        private ToolStripStatusLabel FWStatus;
        private ToolStripStatusLabel FWVersion;
        private ToolStripStatusLabel FlashStatus;
        private ToolStripStatusLabel FlashVersion;
        private Splitter splitter1;
        private Button btnScanner;
        private Button btnCheckBadBlocks;
        private FolderBrowserDialog folderBrowserDialog1;
        private Panel pnlInfo;
        private TextBox txtBlocks;
        private Button btnSettings;
        private Label ProgressLabel;
        private Panel pnlTools;
        private Panel pnlExtra;
        private ToolStripStatusLabel ModeStatus;
        private Button btnRestart;
        private ToolStripStatusLabel XeBuildLabel;
        private ToolStripStatusLabel DashlaunchLabel;
        private ToolStripStatusLabel BlankSpace;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripMenuItem advancedToolStripMenuItem;
        private ToolStripMenuItem customNandProCommandToolStripMenuItem;
        private ToolStripMenuItem CustomXeBuildMenuItem;
        private ToolStripMenuItem writeFusionToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator15;
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
        private ToolStripMenuItem dEVGLCPUKeyToolsToolStripMenuItem;
        private ToolStripMenuItem cBFuseToolStripMenuItem;
        private ToolStripMenuItem flashOpenXeniumToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem logPostToolStripMenuItem;
        private ToolStripMenuItem pOSTMonitorRATERToolStripMenuItem;
        private ToolStripMenuItem sonus360EditorToolStripMenuItem;
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
        private ToolStripMenuItem versionToolStripMenuItem;
        private ToolStripMenuItem checkSecdataToolStripMenuItem;
        private ToolStripMenuItem xValueToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripMenuItem timingAssistantToolStripMenuItem;
        private ToolStripMenuItem installDriversToolStripMenuItem;
        private ToolStripMenuItem rescanDevicesToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem8;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripMenuItem toolStripHexEditor;
        private ToolStripMenuItem nandToolStripMenuItem;
        private ToolStripMenuItem extractFilesToolStripMenuItem;
        private ToolStripMenuItem createDonorNandToolStripMenuItem;
        private ToolStripMenuItem decryptKeyvaultToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem9;
        private ToolStripMenuItem sMCConfigViewerToolStripMenuItem1;
        private ToolStripMenuItem patchNandToolStripMenuItem;
        private ToolStripMenuItem changeLDVToolStripMenuItem;
        private ToolStripMenuItem mTXUSBFirmwareUtilityToolStripMenuItem;
        private ToolStripMenuItem corona4GBToolStripMenuItem;
        private ToolStripMenuItem jRPBLToolStripMenuItem;
        private ToolStripMenuItem updateToolStripMenuItem;
        private ToolStripMenuItem nANDXToolStripMenuItem;
        private ToolStripMenuItem mtxUsbModeToolStripMenuItem;
        private ToolStripMenuItem reportIssueToolStripMenuItem;
        private UI.SplitButton btnIPGetCPU;
        private ContextMenuStrip getCpuKeyContextMenu;
        private ToolStripMenuItem getAndSaveToWorkingFolderToolStripMenuItem;
        private ToolStripMenuItem saveToDesktopToolStripMenuItem;
        private ContextMenuStrip showWorkingFolderMenu;
        private ToolStripMenuItem showRootFolderToolStripMenuItem;
        private ToolStripMenuItem showOutputFolderToolStripMenuItem;
        private UI.SplitButton btnShowWorkingFolder;
        private ToolStripMenuItem xboxOneHDDToolToolStripMenuItem;
    }
}
