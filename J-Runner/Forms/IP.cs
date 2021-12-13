using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Security;
using System.Security.Cryptography;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Timers;
using Microsoft.Win32;
using System.Reflection;
using Microsoft.Win32.SafeHandles;
using System.Management;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using FTPLib;

namespace JRunner
{
    public partial class IP : Form
    {
        public static string cpukey = "";
        public static int ldvvalue = 0;
        public static FTP ftplib = new FTP();
        static List<string> ip = new List<string>();
        static bool found = false;
        static string localIP = "?";
        public IP()
        {
            InitializeComponent();

            btnGetCpu.DialogResult = DialogResult.OK;
            /*IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();

            IPEndPoint[] endPoints = ipProperties.GetActiveTcpListeners();
            TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation info in tcpConnections)
            {
                Console.WriteLine("Local : " + info.LocalEndPoint.Address.ToString()
                + ":" + info.LocalEndPoint.Port.ToString()
                + "\nRemote : " + info.RemoteEndPoint.Address.ToString()
                + ":" + info.RemoteEndPoint.Port.ToString()
                + "\nState : " + info.State.ToString() + "\n\n");
            }*/
            
        }

        private static string getarptable()
        {
            sendAsyncPingPacket(changelastquad(localIP, "255"));
            string sResults = "";
            System.Diagnostics.ProcessStartInfo ps = new System.Diagnostics.ProcessStartInfo("arp", "-a");
            ps.CreateNoWindow = true;
            ps.UseShellExecute = false;
            ps.RedirectStandardOutput = true;
            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {
                proc.StartInfo = ps;
                proc.Start();
                System.IO.StreamReader sr = proc.StandardOutput;
                while (!proc.HasExited) ;
                sResults = sr.ReadToEnd();
            }
            return sResults;
        }
        private static string parsearp(string mac, string table)
        {
            if (MainForm.debugme) Console.WriteLine(mac);
            if (mac == "nomac") return "0.0.0.0";
            string[] values = table.Split('\n');
            string ip = "0.0.0.0";
            foreach (string val in values)
            {
                if (val.ToLower().Contains(mac.ToLower()))
                {
                    if (MainForm.debugme) Console.WriteLine(val);
                    foreach (string lo in val.Split(' '))
                    {
                        if (IsIPv4(lo)) ip = lo;
                    }
                    if (MainForm.debugme) Console.WriteLine(ip);
                }
            }
            return ip;
        }
        private static string getmacaddress()
        {
            if (!File.Exists(variables.filename1)) return "nomac";
            byte[] smc_config;
            bool big_block = false;
            bool corona = false;
            int block_offset;
            smc_config = Nand.Nand.getsmcconfig(variables.filename1, ref big_block, ref corona);
            if (big_block)
            {
                if (corona) block_offset = 0;
                else block_offset = 0x60000;
            }
            else block_offset = 0xC000;
            string mac = Regex.Replace(Oper.ByteArrayToString(Oper.returnportion(smc_config, 0x220 + block_offset, 5)), @"(.{2})(.{2})(.{2})(.{2})(.{2})", @"$1-$2-$3-$4-$5");
            return mac;
        }

