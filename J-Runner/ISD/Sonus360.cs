using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using LibUsbDotNet;
using LibUsbDotNet.Main;

namespace JRunner
{
    public class Sonus360 : IISD
    {
        public delegate void updateProgress_t(int progress);
        public event updateProgress_t UpdateProgress;

        public delegate void log_t(string text);
        public event log_t log;

        private UsbDevice MyUsbDevice;
        private UsbDeviceFinder JRunner = new UsbDeviceFinder(0x11d4, 0x8338);

        public int Open()
        {
            MyUsbDevice = UsbDevice.OpenUsbDevice(JRunner);
            if (MyUsbDevice == null)
            {
                if (variables.debugme)
                    Console.WriteLine("null device...");
                return 1;
            }
            IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
            if (!ReferenceEquals(wholeUsbDevice, null))
            {
                wholeUsbDevice.SetConfiguration(1);
                if (variables.debugme)
                       Console.WriteLine("Claiming Interface...");
                wholeUsbDevice.ClaimInterface(0);
                if (variables.debugme)
                    Console.WriteLine("The Interface is ours!");
            }

            return 0;
        }
        public int Close()
        {
            if (MyUsbDevice != null)
            {
                if (MyUsbDevice.IsOpen)
                {
                    IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                    if (!ReferenceEquals(wholeUsbDevice, null))
                    {
                        wholeUsbDevice.ReleaseInterface(0);
                    }
                    MyUsbDevice.Close();
                }
            }
            MyUsbDevice = null;
            UsbDevice.Exit();

            return 0;
        }
        public int PowerUp()
        {
            UsbSetupPacket packet = new UsbSetupPacket();
            UsbSetupPacket packetread = new UsbSetupPacket();

            byte[] readBuffer = new byte[8];
            byte buf = 0x10;
            int transfer = 0x08;

            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x30;
            packet.RequestType = (byte)UsbRequestType.TypeVendor;

            packetread.Value = 0x00;
            packetread.Index = 0x00;
            packetread.Length = 0x8;
            packetread.Request = 0x31;
            packetread.RequestType = 0xC0;

            ///Access Flash
            MyUsbDevice.ControlTransfer(ref packet, buf, 1, out transfer);
            if (variables.debugme)
                Console.WriteLine("Length Transferred: {0}", transfer);

            MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
            if (variables.debugme)
                Console.WriteLine("Length Transferred: {0}", transfer);
            if (variables.debugme)
                Console.WriteLine("Status: 0x" + readBuffer[0] + "\n");

            Thread.Sleep(50);

            if (readBuffer[0] == 0x80 || readBuffer[0] == 0x60)
                return 1;

            return 0;
        }

        public int GetID()
        {
            UsbSetupPacket packet = new UsbSetupPacket();
            UsbSetupPacket packetread = new UsbSetupPacket();

            byte[] readBuffer = new byte[8];
            byte buf = 0x48;
            int transfer = 0x08;

            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x30;
            packet.RequestType = (byte)UsbRequestType.TypeVendor;

            packetread.Value = 0x00;
            packetread.Index = 0x00;
            packetread.Length = 0x8;
            packetread.Request = 0x31;
            packetread.RequestType = 0xC0;

            byte[] ID = { 0x03, 0xEF, 0x20, 0x01 };
            byte[] ID_2 = { 0x03, 0xEF, 0x20, 0x10 };
            byte[] ID_3 = { 0x03, 0xEF, 0x20, 0x11 };

            MyUsbDevice.ControlTransfer(ref packet, buf, 5, out transfer);
            if (variables.debugme)
                Console.WriteLine("Length Transferred: {0}", transfer);

            MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
            if (variables.debugme)
                Console.WriteLine("Length Transferred: {0}", transfer);

            if (Oper.ByteArrayCompare(ID, Oper.returnportion(readBuffer, 1, 4)))
            {
                return 1;
            }
            else if (Oper.ByteArrayCompare(ID_2, Oper.returnportion(readBuffer, 1, 4)))
            {
                return 2;
            }
            else if (Oper.ByteArrayCompare(ID_3, Oper.returnportion(readBuffer, 1, 4)))
            {
                return 3;
            }

            if (variables.debugme)
                Console.WriteLine("In " + Oper.ByteArrayToString(readBuffer) + "\n");
            return 0;
        }
 
