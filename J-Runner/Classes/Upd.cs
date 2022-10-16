using Ionic.Zip;
using JRunner.Forms;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace JRunner
{
    public static class Upd
    {
        public static int checkStatus = 0; // Default success
        public static bool upToDate = true; // Default true
        public static string failedReason = "Unknown";
        public static string changelog = "Could not retrieve changelog for some reason!"; // Overwritten if successful
        private static string deltaUrl, fullUrl;
        private static int serverRevision = 0;
        public static int minDeltaRevision = 0;
        private static string expectedDeltaMd5 = "";
        private static string expectedFullMd5 = "";
        public static bool deleteFolders = false;
        public static bool noUpdateChk = false;
        public static bool runFullUpdate = false;
        private static WebClient wc = null;
        private static UpdUI updUI = null;
        private static XmlTextReader xml = null;

        public static void check()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; // Enable TLS1.2 to connect to GitHub

            try
            {
                xml = new XmlTextReader("https://raw.githubusercontent.com/Octal450/J-Runner-with-Extras-Updater/master/autoupdate.xml");
                xml.MoveToContent();
                string name = "";

                if (xml.NodeType == XmlNodeType.Element && xml.Name == "jrunner")
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
                                        checkStatus = 1;
                                        throw new Exception(); // Cancel the rest and go to catch
                                    }
                                }
                                else if (key == "revision")
                                {
                                    if (!int.TryParse(xml.Value, out serverRevision))
                                    {
                                        checkStatus = 1;
                                        throw new Exception(); // Cancel the rest and go to catch
                                    }
                                }
                                else if (key == "changelog-multi")
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
                                else if (key == "md5-delta")
                                {
                                    expectedDeltaMd5 = xml.Value;
                                }
                                else if (key == "md5-full")
                                {
                                    expectedFullMd5 = xml.Value;
                                }
                            }
                        }
                    }

                    if (serverRevision == 0) // If this happened we didn't get revision sucessfully, there is never revision 0
                    {
                        checkStatus = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("SSL/TLS")) checkStatus = 2;
                else checkStatus = 1;
            }
            finally
            {
                if (xml != null)
                {
                    xml.Close();
                }
            }

            Thread.Sleep(100);
            MainForm.mainForm.splash.BeginInvoke(new Action(() =>
            {
                MainForm.mainForm.splash.Hide();
            }));
            updUI = new UpdUI();

            if (checkStatus == 0)
            {
                if (runFullUpdate)
                {
                    startFull();
                }
                else if (variables.revision >= serverRevision) // Up to Date
                {
                    upToDate = true;
                    MainForm.mainForm.startMainForm(true);
                }
                else
                {
                    upToDate = false;

                    MainForm.mainForm.splash.BeginInvoke(new Action(() =>
                    {
                        UpdChangelog updChg = new UpdChangelog();
                        updChg.Show();
                        updChg.showChangelog(changelog);
                    }));
                }
            }
            else
            {
                if (runFullUpdate)
                {
                    if (checkStatus == 2) failedReason = "Could not connect to the update server because TLS1.2 is not enabled.";
                    else failedReason = "Could not connect to the update server.";
                    showUpdUI(1);
                }
                else
                {
                    MainForm.mainForm.startMainForm(true);
                }
            }
        }

        public static void startDelta()
        {
            Thread updateDelta = new Thread(() =>
            {
                if (File.Exists(@"delta.zip")) File.Delete(@"delta.zip");

                wc = new WebClient();
                wc.DownloadProgressChanged += updUI.updateProgress;
                wc.DownloadFileCompleted += delta;
                wc.DownloadFileAsync(new Uri(deltaUrl), "delta.zip");
            });
            showUpdUI();
            updateDelta.Start();
        }

        private static void delta(object sender, AsyncCompletedEventArgs e)
        {
            wc.Dispose();

            if (e.Cancelled)
            {
                // Do nothing
            }
            else if (e.Error != null)
            {
                if (File.Exists(@"delta.zip")) File.Delete(@"delta.zip");
                if (e.Error.ToString().Contains("SSL/TLS")) failedReason = "Could not connect to the update server because TLS1.2 is not enabled.";
                else failedReason = "Failed to download the package.";
                setUpdUIPage(1);
            }
            else
            {
                install(true);
            }
        }

        public static void startFull()
        {
            Thread updateFull = new Thread(() =>
            {
                if (File.Exists(@"full.zip")) File.Delete(@"full.zip");

                if (deleteFolders)
                {
                    Thread.Sleep(500); // Make sure all files are released

                    try
                    {
                        if (Directory.Exists("common")) Directory.Delete("common", true);
                        if (Directory.Exists("xeBuild")) Directory.Delete("xeBuild", true);
                    }
                    catch
                    {
                        failedReason = "Failed to cleanup the filesystem.";
                        setUpdUIPage(1);
                    }
                }

                wc = new WebClient();
                wc.DownloadProgressChanged += updUI.updateProgress;
                wc.DownloadFileCompleted += full;
                wc.DownloadFileAsync(new Uri(fullUrl), "full.zip");
            });
            showUpdUI();
            updateFull.Start();
        }

        private static void full(object sender, AsyncCompletedEventArgs e)
        {
            wc.Dispose();

            if (e.Cancelled)
            {
                // Do nothing
            }
            else if (e.Error != null)
            {
                if (File.Exists(@"full.zip")) File.Delete(@"full.zip");
                if (e.Error.ToString().Contains("SSL/TLS")) failedReason = "Could not connect to the update server because TLS1.2 is not enabled.";
                else failedReason = "Failed to download the package.";
                setUpdUIPage(1);
            }
            else
            {
                install();
            }
        }

        private static void install(bool delta = false)
        {
            string filename;
            if (delta) filename = @"delta.zip";
            else filename = @"full.zip";

            string expectedMd5;
            if (delta) expectedMd5 = expectedDeltaMd5;
            else expectedMd5 = expectedFullMd5;

            try
            {
                updUI.installMode();

                if (simpleCheckMD5(filename) != expectedMd5)
                {
                    if (File.Exists(filename)) File.Delete(filename);
                    failedReason = "Package checksum is invalid.";
                    setUpdUIPage(1);
                    return;
                }

                // Install Package
                File.Move(AppDomain.CurrentDomain.FriendlyName, @"JRunner.exe.old");

                using (ZipFile zip = ZipFile.Read(filename))
                {
                    zip.ExtractAll(Environment.CurrentDirectory, ExtractExistingFileAction.OverwriteSilently);
                }
                File.Delete(filename);

                if (AppDomain.CurrentDomain.FriendlyName != "JRunner.exe")
                {
                    if (File.Exists("JRunner.exe")) File.Move("JRunner.exe", AppDomain.CurrentDomain.FriendlyName);
                }

                setUpdUIPage(0);
            }
            catch (Exception ex)
            {
                if (File.Exists(filename)) File.Delete(filename);
                File.AppendAllText("Error.log", ex.ToString() + Environment.NewLine);
                failedReason = "Failed to extract and install the package.";
                setUpdUIPage(1);
            }
        }

        private static string simpleByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }

        private static string simpleCheckMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    string md5str;
                    md5str = simpleByteArrayToString(md5.ComputeHash(stream));
                    stream.Dispose();
                    return md5str;
                }
            }
        }

        public static void cancel()
        {
            try
            {
                wc.CancelAsync();
            }
            catch { }

            Thread.Sleep(100);

            try
            {
                if (File.Exists(@"delta.zip")) File.Delete(@"delta.zip");
                if (File.Exists(@"full.zip")) File.Delete(@"full.zip");
            }
            catch { }

            Application.ExitThread();
            Application.Exit();
        }

        public static void restoreFiles()
        {
            Thread worker = new Thread(() =>
            {
                try
                {
                    ProcessStartInfo jr = new ProcessStartInfo();
                    jr.FileName = "JRunner.exe";
                    jr.Arguments = "/restorefiles";
                    jr.UseShellExecute = true;

                    Process.Start(jr);
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not restore files due to the following error:");
                    Console.WriteLine(ex.ToString());
                }
            });
            worker.Start();
        }

        private static void showUpdUI(int type = 0)
        {
            MainForm.mainForm.splash.BeginInvoke(new Action(() =>
            {
                updUI.Show();
                if (type == 1) updUI.showFailed();
                MainForm.mainForm.splash.Dispose();
            }));
        }

        private static void setUpdUIPage(int type)
        {
            updUI.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (type == 0) updUI.showSuccess();
                    else if (type == 1) updUI.showFailed();
                }
                catch
                {
                    MessageBox.Show("A critical error has occurred!\n\nUpdate UI operation out of sequence\n\nPlease report this to the developers", "Something happened!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cancel();
                }
            }));
        }
    }
}
