using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace JRunner
{
    public class PicoFlasher
    {
        public bool InUse = false;

        enum COMMANDS : byte
        {
            GET_VERSION = 0x00,
            GET_FLASH_CONFIG = 0x01,
            READ_FLASH = 0x02,
            WRITE_FLASH = 0x03,
            ISD1200_INIT = 0x04,
            ISD1200_DEINIT = 0x05,
            ISD1200_READ_ID = 0x06,
            ISD1200_READ_FLASH = 0x07,
            ISD1200_ERASE_FLASH = 0x08,
            ISD1200_WRITE_FLASH = 0x09,
            ISD1200_PLAY_VOICE = 0x10,
            READ_FLASH_STREAM = 0x11,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct CMD
        {
            public COMMANDS cmd;
            public UInt32 lba;
        }

        static List<string> ComPortNames(String VID, String PID)
        {
            String pattern = String.Format("^VID_{0}.PID_{1}", VID, PID);
            Regex _rx = new Regex(pattern, RegexOptions.IgnoreCase);
            List<string> comports = new List<string>();

            RegistryKey rk1 = Registry.LocalMachine;
            RegistryKey rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");

            foreach (String s3 in rk2.GetSubKeyNames())
            {
                RegistryKey rk3 = rk2.OpenSubKey(s3);
                foreach (String s in rk3.GetSubKeyNames())
                {
                    if (_rx.Match(s).Success)
                    {
                        RegistryKey rk4 = rk3.OpenSubKey(s);
                        foreach (String s2 in rk4.GetSubKeyNames())
                        {
                            RegistryKey rk5 = rk4.OpenSubKey(s2);
                            string location = (string)rk5.GetValue("LocationInformation");
                            RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                            string portName = (string)rk6.GetValue("PortName");
                            if (!String.IsNullOrEmpty(portName) && SerialPort.GetPortNames().Contains(portName))
                                comports.Add((string)rk6.GetValue("PortName"));
                        }
                    }
                }
            }
            return comports;
        }

        private SerialPort OpenSerial()
        {
            if (InUse)
                return null;
            List<string> ports = ComPortNames("600D", "7001");

            if (ports.Count <= 0)
            {
                MessageBox.Show("Can't find PicoFlasher com port.\r\nUpdate the PicoFlasher firmware and check your drivers!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            SerialPort serial = new SerialPort();

            serial.PortName = ports[0];

            serial.ReadTimeout = 5000;
            serial.WriteTimeout = 5000;

            serial.Open();

            CMD cmd = new CMD();
            cmd.cmd = COMMANDS.GET_VERSION;
            cmd.lba = 0;

            SendCmd(serial, cmd);

            UInt32 version = RecvUInt32(serial);

            if (version != 1)
            {
                serial.Close();
                MessageBox.Show("Update the PicoFlasher firmware!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            InUse = true;

            return serial;
        }

        private void CloseSerial(SerialPort serial)
        {
            serial.Close();

            InUse = false;
        }

        private void SendCmd(SerialPort serial, CMD cmd)
        {
            int size = Marshal.SizeOf(cmd);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(cmd, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            serial.Write(arr, 0, arr.Length);
        }

        private UInt32 RecvUInt32(SerialPort serial)
        {
            byte[] rxbuffer = new byte[4];
            int got = 0;
            while (got < 4)
                got += serial.Read(rxbuffer, got, rxbuffer.Length - got);

            return BitConverter.ToUInt32(rxbuffer, 0);
        }

        private byte RecvUInt8(SerialPort serial)
        {
            byte[] rxbuffer = new byte[1];
            int got = 0;
            while (got < 1)
                got += serial.Read(rxbuffer, got, rxbuffer.Length - got);

            return rxbuffer[0];
        }

        private uint getFlashSize(SerialPort serial)
        {
            CMD cmd = new CMD();
            cmd.cmd = COMMANDS.GET_FLASH_CONFIG;
            cmd.lba = 0;

            SendCmd(serial, cmd);

            UInt32 flashconfig = RecvUInt32(serial);

            if (flashconfig == 0x00000000 || flashconfig == 0xFFFFFFFF)
            {
                Console.WriteLine("Console Not Found");
                Console.WriteLine("");
                return 0;
            }

            uint flashsize = 0;

            if (flashconfig == 0x23010 /* Jasper 16MB, Trinity */ || flashconfig == 0x43000 /* Corona */ || flashconfig == 0x1198010 /* Xenon, Zephyr, Falcon */)
            {
                flashsize = 16 * 1024 * 1024;
            }
            else if (flashconfig == 0x1198030 /* Xenon, Zephyr, Falcon */)
            {
                flashsize = 64 * 1024 * 1024;
            }
            else if (flashconfig == 0x8A3020 /* Jasper 256MB */)
            {
                flashsize = 256 * 1024 * 1024;
            }
            else if (flashconfig == 0xAA3020 /* Jasper 512MB */)
            {
                flashsize = 512 * 1024 * 1024;
            }

            return flashsize;
        }

        public void getFlashConfig()
        {
            SerialPort serial = OpenSerial();
            if (serial == null)
                return;

            CMD cmd = new CMD();
            cmd.cmd = COMMANDS.GET_FLASH_CONFIG;
            cmd.lba = 0;

            SendCmd(serial, cmd);

            UInt32 flashconfig = RecvUInt32(serial);

            if (flashconfig == 0x00000000 || flashconfig == 0xFFFFFFFF)
            {
                Console.WriteLine("Console Not Found");
                Console.WriteLine("");
                CloseSerial(serial);
                return;
            }

            if (flashconfig == 0x01198010 || flashconfig == 0x01198030)
                Console.WriteLine("Xenon, Zephyr, Falcon");
            else if (flashconfig == 0x00023010)
                Console.WriteLine("Jasper 16MB, Trinity");
            else if (flashconfig == 0x00043000)
                Console.WriteLine("Corona 16MB");
            else if (flashconfig == 0x008A3020)
                Console.WriteLine("Jasper 256MB");
            else if (flashconfig == 0x00AA3020)
                Console.WriteLine("Jasper 512MB");
            else if (flashconfig == 0xC0462002)
                Console.WriteLine("Corona 4GB");
            else
                Console.WriteLine("Unrecongized Flash Config: " + flashconfig.ToString("X8"));
            Console.WriteLine("");

            CloseSerial(serial);
        }
        public void ReadNand(int iterations)
        {
            Thread readerThread = new Thread(() =>
            {
                SerialPort serial = OpenSerial();
                if (serial == null)
                    return;

                uint flashsize = getFlashSize(serial);
                if (flashsize == 0)
                {
                    Console.WriteLine("Unknown flash size!");
                    Console.WriteLine("");
                    CloseSerial(serial);
                    return;
                }

                for (int i = 0; i < iterations; i++)
                {
                    variables.filename = variables.outfolder + "\\nanddump" + (i + 1) + ".bin";
                    if (File.Exists(variables.filename))
                    {
                        if (DialogResult.Cancel == MessageBox.Show("File already exists, it will be DELETED! Press OK to continue", "About to overwrite a nanddump", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                        {
                            Console.WriteLine("Cancelled");
                            Console.WriteLine("");
                            return;
                        };
                    }

                    MainForm.mainForm.PicoFlasherBusy(1);
                    
                    BinaryWriter bw = new BinaryWriter(File.Open(variables.filename, FileMode.Append, FileAccess.Write));

                    /*
                    for (uint j = 0; j < flashsize / 512; j++)
                    {
                        CMD cmd = new CMD();
                        cmd.cmd = COMMANDS.READ_FLASH;
                        cmd.lba = j;

                        SendCmd(serial, cmd);

                        UInt32 ret = RecvUInt32(serial);

                        if (ret != 0)
                        {
                            Console.WriteLine("Error: " + ret.ToString("X"));
                            Console.WriteLine("");
                            break;
                        }

                        byte[] rxbuffer = new byte[0x210];
                        int got = 0;
                        while (got < 4)
                            got += serial.Read(rxbuffer, got, rxbuffer.Length - got);

                        bw.Write(rxbuffer);

                        if (j % 0x20 == 0)
                            MainForm.mainForm.PicoFlasherBlocksUpdate((j / 0x20).ToString("X"), (int)((j * 100) / (flashsize / 512)));
                    }
                    */

                    CMD cmd = new CMD();
                    cmd.cmd = COMMANDS.READ_FLASH_STREAM;
                    cmd.lba = flashsize / 512;

                    SendCmd(serial, cmd);

                    for (uint j = 0; j < flashsize / 512; j++)
                    {
                        UInt32 ret = RecvUInt32(serial);

                        if (ret != 0)
                        {
                            Console.WriteLine("Error: " + ret.ToString("X"));
                            Console.WriteLine("");
                            break;
                        }

                        byte[] rxbuffer = new byte[0x210];
                        int got = 0;
                        while (got < 4)
                            got += serial.Read(rxbuffer, got, rxbuffer.Length - got);

                        bw.Write(rxbuffer);

                        if (j % 0x20 == 0)
                            MainForm.mainForm.PicoFlasherBlocksUpdate((j / 0x20).ToString("X"), (int)((j * 100) / (flashsize / 512)));
                    }


                    bw.Close();

                    MainForm.mainForm.PicoFlasherBusy(0);

                    MainForm.mainForm.PicoFlasherInitNand(i);
                }

                CloseSerial(serial);
            });
            readerThread.Start();
        }
        public void WriteNand(int fixEcc)
        {
            if (String.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;

            if (fixEcc == 0)
            {
                if (Path.GetExtension(variables.filename1) == ".ecc")
                {
                    Console.WriteLine("xFlasher: You need an .bin image");
                    return;
                }
            }
            else
            {
                if (Path.GetExtension(variables.filename1) != ".ecc")
                {
                    Console.WriteLine("xFlasher: You need an .ecc image");
                    return;
                }
            }

            Thread writerThread = new Thread(() =>
            {
                SerialPort serial = OpenSerial();
                if (serial == null)
                    return;

                uint flashsize = getFlashSize(serial);
                if (flashsize == 0)
                {
                    Console.WriteLine("Unknown flash size!");
                    Console.WriteLine("");
                    CloseSerial(serial);
                    return;
                }

                MainForm.mainForm.PicoFlasherBusy(2);

                BinaryReader br = new BinaryReader(File.Open(variables.filename1, FileMode.Open, FileAccess.Read));

                for (uint j = 0; j < flashsize / 512; j++)
                {
                    byte[] read = br.ReadBytes(0x210);
                    if (read == null || read.Length != 0x210)
                        break;

                    // TODO: Implement ECC fixing.

                    CMD cmd = new CMD();
                    cmd.cmd = COMMANDS.WRITE_FLASH;
                    cmd.lba = j;

                    int size = Marshal.SizeOf(cmd) + read.Length;
                    byte[] arr = new byte[size];
                    IntPtr ptr = Marshal.AllocHGlobal(size);
                    Marshal.StructureToPtr(cmd, ptr, true);
                    Marshal.Copy(ptr, arr, 0, size);
                    Marshal.FreeHGlobal(ptr);
                    read.CopyTo(arr, Marshal.SizeOf(cmd));
                    serial.Write(arr, 0, arr.Length);

                    UInt32 ret = RecvUInt32(serial);
                    if (ret != 0)
                    {
                        Console.WriteLine("Error: " + ret.ToString("X"));
                        Console.WriteLine("");
                        break;
                    }

                    if (j % 0x20 == 0)
                        MainForm.mainForm.PicoFlasherBlocksUpdate((j / 0x20).ToString("X"), (int)((j * 100) / (flashsize / 512)));
                }

                br.Close();

                MainForm.mainForm.PicoFlasherBusy(0);

                CloseSerial(serial);

                Console.WriteLine("Write Successful!");
                Console.WriteLine("");

                SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                if (variables.soundsuccess != "") success.SoundLocation = variables.soundsuccess;
                success.Play();
            });
            writerThread.Start();
        }
    }
}
