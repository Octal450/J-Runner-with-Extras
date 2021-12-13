using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Ionic.Zip;
using Microsoft.Win32;
using System.ComponentModel;

namespace JRunner
{
    static class Program
    {
        static bool createdNew = true;
        static string deltaUrl, fullUrl;
        static int serverRevision = 0;
        static int minDeltaRevision = 0;
        static string changelog = "Could not retrieve changelog for some reason!"; // Overwritten if successful
        static Mutex mutex = new Mutex(true, "J-Runner", out createdNew);
        static bool needVcredistx86 = false;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        static WebClient wc = null;
        static UpdateDownload updateDownload = null;
        static XmlTextReader xml = null;
        static object vcredistReg;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomain_UnhandledException;

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            if (createdNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (File.Exists(@"JRunner.exe.old"))
                {
                    File.Delete(@"JRunner.exe.old");
                }

                checkVcredist();

                if (needVcredistx86)
                {
                    MessageBox.Show("Microsoft Visual C++ 2010 Redistributable is required for J-Runner with Extras and some of its components to work correctly\n\nClick OK to begin the installation", "Dependency Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Thread Vcredist = new Thread(() =>
                    {
                        try
                        {
                            ProcessStartInfo vcredistx86 = new ProcessStartInfo("common\\xflasher\\vcredist_x86.exe");
                            vcredistx86.WorkingDirectory = Environment.CurrentDirectory;
                            vcredistx86.UseShellExecute = true;
                            if (Environment.OSVersion.Version.Major > 5) vcredistx86.Verb = "runas";
                            Process vcredistx86p = Process.Start(vcredistx86);
                            vcredistx86p.WaitForExit();
                            checkVcredist(); // Check if install succeeded
                            if (!needVcredistx86) Process.Start(Application.ExecutablePath); // Only start if success
                            Environment.Exit(0);
                        }
                        catch {
                            MessageBox.Show("Dependency installer failed to launch for some reason", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(0);
                        }
                    });
                    Vcredist.Start();
                    return;
                }
                else
                {
                    UpdateCheck updateCheck = new UpdateCheck();
                    updateCheck.Show();

                    try
                    {
                        xml = new XmlTextReader("https://cdn.octalsconsoleshop.com/jrunner/autoupdate.xml");
                        xml.MoveToContent();
                        string name = "";

                        if ((xml.NodeType == XmlNodeType.Element) && (xml.Name == "jrunner"))
                        {
                            while (xml.Read())
                            {
                                if (xml.NodeType == XmlNodeType.Element)
                                {
                                    name = xml.Name;
                                }
                                else
                                {
                                    string key;
                                    if (xml.NodeType == XmlNodeType.Text && xml.HasValue && (key = name) != null)
                                    {
                                        if (key == "min-delta-revision")
                                        {
                                            if (!int.TryParse(xml.Value, out minDeltaRevision))
                                            {
                                                variables.updatechecksuccess = false; // Defaults true
                                                throw new Exception(); // Cancel the rest and go to catch
                                            }
                                        }
                                        else if (key == "revision")
                                        {
                                            if (!int.TryParse(xml.Value, out serverRevision))
                                            {
                                                variables.updatechecksuccess = false; // Defaults true
                                                throw new Exception(); // Cancel the rest and go to catch
                                            }
                                        }
                                        else if (key == "changelog")
                                        {
                                            changelog = xml.Value;
                                        }
                                        else if (key == "delta")
                                        {
                                            deltaUrl = xml.Value;
                                        }
                                        else if (key == "full")
                                        {
                                            fullUrl = xml.Value;
                                        }
                                    }
                                }
                            }

                            if (serverRevision == 0) // If this happened we didn't get revision sucessfully, there is never revision 0
                            {
                                variables.updatechecksuccess = false; // Defaults true
                            }
                        }
                    }
                    catch
                    {
                        variables.updatechecksuccess = false; // Defaults true
                    }
                    finally
                    {
                        if (xml != null)
                        {
                            xml.Close();
                        }
                    }

                    Thread.Sleep(100);
                    updateCheck.Dispose();

                    if (variables.updatechecksuccess)
                    {
                        if (variables.revision >= serverRevision) // Up to Date
                        {
                            variables.uptodate = true;
                            Application.Run(new MainForm());
                        }
                        else
                        {
                            variables.uptodate = false;

                            if (MessageBox.Show("Update for J-Runner with Extras is available.\n\n" + changelog + "\n\nWould you like to download the latest update?", "J-Runner with Extras Updater", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
                            {
                                // Do nothing and launch as normal
                                Application.Run(new MainForm());
                            }
                            else if (variables.revision >= minDeltaRevision) // Delta
                            {
                                updateDownload = new UpdateDownload();

                                Thread updateDelta = new Thread(() =>
                                {
                                    if (File.Exists(@"delta.zip")) File.Delete(@"delta.zip");

                                    wc = new WebClient();
                                    wc.DownloadProgressChanged += updateDownload.updateProgress;
                                    wc.DownloadFileCompleted += deltaUpdate;
                                    wc.DownloadFileAsync(new System.Uri(deltaUrl), "delta.zip");
                                });
                                updateDelta.Start();
                                Application.Run(updateDownload);
                            }
                            else // Full
                            {
                                updateDownload = new UpdateDownload();

                                Thread updateFull = new Thread(() =>
                                {
                                    if (File.Exists(@"full.zip")) File.Delete(@"full.zip");

                                    wc = new WebClient();
                                    wc.DownloadProgressChanged += updateDownload.updateProgress;
                                    wc.DownloadFileCompleted += fullUpdate;
                                    wc.DownloadFileAsync(new System.Uri(fullUrl), "full.zip");
                                });
                                updateFull.Start();
                                Application.Run(updateDownload);
                            }
                        }
                    }
                    else
                    {
                        Application.Run(new MainForm());
                    }
                }
            }
            else
            {
                Thread.Sleep(1000);
                Process current = Process.GetCurrentProcess();
                bool found = false;
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        found = true;
                        NativeMethods.SendMessage(
                            (IntPtr)NativeMethods.HWND_BROADCAST,
                            NativeMethods.WM_SHOWAPP,
                            IntPtr.Zero,
                            IntPtr.Zero);
                        SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
                if (!found) Process.Start(Application.ExecutablePath);
            }
        }

        private static void deltaUpdate(object sender, AsyncCompletedEventArgs e)
        {
            wc.Dispose();

            if (e.Cancelled)
            {
                // Do nothing
            }
            else if (e.Error != null)
            {
                if (File.Exists(@"delta.zip")) File.Delete(@"delta.zip");
                updateDownload.Dispose();
                Application.Run(new UpdateFailed());
            }
            else
            {
                try
                {
                    updateDownload.installMode();

                    File.Move(@"JRunner.exe", @"JRunner.exe.old");

                    // Unzip
                    using (ZipFile zip = ZipFile.Read(@"delta.zip"))
                    {
                        zip.ExtractAll(Environment.CurrentDirectory, ExtractExistingFileAction.OverwriteSilently);
                    }
                    File.Delete(@"delta.zip");
                }
                catch
                {
                    if (File.Exists(@"delta.zip")) File.Delete(@"delta.zip");
                    updateDownload.Dispose();
                    Application.Run(new UpdateFailed());
                }

                updateDownload.Dispose();
                Application.Run(new UpdateSuccess());
            }
        }

        private static void fullUpdate(object sender, AsyncCompletedEventArgs e)
        {
            wc.Dispose();

            if (e.Cancelled)
            {
                // Do nothing
            }
            else if (e.Error != null)
            {
                if (File.Exists(@"full.zip")) File.Delete(@"full.zip");
                updateDownload.Dispose();
                Application.Run(new UpdateFailed());
            }
            else
            {
                try
                {
                    updateDownload.installMode();

                    File.Move(@"JRunner.exe", @"JRunner.exe.old");

                    // Unzip
                    using (ZipFile zip = ZipFile.Read(@"full.zip"))
                    {
                        zip.ExtractAll(Environment.CurrentDirectory, ExtractExistingFileAction.OverwriteSilently);
                    }
                    File.Delete(@"full.zip");
                }
                catch
                {
                    if (File.Exists(@"full.zip")) File.Delete(@"full.zip");
                    updateDownload.Dispose();
                    Application.Run(new UpdateFailed());
                }

                updateDownload.Dispose();
                Application.Run(new UpdateSuccess());
            }
        }

        public static void cancelUpdate()
        {
            wc.CancelAsync();
            Thread.Sleep(100);
            if (File.Exists(@"delta.zip")) File.Delete(@"delta.zip");
            if (File.Exists(@"full.zip")) File.Delete(@"full.zip");
            Application.ExitThread();
            Application.Exit();
        }

        public static void restart()
        {
            Process.Start(Application.ExecutablePath);
            Environment.Exit(0);
        }

        public static void exit()
        {
            foreach (var process in Process.GetProcessesByName("FTDI2SPI"))
            {
                process.Kill();
            }
            foreach (var process in Process.GetProcessesByName("jtag"))
            {
                process.Kill();
            }
            try
            {
                mutex.ReleaseMutex();
                MainForm.mainForm.trayIcon.Visible = false;
                MainForm.mainForm.trayIcon.Dispose();
            }
            catch (Exception) { }
            Application.ExitThread();
            Application.Exit();
            Environment.Exit(0);
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            exit();
        }

        private static void OnCurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //MessageBox.Show(e.ExceptionObject.ToString());
            File.AppendAllText(Path.Combine(variables.pathforit, "Error.log"), e.ExceptionObject.ToString() + Environment.NewLine);
        }

        private static void checkVcredist()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                vcredistReg = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\VisualStudio\10.0\VC\VCRedist\x86", "Installed", null);
                if (vcredistReg != null)
                {
                    if (Convert.ToInt32(vcredistReg) == 0) needVcredistx86 = true;
                    else needVcredistx86 = false;
                }
                else needVcredistx86 = true;
            }
            else
            {
                vcredistReg = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\10.0\VC\VCRedist\x86", "Installed", null);
                if (vcredistReg != null)
                {
                    if (Convert.ToInt32(vcredistReg) == 0) needVcredistx86 = true;
                    else needVcredistx86 = false;
                }
                else needVcredistx86 = true;
            }
        }
    }
}
