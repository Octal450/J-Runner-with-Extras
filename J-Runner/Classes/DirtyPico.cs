using System;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace JRunner
{
    public class DirtyPico
    {

        public string svfPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"SVF\\TimingSvfTemp.svf");
        public string svfRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"SVF");

        public bool ready = false;
        public bool inUse = false;
        public bool waiting = false;
        private string flashconf = "";
        private string jtagdevice = "";
        public int selType = 0;

        private static int initCount = 0;
        private static int inUseCount = 0;
        public static string xFlasherTimeString = "";
        System.Windows.Threading.DispatcherTimer initTimer;
        System.Timers.Timer inUseTimer;

        // SVF Flashing
        public void flashSvf(string filename)
        {
            if (inUse || waiting) return;

            if (Process.GetProcessesByName("jtagDirtyPico").Length > 0)
            {
                Console.WriteLine("DirtyPico: SVF software is already running!");
                return;
            }

            Thread urJtagThread = new Thread(() =>
            {
                try
                {

                    //if (!ready)
                   // {
                    //    waiting = true;
                    //    MainForm.mainForm.xFlasherBusy(-2);
                    //    Console.WriteLine("DirtyPico: Waiting for device to become ready");
                  //  }
                 //   while (!ready)
                 //   {
                        // Do nothing and wait
                  //  }

                    if (!File.Exists(filename))
                    {
                        Console.WriteLine("DirtyPico: File Not Found: {0}", filename);
                        return;
                    }
                    if (Path.GetExtension(filename) != ".svf")
                    {
                        Console.WriteLine("DirtyPico: Wrong File Type: {0}", filename);
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
                        Console.WriteLine("DirtyPico: Could not open temporary file for flashing");
                        Console.WriteLine("DirtyPico: {0} is locked by another process", svfPath);
                        return;
                    }

                    Console.WriteLine("DirtyPico: Flashing {0} via JTAG", Path.GetFileName(filename));

                    Process psi = new Process();
                    psi.StartInfo.FileName = @"common/dirtypico/jtagDirtyPico.exe";
                    psi.StartInfo.CreateNoWindow = false;
                    psi.StartInfo.UseShellExecute = false;
                    psi.StartInfo.RedirectStandardOutput = true;
                    psi.StartInfo.RedirectStandardInput = true;
                    psi.StartInfo.RedirectStandardError = true;

                    inUse = true;
                    psi.Start();

                    StreamWriter wr = psi.StandardInput;
                    StreamReader rr = psi.StandardOutput;

                    wr.WriteLine("cable dirtyjtag");
                    wr.WriteLine("detect");
                    wr.WriteLine("svf " + svfPath);
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
                            Console.WriteLine("DirtyPico: Failed to detect CPLD type");
                        }
                        else
                        {
                            jtagdevice = str.Substring(start, end).Trim().Replace("\r\n", "");
                            Console.WriteLine("xFlasher: {0} Detected", jtagdevice);
                        }

                        Console.WriteLine("DirtyPico: SVF Flash Successful!");
                        Console.WriteLine("");

                        if (variables.playSuccess)
                        {
                            SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                            success.Play();
                        }
                    }
                    else if (strLower.Contains("chain without any parts") == true)
                    {
                        Console.WriteLine("DirtyPico: Could not connect to CPLD");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine("DirtyPico: SVF Flash Failed");
                        Console.WriteLine("");
                    }

                    if (File.Exists(svfPath))
                    {
                        File.Delete(svfPath);
                    }
                }
                catch (Exception ex)
                {
                    inUse = false;

                    Console.WriteLine(ex.Message);
                    if (variables.debugMode) Console.WriteLine(ex.ToString());
                    Console.WriteLine("");
                }
            });
            urJtagThread.Start();
        }
    }
}
