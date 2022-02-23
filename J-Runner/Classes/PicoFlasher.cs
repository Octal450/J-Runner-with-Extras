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
    public class PicoFlasher : IISD
    {
        public delegate void updateProgress_t(int progress);
        public event updateProgress_t UpdateProgress;

        public delegate void log_t(string text);
        public event log_t log;

        public bool InUse = false;

        enum COMMANDS : byte
        {
            GET_VERSION = 0x00,
            GET_FLASH_CONFIG,
            READ_FLASH,
            WRITE_FLASH,
            READ_FLASH_STREAM,

            ISD1200_INIT = 0xA0,
            ISD1200_DEINIT,
            ISD1200_READ_ID ,
            ISD1200_READ_FLASH,
            ISD1200_ERASE_FLASH,
            ISD1200_WRITE_FLASH,
            ISD1200_PLAY_VOICE,
            ISD1200_EXEC_MACRO,
            ISD1200_RESET,

            REBOOT_TO_BOOTLOADER = 0xFE
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

            List<string> ports = null;
            try
            {
                ports = ComPortNames("600D", "7001");
            }
            catch { } // No crash if it fails

            if (ports.Count <= 0)
            {
                MessageBox.Show("Can't find PicoFlasher COM port\n\nUpdate the PicoFlasher firmware and check your drivers", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (version != 2)
            {
                serial.Close();
                MessageBox.Show("PicoFlasher firmware is too old\n\nUpdate the PicoFlasher firmware to continue", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            while (got < rxbuffer.Length)
                got += serial.Read(rxbuffer, got, rxbuffer.Length - got);

            return BitConverter.ToUInt32(rxbuffer, 0);
        }

        private byte RecvUInt8(SerialPort serial)
        {
            byte[] rxbuffer = new byte[1];
            int got = 0;
            while (got < rxbuffer.Length)
                got += serial.Read(rxbuffer, got, rxbuffer.Length - got);

            return rxbuffer[0];
        }

        private uint getFlashConfig(SerialPort serial)
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

            return flashconfig;
        }

        private uint getFlashSize(uint flash_config)
        {
            uint size = 0;

            uint major = (flash_config >> 17) & 3;
            uint minor = (flash_config >> 4) & 3;
            if (major >= 1)
            {
                if (minor == 0) // Corona 16MB
                {
                    if (((flash_config >> 17) & 0x03) != 0x01)
                        size = 16;
                }
                else if (minor == 1) // Jasper 16MB, Trinity 16MB
                {
                    if (((flash_config >> 17) & 0x03) != 0x01)
                        size = 64;
                    else
                        size = 16;
                }
                else if (minor == 2 || minor == 3) // Jasper 256MB / 512MB, Trinity 512MB (XDK)
                    size = (uint)(8 << (int)(((flash_config >> 19) & 0x3) + ((flash_config >> 21) & 0xF)));
            }
            else // Xenon, Zephyr, Falcon
                size = (uint)(8 << (int)minor);

            return size * 1024 * 1024;
        }

        public void getFlashConfig()
        {
            SerialPort serial = OpenSerial();
            if (serial == null)
                return;

            Console.WriteLine("Checking Console...");
            CMD cmd = new CMD();
            cmd.cmd = COMMANDS.GET_FLASH_CONFIG;
            cmd.lba = 0;

            SendCmd(serial, cmd);

            UInt32 flashconfig = RecvUInt32(serial);
            Console.WriteLine("0x" + flashconfig.ToString("X8"));

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
                Console.WriteLine("Unrecongized Flash Config");
            Console.WriteLine("");

            CloseSerial(serial);
        }

        public void ReadNand(int iterations, uint start = 0, uint end = 0)
        {
            Thread readerThread = new Thread(() =>
            {
                SerialPort serial = OpenSerial();
                if (serial == null)
                    return;

                uint flashconfig = getFlashConfig(serial);
                if (flashconfig == 0)
                {
                    Console.WriteLine("Console Not Found");
                    Console.WriteLine("");
                    CloseSerial(serial);
                    return;
                }

                uint flashsize = getFlashSize(flashconfig);
                if (flashsize == 0)
                {
                    Console.WriteLine("Unknown Flash Size");
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
                    Console.WriteLine("Reading Nand to {0}", variables.filename);

                    BinaryWriter bw = new BinaryWriter(File.Open(variables.filename, FileMode.Create, FileAccess.Write));

                    if (start == 0 && end == 0)
                    {
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
                            while (got < rxbuffer.Length)
                                got += serial.Read(rxbuffer, got, rxbuffer.Length - got);

                            bw.Write(rxbuffer);

                            if (j % 0x20 == 0)
                                MainForm.mainForm.PicoFlasherBlocksUpdate((j / 0x20).ToString("X"), (int)((j * 100) / (flashsize / 512)));
                        }
                    }
                    else
                    {
                        for (uint j = (start * 0x20); j < (end * 0x20); j++)
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
                            while (got < rxbuffer.Length)
                                got += serial.Read(rxbuffer, got, rxbuffer.Length - got);

                            bw.Write(rxbuffer);

                            if (j % 0x20 == 0)
                                MainForm.mainForm.PicoFlasherBlocksUpdate((j / 0x20).ToString("X"), (int)((j * 100) / (flashsize / 512)));
                        }
                    }

                    bw.Close();

                    MainForm.mainForm.PicoFlasherBusy(0);

                    Thread.Sleep(1000);
                    MainForm.mainForm.PicoFlasherInitNand(i);
                }

                CloseSerial(serial);
            });
            readerThread.Start();
        }

        public void WriteNand(int fixEcc, uint start = 0, uint end = 0)
        {
            if (String.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;

            if (fixEcc == 0)
            {
                if (Path.GetExtension(variables.filename1) == ".ecc")
                {
                    Console.WriteLine("You need an .bin image");
                    return;
                }
            }
            else
            {
                if (Path.GetExtension(variables.filename1) != ".ecc")
                {
                    Console.WriteLine("You need an .ecc image");
                    return;
                }
            }

            Thread writerThread = new Thread(() =>
            {
                SerialPort serial = OpenSerial();
                if (serial == null)
                    return;

                uint flashconfig = getFlashConfig(serial);
                if (flashconfig == 0)
                {
                    Console.WriteLine("Console Not Found");
                    Console.WriteLine("");
                    CloseSerial(serial);
                    return;
                }

                uint flashsize = getFlashSize(flashconfig);
                if (flashsize == 0)
                {
                    Console.WriteLine("Unknown Flash Size!");
                    Console.WriteLine("");
                    CloseSerial(serial);
                    return;
                }

                int layout = 1;
                if (flashconfig == 0xAA3020 || flashconfig == 0x8A3020)
                    layout = 2;
                else if (flashconfig == 0x1198010)
                    layout = 0;

                MainForm.mainForm.PicoFlasherBusy(2);
                Console.WriteLine("Writing {0} to Nand", Path.GetFileName(variables.filename1));

                BinaryReader br = new BinaryReader(File.Open(variables.filename1, FileMode.Open, FileAccess.Read));

                uint writeend = flashsize / (512 * 8);
                if (start != 0 || end != 0)
                    writeend = end;
                
                for (uint j = start; j < writeend; j++)
                {
                    byte[] read = br.ReadBytes(0x210 * 8);
                    if (read == null || read.Length % 0x210 != 0)
                        break;

                    if (fixEcc == 1)
                        read = JRunner.Nand.Nand.addecc_v2(read, false, (int)(j * 0x4200), layout);

                    for (uint k = 0; k < read.Length / 0x210; k++)
                    {
                        CMD cmd = new CMD();
                        cmd.cmd = COMMANDS.WRITE_FLASH;
                        cmd.lba = j * 8 + k;

                        int size = Marshal.SizeOf(cmd) + 0x210;
                        byte[] arr = new byte[size];
                        IntPtr ptr = Marshal.AllocHGlobal(size);
                        Marshal.StructureToPtr(cmd, ptr, true);
                        Marshal.Copy(ptr, arr, 0, size);
                        Marshal.FreeHGlobal(ptr);
                        Buffer.BlockCopy(read, (int)k * 0x210, arr, Marshal.SizeOf(cmd), 0x210);
                        serial.Write(arr, 0, arr.Length);

                        UInt32 ret = RecvUInt32(serial);
                        if (ret != 0)
                        {
                            Console.WriteLine("Error: " + ret.ToString("X"));
                            Console.WriteLine("");
                            break;
                        }
                    }

                    MainForm.mainForm.PicoFlasherBlocksUpdate((j / 4).ToString("X"), (int)((j * 100) / (flashsize / (512 * 8))));
                }

                br.Close();

                MainForm.mainForm.PicoFlasherBusy(0);

                CloseSerial(serial);

                Console.WriteLine("Write Successful!");
                Console.WriteLine("");

                SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                if (variables.soundsuccess != "") success.SoundLocation = variables.soundsuccess;
                success.Play();

                if (fixEcc != 0)
                {
                    Thread.Sleep(500);
                    MainForm.mainForm.afterWriteEccCleanup();
                }
            });
            writerThread.Start();
        }

        public int Open()
        {
            return 1;
        }

        public int Close()
        {
            return 1;
        }

        public int PowerUp()
        {
            SerialPort serial = OpenSerial();
            if (serial == null)
                return 0;

            CMD cmd = new CMD();
            cmd.cmd = COMMANDS.ISD1200_INIT;
            cmd.lba = 0;

            SendCmd(serial, cmd);

            int ret = RecvUInt8(serial);

            CloseSerial(serial);
            return ret == 0 ? 1 : 0;
        }

        public int GetID()
        {
            SerialPort serial = OpenSerial();
            if (serial == null)
                return 0;

            CMD cmd = new CMD();
            cmd.cmd = COMMANDS.ISD1200_READ_ID;
            cmd.lba = 0;

            SendCmd(serial, cmd);

            int id = RecvUInt8(serial);

            CloseSerial(serial);

            if (id == 0x01)
            {
                return 1;
            }
            else if (id == 0x10)
            {
                return 2;
            }
            else if (id == 0x11)
            {
                return 3;
            }

            return 0;
        }

        public void PowerDown()
        {
            SerialPort serial = OpenSerial();
            if (serial == null)
                return;

            CMD cmd = new CMD();
            cmd.cmd = COMMANDS.ISD1200_DEINIT;
            cmd.lba = 0;

            SendCmd(serial, cmd);

            RecvUInt8(serial);

            CloseSerial(serial);
        }

        public void Reset()
        {
            SerialPort serial = OpenSerial();
            if (serial == null)
                return;

            CMD cmd = new CMD();
            cmd.cmd = COMMANDS.ISD1200_RESET;
            cmd.lba = 0;

            SendCmd(serial, cmd);

            RecvUInt8(serial);

            CloseSerial(serial);
        }

        public int ISD_Read_Flash(string filename)
        {
            SerialPort serial = OpenSerial();
            if (serial == null)
                return 0;

            if (File.Exists(filename))
            {
                if (DialogResult.Cancel == MessageBox.Show("File already exists, it will be DELETED! Press OK to continue", "About to overwrite a nanddump", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                {
                    Console.WriteLine("Cancelled");
                    Console.WriteLine("");
                    return 0;
                };
            }

            BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.Create, FileAccess.Write));

            for (uint i = 0; i < 0xB000 / 512; i++)
            {
                CMD cmd = new CMD();
                cmd.cmd = COMMANDS.ISD1200_READ_FLASH;
                cmd.lba = i;

                SendCmd(serial, cmd);

                byte[] rxbuffer = new byte[512];
                int got = 0;
                while (got < rxbuffer.Length)
                    got += serial.Read(rxbuffer, got, rxbuffer.Length - got);

                bw.Write(rxbuffer);

                UpdateProgress((int)((i * 100) / (0xB000 / 512)));
            }
            UpdateProgress(100);

            bw.Close();

            CloseSerial(serial);

            return 1;
        }

        public int ISD_Erase_Flash()
        {
            SerialPort serial = OpenSerial();
            if (serial == null)
                return 0;

            CMD cmd = new CMD();
            cmd.cmd = COMMANDS.ISD1200_ERASE_FLASH;
            cmd.lba = 0;

            SendCmd(serial, cmd);

            int ret = RecvUInt8(serial);

            CloseSerial(serial);
            return ret == 0 ? 1 : 0;
        }

        public void ISD_Write_Flash(string filename)
        {
            SerialPort serial = OpenSerial();
            if (serial == null)
                return;

            BinaryReader br = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));

            for (uint i = 0; i < 0xB000 / 16; i++)
            {
                byte[] read = br.ReadBytes(16);
                if (read == null || read.Length != 16)
                    break;

                CMD cmd = new CMD();
                cmd.cmd = COMMANDS.ISD1200_WRITE_FLASH;
                cmd.lba = i;

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

                UpdateProgress((int)((i * 100) / (0xB000 / 16)));
            }
            UpdateProgress(100);

            br.Close();

            CloseSerial(serial);
        }

        public Boolean ISD_Verify_Flash(string filename)
        {
            if (!File.Exists(filename))
            {
                log("Image file not found\n");
                return false;
            }
            FileInfo fl = new FileInfo(filename);
            if (fl.Length != 0xB000)
            {
                log("Image file must be 44Kb\n");
                return false;
            }
            BinaryReader rw = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));
            byte[] file = rw.ReadBytes(0xB000);
            rw.Close();
            
            SerialPort serial = OpenSerial();
            if (serial == null)
                return false;

            byte[] data = new byte[0xB000];

            for (uint i = 0; i < 0xB000 / 512; i++)
            {
                CMD cmd = new CMD();
                cmd.cmd = COMMANDS.ISD1200_READ_FLASH;
                cmd.lba = i;

                SendCmd(serial, cmd);

                int got = 0;
                while (got < 512)
                    got += serial.Read(data, (int) i * 512 + got, 512 - got);

                UpdateProgress((int)((i * 100) / (0xB000 / 512)));
            }
            UpdateProgress(100);

            CloseSerial(serial);

            return Oper.ByteArrayCompare(file, data);
        }

        public void ISD_Play(ushort index)
        {
            SerialPort serial = OpenSerial();
            if (serial == null)
                return;

            CMD cmd = new CMD();
            cmd.cmd = COMMANDS.ISD1200_PLAY_VOICE;
            cmd.lba = index;

            SendCmd(serial, cmd);

            RecvUInt8(serial);

            CloseSerial(serial);
        }

        public void ISD_Exec(ushort index)
        {
            SerialPort serial = OpenSerial();
            if (serial == null)
                return;

            CMD cmd = new CMD();
            cmd.cmd = COMMANDS.ISD1200_EXEC_MACRO;
            cmd.lba = index;

            SendCmd(serial, cmd);

            RecvUInt8(serial);

            CloseSerial(serial);
        }
    }
}
