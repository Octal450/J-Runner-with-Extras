using CommPort;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows.Forms;


namespace JRunner
{
    public partial class POST : Form
    {

        public TextWriter _writer = null;
        public Dictionary<int, string> POSTd = new Dictionary<int, string>();

        List<int> values;
        int nudges = 0;
        enum crash : int // BH - Listing crash values
        {
            x08 = 0,
            x09,
            x0B,
            x0C,
            x0D,
            x0E,
            x0F,
            x1C,
            x20,
            x21,
            x22,
            x58,
            xA0,
            xE0,
            xDA,
            xF0,
            xF1,
            xF2,
            xF3,
            x2E,
            x2F
        }
        public int[] crashVAL = new int[Enum.GetValues(typeof(crash)).Length]; //BH -  finding and counting crash values
        // 0x20 crashVAL[0]
        // 0x21 crashVAL[1]
        // 0xDA crashVAL[2]
        // 0xF0 crashVAL[3]
        // 0xF1 crashVAL[4]
        // 0xF2 crashVAL[5]
        // 0xF3 crashVAL[6]

        private bool stop = false, nudge = false, timeoutflag = false;
        private int counterg = 0, globalcounter = 0;
        private int timeout = 500;
        private string ConTypeSel = "";
        #region devices

        public List<string> versions = new List<string>(){
        "03000000",
        "01000000",
        "10000000"
        };

        private DateTime LastDataEventDate = DateTime.Now;
        private UsbDevice MyUsbDevice;
        private UsbDeviceFinder Arm = new UsbDeviceFinder(0xFFFF, 0x0004);
        private UsbDeviceFinder JRunner = new UsbDeviceFinder(0x11d4, 0x8338);
        CommunicationManager comm = new CommunicationManager();
        List<string> parity = new List<string>();
        List<string> stopbits = new List<string>();
        List<string> databits = new List<string>();
        List<string> comports = new List<string>();

        #endregion

        private bool CappeD = false; // BH - declares boolean for checking if rater was capped during use
        public volatile Boolean _bStop = false;
        public volatile Boolean Cooling = false;
        public volatile Boolean enough = false;
        public volatile Boolean Reset = false;
        public void SleepIntermittently(int totalTime)
        {
            int sleptTime = 0;
            int intermittentSleepIncrement = 10;
            // Wake up every 10 milli-seconds too check if we need
            // to stop or not
            txtShow.Visible = true;

            while (!_bStop && sleptTime < totalTime)
            {
                if (Cooling)
                {
                    txtShow.ForeColor = Color.MediumBlue;
                    txtShow.Text = "Cooling: " + ((totalTime - sleptTime) / 1000).ToString();
                }
                else
                {
                    txtShow.ForeColor = Color.DarkGreen;
                    txtShow.Text = "Shutdown: " + ((totalTime - sleptTime) / 1000).ToString();
                }
                Thread.Sleep(intermittentSleepIncrement);
                sleptTime += intermittentSleepIncrement;
            }
            txtShow.Text = "";
            txtShow.Visible = false;


        }

        public POST()
        {
            InitializeComponent();
            //#if Dev 
            //            PhatBut.Enabled = true;
            //            CorBut.Visible = true;
            //            CorBut.Enabled = true;
            //            PhatFBut.Enabled = true;
            //            PhatFBut.Visible = true;
            //#endif
            _writer = new TextBoxStreamWriter(txtOutput);
            Console.SetOut(_writer);
            comm.SetParityValues(ref parity);
            comm.SetStopBitValues(ref stopbits);
            comm.SetPortNameValues(ref comports);
            comm.CurrentTransmissionType = CommPort.CommunicationManager.TransmissionType.Text;


            if (comports.Contains(variables.COMPort)) comm.PortName = variables.COMPort;
            else if (comports.Count >= 1) comm.PortName = comports[0];
            if (parity.Count >= 1) comm.Parity = parity[0];
            if (stopbits.Count >= 2) comm.StopBits = stopbits[1];
            comm.DataBits = "8";
            comm.BaudRate = "115200";
        }
        [STAThread]
        private void DisplayData(string msg, int delete)
        {
            ThreadStart starter = delegate { DData(msg, delete); };
            new Thread(starter).Start();
        }
        private void DData(string msg, int delete)
        {
            txtRate.Invoke(new EventHandler(delegate
            {
                txtRate.SelectedText = string.Empty;
                txtRate.Text = txtRate.Text.Substring(0, txtRate.Text.Length - delete);
                txtRate.AppendText(msg);
                txtRate.ScrollToCaret();
            }));
        }
        private void Timeout(bool stop)
        {
            DisplayData(Environment.NewLine, 0); // BH - action upon Timeout 
            Console.WriteLine("TIMEOUT or SMC CORRUPTED - Shutting Down");

            if (!stop)
            {
                btnNudge.Enabled = false;
                nudge = true;
                nudges++;
            }
            else
            {
                timeoutflag = false;
                _bStop = true;
                variables.escapeloop = true;
                stop = true;
                buttons(true);
                ThreadStart starter = delegate { escapedloop(5000); };
                new Thread(starter).Start();      // BH - action upon timeout
            }
        }

        private void DisplayData2(string msg)
        {
            ThreadStart starter = delegate { DData2(msg); };
            new Thread(starter).Start();
        }
        private void DData2(string msg)
        {
            txtProgress.Invoke(new EventHandler(delegate
            {
                txtProgress.Text = msg;
            }));
        }

