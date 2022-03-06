using DiscordRPC;
using JRunner.Forms;
using LibUsbDotNet.DeviceNotify;
using Microsoft.Win32;
using RenameRegistryKey;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using WinUsb;
using MessageBox = System.Windows.Forms.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

// Copyright (c) 2022 J-Runner with Extras Development Team

namespace JRunner
{
    public partial class MainForm : Form
    {
        #region Variables
        public enum DEVICE
        {
            JR_PROGRAMMER_BOOTLOADER = -1,
            NO_DEVICE = 0,
            JR_PROGRAMMER = 1,
            NAND_X = 2,
            XFLASHER_SPI = 3,
            XFLASHER_EMMC = 4,
            PICOFLASHER = 5,
        }
		
        public static TextWriter _writer = null;
        public static MainForm mainForm;
        private IDeviceNotifier devNotifier;
        public DEVICE device = DEVICE.NO_DEVICE;
        IP myIP = new IP();
        public static Nand.PrivateN nand = new Nand.PrivateN();
        public PicoFlasher picoflasher = new PicoFlasher();
        public xFlasher xflasher = new xFlasher();
        public Mtx_Usb mtx_usb = new Mtx_Usb();
        public xdkbuild XDKbuild = new xdkbuild();
        public rgh3build rgh3Build = new rgh3build();
        private NandX nandx = new NandX();
        private DemoN demon = new DemoN();
        private Panels.NandInfo nandInfo = new Panels.NandInfo();
        private Panels.NandTools nTools = new Panels.NandTools();
        public Panels.XeBuildPanel xPanel = new Panels.XeBuildPanel();
        private Panels.LDrivesInfo ldInfo = new Panels.LDrivesInfo();
        public Panels.XSVFChoice xsvfInfo = new Panels.XSVFChoice();
        List<Control> listInfo = new List<Control>();
        List<Control> listTools = new List<Control>();
        List<Control> listExtra = new List<Control>();
        public static EventWaitHandle _waitmb = new AutoResetEvent(true);
        public static readonly object _object = new object();
        public static AutoResetEvent _event1 = new AutoResetEvent(false);
        public static Nand.VNand vnand;
        public static bool usingVNand = false;
        Regex objAlphaPattern = new Regex("[a-fA-F0-9]{32}$");
        public NotifyIcon trayIcon = null;
        // RPC
        private static DiscordRpcClient rpcClient;
        private bool rpcReady = false;
        private string rpcDevice = "No Device";
        private string rpcStatus = "No Device";
        #endregion

        #region Initialization

        public MainForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            pnlInfo.Controls.Add(nandInfo);
            listInfo.Add(nandInfo);
            pnlTools.Controls.Add(nTools);
            listTools.Add(nTools);
            pnlExtra.Controls.Add(xPanel);
            listExtra.Add(xPanel);
            xflasher.initTimerSetup();
            xflasher.inUseTimerSetup();
            mtx_usb.inUseTimerSetup();
            setUp();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            mainForm = this;
            versionToolStripMenuItem.Text = "V" + variables.version;

            // Make sure we're on top
            bool top = TopMost;
            TopMost = true; // Bring to front
            TopMost = top; // Set it back
            Activate();

            _writer = new TextBoxStreamWriter(txtConsole);
            Console.SetOut(_writer);

            //// BH
            if (variables.location != new Point(0, 0))
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = variables.location;
                if (!Screen.FromControl(this).Bounds.Contains(this.Location))
                {
                    this.DesktopLocation = new Point(100, 100);
                }
            }

            // BH
            if (InvokeRequired)
            {
                this.Invoke(new EventHandler(MainForm_Load), new object[] { sender, e });
                return;
            }

            ContextMenuStrip trayContext = new ContextMenuStrip();
            trayContext.Items.Add("Restore", null, restoreTrayClick);
            trayContext.Items.Add("Exit", null, btnExit_Click);

            trayIcon = new NotifyIcon();
            trayIcon.ContextMenuStrip = trayContext;
            trayIcon.MouseClick += new MouseEventHandler(activateWindow);
            trayIcon.Icon = Properties.Resources.Project3;
            trayIcon.Text = "J-Runner with Extras";
            trayIcon.Visible = false;

            settings();

            printstartuptext(true);

            new Thread(on_load).Start();

            deviceinit();

            if (variables.discordrpc)
            {
                rpcInit();
                new Thread(new ThreadStart(this.rpcCheck)).Start();
            }
            