        public void PowerDown()
        {

            UsbSetupPacket packet = new UsbSetupPacket();
            UsbSetupPacket packetread = new UsbSetupPacket();

            byte[] readBuffer = new byte[8];
            byte buf = 0x12;
            int transfer = 0x08;

            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x30;
            packet.RequestType = (byte)UsbRequestType.TypeVendor;

            packetread.Value = 0x00;
            packetread.Index = 0x00;
            packetread.Length = 0x8;
            packetread.Request = 0x31;
            packetread.RequestType = 0xC0;

            MyUsbDevice.ControlTransfer(ref packet, buf, 1, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);

            MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
            if (variables.debugme)
                Console.WriteLine("Length Transferred: {0}", transfer);
            if (variables.debugme)
                Console.WriteLine("Status: 0x" + readBuffer[0] + "\n");

            if (readBuffer[0] == 0x61)
                Thread.Sleep(2000);
        }
        public void Reset()
        {
            UsbSetupPacket packet = new UsbSetupPacket();
            UsbSetupPacket packetread = new UsbSetupPacket();

            byte[] readBuffer = new byte[8];
            byte buf = 0x14;
            int transfer = 0x08;

            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x30;
            packet.RequestType = (byte)UsbRequestType.TypeVendor;

            packetread.Value = 0x00;
            packetread.Index = 0x00;
            packetread.Length = 0x8;
            packetread.Request = 0x31;
            packetread.RequestType = 0xC0;

            MyUsbDevice.ControlTransfer(ref packet, buf, 1, out transfer);
            if (variables.debugme)
                Console.WriteLine("Length Transferred: {0}", transfer);

            MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
            if (variables.debugme)
                Console.WriteLine("Length Transferred: {0}", transfer);
            if (variables.debugme)
                Console.WriteLine("Status: 0x" + readBuffer[0] + "\n");
        }

        public int ISD_Read_Flash(string filename)
        {
            int ret = 1;
            UsbSetupPacket packet = new UsbSetupPacket();
            UsbSetupPacket packetread = new UsbSetupPacket();

            byte[] readBuffer = new byte[8];
            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x30;
            packet.RequestType = (byte)UsbRequestType.TypeVendor;

            packetread.Value = 0x00;
            packetread.Index = 0x00;
            packetread.Length = 0x8;
            packetread.Request = 0x31;
            packetread.RequestType = 0xC0;
            int transfer = 0x08;

            try
            {
                File.Delete(filename);
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); };

            BinaryWriter sw = new BinaryWriter(File.Open(filename, FileMode.Append, FileAccess.Write));

            byte[] RES = { 0x60, 0x60, 0x60, 0x60 };
            byte[] CMD = new byte[8];

