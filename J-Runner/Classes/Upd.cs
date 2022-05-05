using Ionic.Zip;
using System;
using System.ComponentModel;
using System.IO;
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
        static string changelog = "Could not retrieve changelog for some reason!"; // Overwritten if successful
        static string deltaUrl, fullUrl;
        static int serverRevision = 0;
        static int minDeltaRevision = 0;
        static string expectedDeltaMd5 = "";
        static string expectedFullMd5 = "";
        static WebClient wc = null;
        static UpdateDownload updateDownload = null;
        static XmlTextReader xml = null;

        public static void check()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; // Enable TLS1.2 to connect to GitHub
            UpdateCheck updateCheck = new UpdateCheck();
            updateCheck.Show();

            try
            {
                xml = new XmlTextReader("http://raw.githubusercontent.com/Octal450/J-Runner-with-Extras/master/autoupdate.xml");
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
                        checkStatus = 1; // Defaults success
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
            updateCheck.Dispose();

            if (checkStatus == 0)
            {
                if (variables.revision >= serverRevision) // Up to Date
                {
                    upToDate = true;
                    Application.Run(new MainForm());
                }
                else
                {
                    upToDate = false;

                    if (MessageBox.Show("Updates are available for J-Runner with Extras\n\n" + changelog + "\n\nWould you like to download and install the update?", "J-Runner with Extras", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
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
                            wc.DownloadFileCompleted += delta;
                            wc.DownloadFileAsync(new Uri(deltaUrl), "delta.zip");
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
                            wc.DownloadFileCompleted += full;
                            wc.DownloadFileAsync(new Uri(fullUrl), "full.zip");
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
                updateDownload.Dispose();
                failedReason = "Failed to download the package";
                Application.Run(new UpdateFailed());
            }
            else
            {
                install(true);
            }
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
                updateDownload.Dispose();
                failedReason = "Failed to download the package";
                Application.Run(new UpdateFailed());
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
                updateDownload.installMode();

                if (simpleCheckMD5(filename) != expectedMd5)
                {
                    if (File.Exists(filename)) File.Delete(filename);
                    updateDownload.Dispose();
                    failedReason = "Package checksum is invalid";
                    Application.Run(new UpdateFailed());
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
            }
            catch (Exception ex)
            {
                if (File.Exists(filename)) File.Delete(filename);
                updateDownload.Dispose();
                File.AppendAllText("Error.log", ex.ToString() + Environment.NewLine);
                failedReason = "Failed to extract and install the package";
                Application.Run(new UpdateFailed());
            }

            updateDownload.Dispose();
            Application.Run(new UpdateSuccess());
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
            wc.CancelAsync();
            Thread.Sleep(100);
            if (File.Exists(@"delta.zip")) File.Delete(@"delta.zip");
            if (File.Exists(@"full.zip")) File.Delete(@"full.zip");
            Application.ExitThread();
            Application.Exit();
        }
    }
}
