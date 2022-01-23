using System;
using System.IO;
using System.Threading;

namespace JRunner
{
    public class rgh3build
    {
        private string filename;

        public void create(string board, string cpuKey)
        {
            Console.WriteLine("Converting Image to RGH3...");
            Thread.Sleep(1000); // Important

            string ecc;
            string mhz = "";
            if (MainForm.mainForm.xPanel.getRgh3Mhz() == 10) mhz = "_10";

            if (board == "Corona 16MB") ecc = variables.RGH3_corona;
            else if (board == "Corona 4GB") ecc = variables.RGH3_corona4GB;
            else if (board == "Trinity") ecc = variables.RGH3_trinity;
            else if (board == "Jasper 16MB" || board == "Jasper SB") ecc = variables.RGH3_jasper + mhz;
            else if (board == "Jasper 256MB" || board == "Jasper 512MB") ecc = variables.RGH3_jasperBB + mhz;
            else if (board == "Falcon") ecc = variables.RGH3_falcon + mhz;
            else
            {
                Console.WriteLine("RGH3 Failed: Unsupported Console Type");
                variables.xefinished = true;
                MainForm.mainForm.xPanel.xeExitActual();
                return;
            }

            filename = Path.Combine(variables.xefolder, variables.nandflash);

            Classes.RGH2to3.ConvertRgh2ToRgh3(Path.Combine(variables.pathforit, "common", "ECC", ecc + ".ecc"), filename, cpuKey, filename);

            Console.WriteLine("RGH3 Conversion Finished!");

            variables.xefinished = true;
            MainForm.mainForm.xPanel.xeExitActual();
        }
    }
}