            try
            {
                if (File.Exists(xflasher.svfPath)) File.Delete(xflasher.svfPath);
            }
            catch { }
        }

        private void showApplication()
        {
            ShowInTaskbar = true;
            trayIcon.Visible = false;
            WindowState = FormWindowState.Normal;

            bool top = TopMost;
            TopMost = true; // Bring to front
            TopMost = top; // Set it back

            Activate();
        }

        private void restoreTrayClick(object sender, EventArgs e)
        {
            showApplication();
        }

        private void activateWindow(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                showApplication();
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && variables.minimizetotray)
            {
                ShowInTaskbar = false;
                trayIcon.Visible = true;
            }
        }

        void setUp()
        {
            demon.UpdateBloc += updateBlocks;
            demon.UpdateProgres += updateProgress;
            demon.updateFlas += demon_updateFlas;
            demon.updateMod += demon_updateMod;
            demon.UpdateVer += demon_UpdateVer;
            nTools.ReadClick += btnReadClick;
            nTools.CreateEccClick += btnCreateECCClick;
            nTools.WriteEccClick += btnWriteECCClick;
            nTools.WriteClick += btnWriteClick;
            nTools.CPUDBClick += btnCPUDBClick;
            nTools.ProgramCRClick += btnProgramCRClick;
            nTools.XeBuildClick += btnXeBuildClick;
            nTools.IterChange += nTools_IterChange;
            xsvfInfo.CloseCRClick += xsvfInfo_CloseCRClick;
            xsvfInfo.ProgramCRClick += xsvfInfo_ProgramCRClick;
            xPanel.DeletedDash += xPanel_DeletedDash;
            xPanel.AddedDash += xPanel_AddedDash;
            xPanel.HackChanged += xPanel_HackChanged;
            xPanel.CallMB += xPanel_CallMB;
            xPanel.loadFil += xPanel_loadFil;
            xPanel.updateSourc += xPanel_updateSourc;
            xPanel.UpdateProgres += updateProgress;

            xPanel.Getmb += xPanel_getmb;
            nandInfo.DragDropChanged += nandInfo_DragDropChanged;
            nandx.UpdateProgres += updateProgress;
            nandx.UpdateBloc += updateBlocks;

            ldInfo.UpdateProgres += updateProgress;
            ldInfo.UpdateBloc += updateBlocks;
            ldInfo.UpdateSourc += xPanel_updateSourc;
            ldInfo.CloseLDClick += ldInfo_CloseLDClick;
            ldInfo.doCompar += ldInfo_doCompar;
            ldInfo.UpdateAdditional += ldInfo_UpdateAdditional;
        }

        public delegate void UpdatedDevice();
        public event UpdatedDevice updateDevice;

        public bool IsUsbDeviceConnected(string pid, string vid)
        {
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBControllerDevice"))
            {
                using (var collection = searcher.Get())
                {
                    foreach (var device in collection)
                    {
                        var usbDevice = Convert.ToString(device);

                        if (usbDevice.Contains(pid) && usbDevice.Contains(vid))
                            return true;
                    }
                }
            }
            return false;
        }

        private void deviceinit()
        {
            devNotifier = DeviceNotifier.OpenDeviceNotifier();
            devNotifier.OnDeviceNotify += onDevNotify;

            showDemon(DemoN.FindDemon());

            if (!DemoN.DemonDetected)
            {
                if (IsUsbDeviceConnected("7001", "600D")) // PicoFlasher
                {
                    nTools.setImage(Properties.Resources.picoflasher);
                    //PicoFlasherToolStripMenuItem.Visible = true;
                    device = DEVICE.PICOFLASHER;
                }
                else if (IsUsbDeviceConnected("6010", "0403")) // xFlasher SPI
                {
                    nTools.setImage(Properties.Resources.xflash_spi);
                    xFlasherToolStripMenuItem.Visible = true;
                    device = DEVICE.XFLASHER_SPI;
                    xflasher.ready = true; // Skip init
                }
                else if (IsUsbDeviceConnected("8334", "11D4")) // JR-Programmer Bootloader
                {
                    nTools.setImage(Properties.Resources.usb);
                    jRPBLToolStripMenuItem.Visible = true;
                    device = DEVICE.JR_PROGRAMMER_BOOTLOADER;
                }
                else
                {
                    LibUsbDotNet.Main.UsbRegDeviceList mDevList = LibUsbDotNet.UsbDevice.AllDevices;
                    foreach (LibUsbDotNet.Main.UsbRegistry devic in mDevList)
                    {
                        if (devic.Pid == 0x0004 && devic.Vid == 0xFFFF) // NAND-X
                        {
                            if (variables.mtxUsbMode)
                            {
                                nTools.setImage(Properties.Resources.mtx);
                            }
                            else
                            {
                                nTools.setImage(Properties.Resources.NANDX);
                            }
                            nANDXToolStripMenuItem.Visible = true;
                            device = DEVICE.NAND_X;
                        }
                        else if (devic.Pid == 0x8338 && devic.Vid == 0x11D4) // JR-Programmer
                        {
                            nTools.setImage(Properties.Resources.JRP);
                            jRPToolStripMenuItem.Visible = true;
                            device = DEVICE.JR_PROGRAMMER;
                        }
                    }
                }

                if (device == DEVICE.NO_DEVICE) // Must check this after everything else
                {
                    if (IsUsbDeviceConnected("AAAA", "8816") || IsUsbDeviceConnected("05E3", "0751")) // xFlasher eMMC
                    {
                        nTools.setImage(Properties.Resources.xflash_emmc);
                        xFlasherToolStripMenuItem.Visible = true;
                        device = DEVICE.XFLASHER_EMMC;
                    }
                }
            }

            try // It'll fail if the thing doesn't exist
            {
                if (updateDevice != null)
                    updateDevice();
            }
            catch
            {
                // Do nothing
            }
        }

        private void on_load()
        {
            new Thread(createdirectories).Start();

            check_dash(); // configures the dashes dropdown

            try
            {
                if (xPanel.getComboDash().Items.Count == 4)
                {
                    xPanel.getComboDash().SelectedIndex = 0;
                    variables.dashversion = Convert.ToInt32(xPanel.getComboDash().Text);
                }
                else
                {
                    if (variables.dashes_all.Contains(variables.preferredDash))
                    {
                        xPanel.getComboDash().SelectedIndex = variables.dashes_all.IndexOf(variables.preferredDash);
                        variables.dashversion = Convert.ToInt32(xPanel.getComboDash().Text);
                    }
                    else if (xPanel.getComboDash().Items.Count > 3) xPanel.BeginInvoke((Action)(() => xPanel.getComboDash().SelectedIndex = xPanel.getComboDash().Items.Count - 3));
                }
                xPanel.checkAvailableHackTypes();
            }
            catch
            {
            }

        }
        private void createdirectories()
        {
            if (!Directory.Exists(Path.Combine(variables.pathforit, "output")))
            {
                try
                {
                    Directory.CreateDirectory(Path.Combine(variables.pathforit, "output"));
                }
                catch (DirectoryNotFoundException)
                {
                    variables.pathforit = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    Directory.CreateDirectory(Path.Combine(variables.pathforit, "output"));
                }
            }
            if (Directory.GetFiles(variables.outfolder, "*", SearchOption.TopDirectoryOnly).Length > 0)
            {
                Console.WriteLine("WARNING - Working Folder!");
                Console.WriteLine("Your working folder is not empty, click Show Working Folder to view its contents");
                Console.WriteLine("");
            }
            if (!Directory.Exists(variables.AppData))
            {
                Directory.CreateDirectory(variables.AppData);
            }
        }

        private void printstartuptext(bool firsttime = false)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("J-Runner with Extras");
            Console.WriteLine("Session: {0:F}", DateTime.Now.ToString("MM/dd/yyyy H:mm:ss"));
            if (variables.version.Contains("Beta")) Console.WriteLine("Version: {0}", variables.build);
            else Console.WriteLine("Version: {0}", variables.version);
            if (Upd.checkSuccess)
            {
                if (Upd.upToDate == true)
                {
                    updateAvailableToolStripMenuItem.Visible = false;
                    Console.WriteLine("Status: Up to date");
                }
                else
                {
                    updateAvailableToolStripMenuItem.Visible = true;
                    Console.WriteLine("Status: An update is ready to be installed");
                }
            }
            else
            {
                Console.WriteLine("Status: Could not check for updates");
            }

            Console.WriteLine("");

            if (firsttime)
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
            }

            if (Environment.OSVersion.Version.Major <= 5)
            {
                xflasher.svfPath = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "TimingSvfTemp.svf"); // Required for Windows XP, will not work in Windows Vista and later
                xflasher.svfRoot = Path.GetPathRoot(Environment.SystemDirectory);                                    // Even though it doesn't work on XP, lets keep this just in case we figure out how to make it work

                Console.WriteLine("Support for Windows XP is limited, some features may not function correctly or at all");
                Console.WriteLine("");
            }

            if (!firsttime)
            {
                if (Directory.GetFiles(variables.outfolder, "*", SearchOption.TopDirectoryOnly).Length > 0)
                {
                    Console.WriteLine("WARNING - Working Folder!");
                    Console.WriteLine("Your working folder is not empty, click Show Working Folder to view its contents");
                    Console.WriteLine("");
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            savesettings();
            saveToLog();
        }

        private void saveToLog()
        {
            string file = Path.Combine(variables.pathforit, "Log.txt");
            File.AppendAllText(file, "\n" + txtConsole.Text);
        }

        #region DiscordRPC

        private void rpcInit()
        {
            rpcClient = new DiscordRpcClient("768884149578104883");
            rpcClient.Initialize();
            rpcClient.SetPresence(new RichPresence
            {
                Details = string.Concat(new object[]
                {
                    "Device: ",
                    "Detecting..."
                }),
                State = string.Concat(new object[]
                {
                    "Status: ",
                    "Detecting..."
                }),
                Assets = new Assets
                {
                    LargeImageKey = "j-runner",
                    LargeImageText = "J-Runner with Extras",
                    SmallImageKey = ""
                }
            });
            rpcReady = true;
        }

        private void rpcCheck()
        {
            while (rpcReady)
            {
                rpcUpdate();
                Thread.Sleep(5000);
            }
        }

        private void rpcUpdate()
        {
            if (DemoN.DemonDetected)
            {
                rpcDevice = "DemoN";
            }
            else if (device == DEVICE.NAND_X)
            {
                if (variables.mtxUsbMode) rpcDevice = "MTX USB";
                else rpcDevice = "NAND-X";
            }
            else if (device == DEVICE.JR_PROGRAMMER)
            {
                rpcDevice = "JR-Programmer";
            }
            else if (device == DEVICE.XFLASHER_SPI)
            {
                rpcDevice = "xFlasher SPI";
            }
            else if (device == DEVICE.XFLASHER_EMMC)
            {
                rpcDevice = "xFlasher eMMC";
            }
            else
            {
                rpcDevice = "No Device";
            }

            if (device != 0)
            {
                if (variables.writing)
                {
                    rpcStatus = "Writing NAND";
                }
                else if (variables.reading)
                {
                    rpcStatus = "Reading NAND";
                }
                else
                {
                    rpcStatus = "Idle";
                }
            }
            else
            {
                rpcStatus = "No Device";
            }

            rpcClient.SetPresence(new RichPresence
            {
                Details = string.Concat(new object[]
                {
                    "Device: ",
                    rpcDevice
                }),
                State = string.Concat(new object[]
                {
                    "Status: ",
                    rpcStatus
                }),
                Assets = new Assets
                {
                    LargeImageKey = "j-runner",
                    LargeImageText = "J-Runner with Extras",
                    SmallImageKey = ""
                }
            });
        }

        #endregion

        #endregion

        #region Panels

        void demon_UpdateVer(string version)
        {
            FWVersion.Text = version;
        }

        void demon_updateMod(DemoN.Demon_Modes mode)
        {
            if (mode == DemoN.Demon_Modes.FIRMWARE)
            {
                FlashStatus.Visible = true;
                FlashVersion.Visible = true;
                FWStatus.Text = "FW: "; // fuck :)
            }
            else
            {
                demon.getBootloaderVersion();
                FWStatus.Text = "BL: ";
            }
            ModeVersion.Text = mode.ToString();
        }

        void demon_updateFlas(DemoN.Demon_Switch flash)
        {
            FlashVersion.Text = flash.ToString();
        }

        #region LDrivesPanel

        void ldInfo_CloseLDClick()
        {
            listInfo.Remove(ldInfo);
            pnlInfo.Controls.Remove(ldInfo);
            pnlInfo.Controls.Add(listInfo[listInfo.Count - 1]);
        }

        void ldInfo_UpdateAdditional(string file)
        {
            txtFilePath2.Text = file;
            variables.filename2 = file;
        }

        void ldInfo_doCompar()
        {
            comparenands();
        }

        #endregion

        #region xebuild Panel

        void xPanel_CallMB()
        {
            variables.ctyp = callconsoletypes(ConsoleTypes.Selected.All);
        }

        void xPanel_HackChanged()
        {
            nTools.setbtnCreateECC("Create ECC");
            nTools.setbtnWriteECC("Write\nECC");

            if (xPanel.getRbtnGlitchChecked()) variables.ttyp = variables.hacktypes.glitch;
            else if (xPanel.getRbtnJtagChecked())
            {
                variables.ttyp = variables.hacktypes.jtag;
                nTools.setbtnCreateECC("Create XeLL");
                nTools.setbtnWriteECC("Write\nXeLL");
            }
            else if (xPanel.getRbtnGlitch2Checked()) variables.ttyp = variables.hacktypes.glitch2;
            else if (xPanel.getRbtnGlitch2mChecked()) variables.ttyp = variables.hacktypes.glitch2m;
            else if (xPanel.getRbtnRetailChecked()) variables.ttyp = variables.hacktypes.retail;
            else if (xPanel.getRbtnDevGLChecked()) variables.ttyp = variables.hacktypes.devgl;
            else variables.ttyp = variables.hacktypes.nothing;
        }

        void xPanel_AddedDash()
        {
            ThreadStart starte = delegate { check_dashes(true); };
            Thread th = new Thread(starte);
            th.IsBackground = true;
            th.Start();
        }

        void xPanel_DeletedDash()
        {
            check_dash();
        }

        void xPanel_getmb()
        {
            if (device == DEVICE.PICOFLASHER)
            {
                picoflasher.getFlashConfig();
            }
            else if (device == DEVICE.XFLASHER_SPI)
            {
                xflasher.getFlashConfig();
            }
            else
            {
                getmbtype();
            }
        }

        void xPanel_updateSourc(string filename)
        {
            txtFilePath1.Text = filename;
            variables.filename1 = filename;
            nand_init();
        }

        void xPanel_loadFil(ref string filename, bool erase = false)
        {
            loadfile(ref filename, ref txtFilePath1, erase);
        }

        #endregion

        #region XSVF Panel

        void xsvfInfo_ProgramCRClick()
        {
            if (xsvfInfo.heResult() == -1) return;
            string file;
            if (variables.debugme) Console.WriteLine(xsvfInfo.heResult());
            bool demon = xsvfInfo.deResult();
            if (variables.debugme) Console.WriteLine("demon {0}", demon);
            if (demon)
            {
                if (variables.debugme) Console.WriteLine(variables.demon_xsvf[xsvfInfo.heResult() - 1]);
                file = (variables.demon_xsvf[xsvfInfo.heResult() - 1]);
            }
            else
            {
                if (variables.debugme) Console.WriteLine(variables.xsvf[xsvfInfo.heResult() - 1]);
                file = (variables.xsvf[xsvfInfo.heResult() - 1]);
            }
            programcr(file);

        }

        void xsvfInfo_CloseCRClick()
        {
            listInfo.Remove(xsvfInfo);
            pnlInfo.Controls.Remove(xsvfInfo);
            pnlInfo.Controls.Add(listInfo[listInfo.Count - 1]);
            pnlTools.Enabled = true;
        }

        #endregion

        delegate void SaveFileCallback(byte[] temp, string name, string filter);
        void saveFile(byte[] temp, string name, string filter)
        {
            SaveFileCallback d = new SaveFileCallback(jf_SaveFileDialog);
            this.Invoke(d, new object[] { temp, name, filter });
        }

        void jf_SaveFileDialog(byte[] temp, string name, string filter)
        {
            if (temp != null)
            {
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = name;
                savefile.Filter = filter;
                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(savefile.FileName, temp);
                    variables.filename1 = savefile.FileName;
                    txtFilePath1.Text = variables.filename1;
                }
            }
        }

        void nTools_IterChange(int iter)
        {
            variables.NoReads = iter;
            ldInfo.updateIter(iter);
        }

        void nandInfo_DragDropChanged(string filename)
        {
            this.txtFilePath1.Text = filename;
            variables.filename1 = filename;
            erasevariables();
            if (Path.GetExtension(filename) == ".bin")
            {
                nand_init();
            }
        }
        #endregion

        #region EXEs
        #region LPT

        public void unpack_lpt()
        {
            /*
            if (!File.Exists(Path.Combine(variables.pathforit,"\\inpout32.dll")))
            {
                SaveResourceToDisc("Resources.inpout32.dll", "inpout32.dll", variables.pathforit);
            }
            /**/
            //if (!File.Exists(variables.AppData + "\\inpout32.dll"))
            {
                SaveResourceToDisc("Resources.inpout32.dll", "inpout32.dll", variables.AppData);
            }
            //if (!File.Exists(variables.AppData + "\\LPT_XSVF_Player.exe"))
            {
                SaveResourceToDisc("Resources.LPT_XSVF_Player.exe", "LPT_XSVF_Player.exe", variables.AppData);
            }
            //*/
        }
        private void CopyStream(Stream input, Stream output)
        {
            // Insert null checking here for production
            byte[] buffer = new byte[8192];

            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }
        private void SaveResourceToDisc(string resourceName, string outputName, string servicePath)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            //string servicePath = variables.AppData;
            string resourceFile = Path.Combine(servicePath, outputName);
            if (!File.Exists(resourceFile))
            {
                //Get our namespace.
                string my_namespace = a.GetName().Name.ToString();

                using (Stream st = a.GetManifestResourceStream(my_namespace + "." + resourceName))
                {
                    using (Stream output = new FileStream(resourceFile, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
                    {
                        byte[] buffer = new byte[32768];
                        int read;

                        while ((read = st.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            output.Write(buffer, 0, read);
                        }
                    }
                }
            }
        }

        public void call_lpt_player(string file, string port)
        {
            if (variables.debugme) Console.WriteLine("File: {0} | Port: {1}", Path.GetFileName(file), port);
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = variables.AppData + @"\LPT_XSVF_Player.exe";
            pProcess.StartInfo.Arguments = "\"" + file + "\"" + " " + port;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.WorkingDirectory = variables.pathforit;
            pProcess.StartInfo.RedirectStandardInput = true;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Exited += new EventHandler(lpt_Exited);
            pProcess.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(process_OutputDataReceived);
            try
            {
                pProcess.Start();
                pProcess.BeginOutputReadLine();
                pProcess.WaitForExit();
                if (pProcess.HasExited)
                {
                    pProcess.CancelOutputRead();
                }
            }
            catch (Exception objException)
            {
                MessageBox.Show(objException.ToString());
            }
        }

        #endregion

        private void lpt_Exited(object sender, System.EventArgs e)
        {
            try
            {
                File.Delete(variables.AppData + @"\LPT_XSVF_Player.exe");
                File.Delete(variables.AppData + @"\inpout32.dll");
            }
            catch (Exception) { }

        }
        void process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        #endregion

        #region Basic Functions

        private void abort()
        {
            variables.escapeloop = true;
            ThreadStart starter = delegate { escapedloop(); };
            new Thread(starter).Start();
            if (xflasher.inUse) xflasher.abort();
        }

        #region Nand
        //////////////////////////////////////////////

        void nandcustom(string function, string filename, int size, int startblock, int length)
        {
            if (String.IsNullOrWhiteSpace(filename) && function != "Erase") return;
            if (startblock < 0) startblock = 0;
            if (length < 0) length = 0;

            if (device == DEVICE.PICOFLASHER) // Remove once PicoFlasher supports the filename properly
            {
                MessageBox.Show("PicoFlasher does not support custom operations yet", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Nandsize sizex = Nandsize.S0;
            if (size == 16) sizex = Nandsize.S16;
            else if (size == 64) sizex = Nandsize.S64;
            else if (size == 256) sizex = Nandsize.S256;
            else if (size == 512) sizex = Nandsize.S512;

            ThreadStart starter = null;
            if (!DemoN.DemonDetected)
            {
                if (function == "Read")
                {
                    if (usingVNand)
                    {
                        starter = delegate
                        {
                            vnand.read_v2(filename, startblock, length);
                        };
                    }
                    else if (device == DEVICE.PICOFLASHER)
                    {
                        picoflasher.Read(1, (uint) startblock, (uint) (startblock + length)); // TODO: respect filename
                    }
                    else if (device == DEVICE.XFLASHER_SPI)
                    {
                        xflasher.readNand(size, filename, startblock, length);
                    }
                    else
                    {
                        starter = delegate
                        {
                            nandx.read(filename, sizex, true, startblock, length);
                        };
                    }
                }
                else if (function == "Erase")
                {
                    if (usingVNand)
                    {
                        starter = delegate
                        {
                            vnand.erase_v2(startblock, length);
                        };
                    }
                    else if (device == DEVICE.PICOFLASHER)
                    {
                        MessageBox.Show("PicoFlasher can't erase", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (device == DEVICE.XFLASHER_SPI)
                    {
                        xflasher.writeNand(size, "erase", 0, startblock, length);
                    }
                    else
                    {
                        starter = delegate
                        {
                            nandx.erase(sizex, startblock, length);
                        };
                    }
                }
                else if (function == "Write")
                {
                    if (usingVNand)
                    {
                        starter = delegate
                        {
                            if (Path.GetExtension(filename) == ".ecc") vnand.write_v2(filename, startblock, length, true, true);
                            else vnand.write_v2(filename, startblock, length);
                        };
                    }
                    else if (device == DEVICE.PICOFLASHER)
                    {
                        picoflasher.Write(Path.GetExtension(filename) == ".ecc" ? 1 : 0, (uint)startblock, (uint)(startblock + length)); // TODO: respect filename
                    }
                    else if (device == DEVICE.XFLASHER_SPI)
                    {
                        if (Path.GetExtension(filename) == ".ecc") xflasher.writeNand(16, filename, 1, startblock, length, true);
                        else xflasher.writeNand(size, filename, 0, startblock, length, true);
                    }
                    else
                    {
                        if (device == DEVICE.NAND_X && variables.mtxUsbMode)
                        {
                            if (Path.GetExtension(filename) == ".ecc") mtx_usb.writeNand(16, filename, 1, startblock, length);
                            else mtx_usb.writeNand(size, filename, 0, startblock, length);
                        }
                        else
                        {
                            starter = delegate
                            {
                                if (Path.GetExtension(filename) == ".ecc") nandx.write(filename, sizex, startblock, length, true, true);
                                else nandx.write(filename, sizex, startblock, length);
                            };
                        }
                    }
                }
                else if (function == "Xsvf")
                {
                    if (nTools.getRbtnUSB())
                    {
                        if (device == DEVICE.PICOFLASHER)
                        {
                            MessageBox.Show("PicoFlasher can't write timing", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else if (device == DEVICE.XFLASHER_SPI)
                        {
                            xflasher.flashSvf(filename);
                        }
                        else if (device == DEVICE.XFLASHER_EMMC)
                        {
                            MessageBox.Show("Unable to write timing in eMMC mode\n\nPlease switch to SPI mode", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else if (device == DEVICE.NAND_X && variables.mtxUsbMode)
                        {
                            mtx_usb.flashXsvf(filename);
                        }
                        else
                        {
                            starter = delegate
                            {
                                nandx.xsvf(filename);
                            };
                        }
                    }
                    else
                    {
                        unpack_lpt(); //changed
                        starter = delegate { call_lpt_player(filename, nTools.getLptPort()); };
                        //starter = delegate { LPT_XSVF.lxsvf(filename, txtLPTPort.Text, true); };
                    }
                }
            }
            else
            {
                if (function == "Read")
                {
                    starter = delegate
                    {
                        demon.read(filename, startblock, length);
                    };
                }

                else if (function == "Erase")
                {
                    starter = delegate
                    {
                        demon.erase(startblock, length);
                    };
                }
                else if (function == "Write")
                {
                    starter = delegate
                    {
                        demon.write(filename, startblock, length);
                    };
                }
                else if (function == "Xsvf")
                {
                    starter = delegate
                    {
                        demon.xsvf(filename);
                    };
                }
            }
            if (starter != null)
            {
                try
                {
                    new Thread(starter).Start();
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }

        void nandcustom()
        {
            NandProArg cnaform = new NandProArg();
            cnaform.RunClick += cnaform_RunClick;
            cnaform.Show();
            cnaform.Location = new Point(Location.X + (Width - cnaform.Width) / 2, Location.Y + 105);
        }
        public string FindTextBetween(string text, string left, string right)
        {
            // TODO: Validate input arguments

            int beginIndex = text.IndexOf(left); // find occurence of left delimiter
            if (beginIndex == -1)
                return string.Empty; // or throw exception?

            beginIndex += left.Length;

            int endIndex = text.IndexOf(right, beginIndex); // find occurence of right delimiter
            if (endIndex == -1)
                return string.Empty; // or throw exception?

            return text.Substring(beginIndex, endIndex - beginIndex).Trim();
        }
        void cnaform_RunClick(string function, string filename, int size, int startblock, int length)
        {
            if (size == 0 && function != "Xsvf") return;
            nandcustom(function, filename, size, startblock, length);
        }
        //////////////////////////////////////////////
        void programcr(string filex)
        {
            string file = "";
            if (filex == "") return;
            if (device == DEVICE.XFLASHER_SPI)
                file = variables.pathforit + @"\common\svf\" + filex + ".svf";
            else
                file = variables.pathforit + @"\common\xsvf\" + filex + ".xsvf";

            Console.WriteLine("Programming Glitch Chip");

            if (DemoN.DemonDetected)
            {
                demon.xsvf(file);
            }
            else
            {
                if (nTools.getRbtnUSB())
                {
                     if (device == DEVICE.PICOFLASHER)
                    {
                        MessageBox.Show("PicoFlasher can't to write timing.", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if(device == DEVICE.XFLASHER_SPI)
                    {
                        xflasher.flashSvf(file);
                    }
                    else if (device == DEVICE.XFLASHER_EMMC)
                    {
                        MessageBox.Show("Unable to write timing in eMMC mode\n\nPlease switch to SPI mode", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (device == DEVICE.NAND_X && variables.mtxUsbMode)
                    {
                        mtx_usb.flashXsvf(file);
                    }
                    else
                    {
                        ThreadStart starter = delegate { nandx.xsvf(file); };
                        new Thread(starter).Start();
                        _waitmb.Set();
                    }
                }
                else
                {
                    unpack_lpt(); //changed
                    //ThreadStart starter = delegate { LPT_XSVF.lxsvf(file, txtLPTPort.Text, true); };
                    //new Thread(starter).Start();
                    ThreadStart starter = delegate { call_lpt_player(file, nTools.getLptPort()); };
                    new Thread(starter).Start();
                }
            }
        }
        //////////////////////////////////////////////
        NandX.Errors getmbtype(bool stealth = false)
        {
            if (!stealth) Console.WriteLine("Checking Console...");
            string flashconfig = "";
            NandX.Errors error = NandX.Errors.None;
            error = nandx.getflashmb(ref flashconfig, stealth);
            variables.flashconfig = flashconfig;
            if (error != NandX.Errors.None)
            {
                Console.WriteLine("");
                return error;
            }
            if (variables.debugme) Console.WriteLine(variables.flashconfig);
            if (flashconfig == ("008A3020"))
            {
                variables.ctyp = variables.cunts[6];
                Console.WriteLine(variables.ctyp.Text);
                xPanel.setMBname(variables.ctyp.Text);
            }
            else if (flashconfig == ("00AA3020"))
            {
                variables.ctyp = variables.cunts[7];
                Console.WriteLine(variables.ctyp.Text);
                xPanel.setMBname(variables.ctyp.Text);
            }
            else if (flashconfig == "C0462002")
            {
                error = NandX.Errors.WrongConfig;

                Console.WriteLine("");
                MessageBox.Show("Unable to read/write eMMC type console with an SPI tool\n\nPlease use an eMMC tool", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return error;
            }
            else if (flashconfig == ("01198010"))
            {
                Console.WriteLine("Xenon, Zephyr, Falcon");
            }
            else if (flashconfig == ("00023010"))
            {
                Console.WriteLine("Jasper 16MB, Trinity");
            }
            else if (flashconfig == ("00043000"))
            {
                variables.ctyp = variables.cunts[10];
                Console.WriteLine(variables.ctyp.Text);
                xPanel.setMBname(variables.ctyp.Text);
            }
            try
            {
                if (!Encoding.ASCII.GetString(Oper.returnportion(variables.conf, 0, 50)).Contains("Microsoft"))
                {
                    if (variables.debugme) Console.WriteLine(Encoding.ASCII.GetString(Oper.returnportion(variables.conf, 0, 50)));
                    error = NandX.Errors.WrongHeader;
                }
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            getcb_v(flashconfig);
            Console.WriteLine("");
            _waitmb.Set();
            return error;
        }
        public void getcb_v(string flashconfig)
        {
            if (variables.debugme) Console.WriteLine("\nGetting cb {0}", flashconfig);
            try
            {
                if (variables.conf != null)
                {
                    int temp = Nand.Nand.getcb_build(variables.conf);
                    if (temp >= 9188 && temp <= 9250)
                    {
                        variables.ctyp = variables.cunts[1];
                        xPanel.setMBname(variables.ctyp.Text);
                    }
                    else if (temp >= 4558 && temp <= 4580)
                    {
                        variables.ctyp = variables.cunts[3];
                        xPanel.setMBname(variables.ctyp.Text);
                    }
                    else if (temp >= 6712 && temp <= 6780)
                    {
                        if (flashconfig == "01198010")
                        {
                            variables.ctyp = variables.cunts[5];
                            xPanel.setMBname(variables.ctyp.Text);
                        }
                        else if (flashconfig == "00023010")
                        {
                            variables.ctyp = variables.cunts[4];
                            xPanel.setMBname(variables.ctyp.Text);
                        }
                        else if (flashconfig == "008A3020")
                        {
                            variables.ctyp = variables.cunts[6];
                            xPanel.setMBname(variables.ctyp.Text);
                        }
                        else if (flashconfig == "00AA3020")
                        {
                            variables.ctyp = variables.cunts[7];
                            xPanel.setMBname(variables.ctyp.Text);
                        }
                    }
                    else if (temp >= 13121 && temp <= 13200)
                    {
                        variables.ctyp = variables.cunts[10];
                        xPanel.setMBname(variables.ctyp.Text);
                    }
                    else if ((temp >= 1888 && temp <= 1960) || (temp >= 7373 && temp <= 7378) || temp == 8192)
                    {
                        variables.ctyp = variables.cunts[8];
                        xPanel.setMBname(variables.ctyp.Text);
                    }
                    else if (temp >= 5761 && temp <= 5780)
                    {
                        variables.ctyp = variables.cunts[2];
                        xPanel.setMBname(variables.ctyp.Text);
                    }
                    //else
                    //{
                    //    if (variables.smcmbtype < variables.console_types.Length && variables.smcmbtype >= 0) consolebox.Text = variables.console_types[variables.smcmbtype];
                    //}
                    Console.WriteLine("CB Version: {0}", temp);
                }
                else
                {
                    if (variables.debugme) Console.WriteLine("No config file");
                }
            }
            catch (Exception) { }
            variables.conf = null;
        }

        /// <summary>
        /// 1 - read, 2 write, 3 writeecc
        /// </summary>
        /// <param name="function"></param>
        void getconsoletype(int function, int writelength = 0)
        {
            NandX.Errors error = 0;
            ConsoleTypes.Selected sel = ConsoleTypes.Selected.All;
            bool twombread = false;
            bool sfulldump = false;
            if (function == 1 && variables.ctyp.ID != 11)
            {
                error = getmbtype(true);
                if (error == NandX.Errors.NoFlashConfig) return;
                if (variables.ctyp.ID == 6 || variables.ctyp.ID == 7) sel = ConsoleTypes.Selected.BigBlock;
                if (xPanel.getRbtnJtagChecked() || xPanel.getRbtnGlitchChecked() || xPanel.getRbtnGlitch2Checked()) twombread = true;
                sfulldump = true;
            }

            if (variables.ctyp.Nsize != Nandsize.S16 && variables.ctyp.ID != 11 && !DemoN.DemonDetected) variables.ctyp = callconsoletypes(sel, twombread, sfulldump);

            if (variables.ctyp.ID == -1 && !DemoN.DemonDetected) return;

            if (function == 1)
            {
                if (variables.ctyp.ID == 11)
                {
                    calldrives(variables.outfolder + "\\nanddump1.bin", Panels.LDrivesInfo.Function.Read);
                    return;
                }
                else
                {
                    try
                    {
                        ThreadStart starter = delegate { readnand(error); };
                        new Thread(starter).Start();
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
            }
            else if (function == 2)
            {
                if (variables.ctyp.ID == 11)
                {
                    calldrives(variables.filename1, Panels.LDrivesInfo.Function.Write);
                    return;
                }
                else
                {
                    try
                    {
                        ThreadStart starter = delegate { writenand(false); };
                        new Thread(starter).Start();
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
            }
            else if (function == 3)
            {
                if (variables.ctyp.ID == 11)
                {
                    calldrives(variables.filename1, Panels.LDrivesInfo.Function.Write);
                    return;
                }
                else
                {
                    try
                    {
                        ThreadStart starter = delegate { writenand(true, writelength); };
                        new Thread(starter).Start();
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
            }
        }

        void readnand(NandX.Errors error)
        {
            //int error = 0;
            int twomb = 0;
            if (usingVNand) error = NandX.Errors.None;
            if (!DemoN.DemonDetected)
            {
                //error = getmbtype();
                if (error != NandX.Errors.None && error != NandX.Errors.WrongHeader) return;
                if (variables.debugme) Console.WriteLine("Read Nand");

                #region nandsize
                if ((variables.ctyp.ID == 6 || variables.ctyp.ID == 7) && !variables.fulldump)
                {
                    variables.nandsizex = Nandsize.S64;
                }
                else if (variables.ctyp.ID == 0)
                {
                    variables.nandsizex = Nandsize.S16;
                }
                else
                {
                    if (variables.debugme) Console.WriteLine(variables.ctyp.ID);
                    variables.nandsizex = variables.ctyp.Nsize;
                }
                #endregion

                //if (getmbtype() != 0) return;
                if (variables.twombread) twomb = 0x7C;
            }
            int j = 1;
            for (j = 1; j <= nTools.getNumericIterations();)
            {
                if (variables.debugme) Console.Write(j);
                _waitmb.WaitOne();
                lock (_object)
                {
                    if (variables.debugme) Console.WriteLine(j);
                    _waitmb.Reset();
                    Thread.Sleep(1000);
                    if (j == 2)
                    {
                        if (File.Exists(Path.Combine(variables.pathforit, variables.filename)))
                        {
                            this.txtFilePath1.Text = System.IO.Path.Combine(variables.pathforit, variables.filename1);
                            Thread.Sleep(1000);
                            nand_init();
                            Thread.Sleep(1000);
                        }
                    }
                    else if (j >= 3)
                    {
                        if (File.Exists(Path.Combine(variables.pathforit, variables.filename)))
                        {
                            this.txtFilePath2.Text = System.IO.Path.Combine(variables.pathforit, variables.filename2);
                            new Thread(comparenands).Start();
                        }
                    }

                    variables.filename = variables.outfolder + "\\nanddump" + j + ".bin";
                    variables.iterations = j;
                    if (File.Exists(variables.filename))
                    {
                        if (DialogResult.Cancel == MessageBox.Show("File already exists, it will be DELETED! Press OK to continue", "About to overwrite a nanddump", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                        {
                            Console.WriteLine("Cancelled");
                            Console.WriteLine("");
                            return;
                        };
                        if (!DemoN.DemonDetected)
                        {
                            if (error == NandX.Errors.WrongHeader)
                            {
                                if (DialogResult.Cancel == MessageBox.Show("Header seems to be wrong! This shouldnt happen for stock image! Are you really sure you want to overwrite your previously dumped image???", "Wrong Header", MessageBoxButtons.OKCancel, MessageBoxIcon.Error))
                                {
                                    Console.WriteLine("Cancelled");
                                    Console.WriteLine("");
                                    return;
                                };
                            }
                        }
                    }
                    if (variables.debugme) Console.WriteLine("Starting Reading");

                    if (DemoN.DemonDetected)
                    {
                        demon.read(variables.filename);
                    }
                    else
                    {
                        variables.reading = true;
                        if (!usingVNand)
                        {
                            if (nandx.read(variables.filename, variables.nandsizex, false, 0x0, twomb) != NandX.Errors.None) return;
                        }
                        else vnand.read_v2(variables.filename, 0, twomb);
                        variables.reading = false;
                    }
                    j++;
                    //else Console.WriteLine("Nandpro is already running");
                }
            }
            if (j == 2)
            {
                if (File.Exists(Path.Combine(variables.pathforit, variables.filename)))
                {
                    this.txtFilePath1.BeginInvoke((Action)(() => txtFilePath1.Text = System.IO.Path.Combine(variables.pathforit, variables.filename1)));

                    Thread.Sleep(1000);
                    nand_init();
                }
            }
            else if (j >= 3)
            {
                if (File.Exists(Path.Combine(variables.pathforit, variables.filename)))
                {
                    this.txtFilePath2.BeginInvoke((Action)(() => txtFilePath2.Text = System.IO.Path.Combine(variables.pathforit, variables.filename)));
                    new Thread(comparenands).Start();
                }

            }
        }
        //////////////////////////////////////////////
        /// <summary>
        ///  Write
        /// </summary>
        /// <param name="ecc"></param>
        void writenand(bool ecc, int writelength = 0)
        {
            if (String.IsNullOrWhiteSpace(variables.filename1)) loadfile(ref variables.filename1, ref this.txtFilePath1, true);
            if (String.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;
            if (DemoN.DemonDetected)
            {
                demon.write(variables.filename1);
                if (Path.GetExtension(variables.filename1) == ".ecc")
                {
                    if (variables.tempfile != "")
                    {
                        variables.filename1 = variables.tempfile;
                        txtFilePath1.Text = variables.tempfile;
                    }
                }
            }
            else
            {
                //if (textBox2.Text != "008A3020" && textBox2.Text != "00AA3020") ctypeselected = 0;

                double len = new FileInfo(variables.filename1).Length;
                if (variables.debugme) Console.WriteLine("File Length = {0} | Expected 69206016 for a 64MB nand", len);
                if ((variables.ctyp.ID == 6 || variables.ctyp.ID == 7) && (len == 69206016))
                {
                    variables.nandsizex = Nandsize.S64;
                }
                else if (variables.ctyp.ID == 0)
                {
                    variables.nandsizex = Nandsize.S16;
                }
                else
                {
                    variables.nandsizex = variables.ctyp.Nsize;
                }

                if (Path.GetExtension(variables.filename1) == ".ecc")
                {
                    if (!ecc)
                    {
                        Console.WriteLine("You need an .bin image");
                        return;
                    }
                    NandX.Errors result = NandX.Errors.None;

                    if (!usingVNand) result = nandx.write(variables.filename1, variables.nandsizex, 0, 0x50, true, true);
                    else vnand.write_v2(variables.filename1, 0, 0x50, true, true);

                    Thread.Sleep(500);
                    if (variables.tempfile != "" && result == NandX.Errors.None)
                    {
                        variables.filename1 = variables.tempfile;
                        txtFilePath1.Text = variables.tempfile;
                    }
                }
                else if (Path.GetExtension(variables.filename1) == ".bin")
                {
                    if (ecc)
                    {
                        Console.WriteLine("You need an .ecc image");
                        return;
                    }

                    if (!usingVNand) nandx.write(variables.filename1, variables.nandsizex, 0, writelength);
                    else vnand.write_v2(variables.filename1, 0, writelength);

                    //NandX.write(ref txtBlocks, ref progressBar1, variables.filename1, variables.nandsizex, 0, 0);
                }
            }
        }
        void writefusion()
        {
            if (String.IsNullOrWhiteSpace(variables.filename1)) loadfile(ref variables.filename1, ref this.txtFilePath1, true);
            //if (textBox2.Text != "008A3020" && textBox2.Text != "00AA3020") ctypeselected = 0;
            if (String.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;
            if (DemoN.DemonDetected)
            {
                demon.write_fusion(variables.filename1);
                try
                {
                    SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                    if (variables.soundsuccess != "") success.SoundLocation = variables.soundsuccess;
                    success.Play();
                }
                catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); };
            }
            else
            {
                if (variables.ctyp.ID == -1) variables.ctyp = callconsoletypes(ConsoleTypes.Selected.All);
                if (variables.ctyp.ID == -1) return;
                double len = new FileInfo(variables.filename1).Length;
                if (variables.debugme) Console.WriteLine("File Length = {0} | Expected 69206016 for a 64MB nand", len);
                if ((variables.ctyp.ID == 6 || variables.ctyp.ID == 7) && (len == 69206016))
                {
                    variables.nandsizex = Nandsize.S64;
                }
                else if (variables.ctyp.ID == 0)
                {
                    variables.nandsizex = Nandsize.S16;
                }
                else
                {
                    variables.nandsizex = variables.ctyp.Nsize;
                }

                if (Path.GetExtension(variables.filename1) == ".bin")
                {
                    if (!usingVNand) nandx.write(variables.filename1, variables.nandsizex, 0, 0, true, false);
                    else vnand.write_v2(variables.filename1, 0, 0, true, false);
                }
            }
        }
        void writexell()
        {
            if (String.IsNullOrWhiteSpace(variables.filename1)) loadfile(ref variables.filename1, ref this.txtFilePath1, true);
            if (String.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;
            //if (textBox2.Text != "008A3020" && textBox2.Text != "00AA3020") ctypeselected = 0;
            if (DemoN.DemonDetected)
            {
                demon.write(variables.filename1);
                if (Path.GetExtension(variables.filename1) == ".bin")
                {
                    variables.filename1 = "";
                    txtFilePath1.Text = "";
                    if (variables.tempfile != "")
                    {
                        variables.filename1 = variables.tempfile;
                        txtFilePath1.Text = variables.tempfile;
                    }
                }
            }
            else
            {
                double len = new FileInfo(variables.filename1).Length;
                if (variables.debugme) Console.WriteLine("File Length = {0} | Expected 69206016 for a 64MB nand", len);

                NandX.Errors result = NandX.Errors.None;

                if (!usingVNand) result = nandx.write(variables.filename1, Nandsize.S16, 0, 0x50);
                else vnand.write_v2(variables.filename1, 0, 0x50);

                if (variables.tempfile != "" && result == NandX.Errors.None)
                {
                    variables.filename1 = variables.tempfile;
                    txtFilePath1.Text = variables.tempfile;
                }
            }
        }
        #endregion

        #region Small Stuff
        void movework()
        {
            if (variables.reading) return;
            Thread.Sleep(2000);
            variables.xefolder = Path.Combine(Directory.GetParent(variables.outfolder).FullName, nand.ki.serial);

            //updateS((variables.filename1.Replace(variables.outfolder, variables.xefolder)));
            Console.WriteLine("Moving all files from output folder to {0}", variables.xefolder);
            Console.Write("");
            String l_sDirectoryName = variables.xefolder;
            DirectoryInfo l_dDirInfo = new DirectoryInfo(l_sDirectoryName);
            if (l_dDirInfo.Exists == false)
                Directory.CreateDirectory(l_sDirectoryName);
            List<String> MyFiles = Directory.GetFiles(variables.outfolder, "*.*", SearchOption.TopDirectoryOnly).ToList();
            List<String> myfolders = Directory.GetDirectories(variables.outfolder, "*.*", SearchOption.TopDirectoryOnly).ToList();
            foreach (string fold in myfolders)
            {
                try
                {

                    string name = Path.GetFileName(fold);
                    // if (Directory.Exists(l_dDirInfo + "\\" + fold)) Directory.Delete(l_dDirInfo + "\\" + fold);
                    if (variables.debugme) Console.WriteLine("Moving {0}", fold);


                    if ((fold.Contains(nand.ki.serial)) || ((variables.custname != "") && (fold.Contains(variables.custname))))
                    {
                        System.IO.Directory.Move(fold, Path.Combine(variables.xefolder, name));
                        variables.custname = "";
                    }

                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            foreach (string file in MyFiles)
            {
                if (variables.debugme) Console.WriteLine("Moving {0}", file);
                FileInfo mFile = new FileInfo(file);
                if (new FileInfo(l_dDirInfo + "\\" + mFile.Name).Exists == false)//to remove name collusion
                    mFile.MoveTo(l_dDirInfo + "\\" + mFile.Name);
                else
                {
                    string flname = Path.GetFileNameWithoutExtension(mFile.Name);
                    int number = 1;
                    if (flname.Contains("(") && flname.Contains(")"))
                    {
                        //Console.WriteLine(flname.Substring(0,flname.IndexOf("(")));
                        //number = Convert.ToInt32(flname.Substring(flname.IndexOf("("), 1)) ;
                        string Nflname = flname.Substring(0, flname.IndexOf("("));

                        do
                        {
                            number++;
                        } while (File.Exists(l_dDirInfo + "\\" + Nflname + "(" + number + ")" + mFile.Extension));
                        if (!File.Exists(l_dDirInfo + "\\" + Nflname + "(" + number + ")" + mFile.Extension))
                        {
                            mFile.MoveTo(l_dDirInfo + "\\" + Nflname + "(" + number + ")" + mFile.Extension);
                        }
                    }
                    else
                    {
                        do
                        {
                            number++;
                        } while (File.Exists(l_dDirInfo + "\\" + Path.GetFileNameWithoutExtension(mFile.Name) + "(" + number + ")" + mFile.Extension));
                        if (!File.Exists(l_dDirInfo + "\\" + Path.GetFileNameWithoutExtension(mFile.Name) + "(" + number + ")" + mFile.Extension))
                        {
                            mFile.MoveTo(l_dDirInfo + "\\" + Path.GetFileNameWithoutExtension(mFile.Name) + "(" + number + ")" + mFile.Extension);
                        }
                    }
                }
            }

            variables.filename1 = variables.filename1.Replace(variables.outfolder, variables.xefolder);
            txtFilePath1.BeginInvoke(new Action(() => txtFilePath1.Text = variables.filename1));            
            nand = new Nand.PrivateN(variables.filename1, variables.cpkey); // Re-init because folder changed
        }

        public void nand_init()
        {
            ThreadStart starter = delegate { nandinit(); };
            new Thread(starter).Start();
        }

        private void updatecptextbox()
        {
            if (variables.debugme) Console.WriteLine("Event wait");
            _event1.WaitOne();
            if (variables.debugme) Console.WriteLine("Event Started");
            if (variables.debugme) Console.WriteLine(variables.cpkey);
            txtCPUKey.BeginInvoke(new Action(() => txtCPUKey.Text = variables.cpkey));
        }

        private static void savekvinfo(string savefile)
        {
            try
            {
                if (!nand.ok) return;
                TextWriter tw = new StreamWriter(savefile);
                tw.WriteLine("*******************************************");
                tw.WriteLine("*******************************************");
                string console_type = "";
                if (nand.bl.CB_A >= 9188 && nand.bl.CB_A <= 9250)
                {
                    console_type = "Trinity";
                }
                else if (nand.bl.CB_A >= 13121 && nand.bl.CB_A <= 13200)
                {
                    console_type = "Corona";
                    if (nand.noecc) console_type += " 4GB";
                }
                else if (nand.bl.CB_A >= 6712 && nand.bl.CB_A <= 6780) console_type = "Jasper";
                else if (nand.bl.CB_A >= 4558 && nand.bl.CB_A <= 4590) console_type = "Zephyr";
                else if ((nand.bl.CB_A >= 1888 && nand.bl.CB_A <= 1960) || nand.bl.CB_A == 7373 || nand.bl.CB_A == 8192) console_type = "Xenon";
                else if (nand.bl.CB_A >= 5761 && nand.bl.CB_A <= 5780) console_type = "Falcon";
                else
                {
                    if (variables.smcmbtype < variables.console_types.Length && variables.smcmbtype >= 0) console_type = variables.console_types[variables.smcmbtype];
                }
                tw.WriteLine("Console Type: {0}", console_type);
                tw.WriteLine("");
                tw.WriteLine("Cpu Key: {0}", variables.cpkey);
                tw.WriteLine("");
                tw.WriteLine("KV Type: {0}", nand.ki.kvtype.Replace("0", ""));
                tw.WriteLine("");
                tw.WriteLine("MFR Date: {0}", nand.ki.mfdate);
                tw.WriteLine("");
                tw.WriteLine("Console ID: {0}", nand.ki.consoleid);
                tw.WriteLine("");
                tw.WriteLine("Serial: {0}", nand.ki.serial);
                tw.WriteLine("");
                string region = "";
                if (nand.ki.region == "02FE") region = "PAL/EU";
                else if (nand.ki.region == "00FF") region = "NTSC/US";
                else if (nand.ki.region == "01FE") region = "NTSC/JAP";
                else if (nand.ki.region == "01FF") region = "NTSC/JAP";
                else if (nand.ki.region == "01FC") region = "NTSC/KOR";
                else if (nand.ki.region == "0101") region = "NTSC/HK";
                else if (nand.ki.region == "0201") region = "PAL/AUS";
                else if (nand.ki.region == "7FFF") region = "DEVKIT";
                tw.WriteLine("Region: {0} | {1}", nand.ki.region, region);
                tw.WriteLine("");
                tw.WriteLine("Osig: {0}", nand.ki.osig);
                tw.WriteLine("");
                tw.WriteLine("DVD Key: {0}", nand.ki.dvdkey);
                tw.WriteLine("");
                tw.WriteLine("*******************************************");
                tw.WriteLine("*******************************************");
                tw.Close();
                Console.WriteLine("KV Info saved to file");
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); Console.WriteLine("Failed"); }
        }

        void comparenands()
        {
            if (variables.filename1 == null || variables.filename2 == null) { MessageBox.Show("Input all Files"); return; }
            if (!File.Exists(variables.filename1) || !File.Exists(variables.filename2)) return;
            else
            {
                FileInfo inf = new FileInfo(variables.filename1);
                string time = "";
                if (inf.Length > 64 * 1024 * 1024) time = "Takes a while on big nands";
                Console.WriteLine("Comparing...{0}", time);
                try
                {
                    byte[] temp1 = Nand.BadBlock.find_bad_blocks_b(variables.filename1, true);
                    byte[] temp2 = Nand.BadBlock.find_bad_blocks_b(variables.filename2, true);

                    string temp1_hash = Oper.GetMD5HashFromFile(temp1);
                    string temp2_hash = Oper.GetMD5HashFromFile(temp2);

                    temp1 = null;
                    temp2 = null;
                    //filecompareresult = FileEquals(filename1, filename2);
                    if (temp1_hash == temp2_hash)
                    {
                        Console.WriteLine("Nands are the same");
                        Console.WriteLine("");
                        try
                        {
                            SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                            if (variables.soundcompare != "") success.SoundLocation = variables.soundcompare;
                            success.Play();
                        }
                        catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); };
                        try
                        {
                            string md5file = Path.Combine(Directory.GetParent(variables.filename1).ToString(), "checksum.md5");
                            string hash1 = Oper.GetMD5HashFromFile(variables.filename1);
                            string hash2 = Oper.GetMD5HashFromFile(variables.filename2);
                            if (File.Exists(md5file))
                            {
                                File.AppendAllText(md5file, "\n");
                                File.AppendAllText(md5file, hash1 + " *" + Path.GetFileName(variables.filename1) + "\n");
                                File.AppendAllText(md5file, hash1 + " *" + Path.GetFileName(variables.filename2) + "\n");
                            }
                            else
                            {
                                using (StreamWriter file = new StreamWriter(Path.Combine(Directory.GetParent(variables.filename1).ToString(), "checksum.md5")))
                                {
                                    file.WriteLine("# MD5 checksums generated by J-Runner");
                                    file.WriteLine("{0} *{1}", hash1, Path.GetFileName(variables.filename1));
                                    file.WriteLine("{0} *{1}", hash2, Path.GetFileName(variables.filename2));
                                }
                            }
                            if (variables.deletefiles)
                            {
                                File.Delete(variables.filename2);
                                txtFilePath2.Text = "";
                            }
                        }
                        catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
                    }
                    else
                    {
                        try
                        {
                            SoundPlayer error = new SoundPlayer(Properties.Resources.Error);
                            if (variables.sounderror != "") error.SoundLocation = variables.sounderror;
                            error.Play();
                        }
                        catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); };

                        if (MessageBox.Show("Files do not match!\nShow Differences?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                        {
                            FileEquals(variables.filename1, variables.filename2);
                        }
                        txtFilePath1.Text = "";
                        txtFilePath2.Text = "";
                        variables.filename1 = "";
                        variables.filename2 = "";
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.InnerException.ToString()); }
            }
        }

        public static string parsecpukey(string filename)
        {
            if (Path.GetExtension(filename) == ".txt")
            {
                Regex objAlphaPattern = new Regex("[a-fA-F0-9]{32}$");
                string[] cpu = File.ReadAllLines(filename);
                string cpukey = "";
                bool check = false;
                int i = 0;
                foreach (string line in cpu)
                {
                    if (objAlphaPattern.Match(line).Success) i++;
                    if (i > 1) check = true;
                }
                foreach (string line in cpu)
                {
                    if (check)
                    {
                        if (line.ToUpper().Contains("CPU"))
                        {
                            cpukey = (objAlphaPattern.Match(line).Value);
                        }
                    }
                    else
                    {
                        cpukey = (objAlphaPattern.Match(line).Value);
                        break;
                    }
                    //Console.WriteLine(objAlphaPattern.Match(line).Value);
                }
                if (Nand.Nand.VerifyKey(Oper.StringToByteArray(cpukey))) return cpukey;
                else return "";
            }
            else return "";
        }

        private long CRCbl(string filename)
        {
            crc32 crc = new crc32();
            long hashData = 0;
            if (File.Exists(filename))
            {
                byte[] fileb = File.ReadAllBytes(filename);
                fileb = editbl(fileb);
                hashData = crc.CRC(fileb);
            }
            return hashData;
        }
        private byte[] editbl(byte[] bl)
        {
            int length = Oper.ByteArrayToInt(Oper.returnportion(bl, 0xC, 4));
            if (bl[0] == 0x43 && bl[1] == 0x42)
            {
                for (int i = 0x10; i < 0x40; i++) bl[i] = 0x0;
            }
            else if (bl[0] == 0x43 && bl[1] == 0x44)
            {
                for (int i = 0x10; i < 0x20; i++) bl[i] = 0x0;
            }
            else if (bl[0] == 0x43 && bl[1] == 0x45)
            {
                for (int i = 0x10; i < 0x20; i++) bl[i] = 0x0;
            }
            else if (bl[0] == 0x43 && bl[1] == 0x46)
            {
                for (int i = 0x20; i < 0x230; i++) bl[i] = 0x0;
            }
            else if (bl[0] == 0x43 && bl[1] == 0x47)
            {
                for (int i = 0x10; i < 0x20; i++) bl[i] = 0x0;
            }
            return Oper.returnportion(bl, 0, length);
        }
        bool editblini(string file, string label, string cba, string cbb = "")
        {
            string bla;
            string blb;
            bool splitcb = true;
            if (String.IsNullOrWhiteSpace(cbb)) splitcb = false;
            if (!splitcb)
            {
                if (!File.Exists(Path.Combine(variables.pathforit, "common", "cb_" + cba + ".bin")))
                {
                    Console.WriteLine("{0} not found. Trying to download file..", "common/cb_" + cba + ".bin");
                }
                if (!File.Exists(Path.Combine(variables.pathforit, "common", "cb_" + cba + ".bin")))
                {
                    Console.WriteLine("{0} not found. Insert it manually on the common folder", "cb_" + cba + ".bin");
                    return false;
                }
                bla = "cb_" + cba + ".bin," + CRCbl(Path.Combine(variables.pathforit, "common", "cb_" + cba + ".bin")).ToString("x8");
                blb = "none,00000000";
            }
            else
            {
                if (!File.Exists(Path.Combine(variables.pathforit, "common", "cba_" + cba + ".bin")))
                {
                    Console.WriteLine("{0} not found. Trying to download file..", "common/cba_" + cba + ".bin");
                }
                if (!File.Exists(Path.Combine(variables.pathforit, "common", "cbb_" + cbb + ".bin")))
                {
                    Console.WriteLine("{0} not found. Trying to download file..", "common/cbb_" + cbb + ".bin");
                }
                if (!File.Exists(Path.Combine(variables.pathforit, "common", "cba_" + cba + ".bin")))
                {
                    Console.WriteLine("{0} not found. Insert it manually on the common folder", "cba_" + cba + ".bin");
                    return false;
                }
                if (!File.Exists(Path.Combine(variables.pathforit, "common", "cbb_" + cbb + ".bin")))
                {
                    Console.WriteLine("{0} not found. Insert it manually on the common folder", "cbb_" + cba + ".bin");
                    return false;
                }
                bla = "cba_" + cba + ".bin," + CRCbl(Path.Combine(variables.pathforit, "common", "cba_" + cba + ".bin")).ToString("x8");
                blb = "cbb_" + cbb + ".bin," + CRCbl(Path.Combine(variables.pathforit, "common", "cbb_" + cbb + ".bin")).ToString("x8");
            }
            Console.WriteLine("Editing File..");
            string[] lines = File.ReadAllLines(file);
            int i = 0;
            for (; i < lines.Length; i++)
            {
                if (lines[i] == "") continue;
                else if (lines[i].Contains('[') && lines[i].Contains(label) && lines[i].Contains(']')) break;
            }
            lines[i + 1] = bla;
            lines[i + 2] = blb;
            File.WriteAllLines(file, lines);
            Console.WriteLine("Done");
            return true;
        }

        static int FileEquals(string fileName1, string fileName2)
        {
            // Check the file size and CRC equality here.. if they are equal...    
            try
            {
                using (var file1 = new FileStream(fileName1, FileMode.Open))
                using (var file2 = new FileStream(fileName2, FileMode.Open))
                    return StreamEquals(file1, file2);
            }
            catch (System.IO.IOException)
            {
                return -1;
            }
        }
        static int StreamEquals(Stream stream1, Stream stream2)
        {
            const int bufferSize = 0x4200;
            int count = 0;
            byte[] buffer1 = new byte[bufferSize]; //buffer size
            byte[] buffer2 = new byte[bufferSize];
            while (true)
            {
                count += 0x4200;
                int count1 = stream1.Read(buffer1, 0, bufferSize);
                int count2 = stream2.Read(buffer2, 0, bufferSize);

                if (count1 != count2)
                    return 0;

                if (count1 == 0)
                    return 1;

                // You might replace the following with an efficient "memcmp"
                if (!buffer1.Take(count1).SequenceEqual(buffer2.Take(count2)))
                    Console.WriteLine("0x{0:X4}", (count - 0x4200) / 0x4200);
            }
        }

        #endregion

        #region Nand Manipulation

        public void newSession(bool partial = false)
        {
            if (!String.IsNullOrEmpty(variables.filename1))
            {
                if (!partial)
                {
                    txtCPUKey.Text = "";
                    variables.boardtype = null;
                }

                txtFilePath1.Text = "";
                txtFilePath2.Text = "";
                variables.filename = "";
                variables.filename1 = "";
                variables.filename2 = "";
                variables.xefolder = "";
                variables.cpkey = "";
                variables.gotvalues = false;
                variables.twombread = false;
                variables.fulldump = false;
                variables.flashconfig = "";
                variables.changeldv = 0;
                variables.rghable = true;
                variables.rgh1able = true;
                nand = new Nand.PrivateN();
                nandInfo.clear();
            }

            if (!partial)
            {
                xPanel.clear();
                variables.ctyp = variables.cunts[0];
                txtIP.Text = txtIP.Text.Remove(txtIP.Text.LastIndexOf('.')) + ".";
            }

            progressBar.Value = progressBar.Minimum;

            if (!partial)
            {
                saveToLog();
                txtConsole.Text = "";
                printstartuptext();
            }
        }

        void erasevariables()
        {
            variables.fulldump = false; variables.twombread = false;
            variables.ctyp = variables.cunts[0]; variables.gotvalues = false;
            variables.cpkey = "";
            //variables.outfolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "output");
            xPanel.setMBname("");
            txtCPUKey.Text = "";
            variables.flashconfig = "";
            /*if (variables.changeldv != 0)
            {
                string cfldv = "cfldv=";
                string[] edit = { cfldv };
                string[] delete = { };
                parse_ini.edit_ini(Path.Combine(variables.pathforit, @"xeBuild\data\options.ini"), edit, delete);
             * */
            variables.changeldv = 0;
            //}
            //btnCheckBadBlocks.Visible = true;
        }

        void nandinit()
        {
            bool movedalready = false;
            if (String.IsNullOrEmpty(variables.filename1)) return;
            if (!File.Exists(variables.filename1))
            {
                MessageBox.Show("No file was selected!", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                updateProgress(progressBar.Minimum);
                if (Path.GetExtension(variables.filename1) != ".bin") return;
                variables.gotvalues = true;

                bool sts = objAlphaPattern.IsMatch(variables.cpkey);

                string cpufile = Path.Combine(Path.GetDirectoryName(variables.filename1), "cpukey.txt");
                if (File.Exists(cpufile) && !(variables.cpkey.Length == 32 && sts))
                {
                    variables.cpkey = parsecpukey(cpufile);
                }
                
                if (variables.cpkey.Length != 32 || !objAlphaPattern.IsMatch(variables.cpkey)) variables.cpkey = "";

                bool foundKey = !string.IsNullOrEmpty(variables.cpkey);
                bool gotKeyFromCrc = false;

                if (!foundKey)
                {
                    long filenameKvCrc = Nand.Nand.kvcrc(variables.filename1, true);
                    
                    if (variables.debugme) Console.WriteLine("KV CRC: {0:X}", filenameKvCrc);
                    if (variables.debugme) Console.WriteLine("Searching Registry Entrys");
                    try
                    {
                        variables.cpkey = CpuKeyDB.getkey_s(filenameKvCrc, xPanel.getDataSet());
                        txtCPUKey.BeginInvoke(new Action(() => txtCPUKey.Text = variables.cpkey));
                        if (!string.IsNullOrEmpty(variables.cpkey)) gotKeyFromCrc = true;
                    }
                    catch (NullReferenceException ex) { Console.WriteLine(ex.ToString()); }
                }
                else txtCPUKey.BeginInvoke(new Action(() => txtCPUKey.Text = variables.cpkey));

                Console.WriteLine("Initializing {0}, please wait...", Path.GetFileName(variables.filename1));
                nandInfo.change_tab();
                updateProgress(progressBar.Maximum / 2);
                nand = new Nand.PrivateN(variables.filename1, variables.cpkey);
                if (!nand.ok) return;

                if (variables.debugme) Console.WriteLine("N Key: {0}, V Key: {1}", nand._cpukey, variables.cpkey);

                if (!foundKey && gotKeyFromCrc)
                {
                    if (variables.debugme) Console.WriteLine("Found key in registry");
                    nand.cpukeyverification(variables.cpkey);
                    if (variables.debugme) Console.WriteLine("allmove ", variables.allmove);
                    if (variables.debugme) Console.WriteLine(!variables.filename1.Contains(nand.ki.serial));
                    if (variables.debugme) Console.WriteLine(variables.filename1.Contains(variables.outfolder));
                    if ((variables.allmove) && (!variables.filename1.Contains(nand.ki.serial)) && (variables.filename1.Contains(variables.outfolder)))
                    {
                        if (!movedalready)
                        {
                            Thread Go = new Thread(movework);
                            Go.Start();
                            movedalready = true;
                        }
                    }
                }
                else if (foundKey)
                {
                    if (!CpuKeyDB.getkey_s(variables.cpkey, xPanel.getDataSet()))
                    {
                        if (variables.debugme) Console.WriteLine("Key verification");
                        if (nand.cpukeyverification(variables.cpkey))
                        {
                            Console.WriteLine("CPU Key is Correct");
                            if (variables.debugme) Console.WriteLine("Adding key to registry");
                            CpuKeyDB.regentries entry = new CpuKeyDB.regentries();
                            entry.kvcrc = nand.kvcrc().ToString("X");
                            entry.serial = nand.ki.serial;
                            entry.cpukey = variables.cpkey;
                            entry.extra = Nand.Nand.getConsoleName(nand, variables.flashconfig);
                            entry.dvdkey = nand.ki.dvdkey;
                            entry.osig = nand.ki.osig;
                            entry.region = nand.ki.region;

                            bool reg = CpuKeyDB.addkey_s(entry, xPanel.getDataSet());
                            if (variables.autoExtract && reg)
                            {
                                if (variables.debugme) Console.WriteLine("Auto File Extraction Initiated");
                                extractFilesFromNand();

                            }
                            if (reg) nandInfo.show_cpukey_tab();
                            txtCPUKey.BeginInvoke(new Action(() => txtCPUKey.Text = variables.cpkey));
                            if ((!variables.filename1.Contains(nand.ki.serial)) && (variables.filename1.Contains(variables.outfolder)))
                            {
                                if (!movedalready)
                                {
                                    Thread Go = new Thread(movework);
                                    Go.Start();
                                    movedalready = true;
                                }
                            }
                        }
                        else Console.WriteLine("Wrong CPU Key");
                    }
                }
                
                nandInfo.setNand(nand);
                updateProgress((progressBar.Maximum / 4) * 3); // 75%

                if (nand.ki.serial.Length > 0) // Reset XeFolder
                {
                    string xePath = Path.Combine(Directory.GetParent(variables.outfolder).FullName, nand.ki.serial);
                    if (Directory.Exists(xePath)) variables.xefolder = xePath;
                    else variables.xefolder = "";
                }
                else variables.xefolder = "";

                variables.rgh1able = Nand.ntable.isGlitch1Able(nand.bl.CB_A);

                if (variables.debugme) Console.WriteLine("----------------------");
                variables.ctyp = variables.cunts[0];
                variables.ctyp = Nand.Nand.getConsole(nand, variables.flashconfig);
                xPanel.setMBname(variables.ctyp.Text);
                variables.rghable = true;

                switch (Nand.ntable.getHackfromCB(nand.bl.CB_A))
                {
                    case variables.hacktypes.glitch:
                        xPanel.BeginInvoke(new Action(() => xPanel.setRbtnGlitchChecked(true)));
                        break;
                    case variables.hacktypes.glitch2:
                        xPanel.BeginInvoke(new Action(() => xPanel.setRbtnGlitch2Checked(true)));
                        break;
                    case variables.hacktypes.jtag:
                        xPanel.BeginInvoke(new Action(() => xPanel.setRbtnJtagChecked(true)));
                        break;
                    case variables.hacktypes.devgl:
                        if (xPanel.canDevGL(variables.boardtype))
                            xPanel.BeginInvoke(new Action(() => xPanel.setRbtnDevGLChecked(true)));
                        else
                            xPanel.BeginInvoke(new Action(() => xPanel.setRbtnRetailChecked(true)));
                        break;
                    default:
                        xPanel.BeginInvoke(new Action(() => xPanel.setRbtnRetailChecked(true)));
                        break;
                }

                variables.gotvalues = !String.IsNullOrEmpty(variables.cpkey);
                Console.WriteLine("Nand Initialization Finished");
                Console.WriteLine("");

                updateProgress(progressBar.Maximum);
                if (variables.debugme)
                    Console.WriteLine("allmove ", variables.allmove);
                if (variables.debugme)
                    Console.WriteLine(!variables.filename1.Contains(nand.ki.serial));
                if (variables.debugme)
                    Console.WriteLine(variables.filename1.Contains(variables.outfolder));
                if ((variables.allmove) && (!variables.filename1.Contains(nand.ki.serial)) && (variables.filename1.Contains(variables.outfolder)))
                {
                    if (!movedalready)
                    {
                        Thread Go = new Thread(movework);
                        Go.Start();
                        movedalready = true;
                    }
                }
            }
            catch (SystemException ex)
            {
                Console.WriteLine("Nand Initialization Failed: {0}", ex.GetType().ToString());
                Console.WriteLine("The dump may be incomplete or corrupt");
                if (variables.debugme) Console.WriteLine(ex.ToString());
                Console.WriteLine("");
                updateProgress(progressBar.Minimum);
                return;
            }
        }

        string load_ecc()
        {
            if (Path.GetExtension(variables.filename1) == ".bin")
            {
                variables.tempfile = variables.filename1;
            }

            if (xPanel.getRgh3Checked())
            {
                string mhz = "";
                if (xPanel.getRgh3Mhz() == 10) mhz = "_10";

                switch (variables.ctyp.ID)
                {
                    case 1:
                        variables.filename1 = Path.Combine(variables.pathforit, "common", "ECC", variables.RGH3_trinity + ".ecc");
                        break;
                    case 2:
                        variables.filename1 = Path.Combine(variables.pathforit, "common", "ECC", variables.RGH3_falcon + mhz + ".ecc");
                        break;
                    case 3:
                    case 4:
                    case 5:
                        variables.filename1 = Path.Combine(variables.pathforit, "common", "ECC", variables.RGH3_jasper + mhz + ".ecc");
                        break;
                    case 6:
                    case 7:
                        variables.filename1 = Path.Combine(variables.pathforit, "common", "ECC", variables.RGH3_jasperBB + mhz + ".ecc");
                        break;
                    case 8:
                    case 9:
                        variables.filename1 = Path.Combine(variables.pathforit, "common", "ECC", variables.RGH3_trinity + ".ecc");
                        break;
                    case 10:
                        variables.filename1 = Path.Combine(variables.pathforit, "common", "ECC", variables.RGH3_corona + ".ecc");
                        break;
                    case 11:
                        variables.filename1 = Path.Combine(variables.pathforit, "common", "ECC", variables.RGH3_corona4GB + ".ecc");
                        break;
                    default:
                        return "";
                }
            }
            else
            {
                string wb = "";
                string smcp = "";
                string cr4 = "";
                if (xPanel.getWBChecked() > 0) wb = "_WB";
                if (xPanel.getSMCPChecked()) smcp = "_SMC+";
                else if (xPanel.getCR4Checked()) cr4 = "_CR4";

                switch (variables.ctyp.ID)
                {
                    case 1:
                        variables.filename1 = Path.Combine(variables.pathforit, "common", "ECC", variables.RGX_trinity + cr4 + smcp + ".ecc");
                        break;
                    case 2:
                        variables.filename1 = Path.Combine(variables.pathforit, "common", "ECC", variables.RGX_falcon + cr4 + smcp + ".ecc");
                        break;
                    case 3:
                        variables.filename1 = Path.Combine(variables.pathforit, "common", "ECC", variables.RGX_falcon + cr4 + smcp + ".ecc"); // Use Falcon
                        Console.WriteLine("Using Falcon type for Zephyr");
                        break;
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        variables.filename1 = Path.Combine(variables.pathforit, "common", "ECC", variables.RGX_jasper + cr4 + smcp + ".ecc");
                        break;
                    case 8:
                        variables.filename1 = Path.Combine(variables.pathforit, "common", "ECC", variables.RGX_xenon + ".ecc"); // No CR4 or SMC+
                        break;
                    case 9:
                        variables.filename1 = Path.Combine(variables.pathforit, "common", "ECC", variables.RGX_falcon + cr4 + smcp + ".ecc");
                        break;
                    case 10:
                        variables.filename1 = Path.Combine(variables.pathforit, "common", "ECC", variables.RGX_corona + wb + cr4 + smcp + ".ecc");
                        break;
                    case 11:
                        variables.filename1 = Path.Combine(variables.pathforit, "common", "ECC", variables.RGX_corona4GB + wb + cr4 + smcp + ".ecc");
                        break;
                    default:
                        return "";
                }
            }
            txtFilePath1.Text = variables.filename1;
            return variables.filename1;
        }

        void createecc_v2()
        {
            //Thread.CurrentThread.Join();

            if (xPanel.getRbtnRetailChecked()) Console.WriteLine("You are creating an ecc image and you have selected {0}!", variables.ttyp);
            else if (xPanel.getRbtnJtagChecked()) Console.WriteLine("You are creating an ecc image and you have selected {0}!", variables.ttyp);
            //savedir();

            if (File.Exists(variables.filename1))
            {
                if (variables.debugme) Console.WriteLine("Filename1 = {0}", variables.filename1);
                if (Path.GetExtension(variables.filename1) == ".bin")
                {
                    variables.tempfile = variables.filename1;
                    progressBar.Value = progressBar.Minimum;
                    int result = 0;
                    try
                    {
                        bool sts = objAlphaPattern.IsMatch(txtCPUKey.Text);

                        ECC ecc = new ECC();
                        result = ecc.creatergh2ecc(variables.filename1, variables.outfolder, ref this.progressBar, txtCPUKey.Text);
                        /*
                        if (comboRGH.SelectedIndex == 0)
                        {
                            result = Nand.createeccimage(variables.filename1, variables.outfolder, ref this.progressBar1);
                        }
                        else
                        {
                            Console.WriteLine("Constructing an rgh2 ecc image");
                            if (sts) result = ECC.creatergh2(variables.filename1, variables.outfolder, ref this.progressBar1, cpukeytext.Text);
                            else result = ECC.creatergh2(variables.filename1, variables.outfolder, ref this.progressBar1);
                        }
                        */
                    }
                    catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
                    if (result == 1)
                    {
                        variables.filename1 = Path.Combine(variables.outfolder, "glitch.ecc");
                        txtFilePath1.Text = variables.filename1;
                    }
                    else if (result == 5)
                    {
                        progressBar.Value = progressBar.Maximum;
                    }
                    else
                    {
                        Console.WriteLine("Failed to create ecc image");
                        Console.WriteLine("");
                    }
                }
            }
        }

        void createxell()
        {
            if (String.IsNullOrWhiteSpace(variables.filename1))
            {
                loadfile(ref variables.filename1, ref this.txtFilePath1, true);
                if (String.IsNullOrWhiteSpace(variables.filename1))
                {
                    MessageBox.Show("No file was selected!", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (variables.ctyp.ID == -1) variables.ctyp = callconsoletypes(ConsoleTypes.Selected.JTAG);
            if (variables.ctyp.ID == -1) return;
            if (File.Exists(variables.filename1))
            {
                variables.tempfile = variables.filename1;
                if (variables.debugme) Console.WriteLine("Filename1 = {0}", variables.filename1);
                if (Path.GetExtension(variables.filename1) == ".bin")
                {
                    byte[] Keyraw = Nand.Nand.getrawkv(variables.filename1);
                    long size1 = 0;
                    string xellfile;
                    if (variables.ctyp.ID == 1) return;
                    else if (variables.ctyp.ID == 8) xellfile = "xenon.bin";
                    else if (variables.ctyp.ID == 2)
                    {
                        if (xPanel.getAudClampChecked()) xellfile = "falcon_aud_clamp.bin";
                        else xellfile = "falcon.bin";
                    }
                    else if (variables.ctyp.ID == 3)
                    {
                        if (xPanel.getAudClampChecked()) xellfile = "zephyr_aud_clamp.bin";
                        else xellfile = "zephyr.bin";
                    }
                    else if (variables.ctyp.ID == 4 || variables.ctyp.ID == 5)
                    {
                        if (xPanel.getAudClampChecked()) xellfile = "jasper_aud_clamp.bin";
                        else xellfile = "jasper.bin";
                    }
                    else if (variables.ctyp.ID == 6 || variables.ctyp.ID == 7)
                    {
                        if (xPanel.getAudClampChecked()) xellfile = "jasper_bb_aud_clamp.bin";
                        else xellfile = "jasper_bb.bin";
                    }
                    else return;
                    if (variables.debugme) Console.WriteLine(xellfile);


                    byte[] xellous = Oper.openfile(Path.Combine(variables.pathforit, "common\\xell\\" + xellfile), ref size1, 0);
                    if (variables.debugme) Console.WriteLine("{0} file loaded successfully", xellfile);
                    if (variables.debugme) Console.WriteLine("{0:X} | {1:X}", xellous.Length, Keyraw.Length);

                    Buffer.BlockCopy(Keyraw, 0, xellous, 0x4200, 0x4200);

                    if (xPanel.getRJtagChecked())
                    {
                        int layout = 0;
                        if (variables.ctyp.ID == 6 || variables.ctyp.ID == 7) layout = 2;
                        else if (variables.ctyp.ID == 4 || variables.ctyp.ID == 5) layout = 1;
                        byte[] SMC;
                        byte[] smc_len = new byte[4], smc_start = new byte[4];
                        Buffer.BlockCopy(xellous, 0x78, smc_len, 0, 4);
                        Buffer.BlockCopy(xellous, 0x7C, smc_start, 0, 4);
                        SMC = new byte[Oper.ByteArrayToInt(smc_len)];
                        Buffer.BlockCopy(Nand.Nand.unecc(xellous), Oper.ByteArrayToInt(smc_start), SMC, 0, Oper.ByteArrayToInt(smc_len));
                        SMC = Nand.Nand.addecc_v2(Nand.Nand.encrypt_SMC(Nand.Nand.patch_SMC(Nand.Nand.decrypt_SMC(SMC))), true, 0, layout);
                        Buffer.BlockCopy(SMC, 0, xellous, (Oper.ByteArrayToInt(smc_start) / 0x200) * 0x210, (Oper.ByteArrayToInt(smc_len) / 0x200) * 0x210);
                    }

                    variables.filename1 = Path.Combine(variables.outfolder, xellfile);
                    if (variables.debugme) Console.WriteLine(variables.filename1);
                    Oper.savefile(xellous, variables.filename1);
                    if (variables.debugme) Console.WriteLine("Saved Successfully");
                    txtFilePath1.Text = variables.filename1;
                    Console.WriteLine("XeLL file created successfully {0}", xellfile);
                    Console.WriteLine("");
                }
            }
        }

        public void extractFilesFromNand()
        {
            if (!nand.ok)
            {
                MessageBox.Show("No nand loaded in source", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Console.WriteLine("Extracting Files...");
            string tmpout = "";
            if (variables.modder && variables.custname != "")
            {
                tmpout = Path.Combine(getCurrentWorkingFolder(), "Extracts-" + variables.custname);
            }
            else
            {
                tmpout = Path.Combine(getCurrentWorkingFolder(), "Extracts-" + nand.ki.serial);
            }

            if (Directory.Exists(tmpout) == false)
            {
                Directory.CreateDirectory(tmpout);
            }

            Console.WriteLine("Saving SMC_en.bin");
            Oper.savefile(Nand.Nand.encrypt_SMC(nand._smc), Path.Combine(tmpout, "SMC_en.bin"));
            Console.WriteLine("Saving SMC_dec.bin");
            Oper.savefile(nand._smc, Path.Combine(tmpout, "SMC_dec.bin"));
            Console.WriteLine("Saving KV_en.bin");
            Oper.savefile(nand._rawkv, Path.Combine(tmpout, "KV_en.bin"));

            if (!String.IsNullOrEmpty(nand._cpukey))
            {
                Console.WriteLine("Saving KV_dec.bin");
                Oper.savefile(Nand.Nand.decryptkv(nand._rawkv, Oper.StringToByteArray(nand._cpukey)), Path.Combine(tmpout, "KV_dec.bin"));
            }
            Console.WriteLine("Saving smc_config.bin");
            nand.getsmcconfig();
            Oper.savefile(nand._smc_config, Path.Combine(tmpout, "smc_config.bin"));

            if (variables.ctyp.ID == 1 || variables.ctyp.ID == 10 || variables.ctyp.ID == 11)
            {
                byte[] t;
                Console.WriteLine("Working...");
                byte[] fcrt = nand.exctractFSfile("fcrt.bin");
                if (fcrt != null)
                {
                    Console.WriteLine("Saving fcrt_en.bin");
                    Oper.savefile(fcrt, Path.Combine(tmpout, "fcrt_en.bin"));
                    byte[] fcrt_dec;
                    if (Nand.Nand.decrypt_fcrt(fcrt, Oper.StringToByteArray(nand._cpukey), out fcrt_dec))
                    {
                        Console.WriteLine("Saving fcrt_dec.bin");
                        File.WriteAllBytes(Path.Combine(tmpout, "fcrt_dec.bin"), fcrt_dec);
                    }
                    t = responses(fcrt, Oper.StringToByteArray(nand._cpukey), nand.ki.dvdkey);

                    if (t != null)
                    {

                        Console.WriteLine("Saving C-R.bin");
                        File.WriteAllBytes(Path.Combine(tmpout, "C-R.bin"), t);
                        Console.WriteLine("Saving key.bin");
                        File.WriteAllBytes(Path.Combine(tmpout, "key.bin"), Oper.StringToByteArray(nand.ki.dvdkey));
                    }
                    else Console.WriteLine("Failed to create C-R.bin");
                }
                else Console.WriteLine("Failed to find fcrt.bin");
            }
            Console.WriteLine("Location: {0}", tmpout);
            Console.WriteLine("Done");
            Console.WriteLine("");
        }
        public static byte[] responses(byte[] fcrt, byte[] cpukey, string dvdkey = "")
        {
            byte[] fcrt_dec;
            if (Nand.Nand.decrypt_fcrt(fcrt, cpukey, out fcrt_dec))
            {
                byte[] rfct = new byte[0x1F6 * 0x13];
                Oper.removeByteArray(ref fcrt_dec, 0, 0x140);
                Random rnd = new Random();
                int[] randomNumbers = Enumerable.Range(0, 502).OrderBy(i => rnd.Next()).ToArray();
                int counter = 0;
                while (counter < (rfct.Length / 0x13))
                {
                    byte[] cr = Oper.returnportion(fcrt_dec, counter * 0x20, 0x20);
                    Oper.removeByteArray(ref cr, 2, 0x10 - 3);
                    Buffer.BlockCopy(cr, 0, rfct, randomNumbers[counter] * cr.Length, cr.Length);
                    counter++;
                }
                for (int i = 0; i < 0x1f6; i++)
                {
                    if (Oper.allsame(Oper.returnportion(fcrt_dec, i * 0x20, 0x10), 0x00)) continue;
                    for (int j = i + 1; j < 0x1f6; j++)
                    {
                        if (Oper.allsame(Oper.returnportion(fcrt_dec, j * 0x20, 0x10), 0x00)) continue;
                        if (rfct[i * 0x13] == rfct[j * 0x13] &&
                            rfct[(i * 0x13) + 1] == rfct[(j * 0x13) + 1] &&
                            rfct[(i * 0x13) + 2] == rfct[(j * 0x13) + 2])
                        {
                            if (variables.debugme) Console.WriteLine("You're FUCKED");
                        }
                    }
                }
                return encryptFirmware(rfct, variables.xor, rfct.Length);
            }
            return null;
        }
        private static byte swapBits(byte chunk, int[] bits)
        {
            byte result = 0;
            //var bit = (b & (1 << bits[i])) != 0;
            int i;
            for (i = 0; i < 8; i++)
            {
                byte bit = (byte)((chunk & (1 << bits[i])) >> bits[i]);
                result = (byte)((result << 1) | bit);
            }
            return result;
        }
        private static byte[] encryptFirmware(byte[] inputBuffer, byte[] XorList, int size)
        {
            int[] encryptBits = { 3, 2, 7, 6, 1, 0, 5, 4 };
            int i;
            byte bt, done;
            byte[] outputBuffer = new byte[size];
            for (i = 0; i < size; i++)
            {
                bt = (byte)(inputBuffer[i] ^ XorList[i]);
                done = swapBits(bt, encryptBits);
                outputBuffer[i] = done;
            }
            return outputBuffer;
        }

        public void createDonor(string con, string hack, string smc, string cpuk, string kvPath, string fcrtPath, string smcConfPath, int ldv, bool nofcrt)
        {
            newSession(true);

            Console.WriteLine("=======================");
            Console.WriteLine("Starting Donor Nand Creation");

            // Set Clean SMC if needed
            if (hack == "Retail" || hack == "Glitch" || hack == "DEVGL") xPanel.setCleanSMCChecked(true);
            if (hack == "Glitch2" || hack == "Glitch2m")
            {
                if (smc == "Glitch") xPanel.setCleanSMCChecked(true);
            }

            variables.cpkey = cpuk; // Copy CPU Key
            txtCPUKey.BeginInvoke(new Action(() => txtCPUKey.Text = cpuk));
            variables.highldv = ldv; // Copy LDV
            variables.changeldv = 2; // Enable Custom LDV

            Thread donorThread = new Thread(() =>
            {
                try
                {
                    Console.WriteLine("Copying Files Into Place...");

                    if (File.Exists(variables.xePath + "nanddump.bin")) File.Delete(variables.xePath + "nanddump.bin"); // Just in case

                    // Copy KV
                    if (kvPath == "donor")
                    {
                        string kv;
                        if (con.Contains("Trinity") || con.Contains("Corona")) kv = "slim_nofcrt";
                        else if (con.Contains("Xenon")) kv = "phat_t1";
                        else kv = "phat_t2";
                        File.Copy(Path.Combine(variables.donorPath, kv + ".bin"), variables.xePath + "KV.bin", true);
                    }
                    else File.Copy(kvPath, variables.xePath + "KV.bin", true);
                    Console.WriteLine("Copied KV.bin");

                    // Copy FCRT and set nofcrt if needed
                    if (fcrtPath != "unneeded")
                    {
                        if (fcrtPath == "donor") File.Copy(Path.Combine(variables.donorPath, "fcrt.bin"), variables.xePath + "fcrt.bin", true);
                        else File.Copy(fcrtPath, variables.xePath + "fcrt.bin", true);
                        xPanel.setNoFcrt(nofcrt);
                        Console.WriteLine("Copied fcrt.bin");
                    }
                    else
                    {
                        if (File.Exists(variables.xePath + "fcrt.bin")) File.Delete(variables.xePath + "fcrt.bin");
                        xPanel.setNoFcrt(false);
                    }

                    // Copy SMC - only needed for RGH3
                    if ((hack == "Glitch2" || hack == "Glitch2m") && smc == "RGH3")
                    {
                        if (con.Contains("Corona")) File.Copy(variables.xePath + "CORONA_CLEAN.bin", variables.xePath + "SMC.bin", true);
                        else if (con.Contains("Trinity")) File.Copy(variables.xePath + "TRINITY_CLEAN.bin", variables.xePath + "SMC.bin", true);
                        else if (con.Contains("Jasper")) File.Copy(variables.xePath + "JASPER_CLEAN.bin", variables.xePath + "SMC.bin", true);
                        else if (con.Contains("Falcon")) File.Copy(variables.xePath + "FALCON_CLEAN.bin", variables.xePath + "SMC.bin", true);
                        else if (con.Contains("Zephyr")) File.Copy(variables.xePath + "ZEPHYR_CLEAN.bin", variables.xePath + "SMC.bin", true); // Just in case we ever re-use this code for non RGH3
                        else if (con.Contains("Xenon")) File.Copy(variables.xePath + "XENON_CLEAN.bin", variables.xePath + "SMC.bin", true); // Just in case we ever re-use this code for non RGH3
                        Console.WriteLine("Copied SMC.bin");
                    }

                    // Copy SMC Config
                    if (smcConfPath == "donor")
                    {
                        string smcConfig;

                        // Catch all types
                        if (con.Contains("Corona")) smcConfig = "Corona";
                        else if (con.Contains("Jasper")) smcConfig = "Jasper";
                        else if (con.Contains("Trinity")) smcConfig = "Trinity";
                        else smcConfig = con;

                        File.Copy(Path.Combine(variables.donorPath, "smc_config", smcConfig + ".bin"), variables.xePath + "smc_config.bin", true);
                    }
                    else File.Copy(smcConfPath, variables.xePath + "smc_config.bin", true);
                    Console.WriteLine("Copied smc_config.bin");

                    // Launch XeBuild
                    Thread.Sleep(1000);
                    nand = new Nand.PrivateN();
                    nand._cpukey = txtCPUKey.Text;
                    string kvfile = Path.Combine(variables.pathforit, @"xebuild\data\kv.bin");
                    if (File.Exists(kvfile))
                    {
                        nand._rawkv = File.ReadAllBytes(kvfile);
                        nand.updatekvval();
                    }
                    xPanel.createxebuild_v2(true, nand, true);
                }
                catch
                {
                    Console.WriteLine("Donor Nand Creation Failed");
                    Console.WriteLine("");
                    return;
                }
            });
            donorThread.Start();
        }

        private void createDonorAdvanced()
        {
            newSession(true);
            nand = new Nand.PrivateN();
            nand._cpukey = txtCPUKey.Text;
            string kvfile = Path.Combine(variables.pathforit, @"xebuild\data\kv.bin");
            if (File.Exists(kvfile))
            {
                nand._rawkv = File.ReadAllBytes(kvfile);
                nand.updatekvval();

            }
            ThreadStart starter = delegate { xPanel.createxebuild_v2(true, nand, false); };
            new Thread(starter).Start();
        }

        public void startKvDecrypt(string path, string key)
        {
            Thread decryptThread = new Thread(() =>
            {
                try
                {
                    if (File.Exists(path))
                    {
                        Console.WriteLine("Decrypting Keyvault...");
                        byte[] data = Nand.Nand.decryptkv(File.ReadAllBytes(path), Oper.StringToByteArray(key));
                        Thread.Sleep(250);
                        if (data != null)
                        {
                            string outPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path).Replace("_en", "") + "_dec" + Path.GetExtension(path));
                            Oper.savefile(data, outPath);
                            Console.WriteLine("Decrypted Successfully: " + outPath);
                        }
                        else Console.WriteLine("Decrypt Failed");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Decrypt Failed");
                    Console.WriteLine(ex.Message);
                }
            });
            decryptThread.Start();
        }
        #endregion

        #region Forms

        void loadfile(ref string filename, ref TextBox tx, bool erase = false)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Nand files (*.bin;*.ecc)|*.bin;*.ecc|HEX files (*.hex)|*.hex|All files (*.*)|*.*";
            openFileDialog1.Title = "Select Nand File";
            if (variables.FindFolder != "")
            {
                openFileDialog1.InitialDirectory = variables.FindFolder;
                variables.FindFolder = "";
            }
            else openFileDialog1.InitialDirectory = variables.currentdir;
            openFileDialog1.RestoreDirectory = false;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (erase) erasevariables();
                filename = openFileDialog1.FileName;
                if (!String.IsNullOrWhiteSpace(filename)) tx.Text = filename;
            }
            variables.currentdir = filename;
        }

        consoles callconsoletypes(ConsoleTypes.Selected selec, bool twomb = false, bool full = false)
        {
            ConsoleTypes myNewForm = new ConsoleTypes();
            myNewForm.sel = selec;
            myNewForm.twombread = twomb;
            myNewForm.sfulldump = full;
            myNewForm.ShowDialog();
            if (myNewForm.DialogResult == DialogResult.Cancel) return (variables.cunts[0]);
            if (myNewForm.heResult().ID == -1) return variables.cunts[0];
            variables.fulldump = myNewForm.fulldump();
            variables.twombread = myNewForm.twombdump();
            if (variables.debugme) Console.WriteLine("fulldump variable = {0}", variables.fulldump);
            //if (variables.debugme) Console.WriteLine(myNewForm.heResult());
            xPanel.setMBname(myNewForm.heResult().Text);
            return (myNewForm.heResult());
        }

        CpuKeyDB mycpukeydb;
        void callcpukeydb()
        {
            if (Application.OpenForms.OfType<CpuKeyDB>().Any())
            {
                mycpukeydb.WindowState = FormWindowState.Normal;
                mycpukeydb.Activate();
            }
            else
            {
                mycpukeydb = new CpuKeyDB();
                mycpukeydb.Show();
                mycpukeydb.Location = new Point(Location.X + (Width - mycpukeydb.Width) / 2, Location.Y + (Height - mycpukeydb.Height) / 2);
                mycpukeydb.FormClosed += new FormClosedEventHandler(mycpukeydb_FormClosed);
            }
        }

        // Deprecated
        //string callxsvf()
        //{
        //    xsvf_types mForm = new xsvf_types();
        //    mForm.ShowDialog();
        //    if (mForm.DialogResult != DialogResult.OK) return ("");
        //    if (mForm.heResult() == -1) return "";
        //    if (variables.debugme) Console.WriteLine(mForm.heResult());
        //    bool demon = mForm.deResult();
        //    if (variables.debugme) Console.WriteLine("demon {0}", demon);
        //    if (demon)
        //    {
        //        if (variables.debugme) Console.WriteLine(variables.demon_xsvf[mForm.heResult() - 1]);
        //        return (variables.demon_xsvf[mForm.heResult() - 1]);
        //    }
        //    else
        //    {
        //        if (variables.debugme) Console.WriteLine(variables.xsvf[mForm.heResult() - 1]);
        //        return (variables.xsvf[mForm.heResult() - 1]);
        //    }
        //}

        void calldrives(string filename = "", Panels.LDrivesInfo.Function f = Panels.LDrivesInfo.Function.ReadWrite)
        {
            ldInfo.setup(f);
            pnlInfo.Controls.Clear();
            pnlInfo.Controls.Add(ldInfo);
            if (listInfo.Contains(ldInfo)) listInfo.Remove(ldInfo);
            listInfo.Add(ldInfo);
            //Forms.LDrives ld = new Forms.LDrives(filename, f, numericiter);
            //ld.Show();
            //ld.FormClosed += ld_FormClosed;
        }

        void ld_FormClosed(object sender, FormClosedEventArgs e)
        {
            string getfilename = "";
            getfilename = Forms.LDrives.filename;
            if (!string.IsNullOrEmpty(getfilename) && Forms.LDrives.fu != Forms.LDrives.Function.Write)
            {
                txtFilePath1.Text = getfilename;
                variables.filename1 = getfilename;
            }
            if (Forms.LDrives.fu == Forms.LDrives.Function.Read && Forms.LDrives.files.Count > 0)
            {
                int i = 0;
                int cnt = Forms.LDrives.files.Count;
                foreach (string filename in Forms.LDrives.files)
                {
                    if (i == 0)
                    {
                        txtFilePath1.Text = filename;
                        variables.filename1 = filename;
                        nand_init();
                    }
                    else if (i == 1)
                    {
                        txtFilePath2.Text = filename;
                        variables.filename2 = filename;
                        new Thread(comparenands).Start();
                    }
                    i++;
                }
            }
            //nand_init();
        }

        void mycpukeydb_FormClosed(object sender, FormClosedEventArgs e)
        {
            txtCPUKey.Text = variables.cpkey;
        }

        #endregion

        #endregion

        #region User Input

        void updateProgress(int progress)
        {
            progressBar.BeginInvoke((Action)(() => progressBar.Value = progress));
        }

        void updateBlocks(String progress)
        {
            txtBlocks.BeginInvoke((Action)(() => txtBlocks.Text = progress));
        }

        #region Menu Bar

        #region JRunner

        Form shade;
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shade = new Form();
            shade.ControlBox = false;
            shade.FormBorderStyle = FormBorderStyle.None;
            shade.Text = "";
            shade.Size = ClientSize;
            shade.BackColor = Color.Black;
            shade.Opacity = 0.4f;
            shade.ShowInTaskbar = false;
            shade.Show();
            shade.Location = PointToScreen(Point.Empty);

            this.Text = "Press escape to return to work";
            Form about = new Forms.About();
            about.Show();
            about.Location = new Point(Location.X + (Width - about.Width) / 2, Location.Y + (Height - about.Height) / 2);
        }

        public void killShade()
        {
            shade.Dispose();
            this.Text = "J-Runner with Extras";
        }

        private void reportIssueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Issues issues = new Issues();
            issues.ShowDialog();
        }

        private void changelogToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            TextViewer tv = new TextViewer();
            tv.Show();
            tv.Location = new Point(Location.X + (Width - tv.Width) / 2, Location.Y + (Height - tv.Height) / 2);
            tv.LoadFile("Changelog.txt");
        }

        private void shortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("J-Runner with Extras has several shortcut keybinds:\n\n" +
                "Operations:\n" +
                "Esc - Cancel active task (if possible)\n" +
                "F1 - New Session\n" +
                "F9 - Try CPU Key against database\n" +
                "CTRL+F1 - Restart\n" +
                "ALT+F4 - Exit\n\n" +
                "Device:\n" +
                "F2 - Get console type\n" +
                "F3 - Program Timing File\n" +
                "F4 - Custom Nand/Timing File Functions\n" +
                "F5 - Corona 4GB Read/Write\n" +
                "F12 - Send Timing File via JTAG (if enabled)\n\n" +
                "Interface:\n" +
                "F6 - Timing Assistant\n" +
                "CTRL+H - Shortcuts",
                "Shortcuts", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        #endregion

        #region Tools

        POST ps;
        private void pOSTMonitorRATERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<POST>().Any())
            {
                ps.WindowState = FormWindowState.Normal;
                ps.Activate();
            }
            else
            {
                ps = new POST();
                ps.Show();
                ps.Location = new Point(Location.X + (Width - ps.Width) / 2, Location.Y + (Height - ps.Height) / 2);
            }
        }

        Comport cm;
        private void btnCOM_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Comport>().Any())
            {
                cm.WindowState = FormWindowState.Normal;
                cm.Activate();
            }
            else
            {
                cm = new Comport();
                cm.Show();
                cm.Location = new Point(Location.X + (Width - cm.Width) / 2, Location.Y + (Height - cm.Height) / 2);
            }
        }

        private void soundEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Application.OpenForms.OfType<SoundEditor>().Any())
            {
                SoundEditor se = new SoundEditor();
                se.ShowDialog();
            }
        }

        private void rescanDevicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar.Value = progressBar.Minimum;
            deviceinit();
            Thread.Sleep(100);
            if (listInfo.Contains(ldInfo)) ldInfo.refreshDrives(true);
            else progressBar.Value = progressBar.Maximum;
        }

        private void mTXUSBFirmwareUtilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("mtx-utility");
            if (processes.Length == 0)
            {
                try
                {
                    ProcessStartInfo mtxUtility = new ProcessStartInfo("mtx-utility.exe");
                    mtxUtility.WorkingDirectory = Path.Combine(Environment.CurrentDirectory, "common\\mtx-utility");
                    mtxUtility.UseShellExecute = true;
                    Process.Start(mtxUtility);
                }
                catch { }
            }
            else
            {
                MessageBox.Show("The utility is already running", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        XB1HDD xb1hdd;
        private void xboxOneHDDToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<XB1HDD>().Any())
            {
                xb1hdd.WindowState = FormWindowState.Normal;
                xb1hdd.Activate();
            }
            else
            {
                xb1hdd = new XB1HDD();
                xb1hdd.Show();
                xb1hdd.Location = new Point(Location.X + (Width - xb1hdd.Width) / 2, Location.Y + (Height - xb1hdd.Height) / 2);
            }
        }

        Timing timing;
        private void timingAssistantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timingAssistant();
        }

        public void timingAssistant()
        {
            if (Application.OpenForms.OfType<Timing>().Any())
            {
                timing.Activate();
            }
            else
            {
                timing = new Timing();
                timing.Show();
                timing.Location = new Point(Location.X + (Width - timing.Width) / 2 - 175, Location.Y + 60);
            }
        }

        Nand.CB_Fuse cb;
        private void cBFuseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Nand.CB_Fuse>().Any())
            {
                cb.Activate();
            }
            else
            {
                cb = new Nand.CB_Fuse();
                cb.Show();
                cb.Location = new Point(Location.X + (Width - cb.Width) / 2, Location.Y + (Height - cb.Height) / 2);
            }
        }

        #endregion

        #region Nand

        private void extractFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            extractFilesFromNand();
        }

        CreateDonorNand cdonor;
        private void createDonorNandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                createDonorAdvanced();
                return;
            }

            if (variables.ctyp.ID == -1) variables.ctyp = callconsoletypes(ConsoleTypes.Selected.All);
            if (variables.ctyp.ID == -1) return;
            if (Application.OpenForms.OfType<CreateDonorNand>().Any())
            {
                cdonor.WindowState = FormWindowState.Normal;
                cdonor.Activate();
            }
            else
            {
                cdonor = new CreateDonorNand();
                cdonor.Show();
                cdonor.Location = new Point(Location.X + 14, Location.Y + (Height - cdonor.Height) - 14);
            }
        }

        private void decryptKeyvaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeyvaultDecrypter dk = new KeyvaultDecrypter();
            dk.ShowDialog();
        }

        private void sMCConfigViewerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SMCConfig smcedit = new SMCConfig();
            smcedit.ShowDialog();
        }

        private void patchNandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!nand.ok)
            {
                MessageBox.Show("No nand loaded in source", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            variables.cpkey = txtCPUKey.Text;
            patch patchform = new patch();
            patchform.frm1 = this;
            patchform.ShowDialog();
        }

        private void changeLDVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xeBuildOptions xbo = new xeBuildOptions();
            xbo.disableAdv();
            xbo.ShowDialog();
        }

        #endregion

        #region Advanced

        private void customNandProCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nandcustom();
        }

        private void corona4GBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calldrives();
        }

        private void CustomXeBuildMenuItem_Click(object sender, EventArgs e)
        {
            CustomXebuild CX = new CustomXebuild();
            CX.ShowDialog();
            if (CX.DialogResult == DialogResult.Cancel) return;
            else if (CX.DialogResult == DialogResult.OK)
            {
                Classes.xebuild xe = new Classes.xebuild();
                xe.xeExit += xPanel.xe_xeExit;
                ThreadStart starter = delegate { xe.build(CX.getString()); };
                Thread thr = new Thread(starter);
                thr.IsBackground = true;
                thr.Start();
            }
        }

        private void writeFusionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThreadStart starter = delegate { writefusion(); };
            new Thread(starter).Start();
        }

        HexEdit.HexViewer hv;
        private void toolStripHexEditor_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<HexEdit.HexViewer>().Any())
            {
                hv.WindowState = FormWindowState.Normal;
                hv.Activate();
            }
            else
            {
                hv = new HexEdit.HexViewer(txtFilePath1.Text);
                hv.ShowDialog();
            }
        }

        CPUKeyGen cpu;
        private void cPUKeyToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<CPUKeyGen>().Any())
            {
                cpu.WindowState = FormWindowState.Normal;
                cpu.Activate();
            }
            else
            {
                cpu = new CPUKeyGen();
                cpu.Show();
                cpu.Location = new Point(Location.X + (Width - cpu.Width) / 2, Location.Y + 155);
            }
        }

        private void checkSecdataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(variables.filename1))
            {
                MessageBox.Show("No nand loaded in source", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (nand == null || !nand.ok) return;
            if (String.IsNullOrWhiteSpace(txtCPUKey.Text))
            {
                MessageBox.Show("No CPU Key entered", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] secdata = nand.exctractFSfile("secdata.bin");
            Nand.Nand.DecryptSecData(secdata, Oper.StringToByteArray(txtCPUKey.Text));
        }

        private void xValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.XValue x = new Forms.XValue();
            x.ShowDialog();
        }

        //private void toolStripMenuItemVNand_Click(object sender, EventArgs e)
        //{
        //    Nand.VNandForm f = new Nand.VNandForm();
        //    if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        if (f.filename == null)
        //        {
        //            MessageBox.Show("You did not select anything", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        vnand = new Nand.VNand(f.filename, f.console, f.flashconfig, f.BadBlocks);
        //        vnand.create();
        //        usingVNand = true;
        //    }
        //}

        //Pirs.STFS p;
        //private void pirsToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (Application.OpenForms.OfType<Pirs.STFS>().Any())
        //    {
        //        p.WindowState = FormWindowState.Normal;
        //        p.Activate();
        //    }
        //    else
        //    {
        //        p = new Pirs.STFS(txtFilePath1.Text, txtFilePath2.Text);
        //        p.Show();
        //        p.Location = new Point(Location.X + (Width - p.Width) / 2, Location.Y + (Height - p.Height) / 2);
        //    }
        //}

        #endregion

        #region xFlasher

        private void installDriversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!xflasher.osCheck()) return;

            Thread xFlasherDrivers = new Thread(() =>
            {
                try
                {
                    ProcessStartInfo xflasherdrivers = new ProcessStartInfo("common\\drivers\\xFlasher-Drivers.exe");
                    xflasherdrivers.WorkingDirectory = Environment.CurrentDirectory;
                    xflasherdrivers.UseShellExecute = true;
                    xflasherdrivers.Verb = "runas";
                    Process.Start(xflasherdrivers);
                }
                catch
                {
                    MessageBox.Show("Could not launch driver installer for some reason!\n\nPlease launch it manually from the common\\drivers folder", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
            xFlasherDrivers.Start();
        }

        private void flashOpenXeniumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (device == DEVICE.XFLASHER_SPI)
            {
                MessageBox.Show("Connect OpenXenium and press OK", "Connect Device", MessageBoxButtons.OK, MessageBoxIcon.Information);
                xflasher.flashSvf(variables.pathforit + @"\common\svf\openxenium.svf");
            }
            else
            {
                MessageBox.Show("This only works with xFlasher in SPI Mode!", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region NAND-X

        private void mtxUsbModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            variables.mtxUsbMode = mtxUsbModeToolStripMenuItem.Checked = !mtxUsbModeToolStripMenuItem.Checked;

            if (device == DEVICE.NAND_X)
            {
                if (variables.mtxUsbMode)
                {
                    nTools.setImage(Properties.Resources.mtx);
                }
                else
                {
                    nTools.setImage(Properties.Resources.NANDX);
                }
            }
        }

        #endregion

        #region JR-P

        private void powerOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nandx.PowerUp();
        }

        private void shutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nandx.PowerDown();
        }

        private void bootloaderModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nandx.Update();
        }

        private void logPostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThreadStart starter = delegate { nandx.log_post(); };
            new Thread(starter).Start();
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "HEX files (*.hex)|*.hex|All files (*.*)|*.*";
            openFileDialog1.Title = "Select HEX File";
            openFileDialog1.RestoreDirectory = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                variables.filename1 = openFileDialog1.FileName;
            }
            else return;
            if (variables.filename1 != null) this.txtFilePath1.Text = variables.filename1;
            variables.currentdir = variables.filename1;
            ThreadStart starter = delegate { HID.program(ref this.progressBar); };
            Thread start = new Thread(starter);
            start.IsBackground = true;
            start.Start();
        }

        #endregion

        #region DemoN

        private void powerOnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            demon.Power_On();
        }

        private void powerOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            demon.Power_Off();
        }

        private void toggleNANDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            demon.toggle();
        }

        private void connectToUARTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            demon_uart demon_uart_frm = new demon_uart();
            demon_uart_frm.Show();
        }

        private void getInvalidBlocksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> invalidblocks = new List<int>();
                demon.get_Invalid_Blocks(ref invalidblocks);
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
        }

        private void updateFwToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "(*.bin)|*.bin|All files (*.*)|*.*";
            openFileDialog1.Title = "Select Firmware File";
            openFileDialog1.RestoreDirectory = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                variables.filename1 = openFileDialog1.FileName;
            }
            else return;
            if (variables.filename1 != null) this.txtFilePath1.Text = variables.filename1;
            ThreadStart starter = delegate { demon.Update_DemoN(variables.filename1); };
            Thread start = new Thread(starter);
            start.Start();
        }

        #endregion

        #region Update

        private void updateAvailableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        #endregion

        #endregion

        #endregion

        #region Buttons

        #region Basic Buttons

        private void btnNewSession_Click(object sender, EventArgs e)
        {
            newSession();
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            Program.exit();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Settings mForm = new Settings();
            mForm.ShowDialog();
        }

        public string getCurrentWorkingFolder()
        {
            if (!String.IsNullOrWhiteSpace(nand.ki.serial))
            {
                if (variables.xefolder != null && variables.xefolder != "")
                {
                    if (Directory.Exists(variables.xefolder)) return (variables.xefolder);
                    else return variables.outfolder;
                }
                else if (Directory.Exists(Path.Combine(Directory.GetParent(variables.outfolder).FullName, nand.ki.serial)))
                {
                    return Path.Combine(Directory.GetParent(variables.outfolder).FullName, nand.ki.serial);
                }
                else return variables.outfolder;
            }
            else return variables.outfolder;
        }

        private void btnShowWorkingFolder_Click(object sender, EventArgs e)
        {
            Process.Start(getCurrentWorkingFolder());
        }

        private void showRootFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        private void showOutputFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(variables.outfolder);
        }

        void btnReadClick()
        {
            if (device == DEVICE.PICOFLASHER)
            {
                picoflasher.Read(nTools.getNumericIterations());
            }
            else if (device == DEVICE.XFLASHER_SPI)
            {
                xflasher.readNandAuto(0, nTools.getNumericIterations());
            }
            else if (device == DEVICE.XFLASHER_EMMC)
            {
                variables.ctyp = variables.cunts[11];
                xPanel.setMBname(variables.cunts[11].Text);
                getconsoletype(1);
            }
            else
            {
                if (variables.modder)
                {
                    custform CFrom = new JRunner.Forms.custform();

                    CFrom.ShowDialog();
                }
                getconsoletype(1);
            }
        }

        void btnCreateECCClick()
        {
            if (String.IsNullOrWhiteSpace(variables.filename1))
            {
                loadfile(ref variables.filename1, ref this.txtFilePath1, true);
                if (String.IsNullOrWhiteSpace(variables.filename1))
                {
                    MessageBox.Show("No file was selected!", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (xPanel.getRbtnJtagChecked())
            {
                createxell();
            }
            else if (xPanel.getRbtnGlitchChecked())
            {
                Thread thr = new Thread(createecc_v2);
                thr.IsBackground = true;
                thr.Start();
            }
            else if (xPanel.getRbtnGlitch2Checked() || xPanel.getRbtnGlitch2mChecked())
            {
                if (xPanel.getRgh3Checked() && (variables.ctyp.ID == 3 || variables.ctyp.ID == 8))
                {
                    MessageBox.Show("RGH3 is not supported on this board type", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (nand == null || !nand.ok) return;
                byte[] kv = new byte[0x4200];
                if (nand.noecc) kv = nand._rawkv;
                else
                {
                    FileStream infile = new FileStream(nand._filename, FileMode.Open, FileAccess.Read);
                    BinaryReader file = new BinaryReader(infile);
                    file.BaseStream.Seek(0x4200, SeekOrigin.Begin);
                    file.Read(kv, 0, 0x4200);
                    infile.Close();
                }
                if (String.IsNullOrWhiteSpace(load_ecc())) return;
                File.Copy(variables.filename1, Path.Combine(variables.outfolder, "glitch.ecc"), true);
                variables.filename1 = Path.Combine(variables.outfolder, "glitch.ecc");
                txtFilePath1.Text = variables.filename1;
                Nand.Nand.injectRawKV(variables.filename1, kv);
                Console.WriteLine("ECC created");
                Console.WriteLine("");
            }
            else
            {
                MessageBox.Show("Impossible to create an ECC for this hack type", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void btnProgramCRClick()
        {
            pnlInfo.Controls.Clear();
            pnlInfo.Controls.Add(xsvfInfo);
            if (listInfo.Contains(xsvfInfo)) listInfo.Remove(xsvfInfo);
            listInfo.Add(xsvfInfo);
            pnlTools.Enabled = false;
            xsvfInfo.boardCheck(variables.boardtype);
        }

        public void openXsvfInfo()
        {
            pnlInfo.Controls.Clear();
            pnlInfo.Controls.Add(xsvfInfo);
            if (listInfo.Contains(xsvfInfo)) listInfo.Remove(xsvfInfo);
            listInfo.Add(xsvfInfo);
            pnlTools.Enabled = false;
        }

        void btnWriteECCClick()
        {
            if (device == DEVICE.PICOFLASHER)
            {
                picoflasher.Write(nTools.getbtnWriteECC().Contains("XeLL") ? 0 : 1, 0, 0, true);
            }
            else if (device == DEVICE.XFLASHER_SPI)
            {
                if (nTools.getbtnWriteECC().Contains("XeLL"))
                {
                    xflasher.writeXeLLAuto();
                }
                else
                {
                    xflasher.writeEccAuto();
                }
            }
            else if (nTools.getbtnWriteECC().Contains("XeLL"))
            {
                if (variables.debugme) Console.WriteLine("xell");
                ThreadStart starter = delegate { writexell(); };
                new Thread(starter).Start();
            }
            else if (device == DEVICE.XFLASHER_EMMC)
            {
                variables.ctyp = variables.cunts[11];
                xPanel.setMBname(variables.cunts[11].Text);
                getconsoletype(3);
            }
            else
            {
                if (device == DEVICE.NAND_X && variables.mtxUsbMode)
                {
                    if (nTools.getbtnWriteECC().Contains("XeLL"))
                    {
                        mtx_usb.writeXeLLAuto();
                    }
                    else
                    {
                        mtx_usb.writeEccAuto();
                    }
                }
                else getconsoletype(3);
            }
        }

        void btnWriteClick()
        {
            if (device == DEVICE.PICOFLASHER)
            {
                picoflasher.Write(0);
            }
            else if (device == DEVICE.XFLASHER_SPI)
            {
                xflasher.writeNandAuto();
            }
            else if (device == DEVICE.XFLASHER_EMMC)
            {
                variables.ctyp = variables.cunts[11];
                xPanel.setMBname(variables.cunts[11].Text);
                getconsoletype(2);
            }
            else
            {
                if (device == DEVICE.NAND_X && variables.mtxUsbMode) mtx_usb.writeNandAuto();
                else getconsoletype(2);
            }
        }

        private void btnXeBuildClick()
        {
            if (variables.ctyp.ID == -1) return;
            ThreadStart starter = delegate { xPanel.createxebuild_v2(false, nand, false); };
            new Thread(starter).Start();
        }

        #endregion

        #region File Buttons

        void btnLoadFile1_Click(object sender, System.EventArgs e)
        {
            loadfile(ref variables.filename1, ref this.txtFilePath1, true);
            Thread.Sleep(100);
            nand_init();
        }

        void btnLoadFile2_Click(object sender, System.EventArgs e)
        {
            loadfile(ref variables.filename2, ref this.txtFilePath2);
            Thread.Sleep(100);
            if (variables.debugme) Console.WriteLine("filename2/currentdir = {0}", variables.filename2);
        }

        void comparebutton_Click(object sender, System.EventArgs e)
        {
            new Thread(comparenands).Start();
        }

        #endregion

        #region Function Buttons

        private void btnWorkingFolder_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(nand.ki.serial))
            {
                if (variables.xefolder != null && variables.xefolder != "")
                {
                    if (Directory.Exists(variables.xefolder)) Process.Start(variables.xefolder);
                }
                else if (Directory.Exists(Path.Combine(Directory.GetParent(variables.outfolder).FullName, nand.ki.serial)))
                {
                    Process.Start(Path.Combine(Directory.GetParent(variables.outfolder).FullName, nand.ki.serial));
                }
                else Process.Start(variables.outfolder);
            }
            else
            {
                Process.Start(variables.outfolder);
            }

        }

        private void btnCPUDBClick()
        {
            callcpukeydb();
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            nand_init();
        }

        #endregion

        #region IP stuff

        private void btnIPGetCPU_Click(object sender, EventArgs e)
        {
            ThreadStart starter = delegate { myIP.IP_GetCpuKey(txtIP.Text); };
            new Thread(starter).Start();
            if (variables.debugme) Console.WriteLine("-----{0}--------", variables.cpkey);
            new Thread(updatecptextbox).Start();
        }

        private void getAndSaveToWorkingFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThreadStart starter = delegate { myIP.IP_GetCpuKey(txtIP.Text, 1); };
            new Thread(starter).Start();
            if (variables.debugme) Console.WriteLine("-----{0}--------", variables.cpkey);
            new Thread(updatecptextbox).Start();
        }

        private void saveToDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThreadStart starter = delegate { myIP.IP_GetCpuKey(txtIP.Text, 2); };
            new Thread(starter).Start();
            if (variables.debugme) Console.WriteLine("-----{0}--------", variables.cpkey);
            new Thread(updatecptextbox).Start();
        }

        private void btnScanner_Click(object sender, EventArgs e)
        {
            if (variables.isscanningip)
            {
                Console.WriteLine("Already scanning, please wait!");
            }
            else
            {
                ThreadStart starter = delegate { myIP.IPScanner(this.progressBar); };
                new Thread(starter).Start();
                if (variables.debugme) Console.WriteLine("-----{0}--------", variables.cpkey);
                new Thread(updatecptextbox).Start();
            }
        }

        #endregion

        #endregion

        #region Keyboard Events

        void txtCPUKey_TextChanged(object sender, System.EventArgs e)
        {
            if (variables.current_mode == variables.JR_MODE.MODEFW) return;
            if (txtCPUKey.Text.Length == 32)
            {
                if (objAlphaPattern.IsMatch(txtCPUKey.Text))
                {
                    if (Nand.Nand.VerifyKey(Oper.StringToByteArray(txtCPUKey.Text)))
                    {
                        variables.cpkey = txtCPUKey.Text;
                        if (!variables.gotvalues && File.Exists(variables.filename1))
                        {
                            nand_init();
                        }
                    }
                    else Console.WriteLine("Bad CPU Key");
                }
            }
            if (txtCPUKey.Text == "AUTOGGISBETTER")
            {
                MessageBox.Show("Lol no");
            }
        }

        private void txtIP_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ThreadStart starter = delegate { myIP.IP_GetCpuKey(txtIP.Text); };
                new Thread(starter).Start();
                if (variables.debugme) Console.WriteLine("-----{0}--------", variables.cpkey);
                new Thread(updatecptextbox).Start();
            }
        }

        #endregion

        #region Clicks

        private void txtConsole_DoubleClick(object sender, EventArgs e)
        {
            File.AppendAllText(Path.Combine(variables.pathforit, "tempLog.txt"), txtConsole.Text);
            System.Diagnostics.Process.Start(Path.Combine(variables.pathforit, "tempLog.txt"));
            Thread.Sleep(1000);
            File.Delete(Path.Combine(variables.pathforit, "tempLog.txt"));
        }

        #endregion

        #region Drag & Drops

        void txtFilePath1_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            this.txtFilePath1.Text = s[0];
            variables.filename1 = s[0];
            if (variables.current_mode != variables.JR_MODE.MODEFW) erasevariables();
            if (Path.GetExtension(s[0]) == ".bin")
            {
                nand_init();
            }

        }
        void txtFilePath1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }
        void txtFilePath2_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            this.txtFilePath2.Text = s[0];
            variables.filename2 = s[0];
        }
        void txtFilePath2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }
        void txtCPUKey_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (File.Exists(s[0]))
            {
                FileInfo f = new FileInfo(s[0]);
                if (f.Length == 16) txtCPUKey.Text = Oper.ByteArrayToString(File.ReadAllBytes(s[0]));
            }
            if (Path.GetExtension(s[0]) == ".txt")
            {
                Regex objAlphaPattern = new Regex("[a-fA-F0-9]{32}$");
                string[] cpu = File.ReadAllLines(s[0]);
                string cpukey = "";
                bool check = false;
                int i = 0;
                foreach (string line in cpu)
                {
                    if (objAlphaPattern.Match(line).Success) i++;
                    if (i > 1) check = true;
                }
                foreach (string line in cpu)
                {
                    if (check)
                    {
                        if (line.ToUpper().Contains("CPU"))
                        {
                            cpukey = objAlphaPattern.Match(line).Value;
                        }
                    }
                    else
                    {
                        cpukey = objAlphaPattern.Match(line).Value;
                        break;
                    }
                    if (variables.debugme) Console.WriteLine(objAlphaPattern.Match(line).Value);
                }
                txtCPUKey.Text = cpukey;
            }
        }
        void txtCPUKey_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        #endregion

        #region KeyUp

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                abort();
            }
            //else if (e.KeyCode == Keys.F1) // Handled from WinForms Menubar
            //{
            //}
            else if (e.KeyCode == Keys.F2 && e.Control && e.Alt)
            {
                if (variables.debugme)
                {
                    Console.WriteLine("Debugging Off");
                    variables.debugme = false;
                }
                else
                {
                    Console.WriteLine("Debugger On");
                    variables.debugme = true;
                }
            }
            else if (e.Control && e.KeyCode == Keys.F3)
            {
                if (!variables.extractfiles) { variables.extractfiles = true; Console.WriteLine("Extract Files On"); }
                else { variables.extractfiles = false; Console.WriteLine("Extract Files Off"); }
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                ThreadStart starter = delegate { demon.Read_Serial_Port(); };
                new Thread(starter).Start();
            }
            else if (e.Control && e.KeyCode == Keys.F8)
            {
                if (variables.filename1 == null || variables.filename2 == null) return;
                long size1 = 0;
                long size2 = 0;
                byte[] file1 = Oper.openfile(variables.filename1, ref size1, 0);
                byte[] file2 = Oper.openfile(variables.filename2, ref size2, 0);
                Oper.savefile(Oper.concatByteArrays(file1, Oper.returnportion(file2, file1.Length, file2.Length - file1.Length), file1.Length, file2.Length - file1.Length), "stocknand.bin");
            }
            else if (e.KeyCode == Keys.F2)
            {
                if (device == DEVICE.PICOFLASHER)
                {
                    picoflasher.getFlashConfig();
                }
                else if (device == DEVICE.XFLASHER_SPI)
                {
                    xflasher.getFlashConfig();
                }
                else
                {
                    getmbtype();
                }
            }
            else if (e.KeyCode == Keys.F3)
            {
                btnProgramCRClick();
            }
            //else if (e.KeyCode == Keys.F4) // Handled from WinForms Menubar
            //{
            //}
            //else if (e.KeyCode == Keys.F5) // Handled from WinForms Menubar
            //{
            //}
            //else if (e.KeyCode == Keys.F6) // Handled from WinForms Menubar
            //{
            //}
            else if (e.KeyCode == Keys.F7)
            {
            }
            else if (e.KeyCode == Keys.F8)
            {
            }
            else if (e.KeyCode == Keys.F9)
            {
                new Thread(trykeys).Start();
            }
            else if (e.KeyCode == Keys.F10)
            {
            }
            else if (e.KeyCode == Keys.F11)
            {
            }
            else if (e.KeyCode == Keys.F12)
            {
                if (listInfo.Contains(xsvfInfo) && variables.timingonkeypress)
                {
                    xsvfInfo_ProgramCRClick();
                }
            }
        }

        void escapedloop()
        {
            Thread.Sleep(2000);
            variables.escapeloop = false;
        }

        #endregion

        #region Demon
        bool showingdemon = false;
        protected override void WndProc(ref Message m)
        {
            try
            {
                // The OnDeviceChange routine processes WM_DEVICECHANGE messages.
                if (m.Msg == DeviceManagement.WM_DEVICECHANGE)
                {
                    if ((m.WParam.ToInt32() == DeviceManagement.DBT_DEVICEREMOVECOMPLETE))
                    {
                        //Console.WriteLine(DemoN.BootloaderPathName);
                        if (DemoN.DemonManagement.DeviceNameMatch(m, DemoN.DemonPathName))
                        {
                            DemoN.DemonDetected = false;
                            showDemon(false);
                        }
                    }
                    else if (m.WParam.ToInt32() == DeviceManagement.DBT_DEVICEARRIVAL)
                    {
                        if (!HID.BootloaderDetected) HID.FindBootloader();
                    }
                    else
                    {
                        if (!HID.BootloaderDetected) HID.FindBootloader();
                        if (!showingdemon) showDemon(DemoN.FindDemon());
                    }
                }
                else if (m.Msg == NativeMethods.WM_SHOWAPP)
                {
                    showApplication();
                }

                // Let the base form process the message
                base.WndProc(ref m);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void showDemon(bool show) // TODO: This should be not seperate
        {
            if (variables.debugme) Console.WriteLine("ShowDemoN {0}", show);
            showingdemon = true;
            if (show)
            {
                Thread.Sleep(100);
                demon.get_mode();

                if (DemoN.mode == DemoN.Demon_Modes.FIRMWARE)
                {
                    demon.get_firmware();
                    demon.get_external_flash(false);
                }

                nTools.setImage(Properties.Resources.demon);
                demoNToolStripMenuItem.Visible = true;
                ModeStatus.Visible = true;
                ModeVersion.Visible = true;
                FWStatus.Visible = true;
                FWVersion.Visible = true;
            }
            else
            {
                if (device == DEVICE.JR_PROGRAMMER_BOOTLOADER)
                {
                    nTools.setImage(Properties.Resources.usb);
                }
                else if (device == DEVICE.JR_PROGRAMMER)
                {
                    nTools.setImage(Properties.Resources.JRP);
                }
                else if (device == DEVICE.NAND_X)
                {
                    if (variables.mtxUsbMode)
                    {
                        nTools.setImage(Properties.Resources.mtx);
                    }
                    else
                    {
                        nTools.setImage(Properties.Resources.NANDX);
                    }
                }
                else if (device == DEVICE.XFLASHER_SPI)
                {
                    nTools.setImage(Properties.Resources.xflash_spi);
                }
                else if (device == DEVICE.XFLASHER_EMMC)
                {
                    nTools.setImage(Properties.Resources.xflash_emmc);
                }
                else if (device == DEVICE.PICOFLASHER)
                {
                    nTools.setImage(Properties.Resources.picoflasher);
                }
                else
                {
                    nTools.setImage(null);
                }
                //Console.WriteLine(device);
                ModeStatus.Visible = false;
                ModeVersion.Visible = false;
                demoNToolStripMenuItem.Visible = false;
                FWStatus.Visible = false;
                FWVersion.Visible = false;
                FlashStatus.Visible = false;
                FlashVersion.Visible = false;
            }
            showingdemon = false;
        }
        private void onDevNotify(object sender, DeviceNotifyEventArgs e)
        {
            try
            {
                if (variables.debugme) Console.WriteLine("DevNotify - {0}", e.Device.Name);
                if (variables.debugme) Console.WriteLine("EventType - {0}", e.EventType);
                if (e.EventType == LibUsbDotNet.DeviceNotify.EventType.DeviceArrival)
                {
                    if (e.Device.IdVendor == 0x600D && e.Device.IdProduct == 0x7001) // PicoFlasher
                    {
                        if (!DemoN.DemonDetected) nTools.setImage(Properties.Resources.picoflasher);
                        //PicoFlasherToolStripMenuItem.Visible = true;
                        device = DEVICE.PICOFLASHER;
                    }
                    else if (e.Device.IdVendor == 0x0403 && e.Device.IdProduct == 0x6010) // xFlasher SPI
                    {
                        if (!DemoN.DemonDetected) nTools.setImage(Properties.Resources.xflash_spi);
                        xFlasherToolStripMenuItem.Visible = true;
                        device = DEVICE.XFLASHER_SPI;
                        xflasher.initDevice();
                    }
                    else if (e.Device.IdVendor == 0xFFFF && e.Device.IdProduct == 0x004) // NAND-X
                    {
                        if (!DemoN.DemonDetected)
                        {
                            if (variables.mtxUsbMode)
                            {
                                nTools.setImage(Properties.Resources.mtx);
                            }
                            else
                            {
                                nTools.setImage(Properties.Resources.NANDX);
                            }
                        }
                        nANDXToolStripMenuItem.Visible = true;
                        device = DEVICE.NAND_X;
                    }
                    else if (e.Device.IdVendor == 0x11D4 && e.Device.IdProduct == 0x8338) // JR-Programmer
                    {
                        if (!DemoN.DemonDetected) nTools.setImage(Properties.Resources.JRP);
                        jRPBLToolStripMenuItem.Visible = false;
                        jRPToolStripMenuItem.Visible = true;
                        device = DEVICE.JR_PROGRAMMER;
                    }
                    else if (e.Device.IdVendor == 0x11D4 && e.Device.IdProduct == 0x8334) // JR-Programmer Bootloader
                    {
                        if (!DemoN.DemonDetected) nTools.setImage(Properties.Resources.usb);
                        jRPToolStripMenuItem.Visible = false;
                        jRPBLToolStripMenuItem.Visible = true;
                        device = DEVICE.JR_PROGRAMMER_BOOTLOADER;
                    }
                    else if ((e.Device.IdVendor == 0xAAAA && e.Device.IdProduct == 0x8816) || (e.Device.IdVendor == 0x05E3 && e.Device.IdProduct == 0x0751)) // xFlasher eMMC
                    {
                        if (!DemoN.DemonDetected) nTools.setImage(Properties.Resources.xflash_emmc);
                        xFlasherToolStripMenuItem.Visible = true;
                        device = DEVICE.XFLASHER_EMMC;
                    }
                }
                else if (e.EventType == LibUsbDotNet.DeviceNotify.EventType.DeviceRemoveComplete)
                {
                    if (e.Device.IdVendor == 0x600D && e.Device.IdProduct == 0x7001)
                    {
                        if (!DemoN.DemonDetected) nTools.setImage(null);
                        //PicoFlasherToolStripMenuItem.Visible = false;
                        device = DEVICE.NO_DEVICE;
                    }
                    else if(e.Device.IdVendor == 0x11d4 && e.Device.IdProduct == 0x8334)
                    {
                        HID.BootloaderDetected = false;
                        if (!DemoN.DemonDetected) nTools.setImage(null);
                        jRPBLToolStripMenuItem.Visible = false;
                        device = 0;
                    }
                    else if (e.Device.IdVendor == 0xFFFF && e.Device.IdProduct == 0x004)
                    {
                        if (!DemoN.DemonDetected) nTools.setImage(null);
                        nANDXToolStripMenuItem.Visible = false;
                        device = 0;
                    }
                    else if (e.Device.IdVendor == 0x11d4 && e.Device.IdProduct == 0x8338)
                    {
                        if (!DemoN.DemonDetected) nTools.setImage(null);
                        jRPToolStripMenuItem.Visible = false;
                        device = DEVICE.NO_DEVICE;
                    }
                    else if ((e.Device.IdVendor == 0x0403 && e.Device.IdProduct == 0x6010) ||
                        (e.Device.IdVendor == 0x05E3 && e.Device.IdProduct == 0x0751) ||
                         (e.Device.IdVendor == 0xAAAA && e.Device.IdProduct == 0x8816))
                    {
                        if (!DemoN.DemonDetected) nTools.setImage(null);
                        xFlasherToolStripMenuItem.Visible = false;
                        device = DEVICE.NO_DEVICE;
                    }
                }

                if (listInfo.Contains(ldInfo)) ldInfo.refreshDrives(true);
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            try // It'll fail if the thing doesn't exist
            {
                if (updateDevice != null)
                    updateDevice();
            }
            catch
            {
                // Do nothing
            }
        }
        #endregion

        #region Settings & Dashes

        public void trykeys()
        {
            if (variables.filename1 == null || variables.filename1 == "") return;
            long size = 0;
            byte[] keyvault = Nand.Nand.getkv(Oper.openfile(variables.filename1, ref size, 0));
            int index = 0;
            RegistryKey cpukeydb = Registry.CurrentUser.CreateSubKey("CPUKey_DB", RegistryKeyPermissionCheck.ReadWriteSubTree);
            foreach (string valueName in cpukeydb.GetValueNames())
            {
                if (valueName == "Index")
                {
                    index = Convert.ToInt32(cpukeydb.GetValue(valueName));
                    Console.WriteLine("Checking against {0} keys. Might take some time.", index);
                    for (int i = 1; i <= index; i++)
                    {
                        try
                        {
                            RegistryKey cpukeys = cpukeydb.OpenSubKey(i.ToString(), true);
                            if (cpukeys.GetValue("Deleted") != null)
                            {
                                RegistryUtilities.RenameSubKey(cpukeydb, index.ToString(), i.ToString());
                                index = index - 1;
                                cpukeys.SetValue("Index", i); ;
                                cpukeydb.SetValue("Index", index);
                                cpukeys.DeleteValue("Deleted");
                                //continue;
                            }
                            if (Nand.Nand.cpukeyverification(keyvault, cpukeys.GetValue("cpukey").ToString()))
                            {
                                variables.cpkey = cpukeys.GetValue("cpukey").ToString();
                                txtCPUKey.Text = cpukeys.GetValue("cpukey").ToString();
                                Console.WriteLine("Key found");
                                return;
                            }
                        }
                        catch (SystemException ex)
                        {
                            if (variables.debugme) Console.WriteLine(ex.ToString());
                            continue;
                        }
                        catch (Exception ex)
                        {
                            if (variables.debugme) Console.WriteLine(ex.ToString());
                            continue;
                        }
                    }
                    Console.WriteLine("Key wasnt found in Database");
                }
            }

        }

        public void savesettings()
        {
            xml x = new xml();
            try
            {
                x.create(variables.settingsfile);
                x.start();
                foreach (string name in variables.settings)
                {
                    switch (name)
                    {
                        case "xebuild":
                            x.write(name, variables.xebuild);
                            break;
                        case "FileChecks":
                            x.write(name, variables.checkfiles.ToString());
                            break;
                        case "location":
                            x.write(name, variables.location.ToString());
                            break;
                        case "COMPort":
                            x.write(name, variables.COMPort);
                            break;
                        case "Errorsound":
                            x.write(name, variables.sounderror);
                            break;
                        case "Comparesound":
                            x.write(name, variables.soundcompare);
                            break;
                        case "Successsound":
                            x.write(name, variables.soundsuccess);
                            break;
                        case "Delay":
                            x.write(name, variables.delay.ToString());
                            break;
                        case "DashLaunchE":
                            x.write(name, variables.DashLaunchE.ToString());
                            break;
                        case "IP":
                            x.write(name, variables.ip);
                            break;
                        case "NoReads":
                            x.write(name, variables.NoReads.ToString());
                            break;
                        case "IPStart":
                            x.write(name, variables.IPstart);
                            break;
                        case "IPEnd":
                            x.write(name, variables.IPend);
                            break;
                        case "XebuildName":
                            x.write(name, variables.nandflash);
                            break;
                        case "dashlaunch":
                            x.write(name, variables.dashlaunch);
                            break;
                        case "preferredDash":
                            x.write(name, variables.preferredDash);
                            break;
                        case "KeepFiles":
                            x.write(name, variables.deletefiles.ToString());
                            break;
                        case "WorkingDir":
                            x.write(name, variables.outfolder);
                            break;
                        case "LPTport":
                            x.write(name, variables.LPTport);
                            break;
                        case "AutoExtract":
                            x.write(name, variables.autoExtract.ToString());
                            break;
                        case "AllMove":
                            x.write(name, variables.allmove.ToString());
                            break;
                        case "Modder":
                            x.write(name, variables.modder.ToString());
                            break;
                        case "DiscordRPC":
                            x.write(name, variables.discordrpc.ToString());
                            break;
                        case "TimingOnKeypress":
                            x.write(name, variables.timingonkeypress.ToString());
                            break;
                        case "LogBackground":
                            x.write(name, ColorTranslator.ToHtml(variables.logbackground));
                            break;
                        case "LogText":
                            x.write(name, ColorTranslator.ToHtml(variables.logtext));
                            break;
                        case "MinimizeToTray":
                            x.write(name, variables.minimizetotray.ToString());
                            break;
                        case "SlimPreferSrgh":
                            x.write(name, variables.slimprefersrgh.ToString());
                            break;
                        case "MtxUsbMode":
                            x.write(name, variables.mtxUsbMode.ToString());
                            break;
                        default:
                            break;
                    }
                }
            }
            finally
            {
                x.end();
                x.close();
            }
        }
        void loadsettings()
        {
            if (File.Exists(variables.settingsfile))
            {
                xml x = new xml();
                x.load(File.ReadAllText(variables.settingsfile));
                foreach (string name in variables.settings)
                {
                    string val = x.readsetting(name);
                    bool bvalue;
                    switch (name)
                    {
                        case "xebuild":
                            string xmd5 = Oper.GetMD5HashFromFile(variables.update_path + "xeBuild.exe").ToUpper();

                            if (variables.xebuilds.ContainsKey(xmd5.ToUpper()))
                            {
                                if (variables.debugme) Console.WriteLine("Known xebuild md5 found");
                                XeBuildVersion.Text = variables.xebuilds[xmd5.ToUpper()];
                                variables.xebuild = variables.xebuilds[xmd5.ToUpper()];
                            }
                            else
                            {
                                XeBuildVersion.Text = val;
                                variables.xebuild = val;
                            }
                            break;
                        case "FileChecks":
                            bvalue = true;
                            if (!bool.TryParse(val, out bvalue)) bvalue = true;
                            variables.checkfiles = bvalue;
                            break;
                        case "location":
                            int xy = 0;
                            int y = 0;
                            if (!String.IsNullOrWhiteSpace(val))
                            {
                                var g = Regex.Replace(val, @"[\{\}a-zA-Z=]", "").Split(',');
                                int.TryParse(g[0], out xy);
                                int.TryParse(g[1], out y);
                            }
                            variables.location = new Point(xy, y);
                            break;
                        case "COMPort":
                            variables.COMPort = val;
                            break;
                        case "Errorsound":
                            variables.sounderror = val;
                            break;
                        case "Comparesound":
                            variables.soundcompare = val;
                            break;
                        case "Successsound":
                            variables.soundsuccess = val;
                            break;
                        case "Delay":
                            int ivalue = 0;
                            int.TryParse(val, out ivalue);
                            variables.delay = ivalue;
                            break;
                        case "DashLaunchE":
                            bvalue = false;
                            bool.TryParse(val, out bvalue);
                            variables.DashLaunchE = bvalue;
                            xPanel.setDLPatches(bvalue);
                            break;
                        case "IP":
                            if (!string.IsNullOrWhiteSpace(val))
                            {
                                variables.ip = val;
                                txtIP.Text = val + ".";
                            }
                            else
                            {
                                string localIP = IP.getGatewayIp();
                                txtIP.Text = localIP.Remove(localIP.LastIndexOf('.')) + ".";
                            }
                            break;
                        case "NoReads":
                            decimal dvalue = 2;
                            decimal.TryParse(val, out dvalue);
                            if (dvalue == 0) dvalue = 2;
                            nTools.setNumericIterations(dvalue);
                            variables.NoReads = dvalue;
                            break;
                        case "IPStart":
                            variables.IPstart = val;
                            break;
                        case "IPEnd":
                            variables.IPend = val;
                            break;
                        case "XebuildName":
                            if (!string.IsNullOrWhiteSpace(val)) variables.nandflash = val;
                            break;
                        case "dashlaunch":
                            string dlmd5 = Oper.GetMD5HashFromFile(variables.update_path + "launch.xex").ToUpper();

                            if (variables.dls.ContainsKey(dlmd5.ToUpper()))
                            {
                                if (variables.debugme) Console.WriteLine("Known dl md5 found");
                                DashlaunchVersion.Text = variables.dls[dlmd5.ToUpper()];
                                variables.dashlaunch = variables.dls[dlmd5.ToUpper()];
                            }
                            else
                            {
                                DashlaunchVersion.Text = val;
                                variables.dashlaunch = val;
                            }
                            break;
                        case "preferredDash":
                            variables.preferredDash = val;
                            break;
                        case "KeepFiles":
                            bvalue = false;
                            bool.TryParse(val, out bvalue);
                            variables.deletefiles = bvalue;
                            break;
                        case "WorkingDir":
                            if (!string.IsNullOrWhiteSpace(val))
                            {
                                variables.outfolder = val;
                                if (!Directory.Exists(variables.outfolder))
                                {
                                    try
                                    {
                                        Directory.CreateDirectory(variables.outfolder);
                                    }
                                    catch (System.IO.DirectoryNotFoundException)
                                    {
                                        variables.outfolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                                        Directory.CreateDirectory(variables.outfolder);
                                    }
                                }
                            }
                            break;
                        case "LPTport":
                            nTools.setLptPort(val);
                            variables.LPTport = val;
                            break;
                        case "AutoExtract":
                            bvalue = false;
                            if (!bool.TryParse(val, out bvalue)) bvalue = false;
                            variables.autoExtract = bvalue;
                            break;
                        case "AllMove":
                            bvalue = false;
                            if (!bool.TryParse(val, out bvalue)) bvalue = false;
                            variables.allmove = bvalue;
                            break;
                        case "Modder":
                            bvalue = false;
                            if (!bool.TryParse(val, out bvalue)) bvalue = false;
                            variables.modder = bvalue;
                            break;
                        case "DiscordRPC":
                            bvalue = true;
                            if (!bool.TryParse(val, out bvalue)) bvalue = true;
                            variables.discordrpc = bvalue;
                            break;
                        case "TimingOnKeypress":
                            bvalue = false;
                            if (!bool.TryParse(val, out bvalue)) bvalue = false;
                            variables.timingonkeypress = bvalue;
                            break;
                        case "LogBackground":
                            if (!string.IsNullOrWhiteSpace(val))
                            {
                                try
                                {
                                    variables.logbackground = ColorTranslator.FromHtml(val);
                                    // Updated in LogText
                                }
                                catch { }
                            }
                            break;
                        case "LogText":
                            if (!string.IsNullOrWhiteSpace(val))
                            {
                                try
                                {
                                    variables.logtext = ColorTranslator.FromHtml(val);
                                    updateLogColor();
                                }
                                catch { }
                            }
                            break;
                        case "MinimizeToTray":
                            bvalue = false;
                            if (!bool.TryParse(val, out bvalue)) bvalue = false;
                            variables.minimizetotray = bvalue;
                            break;
                        case "SlimPreferSrgh":
                            bvalue = false;
                            if (!bool.TryParse(val, out bvalue)) bvalue = false;
                            variables.slimprefersrgh = bvalue;
                            break;
                        case "MtxUsbMode":
                            bvalue = false;
                            if (!bool.TryParse(val, out bvalue)) bvalue = false;
                            mtxUsbModeToolStripMenuItem.Checked = variables.mtxUsbMode = bvalue;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                string localIP = IP.getGatewayIp();
                txtIP.Text = localIP.Remove(localIP.LastIndexOf('.')) + ".";
            }
            IP.initaddresses();
        }
        void settings()
        {
            loadsettings();
        }

        void add_dash()
        {
            addDash newdash = new addDash();
            newdash.ShowDialog();
            //check_dash();
            ThreadStart starte = delegate { check_dashes(true); };
            Thread th = new Thread(starte);
            th.IsBackground = true;
            th.Start();
            //check_dash();
        }
        void del_dash()
        {
            Dashes.delDash deldash = new Dashes.delDash();
            deldash.ShowDialog();
            check_dash();
        }

        void check_dash()
        {
            DataTable dashtable = xPanel.getDataSet().DataTable2;
            int counter = 0;
            dashtable.Rows.Clear();
            Thread.Sleep(10);
            if (Directory.Exists(Path.Combine(variables.currentdir, "xeBuild")))
            {
                try
                {
                    variables.dashes_all = new List<string>();
                    Regex regex = new Regex("^[0-9]+$");

                    foreach (string a in Directory.GetDirectories(Path.Combine(variables.currentdir, "xeBuild")))
                    {
                        if (regex.IsMatch(Path.GetFileNameWithoutExtension(a))) variables.dashes_all.Add(Path.GetFileNameWithoutExtension(a));
                    }
                    variables.dashes_all.Sort((a, b) => Convert.ToInt32(a) - Convert.ToInt32(b));
                    if (variables.debugme) Console.WriteLine("Checking dashes");
                    foreach (string valueName in variables.dashes_all)
                    {
                        if (variables.debugme) Console.WriteLine(valueName);
                        DataRow dashcombo = dashtable.NewRow();
                        dashcombo[0] = counter;
                        dashcombo[1] = valueName;
                        dashtable.Rows.Add(dashcombo);
                        counter++;
                    }
                }
                catch (NullReferenceException) { }
            }
            Thread.Sleep(10);
            DataRow dashrows = dashtable.NewRow();
            dashrows[0] = counter;
            dashrows[1] = "-------------";
            dashtable.Rows.Add(dashrows);
            counter++;
            DataRow dashrows1 = dashtable.NewRow();
            dashrows1[0] = counter;
            dashrows1[1] = "Add Kernel";
            dashtable.Rows.Add(dashrows1);
            counter++;
            DataRow dashrows2 = dashtable.NewRow();
            dashrows2[0] = counter;
            dashrows2[1] = "Del Kernel";
            dashtable.Rows.Add(dashrows2);
            counter++;
            Thread.Sleep(10);
            try
            {
                if (xPanel.getComboDash().Items.Count == 4)
                {
                    xPanel.getComboDash().SelectedIndex = 1;
                    xPanel.getComboDash().SelectedIndex = 0;
                    int n = 0;
                    bool isNumeric = int.TryParse(xPanel.getComboDash().Text, out n);
                    if (isNumeric) variables.dashversion = n;
                }
                else
                {
                    if (variables.dashes_all.Contains(variables.preferredDash))
                    {
                        if (xPanel.getComboDash().Items.Count >= variables.dashes_all.IndexOf(variables.preferredDash)) xPanel.getComboDash().SelectedIndex = variables.dashes_all.IndexOf(variables.preferredDash);
                        int n = 0;
                        bool isNumeric = int.TryParse(xPanel.getComboDash().Text, out n);
                        if (isNumeric) variables.dashversion = n;
                    }
                    else if (xPanel.getComboDash().Items.Count > 3) xPanel.BeginInvoke((Action)(() => xPanel.getComboDash().SelectedIndex = xPanel.getComboDash().Items.Count - 3));
                }
            }
            catch (InvalidOperationException)
            {
            }
            Thread.Sleep(100);
        }

        void check_dashes(bool check = false)
        {
            variables.dashes_all.Sort();
            if (check) check_dash();
        }

        #endregion

        #region General device interactions with UI

        public void afterWriteEccCleanup()
        {
            if (variables.tempfile != "")
            {
                variables.filename1 = variables.tempfile;
                txtFilePath1.Text = variables.tempfile;
            }
        }

        #endregion

        #region xFlasher interactions with UI

        public void xFlasherInitNand(int i = 2)
        {
            if (i == 2 && File.Exists(variables.filename))
            {
                txtFilePath1.BeginInvoke((Action)(() => txtFilePath1.Text = Path.Combine(variables.filename)));
                variables.filename1 = variables.filename;
                nand_init();
            }
            if (i == 3 && File.Exists(variables.filename))
            {
                txtFilePath2.BeginInvoke((Action)(() => txtFilePath2.Text = Path.Combine(variables.filename)));
                variables.filename2 = variables.filename;
                new Thread(comparenands).Start();
            }
        }

        public void xFlasherNandSelShow(int xfseltype, bool bigblock = false)
        {
            variables.xfSelType = xfseltype;
            xFlasherNandSel xfselform = new xFlasherNandSel();
            xfselform.TopMost = true;
            xfselform.SizeClick += xFlasherSizeClick;
            xfselform.BigBlock(bigblock);
            xfselform.Show();
        }

        void xFlasherSizeClick(int size)
        {
            if (variables.xfSelType == 1)
            {
                xflasher.readNandAuto(size, nTools.getNumericIterations(), true);
                variables.xfSelType = 0;
            }
            else if (variables.xfSelType == 2)
            {
                xflasher.writeNand(size, variables.filename1);
                variables.xfSelType = 0;
            }
        }

        public void xFlasherBusy(int mode)
        {
            if (mode > 0)
            {
                ProgressLabel.BeginInvoke(new Action(() => {
                    if (mode == 3) ProgressLabel.Text = "Erasing";
                    else if (mode == 2) ProgressLabel.Text = "Writing";
                    else if (mode == 1) ProgressLabel.Text = "Reading";
                }));
                progressBar.BeginInvoke(new Action(() => progressBar.Style = ProgressBarStyle.Blocks));
            }
            else if (mode == -2)
            {
                progressBar.BeginInvoke(new Action(() => progressBar.Style = ProgressBarStyle.Marquee));
            }
            else if (mode == -1)
            {
                ProgressLabel.BeginInvoke(new Action(() => ProgressLabel.Text = "Progress"));
                progressBar.BeginInvoke(new Action(() => {
                    progressBar.Style = ProgressBarStyle.Blocks;
                    progressBar.Value = progressBar.Minimum;
                }));
                txtBlocks.BeginInvoke(new Action(() => txtBlocks.Text = ""));
            }
            else
            {
                ProgressLabel.BeginInvoke(new Action(() => ProgressLabel.Text = "Progress"));
                progressBar.BeginInvoke(new Action(() => {
                    progressBar.Style = ProgressBarStyle.Blocks;
                    progressBar.Value = progressBar.Maximum;
                }));
                txtBlocks.BeginInvoke(new Action(() => { txtBlocks.Text = ""; }));
            }
        }

        public void xFlasherBlocksUpdate(string str, int progress)
        {
            if (xflasher.inUse)
            {
                txtBlocks.BeginInvoke((Action)(() => txtBlocks.Text = str));
                if (progress >= 0) progressBar.BeginInvoke((Action)(() => progressBar.Value = progress)); // Just in case
                else progressBar.BeginInvoke((Action)(() => progressBar.Value = 0));
            }
        }

        #endregion

        #region PicoFlasher interactions with UI
        public void PicoFlasherInitNand(int idx)
        {
            if (idx == 0 && File.Exists(variables.filename))
            {
                txtFilePath1.BeginInvoke((Action)(() => txtFilePath1.Text = Path.Combine(variables.filename)));
                variables.filename1 = variables.filename;
                nand_init();
            }
            if (idx == 1 && File.Exists(variables.filename))
            {
                txtFilePath2.BeginInvoke((Action)(() => txtFilePath2.Text = Path.Combine(variables.filename)));
                variables.filename2 = variables.filename;
                new Thread(comparenands).Start();
            }
        }

        public void PicoFlasherNandSelShow(int xfseltype, bool bigblock = false)
        {
            variables.xfSelType = xfseltype;
            xFlasherNandSel xfselform = new xFlasherNandSel();
            xfselform.TopMost = true;
            xfselform.SizeClick += PicoFlasherSizeClick;
            xfselform.BigBlock(bigblock);
            xfselform.Show();
        }

        void PicoFlasherSizeClick(int size)
        {
            if (variables.xfSelType == 1)
            {
                xflasher.readNandAuto(size, nTools.getNumericIterations(), true);
                variables.xfSelType = 0;
            }
            else if (variables.xfSelType == 2)
            {
                xflasher.writeNand(size, variables.filename1);
                variables.xfSelType = 0;
            }
        }

        public void PicoFlasherBusy(int mode)
        {
            if (mode > 0)
            {
                ProgressLabel.BeginInvoke(new Action(() => {
                    if (mode == 3) ProgressLabel.Text = "Erasing";
                    else if (mode == 2) ProgressLabel.Text = "Writing";
                    else if (mode == 1) ProgressLabel.Text = "Reading";
                }));
                progressBar.BeginInvoke(new Action(() => { progressBar.Style = ProgressBarStyle.Blocks; }));
            }
            else if (mode == -2)
            {
                progressBar.BeginInvoke(new Action(() => { progressBar.Style = ProgressBarStyle.Marquee; }));
            }
            else if (mode == -1)
            {
                ProgressLabel.BeginInvoke(new Action(() => { ProgressLabel.Text = "Progress"; }));
                progressBar.BeginInvoke(new Action(() => {
                    progressBar.Style = ProgressBarStyle.Blocks;
                    progressBar.Value = progressBar.Minimum;
                }));
                txtBlocks.BeginInvoke(new Action(() => { txtBlocks.Text = ""; }));
            }
            else
            {
                ProgressLabel.BeginInvoke(new Action(() => { ProgressLabel.Text = "Progress"; }));
                progressBar.BeginInvoke(new Action(() => {
                    progressBar.Style = ProgressBarStyle.Blocks;
                    progressBar.Value = progressBar.Maximum;
                }));
                txtBlocks.BeginInvoke(new Action(() => { txtBlocks.Text = ""; }));
            }
        }

        public void PicoFlasherBlocksUpdate(string str, int progress)
        {
            if (picoflasher.InUse)
            {
                txtBlocks.BeginInvoke((Action)(() => txtBlocks.Text = str));
                if (progress >= 0)
                    progressBar.BeginInvoke((Action)(() => progressBar.Value = progress));
                else
                    progressBar.BeginInvoke((Action)(() => progressBar.Value = 0));
            }
        }
        #endregion

        #region Matrix Flasher interactions with UI

        public void mtxBusy(int mode)
        {
            if (mode > 0)
            {
                ProgressLabel.Text = "Writing";
                progressBar.BeginInvoke((Action)(() => progressBar.Style = ProgressBarStyle.Marquee));
                txtBlocks.Text = "";
            }
            else
            {
                ProgressLabel.Text = "Progress";
                progressBar.BeginInvoke((Action)(() => progressBar.Style = ProgressBarStyle.Blocks));
                progressBar.BeginInvoke((Action)(() => progressBar.Value = progressBar.Maximum));
                txtBlocks.Text = "";
            }
        }

        #endregion

        #region Misc
        private void demonSerialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.DemonCom good = new Forms.DemonCom();
            good.Show();
        }
        private static Demon _demon;

        private bool StartDemon()
        {
            _demon = new Demon();
            _demon.NewSerialDataRecieved += MonitorOnNewSerialDataRecieved;
            return _demon.Start();
        }

        public bool KillDemon()
        {
            _demon.Stop();
            return true;
        }

        private void MonitorOnNewSerialDataRecieved(object sender, SerialDataEventArgs e)
        {
            var tmp = Encoding.UTF8.GetString(e.Data);
            Console.Write(tmp);

        }

        public void updateLogColor()
        {
            txtConsole.BackColor = variables.logbackground;
            txtConsole.ForeColor = variables.logtext;
        }

        public string getTxtCpuKey()
        {
            return txtCPUKey.Text;
        }

        #endregion
    }
}
