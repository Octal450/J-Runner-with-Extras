using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace JRunner
{
    static class Program
    {
        [DllImport("Shcore.dll")]
        static extern int SetProcessDpiAwareness(int PROCESS_DPI_AWARENESS);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        static bool createdNew = true;
        static Mutex mutex = new Mutex(true, "J-Runner", out createdNew);
        static bool needVcredistx86 = false;
        static object vcredistReg;

        // Arguments
        public static bool noVcredistChk = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomain_UnhandledException;

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            if (createdNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Determine current Windows version
                if (Environment.OSVersion.Version.Major >= 10) variables.currentOS = variables.Windows.W10_11;
                else if (Environment.OSVersion.Version.Major >= 6)
                {
                    if (Environment.OSVersion.Version.Minor >= 3) variables.currentOS = variables.Windows.Win81;
                    else if (Environment.OSVersion.Version.Minor == 2) variables.currentOS = variables.Windows.Win8;
                    else if (Environment.OSVersion.Version.Minor == 1) variables.currentOS = variables.Windows.Win7;
                    else variables.currentOS = variables.Windows.Vista;
                }
                else if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1) variables.currentOS = variables.Windows.XP;

                if (variables.currentOS == variables.Windows.XP)
                {
                    MessageBox.Show("This version of Windows is not supported\n\nJ-Runner with Extras requires Microsoft Windows Vista or later", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // Shcore.dll doesn't exist on pre Windows 8.1, these OS do not require this to upscale the UI anyways
                if (variables.currentOS == variables.Windows.Win81 || variables.currentOS == variables.Windows.W10_11)
                {
                    SetProcessDpiAwareness(2); // PerMonitorAware
                }

                if (File.Exists(@"JRunner.exe.old"))
                {
                    File.Delete(@"JRunner.exe.old");
                }

                foreach (string a in args)
                {
                    if (a.Contains("/novcredist"))
                    {
                        noVcredistChk = true;
                    }
                    if (a.Contains("/noupdate"))
                    {
                        Upd.noUpdateChk = true;
                    }
                    if (a.Contains("/fullupdate"))
                    {
                        Upd.runFullUpdate = true;
                    }
                    if (a.Contains("/restorefiles"))
                    {
                        Upd.runFullUpdate = true;
                        Upd.deleteFolders = true;
                    }
                }

                if (!Upd.runFullUpdate)
                {
                    if (!Directory.Exists("common") || !Directory.Exists("xeBuild"))
                    {
                        if (MessageBox.Show("Critical support files required for correct operation are missing\n\nDo you want to download the required support files?\n\nAll files inside common and xeBuild will be deleted and replaced with clean versions!", "Missing Files", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            Upd.runFullUpdate = true;
                        }
                    }
                }

                if (Upd.runFullUpdate)
                {
                    Application.Run(new MainForm()); // Updater takes it from here
                }
                else
                {
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
                            catch
                            {
                                MessageBox.Show("Dependency installer failed to launch for some reason", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(0);
                            }
                        });
                        Vcredist.Start();
                        return;
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

        public static void restart()
        {
            Process.Start(Application.ExecutablePath);
            Environment.Exit(0);
        }

        public static void exit()
        {
            foreach (var process in Process.GetProcessesByName("jtag"))
            {
                process.Kill();
            }
            try
            {
                mutex.ReleaseMutex();
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
            File.AppendAllText(Path.Combine(variables.rootfolder, "Error.log"), e.ExceptionObject.ToString() + Environment.NewLine);
        }

        public static float getScalingFactor()
        {
            float dpiX = 96;
            try
            {
                using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
                {
                    dpiX = graphics.DpiX;
                }
            }
            catch { }
            return dpiX / 96;
        }

        private static void checkVcredist()
        {
            if (noVcredistChk)
            {
                needVcredistx86 = false;
                return;
            }

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
