using DiskManagement;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class XB1HDD : Form
    {
        private const int ERROR_INSUFFICIENT_BUFFER = 0x7A;
        private bool showall = false;
        private static readonly byte[] PC_Signature = new byte[] { 0x55, 170 };
        private static readonly byte[] XBOX_Signature = new byte[] { 0x99, 0xcc };

        string DISK_GUID = "DB4B34A2DED666479EB54109A12228E5";
        string TEMP_CONTENT_GUID = "A57D72B3ACA33D4B9FD62EA54441011B";
        string USER_CONTENT_GUID = "E0B59B865633E64B85F729323A675CC7";
        string SYSTEM_SUPPORT_GUID = "477A0DC9B9CCBA4C8C660459F6B85724";
        string SYSTEM_UPDATE_GUID = "D76A059AED324141AEB1AFB9BD5565DC";
        string SYSTEM_UPDATE2_GUID = "7C19B224019DF945A8E1DBBCFA161EB2";

        string TEMP_CONTENT_L = "Temp Content";
        string USER_CONTENT_L = "User Content";
        string SYSTEM_SUPPORT_L = "System Support";
        string SYSTEM_UPDATE_L = "System Update";
        string SYSTEM_UPDATE2_L = "System Update 2";

        long[] PARTITION_SIZES = {
            44023414784,
            0,
            42949672960,
            12884901888,
            7516192768
        };

        public enum DeviceMode
        {
            None,
            Xbox,
            PC
        }

        public XB1HDD()
        {
            InitializeComponent();
        }

        private void XB1HDD_Load(object sender, EventArgs e)
        {
            contextMenuStrip1.Enabled = false;
            new Thread(enumerate).Start();
        }

        private void enumerate()
        {
            Environment.GetLogicalDrives();
            listView1.Items.Clear();
            List<string> pdrives = GetPhysicalDriveList();
            int i = 0;
            foreach (string info in pdrives)
            {
                List<string> letter = GetLetters(Convert.ToInt32(info.Replace(@"PhysicalDrive", "").ToString()));
                if (variables.debugme) Console.WriteLine("{0} - {1}", info, letter.Count);
                if (letter.Count == 0)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = info;
                    lvi.SubItems.Add("");
                    lvi.SubItems.Add("");
                    lvi.SubItems.Add("");
                    lvi.SubItems.Add("");
                    lvi.SubItems.Add(((GetSize(Convert.ToInt32(info.Replace(@"PhysicalDrive", "").ToString())) / 1024f) / 1024f / 1024f).ToString());
                    if (showall) lvi.SubItems.Add(GetMode(info).ToString());
                    else lvi.SubItems.Add("");
                    listView1.Items.Add(lvi);
                }
                else
                {
                    int j = 0;
                    foreach (string drive in letter)
                    {
                        if (variables.debugme) Console.WriteLine("{0} - {1}", info, drive);
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
                            if (variables.debugme) Console.WriteLine("Drive is ready");
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                            lvi.SubItems.Add("");
                            lvi.SubItems.Add(((GetSize(Convert.ToInt32(info.Replace(@"PhysicalDrive", "").ToString())) / 1024f) / 1024f / 1024f).ToString());
                        }
                        if (showall) lvi.SubItems.Add(GetMode(info).ToString());
                        else lvi.SubItems.Add("");
                        if (driv.DriveType == DriveType.Fixed || showall)
                            listView1.BeginInvoke(new Action(() => listView1.Items.Add(lvi)));
                        j++;
                    }
                }
                //lvi.SubItems.Add(string.Join("", getletters(Convert.ToInt32(info[info.Length -1].ToString()))));
                i++;
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
                            maxResponseSize = (int)(maxResponseSize * 5);
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
                if (variables.debugme) Console.WriteLine(deviceId);
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
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            return driveLetters;
        }
        private long GetSize(int drive)
        {
            int returnedBytes;
            IntPtr buffer = Marshal.AllocHGlobal(sizeof(ulong));
            bool result = DeviceIoControl(CreateFile(@"\\.\PHYSICALDRIVE" + drive, FileAccess.Read, FileShare.ReadWrite, 0, FileMode.Open, 0, IntPtr.Zero), 0x0007405C, IntPtr.Zero, 0, buffer, sizeof(ulong), out returnedBytes, IntPtr.Zero);
            long sessionId = Marshal.ReadInt64(buffer);
            if (!result) sessionId = 0;
            if (variables.debugme) Console.WriteLine(result);
            if (variables.debugme) Console.WriteLine(sessionId);
            if (variables.debugme) Console.WriteLine(returnedBytes);
            Marshal.FreeHGlobal(buffer);
            return sessionId;
        }

        #region DLLImports

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint QueryDosDevice(string lpDeviceName, IntPtr lpTargetPath, int ucchMax);

        [DllImport("Kernel32.dll")]
        static extern SafeFileHandle CreateFile(string filename, [MarshalAs(UnmanagedType.U4)]FileAccess fileaccess, [MarshalAs(UnmanagedType.U4)]FileShare fileshare, int securityattributes, [MarshalAs(UnmanagedType.U4)]FileMode creationdisposition, int flags, IntPtr template);

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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //contextMenuStrip1.Visible = checkBox1.Checked;
            contextMenuStrip1.Enabled = checkBox1.Checked;
            showall = checkBox1.Checked;
            new Thread(enumerate).Start();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            if (MessageBox.Show("This will erase your hard disk, or maybe fuck it up too.\nALL files on it will be deleted.\nAre you REALLY sure you want to procceed?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No) return;
            if (MessageBox.Show("The actions performed CANNOT be reversed!\nAre you SURE you want to convert " + listView1.SelectedItems[0].SubItems[1].Text + " to an Xbox ONE Disk?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No) return;
            new Thread(doBusiness).Start();
        }

        private void doBusiness()
        {
            string disk = (listView1.SelectedItems[0].SubItems[0].Text).Replace("PhysicalDrive", "");
            clean(disk);
            Console.WriteLine(".");
            PARTITION_SIZES[1] = 0;
            long totalSize = 0;
            foreach (long n in PARTITION_SIZES) totalSize += n;

            PARTITION_SIZES[1] = GetSize(Convert.ToInt32(disk)) - totalSize - (140 * 1024 * 1024);

            createPartitions(disk);
            Console.WriteLine(".");

            byte[] data;
            if (!read(disk, out data)) return;

            byte[] header = new byte[0x200];
            Buffer.BlockCopy(data, 0x200, header, 0, header.Length);
            byte[] pttable = new byte[data.Length - 0x400];
            Buffer.BlockCopy(data, 0x400, pttable, 0, pttable.Length);

            Console.WriteLine("Fixing partition table");
            fixup_part_table(ref pttable);
            Console.WriteLine("Calculating and fixing checksums");
            fixup_header(ref header, pttable);
            Console.WriteLine("Writing to disk");

            Buffer.BlockCopy(header, 0, data, 0x200, 0x200);
            Buffer.BlockCopy(pttable, 0, data, 0x400, pttable.Length);
            Thread.Sleep(1000);
            write(disk, header, data);

            print();
        }

        private bool read(string disk, out byte[] sectors)
        {
            sectors = null;

            string ldrive = "PhysicalDrive" + disk;
            Console.WriteLine(ldrive);
            var diskGeometry = DiskGeometry.FromDevice(@"\\.\" + ldrive.Replace("\\", ""));

            if (diskGeometry == null)
            {
                Console.WriteLine("Failed to get DiskGeometry.");
                return false;
            }

            uint track = diskGeometry.BytesPerSector;
            sectors = new byte[diskGeometry.BytesPerSector * 34];
            long i = 0;
            try
            {
                Console.WriteLine("Reading from {0}", ldrive);
                SafeFileHandle sfh = CreateFile(@"\\.\" + ldrive.Replace("\\", ""), FileAccess.Read, FileShare.Read, 0, FileMode.Open, 0, IntPtr.Zero);
                FileStream fs = new FileStream(sfh, FileAccess.Read);
                byte[] temp = new byte[track];
                while (i < 34 && !variables.escapeloop)
                {
                    fs.Read(temp, 0, (int)track);
                    Buffer.BlockCopy(temp, 0, sectors, (int)(i * track), (int)track);
                    i++;
                }
                CloseHandle(sfh);
                fs.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString() + i); return false; }
            return true;
        }

        private void write(string disk, byte[] header, byte[] sectors)
        {
            const uint OPEN_EXISTING = 3;
            const uint GENERIC_WRITE = (0x40000000);
            const uint FSCTL_LOCK_VOLUME = 0x00090018;
            const uint FSCTL_UNLOCK_VOLUME = 0x0009001c;
            const uint FSCTL_DISMOUNT_VOLUME = 0x00090020;

            string ldrive = "PhysicalDrive" + disk;
            if (variables.debugme) Console.WriteLine(ldrive);

            bool success = false;
            int intOut;
            string deviceId = @"\\.\" + ldrive;

            var diskGeometry = DiskGeometry.FromDevice(@"\\.\" + ldrive.Replace("\\", ""));

            List<string> logicaldrives = GetLetters(Convert.ToInt32(ldrive.Replace(@"PhysicalDrive", "").Replace("\\", "").Replace(".", "").ToString()));

            SafeFileHandle diskHandle = CreateFile(deviceId, GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
            if (diskHandle.IsInvalid)
            {
                int lastError = Marshal.GetLastWin32Error();
                Console.WriteLine(@"!! Error ({0}): {1}", lastError, new Win32Exception(lastError).Message);
                Console.WriteLine(deviceId + " open error.");
                return;
            }
            if (variables.debugme) Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": opened.");

            List<SafeFileHandle> lhandles = new List<SafeFileHandle>();
            List<string> lnames = new List<string>();
            int i = 0;

            if (variables.debugme) Console.WriteLine(logicaldrives.Count);
            foreach (string logdrive in logicaldrives)
            {
                if (variables.debugme) Console.WriteLine("Opening logical drives");
                string ldevid = @"\\.\" + logdrive.Replace("\\", "").Replace(".", "");
                if (variables.debugme) Console.WriteLine(ldevid);
                SafeFileHandle ldiskHandle = CreateFile(ldevid, GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
                if (ldiskHandle.IsInvalid)
                {
                    Console.WriteLine(ldevid + " open error.");
                    break;
                }
                if (variables.debugme) Console.WriteLine(ldevid + " " + Marshal.GetHRForLastWin32Error().ToString() + ": opened.");
                lhandles.Add(ldiskHandle);
                lnames.Add(ldevid);

                success = DeviceIoControl(ldiskHandle, FSCTL_LOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
                if (!success)
                {
                    Console.WriteLine(ldevid + " lock error.");
                    CloseHandle(ldiskHandle);
                    break;
                }

                if (variables.debugme) Console.WriteLine(ldevid + " " + Marshal.GetHRForLastWin32Error().ToString() + ": locked.");

                success = DeviceIoControl(ldiskHandle, FSCTL_DISMOUNT_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
                if (!success)
                {
                    Console.WriteLine(ldevid + " " + Marshal.GetHRForLastWin32Error().ToString() + ": dismount error.");
                    DeviceIoControl(ldiskHandle, FSCTL_UNLOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
                    CloseHandle(ldiskHandle);
                    return;
                }
            }
            success = DeviceIoControl(diskHandle, FSCTL_LOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
            if (!success)
            {
                Console.WriteLine(deviceId + " lock error.");
                CloseHandle(diskHandle);
                return;
            }

            if (variables.debugme) Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": locked.");

            success = DeviceIoControl(diskHandle, FSCTL_DISMOUNT_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
            if (!success)
            {
                Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": dismount error.");
                DeviceIoControl(diskHandle, FSCTL_UNLOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
                CloseHandle(diskHandle);
                return;
            }

            if (variables.debugme) Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": unmounted.");

            //uint numTotalSectors = 0x795FFF;//DiskSize / 512;
            //uint numTotalSectors = 0x702000;
            uint sector = diskGeometry.BytesPerSector;
            long totaltracks = 33;

            byte[] junkBytes = new byte[(int)sector];

            if (variables.debugme) Console.WriteLine(totaltracks);
            FileStream fw = new FileStream(diskHandle, FileAccess.ReadWrite);

            for (uint sectorNum = 0; sectorNum < totaltracks; sectorNum++)
            {

                Buffer.BlockCopy(sectors, (int)sectorNum * 0x200, junkBytes, 0, 0x200);

                fw.Write(junkBytes, 0, (int)sector);
            }

            fw.Seek(diskGeometry.MaximumLinearAddress - 0x200, SeekOrigin.Begin);
            fw.Write(header, 0, 0x200);


            i = 0;
            foreach (SafeFileHandle sfh in lhandles)
            {
                success = DeviceIoControl(sfh, FSCTL_UNLOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
                if (success)
                {
                    if (variables.debugme) Console.WriteLine(lnames[i] + " " + Marshal.GetHRForLastWin32Error().ToString() + ": unlocked.");
                }
                else
                {
                    Console.WriteLine(lnames[i] + " " + Marshal.GetHRForLastWin32Error().ToString() + ": unlock error: " + Marshal.GetHRForLastWin32Error().ToString());
                }
                i++;
            }


            success = DeviceIoControl(diskHandle, FSCTL_UNLOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
            if (success)
            {
                if (variables.debugme) Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": unlocked.");
            }
            else
            {
                Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": unlock error: " + Marshal.GetHRForLastWin32Error().ToString());
            }

            i = 0;
            foreach (SafeFileHandle sfh in lhandles)
            {
                success = CloseHandle(sfh);
                if (success)
                {
                    if (variables.debugme) Console.WriteLine(lnames[i] + " " + Marshal.GetHRForLastWin32Error().ToString() + ": handle closed.");
                }
                else
                {
                    Console.WriteLine(lnames[i] + " " + Marshal.GetHRForLastWin32Error().ToString() + ": close handle error: " + Marshal.GetHRForLastWin32Error().ToString());
                }
                i++;
            }


            success = CloseHandle(diskHandle);
            if (success)
            {
                if (variables.debugme) Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": handle closed.");
            }
            else
            {
                Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": close handle error: " + Marshal.GetHRForLastWin32Error().ToString());
            }
            try
            {
                fw.Close();
            }
            catch (Exception) { }
            Environment.GetLogicalDrives();
        }
        private void write2(string disk, byte[] header, byte[] sectors)
        {
            string ldrive = "PhysicalDrive" + disk;
            var diskGeometry = DiskGeometry.FromDevice(@"\\.\" + ldrive.Replace("\\", ""));
            uint sector = diskGeometry.BytesPerSector;

            long i = 0;

            try
            {
                FileStream fs = new FileStream(CreateFile(@"\\.\" + ldrive.Replace("\\", ""), FileAccess.ReadWrite, FileShare.ReadWrite, 0, FileMode.Open, 0, IntPtr.Zero), FileAccess.ReadWrite);
                byte[] temp = new byte[sector];
                while (i < 34 && !variables.escapeloop)
                {
                    Buffer.BlockCopy(sectors, (int)i * 0x200, temp, 0, 0x200);
                    fs.Write(temp, 0, (int)sector);
                    i++;
                }
                fs.Seek(diskGeometry.MaximumLinearAddress - 0x200, SeekOrigin.Begin);
                fs.Write(header, 0, 0x200);

                fs.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); if (variables.debugme) Console.WriteLine(ex.ToString()); }
        }

        private void print()
        {
            Console.WriteLine("You need to copy the following files to the new hdd");
            Console.WriteLine("└── [4.0K]  System Update");
            Console.WriteLine("    ├── [4.0K]  A");
            Console.WriteLine("    │   ├── [341M]  deltas.xvd");
            Console.WriteLine("    │   ├── [ 36M]  SettingsTemplate.xvd");
            Console.WriteLine("    │   ├── [ 24M]  sosinit.xvd");
            Console.WriteLine("    │   ├── [ 62M]  sostmpl.xvd");
            Console.WriteLine("    │   ├── [267M]  systemaux.xvd");
            Console.WriteLine("    │   └── [850M]  system.xvd");
            Console.WriteLine("    ├── [4.0K]  B");
            Console.WriteLine("    │   ├── [ 30M]  SettingsTemplate.xvd");
            Console.WriteLine("    │   ├── [ 23M]  sosinit.xvd");
            Console.WriteLine("    │   ├── [ 45M]  sostmpl.xvd");
            Console.WriteLine("    │   ├── [ 91M]  systemaux.xvd");
            Console.WriteLine("    │   └── [761M]  system.xvd");
            Console.WriteLine("    └── [ 44M]  updater.xvd");
            Console.WriteLine("");
            Console.WriteLine("Thanks for this Juvenal!");
            try
            {
                Process.Start("T:\\");
            }
            catch (Exception) { }
        }

        private List<int> getVolumes(string disk)
        {
            List<int> ret = new List<int>();
            string file = "dpScript.scr";
            if (File.Exists(file)) File.Delete(file);


            File.AppendAllText(file,
                string.Format(
                "SELECT DISK={0}\n" +
                "DETAIL DISK\n" +
                "EXIT", disk));
            string r = diskPart(Path.GetFullPath(file));
            string[] t = r.Split('\n');
            bool flag = true;
            foreach (string line in t)
            {
                if (line.Contains("Volume"))
                {
                    if (flag) flag = false;
                    else
                    {
                        int num = Convert.ToInt32((line.Split(' '))[3].Trim());
                        Console.WriteLine(num);
                        ret.Add(num);
                    }
                }
            }
            if (File.Exists(file)) File.Delete(file);
            return ret;
        }

        private void clean(string disk)
        {
            string file = "dpScript.scr";
            if (File.Exists(file)) File.Delete(file);

            File.AppendAllText(file,
                string.Format(
                "SELECT DISK={0}\n" +
                "CLEAN\n" +
                "EXIT", disk));
            diskPart(Path.GetFullPath(file));
            if (File.Exists(file)) File.Delete(file);
        }

        private void createPartitions(string disk)
        {
            long temp_start = 1024;
            long temp_end = temp_start + (PARTITION_SIZES[0] / 1024);
            long user_end = temp_end + (PARTITION_SIZES[1] / 1024);
            long sys_end = user_end + (PARTITION_SIZES[2] / 1024);
            long upt_end = sys_end + (PARTITION_SIZES[3] / 1024);
            long upt2_end = upt_end + (PARTITION_SIZES[4] / 1024);
            string file = "dpScript.scr";
            if (File.Exists(file)) File.Delete(file);

            string commands = string.Format(
                "SELECT DISK={0}\n" +
                "CLEAN\n" +
                "CONVERT GPT\n" +
                "CREATE PARTITION PRIMARY SIZE={1}\n" +
                "FORMAT FS=NTFS LABEL=\"{2}\" QUICK\n" +
                "ASSIGN LETTER=Q\n" +
                "CREATE PARTITION PRIMARY SIZE={3}\n" +
                "FORMAT FS=NTFS LABEL=\"{4}\" QUICK\n" +
                "ASSIGN LETTER=R\n" +
                "CREATE PARTITION PRIMARY SIZE={5}\n" +
                "FORMAT FS=NTFS LABEL=\"{6}\" QUICK\n" +
                "ASSIGN LETTER=S\n" +
                "CREATE PARTITION PRIMARY SIZE={7}\n" +
                "FORMAT FS=NTFS LABEL=\"{8}\" QUICK\n" +
                "ASSIGN LETTER=T\n" +
                "CREATE PARTITION PRIMARY SIZE={9}\n" +
                "FORMAT FS=NTFS LABEL=\"{10}\" QUICK\n" +
                "ASSIGN LETTER=U\n" +
                "EXIT", disk,
                PARTITION_SIZES[0] / 1024 / 1024,
                TEMP_CONTENT_L,
                PARTITION_SIZES[1] / 1024 / 1024,
                USER_CONTENT_L,
                PARTITION_SIZES[2] / 1024 / 1024,
                SYSTEM_SUPPORT_L,
                PARTITION_SIZES[3] / 1024 / 1024,
                SYSTEM_UPDATE_L,
                PARTITION_SIZES[4] / 1024 / 1024,
                SYSTEM_UPDATE2_L
                );
            File.AppendAllText(file, commands);

            diskPart(Path.GetFullPath(file));
            if (File.Exists(file)) File.Delete(file);
        }

        private void fixup_header(ref byte[] header, byte[] pttable)
        {
            crc32 c = new crc32();
            byte[] chk_gpt_pt = BitConverter.GetBytes(c.CRC(pttable));

            header.Replace(chk_gpt_pt, 88, 4);

            header.Replace(Oper.StringToByteArray(DISK_GUID), 56, 16);

            for (int i = 16; i <= 19; i++)
            {
                header[i] = 0x00;
            }

            c = new crc32();
            byte[] chk_header = BitConverter.GetBytes(c.CRC(Oper.returnportion(header, 0, 0x5C)));

            header.Replace(chk_header, 16, 4);
        }

        private void fixup_part_table(ref byte[] pttable)
        {
            pttable.Replace(Oper.StringToByteArray(TEMP_CONTENT_GUID), 16, 16);

            pttable.Replace(Oper.StringToByteArray(USER_CONTENT_GUID), 144, 16);

            pttable.Replace(Oper.StringToByteArray(SYSTEM_SUPPORT_GUID), 272, 16);

            pttable.Replace(Oper.StringToByteArray(SYSTEM_UPDATE_GUID), 400, 16);

            pttable.Replace(Oper.StringToByteArray(SYSTEM_UPDATE2_GUID), 528, 16);

            for (int i = 56; i <= 127; i++)
            {
                if (i - 56 < TEMP_CONTENT_L.Length) pttable[i] = (byte)TEMP_CONTENT_L[i - 56];
                else pttable[i] = 0x00;
            }
            for (int i = 184; i <= 255; i++)
            {
                if (i - 184 < USER_CONTENT_L.Length) pttable[i] = (byte)USER_CONTENT_L[i - 184];
                else pttable[i] = 0x00;
            }
            for (int i = 312; i <= 383; i++)
            {
                if (i - 312 < SYSTEM_SUPPORT_L.Length) pttable[i] = (byte)SYSTEM_SUPPORT_L[i - 312];
                else pttable[i] = 0x00;
            }
            for (int i = 440; i <= 511; i++)
            {
                if (i - 440 < SYSTEM_UPDATE_L.Length) pttable[i] = (byte)SYSTEM_UPDATE_L[i - 440];
                else pttable[i] = 0x00;
            }
            for (int i = 568; i <= 639; i++)
            {
                if (i - 568 < SYSTEM_UPDATE2_L.Length) pttable[i] = (byte)SYSTEM_UPDATE2_L[i - 568];
                else pttable[i] = 0x00;
            }
        }

        public string diskPart(string script)
        {
            string output = "";
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "DiskPart.exe";
            string arguments = "/s \"" + script + "\"";
            if (variables.debugme) Console.WriteLine(arguments);
            pProcess.StartInfo.Arguments = arguments;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.WorkingDirectory = variables.pathforit;
            pProcess.StartInfo.RedirectStandardInput = true;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            //pProcess.Exited += new EventHandler(xeExit);
            try
            {
                AutoResetEvent outputWaitHandle = new AutoResetEvent(false);
                pProcess.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        output += e.Data + "\n";
                        if (!e.Data.Contains("percent")) Console.WriteLine(e.Data);
                    }
                };
                pProcess.Start();
                pProcess.BeginOutputReadLine();
                pProcess.WaitForExit();

                if (pProcess.HasExited)
                {
                    pProcess.CancelOutputRead();
                }
            }
            catch (Exception objException)
            {
                Console.WriteLine(objException.Message);
            }

            return output;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            new Thread(enumerate).Start();
        }


        private void ChangeDeviceMode(string deviceName, DeviceMode mode)
        {
            byte[] bytes = new byte[0x200];
            int numBytesRead = 0;
            int numBytesWritten = 0;
            SafeFileHandle handle = CreateFile(deviceName, 0xc0000000, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);
            if (handle.IsInvalid)
            {
                handle.Close();
            }
            ReadFile(handle, bytes, 0x200, out numBytesRead, IntPtr.Zero);
            byte[] first = new byte[] { bytes[510], bytes[0x1ff] };
            if (mode == DeviceMode.Xbox && first.SequenceEqual<byte>(PC_Signature))
            {
                bytes[510] = XBOX_Signature[0];
                bytes[0x1ff] = XBOX_Signature[1];
            }
            else if (mode == DeviceMode.PC && first.SequenceEqual<byte>(XBOX_Signature))
            {
                bytes[510] = PC_Signature[0];
                bytes[0x1ff] = PC_Signature[1];
            }
            else
            {
                handle.Close();
                return;
            }
            int l = 0;
            SetFilePointer(handle, 0, out l, EMoveMethod.Begin);
            WriteFile(handle, bytes, 0x200, out numBytesWritten, IntPtr.Zero);
            handle.Close();
        }

        private DeviceMode GetMode(string lpFileName)
        {
            lpFileName = "\\\\.\\" + lpFileName.ToUpper();
            int numBytesRead = 0;
            byte[] bytes = new byte[0x200];
            DeviceMode mode = DeviceMode.None;
            SafeFileHandle handle = CreateFile(lpFileName, 0x80000000, 1, IntPtr.Zero, 3, 0, IntPtr.Zero);
            if (handle.IsInvalid)
            {
                handle.Close();
                return DeviceMode.None;
            }
            ReadFile(handle, bytes, 0x200, out numBytesRead, IntPtr.Zero);
            byte[] first = new byte[] { bytes[510], bytes[0x1ff] };

            if (first.SequenceEqual<byte>(XBOX_Signature))
            {
                mode = DeviceMode.Xbox;
            }
            else if (first.SequenceEqual<byte>(PC_Signature))
            {
                if (Oper.allsame(Oper.returnportion(bytes, 0, 440), 0x00))
                {
                    mode = DeviceMode.PC;
                }
            }
            handle.Close();
            return mode;
        }

        private void changeModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            DeviceMode mode = DeviceMode.None;
            if (listView1.SelectedItems[0].SubItems[6].Text == DeviceMode.None.ToString()) return;
            else if (listView1.SelectedItems[0].SubItems[6].Text == DeviceMode.Xbox.ToString()) mode = DeviceMode.PC;
            else if (listView1.SelectedItems[0].SubItems[6].Text == DeviceMode.PC.ToString()) mode = DeviceMode.Xbox;
            ThreadStart starter = delegate { ChangeDeviceMode("\\\\.\\" + listView1.SelectedItems[0].SubItems[0].Text.ToUpper(), mode); };
            new Thread(starter).Start();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView1.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }

        }
    }
}
