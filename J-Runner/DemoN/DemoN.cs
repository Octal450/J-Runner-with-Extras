using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WinUsb;


namespace JRunner
{
    class DemoN
    {
        private static IntPtr DemonNotificationHandle;
        public static DeviceManagement DemonManagement = new DeviceManagement();
        public static Boolean DemonDetected = false, isOpen = false;
        public static String DemonPathName;
        public static WinUsbDevice Demon = new WinUsbDevice();
        private Demon_Switch flash;
        public static Demon_Modes mode;
        private byte[] fw, flashid, devid, nand;
        private int devi = -1, manu = -1;
        private bool convert = true;

        public delegate void updateProgress(int progress);
        public event updateProgress UpdateProgres;
        public delegate void updateBlock(string block);
        public event updateBlock UpdateBloc;
        public delegate void updateMode(Demon_Modes mode);
        public event updateMode updateMod;
        public delegate void updateFlash(Demon_Switch flash);
        public event updateFlash updateFlas;
        public delegate void updateVersion(string version);
        public event updateVersion UpdateVer;

        public enum Demon_Modes : byte
        {
            BOOTLOADER = 0x00,
            FIRMWARE = 0x01
        }
        public enum Demon_Switch : byte
        {
            XBOX360 = 0x00,
            DEMON = 0x01
        }
        enum Demon_Commands : byte
        {
            COMMAND_GET_MODE = 0x00, // rep 00 = bootloader; 01 = nrm
            COMMAND_GET_PROTOCOL_VERSION = 0x01,// returns 2 bytes
            COMMAND_GET_DEVICE_ID = 0x02, // returns 2 bytes
            COMMAND_GET_FIRMWARE_VERSION = 0x03,// two byte reply, first byte minor second byte major
            COMMAND_RUN_BOOTLOADER = 0x04,
            COMMAND_GET_EXT_FLASH = 0x05, // rep 00 = int; 01 = demflash
            COMMAND_SET_EXT_FLASH = 0x06,
            COMMAND_ACQUIRE_EXT_FLASH = 0x07,
            COMMAND_RELEASE_EXT_FLASH = 0x08,
            COMMAND_GET_EXT_FLASH_ID = 0x09,// 0xDA 0xAD (0xADDA) - 256M; 0x73 0xAD 16M; 0x76 0xAD 64M
            COMMAND_GET_INVALID_BLOCKS = 0x0a,
            COMMAND_ERASE_EXT_FLASH_BLOCK = 0x0b,
            COMMAND_ERASE_ALL_EXT_FLASH_BLOCKS = 0x0c,
            COMMAND_READ_EXT_FLASH_BLOCK = 0x0d,
            COMMAND_PROGRAM_EXT_FLASH_BLOCK = 0x0e,
            COMMAND_ASSERT_SB_RESET = 0x0f,
            COMMAND_DEASSERT_SB_RESET = 0x10,
            COMMAND_READ_SERIAL_PORT = 0x11,// returns 00 00 on no data
            COMMAND_WRITE_SERIAL_PORT = 0x12,
            COMMAND_EXEC_XSVF = 0x13,
            COMMAND_POWER_ON = 0x14,
            COMMAND_POWER_OFF = 0x15,
            COMMAND_GET_BOOTLOADER_VERSION = 0x80,
            COMMAND_RUN_FIRMWARE = 0x81,
            COMMAND_CHECK_FIRMWARE = 0x82,
            COMMAND_BEGIN_FIRMWARE_UPDATE = 0x83,
            COMMAND_END_FIRMWARE_UPDATE = 0x84,
            COMMAND_READ_INT_FLASH_PAGE = 0x85,
            COMMAND_ERASE_INT_FLASH_PAGE = 0x86,
            COMMAND_PROGRAM_INT_FLASH_PAGE = 0x87
        }

        private string XSVF_ERROR(int error)
        {
            if (error == 0x00) return "None";
            else if (error == 0x01) return "UNKNOWN";
            else if (error == 0x02) return "TDO MISMATCH";
            else if (error == 0x03) return "MAX Retries";
            else if (error == 0x04) return "Illegal Command";
            else if (error == 0x05) return "Illegal State";
            else if (error == 0x06) return "Data Overflow";
            else if (error == 0x07) return "IO";
            else return "";
        }


