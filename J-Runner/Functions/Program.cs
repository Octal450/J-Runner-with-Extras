using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace JRunner
{
    static class Program
    {
        static bool createdNew = true;

        static Mutex mutex = new Mutex(true, "J-Runner", out createdNew);
        static bool needVcredistx86 = false;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
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
                    Upd.check();
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
