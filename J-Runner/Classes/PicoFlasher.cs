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

        public int Version = 0;

        enum COMMANDS : byte
        {
            GET_VERSION = 0x00,
            GET_FLASH_CONFIG,
            READ_FLASH,
            WRITE_FLASH,
            READ_FLASH_STREAM,

            EMMC_DETECT = 0x50,
            EMMC_INIT,
            EMMC_GET_CID,
            EMMC_GET_CSD,
            EMMC_GET_EXT_CSD,
            EMMC_READ,
            EMMC_READ_STREAM,
            EMMC_WRITE,

            ISD1200_INIT = 0xA0,
            ISD1200_DEINIT,
            ISD1200_READ_ID,
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
                            RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                            string portName = (string)rk6.GetValue("PortName");
                            if (!string.IsNullOrEmpty(portName) && SerialPort.GetPortNames().Contains(portName))
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
            SerialPort serial = new SerialPort();

            try
            {
                ports = ComPortNames("600D", "7001");

                if (ports.Count <= 0)
                {
                    MessageBox.Show("Can't find PicoFlasher COM port\n\nUpdate the PicoFlasher firmware and check your drivers", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                serial.PortName = ports[0];
                serial.ReadTimeout = 5000;
                serial.WriteTimeout = 5000;

                serial.Open();

                CMD cmd = new CMD();
                cmd.cmd = COMMANDS.GET_VERSION;
                cmd.lba = 0;

                SendCmd(serial, cmd);

                Version = (int)RecvUInt32(serial);

                if (Version < 2)
                {
                    serial.Close();
                    MessageBox.Show("PicoFlasher firmware is too old\n\nUpdate the PicoFlasher firmware to continue", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                if (Version < 3) {
                    Console.WriteLine("Old PicoFlasher v" + Version + " doesn't support eMMC, update firmware if this is needed.");
                }

                InUse = true;
            }
            catch (Exception ex)
            {
                if (variables.debugMode) Console.WriteLine(ex.ToString());
                else Console.WriteLine(ex.GetType());

                MessageBox.Show("PicoFlasher COM port could not be found\n\nYou may need to update the PicoFlasher firmware or check your connections to continue", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
            Console.WriteLine("Checking Console...");
            CMD cmd = new CMD();
            cmd.cmd = COMMANDS.GET_FLASH_CONFIG;
            cmd.lba = 0;

            SendCmd(serial, cmd);

            UInt32 flashconfig = RecvUInt32(serial);
            Console.WriteLine("Flash Config: 0x" + flashconfig.ToString("X8"));

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
                else if (minor == 2 || minor == 3) // Jasper, Trinity, Corona, 256MB/512MB
                    size = (uint)(8 << (int)(((flash_config >> 19) & 0x3) + ((flash_config >> 21) & 0xF)));
            }
            else // Xenon, Zephyr, Falcon
                size = (uint)(8 << (int)minor);

            return size * 1024 * 1024;
        }

        private UInt32 UNSTUFF_BITS(UInt32[] resp, int start, int size)
        {
            int __size = size;
            UInt32 __mask = (UInt32)(__size < 32 ? 1 << __size : 0) - 1;
            int __off = 3 - ((start) / 32);
            int __shft = (start) & 31;
            UInt32 __res;

            __res = resp[__off] >> __shft;
            if (__size + __shft > 32)
                __res |= resp[__off - 1] << ((32 - __shft) % 32);

            return __res & __mask;
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
            Console.WriteLine("Flash Config: 0x" + flashconfig.ToString("X8"));

            byte emmc_det = 0;
            if (Version >= 3) {
                cmd.cmd = COMMANDS.EMMC_DETECT;
                SendCmd(serial, cmd);
                emmc_det = RecvUInt8(serial);
            }

            if (flashconfig != 0x00000000 && flashconfig != 0xFFFFFFFF && flashconfig != 0xC0462002)
            {
                if (flashconfig == 0x01198010)
                    Console.WriteLine("Xenon, Zephyr, Falcon: 16MB");
                else if (flashconfig == 0x01198030)
                    Console.WriteLine("Xenon, Zephyr, Falcon: 64MB");
                else if (flashconfig == 0x00023010)
                    Console.WriteLine("Jasper, Trinity: 16MB");
                else if (flashconfig == 0x00043000)
                    Console.WriteLine("Corona: 16MB");
                else if (flashconfig == 0x008A3020)
                    Console.WriteLine("Jasper, Trinity: 256MB");
                else if (flashconfig == 0x00AA3020)
                    Console.WriteLine("Jasper, Trinity: 512MB");
                else if (flashconfig == 0x008C3020)
                    Console.WriteLine("Corona: 256MB");
                else if (flashconfig == 0x00AC3020)
                    Console.WriteLine("Corona: 512MB");
                else
                    Console.WriteLine("Unrecongized Flash Config");
            }
            else if (emmc_det == 0 && flashconfig == 0xC0462002)
            {
                Console.WriteLine("Corona: 4GB (eMMC not connected)");
            }
            else if (emmc_det != 0)
            {
                if (flashconfig == 0xC0462002)
                    Console.WriteLine("Corona: 4GB");
                else if (flashconfig != 0x00000000 && flashconfig != 0xFFFFFFFF)
                    Console.WriteLine("Unrecongized Flash Config");

                cmd.cmd = COMMANDS.EMMC_INIT;
                SendCmd(serial, cmd);
                UInt32 emmc_init_ret = RecvUInt32(serial);

                if (emmc_init_ret != 0)
                {
                    Console.WriteLine("eMMC init failed: " + emmc_init_ret);
                }
                else
                {
                    cmd.cmd = COMMANDS.EMMC_GET_CID;
                    SendCmd(serial, cmd);

                    byte[] CID = new byte[16];
                    int got = 0;
                    while (got < CID.Length)
                        got += serial.Read(CID, got, CID.Length - got);

                    cmd.cmd = COMMANDS.EMMC_GET_CSD;
                    SendCmd(serial, cmd);

                    byte[] CSD = new byte[16];
                    got = 0;
                    while (got < CSD.Length)
                        got += serial.Read(CSD, got, CSD.Length - got);

                    cmd.cmd = COMMANDS.EMMC_GET_EXT_CSD;
                    SendCmd(serial, cmd);

                    byte[] ext_csd = new byte[512];
                    got = 0;
                    while (got < CSD.Length)
                        got += serial.Read(ext_csd, got, ext_csd.Length - got);

                    if (variables.debugMode)
                    {
                        Console.Write("CID: ");
                        for (int i = 0; i < CID.Length; i++)
                            Console.Write(CID[i].ToString("X2"));
                        Console.WriteLine("");

                        Console.Write("CSD: ");
                        for (int i = 0; i < CSD.Length; i++)
                            Console.Write(CSD[i].ToString("X2"));
                        Console.WriteLine("");
                    }
                    
                    UInt32[] csd = new UInt32[4];
                    Buffer.BlockCopy(CSD, 0, csd, 0, CSD.Length);

                    for (int i = 0; i < 4; i++)
                    {
                        byte[] bytes = BitConverter.GetBytes(csd[i]);
                        Array.Reverse(bytes);
                        csd[i] = BitConverter.ToUInt32(bytes, 0);
                    }

                    if (variables.debugMode)
                    {
                        Console.WriteLine("CSD_STRUCTURE: " + UNSTUFF_BITS(csd, 126, 2).ToString("X"));
                        Console.WriteLine("TAAC: " + UNSTUFF_BITS(csd, 112, 8).ToString("X"));
                        Console.WriteLine("TRAN_SPEED: " + UNSTUFF_BITS(csd, 96, 8).ToString("X"));
                        Console.WriteLine("READ_BL_LEN: " + UNSTUFF_BITS(csd, 80, 4).ToString("X"));
                        Console.WriteLine("READ_BL_PARTIAL: " + UNSTUFF_BITS(csd, 79, 1).ToString("X"));
                        Console.WriteLine("C_SIZE: " + UNSTUFF_BITS(csd, 62, 12).ToString("X"));
                        Console.WriteLine("C_SIZE_MULT: " + UNSTUFF_BITS(csd, 47, 3).ToString("X"));
                        Console.WriteLine("ERASE_GRP_SIZE: " + UNSTUFF_BITS(csd, 42, 5).ToString("X"));

                        Console.WriteLine("Extended CSD V1." + ext_csd[194].ToString()); // EXT_CSD_STRUCTURE
                        Console.WriteLine(" Spec Version:  " + UNSTUFF_BITS(csd, 122, 4).ToString("X2"));
                        Console.WriteLine(" Extended Rev:  1." + ext_csd[192].ToString()); // EXT_CSD_REV
                    }

                    byte[] devver = new byte[4];
                    Buffer.BlockCopy(ext_csd, 262, devver, 0, 2); // EXT_CSD_DEVICE_VERSION

                    if (variables.debugMode)
                    {
                        Console.WriteLine(" Dev Version:   " + BitConverter.ToUInt32(devver, 0).ToString());
                        Console.WriteLine(" Cmd Classes:   " + UNSTUFF_BITS(csd, 84, 12).ToString("X2"));
                        Console.WriteLine(" Capacity:      " + ((UNSTUFF_BITS(csd, 62, 12) == 0xfff && UNSTUFF_BITS(csd, 47, 3) == 7) ? "High" : "Low"));
                    }

                    byte card_type = ext_csd[196]; // EXT_CSD_CARD_TYPE
                    int speed = 0;
                    if ((card_type & (1 << 0)) != 0) // EXT_CSD_CARD_TYPE_HS_26
                        speed = (26 << 16) | 26;
                    if ((card_type & (1 << 1)) != 0) // EXT_CSD_CARD_TYPE_HS_52
                        speed = (52 << 16) | 52;
                    if ((card_type & (1 << 2)) != 0) // EXT_CSD_CARD_TYPE_DDR_1_8V
                        speed = (52 << 16) | 104;
                    if ((card_type & (1 << 4)) != 0) // EXT_CSD_CARD_TYPE_HS200_1_8V
                        speed = (200 << 16) | 200;
                    if ((card_type & (1 << 6)) != 0) // EXT_CSD_CARD_TYPE_HS400_1_8V
                        speed = (200 << 16) | 400;

                    if (variables.debugMode)
                    {
                        Console.WriteLine(" Max Rate:      " + (speed & 0xFFFF).ToString() + " MB/s (" + ((speed >> 16) & 0xFFFF).ToString() + " MHz)");
                        Console.Write(" Type Support:  ");
                        if ((card_type & (1 << 0)) != 0) // EXT_CSD_CARD_TYPE_HS_26
                            Console.Write("HS26");
                        if ((card_type & (1 << 1)) != 0) // EXT_CSD_CARD_TYPE_HS_52
                            Console.Write(", HS52");
                        if ((card_type & (1 << 2)) != 0) // EXT_CSD_CARD_TYPE_DDR_1_8V
                            Console.Write(", DDR52_1.8V");
                        if ((card_type & (1 << 4)) != 0) // EXT_CSD_CARD_TYPE_HS200_1_8V
                            Console.Write(", HS200_1.8V");
                        if ((card_type & (1 << 6)) != 0) // EXT_CSD_CARD_TYPE_HS400_1_8V
                            Console.Write(", HS400_1.8V");
                        Console.WriteLine("");
                    }
                }
            }
            else
            {
                Console.WriteLine("Console Not Found");
            }

            Console.WriteLine("");
            CloseSerial(serial);
        }

        private void ReadNand(int iterations, uint start = 0, uint end = 0)
        {
            Thread readerThread = new Thread(() =>
            {
                SerialPort serial = OpenSerial();
                if (serial == null)
                    return;

                uint flashconfig = getFlashConfig(serial);
                if (flashconfig == 0)
                {
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

                if (flashsize == 268435456 || flashsize == 536870912)
                {
                    DialogResult bbdr = MessageBox.Show("A big block nand has been detected\n\nDo you want to dump only the system partition? (recommended)", "Nand Dump Size", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    
                    if (bbdr == DialogResult.Cancel)
                    {
                        CloseSerial(serial);
                        return;
                    }
                    else if (bbdr == DialogResult.Yes)
                    {
                        flashsize = 67108864;
                    }
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

                    Console.WriteLine("Read Successful!");
                    Console.WriteLine("");

                    if (variables.playSuccess)
                    {
                        SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                        success.Play();
                    }

                    Thread.Sleep(1000);
                    MainForm.mainForm.PicoFlasherInitNand(i);

                    if (i + 1 < iterations)
                        Thread.Sleep(1000);
                }

                CloseSerial(serial);
            });
            readerThread.Start();
        }

        private void WriteNand(int fixEcc, uint start = 0, uint end = 0, bool isEccOrXell = false)
        {
            if (string.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;

            Thread writerThread = new Thread(() =>
            {
                SerialPort serial = OpenSerial();
                if (serial == null)
                    return;

                uint flashconfig = getFlashConfig(serial);
                if (flashconfig == 0)
                {
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

                if (variables.playSuccess)
                {
                    SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                    success.Play();
                }

                if (isEccOrXell)
                {
                    Thread.Sleep(500);
                    MainForm.mainForm.afterWriteXeLLCleanup();
                }
            });
            writerThread.Start();
        }

        private void ReadEmmc(int iterations, uint start = 0, uint end = 0)
        {
            getFlashConfig();

            Thread readerThread = new Thread(() =>
            {
                SerialPort serial = OpenSerial();
                if (serial == null)
                    return;

                uint flashsize = 48 * 1024 * 1024; // On EMMC we only need the first 48MB

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

                    BinaryWriter bw = new BinaryWriter(File.Open(variables.filename, FileMode.Create, FileAccess.Write));

                    if (start == 0 && end == 0)
                    {
                        CMD cmd = new CMD();
                        cmd.cmd = COMMANDS.EMMC_READ_STREAM;
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

                            byte[] rxbuffer = new byte[0x200];
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
                            cmd.cmd = COMMANDS.EMMC_READ;
                            cmd.lba = j;

                            SendCmd(serial, cmd);

                            UInt32 ret = RecvUInt32(serial);

                            if (ret != 0)
                            {
                                Console.WriteLine("Error: " + ret.ToString("X"));
                                Console.WriteLine("");
                                break;
                            }

                            byte[] rxbuffer = new byte[0x200];
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

                    Console.WriteLine("Read Successful!");
                    Console.WriteLine("");

                    if (variables.playSuccess)
                    {
                        SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                        success.Play();
                    }

                    Thread.Sleep(1000);
                    MainForm.mainForm.PicoFlasherInitNand(i);

                    if (i + 1 < iterations)
                        Thread.Sleep(1000);
                }

                CloseSerial(serial);
            });
            readerThread.Start();
        }

        private void WriteEmmc(uint start = 0, uint end = 0, bool isEccOrXell = false)
        {
            if (string.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;

            getFlashConfig();

            Thread writerThread = new Thread(() =>
            {
                SerialPort serial = OpenSerial();
                if (serial == null)
                    return;

                uint flashsize = 48 * 1024 * 1024; // On EMMC we only need the first 48MB

                MainForm.mainForm.PicoFlasherBusy(2);

                BinaryReader br = new BinaryReader(File.Open(variables.filename1, FileMode.Open, FileAccess.Read));

                uint writeend = flashsize / (512 * 8);
                if (start != 0 || end != 0)
                    writeend = end;

                for (uint j = start; j < writeend; j++)
                {
                    byte[] read = br.ReadBytes(0x200 * 8);
                    if (read == null || read.Length % 0x200 != 0)
                        break;

                    for (uint k = 0; k < read.Length / 0x200; k++)
                    {
                        CMD cmd = new CMD();
                        cmd.cmd = COMMANDS.EMMC_WRITE;
                        cmd.lba = j * 8 + k;

                        int size = Marshal.SizeOf(cmd) + 0x200;
                        byte[] arr = new byte[size];
                        IntPtr ptr = Marshal.AllocHGlobal(size);
                        Marshal.StructureToPtr(cmd, ptr, true);
                        Marshal.Copy(ptr, arr, 0, size);
                        Marshal.FreeHGlobal(ptr);
                        Buffer.BlockCopy(read, (int)k * 0x200, arr, Marshal.SizeOf(cmd), 0x200);
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

                if (variables.playSuccess)
                {
                    SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                    success.Play();
                }

                if (isEccOrXell)
                {
                    Thread.Sleep(500);
                    MainForm.mainForm.afterWriteXeLLCleanup();
                }
            });
            writerThread.Start();
        }

        public void Read(int iterations, uint start = 0, uint end = 0)
        {
            SerialPort serial = OpenSerial();
            if (serial == null)
                return;

            byte emmc_det = 0;
            if (Version >= 3) {
                CMD cmd = new CMD();
                cmd.cmd = COMMANDS.EMMC_DETECT;
                cmd.lba = 0;
                SendCmd(serial, cmd);
                emmc_det = RecvUInt8(serial);
            }

            CloseSerial(serial);

            if (emmc_det == 0)
                ReadNand(iterations, start, end);
            else
                ReadEmmc(iterations, start, end);
        }

        public void Write(int fixEcc, uint start = 0, uint end = 0, bool isEccOrXell = false)
        {
            SerialPort serial = OpenSerial();
            if (serial == null)
                return;

            byte emmc_det = 0;
            if (Version >= 3) {
                CMD cmd = new CMD();
                cmd.cmd = COMMANDS.EMMC_DETECT;
                cmd.lba = 0;
                SendCmd(serial, cmd);
                emmc_det = RecvUInt8(serial);
            }

            CloseSerial(serial);

            if (emmc_det == 0)
                WriteNand(fixEcc, start, end, isEccOrXell);
            else
                WriteEmmc(start, end, isEccOrXell);
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
