using DiskManagement;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace JRunner.Panels
{
    public partial class LDrivesInfo : UserControl
    {

        private bool showall = false;
        private bool enumearting = false;
        public static Function fu = Function.ReadWrite;

        //byte MediaType = 0xb;
        //uint DiskSize = 0xe0400000;
        //uint BytesPerSector = 0x200;
        //uint SectorsPerTrack = 0x3f;
        //uint TracksPerCylinder = 0xff;
        //uint Cylinders = 0x1c9;
        //uint size = 3762290688;

        public enum Function : int
        {
            Read,
            Write,
            ReadWrite
        }

        private const int ERROR_INSUFFICIENT_BUFFER = 0x7A;

        public LDrivesInfo()
        {
            InitializeComponent();
        }
        public LDrivesInfo(Function f)
        {
            InitializeComponent();
            fu = f;
        }

        public void setup(Function f)
        {
            fu = f;
            btnWrite.Enabled = btnErase.Enabled = btnRead.Enabled = true;
            if (fu == Function.Read) btnWrite.Enabled = btnErase.Enabled = false;
            else if (fu == Function.Write) btnRead.Enabled = btnErase.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                MainForm.mainForm.ldInfo_CloseLDClick();
            }
            catch (Exception) { }
        }

        private void btnErase_Click(object sender, EventArgs e)
        {
            new Thread(startErase).Start();
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            new Thread(startWrite).Start();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            new Thread(startRead).Start();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refreshDrives();
        }

        public void refreshDrives(bool wait = false)
        {
            listView1.Items.Clear();
            Thread enumThread = new Thread(() => enumerate(wait));
            enumThread.Start();
        }

        private void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            showall = chkShowAll.Checked;
            Thread enumThread = new Thread(() => enumerate());
            enumThread.Start();
        }

        private void LDrivesInfo_Load(object sender, EventArgs e)
        {
            if (fu == Function.Read) btnWrite.Enabled = btnErase.Enabled = false;
            else if (fu == Function.Write) btnRead.Enabled = btnErase.Enabled = false;

            Thread enumThread = new Thread(() => enumerate());
            enumThread.Start();
        }

        private void enumerate(bool wait = false)
        {
            if (!enumearting)
            {
                enumearting = true;
                if (wait) Thread.Sleep(500);
                Environment.GetLogicalDrives();
                listView1.Items.Clear();
                List<string> pdrives = GetPhysicalDriveList();
                int i = 0;
                foreach (string info in pdrives)
                {
                    int driveNumber = Convert.ToInt32(info.Replace(@"PhysicalDrive", ""));
                    List<string> letter = GetLetters(driveNumber);
                    if (variables.debugMode) Console.WriteLine("{0} - {1}", info, letter.Count);
                    if (letter.Count == 0)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = info;
                        lvi.SubItems.Add("");
                        lvi.SubItems.Add("");
                        lvi.SubItems.Add("");
                        lvi.SubItems.Add("");
                        var x = DiskGeometry.FromDevice(@"\\.\PHYSICALDRIVE" + driveNumber);
                        if (x != null) lvi.SubItems.Add(((x.DiskSize / 1024f) / 1024f).ToString());
                        else lvi.SubItems.Add("");
                        listView1.Items.Add(lvi);
                    }
                    else
                    {
                        int j = 0;
                        foreach (string drive in letter)
                        {
                            if (variables.debugMode) Console.WriteLine("{0} - {1}", info, drive);
                            ListViewItem lvi = new ListViewItem();
                            lvi.Text = info;
                            DriveInfo driv = new DriveInfo(drive.Replace(@"\\.\", ""));
                            lvi.SubItems.Add(driv.Name);
                            lvi.SubItems.Add(driv.DriveType.ToString());
                            if (driv.IsReady == true)
                            {
                                lvi.SubItems.Add(driv.VolumeLabel);
                                lvi.SubItems.Add(driv.DriveFormat);
                                lvi.SubItems.Add((driv.TotalSize / (1024f) / 1024f).ToString());
                                if (variables.debugMode) Console.WriteLine("Drive is ready");
                            }
                            else
                            {
                                lvi.SubItems.Add("");
                                lvi.SubItems.Add("");
                                var x = DiskGeometry.FromDevice(@"\\.\PHYSICALDRIVE" + driveNumber);
                                if (x != null) lvi.SubItems.Add(((x.DiskSize / 1024f) / 1024f).ToString());
                                else lvi.SubItems.Add("");
                            }
                            if (driv.DriveType == DriveType.Removable || showall) listView1.Items.Add(lvi);
                            j++;
                            MainForm.mainForm.updateProgress(((i + j) * 100) / (pdrives.Count * letter.Count));
                        }
                    }
                    //lvi.SubItems.Add(string.Join("", getletters(Convert.ToInt32(info[info.Length -1].ToString()))));
                    i++;
                    MainForm.mainForm.updateProgress((i * 100) / pdrives.Count);
                }
                enumearting = false;
            }
        }

        private void buttons(bool what)
        {
            if (fu != Function.Write) btnRead.Enabled = what;
            if (fu != Function.Read) btnWrite.Enabled = what;
            if (fu == Function.ReadWrite) btnErase.Enabled = what;
            btnRefresh.Enabled = what;
        }

        private void startRead()
        {
            buttons(false);
            variables.reading = true;
            if (variables.numReads != 1)
            {
                for (int i = 1; i <= variables.numReads; i++)
                {
                    string filename = variables.outfolder + "\\nanddump" + i + ".bin";
                    variables.reading = true;
                    int result = read(filename);
                    variables.reading = false;
                    if (result == 0) break;
                    Thread.Sleep(1000);
                    if (i == 1) MainForm.mainForm.xPanel_updateSource(filename);
                    else
                    {
                        MainForm.mainForm.ldInfo_UpdateAdditional(filename);
                        MainForm.mainForm.compareNands();
                    }
                    Thread.Sleep(1000);
                }
            }
            else
            {
                read(variables.outfolder + "\\nanddump1.bin");
                variables.reading = false;
                MainForm.mainForm.xPanel_updateSource(variables.outfolder + "\\nanddump1.bin");
            }
            buttons(true);
        }

        private int read(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return 0;
            if (listView1.SelectedItems.Count == 0) return 0;

            if (File.Exists(filename))
            {
                if (DialogResult.Cancel == MessageBox.Show("A nand dump already exists!\n\nContinuing will cause the contents to be overwritten!", "File Conflict", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                {
                    Console.WriteLine("Cancelled");
                    Console.WriteLine("");
                    return 0;
                };

                try
                {
                    File.Delete(filename);
                }
                catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); return 0; }
            }

            string ldrive = listView1.SelectedItems[0].SubItems[1].Text;
            Console.WriteLine(ldrive);
            var diskGeometry = DiskGeometry.FromDevice(@"\\.\" + ldrive.Replace("\\", ""));

            if (diskGeometry == null)
            {
                Console.WriteLine("Failed to get DiskGeometry.");
                Console.WriteLine("");
                return 0;
            }

            uint track = diskGeometry.BytesPerSector * (diskGeometry.Sector + 1);
            long tracks = 0x600;
            if (chkFullDump.Checked) tracks = diskGeometry.DiskSize / track;

            long i = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                Console.WriteLine("Reading from {0} to {1}", ldrive, filename);
                FileStream fs = new FileStream(CreateFile(@"\\.\" + ldrive.Replace("\\", ""), FileAccess.Read, FileShare.Read, 0, FileMode.Open, 0, IntPtr.Zero), FileAccess.Read);
                FileStream fw = new FileStream(filename, FileMode.OpenOrCreate);
                byte[] temp = new byte[track];
                while (i < tracks && !variables.escapeloop)
                {
                    MainForm.mainForm.updateProgress((int)((i * 100) / tracks));
                    MainForm.mainForm.updateBlock(((i * track) / 1024 / 1024).ToString("F0") + "MB");
                    i++;
                    fs.Read(temp, 0, (int)track);
                    fw.Write(temp, 0, (int)track);
                }
                fs.Close();
                fw.Close();
                MainForm.mainForm.updateBlock("");
                MainForm.mainForm.updateProgress(100);
                stopwatch.Stop();
                Console.WriteLine("Read Successful! Time Elapsed: {0}:{1:D2}", stopwatch.Elapsed.Minutes + (stopwatch.Elapsed.Hours * 60), stopwatch.Elapsed.Seconds);
                Console.WriteLine("");

                if (variables.playSuccess)
                {
                    SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                    success.Play();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString() + i); }
            return 1;
        }

        private void startWrite()
        {
            if (listView1.SelectedItems.Count == 0) return;
            buttons(false);
            write(variables.filename1);
            if (variables.debugMode) Console.WriteLine("changing back to old file");
            if (Path.GetExtension(variables.filename1) == ".ecc")
            {
                MainForm.mainForm.afterWriteXeLLCleanup();
            }
            buttons(true);
        }

        private void write(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return;
            if (listView1.SelectedItems.Count == 0) return;
            string ldrive = listView1.SelectedItems[0].SubItems[1].Text;
            if (listView1.SelectedItems[0].SubItems[2].Text != "Removable") { Console.WriteLine("Must be a removable type"); return; }
            if (MessageBox.Show("You are about to write to " + ldrive + "\n\nAre you sure you want to continue?", "Steep Hill Ahead", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) return;

            var diskGeometry = DiskGeometry.FromDevice(@"\\.\" + ldrive.Replace("\\", ""));
            uint track = diskGeometry.BytesPerSector * (diskGeometry.Sector + 1);
            long tracks;
            long i = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                Console.WriteLine("Writing to {0} from {1}", ldrive, filename);
                FileStream fs = new FileStream(CreateFile(@"\\.\" + ldrive.Replace("\\", ""), FileAccess.ReadWrite, FileShare.ReadWrite, 0, FileMode.Open, 0, IntPtr.Zero), FileAccess.ReadWrite);
                FileStream fw = new FileStream(filename, FileMode.OpenOrCreate);
                tracks = fw.Length / track;
                byte[] temp = new byte[track];
                while (i < tracks && !variables.escapeloop)
                {
                    MainForm.mainForm.updateProgress((int)((i * 100) / tracks));
                    MainForm.mainForm.updateBlock(((i * track) / 1024 / 1024).ToString("F0") + "MB");
                    i++;
                    fw.Read(temp, 0, (int)track);
                    fs.Write(temp, 0, (int)track);
                }
                fs.Close();
                fw.Close();
                MainForm.mainForm.updateBlock("");
                MainForm.mainForm.updateProgress(100);
                stopwatch.Stop();
                Console.WriteLine("Write Successful! Time Elapsed: {0}:{1:D2}:{2}", stopwatch.Elapsed.Minutes + (stopwatch.Elapsed.Hours * 60), stopwatch.Elapsed.Seconds, stopwatch.Elapsed.Milliseconds);
                Console.WriteLine("");

                if (variables.playSuccess)
                {
                    SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                    success.Play();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (variables.debugMode) Console.WriteLine(ex.ToString());
                Console.WriteLine("");
            }
        }

        private void startErase()
        {
            if (listView1.SelectedItems.Count == 0) return;
            buttons(false);
            erase();
            buttons(true);
        }

        private void erase()
        {
            if (listView1.SelectedItems.Count == 0) return;
            string ldrive = listView1.SelectedItems[0].SubItems[1].Text;
            if (listView1.SelectedItems[0].SubItems[2].Text != "Removable") { Console.WriteLine("Must be a removable type"); return; }
            if (MessageBox.Show("You are about to erase " + ldrive + "\n\nAre you sure you want to continue?", "Steep Hill Ahead", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) return;

            var diskGeometry = DiskGeometry.FromDevice(@"\\.\" + ldrive.Replace("\\", ""));
            uint track = diskGeometry.BytesPerSector * (diskGeometry.Sector + 1);
            long tracks = diskGeometry.DiskSize / track;

            long i = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                Console.WriteLine("Erasing {0}", ldrive);
                FileStream fs = new FileStream(CreateFile(@"\\.\" + ldrive.Replace("\\", ""), FileAccess.ReadWrite, FileShare.ReadWrite, 0, FileMode.Open, 0, IntPtr.Zero), FileAccess.ReadWrite);
                byte[] temp = new byte[track];
                while (i < tracks && !variables.escapeloop)
                {
                    MainForm.mainForm.updateProgress((int)((i * 100) / tracks));
                    MainForm.mainForm.updateBlock(((i * track) / 1024 / 1024).ToString("F0") + "MB");
                    i++;
                    fs.Write(temp, 0, (int)track);
                }
                fs.Close();
                MainForm.mainForm.updateBlock("");
                MainForm.mainForm.updateProgress(100);
                stopwatch.Stop();
                Console.WriteLine("Erase Successful! Time Elapsed: {0}:{1:D2}:{2}", stopwatch.Elapsed.Minutes + (stopwatch.Elapsed.Hours * 60), stopwatch.Elapsed.Seconds, stopwatch.Elapsed.Milliseconds);
                Console.WriteLine("");

                if (variables.playSuccess)
                {
                    SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                    success.Play();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (variables.debugMode) Console.WriteLine(ex.ToString());
                Console.WriteLine("");
            }
        }

        private static List<string> GetPhysicalDriveList()
        {
            uint returnSize = 0;
            // Arbitrary initial buffer size
            int maxResponseSize = 100;

            IntPtr response = IntPtr.Zero;

            string allDevices = null;
            string[] devices = null;

            while (returnSize == 0)
            {
                // Allocate response buffer for native call
                response = Marshal.AllocHGlobal(maxResponseSize);

                // Check out of memory condition
                if (response != IntPtr.Zero)
                {
                    try
                    {
                        // List DOS devices
                        returnSize = QueryDosDevice(null, response, maxResponseSize);

                        // List success
                        if (returnSize != 0)
                        {
                            // Result is returned as null-char delimited multistring
                            // Dereference it from ANSI charset
                            allDevices = Marshal.PtrToStringAnsi(response, maxResponseSize);
                        }
                        // The response buffer is too small, reallocate it exponentially and retry
                        else if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
                        {
                            maxResponseSize = maxResponseSize * 5;
                        }
                        // Fatal error has occured, throw exception
                        else
                        {
                            Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
                        }
                    }
                    finally
                    {
                        // Always free the allocated response buffer
                        Marshal.FreeHGlobal(response);
                    }
                }
                else
                {
                    throw new OutOfMemoryException("Out of memory when allocating space for QueryDosDevice command!");
                }
            }

            // Split zero-character delimited multi-string
            devices = allDevices.Split('\0');
            // QueryDosDevices lists alot of devices, return only PhysicalDrives
            return devices.Where(device => device.StartsWith("PhysicalDrive")).ToList<string>();
        }
        private List<string> GetLetters(int numberofdrive)
        {
            List<string> driveLetters = new List<string>();
            try
            {
                string deviceId = @"\\.\PHYSICALDRIVE" + numberofdrive;
                if (variables.debugMode) Console.WriteLine(deviceId);
                string queryString = "ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + deviceId + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition";
                ManagementObjectSearcher diskSearcher = new ManagementObjectSearcher("root\\CIMV2", queryString);
                ManagementObjectCollection diskMoc = diskSearcher.Get();
                foreach (ManagementObject diskMo in diskMoc)
                {
                    queryString = "ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + diskMo["DeviceID"] + "'} WHERE AssocClass = Win32_LogicalDiskToPartition";
                    ManagementObjectSearcher driveSearcher = new ManagementObjectSearcher("root\\CIMV2", queryString);

                    ManagementObjectCollection driveMoc = driveSearcher.Get();
                    foreach (ManagementObject driveMo in driveMoc)
                    {
                        driveLetters.Add("\\\\.\\" + driveMo["DeviceID"].ToString());
                    }
                }
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
            return driveLetters;
        }

        private void LDrives_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                variables.escapeloop = true;
                ThreadStart starter = delegate { escapedexit(5000); };
                new Thread(starter).Start();
            }
            else if (e.KeyCode == Keys.F3)
            {
                Console.WriteLine(listView1.SelectedItems[0].SubItems[0].Text);
                TestMethod(listView1.SelectedItems[0].SubItems[0].Text);
                Console.WriteLine("Done");
            }
        }
        void escapedexit(int time)
        {
            Thread.Sleep(time);
            variables.escapeloop = false;
        }

        public static void TestMethod(string device)
        {
            var diskGeometry = DiskGeometry.FromDevice(device);
            var cubicAddress = diskGeometry.MaximumCubicAddress;

            Console.WriteLine("Media Type: {0}", diskGeometry.MediaTypeName);
            Console.WriteLine("");

            Console.WriteLine("Maximum Linear Address: {0}", diskGeometry.MaximumLinearAddress);
            Console.WriteLine("Last Cylinder Number: {0}", cubicAddress.Cylinder);
            Console.WriteLine("Last Head Number: {0}", cubicAddress.Head);
            Console.WriteLine("Last Sector Number: {0}", cubicAddress.Sector);
            Console.WriteLine("");

            Console.WriteLine("Cylinders: {0}", diskGeometry.Cylinder);
            Console.WriteLine("Tracks Per Cylinder: {0}", diskGeometry.Head);
            Console.WriteLine("Sectors Per Track: {0}", diskGeometry.Sector);
            Console.WriteLine("");

            Console.WriteLine("Bytes Per Sector: {0}", diskGeometry.BytesPerSector);
            Console.WriteLine("Bytes Per Cylinder: {0}", diskGeometry.BytesPerCylinder);
            Console.WriteLine("Total Disk Space: {0}", diskGeometry.DiskSize);
            Console.WriteLine("");
        }

        public void updateIter(int n)
        {
            iterations.Text = "Nand Reads: " + n;
        }

        #region DLLImports

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint QueryDosDevice(string lpDeviceName, IntPtr lpTargetPath, int ucchMax);

        [DllImport("Kernel32.dll")]
        static extern SafeFileHandle CreateFile(string filename, [MarshalAs(UnmanagedType.U4)] FileAccess fileaccess, [MarshalAs(UnmanagedType.U4)] FileShare fileshare, int securityattributes, [MarshalAs(UnmanagedType.U4)] FileMode creationdisposition, int flags, IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool GetDiskFreeSpace(string lpRootPathName,
        out uint lpSectorsPerCluster,
        out uint lpBytesPerSector,
        out uint lpNumberOfFreeClusters,
        out uint lpTotalNumberOfClusters);

        public enum EMoveMethod : uint
        {
            Begin = 0,
            Current = 1,
            End = 2
        }

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint SetFilePointer([In] SafeFileHandle hFile, [In] long lDistanceToMove, [Out] out int lpDistanceToMoveHigh, [In] EMoveMethod dwMoveMethod);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32", SetLastError = true)]
        internal extern static int ReadFile(SafeFileHandle handle, byte[] bytes, int numBytesToRead, out int numBytesRead, IntPtr overlapped_MustBeZero);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal extern static int WriteFile(SafeFileHandle handle, byte[] bytes, int numBytesToWrite, out int numBytesWritten, IntPtr overlapped_MustBeZero);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool DeviceIoControl(SafeFileHandle hDevice, uint dwIoControlCode, IntPtr lpInBuffer, int nInBufferSize, IntPtr lpOutBuffer, int nOutBufferSize, out int lpBytesReturned, IntPtr lpOverlapped);
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool DeviceIoControl(SafeFileHandle hDevice, uint dwIoControlCode, byte[] lpInBuffer, int nInBufferSize, byte[] lpOutBuffer, int nOutBufferSize, out int lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool CloseHandle(SafeFileHandle handle);

        #endregion

        private void chkFullDump_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFullDump.Checked)
            {
                MessageBox.Show("Warning: This function is for advanced users only\n\nYou should not do a full dump unless you have a specific reason", "Steep Hill Ahead", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
