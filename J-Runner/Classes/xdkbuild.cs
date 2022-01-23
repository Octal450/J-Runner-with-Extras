using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace JRunner
{
    public class xdkbuild
    {
        private string filename;
        private string sc;
        private string key1bl_options = "";

        public void create(string board)
        {
            Console.WriteLine("Converting Image to XDKbuild...");
            Thread.Sleep(1000); // Important

            try
            {
                key1bl_options = File.ReadAllText(variables.pathforit + @"\xeBuild\options.ini");
                key1bl_options = key1bl_options.Substring(key1bl_options.IndexOf("1blkey = ") + 9, 32);
            }
            catch
            {
                Console.WriteLine("XDKbuild Failed: Invalid 1BL key");
                return;
            }

            filename = Path.Combine(variables.xefolder, variables.nandflash);

            if (board == "jasper256") board = "jasper";
            if (board == "jasper512") board = "jasper";
            if (board == "corona4g") board = "corona";

            sc = board + ".bin";

            Process pProcess = new Process();
            pProcess.StartInfo.FileName = variables.pathforit + @"\xeBuild\XDKbuild\XDKbuild.exe";
            pProcess.StartInfo.Arguments = "\"" + filename + "\" " + key1bl_options + " " + sc;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.WorkingDirectory = variables.pathforit + @"\xeBuild\XDKbuild\";
            pProcess.StartInfo.RedirectStandardInput = true;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;

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
                        if (e.Data != null && e.Data.Contains("Virtual Fuses Set To:")) { variables.xefinished = true; }
                    }
                };
                pProcess.Start();
                pProcess.BeginOutputReadLine();
                pProcess.StandardInput.WriteLine("enter");
                pProcess.WaitForExit();

                if (pProcess.HasExited)
                {
                    pProcess.CancelOutputRead();
                    Console.WriteLine("XDKbuild Conversion Finished!");
                    Console.WriteLine("Remember to program a compatible timing file!");
                    MainForm.mainForm.xPanel.xeExitActual();
                }
            }
            catch (Exception objException)
            {
                Console.WriteLine(objException.Message);
            }
        }
    }
}
