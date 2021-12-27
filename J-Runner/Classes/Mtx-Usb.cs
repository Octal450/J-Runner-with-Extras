using System;
using System.IO;
using System.Windows.Forms;

namespace JRunner
{
    public class Mtx_Usb
    {
        // Based on xFlasher system, added due to IoTimedOut issue
        public void writeXeLLAuto()
        {
            if (String.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;

            if (Path.GetExtension(variables.filename1) == ".ecc")
            {
                Console.WriteLine("You need an .bin image");
                return;
            }

            writeNand(16, variables.filename1, 2);
        }

        public void writeEccAuto()
        {
            if (String.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;

            if (Path.GetExtension(variables.filename1) != ".ecc")
            {
                Console.WriteLine("You need an .ecc image");
                return;
            }

            writeNand(16, variables.filename1, 1);
        }

        public void writeNandAuto()
        {
            if (String.IsNullOrWhiteSpace(variables.filename1)) return;
            if (!File.Exists(variables.filename1)) return;

            if (Path.GetExtension(variables.filename1) == ".ecc")
            {
                Console.WriteLine("You need an .bin image");
                return;
            }

            double len = new FileInfo(variables.filename1).Length;
            if (len == 50331648)
            {
                MessageBox.Show("Unable to write eMMC type image with SPI tool\n\nPlease use an eMMC tool", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (len == 553648128)
            {
                variables.nandsizex = Nandsize.S512;
                writeNand(512, variables.filename1);
            }
            else if (len == 276824064)
            {
                variables.nandsizex = Nandsize.S256;
                writeNand(256, variables.filename1);
            }
            else if (len == 69206016)
            {
                variables.nandsizex = Nandsize.S64;
                writeNand(64, variables.filename1);
            }
            else if (len == 17301504)
            {
                variables.nandsizex = Nandsize.S16;
                writeNand(16, variables.filename1);
            }
            else if (len == 1351680)
            {
                variables.nandsizex = Nandsize.S16;
                writeNand(16, variables.filename1, 3);
            }
            else
            {
                MessageBox.Show("Nand is not a valid size", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void writeNand(int size, string filename, int mode = 0, int startblock = 0, int length = 0)
        {

        }
    }
}
