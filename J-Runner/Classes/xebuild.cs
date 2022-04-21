using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace JRunner.Classes
{

    class xebuild
    {
        public enum XebuildError
        {
            none = 0,
            nocpukey,
            nofile,
            filemissing,
            nobinfile,
            nodash,
            noconsole,
            noinis,
            nobootloaders,
            wrongcpukey
        }

        private string _cpukey;
        private variables.hacktypes _ttype;
        private int _dash;
        private consoles _ctype;
        private bool _audclamp;
        private bool _CR4;
        private bool _SMCP;
        private bool _CleanSMC;
        private bool _rgh3;
        private bool _bigffs;
        private bool _zfuse;
        private bool _xdkbuild;
        private bool _fullDataClean;
        private bool _altoptions;
        private bool _DLpatches;
        private bool _includeLaunch;
        private bool _rjtag;
        private bool _nowrite;
        private bool _noava;
        private bool _clean;
        private bool _noreeb;
        private bool _xlusb;
        private Nand.PrivateN _nand;
        private List<String> _patches;

        public void loadvariables(string cpukey, variables.hacktypes ttype, int dash, consoles ctype, List<String> patches, Nand.PrivateN nand, bool altoptions, bool DLpatches, bool includeLaunch, bool audclamp, bool rjtag, bool cleansmc, bool cr4, bool smcp, bool rgh3, bool bigffs, bool zfuse, bool xdkbuild, bool xlusb, bool fullDataClean)
        {
            this._cpukey = cpukey;
            this._ttype = ttype;
            this._dash = dash;
            this._ctype = ctype;
            this._patches = patches;
            this._nand = nand;
            this._altoptions = altoptions;
            this._DLpatches = DLpatches;
            this._includeLaunch = includeLaunch;
            this._audclamp = audclamp;
            this._rjtag = rjtag;
            this._CleanSMC = cleansmc;
            this._CR4 = cr4;
            this._SMCP = smcp;
            this._rgh3 = rgh3;
            this._bigffs = bigffs;
            this._zfuse = zfuse;
            this._xdkbuild = xdkbuild;
            this._xlusb = xlusb;
            this._fullDataClean = fullDataClean;
        }

        public List<int> getCBs()
        {
            List<int> cbs = new List<int>();
            string[] ommit = { "version", "security", "flashfs" };
            foreach (string s in parse_ini.getlabels(Path.Combine(variables.rootfolder, @"xeBuild\" + _dash + @"\_" + _ttype + ".ini")))
            {
                if (!ommit.Contains(s) && s.Contains(_ctype.Ini))
                {
                    int cb;
                    if (int.TryParse(s.Replace(_ctype.Ini + "bl_", ""), out cb)) cbs.Add(cb);
                }
            }
            //int cba;
            //string normalc = (parse_ini.parselabel(Path.Combine(variables.pathforit, @"xeBuild\" + _dash + @"\_" + _ttype + ".ini"), _ctype.Ini + "bl")[0]);
            //string normalcb = normalc.Substring(normalc.IndexOf("_") + 1, normalc.IndexOf(".") - normalc.IndexOf("_") - 1);
            //if (int.TryParse(normalcb, out cba)) cbs.Add(cba);

            return cbs;
        }

        void copySMC()
        {
            if (_ttype == variables.hacktypes.jtag && !File.Exists(Path.Combine(variables.xePath, "SMC.bin")))
            {
                if (_ctype.ID == 2)
                {
                    if (_audclamp) File.Copy(variables.xePath + "SMCaud.bin", variables.xePath + "SMC.bin", true);
                    else File.Copy(variables.xePath + "SMCfzj.bin", variables.xePath + "SMC.bin", true);

                }
                else if (_ctype.ID == 3)
                {
                    if (_audclamp) File.Copy(variables.xePath + "SMCaud.bin", variables.xePath + "SMC.bin", true);
                    else File.Copy(variables.xePath + "SMCfzj.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 8)
                {
                    File.Copy(variables.xePath + "SMCx.bin", variables.xePath + "SMC.bin", true);
                }
                else
                {
                    if (_audclamp) File.Copy(variables.xePath + "SMCaud.bin", variables.xePath + "SMC.bin", true);
                    else File.Copy(variables.xePath + "SMCfzj.bin", variables.xePath + "SMC.bin", true);
                }

                if (_rjtag)
                {
                    File.WriteAllBytes(variables.xePath + "SMC.bin", Nand.Nand.patch_SMC((File.ReadAllBytes(variables.xePath + "SMC.bin"))));
                }
                variables.copiedSMC = true;
            }
            else if (_ttype != variables.hacktypes.jtag && _CleanSMC)
            {
                if (_ctype.ID == 1 || _ctype.ID == 12)
                {
                    File.Copy(variables.xePath + "TRINITY_CLEAN.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 2 || _ctype.ID == 9)
                {
                    File.Copy(variables.xePath + "FALCON_CLEAN.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 3)
                {
                    File.Copy(variables.xePath + "ZEPHYR_CLEAN.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 4 || _ctype.ID == 5 || _ctype.ID == 6 || _ctype.ID == 7)
                {
                    File.Copy(variables.xePath + "JASPER_CLEAN.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 8)
                {
                    File.Copy(variables.xePath + "XENON_CLEAN.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 10 || _ctype.ID == 11)
                {
                    File.Copy(variables.xePath + "CORONA_CLEAN.bin", variables.xePath + "SMC.bin", true);
                }
                variables.copiedSMC = true;
            }
            else if ((_ttype == variables.hacktypes.glitch2 || _ttype == variables.hacktypes.glitch2m) && _CR4)
            {
                if (_ctype.ID == 1 || _ctype.ID == 12)
                {
                    File.Copy(variables.xePath + "TRINITY_CR4.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 2 || _ctype.ID == 3 || _ctype.ID == 9)
                {
                    File.Copy(variables.xePath + "FALCON_CR4.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 4 || _ctype.ID == 5 || _ctype.ID == 6 || _ctype.ID == 7)
                {
                    File.Copy(variables.xePath + "JASPER_CR4.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 10 || _ctype.ID == 11)
                {
                    File.Copy(variables.xePath + "CORONA_CR4.bin", variables.xePath + "SMC.bin", true);
                }
                variables.copiedSMC = true;
            }
            else if ((_ttype == variables.hacktypes.glitch2 || _ttype == variables.hacktypes.glitch2m) && _SMCP)
            {
                if (_ctype.ID == 1 || _ctype.ID == 12)
                {
                    File.Copy(variables.xePath + "TRINITY_SMC+.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 2 || _ctype.ID == 3 || _ctype.ID == 9)
                {
                    File.Copy(variables.xePath + "FALCON_SMC+.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 4 || _ctype.ID == 5 || _ctype.ID == 6 || _ctype.ID == 7)
                {
                    File.Copy(variables.xePath + "JASPER_SMC+.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 10 || _ctype.ID == 11)
                {
                    File.Copy(variables.xePath + "CORONA_SMC+.bin", variables.xePath + "SMC.bin", true);
                }
                variables.copiedSMC = true;
            }
            else
            {
                variables.copiedSMC = false;
            }
        }
        void copySMCcustom()
        {
            if (_ttype == variables.hacktypes.jtag)
            {
                if (_ctype.ID == 2)
                {
                    if (_audclamp) File.Copy(variables.xePath + "SMCaud.bin", variables.xePath + "SMC.bin", true);
                    else File.Copy(variables.xePath + "SMCfzj.bin", variables.xePath + "SMC.bin", true);

                }
                else if (_ctype.ID == 3)
                {
                    if (_audclamp) File.Copy(variables.xePath + "SMCaud.bin", variables.xePath + "SMC.bin", true);
                    else File.Copy(variables.xePath + "SMCfzj.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 8)
                {
                    File.Copy(variables.xePath + "SMCx.bin", variables.xePath + "SMC.bin", true);
                }
                else
                {
                    if (_audclamp) File.Copy(variables.xePath + "SMCaud.bin", variables.xePath + "SMC.bin", true);
                    else File.Copy(variables.xePath + "SMCfzj.bin", variables.xePath + "SMC.bin", true);
                }

                if (_rjtag && _ctype.ID != 8)
                {
                    File.WriteAllBytes(variables.xePath + "SMC.bin", Nand.Nand.patch_SMC((File.ReadAllBytes(variables.xePath + "SMC.bin"))));
                }
                variables.copiedSMC = true;
            }
            else if (_ttype != variables.hacktypes.jtag && _CleanSMC)
            {
                if (_ctype.ID == 1 || _ctype.ID == 12)
                {
                    File.Copy(variables.xePath + "TRINITY_CLEAN.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 2 || _ctype.ID == 9)
                {
                    File.Copy(variables.xePath + "FALCON_CLEAN.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 3)
                {
                    File.Copy(variables.xePath + "ZEPHYR_CLEAN.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 4 || _ctype.ID == 5 || _ctype.ID == 6 || _ctype.ID == 7)
                {
                    File.Copy(variables.xePath + "JASPER_CLEAN.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 8)
                {
                    File.Copy(variables.xePath + "XENON_CLEAN.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 10 || _ctype.ID == 11)
                {
                    File.Copy(variables.xePath + "CORONA_CLEAN.bin", variables.xePath + "SMC.bin", true);
                }
                variables.copiedSMC = true;
            }
            else if ((_ttype == variables.hacktypes.glitch2 || _ttype == variables.hacktypes.glitch2m) && _CR4)
            {
                if (_ctype.ID == 1 || _ctype.ID == 12)
                {
                    File.Copy(variables.xePath + "TRINITY_CR4.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 2 || _ctype.ID == 3 || _ctype.ID == 9)
                {
                    File.Copy(variables.xePath + "FALCON_CR4.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 4 || _ctype.ID == 5 || _ctype.ID == 6 || _ctype.ID == 7)
                {
                    File.Copy(variables.xePath + "JASPER_CR4.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 10 || _ctype.ID == 11)
                {
                    File.Copy(variables.xePath + "CORONA_CR4.bin", variables.xePath + "SMC.bin", true);
                }
                variables.copiedSMC = true;
            }
            else if ((_ttype == variables.hacktypes.glitch2 || _ttype == variables.hacktypes.glitch2m) && _SMCP)
            {
                if (_ctype.ID == 1 || _ctype.ID == 12)
                {
                    File.Copy(variables.xePath + "TRINITY_SMC+.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 2 || _ctype.ID == 3 || _ctype.ID == 9)
                {
                    File.Copy(variables.xePath + "FALCON_SMC+.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 4 || _ctype.ID == 5 || _ctype.ID == 6 || _ctype.ID == 7)
                {
                    File.Copy(variables.xePath + "JASPER_SMC+.bin", variables.xePath + "SMC.bin", true);
                }
                else if (_ctype.ID == 10 || _ctype.ID == 11)
                {
                    File.Copy(variables.xePath + "CORONA_SMC+.bin", variables.xePath + "SMC.bin", true);
                }
                variables.copiedSMC = true;
            }
            else
            {
                variables.copiedSMC = false;
            }
        }

        private void copyXLUsb()
        {
            if (File.Exists(Path.Combine(variables.rootfolder, @"xeBuild\" + _dash + @"\xl_usb\xam.xex")))
            {
                if (File.Exists(Path.Combine(variables.rootfolder, @"xeBuild\" + _dash + @"\xam.xex")))
                {
                    File.Move(Path.Combine(variables.rootfolder, @"xeBuild\" + _dash + @"\xam.xex"), Path.Combine(variables.rootfolder, @"xeBuild\" + _dash + @"\xam.xex.tmp"));
                }

                File.Copy(Path.Combine(variables.rootfolder, @"xeBuild\" + _dash + @"\xl_usb\xam.xex"), Path.Combine(variables.rootfolder, @"xeBuild\" + _dash + @"\xam.xex"), true);

                string buildIni = Path.Combine(variables.rootfolder, @"xeBuild\" + _dash + @"\_" + variables.ttyp.ToString() + ".ini");
                string xlUsbIni = Path.Combine(variables.rootfolder, @"xeBuild\" + _dash + @"\xl_usb\_" + variables.ttyp.ToString() + ".ini");
                if (File.Exists(xlUsbIni))
                {
                    if (File.Exists(buildIni))
                    {
                        File.Move(buildIni, buildIni + ".tmp");
                    }

                    File.Copy(xlUsbIni, buildIni, true);
                }

                variables.copiedXLUsb = true;
            }
            else
            {
                variables.copiedXLUsb = false;
            }
        }

        void checkDashLaunch()
        {
            if (_DLpatches && _includeLaunch)
            {
                if (!File.Exists(Path.Combine(variables.launchpath, _dash + @"\launch.ini")))
                {
                    if (File.Exists(Path.Combine(variables.launchpath, "launch.ini")))
                        System.IO.File.Copy(Path.Combine(variables.launchpath, "launch.ini"), Path.Combine(variables.launchpath, _dash + @"\launch.ini"), true);
                    else if (File.Exists(Path.Combine(variables.launchpath, "launch_default.ini")))
                        System.IO.File.Copy(Path.Combine(variables.launchpath, @"launch_default.ini"), Path.Combine(variables.launchpath, _dash + @"\launch.ini"), true);
                }
            }
            edittheini();
        }
        void edittheini()
        {
            if (variables.debugme) Console.WriteLine(_dash);
            foreach (variables.hacktypes type in Enum.GetValues(typeof(variables.hacktypes)))
            {
                if (type == variables.hacktypes.retail || type == variables.hacktypes.nothing) continue;
                string file = Path.Combine(variables.rootfolder, @"xeBuild\" + _dash + @"\_" + type + ".ini");
                string[] writepatches = { @"..\launch.xex", @"..\lhelper.xex", @"..\launch.ini" };
                string[] writepatches2 = { @"..\launch.xex", @"..\lhelper.xex" };
                string[] empty = { };

                if (File.Exists(file))
                {
                    if (_DLpatches)
                    {
                        parse_ini.edit_ini(file, _includeLaunch ? writepatches : writepatches2, empty);
                    }
                    else
                    {
                        parse_ini.edit_ini(file, empty, writepatches);
                    }
                }
                else if (variables.debugme) Console.WriteLine("Couldn't add dashlaunch patches to {0}", file);
            }
        }
        void savekvinfo(string savefile)
        {
            try
            {
                if (!_nand.ok) return;
                TextWriter tw = new StreamWriter(savefile);
                tw.WriteLine("*******************************************");
                tw.WriteLine("*******************************************");
                string console_type = _ctype.Text;
                tw.WriteLine("Console Type: {0}", console_type);
                tw.WriteLine("");
                tw.WriteLine("Cpu Key: {0}", _cpukey);
                tw.WriteLine("");
                tw.WriteLine("KV Type: {0}", _nand.ki.kvtype.Replace("0", ""));
                tw.WriteLine("");
                tw.WriteLine("MFR Date: {0}", _nand.ki.mfdate);
                tw.WriteLine("");
                tw.WriteLine("Console ID: {0}", _nand.ki.consoleid);
                tw.WriteLine("");
                tw.WriteLine("Serial: {0}", _nand.ki.serial);
                tw.WriteLine("");
                string region = "";
                if (_nand.ki.region == "02FE") region = "PAL/EU";
                else if (_nand.ki.region == "00FF") region = "NTSC/US";
                else if (_nand.ki.region == "01FE") region = "NTSC/JAP";
                else if (_nand.ki.region == "01FF") region = "NTSC/JAP";
                else if (_nand.ki.region == "01FC") region = "NTSC/KOR";
                else if (_nand.ki.region == "0101") region = "NTSC/HK";
                else if (_nand.ki.region == "0201") region = "PAL/AUS";
                else if (_nand.ki.region == "7FFF") region = "DEVKIT";
                tw.WriteLine("Region: {0} | {1}", _nand.ki.region, region);
                tw.WriteLine("");
                tw.WriteLine("Osig: {0}", _nand.ki.osig);
                tw.WriteLine("");
                tw.WriteLine("DVD Key: {0}", _nand.ki.dvdkey);
                tw.WriteLine("");
                tw.WriteLine("*******************************************");
                tw.WriteLine("*******************************************");
                tw.Close();
                Console.WriteLine("KV Info saved to file");
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); Console.WriteLine("Failed"); Console.WriteLine(""); }
        }

        XebuildError doSomeChecks()
        {
            if (String.IsNullOrEmpty(_cpukey)) return XebuildError.nocpukey;

            if (_ctype.ID == -1) return XebuildError.noconsole;
            if (_dash == 0) return XebuildError.nodash;
            string ini = (variables.launchpath + @"\" + _dash + @"\_" + _ttype + ".ini");
            string ctypebtldr = _ctype.Ini + "bl";
            if (ctypebtldr == "xenonbl" || ctypebtldr == "zephyrbl") ctypebtldr = "falconbl";
            if (!File.Exists(ini)) return XebuildError.noinis;
            if (!parse_ini.getlabels(ini).Contains(ctypebtldr)) return XebuildError.nobootloaders;

            return XebuildError.none;
        }
        void moveXell()
        {
            if (_ttype != variables.hacktypes.retail)
            {
                File.Copy(Path.Combine(variables.rootfolder, @"common\xell\xell-2f.bin"), Path.Combine(variables.rootfolder, @"xebuild\data\xell-2f.bin"), true);
                File.Copy(Path.Combine(variables.rootfolder, @"common\xell\xell-gggggg.bin"), Path.Combine(variables.rootfolder, @"xebuild\data\xell-gggggg.bin"), true);
            }
        }
        void moveOptions()
        {
            if (_altoptions)
            {
                Console.WriteLine("Using edited settings");
                File.Copy(Path.Combine(variables.rootfolder, @"xebuild\options_edited.ini"), Path.Combine(variables.rootfolder, @"xebuild\data\options.ini"), true);
            }
            else
            {
                File.Copy(Path.Combine(variables.rootfolder, @"xebuild\options.ini"), Path.Combine(variables.rootfolder, @"xebuild\data\options.ini"), true);
            }
        }

        public XebuildError createxebuild(bool custom)
        {
            XebuildError result = XebuildError.none;
            result = doSomeChecks();
            if (result != XebuildError.none) return result;
            moveXell();
            moveOptions();
            if (variables.changeldv != 0)
            {
                string cfldv = "cfldv=" + variables.highldv.ToString();
                string[] edit = { cfldv };
                string[] delete = { };
                parse_ini.edit_ini(Path.Combine(variables.rootfolder, @"xeBuild\data\options.ini"), edit, delete);
            }

            Console.WriteLine("XeBuild Initialized");
            if (!custom) copySMC();
            else copySMCcustom();
            if (_xlusb) copyXLUsb();
            variables.fullDataClean = _fullDataClean;

            checkDashLaunch();

            if (variables.changeldv != 0)
            {
                string cfldv = "cfldv=" + variables.highldv.ToString();
                string[] edit = { cfldv };
                string[] delete = { };
                parse_ini.edit_ini(Path.Combine(variables.rootfolder, @"xeBuild\data\options.ini"), edit, delete);
            }

            Console.WriteLine("Kernel Selected: {0}", _dash);


            variables.xefolder = Path.Combine(Directory.GetParent(variables.outfolder).FullName, _nand.ki.serial);
            if (variables.debugme) Console.WriteLine("outfolder: {0}", variables.xefolder);
            if (!Directory.Exists(variables.xefolder)) Directory.CreateDirectory(variables.xefolder);
            File.WriteAllText(System.IO.Path.Combine(variables.xefolder, variables.cpukeypath), _cpukey);
            savekvinfo(Path.Combine(variables.xefolder, "KV_Info.txt"));
            if (variables.changeldv != 0) variables.changeldv = 2;

            return result;
        }

        public void build()
        {
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = variables.rootfolder + @"\xeBuild\xeBuild.exe";
            string arguments = "";
            string boardtype = _ctype.XeBuild;
            arguments = "-t " + _ttype;

            if (_ttype == variables.hacktypes.glitch2 || _ttype == variables.hacktypes.glitch2m)
            {
                if (boardtype == "xenon")
                {
                    boardtype = "falcon";
                    Console.WriteLine("Using Falcon type for Xenon");
                }
                else if (boardtype == "zephyr")
                {
                    boardtype = "falcon";
                    Console.WriteLine("Using Falcon type for Zephyr");
                }
            }

            if (_xdkbuild)
            {
                if (boardtype == "jasper256" || boardtype == "jasper512" || boardtype == "trinitybb") // requires bigffs
                {
                    arguments += " -c " + boardtype.Substring(0, 6) + "bigffs -i flash";
                }
                else if (boardtype == "corona4g")
                {
                    arguments += " -c " + boardtype + " -i flash";
                }
                else // no bigffs!
                {
                    arguments += " -c " + boardtype;
                }
            }
            else if (_bigffs)
            {
                if (boardtype == "jasper256" || boardtype == "jasper512" || boardtype == "trinitybb")
                {
                    arguments += " -c " + boardtype.Substring(0, 6) + "bigffs";
                }
                else
                {
                    arguments += " -c " + boardtype + "bigffs";
                }
            }
            else
            {
                arguments += " -c " + boardtype;
            }

            if (_zfuse && _ttype == variables.hacktypes.devgl)
            {
                arguments += " -a hvfixkeys";
            }

            if (_xlusb) arguments += " -a xl_usb";

            foreach (String patch in _patches)
            {
                arguments += " " + patch;
            }
            if (variables.debugme) arguments += " -v";
            arguments += " -noenter";
            arguments += " -f " + _dash;
            arguments += " -d data";
            arguments += " \"" + variables.xefolder + "\\" + variables.nandflash + "\" ";

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            arguments = regex.Replace(arguments, @" ");

            if (variables.debugme) Console.WriteLine(variables.rootfolder);
            if (variables.debugme) Console.WriteLine("---" + variables.rootfolder + @"\xeBuild\xeBuild.exe");
            if (variables.debugme) Console.WriteLine(arguments);
            pProcess.StartInfo.Arguments = arguments;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.WorkingDirectory = variables.rootfolder;
            pProcess.StartInfo.RedirectStandardInput = true;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            if (!_xdkbuild && !_rgh3) pProcess.Exited += new EventHandler(xeExit);
            //pProcess.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(DataReceived);
            //pProcess.Exited += new EventHandler(xe_Exited);
            //pProcess.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(process_OutputDataReceived);
            try
            {
                AutoResetEvent outputWaitHandle = new AutoResetEvent(false);
                pProcess.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        Console.WriteLine(e.Data);
                        if (e.Data != null && e.Data.Contains("image built") && !_xdkbuild && !_rgh3) { variables.xefinished = true; }
                    }
                };
                pProcess.Start();
                pProcess.BeginOutputReadLine();
                pProcess.StandardInput.WriteLine("enter");
                pProcess.WaitForExit();

                if (pProcess.HasExited)
                {
                    pProcess.CancelOutputRead();
                }
            }
            catch (Exception objException)
            {
                Console.WriteLine(objException.Message);
            }

            if (_xdkbuild && _rgh3)
            {
                MainForm.mainForm.XDKbuild.create(boardtype, true);
                MainForm.mainForm.rgh3Build.create(_ctype.Text, "00000000000000000000000000000000");
            }
            else if (_xdkbuild) MainForm.mainForm.XDKbuild.create(boardtype);
            else if (_rgh3) MainForm.mainForm.rgh3Build.create(_ctype.Text, _cpukey);
        }

        public void build(string arguments)
        {
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = variables.rootfolder + @"\xeBuild\xeBuild.exe";
            string boardtype = _ctype.XeBuild;

            if (_ttype == variables.hacktypes.glitch2 || _ttype == variables.hacktypes.glitch2m)
            {
                if (boardtype == "xenon")
                {
                    boardtype = "falcon";
                    Console.WriteLine("Using Falcon type for Xenon");
                }
                else if (boardtype == "zephyr")
                {
                    boardtype = "falcon";
                    Console.WriteLine("Using Falcon type for Zephyr");
                }
            }

            if (_xdkbuild)
            {
                if (boardtype == "jasper256" || boardtype == "jasper512" || boardtype == "trinitybb") // requires bigffs
                {
                    arguments += " -c " + boardtype.Substring(0, 6) + "bigffs -i flash";
                }
                else if (boardtype == "corona4g")
                {
                    arguments += " -c " + boardtype + " -i flash";
                }
                else // no bigffs!
                {
                    arguments += " -c " + boardtype;
                }
            }
            else if (_bigffs)
            {
                if (boardtype == "jasper256" || boardtype == "jasper512" || boardtype == "trinitybb")
                {
                    arguments += " -c " + boardtype.Substring(0, 6) + "bigffs";
                }
                else
                {
                    arguments += " -c " + boardtype + "bigffs";
                }
            }
            else
            {
                arguments += " -c " + boardtype;
            }

            if (_zfuse && _ttype == variables.hacktypes.devgl)
            {
                arguments += " -a hvfixkeys";
            }

            if (_xlusb) arguments += " -a xl_usb";

            if (variables.debugme) Console.WriteLine(variables.rootfolder);
            if (variables.debugme) Console.WriteLine("---" + variables.rootfolder + @"\xeBuild\xeBuild.exe");
            if (variables.debugme) Console.WriteLine(arguments);
            pProcess.StartInfo.Arguments = arguments;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.WorkingDirectory = variables.rootfolder;
            pProcess.StartInfo.RedirectStandardInput = true;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            if (!_xdkbuild && !_rgh3) pProcess.Exited += new EventHandler(xeExit);
            //pProcess.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(DataReceived);
            //pProcess.Exited += new EventHandler(xe_Exited);
            //pProcess.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(process_OutputDataReceived);
            try
            {
                AutoResetEvent outputWaitHandle = new AutoResetEvent(false);
                pProcess.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        Console.WriteLine(e.Data);
                        if (e.Data != null && e.Data.Contains("image built") && !_xdkbuild && !_rgh3) { variables.xefinished = true; }
                    }
                };
                pProcess.Start();
                pProcess.BeginOutputReadLine();
                pProcess.StandardInput.WriteLine("enter");
                pProcess.WaitForExit();

                if (pProcess.HasExited)
                {
                    pProcess.CancelOutputRead();
                }

                if (_xdkbuild) MainForm.mainForm.XDKbuild.create(boardtype);
                else if (_rgh3) MainForm.mainForm.rgh3Build.create(_ctype.Text, _cpukey);
            }
            catch (Exception objException)
            {
                Console.WriteLine(objException.Message);
            }


        }


        ////////////////////////////////////////////////


        public void Uloadvariables(int dash, variables.hacktypes ttype, List<String> patches, bool altoptions, bool nowrite, bool noava, bool clean, bool noreeb, bool DLpatches, bool includeLaunch)
        {
            this._dash = dash;
            this._ttype = ttype;
            this._patches = patches;
            this._nowrite = nowrite;
            this._noava = noava;
            this._clean = clean;
            this._altoptions = altoptions;
            this._noreeb = noreeb;
            this._DLpatches = DLpatches;
            this._includeLaunch = includeLaunch;
        }

        public XebuildError createxebuild()
        {
            XebuildError result = XebuildError.none;
            if (_dash == 0) result = XebuildError.nodash;
            if (result != XebuildError.none) return result;
            //File.Copy(Path.Combine(variables.pathforit, @"common\xell\xell-2f.bin"), Path.Combine(variables.pathforit, @"xeBuild\xell-2f.bin"), true);
            //File.Copy(Path.Combine(variables.pathforit, @"common\xell\xell-gggggg.bin"), Path.Combine(variables.pathforit, @"xeBuild\xell-gggggg.bin"), true);
            moveOptions();

            Console.WriteLine("Load Files Initiliazation Finished");
            checkDashLaunch();

            Console.WriteLine("Started Updating Console to {0}", _dash);
            variables.xefolder = variables.outfolder;

            return result;
        }

        public void update()
        {
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = variables.rootfolder + @"\xeBuild\xeBuild.exe";
            string arguments = "update ";
            foreach (String patch in _patches)
            {
                arguments += " " + patch;
            }
            if (variables.debugme) arguments += " -v";
            if (_noava) arguments += " -noava";
            if (_nowrite) arguments += " -nowrite";
            if (_clean) arguments += " -clean";
            if (_noreeb) arguments += " -noreeb";
            arguments += " -noenter";
            arguments += " -f " + _dash;
            arguments += " -d ";
            arguments += "\"" + variables.outfolder + "\"";

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            arguments = regex.Replace(arguments, @" ");

            if (variables.debugme) Console.WriteLine(variables.rootfolder);
            if (variables.debugme) Console.WriteLine("---" + variables.rootfolder + @"\xeBuild\xeBuild.exe");
            if (variables.debugme) Console.WriteLine(arguments);
            pProcess.StartInfo.Arguments = arguments;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.WorkingDirectory = variables.rootfolder;
            pProcess.StartInfo.RedirectStandardInput = true;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Exited += new EventHandler(xeExit);
            try
            {
                AutoResetEvent outputWaitHandle = new AutoResetEvent(false);
                pProcess.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        Console.WriteLine(e.Data);
                        if (e.Data != null && e.Data.Contains("image built")) { variables.xefinished = true; }
                    }
                };
                pProcess.Start();
                pProcess.BeginOutputReadLine();
                pProcess.WaitForExit();

                if (pProcess.HasExited)
                {
                    pProcess.CancelOutputRead();
                }
            }
            catch (Exception objException)
            {
                Console.WriteLine(objException.Message);
            }
        }

        public void client(string args)
        {
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = variables.rootfolder + @"\xeBuild\xeBuild.exe";
            string arguments = "client ";
            arguments += args;
            if (variables.debugme) arguments += " -v";
            arguments += " -noenter";

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            arguments = regex.Replace(arguments, @" ");

            if (variables.debugme) Console.WriteLine(variables.rootfolder);
            if (variables.debugme) Console.WriteLine("---" + variables.rootfolder + @"\xeBuild\xeBuild.exe");
            if (variables.debugme) Console.WriteLine(arguments);
            pProcess.StartInfo.Arguments = arguments;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.WorkingDirectory = variables.rootfolder;
            pProcess.StartInfo.RedirectStandardInput = true;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            //pProcess.Exited += new EventHandler(xeExit);
            try
            {
                AutoResetEvent outputWaitHandle = new AutoResetEvent(false);
                pProcess.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        Console.WriteLine(e.Data);
                    }
                };
                pProcess.Start();
                pProcess.BeginOutputReadLine();
                pProcess.WaitForExit();

                if (pProcess.HasExited)
                {
                    pProcess.CancelOutputRead();
                }
            }
            catch (Exception objException)
            {
                Console.WriteLine(objException.Message);
            }


        }

        public delegate void xeExited(object sender, EventArgs e);
        public event xeExited xeExit;


    }
}
