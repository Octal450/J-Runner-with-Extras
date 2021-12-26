using LibUsbDotNet;
using LibUsbDotNet.Main;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace JRunner
{
    internal class NandX
    {
        private static Dictionary<int, string> POST = new Dictionary<int, string>();
        private static List<string> versions = new List<string>(){
            "03000000",
            "01000000",
            "10000000"
        };

        //private static UsbDevice MyUsbDevice;
        private static UsbDeviceFinder NANDX = new UsbDeviceFinder(0xFFFF, 0x0004);
        private static UsbDeviceFinder JRP = new UsbDeviceFinder(0x11d4, 0x8338);
        private int timeout = 1000;
        private bool jrp = false;
        public static bool InUse = false;

        public delegate void updateProgress(int progress);
        public event updateProgress UpdateProgres;
        public delegate void updateBlock(string block);
        public event updateBlock UpdateBloc;

        enum Commands : byte
        {
            /// <summary>
            /// FlashDataRead 0x01
            /// </summary>
            FlashDataRead = 0x01,
            /// <summary>
            /// FlashDataWrite 0x02
            /// </summary>
            FlashDataWrite = 0x02,
            /// <summary>
            /// FlashDataInit 0x03
            /// </summary>
            FlashDataInit = 0x03,
            /// <summary>
            /// FlashDataDeInit 0x04
            /// </summary>
            FlashDataDeInit = 0x04,
            /// <summary>
            /// FlashDataStatus 0x05
            /// </summary>
            FlashDataStatus = 0x05,
            /// <summary>
            /// FlashDataErase 0x06
            /// </summary>
            FlashDataErase = 0x06,
            /// <summary>
            /// SPI 0x07
            /// </summary>
            SPI = 0x07,
            /// <summary>
            /// Version 0x08
            /// </summary>
            Version = 0x08,
            /// <summary>
            /// JTAG 0x09
            /// </summary>
            JTAG = 0x09,
            /// <summary>
            /// POSTInit 0xA
            /// </summary>
            POSTInit = 0xA,
            /// <summary>
            /// POSTGet 0xB
            /// </summary>
            POSTGet = 0xB,
            /// <summary>
            /// PowerUp 0x10
            /// </summary>
            PowerUp = 0x10,
            /// <summary>
            /// ShutDown 0x11
            /// </summary>
            ShutDown = 0x11,
            /// <summary>
            /// ISD_1 0x30
            /// </summary>
            ISD_1 = 0x30,
            /// <summary>
            /// ISD_2 0x31
            /// </summary>
            ISD_2 = 0x31,
            /// <summary>
            /// XSVF_1 0x2E
            /// </summary>
            XSVF_1 = 0x2E,
            /// <summary>
            /// XSVF_2 0x2F
            /// </summary>
            XSVF_2 = 0x2F,
            /// <summary>
            /// Update 0xF0
            /// </summary>
            Update = 0xF0
        }
        public enum Errors
        {
            None,
            Unknown,
            DeviceInUse,
            DeviceNotFound,
            WrongVersion,
            NoFlashConfig,
            FailedGetVersion,
            FailedGetConfig,
            WrongConfig,
            WrongHeader,
            GeneralError,
            NoFile,
            WrongFile
        }

        private UsbDevice OpenDevice(bool jrponly = false)
        {
            if (InUse) return null;
            UsbDevice MyUsbDevice = null;
            try
            {
                MyUsbDevice = UsbDevice.OpenUsbDevice(JRP);
                jrp = true;
                if (MyUsbDevice == null)
                {
                    if (!jrponly) MyUsbDevice = UsbDevice.OpenUsbDevice(NANDX);
                    jrp = false;
                }
                if (MyUsbDevice == null)
                {
                    return null;
                }
                IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                if (!ReferenceEquals(wholeUsbDevice, null))
                {
                    wholeUsbDevice.SetConfiguration(1);
                    if (variables.debugme) Console.WriteLine("Claiming Interface...");
                    wholeUsbDevice.ClaimInterface(0);
                    if (variables.debugme) Console.WriteLine("The Interface is ours!");
                }
                InUse = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return MyUsbDevice;
        }

        private void CloseDevice(UsbDevice dev)
        {
            if (dev != null)
            {
                if (dev.IsOpen)
                {
                    IUsbDevice wholeUsbDevice = dev as IUsbDevice;
                    if (!ReferenceEquals(wholeUsbDevice, null))
                    {
                        wholeUsbDevice.ReleaseInterface(0);
                    }
                    dev.Close();
                }
            }
            dev = null;

            UsbDevice.Exit();
            InUse = false;
        }



        private bool ArmVersion(UsbDevice dev, UsbEndpointReader reader, out byte[] readBuffer, out ErrorCode ec, out int bytesRead)
        {
            int tries = 1;
            int lengthTransfered = 0x10;
            byte[] buffer = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            ec = ErrorCode.None;
            readBuffer = new byte[4];
            bytesRead = 0;

            UsbSetupPacket packet = new UsbSetupPacket();

            packet.RequestType = (byte)UsbRequestType.TypeVendor;
            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = (byte)Commands.Version;
            ///Arm version
            if (dev.ControlTransfer(ref packet, buffer, 8, out lengthTransfered))
            {
                do
                {
                    ec = reader.Read(readBuffer, timeout, out bytesRead);
                    if (variables.debugme) { Console.WriteLine("Bytes Read {0}", bytesRead); Console.WriteLine("Read Buffer {0}", Oper.ByteArrayToString(readBuffer)); }
                    if (variables.debugme) Console.WriteLine(ec.ToString());
                    if (variables.debugme) Console.WriteLine("Retry {0}", tries);
                    tries++;
                }
                while (ec != ErrorCode.Success && tries < 5);
            }
            else return false;
            return true;
        }

        private bool FlashConfig(UsbDevice dev, UsbEndpointReader reader, out byte[] readBuffer, out ErrorCode ec, out int bytesRead)
        {
            int tries = 1;
            int lengthTransfered = 0x10;
            byte[] buffer = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            ec = ErrorCode.None;
            readBuffer = new byte[4];
            bytesRead = 0;

            UsbSetupPacket packet = new UsbSetupPacket();

            packet.RequestType = (byte)UsbRequestType.TypeVendor;
            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = (byte)Commands.FlashDataInit;
            ///Arm version
            if (dev.ControlTransfer(ref packet, buffer, 8, out lengthTransfered))
            {
                do
                {
                    ec = reader.Read(readBuffer, timeout, out bytesRead);
                    if (variables.debugme) { Console.WriteLine("Bytes Read {0}", bytesRead); Console.WriteLine("Read Buffer {0}", Oper.ByteArrayToString(readBuffer)); }
                    if (variables.debugme) Console.WriteLine(ec.ToString());
                    if (variables.debugme) Console.WriteLine("Retry {0}", tries);
                    tries++;
                }
                while (ec != ErrorCode.Success && tries < 5);
            }
            else return false;
            return true;
        }

        private bool DeInit(UsbDevice dev)
        {
            int lengthTransfered = 0x10;
            byte[] buffer = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            UsbSetupPacket packet = new UsbSetupPacket();

            packet.RequestType = (byte)UsbRequestType.TypeVendor;
            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x0;
            packet.Request = (byte)Commands.FlashDataDeInit;
            return dev.ControlTransfer(ref packet, buffer, 8, out lengthTransfered);
        }



        private Errors read_v2(string filename, Nandsize nsize, bool print = true, int startblock = 0, int length = 0)
        {
            lock (MainForm._object)
            {
                if (InUse) return Errors.DeviceInUse;
                UsbDevice MyUsbDevice = null;
                try
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    MyUsbDevice = OpenDevice();

                    if (MyUsbDevice == null) return Errors.DeviceNotFound;
                    UsbEndpointReader reader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep02);
                    ErrorCode ec = ErrorCode.None;

                    byte[] readBuffer = new byte[4];
                    byte[] readBuf = new byte[0x4200];
                    int bytesRead;

                    ///Arm Version
                    if (!ArmVersion(MyUsbDevice, reader, out readBuffer, out ec, out bytesRead)) return Errors.FailedGetVersion; //1;
                    if (print) Console.WriteLine("Version: {0}", Oper.ByteArrayToString(readBuffer).Substring(0, 2));
                    if (!versions.Contains(Oper.ByteArrayToString(readBuffer)))
                    {
                        Console.WriteLine("Wrong Version");
                        return Errors.WrongVersion;// 2;
                    }

                    Thread.Sleep(100);

                    ///FlashConfig
                    readBuffer = new byte[4];
                    if (!FlashConfig(MyUsbDevice, reader, out readBuffer, out ec, out bytesRead)) return Errors.FailedGetConfig; //1;
                    Array.Reverse(readBuffer, 0, 0x4);
                    if (print) Console.WriteLine("Flash Config: 0x" + BitConverter.ToString(readBuffer, 0, 0x4).Replace("-", ""));
                    if (Oper.ByteArrayToString(readBuffer) == "00000000")
                    {
                        Console.WriteLine("Can Not Continue");
                        Console.WriteLine("");
                        return Errors.NoFlashConfig;// 2;
                    }
                    bool found = false;
                    foreach (string fconf in variables.flashconfigs)
                    {
                        if (string.Equals(BitConverter.ToString(readBuffer, 0, 0x4).Replace("-", ""), fconf))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        Console.WriteLine("Unrecongized Flash Config");
                    }

                    try
                    {
                        File.Delete(filename);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); };


                    if (length == 0)
                    {
                        length = nsize.GetHashCode();
                    }
                    Console.WriteLine("Reading Nand to {0}", filename);
                    BinaryWriter sw = new BinaryWriter(File.Open(filename, FileMode.Append, FileAccess.Write));
                    int i = startblock;
                    while (i < (length + startblock) && !variables.escapeloop)
                    {
                        UpdateProgres((i * 100) / (length + startblock - 1));
                        UpdateBloc(i.ToString("X"));

                        readBuf = new byte[0x4200];
                        int lengthTransfered = 0;
                        bool result = read_sector(MyUsbDevice, reader, i, out readBuf, out lengthTransfered, out ec);
                        if (variables.debugme) Console.WriteLine(result);
                        Thread.Sleep(1);

                        try
                        {
                            sw.Write(readBuf);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        i++;
                    }
                    readBuf = null;
                    variables.escapeloop = false;
                    sw.Close();

                    DeInit(MyUsbDevice);

                    stopwatch.Stop();
                    UpdateBloc("");
                    Console.WriteLine("Read Successful! Time Elapsed: {0}:{1:D2}", stopwatch.Elapsed.Minutes + (stopwatch.Elapsed.Hours * 60), stopwatch.Elapsed.Seconds);
                    Console.WriteLine("");
                    return Errors.None;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                finally
                {
                    CloseDevice(MyUsbDevice);
                    MainForm._waitmb.Reset();
                    MainForm._waitmb.Set();
                }
            }
            return Errors.Unknown;
        }

        private Errors erase_v2(Nandsize nsize, bool print = true, int startblock = 0, int length = 0)
        {
            lock (MainForm._object)
            {
                if (InUse) return Errors.DeviceInUse;
                UsbDevice MyUsbDevice = null;
                try
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    MyUsbDevice = OpenDevice();

                    if (MyUsbDevice == null) return Errors.DeviceNotFound;
                    UsbEndpointReader reader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep02);
                    ErrorCode ec = ErrorCode.None;

                    byte[] readBuffer = new byte[4];
                    int bytesRead;

                    ///Arm Version
                    if (!ArmVersion(MyUsbDevice, reader, out readBuffer, out ec, out bytesRead)) return Errors.FailedGetVersion; //1;
                    if (print) Console.WriteLine("Version: {0}", Oper.ByteArrayToString(readBuffer).Substring(0, 2));
                    if (!versions.Contains(Oper.ByteArrayToString(readBuffer)))
                    {
                        Console.WriteLine("Wrong Version");
                        return Errors.WrongVersion;// 2;
                    }

                    Thread.Sleep(100);

                    ///FlashConfig
                    readBuffer = new byte[4];
                    if (!FlashConfig(MyUsbDevice, reader, out readBuffer, out ec, out bytesRead)) return Errors.FailedGetConfig; //1;
                    Array.Reverse(readBuffer, 0, 0x4);
                    if (print) Console.WriteLine("Flash Config: 0x" + BitConverter.ToString(readBuffer, 0, 0x4).Replace("-", ""));
                    //if (Oper.ByteArrayToString(readBuffer) == "00000000")
                    {
                        //Console.WriteLine("Can not Continue");
                        //Console.WriteLine("");
                        //return Errors.NoFlashConfig;// 2;
                    }
                    bool found = false;
                    foreach (string fconf in variables.flashconfigs)
                    {
                        if (Oper.ByteArrayToString(readBuffer) == "00000000")
                        {
                            found = true;
                            break;
                        }
                        if (string.Equals(BitConverter.ToString(readBuffer, 0, 0x4).Replace("-", ""), fconf))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        Console.WriteLine("Unrecongized Flash Config");
                    }

                    if (length == 0)
                    {
                        length = nsize.GetHashCode();
                    }
                    Console.WriteLine("Erasing Nand");

                    int i = startblock;
                    while (i < (length + startblock) && !variables.escapeloop)
                    {
                        UpdateProgres((i * 100) / (length + startblock - 1));
                        UpdateBloc(i.ToString("X"));

                        erase_sector(MyUsbDevice, reader, i, out ec);

                        i++;
                    }
                    variables.escapeloop = false;

                    DeInit(MyUsbDevice);

                    stopwatch.Stop();
                    UpdateBloc("");
                    Console.WriteLine("Erase Successful! Time Elapsed: {0}:{1:D2}", stopwatch.Elapsed.Minutes + (stopwatch.Elapsed.Hours * 60), stopwatch.Elapsed.Seconds);
                    Console.WriteLine("");
                    return Errors.None;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                finally
                {
                    CloseDevice(MyUsbDevice);
                    MainForm._waitmb.Reset();
                    MainForm._waitmb.Set();
                }
            }
            return Errors.Unknown;
        }

        private Errors write_v2(string filename, Nandsize nsize, bool print = true, int startblock = 0, int length = 0, bool remap = false, bool fixecc = false)
        {
            lock (MainForm._object)
            {
                if (InUse) return Errors.DeviceInUse;
                UsbDevice MyUsbDevice = null;
                try
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    MyUsbDevice = OpenDevice();

                    if (MyUsbDevice == null) return Errors.DeviceNotFound;
                    UsbEndpointReader reader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep02);
                    UsbEndpointWriter writer = MyUsbDevice.OpenEndpointWriter(WriteEndpointID.Ep05);
                    ErrorCode ec = ErrorCode.None;

                    byte[] readBuffer = new byte[4];
                    int bytesRead;

                    ///Arm Version
                    if (!ArmVersion(MyUsbDevice, reader, out readBuffer, out ec, out bytesRead)) return Errors.FailedGetVersion; //1;
                    if (print) Console.WriteLine("Version: {0}", Oper.ByteArrayToString(readBuffer).Substring(0, 2));
                    if (!versions.Contains(Oper.ByteArrayToString(readBuffer)))
                    {
                        Console.WriteLine("Wrong Version");
                        return Errors.WrongVersion;// 2;
                    }

                    Thread.Sleep(100);

                    ///FlashConfig
                    readBuffer = new byte[4];
                    if (!FlashConfig(MyUsbDevice, reader, out readBuffer, out ec, out bytesRead)) return Errors.FailedGetConfig; //1;
                    Array.Reverse(readBuffer, 0, 0x4);
                    if (print) Console.WriteLine("Flash Config: 0x" + BitConverter.ToString(readBuffer, 0, 0x4).Replace("-", ""));
                    if (Oper.ByteArrayToString(readBuffer) == "00000000")
                    {
                        Console.WriteLine("Can not Continue");
                        Console.WriteLine("");
                        return Errors.NoFlashConfig;// 2;
                    }
                    bool found = false;
                    foreach (string fconf in variables.flashconfigs)
                    {
                        if (string.Equals(BitConverter.ToString(readBuffer, 0, 0x4).Replace("-", ""), fconf))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        Console.WriteLine("Unrecongized Flash Config");
                    }
                    string flashconfig = BitConverter.ToString(readBuffer, 0, 0x4).Replace("-", "");

                    int layout = 1;
                    if (flashconfig == "00AA3020" || flashconfig == "008A3020") layout = 2;
                    else if (flashconfig == "01198010") layout = 0;
                    else layout = 1;

                    byte[] writeBuffer = new byte[0x4200];
                    BinaryReader rw = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));

                    if (length == 0)
                    {
                        length = nsize.GetHashCode();
                        if (variables.debugme) Console.WriteLine("Length: {0:X} - size: {1}", length, nsize);
                    }
                    long filesize;
                    FileInfo fl = new FileInfo(filename);
                    filesize = fl.Length / 0x4200;
                    if (variables.debugme) Console.WriteLine("FileSize: {0:X}", filesize);
                    if (startblock + length > filesize)
                    {
                        length = (int)filesize - startblock;
                    }
                    if (length <= 0) length = 0;

                    List<int> badblocks = new List<int>();
                    Console.WriteLine("Writing {0} to Nand", Path.GetFileName(filename));
                    variables.writing = true;
                    int i = startblock;
                    if (variables.debugme) Console.WriteLine("Start: {0:X} - Length: {1:X}", startblock, length);
                    while (i < (length + startblock) && !variables.escapeloop)
                    {

                        try
                        {
                            writeBuffer = rw.ReadBytes(0x4200);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }

                        if (fixecc) writeBuffer = Nand.Nand.addecc_v2(writeBuffer, false, i * 0x4200, layout);
                        if (length + startblock - 1 != 0) UpdateProgres((i * 100) / (length + startblock - 1));
                        UpdateBloc(i.ToString("X"));
                        int lengthTransfered = 0;
                        readBuffer = new byte[4];
                        if (!write_sector_v2(MyUsbDevice, reader, writer, i, ref writeBuffer, out readBuffer, out lengthTransfered, out ec)) Console.WriteLine("Failed to write 0x{0:X} block", i);
                        if (Oper.ByteArrayToString(readBuffer).Substring(5, 3) == "258" || Oper.ByteArrayToString(readBuffer).Substring(5, 3) == "202")
                        {
                            badblocks.Add(i);
                        }

                        i++;
                    }
                    if (badblocks.Count != 0 && remap)
                    {
                        Console.WriteLine("Starting remapping process");
                        int count = badblocks.Count;

                        while (count != 0)
                        {
                            int number = badblocks.Count - count;
                            int reserveblockpos;
                            i = badblocks[number];
                            rw.BaseStream.Seek(0x4200 * i, SeekOrigin.Begin);
                            writeBuffer = rw.ReadBytes(0x4200);

                            if (fixecc) writeBuffer = Nand.Nand.addecc_v2(writeBuffer, false, i * 0x4200, layout);

                            if (flashconfig == "00AA3020" || flashconfig == "008A3020")
                            {
                                reserveblockpos = 0x1FF;
                            }
                            else
                            {
                                reserveblockpos = 0x3FF;
                            }

                            Console.WriteLine("Remapping Block {0:X} @ {1:X}", i, reserveblockpos - number);
                            int lengthTransfered = 0;
                            readBuffer = new byte[4];
                            if (!write_sector_v2(MyUsbDevice, reader, writer, reserveblockpos - number, ref writeBuffer, out readBuffer, out lengthTransfered, out ec)) Console.WriteLine("Failed to write 0x{0:X} block", i);
                            if (Oper.ByteArrayToString(readBuffer) != "00020000")
                            {
                                Array.Reverse(readBuffer);
                                Console.WriteLine("Error: {0} writing block {1:X}", Oper.ByteArrayToString(readBuffer).Substring(5, 3), i);
                            }
                            count--;
                        }
                    }

                    variables.escapeloop = false;
                    rw.Close();
                    DeInit(MyUsbDevice);

                    stopwatch.Stop();
                    UpdateBloc("");
                    variables.writing = false;
                    Console.WriteLine("Write Successful! Time Elapsed: {0}:{1:D2}", stopwatch.Elapsed.Minutes + (stopwatch.Elapsed.Hours * 60), stopwatch.Elapsed.Seconds);
                    Console.WriteLine("");
                    return Errors.None;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                finally
                {
                    CloseDevice(MyUsbDevice);
                    MainForm._waitmb.Reset();
                    MainForm._waitmb.Set();
                }
            }
            return Errors.Unknown;
        }



        private bool read_sector(UsbDevice dev, UsbEndpointReader reader, int sector, out byte[] readBuf, out int lengthTransfered, out ErrorCode ec)
        {
            UsbSetupPacket packet = new UsbSetupPacket();
            packet.RequestType = (byte)UsbRequestType.TypeVendor;
            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x0;

            ec = ErrorCode.None;
            byte[] readBuffer = new byte[4];
            readBuf = new byte[0x4200];
            int bytesRead = 0;
            lengthTransfered = 0x10;

            byte[] buffer = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            buffer[0] = (byte)(sector & 0xFF);
            buffer[1] = (byte)((sector & 0xFF00) / 255);
            buffer[5] = 66;

            packet.Request = (byte)Commands.FlashDataRead;
            if (dev.ControlTransfer(ref packet, buffer, 8, out lengthTransfered))
            {
                if (variables.debugme) Console.WriteLine("FlashDataRead");
                ec = reader.Read(readBuf, 0, 0x4200, timeout, out bytesRead);
                if (ec != ErrorCode.Success) Console.WriteLine(ec.ToString());
                if (bytesRead == 0) throw new Exception(string.Format("{0}:No more bytes!", ec));
            }
            else return false;

            packet.Request = (byte)Commands.FlashDataStatus;
            if (dev.ControlTransfer(ref packet, buffer, 8, out lengthTransfered))
            {
                if (variables.debugme) Console.WriteLine("FlashDataStatus");
                ec = reader.Read(readBuffer, 0, 0x4, timeout, out bytesRead);
                if (Oper.ByteArrayToString(readBuffer) != "00020000" && Oper.ByteArrayToString(readBuffer) != "00060000")
                {
                    Array.Reverse(readBuffer);
                    Console.WriteLine("Error: {0} reading block {1:X}", Oper.ByteArrayToString(readBuffer).Substring(5, 3), sector);
                }
                if (ec != ErrorCode.Success) Console.WriteLine(ec.ToString());
                if (bytesRead == 0) throw new Exception(string.Format("{0}:No more bytes!", ec));
            }
            else return false;
            return true;
        }

        private bool erase_sector(UsbDevice dev, UsbEndpointReader reader, int sector, out ErrorCode ec)
        {
            UsbSetupPacket packet = new UsbSetupPacket();
            packet.RequestType = (byte)UsbRequestType.TypeVendor;
            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x0;

            ec = ErrorCode.None;
            byte[] readBuffer = new byte[4];
            int bytesRead = 0;
            int lengthTransfered = 0x10;

            byte[] buffer = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            buffer[0] = (byte)(sector & 0xFF);
            buffer[1] = (byte)((sector & 0xFF00) / 255);
            buffer[4] = 4;
            packet.Request = (byte)Commands.FlashDataErase;
            if (dev.ControlTransfer(ref packet, buffer, 8, out lengthTransfered))
            {
                if (variables.debugme) Console.WriteLine("FlashDataErase");
                ec = reader.Read(readBuffer, 0, 0x4, timeout, out bytesRead);
                if (Oper.ByteArrayToString(readBuffer) != "00020000" && Oper.ByteArrayToString(readBuffer) != "00000000")
                {
                    Array.Reverse(readBuffer);
                    Console.WriteLine("Error: {0} erasing block {1:X}", Oper.ByteArrayToString(readBuffer).Substring(5, 3), sector);
                }
            }
            else return false;

            if (variables.debugme) Console.WriteLine("FlashDataStatus");
            packet.Request = (byte)Commands.FlashDataStatus;
            if (dev.ControlTransfer(ref packet, buffer, 8, out lengthTransfered))
            {
                ec = reader.Read(readBuffer, 0, 0x4, timeout, out bytesRead);
                if (Oper.ByteArrayToString(readBuffer) != "00020000" && Oper.ByteArrayToString(readBuffer) != "00000000")
                {
                    Array.Reverse(readBuffer);
                    Console.WriteLine("Error: {0} erasing block {1:X}", Oper.ByteArrayToString(readBuffer).Substring(5, 3), sector);
                }
            }
            else return false;
            return true;
        }

        private bool write_sector(UsbDevice dev, UsbEndpointReader reader, UsbEndpointWriter writer, int sector, ref byte[] writeBuf, out byte[] readBuffer, out int lengthTransfered, out ErrorCode ec)
        {
            UsbSetupPacket packet = new UsbSetupPacket();
            packet.RequestType = (byte)UsbRequestType.TypeVendor;
            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x0;
            ec = ErrorCode.None;
            readBuffer = new byte[4];
            int bytesRead = 0;
            lengthTransfered = 0x10;

            byte[] buffer = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            buffer[0] = (byte)(sector & 0xFF);
            buffer[1] = (byte)((sector & 0xFF00) / 255);
            buffer[5] = 66;

            packet.Request = (byte)Commands.FlashDataWrite;
            if (variables.debugme) Console.WriteLine("FlashDataWrite");
            if (dev.ControlTransfer(ref packet, buffer, 8, out lengthTransfered))
            {
                ec = writer.Write(writeBuf, timeout, out bytesRead);
                if (ec != ErrorCode.Success) Console.WriteLine(ec.ToString());
                //if (bytesRead == 0) throw new Exception(string.Format("{0}:No more bytes!", ec));

                if (!jrp)
                {
                    packet.Request = (byte)Commands.SPI;
                    if (variables.debugme) Console.WriteLine("SPI");
                    if (!dev.ControlTransfer(ref packet, buffer, 8, out lengthTransfered)) return false;
                }
            }
            else return false;
            if (jrp) Thread.Sleep(variables.delay);
            packet.Request = (byte)Commands.FlashDataStatus;
            if (variables.debugme) Console.WriteLine("FlashDataStatus");
            if (dev.ControlTransfer(ref packet, buffer, 8, out lengthTransfered))
            {
                ec = reader.Read(readBuffer, 0, 0x4, timeout, out bytesRead);
                if (Oper.ByteArrayToString(readBuffer) != "00020000" && Oper.ByteArrayToString(readBuffer) != "00060000")
                {
                    Array.Reverse(readBuffer);
                    Console.WriteLine("Error: {0} writing block {1:X}", Oper.ByteArrayToString(readBuffer).Substring(5, 3), sector);
                }
                if (ec != ErrorCode.Success) Console.WriteLine(ec.ToString());
                //if (bytesRead == 0) throw new Exception(string.Format("{0}:No more bytes!", ec));
            }
            else return false;
            return true;
        }

        private bool write_sector_v2(UsbDevice dev, UsbEndpointReader reader, UsbEndpointWriter writer, int sector, ref byte[] writeBuf, out byte[] readBuffer, out int lengthTransfered, out ErrorCode ec)
        {
            UsbSetupPacket packet = new UsbSetupPacket();
            packet.RequestType = (byte)UsbRequestType.TypeVendor;
            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x0;
            ec = ErrorCode.None;
            readBuffer = new byte[4];
            int bytesRead = 0;
            lengthTransfered = 0x10;

            byte[] buffer = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            buffer[0] = (byte)(sector & 0xFF);
            buffer[1] = (byte)((sector & 0xFF00) / 255);
            buffer[5] = 66;

            packet.Request = (byte)Commands.FlashDataWrite;
            if (variables.debugme) Console.WriteLine("FlashDataWrite");
            if (dev.ControlTransfer(ref packet, buffer, 8, out lengthTransfered))
            {
                ec = writer.Write(writeBuf, timeout, out bytesRead);
                if (ec != ErrorCode.Success) Console.WriteLine(ec.ToString());
                //if (bytesRead == 0) throw new Exception(string.Format("{0}:No more bytes!", ec));

                if (!jrp)
                {
                    packet.Request = (byte)Commands.SPI;
                    if (variables.debugme) Console.WriteLine("SPI");
                    if (!dev.ControlTransfer(ref packet, buffer, 8, out lengthTransfered)) return false;
                }
            }
            else return false;

            int counter = 0;
            while (counter < 5)
            {
                packet.Request = (byte)Commands.FlashDataStatus;
                if (variables.debugme) Console.WriteLine("FlashDataStatus");
                if (dev.ControlTransfer(ref packet, buffer, 8, out lengthTransfered))
                {
                    ec = reader.Read(readBuffer, 0, 0x4, 10, out bytesRead);
                    //if (ec != ErrorCode.Success) Console.WriteLine(ec.ToString());
                    //if (bytesRead == 0) throw new Exception(string.Format("{0}:No more bytes!", ec));
                }
                else return false;
                counter++;
                if (ec == ErrorCode.Success)
                {
                    if (Oper.ByteArrayToString(readBuffer) != "00020000" && Oper.ByteArrayToString(readBuffer) != "00060000")
                    {
                        Array.Reverse(readBuffer);
                        Console.WriteLine("Error: {0} writing block {1:X}", Oper.ByteArrayToString(readBuffer).Substring(5, 3), sector);
                    }
                    break;
                }
            }
            return true;
        }

        #region JR-P

        public void PowerUp()
        {
            if (InUse) return;
            UsbDevice MyUsbDevice = null;
            try
            {
                MyUsbDevice = OpenDevice();
                if (MyUsbDevice == null) { Console.WriteLine("Device Not Found."); return; }
                UsbSetupPacket packet = new UsbSetupPacket();
                packet.RequestType = (byte)UsbRequestType.TypeVendor;

                packet.Value = 0x00;
                packet.Index = 0x00;
                packet.Length = 0x8;
                packet.Request = (byte)Commands.PowerUp;
                int LengthTransferred = 0x10;
                byte[] buffer = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out LengthTransferred);
                if (variables.debugme) Console.WriteLine("Length Transferred {0}", LengthTransferred);
                Console.WriteLine("Power Up");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally
            {
                CloseDevice(MyUsbDevice);
            }
        }

        public void PowerDown()
        {
            if (InUse) return;
            UsbDevice MyUsbDevice = null;
            try
            {
                MyUsbDevice = OpenDevice();
                if (MyUsbDevice == null) { Console.WriteLine("Device not found."); return; }
                UsbSetupPacket packet = new UsbSetupPacket();
                packet.RequestType = (byte)UsbRequestType.TypeVendor;

                packet.Value = 0x00;
                packet.Index = 0x00;
                packet.Length = 0x8;
                packet.Request = (byte)Commands.ShutDown;
                int LengthTransferred = 0x10;
                byte[] buffer = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out LengthTransferred);
                if (variables.debugme) Console.WriteLine("Length Transferred {0}", LengthTransferred);
                Console.WriteLine("Shutdown");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally
            {
                CloseDevice(MyUsbDevice);
            }
        }

        public void Update()
        {
            if (InUse) return;
            UsbDevice MyUsbDevice = null;
            try
            {
                MyUsbDevice = OpenDevice();
                if (MyUsbDevice == null) { Console.WriteLine("Device Not Found."); return; }
                UsbSetupPacket packet = new UsbSetupPacket();
                packet.RequestType = (byte)UsbRequestType.TypeVendor;

                packet.Value = 0x00;
                packet.Index = 0x00;
                packet.Length = 0x8;
                packet.Request = (byte)Commands.Update;
                int LengthTransferred = 0x10;
                byte[] buffer = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out LengthTransferred);
                if (variables.debugme) Console.WriteLine("Length Transferred {0}", LengthTransferred);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally
            {
                CloseDevice(MyUsbDevice);
            }
        }

        #endregion


        #region read

        public Errors read(string filename, Nandsize nsize, bool print = true, int startblock = 0, int length = 0)
        {
            Errors result = Errors.None;
            result = read_v2(filename, nsize, print, startblock, length);
            if (result == Errors.DeviceNotFound) { Console.WriteLine(("Device Not Found")); return Errors.DeviceNotFound; }

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
            return result;
        }

        public Errors getflashmb(ref string flashconf, bool stealth)
        {
            Errors result = Errors.None;
            result = getflashmb_JRunner(ref flashconf);
            if (result == Errors.DeviceNotFound) { if (!stealth) Console.WriteLine(("Device Not Found")); }
            return result;
        }

        private Errors getflashmb_JRunner(ref string flashconf)
        {
            UsbDevice MyUsbDevice = null;
            if (variables.debugme) Console.WriteLine("Entered Get flashconfig");
            if (InUse) return Errors.DeviceInUse;
            try
            {
                MyUsbDevice = OpenDevice();
                if (MyUsbDevice == null) return Errors.DeviceNotFound;
                UsbEndpointReader reader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep02);
                ErrorCode ec = ErrorCode.None;

                byte[] readBuffer = new byte[4];
                byte[] readBuf = new byte[0x4200];
                int bytesRead;

                ///Arm Version
                if (!ArmVersion(MyUsbDevice, reader, out readBuffer, out ec, out bytesRead)) return Errors.FailedGetVersion; //1;
                Console.WriteLine("Version: {0}", Oper.ByteArrayToString(readBuffer).Substring(0, 2));
                if (!versions.Contains(Oper.ByteArrayToString(readBuffer)))
                {
                    Console.WriteLine("Wrong Version");
                    return Errors.WrongVersion;// 2;
                }

                Thread.Sleep(100);
                ///FlashConfig
                readBuffer = new byte[4];
                if (!FlashConfig(MyUsbDevice, reader, out readBuffer, out ec, out bytesRead)) return Errors.FailedGetConfig; //1;
                Array.Reverse(readBuffer, 0, 0x4);
                flashconf = Oper.ByteArrayToString(readBuffer);
                Console.WriteLine("Flash Config: 0x{0}", BitConverter.ToString(readBuffer, 0, 0x4).Replace("-", ""));
                if (variables.debugme) analyzeflashconfig(Oper.ByteArrayToInt(readBuffer));
                if (flashconf == "00000000")
                {
                    Console.WriteLine("Console Not Found");
                    return Errors.NoFlashConfig;// 2;
                }

                bool found = false;
                foreach (string fconf in variables.flashconfigs)
                {
                    if (string.Equals(BitConverter.ToString(readBuffer, 0, 0x4).Replace("-", ""), fconf))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Console.WriteLine("Unrecongized Flash Config");
                }

                variables.conf = null;
                byte[] temp = { };
                if (variables.debugme) Console.WriteLine("Reading Nand\n");
                int lengthTransfered = 0;
                for (int i = 0; i <= 3; i++)
                {
                    read_sector(MyUsbDevice, reader, i, out readBuf, out lengthTransfered, out ec);
                    temp = Oper.addtoflash_v2(temp, readBuf);
                }
                variables.conf = temp;
                DeInit(MyUsbDevice);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                if (variables.debugme) Console.WriteLine(ex.ToString());
            }
            finally
            {
                CloseDevice(MyUsbDevice);
                MainForm._waitmb.Reset();
                MainForm._waitmb.Set();
            }

            return Errors.None;
        }

        #endregion

        #region erase

        public Errors erase(Nandsize nsize, int startblock = 0, int length = 0)
        {
            Errors result = 0;
            result = erase_v2(nsize, true, startblock, length);
            if (result == Errors.DeviceNotFound) { Console.WriteLine(("Device Not Found")); return result; }

            try
            {
                SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                if (variables.soundsuccess != "") success.SoundLocation = variables.soundsuccess;
                success.Play();
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); };
            return result;
        }

        #endregion

        #region write

        public Errors write(string filename, Nandsize nsize, int startblock = 0, int length = 0, bool remap = false, bool fixecc = false)
        {
            variables.writing = true;
            Errors result = Errors.None;
            result = write_v2(filename, nsize, true, startblock, length, remap, fixecc);
            if (result == Errors.DeviceNotFound) { Console.WriteLine(("Device Not Found")); return Errors.DeviceNotFound; }

            try
            {
                SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                if (variables.soundsuccess != "") success.SoundLocation = variables.soundsuccess;
                success.Play();
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); variables.writing = false; };
            variables.writing = false;
            return result;
        }

        #endregion

        #region xilinx id
        private byte[] x32_id = {
            0x07,0x20,0x12,0x00,0x12,0x01,0x02,0x08,0x01,0x08,0x00,0x00,0x00,0x20,0x01,0x0F,
            0xFF,0x8F,0xFF,0x09,0x00,0x00,0x00,0x00,0xF6,0xE1,0xF0,0x93,0x00,0x00,0x00,0x00
                                      };
        private byte[] x64_id = {
            0x07,0x20,0x12,0x00,0x12,0x01,0x02,0x08,0x01,0x08,0x00,0x00,0x00,0x20,0x01,0x0F,
            0xFF,0x8F,0xFF,0x09,0x00,0x00,0x00,0x00,0xF6,0xE5,0xF0,0x93,0x00,0x00,0x00,0x00
                                      };
        private byte[] x128_id = {
             0x07,0x20,0x12,0x00,0x12,0x01,0x02,0x08,0x01,0x08,0x00,0x00,0x00,0x20,0x01,0x0F,
             0xFF,0x8F,0xFF,0x09,0x00,0x00,0x00,0x00,0xF6,0xD8,0xF0,0x93,0x00,0x00,0x00,0x00
                                       };
        private byte[] x256_id = {
            0x07,0x20,0x12,0x00,0x12,0x01,0x02,0x08,0x01,0x08,0x00,0x00,0x00,0x20,0x01,0x0F,
            0xFF,0x8F,0xFF,0x09,0x00,0x00,0x00,0x00,0xF6,0xD4,0xF0,0x93,0x00,0x00,0x00,0x00
        };
        private byte[] protected_id = {

    0x07, 0x00, 0x13, 0x00, 0x14, 0x00, 0x12, 0x00, 0x12, 0x01, 0x02, 0x08, 0x01, 0x08, 0x00, 0x00,
    0x00, 0x20, 0x01, 0x0F, 0xFF, 0x8F, 0xFF, 0x04, 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00,
    0x00, 0xF6, 0xD8, 0xF0, 0x93, 0x02, 0x08, 0xFF, 0x02, 0x08, 0x01, 0x09, 0x00, 0x00, 0x00, 0x00,
    0xF6, 0xD8, 0xF0, 0x93, 0x02, 0x08, 0xFF, 0x02, 0x08, 0xFF, 0x02, 0x08, 0xFD, 0x01, 0xFF, 0xFF,
    0xFF, 0xFF, 0x09, 0x00, 0x00, 0x00, 0x00, 0x31, 0x39, 0x31, 0x31, 0x07, 0x00, 0x07, 0x20, 0x12,
    0x00, 0x12, 0x01, 0x04, 0x00, 0x00, 0x00, 0x00, 0x02, 0x08, 0xFF, 0x08, 0x00, 0x00, 0x00, 0x01,
    0x01, 0x00, 0x09, 0x00, 0x00, 0x00,

                                      };

        #endregion

        #region XSVF

        #region Arm

        private Errors xsvfwrite(string filename, UsbDevice MyUsbDevice)
        {
            lock (MainForm._object)
            {
                try
                {
                    if (MyUsbDevice == null) return Errors.DeviceNotFound;

                    UsbSetupPacket packet = new UsbSetupPacket();
                    packet.RequestType = (byte)UsbRequestType.TypeVendor;
                    ErrorCode ec = ErrorCode.None;
                    UsbEndpointReader reader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep02);
                    UsbEndpointWriter writer = MyUsbDevice.OpenEndpointWriter(WriteEndpointID.Ep05);
                    byte[] readBuffer = new byte[4];
                    int bytesRead;


                    packet.Value = 0x00;
                    packet.Index = 0x00;
                    packet.Length = 0x8;
                    packet.Request = (byte)Commands.Version;
                    int hello = 0x10;
                    byte[] buffer = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                    buffer[4] = 0x4;
                    MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out hello);
                    ec = reader.Read(readBuffer, timeout, out bytesRead);
                    Console.WriteLine("Version: {0}", Oper.ByteArrayToString(readBuffer).Substring(0, 2));
                    if (readBuffer[0] != 0x03)
                    {
                        Console.WriteLine("Wrong Arm Version");
                        return Errors.WrongVersion;
                    }

                    MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out hello);
                    ec = reader.Read(readBuffer, timeout, out bytesRead);

                    MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out hello);
                    ec = reader.Read(readBuffer, timeout, out bytesRead);
                    buffer[4] = 0x0;

                    Console.WriteLine("Flashing {0} to Glitch Chip", Path.GetFileName(filename));
                    FileInfo finfo = new FileInfo(filename);
                    long length = finfo.Length;
                    int len = 0;
                    byte[] writeBuffer = new byte[length];
                    BinaryReader rw = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));
                    writeBuffer = rw.ReadBytes((int)length);
                    rw.Close();
                    writeBuffer = compression(writeBuffer, ref len);

                    packet.Request = (byte)Commands.FlashDataWrite;
                    buffer[4] = (byte)((len) & 0xFF);
                    buffer[5] = (byte)((len >> 8) & 0xFF);
                    buffer[6] = (byte)((len >> 16) & 0xFF);
                    buffer[7] = (byte)((len >> 24) & 0xFF);
                    MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out hello);
                    //ec = reader.Read(readBuffer, timeout, out bytesRead);



                    ec = writer.Write(writeBuffer, timeout, out bytesRead);
                    writeBuffer = null;
                    buffer[4] = 0x0;
                    buffer[5] = 0x0;
                    buffer[6] = 0x0;
                    buffer[7] = 0x0;
                    packet.Request = (byte)Commands.JTAG;
                    MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out hello);
                    ec = reader.Read(readBuffer, timeout, out bytesRead);
                    if (variables.debugme) Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                    buffer[4] = 0x4;
                    packet.Request = (byte)Commands.FlashDataStatus;
                    MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out hello);
                    ec = reader.Read(readBuffer, timeout, out bytesRead);

                    if (variables.debugme) Console.WriteLine("FlashDataDeInit");
                    if (variables.debugme) Console.WriteLine(Oper.ByteArrayToString(readBuffer));
                    Array.Reverse(readBuffer);
                    if (readBuffer.toUint() != 0)
                    {
                        Console.WriteLine("Failed with 0x{0:X} status!\n", Oper.ByteArrayToString(readBuffer, 2));
                        return Errors.GeneralError;
                    }
                    else
                    {
                        Console.WriteLine("Done");
                        Console.WriteLine("");
                    }
                    return Errors.None;
                }
                catch (Exception ex)
                {
                    if (variables.debugme) Console.WriteLine(ex.ToString());
                    Console.WriteLine(ex.Message);
                }
            }
            return Errors.None;
        }

        private byte[] compression(byte[] image, ref int length)
        {
            int i = 0;
            length = 0;
            byte count = 0, last = 0;
            Console.WriteLine("Input Filesize: {0:X}", image.Length);
            byte[] data = new byte[image.Length + image.Length];
            for (i = 0; i < image.Length; i++, length++)
            {
                data[length] = image[i];
                if ((i > 2) && (image[i - 1] == image[i - 2]))
                {

                    count = 0;
                    last = image[i - 1];
                    while (i < image.Length && image[i] == last)
                    {
                        i++;
                        count++;
                    }

                    data[length] = count;
                    length++;
                    //Console.WriteLine("{0:X} - {1:X}", i, j);

                    if (i >= image.Length)
                    {
                        Console.WriteLine("Output Filesize: {0:X}", length);
                        return Oper.returnportion(data, 0, length);
                    }
                    data[length] = image[i];

                    i++;
                    length++;
                    if (i >= image.Length)
                    {
                        Console.WriteLine("Output Filesize: {0:X}", length);
                        return Oper.returnportion(data, 0, length);
                    }
                    data[length] = image[i];
                }
            }
            Console.WriteLine("Output Filesize: {0:X}", length);
            return data = Oper.returnportion(data, 0, length);
        }

        #endregion
        #region Spi

        public Errors xsvf(string filename)
        {

            UsbDevice MyUsbDevice = null;
            if (!File.Exists(filename))
            {
                Console.WriteLine("Couldnt find file {0}", filename);
                return Errors.NoFile;
            }
            if (Path.GetExtension(filename) != ".xsvf")
            {
                Console.WriteLine("File type is not XSVF: {0}", filename);
                return Errors.WrongFile;
            }
            if (InUse) return Errors.DeviceInUse;
            try
            {
                MyUsbDevice = OpenDevice();

                if (MyUsbDevice == null) { Console.WriteLine("Device not found."); return Errors.DeviceNotFound; }
                if (!jrp) { return xsvfwrite(filename, MyUsbDevice); }
                UsbEndpointReader reader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep02);
                UsbEndpointWriter writer = MyUsbDevice.OpenEndpointWriter(WriteEndpointID.Ep05);
                Console.WriteLine("USB XSVF Player Initialized");

                if (sendXSVF(x64_id, MyUsbDevice, writer) == 1)
                {
                    Console.WriteLine("Xilinx XC2C64A ......... [DETECTED]");
                    if (!Oper.ByteArrayCompare(Oper.returnportion(x64_id, 0x13, 9), Oper.returnportion(File.ReadAllBytes(filename), 0x1C, 9)))
                    {
                        Console.WriteLine("Unsupported XSVF file");
                        return Errors.WrongFile;
                    }
                }
                else if (sendXSVF(x32_id, MyUsbDevice, writer) == 1)
                {
                    Console.WriteLine("Xilinx XC2C32A ......... [DETECTED]");
                    if (!Oper.ByteArrayCompare(Oper.returnportion(x32_id, 0x13, 9), Oper.returnportion(File.ReadAllBytes(filename), 0x1C, 9)))
                    {
                        Console.WriteLine("Unsupported XSVF file");
                        return Errors.WrongFile;
                    }
                }
                else if (sendXSVF(x128_id, MyUsbDevice, writer) == 1)
                {
                    Console.WriteLine("Xilinx XC2C128 ......... [DETECTED]");
                    if (sendXSVF(protected_id, MyUsbDevice, writer) == 1)
                    {
                        //Console.WriteLine("Protection check");
                        if (DialogResult.No == MessageBox.Show("Factory fw sign detected, writing this device is a one way process, and cant be reversed.\n Do you wish to proceed?", "Protected", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                        {
                            return Errors.GeneralError;
                        };
                    }
                    if (!Oper.ByteArrayCompare(Oper.returnportion(x128_id, 0x13, 9), Oper.returnportion(File.ReadAllBytes(filename), 0x1C, 9)))
                    {
                        Console.WriteLine("Unsupported XSVF file");
                        return Errors.WrongFile;
                    }
                }
                else if (sendXSVF(x256_id, MyUsbDevice, writer) == 1)
                {
                    Console.WriteLine("Xilinx XC2C256 ......... [DETECTED]");
                    if (!Oper.ByteArrayCompare(Oper.returnportion(x256_id, 0x13, 9), Oper.returnportion(File.ReadAllBytes(filename), 0x1C, 9)))
                    {
                        Console.WriteLine("Unsupported XSVF file");
                        return Errors.WrongFile;
                    }
                }
                else
                {
                    Console.WriteLine("Xilinx Device ..... [NOT DETECTED]");
                    return Errors.DeviceNotFound;
                }
                if (sendErase(MyUsbDevice) == 0)
                {
                    Console.WriteLine("Erase Failed");
                    return Errors.GeneralError;
                }
                else
                    Console.WriteLine("Erase Succeeded");

                Console.WriteLine("File: " + Path.GetFileName(filename));
                Console.WriteLine("Sending Out Packets .........");

                if (sendcode(filename, MyUsbDevice, writer) == 1)
                {
                    Console.WriteLine("Success");
                    return Errors.None;
                }
                else
                {
                    Console.WriteLine("Write Failed!");
                }
                return Errors.GeneralError;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            finally
            {
                CloseDevice(MyUsbDevice);
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
            return Errors.None;
        }

        private int sendXSVF(byte[] payload, UsbDevice MyUsbDevice, UsbEndpointWriter writer)
        {
            int[] Status = { 0x0, 0x00 };
            int CMD = 0x20;

            ErrorCode ec = ErrorCode.None;
            UsbSetupPacket packet = new UsbSetupPacket();

            byte[] readBuffer = new byte[2];
            int bytesRead;

            packet.RequestType = (byte)UsbRequestType.TypeVendor;
            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x1;
            packet.Request = 0x2E;
            int LengthTransferred = 0x10;


            if (MyUsbDevice.ControlTransfer(ref packet, CMD, 1, out LengthTransferred))
            //int usb_control_msg(HANDLE *dev, int requesttype, int request, int value, int index, char *bytes, int size, int timeout);
            // int usb_bulk_write(HANDLE *dev, int ep, char *bytes, int size, int timeout);
            {			// class  xsvf_cmd
                Thread.Sleep(200);

                while (true)
                {
                    Status[0] = 0x20;
                    packet.Request = 0x2F;
                    packet.RequestType = 0xC0;
                    packet.Length = 0x2;
                    while (Status[0] == 0x20)
                    {
                        //wholeUsbDevice.ControlTransfer(ref packet, Status, 2, out LengthTransferred);
                        MyUsbDevice.ControlTransfer(ref packet, Status, 2, out LengthTransferred); //  xsvf_poll for status
                        if (variables.debugme) Console.WriteLine("Status[0] 0x{0:X}", Status[0]);
                        Thread.Sleep(5);
                    } 												//	already on a cmd
                    if (variables.debugme) Console.WriteLine("Status 0x{0:X}", Status[0]);
                    if (Status[0] != 0x21)															//  status != 0x21 -- xsvf_out
                        break;																			//  hang for ready from PIC

                    ec = writer.Write(payload, timeout, out bytesRead);
                    if (variables.debugme) Console.WriteLine("Bytes Read {0}", bytesRead);
                    if (bytesRead < 0x20)
                    {						// endpoint 0x05, payload, len 0x20, TO = 5s
                        return 0;
                    }
                    Thread.Sleep(5);
                }

                if (Status[0] == 0x22)
                {																	// status = 0x22  xsvf_ok  good!
                    return 1;
                }
            }
            return 0;
        }

        private int sendErase(UsbDevice MyUsbDevice)
        {
            UsbSetupPacket packet = new UsbSetupPacket();

            byte[] readBuffer = new byte[2];
            byte[] Status = new byte[2];//Status[0]
            byte CMD = 0x24;
            packet.RequestType = (byte)UsbRequestType.TypeVendor;

            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x1;
            packet.Request = 0x2E;
            int LengthTransferred = 0x10;
            byte[] buffer = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            if (MyUsbDevice.ControlTransfer(ref packet, CMD, 1, out LengthTransferred))
            {			// class  xsvf_cmd
                Thread.Sleep(400);

                while (true)
                {
                    Status[0] = CMD;
                    while (Status[0] == CMD)
                    {
                        packet.Request = 0x2F;
                        packet.RequestType = 0xC0;
                        packet.Length = 0x2;
                        //wholeUsbDevice.ControlTransfer(ref packet, Status, 2, out LengthTransferred);
                        MyUsbDevice.ControlTransfer(ref packet, Status, 2, out LengthTransferred); //  xsvf_poll for status
                        if (variables.debugme) Console.WriteLine("Status[0] 0x{0:X}", Status[0]);
                        //	alr	//  xsvf_poll for status
                        Thread.Sleep(5);
                    }											//	already on a cmd
                    if (Status[0] == 0x22)
                    {																	// status = 0x22  xsvf_ok  good!
                        return 1;
                    }
                }
            }
            return 0;
        }

        private int sendcode(string filename, UsbDevice MyUsbDevice, UsbEndpointWriter writer)
        {
            ErrorCode ec = ErrorCode.None;
            UsbSetupPacket packet = new UsbSetupPacket();
            byte[] readBuffer = new byte[4];
            int bytesRead;
            byte[] Status = new byte[2];//Status[0]
            byte CMD = 0x20;
            byte[] buffer = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] writeBuffer = new byte[64];

            packet.RequestType = (byte)UsbRequestType.TypeVendor;
            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x1;
            packet.Request = 0x2E;
            int LengthTransferred = 0x0;

            int j = 0, i = 0;

            long filesize;
            FileInfo fl = new FileInfo(filename);
            filesize = fl.Length;
            if (variables.debugme) Console.WriteLine("Filesize {0}", filesize);
            int rounded = ((((int)filesize / 64) + 1) * 64);
            byte[] firstbuffer = new byte[rounded];
            if (variables.debugme) Console.WriteLine("Rounded {0}", rounded);
            BinaryReader rw = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));
            for (int k = 0; k < filesize; k++)
            {
                firstbuffer[k] = rw.ReadByte();
            }
            rw.Close();
            if (MyUsbDevice.ControlTransfer(ref packet, CMD, 1, out LengthTransferred))
            {			// class  xsvf_cmd
                Thread.Sleep(200);

                for (j = 200; ; j = 5)
                {
                    Thread.Sleep(j);
                    Status[0] = 0x20;

                    while (Status[0] == 0x20)
                    {
                        Thread.Sleep(5);
                        packet.Request = 0x2F;
                        packet.RequestType = 0xC0;
                        packet.Length = 2;
                        MyUsbDevice.ControlTransfer(ref packet, Status, 2, out LengthTransferred);//  xsvf_poll for status
                    } 												//	already on a cmd
                    if (variables.debugme) Console.WriteLine("Status 0x{0:X}", Status[0]);
                    if (variables.debugme) Console.WriteLine("LengthTransferred 0x{0:X}", LengthTransferred);
                    if (Status[0] != 0x21)
                        break;

                    //  status != 0x21 -- xsvf_out

                    writeBuffer = Oper.returnportion(firstbuffer, i, 64);
                    ec = writer.Write(writeBuffer, timeout, out bytesRead);
                    if (variables.debugme) Console.WriteLine("Bytes Read: {0}", bytesRead);
                    if (bytesRead < 64)
                    {						// endpoint 0x05, payload, len 0x20, TO = 5s
                        return 0;
                    }
                    UpdateProgres(((i / 64) * 100 / ((int)filesize / 64)));
                    UpdateBloc((i / 64).ToString());
                    i += 64;
                }										//	already on a cmd
                if (Status[0] == 0x22)
                {																	// status = 0x22  xsvf_ok  good!
                    return 1;
                }

            }
            return 0;
        }

        #endregion

        #endregion

        #region POST monitor

        public void log_post()
        {
            if (InUse) return;
            lock (MainForm._object)
            {
                UsbDevice MyUsbDevice = null;

                try
                {
                    enumerate_post();
                }
                catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
                try
                {
                    MyUsbDevice = OpenDevice(true);

                    if (MyUsbDevice == null) { Console.WriteLine("Device Not Found"); return; }

                    UsbEndpointReader reader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep02);
                    UsbSetupPacket packet = new UsbSetupPacket();
                    ErrorCode ec = ErrorCode.None;

                    byte[] readBuffer = new byte[4];
                    int bytesRead;

                    ///Arm Version
                    if (!ArmVersion(MyUsbDevice, reader, out readBuffer, out ec, out bytesRead)) return;
                    Console.WriteLine("Version: {0}", Oper.ByteArrayToString(readBuffer).Substring(0, 2));
                    if (versions[0] != (Oper.ByteArrayToString(readBuffer)) && versions[2] != (Oper.ByteArrayToString(readBuffer)))
                    {
                        Console.WriteLine("Wrong Version");
                        return;
                    }

                    Thread.Sleep(100);

                    packet.RequestType = (byte)UsbRequestType.TypeVendor;
                    packet.Value = 0x00;
                    packet.Index = 0x00;
                    packet.Length = 0x8;
                    packet.Request = 0x8;
                    int lengthTransfered = 0x10;
                    byte[] buffer = { 0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00 };

                    Console.WriteLine("Press escape to exit");
                    Console.WriteLine("Waiting for POST to change");
                    variables.logtofile = false;
                    packet.Request = (byte)Commands.POSTInit;
                    MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out lengthTransfered);
                    packet.Request = (byte)Commands.POSTGet;

                    List<int> dl = new List<int>();
                    dl.Add(0x00); dl.Add(0xFF); dl.Add(0x02);
                    bool output = false;
                    while (!variables.escapeloop)
                    {
                        MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out lengthTransfered);
                        ec = reader.Read(readBuffer, timeout, out bytesRead);
                        if (variables.debugme) Console.WriteLine("Bytes Read {0} - Read Buffer {1}", bytesRead, Oper.ByteArrayToString(readBuffer));
                        if (!dl.Contains((readBuffer)[0])) output = true;
                        if (output && !dl.Contains((readBuffer)[0]))
                        {
                            try
                            {
                                if (POST.ContainsKey(readBuffer[0]))
                                {
                                    Console.WriteLine("Post {0} - {1}", Oper.ByteArrayToString(readBuffer).Substring(0, 2), POST[readBuffer[0]]);
                                }
                                else
                                {
                                    Console.WriteLine("Post {0}", Oper.ByteArrayToString(readBuffer).Substring(0, 2));
                                }
                            }
                            catch (Exception ex)
                            {
                                if (variables.debugme) Console.WriteLine(ex.ToString());
                            }
                        }
                    }
                    reader.ReadFlush();
                    reader.Flush();
                    reader.Dispose();
                    variables.escapeloop = false;
                    Console.WriteLine("Done");
                    Console.WriteLine("");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    variables.logtofile = true;
                    CloseDevice(MyUsbDevice);

                    MainForm._waitmb.Reset();
                    MainForm._waitmb.Set();

                }
            }
            return;
        }

        private void enumerate_post()
        {
            if (POST.Count != 0) return;
            // 1BL 0 0x1*
            POST.Add(0x10, "Payload/1BL started");
            POST.Add(0x11, "FSB_CONFIG_PHY_CONTROL");
            POST.Add(0x12, "FSB_CONFIG_RX_STATE");
            POST.Add(0x13, "FSB_CONFIG_TX_STATE");
            POST.Add(0x14, "FSB_CONFIG_TX_CREDITS");
            POST.Add(0x15, "FETCH_OFFSET");
            POST.Add(0x16, "FETCH_HEADER");
            POST.Add(0x17, "VERIFY_HEADER");
            POST.Add(0x18, "FETCH_CONTENTS");
            POST.Add(0x19, "HMACSHA_COMPUTE");
            POST.Add(0x1A, "RC4_INITIALIZE");
            POST.Add(0x1B, "RC4_DECRYPT");
            POST.Add(0x1C, "SHA_COMPUTE");
            POST.Add(0x1D, "SIG_VERIFY");
            POST.Add(0x1E, "BRANCH");
            //1BL Hardware Exception - 0x81-0x91
            POST.Add(0x81, "Panic - MACHINE_CHECK");
            POST.Add(0x82, "Panic - DATA_STORAGE");
            POST.Add(0x83, "Panic - DATA_SEGMENT");
            POST.Add(0x84, "Panic - INSTRUCTION_STORAGE");
            POST.Add(0x85, "Panic - INSTRUCTION_SEGMENT");
            POST.Add(0x86, "Panic - EXTERNAL");
            POST.Add(0x87, "Panic - ALIGNMENT");
            POST.Add(0x88, "Panic - PROGRAM");
            POST.Add(0x89, "Panic - FPU_UNAVAILABLE");
            POST.Add(0x8A, "Panic - DECREMENTER");
            POST.Add(0x8B, "Panic - HYPERVISOR_DECREMENTER");
            POST.Add(0x8C, "Panic - SYSTEM_CALL");
            POST.Add(0x8D, "Panic - TRACE");
            POST.Add(0x8E, "Panic - VPU_UNAVAILABLE");
            POST.Add(0x8F, "Panic - MAINTENANCE");
            POST.Add(0x90, "Panic - VMX_ASSIST");
            POST.Add(0x91, "Panic - THERMAL_MANAGEMENT");
            //1BL Errors - 0x92 - 0x98
            POST.Add(0x92, "Panic - 1BL is executed on wrong CPU thread (panic)");
            POST.Add(0x93, "Panic - TOO_MANY_CORES");
            POST.Add(0x94, "Panic - VERIFY_OFFSET");
            POST.Add(0x95, "Panic - VERIFY_HEADER");
            POST.Add(0x96, "Panic - SIG_VERIFY");
            POST.Add(0x97, "Panic - NONHOST_RESUME_STATUS");
            POST.Add(0x98, "Panic - NEXT_STAGE_SIZE");
            //CB_A 0xD0 -0xDB
            POST.Add(0xD0, "CB_A entry point reached");
            POST.Add(0xD1, "READ_FUSES");
            POST.Add(0xD2, "VERIFY_OFFSET_CB_B");
            POST.Add(0xD3, "FETCH_HEADER_CB_B");
            POST.Add(0xD4, "VERIFY_HEADER_CB_B");
            POST.Add(0xD5, "FETCH_CONTENTS_CB_B");
            POST.Add(0xD6, "HMACSHA_COMPUTE_CB_B");
            POST.Add(0xD7, "RC4_INITIALIZE_CB_B");
            POST.Add(0xD8, "RC4_DECRYPT_CB_B");
            POST.Add(0xD9, "SHA_COMPUTE_CB_B");
            POST.Add(0xDA, "SHA_VERIFY_CB_B");
            POST.Add(0xDB, "BRANCH_CB_B");
            //CB_A Errors 0xF0- 0xF3
            POST.Add(0xF0, "Panic - VERIFY_OFFSET_CB_B");
            POST.Add(0xF1, "Panic - VERIFY_HEADER_CB_B");
            POST.Add(0xF2, "Panic - SHA_VERIFY_CB_B");
            POST.Add(0xF3, "Panic - ENTRY_SIZE_INVALID_CB_B");
            //CB 0x20-3B
            POST.Add(0x20, "CB entry point reached");
            POST.Add(0x21, "INIT_SECOTP");
            POST.Add(0x22, "INIT_SECENG");
            POST.Add(0x23, "INIT_SYSRAM");
            POST.Add(0x24, "VERIFY_OFFSET_3BL_CC");
            POST.Add(0x25, "LOCATE_3BL_CC");
            POST.Add(0x26, "FETCH_HEADER_3BL_CC");
            POST.Add(0x27, "VERIFY_HEADER_3BL_CC");
            POST.Add(0x28, "FETCH_CONTENTS_3BL_CC");
            POST.Add(0x29, "HMACSHA_COMPUTE_3BL_CC");
            POST.Add(0x2A, "RC4_INITIALIZE_3BL_CC");
            POST.Add(0x2B, "RC4_DECRYPT_3BL_CC");
            POST.Add(0x2C, "SHA_COMPUTE_3BL_CC");
            POST.Add(0x2D, "SIG_VERIFY_3BL_CC");
            POST.Add(0x2E, "HWINIT");
            POST.Add(0x2F, "RELOCATE");
            POST.Add(0x30, "VERIFY_OFFSET_4BL_CD");
            POST.Add(0x31, "FETCH_HEADER_4BL_CD");
            POST.Add(0x32, "VERIFY_HEADER_4BL_CD");
            POST.Add(0x33, "FETCH_CONTENTS_4BL_CD");
            POST.Add(0x34, "HMACSHA_COMPUTE_4BL_CD");
            POST.Add(0x35, "RC4_INITIALIZE_4BL_CD");
            POST.Add(0x36, "RC4_DECRYPT_4BL_CD");
            POST.Add(0x37, "SHA_COMPUTE_4BL_CD");
            POST.Add(0x38, "SIG_VERIFY_4BL_CD");
            POST.Add(0x39, "SHA_VERIFY_4BL_CD");
            POST.Add(0x3A, "BRANCH");
            POST.Add(0x3B, "PCI_INIT");
            //2BL errors  0x9B-0xB0
            POST.Add(0x9B, "Panic - VERIFY_SECOTP_1");
            POST.Add(0x9C, "Panic - VERIFY_SECOTP_2");
            POST.Add(0x9D, "Panic - VERIFY_SECOTP_3");
            POST.Add(0x9E, "Panic - Panic - VERIFY_SECOTP_4");
            POST.Add(0x9F, "Panic - VERIFY_SECOTP_5");
            POST.Add(0xA0, "Panic - VERIFY_SECOTP_6");
            POST.Add(0xA1, "Panic - VERIFY_SECOTP_7");
            POST.Add(0xA2, "Panic - VERIFY_SECOTP_8");
            POST.Add(0xA3, "Panic - VERIFY_SECOTP_9");
            POST.Add(0xA4, "Panic - VERIFY_SECOTP_10");
            POST.Add(0xA5, "Panic - VERIFY_OFFSET_3BL_CC");
            POST.Add(0xA6, "Panic - LOCATE_3BL_CC");
            POST.Add(0xA7, "Panic - VERIFY_HEADER_3BL_CC");
            POST.Add(0xA8, "Panic - SIG_VERIFY_3BL_CC");
            POST.Add(0xA9, "Panic - HWINIT");
            POST.Add(0xAA, "Panic - VERIFY_OFFSET_4BL_CD");
            POST.Add(0xAB, "Panic - VERIFY_HEADER_4BL_CD");
            POST.Add(0xAC, "Panic - SIG_VERIFY_4BL_CD");
            POST.Add(0xAD, "Panic - SHA_VERIFY_4BL_CD");
            POST.Add(0xAE, "Panic - UNEXPECTED_INTERRUPT");
            POST.Add(0xAF, "Panic - UNSUPPORTED_RAM_SIZE");
            POST.Add(0xB0, "Panic - VERIFY_CONSOLE_TYPE");
            //4BL 0x40-0x53
            POST.Add(0x40, "Entrypoint of CD reached");
            POST.Add(0x41, "VERIFY_OFFSET");
            POST.Add(0x42, "FETCH_HEADER");
            POST.Add(0x43, "VERIFY_HEADER");
            POST.Add(0x44, "FETCH_CONTENTS");
            POST.Add(0x45, "HMACSHA_COMPUTE");
            POST.Add(0x46, "RC4_INITIALIZE");
            POST.Add(0x47, "RC4_DECRYPT");
            POST.Add(0x48, "SHA_COMPUTE");
            POST.Add(0x49, "SHA_VERIFY");
            POST.Add(0x4A, "LOAD_6BL_CF");
            POST.Add(0x4B, "LZX_EXPAND");
            POST.Add(0x4C, "SWEEP_CACHES");
            POST.Add(0x4D, "DECODE_FUSES");
            POST.Add(0x4E, "FETCH_OFFSET_6BL_CF");
            POST.Add(0x4F, "VERIFY_OFFSET_6BL_CF");
            POST.Add(0x50, "LOAD_UPDATE_1");
            POST.Add(0x51, "LOAD_UPDATE_2");
            POST.Add(0x52, "BRANCH");
            POST.Add(0x53, "DECRYT_VERIFY_HV_CERT");
            //4BL Errors 0xB1 - 0xB7
            POST.Add(0xB1, "Panic - VERIFY_OFFSET");
            POST.Add(0xB2, "Panic - VERIFY_HEADER");
            POST.Add(0xB3, "Panic - SHA_VERIFY");
            POST.Add(0xB4, "Panic - LZX_EXPAND");
            POST.Add(0xB5, "Panic - VERIFY_OFFSET_6BL");
            POST.Add(0xB6, "Panic - DECODE_FUSES");
            POST.Add(0xB7, "Panic - UPDATE_MISSING");
            //6BL 0xC1 - 0xC8
            POST.Add(0xC1, "LZX_EXPAND_1");
            POST.Add(0xC2, "LZX_EXPAND_2");
            POST.Add(0xC3, "LZX_EXPAND_3");
            POST.Add(0xC4, "LZX_EXPAND_4");
            POST.Add(0xC5, "LZX_EXPAND_5");
            POST.Add(0xC6, "LZX_EXPAND_6");
            POST.Add(0xC7, "LZX_EXPAND_7");
            POST.Add(0xC8, "SHA_VERIFY");
            //HV 0x58-0x5F - 0xFF
            POST.Add(0x58, "INIT_HYPERVISOR");
            POST.Add(0x59, "INIT_SOC_MMIO");
            POST.Add(0x5A, "INIT_XEX_TRAINING");
            POST.Add(0x5B, "INIT_KEYRING");
            POST.Add(0x5C, "INIT_KEYS");
            POST.Add(0x5D, "INIT_SOC_INT");
            POST.Add(0x5E, "INIT_SOC_INT_COMPLETE");
            POST.Add(0xFF, "FATAL");
            // Kernel 0x60-79
            POST.Add(0x60, "INIT_KERNEL");
            POST.Add(0x61, "INIT_HAL_PHASE_0");
            POST.Add(0x62, "INIT_PROCESS_OBJECTS");
            POST.Add(0x63, "INIT_KERNEL_DEBUGGER");
            POST.Add(0x64, "INIT_MEMORY_MANAGER");
            POST.Add(0x65, "INIT_STACKS");
            POST.Add(0x66, "INIT_OBJECT_SYSTEM");
            POST.Add(0x67, "INIT_PHASE1_THREAD");
            POST.Add(0x68, "Started phase 1 Initialization + INIT_PROCESSORS");
            POST.Add(0x69, "INIT_KEY_VAULT");
            POST.Add(0x6A, "INIT_HAL_PHASE_1");
            POST.Add(0x6B, "INIT_SFC_DRIVER");
            POST.Add(0x6C, "INIT_SECURITY");
            POST.Add(0x6D, "INIT_KEY_EX_VAULT");
            POST.Add(0x6E, "INIT_SETTINGS");
            POST.Add(0x6F, "INIT_POWER_MODE");
            POST.Add(0x70, "INIT_VIDEO_DRIVER");
            POST.Add(0x71, "INIT_AUDIO_DRIVER");
            POST.Add(0x72, "INIT_BOOT_ANIMATION + XMADecoder & XAudioRender Init");
            POST.Add(0x73, "INIT_SATA_DRIVER");
            POST.Add(0x74, "INIT_SHADOWBOOT");
            POST.Add(0x75, "INIT_DUMP_SYSTEM");
            POST.Add(0x76, "INIT_SYSTEM_ROOT");
            POST.Add(0x77, "INIT_OTHER_DRIVERS");
            POST.Add(0x78, "INIT_STFS_DRIVER");
            POST.Add(0x79, "LOAD_XAM");

            POST.Add(0xB8, "Panic - CF auth failed");

        }

        #endregion

        public void analyzeflashconfig(int flashconfig)
        {
            XConfig x = new XConfig(flashconfig);
            x.printConfig();
        }

        class XConfig
        {
            int config;
            int controllertype;
            int blocktype;
            int pagesz;
            int metasz;
            int blocksz;
            int sizeblocks;
            int fsblocks;
            int metatype;
            int sizesmallblocks;
            int blocksperlittle;
            string msg = "";

            public XConfig(int config)
            {
                this.config = config;
                this.controllertype = config >> 17 & 3;
                this.blocktype = config >> 4 & 3;

                this.pagesz = 0x200;
                this.metasz = 0x10;
                this.metatype = 0;
                this.blocksz = 0;
                this.sizeblocks = 0;
                this.sizesmallblocks = 0;
                this.fsblocks = 0;
                int ctype = this.controllertype;
                int btype = this.blocktype;
                if (ctype == 0)
                {
                    this.metatype = 0;
                    this.blocksz = 0x20;
                    if (btype == 0)
                    {
                        msg = "nand type 0:0 is invalid";
                    }
                    else if (btype == 1)
                    {
                        this.sizeblocks = 0x400;
                        this.fsblocks = 0x3E0;
                    }
                    else if (btype == 2)
                    {
                        this.sizeblocks = 0x800;
                        this.fsblocks = 0x7C0;
                    }
                    else if (btype == 3)
                    {
                        this.sizeblocks = 0x1000;
                        this.fsblocks = 0xF80;
                    }
                }
                else if (ctype == 1 && btype == 0)
                {
                    msg = "nand type 1:0 is invalid";
                }
                else if ((ctype == 1 || ctype == 2) && (btype == 0 || btype == 1))
                {
                    this.metatype = 1;
                    this.blocksz = 0x20;
                    if (btype == 0 || (btype == 1 && ctype == 1))
                    {
                        this.sizeblocks = 0x400;
                        this.fsblocks = 0x3E0;
                    }
                    else if (ctype == 2 && btype == 1)
                    {
                        this.sizeblocks = 0x1000;
                        this.fsblocks = 0xF80;
                    }
                }
                else if ((ctype == 1 || ctype == 2) && (btype == 2 || btype == 3))
                {
                    this.metatype = 2;
                    if (btype == 2)
                    {
                        this.blocksz = 0x100;
                        this.sizeblocks = 1 << ((config >> 19 & 3) + (config >> 21 & 15) + 23) >> 17;
                        this.fsblocks = 0x1E0;
                    }
                    else if (btype == 3)
                    {
                        this.blocksz = 0x200;
                        this.sizeblocks = 1 << ((config >> 19 & 3) + (config >> 21 & 15) + 23) >> 18;
                        this.fsblocks = 0xF0;
                    }
                }
                else
                {
                    msg = String.Format("controller type {0} is invalid", ctype);
                }

                this.sizesmallblocks = this.sizeblocks * (this.blocksz / 0x20);
                this.blocksperlittle = this.blocksz / 0x20;
            }

            public void printConfig()
            {
                string fmt = String.Format("FlashConfig: 0x{0:X}\nPageSize: 0x{1:X}\nMetaSize: 0x{2:X}\nMetaType: 0x{3:X}\nBlockSize: 0x{4:X}\nSizeInBlocks: 0x{5:X}\nSizeInSBlocks: 0x{6:X}\nFileBlocks: 0x{7:X}",
                    this.config,
                   this.pagesz,
                   this.metasz,
                   this.metatype,
                   this.blocksz,
                   this.sizeblocks,
                   this.sizesmallblocks,
                   this.fsblocks
           );
                if (String.IsNullOrWhiteSpace(msg)) Console.WriteLine(fmt);
                else Console.WriteLine(msg);
            }
        }

    }
}
/*
//Status Register bitmasks
#define STATUS_ILL_LOG      	0x800u			//Illegal Logical Access
#define STATUS_PIN_WP_N     	0x400u			//NAND Not Write Protected
#define STATUS_PIN_BY_N     	0x200u			//NAND Not Busy
#define STATUS_INT_CP       	0x100u			//Interrupt
#define STATUS_ADDR_ER      	0x80u			//Address Alignment Error
#define STATUS_BB_ER        	0x40u			//Bad Block Error
#define STATUS_RNP_ER       	0x20u			//Logical Replacement not found
#define STATUS_ECC_ER       	0x1Cu			//ECC Error, 3 bits, need to determine each
#define STATUS_WR_ER        	0x2u			//Write or Erase Error
#define STATUS_BUSY         	0x1u			//Busy
#define STATUS_ERROR			(STATUS_ILL_LOG|STATUS_ADDR_ER|STATUS_BB_ER|STATUS_RNP_ER|STATUS_ECC_ER|STATUS_WR_ER)
*/