        public static Boolean FindDemon()
        {
            Boolean deviceFound;
            String devicePathName = "";
            Boolean success;

            try
            {
                if (!(DemonDetected))
                {
                    Thread.Sleep(100);
                    //  Convert the device interface GUID String to a GUID object: 
                    System.Guid winUsbDemoGuid =
                        new System.Guid(variables.DEMON_GUID_STRING);

                    deviceFound = DemonManagement.FindDeviceFromGuid(winUsbDemoGuid, ref devicePathName);
                    if (deviceFound == true)
                    {
                        success = true;
                        if (success)
                        {
                            if (variables.debugme) Console.WriteLine("DemoN detected-174");
                            DemonDetected = true;

                            // Save DevicePathName so OnDeviceChange() knows which name is my device.

                            DemonPathName = devicePathName;
                        }
                        else
                        {
                            // There was a problem in retrieving the information.
                            if (variables.debugme) Console.WriteLine("DemoN not detected-230");
                            DemonDetected = false;
                            DeInitDemoN();
                        }
                    }
                    //InitDemoN(devicePathName);

                    if (DemonDetected)
                    {

                        // The device was detected.
                        // Register to receive notifications if the device is removed or attached.

                        success = DemonManagement.RegisterForDeviceNotifications
                            (DemonPathName,
                            MainForm.mainForm.Handle,
                            winUsbDemoGuid,
                            ref DemonNotificationHandle);
                        if (variables.debugme) Console.WriteLine("Registered for notifications {0}", success);
                    }
                    else
                    {
                        if (variables.debugme) Console.WriteLine("DemoN not found.-211");
                    }
                }
                else
                {
                    if (variables.debugme) Console.WriteLine("DemoN detected.-216");
                }


                return DemonDetected;

            }
            catch (Exception ex)
            {
                if (variables.debugme) Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public Boolean InitDemoN(string devicePathName)
        {
            if (isOpen) Thread.Sleep(10);
            if (isOpen) { if (variables.debugme) { Console.WriteLine("Device is Already Open"); } return false; }
            try
            {
                System.Guid winUsbDemoGuid =
                            new System.Guid(variables.DEMON_GUID_STRING);
                bool success = false;
                if (DemonManagement.FindDeviceFromGuid(winUsbDemoGuid, ref devicePathName))
                {
                    // Fill an array with the device path names of all attached devices with matching GUIDs.

                    success = Demon.GetDeviceHandle(devicePathName);

                    if (success)
                    {
                        isOpen = true;
                        if (Demon.InitializeDevice())
                        {
                            //if (variables.debugme) Console.WriteLine("Initialized Device.");
                        }
                    }
                    else
                    {
                        if (variables.debugme) Console.WriteLine("Failed. 181");
                        return false;
                    }
                }
                else
                {
                    if (variables.debugme) Console.WriteLine("No device. 187");
                    DemonDetected = false;
                }
                return success;
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            return false;
        }
        public static void DeInitDemoN()
        {
            if (isOpen)
            {
                try
                {
                    Demon.CloseDeviceHandle();
                }
                catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
                isOpen = false;
            }
        }

        public void read(string filename, int startblock = 0, int length = 0)
        {
            if (DemonDetected)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                try
                {
                    File.Delete(filename);
                }
                catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
                if (!InitDemoN(DemonPathName))
                {
                    Console.WriteLine("Failed to grab handle");
                    return;
                }
                if (variables.iterations == 1)
                {
                    byte flas = new byte();
                    flas = demon_get_external_flash();
                    Console.WriteLine(((Demon_Switch)flas).ToString());
                    if (nand[0] != flas)
                    {
                        if (DialogResult.Cancel == MessageBox.Show(((Demon_Switch)flas).ToString() + " Nand is selected, Proceed?", "Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                        {
                            Console.WriteLine("Cancelled");
                            Console.WriteLine("");
                            DeInitDemoN();
                            return;
                        };
                    }
                    devid = demon_get_id();
                    fw = demon_get_firmware();

                    flashid = demon_get_external_flash_id();
                    List<int> Invalid_Blocks = new List<int>();
                    if (!demon_get_Invalid_Blocks(ref Invalid_Blocks)) { DeInitDemoN(); return; }
                    if (!setNand()) { DeInitDemoN(); return; }
                    string hw = "";
                    if (devid[0] == 0x01 && devid[1] == 0x00) hw = "Slim";
                    else if (devid[0] == 0x00 && devid[1] == 0x00) hw = "Phat";
                    if (hw == "") Console.WriteLine("Hardware   : Demon ({0:X}.{1:X})", ((devid[1]) & 0xFF), (devid[0] & 0xFF));
                    else Console.WriteLine("Hardware   : Demon {0}", hw);
                    Console.WriteLine("Firmware   : {0}.{1}", ((fw[1]) & 0xFF), (fw[0] & 0xFF));
                    Console.WriteLine("Flash ID   : 0x{0:4X} {1} ({2} - {3} block)", Oper.ByteArrayToString(flashid), getManuString(), getNameString(), Nands.Fdevi[devi].Bigblock ? "Big" : "Small");
                    Console.WriteLine("Flash Size : 0x{0:X} blocks of 0x{1:X} bytes", getNumBlocks(), getBlockSize());
                    if (Invalid_Blocks.Count > 0)
                    {
                        Console.Write("Invalid Blocks: ");
                        foreach (int n in Invalid_Blocks)
                        {
                            Console.Write("0x{0:X} ", n);
                        }
                        Console.WriteLine("");
                    }
                }
                acquire_flash();
                int blsize = getBlockSize();
                if (length == 0) length = (int)getNumBlocks();
                bool isBigBlock = Nands.Fdevi[devi].Bigblock;
                if (variables.debugme) Console.WriteLine("filename {0}\n Startblock {1:X} - Length {2:X}", filename, startblock, length);

                read_DemoN(filename, startblock, length, blsize, isBigBlock);

                release_flash();
                DeInitDemoN();
                stopwatch.Stop();
                UpdateProgres(100);
                Console.WriteLine("Read Successful! Time Elapsed: {0}:{1:D2}", stopwatch.Elapsed.Minutes + (stopwatch.Elapsed.Hours * 60), stopwatch.Elapsed.Seconds);
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Device Not Found");
            }
            MainForm._waitmb.Reset();
            MainForm._waitmb.Set();
            if (variables.iterations == 1)
            {
                if (File.Exists(Path.Combine(variables.pathforit, variables.filename)))
                {
                    variables.filename1 = Path.Combine(variables.pathforit, variables.filename);
                }
            }
            if (variables.iterations >= 2)
            {
                if (File.Exists(Path.Combine(variables.pathforit, variables.filename)))
                {
                    variables.filename2 = Path.Combine(variables.pathforit, variables.filename);
                }
            }
            try
            {
                SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                if (variables.soundsuccess != "") success.SoundLocation = variables.soundsuccess;
                success.Play();
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); };

        }
        public void read_DemoN(string filename, long startblock, long length, long blocklength, bool bigblock)
        {
            byte[] databuffer = new byte[1];
            byte[] blockbuffer = new byte[2];
            byte[] readBuf = new byte[blocklength];
            Console.WriteLine("Reading Nand");
            BinaryWriter sw = new BinaryWriter(File.Open(filename, FileMode.Append, FileAccess.Write));
            long i = startblock;
            databuffer[0] = ((byte)Demon_Commands.COMMAND_READ_EXT_FLASH_BLOCK);
            while (i < (length + startblock) && !variables.escapeloop)
            {
                UpdateBloc(i.ToString("X"));
                UpdateProgres((int)((100 * i) / (length + startblock)));
                blockbuffer[0] = (byte)(i & 0xFF);
                blockbuffer[1] = (byte)((i & 0xFF00) / 255);

                if (!device_bulk_write(Demon, databuffer, Convert.ToUInt32(databuffer.Length)))
                {
                    if (variables.debugme) Console.WriteLine("Failed to send command");
                }
                if (!device_bulk_write(Demon, blockbuffer, Convert.ToUInt32(blockbuffer.Length)))
                {
                    if (variables.debugme) Console.WriteLine("Failed to send block");
                }
                if (!device_bulk_read(Demon, ref readBuf, (uint)blocklength))
                {
                    if (variables.debugme) Console.WriteLine("Failed to read");
                }

                if (Nands.Fdevi[devi].Bigblock && convert)
                {
                    Nand.Nand.sparedatatonormal(ref readBuf);
                }

                try
                {
                    sw.Write(readBuf);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                i++;
            }
            readBuf = null;
            UpdateBloc("");
            sw.Close();
        }

        public void write(string filename, int startblock = 0, int length = 0)
        {
            if (DemonDetected)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                if (!InitDemoN(DemonPathName))
                {
                    Console.WriteLine("Failed to grab handle");
                    return;
                }

                byte flas = new byte();
                flas = demon_get_external_flash();
                Console.WriteLine(((Demon_Switch)flas).ToString());
                if (nand[0] != flas)
                {
                    if (DialogResult.Cancel == MessageBox.Show(((Demon_Switch)flas).ToString() + " Nand is selected, Proceed?", "Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                    {
                        Console.WriteLine("Cancelled");
                        Console.WriteLine("");
                        DeInitDemoN();
                        return;
                    };
                }
                devid = demon_get_id();
                fw = demon_get_firmware();

                flashid = demon_get_external_flash_id();
                List<int> Invalid_Blocks = new List<int>();
                if (!demon_get_Invalid_Blocks(ref Invalid_Blocks)) { DeInitDemoN(); return; }
                if (!setNand()) { DeInitDemoN(); return; }
                string hw = "";
                if (devid[0] == 0x01 && devid[1] == 0x00) hw = "Slim";
                else if (devid[0] == 0x00 && devid[1] == 0x00) hw = "Phat";
                if (hw == "") Console.WriteLine("Hardware   : Demon ({0:X}.{1:X})", ((devid[1]) & 0xFF), (devid[0] & 0xFF));
                else Console.WriteLine("Hardware   : Demon {0}", hw);
                Console.WriteLine("Firmware   : {0}.{1}", ((fw[1]) & 0xFF), (fw[0] & 0xFF));
                Console.WriteLine("Flash ID   : 0x{0:4X} {1} ({2} - {3} block)", Oper.ByteArrayToString(flashid), getManuString(), getNameString(), Nands.Fdevi[devi].Bigblock ? "Big" : "Small");
                Console.WriteLine("Flash Size : 0x{0:X} blocks of 0x{1:X} bytes", getNumBlocks(), getBlockSize());
                if (Invalid_Blocks.Count > 0)
                {
                    Console.Write("Invalid Blocks : ");
                    foreach (int n in Invalid_Blocks)
                    {
                        Console.Write("0x{0:X} ", n);
                    }
                    Console.WriteLine("");
                }

                acquire_flash();
                int blsize = getBlockSize();
                if (length == 0) length = (int)getNumBlocks();
                bool isBigBlock = Nands.Fdevi[devi].Bigblock;
                if (variables.debugme) Console.WriteLine("filename {0}\n Startblock {1:X} - Length {2:X}", filename, startblock, length);

                write_DemoN(filename, startblock, length, blsize, isBigBlock);

                release_flash();
                DeInitDemoN();
                stopwatch.Stop();
                UpdateProgres(100);
                Console.WriteLine("Write Successful! Time Elapsed: {0}:{1:D2}", stopwatch.Elapsed.Minutes + (stopwatch.Elapsed.Hours * 60), stopwatch.Elapsed.Seconds);
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Device Not Found");
            }
            MainForm._waitmb.Reset();
            MainForm._waitmb.Set();

            try
            {
                SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                if (variables.soundsuccess != "") success.SoundLocation = variables.soundsuccess;
                success.Play();
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); };
        }
        public void write_DemoN(string filename, int startblock, int length, int blocklength, bool bigblock)
        {
            byte[] databuffer = new byte[1];
            byte[] blockbuffer = new byte[2];
            byte[] writeBuffer = new byte[blocklength];
            BinaryReader rw = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));

            long filesize;
            FileInfo fl = new FileInfo(filename);
            filesize = fl.Length / blocklength;
            if (length == 0) length = (int)filesize;
            if (startblock + length > filesize)
            {
                length = (int)filesize - startblock;
            }
            if (length <= 0) length = 0;
            if (variables.debugme) Console.WriteLine("filename {0}\n Startblock {1:X} - Length {2:X}", filename, startblock, length);
            Console.WriteLine("Writing Nand");
            int i = startblock;
            while (i < (length + startblock) && !variables.escapeloop)
            {
                UpdateBloc(i.ToString("X"));
                UpdateProgres((100 * i) / (length + startblock));
                blockbuffer[0] = (byte)(i & 0xFF);
                blockbuffer[1] = (byte)((i & 0xFF00) / 255);

                writeBuffer = rw.ReadBytes(blocklength);

                if (Nands.Fdevi[devi].Bigblock && convert)
                {
                    Nand.Nand.sparedatatoraw(ref writeBuffer);
                }

                databuffer[0] = (byte)Demon_Commands.COMMAND_ERASE_EXT_FLASH_BLOCK;
                if (!device_bulk_write(Demon, databuffer, Convert.ToUInt32(databuffer.Length)))
                {
                    if (variables.debugme) Console.WriteLine("Failed to erase");
                }
                if (!device_bulk_write(Demon, blockbuffer, Convert.ToUInt32(blockbuffer.Length)))
                {
                    if (variables.debugme) Console.WriteLine("Failed to send eblocknumber");
                }

                databuffer[0] = (byte)Demon_Commands.COMMAND_PROGRAM_EXT_FLASH_BLOCK;
                if (!device_bulk_write(Demon, databuffer, Convert.ToUInt32(databuffer.Length)))
                {
                    if (variables.debugme) Console.WriteLine("Failed to program");
                }
                if (!device_bulk_write(Demon, blockbuffer, Convert.ToUInt32(blockbuffer.Length)))
                {
                    if (variables.debugme) Console.WriteLine("Failed to send blocknumber");
                }
                if (!device_bulk_write(Demon, writeBuffer, Convert.ToUInt32(writeBuffer.Length)))
                {
                    if (variables.debugme) Console.WriteLine("Failed to send block");
                }


                i++;
            }
            rw.Close();
            UpdateBloc("");
            writeBuffer = null;
        }

