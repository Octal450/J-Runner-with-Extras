using LibUsbDotNet;
using LibUsbDotNet.Main;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace JRunner.Functions
{
    public partial class CtrlTr : Form
    {
        [DllImport("libusb0.dll")]
        public static extern int puts(string c);

        public CtrlTr()
        {
            InitializeComponent();
        }

        private static UsbDevice MyUsbDevice;
        private static UsbDeviceFinder x360usb = new UsbDeviceFinder(0x11d4, 0x8333);

        void send()
        {
            if (MyUsbDevice != null && MyUsbDevice.IsOpen)
            {
                Console.WriteLine("Device Already in Use");
                return;
            }
            try
            {

                MyUsbDevice = UsbDevice.OpenUsbDevice(x360usb);
                if (MyUsbDevice == null)
                {
                    Console.WriteLine("Device Not Found");
                    return;
                }
                //ErrorCode ec = ErrorCode.None;
                IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                if (!ReferenceEquals(wholeUsbDevice, null))
                {
                    wholeUsbDevice.SetConfiguration(1);
                    if (variables.debugme) Console.WriteLine("Claiming Interface...");
                    wholeUsbDevice.ClaimInterface(0);
                    if (variables.debugme) Console.WriteLine("The Interface is ours!");
                }
                UsbEndpointReader reader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep01);
                UsbEndpointWriter writer = MyUsbDevice.OpenEndpointWriter(WriteEndpointID.Ep01);
                ErrorCode ec = ErrorCode.None;

                int bytesRead = 1;
                int timeout = 500;
                byte[] writeBuffer = Oper.StringToByteArray_v2(txtBuffer.Text);
                byte[] readBuffer = new byte[64];

                ec = writer.Write(writeBuffer, 500, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer, 0, bytesRead));

                Console.WriteLine("Sent");
                //ec = reader.Read(readBuffer, timeout, out bytesRead);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
            }
            finally
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
            }
        }

        void test()
        {
            if (MyUsbDevice != null && MyUsbDevice.IsOpen)
            {
                Console.WriteLine("Device Already in Use");
                return;
            }
            try
            {

                MyUsbDevice = UsbDevice.OpenUsbDevice(x360usb);
                if (MyUsbDevice == null)
                {
                    Console.WriteLine("Device Not Found");
                    return;
                }
                //ErrorCode ec = ErrorCode.None;
                IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                if (!ReferenceEquals(wholeUsbDevice, null))
                {
                    wholeUsbDevice.SetConfiguration(1);
                    if (variables.debugme) Console.WriteLine("Claiming Interface...");
                    wholeUsbDevice.ClaimInterface(0);
                    if (variables.debugme) Console.WriteLine("The Interface is ours!");
                }
                UsbEndpointReader reader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep01);
                UsbEndpointWriter writer = MyUsbDevice.OpenEndpointWriter(WriteEndpointID.Ep01);
                ErrorCode ec = ErrorCode.None;

                int bytesRead = 1;
                int timeout = 500;
                byte[] writeBuffer;
                byte[] readBuffer = new byte[4];

                writeBuffer = new byte[] { 0x17, 0x00, 0x00 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x16, 0xA0, 0x00 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x27 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x27 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x26 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x17, 0xEC, 0x00 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x27 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x21 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x26 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x25 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x24 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x23 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x22 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x27 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x11, 0x00, 0x00 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x12, 0x00, 0x00 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x14, 0x00, 0x00 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x15, 0x02, 0x00 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x26 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                writeBuffer = new byte[] { 0x17, 0xA0, 0x00 };
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                writeBuffer = new byte[] { 0x27 };
                readBuffer = new byte[4];
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                readBuffer = new byte[4];
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                writeBuffer = new byte[] { 0x41, 0x0C, 0x12, 0x00, 0x00, 0x00, 0x60, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                readBuffer = new byte[4];
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                writeBuffer = new byte[] { 0x27 };
                readBuffer = new byte[4];
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                readBuffer = new byte[4];
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                writeBuffer = new byte[] { 0x25 };
                readBuffer = new byte[4];
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                writeBuffer = new byte[] { 0x24 };
                readBuffer = new byte[4];
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Oper.ByteArrayToString(readBuffer));

                writeBuffer = new byte[] { 0x40, 0x40 };
                readBuffer = new byte[64];
                ec = writer.Write(writeBuffer, timeout, out bytesRead);
                Console.WriteLine("Write {0} - Status: {1} - Length: {2}", Oper.ByteArrayToString(writeBuffer), ec, bytesRead);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                Console.WriteLine("Read - Status: {0} - Length: {1}", ec, bytesRead);
                Console.WriteLine(Encoding.ASCII.GetString(readBuffer));


                Console.WriteLine("Sent");
                //ec = reader.Read(readBuffer, timeout, out bytesRead);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
            }
            finally
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
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            send();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            test();
        }

    }
}
