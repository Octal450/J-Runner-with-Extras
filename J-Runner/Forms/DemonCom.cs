using LibUsbDotNet;
using LibUsbDotNet.Main;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace JRunner.Forms
{

    public partial class DemonCom : Form
    {
        public DemonCom()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private static Demon _demon;

        private bool StartDemon()
        {
            _demon = new Demon();
            _demon.NewSerialDataRecieved += MonitorOnNewSerialDataRecieved;
            return _demon.Start();
        }

        private void MonitorOnNewSerialDataRecieved(object sender, SerialDataEventArgs e)
        {
            var tmp = Encoding.UTF8.GetString(e.Data);
            Console.Write(tmp);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.SetOut(new ControlWriter(textBox1));
            StartDemon();
        }

        private void DemonCom_Load(object sender, EventArgs e)
        {
            updateLogColor();
        }

        private void DemonCom_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Restart();
        }

        public void updateLogColor()
        {
            textBox1.BackColor = variables.logbackground;
            textBox1.ForeColor = variables.logtext;
        }
    }

    public class ControlWriter : TextWriter
    {
        private Control textbox1;
        public ControlWriter(Control textbox)
        {
            this.textbox1 = textbox;
        }

        public override void Write(char value)
        {
            textbox1.Text += value;
        }

        public override void Write(string value)
        {
            textbox1.Text += value;
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }

    internal sealed class SerialDataEventArgs : EventArgs
    {
        private readonly byte[] _data;

        public SerialDataEventArgs(byte[] dataInByteArray)
        {
            _data = dataInByteArray;
        }

        public byte[] Data
        {
            get { return _data; }
        }
    }
    internal class Demon
    {
        //private const int CYGNOS_PID = 0x0001;
        //private const int CYGNOS_VID = 0x2120;
        private const int DemonPid = 0x444e;
        private const int DemonVid = 0x11d4;

        private UsbDevice _device;
        private UsbEndpointReader _reader;
        private UsbEndpointWriter _writer;

        public event EventHandler<SerialDataEventArgs> NewSerialDataRecieved;

        public bool SendCommand(DemonCommands cmd)
        {
            int ret;
            var buf = new byte[1];
            buf[0] = (byte)cmd;
            return (_writer.Write(buf, 1500, out ret) == ErrorCode.None);
        }

        private byte ReadByte()
        {
            int ret;
            var buf = new byte[1];
            var res = _reader.Read(buf, 1500, out ret);
            if (res == ErrorCode.None)
                return buf[0];
            if (res == ErrorCode.Win32Error)
                throw new Win32Exception();
            throw new Exception(res.ToString());
        }

        private byte[] ReadBuffer()
        {
            int ret;
            var buf = new byte[0x600];
            var res = _reader.Read(buf, 1500, out ret);
            if (res == ErrorCode.None)
            {
                if (ret <= 2)
                    return null;
                var retbuf = new byte[ret - 2];
                Buffer.BlockCopy(buf, 2, retbuf, 0, retbuf.Length);
                return retbuf;
            }
            if (res == ErrorCode.Win32Error)
                throw new Win32Exception();
            throw new Exception(res.ToString());
        }

        private byte GetMode()
        {
            if (!SendCommand(DemonCommands.GET_MODE))
                throw new Exception();
            return ReadByte();
        }

        private bool Open()
        {
            _device = UsbDevice.OpenUsbDevice(new UsbDeviceFinder(DemonVid, DemonPid));
            if (_device == null || !_device.IsOpen)
                return false;
            var wholeUsbDevice = _device as IUsbDevice;
            if (!ReferenceEquals(wholeUsbDevice, null))
            {
                wholeUsbDevice.SetConfiguration(1);
                wholeUsbDevice.ClaimInterface(0);
            }
            _reader = _device.OpenEndpointReader(ReadEndpointID.Ep02);
            _writer = _device.OpenEndpointWriter(WriteEndpointID.Ep01);
            return true;
        }

        public bool Start()
        {
            if (!Open())
                return false;
            if (GetMode() != 1)
            {
                SendCommand(DemonCommands.BTL_LEAVE);
                return false;
            }
            var bw = new BackgroundWorker();
            bw.DoWork += (sender, args) =>
            {
                while (true)
                {
                    SendCommand(DemonCommands.READ_SERIAL_PORT);
                    var ret = ReadBuffer();
                    if (ret != null)
                    {
                        var handler = NewSerialDataRecieved;
                        if (handler != null)
                            handler.Invoke(null, new SerialDataEventArgs(ret));
                    }
                    Thread.Sleep(25);
                }
            };
            bw.RunWorkerAsync();
            return true;
        }

        public void Stop()
        {
            var wholeUsbDevice = _device as IUsbDevice;
            if (!ReferenceEquals(wholeUsbDevice, null))
                wholeUsbDevice.ReleaseInterface(0);
            _device.Close();
            _device = null;
        }

        internal enum DemonCommands : byte
        {
            GET_MODE = 0x00, // rep 00 = bootloader; 01 = nrm
            GET_PROTOCOL_VERSION = 0x01, // returns 2 bytes, MAJ,MIN
            GET_DEVICE_ID = 0x02, // returns 2 bytes, 0 = fat16, 1 = slim16
            GET_FIRMWARE_VERSION = 0x03, // two byte reply, first byte minor second byte major
            RUN_BOOTLOADER = 0x04,
            GET_EXT_FLASH = 0x05, // rep 00 = int; 01 = demflash
            SET_EXT_FLASH = 0x06,
            ACQUIRE_EXT_FLASH = 0x07,
            RELEASE_EXT_FLASH = 0x08,
            GET_EXT_FLASH_ID = 0x09, // 0xDA 0xAD (0xADDA) - 256M; 0x73 0xAD 16M; 0x76 0xAD 64M
            GET_INVALID_BLOCKS = 0x0a,
            ERASE_EXT_FLASH_BLOCK = 0x0b,
            ERASE_ALL_EXT_FLASH_BLOCKS = 0x0c,
            READ_EXT_FLASH_BLOCK = 0x0d,
            PROGRAM_EXT_FLASH_BLOCK = 0x0e,
            ASSERT_SB_RESET = 0x0f,
            DEASSERT_SB_RESET = 0x10,
            READ_SERIAL_PORT = 0x11, // returns 00 00 on no data
            WRITE_SERIAL_PORT = 0x12,
            EXEC_XSVF = 0x13,
            POWER_ON = 0x14,
            POWER_OFF = 0x15,

            // commands 0x0, 0x1, 0x2 are recycled
            BTL_LEAVE = 0x81,
            BTL_82 = 0x82,
            BTL_LOCK = 0x83,
            BTL_UNLOCK = 0x84,
            BTL_ERASE_PAGE = 0x86, // pages are 256b?
            BTL_WRITE_PAGE = 0x87, // command followed by 256b data
        }
    }
}
