using System;
using System.IO;
using System.Threading;

namespace JRunner
{
    public class rgh3build
    {
        private string filename;

        public void create(string board, string cpuKey, bool sequenced = false)
        {
            Console.WriteLine("Converting Image to RGH3...");
            Thread.Sleep(1000); // Important

            string ecc;
            string mhz = "";
            if (sequenced)
            {
                if (MainForm.mainForm.xPanel.getRgh3Mhz() == 10) mhz = "_10";
            }

            if (board == "Corona 16MB") ecc = variables.RGH3_corona;
            else if (board == "Corona 4GB") ecc = variables.RGH3_corona4GB;
            else if (board == "Trinity 16MB") ecc = variables.RGH3_trinity;
            else if (board == "Jasper 16MB" || board == "Jasper SB") ecc = variables.RGH3_jasper + mhz;
            else if (board == "Jasper BB") ecc = variables.RGH3_jasperBB + mhz;
            else if (board == "Falcon") ecc = variables.RGH3_falcon + mhz;
            else
            {
                Console.WriteLine("RGH3 Failed: Unsupported Console Type");
                if (sequenced)
                {
                    variables.xefinished = true;
                    MainForm.mainForm.xPanel.xeExitActual();
                }
                return;
            }

            if (sequenced) filename = Path.Combine(variables.xefolder, variables.updflash);
            else filename = variables.filename1;

            try
            {
                Classes.RGH2to3.ConvertRgh2ToRgh3(Path.Combine(variables.rootfolder, "common", "ECC", ecc + ".ecc"), filename, cpuKey, filename);
            }
            catch (Exception ex)
            {
                if (variables.debugMode) Console.WriteLine(ex.ToString());
                Console.WriteLine("Failed: The image is either already RGH3, or an unsupported image type");
                Console.WriteLine("");
                return;
            }

            Console.WriteLine("RGH3 Conversion Finished!");

            if (sequenced)
            {
                variables.xefinished = true;
                MainForm.mainForm.xPanel.xeExitActual();
            }
            else
            {
                MainForm.mainForm.nand_init();
            }
        }
    }
}