        public void erase(int startblock = 0, int length = 0)
        {
            if (DemonDetected)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                if (!InitDemoN(DemonPathName))
                {
                    Console.WriteLine("Failed to grab handle");
                    return;
                }

                byte flas = new byte();
                flas = demon_get_external_flash();
                Console.WriteLine(((Demon_Switch)flas).ToString());
                if (nand[0] != flas)
                {
                    if (DialogResult.Cancel == MessageBox.Show(((Demon_Switch)flas).ToString() + " Nand is selected, Proceed?", "Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                    {
                        Console.WriteLine("Cancelled");
                        Console.WriteLine("");
                        DeInitDemoN();
                        return;
                    };
                }
                devid = demon_get_id();
                fw = demon_get_firmware();

                flashid = demon_get_external_flash_id();
                List<int> Invalid_Blocks = new List<int>();
                if (!demon_get_Invalid_Blocks(ref Invalid_Blocks)) { DeInitDemoN(); return; }
                if (!setNand()) { DeInitDemoN(); return; }
                string hw = "";
                if (devid[0] == 0x01 && devid[1] == 0x00) hw = "Slim";
                else if (devid[0] == 0x00 && devid[1] == 0x00) hw = "Phat";
                if (hw == "") Console.WriteLine("Hardware   : Demon ({0:X}.{1:X})", ((devid[1]) & 0xFF), (devid[0] & 0xFF));
                else Console.WriteLine("Hardware   : Demon {0}", hw);
                Console.WriteLine("Firmware   : {0}.{1}", ((fw[1]) & 0xFF), (fw[0] & 0xFF));
                Console.WriteLine("Flash ID   : 0x{0:4X} {1} ({2} - {3} block)", Oper.ByteArrayToString(flashid), getManuString(), getNameString(), Nands.Fdevi[devi].Bigblock ? "Big" : "Small");
                Console.WriteLine("Flash Size : 0x{0:X} blocks of 0x{1:X} bytes", getNumBlocks(), getBlockSize());
                if (Invalid_Blocks.Count > 0)
                {
                    Console.Write("Invalid Blocks : ");
                    foreach (int n in Invalid_Blocks)
                    {
                        Console.Write("0x{0:X} ", n);
                    }
                    Console.WriteLine("");
                }
                acquire_flash();
                int blsize = getBlockSize();
                bool isBigBlock = Nands.Fdevi[devi].Bigblock;
                if (length == 0)
                {
                    Console.WriteLine("Erasing Whole Nand");
                    byte[] databuffer = new byte[1];
                    databuffer[0] = (byte)Demon_Commands.COMMAND_ERASE_ALL_EXT_FLASH_BLOCKS;
                    if (!device_bulk_write(Demon, databuffer, Convert.ToUInt32(databuffer.Length)))
                    {
                        if (variables.debugme) Console.WriteLine("Failed to erase");
                    }
                }
                else
                {
                    if (variables.debugme) Console.WriteLine("Startblock {1:X} - Length {2:X}", startblock, length);
                    erase_DemoN(startblock, length, blsize, isBigBlock);
                }

                release_flash();
                DeInitDemoN();
                stopwatch.Stop();
                UpdateProgres(100);
                Console.WriteLine("Erase Successful! Time Elapsed: {0}:{1:D2}", stopwatch.Elapsed.Minutes + (stopwatch.Elapsed.Hours * 60), stopwatch.Elapsed.Seconds);
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Device Not Found");
            }
            MainForm._waitmb.Reset();
            MainForm._waitmb.Set();

            try
            {
                SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                if (variables.soundsuccess != "") success.SoundLocation = variables.soundsuccess;
                success.Play();
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); };
        }
        public void erase_DemoN(int startblock, int length, int blocklength, bool bigblock)
        {
            byte[] databuffer = new byte[1];
            byte[] blockbuffer = new byte[2];

            Console.WriteLine("Erasing Nand");
            int i = startblock;
            databuffer[0] = (byte)Demon_Commands.COMMAND_ERASE_EXT_FLASH_BLOCK;
            while (i < (length + startblock))
            {
                UpdateBloc(i.ToString("X"));
                UpdateProgres((100 * i) / (length + startblock));
                blockbuffer[0] = (byte)(i & 0xFF);
                blockbuffer[1] = (byte)((i & 0xFF00) / 255);

                if (!device_bulk_write(Demon, databuffer, Convert.ToUInt32(databuffer.Length)))
                {
                    if (variables.debugme) Console.WriteLine("Failed to erase");
                }
                if (!device_bulk_write(Demon, blockbuffer, Convert.ToUInt32(blockbuffer.Length)))
                {
                    if (variables.debugme) Console.WriteLine("Failed to send eblocknumber");
                }

                i++;
            }
            UpdateBloc("");
        }

