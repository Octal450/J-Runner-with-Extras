using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace JRunner
{
    public class Mtx_Usb
    {
        // Based on xFlasher system, added due to IoTimedOut issue
        private static int inUseCount = 0;
        public static string mtxTimeString = "";
        System.Timers.Timer inUseTimer;

        // Libraries
        public void inUseTimerSetup()
        {
            inUseTimer = new System.Timers.Timer(1000);
            inUseTimer.Elapsed += inUseTimerUpd;
        }

        private void inUseTimerUpd(object source, EventArgs e)
        {
            inUseCount++;

            if (inUseCount > 59) mtxTimeString = TimeSpan.FromSeconds(inUseCount).ToString(@"m\:ss") + " min(s)";
            else if (inUseCount >= 0) mtxTimeString = inUseCount + " sec(s)";
        }

        public void writeXeLLAuto()
        {
            if (String.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;

            if (Path.GetExtension(variables.filename1) == ".ecc")
            {
                Console.WriteLine("You need an .bin image");
                return;
            }

            writeNand(16, variables.filename1, 2, 0, 80); // startblock + length (hex) for display purposes only, not required
        }

        public void writeEccAuto()
        {
            if (String.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;

            if (Path.GetExtension(variables.filename1) != ".ecc")
            {
                Console.WriteLine("You need an .ecc image");
                return;
            }

            writeNand(16, variables.filename1, 1, 0, 80); // startblock + length (hex) for display purposes only, not required
        }

        public void writeNandAuto()
        {
            if (String.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;

            if (Path.GetExtension(variables.filename1) == ".ecc")
            {
                Console.WriteLine("You need an .bin image");
                return;
            }

            double len = new FileInfo(variables.filename1).Length;
            if (len == 50331648)
            {
                MessageBox.Show("Unable to write eMMC type image with an SPI tool\n\nPlease use an eMMC tool", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public void writeNand(int size, string filename, int mode = 0, int startblock = 0, int length = 0)
        {
            if (NandX.InUse)
            {
                Console.WriteLine("MTX USB: Device Is Busy");
                return;
            }

            Thread nandThread = new Thread(() =>
            {
                try
                {
                    variables.writing = true;
                    MainForm.mainForm.mtxBusy(1);

                    Console.WriteLine("Writing {0} to Nand via NandPro", Path.GetFileName(filename));
                    inUseTimer.Enabled = true;

                    string slArg = "";
                    if (startblock > 0 || length > 0) {
                        slArg = " " + startblock.ToString("X") + " " + length.ToString("X");
                    }

                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = "common/mtx-tools/NandPro2e.exe";
                    if (mode == 1) process.StartInfo.Arguments = "usb: +w" + size + " \"" + filename + "\"" + slArg;
                    else process.StartInfo.Arguments = "usb: -w" + size + " \"" + filename + "\"" + slArg;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.WorkingDirectory = Path.Combine(variables.pathforit, "common/mtx-tools");
                    process.StartInfo.CreateNoWindow = false;

                    NandX.InUse = true;
                    process.Start();
                    process.WaitForExit();

                    inUseTimer.Enabled = false;
                    inUseCount = 0;
                    NandX.InUse = false;
                    variables.writing = false;
                    MainForm.mainForm.mtxBusy(0);
                    Console.WriteLine("NandPro: Completed! Time Elapsed: {0}", mtxTimeString);
                    Console.WriteLine("");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (variables.debugme) Console.WriteLine(ex.ToString());
                    Console.WriteLine("");
                }
            });
            nandThread.Start();
        }

        public void flashXsvf(string filename)
        {
            if (NandX.InUse)
            {
                Console.WriteLine("MTX USB: Device Is Busy");
                return;
            }

            Thread xsvfThread = new Thread(() =>
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = "common/mtx-tools/xsvf/xsvf.exe";
                    process.StartInfo.Arguments = "\"" + filename + "\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.WorkingDirectory = variables.pathforit;
                    process.StartInfo.CreateNoWindow = false;

                    NandX.InUse = true;
                    process.Start();
                    process.WaitForExit();

                    NandX.InUse = false;
                    Console.WriteLine("Xsvf: Completed!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (variables.debugme) Console.WriteLine(ex.ToString());
                    Console.WriteLine("");
                }
            });
            xsvfThread.Start();
        }
    }
}
