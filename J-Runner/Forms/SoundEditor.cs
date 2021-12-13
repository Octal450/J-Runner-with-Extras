using LibUsbDotNet;
using LibUsbDotNet.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class SoundEditor : Form
    {
        private UsbDevice MyUsbDevice;
        private UsbDeviceFinder JRunner = new UsbDeviceFinder(0x11d4, 0x8338);
        public SoundEditor()
        {
            InitializeComponent();
        }

        enum ISD_Function : byte
        {
            read = 0x1,
            write,
            verify,
            play_power,
            play_eject
        }

        private int Intro()
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
            Console.WriteLine("Sending Flash Init...");
            MyUsbDevice.ControlTransfer(ref packet, buf, 1, out transfer);
            if (MainForm.debugme) Console.WriteLine("Length Transferred: {0}", transfer);

            MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
            if (MainForm.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
            Console.WriteLine("Status: 0x{0:X}", readBuffer[0]);

            if (readBuffer[0] == 0x80 || readBuffer[0] == 0x60) return 1;
            else return 0;
        }
        private int GetID()
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

            Console.WriteLine("Sending Flash ID...");
            MyUsbDevice.ControlTransfer(ref packet, buf, 5, out transfer);
            if (MainForm.debugme) Console.WriteLine("Length Transferred: {0}", transfer);

            MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
            if (MainForm.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
            if (NandX.ByteArrayCompare(ID, Oper.returnportion(readBuffer, 1, 4)))
            {
                Console.WriteLine("ISD-2100 detected");
                return 1;
            }
            else
            {
                Console.WriteLine("no ISD-2100 detected");
                Console.WriteLine("In {0}", Oper.ByteArrayToString(readBuffer));
                return 0;
            }
        }
        private void Outro()
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
            
            Console.WriteLine("Sending Flash de-Init...");
            MyUsbDevice.ControlTransfer(ref packet, buf, 1, out transfer);
            if (MainForm.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
                
            MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
            if (MainForm.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
            Console.WriteLine("Status: 0x{0:X}", readBuffer[0]);



            if (readBuffer[0] == 0x61) Thread.Sleep(2000);
        }
        private void Normal()
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

            Console.WriteLine("Setting ISD2100 to normal mode..");
            MyUsbDevice.ControlTransfer(ref packet, buf, 1, out transfer);
            if (MainForm.debugme) Console.WriteLine("Length Transferred: {0}", transfer);

            MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
            if (MainForm.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
            Console.WriteLine("Status: 0x{0:X}", readBuffer[0]);
        }

        private void ISD_Read_Flash()
        {
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

            try
            {
                File.Delete(txtFile.Text);
            }
            catch (Exception ex) { if (MainForm.debugme) Console.WriteLine(ex.ToString()); };

            BinaryWriter sw = new BinaryWriter(File.Open(txtFile.Text, FileMode.Append, FileAccess.Write));

            Console.WriteLine("Reading from Flash..");

            byte[] RES = { 0x60, 0x60, 0x60, 0x60 };
            byte[] CMD = new byte[8];

            int fails = 0;
            CMD[0] = 0xA2;
            for (int i = 0; i < 0xB000; i += 4)
            {
                if (variables.escapeloop) break;
                progressBar1.Value = (i * progressBar1.Maximum) / 0xB000;

                if ((i % 0x400 == 0)) Thread.Sleep(20);

                CMD[3] = (byte)(i & 0x00ff);
                CMD[2] = (byte)((i & 0xff00) >> 8);

                MyUsbDevice.ControlTransfer(ref packet, CMD, 8, out transfer);
                Thread.Sleep(variables.delay);

                MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
                if (NandX.ByteArrayCompare(readBuffer, RES, 4))
                {
                    sw.Write(Oper.returnportion(readBuffer, 4, 4));
                    fails = 0;
                }
                else
                {
                    if (fails == 6)
                    {
                        Console.WriteLine("\nRead Failed..\n");
                        break;
                    }
                    i -= 4;
                    Console.Write(":");
                    fails++;
                }
            }
            progressBar1.Value = progressBar1.Maximum;
            sw.Close();
            return;
        }
        private void ISD_Erase_Flash()
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
            Console.WriteLine("Sending Flash Erase...");
            MyUsbDevice.ControlTransfer(ref packet, buf, 2, out transfer);
            if (MainForm.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
            Thread.Sleep(variables.delay);
            MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
            if (MainForm.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
            //Console.WriteLine("Status: 0x{0:X}", readBuffer[0]);

            if (readBuffer[0] == 0x60 || readBuffer[0] == 0x60) Console.WriteLine("Flash Erased..");
            else if (readBuffer[0] == 0x40 || readBuffer[0] == 0x40) Console.WriteLine("Flash Erased..");
            else Console.WriteLine("Flash Erase Failed !");
        }
        private void ISD_Write_Flash()
        {
            long filesize;
            FileInfo fl = new FileInfo(txtFile.Text);
            filesize = fl.Length;
            if (!File.Exists(txtFile.Text))
            {
                Console.WriteLine("Image file not found");
                return;
            }
            if (filesize != 0xB000)
            {
                Console.WriteLine("Image file must be 44Kb");
                return;
            }
            BinaryReader rw = new BinaryReader(File.Open(txtFile.Text, FileMode.Open, FileAccess.Read));

            Console.WriteLine("Writing to Flash..");

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

            byte[] RES = { 0x60, 0x60, 0x60, 0x60, 0x60, 0x60, 0x60, 0x60 };
            byte[] CMD = new byte[8];
            CMD[0] = 0xA0;

            for (int i = 0; i < 0xB000; i += 4)
            {
                if (variables.escapeloop) break;
                progressBar1.Value = (i * progressBar1.Maximum) / 0xB000;

                byte[] buffer = rw.ReadBytes(0x4);
                Buffer.BlockCopy(buffer, 0, CMD, 4, 4);

                CMD[3] = (byte)(i & 0x00ff);
                CMD[2] = (byte)((i & 0xff00) >> 8);

                MyUsbDevice.ControlTransfer(ref packet, CMD, 8, out transfer);
                Thread.Sleep(variables.delay);
                MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
                if (NandX.ByteArrayCompare(readBuffer, RES, 5))
                {
                }
                else
                {
                    i -= 4;
                    Console.Write(":");
                }
            }
            progressBar1.Value = progressBar1.Maximum;
            rw.Close();
        }
        private void ISD_Verify_Flash()
        {
            long filesize;
            FileInfo fl = new FileInfo(txtFile.Text);
            filesize = fl.Length;
            if (!File.Exists(txtFile.Text))
            {
                Console.WriteLine("Image file not found");
                return;
            }
            if (filesize != 0xB000)
            {
                Console.WriteLine("Image file must be 44Kb");
                return;
            }
            BinaryReader rw = new BinaryReader(File.Open(txtFile.Text, FileMode.Open, FileAccess.Read));
            byte[] file = rw.ReadBytes(0xB000);
            rw.Close();
            Console.WriteLine("Verifing with Flash..");

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
                if (variables.escapeloop) break;
                progressBar1.Value = ((i) * progressBar1.Maximum) / 0xB000;

                CMD[3] = (byte)(i & 0x00ff);
                CMD[2] = (byte)((i & 0xff00) >> 8);

                MyUsbDevice.ControlTransfer(ref packet, CMD, 8, out transfer);
                Thread.Sleep(variables.delay);
                MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
                if (NandX.ByteArrayCompare(readBuffer, RES, 4))
                {
                    Buffer.BlockCopy(readBuffer, 4, data, i, 4);
                    fails = 0;
                }
                else
                {
                    if (fails == 6)
                    {
                        Console.WriteLine("\nRead Failed..\n");
                        break;
                    }
                    i -= 4;
                    Console.Write(":");
                    fails++;
                }
            }
            progressBar1.Value = progressBar1.Maximum;
            if (NandX.ByteArrayCompare(file, data)) Console.WriteLine("VERIFIED! Flash matches File..");
            else Console.WriteLine("Verify Failed !!..");
        }

        private void ISD_Play_Power()
        {
            UsbSetupPacket packet = new UsbSetupPacket();

            byte[] buf = { 0xA6, 0x00, 0x05 };
            int transfer = 0x08;

            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x30;
            packet.RequestType = (byte)UsbRequestType.TypeVendor;

            ///Access Flash
            Console.WriteLine("Playing Power...");
            MyUsbDevice.ControlTransfer(ref packet, buf, 3, out transfer);
            if (MainForm.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
        }
        private void ISD_Play_Eject()
        {
            UsbSetupPacket packet = new UsbSetupPacket();

            byte[] buf = { 0xA6, 0x00, 0x06 };
            int transfer = 0x08;

            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x30;
            packet.RequestType = (byte)UsbRequestType.TypeVendor;

            ///Access Flash
            Console.WriteLine("Playing Eject...");
            MyUsbDevice.ControlTransfer(ref packet, buf, 3, out transfer);
            if (MainForm.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
        }

        /// <summary>
        /// 1 - read
        /// 2 - write
        /// 3 - verify
        /// 4 - play power
        /// 5 - play eject
        /// </summary>
        /// <param name="function"></param>
        private void do_stuff(ISD_Function fun)
        {
            if (MyUsbDevice != null && MyUsbDevice.IsOpen)
            {
                Console.WriteLine("Device Already in Use");
                return;
            }
            try
            {
                MyUsbDevice = UsbDevice.OpenUsbDevice(JRunner);
                if (MyUsbDevice == null)
                {
                    return;
                }
                IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                if (!ReferenceEquals(wholeUsbDevice, null))
                {
                    wholeUsbDevice.SetConfiguration(1);
                    if (MainForm.debugme) Console.WriteLine("Claiming Interface...");
                    wholeUsbDevice.ClaimInterface(0);
                    if (MainForm.debugme) Console.WriteLine("The Interface is ours!");
                }

                Stopwatch stopwatch = new Stopwatch();
                if (Intro() == 0)
                {
                    Console.WriteLine("SPI Init Failed !");
                    return;
                }
                Thread.Sleep(50);
                if (GetID() == 0)
                {
                    Console.WriteLine("SPI ID Failed !");
                    return;
                }
                Thread.Sleep(50);
                enable(false);

                stopwatch.Start();

                switch (fun)
                {
                    case ISD_Function.read:
                        ISD_Read_Flash();
                        if (veraftreadchk.Checked)
                        {
                            Thread.Sleep(250);
                            ISD_Verify_Flash();
                        }
                        break;
                    case ISD_Function.write:
                        ISD_Erase_Flash();
                        Thread.Sleep(250);
                        ISD_Write_Flash();
                        if (veraftreadchk.Checked)
                        {
                            Thread.Sleep(250);
                            ISD_Verify_Flash();
                        }
                        break;
                    case ISD_Function.verify:
                        ISD_Verify_Flash();
                        break;
                    case ISD_Function.play_power:
                        ISD_Play_Power();
                        Thread.Sleep(2500);
                        break;
                    case ISD_Function.play_eject:
                        ISD_Play_Eject();
                        Thread.Sleep(2500);
                        break;
                    default:
                        Console.WriteLine("You shouldn't be here");
                        break;
                }

                stopwatch.Stop();
                Console.WriteLine("Done!");
                Console.WriteLine("in {0}:{1:D2} min:sec", stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);
                Console.WriteLine("");
                Outro();
                Normal();
                enable(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                if (MainForm.debugme) Console.WriteLine(ex.ToString());
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
            return;
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtFile.Text)) loadfile();
            ThreadStart starter = delegate { do_stuff(ISD_Function.read); };
            new Thread(starter).Start();
        }
        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtFile.Text)) loadfile();
            ThreadStart starter = delegate { do_stuff(ISD_Function.verify); };
            new Thread(starter).Start();
        }
        private void btnWrite_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtFile.Text)) loadfile();
            ThreadStart starter = delegate { do_stuff(ISD_Function.write); };
            new Thread(starter).Start();
        }
        private void btnPlayPower_Click(object sender, EventArgs e)
        {
            ThreadStart starter = delegate { do_stuff(ISD_Function.play_power); };
            new Thread(starter).Start();
        }
        private void btnPlayEject_Click(object sender, EventArgs e)
        {
            ThreadStart starter = delegate { do_stuff(ISD_Function.play_eject); };
            new Thread(starter).Start();
        }

        private void enable(bool what)
        {
            if (txtFile.Enabled && what) btnFile.Enabled = what;
            btnPlayEject.Enabled = what;
            btnPlayPower.Enabled = what;
            btnRead.Enabled = what;
            btnWrite.Enabled = what;
            btnVerify.Enabled = what;
        }
        private void loadfile()
        {
            rbtnOther.Checked = true;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "(*.bin)|*.bin|All files (*.*)|*.*";
            openFileDialog1.Title = "Select a File";
            openFileDialog1.CheckFileExists = false;
            
            //openFileDialog1.InitialDirectory = variables.currentdir;
            openFileDialog1.RestoreDirectory = false;
            //openFileDialog1.ShowDialog();
            
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                this.txtFile.Text = openFileDialog1.FileName;
            }
            else enable(true);  
        }
        private void btnFile_Click(object sender, EventArgs e)
        {
            loadfile();
        }
        private void txtFile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }
        private void txtFile_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            this.txtFile.Text = s[0];
        }

        private void rbtnOther_CheckedChanged(object sender, EventArgs e)
        {
            txtFile.Text = "";
            txtFile.Enabled = rbtnOther.Checked;
            btnFile.Enabled = rbtnOther.Checked;
            btnRead.Enabled = true;
          
        }
        private void rbtnFile_CheckedChanged(object sender, EventArgs e)
        {
            string filename = "";
            if (rbtnGOW3.Checked)
            {
                filename = "GOW3.bin";
                btnRead.Enabled = false;
            }
            else if (rbtnMW3.Checked) 
            {
                filename = "MW3.bin";
                btnRead.Enabled = false;
            }
            else if (rbtnStarWars.Checked) 
            {
                filename = "SW.bin";
                btnRead.Enabled = false;
            }
            else if (rbtnHalo.Checked)
            {
                filename = "Halo3.bin";
                btnRead.Enabled = false;
            }
            else if (rbtnStandard.Checked) 
            {
                filename = "Standard.bin";
                btnRead.Enabled = false;
            }

            txtFile.Text = Path.Combine(variables.pathforit, "common", "sounds", filename);
            txtFile.SelectionStart = txtFile.Text.Length;
            txtFile.ScrollToCaret();
        }

        private void SoundEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                variables.escapeloop = true;
                ThreadStart starter = delegate { escapedloop(); };
                new Thread(starter).Start();
            }
        }
        void escapedloop()
        {
            Thread.Sleep(2000);
            variables.escapeloop = false;
        }

        private void veraftreadchk_CheckedChanged(object sender, EventArgs e)
        {
            if (veraftreadchk.Checked == true) btnVerify.Enabled = false;
            else btnVerify.Enabled = true;
        }
    }
}
