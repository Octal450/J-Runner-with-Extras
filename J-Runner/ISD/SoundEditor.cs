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
using System.Threading.Tasks;
using EricOulashin;
using WaveLib;
using Yeti.MMedia;
using System.Media;


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
            play_eject,
            play_custom1,
            play_custom2,
            play_custom3
        }

        private void coolfun()
        {
            for (int i = 25; i > 0; i--)
            {
                this.pictureBox1.Visible = false;
                Thread.Sleep(i * 5);
                this.pictureBox1.Visible = true;
                Thread.Sleep(i * 5);
            }
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
            txtOutput.AppendText("Sending Flash Init...\n");
            MyUsbDevice.ControlTransfer(ref packet, buf, 1, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);

            MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
            txtOutput.AppendText("Status: 0x" +  readBuffer[0] + "\n");

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
            byte[] ID_2 = { 0x03, 0xEF, 0x20, 0x10 };
            byte[] ID_3 = { 0x03, 0xEF, 0x20, 0x11 };

            txtOutput.AppendText("Sending Flash ID...\n");
            MyUsbDevice.ControlTransfer(ref packet, buf, 5, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);

            MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
            if (Oper.ByteArrayCompare(ID, Oper.returnportion(readBuffer, 1, 4)))
            {
                txtOutput.AppendText("ISD-2110 detected\n");
                return 1;
            }
            else if (Oper.ByteArrayCompare(ID_2, Oper.returnportion(readBuffer, 1, 4)))
            {
                txtOutput.AppendText("ISD-2115 detected\n");
                return 1;
            }
            else if (Oper.ByteArrayCompare(ID_3, Oper.returnportion(readBuffer, 1, 4)))
            {
                txtOutput.AppendText("ISD-2130 detected\n");
                return 1;
            }
            else
            {
                txtOutput.AppendText("no ISD chip detected\n");
                txtOutput.AppendText("In " + Oper.ByteArrayToString(readBuffer) + "\n");
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

            txtOutput.AppendText("Sending Flash de-Init...\n");
            MyUsbDevice.ControlTransfer(ref packet, buf, 1, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);

            MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
            txtOutput.AppendText("Status: 0x" + readBuffer[0] + "\n");



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

            txtOutput.AppendText("Setting ISD to normal mode..\n");
            MyUsbDevice.ControlTransfer(ref packet, buf, 1, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);

            MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
            txtOutput.AppendText("Status: 0x" + readBuffer[0] + "\n");
        }

        private void ISD_Read_Flash(string filename)
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
                File.Delete(filename);
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); };

            BinaryWriter sw = new BinaryWriter(File.Open(filename, FileMode.Append, FileAccess.Write));

            txtOutput.AppendText("Reading from Flash..\n");

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
                if (Oper.ByteArrayCompare(readBuffer, RES, 4))
                {
                    sw.Write(Oper.returnportion(readBuffer, 4, 4));
                    fails = 0;
                }
                else
                {
                    if (fails == 6)
                    {
                        txtOutput.AppendText("\nRead Failed..\n");
                        break;
                    }
                    i -= 4;
                    Console.Write("X");
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
            txtOutput.AppendText("Sending Flash Erase...\n");
            MyUsbDevice.ControlTransfer(ref packet, buf, 2, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
            Thread.Sleep(variables.delay);
            MyUsbDevice.ControlTransfer(ref packetread, readBuffer, 8, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
            //Console.WriteLine("Status: 0x{0:X}", readBuffer[0]);

            if (readBuffer[0] == 0x60 || readBuffer[0] == 0x60) txtOutput.AppendText("Flash Erased..\n");
            else if (readBuffer[0] == 0x40 || readBuffer[0] == 0x40) txtOutput.AppendText("Flash Erased..\n");
            else txtOutput.AppendText("Flash Erase Failed !\n");
        }
        private void ISD_Write_Flash(string filename)
        {
            long filesize;
            FileInfo fl = new FileInfo(filename);
            filesize = fl.Length;
            if (!File.Exists(filename))
            {
                txtOutput.AppendText("Image file not found\n");
                return;
            }
            if (filesize != 0xB000)
            {
                txtOutput.AppendText("Image file must be 44Kb\n");
                return;
            }
            try
            {
                BinaryReader rw = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));

                txtOutput.AppendText("Writing to Flash.." + filename + "\n");

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
                    if (variables.escapeloop) break;
                    progressBar1.Value = (i * progressBar1.Maximum) / 0xB000;
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
                        txtOutput.AppendText("byte mismatch, retrying..\n");
                        if (variables.debugme) Console.WriteLine("Slowing down...");
                        if (variables.debugme) Console.WriteLine("additional delay now set to :" + AddPause);

                        if (AddPause > 16)
                        {
                            variables.escapeloop = true;
                            txtOutput.AppendText("Write Failed\n");
                            ThreadStart starter = delegate { escapedloop(); };
                            new Thread(starter).Start();
                            break;
                        }
                    }
                    //if (!NandX.ByteArrayCompare(readBuffer, RES, 5))
                    //{
                    //    i -= 4;
                    //    Console.Write("X");
                    //}
                }

                if (stayPause > 0) Console.WriteLine("Consider changing delay in setting page to : " + (variables.delay + 1));
                progressBar1.Value = progressBar1.Maximum;
                rw.Close();
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
        }
        private void ISD_Verify_Flash(string filename)
        {
            long filesize;
            FileInfo fl = new FileInfo(filename);
            filesize = fl.Length;
            if (!File.Exists(filename))
            {
                txtOutput.AppendText("Image file not found\n");
                return;
            }
            if (filesize != 0xB000)
            {
                txtOutput.AppendText("Image file must be 44Kb\n");
                return;
            }
            BinaryReader rw = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));
            byte[] file = rw.ReadBytes(0xB000);
            rw.Close();
            txtOutput.AppendText("Verifying with Flash..\n");

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
                        variables.escapeloop = true;

                        new Thread(escapedloop).Start();
                        break;
                    }
                    fails++;
                } while (!Oper.ByteArrayCompare(readBuffer, RES, 4));
            }
            progressBar1.Value = progressBar1.Maximum;

            if (Oper.ByteArrayCompare(file, data)) txtOutput.AppendText("VERIFIED! Flash matches File..\n");
            else txtOutput.AppendText("Verify Failed !!..\n");
        }

        private void ISD_Play(byte[] buf)
        {
            UsbSetupPacket packet = new UsbSetupPacket();

            int transfer = 0x08;

            packet.Value = 0x00;
            packet.Index = 0x00;
            packet.Length = 0x8;
            packet.Request = 0x30;
            packet.RequestType = (byte)UsbRequestType.TypeVendor;

            ///Access Flash
            txtOutput.AppendText("Playing...\n");
            MyUsbDevice.ControlTransfer(ref packet, buf, 3, out transfer);
            if (variables.debugme) Console.WriteLine("Length Transferred: {0}", transfer);
        }

        /// <summary>
        /// 1 - read
        /// 2 - write
        /// 3 - verify
        /// 4 - play power
        /// 5 - play eject
        /// 6 - play custom 1
        /// 7 - play custom 2
        /// 8 - play custom 3
        /// </summary>
        /// <param name="function"></param>
        private void do_stuff(ISD_Function fun, string filename)
        {
            if (MyUsbDevice != null && MyUsbDevice.IsOpen)
            {
                txtOutput.AppendText("Device Already in Use\n");
                return;
            }
            try
            {
                MyUsbDevice = UsbDevice.OpenUsbDevice(JRunner);
                if (MyUsbDevice == null)
                {
                    if (variables.debugme) Console.WriteLine("null device...");
                    return;
                }
                IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                if (!ReferenceEquals(wholeUsbDevice, null))
                {
                    wholeUsbDevice.SetConfiguration(1);
                    if (variables.debugme) Console.WriteLine("Claiming Interface...");
                    wholeUsbDevice.ClaimInterface(0);
                    if (variables.debugme) Console.WriteLine("The Interface is ours!");
                }

                Stopwatch stopwatch = new Stopwatch();
                if (Intro() == 0)
                {
                    txtOutput.AppendText("SPI Init Failed !\n");
                    return;
                }
                Thread.Sleep(50);
                if (GetID() == 0)
                {
                    txtOutput.AppendText("SPI ID Failed !\n");
                    return;
                }
                Thread.Sleep(50);
                enable(false);

                stopwatch.Start();

                switch (fun)
                {
                    case ISD_Function.read:
                        ISD_Read_Flash(filename);
                        if (veraftreadchk.Checked)
                        {
                            Thread.Sleep(250);
                            ISD_Verify_Flash(filename);
                        }
                        break;
                    case ISD_Function.write:
                        ISD_Erase_Flash();
                        Thread.Sleep(250);
                        ISD_Write_Flash(filename);
                        if (veraftreadchk.Checked)
                        {
                            Thread.Sleep(250);
                            ISD_Verify_Flash(filename);
                        }
                        break;
                    case ISD_Function.verify:
                        ISD_Verify_Flash(filename);
                        break;
                    case ISD_Function.play_power:
                        ISD_Play(new byte[] { 0xA6, 0x00, 0x05 });
                        Thread.Sleep(2500);
                        break;
                    case ISD_Function.play_eject:
                        ISD_Play(new byte[] { 0xA6, 0x00, 0x06 });
                        Thread.Sleep(2500);
                        break;
                    case ISD_Function.play_custom1:
                        ISD_Play(new byte[] { 0xB0, 0x00, 0x03 });
                        Thread.Sleep(2500);
                        break;
                    case ISD_Function.play_custom2:
                        ISD_Play(new byte[] { 0xB0, 0x00, 0x04 });
                        Thread.Sleep(2500);
                        break;
                    case ISD_Function.play_custom3:
                        ISD_Play(new byte[] { 0xB0, 0x00, 0x05 });
                        Thread.Sleep(2500);
                        break;
                    default:
                        txtOutput.AppendText("You shouldn't be here\n");
                        break;
                }

                stopwatch.Stop();
                txtOutput.AppendText("Done\n");
                txtOutput.AppendText("in " +  stopwatch.Elapsed.Minutes + ":" + stopwatch.Elapsed.Seconds + "  min:sec\n");
                //Console.WriteLine("");
                Outro();
                Normal();
                enable(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                if (variables.debugme) Console.WriteLine(ex.ToString());
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
            if (String.IsNullOrEmpty(txtFile.Text)) return;
            ThreadStart starter = delegate { do_stuff(ISD_Function.read, txtFile.Text); };
            new Thread(starter).Start();
        }
        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtFile.Text)) loadfile();
            if (String.IsNullOrEmpty(txtFile.Text)) return;
            ThreadStart starter = delegate { do_stuff(ISD_Function.verify, txtFile.Text); };
            new Thread(starter).Start();
        }
        private void btnWrite_Click(object sender, EventArgs e)
        {
            ThreadStart starter = delegate { do_stuff(ISD_Function.write, txtFile.Text); };
            new Thread(starter).Start();
        }
        private void btnPlayPower_Click(object sender, EventArgs e)
        {
            ThreadStart starter = delegate { do_stuff(ISD_Function.play_power, ""); };
            new Thread(starter).Start();
        }
        private void btnPlayEject_Click(object sender, EventArgs e)
        {
            ThreadStart starter = delegate { do_stuff(ISD_Function.play_eject, ""); };
            new Thread(starter).Start();
        }
        private void btnCustom1_Click(object sender, EventArgs e)
        {
            ThreadStart starter = delegate { do_stuff(ISD_Function.play_custom1, ""); };
            new Thread(starter).Start();
        }
        private void btnCustom2_Click(object sender, EventArgs e)
        {
            ThreadStart starter = delegate { do_stuff(ISD_Function.play_custom2, ""); };
            new Thread(starter).Start();
        }
        private void btnCustom3_Click(object sender, EventArgs e)
        {
            ThreadStart starter = delegate { do_stuff(ISD_Function.play_custom3, ""); };
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
            btnCustom1.Enabled = what;
            btnCustom2.Enabled = what;
            btnCustom3.Enabled = what;
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

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
            groupBox2.Enabled = rbtnOther.Checked;
            txtOutput.Enabled = true;

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
            else if (rbtnHalo4.Checked)
            {
                filename = "Halo4.bin";
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

        private void SoundEditor_Load(object sender, EventArgs e)
        {
            new Thread(coolfun).Start();
            updateLogColor();
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (((txtStart.Text != "") & (Path.GetExtension(txtStart.Text) == ".bin")) & ((txtEject.Text != "") & (Path.GetExtension(txtEject.Text) == ".bin")))
            {
                convert(txtStart.Text, txtEject.Text, txtPower.Text, Path.GetDirectoryName(Application.ExecutablePath));
            }
            else txtOutput.AppendText(".bin files required\n");
        }

        private void btnStartOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "(*.bin;*.wav)|*.bin;*.wav|All files (*.*)|*.*";
            openFileDialog1.Title = "Select a File";
            //openFileDialog1.InitialDirectory = variables.currentdir;
            openFileDialog1.RestoreDirectory = false;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtStart.Text = openFileDialog1.FileName;
            }

        }
        private void btnEjectOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "(*.bin;*.wav)|*.bin;*.wav|All files (*.*)|*.*";
            openFileDialog1.Title = "Select a File";
            //openFileDialog1.InitialDirectory = variables.currentdir;
            openFileDialog1.RestoreDirectory = false;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtEject.Text = openFileDialog1.FileName;
            }
            
        }
        private void btnPowerOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "(*.bin;*.wav)|*.bin;*.wav|All files (*.*)|*.*";
            openFileDialog1.Title = "Select a File";
            //openFileDialog1.InitialDirectory = variables.currentdir;
            openFileDialog1.RestoreDirectory = false;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPower.Text = openFileDialog1.FileName;
            }
            
        }

        private void convert(string infolderstart, string infoldereject, string infolderpower, string outfolder)
        {
            
            string outname = Path.Combine(variables.outfolder, txtFinalbin.Text + ".bin");
            if (txtFinalbin.Text == null)
            {
                outname = Path.Combine(variables.outfolder, "Custom.bin");
            }

            if (txtPower.Enabled == false) infolderpower = infolderstart;

            byte[] start = File.ReadAllBytes(infolderstart);
            byte[] eject = File.ReadAllBytes(infoldereject);
            byte[] power = new byte[0];
            if ((txtPower.Enabled) & (Path.GetExtension(txtPower.Text) == ".bin"))
            {

                power = File.ReadAllBytes(infolderpower);
            }
            else
            {

                power = start;
            }
            byte[] file = generate(start, eject, power);
            if (file != null)
            {
                File.WriteAllBytes(outname, generate(start, eject, power));
                txtOutput.AppendText(txtFinalbin.Text + ".bin Created successfully\n");
                txtFile.Text = outname; 
                
                
            }
            else txtOutput.AppendText("Failed to create " + txtFinalbin.Text + ".bin\n");


            // }
        }

        private byte[] generate(byte[] start, byte[] ej, byte[] power)
        {
            ISD_NAND ISN = new ISD_NAND();

            
            txtOutput.AppendText("Checking file..\n");

            if (radioButton1.Checked)
            {
                if (start.Length + ej.Length > 44800)
                {
                    txtOutput.AppendText("Start Up Sound size " + start.Length.ToString("X") + "\n");
                    txtOutput.AppendText("Eject Sound size " + ej.Length.ToString("X") + "\n");
                    txtOutput.AppendText("Files are too big. Combined files must be less than 44800 bytes\n");

                    return null;
                }
                else
                {
                    return ISN.generate_isd_bin(start, ej);
                }
            }
            else if (radioButton2.Checked)
            {
                if (start.Length + ej.Length + power.Length > 44800)
                {
                    txtOutput.AppendText("Start Up Sound size " + start.Length.ToString("X") + "\n");
                    txtOutput.AppendText("Eject Sound size " + ej.Length.ToString("X") + "\n");
                    txtOutput.AppendText("Shutdown Sound size " + power.Length.ToString("X") + "\n");
                    txtOutput.AppendText("Files are too big. Combined files must be less than 44800 bytes\n");

                    return null;
                }
                else
                {
                    return ISN.generate_isd_bin(start, power, ej);
                }
            }
            else
            {
                if (start.Length + ej.Length + power.Length > 44800)
                {
                    txtOutput.AppendText("Start Up Sound size " + start.Length.ToString("X") + "\n");
                    txtOutput.AppendText("Eject Sound size " + ej.Length.ToString("X") + "\n");
                    txtOutput.AppendText("Glitch Sound size " + power.Length.ToString("X") + "\n");
                    txtOutput.AppendText("Files are too big. Combined files must be less than 44800 bytes\n");

                    return null;
                }
                else
                {
                    return ISN.generate_isd_bin1(start, power, ej);
                }
            }
        }

        private void txtStart_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            txtStart.Text = s[0];
            
        }
        private void txtStart_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
           
        }
        private void txtEject_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
           
        }
        private void txtEject_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            txtEject.Text = s[0];
            
        }
        private void txtPower_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            txtPower.Text = s[0];
            
        }
        private void txtPower_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
           
        }
        private void checkTexts()
        {
            if ((File.Exists(txtStart.Text) && Path.GetExtension(txtStart.Text) == ".wav") || (File.Exists(txtEject.Text) && Path.GetExtension(txtEject.Text) == ".wav") || (File.Exists(txtPower.Text) && Path.GetExtension(txtPower.Text) == ".wav"))
            {
                btnConvert.Enabled = true;
            }
            else { btnConvert.Enabled = false; };
            if ((File.Exists(txtStart.Text) && Path.GetExtension(txtStart.Text) == ".bin") && (File.Exists(txtEject.Text) && Path.GetExtension(txtEject.Text) == ".bin"))
            {

                if (txtPower.Enabled == true) 
                {
                   if (File.Exists(txtPower.Text) && Path.GetExtension(txtPower.Text) == ".bin")
                   {
                        btnStart.Enabled = true;
                   }
                   else {btnStart.Enabled = false;}
                }
                else
                {
                  btnStart.Enabled = true;  

                }
                
            }
            else { btnStart.Enabled = false; }
        }
        private void txtStart_TextChanged(object sender, EventArgs e)
        {
            checkTexts();
            if (File.Exists(txtStart.Text) && Path.GetExtension(txtStart.Text) == ".wav")
            { 
                btnStartupPlay.Visible = true;
                
            }
            else 
            { 
                btnStartupPlay.Visible = false; 
            }
        }

        private void txtEject_TextChanged(object sender, EventArgs e)
        {
            checkTexts();
            if (File.Exists(txtEject.Text) && Path.GetExtension(txtEject.Text) == ".wav")
            { btnEjectPlay.Visible = true; }
            else { btnEjectPlay.Visible = false; }
        }

        private void txtPower_TextChanged(object sender, EventArgs e)
        {
            checkTexts();
            if (File.Exists(txtPower.Text) && Path.GetExtension(txtPower.Text) == ".wav")
            { btnShutdownPlay.Visible = true; }
            else { btnShutdownPlay.Visible = false; }

        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            checkTexts();
            if (radioButton1.Checked)
            {
                label4.Enabled = false;
                txtPower.Text = null;
                txtPower.Enabled = false;
                btnPowerOpen.Enabled = false;
                groupBox4.Enabled = true;
                groupBox5.Enabled = false;
            }
            else if (radioButton2.Checked)
            {
                label4.Text = "Shutdown sound ";
                label4.Enabled = true;
                txtPower.Enabled = true;
                btnPowerOpen.Enabled = true;
                groupBox5.Enabled = true;
                groupBox4.Enabled = false;
            }
            else if (radioButton3.Checked)
            {
                label4.Text = "Glitch sound ";
                label4.Enabled = true;
                txtPower.Enabled = true;
                btnPowerOpen.Enabled = true;
                groupBox5.Enabled = true;
                groupBox4.Enabled = false;
            }
        }

        class ISD_NAND
        {
            /// <summary>
            /// Load Startup sound on array startup_sound
            /// Load eject sound on array eject_sound
            /// Length MAX of both files are 42000 byte
            /// </summary>
            public int count;
            public byte[] isd_bin = new byte[0xB000];
            public byte[] startup;
            public byte[] shutdown;
            //public byte[] glitch;
            public byte[] eject;
            public byte[] header;
            public byte[] table;

            private byte[] split_nible(int num)
            {
                byte[] bytes = new byte[2];
                bytes[1] = (byte)(num >> 8);
                bytes[0] = (byte)num;
                return bytes;
            }

            
        public byte[] wav_to_raw(byte[] wav)
        {
            byte[] raw = new byte[wav.Length];
            byte[] magic = { 0x64, 0x61, 0x74, 0x61 };
            byte[] length = new byte[4];
            int i = 0;
            int got = 0;
            int offset = 0;
            bool data = false;
            for (int counter = 0; counter < wav.Length; counter++)
            {
                if (!data)
                {
                    if (got >= 3)
                    {
                        data = true;
                        length[0] = wav[counter + 4];
                        length[1] = wav[counter + 3];
                        length[2] = wav[counter + 2];
                        length[3] = wav[counter + 1];
                        counter += 4;
                        offset = counter;
                    }
                    else
                    {
                        if (wav[counter] == magic[got]) got++;
                        else got = 0;
                    }
                }
                else
                {
                    if (counter >= bytearray_to_int(length) + offset)
                    {
                        break;
                    }
                    if ((i % 0xFFB) == 0)
                    {
                        Debug.WriteLine("{0:X}", i);
                        raw[i] = 0x78;
                        i++;
                    }
                    counter++;
                    raw[i] = wav[counter];
                    i++;
                }
            }
            byte[] temp = new byte[i];
            Buffer.BlockCopy(raw, 0, temp, 0, i);
            raw = temp;

            return raw;
        }
        public static int bytearray_to_int(byte[] value)
        {
            return Convert.ToInt32(ByteArrayToString(value), 16);
        }
        public static string ByteArrayToString(byte[] ba, int startindex = 0, int length = 0)
        {
            if (ba == null) return "";
            string hex = BitConverter.ToString(ba);
            if (startindex == 0 && length == 0) hex = BitConverter.ToString(ba);
            else if (length == 0 && startindex != 0) hex = BitConverter.ToString(ba, startindex);
            else if (length != 0 && startindex != 0) hex = BitConverter.ToString(ba, startindex, length);
            return hex.Replace("-", "");
        }
        public byte[] wav_to_wav(Stream S)
        {
            WaveStream ws = new WaveStream(S);
            WaveFormat wf = new WaveFormat(8000, 16, 1);
            MemoryStream mS = new MemoryStream();
            WaveWriter ww = new WaveWriter(mS, wf);
            byte[] buffer = new byte[ww.OptimalBufferSize];

            int read = 0;
            int actual = 0;
            long total = ws.Length;

            while ((read = ws.Read(buffer, 0, buffer.Length)) > 0)
            {
                ww.Write(buffer, 0, read);
                actual += read;
            }

            ww.Close();
            ws.Close();
            return mS.ToArray();
        }

            public byte[] generate_isd_bin(byte[] startup, byte[] eject)
            {
                ISD_NAND bin = new ISD_NAND();

                byte[] header = {
                                       0xCF, 0xB, 0x0, 0x0, 0x0, 0x59, 0x8A, 0x0, 0x7D,
                                       0x8A, 0x0, 0x7E, 0x8A, 0x0, 0x9C, 0x8A, 0x0, 0x9D,
                                       0x8A, 0x0, 0xBB, 0x8A, 0x0, 0xBC, 0x8A, 0x0, 0xC5,
                                       0x8A, 0x0, 0xC6, 0x8A, 0x0, 0xCF, 0x8A, 0x0, 0x3E,
                                       0x0, 0x0, 0x4C, 0x7A, 0x0, 0x4D, 0x7A, 0x0, 0x58,
                                       0x8A, 0x0, 0x0, 0xFF, 0xFF, 0x6B, 0x3B, 0x66, 0x44,
                                       0x62, 0x62, 0x3B, 0x1E, 0x92, 0xC3, 0x4C, 0x0
                                   };
                byte[] table = {
                                      0xB8, 0x1A, 0x0, 0xB8, 0x1B, 0x0, 0xB8, 0x19, 0x40, 0xB8,
                                      0x1D, 0x0, 0xB8, 0x1E, 0x28, 0xB8, 0x1F, 0x0, 0xB8, 0x0,
                                      0x64, 0xB8, 0x1, 0x20, 0xB8, 0x2, 0x44, 0xB8, 0x26, 0x3,
                                      0xB8, 0x3, 0x2, 0xB8, 0x2A, 0x4, 0x12, 0xB8, 0x1A, 0x0, 0xB8,
                                      0x1B, 0x0, 0xB8, 0x1E, 0x28, 0xB8, 0x1F, 0x0, 0xB8, 0x26, 0x3,
                                      0xB8, 0x2A, 0x4, 0xB8, 0x1, 0x20, 0xB8, 0x3, 0x2, 0xB8, 0x0,
                                      0x64, 0xB8, 0x2, 0x44, 0xFF, 0xB8, 0x1A, 0x0, 0xB8, 0x19, 0x0,
                                      0xB8, 0x1B, 0x0, 0xB8, 0x1E, 0x28, 0xB8, 0x1F, 0x0, 0xB8, 0x26,
                                      0x3, 0xB8, 0x2A, 0x4, 0xB8, 0x1, 0x40, 0xB8, 0x2, 0x44, 0xB8,
                                      0x3, 0x2, 0xFF, 0xB8, 0x3, 0x16, 0xA6, 0x0, 0x6, 0xFC, 0x0, 0x0,
                                      0x12, 0xB8, 0x3, 0x0, 0xA6, 0x0, 0x5, 0xFC, 0x0, 0x0, 0x12, 0x49,
                                      0x53, 0x44, 0x2D, 0x56, 0x50, 0x45, 0x20, 0x56, 0x65, 0x72, 0x20,
                                      0x32, 0x31, 0x30, 0x2E, 0x30, 0x30, 0x34, 0x35, 0x20, 0x30, 0x35,
                                      0x2F, 0x31, 0x31, 0x2F, 0x32, 0x30, 0x31, 0x31, 0x20, 0x20
                                  };
                bin.header = header;
                bin.table = table;
                bin.startup = startup;
                bin.eject = eject;
                bin.count = startup.Length + eject.Length;

                //Dim dato(1) As Integer
                byte[] dato = new byte[2];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start User Sound at Offset 0x3E" & vbCrLf
                dato = split_nible(bin.count + 0x3D);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End User Sound at Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                dato = split_nible(bin.count + 0x3E);

                //'-----EntryPoint TablePOI
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TablePOI Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[5] = dato[0];
                bin.header[6] = dato[1];
                dato = split_nible(bin.count + 0x3E + 0x24);
                bin.header[8] = dato[0];
                bin.header[9] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TablePOI Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----EntryPoint TablePU
                dato = split_nible(bin.count + 0x3E + 0x25);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TablePU Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0xB] = dato[0];
                bin.header[0xC] = dato[1];
                dato = split_nible(bin.count + 0x3E + 0x43);
                bin.header[0xE] = dato[0];
                bin.header[0xF] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TablePU Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----EntryPoint GPIO
                dato = split_nible(bin.count + 0x3E + 0x44);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TableGPIO Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0x11] = dato[0];
                bin.header[0x12] = dato[1];
                dato = split_nible(bin.count + 0x3E + 0x62);
                bin.header[0x14] = dato[0];
                bin.header[0x15] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TableGPIO Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----EntryPoint VM SCRIPT
                dato = split_nible(bin.count + 0x3E + 0x63);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TableVMScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0x17] = dato[0];
                bin.header[0x18] = dato[1];
                dato = split_nible(bin.count + 0x3E + 0x6C);
                bin.header[0x1A] = dato[0];
                bin.header[0x1B] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TableVMScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----EntryPoint MACRO SCRIPT
                dato = split_nible(bin.count + 0x3E + 0x6D);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TableMacroScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0x1D] = dato[0];
                bin.header[0x1E] = dato[1];
                dato = split_nible(bin.count + 0x3E + 0x76);
                bin.header[0x20] = dato[0];
                bin.header[0x21] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TableMacroScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----Startup Sound Pointer
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Startup Sound Start Offset 0x" & Convert.ToString(0x3E, 16) & vbCrLf
                bin.header[0x23] = 0x3E;
                //bin.header(0x24) = 0
                dato = split_nible(bin.startup.Length - 1 + 0x3E);
                bin.header[0x26] = dato[0];
                bin.header[0x27] = dato[1];
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Startup Sound End Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----Eject Sound Pointer
                dato = split_nible(bin.startup.Length + 0x3E);
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Eject Sound Start Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0x29] = dato[0];
                bin.header[0x2A] = dato[1];
                dato = split_nible(bin.startup.Length + bin.eject.Length - 1 + 0x3E);
                bin.header[0x2C] = dato[0];
                bin.header[0x2D] = dato[1];
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Eject Sound End Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------


                int i = 0;

                Buffer.BlockCopy(bin.header, 0, bin.isd_bin, i, bin.header.Length);
                i += bin.header.Length;

                Buffer.BlockCopy(bin.startup, 0, bin.isd_bin, i, bin.startup.Length);
                i += bin.startup.Length;

                Buffer.BlockCopy(bin.eject, 0, bin.isd_bin, i, bin.eject.Length);
                i += bin.eject.Length;

                Buffer.BlockCopy(bin.table, 0, bin.isd_bin, i, bin.table.Length);
                i += bin.table.Length;

                for (; i < bin.isd_bin.Length; i++)
                {
                    bin.isd_bin[i] = 0xFF;
                }

                return bin.isd_bin;
            }
            public byte[] generate_isd_bin(byte[] startup, byte[] power, byte[] eject)
            {
                ISD_NAND bin = new ISD_NAND();

                byte[] header = {
                                0xCF, 0x4, 0x0, 0x0, 0x0, 0x6D, 0x1C, 0x0, 0x88,
                                0x1C, 0x0, 0x89, 0x1C, 0x0, 0x8F, 0x1C, 0x0, 0x90,
                                0x1C, 0x0, 0x9C, 0x1C, 0x0, 0x9D, 0x1C, 0x0, 0xA3,
                                0x1C, 0x0, 0xA4, 0x1C, 0x0, 0xAA, 0x1C, 0x0, 0xAB, 0x1C, 0x0, 0xAE, 0x1C, 0x0, 0x4A,
                                0x0, 0x0, 0xAA, 0x9, 0x0, 0xAB, 0x9, 0x0, 0xB,
                                0x13, 0x0, 0xC, 0x13, 0x0, 0x1C, 0x6C, 0x0, 0x0, 0xFF, 0xFF, 0x50, 0x62, 0x2B, 0x6E,
                                0x3B, 0x50, 0x62, 0x1E, 0x0, 0x0, 0x0, 0x0
                            };
                byte[] table = {
                               0xB8, 0x2, 0x44, 0xB8, 0x3, 0x0, 0xB8, 0x1A, 0x0, 0xB8, 0x1B, 0x3F, 0xB8, 0x1D, 0x3F, 0xB8,
                                0x26, 0x5, 0xB8, 0x2A, 0x3, 0xB8, 0x1E, 0xFF, 0xB8, 0x1F, 0x0, 0x12, 0xB8, 0x2, 0x44, 0xB8, 0x3,
                                0x0, 0xFF, 0xB8, 0x1, 0x0, 0xB8, 0x2, 0x44, 0xB8, 0x3, 0x0, 0xB8, 0x1, 0x0, 0xFF, 0xB8, 0x2A,
                                0x4, 0xA6, 0x0, 0x6, 0x12, 0xB8, 0x2A, 0x3, 0xA6, 0x0, 0x7, 0x12, 0xA6, 0x0, 0x8, 0xFF, 0x49,
                                0x53, 0x44, 0x2D, 0x56, 0x50, 0x45, 0x20, 0x56, 0x65, 0x72, 0x20, 0x32, 0x31, 0x30, 0x2E, 0x30, 0x30,
                                0x34, 0x36, 0x20, 0x30, 0x39, 0x2F, 0x32, 0x38, 0x2F, 0x32, 0x30, 0x31, 0x32, 0x20, 0x20
                           };
                bin.header = header;
                bin.table = table;
                bin.startup = startup;
                bin.eject = eject;
                bin.shutdown = power;
                bin.count = startup.Length + eject.Length + bin.shutdown.Length;

                //Dim dato(1) As Integer
                byte[] dato = new byte[2];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start User Sound at Offset 0x3E" & vbCrLf
                dato = split_nible(bin.count + 0x49);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End User Sound at Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                dato = split_nible(bin.count + 0x4A);

                //'-----EntryPoint TablePOI
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TablePOI Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[5] = dato[0];
                bin.header[6] = dato[1];
                dato = split_nible(bin.count + 0x4A + 0x1B);
                bin.header[8] = dato[0];
                bin.header[9] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TablePOI Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----EntryPoint TablePU
                dato = split_nible(bin.count + 0x4A + 0x1C);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TablePU Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0xB] = dato[0];
                bin.header[0xC] = dato[1];
                dato = split_nible(bin.count + 0x4A + 0x22);
                bin.header[0xE] = dato[0];
                bin.header[0xF] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TablePU Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----EntryPoint GPIO
                dato = split_nible(bin.count + 0x4A + 0x23);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TableGPIO Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0x11] = dato[0];
                bin.header[0x12] = dato[1];
                dato = split_nible(bin.count + 0x4A + 0x2F);
                bin.header[0x14] = dato[0];
                bin.header[0x15] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TableGPIO Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----EntryPoint VM SCRIPT
                dato = split_nible(bin.count + 0x4A + 0x30);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TableVMScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0x17] = dato[0];
                bin.header[0x18] = dato[1];
                dato = split_nible(bin.count + 0x4A + 0x36);
                bin.header[0x1A] = dato[0];
                bin.header[0x1B] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TableVMScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----EntryPoint MACRO SCRIPT
                dato = split_nible(bin.count + 0x4A + 0x37);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TableMacroScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0x1D] = dato[0];
                bin.header[0x1E] = dato[1];
                dato = split_nible(bin.count + 0x4A + 0x3D);
                bin.header[0x20] = dato[0];
                bin.header[0x21] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TableMacroScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----EntryPoint LOOP SCRIPT
                dato = split_nible(bin.count + 0x4A + 0x3E);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TableMacroScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0x23] = dato[0];
                bin.header[0x24] = dato[1];
                dato = split_nible(bin.count + 0x4A + 0x41);
                bin.header[0x26] = dato[0];
                bin.header[0x27] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TableMacroScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----Startup Sound Pointer
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Startup Sound Start Offset 0x" & Convert.ToString(0x3E, 16) & vbCrLf
                bin.header[0x29] = 0x4A;
                //bin.header(0x2A) = 0
                dato = split_nible(bin.startup.Length - 1 + 0x4A);
                bin.header[0x2C] = dato[0];
                bin.header[0x2D] = dato[1];
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Startup Sound End Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----Shutdown Sound Pointer
                dato = split_nible(bin.startup.Length + 0x4A);
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Startup Sound Start Offset 0x" & Convert.ToString(0x3E, 16) & vbCrLf
                bin.header[0x2F] = dato[0];
                bin.header[0x30] = dato[1];
                dato = split_nible(bin.startup.Length + bin.shutdown.Length - 1 + 0x4A);
                bin.header[0x32] = dato[0];
                bin.header[0x33] = dato[1];
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Startup Sound End Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----Eject Sound Pointer
                dato = split_nible(bin.startup.Length + bin.shutdown.Length + 0x4A);
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Eject Sound Start Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0x35] = dato[0];
                bin.header[0x36] = dato[1];
                dato = split_nible(bin.startup.Length + bin.eject.Length + bin.shutdown.Length - 1 + 0x4A);
                bin.header[0x38] = dato[0];
                bin.header[0x39] = dato[1];
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Eject Sound End Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------


                int i = 0;

                Buffer.BlockCopy(bin.header, 0, bin.isd_bin, i, bin.header.Length);
                i += bin.header.Length;

                Buffer.BlockCopy(bin.startup, 0, bin.isd_bin, i, bin.startup.Length);
                i += bin.startup.Length;

                Buffer.BlockCopy(bin.shutdown, 0, bin.isd_bin, i, bin.shutdown.Length);
                i += bin.shutdown.Length;

                Buffer.BlockCopy(bin.eject, 0, bin.isd_bin, i, bin.eject.Length);
                i += bin.eject.Length;

                Buffer.BlockCopy(bin.table, 0, bin.isd_bin, i, bin.table.Length);
                i += bin.table.Length;

                for (; i < bin.isd_bin.Length; i++)
                {
                    bin.isd_bin[i] = 0xFF;
                }

                return bin.isd_bin;
            }
            public byte[] generate_isd_bin1(byte[] startup, byte[] power, byte[] eject)
            {
                ISD_NAND bin = new ISD_NAND();

                byte[] header = {0xCF, 0x4, 0x0, 0x0, 0x0, 0x6D, 0x1C, 0x0, 0x88,
                                       0x1C, 0x0, 0x89, 0x1C, 0x0, 0x8F, 0x1C, 0x0, 0x90,
                                       0x1C, 0x0, 0x9C, 0x1C, 0x0, 0x9D, 0x1C, 0x0, 0xA3,
                                       0x1C, 0x0, 0xA4, 0x1C, 0x0, 0xAA, 0x1C, 0x0, 0xAB, 0x1C, 0x0, 0xAE, 0x1C, 0x0, 0x4A,
                                       0x0, 0x0, 0xAA, 0x9, 0x0, 0xAB, 0x9, 0x0, 0xB,
                                       0x13, 0x0, 0xC, 0x13, 0x0, 0x1C, 0x6C, 0x0, 0x0, 0xFF, 0xFF, 0x50, 0x62, 0x2B, 0x6E,
                                       0x3B, 0x50, 0x62, 0x1E, 0x0, 0x0, 0x0, 0x0};
                byte[] table = {0xB8, 0x2, 0x44, 0xB8, 0x3, 0x0, 0xB8, 0x1A, 0x0, 0xB8, 0x1B, 0x3F, 0xB8, 0x1D, 0x3F, 0xB8,
                         0x26, 0x5, 0xB8, 0x28, 0x4, 0xB8, 0x2A, 0x3, 0xB8, 0x1E, 0xFF, 0xB8, 0x1F, 0x0, 0x12, 0xB8, 0x2, 0x44, 0xB8, 0x3,
                         0x0, 0xFF, 0xB8, 0x1, 0x0, 0xB8, 0x2, 0x44, 0xB8, 0x3, 0x0, 0xB8, 0x1, 0x0, 0xFF, 0xA6, 0x0, 0x6, 0x12, 0xA6, 0x0, 0x7, 0xFF, 0xA6, 0x0, 0x8, 0xFF, 0x49,
                         0x53, 0x44, 0x2D, 0x56, 0x50, 0x45, 0x20, 0x56, 0x65, 0x72, 0x20, 0x32, 0x31, 0x30, 0x2E, 0x30, 0x30,
                         0x34, 0x36, 0x20, 0x30, 0x39, 0x2F, 0x32, 0x38, 0x2F, 0x32, 0x30, 0x31, 0x32, 0x20, 0x20};
                bin.header = header;
                bin.table = table;
                bin.startup = startup;
                bin.eject = eject;
                bin.shutdown = power;
                bin.count = startup.Length + eject.Length + bin.shutdown.Length;

                //Dim dato(1) As Integer
                byte[] dato = new byte[2];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start User Sound at Offset 0x3E" & vbCrLf
                dato = split_nible(bin.count + 0x49);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End User Sound at Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                dato = split_nible(bin.count + 0x4A);

                //'-----EntryPoint TablePOI
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TablePOI Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[5] = dato[0];
                bin.header[6] = dato[1];
                dato = split_nible(bin.count + 0x4A + 0x1E);
                bin.header[8] = dato[0];
                bin.header[9] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TablePOI Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----EntryPoint TablePU
                dato = split_nible(bin.count + 0x4A + 0x1F);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TablePU Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0xB] = dato[0];
                bin.header[0xC] = dato[1];
                dato = split_nible(bin.count + 0x4A + 0x25);
                bin.header[0xE] = dato[0];
                bin.header[0xF] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TablePU Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----EntryPoint GPIO
                dato = split_nible(bin.count + 0x4A + 0x26);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TableGPIO Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0x11] = dato[0];
                bin.header[0x12] = dato[1];
                dato = split_nible(bin.count + 0x4A + 0x32);
                bin.header[0x14] = dato[0];
                bin.header[0x15] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TableGPIO Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----EntryPoint VM SCRIPT
                dato = split_nible(bin.count + 0x4A + 0x33);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TableVMScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0x17] = dato[0];
                bin.header[0x18] = dato[1];
                dato = split_nible(bin.count + 0x4A + 0x36);
                bin.header[0x1A] = dato[0];
                bin.header[0x1B] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TableVMScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----EntryPoint MACRO SCRIPT
                dato = split_nible(bin.count + 0x4A + 0x37);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TableMacroScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0x1D] = dato[0];
                bin.header[0x1E] = dato[1];
                dato = split_nible(bin.count + 0x4A + 0x3A);
                bin.header[0x20] = dato[0];
                bin.header[0x21] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TableMacroScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----EntryPoint LOOP SCRIPT
                dato = split_nible(bin.count + 0x4A + 0x3B);
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "Start EP TableMacroScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0x23] = dato[0];
                bin.header[0x24] = dato[1];
                dato = split_nible(bin.count + 0x4A + 0x3E);
                bin.header[0x26] = dato[0];
                bin.header[0x27] = dato[1];
                //'Form1.TextBox3.Text = Form1.TextBox3.Text & "End EP TableMacroScript Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----Startup Sound Pointer
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Startup Sound Start Offset 0x" & Convert.ToString(0x3E, 16) & vbCrLf
                bin.header[0x29] = 0x4A;
                //bin.header(0x2A) = 0
                dato = split_nible(bin.startup.Length - 1 + 0x4A);
                bin.header[0x2C] = dato[0];
                bin.header[0x2D] = dato[1];
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Startup Sound End Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----Shutdown Sound Pointer
                dato = split_nible(bin.startup.Length + 0x4A);
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Startup Sound Start Offset 0x" & Convert.ToString(0x3E, 16) & vbCrLf
                bin.header[0x2F] = dato[0];
                bin.header[0x30] = dato[1];
                dato = split_nible(bin.startup.Length + bin.shutdown.Length - 1 + 0x4A);
                bin.header[0x32] = dato[0];
                bin.header[0x33] = dato[1];
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Startup Sound End Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------

                //'-----Eject Sound Pointer
                dato = split_nible(bin.startup.Length + bin.shutdown.Length + 0x4A);
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Eject Sound Start Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                bin.header[0x35] = dato[0];
                bin.header[0x36] = dato[1];
                dato = split_nible(bin.startup.Length + bin.eject.Length + bin.shutdown.Length - 1 + 0x4A);
                bin.header[0x38] = dato[0];
                bin.header[0x39] = dato[1];
                //Form1.TextBox3.Text = Form1.TextBox3.Text & "Eject Sound End Offset 0x" & Convert.ToString(dato(1), 16) & Convert.ToString(dato(0), 16) & vbCrLf
                //'--------------------------


                int i = 0;

                Buffer.BlockCopy(bin.header, 0, bin.isd_bin, i, bin.header.Length);
                i += bin.header.Length;

                Buffer.BlockCopy(bin.startup, 0, bin.isd_bin, i, bin.startup.Length);
                i += bin.startup.Length;

                Buffer.BlockCopy(bin.shutdown, 0, bin.isd_bin, i, bin.shutdown.Length);
                i += bin.shutdown.Length;

                Buffer.BlockCopy(bin.eject, 0, bin.isd_bin, i, bin.eject.Length);
                i += bin.eject.Length;

                Buffer.BlockCopy(bin.table, 0, bin.isd_bin, i, bin.table.Length);
                i += bin.table.Length;

                for (; i < bin.isd_bin.Length; i++)
                {
                    bin.isd_bin[i] = 0xFF;
                }

                return bin.isd_bin;
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            {

                ISD_NAND ISN = new ISD_NAND();
                if ((txtStart.Text != "") & (Path.GetExtension(txtStart.Text) == ".wav"))
                {
                    WAVFormat w = WAVFile.GetAudioFormat(txtStart.Text);


                    if ((w.SampleRateHz == 8000) & (w.BitsPerSample == 16))
                    {
                        string Fname = Path.GetFileNameWithoutExtension(txtStart.Text);
                        string outname = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Fname + ".bin");
                        txtOutput.AppendText(Path.GetExtension(txtStart.Text) + "\n");
                        byte[] start = File.ReadAllBytes(txtStart.Text);
                        txtOutput.AppendText("Compressing files.. Stage 2\n");
                        start = ISN.wav_to_raw(start);
                        File.WriteAllBytes(outname, start);
                        txtOutput.AppendText("Start.bin Created\n");
                        txtStart.Text = outname;
                    }
                    else
                    {
                        txtOutput.AppendText("Not valid .wav format for Start Sound\n");
                        txtOutput.AppendText("SampleRate: " + w.SampleRateHz.ToString() + "\n");
                        txtOutput.AppendText("Bits Per Sample: " + w.BitsPerSample.ToString() + "\n");
                        txtOutput.AppendText("Stereo: " + w.IsStereo.ToString() + "\n");
                    }
                }
                else txtOutput.AppendText("No valid .wav file for Start Sound\n");
                Thread.Sleep(250);
                if ((txtEject.Text != "") & (Path.GetExtension(txtEject.Text) == ".wav"))
                {
                    WAVFormat w1 = WAVFile.GetAudioFormat(txtEject.Text);

                    if ((w1.SampleRateHz == 8000) & (w1.BitsPerSample == 16))
                    {
                        string Fname = Path.GetFileNameWithoutExtension(txtEject.Text);
                        string outname = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Fname + ".bin");

                        byte[] start = File.ReadAllBytes(txtEject.Text);
                        txtOutput.AppendText("Compressing files.. Stage 2\n");
                        start = ISN.wav_to_raw(start);
                        File.WriteAllBytes(outname, start);
                        txtOutput.AppendText("Eject.bin Created\n");
                        txtEject.Text = outname;
                    }
                    else
                    {
                        txtOutput.AppendText("Not valid .wav format for Eject Sound\n");
                        txtOutput.AppendText("SampleRate: " + w1.SampleRateHz.ToString() + "\n");
                        txtOutput.AppendText("Bits Per Sample: " + w1.BitsPerSample.ToString() + "\n");
                        txtOutput.AppendText("Stereo: " + w1.IsStereo.ToString() + "\n");
                    }
                }
                else txtOutput.AppendText("No valid .wav file for Eject Sound\n");
                Thread.Sleep(250);
                if ((txtPower.Enabled) & (Path.GetExtension(txtPower.Text) == ".wav") & (txtPower.Text != ""))
                {
                    WAVFormat w2 = WAVFile.GetAudioFormat(txtPower.Text);

                    if ((w2.SampleRateHz == 8000) & (w2.BitsPerSample == 16))
                    {
                        string Fname = Path.GetFileNameWithoutExtension(txtPower.Text);
                        string outname = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Fname + ".bin");

                        byte[] start = File.ReadAllBytes(txtPower.Text);
                        txtOutput.AppendText("Compressing files.. Stage 2\n");
                        start = ISN.wav_to_raw(start);
                        File.WriteAllBytes(outname, start);
                        txtOutput.AppendText("Power.bin Created\n");
                        txtPower.Text = outname;
                    }
                    else
                    {
                        if (txtPower.Enabled)
                        {
                            txtOutput.AppendText("Not valid .wav format for Power/Glitch Sound\n");
                            txtOutput.AppendText("SampleRate: " + w2.SampleRateHz.ToString() + "\n");
                            txtOutput.AppendText("Bits Per Sample: " + w2.BitsPerSample.ToString() + "\n");
                            txtOutput.AppendText("Stereo: " + w2.IsStereo.ToString() + "\n");
                        }
                    }
                }
                else
                {
                    if (txtPower.Enabled) txtOutput.AppendText("No valid .wav file for Power/Glitch Sound\n");
                }
            }
        }

        private void btnStartupPlay_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtStart.Text) && Path.GetExtension(txtStart.Text) == ".wav")
            {
                try
                {
                    SoundPlayer success = new SoundPlayer(txtStart.Text);
                    success.Play();
                }
                catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); };
            }
        }

        private void btnShutdownPlay_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtPower.Text) && Path.GetExtension(txtPower.Text) == ".wav")
            {
                try
                {
                    SoundPlayer success = new SoundPlayer(txtPower.Text);
                    success.Play();
                }
                catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); };
            }
        }

        private void btnEjectPlay_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtEject.Text) && Path.GetExtension(txtEject.Text) == ".wav")
            {
                try
                {
                    SoundPlayer success = new SoundPlayer(txtEject.Text);
                    success.Play();
                }
                catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); };
            }
        }



        public void updateLogColor()
        {
            txtOutput.BackColor = variables.logbackground;
            txtOutput.ForeColor = variables.logtext;
        }
    }
}