        private void btnGetCpu_Click(object sender, EventArgs e)
        {
            IP_GetCpuKey(txtIP.Text);
            this.Close();
            //File.WriteAllText("Fuses.txt", fuses);
        }
        public static string IP_GetCpuKey(string ip)
        {
            ldvvalue = 0;
            cpukey = "";
            if (File.Exists(Path.Combine(variables.outfolder, "Fuses.txt"))) File.Delete(Path.Combine(variables.outfolder, "Fuses.txt"));
            string fuses = ("http://" + ip + @"/FUSE");
            WebClient Client = new WebClient();
            try
            {
                string page = Client.DownloadString("http://" + ip);
                string regex = @"(?<=<title.*>)([\s\S]*)(?=</title>)";
                Regex ex = new Regex(regex, RegexOptions.IgnoreCase);
                if (MainForm.debugme) Console.WriteLine(ex.Match(page).Value.Trim());
                Console.WriteLine("Getting info from ip {0}...", ip);
                if (ex.Match(page).Value.Trim().Contains("Reloaded"))
                {
                    string fuse = Client.DownloadString(fuses);
                    string[] fuck = Regex.Split(fuse, "\n");
                    foreach (char c in fuck[7].Substring(fuck[7].IndexOf(':')))
                    {
                        if (c == 'f') ldvvalue++;
                    }
                    foreach (char c in fuck[8].Substring(fuck[8].IndexOf(':')))
                    {
                        if (c == 'f') ldvvalue++;
                    }
                    Console.WriteLine("LockDownValue is {0}", ldvvalue);
                    StreamWriter SW = File.AppendText(Path.Combine(variables.outfolder, "Fuses.txt"));
                    foreach (string oi in fuck)
                    {
                        SW.WriteLine(oi);
                    }
                    string cpukeytag = page.Substring(page.IndexOf("CPU Key:"), 70);
                    if (MainForm.debugme) Console.WriteLine("Cpukey before edit: {0}", cpukeytag);
                    cpukey = cpukeytag.Substring(cpukeytag.IndexOf("<td>") + 4, 32);
                    variables.cpkey = cpukey;
                    if (MainForm.debugme) Console.WriteLine("Cpukey: {0}", cpukey);
                    string dvdkeytag = page.Substring(page.IndexOf("DVD Key:"), 70);
                    if (MainForm.debugme) Console.WriteLine("DVDkey before edit: {0}", dvdkeytag);

                    cpukeytag = StripTagsCharArray(cpukeytag);
                    dvdkeytag = StripTagsCharArray(dvdkeytag);
                    if (MainForm.debugme) Console.WriteLine("Cpukey after edit: {0}", cpukeytag);
                    if (MainForm.debugme) Console.WriteLine("dvdkey after edit: {0}", dvdkeytag);
                    SW.WriteLine("");
                    SW.WriteLine(cpukeytag);
                    SW.WriteLine(dvdkeytag);
                    MainForm._event1.Set();
                    SW.Close();
                }
                else if (ex.Match(page).Value.Trim().Contains("XeLLous"))
                {
                    string cpukeytag = page.Substring(page.IndexOf("CPU"), 70);
                    if (MainForm.debugme) Console.WriteLine("Cpukey before edit: {0}", cpukeytag);
                    cpukey = cpukeytag.Substring(cpukeytag.IndexOf("<td>") + 4, 32);
                    variables.cpkey = cpukey;
                    if (MainForm.debugme) Console.WriteLine("Cpukey: {0}", cpukey);

                    cpukeytag = StripTagsCharArray(cpukeytag);
                    if (MainForm.debugme) Console.WriteLine("Cpukey after edit: {0}", cpukeytag);
                    string fuse = Client.DownloadString(fuses);
                    string[] fuck = Regex.Split(fuse, "\n");
                    foreach (char c in fuck[8].Substring(fuck[8].IndexOf(':')))
                    {
                        if (c == 'f') ldvvalue++;
                    }
                    foreach (char c in fuck[9].Substring(fuck[9].IndexOf(':')))
                    {
                        if (c == 'f') ldvvalue++;
                    }
                    if (MainForm.debugme) Console.WriteLine(ldvvalue);
                    StreamWriter SW = File.AppendText(Path.Combine(variables.outfolder, "Fuses.txt"));
                    for (int i = 1; i < fuck.Count(); i++) 
                    {
                        SW.WriteLine(fuck[i]);
                    }
                    SW.Close();
                    MainForm._event1.Set();
                }
            }
            catch (System.Net.WebException) { Console.WriteLine("Connection TimeOut"); return cpukey; }
            catch (Exception ex) { Console.WriteLine(ex.Message); return cpukey; }
            if (MainForm.debugme) Console.WriteLine("Finished");
            return cpukey;
        }
        public static string IP_GetCpuKey(string ip, bool print)
        {
            ldvvalue = 0;
            cpukey = "";
            if (MainForm.debugme) Console.WriteLine(ip);
            if (File.Exists(Path.Combine(variables.outfolder, "Fuses.txt"))) File.Delete(Path.Combine(variables.outfolder, "Fuses.txt"));
            string fuses = ("http://" + ip + @"/FUSE");
            WebClient Client = new WebClient();
            try
            {
                string page = Client.DownloadString("http://" + ip);
                string regex = @"(?<=<title.*>)([\s\S]*)(?=</title>)";
                Regex ex = new Regex(regex, RegexOptions.IgnoreCase);
                if (MainForm.debugme) Console.WriteLine(ex.Match(page).Value.Trim());
                if (print) Console.WriteLine("Getting info from ip {0}...", ip);
                if (ex.Match(page).Value.Trim().Contains("Reloaded"))
                {
                    Console.WriteLine("");
                    found = true;
                    string fuse = Client.DownloadString(fuses);
                    string[] fuck = Regex.Split(fuse, "\n");
                    foreach (char c in fuck[7].Substring(fuck[7].IndexOf(':')))
                    {
                        if (c == 'f') ldvvalue++;
                    }
                    foreach (char c in fuck[8].Substring(fuck[8].IndexOf(':')))
                    {
                        if (c == 'f') ldvvalue++;
                    }
                    Console.WriteLine("LockDownValue is {0}", ldvvalue);
                    StreamWriter SW = File.AppendText(Path.Combine(variables.outfolder, "Fuses.txt"));
                    foreach (string oi in fuck)
                    {
                        SW.WriteLine(oi);
                    }
                    string cpukeytag = page.Substring(page.IndexOf("CPU Key:"), 70);
                    if (MainForm.debugme) Console.WriteLine("Cpukey before edit: {0}", cpukeytag);
                    cpukey = cpukeytag.Substring(cpukeytag.IndexOf("<td>") + 4, 32);
                    variables.cpkey = cpukey;
                    if (MainForm.debugme) Console.WriteLine("Cpukey: {0}", cpukey);
                    string dvdkeytag = page.Substring(page.IndexOf("DVD Key:"), 70);
                    if (MainForm.debugme) Console.WriteLine("DVDkey before edit: {0}", dvdkeytag);

                    cpukeytag = StripTagsCharArray(cpukeytag);
                    dvdkeytag = StripTagsCharArray(dvdkeytag);
                    if (MainForm.debugme) Console.WriteLine("Cpukey after edit: {0}", cpukeytag);
                    if (MainForm.debugme) Console.WriteLine("dvdkey after edit: {0}", dvdkeytag);
                    Console.WriteLine("CPU Key is {0}", cpukey);
                    SW.WriteLine("");
                    SW.WriteLine(cpukeytag);
                    SW.WriteLine(dvdkeytag);
                    MainForm._event1.Set();
                    SW.Close();
                }
                else if (ex.Match(page).Value.Trim().Contains("XeLLous"))
                {
                    Console.WriteLine("");
                    found = true;
                    string cpukeytag = page.Substring(page.IndexOf("CPU"), 70);
                    if (MainForm.debugme) Console.WriteLine("Cpukey before edit: {0}", cpukeytag);
                    cpukey = cpukeytag.Substring(cpukeytag.IndexOf("<td>") + 4, 32);
                    variables.cpkey = cpukey;
                    if (MainForm.debugme) Console.WriteLine("Cpukey: {0}", cpukey);

                    cpukeytag = StripTagsCharArray(cpukeytag);
                    if (MainForm.debugme) Console.WriteLine("Cpukey after edit: {0}", cpukeytag);
                    string fuse = Client.DownloadString(fuses);
                    string[] fuck = Regex.Split(fuse, "\n");
                    foreach (char c in fuck[8].Substring(fuck[8].IndexOf(':')))
                    {
                        if (c == 'f') ldvvalue++;
                    }
                    foreach (char c in fuck[9].Substring(fuck[9].IndexOf(':')))
                    {
                        if (c == 'f') ldvvalue++;
                    }
                    Console.WriteLine("LockDownValue is {0}", ldvvalue);
                    Console.WriteLine("CPU Key is {0}", cpukey);
                    StreamWriter SW = File.AppendText(Path.Combine(variables.outfolder, "Fuses.txt"));
                    for (int i = 1; i < fuck.Count(); i++)
                    {
                        SW.WriteLine(fuck[i]);
                    }
                    SW.Close();
                    MainForm._event1.Set();
                }
            }
            catch (System.Net.WebException) { if (print) Console.WriteLine("Connection TimeOut"); return cpukey; }
            catch (Exception ex) { if (print)Console.WriteLine(ex.ToString()); return cpukey; }
            if (MainForm.debugme) Console.WriteLine("Finished");
            return cpukey;
        }

