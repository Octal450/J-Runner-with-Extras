using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace JRunner
{
    public static class Backup
    {
        public static string getAutoBackupName()
        {
            string board = "";
            if (!string.IsNullOrWhiteSpace(variables.boardtype)) board = variables.boardtype;
            if (board.Contains("Corona")) board = "Corona";
            if (board.Contains("Trinity")) board = "Trinity";
            if (board.Contains("Jasper")) board = "Jasper";

            string serial = MainForm.mainForm.getNand().ki.serial;
            if (!string.IsNullOrWhiteSpace(serial))
            {
                return board + " " + serial;
            }
            else return "";
        }

        public static string getAutoBackupTarget()
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

        public static void backupToZip(string target, string path)
        {
            Forms.BackupProgress bp = new Forms.BackupProgress();

            Thread worker = new Thread(() =>
            {
                MainForm.mainForm.BeginInvoke(new Action(() => {
                    MainForm.mainForm.Enabled = false;
                    bp.Show();
                }));

                try
                {
                    if (File.Exists(path)) File.Delete(path);

                    ZipFile.CreateFromDirectory(target, path); // Assume its a directory
                    Console.WriteLine("ZIP Backup Saved: " + path);
                    Console.WriteLine("");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occured during backup: " + ex.GetType());
                }

                MainForm.mainForm.BeginInvoke(new Action(() => {
                    MainForm.mainForm.Enabled = true;
                    bp.Close();
                }));
            });
            worker.Start();
        }
    }
}
