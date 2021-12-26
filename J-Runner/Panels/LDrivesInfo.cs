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
        private bool force = false;
        private bool enumearting = false;
        //public static string filename = "";
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

        public delegate void ClickedCloseLD();
        public event ClickedCloseLD CloseLDClick;

        public delegate void UpdateProgress(int progress);
        public event UpdateProgress UpdateProgres;
        public delegate void UpdateBlock(string block);
        public event UpdateBlock UpdateBloc;
        public delegate void UpdateFile(string file);
        public event UpdateFile UpdateSourc;

        public delegate void UpdateAddit(string file);
        public event UpdateAddit UpdateAdditional;
        public delegate void doCompare();
        public event doCompare doCompar;

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                CloseLDClick();
            }
            catch (Exception) { }
        }

        private void btnErase_Click(object sender, EventArgs e)
        {
            new Thread(erase).Start();
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            new Thread(Write_b).Start();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            new Thread(Read_b).Start();
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
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
                    if (variables.debugme) Console.WriteLine("{0} - {1}", info, letter.Count);
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
                                var x = DiskGeometry.FromDevice(@"\\.\PHYSICALDRIVE" + driveNumber);
                                if (x != null) lvi.SubItems.Add(((x.DiskSize / 1024f) / 1024f).ToString());
                                else lvi.SubItems.Add("");
                            }
                            if (driv.DriveType == DriveType.Removable || showall) listView1.Items.Add(lvi);
                            j++;
                            UpdateProgres(((i + j) * 100) / (pdrives.Count * letter.Count));
                        }
                    }
                    //lvi.SubItems.Add(string.Join("", getletters(Convert.ToInt32(info[info.Length -1].ToString()))));
                    i++;
                    UpdateProgres((i * 100) / pdrives.Count);
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

        private void Read_b()
        {
            buttons(false);
            variables.reading = true;
            if (variables.NoReads != 1)
            {
                for (int i = 1; i <= variables.NoReads; i++)
                {
                    string filename = variables.outfolder + "\\nanddump" + i + ".bin";
                    variables.reading = true;
                    int result = read(filename);
                    variables.reading = false;
                    if (result == 0) break;
                    Thread.Sleep(1000);
                    if (i == 1) UpdateSourc(filename);
                    else
                    {
                        UpdateAdditional(filename);
                        doCompar();
                    }
                    Thread.Sleep(1000);
                }
            }
            else
            {
                read(variables.outfolder + "\\nanddump1.bin");
                variables.reading = false;
                UpdateSourc(variables.outfolder + "\\nanddump1.bin");
            }
            buttons(true);
        }
        private int read(string filename)
        {
            if (String.IsNullOrEmpty(filename)) return 0;
            if (listView1.SelectedItems.Count == 0) return 0;

            if (File.Exists(filename))
            {
                if (DialogResult.Cancel == MessageBox.Show("File already exists, it will be DELETED! Press OK to continue", "File Already Exists", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                {
                    Console.WriteLine("Cancelled");
                    Console.WriteLine("");
                    return 0;
                };

                try
                {
                    File.Delete(filename);
                }
                catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); return 0; }
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
                    UpdateProgres((int)((i * 100) / tracks));
                    UpdateBloc(((i * track) / 1024 / 1024).ToString("F0") + "MB");
                    i++;
                    fs.Read(temp, 0, (int)track);
                    fw.Write(temp, 0, (int)track);
                }
                fs.Close();
                fw.Close();
                UpdateBloc("");
                UpdateProgres(100);
                stopwatch.Stop();
                Console.WriteLine("Read Successful! Time Elapsed: {0}:{1:D2}", stopwatch.Elapsed.Minutes + (stopwatch.Elapsed.Hours * 60), stopwatch.Elapsed.Seconds);
                Console.WriteLine("");

                SoundPlayer successSound = new SoundPlayer(Properties.Resources.chime);
                if (variables.soundsuccess != "") successSound.SoundLocation = variables.soundsuccess;
                successSound.Play();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString() + i); }
            return 1;
        }

        private void Write_b()
        {
            if (listView1.SelectedItems.Count == 0) return;
            buttons(false);
            if (!force) write2(variables.filename1);
            else write(variables.filename1);
            if (variables.debugme) Console.WriteLine("changing back to old file");
            if (Path.GetExtension(variables.filename1) == ".ecc")
            {
                if (variables.tempfile != "")
                {
                    variables.filename1 = variables.tempfile;
                    UpdateSourc(variables.tempfile);
                }
            }
            buttons(true);
        }
        private void write(string filename)
        {
            if (!File.Exists(variables.filename1)) return;

            const uint OPEN_EXISTING = 3;
            const uint GENERIC_WRITE = (0x40000000);
            const uint FSCTL_LOCK_VOLUME = 0x00090018;
            const uint FSCTL_UNLOCK_VOLUME = 0x0009001c;
            const uint FSCTL_DISMOUNT_VOLUME = 0x00090020;

            if (listView1.SelectedItems.Count == 0) return;
            string ldrive = listView1.SelectedItems[0].SubItems[0].Text;
            if (variables.debugme) Console.WriteLine(ldrive);

            bool success = false;
            int intOut;
            string deviceId = @"\\.\" + ldrive;

            var diskGeometry = DiskGeometry.FromDevice(@"\\.\" + ldrive.Replace("\\", ""));

            List<string> logicaldrives = GetLetters(Convert.ToInt32(ldrive.Replace(@"PhysicalDrive", "").Replace("\\", "").Replace(".", "").ToString()));

            SafeFileHandle diskHandle = CreateFile(deviceId, GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
            if (diskHandle.IsInvalid)
            {
                Console.WriteLine(deviceId + " open error.");
                Console.WriteLine("");
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
                    Console.WriteLine("");
                    break;
                }
                if (variables.debugme) Console.WriteLine(ldevid + " " + Marshal.GetHRForLastWin32Error().ToString() + ": opened.");
                lhandles.Add(ldiskHandle);
                lnames.Add(ldevid);

                success = DeviceIoControl(ldiskHandle, FSCTL_LOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
                if (!success)
                {
                    Console.WriteLine(ldevid + " lock error.");
                    Console.WriteLine("");
                    CloseHandle(ldiskHandle);
                    break;
                }

                if (variables.debugme) Console.WriteLine(ldevid + " " + Marshal.GetHRForLastWin32Error().ToString() + ": locked.");

                success = DeviceIoControl(ldiskHandle, FSCTL_DISMOUNT_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
                if (!success)
                {
                    Console.WriteLine(ldevid + " " + Marshal.GetHRForLastWin32Error().ToString() + ": dismount error.");
                    Console.WriteLine("");
                    DeviceIoControl(ldiskHandle, FSCTL_UNLOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
                    CloseHandle(ldiskHandle);
                    return;
                }
            }
            success = DeviceIoControl(diskHandle, FSCTL_LOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
            if (!success)
            {
                Console.WriteLine(deviceId + " lock error.");
                Console.WriteLine("");
                CloseHandle(diskHandle);
                return;
            }

            if (variables.debugme) Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": locked.");

            success = DeviceIoControl(diskHandle, FSCTL_DISMOUNT_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
            if (!success)
            {
                Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": dismount error.");
                Console.WriteLine("");
                DeviceIoControl(diskHandle, FSCTL_UNLOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
                CloseHandle(diskHandle);
                return;
            }

            if (variables.debugme) Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": unmounted.");

            //uint numTotalSectors = 0x795FFF;//DiskSize / 512;
            //uint numTotalSectors = 0x702000;
            uint track = diskGeometry.BytesPerSector * (diskGeometry.Sector + 1);
            long totaltracks = diskGeometry.DiskSize / track;

            byte[] junkBytes = new byte[(int)track];

            FileStream fs = new FileStream(filename, FileMode.Open);
            if (fs.Length / (track) < totaltracks) totaltracks = (int)(fs.Length / (track));
            if (variables.debugme) Console.WriteLine(totaltracks);
            FileStream fw = new FileStream(diskHandle, FileAccess.ReadWrite);
            uint offset = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (uint sectorNum = 0; sectorNum < totaltracks; sectorNum++)
            {
                if (variables.escapeloop) break;
                int numBytesWritten = (int)diskGeometry.BytesPerSector;
                //int moveToHigh;
                try
                {
                    fs.Read(junkBytes, 0, (int)track);
                    offset++;
                    int value = (int)((offset * 100) / totaltracks);
                    //if (offset % 1000 == 0) Console.WriteLine(offset + " " + value);
                    UpdateProgres(value);
                }
                catch (Exception ex) { Console.WriteLine("{0} - {1} - {2}", offset, sectorNum, ex.ToString()); break; }


                //uint rvalsfp = SetFilePointer(diskHandle, sectorNum * numBytesPerSector, out moveToHigh, EMoveMethod.Begin);

                //Console.WriteLine("File pointer set " + Marshal.GetHRForLastWin32Error().ToString() + ": " + (sectorNum * numBytesPerSector).ToString());

                fw.Write(junkBytes, 0, (int)track);

                //int rval = WriteFile(diskHandle, junkBytes, junkBytes.Length, out numBytesWritten, IntPtr.Zero);

                if (numBytesWritten != junkBytes.Length)
                {
                    //Console.WriteLine("Write error on track " + sectorNum.ToString() + " from " + (sectorNum * numBytesPerSector).ToString() + "-" + moveToHigh.ToString() + " " + Marshal.GetHRForLastWin32Error().ToString() + ": Only " + numBytesWritten.ToString() + "/" + junkBytes.Length.ToString() + " bytes written.");
                    //break;
                }
                else
                {
                    //Console.WriteLine("Write success " + Marshal.GetHRForLastWin32Error().ToString() + ": " + numBytesWritten.ToString() + "/" + junkBytes.Length.ToString() + " bytes written.");
                }
            }
            stopwatch.Stop();
            Console.WriteLine("Write Successful! Time Elapsed: {0}:{1:D2}", stopwatch.Elapsed.Minutes + (stopwatch.Elapsed.Hours * 60), stopwatch.Elapsed.Seconds);
            Console.WriteLine("");
            fs.Close();

            SoundPlayer successSound = new SoundPlayer(Properties.Resources.chime);
            if (variables.soundsuccess != "") successSound.SoundLocation = variables.soundsuccess;
            successSound.Play();

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
                    Console.WriteLine("");
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
                Console.WriteLine("");
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
                    Console.WriteLine("");
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
                Console.WriteLine("");
            }
            try
            {
                fw.Close();
            }
            catch (Exception) { }
            Environment.GetLogicalDrives();
        }
        private void write2(string filename)
        {
            if (String.IsNullOrEmpty(filename)) return;
            if (listView1.SelectedItems.Count == 0) return;
            string ldrive = listView1.SelectedItems[0].SubItems[1].Text;
            if (listView1.SelectedItems[0].SubItems[2].Text != "Removable") { Console.WriteLine("Must be a removable type"); return; }
            if (MessageBox.Show("You are about to write to " + ldrive + ". Continue?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) return;

            var diskGeometry = DiskGeometry.FromDevice(@"\\.\" + ldrive.Replace("\\", ""));
            uint track = diskGeometry.BytesPerSector * (diskGeometry.Sector + 1);
            long tracks = diskGeometry.DiskSize / track;

            long i = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                Console.WriteLine("Writing to {0} from {1}", ldrive, filename);
                FileStream fs = new FileStream(CreateFile(@"\\.\" + ldrive.Replace("\\", ""), FileAccess.ReadWrite, FileShare.ReadWrite, 0, FileMode.Open, 0, IntPtr.Zero), FileAccess.ReadWrite);
                FileStream fw = new FileStream(filename, FileMode.OpenOrCreate);
                tracks = (fw.Length / track);
                byte[] temp = new byte[track];
                while (i < tracks && !variables.escapeloop)
                {
                    UpdateProgres((int)((i * 100) / tracks));
                    i++;
                    fw.Read(temp, 0, (int)track);
                    fs.Write(temp, 0, (int)track);
                }
                fs.Close();
                fw.Close();
                UpdateProgres(100);
                stopwatch.Stop();
                Console.WriteLine("Write Successful! Time Elapsed: {0}:{1:D2}:{2}", stopwatch.Elapsed.Minutes + (stopwatch.Elapsed.Hours * 60), stopwatch.Elapsed.Seconds, stopwatch.Elapsed.Milliseconds);
                Console.WriteLine("");

                SoundPlayer successSound = new SoundPlayer(Properties.Resources.chime);
                if (variables.soundsuccess != "") successSound.SoundLocation = variables.soundsuccess;
                successSound.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (variables.debugme) Console.WriteLine(ex.ToString());
                Console.WriteLine("");
            }
        }

        private void erase()
        {
            if (listView1.SelectedItems.Count == 0) return;
            buttons(false);
            if (force) erase_v1();
            else erase_v2();
            buttons(true);
        }
        private void erase_v1()
        {
            const uint OPEN_EXISTING = 3;
            const uint GENERIC_WRITE = (0x40000000);
            const uint FSCTL_LOCK_VOLUME = 0x00090018;
            const uint FSCTL_UNLOCK_VOLUME = 0x0009001c;
            const uint FSCTL_DISMOUNT_VOLUME = 0x00090020;

            if (listView1.SelectedItems.Count == 0) return;
            string ldrive = listView1.SelectedItems[0].SubItems[0].Text;
            if (variables.debugme) Console.WriteLine(ldrive);

            bool success = false;
            int intOut;
            string deviceId = @"\\.\" + ldrive;
            var diskGeometry = DiskGeometry.FromDevice(@"\\.\" + ldrive.Replace("\\", ""));
            List<string> logicaldrives = GetLetters(Convert.ToInt32(ldrive.Replace(@"PhysicalDrive", "").Replace("\\", "").Replace(".", "").ToString()));

            SafeFileHandle diskHandle = CreateFile(deviceId, GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
            if (diskHandle.IsInvalid)
            {
                Console.WriteLine(deviceId + " open error.");
                Console.WriteLine("");
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
                    Console.WriteLine("");
                    break;
                }
                if (variables.debugme) Console.WriteLine(ldevid + " " + Marshal.GetHRForLastWin32Error().ToString() + ": opened.");
                lhandles.Add(ldiskHandle);
                lnames.Add(ldevid);

                success = DeviceIoControl(ldiskHandle, FSCTL_LOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
                if (!success)
                {
                    Console.WriteLine(ldevid + " lock error.");
                    Console.WriteLine("");
                    CloseHandle(ldiskHandle);
                    break;
                }

                if (variables.debugme) Console.WriteLine(ldevid + " " + Marshal.GetHRForLastWin32Error().ToString() + ": locked.");

                success = DeviceIoControl(ldiskHandle, FSCTL_DISMOUNT_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
                if (!success)
                {
                    Console.WriteLine(ldevid + " " + Marshal.GetHRForLastWin32Error().ToString() + ": dismount error.");
                    Console.WriteLine("");
                    DeviceIoControl(ldiskHandle, FSCTL_UNLOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
                    CloseHandle(ldiskHandle);
                    return;
                }
            }
            success = DeviceIoControl(diskHandle, FSCTL_LOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
            if (!success)
            {
                Console.WriteLine(deviceId + " lock error.");
                Console.WriteLine("");
                CloseHandle(diskHandle);
                return;
            }

            if (variables.debugme) Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": locked.");

            success = DeviceIoControl(diskHandle, FSCTL_DISMOUNT_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
            if (!success)
            {
                Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": dismount error.");
                Console.WriteLine("");
                DeviceIoControl(diskHandle, FSCTL_UNLOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
                CloseHandle(diskHandle);
                return;
            }

            if (variables.debugme) Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": unmounted.");

            //uint numTotalSectors = 0x795FFF;//DiskSize / 512;
            //uint numTotalSectors = 0x702000;
            uint track = diskGeometry.BytesPerSector * (diskGeometry.Sector + 1);
            long totaltracks = diskGeometry.DiskSize / track;

            byte[] junkBytes = new byte[(int)track];

            if (variables.debugme) Console.WriteLine(totaltracks);
            FileStream fw = new FileStream(diskHandle, FileAccess.ReadWrite);
            uint offset = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (uint sectorNum = 0; sectorNum < totaltracks; sectorNum++)
            {
                if (variables.escapeloop) break;
                int numBytesWritten = (int)diskGeometry.BytesPerSector;
                //int moveToHigh;
                try
                {
                    offset++;
                    int value = (int)((offset * 100) / totaltracks);
                    //if (offset % 1000 == 0) Console.WriteLine(offset + " " + value);
                    UpdateProgres(value);
                }
                catch (Exception ex) { Console.WriteLine("{0} - {1} - {2}", offset, sectorNum, ex.ToString()); break; }


                //uint rvalsfp = SetFilePointer(diskHandle, sectorNum * numBytesPerSector, out moveToHigh, EMoveMethod.Begin);

                //Console.WriteLine("File pointer set " + Marshal.GetHRForLastWin32Error().ToString() + ": " + (sectorNum * numBytesPerSector).ToString());

                fw.Write(junkBytes, 0, (int)track);

                //int rval = WriteFile(diskHandle, junkBytes, junkBytes.Length, out numBytesWritten, IntPtr.Zero);

                if (numBytesWritten != junkBytes.Length)
                {
                    //Console.WriteLine("Write error on track " + sectorNum.ToString() + " from " + (sectorNum * numBytesPerSector).ToString() + "-" + moveToHigh.ToString() + " " + Marshal.GetHRForLastWin32Error().ToString() + ": Only " + numBytesWritten.ToString() + "/" + junkBytes.Length.ToString() + " bytes written.");
                    //break;
                }
                else
                {
                    //Console.WriteLine("Write success " + Marshal.GetHRForLastWin32Error().ToString() + ": " + numBytesWritten.ToString() + "/" + junkBytes.Length.ToString() + " bytes written.");
                }
            }
            stopwatch.Stop();
            Console.WriteLine("Erase Successful! Time Elapsed: {0}:{1:D2}", stopwatch.Elapsed.Minutes + (stopwatch.Elapsed.Hours * 60), stopwatch.Elapsed.Seconds);
            Console.WriteLine("");

            SoundPlayer successSound = new SoundPlayer(Properties.Resources.chime);
            if (variables.soundsuccess != "") successSound.SoundLocation = variables.soundsuccess;
            successSound.Play();

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
                    Console.WriteLine("");
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
                Console.WriteLine("");
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
                    Console.WriteLine("");
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
                Console.WriteLine("");
            }
            try
            {
                fw.Close();
            }
            catch (Exception) { }
            Environment.GetLogicalDrives();
        }
        private void erase_v2()
        {
            if (listView1.SelectedItems.Count == 0) return;
            string ldrive = listView1.SelectedItems[0].SubItems[1].Text;
            if (listView1.SelectedItems[0].SubItems[2].Text != "Removable") { Console.WriteLine("Must be a removable type"); return; }
            if (MessageBox.Show("You are about to erase " + ldrive + ". Continue?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) return;

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
                    UpdateProgres((int)((i * 100) / tracks));
                    i++;
                    fs.Write(temp, 0, (int)track);
                }
                fs.Close();
                UpdateProgres(100);
                stopwatch.Stop();
                Console.WriteLine("Erase Successful! Time Elapsed: {0}:{1:D2}:{2}", stopwatch.Elapsed.Minutes + (stopwatch.Elapsed.Hours * 60), stopwatch.Elapsed.Seconds, stopwatch.Elapsed.Milliseconds);
                Console.WriteLine("");

                SoundPlayer successSound = new SoundPlayer(Properties.Resources.chime);
                if (variables.soundsuccess != "") successSound.SoundLocation = variables.soundsuccess;
                successSound.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (variables.debugme) Console.WriteLine(ex.ToString());
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

        private void LDrives_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                variables.escapeloop = true;
                ThreadStart starter = delegate { escapedexit(5000); };
                new Thread(starter).Start();
            }
            else if (e.KeyCode == Keys.F2)
            {
                force = !force;
                if (force) Console.WriteLine("Alternative method");
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

            Console.WriteLine("            media type: {0}", diskGeometry.MediaTypeName);
            Console.WriteLine();

            Console.WriteLine("maximum linear address: {0}", diskGeometry.MaximumLinearAddress);
            Console.WriteLine("  last cylinder number: {0}", cubicAddress.Cylinder);
            Console.WriteLine("      last head number: {0}", cubicAddress.Head);
            Console.WriteLine("    last sector number: {0}", cubicAddress.Sector);
            Console.WriteLine();

            Console.WriteLine("             cylinders: {0}", diskGeometry.Cylinder);
            Console.WriteLine("   tracks per cylinder: {0}", diskGeometry.Head);
            Console.WriteLine("     sectors per track: {0}", diskGeometry.Sector);
            Console.WriteLine();

            Console.WriteLine("      bytes per sector: {0}", diskGeometry.BytesPerSector);
            Console.WriteLine("    bytes per cylinder: {0}", diskGeometry.BytesPerCylinder);
            Console.WriteLine("      total disk space: {0}", diskGeometry.DiskSize);
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

    }
}