        public static void generate(string ip)
        {
            try
            {
                ftplib.user = "jrunner";
                ftplib.pass = "rocks";
                Console.WriteLine("--> Connecting...");
                ftplib.Connect(ip, ftplib.user, ftplib.pass);

                RegistryKey JRunnerO = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
                string[] values = JRunnerO.GetValueNames();
                byte[] value = (byte[])JRunnerO.GetValue("DigitalProductId");
                string file = "C:\\" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".bin";
                Oper.savefile(value, file);
                int perc = 0;
                if (!ftplib.IsConnected)
                {
                    Console.WriteLine("E: Must be connected to a server.");
                    return;
                }

                ftplib.OpenUpload(file, System.IO.Path.GetFileName(file));
                while (ftplib.DoUpload() > 0)
                {
                    perc = (int)(((ftplib.BytesTotal) * 100) / ftplib.FileSize);
                    Console.Write("\rUpload: {0}/{1} {2}%", ftplib.BytesTotal, ftplib.FileSize, perc);
                    Console.Out.Flush();
                }
                Console.WriteLine("");
                File.Delete(file);
                ftplib.Disconnect();
                Console.WriteLine("Contact Author for key");
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        public static bool IsIPv4(string value)
        {
            var quads = value.Split('.');

            // if we do not have 4 quads, return false
            if (quads.Length != 4) return false;

            // for each quad
            foreach (var quad in quads)
            {
                int q;
                // if parse fails 
                // or length of parsed int != length of quad string (i.e.; '1' vs '001')
                // or parsed int < 0
                // or parsed int > 255
                // return false
                if (!Int32.TryParse(quad, out q)
                    || !q.ToString().Length.Equals(quad.Length)
                    || q < 0
                    || q > 255) { return false; }

            }

            return true;
        }
        static string changelastquad(string ip, string lastquad)
        {
            string[] quads = ip.Split('.');
            if (quads.Length != 4) return "0.0.0.0";
            quads[3] = lastquad;
            return String.Join(".", quads);
        }

        public static void initaddresses()
        {
            localIP = "?";
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ips in host.AddressList)
            {
                if (ips.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = ips.ToString();
                }
            }
            if (!IsIPv4(variables.IPstart))
            {
                if (localIP != "?")
                {
                    variables.IPstart = changelastquad(localIP, "0");
                }
            }
            if (!IsIPv4(variables.IPend))
            {
                if (localIP != "?")
                {
                    variables.IPend = changelastquad(localIP, "255");
                }
            }
        }

        private static void sendAsyncPingPacket(string hostToPing)
        {
            try
            {
                int timeout = 100;
                Ping pingPacket = new Ping();
                AutoResetEvent waiter = new AutoResetEvent(false);
                pingPacket.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);
                string data = "Ping test check";
                byte[] byteBuffer = Encoding.ASCII.GetBytes(data);
                PingOptions pingOptions = new PingOptions(255, true);
                pingPacket.SendAsync(hostToPing, timeout, byteBuffer, pingOptions, waiter);
            }
            catch (PingException)
            {
                Console.WriteLine("INVALID IP ADDRESS FOUND");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exceptin " + ex.Message);
            }

        }
        private static void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    Console.WriteLine("Ping canceled.");
                    ((AutoResetEvent)e.UserState).Set();
                }
                if (e.Error != null)
                {
                    Console.WriteLine("Ping failed>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ");
                    ((AutoResetEvent)e.UserState).Set();
                }