        public void xsvf(string filename)
        {
            if (DemonDetected)
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine("Couldnt find file {0}", filename);
                    return;
                }
                if (Path.GetExtension(filename) != ".xsvf")
                {
                    Console.WriteLine("Wrong file {0}", filename);
                    return;
                }
                byte[] writeBuffer = new byte[64];
                byte[] readbuffer = new byte[1];
                byte[] blength = new byte[4];
                long filesize;
                FileInfo fl = new FileInfo(filename);
                filesize = fl.Length;
                byte[] file = new byte[filesize];
                if (variables.debugme) Console.WriteLine("Filesize {0}", filesize);
                BinaryReader rw = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));
                file = rw.ReadBytes((int)filesize);
                rw.Close();
                blength[0] = (byte)((filesize) & 0xFF);
                blength[1] = (byte)((filesize >> 8) & 0xFF);
                blength[2] = (byte)((filesize >> 16) & 0xFF);
                blength[3] = (byte)((filesize >> 24) & 0xFF);
                int rem = (int)filesize % 64; // remaining bytes from size/64
                int reps = (int)filesize / 64;
                if (variables.debugme) Console.WriteLine("filesize 0x{0:X} - iterations 0x{1:X} - remaining 0x{2:X}", filesize, reps, rem);
                Console.WriteLine("sending xsvf to demon...");
                if (!InitDemoN(DemonPathName))
                {
                    Console.WriteLine("Failed to grab handle");
                    return;
                }
                device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_EXEC_XSVF);
                if (variables.debugme) Console.WriteLine("blen {0}", Oper.ByteArrayToString(blength));
                device_bulk_write(Demon, blength, 4); // send xsvf size
                int i = 0;
                for (i = 0; i < reps; i++)
                {
                    UpdateProgres((i * 100) / reps);
                    writeBuffer = Oper.returnportion(file, i * 64, 64);
                    device_bulk_write(Demon, writeBuffer, 64);
                    device_bulk_read(Demon, ref readbuffer, 1);
                    if (readbuffer[0] != 0)
                    {
                        if (variables.debugme) Console.WriteLine("0x{0:X}", readbuffer[0]);
                        if (XSVF_ERROR(readbuffer[0]) != "")
                        {
                            Console.WriteLine("Failed - {0}", XSVF_ERROR(readbuffer[0]));
                        }
                        else Console.WriteLine("Failed");
                        break;
                    }
                }
                if (variables.debugme) Console.WriteLine("counter - 0x{0:X}", i);
                if (i == reps)
                {
                    if (rem != 0)
                    {
                        writeBuffer = Oper.returnportion(file, i * 64, rem);
                        device_bulk_write(Demon, writeBuffer, (uint)rem);
                        device_bulk_read(Demon, ref readbuffer, 1);
                    }
                    if (readbuffer[0] != 0)
                    {
                        if (variables.debugme) Console.WriteLine("0x{0:X}", readbuffer[0]);
                        if (XSVF_ERROR(readbuffer[0]) != "")
                        {
                            Console.WriteLine("Failed - {0}", XSVF_ERROR(readbuffer[0]));
                        }
                        else Console.WriteLine("Failed");
                    }
                    else Console.WriteLine("Done");
                }
                UpdateProgres(100);
                DeInitDemoN();
            }
            else
            {
                Console.WriteLine("Device not found.");
            }
        }

        public void Update_DemoN(string filename)
        {
            if (DemonDetected)
            {
                long fsize = 0;
                byte[] file = Oper.openfile(filename, ref fsize, 0);
                if (fsize < 0x20) { Console.WriteLine("Wrong File"); return; }

                byte[] Archive_Header = Oper.returnportion(file, 0, 16);
                byte[] Firmware_Header = Oper.returnportion(file, 16, 16);

                if (Archive_Header[0] != 0x46 || Archive_Header[1] != 0x57 || Archive_Header[2] != 0x41 || Archive_Header[3] != 0x00)
                {
                    Console.WriteLine("Bad File Format"); return;
                }

                byte[] vendor_id = Oper.returnportion(Firmware_Header, 0, 2);
                byte[] product_id = Oper.returnportion(Firmware_Header, 2, 2);
                byte[] fw_version = Oper.returnportion(Firmware_Header, 4, 2);
                byte[] fw_size = Oper.returnportion(Firmware_Header, 6, 4);

                if (vendor_id[0] != 0x11 || vendor_id[1] != 0xD4)
                {
                    Console.WriteLine("Firmware is from a different vendor!");
                    return;
                }

                if (!InitDemoN(DemonPathName))
                {
                    Console.WriteLine("Failed to grab handle");
                    return;
                }
                byte[] demid = demon_get_id();
                if (variables.debugme) Console.WriteLine("{0} - {1}", Oper.ByteArrayToString(demid), Oper.ByteArrayToString(product_id));
                if (product_id[0] != 0x00 && product_id[1] != 0x00)
                {
                    if (demid[0] != product_id[1] || demid[1] != product_id[0])
                    {
                        Console.WriteLine("Firmware is for a different product!");
                        DeInitDemoN();
                        return;
                    }
                }

                string deviceid = "";
                switch (Oper.ByteArrayToString(product_id))
                {
                    case "0000":
                        switch (Oper.ByteArrayToString(demid))
                        {
                            case "0000":
                                deviceid = "DemoN Phat";
                                break;
                            case "0100":
                                deviceid = "DemoN Slim";
                                break;
                            default:
                                deviceid = "DemoN Phat/Slim";
                                break;
                        }
                        break;
                    case "0001":
                        deviceid = "DemoN Rev. A";
                        break;
                    default:
                        DeInitDemoN();
                        return;
                }
                int fwsize = Convert.ToInt32(Oper.ByteArrayToString(fw_size), 16);
                if (fsize != (0x20 + fwsize + 256)) { Console.WriteLine("Wrong File"); DeInitDemoN(); return; }

                Console.WriteLine("\n***** Firmware Information *****");
                Console.WriteLine("Manufacturer:\tTeam Xecuter");
                Console.WriteLine("Device:\t\t{0}", deviceid);
                Console.WriteLine("Version:\t\t{0}.{1}", fw_version[0], fw_version[1]);
                Console.WriteLine("Size:\t\t{0} bytes", fwsize);
                Console.WriteLine("");
                byte[] data = Oper.returnportion(file, 0x20, fwsize);

                if (verify_fw(Oper.returnportion(file, 0, fwsize + 0x20), Oper.returnportion(file, fwsize + 0x20, 256)))
                {
                    update_fw(data);
                }
                else Console.WriteLine("Failed to verify fw integrity");
                DeInitDemoN();
            }
            else
            {
                if (variables.debugme) Console.WriteLine("Device not found.");
            }
        }
        public byte[] r_DemoN()
        {
            if (DemonDetected)
            {
                if (!InitDemoN(DemonPathName))
                {
                    Console.WriteLine("Failed to grab handle");
                    return null;
                }
                byte[] d = read_fw();

                DeInitDemoN();
                return d;
            }
            else
            {
                if (variables.debugme) Console.WriteLine("Device not found.");
            }
            return null;
        }
        private byte[] read_fw()
        {
            if (demon_get_mode()[0] == (byte)Demon_Modes.FIRMWARE)
            {
                Console.WriteLine("Entering bootloader mode.");
                demon_runBootloader();
                DeInitDemoN();
                DemonDetected = false;
                for (int i = 0; i < 50; i++)
                {
                    //if (FindDemon()) break;
                    if (DemonDetected) break;
                    Thread.Sleep(100);
                }
                Thread.Sleep(600);
                if (!FindDemon()) return null;
                Thread.Sleep(100);
                if (!InitDemoN(DemonPathName))
                {
                    Console.WriteLine("Failed to grab handle");
                    return null;
                }
            }
            Console.WriteLine("Programming.");
            //demon_beginFirmwareUpdate();
            byte[] page = new byte[1];
            byte[] data = new byte[256 * 256];
            for (int i = 0; i < 256; i++)
            {
                UpdateProgres((100 * i) / 223);
                page[0] = (byte)i;
                byte[] buffer = demon_readIntFlashPage(page);
                Buffer.BlockCopy(buffer, 0, data, i * 256, 256);
                Thread.Sleep(5);
            }
            //demon_endFirmwareUpdate();
            Console.WriteLine("Leaving bootloader mode.");
            demon_runFirmware();
            Console.WriteLine("Programming completed successfully!");
            return data;
        }
        private void update_fw(byte[] data)
        {
            if (demon_get_mode()[0] == (byte)Demon_Modes.FIRMWARE)
            {
                Console.WriteLine("Entering bootloader mode.");
                demon_runBootloader();
                DeInitDemoN();
                DemonDetected = false;
                for (int i = 0; i < 50; i++)
                {
                    //if (FindDemon()) break;
                    if (DemonDetected) break;
                    Thread.Sleep(100);
                }
                Thread.Sleep(600);
                if (!FindDemon()) return;
                Thread.Sleep(100);
                if (!InitDemoN(DemonPathName))
                {
                    Console.WriteLine("Failed to grab handle");
                    return;
                }
            }
            Console.WriteLine("Programming.");
            demon_beginFirmwareUpdate();
            byte[] page = new byte[1];
            byte[] buffer = new byte[256];
            for (int i = 0; i < 224; i++)
            {
                UpdateProgres((100 * i) / 223);
                page[0] = (byte)i;
                demon_eraseIntFlashPage(page);
                buffer = Oper.returnportion(data, i * 256, 256);
                demon_programIntFlashPage(page, buffer);
                Thread.Sleep(5);
            }
            demon_endFirmwareUpdate();
            Console.WriteLine("Leaving bootloader mode.");
            demon_runFirmware();
            Console.WriteLine("Programming completed successfully!");
        }
        private bool verify_fw(byte[] signedData, byte[] signature)
        {
            bool success = false;
            RSACryptoServiceProvider rsaCSP = DecodePEMKey(Encoding.ASCII.GetString(getpublickey()).Trim());
            if (rsaCSP == null) return false;
            success = rsaCSP.VerifyData(signedData, CryptoConfig.MapNameToOID("SHA256"), signature);
            return success;
        }

        public void Read_Serial_Port()
        {
            if (DemonDetected)
            {
                if (!InitDemoN(DemonPathName))
                {
                    Console.WriteLine("Failed to grab handle");
                    return;
                }
                byte[] buffer = new byte[0x21000];
                UInt32 length = 0;
                Console.WriteLine("Press ESC to exit");
                while (!variables.escapeloop)
                {
                    try
                    {
                        length = 0;
                        buffer = demon_read_Serial(ref length);
                        if (variables.debugme) Console.WriteLine("{0:X} - {1:X}", buffer.Length, length);
                        if (length != 0 && length != 2) Console.WriteLine(Encoding.ASCII.GetString(buffer));
                        Thread.Sleep(500);
                    }
                    catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
                }
                DeInitDemoN();
                Console.WriteLine("Done");
            }
            else
            {
                if (variables.debugme) Console.WriteLine("Device not found.");
            }
        }

        public void write_fusion(string filename, int startblock = 0, int length = 0)
        {
            if (DemonDetected)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Demon_Switch tmpflash = flash;
                get_external_flash(true);
                if (flash != tmpflash)
                {
                    if (DialogResult.Cancel == MessageBox.Show(flash + " Nand is selected, Proceed?", "Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                    {
                        Console.WriteLine("Cancelled");
                        Console.WriteLine("");
                        return;
                    };
                }
                get_external_flash_id();
                get_id();
                get_firmware();
                List<int> Invalid_Blocks = new List<int>();
                if (!get_Invalid_Blocks(ref Invalid_Blocks)) return;
                if (!setNand()) return;
                string hw = "";
                if (devid[0] == 0x01 && devid[1] == 0x00) hw = "Slim";
                else if (devid[0] == 0x00 && devid[1] == 0x00) hw = "Phat";
                if (hw == "") Console.WriteLine("Hardware   : Demon ({0:X}.{1:X})", ((devid[1]) & 0xFF), (devid[0] & 0xFF));
                else Console.WriteLine("Hardware   : Demon {0}", hw);
                Console.WriteLine("Firmware   : {0}.{1}", ((fw[1]) & 0xFF), (fw[0] & 0xFF));
                Console.WriteLine("Flash ID   : 0x{0:4X} {1} ({2} - {3} block)", Oper.ByteArrayToString(flashid), getManuString(), getNameString(), Nands.Fdevi[devi].Bigblock ? "Big" : "Small");
                Console.WriteLine("Flash Size : 0x{0:X} blocks of 0x{1:X} bytes", getNumBlocks(), getBlockSize());
                if (Invalid_Blocks != null)
                {
                    Console.Write("Invalid Blocks : ");
                    foreach (int n in Invalid_Blocks)
                    {
                        Console.Write("0x{0:X} ", n);
                    }
                    Console.WriteLine("");
                }


                if (!InitDemoN(DemonPathName))
                {
                    Console.WriteLine("Failed to grab handle");
                    return;
                }
                acquire_flash();
                int blsize = getBlockSize();
                if (length == 0) length = (int)getNumBlocks();
                bool isBigBlock = Nands.Fdevi[devi].Bigblock;
                if (variables.debugme) Console.WriteLine("filename {0}\n Startblock {1:X} - Length {2:X}", filename, startblock, length);

                write_fusion_DemoN(filename, Invalid_Blocks, startblock, length, blsize, isBigBlock);

                release_flash();
                DeInitDemoN();
                stopwatch.Stop();
                Console.WriteLine("Write Successful! Time Elapsed: {0}:{1:D2}", stopwatch.Elapsed.Minutes + (stopwatch.Elapsed.Hours * 60), stopwatch.Elapsed.Seconds);
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Device Not Found");
            }
            MainForm._waitmb.Reset();
            MainForm._waitmb.Set();

            try
            {
                SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                if (variables.soundsuccess != "") success.SoundLocation = variables.soundsuccess;
                success.Play();
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); };
        }
        public void write_fusion_DemoN(string filename, List<int> invalid_blocks, int startblock, int length, int blocklength, bool bigblock)
        {
            byte[] databuffer = new byte[1];
            byte[] blockbuffer = new byte[2];
            byte[] writeBuffer = new byte[blocklength];
            BinaryReader rw = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));

            long filesize;
            FileInfo fl = new FileInfo(filename);
            filesize = fl.Length / blocklength;
            if (length == 0) length = (int)filesize;
            if (startblock + length > filesize)
            {
                length = (int)filesize - startblock;
            }
            if (length <= 0) length = 0;
            if (variables.debugme) Console.WriteLine("filename {0}\n Startblock {1:X} - Length {2:X}", filename, startblock, length);
            Console.WriteLine("Writing Nand");
            int i = startblock;
            while (i < (length + startblock))
            {
                UpdateBloc(i.ToString("X"));
                UpdateProgres((100 * i) / (length + startblock));
                blockbuffer[0] = (byte)(i & 0xFF);
                blockbuffer[1] = (byte)((i & 0xFF00) / 255);

                writeBuffer = rw.ReadBytes(blocklength);
                if (Nands.Fdevi[devi].Bigblock && convert)
                {
                    Nand.Nand.sparedatatoraw(ref writeBuffer);
                }

                databuffer[0] = (byte)Demon_Commands.COMMAND_ERASE_EXT_FLASH_BLOCK;
                if (!device_bulk_write(Demon, databuffer, Convert.ToUInt32(databuffer.Length)))
                {
                    if (variables.debugme) Console.WriteLine("Failed to erase");
                }
                if (!device_bulk_write(Demon, blockbuffer, Convert.ToUInt32(blockbuffer.Length)))
                {
                    if (variables.debugme) Console.WriteLine("Failed to send eblocknumber");
                }

                databuffer[0] = (byte)Demon_Commands.COMMAND_PROGRAM_EXT_FLASH_BLOCK;
                if (!device_bulk_write(Demon, databuffer, Convert.ToUInt32(databuffer.Length)))
                {
                    if (variables.debugme) Console.WriteLine("Failed to program");
                }
                if (!device_bulk_write(Demon, blockbuffer, Convert.ToUInt32(blockbuffer.Length)))
                {
                    if (variables.debugme) Console.WriteLine("Failed to send blocknumber");
                }
                if (!device_bulk_write(Demon, writeBuffer, Convert.ToUInt32(writeBuffer.Length)))
                {
                    if (variables.debugme) Console.WriteLine("Failed to send block");
                }


                i++;
            }
            if (invalid_blocks != null)
            {
                Console.WriteLine("Starting remapping process");
                int reserveblockpos;
                if (Nands.Fdevi[devi].Bigblock)
                {
                    reserveblockpos = 0x1FF;
                }
                else
                {
                    reserveblockpos = 0x3FF;
                }
                int number = 0;
                foreach (int block in invalid_blocks)
                {
                    rw.BaseStream.Seek(blocklength * block, SeekOrigin.Begin);
                    writeBuffer = rw.ReadBytes(blocklength);
                    if (Nands.Fdevi[devi].Bigblock && convert)
                    {
                        Nand.Nand.sparedatatoraw(ref writeBuffer);
                    }

                    blockbuffer[0] = ((byte)((reserveblockpos - number) & 0xFF));
                    blockbuffer[1] = (byte)(((reserveblockpos - number) & 0xFF00) / 255);

                    Console.WriteLine("Remapping Block 0x{0:X} @ 0x{1:X}", block, reserveblockpos - number);

                    if (!device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_ERASE_EXT_FLASH_BLOCK))
                    {
                        if (variables.debugme) Console.WriteLine("Failed to erase");
                    }
                    if (!device_bulk_write(Demon, blockbuffer, Convert.ToUInt32(blockbuffer.Length)))
                    {
                        if (variables.debugme) Console.WriteLine("Failed to send eblocknumber");
                    }

                    if (!device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_PROGRAM_EXT_FLASH_BLOCK))
                    {
                        if (variables.debugme) Console.WriteLine("Failed to program");
                    }
                    if (!device_bulk_write(Demon, blockbuffer, Convert.ToUInt32(blockbuffer.Length)))
                    {
                        if (variables.debugme) Console.WriteLine("Failed to send blocknumber");
                    }
                    if (!device_bulk_write(Demon, writeBuffer, Convert.ToUInt32(writeBuffer.Length)))
                    {
                        if (variables.debugme) Console.WriteLine("Failed to send block");
                    }
                    number++;
                }
            }
            rw.Close();
            UpdateBloc("");
            writeBuffer = null;
        }

        private string getNameString()
        {
            return Nands.Fdevi[devi].Name;
        }
        private string getManuString()
        {
            return Nands.Fman[manu].Name;
        }
        private long getNumBlocks()
        {
            long usersize = Nands.Fdevi[devi].Chipsize * 1024 * 1024;
            if (variables.debugme) Console.WriteLine("getnumblocks - UserSize 0x{0:X}", usersize);
            long userblock = (Nands.Fdevi[devi].Pagesize * Nands.Fdevi[devi].PagePB);
            if (variables.debugme) Console.WriteLine("getnumblocks - UserBlock 0x{0:X}", userblock);
            return usersize / userblock;
        }
        private int getBlockSize()
        {
            return (int)((Nands.Fdevi[devi].Pagesize + Nands.Fdevi[devi].Sparesize) * Nands.Fdevi[devi].PagePB);
        }
        private int getUserBlockSize()
        {
            return (int)(Nands.Fdevi[devi].Pagesize * Nands.Fdevi[devi].PagePB);
        }
        private int getNandFileSize()
        {
            int usersize = (int)Nands.Fdevi[devi].Chipsize * 1024 * 1024;
            int userblock = (int)(Nands.Fdevi[devi].Pagesize * Nands.Fdevi[devi].PagePB);
            int blocksize = (int)((Nands.Fdevi[devi].Pagesize + Nands.Fdevi[devi].Sparesize) * Nands.Fdevi[devi].PagePB);
            int blocks = usersize / userblock;
            return blocks * blocksize;
        }
        private bool setNand()
        {
            int i;
            manu = -1;
            devi = -1;
            if (variables.debugme) Console.WriteLine("0x{0:X}", Oper.ByteArrayToString(flashid));
            for (i = 0; manu == -1; i++)
            {
                if (Nands.Fman[i].ID == flashid[1])
                {
                    //printf("man %d:%02x:%02x\n", i, man[i].id, bMan);
                    manu = i;
                }
                else if (Nands.Fman[i].ID == 0) // not found
                {
                    Console.WriteLine("manufacturer for code {0:X2} not found\n", flashid[1]);
                    manu = -1;
                    devi = -1;
                    return false;
                }
            }
            for (i = 0; devi == -1; i++)
            {
                if (Nands.Fdevi[i].ID == flashid[0])
                {
                    devi = i;
                    //printf("dev %d:%02x:%02x\n", i, fdev[i].id, bDev);
                }
                else if (Nands.Fdevi[i].ID == 0) // not found
                {
                    Console.WriteLine("device for code {0:X2} not found\n", flashid[0]);
                    devi = -1;
                    manu = -1;
                    return false;
                }
            }
            return true;
        }

        private bool acquire_flash()
        {
            bool success = false;

            success = device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_ASSERT_SB_RESET);
            if (!success && variables.debugme) Console.WriteLine("Failed to assert SB");

            success = device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_ACQUIRE_EXT_FLASH);
            if (!success && variables.debugme) Console.WriteLine("Failed to acquire flash");
            return success;
        }
        private bool release_flash()
        {
            bool success = false;

            success = device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_RELEASE_EXT_FLASH);
            if (!success && variables.debugme) Console.WriteLine("Failed to release flash");

            success = device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_DEASSERT_SB_RESET);
            if (!success && variables.debugme) Console.WriteLine("Failed to deassert SB");
            return success;
        }

        private byte[] demon_read_Serial(ref UInt32 length)
        {
            byte[] buffer = new byte[0x21000];
            if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_READ_SERIAL_PORT))
            {
                length = device_bulk_read_once(Demon, ref buffer, 0x21000);
                if (length != 0)
                {
                    if (variables.debugme) Console.WriteLine(buffer.Length);
                    //if (variables.debugme) Console.WriteLine(Nand.ByteArrayToString(buffer));
                    return buffer;
                }
                else
                {
                    if (variables.debugme) Console.WriteLine("The attempt to read bulk data has failed.");
                }
            }
            return null;
        }
        private bool demon_write_Serial(byte[] data)
        {
            bool res = false;
            if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_WRITE_SERIAL_PORT))
            {
                byte[] length = new byte[4];
                length[0] = (byte)data.Length;
                length[1] = (byte)(((uint)data.Length >> 8) & 0xFF);
                length[2] = (byte)(((uint)data.Length >> 16) & 0xFF);
                length[3] = (byte)(((uint)data.Length >> 24) & 0xFF);
                res = device_bulk_write(Demon, length, (uint)length.Length);
                res = device_bulk_write(Demon, data, (uint)data.Length);
                if (!res)
                {
                    if (variables.debugme) Console.WriteLine("The attempt to write bulk data has failed.");
                }
            }
            return res;
        }
        private byte[] demon_get_mode()
        {
            byte[] readbuffer = new byte[1];
            if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_MODE))
            {
                if (device_bulk_read(Demon, ref readbuffer, 1))
                {
                    if (variables.debugme) Console.WriteLine(readbuffer[0]);
                    updateMod((Demon_Modes)readbuffer[0]);
                    return readbuffer;
                }
                else
                {
                    if (variables.debugme) Console.WriteLine("The attempt to read bulk data has failed.");
                }
            }
            else
            {
                if (variables.debugme) Console.WriteLine("Failed to get mode");
            }
            return null;
        }
        private void demon_runBootloader()
        {
            if (!device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_RUN_BOOTLOADER))
            {
                if (variables.debugme) Console.WriteLine("Failed to run Bootloader");
            }
        }
        private byte demon_get_external_flash()
        {
            byte[] readbuffer = new byte[1];
            if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_EXT_FLASH))
            {
                if (device_bulk_read(Demon, ref readbuffer, 1))
                {
                    updateFlas((Demon_Switch)readbuffer[0]);
                    return readbuffer[0];
                }
                else
                {
                    if (variables.debugme) Console.WriteLine("The attempt to read bulk data has failed.");
                }
            }
            else { if (variables.debugme) Console.WriteLine("Failed to get external flash"); }
            return readbuffer[0];
        }
        private byte[] demon_get_id()
        {
            byte[] readbuffer = new byte[2];
            if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_DEVICE_ID))
            {
                if (device_bulk_read(Demon, ref readbuffer, 2))
                {
                    return readbuffer;
                }
                else
                {
                    Console.WriteLine("The attempt to read bulk data has failed.");
                }
            }
            else Console.WriteLine("Bulk OUT transfer failed.");
            return readbuffer;
        }
        private byte[] demon_get_firmware()
        {
            byte[] readbuffer = new byte[2];
            if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_FIRMWARE_VERSION))
            {
                if (device_bulk_read(Demon, ref readbuffer, 2))
                {
                    UpdateVer(readbuffer[1].ToString() + "." + readbuffer[0].ToString());
                    return readbuffer;
                }
                else
                {
                    if (variables.debugme) Console.WriteLine("The attempt to read bulk data has failed.");
                }
            }
            else { if (variables.debugme) Console.WriteLine("Bulk OUT transfer failed."); }
            return readbuffer;
        }
        private byte[] demon_get_external_flash_id()
        {
            byte[] readbuffer = new byte[2];
            acquire_flash();
            if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_EXT_FLASH_ID))
            {
                if (device_bulk_read(Demon, ref readbuffer, 2))
                {
                    return readbuffer;
                }
                else
                {
                    Console.WriteLine("The attempt to read bulk data has failed.");
                }
            }
            else Console.WriteLine("Bulk OUT transfer failed.");
            release_flash();
            return readbuffer;
        }
        private byte[] demon_getBootloaderVersion()
        {
            byte[] readbuffer = new byte[2];
            device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_BOOTLOADER_VERSION);
            device_bulk_read(Demon, ref readbuffer, 2);
            UpdateVer(readbuffer[1].ToString() + "." + readbuffer[0].ToString());
            if (variables.debugme) Console.WriteLine(Oper.ByteArrayToString(readbuffer));
            return readbuffer;
        }
        private bool demon_get_Invalid_Blocks(ref List<int> Invalid_Blocks)
        {
            acquire_flash();
            if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_INVALID_BLOCKS))
            {
                byte[] number = new byte[2];
                device_bulk_read_once(Demon, ref number, 2);
                byte[] blocknumber = new byte[2], block = new byte[2];
                Buffer.BlockCopy(number, 0, blocknumber, 0, 2);
                Array.Reverse(blocknumber);
                if (variables.debugme)
                {
                    Console.WriteLine("Number {0}", Oper.ByteArrayToString(number));
                    Console.WriteLine("Number {0:X}", Convert.ToInt32(Oper.ByteArrayToString(number), 16));
                    Console.WriteLine("BlockNumber {0}", Oper.ByteArrayToString(blocknumber));
                    Console.WriteLine("BlockNumber {0:X}", Convert.ToInt32(Oper.ByteArrayToString(blocknumber), 16));
                }
                int numb = Convert.ToInt32(Oper.ByteArrayToString(blocknumber), 16);
                if (numb > 64)
                {
                    release_flash();
                    return true;
                }
                else if (numb < 1)
                {
                    release_flash();
                    return true;
                }

                for (int i = 0; i < numb; i++)
                {
                    number = new byte[2];
                    device_bulk_read_once(Demon, ref number, 2);
                    block = new byte[2];
                    Buffer.BlockCopy(number, 0, block, 0, 2);
                    Array.Reverse(block);
                    if (variables.debugme)
                    {
                        Console.WriteLine("Block {0}", Oper.ByteArrayToString(block));
                        Console.WriteLine("Block {0:X}", Convert.ToInt32(Oper.ByteArrayToString(block), 16));
                    }
                    if (Convert.ToInt32(Oper.ByteArrayToString(blocknumber), 16) == 0x00)
                    {
                        if (variables.debugme) Console.WriteLine("Break");
                        break;
                    }
                    Invalid_Blocks.Add(Convert.ToInt32(Oper.ByteArrayToString(block), 16));
                }
                if (variables.debugme) Console.WriteLine("Releasing Flash");
                release_flash();
                return true;
            }
            else { if (variables.debugme) Console.WriteLine("Bulk OUT transfer failed."); }
            release_flash();
            return false;
        }
        private void demon_beginFirmwareUpdate()
        {
            if (!device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_BEGIN_FIRMWARE_UPDATE))
            {
                if (variables.debugme) Console.WriteLine("Failed to begin fw update");
            }
        }
        private void demon_endFirmwareUpdate()
        {
            if (!device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_END_FIRMWARE_UPDATE))
            {
                if (variables.debugme) Console.WriteLine("Failed to end fw update");
            }
        }
        private void demon_runFirmware()
        {
            if (!device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_RUN_FIRMWARE))
            {
                if (variables.debugme) Console.WriteLine("Failed to run fw");
            }
        }

        private byte[] demon_readIntFlashPage(byte[] page)
        {
            byte[] buffer = new byte[256];
            if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_READ_INT_FLASH_PAGE))
            {
                if (device_bulk_write(Demon, page, Convert.ToUInt32(page.Length)))
                {
                    if (!device_bulk_read(Demon, ref buffer, 256))
                    {
                        if (variables.debugme) Console.WriteLine("Failed to get block data");
                    }
                }
                else
                {
                    if (variables.debugme) Console.WriteLine("Failed to send program block");
                }
            }
            else
            {
                if (variables.debugme) Console.WriteLine("Failed to send program command");
            }
            return buffer;
        }
        private void demon_eraseIntFlashPage(byte[] page)
        {
            if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_ERASE_INT_FLASH_PAGE))
            {
                if (!device_bulk_write(Demon, page, Convert.ToUInt32(page.Length)))
                {
                    if (variables.debugme) Console.WriteLine("Failed to send erase block");
                }
            }
            else
            {
                if (variables.debugme) Console.WriteLine("Failed to send erase command");
            }
        }
        private void demon_programIntFlashPage(byte[] page, byte[] buffer)
        {
            if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_PROGRAM_INT_FLASH_PAGE))
            {
                if (device_bulk_write(Demon, page, Convert.ToUInt32(page.Length)))
                {
                    if (!device_bulk_write(Demon, buffer, 256))
                    {
                        if (variables.debugme) Console.WriteLine("Failed to send block data");
                    }
                }
                else
                {
                    if (variables.debugme) Console.WriteLine("Failed to send program block");
                }
            }
            else
            {
                if (variables.debugme) Console.WriteLine("Failed to send program command");
            }
        }


        public void toggle()
        {
            if (DemonDetected)
            {
                if (!InitDemoN(DemonPathName)) return;
                if (variables.debugme) Console.WriteLine("Init");
                byte[] readbuffer = new byte[1];
                if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_EXT_FLASH))
                {
                    if (device_bulk_read(Demon, ref readbuffer, 1))
                    {
                        Console.Write("Current: ");
                        if (readbuffer[0] == (byte)Demon_Switch.DEMON)
                        {
                            Console.WriteLine("DemoN");
                            readbuffer[0] = (byte)Demon_Switch.XBOX360;
                            Console.WriteLine("Switching to XBOX");
                        }
                        else if (readbuffer[0] == (byte)Demon_Switch.XBOX360)
                        {
                            Console.WriteLine("XBOX");
                            readbuffer[0] = (byte)Demon_Switch.DEMON;
                            Console.WriteLine("Switching to DemoN");
                        }
                        //Console.WriteLine("Selected {0}", readbuffer[0]);
                        device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_ASSERT_SB_RESET);
                        if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_SET_EXT_FLASH))
                        {
                            device_bulk_write(Demon, readbuffer, Convert.ToUInt32(readbuffer.Length));
                            device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_DEASSERT_SB_RESET);
                        }
                        else Console.WriteLine("Failed");

                        updateFlas(((Demon_Switch)readbuffer[0]));
                    }
                    else
                    {
                        Console.WriteLine("The attempt to read bulk data has failed.");
                    }
                }
                else Console.WriteLine("Bulk OUT transfer failed.");
                DeInitDemoN();
            }
            else
            {
                Console.WriteLine("Device not found.");
            }
        }
        public void toggle(Demon_Switch switc)
        {
            if (DemonDetected)
            {
                if (!InitDemoN(DemonPathName)) return;
                byte[] readbuffer = new byte[1];
                if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_EXT_FLASH))
                {
                    if (device_bulk_read(Demon, ref readbuffer, 1))
                    {
                        bool change = false;
                        Console.Write("Current: ");
                        if (readbuffer[0] == (byte)Demon_Switch.DEMON)
                        {
                            Console.WriteLine("DemoN");
                            if (readbuffer[0] != (byte)switc)
                            {
                                readbuffer[0] = (byte)Demon_Switch.XBOX360;
                                Console.WriteLine("Switching to XBOX");
                                change = true;
                            }
                        }
                        else if (readbuffer[0] == (byte)Demon_Switch.XBOX360)
                        {
                            Console.WriteLine("XBOX");
                            if (readbuffer[0] != (byte)switc)
                            {
                                readbuffer[0] = (byte)Demon_Switch.DEMON;
                                Console.WriteLine("Switching to DemoN");
                                change = true;
                            }
                        }
                        //Console.WriteLine("Selected {0}", readbuffer[0]);
                        if (change)
                        {
                            device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_ASSERT_SB_RESET);
                            if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_SET_EXT_FLASH))
                            {
                                device_bulk_write(Demon, readbuffer, Convert.ToUInt32(readbuffer.Length));
                            }
                            else Console.WriteLine("Failed");
                            device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_DEASSERT_SB_RESET);
                        }

                        updateFlas((Demon_Switch)readbuffer[0]);

                    }
                    else
                    {
                        Console.WriteLine("The attempt to read bulk data has failed.");
                    }
                }
                else Console.WriteLine("Bulk OUT transfer failed.");
                DeInitDemoN();
            }
            else
            {
                Console.WriteLine("Device not found.");
            }
        }
        public void Power_On()
        {
            if (DemonDetected)
            {
                if (!InitDemoN(DemonPathName)) return;
                if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_POWER_ON))
                {
                    Console.WriteLine("Power On");
                }
                else { if (variables.debugme) Console.WriteLine("Bulk OUT transfer failed."); }
                DeInitDemoN();
            }
            else
            {
                if (variables.debugme) Console.WriteLine("Device not found.");
            }
        }
        public void Power_Off()
        {
            if (DemonDetected)
            {
                if (!InitDemoN(DemonPathName)) return;
                if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_POWER_OFF))
                {
                    Console.WriteLine("Power off");
                }
                else { if (variables.debugme) Console.WriteLine("Bulk OUT transfer failed."); }
                DeInitDemoN();
            }
            else
            {
                if (variables.debugme) Console.WriteLine("Device not found.");
            }
        }
        public bool get_Invalid_Blocks(ref List<int> Invalid_Blocks)
        {
            if (DemonDetected)
            {
                if (!InitDemoN(DemonPathName)) return false;
                acquire_flash();
                if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_INVALID_BLOCKS))
                {
                    byte[] number = new byte[2];
                    device_bulk_read_once(Demon, ref number, 2);
                    byte[] blocknumber = new byte[2], block = new byte[2];
                    Buffer.BlockCopy(number, 0, blocknumber, 0, 2);
                    Array.Reverse(blocknumber);
                    if (variables.debugme)
                    {
                        Console.WriteLine("Number {0}", Oper.ByteArrayToString(number));
                        Console.WriteLine("Number {0:X}", Convert.ToInt32(Oper.ByteArrayToString(number), 16));
                        Console.WriteLine("BlockNumber {0}", Oper.ByteArrayToString(blocknumber));
                        Console.WriteLine("BlockNumber {0:X}", Convert.ToInt32(Oper.ByteArrayToString(blocknumber), 16));
                    }
                    int numb = Convert.ToInt32(Oper.ByteArrayToString(blocknumber), 16);
                    if (numb > 64)
                    {
                        release_flash();
                        DeInitDemoN();
                        return true;
                    }
                    else if (numb < 1)
                    {
                        release_flash();
                        DeInitDemoN();
                        return true;
                    }

                    for (int i = 0; i < numb; i++)
                    {
                        number = new byte[2];
                        device_bulk_read_once(Demon, ref number, 2);
                        block = new byte[2];
                        Buffer.BlockCopy(number, 0, block, 0, 2);
                        Array.Reverse(block);
                        if (variables.debugme)
                        {
                            Console.WriteLine("Block {0}", Oper.ByteArrayToString(block));
                            Console.WriteLine("Block {0:X}", Convert.ToInt32(Oper.ByteArrayToString(block), 16));
                        }
                        if (Convert.ToInt32(Oper.ByteArrayToString(blocknumber), 16) == 0x00)
                        {
                            if (variables.debugme) Console.WriteLine("Break");
                            break;
                        }
                        Invalid_Blocks.Add(Convert.ToInt32(Oper.ByteArrayToString(block), 16));
                    }
                    if (variables.debugme) Console.WriteLine("Releasing Flash");
                    release_flash();
                    DeInitDemoN();
                    return true;
                }
                else { if (variables.debugme) Console.WriteLine("Bulk OUT transfer failed."); }
                release_flash();
                DeInitDemoN();
            }
            else
            {
                if (variables.debugme) Console.WriteLine("Device not found.");
                return false;
            }
            return false;
        }
        public int get_external_flash(bool print)
        {
            if (DemonDetected)
            {
                if (!InitDemoN(DemonPathName)) return -1;
                byte[] readbuffer = new byte[1];
                if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_EXT_FLASH))
                {
                    if (device_bulk_read(Demon, ref readbuffer, 1))
                    {
                        //if (variables.debugme) Console.WriteLine(Nand.ByteArrayToString(readbuffer));
                        //if (variables.debugme) Console.WriteLine("External Flash: {0}", readbuffer[0]);
                        flash = (Demon_Switch)readbuffer[0];

                        nand = readbuffer;
                        updateFlas(flash);
                    }
                    else
                    {
                        if (variables.debugme) Console.WriteLine("The attempt to read bulk data has failed.");
                    }
                }
                else { if (variables.debugme) Console.WriteLine("Bulk OUT transfer failed."); }
                DeInitDemoN();
                return 0;
            }
            else
            {
                if (variables.debugme) Console.WriteLine("Device not found.");
            }
            return -1;
        }
        public void get_mode()
        {
            if (DemonDetected)
            {
                if (!InitDemoN(DemonPathName)) return;
                byte[] readbuffer = new byte[1];
                if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_MODE))
                {
                    if (device_bulk_read(Demon, ref readbuffer, 1))
                    {
                        if (variables.debugme) Console.WriteLine(Oper.ByteArrayToString(readbuffer));
                        if (variables.debugme) Console.WriteLine("Mode: {0}", readbuffer[0]);
                        if (readbuffer[0] == 0x01)
                        {
                            if (variables.debugme) Console.WriteLine("Normal");
                            mode = Demon_Modes.FIRMWARE;
                        }
                        else if (readbuffer[0] == 0x00)
                        {
                            if (variables.debugme) Console.WriteLine("Bootloader");
                            mode = Demon_Modes.BOOTLOADER;
                        }
                        updateMod(mode);
                    }
                    else
                    {
                        if (variables.debugme) Console.WriteLine("The attempt to read bulk data has failed.");
                    }
                }
                else { if (variables.debugme) Console.WriteLine("Bulk OUT transfer failed."); }
                DeInitDemoN();
            }
            else
            {
                if (variables.debugme) Console.WriteLine("Device not found.");
            }
        }
        public void getBootloaderVersion()
        {
            if (DemonDetected)
            {
                if (!InitDemoN(DemonPathName)) return;
                byte[] readbuffer = new byte[2];
                if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_BOOTLOADER_VERSION))
                {
                    if (device_bulk_read(Demon, ref readbuffer, 2))
                    {
                        if (variables.debugme) Console.WriteLine(Oper.ByteArrayToString(readbuffer));
                        if (variables.debugme) Console.WriteLine("BL Version: {0}.{1}", readbuffer[1], readbuffer[0]);
                        fw = readbuffer;
                        UpdateVer(readbuffer[1].ToString() + "." + readbuffer[0].ToString());
                    }
                    else
                    {
                        if (variables.debugme) Console.WriteLine("The attempt to read bulk data has failed.");
                    }
                }
                else { if (variables.debugme) Console.WriteLine("Bulk OUT transfer failed."); }
                DeInitDemoN();
            }
            else
            {
                if (variables.debugme) Console.WriteLine("Device not found.");
            }
        }
        public void get_firmware()
        {
            if (DemonDetected)
            {
                if (!InitDemoN(DemonPathName)) return;
                byte[] readbuffer = new byte[2];
                if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_FIRMWARE_VERSION))
                {
                    if (device_bulk_read(Demon, ref readbuffer, 2))
                    {
                        if (variables.debugme) Console.WriteLine(Oper.ByteArrayToString(readbuffer));
                        if (variables.debugme) Console.WriteLine("FW Version: {0}.{1}", readbuffer[1], readbuffer[0]);
                        fw = readbuffer;
                        UpdateVer(readbuffer[1].ToString() + "." + readbuffer[0].ToString());
                    }
                    else
                    {
                        if (variables.debugme) Console.WriteLine("The attempt to read bulk data has failed.");
                    }
                }
                else { if (variables.debugme) Console.WriteLine("Bulk OUT transfer failed."); }
                DeInitDemoN();
            }
            else
            {
                if (variables.debugme) Console.WriteLine("Device not found.");
            }
        }
        public void get_external_flash_id()
        {
            if (DemonDetected)
            {
                if (!InitDemoN(DemonPathName)) return;
                byte[] readbuffer = new byte[2];
                acquire_flash();
                if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_EXT_FLASH_ID))
                {
                    if (device_bulk_read(Demon, ref readbuffer, 2))
                    {
                        if (variables.debugme) Console.WriteLine(Oper.ByteArrayToString(readbuffer));
                        flashid = readbuffer;
                    }
                    else
                    {
                        Console.WriteLine("The attempt to read bulk data has failed.");
                    }
                }
                else Console.WriteLine("Bulk OUT transfer failed.");
                release_flash();
                DeInitDemoN();
            }
            else
            {
                Console.WriteLine("Device not found.");
            }
        }
        public void get_protocol()
        {
            if (DemonDetected)
            {
                if (!InitDemoN(DemonPathName)) return;
                byte[] readbuffer = new byte[2];
                if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_PROTOCOL_VERSION))
                {
                    // Console.WriteLine("Data sent via bulk transfer.");
                    if (device_bulk_read(Demon, ref readbuffer, 2))
                    {
                        // Console.WriteLine("Data received via bulk transfer:");
                        Console.WriteLine(Oper.ByteArrayToString(readbuffer));
                        Console.WriteLine("Protocol: {0}", readbuffer[0]);
                    }
                    else
                    {
                        Console.WriteLine("The attempt to read bulk data has failed.");
                    }
                }
                else Console.WriteLine("Bulk OUT transfer failed.");
                DeInitDemoN();
            }
            else
            {
                Console.WriteLine("Device not found.");
            }
        }
        public void get_id()
        {
            if (DemonDetected)
            {
                if (!InitDemoN(DemonPathName)) return;
                // change the 3 to a constant
                byte[] readbuffer = new byte[2];
                if (device_bulk_write(Demon, (byte)Demon_Commands.COMMAND_GET_DEVICE_ID))
                {
                    //Console.WriteLine("Data sent via bulk transfer.");
                    if (device_bulk_read(Demon, ref readbuffer, 2))
                    {
                        //Console.WriteLine("Data received via bulk transfer:");
                        //Console.WriteLine(Nand.ByteArrayToString(readbuffer));
                        //Console.WriteLine("ID: {0}", readbuffer[0]);
                        devid = readbuffer;
                    }
                    else
                    {
                        Console.WriteLine("The attempt to read bulk data has failed.");
                    }
                }
                else Console.WriteLine("Bulk OUT transfer failed.");
                DeInitDemoN();
            }
            else
            {
                Console.WriteLine("Device not found.");
            }
        }

        private bool device_bulk_write(WinUsbDevice myDevice, byte[] databuffer, uint bytesToSend)
        {
            try
            {
                bool success;
                success = myDevice.SendViaBulkTransfer
                        (ref databuffer,
                        bytesToSend);

                return success;
            }
            catch (Exception ex)
            {
                if (variables.debugme) Console.WriteLine(ex.ToString());
                return false;
            }
        }
        private bool device_bulk_write(WinUsbDevice myDevice, byte databuffer)
        {
            try
            {
                byte[] db = new byte[1];
                db[0] = databuffer;
                bool success;
                success = myDevice.SendViaBulkTransfer
                        (ref db, 1);

                return success;
            }
            catch (Exception ex)
            {
                if (variables.debugme) Console.WriteLine(ex.ToString());
                return false;
            }
        }
        private bool device_bulk_read(WinUsbDevice myDevice, ref byte[] buffer, UInt32 bytestoread)
        {
            bool success = false;
            UInt32 bytesRead = 0;
            try
            {
                while (bytestoread > 0)
                {
                    myDevice.ReadViaBulkTransfer(myDevice.myDevInfo.bulkInPipe, bytestoread, ref buffer, ref bytesRead, ref success);
                    bytestoread -= bytesRead;
                }
                return success;
            }
            catch (Exception ex)
            {
                if (variables.debugme) Console.WriteLine(ex.ToString());
                return false;
            }
        }
        private uint device_bulk_read_once(WinUsbDevice myDevice, ref byte[] buffer, UInt32 bytestoread)
        {
            bool success = false;
            UInt32 bytesRead = 0;
            try
            {
                myDevice.ReadViaBulkTransfer(myDevice.myDevInfo.bulkInPipe, bytestoread, ref buffer, ref bytesRead, ref success);
                bytestoread -= bytesRead;
                return bytesRead;
            }
            catch (Exception ex)
            {
                if (variables.debugme) Console.WriteLine(ex.ToString());
                return bytesRead;
            }
        }


        /// <summary>
        /// RSA STUFF
        /// </summary>
        static bool verbose = false;
        const String pempubheader = "-----BEGIN PUBLIC KEY-----";
        const String pempubfooter = "-----END PUBLIC KEY-----";
        private static RSACryptoServiceProvider DecodePEMKey(String pemstr)
        {
            byte[] pempublickey;

            if (pemstr.StartsWith(pempubheader) && pemstr.EndsWith(pempubfooter))
            {
                if (verbose) Console.WriteLine("Trying to decode and parse a PEM public key ..");
                pempublickey = DecodeOpenSSLPublicKey(pemstr);
                if (pempublickey != null)
                {
                    if (verbose)
                        showBytes("\nRSA public key", pempublickey);
                    //PutFileBytes("rsapubkey.pem", pempublickey, pempublickey.Length) ;
                    RSACryptoServiceProvider rsa = DecodeX509PublicKey(pempublickey);
                    if (verbose) Console.WriteLine("\nCreated an RSACryptoServiceProvider instance\n");
                    String xmlpublickey = rsa.ToXmlString(false);
                    if (verbose) Console.WriteLine("\nXML RSA public key:  {0} bits\n{1}\n", rsa.KeySize, xmlpublickey);
                    return rsa;
                }
            }
            else
            {
                Console.WriteLine("Not a PEM public key or a PKCS #8");
                return null;
            }
            return null;
        }
        private static RSACryptoServiceProvider DecodeX509PublicKey(byte[] x509key)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];
            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            MemoryStream mem = new MemoryStream(x509key);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;

            try
            {

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                seq = binr.ReadBytes(15);		//read the Sequence OID
                if (!Oper.ByteArrayCompare(seq, SeqOID))	//make sure Sequence for OID is correct
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8103)	//data read as little endian order (actual data order for Bit String is 03 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8203)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x00)		//expect null byte next
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                byte lowbyte = 0x00;
                byte highbyte = 0x00;

                if (twobytes == 0x8102)	//data read as little endian order (actual data order for Integer is 02 81)
                    lowbyte = binr.ReadByte();	// read next bytes which is bytes in modulus
                else if (twobytes == 0x8202)
                {
                    highbyte = binr.ReadByte();	//advance 2 bytes
                    lowbyte = binr.ReadByte();
                }
                else
                    return null;
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                int modsize = BitConverter.ToInt32(modint, 0);

                byte firstbyte = binr.ReadByte();
                binr.BaseStream.Seek(-1, SeekOrigin.Current);

                if (firstbyte == 0x00)
                {	//if first byte (highest order) of modulus is zero, don't include it
                    binr.ReadByte();	//skip this null byte
                    modsize -= 1;	//reduce modulus buffer size by 1
                }

                byte[] modulus = binr.ReadBytes(modsize);	//read the modulus bytes

                if (binr.ReadByte() != 0x02)			//expect an Integer for the exponent data
                    return null;
                int expbytes = binr.ReadByte();		// should only need one byte for actual exponent data (for all useful values)
                byte[] exponent = binr.ReadBytes(expbytes);


                if (verbose) showBytes("\nExponent", exponent);
                if (verbose) showBytes("\nModulus", modulus);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAKeyInfo = new RSAParameters();
                RSAKeyInfo.Modulus = modulus;
                RSAKeyInfo.Exponent = exponent;
                RSA.ImportParameters(RSAKeyInfo);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }

            finally { binr.Close(); }

        }
        private static byte[] DecodeOpenSSLPublicKey(String instr)
        {
            const String pempubheader = "-----BEGIN PUBLIC KEY-----";
            const String pempubfooter = "-----END PUBLIC KEY-----";
            String pemstr = instr.Trim();
            byte[] binkey;
            if (!pemstr.StartsWith(pempubheader) || !pemstr.EndsWith(pempubfooter))
                return null;
            StringBuilder sb = new StringBuilder(pemstr);
            sb.Replace(pempubheader, "");  //remove headers/footers, if present
            sb.Replace(pempubfooter, "");

            String pubstr = sb.ToString().Trim();	//get string after removing leading/trailing whitespace

            try
            {
                binkey = Convert.FromBase64String(pubstr);
            }
            catch (System.FormatException)
            {		//if can't b64 decode, data is not valid
                return null;
            }
            return binkey;
        }
        private static void showBytes(String info, byte[] data)
        {
            Console.WriteLine("{0}  [{1} bytes]", info, data.Length);
            for (int i = 1; i <= data.Length; i++)
            {
                Console.Write("{0:X2}  ", data[i - 1]);
                if (i % 16 == 0)
                    Console.WriteLine();
            }
            Console.WriteLine("\n\n");
        }

        #region PublicKey

        private static byte[] getpublickey()
        {
            byte[] publickey =
        {
    0x2d, 0x2d, 0x2d, 0x2d, 0x2d, 0x42, 0x45, 0x47,
    0x49, 0x4e, 0x20, 0x50, 0x55, 0x42, 0x4c, 0x49,
    0x43, 0x20, 0x4b, 0x45, 0x59, 0x2d, 0x2d, 0x2d,
    0x2d, 0x2d, 0x0a, 0x4d, 0x49, 0x49, 0x42, 0x49,
    0x6a, 0x41, 0x4e, 0x42, 0x67, 0x6b, 0x71, 0x68,
    0x6b, 0x69, 0x47, 0x39, 0x77, 0x30, 0x42, 0x41,
    0x51, 0x45, 0x46, 0x41, 0x41, 0x4f, 0x43, 0x41,
    0x51, 0x38, 0x41, 0x4d, 0x49, 0x49, 0x42, 0x43,
    0x67, 0x4b, 0x43, 0x41, 0x51, 0x45, 0x41, 0x31,
    0x53, 0x63, 0x61, 0x72, 0x41, 0x45, 0x69, 0x4c,
    0x36, 0x66, 0x41, 0x4b, 0x76, 0x52, 0x77, 0x63,
    0x67, 0x6b, 0x6f, 0x0a, 0x66, 0x65, 0x78, 0x7a,
    0x70, 0x37, 0x7a, 0x62, 0x6b, 0x76, 0x6f, 0x37,
    0x49, 0x52, 0x37, 0x33, 0x49, 0x50, 0x34, 0x46,
    0x57, 0x43, 0x59, 0x58, 0x59, 0x54, 0x6a, 0x36,
    0x4a, 0x66, 0x6e, 0x43, 0x66, 0x4c, 0x71, 0x5a,
    0x69, 0x78, 0x47, 0x75, 0x38, 0x6c, 0x63, 0x49,
    0x39, 0x44, 0x6a, 0x5a, 0x46, 0x63, 0x57, 0x66,
    0x53, 0x45, 0x50, 0x6f, 0x42, 0x4d, 0x64, 0x2f,
    0x46, 0x30, 0x4c, 0x75, 0x0a, 0x78, 0x33, 0x30,
    0x73, 0x53, 0x61, 0x6a, 0x44, 0x58, 0x4f, 0x61,
    0x35, 0x6b, 0x75, 0x31, 0x4d, 0x73, 0x2b, 0x5a,
    0x32, 0x66, 0x41, 0x6f, 0x5a, 0x6a, 0x57, 0x5a,
    0x7a, 0x78, 0x6f, 0x63, 0x35, 0x56, 0x79, 0x4e,
    0x51, 0x6a, 0x49, 0x58, 0x62, 0x4b, 0x6e, 0x30,
    0x46, 0x37, 0x59, 0x34, 0x30, 0x33, 0x2f, 0x48,
    0x50, 0x71, 0x58, 0x31, 0x33, 0x37, 0x41, 0x73,
    0x37, 0x4e, 0x30, 0x6c, 0x46, 0x0a, 0x57, 0x79,
    0x71, 0x71, 0x78, 0x70, 0x58, 0x67, 0x30, 0x48,
    0x57, 0x2b, 0x7a, 0x4f, 0x4a, 0x6f, 0x47, 0x6a,
    0x6c, 0x46, 0x56, 0x59, 0x56, 0x38, 0x52, 0x50,
    0x42, 0x48, 0x45, 0x33, 0x64, 0x38, 0x4d, 0x79,
    0x30, 0x78, 0x69, 0x34, 0x53, 0x63, 0x32, 0x35,
    0x48, 0x33, 0x71, 0x58, 0x30, 0x78, 0x77, 0x68,
    0x4e, 0x6f, 0x43, 0x39, 0x35, 0x64, 0x50, 0x32,
    0x62, 0x6b, 0x45, 0x74, 0x70, 0x54, 0x0a, 0x72,
    0x43, 0x42, 0x64, 0x33, 0x4a, 0x68, 0x46, 0x65,
    0x45, 0x64, 0x6d, 0x6f, 0x68, 0x77, 0x70, 0x48,
    0x6a, 0x6d, 0x52, 0x72, 0x4c, 0x45, 0x4b, 0x67,
    0x46, 0x77, 0x65, 0x4c, 0x6f, 0x7a, 0x47, 0x68,
    0x76, 0x6e, 0x65, 0x52, 0x4b, 0x5a, 0x36, 0x76,
    0x6f, 0x6f, 0x6b, 0x6d, 0x74, 0x43, 0x41, 0x50,
    0x70, 0x58, 0x41, 0x35, 0x31, 0x35, 0x68, 0x77,
    0x42, 0x61, 0x75, 0x67, 0x63, 0x6c, 0x41, 0x0a,
    0x68, 0x68, 0x36, 0x49, 0x71, 0x68, 0x30, 0x61,
    0x32, 0x53, 0x6e, 0x4e, 0x66, 0x44, 0x72, 0x6a,
    0x48, 0x30, 0x2b, 0x50, 0x32, 0x4d, 0x4d, 0x57,
    0x2f, 0x65, 0x45, 0x63, 0x34, 0x6a, 0x49, 0x54,
    0x65, 0x6c, 0x6d, 0x46, 0x2f, 0x61, 0x6e, 0x56,
    0x56, 0x72, 0x59, 0x74, 0x44, 0x45, 0x6d, 0x4c,
    0x70, 0x53, 0x79, 0x2f, 0x52, 0x71, 0x39, 0x63,
    0x45, 0x45, 0x77, 0x6f, 0x75, 0x4a, 0x74, 0x35,
    0x0a, 0x76, 0x77, 0x49, 0x44, 0x41, 0x51, 0x41,
    0x42, 0x0a, 0x2d, 0x2d, 0x2d, 0x2d, 0x2d, 0x45,
    0x4e, 0x44, 0x20, 0x50, 0x55, 0x42, 0x4c, 0x49,
    0x43, 0x20, 0x4b, 0x45, 0x59, 0x2d, 0x2d, 0x2d,
    0x2d, 0x2d, 0x0a
        };
            return publickey;
        }

        #endregion
    }
}
