using HidLibrary;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WinUsb;

namespace JRunner
{
    class HID
    {
        private static HidDevice[] _deviceList = HidDevices.Enumerate(0x11d4, 0x8334).ToArray();
        private static HidDevice _selectedDevice;
        private static HidReport re;
        public static Boolean BootloaderDetected = false;
        //public delegate void ReadHandlerDelegate(HidReport report);
        private static IntPtr BootloaderNotificationHandle;
        private static DeviceManagement BootloaderManagement = new DeviceManagement();
        private static String BootloaderPathName;
        private static WinUsbDevice Bootloader = new WinUsbDevice();
        private static Boolean FindBootloader_WIN()
        {
            Boolean deviceFound;
            String devicePathName = "";
            Boolean success;

            try
            {
                if (!(HID.BootloaderDetected))
                {

                    //  Convert the device interface GUID String to a GUID object: 

                    System.Guid winUsbDemoGuid =
                        new System.Guid(variables.BOOTLOADER_GUID_STRING);

                    // Fill an array with the device path names of all attached devices with matching GUIDs.

                    deviceFound = BootloaderManagement.FindDeviceFromGuid
                        (winUsbDemoGuid,
                        ref devicePathName);
                    if (variables.debugme) Console.WriteLine("DemoN - {0}", devicePathName);
                    if (deviceFound == true)
                    {
                        success = Bootloader.GetDeviceHandle(devicePathName);

                        if (success)
                        {
                            if (variables.debugme) Console.WriteLine("Bootloader Attached");
                            HID.BootloaderDetected = true;

                            // Save DevicePathName so OnDeviceChange() knows which name is my device.

                            BootloaderPathName = devicePathName;
                        }
                        else
                        {
                            // There was a problem in retrieving the information.

                            HID.BootloaderDetected = false;
                            Bootloader.CloseDeviceHandle();
                        }
                    }

                    if (HID.BootloaderDetected)
                    {

                        // The device was detected.
                        // Register to receive notifications if the device is removed or attached.
                        if (variables.debugme) Console.WriteLine("Bootloader attached.-106");
                        success = BootloaderManagement.RegisterForDeviceNotifications
                            (BootloaderPathName,
                            MainForm.mainForm.Handle,
                            winUsbDemoGuid,
                            ref BootloaderNotificationHandle);
                        if (success)
                        {
                            if (Bootloader.InitializeDevice())
                            {
                                if (variables.debugme) Console.WriteLine("Bootloader Attached");
                            }
                            else
                            {
                                if (variables.debugme) Console.WriteLine("Failed 121");
                            }
                        }
                        else
                        {
                            if (variables.debugme) Console.WriteLine("Failed 126");
                        }
                    }
                    else
                    {
                        if (variables.debugme) Console.WriteLine("Bootloader Not Found.");
                    }
                }
                else
                {
                    if (variables.debugme) Console.WriteLine("Bootloader attached.-128");
                }


                return HID.BootloaderDetected;

            }
            catch (Exception ex)
            {
                if (variables.debugme) Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public static Boolean FindBootloader()
        {
            _deviceList = HidDevices.Enumerate(0x11d4, 0x8334).ToArray();
            if (_deviceList.Count() != 0) _selectedDevice = _deviceList[0];
            else return false;
            int version = HID._selectedDevice.Attributes.Version;
            Console.WriteLine("Bootloader Attached: {0:X}.{1:X}.{2:X}", version.ToString("X").Substring(0, 1), version.ToString("X").Substring(1, 1), version.ToString("X").Substring(2, 1));
            FindBootloader_WIN();
            BootloaderPathName = _selectedDevice.DevicePath;
            if (variables.debugme) Console.WriteLine("FindBootloader() - {0}", _selectedDevice.DevicePath);
            BootloaderDetected = true;
            return true;
        }

        private void parsehex()
        {
            try
            {
                byte[] file = File.ReadAllBytes(variables.filename1);
                if (file[0] != 0x3A) { Console.WriteLine("Bad file"); return; }
                byte[] length = new byte[2];
                byte[] address = new byte[4];
                byte[] record = new byte[2];
                byte[] line, write;
                int i = 0, len = 0, add = 0, rec = 0;
                while (!variables.escapeloop)
                {
                    length = Oper.returnportion(file, i + 1, 2);
                    address = Oper.returnportion(file, i + 3, 4);
                    record = Oper.returnportion(file, i + 7, 2);
                    len = Convert.ToInt32((ASCIIEncoding.ASCII.GetString(length)), 16);
                    rec = Convert.ToInt32((ASCIIEncoding.ASCII.GetString(record)), 16);
                    add = Convert.ToInt32((ASCIIEncoding.ASCII.GetString(address)), 16);
                    line = Oper.returnportion(file, i + 9, len * 2);
                    write = Oper.StringToByteArray(ASCIIEncoding.ASCII.GetString(line));
                    Console.WriteLine("len: 0x{0:X}", len);
                    Console.WriteLine("rec: 0x{0:X}", rec);
                    Console.WriteLine("add: 0x{0:X}", add);
                    //Console.WriteLine(ByteArrayToString(write));
                    Console.WriteLine("");
                    if ((add >= 0x800) && (add < 0x6000) && (rec == 0))
                    {
                        Console.WriteLine(Oper.ByteArrayToString(write));
                        Console.WriteLine("");
                    }
                    if (rec == 0x01) break;
                    i = i + 2 + 4 + 2 + len + len + 2 + 2 + 1;
                    if (file[i] != 0x3A) break;
                }
                Console.WriteLine("Done");
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            variables.escapeloop = false;
        }
        private static void erase()
        {
            byte[] command = new byte[8];
            command[1] = 0x02;
            command[4] = 0x08;
            command[6] = 0xE0;
            HidReport re = new HidReport(8, new HidDeviceData(command, HidDeviceData.ReadStatus.Success));
            if (!_selectedDevice.WriteReport(re, 5000)) Console.WriteLine("Erase Failed");
            Thread.Sleep(0x250);
            command[4] = 0x40;
            command[6] = 0x80;
            re = new HidReport(8, new HidDeviceData(command, HidDeviceData.ReadStatus.Success));
            if (!_selectedDevice.WriteReport(re, 5000)) Console.WriteLine("Erase Failed");
        }
        private static bool write(ref ProgressBar pb)
        {
            try
            {
                string filename = variables.filename1;
                FileInfo fl = new FileInfo(filename);
                bool rty = false;
                int reTRY = 0;
                int filesize = (int)fl.Length;
                byte[] file = File.ReadAllBytes(filename);
                if (file[0] != 0x3A) { Console.WriteLine("Bad file"); return false; }
                byte[] length = new byte[2];
                byte[] address = new byte[4];
                byte[] record = new byte[2];
                byte[] line, write, offset;
                int i = 0, len = 0, add = 0, rec = 0;
                while (!variables.escapeloop)
                {
                    length = Oper.returnportion(file, i + 1, 2);
                    address = Oper.returnportion(file, i + 3, 4);
                    record = Oper.returnportion(file, i + 7, 2);
                    len = Convert.ToInt32((ASCIIEncoding.ASCII.GetString(length)), 16);
                    rec = Convert.ToInt32((ASCIIEncoding.ASCII.GetString(record)), 16);
                    add = Convert.ToInt32((ASCIIEncoding.ASCII.GetString(address)), 16);
                    offset = Oper.StringToByteArray(ASCIIEncoding.ASCII.GetString(address));
                    Array.Reverse(offset);
                    line = Oper.returnportion(file, i + 9, len * 2);
                    write = Oper.StringToByteArray(ASCIIEncoding.ASCII.GetString(line));
                    if ((add >= 0x800) && (add < 0x6000) && (rec == 0))
                    {
                        byte[] send = new byte[0x3F];
                        send[1] = 0x1;
                        send[6] = 0x10;
                        Buffer.BlockCopy(offset, 0, send, 3, 2);
                        Buffer.BlockCopy(write, 0, send, 0x7, write.Length);
                        pb.Value = ((i * pb.Maximum) / filesize);
                        re = new HidReport(0x3F, new HidDeviceData(send, HidDeviceData.ReadStatus.Success));
                        if (!_selectedDevice.WriteReport(re, 5000))
                        {
                            Console.Write(".");
                            reTRY++;
                            if (variables.debugme) Console.WriteLine(" Retry: " + reTRY);

                            if (variables.debugme) Console.WriteLine(" Command: {0:X}", Oper.ByteArrayToString(send));
                            if (variables.debugme) Console.WriteLine(" Data: {0:X}", Oper.ByteArrayToString(write));
                            if (variables.debugme) Console.WriteLine(" Offset: {0:X}", Oper.ByteArrayToString(offset));

                            rty = true;
                        }
                        else if (i % 8 == 0)
                        {
                            rty = false;
                            reTRY = 0;
                            //i = i + 2 + 4 + 2 + len + len + 2 + 2 + 1;
                        }
                        Thread.Sleep(0x7);
                    }
                    if (rec == 0x01) break;
                    if (!rty) i = i + 2 + 4 + 2 + len + len + 2 + 2 + 1;
                    if (reTRY >= 0x0A) return false;
                    if (file[i] != 0x3A) break;
                }
                return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); return false; }
            finally { variables.escapeloop = false; }
        }
        private static bool verify(ref ProgressBar pb)
        {
            try
            {
                string filename = variables.filename1;
                FileInfo fl = new FileInfo(filename);
                int filesize = (int)fl.Length;
                byte[] file = File.ReadAllBytes(filename);
                if (file[0] != 0x3A) { Console.WriteLine("Bad file"); return false; }
                byte[] length = new byte[2];
                byte[] address = new byte[4];
                byte[] record = new byte[2];
                byte[] line, write, offset;
                int i = 0, len = 0, add = 0, rec = 0;
                while (!variables.escapeloop)
                {
                    length = Oper.returnportion(file, i + 1, 2);
                    address = Oper.returnportion(file, i + 3, 4);
                    record = Oper.returnportion(file, i + 7, 2);
                    len = Convert.ToInt32((ASCIIEncoding.ASCII.GetString(length)), 16);
                    rec = Convert.ToInt32((ASCIIEncoding.ASCII.GetString(record)), 16);
                    add = Convert.ToInt32((ASCIIEncoding.ASCII.GetString(address)), 16);
                    offset = Oper.StringToByteArray(ASCIIEncoding.ASCII.GetString(address));
                    Array.Reverse(offset);
                    line = Oper.returnportion(file, i + 9, len * 2);
                    write = Oper.StringToByteArray(ASCIIEncoding.ASCII.GetString(line));
                    if ((add >= 0x800) && (add < 0x6000) && (rec == 0))
                    {
                        byte[] send = new byte[0x3F];
                        send[1] = 0x5;
                        send[6] = 0x10;
                        Buffer.BlockCopy(offset, 0, send, 3, 2);
                        pb.Value = ((i * pb.Maximum) / filesize);
                        re = new HidReport(0x3F, new HidDeviceData(send, HidDeviceData.ReadStatus.Success));
                        byte[] returnb = null;
                        if (!_selectedDevice.WriteReport(re, 5000)) Console.Write("X");
                        else
                        {
                            re = _selectedDevice.ReadReport(5000);
                            returnb = re.Data;
                        }
                        if (!Oper.ByteArrayCompare(Oper.returnportion(returnb, 6, write.Length), write))
                        {
                            //if (comboBox1.Text == "2") Console.WriteLine("Data: {0:X}", ByteArrayToString(datab));
                            if (variables.debugme) Console.WriteLine("Return: {0:X}", Oper.ByteArrayToString(returnb));
                            if (variables.debugme) Console.WriteLine("Data: {0:X}", Oper.ByteArrayToString(Oper.returnportion(returnb, 6, 0x10)));
                            if (variables.debugme) Console.WriteLine("File: {0:X}", Oper.ByteArrayToString(write));
                        }
                        Thread.Sleep(0x5);
                    }
                    if (rec == 0x01) break;
                    i = i + 2 + 4 + 2 + len + len + 2 + 2 + 1;
                    if (file[i] != 0x3A) break;
                }
                variables.escapeloop = false;
                return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); return false; }
        }
        private static void restart()
        {
            byte[] send = new byte[2];
            send[1] = 0x04;
            HidReport re = new HidReport(2, new HidDeviceData(send, HidDeviceData.ReadStatus.Success));
            if (!_selectedDevice.WriteReport(re, 5000)) Console.WriteLine("Failed");
        }
        public static void program(ref ProgressBar pb)
        {
            if (String.IsNullOrEmpty(variables.filename1)) { MessageBox.Show("No file was selected!", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (_deviceList.Count() != 0) _selectedDevice = _deviceList[0];
            else { Console.WriteLine("Device Not Found"); return; }
            if (_selectedDevice.IsConnected)
            {
                if (!_selectedDevice.IsOpen) _selectedDevice.OpenDevice();
                Console.WriteLine("Erasing Flash...");
                erase();
                Thread.Sleep(175);
                Console.WriteLine("Writing Flash...");
                if (!write(ref pb))
                {
                    Console.WriteLine("Write Failed");
                    Console.WriteLine("");
                    return;
                }
                Thread.Sleep(10);
                pb.Value = 0;
                _selectedDevice.CloseDevice();
                _selectedDevice.OpenDevice();
                Console.WriteLine("Verifying Flash...");
                if (verify(ref pb)) Console.WriteLine("Write Successful");
                else
                {
                    Console.WriteLine("Write Failed");
                    Console.WriteLine("");
                    return;
                }
                pb.Value = pb.Maximum;
                Thread.Sleep(10);
                Console.WriteLine("Restarting Device...");
                restart();
                _selectedDevice.CloseDevice();
                Console.WriteLine("Done");
                Console.WriteLine("");
            }
        }

        public static void ver(ref ProgressBar pb)
        {
            if (String.IsNullOrEmpty(variables.filename1)) { MessageBox.Show("No file was selected!", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (_deviceList.Count() != 0) _selectedDevice = _deviceList[0];
            else { Console.WriteLine("Device Not Found"); return; }
            if (_selectedDevice.IsConnected)
            {
                if (!_selectedDevice.IsOpen) _selectedDevice.OpenDevice();
                Console.WriteLine("Verifying Flash...");
                if (verify(ref pb)) Console.WriteLine("Write Successful");
                else
                {
                    Console.WriteLine("Write Failed");
                    Console.WriteLine("");
                    return;
                }
                pb.Value = pb.Maximum;
                Thread.Sleep(10);
                Console.WriteLine("Restarting Device...");
                restart();
                _selectedDevice.CloseDevice();
                Console.WriteLine("Done");
                Console.WriteLine("");
            }
        }
    }
}