                PingReply reply = e.Reply;

                if (reply.Status == IPStatus.Success)
                {
                    if (!ip.Contains(reply.Address.ToString())) ip.Add(reply.Address.ToString());
                }
                ((AutoResetEvent)e.UserState).Set();
            }
            catch (PingException)
            {
                Console.WriteLine("INVALID IP ADDRESS");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.Message);
            }
        }
        public static void DisplayReply(PingReply reply)
        {
            if (reply == null)
                return;

            //Console.WriteLine("ping status: {0}", reply.Status);
            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", reply.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
            }
        }
        private static long ToInt(string addr)
        {

            return (long)(uint)System.Net.IPAddress.NetworkToHostOrder(
                BitConverter.ToInt32(IPAddress.Parse(addr).GetAddressBytes(), 0));
        }
        private static string ToAddr(long address)
        {
            return System.Net.IPAddress.Parse(address.ToString()).ToString();
        }
        private static void scanLiveHosts(string ipFrom, string ipTo, ref ProgressBar pb)
        {
            long from = ToInt(ipFrom);
            long to = ToInt(ipTo);
            int i;
            long ipLong = ToInt(ipFrom);
            while (ipLong < to)
            {
                i = ((int)(ipLong - from) * pb.Maximum) / (int)(to - from);
                pb.Value = i;
                //Console.Write("\r{0}%   ", i);
                string address = ToAddr(ipLong);
                sendAsyncPingPacket(address);
                sendAsyncPingPacket(address);
                sendAsyncPingPacket(address);
                sendAsyncPingPacket(address);
                Thread.Sleep(5);
                ipLong++;
            }
            //Console.WriteLine("\r100%   ");
            pb.Value = pb.Maximum;
        }

        public string getkey()
        {
            return cpukey;
        }
        public int getldv()
        {
            return ldvvalue;
        }

        public static void IPScanner(ref ProgressBar pb)
        {
            initaddresses();
            IP_GetCpuKey(parsearp(getmacaddress(), getarptable()), false);
            if (found)
            {
                Console.WriteLine("Finished");
                return;
            }
            bool use = false;
            Console.Write("Scanning..");
            scanLiveHosts(variables.IPstart, variables.IPend, ref pb);
            Thread.Sleep(500);
            found = false;
            foreach (string o in ip)
            {
                IPAddress myScanIP = IPAddress.Parse(o);
                IPHostEntry myScanHost = null;
                try
                {
                    if (use) myScanHost = Dns.GetHostEntry(myScanIP);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                if (myScanHost != null)
                {
                    Console.WriteLine(myScanHost.HostName.ToString() + "\t");
                }
                Console.Write(".");
                if (o != localIP) IP_GetCpuKey(o, false);
                if (found) break;
            }
            Console.WriteLine("");
            if (!found) Console.WriteLine("No Xbox Detected");
            Console.WriteLine("Finished");
        }


        public static string DownloadWebPage(string Url)
        {
            // Open a connection
            HttpWebRequest WebRequestObject = (HttpWebRequest)HttpWebRequest.Create(new Uri(Url));

            // You can also specify additional header values like 
            // the user agent or the referer:
            WebRequestObject.UserAgent = ".NET Framework/2.0";

            // Request response:
            WebResponse Response = WebRequestObject.GetResponse();

            // Open data stream:
            Stream WebStream = Response.GetResponseStream();

            // Create reader object:
            StreamReader Reader = new StreamReader(WebStream);

            // Read the entire stream content:
            string PageContent = Reader.ReadToEnd();

            // Cleanup
            Reader.Close();
            WebStream.Close();
            Response.Close();

            return PageContent;
        }
        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
