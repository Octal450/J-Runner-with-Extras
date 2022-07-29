using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Windows;

namespace JRunner
{
    public static class Backup
    {
        public static bool scheduleBackup = false;

        public static string getBackupName()
        {
            string board = "";
            if (!string.IsNullOrWhiteSpace(variables.boardtype)) board = variables.boardtype;
            if (board.Contains("Corona")) board = "Corona";
            if (board.Contains("Trinity")) board = "Trinity";
            if (board.Contains("Jasper")) board = "Jasper";

            string serial = MainForm.mainForm.getNand().ki.serial;

            string date = DateTime.Now.ToString("MM-dd-yyyy");

            if (!string.IsNullOrWhiteSpace(serial))
            {
                if (variables.backupNaming == 1) return board + " " + serial + " " + date;
                else if (variables.backupNaming == 2) return serial;
                else return board + " " + serial;
            }
            else return "";
        }

        public static string getBackupPath()
        {
            string working = MainForm.mainForm.getCurrentWorkingFolder();
            if (variables.filename1.Contains(working))
            {
                return working;
            }
            else
            {
                string tempPath = Path.GetDirectoryName(variables.filename1);
                MessageBoxResult mbr = MessageBox.Show("Could not find an XeBuild folder for this nand\n\nDo you want to backup everything in " + tempPath, "No XeBuild Folder", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (mbr == MessageBoxResult.Yes)
                {
                    return tempPath;
                }
                else if (mbr == MessageBoxResult.No)
                {
                    CommonOpenFileDialog openDialog = new CommonOpenFileDialog();
                    openDialog.InitialDirectory = Oper.FilePickerInitialPath(tempPath);
                    openDialog.RestoreDirectory = false;
                    openDialog.IsFolderPicker = true;

                    if (openDialog.ShowDialog() == CommonFileDialogResult.Ok) return openDialog.FileName;
                    else return "";
                }
                else return "";
            }
        }

        public static void autoBackup()
        {
            scheduleBackup = false;
            string target;
            string path;

            try
            {
                DirectoryInfo rootInfo = new DirectoryInfo(variables.backupRoot);
                if (!rootInfo.Exists) Directory.CreateDirectory(variables.backupRoot);

                target = getBackupPath();
                path = Path.Combine(variables.backupRoot, getBackupName());
                string pathTest = Path.GetFullPath(path);
            }
            catch
            {
                MessageBox.Show("Backup failed because the path is not valid\n\nCheck that the folder set in Settings is correct and exists", "Can't", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(target) || string.IsNullOrEmpty(path)) // Should NEVER happen
            {
                MessageBox.Show("Backup encountered an unknown error\n\nBlank target or path", "Can't", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (variables.backupType == 1) backupToFolder(target, path);
            else backupToZip(target, path + ".zip");
        }

        public static void backupToZip(string target, string path, bool delIfExist = false)
        {
            Thread worker = new Thread(() =>
            {
                Forms.ProgressIndeterminate pi = new Forms.ProgressIndeterminate();

                MainForm.mainForm.BeginInvoke(new Action(() => {
                    MainForm.mainForm.Enabled = false;
                    pi.Show();
                    pi.updateTitle("Backing Up");
                }));
                
                try
                {
                    if (File.Exists(path))
                    {
                        if (delIfExist) // To remove name collusion
                        {
                            File.Delete(path);
                        }
                        else
                        {
                            FileInfo fileInfo = new FileInfo(path);
                            string folder = Path.GetDirectoryName(path);
                            string filename = Path.GetFileNameWithoutExtension(fileInfo.Name);
                            int number = 0;

                            if (filename.Contains("(") && filename.Contains(")"))
                            {
                                string nfilename = filename.Substring(0, filename.IndexOf(" ("));

                                do
                                {
                                    number++;
                                }
                                while (File.Exists(folder + "\\" + nfilename + " (" + number + ")" + fileInfo.Extension));

                                if (!File.Exists(folder + "\\" + nfilename + " (" + number + ")" + fileInfo.Extension))
                                {
                                    path = folder + "\\" + nfilename + " (" + number + ")" + fileInfo.Extension;
                                }
                            }
                            else
                            {
                                do
                                {
                                    number++;
                                }
                                while (File.Exists(folder + "\\" + Path.GetFileNameWithoutExtension(fileInfo.Name) + " (" + number + ")" + fileInfo.Extension));

                                if (!File.Exists(folder + "\\" + Path.GetFileNameWithoutExtension(fileInfo.Name) + " (" + number + ")" + fileInfo.Extension))
                                {
                                    path = folder + "\\" + Path.GetFileNameWithoutExtension(fileInfo.Name) + " (" + number + ")" + fileInfo.Extension;
                                }
                            }
                        }
                    }

                    ZipFile.CreateFromDirectory(target, path); // Assume its a directory
                    Console.WriteLine("ZIP Backup Saved: " + path);
                    Console.WriteLine("");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occured during backup: " + ex.GetType());
                    if (variables.debugMode) Console.WriteLine(ex);
                }

                MainForm.mainForm.BeginInvoke(new Action(() => {
                    MainForm.mainForm.Enabled = true;
                    pi.Close();
                }));
            });
            worker.Start();
        }

        public static void backupToFolder(string target, string path) // Does not include subfolders
        {
            Thread worker = new Thread(() =>
            {
                Forms.ProgressIndeterminate pi = new Forms.ProgressIndeterminate();

                MainForm.mainForm.BeginInvoke(new Action(() => {
                    MainForm.mainForm.Enabled = false;
                    pi.Show();
                    pi.updateTitle("Backing Up");
                }));

                try
                {
                    DirectoryInfo pathInfo = new DirectoryInfo(path);
                    if (!pathInfo.Exists) Directory.CreateDirectory(path);

                    List<string> files = Directory.GetFiles(target, "*.*", SearchOption.TopDirectoryOnly).ToList();

                    foreach (string file in files)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        if (new FileInfo(pathInfo + "\\" + fileInfo.Name).Exists == false) // To remove name collusion
                        {
                            fileInfo.CopyTo(pathInfo + "\\" + fileInfo.Name);
                        }
                        else
                        {
                            string filename = Path.GetFileNameWithoutExtension(fileInfo.Name);
                            int number = 0;

                            if (filename.Contains("(") && filename.Contains(")"))
                            {
                                string nfilename = filename.Substring(0, filename.IndexOf(" ("));

                                do
                                {
                                    number++;
                                }
                                while (File.Exists(pathInfo + "\\" + nfilename + " (" + number + ")" + fileInfo.Extension));

                                if (!File.Exists(pathInfo + "\\" + nfilename + " (" + number + ")" + fileInfo.Extension))
                                {
                                    fileInfo.CopyTo(pathInfo + "\\" + nfilename + " (" + number + ")" + fileInfo.Extension);
                                }
                            }
                            else
                            {
                                do
                                {
                                    number++;
                                }
                                while (File.Exists(pathInfo + "\\" + Path.GetFileNameWithoutExtension(fileInfo.Name) + " (" + number + ")" + fileInfo.Extension));

                                if (!File.Exists(pathInfo + "\\" + Path.GetFileNameWithoutExtension(fileInfo.Name) + " (" + number + ")" + fileInfo.Extension))
                                {
                                    fileInfo.CopyTo(pathInfo + "\\" + Path.GetFileNameWithoutExtension(fileInfo.Name) + " (" + number + ")" + fileInfo.Extension);
                                }
                            }
                        }
                    }

                    Console.WriteLine("Folder Backup Saved: " + path);
                    Console.WriteLine("");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occured during backup: " + ex.GetType());
                    if (variables.debugMode) Console.WriteLine(ex);
                }

                MainForm.mainForm.BeginInvoke(new Action(() => {
                    MainForm.mainForm.Enabled = true;
                    pi.Close();
                }));
            });
            worker.Start();
        }
    }
}
