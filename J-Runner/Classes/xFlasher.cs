using System;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace JRunner
{
    public class xFlasher
    {
        [DllImport(@"common\\xflasher\\FTDI2SPI.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int spi(int mode, int size, string file, int startblock = 0, int length = 0);

        [DllImport(@"common\\xflasher\\FTDI2SPI.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int spiGetBlocks();

        [DllImport(@"common\\xflasher\\FTDI2SPI.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int spiGetConfig();

        [DllImport(@"common\\xflasher\\FTDI2SPI.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void spiStop();

        public string svfPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Octal450\\TimingSvfTemp.svf");
        public string svfRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Octal450");

        public bool ready = false;
        public bool inUse = false;
        public bool waiting = false;
        private string flashconf = "";
        private string jtagdevice = "";

        private static int initCount = 0;
        private static int inUseCount = 0;
        public static string xFlasherTimeString = "";
        System.Windows.Threading.DispatcherTimer initTimer;
        System.Timers.Timer inUseTimer;

        // Libraries
        public bool osCheck()
        {
            if (Environment.OSVersion.Version.Major > 6) // Win 10+
            {
                return true;
            }
            else if (Environment.OSVersion.Version.Major == 6)
            {
                if (Environment.OSVersion.Version.Minor > 0) return true; // Win 7/8/8.1
                else if (Environment.OSVersion.ServicePack == "Service Pack 2") return true; // Vista SP2
                else // Vista RTM/SP1
                {
                    MessageBox.Show("This version of Windows is not supported\n\nxFlasher requires Microsoft Windows Vista Service Pack 2 or later", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else // XP and older
            {
                MessageBox.Show("This version of Windows is not supported\n\nxFlasher requires Microsoft Windows Vista Service Pack 2 or later", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void initTimerSetup()
        {
            initTimer = new System.Windows.Threading.DispatcherTimer();
            initTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            initTimer.Tick += initTimerUpd;
        }

        public void initDevice()
        {
            initTimer.Stop();
            initCount = 0;
            ready = false;
            initTimer.Start();
        }

        private void initTimerUpd(object source, EventArgs e)
        {
            if (initCount < 16)
            {
                initCount++;
            }
            else
            {
                ready = true;
                initTimer.Stop();
                if (!inUse) MainForm.mainForm.xFlasherBusy(-1);
                waiting = false;
            }
        }

        public void inUseTimerSetup()
        {
            inUseTimer = new System.Timers.Timer(1000);
            inUseTimer.Elapsed += inUseTimerUpd;
        }

        private void inUseTimerUpd(object source, EventArgs e)
        {
            inUseCount++;

            if (inUseCount > 59) xFlasherTimeString = TimeSpan.FromSeconds(inUseCount).ToString(@"m\:ss") + " min(s)";
            else if (inUseCount >= 0) xFlasherTimeString = inUseCount + " sec(s)";
        }

        public void abort()
        {
            spiStop();
        }

        // Flash Config
        public void getFlashConfig()
        {
            if (!osCheck()) return;

            if (waiting) return;

            if (inUse)
            {
                Console.WriteLine("xFlasher: Device Is Busy");
                return;
            }

            Thread ftdiThread = new Thread(() =>
            {
                try
                {
                    if (!ready)
                    {
                        waiting = true;
                        MainForm.mainForm.xFlasherBusy(-2);
                        Console.WriteLine("xFlasher: Waiting for device to become ready");
                    }
                    while (!ready)
                    {
                        // Do nothing and wait
                    }

                    getFlashConfigActual();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (variables.debugme) Console.WriteLine(ex.ToString());
                    Console.WriteLine("");
                }
            });
            ftdiThread.Start();
        }

        public int getFlashConfigActual(bool auto = false)
        {
            Console.WriteLine("xFlasher: Checking Console...");
            inUse = true;

            int result = spi(0, 16, "common/xflasher/nand.bin");

            flashconf = spiGetConfig().ToString("X8");

            if (File.Exists("common/xflasher/nand.bin"))
            {
                File.Delete("common/xflasher/nand.bin");
            }

            inUse = false;

            if (result == 0 || result == -3 || result == -4)
            {
                Console.WriteLine("Flash Config: 0x{0}", flashconf);

                if (flashconf == "00000000" || flashconf == "FFFFFFFF")
                {
                    Console.WriteLine("xFlasher: Console Not Found");
                    if (auto) Console.WriteLine("xFlasher: Can Not Continue");
                    Console.WriteLine("");
                    return 1;
                }

                if (flashconf == "C0462002")
                {
                    Console.WriteLine("Corona 4GB");

                    if (auto)
                    {
                        Console.WriteLine("");
                        MessageBox.Show("Unable to read/write eMMC type console in SPI mode\n\nPlease switch to eMMC mode", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return 1;
                    }
                }
                else if (result == -4)
                {
                    Console.WriteLine("xFlasher: Unknown Nand");

                    return 1;
                }
                else if (flashconf == "00023010") Console.WriteLine("Jasper 16MB, Trinity");
                else if (flashconf == "00043000") Console.WriteLine("Corona 16MB");
                else if (flashconf == "008A3020") Console.WriteLine("Jasper 256MB");
                else if (flashconf == "00AA3020") Console.WriteLine("Jasper 512MB");
                else if (flashconf == "01198010" || flashconf == "01198030") Console.WriteLine("Xenon, Zephyr, Falcon");
                else Console.WriteLine("Unrecongized Flash Config");

                Console.WriteLine("");
                return 0;
            }
            else if (result == -2)
            {
                Console.WriteLine("xFlasher: Device Not Initialized");
                Console.WriteLine("");
                return 1;
            }
            else
            {
                Console.WriteLine("xFlasher: Unknown Error");
                Console.WriteLine("");
                return 1;
            }
        }

        // Read Nand
        public void readNandAuto(int size, int iterations, bool skipboardcheck = false) // Automated read, do not use for any special/custom read
        {
            if (!osCheck()) return;

            if (waiting) return;

            if (inUse)
            {
                return;
            }

            Thread ftdiThread = new Thread(() =>
            {
                try
                {
                    if (!ready)
                    {
                        waiting = true;
                        MainForm.mainForm.xFlasherBusy(-2);
                        Console.WriteLine("xFlasher: Waiting for device to become ready");
                    }
                    while (!ready)
                    {
                        // Do nothing and wait
                    }

                    if (!skipboardcheck)
                    {
                        if (getFlashConfigActual(true) != 0)
                        {
                            return;
                        }

                        if (size == 0) // Auto Detect Dump Size From Board
                        {
                            if (flashconf == "00023010" || flashconf == "00043000" || flashconf == "01198010")
                            {
                                size = 16;
                            }
                            else if (flashconf == "01198030")
                            {
                                size = 64;
                            }
                            else if (flashconf == "008A3020" || flashconf == "00AA3020")
                            {
                                MainForm.mainForm.BeginInvoke((Action)(() => MainForm.mainForm.xFlasherNandSelShow(1, true))); // Ask BB
                                return;
                            }
                            else
                            {
                                MainForm.mainForm.BeginInvoke((Action)(() => MainForm.mainForm.xFlasherNandSelShow(1))); // Ask
                                return;
                            }
                        }
                    }

                    int i = 1;
                    for (i = 1; i <= iterations;)
                    {
                        variables.iterations = i;
                        if (i == 1)
                        {
                            if (iterations == 1)
                            {
                                Console.WriteLine("xFlasher: {0} Nand Read Queued", iterations);
                            }
                            else
                            {
                                Console.WriteLine("xFlasher: {0} Nand Reads Queued", iterations);
                            }
                        }
                        else
                        {
                            Thread.Sleep(1000);
                            MainForm.mainForm.xFlasherInitNand(i);
                            Thread.Sleep(1000);
                        }

                        variables.filename = variables.outfolder + "\\nanddump" + i + ".bin";
                        if (File.Exists(variables.filename))
                        {
                            if (DialogResult.Cancel == MessageBox.Show("File already exists, it will be DELETED! Press OK to continue", "About to overwrite a nanddump", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                            {
                                Console.WriteLine("xFlasher: Cancelled");
                                Console.WriteLine("");
                                return;
                            };
                        }

                        variables.reading = true;
                        MainForm.mainForm.xFlasherBusy(1);
                        Console.WriteLine("xFlasher: Reading Nand to {0}", variables.filename);

                        Thread blocksThread = new Thread(() =>
                        {
                            getBlocks(0, size * 64);
                        });

                        inUse = true;
                        blocksThread.Start();

                        int result = spi(1, size, variables.filename);

                        inUseTimer.Enabled = false;
                        inUseCount = 0;
                        inUse = false;
                        variables.reading = false;
                        MainForm.mainForm.xFlasherBusy(0);

                        if (result == -1)
                        {
                            i = iterations + 1;
                            Console.WriteLine("xFlasher: Aborted!");
                            Console.WriteLine("");
                            return;
                        }
                        else if (result == 0)
                        {
                            Console.WriteLine("xFlasher: Read Successful! Time Elapsed: {0}", xFlasherTimeString);
                            Console.WriteLine("");

                            SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                            if (variables.soundsuccess != "") success.SoundLocation = variables.soundsuccess;
                            success.Play();
                        }
                        else if (result == -2)
                        {
                            Console.WriteLine("xFlasher: Device Not Initialized");
                            Console.WriteLine("");
                            return;
                        }
                        else if (result == -3)
                        {
                            Console.WriteLine("xFlasher: Console Not Found");
                            Console.WriteLine("");
                            return;
                        }
                        else if (result == -4)
                        {
                            Console.WriteLine("xFlasher: Unknown Nand");
                            Console.WriteLine("");
                            return;
                        }
                        else if (result == -11)
                        {
                            Console.WriteLine("xFlasher: Couldn't Open File");
                            Console.WriteLine("");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("xFlasher: Unknown Error");
                            Console.WriteLine("");
                            return;
                        }

                        i++;
                    }
                    if (i > iterations)
                    {
                        Thread.Sleep(1000);
                        MainForm.mainForm.xFlasherInitNand(i);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (variables.debugme) Console.WriteLine(ex.ToString());
                    Console.WriteLine("");
                }
            });
            ftdiThread.Start();
        }

        public void readNand(int size, string filename, int startblock = 0, int length = 0)
        {
            if (!osCheck()) return;

            if (waiting) return;

            if (String.IsNullOrWhiteSpace(filename)) return;

            if (inUse)
            {
                return;
            }

            Thread ftdiThread = new Thread(() =>
            {
                try
                {

                    if (!ready)
                    {
                        waiting = true;
                        MainForm.mainForm.xFlasherBusy(-2);
                        Console.WriteLine("xFlasher: Waiting for device to become ready");
                    }
                    while (!ready)
                    {
                        // Do nothing and wait
                    }

                    if (getFlashConfigActual(true) != 0)
                    {
                        return;
                    }

                    if (File.Exists(filename))
                    {
                        if (DialogResult.Cancel == MessageBox.Show("File already exists, it will be DELETED! Press OK to continue", "About to overwrite a nanddump", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                        {
                            Console.WriteLine("xFlasher: Cancelled");
                            Console.WriteLine("");
                            return;
                        };
                    }

                    variables.reading = true;
                    MainForm.mainForm.xFlasherBusy(1);
                    Console.WriteLine("xFlasher: Reading Nand to {0}", filename);

                    Thread blocksThread = new Thread(() =>
                    {
                        int len = size * 64;
                        if (length > 0) len = length;
                        getBlocks(startblock, len);
                    });

                    inUse = true;
                    blocksThread.Start();

                    int result = spi(1, size, filename, startblock, length);

                    inUseTimer.Enabled = false;
                    inUseCount = 0;
                    inUse = false;
                    variables.reading = false;
                    MainForm.mainForm.xFlasherBusy(0);

                    if (result == -1)
                    {
                        Console.WriteLine("xFlasher: Aborted!");
                        Console.WriteLine("");
                        return;
                    }
                    else if (result == 0)
                    {
                        Console.WriteLine("xFlasher: Read Successful! Time Elapsed: {0}", xFlasherTimeString);
                        Console.WriteLine("");

                        SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                        if (variables.soundsuccess != "") success.SoundLocation = variables.soundsuccess;
                        success.Play();
                    }
                    else if (result == -2)
                    {
                        Console.WriteLine("xFlasher: Device Not Initialized");
                        Console.WriteLine("");
                        return;
                    }
                    else if (result == -3)
                    {
                        Console.WriteLine("xFlasher: Console Not Found");
                        Console.WriteLine("");
                        return;
                    }
                    else if (result == -4)
                    {
                        Console.WriteLine("xFlasher: Unknown Nand");
                        Console.WriteLine("");
                        return;
                    }
                    else if (result == -11)
                    {
                        Console.WriteLine("xFlasher: Couldn't Open File");
                        Console.WriteLine("");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("xFlasher: Unknown Error");
                        Console.WriteLine("");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (variables.debugme) Console.WriteLine(ex.ToString());
                    Console.WriteLine("");
                }
            });
            ftdiThread.Start();
        }

        // Write XeLL/ECC/Nand
        public void writeXeLLAuto()
        {
            if (String.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;

            if (Path.GetExtension(variables.filename1) == ".ecc")
            {
                Console.WriteLine("xFlasher: You need an .bin image");
                return;
            }

            writeNand(16, variables.filename1, 2);
        }

        public void writeEccAuto()
        {
            if (String.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;

            if (Path.GetExtension(variables.filename1) != ".ecc")
            {
                Console.WriteLine("xFlasher: You need an .ecc image");
                return;
            }

            writeNand(16, variables.filename1, 1);
        }

        public void writeNandAuto()
        {
            if (String.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;

            if (Path.GetExtension(variables.filename1) == ".ecc")
            {
                Console.WriteLine("xFlasher: You need an .bin image");
                return;
            }

            double len = new FileInfo(variables.filename1).Length;
            if (len == 50331648)
            {
                MessageBox.Show("Unable to write eMMC type image in SPI mode\n\nPlease switch to eMMC mode", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (len == 553648128)
            {
                variables.nandsizex = Nandsize.S512;
                writeNand(512, variables.filename1);
            }
            else if (len == 276824064)
            {
                variables.nandsizex = Nandsize.S256;
                writeNand(256, variables.filename1);
            }
            else if (len == 69206016)
            {
                variables.nandsizex = Nandsize.S64;
                writeNand(64, variables.filename1);
            }
            else if (len == 17301504)
            {
                variables.nandsizex = Nandsize.S16;
                writeNand(16, variables.filename1);
            }
            else if (len == 1351680)
            {
                variables.nandsizex = Nandsize.S16;
                writeNand(16, variables.filename1, 3);
            }
            else
            {
                MessageBox.Show("Nand is not a valid size", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void writeNand(int size, string filename, int mode = 0, int startblock = 0, int length = 0, bool skipboardcheck = false)
        {
            if (!osCheck()) return;

            if (waiting) return;

            if (filename != "erase")
            {
                if (String.IsNullOrWhiteSpace(filename)) return;
                if (!File.Exists(filename)) return;
            }

            if (inUse)
            {
                Console.WriteLine("xFlasher: Device Is Busy");
                return;
            }

            Thread ftdiThread = new Thread(() =>
            {
                try
                {

                    if (!ready)
                    {
                        waiting = true;
                        MainForm.mainForm.xFlasherBusy(-2);
                        Console.WriteLine("xFlasher: Waiting for device to become ready");
                    }
                    while (!ready)
                    {
                        // Do nothing and wait
                    }

                    if (getFlashConfigActual(true) != 0)
                    {
                        return;
                    }

                    if (mode == 0 && filename != "erase" && !skipboardcheck)
                    {
                        if (flashconf == "00023010" || flashconf == "00043000" || flashconf == "01198010")
                        {
                            if (size != 16)
                            {
                                if (DialogResult.No == MessageBox.Show("You are attempting to write a " + size + "MB Nand to a board with a 16MB Flash Config.\n\nAre you sure that you want to do that?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                                {
                                    Console.WriteLine("xFlasher: Cancelled");
                                    Console.WriteLine("");
                                    return;
                                }
                            }
                        }
                        else if (flashconf == "01198030")
                        {
                            if (size != 64)
                            {
                                if (DialogResult.No == MessageBox.Show("You are attempting to write a " + size + "MB Nand to a board with a 64MB Flash Config.\n\nAre you sure that you want to do that?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                                {
                                    Console.WriteLine("xFlasher: Cancelled");
                                    Console.WriteLine("");
                                    return;
                                }
                            }
                        }
                        else if (flashconf == "008A3020" || flashconf == "00AA3020")
                        {
                            if (size == 16)
                            {
                                if (DialogResult.No == MessageBox.Show("You are attempting to write a " + size + "MB Nand to a board with a 64/256/512MB Flash Config.\n\nAre you sure that you want to do the things?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                                {
                                    Console.WriteLine("xFlasher: Cancelled");
                                    Console.WriteLine("");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (DialogResult.No == MessageBox.Show("I think the correct size is " + size + "MB.\n\nDo you want to continue as " + size + "MB?", "Unrecognized Flash Config", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                            {
                                MainForm.mainForm.BeginInvoke((Action)(() => MainForm.mainForm.xFlasherNandSelShow(2))); // Ask
                                return;
                            }
                        }
                    }

                    variables.writing = true;

                    if (filename == "erase")
                    {
                        MainForm.mainForm.xFlasherBusy(3);
                        Console.WriteLine("xFlasher: Erasing Nand");
                    }
                    else
                    {
                        MainForm.mainForm.xFlasherBusy(2);
                        Console.WriteLine("xFlasher: Writing {0} to Nand", Path.GetFileName(filename));
                    }

                    Thread blocksThread = new Thread(() =>
                    {
                        if (mode >= 1) getBlocks(0, 80);
                        else
                        {
                            int len = size * 64;
                            if (length > 0) len = length;
                            getBlocks(startblock, len);
                        }
                    });

                    inUse = true;
                    blocksThread.Start();

                    int result;
                    if (filename == "erase")
                    {
                        result = spi(5, size, "erase", startblock, length);
                    }
                    else if (mode == 1)
                    {
                        result = spi(4, size, filename, startblock, length);
                    }
                    else
                    {
                        result = spi(3, size, filename, startblock, length);
                    }

                    inUseTimer.Enabled = false;
                    inUseCount = 0;
                    inUse = false;
                    variables.writing = false;
                    MainForm.mainForm.xFlasherBusy(0);

                    if (result == -1)
                    {
                        Console.WriteLine("xFlasher: Aborted!");
                        Console.WriteLine("");
                        return;
                    }
                    else if (result == 0)
                    {
                        if (filename == "erase") Console.WriteLine("xFlasher: Erase Successful! Time Elapsed: {0}", xFlasherTimeString);
                        else Console.WriteLine("xFlasher: Write Successful! Time Elapsed: {0}", xFlasherTimeString);
                        Console.WriteLine("");

                        SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                        if (variables.soundsuccess != "") success.SoundLocation = variables.soundsuccess;
                        success.Play();

                        if (mode >= 1)
                        {
                            Thread.Sleep(500);
                            MainForm.mainForm.xFlasherEccCleanup();
                        }
                    }
                    else if (result == -2)
                    {
                        Console.WriteLine("xFlasher: Device Not Initialized");
                        Console.WriteLine("");
                        return;
                    }
                    else if (result == -3)
                    {
                        Console.WriteLine("xFlasher: Console Not Found");
                        Console.WriteLine("");
                        return;
                    }
                    else if (result == -4)
                    {
                        Console.WriteLine("xFlasher: Unknown Nand");
                        Console.WriteLine("");
                        return;
                    }
                    else if (result == -11)
                    {
                        Console.WriteLine("xFlasher: Couldn't Open File");
                        Console.WriteLine("");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("xFlasher: Unknown Error");
                        Console.WriteLine("");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (variables.debugme) Console.WriteLine(ex.ToString());
                    Console.WriteLine("");
                }
            });
            ftdiThread.Start();
        }

        private void getBlocks(int start, int length)
        {
            int blocks;
            while (inUse)
            {
                blocks = spiGetBlocks();
                if (blocks >= 0)
                {
                    if (!inUseTimer.Enabled)
                    {
                        xFlasherTimeString = "< 1 sec(s)"; // If it doesn't update at least once, time was less than 1 second
                        inUseTimer.Enabled = true;
                    }
                    MainForm.mainForm.xFlasherBlocksUpdate(blocks.ToString("X"), ((blocks - start) * 100) / length);
                }
                else MainForm.mainForm.xFlasherBlocksUpdate("Initializing", 0);
                Thread.Sleep(40);
            }
        }

        // SVF Flashing
        public void flashSvf(string filename)
        {
            if (!osCheck()) return;

            if (waiting) return;

            if (inUse)
            {
                Console.WriteLine("xFlasher: Device Is Busy");
                return;
            }

            if (Process.GetProcessesByName("jtag").Length > 0)
            {
                Console.WriteLine("xFlasher: SVF software is already running, Can Not Continue");
                return;
            }

            Thread urJtagThread = new Thread(() =>
            {
                try
                {

                    if (!ready)
                    {
                        waiting = true;
                        MainForm.mainForm.xFlasherBusy(-2);
                        Console.WriteLine("xFlasher: Waiting for device to become ready");
                    }
                    while (!ready)
                    {
                        // Do nothing and wait
                    }

                    if (!File.Exists(filename))
                    {
                        Console.WriteLine("xFlasher: File Not Found: {0}", filename);
                        return;
                    }
                    if (Path.GetExtension(filename) != ".svf")
                    {
                        Console.WriteLine("xFlasher: Wrong File Type: {0}", filename);
                        return;
                    }

                    try
                    {
                        Directory.CreateDirectory(svfRoot);
                        if (File.Exists(svfPath))
                        {
                            File.Delete(svfPath);
                        }
                        File.Copy(filename, svfPath);
                    }
                    catch
                    {
                        Console.WriteLine("xFlasher: Could not open temporary file for flashing");
                        Console.WriteLine("xFlasher: {0} is locked by another process", svfPath);
                        return;
                    }

                    Console.WriteLine("xFlasher: Flashing {0} via JTAG", Path.GetFileName(filename));

                    Process psi = new Process();
                    psi.StartInfo.FileName = @"common/xflasher/jtag.exe";
                    psi.StartInfo.CreateNoWindow = true;
                    psi.StartInfo.UseShellExecute = false;
                    psi.StartInfo.RedirectStandardOutput = true;
                    psi.StartInfo.RedirectStandardInput = true;
                    psi.StartInfo.RedirectStandardError = true;

                    inUse = true;
                    psi.Start();

                    StreamWriter wr = psi.StandardInput;
                    StreamReader rr = psi.StandardOutput;

                    wr.WriteLine("cable ft2232");
                    wr.WriteLine("detect");
                    wr.WriteLine("svf " + svfPath + " progress");
                    wr.WriteLine("quit");
                    wr.Flush();
                    wr.Close();

                    string str = "";
                    str = "--";
                    str += rr.ReadToEnd().Replace("\n", "\r\n");

                    if (str.Length >= 4)
                    {
                        str = str.Remove(str.Length - 4, 4);
                    }

                    string strLower = str.ToLower();
                    inUse = false;

                    if (strLower.Contains("99%"))
                    {
                        int start = str.IndexOf("Part(0):") + 8;
                        int end = str.IndexOf("Stepping:") - start;

                        if (start <= 0 || end <= 0)
                        {
                            Console.WriteLine("xFlasher: Failed to detect CPLD type");
                        }
                        else
                        {
                            jtagdevice = str.Substring(start, end).Trim().Replace("\r\n", "");
                            Console.WriteLine("xFlasher: {0} Detected", jtagdevice);
                        }

                        Console.WriteLine("xFlasher: SVF Flash Successful!");
                        Console.WriteLine("");
                        SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                        if (variables.soundsuccess != "") success.SoundLocation = variables.soundsuccess;
                        success.Play();
                    }
                    else if (strLower.Contains("chain without any parts") == true)
                    {
                        Console.WriteLine("xFlasher: Could not connect to CPLD");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine("xFlasher: SVF Flash Failed");
                        Console.WriteLine("");
                    }

                    if (File.Exists(svfPath))
                    {
                        File.Delete(svfPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (variables.debugme) Console.WriteLine(ex.ToString());
                    Console.WriteLine("");
                }
            });
            urJtagThread.Start();
        }
    }
}