        private int Pwr(UsbEndpointReader reader)
        {
            try
            {
                ErrorCode ec = ErrorCode.None;
                UsbSetupPacket packet = new UsbSetupPacket();
                packet.RequestType = (byte)UsbRequestType.TypeVendor;
                byte[] readBuffer = new byte[4];
                int bytesRead = 0;

                packet.Value = 0x00;
                packet.Index = 0x00;
                packet.Length = 0x8;
                packet.Request = 0x10;
                int LengthTransferred = 0x10;
                byte[] buffer = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out LengthTransferred);
                if (variables.debugme) Console.WriteLine("Length Transferred {0}", LengthTransferred);
                Console.WriteLine("Power Up");
                btnNudge.Enabled = true;
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                if (variables.debugme) Console.WriteLine(ec.ToString());
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
            }
            return 0;
        }
        private int comPwr()
        {
            try
            {
                if (!comm.OpenPort()) return 1;

                for (int i = 0; i < 4; i++)
                {
                    Thread.Sleep(256);
                    comm.DtrEnable = false;
                    Thread.Sleep(256);
                    comm.DtrEnable = true;
                }

                comm.ClosePort();
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            return 0;
        }
        private int ShutD(UsbEndpointReader reader)
        {
            try
            {
                ErrorCode ec = ErrorCode.None;
                byte[] readBuffer = new byte[4];
                int bytesRead = 0;
                UsbSetupPacket packet = new UsbSetupPacket();
                packet.RequestType = (byte)UsbRequestType.TypeVendor;

                packet.Value = 0x00;
                packet.Index = 0x00;
                packet.Length = 0x8;
                packet.Request = 0x11;
                int LengthTransferred = 0x10;
                byte[] buffer = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };


                ///Arm version
                MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out LengthTransferred);
                if (variables.debugme) Console.WriteLine("Length Transferred {0}", LengthTransferred);
                Console.WriteLine("Shutdown");
                Reset = false;
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                if (variables.debugme) Console.WriteLine(ec.ToString());
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                if (variables.debugme) { Console.WriteLine(ex.ToString()); }
            }
            return 0;
        }
        private int comShutD()
        {
            return 0;
        }

        private int log_post(UsbEndpointReader reader)
        {
            try
            {
                UsbSetupPacket packet = new UsbSetupPacket();

                packet.RequestType = (byte)UsbRequestType.TypeVendor;

                packet.Value = 0x00;
                packet.Index = 0x00;
                packet.Length = 0x8;
                packet.Request = 0x8;
                int length = 0x10;
                byte[] buffer = { 0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00 };


                Console.WriteLine("Waiting for POST to change");

                packet.Request = 0xA;
                MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out length);
                packet.Request = 0xB;

                get_values(reader, packet);
                reader.ReadFlush();
                reader.Flush();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                if (variables.debugme) { Console.WriteLine(ex.ToString()); }
            }

            return 0;
        }
        private void get_values(UsbEndpointReader reader, UsbSetupPacket packet)
        {
            ErrorCode ec = ErrorCode.None;
            DateTime currtime = DateTime.Now; // BH - Grabs current system time stores it as datetime varible
            byte[] readBuffer = new byte[4];
            int length = 0x10;
            byte[] buffer = { 0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00 };
            int counter = 0;
            int countCycleValues = 0;
            int countResetValues = 0;
            int bytesRead = 0;
            //bool fin = false;
            enough = false;
            bool output = false;
            bool once = true;
            bool PowUp = false;
            byte CurrBootVal = 0x2E;
            byte CurrCycleVal = 0x00;
            byte LastValue = 0x00;
            string psot = "";

            List<int> dl = new List<int>();
            List<int> reset = new List<int>();
            List<int> cycles = new List<int>();
            List<int> crashes = new List<int>(); // BH added for crashes list

            dl.Add(0xFF); dl.Add(0x00); dl.Add(0x02);

            if (SlimBut.Checked == true)
            {
                crashes.Add(0xF0); crashes.Add(0xF1); crashes.Add(0xF2); crashes.Add(0xF3); crashes.Add(0x20); crashes.Add(0x21); crashes.Add(0xDA); crashes.Add(0x2E); crashes.Add(0x22); crashes.Add(0xA0); // possible crash
                cycles.Add(0xD2); cycles.Add(0xD3); cycles.Add(0xD4); cycles.Add(0xD5); cycles.Add(0xD6); cycles.Add(0xD7); cycles.Add(0xD8); // BH - added additional values for new counter to register
                reset.Add(0x2F); reset.Add(0x31); reset.Add(0x33); reset.Add(0x34); reset.Add(0x39); reset.Add(0x3A); reset.Add(0x3B); reset.Add(0x79);  //reset.Add(0x2E); 
            }
            else if (PhatFBut.Checked == true)
            {
                reset.Add(0x60); reset.Add(0x61); reset.Add(0x62); reset.Add(0x63); reset.Add(0x64); reset.Add(0x65); reset.Add(0x67); reset.Add(0x69); reset.Add(0x6A); reset.Add(0x79); //reset.Add(0x2E); 
                cycles.Add(0x16); cycles.Add(0x17); cycles.Add(0x18); cycles.Add(0x19); cycles.Add(0x1A); cycles.Add(0x1C); //cycles.Add(0xD8); // BH - added additional values for new counter to register
                crashes.Add(0x20); crashes.Add(0x21); crashes.Add(0x22); crashes.Add(0xA0); crashes.Add(0xE0); crashes.Add(0x2E); crashes.Add(0x2F); crashes.Add(0x58);// possible crash
            }
            else if (PhatBut.Checked == true)
            {
                reset.Add(0x2F); reset.Add(0x31); reset.Add(0x33); reset.Add(0x34); reset.Add(0x39); reset.Add(0x3A); reset.Add(0x3B); reset.Add(0x75);  //reset.Add(0x2E); 
                cycles.Add(0x01); cycles.Add(0x02); cycles.Add(0x03); cycles.Add(0x04); cycles.Add(0x05); cycles.Add(0x06); //cycles.Add(0xD8); // BH - added additional values for new counter to register
                crashes.Add(0x09); crashes.Add(0x08); crashes.Add(0x1C); // possible crash
            }
            else if (CorBut.Checked == true)
            {
                cycles.Add(0x07); cycles.Add(0x08); cycles.Add(0x09); cycles.Add(0x0A); cycles.Add(0x05); cycles.Add(0x06); //cycles.Add(0xD8); // BH - added additional values for new counter to register
                crashes.Add(0x0B); crashes.Add(0x0C); crashes.Add(0x0D); // possible crash
                reset.Add(0x29); reset.Add(0x2A); reset.Add(0x2B); reset.Add(0x2C); reset.Add(0x2E); reset.Add(0x27); reset.Add(0x28); reset.Add(0x79);
            }
            else if (cr4but.Checked == true)
            {
                cycles.Add(0x09); cycles.Add(0x0A); cycles.Add(0x0B); cycles.Add(0x0C); cycles.Add(0x08); cycles.Add(0x07); //cycles.Add(0xD8); // BH - added additional values for new counter to register
                crashes.Add(0x0F); crashes.Add(0x10); crashes.Add(0x11); // possible crash
                reset.Add(0x29); reset.Add(0x2A); reset.Add(0x2B); reset.Add(0x2C); reset.Add(0x2E); reset.Add(0x27); reset.Add(0x28); reset.Add(0x79);
            }

            if (counterg % 5 == 0 && counterg != 0) DisplayData(Environment.NewLine, 0);
            DisplayData("0", 0);
            CurrBootVal = 0x00;
            nudge = false;
            countCycleValues = 0;
            countResetValues = 0;

            while (!variables.escapeloop)
            {
                MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out length);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                //if (variables.debugme) Console.WriteLine("Bytes Read {0} - Read Buffer {1}", bytesRead, Nand.ByteArrayToString(readBuffer));
                if (!dl.Contains((readBuffer)[0])) output = true;

                if (output && !dl.Contains((readBuffer)[0])) // changed
                {
                    try
                    {
                        psot = Oper.ByteArrayToString(readBuffer).Substring(0, 2);


                        if (POSTd.ContainsKey(readBuffer[0]))
                        {
                            if (variables.debugme) Console.WriteLine("Post {0} - {1}  > {2} ", psot, POSTd[readBuffer[0]], DateTime.Now.ToString("mm:ss:fff"));
                            else Console.WriteLine("Post {0} - {1} ", psot, POSTd[readBuffer[0]]);
                        }
                        else
                        {
                            if (variables.debugme) Console.WriteLine("Post {0}  > {1} ", psot, DateTime.Now.ToString("mm:ss:fff"));
                            else Console.WriteLine("Post {0} ", psot);
                        }



                        currtime = DateTime.Now; // BH - Updates variable with current system time after a POST(knowN) value is displayed
                        if ((crashes.Contains(LastValue)) && ((readBuffer[0]) < LastValue))

                        {
                            if (SlimBut.Checked == true)
                            {

                                if ((LastValue == 0x20) && PowUp)
                                {
                                    crashVAL[(int)crash.x20]++;
                                }
                                if ((LastValue == 0x21) && PowUp)
                                {
                                    crashVAL[(int)crash.x21]++;
                                }
                                if ((LastValue == 0xA0) && PowUp)
                                {
                                    crashVAL[(int)crash.xA0]++;
                                }
                                if ((LastValue == 0x22) && PowUp)
                                {
                                    crashVAL[(int)crash.x22]++;
                                }
                                if ((LastValue == 0x2E) && PowUp)
                                {
                                    crashVAL[(int)crash.x2E]++;
                                }
                                if ((LastValue == 0xDA) && PowUp) crashVAL[(int)crash.xDA]++;
                                if ((LastValue == 0xF0) && PowUp)
                                {
                                    crashVAL[(int)crash.xF0]++;
                                }
                                if ((LastValue == 0xF1) && PowUp)
                                {
                                    crashVAL[(int)crash.xF1]++;
                                }
                                if ((LastValue == 0xF2) && PowUp)
                                {
                                    crashVAL[(int)crash.xF2]++;
                                }
                                if ((LastValue == 0xF3) && PowUp)
                                {
                                    crashVAL[(int)crash.xF3]++;
                                }
                            }
                            else if (PhatFBut.Checked == true)
                            {
                                if ((LastValue == 0x20) && PowUp)
                                {
                                    crashVAL[(int)crash.x20]++;
                                }
                                if ((LastValue == 0x21) && PowUp)
                                {
                                    crashVAL[(int)crash.x21]++;
                                }
                                if ((LastValue == 0xA0) && (counter == 0)) { counter++; crashVAL[(int)crash.xA0]++; }
                                if ((LastValue == 0xA0) && PowUp)
                                {
                                    crashVAL[(int)crash.xA0]++;
                                }
                                if ((LastValue == 0x22) && PowUp)
                                {
                                    crashVAL[(int)crash.x22]++;
                                }
                                if ((LastValue == 0x2E) && PowUp)
                                {
                                    crashVAL[(int)crash.x2E]++;
                                }
                            }
                            else if (PhatBut.Checked == true)
                            {
                                if ((LastValue == 0x08))
                                {
                                    crashVAL[(int)crash.x08]++;
                                }
                                if ((LastValue == 0x09))
                                {
                                    crashVAL[(int)crash.x09]++;
                                }
                                if ((LastValue == 0x1C))
                                {
                                    crashVAL[(int)crash.x1C]++;
                                }
                            }
                            else if (CorBut.Checked == true)
                            {
                                if ((LastValue == 0x0B))
                                {
                                    crashVAL[(int)crash.x0B]++;
                                }
                                if ((LastValue == 0x0C))
                                {
                                    crashVAL[(int)crash.x0C]++;
                                }
                                if ((LastValue == 0x0D))
                                {
                                    crashVAL[(int)crash.x0D]++;
                                }
                            }
                            else if (cr4but.Checked == true)
                            {
                                if ((LastValue == 0x0D))
                                {
                                    crashVAL[(int)crash.x0D]++;
                                }
                                if ((LastValue == 0x0E))
                                {
                                    crashVAL[(int)crash.x0E]++;
                                }
                                if ((LastValue == 0x0F))
                                {
                                    crashVAL[(int)crash.x0F]++;
                                }
                            }
                        }
                        LastValue = (readBuffer[0]);
                        if (reset.Contains((readBuffer)[0]))
                        {
                            if ((readBuffer[0]) > CurrBootVal)
                            {
                                if ((readBuffer[0]) != 0x3B) CurrBootVal = (readBuffer[0]);
                                if (variables.debugme) Console.WriteLine(CurrBootVal.ToString());
                                countResetValues++; // BH Added counter on booted values to prevent random values counting as a boot
                                if (countResetValues >= 3) // BH ensure 3 out of 6 values have been shown
                                {
                                    Reset = true;

                                    if (SlimBut.Checked == true) { crashVAL[(int)crash.x2E]--; }
                                    else if (PhatFBut.Checked == true) { crashVAL[(int)crash.x2E]--; }
                                    else if (PhatBut.Checked == true) { crashVAL[(int)crash.x1C]--; }
                                    else if (CorBut.Checked == true) { crashVAL[(int)crash.x0D]--; }
                                    else if (cr4but.Checked == true) { crashVAL[(int)crash.x0F]--; }
                                    if (variables.debugme) Console.WriteLine("reset");

                                    if ((counter == 0))
                                    {
                                        if (variables.debugme) Console.WriteLine("Counter: {0}", counter.ToString());
                                        counter++;
                                        globalcounter++;
                                        DisplayData(counter.ToString(), (counter - 1).ToString().Length);
                                        countCycleValues = 0; // BH - resets counter after a registered glitch cycle
                                        CurrBootVal = 0x2E;
                                    }
                                    if (once)
                                    {
                                        DisplayData(", ", 0);

                                    }
                                    once = false;

                                    if (nudge) variables.escapeloop = true;
                                    Thread thread = new Thread(new ThreadStart(NewThreadENough));
                                    thread.Start();
                                    countResetValues = 0; // resets counter for boot
                                    if (variables.debugme) Console.WriteLine("Count Reset Values: {0}", countResetValues.ToString());
                                }
                            }
                        }
                        else if ((cycles.Contains(readBuffer[0]) && (SlimBut.Checked == true) && (!Reset)))
                        {
                            if ((readBuffer[0]) < CurrCycleVal) { countCycleValues = 0; CurrCycleVal = (readBuffer[0]); }
                            countCycleValues++; // BH - added additional counter to check for multiple D values prior to 0xD8 to count as a glitch
                            if (variables.debugme) Console.WriteLine("Cycles {0}", countCycleValues);
                            if ((countCycleValues >= 3) && (readBuffer[0] == 0xD8)) // BH - prevents out of sequence 0XD8 registering a glitch
                            {

                                if (variables.debugme) Console.WriteLine("Cycle values started sequence");
                                if (counter >= numericCap.Value && numericCap.Value != 0) { DisplayData("+, ", 0); break; }
                                counter++;
                                PowUp = true;
                                globalcounter++;
                                DisplayData(counter.ToString(), (counter - 1).ToString().Length);
                                countResetValues = 0; // BH resets boot min required boot counter
                                countCycleValues = 0; // BH - resets counter after a registered glitch cycle
                                CurrBootVal = 0x00;
                                CurrCycleVal = (readBuffer[0]);
                            }
                        }
                        else if ((cycles.Contains(readBuffer[0]) && (PhatBut.Checked == true) && (!Reset)))
                        {
                            if ((readBuffer[0]) < CurrCycleVal) { countCycleValues = 0; CurrCycleVal = (readBuffer[0]); }
                            countCycleValues++;
                            if (variables.debugme) Console.WriteLine("Count Cycle Values: {0}", countCycleValues.ToString());
                            if ((countCycleValues >= 5))
                            {

                                if (variables.debugme) Console.WriteLine("Cycle values started sequence");
                                if (counter >= numericCap.Value && numericCap.Value != 0) { DisplayData("+, ", 0); break; }
                                counter++;
                                PowUp = true;
                                globalcounter++;
                                DisplayData(counter.ToString(), (counter - 1).ToString().Length);
                                countResetValues = 0; // BH resets boot min required boot counter
                                countCycleValues = 0; // BH - resets counter after a registered glitch cycle
                                CurrBootVal = 0x00;
                                CurrCycleVal = (readBuffer[0]);
                            }
                        }
                        else if ((cycles.Contains(readBuffer[0]) && (PhatFBut.Checked == true) && (!Reset)))
                        {
                            if ((readBuffer[0]) < CurrCycleVal) { countCycleValues = 0; CurrCycleVal = (readBuffer[0]); }

                            countCycleValues++; // BH - added additional counter to check for multiple D values prior to 0xD8 to count as a glitch
                            if (variables.debugme) Console.WriteLine("Count Cycle Values: {0}", countCycleValues.ToString());
                            if ((countCycleValues >= 5))
                            {

                                if (variables.debugme) Console.WriteLine("Cycle values started sequence");
                                if (counter >= numericCap.Value && numericCap.Value != 0) { DisplayData("+, ", 0); break; }
                                counter++;
                                PowUp = true;
                                globalcounter++;
                                DisplayData(counter.ToString(), (counter - 1).ToString().Length);
                                countResetValues = 0; // BH resets boot min required boot counter
                                countCycleValues = 0; // BH - resets counter after a registered glitch cycle
                                CurrBootVal = 0x00;
                                CurrCycleVal = (readBuffer[0]);
                            }
                        }
                        else if ((cycles.Contains(readBuffer[0]) && (CorBut.Checked == true) && (!Reset)))
                        {
                            if ((readBuffer[0]) < CurrCycleVal) { countCycleValues = 0; CurrCycleVal = (readBuffer[0]); }
                            countCycleValues++; // BH - added additional counter to check for multiple D values prior to 0xD8 to count as a glitch
                            if (variables.debugme) Console.WriteLine("Count Cycle Values: {0}", countCycleValues.ToString());
                            if ((countCycleValues >= 5))
                            {

                                if (variables.debugme) Console.WriteLine("Cycle values started sequence");
                                if (counter >= numericCap.Value && numericCap.Value != 0) { DisplayData("+, ", 0); break; }
                                counter++;
                                PowUp = true;
                                globalcounter++;
                                DisplayData(counter.ToString(), (counter - 1).ToString().Length);
                                countResetValues = 0; // BH resets boot min required boot counter
                                countCycleValues = 0; // BH - resets counter after a registered glitch cycle
                                CurrBootVal = 0x00;
                                CurrCycleVal = (readBuffer[0]);
                            }
                        }
                        else if ((cycles.Contains(readBuffer[0]) && (cr4but.Checked == true) && (!Reset)))
                        {
                            if ((readBuffer[0]) < CurrCycleVal) { countCycleValues = 0; CurrCycleVal = (readBuffer[0]); }
                            countCycleValues++; // BH - added additional counter to check for multiple D values prior to 0xD8 to count as a glitch
                            if (variables.debugme) Console.WriteLine("Count Cycle Values: {0}", countCycleValues.ToString());
                            if ((countCycleValues >= 5))
                            {

                                if (variables.debugme) Console.WriteLine("Cycle values started sequence");
                                if (counter >= numericCap.Value && numericCap.Value != 0) { DisplayData("+, ", 0); break; }
                                counter++;
                                PowUp = true;
                                globalcounter++;
                                DisplayData(counter.ToString(), (counter - 1).ToString().Length);
                                countResetValues = 0; // BH resets boot min required boot counter
                                countCycleValues = 0; // BH - resets counter after a registered glitch cycle
                                CurrBootVal = 0x00;
                                CurrCycleVal = (readBuffer[0]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (variables.debugme) Console.WriteLine(ex.ToString());
                    }
                }
                else
                {
                    if (enough)
                    {
                        SleepIntermittently((int)numericSDPause.Value * 1000);
                        variables.escapeloop = true;
                    }
                }

                if (nudge)
                {
                    if (variables.debugme) Console.WriteLine("Nudged");
                    if (once) DisplayData("n, ", 0); // BH - append value with "n" if it was a nudged value
                    enough = true;
                    once = false;
                    break;
                }
                if ((currtime.Ticks + 600000000 + (numericCool.Value * 10000000)) <= DateTime.Now.Ticks)
                {
                    Timeout(timeoutflag); // checks current system time against variable plus 60 seconds - if not updated by POST then shutdown
                    timeoutflag = !timeoutflag;
                }

            }

            if (nudge == false) // BH - added to check if nudged, if nudged value is NOT passed to avgs box
            {
                if (counter != 0) values.Add(counter);  // BH - if system boots straight away and check of D values has not registered a boot value of 1 is passed to avgs
            }

            //reader.ReadFlush();
            //reader.Flush();
            variables.escapeloop = false;
        }
        void enumerate_post()
        {
            POSTd.Clear();
            if (PhatBut.Checked == true) // fake post
            {

                POSTd.Add(0x01, "BOOT SEQUENCE STARTING");

                POSTd.Add(0x02, "FETCH_HEADER");

                POSTd.Add(0x03, "RJTAG MAGIC STARTING");
                POSTd.Add(0x04, ".");

                POSTd.Add(0x05, "..");

                POSTd.Add(0x06, "...");


                POSTd.Add(0x07, "CB entry point reached");
                POSTd.Add(0x08, "GLITCH CHECK COMMENCE");

                POSTd.Add(0x09, "HWINIT");
                POSTd.Add(0x0A, ".");
                POSTd.Add(0x0B, "GLITCH SUCCESSFUL");
                POSTd.Add(0x0C, "..");
                POSTd.Add(0x0D, "...");
                POSTd.Add(0x0E, "....");
                POSTd.Add(0x0F, ".....");
                POSTd.Add(0x10, "......");
                POSTd.Add(0x11, ".......");
                POSTd.Add(0x12, "........");
                POSTd.Add(0x13, ".........");
                POSTd.Add(0x14, "..........");
                POSTd.Add(0x15, "CONTINUING");
                POSTd.Add(0x16, ".");
                POSTd.Add(0x17, "..");
                POSTd.Add(0x18, "...");
                POSTd.Add(0x19, "....");
                POSTd.Add(0x1A, ".....");
                POSTd.Add(0x1B, "......");
                POSTd.Add(0x1C, "CHECK JTAG WIRING");
                POSTd.Add(0x1D, "CHECK PASSED");
                POSTd.Add(0x1E, ".");
                POSTd.Add(0x1F, "..");
                POSTd.Add(0x20, "...");
                POSTd.Add(0x21, "....");


                POSTd.Add(0x22, "Entrypoint reached");
                POSTd.Add(0x23, ".");
                POSTd.Add(0x24, "..");
                POSTd.Add(0x25, "...");
                POSTd.Add(0x26, "....");
                POSTd.Add(0x27, ".....");
                POSTd.Add(0x28, "......");
                POSTd.Add(0x29, ".......");
                POSTd.Add(0x2A, "........");
                POSTd.Add(0x2B, ".........");
                POSTd.Add(0x2C, "..........");
                POSTd.Add(0x2D, "...........");
                POSTd.Add(0x2E, "............");
                POSTd.Add(0x2F, "INIT KERNEL");
                POSTd.Add(0x30, ".");
                POSTd.Add(0x31, "..");
                POSTd.Add(0x32, "...");
                POSTd.Add(0x33, "....");
                POSTd.Add(0x34, ".....");
                POSTd.Add(0x35, "Check Video Cable");


                POSTd.Add(0x36, "INIT_KEY_EX_VAULT");
                POSTd.Add(0x37, ".");
                POSTd.Add(0x38, "..");
                POSTd.Add(0x39, "...");
                POSTd.Add(0x3A, "....");
                POSTd.Add(0x3B, ".....");
                POSTd.Add(0x3C, "LOAD XAM");

                POSTd.Add(0x3D, "BOOTED");


            }
            else if (SlimBut.Checked == true)
            {
                // 1BL 0 0x1*
                POSTd.Add(0x10, "Payload/1BL started");
                POSTd.Add(0x11, "FSB_CONFIG_PHY_CONTROL");
                POSTd.Add(0x12, "FSB_CONFIG_RX_STATE");
                POSTd.Add(0x13, "FSB_CONFIG_TX_STATE");
                POSTd.Add(0x14, "FSB_CONFIG_TX_CREDITS");
                POSTd.Add(0x15, "FETCH_OFFSET");
                POSTd.Add(0x16, "FETCH_HEADER");
                POSTd.Add(0x17, "VERIFY_HEADER");
                POSTd.Add(0x18, "FETCH_CONTENTS");
                POSTd.Add(0x19, "HMACSHA_COMPUTE");
                POSTd.Add(0x1A, "RC4_INITIALIZE");
                POSTd.Add(0x1B, "RC4_DECRYPT");
                POSTd.Add(0x1C, "SHA_COMPUTE");
                POSTd.Add(0x1D, "SIG_VERIFY");
                POSTd.Add(0x1E, "BRANCH");
                //1BL Hardware Exception - 0x81-0x91
                POSTd.Add(0x81, "Panic - MACHINE_CHECK");
                POSTd.Add(0x82, "Panic - DATA_STORAGE");
                POSTd.Add(0x83, "Panic - DATA_SEGMENT");
                POSTd.Add(0x84, "Panic - INSTRUCTION_STORAGE");
                POSTd.Add(0x85, "Panic - INSTRUCTION_SEGMENT");
                POSTd.Add(0x86, "Panic - EXTERNAL");
                POSTd.Add(0x87, "Panic - ALIGNMENT");
                POSTd.Add(0x88, "Panic - PROGRAM");
                POSTd.Add(0x89, "Panic - FPU_UNAVAILABLE");
                POSTd.Add(0x8A, "Panic - DECREMENTER");
                POSTd.Add(0x8B, "Panic - HYPERVISOR_DECREMENTER");
                POSTd.Add(0x8C, "Panic - SYSTEM_CALL");
                POSTd.Add(0x8D, "Panic - TRACE");
                POSTd.Add(0x8E, "Panic - VPU_UNAVAILABLE");
                POSTd.Add(0x8F, "Panic - MAINTENANCE");
                POSTd.Add(0x90, "Panic - VMX_ASSIST");
                POSTd.Add(0x91, "Panic - THERMAL_MANAGEMENT");
                //1BL Errors - 0x92 - 0x98
                POSTd.Add(0x92, "Panic - 1BL is executed on wrong CPU thread (panic)");
                POSTd.Add(0x93, "Panic - TOO_MANY_CORES");
                POSTd.Add(0x94, "Panic - VERIFY_OFFSET");
                POSTd.Add(0x95, "Panic - VERIFY_HEADER");
                POSTd.Add(0x96, "Panic - SIG_VERIFY");
                POSTd.Add(0x97, "Panic - NONHOST_RESUME_STATUS");
                POSTd.Add(0x98, "Panic - NEXT_STAGE_SIZE");
                //CB_A 0xD0 -0xDB
                POSTd.Add(0xD0, "CB_A entry point reached");
                POSTd.Add(0xD1, "READ_FUSES");
                POSTd.Add(0xD2, "VERIFY_OFFSET_CB_B");
                POSTd.Add(0xD3, "FETCH_HEADER_CB_B");
                POSTd.Add(0xD4, "VERIFY_HEADER_CB_B");
                POSTd.Add(0xD5, "FETCH_CONTENTS_CB_B");
                POSTd.Add(0xD6, "HMACSHA_COMPUTE_CB_B");
                POSTd.Add(0xD7, "RC4_INITIALIZE_CB_B");
                POSTd.Add(0xD8, "RC4_DECRYPT_CB_B");
                POSTd.Add(0xD9, "SHA_COMPUTE_CB_B");
                POSTd.Add(0xDA, "SHA_VERIFY_CB_B");
                POSTd.Add(0xDB, "BRANCH_CB_B");
                //CB_A Errors 0xF0- 0xF3
                POSTd.Add(0xF0, "Panic - VERIFY_OFFSET_CB_B");
                POSTd.Add(0xF1, "Panic - VERIFY_HEADER_CB_B");
                POSTd.Add(0xF2, "Panic - SHA_VERIFY_CB_B");
                POSTd.Add(0xF3, "Panic - ENTRY_SIZE_INVALID_CB_B");
                //CB 0x20-3B
                POSTd.Add(0x20, "CB entry point reached");
                POSTd.Add(0x21, "INIT_SECOTP");
                POSTd.Add(0x22, "INIT_SECENG");
                POSTd.Add(0x23, "INIT_SYSRAM");
                POSTd.Add(0x24, "VERIFY_OFFSET_3BL_CC");
                POSTd.Add(0x25, "LOCATE_3BL_CC");
                POSTd.Add(0x26, "FETCH_HEADER_3BL_CC");
                POSTd.Add(0x27, "VERIFY_HEADER_3BL_CC");
                POSTd.Add(0x28, "FETCH_CONTENTS_3BL_CC");
                POSTd.Add(0x29, "HMACSHA_COMPUTE_3BL_CC");
                POSTd.Add(0x2A, "RC4_INITIALIZE_3BL_CC");
                POSTd.Add(0x2B, "RC4_DECRYPT_3BL_CC");
                POSTd.Add(0x2C, "SHA_COMPUTE_3BL_CC");
                POSTd.Add(0x2D, "SIG_VERIFY_3BL_CC");
                POSTd.Add(0x2E, "HWINIT");
                POSTd.Add(0x2F, "RELOCATE");
                POSTd.Add(0x30, "VERIFY_OFFSET_4BL_CD");
                POSTd.Add(0x31, "FETCH_HEADER_4BL_CD");
                POSTd.Add(0x32, "VERIFY_HEADER_4BL_CD");
                POSTd.Add(0x33, "FETCH_CONTENTS_4BL_CD");
                POSTd.Add(0x34, "HMACSHA_COMPUTE_4BL_CD");
                POSTd.Add(0x35, "RC4_INITIALIZE_4BL_CD");
                POSTd.Add(0x36, "RC4_DECRYPT_4BL_CD");
                POSTd.Add(0x37, "SHA_COMPUTE_4BL_CD");
                POSTd.Add(0x38, "SIG_VERIFY_4BL_CD");
                POSTd.Add(0x39, "SHA_VERIFY_4BL_CD");
                POSTd.Add(0x3A, "BRANCH");
                POSTd.Add(0x3B, "PCI_INIT");
                //2BL errors  0x9B-0xB0
                POSTd.Add(0x9B, "Panic - VERIFY_SECOTP_1");
                POSTd.Add(0x9C, "Panic - VERIFY_SECOTP_2");
                POSTd.Add(0x9D, "Panic - VERIFY_SECOTP_3");
                POSTd.Add(0x9E, "Panic - Panic - VERIFY_SECOTP_4");
                POSTd.Add(0x9F, "Panic - VERIFY_SECOTP_5");
                POSTd.Add(0xA0, "Panic - VERIFY_SECOTP_6");
                POSTd.Add(0xA1, "Panic - VERIFY_SECOTP_7");
                POSTd.Add(0xA2, "Panic - VERIFY_SECOTP_8");
                POSTd.Add(0xA3, "Panic - VERIFY_SECOTP_9");
                POSTd.Add(0xA4, "Panic - VERIFY_SECOTP_10");
                POSTd.Add(0xA5, "Panic - VERIFY_OFFSET_3BL_CC");
                POSTd.Add(0xA6, "Panic - LOCATE_3BL_CC");
                POSTd.Add(0xA7, "Panic - VERIFY_HEADER_3BL_CC");
                POSTd.Add(0xA8, "Panic - SIG_VERIFY_3BL_CC");
                POSTd.Add(0xA9, "Panic - HWINIT");
                POSTd.Add(0xAA, "Panic - VERIFY_OFFSET_4BL_CD");
                POSTd.Add(0xAB, "Panic - VERIFY_HEADER_4BL_CD");
                POSTd.Add(0xAC, "Panic - SIG_VERIFY_4BL_CD");
                POSTd.Add(0xAD, "Panic - SHA_VERIFY_4BL_CD");
                POSTd.Add(0xAE, "Panic - UNEXPECTED_INTERRUPT");
                POSTd.Add(0xAF, "Panic - UNSUPPORTED_RAM_SIZE");
                POSTd.Add(0xB0, "Panic - VERIFY_CONSOLE_TYPE");
                //4BL 0x40-0x53
                POSTd.Add(0x40, "Entrypoint of CD reached");
                POSTd.Add(0x41, "VERIFY_OFFSET");
                POSTd.Add(0x42, "FETCH_HEADER");
                POSTd.Add(0x43, "VERIFY_HEADER");
                POSTd.Add(0x44, "FETCH_CONTENTS");
                POSTd.Add(0x45, "HMACSHA_COMPUTE");
                POSTd.Add(0x46, "RC4_INITIALIZE");
                POSTd.Add(0x47, "RC4_DECRYPT");
                POSTd.Add(0x48, "SHA_COMPUTE");
                POSTd.Add(0x49, "SHA_VERIFY");
                POSTd.Add(0x4A, "LOAD_6BL_CF");
                POSTd.Add(0x4B, "LZX_EXPAND");
                POSTd.Add(0x4C, "SWEEP_CACHES");
                POSTd.Add(0x4D, "DECODE_FUSES");
                POSTd.Add(0x4E, "FETCH_OFFSET_6BL_CF");
                POSTd.Add(0x4F, "VERIFY_OFFSET_6BL_CF");
                POSTd.Add(0x50, "LOAD_UPDATE_1");
                POSTd.Add(0x51, "LOAD_UPDATE_2");
                POSTd.Add(0x52, "BRANCH");
                POSTd.Add(0x53, "DECRYT_VERIFY_HV_CERT");
                //4BL Errors 0xB1 - 0xB7
                POSTd.Add(0xB1, "Panic - VERIFY_OFFSET");
                POSTd.Add(0xB2, "Panic - VERIFY_HEADER");
                POSTd.Add(0xB3, "Panic - SHA_VERIFY");
                POSTd.Add(0xB4, "Panic - LZX_EXPAND");
                POSTd.Add(0xB5, "Panic - VERIFY_OFFSET_6BL");
                POSTd.Add(0xB6, "Panic - DECODE_FUSES");
                POSTd.Add(0xB7, "Panic - UPDATE_MISSING");
                //6BL 0xC1 - 0xC8
                POSTd.Add(0xC1, "LZX_EXPAND_1");
                POSTd.Add(0xC2, "LZX_EXPAND_2");
                POSTd.Add(0xC3, "LZX_EXPAND_3");
                POSTd.Add(0xC4, "LZX_EXPAND_4");
                POSTd.Add(0xC5, "LZX_EXPAND_5");
                POSTd.Add(0xC6, "LZX_EXPAND_6");
                POSTd.Add(0xC7, "LZX_EXPAND_7");
                POSTd.Add(0xC8, "SHA_VERIFY");
                //HV 0x58-0x5F - 0xFF
                POSTd.Add(0x58, "INIT_HYPERVISOR");
                POSTd.Add(0x59, "INIT_SOC_MMIO");
                POSTd.Add(0x5A, "INIT_XEX_TRAINING");
                POSTd.Add(0x5B, "INIT_KEYRING");
                POSTd.Add(0x5C, "INIT_KEYS");
                POSTd.Add(0x5D, "INIT_SOC_INT");
                POSTd.Add(0x5E, "INIT_SOC_INT_COMPLETE");
                POSTd.Add(0xFF, "FATAL");
                // Kernel 0x60-79
                POSTd.Add(0x60, "INIT_KERNEL");
                POSTd.Add(0x61, "INIT_HAL_PHASE_0");
                POSTd.Add(0x62, "INIT_PROCESS_OBJECTS");
                POSTd.Add(0x63, "INIT_KERNEL_DEBUGGER");
                POSTd.Add(0x64, "INIT_MEMORY_MANAGER");
                POSTd.Add(0x65, "INIT_STACKS");
                POSTd.Add(0x66, "INIT_OBJECT_SYSTEM");
                POSTd.Add(0x67, "INIT_PHASE1_THREAD");
                POSTd.Add(0x68, "Started phase 1 Initialization + INIT_PROCESSORS");
                POSTd.Add(0x69, "INIT_KEY_VAULT");
                POSTd.Add(0x6A, "INIT_HAL_PHASE_1");
                POSTd.Add(0x6B, "INIT_SFC_DRIVER");
                POSTd.Add(0x6C, "INIT_SECURITY");
                POSTd.Add(0x6D, "INIT_KEY_EX_VAULT");
                POSTd.Add(0x6E, "INIT_SETTINGS");
                POSTd.Add(0x6F, "INIT_POWER_MODE");
                POSTd.Add(0x70, "INIT_VIDEO_DRIVER");
                POSTd.Add(0x71, "INIT_AUDIO_DRIVER");
                POSTd.Add(0x72, "INIT_BOOT_ANIMATION + XMADecoder & XAudioRender Init");
                POSTd.Add(0x73, "INIT_SATA_DRIVER");
                POSTd.Add(0x74, "INIT_SHADOWBOOT");
                POSTd.Add(0x75, "INIT_DUMP_SYSTEM");
                POSTd.Add(0x76, "INIT_SYSTEM_ROOT");
                POSTd.Add(0x77, "INIT_OTHER_DRIVERS");
                POSTd.Add(0x78, "INIT_STFS_DRIVER");
                POSTd.Add(0x79, "LOAD_XAM");

                POSTd.Add(0xB8, "Panic - CF auth failed");
            }
            else if (PhatFBut.Checked == true)
            {
                // 1BL 0 0x1*
                POSTd.Add(0x10, "Payload/1BL started");
                POSTd.Add(0x11, "FSB_CONFIG_PHY_CONTROL");
                POSTd.Add(0x12, "FSB_CONFIG_RX_STATE");
                POSTd.Add(0x13, "FSB_CONFIG_TX_STATE");
                POSTd.Add(0x14, "FSB_CONFIG_TX_CREDITS");
                POSTd.Add(0x15, "FETCH_OFFSET");
                POSTd.Add(0x16, "FETCH_HEADER");
                POSTd.Add(0x17, "VERIFY_HEADER");
                POSTd.Add(0x18, "FETCH_CONTENTS");
                POSTd.Add(0x19, "HMACSHA_COMPUTE");
                POSTd.Add(0x1A, "RC4_INITIALIZE");
                POSTd.Add(0x1B, "RC4_DECRYPT");
                POSTd.Add(0x1C, "SHA_COMPUTE");
                POSTd.Add(0x1D, "SIG_VERIFY");
                POSTd.Add(0x1E, "BRANCH");
                //1BL Hardware Exception - 0x81-0x91
                POSTd.Add(0x81, "Panic - MACHINE_CHECK");
                POSTd.Add(0x82, "Panic - DATA_STORAGE");
                POSTd.Add(0x83, "Panic - DATA_SEGMENT");
                POSTd.Add(0x84, "Panic - INSTRUCTION_STORAGE");
                POSTd.Add(0x85, "Panic - INSTRUCTION_SEGMENT");
                POSTd.Add(0x86, "Panic - EXTERNAL");
                POSTd.Add(0x87, "Panic - ALIGNMENT");
                POSTd.Add(0x88, "Panic - PROGRAM");
                POSTd.Add(0x89, "Panic - FPU_UNAVAILABLE");
                POSTd.Add(0x8A, "Panic - DECREMENTER");
                POSTd.Add(0x8B, "Panic - HYPERVISOR_DECREMENTER");
                POSTd.Add(0x8C, "Panic - SYSTEM_CALL");
                POSTd.Add(0x8D, "Panic - TRACE");
                POSTd.Add(0x8E, "Panic - VPU_UNAVAILABLE");
                POSTd.Add(0x8F, "Panic - MAINTENANCE");
                POSTd.Add(0x90, "Panic - VMX_ASSIST");
                POSTd.Add(0x91, "Panic - THERMAL_MANAGEMENT");
                //1BL Errors - 0x92 - 0x98
                POSTd.Add(0x92, "Panic - 1BL is executed on wrong CPU thread (panic)");
                POSTd.Add(0x93, "Panic - TOO_MANY_CORES");
                POSTd.Add(0x94, "Panic - VERIFY_OFFSET");
                POSTd.Add(0x95, "Panic - VERIFY_HEADER");
                POSTd.Add(0x96, "Panic - SIG_VERIFY");
                POSTd.Add(0x97, "Panic - NONHOST_RESUME_STATUS");
                POSTd.Add(0x98, "Panic - NEXT_STAGE_SIZE");
                //CB_A 0xD0 -0xDB
                POSTd.Add(0xD0, "CB_A entry point reached");
                POSTd.Add(0xD1, "READ_FUSES");
                POSTd.Add(0xD2, "VERIFY_OFFSET_CB_B");
                POSTd.Add(0xD3, "FETCH_HEADER_CB_B");
                POSTd.Add(0xD4, "VERIFY_HEADER_CB_B");
                POSTd.Add(0xD5, "FETCH_CONTENTS_CB_B");
                POSTd.Add(0xD6, "HMACSHA_COMPUTE_CB_B");
                POSTd.Add(0xD7, "RC4_INITIALIZE_CB_B");
                POSTd.Add(0xD8, "RC4_DECRYPT_CB_B");
                POSTd.Add(0xD9, "SHA_COMPUTE_CB_B");
                POSTd.Add(0xDA, "SHA_VERIFY_CB_B");
                POSTd.Add(0xDB, "BRANCH_CB_B");
                //CB_A Errors 0xF0- 0xF3
                POSTd.Add(0xF0, "Panic - VERIFY_OFFSET_CB_B");
                POSTd.Add(0xF1, "Panic - VERIFY_HEADER_CB_B");
                POSTd.Add(0xF2, "Panic - SHA_VERIFY_CB_B");
                POSTd.Add(0xF3, "Panic - ENTRY_SIZE_INVALID_CB_B");
                //CB 0x20-3B
                POSTd.Add(0x20, "CB entry point reached");
                POSTd.Add(0x21, "INIT_SECOTP");
                POSTd.Add(0x22, "INIT_SECENG");
                POSTd.Add(0x23, "INIT_SYSRAM");
                POSTd.Add(0x24, "VERIFY_OFFSET_3BL_CC");
                POSTd.Add(0x25, "LOCATE_3BL_CC");
                POSTd.Add(0x26, "FETCH_HEADER_3BL_CC");
                POSTd.Add(0x27, "VERIFY_HEADER_3BL_CC");
                POSTd.Add(0x28, "FETCH_CONTENTS_3BL_CC");
                POSTd.Add(0x29, "HMACSHA_COMPUTE_3BL_CC");
                POSTd.Add(0x2A, "RC4_INITIALIZE_3BL_CC");
                POSTd.Add(0x2B, "RC4_DECRYPT_3BL_CC");
                POSTd.Add(0x2C, "SHA_COMPUTE_3BL_CC");
                POSTd.Add(0x2D, "SIG_VERIFY_3BL_CC");
                POSTd.Add(0x2E, "HWINIT");
                POSTd.Add(0x2F, "RELOCATE");
                POSTd.Add(0x30, "VERIFY_OFFSET_4BL_CD");
                POSTd.Add(0x31, "FETCH_HEADER_4BL_CD");
                POSTd.Add(0x32, "VERIFY_HEADER_4BL_CD");
                POSTd.Add(0x33, "FETCH_CONTENTS_4BL_CD");
                POSTd.Add(0x34, "HMACSHA_COMPUTE_4BL_CD");
                POSTd.Add(0x35, "RC4_INITIALIZE_4BL_CD");
                POSTd.Add(0x36, "RC4_DECRYPT_4BL_CD");
                POSTd.Add(0x37, "SHA_COMPUTE_4BL_CD");
                POSTd.Add(0x38, "SIG_VERIFY_4BL_CD");
                POSTd.Add(0x39, "SHA_VERIFY_4BL_CD");
                POSTd.Add(0x3A, "BRANCH");
                POSTd.Add(0x3B, "PCI_INIT");
                //2BL errors  0x9B-0xB0
                POSTd.Add(0x9B, "Panic - VERIFY_SECOTP_1");
                POSTd.Add(0x9C, "Panic - VERIFY_SECOTP_2");
                POSTd.Add(0x9D, "Panic - VERIFY_SECOTP_3");
                POSTd.Add(0x9E, "Panic - Panic - VERIFY_SECOTP_4");
                POSTd.Add(0x9F, "Panic - VERIFY_SECOTP_5");
                POSTd.Add(0xA0, "Panic - VERIFY_SECOTP_6");
                POSTd.Add(0xA1, "Panic - VERIFY_SECOTP_7");
                POSTd.Add(0xA2, "Panic - VERIFY_SECOTP_8");
                POSTd.Add(0xA3, "Panic - VERIFY_SECOTP_9");
                POSTd.Add(0xA4, "Panic - VERIFY_SECOTP_10");
                POSTd.Add(0xA5, "Panic - VERIFY_OFFSET_3BL_CC");
                POSTd.Add(0xA6, "Panic - LOCATE_3BL_CC");
                POSTd.Add(0xA7, "Panic - VERIFY_HEADER_3BL_CC");
                POSTd.Add(0xA8, "Panic - SIG_VERIFY_3BL_CC");
                POSTd.Add(0xA9, "Panic - HWINIT");
                POSTd.Add(0xAA, "Panic - VERIFY_OFFSET_4BL_CD");
                POSTd.Add(0xAB, "Panic - VERIFY_HEADER_4BL_CD");
                POSTd.Add(0xAC, "Panic - SIG_VERIFY_4BL_CD");
                POSTd.Add(0xAD, "Panic - SHA_VERIFY_4BL_CD");
                POSTd.Add(0xAE, "Panic - UNEXPECTED_INTERRUPT");
                POSTd.Add(0xAF, "Panic - UNSUPPORTED_RAM_SIZE");
                POSTd.Add(0xB0, "Panic - VERIFY_CONSOLE_TYPE");
                //4BL 0x40-0x53
                POSTd.Add(0x40, "Entrypoint of CD reached");
                POSTd.Add(0x41, "VERIFY_OFFSET");
                POSTd.Add(0x42, "FETCH_HEADER");
                POSTd.Add(0x43, "VERIFY_HEADER");
                POSTd.Add(0x44, "FETCH_CONTENTS");
                POSTd.Add(0x45, "HMACSHA_COMPUTE");
                POSTd.Add(0x46, "RC4_INITIALIZE");
                POSTd.Add(0x47, "RC4_DECRYPT");
                POSTd.Add(0x48, "SHA_COMPUTE");
                POSTd.Add(0x49, "SHA_VERIFY");
                POSTd.Add(0x4A, "LOAD_6BL_CF");
                POSTd.Add(0x4B, "LZX_EXPAND");
                POSTd.Add(0x4C, "SWEEP_CACHES");
                POSTd.Add(0x4D, "DECODE_FUSES");
                POSTd.Add(0x4E, "FETCH_OFFSET_6BL_CF");
                POSTd.Add(0x4F, "VERIFY_OFFSET_6BL_CF");
                POSTd.Add(0x50, "LOAD_UPDATE_1");
                POSTd.Add(0x51, "LOAD_UPDATE_2");
                POSTd.Add(0x52, "BRANCH");
                POSTd.Add(0x53, "DECRYT_VERIFY_HV_CERT");
                //4BL Errors 0xB1 - 0xB7
                POSTd.Add(0xB1, "Panic - VERIFY_OFFSET");
                POSTd.Add(0xB2, "Panic - VERIFY_HEADER");
                POSTd.Add(0xB3, "Panic - SHA_VERIFY");
                POSTd.Add(0xB4, "Panic - LZX_EXPAND");
                POSTd.Add(0xB5, "Panic - VERIFY_OFFSET_6BL");
                POSTd.Add(0xB6, "Panic - DECODE_FUSES");
                POSTd.Add(0xB7, "Panic - UPDATE_MISSING");
                //6BL 0xC1 - 0xC8
                POSTd.Add(0xC1, "LZX_EXPAND_1");
                POSTd.Add(0xC2, "LZX_EXPAND_2");
                POSTd.Add(0xC3, "LZX_EXPAND_3");
                POSTd.Add(0xC4, "LZX_EXPAND_4");
                POSTd.Add(0xC5, "LZX_EXPAND_5");
                POSTd.Add(0xC6, "LZX_EXPAND_6");
                POSTd.Add(0xC7, "LZX_EXPAND_7");
                POSTd.Add(0xC8, "SHA_VERIFY");
                //HV 0x58-0x5F - 0xFF
                POSTd.Add(0x58, "INIT_HYPERVISOR");
                POSTd.Add(0x59, "INIT_SOC_MMIO");
                POSTd.Add(0x5A, "INIT_XEX_TRAINING");
                POSTd.Add(0x5B, "INIT_KEYRING");
                POSTd.Add(0x5C, "INIT_KEYS");
                POSTd.Add(0x5D, "INIT_SOC_INT");
                POSTd.Add(0x5E, "INIT_SOC_INT_COMPLETE");
                POSTd.Add(0xFF, "FATAL");
                // Kernel 0x60-79
                POSTd.Add(0x60, "INIT_KERNEL");
                POSTd.Add(0x61, "INIT_HAL_PHASE_0");
                POSTd.Add(0x62, "INIT_PROCESS_OBJECTS");
                POSTd.Add(0x63, "INIT_KERNEL_DEBUGGER");
                POSTd.Add(0x64, "INIT_MEMORY_MANAGER");
                POSTd.Add(0x65, "INIT_STACKS");
                POSTd.Add(0x66, "INIT_OBJECT_SYSTEM");
                POSTd.Add(0x67, "INIT_PHASE1_THREAD");
                POSTd.Add(0x68, "Started phase 1 Initialization + INIT_PROCESSORS");
                POSTd.Add(0x69, "INIT_KEY_VAULT");
                POSTd.Add(0x6A, "INIT_HAL_PHASE_1");
                POSTd.Add(0x6B, "INIT_SFC_DRIVER");
                POSTd.Add(0x6C, "INIT_SECURITY");
                POSTd.Add(0x6D, "INIT_KEY_EX_VAULT");
                POSTd.Add(0x6E, "INIT_SETTINGS");
                POSTd.Add(0x6F, "INIT_POWER_MODE");
                POSTd.Add(0x70, "INIT_VIDEO_DRIVER");
                POSTd.Add(0x71, "INIT_AUDIO_DRIVER");
                POSTd.Add(0x72, "INIT_BOOT_ANIMATION + XMADecoder & XAudioRender Init");
                POSTd.Add(0x73, "INIT_SATA_DRIVER");
                POSTd.Add(0x74, "INIT_SHADOWBOOT");
                POSTd.Add(0x75, "INIT_DUMP_SYSTEM");
                POSTd.Add(0x76, "INIT_SYSTEM_ROOT");
                POSTd.Add(0x77, "INIT_OTHER_DRIVERS");
                POSTd.Add(0x78, "INIT_STFS_DRIVER");
                POSTd.Add(0x79, "LOAD_XAM");

                POSTd.Add(0xB8, "Panic - CF auth failed");
            }
            else if (CorBut.Checked == true)
            {
                // 1BL 0 0x1*
                POSTd.Add(0x01, ".");
                //POSTd.Add(0x11, "FSB_CONFIG_PHY_CONTROL");
                //POSTd.Add(0x12, "FSB_CONFIG_RX_STATE");
                //POSTd.Add(0x13, "FSB_CONFIG_TX_STATE");
                //POSTd.Add(0x14, "FSB_CONFIG_TX_CREDITS");
                //POSTd.Add(0x15, "FETCH_OFFSET");
                POSTd.Add(0x02, "BOOT SEQUENCE STARTING");
                //POSTd.Add(0x17, "VERIFY_HEADER");
                //POSTd.Add(0x18, "FETCH_CONTENTS");
                POSTd.Add(0x03, "..");
                POSTd.Add(0x04, "...");
                //POSTd.Add(0x1B, "RC4_DECRYPT");
                POSTd.Add(0x05, "...");
                //POSTd.Add(0x1D, "SIG_VERIFY");
                POSTd.Add(0x06, "....");

                //CB 0x20-3B
                POSTd.Add(0x07, ".....");
                POSTd.Add(0x08, "......");
                //POSTd.Add(0x22, "INIT_SECENG");
                POSTd.Add(0x09, ".......");
                POSTd.Add(0x0A, "........");
                POSTd.Add(0x0B, "GLITCH CHECK COMMENCE");
                POSTd.Add(0x0C, "CB_B ENTRY REACHED");
                POSTd.Add(0x0D, "GLITCH SUCCESSFUL");
                POSTd.Add(0x0E, "....");
                POSTd.Add(0x0F, ".....");
                POSTd.Add(0x10, "......");
                POSTd.Add(0x11, ".......");
                POSTd.Add(0x12, "........");
                POSTd.Add(0x13, ".........");
                POSTd.Add(0x14, "..........");
                POSTd.Add(0x15, "CONTINUING");
                POSTd.Add(0x16, ".");
                POSTd.Add(0x17, "..");
                POSTd.Add(0x18, "...");
                POSTd.Add(0x19, "....");
                POSTd.Add(0x1A, ".....");
                POSTd.Add(0x1B, "......");
                POSTd.Add(0x1C, ".......");
                POSTd.Add(0x1D, "........");
                POSTd.Add(0x1E, ".");
                POSTd.Add(0x1F, "..");
                POSTd.Add(0x20, "...");
                POSTd.Add(0x21, "....");

                //4BL 0x40-0x53
                POSTd.Add(0x22, "Entrypoint reached");
                POSTd.Add(0x23, ".");
                POSTd.Add(0x24, "..");
                POSTd.Add(0x25, "...");
                POSTd.Add(0x26, "....");
                POSTd.Add(0x27, ".....");
                POSTd.Add(0x28, "......");
                POSTd.Add(0x29, ".......");
                POSTd.Add(0x2A, "........");
                POSTd.Add(0x2B, ".........");
                POSTd.Add(0x2C, "LOAD XAM");
                POSTd.Add(0x2D, "...........");
                POSTd.Add(0x2E, "............");
                POSTd.Add(0x2F, "INIT KERNEL");

            }
            else if (cr4but.Checked == true)
            {
                // 1BL 0 0x1*
                POSTd.Add(0x01, ".");
                //POSTd.Add(0x11, "FSB_CONFIG_PHY_CONTROL");
                //POSTd.Add(0x12, "FSB_CONFIG_RX_STATE");
                //POSTd.Add(0x13, "FSB_CONFIG_TX_STATE");
                //POSTd.Add(0x14, "FSB_CONFIG_TX_CREDITS");
                //POSTd.Add(0x15, "FETCH_OFFSET");
                POSTd.Add(0x02, "..");
                //POSTd.Add(0x17, "VERIFY_HEADER");
                //POSTd.Add(0x18, "FETCH_CONTENTS");
                POSTd.Add(0x03, "...");
                POSTd.Add(0x04, "BOOT SEQUENCE STARTING");
                //POSTd.Add(0x1B, "RC4_DECRYPT");
                POSTd.Add(0x05, "...");
                //POSTd.Add(0x1D, "SIG_VERIFY");
                POSTd.Add(0x06, "....");

                //CB 0x20-3B
                POSTd.Add(0x07, ".....");
                POSTd.Add(0x08, "......");
                //POSTd.Add(0x22, "INIT_SECENG");
                POSTd.Add(0x09, ".......");
                POSTd.Add(0x0A, "........");
                POSTd.Add(0x0B, ".........");
                POSTd.Add(0x0C, "..........");
                POSTd.Add(0x0D, "GLITCH CHECK COMMENCE");
                POSTd.Add(0x0E, "CB_B ENTRY REACHED");
                POSTd.Add(0x0F, "GLITCH SUCCESSFUL");
                POSTd.Add(0x10, "......");
                POSTd.Add(0x11, ".......");
                POSTd.Add(0x12, "........");
                POSTd.Add(0x13, ".........");
                POSTd.Add(0x14, "..........");
                POSTd.Add(0x15, "CONTINUING");
                POSTd.Add(0x16, ".");
                POSTd.Add(0x17, "..");
                POSTd.Add(0x18, "...");
                POSTd.Add(0x19, "....");
                POSTd.Add(0x1A, ".....");
                POSTd.Add(0x1B, "......");
                POSTd.Add(0x1C, ".......");
                POSTd.Add(0x1D, "........");
                POSTd.Add(0x1E, ".");
                POSTd.Add(0x1F, "..");
                POSTd.Add(0x20, "...");
                POSTd.Add(0x21, "....");

                //4BL 0x40-0x53
                POSTd.Add(0x22, ".");
                POSTd.Add(0x23, "..");
                POSTd.Add(0x24, "Entrypoint reached");
                POSTd.Add(0x25, "...");
                POSTd.Add(0x26, "....");
                POSTd.Add(0x27, ".....");
                POSTd.Add(0x28, "......");
                POSTd.Add(0x29, ".......");
                POSTd.Add(0x2A, "........");
                POSTd.Add(0x2B, ".........");
                POSTd.Add(0x2C, "...........");
                POSTd.Add(0x2D, "............");
                POSTd.Add(0x2E, "LOAD XAM");
                POSTd.Add(0x2F, ".............");
                POSTd.Add(0x30, "..............");
                POSTd.Add(0x31, "INIT KERNEL");
            }
        }
        private void rate_post()
        {
            if (MyUsbDevice != null && MyUsbDevice.IsOpen)
            {
                Console.WriteLine("Device Busy");

                buttons(false);
                //stop = true;
                return;
            }
            try
            {
                enumerate_post();
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            try
            {
                int tries = 1;
                MyUsbDevice = UsbDevice.OpenUsbDevice(JRunner);
                if (MyUsbDevice == null)
                {
                    MyUsbDevice = UsbDevice.OpenUsbDevice(Arm);
                }
                if (MyUsbDevice == null)
                {
                    Console.WriteLine("Device Not Found");
                    buttons(true);
                    return;
                }
                IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                if (!ReferenceEquals(wholeUsbDevice, null))
                {
                    wholeUsbDevice.SetConfiguration(1);
                    if (variables.debugme) Console.WriteLine("Claiming Interface...");
                    wholeUsbDevice.ClaimInterface(0);
                    if (variables.debugme) Console.WriteLine("The Interface is Ours!");
                }

                buttons(false);

                UsbEndpointReader reader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep02);
                int error = 0;

                UsbSetupPacket packet = new UsbSetupPacket();
                ErrorCode ec = ErrorCode.None;

                packet.RequestType = (byte)UsbRequestType.TypeVendor;
                byte[] readBuffer = new byte[4];
                int bytesRead = 4;


                packet.Value = 0x00;
                packet.Index = 0x00;
                packet.Length = 0x8;
                packet.Request = 0x8;
                int length = 0x10;
                byte[] buffer = { 0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00 };

                ///Arm version
                MyUsbDevice.ControlTransfer(ref packet, buffer, 8, out length);
                ec = reader.Read(readBuffer, timeout, out bytesRead);
                if (variables.debugme) { Console.WriteLine("Bytes Read {0}", bytesRead); Console.WriteLine("Read Buffer {0}", Oper.ByteArrayToString(readBuffer)); }
                while (ec != ErrorCode.Success && tries < 5)
                {
                    if (variables.debugme) Console.WriteLine(ec.ToString());
                    if (variables.debugme) Console.WriteLine("Retry {0}", tries);
                    ec = reader.Read(readBuffer, timeout, out bytesRead);
                    if (variables.debugme) { Console.WriteLine("Bytes Read {0}", bytesRead); Console.WriteLine("Read Buffer {0}", Oper.ByteArrayToString(readBuffer)); }
                    tries++;
                }
                Console.WriteLine("Version: {0}", Oper.ByteArrayToString(readBuffer).Substring(0, 2));

                if (versions[0] != (Oper.ByteArrayToString(readBuffer)) && versions[2] != (Oper.ByteArrayToString(readBuffer)))
                {
                    Console.WriteLine("Wrong Version");
                    buttons(true);
                    return;
                }

                while (!stop && !_bStop)
                {
                    error = Pwr(reader);
                    if (error == 1) { break; }
                    //Thread.Sleep(60);
                    error = log_post(reader);
                    progress();
                    if (error == 3) { break; }
                    error = ShutD(reader);
                    if (error == 1) { break; }
                    counterg++;

                    if (numericIter.Value != 0)
                    {
                        if (variables.debugme) Console.WriteLine("Counterg: {0}  nudges: {1}", counterg, nudges);
                        if (numericIter.Value <= (counterg - nudges))
                        {
                            DisplayData(Environment.NewLine, 0);
                            Console.WriteLine("Reached No. of Boots Required");

                            try
                            {
                                SoundPlayer success = new SoundPlayer(Properties.Resources.chime);
                                if (variables.soundcompare != "") success.SoundLocation = variables.soundcompare;
                                success.Play();
                            }
                            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); };
                            break;
                        }
                    }

                    Cooling = true;
                    SleepIntermittently((int)numericCool.Value * 1000);
                    Cooling = false;
                }
                buttons(true);
                //if ((values.Count >= 20) && (stop == true)) 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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

                MainForm._waitmb.Reset();
                MainForm._waitmb.Set();
            }
        }
        void NewThreadENough()
        {
            Thread.Sleep(2000);
            enough = true;
        }
        void buttons(bool value)
        {
            btnStart.Enabled = value;
            SetupDetailBtn.Enabled = btnStart.Enabled;
            PostOutButton.Enabled = btnStart.Enabled;
            btnStop.Enabled = !value;
            btnNudge.Enabled = !value;
            btnClearRater.Enabled = value;
            btnClearOutput.Enabled = value;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.SystemTxtbox.Visible = false;
            this.SetupDetailBtn.Enabled = false;
            for (int i = 0; i < crashVAL.Length; i++) crashVAL[i] = 0; // BH - reset crash value counters
            stop = false;
            _bStop = false;
            nudges = 0;
            btnStart.Enabled = false;
            values = new List<int>();
            counterg = 0;
            CappeD = false; // BH - reset capped check to false
            if (numericCap.Value != 0) CappeD = true; // BH - check if capped value is used and set check accordingly
            RaterPIC.Image = global::JRunner.Properties.Resources.hourglass_clock;// BH - Blank pic on start
            if (SlimBut.Checked) ConTypeSel = "Slim";
            if (CorBut.Checked) ConTypeSel = "Cor 3+";
            if (PhatFBut.Checked) ConTypeSel = "Phat";
            if (cr4but.Checked) ConTypeSel = "CR4";
            Console.WriteLine(ConTypeSel + " Selected");
            ThreadStart starter = delegate { rate_post(); };
            new Thread(starter).Start();
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            this.SetupDetailBtn.Enabled = true;
            DisplayData(Environment.NewLine, 0);
            txtRate.Cursor = Cursors.IBeam;
            txtProgress.Cursor = Cursors.IBeam;
            CycleClipBtn.Visible = true;
            PostOutButton.Visible = true;
            ResultsClipBtn.Visible = true;
            variables.escapeloop = true;
            _bStop = true;
            btnStop.Enabled = false;
            btnNudge.Enabled = false;
            stop = true;


            ThreadStart starter = delegate { escapedloop(5000); };
            new Thread(starter).Start();

            if (nudge)
            {
                Console.WriteLine("Stopped Mid-Nudge: Resetting................");
                Thread.Sleep(4000);
            }
            else if (Cooling)
            {
                Console.WriteLine("Resetting................");
                Thread.Sleep(3000);
            }
            buttons(true);
        }
        void escapedloop(int time)
        {
            Thread.Sleep(time);
            variables.escapeloop = false;
            nudge = false;
        }
        void escapedexit(int time)
        {
            Thread.Sleep(time);
            variables.escapeloop = false;
        }

        private void POST_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!btnStart.Enabled)
            {
                e.Cancel = true;
                return;
            }
            stop = true;

            _bStop = true;

            variables.escapeloop = true;
            ThreadStart starter = delegate { escapedexit(5000); };
            new Thread(starter).Start();
            if (Cooling) Thread.Sleep(4000);
            Console.SetOut(MainForm._writer);
        }

        private void btnNudge_Click(object sender, EventArgs e)
        {
            btnNudge.Enabled = false;
            nudge = true;
            nudges++;

        }
        private void btnClearOutput_Click(object sender, EventArgs e)
        {
            txtOutput.Text = "";
        }
        private void btnClearRater_Click(object sender, EventArgs e)
        {
            txtRate.Text = ""; // BH - clears rater text box
            RaterPIC.Image = global::JRunner.Properties.Resources.hourglass_clock;// BH - Blank pic on reset
            RaterRes.Text = "More Data Req";// BH - reset rater text on clear
            txtProgress.Text = ""; // BH - added to clear avgs box at same time as glitch numbers
        }

        void progress()
        {
            try
            {
                if (values == null || values.Count <= 0) return;
                string text = "";
                values.Sort();
                if (numericIter.Value != 0) text += "Overall: " + values.Count + " / " + numericIter.Value + Environment.NewLine;
                else text += "Overall: " + values.Count + Environment.NewLine;
                text += "Average: " + values.Average().ToString("F2") + Environment.NewLine;
                text += "Median: " + values.Median() + Environment.NewLine;

                float score = 0;
                foreach (int val in values)
                {
                    float wth;
                    if (val == 1) wth = 1;
                    else if (val == 2) wth = 0.8F;
                    else if (val == 3) wth = 0.6F;
                    else if (val == 4) wth = 0.4F;
                    else if (val == 5) wth = 0.2F;
                    else if (val > 10) wth = 0;
                    else wth = (1 / (float)val);
                    if (val != 0) score += wth;
                }
                score = (score / values.Count) * 10;
                text += String.Format("Score: {0:F}{1}{1}", score, Environment.NewLine);

                var g = values.GroupBy(i => i);
                foreach (var grp in g) //BH - display %'s and group %'s to give a rating!
                {
                    float divide = ((numericIter.Value != 0) && (stop == false)) ? (float)numericIter.Value : values.Count;
                    float val = grp.Count() * 100F / divide;
                    text += String.Format("Cycle {0} - {1:F2}%{2}", grp.Key, val, Environment.NewLine);
                }

                text += Environment.NewLine;
                float sum = 0;
                foreach (var grp in g) //BH - display %'s and group %'s to give a rating!
                {
                    float val = grp.Count() * 100F / values.Count;

                    sum += val;
                    if ((grp.Key % 2 != 0 && grp.Key + 1 != values.Max()) || grp.Key == values.Max())
                    {
                        text += String.Format("{0:F0}% within {1} Cycles{2}", sum, grp.Key, Environment.NewLine);
                    }
                }

                if (values.Count >= 20)
                {
                    if (score >= 9.75F) // BH - add image for median
                    {
                        RaterRes.Text = "WTF!";
                        RaterPIC.Image = global::JRunner.Properties.Resources.perfecto;
                    }
                    else if (score >= 8.75F) // BH - add image for median
                    {
                        RaterRes.Text = "Perfecto!";
                        RaterPIC.Image = global::JRunner.Properties.Resources.perfecto;
                    }
                    else if (score >= 8.0F)
                    {
                        RaterPIC.Image = global::JRunner.Properties.Resources.legend;
                        RaterRes.Text = "Legend";
                    }
                    else if (score >= 7.0F)
                    {
                        RaterPIC.Image = global::JRunner.Properties.Resources.great;
                        RaterRes.Text = "Awesome";
                    }
                    else if (score >= 6.0F)
                    {
                        RaterPIC.Image = global::JRunner.Properties.Resources.good;
                        RaterRes.Text = "Good";
                    }
                    else if (score >= 5.0F)
                    {
                        RaterPIC.Image = global::JRunner.Properties.Resources.average;
                        RaterRes.Text = "Average";
                    }
                    else
                    {
                        RaterPIC.Image = global::JRunner.Properties.Resources.bad;
                        RaterRes.Text = "Fair";
                    }
                }

                //
                //
                //
                //
                // Calculate most fails and display if capped
                //
                //
                //

                List<int> pos = new List<int>();
                int max = 0;
                for (int i = 0; i < crashVAL.Length; i++)
                {
                    if (crashVAL[i] > 0)
                    {
                        if (crashVAL[i] == max)
                        {
                            pos.Add(i);
                        }
                        else if (crashVAL[i] > max)
                        {
                            max = crashVAL[i];
                            pos.Clear();
                            pos.Add(i);
                        }
                    }
                }


                text += Environment.NewLine;
                if (CappeD == true) text += "* Capped *" + Environment.NewLine;
                foreach (int lo in pos)
                {
                    if ((values.Count >= 20))
                    {
                        text += "Fails most on: " + "0" + (crash)lo + Environment.NewLine;
                    }
                    else
                    {
                        Console.WriteLine("Most Fails(cumulative): 0{0}", (crash)lo);
                        // if (variables.debugme) Console.WriteLine("Number of fails on 0x09: 0{0}", crashVAL[(int)crash.x09]);
                    }
                }
                DisplayData2(text);

                //
                //
                //
                //      BH display fails and if capped
                //
                //
                //

            }
            catch (Exception) { }
        }

        private void numericCap_ValueChanged(object sender, EventArgs e)
        {
            CappeD = true;
        }

        private void CycleClipBtn_Click(object sender, EventArgs e)
        {
            if ((txtRate.Text.Length >= 1)) Clipboard.SetText(txtRate.Text);
            //
        }
        private void ResultsClipBtn_Click(object sender, EventArgs e)
        {
            if ((txtProgress.Text.Length >= 1))
            {
                Clipboard.SetText(txtProgress.Text);
            }
        }
        private void txtRate_Enter(object sender, EventArgs e)
        {
            if (stop == false)
            {
                CycleClipBtn.Visible = true;
                txtRate.Cursor = Cursors.No;
                ActiveControl = CycleClipBtn;
            }
        }
        private void txtOutput_Enter(object sender, EventArgs e)
        {

            PostOutButton.Visible = true;
        }
        private void txtProgress_Enter(object sender, EventArgs e)
        {
            if (stop == false)
            {
                ResultsClipBtn.Visible = true;
                txtProgress.Cursor = Cursors.No;
                ActiveControl = ResultsClipBtn;
            }
        }

        private void PhatBut_CheckedChanged(object sender, EventArgs e)
        {
            PhatBut.Checked = !SlimBut.Checked & !CorBut.Checked & !PhatFBut.Checked & !cr4but.Checked;
        }

        private void SlimBut_CheckedChanged(object sender, EventArgs e)
        {
            // if (SlimBut.Checked) ConTypeSel = "Slim";
        }

        private void PhatFBut_CheckedChanged(object sender, EventArgs e)
        {
            // if (PhatFBut.Checked) ConTypeSel = "Phat";
        }

        private void ScreenshotBTN_Click(object sender, EventArgs e)
        {
            Rectangle bounds = this.Bounds;
            Size block = bounds.Size;
            block.Height += 10;
            block.Width += 10;
            using (Bitmap bitmap = new Bitmap(bounds.Width + 10, bounds.Height + 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left - 5, bounds.Top - 5), Point.Empty, block);
                }



                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Convert.ToString(Environment.SpecialFolder.MyDocuments);
                saveFileDialog1.FileName = "My Rater Screenshot";
                saveFileDialog1.Filter = "(*.png)|*.png|All Files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (variables.debugme) Console.WriteLine(saveFileDialog1.FileName);//Do what you want here
                }
                bitmap.Save(saveFileDialog1.FileName, ImageFormat.Png);
            }
            if (this.SystemTxtbox.Visible == true) { (this.SystemTxtbox.Visible) = !(this.SystemTxtbox.Visible); }
        }

        private void SetupDetailBtn_Click(object sender, EventArgs e)
        {
            (this.SystemTxtbox.Visible) = !(this.SystemTxtbox.Visible);

        }

        private void PostOutButton_Click(object sender, EventArgs e)
        {
            if ((txtOutput.Text.Length >= 1))
            {
                //Clipboard.SetText(txtOutput.Text);
                SaveFileDialog saveFileDialog2 = new SaveFileDialog();
                saveFileDialog2.InitialDirectory = Convert.ToString(Environment.SpecialFolder.MyDocuments);
                saveFileDialog2.FileName = "My Post output";
                saveFileDialog2.Filter = "(*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog2.FilterIndex = 1;
                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog2.FileName, txtOutput.Text);
                }
                Clipboard.SetText("[CODE]" + txtOutput.Text + "[/CODE]");
                MessageBox.Show("Post Out window contents copied to Clipboard & Prepared for pasting to the forum as [CODE][/CODE].", "For TX Forum Use");
            }
        }

        private void CorBut_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RaterSettings_Enter(object sender, EventArgs e)
        {

        }

        private void POST_Load(object sender, EventArgs e)
        {
            updateLogColor();
        }

        public void updateLogColor()
        {
            txtOutput.BackColor = variables.logbackground;
            txtProgress.BackColor = variables.logbackground;
            txtRate.BackColor = variables.logbackground;
            txtOutput.ForeColor = variables.logtext;
            txtProgress.ForeColor = variables.logtext;
            txtRate.ForeColor = variables.logtext;
        }
    }
    public static class EnumerableExtensions
    {
        public static decimal Median(this IEnumerable<int> list)
        {
            int[] temp = list.ToArray();
            Array.Sort(temp);

            int count = temp.Length;
            if (count == 0)
            {
                throw new InvalidOperationException("Empty collection");
            }
            else if (count % 2 == 0)
            {
                // count is even, average two middle elements
                int a = temp[count / 2 - 1];
                int b = temp[count / 2];
                return (a + b) / 2m;
            }
            else
            {
                // count is odd, return the middle element
                return temp[count / 2];
            }

        }
    }
}