            int fails = 0;
            CMD[0] = 0xA2;
            for (int i = 0; i < 0xB000; i += 4)
            {
                if (variables.escapeloop) break;
                UpdateProgress((i * 100) / 0xB000);

                if ((i % 0x400 == 0)) Thread.Sleep(20);

                CMD[3] = (byte)(i & 0x00ff);
                CMD[2] = (byte)((i & 0xff00) >> 8);

                MyUsbDevice.ControlTransfer(ref packet, CMD, 8, out transfer);
                Thread.Sleep(variables.delay);

                MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
                if (Oper.ByteArrayCompare(readBuffer, RES, 4))
                {
                    sw.Write(Oper.returnportion(readBuffer, 4, 4));
                    fails = 0;
                }
                else
                {
                    if (fails == 6)
                    {
                        ret = 0;
                        break;
                    }
                    i -= 4;
                    Console.Write("X");
                    fails++;
                }
            }
            UpdateProgress(100);
            sw.Close();
            return ret;
        }
        public int ISD_Erase_Flash()
        {
            UsbSetupPacket packet = new UsbSetupPacket();
            UsbSetupPacket packetread = new UsbSetupPacket();

            byte[] readBuffer = new byte[8];
            byte[] buf = { 0x26, 0x01 };
            int transfer = 0x08;

            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x30;
            packet.RequestType = (byte)UsbRequestType.TypeVendor;

            packetread.Value = 0x00;
            packetread.Index = 0x00;
            packetread.Length = 0x8;
            packetread.Request = 0x31;
            packetread.RequestType = 0xC0;

            ///Access Flash
            MyUsbDevice.ControlTransfer(ref packet, buf, 2, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
            Thread.Sleep(variables.delay);
            MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
            //Console.WriteLine("Status: 0x{0:X}", readBuffer[0]);

            if (readBuffer[0] != 0x60 && readBuffer[0] != 0x40)
                return 0;

            return 1;
        }
        public void ISD_Write_Flash(string filename)
        {
            long filesize;
            FileInfo fl = new FileInfo(filename);
            filesize = fl.Length;
            if (!File.Exists(filename))
            {
                log("Image file not found\n");
                return;
            }
            if (filesize != 0xB000)
            {
                log("Image file must be 44Kb\n");
                return;
            }
            try
            {
                BinaryReader rw = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));

                log("Writing to Flash.." + filename + "\n");

                UsbSetupPacket packet = new UsbSetupPacket();
                UsbSetupPacket packetread = new UsbSetupPacket();
                //ErrorCode ec = ErrorCode.None;

                byte[] readBuffer = new byte[8];
                //Thread.Sleep(variables.delay);
                packet.Value = 0x00;
                packet.Index = 0x00;
                packet.Length = 0x8;
                packet.Request = 0x30;
                packet.RequestType = (byte)UsbRequestType.TypeVendor;

                packetread.Value = 0x00;
                packetread.Index = 0x00;
                packetread.Length = 0x8;
                packetread.Request = 0x31;
                packetread.RequestType = 0xC0;
                int transfer = 0x08;
                int AddPause = 0;
                int stayPause = 0;
                byte[] RES = { 0x60, 0x60, 0x60, 0x60, 0x60, 0x60, 0x60, 0x60 };
                byte[] CMD = new byte[8];
                CMD[0] = 0xA0;

                for (int i = 0; i < 0xB000; i += 4)
                {
                    if (variables.escapeloop)
                        break;
                    UpdateProgress((i * 100) / 0xB000);
                    if (AddPause != 0) stayPause++;
                    AddPause = 0;

                    byte[] buffer = rw.ReadBytes(0x4);
                    CMD[4] = 0x00; CMD[5] = 0x00; CMD[6] = 0x00; CMD[7] = 0x00;

                    Buffer.BlockCopy(buffer, 0, CMD, 4, buffer.Length);

                    CMD[3] = (byte)(i & 0x00ff);
                    CMD[2] = (byte)((i & 0xff00) >> 8);

                    MyUsbDevice.ControlTransfer(ref packet, CMD, 8, out transfer);
                    Thread.Sleep(variables.delay + stayPause);
                    MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
                    while (!Oper.ByteArrayCompare(readBuffer, RES, 5))
                    {
                        AddPause += 2;
                        Thread.Sleep(variables.delay + AddPause + stayPause);
                        MyUsbDevice.ControlTransfer(ref packet, CMD, 8, out transfer);
                        Thread.Sleep(variables.delay + AddPause + stayPause);
                        MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
                        log("byte mismatch, retrying..\n");
                        if (variables.debugme)
                            Console.WriteLine("Slowing down...");
                        if (variables.debugme)
                            Console.WriteLine("additional delay now set to :" + AddPause);

                        if (AddPause > 16)
                        {
                            log("Write Failed\n");
                            break;
                        }
                    }
                }

                if (stayPause > 0) Console.WriteLine("Consider changing delay in setting page to : " + (variables.delay + 1));
                UpdateProgress(100);
                rw.Close();
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
        }
        public Boolean ISD_Verify_Flash(string filename)
        {
            long filesize;
            FileInfo fl = new FileInfo(filename);
            filesize = fl.Length;
            if (!File.Exists(filename))
            {
                log("Image file not found\n");
                return false;
            }
            if (filesize != 0xB000)
            {
                log("Image file must be 44Kb\n");
                return false;
            }
            BinaryReader rw = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));
            byte[] file = rw.ReadBytes(0xB000);
            rw.Close();
            log("Verifying with Flash..\n");

            UsbSetupPacket packet = new UsbSetupPacket();
            UsbSetupPacket packetread = new UsbSetupPacket();
            //ErrorCode ec = ErrorCode.None;

            byte[] readBuffer = new byte[8];
            byte[] data = new byte[0xB000];
            //Thread.Sleep(variables.delay);
            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x30;
            packet.RequestType = (byte)UsbRequestType.TypeVendor;

            packetread.Value = 0x00;
            packetread.Index = 0x00;
            packetread.Length = 0x8;
            packetread.Request = 0x31;
            packetread.RequestType = 0xC0;
            int transfer = 0x08;

            byte[] RES = { 0x60, 0x60, 0x60, 0x60 };
            byte[] CMD = new byte[8];

            int fails = 0;
            CMD[0] = 0xA2;
            for (int i = 0; i < 0xB000; i += 4)
            {
                if (variables.escapeloop)
                    break;
                UpdateProgress((i * 100) / 0xB000);

                CMD[3] = (byte)(i & 0x00ff);
                CMD[2] = (byte)((i & 0xff00) >> 8);

                fails = 0;
                do
                {
                    if (fails == 1) Console.Write("x");
                    MyUsbDevice.ControlTransfer(ref packet, CMD, 8, out transfer);
                    Thread.Sleep(variables.delay + fails);
                    MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);

                    Buffer.BlockCopy(readBuffer, 4, data, i, 4);

                    if (fails == 6)
                    {
                        break;
                    }
                    fails++;
                } while (!Oper.ByteArrayCompare(readBuffer, RES, 4));
            }
            UpdateProgress(100);

            return Oper.ByteArrayCompare(file, data);
        }

        public void ISD_Play(UInt16 index)
        {
            UsbSetupPacket packet = new UsbSetupPacket();

            int transfer = 0x08;

            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x30;
            packet.RequestType = (byte)UsbRequestType.TypeVendor;

            byte[] buf = new byte[3];
            buf[0] = 0xA6;
            buf[3] = (byte)(index & 0x00ff);
            buf[2] = (byte)((index & 0xff00) >> 8);

            ///Access Flash
            MyUsbDevice.ControlTransfer(ref packet, buf, 3, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
        }

        public void ISD_Exec(UInt16 index)
        {
            UsbSetupPacket packet = new UsbSetupPacket();

            int transfer = 0x08;

            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x30;
            packet.RequestType = (byte)UsbRequestType.TypeVendor;

            byte[] buf = new byte[3];
            buf[0] = 0xB0;
            buf[3] = (byte)(index & 0x00ff);
            buf[2] = (byte)((index & 0xff00) >> 8);

            ///Access Flash
            MyUsbDevice.ControlTransfer(ref packet, buf, 3, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
        }
    }
}
