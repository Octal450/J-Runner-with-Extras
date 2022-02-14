using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace JRunner
{
    class IP
    {
        private static string cpukey = "";
        private static int ldvvalue = 0;
        static List<string> ip = new List<string>();
        static bool found = false;
        static string localGatewayIp = "?";

        string getarptable()
        {
            sendAsyncPingPacket(changelastquad(localGatewayIp, "255"));
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
        string parsearp(string mac, string table)
        {
            if (variables.debugme) Console.WriteLine(mac);
            if (mac == "nomac") return "0.0.0.0";
            string[] values = table.Split('\n');
            string ip = "0.0.0.0";
            foreach (string val in values)
            {
                if (val.ToLower().Contains(mac.ToLower()))
                {
                    if (variables.debugme) Console.WriteLine(val);
                    foreach (string lo in val.Split(' '))
                    {
                        if (IsIPv4(lo)) ip = lo;
                    }
                    if (variables.debugme) Console.WriteLine(ip);
                }
            }
            return ip;
        }
        string getmacaddress()
        {
            if (!File.Exists(variables.filename1)) return "nomac";
            byte[] smc_config;
            int block_offset;
            smc_config = Nand.Nand.getsmcconfig(variables.filename1, out block_offset);
            string mac = Regex.Replace(Oper.ByteArrayToString(Oper.returnportion(smc_config, 0x220 + block_offset, 6)), @"(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})", @"$1-$2-$3-$4-$5-$6");
            return mac;
        }

        public string IP_GetCpuKey(string ip, int saveDir = 0)
        {
            ldvvalue = 0;
            cpukey = "";
            string folder = variables.outfolder;
            if (saveDir == 2) folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            else if (saveDir == 1) folder = MainForm.mainForm.getCurrentWorkingFolder();
            if (File.Exists(Path.Combine(folder, "Fuses.txt"))) File.Delete(Path.Combine(folder, "Fuses.txt"));
            string fuses = ("http://" + ip + @"/FUSE");
            WebClient Client = new WebClient();
            try
            {
                string page = Client.DownloadString("http://" + ip);
                string regex = @"(?<=<title.*>)([\s\S]*)(?=</title>)";
                Regex ex = new Regex(regex, RegexOptions.IgnoreCase);
                if (variables.debugme) Console.WriteLine(ex.Match(page).Value.Trim());
                Console.WriteLine("Getting info from {0}...", ip);
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
                    StreamWriter SW = File.AppendText(Path.Combine(folder, "Fuses.txt"));
                    foreach (string oi in fuck)
                    {
                        SW.WriteLine(oi);
                    }
                    string cpukeytag = page.Substring(page.IndexOf("CPU Key:"), 70);
                    if (variables.debugme) Console.WriteLine("Cpukey before edit: {0}", cpukeytag);
                    cpukey = cpukeytag.Substring(cpukeytag.IndexOf("<td>") + 4, 32);
                    variables.cpkey = cpukey;
                    if (variables.debugme) Console.WriteLine("Cpukey: {0}", cpukey);
                    string dvdkeytag = page.Substring(page.IndexOf("DVD Key:"), 70);
                    if (variables.debugme) Console.WriteLine("DVDkey before edit: {0}", dvdkeytag);

                    cpukeytag = StripTagsCharArray(cpukeytag);
                    dvdkeytag = StripTagsCharArray(dvdkeytag);
                    if (variables.debugme) Console.WriteLine("Cpukey after edit: {0}", cpukeytag);
                    if (variables.debugme) Console.WriteLine("dvdkey after edit: {0}", dvdkeytag);
                    SW.WriteLine("");
                    SW.WriteLine(cpukeytag);
                    SW.WriteLine(dvdkeytag);
                    MainForm._event1.Set();
                    SW.Close();
                }
                else if (ex.Match(page).Value.Trim().Contains("XeLLous"))
                {
                    string cpukeytag = page.Substring(page.IndexOf("CPU"), 70);
                    if (variables.debugme) Console.WriteLine("Cpukey before edit: {0}", cpukeytag);
                    cpukey = cpukeytag.Substring(cpukeytag.IndexOf("<td>") + 4, 32);
                    variables.cpkey = cpukey;
                    if (variables.debugme) Console.WriteLine("Cpukey: {0}", cpukey);

                    cpukeytag = StripTagsCharArray(cpukeytag);
                    if (variables.debugme) Console.WriteLine("Cpukey after edit: {0}", cpukeytag);
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
                    if (variables.debugme) Console.WriteLine(ldvvalue);
                    StreamWriter SW = File.AppendText(Path.Combine(folder, "Fuses.txt"));
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
            if (variables.debugme) Console.WriteLine("Finished");
            return cpukey;
        }
        public string IP_GetCpuKey(string ip, bool print, int saveDir = 0)
        {
            ldvvalue = 0;
            cpukey = "";
            if (variables.debugme) Console.WriteLine(ip);
            string folder = variables.outfolder;
            if (saveDir == 2) folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            else if (saveDir == 1) folder = MainForm.mainForm.getCurrentWorkingFolder();
            if (File.Exists(Path.Combine(folder, "Fuses.txt"))) File.Delete(Path.Combine(folder, "Fuses.txt"));
            string fuses = ("http://" + ip + @"/FUSE");
            WebClient Client = new WebClient();
            try
            {
                string page = Client.DownloadString("http://" + ip);
                string regex = @"(?<=<title.*>)([\s\S]*)(?=</title>)";
                Regex ex = new Regex(regex, RegexOptions.IgnoreCase);
                if (variables.debugme) Console.WriteLine(ex.Match(page).Value.Trim());
                if (print) Console.WriteLine("Getting info from {0}...", ip);
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
                    Console.WriteLine("Lock Down Value: {0}", ldvvalue);
                    StreamWriter SW = File.AppendText(Path.Combine(folder, "Fuses.txt"));
                    foreach (string oi in fuck)
                    {
                        SW.WriteLine(oi);
                    }
                    string cpukeytag = page.Substring(page.IndexOf("CPU Key:"), 70);
                    if (variables.debugme) Console.WriteLine("Cpukey before edit: {0}", cpukeytag);
                    cpukey = cpukeytag.Substring(cpukeytag.IndexOf("<td>") + 4, 32);
                    variables.cpkey = cpukey;
                    if (variables.debugme) Console.WriteLine("Cpukey: {0}", cpukey);
                    string dvdkeytag = page.Substring(page.IndexOf("DVD Key:"), 70);
                    if (variables.debugme) Console.WriteLine("DVDkey before edit: {0}", dvdkeytag);

                    cpukeytag = StripTagsCharArray(cpukeytag);
                    dvdkeytag = StripTagsCharArray(dvdkeytag);
                    if (variables.debugme) Console.WriteLine("Cpukey after edit: {0}", cpukeytag);
                    if (variables.debugme) Console.WriteLine("dvdkey after edit: {0}", dvdkeytag);
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
                    if (variables.debugme) Console.WriteLine("Cpukey before edit: {0}", cpukeytag);
                    cpukey = cpukeytag.Substring(cpukeytag.IndexOf("<td>") + 4, 32);
                    variables.cpkey = cpukey;
                    if (variables.debugme) Console.WriteLine("Cpukey: {0}", cpukey);

                    cpukeytag = StripTagsCharArray(cpukeytag);
                    if (variables.debugme) Console.WriteLine("Cpukey after edit: {0}", cpukeytag);
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
                    StreamWriter SW = File.AppendText(Path.Combine(folder, "Fuses.txt"));
                    for (int i = 1; i < fuck.Count(); i++)
                    {
                        SW.WriteLine(fuck[i]);
                    }
                    SW.Close();
                    MainForm._event1.Set();
                }
            }
            catch (System.Net.WebException) { if (print) Console.WriteLine("Connection TimeOut"); return cpukey; }
            catch (Exception ex) { if (print) Console.WriteLine(ex.ToString()); return cpukey; }
            if (variables.debugme) Console.WriteLine("Finished");
            return cpukey;
        }



        static bool IsIPv4(string value)
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

        public static string getGatewayIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string address;
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    address = ip.ToString();
                    return address.Substring(0, address.LastIndexOf('.') + 1);
                }
            }
            return "?";
        }

        public static void initaddresses()
        {
            localGatewayIp = getGatewayIp();
            if (!IsIPv4(variables.IPstart))
            {
                if (localGatewayIp != "?")
                {
                    variables.IPstart = changelastquad(localGatewayIp, "0");
                }
            }
            if (!IsIPv4(variables.IPend))
            {
                if (localGatewayIp != "?")
                {
                    variables.IPend = changelastquad(localGatewayIp, "255");
                }
            }
        }

        private void sendAsyncPingPacket(string hostToPing)
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
        private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
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
        public void DisplayReply(PingReply reply)
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
        private long ToInt(string addr)
        {

            return (uint)System.Net.IPAddress.NetworkToHostOrder(
                BitConverter.ToInt32(IPAddress.Parse(addr).GetAddressBytes(), 0));
        }
        private string ToAddr(long address)
        {
            return System.Net.IPAddress.Parse(address.ToString()).ToString();
        }
        private void scanLiveHosts(string ipFrom, string ipTo, ProgressBar pb)
        {
            long from = ToInt(ipFrom);
            long to = ToInt(ipTo);
            int i;
            long ipLong = ToInt(ipFrom);
            while (ipLong < to)
            {
                i = ((int)(ipLong - from) * pb.Maximum) / (int)(to - from);
                pb.BeginInvoke(new Action(() => pb.Value = i));
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
            pb.BeginInvoke(new Action(() => pb.Value = pb.Maximum));
        }

        public string getkey()
        {
            return cpukey;
        }
        public int getldv()
        {
            return ldvvalue;
        }

        public void IPScanner(ProgressBar pb)
        {
            found = false;
            initaddresses();
            Console.WriteLine("Tip: Load the nand on source to have quicker results.");
            variables.isscanningip = true;
            try
            {
                IP_GetCpuKey(parsearp(getmacaddress(), getarptable()), false);
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            if (found)
            {
                Console.WriteLine("Finished");
                Console.WriteLine("");
                variables.isscanningip = false;
                return;
            }
            bool use = false;
            Console.WriteLine("IP Scan Stage 1: Please wait...");
            scanLiveHosts(variables.IPstart, variables.IPend, pb);
            Thread.Sleep(500);
            found = false;
            Console.WriteLine("IP Scan Stage 2: This will take some time...");
            pb.BeginInvoke(new Action(() => pb.Value = pb.Minimum));
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
                pb.BeginInvoke(new Action(() => pb.Value = pb.Value + (100 / (ip.Count + 1))));
                if (o != localGatewayIp) IP_GetCpuKey(o, false);
                if (found) break;
            }
            Console.WriteLine("");
            pb.BeginInvoke(new Action(() => pb.Value = pb.Maximum));
            if (!found) Console.WriteLine("No Xbox Detected");
            else Console.WriteLine("Finished");
            Console.WriteLine("");
            variables.isscanningip = false;
        }

        public string DownloadWebPage(string Url)
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
        public string StripTagsCharArray(string source)
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
