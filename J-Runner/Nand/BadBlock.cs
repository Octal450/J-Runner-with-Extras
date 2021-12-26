using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JRunner.Nand
{
    class BadBlock
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <param name="bigblock"></param>
        /// <returns></returns>
        /* 
         * if (layout == IMAGE_LAYOUT_0) {
            self->marker_ofs = 517;
            self->id_ofs = 512;
        } else if (layout == IMAGE_LAYOUT_1) {
            self->marker_ofs = 517;
            self->id_ofs = 513;
        } else {
            self->marker_ofs = 512;
            self->id_ofs = 513;
        }
        IMAGE_LAYOUT_0: xenon, zephyr, falcon
        IMAGE_LAYOUT_1: jasper 16, trinity
        IMAGE_LAYOUT_2: jasper 256/512
        */
        public static bool checkifbadblock(byte[] block, int blocknumber, bool bigblock = false, bool stealth = false)
        {
            if (block.Length != 0x4200 && block.Length != 0x21000)
            {
                if (variables.debugme) Console.WriteLine("Wrong Size: 0x{0:X}", block.Length);
                return false;
            }

            int blocksize;
            if (!bigblock) blocksize = 0x4200;
            else blocksize = 0x21000;

            int markeroffset = !bigblock ? 0x205 : 0x200;

            int counter = 0;
            bool alreadyfound = false;
            bool flag = false;
            for (counter = 0; counter + 496 <= blocksize; counter += 0x210)
            {

                if (Oper.allsame(Oper.returnportion(block, counter + 0x200, 0x10), 0x00) || (block[counter + markeroffset] != 0xFF && ((!flag && bigblock) || !bigblock)))
                {
                    //if ((stealth && variables.debugme) || !stealth) Console.WriteLine("Bad Block ID @ 0x{0:X4} [Offset: 0x{1:X}]", blocknumber, blocknumber * blocksize);
                    alreadyfound = true;
                    break;
                }
                flag = true;
            }
            if (alreadyfound) return true;
            else return false;
        }

        public static List<int> checkifremapped(byte[] reserved, List<int> badblocks, bool bigblock = false, bool stealth = false)
        {
            if (badblocks.Count == 0) return new List<int>();

            List<int> remappedlist = new List<int>();
            for (int i = 0; i < badblocks.Count; i++) remappedlist.Add(-1);
            bool found = false;
            int left = 0, blocksize, reserveblockpos, markeroffset, idoffset;

            if (bigblock)
            {
                blocksize = 0x21000;
                markeroffset = 0x200;
                idoffset = 0x201;
                //reserveblockpos = 0x1FF;
                reserveblockpos = 0x200;
            }
            else
            {
                //reserveblockpos = 0x3FF;
                reserveblockpos = 0x400;
                blocksize = 0x4200;
                markeroffset = 0x205;
                idoffset = 0x200;
            }

            int reservestartpos = reserveblockpos - 0x20;
            //if (variables.debugme) Console.WriteLine("-C- Blocksize {0:X} - ReserveBlockPos {1:X} - ReserveStartPos {2:X} ", blocksize, reserveblockpos, reservestartpos);
            try
            {
                for (int block = 0x1F; block != 0x0; block--)
                {
                    if (left == badblocks.Count) break;
                    //if (variables.debugme) Console.WriteLine("-C- Block {0:X}", block);

                    if (!Oper.allsame(Oper.returnportion(reserved, (blocksize * (block + 1)) - 0x10, 0x10), 0xFF) || !Oper.allsame(Oper.returnportion(reserved, (blocksize * block) + 0x200, 0x10), 0xFF))
                    {
                        string blockid = Oper.ByteArrayToString(Oper.returnportion(reserved, (blocksize * (block + 1)) - (0x210 - idoffset), 0x4).Reverse().ToArray());
                        string blockid2 = Oper.ByteArrayToString(Oper.returnportion(reserved, (blocksize * (block)) + idoffset, 0x4).Reverse().ToArray());

                        if (blockid.Substring(6, 2) == "00") blockid = blockid.Remove(6).Insert(0, "00");
                        if (blockid2.Substring(6, 2) == "00") blockid2 = blockid2.Remove(6).Insert(0, "00");

                        //if (variables.debugme) Console.WriteLine("1 {0}", blockid);
                        //if (variables.debugme) Console.WriteLine("2 {0}", blockid2);

                        if (reserved.Length > ((blocksize * (block + 1)) - (0x210 - markeroffset)))
                        {
                            //if (variables.debugme) Console.WriteLine("{0:X} - {1:X}", (blocksize * (block + 1)) - (0x210 - markeroffset), (blocksize * block) + markeroffset);
                            if ((reserved[(blocksize * (block + 1)) - (0x210 - markeroffset)] != 0xFF) &&
                            (reserved[(blocksize * block) + markeroffset] != 0xFF)) continue;
                        }

                        int index = 0;
                        foreach (int bblocks in badblocks)
                        {
                            if (bblocks == -1) continue;
                            if (remappedlist[index] != -1)
                            {
                                index++;
                                continue;
                            }


                            //if (variables.debugme) Console.WriteLine("B {0:X}", (bblocks).ToString("X8"));
                            if (blockid.Contains((bblocks).ToString("X8")) || blockid2.Contains((bblocks).ToString("X8")))
                            {
                                found = true;
                                remappedlist[index] = block;
                                if ((stealth && variables.debugme) || !stealth) Console.WriteLine("Bad Block ID  {0:X4} Found @ 0x{1:X4} [Offset: 0x{2:X}]", badblocks[index], reservestartpos + block, blocksize * (reservestartpos + block));
                                left++;
                                break;
                            }

                            index++;
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message.ToString()); if (variables.debugme) Console.WriteLine(ex.ToString()); }

            if (!found)
            {
                if ((variables.debugme)) Console.WriteLine("Can't fix it. Remapped Blocks don't exist.");
                return remappedlist;
            }

            return remappedlist;
        }

        public static byte[] remapbadblocks(byte[] image, List<int> badblocks, List<int> remappedblocks, bool bigblock = false, bool stealth = false)
        {
            if (badblocks.Count == 0 || remappedblocks.Count == 0) return image;

            int blocksize;
            if (!bigblock) blocksize = 0x4200;
            else blocksize = 0x21000;

            int reserveblockpos;
            if (blocksize == 0x4200) reserveblockpos = 0x3FF;
            else reserveblockpos = 0x1FF;

            int left = 0;
            int reservestartpos = reserveblockpos - 0x20;
            while (left != badblocks.Count)
            {
                if (remappedblocks[left] != -1)
                {
                    byte[] block = Oper.returnportion(image, blocksize * (remappedblocks[left] + reservestartpos + 1), blocksize);
                    //if (variables.debugme) Console.WriteLine("Block {0:X} is performing operations", badblocks[left]);
                    //if (variables.debugme) Console.WriteLine("{0:X}", remappedblocks[left] + reservestartpos + 1);
                    for (int i = 0; i < blocksize; i++)
                    {
                        image[(badblocks[left]) * blocksize + i] = block[i];
                        image[(remappedblocks[left] + reservestartpos + 1) * blocksize + i] = 0xFF;
                    }
                    //if ((variables.debugme)) Console.WriteLine("");
                    if ((stealth && variables.debugme) || !stealth) Console.WriteLine("Block ID {0:X4} [Offset: 0x{1:X}] remapped to Block ID {2:X4} [Offset: 0x{3:X}]", remappedblocks[left] + reservestartpos + 1, blocksize * (remappedblocks[left] + reservestartpos + 1), badblocks[left], badblocks[left] * blocksize);
                }
                left++;
            }
            //if ((variables.debugme)) Console.WriteLine("");
            if ((stealth && variables.debugme) || !stealth) Console.WriteLine("Bad Blocks Remapped");
            return image;
        }

        public static byte[] remapbadblocks(byte[] image, byte[] reserved, List<int> badblocks, List<int> remappedblocks, bool bigblock = false, bool stealth = false)
        {
            if (badblocks.Count == 0 || remappedblocks.Count == 0) return image;

            int blocksize;
            if (!bigblock) blocksize = 0x4200;
            else blocksize = 0x21000;

            int reserveblockpos;
            if (blocksize == 0x4200) reserveblockpos = 0x3FF;
            else reserveblockpos = 0x1FF;

            int left = 0;
            int reservestartpos = reserveblockpos - 0x20;
            while (left != badblocks.Count)
            {
                if (remappedblocks[left] != -1)
                {
                    byte[] block = new byte[blocksize];
                    //if (variables.debugme) Console.WriteLine("roffset: {0:X}", blocksize * (remappedblocks[left]));
                    Buffer.BlockCopy(reserved, blocksize * (remappedblocks[left]), block, 0, blocksize);
                    // if (variables.debugme) Console.WriteLine("Block {0:X} is performing operations", badblocks[left]);
                    //if (variables.debugme) Console.WriteLine("{0:X}", remappedblocks[left] + reservestartpos + 1);
                    for (int i = 0; i < blocksize; i++)
                    {
                        image[(badblocks[left]) * blocksize + i] = block[i];
                    }
                    //if ((variables.debugme)) Console.WriteLine("");
                    if ((stealth && variables.debugme) || !stealth) Console.WriteLine("Block ID {0:X4} [Offset: 0x{1:X}] remapped to Block ID {2:X4} [Offset: 0x{3:X}]", remappedblocks[left] + reservestartpos + 1, blocksize * (remappedblocks[left] + reservestartpos + 1), badblocks[left], badblocks[left] * blocksize);
                }
                left++;
            }
            //if ((variables.debugme)) Console.WriteLine("");
            if ((stealth && variables.debugme) || !stealth) Console.WriteLine("Bad Blocks Remapped");
            return image;
        }

        public static string find_bad_blocks(string filename, bool stealth)
        {

            if ((stealth && variables.debugme) || !stealth) Console.WriteLine("");
            long imgsize = 0;
            FileInfo file = new FileInfo(filename);
            byte[] image;
            int blocksize, reservedoffset;
            bool bigblock;

            if (file.Length >= 0x4200000)
            {
                image = Oper.openfile(filename, ref imgsize, 0x4200000);
                blocksize = 0x21000;
                bigblock = true;
                reservedoffset = 0x1E0;
            }
            else
            {
                image = Oper.openfile(filename, ref imgsize, 0);
                blocksize = 0x4200;
                bigblock = false;
                reservedoffset = 0x3E0;
            }

            if (image[0x205] == 0xFF || image[0x415] == 0xFF || image[0x200] == 0xFF)
            { }
            else
            {
                Console.WriteLine("Can't check for bad blocks. No Spare data. Possibly Corona 4GB");
                return filename;
            }

            List<int> badblocks = new List<int>();
            List<int> remappedblocks = new List<int>();


            if (variables.debugme) Console.WriteLine("-F-Image Size: 0x{0:X} | imagesize: 0x{1:X} | File Size: 0x{0:X}", image.Length, blocksize, file.Length);

            int counter;
            for (counter = 0; counter < image.Length / blocksize; counter++)
            {
                if (checkifbadblock(Oper.returnportion(image, counter * blocksize, blocksize), counter, bigblock))
                {
                    badblocks.Add(counter);
                }
                if (badblocks.Count >= 0x20)
                {
                    Console.WriteLine("Too Many Bad Blocks");
                    return filename;
                }
            }

            if ((stealth && variables.debugme) || !stealth) Console.WriteLine("");

            if (badblocks.Count == 0)
            {
                if ((stealth && variables.debugme) || !stealth) if ((stealth && variables.debugme) || !stealth) Console.WriteLine("Bad Blocks Don't Exist");
                return filename;
            }
            //find if remapped

            remappedblocks = checkifremapped(Oper.returnportion(image, reservedoffset * blocksize, 0x20 * blocksize), badblocks, bigblock);


            bool found = false;
            foreach (int blockoffset in remappedblocks)
            {
                if (blockoffset != -1)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                if ((stealth && variables.debugme) || !stealth) Console.WriteLine("Can't fix it. Remapped Blocks don't exist.");
                return filename;
            }

            if (!stealth) if (MessageBox.Show("Bad Blocks have been found.\nRemap?", "Remap", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                == DialogResult.No) return filename;

            image = remapbadblocks(image, badblocks, remappedblocks, bigblock);

            string filename1 = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename) + "_nobadblocks.bin");
            if (variables.debugme) Console.WriteLine(filename1);
            Oper.savefile(image, filename1);
            if ((stealth && variables.debugme) || !stealth) Console.WriteLine("");
            if ((stealth && variables.debugme) || !stealth) Console.WriteLine("Fixed Image saved to {0}", filename1);
            return filename1;
        }

        public static byte[] find_bad_blocks_b(string filename, bool stealth)
        {
            if ((stealth && variables.debugme) || !stealth) Console.WriteLine("");
            long imgsize = 0;
            FileInfo file = new FileInfo(filename);
            byte[] image;
            int blocksize, reservedoffset;
            bool bigblock;

            if (file.Length >= 0x4200000)
            {
                image = Oper.openfile(filename, ref imgsize, 0x4200000);
                blocksize = 0x21000;
                bigblock = true;
                reservedoffset = 0x1E0;
            }
            else
            {
                image = Oper.openfile(filename, ref imgsize, 0);
                blocksize = 0x4200;
                bigblock = false;
                reservedoffset = 0x3E0;
            }

            if (!Nand.hasecc_v2(ref image))
            {
                Console.WriteLine("Can't check for bad blocks. No Spare data. Possibly Corona 4GB");
                return image;
            }

            List<int> badblocks = new List<int>();
            List<int> remappedblocks = new List<int>();


            if (variables.debugme) Console.WriteLine("-B-Image Size: 0x{0:X} | imagesize: 0x{1:X} | File Size: 0x{0:X}", image.Length, blocksize, file.Length);

            int counter;
            for (counter = 0; counter < image.Length / blocksize; counter++)
            {
                if (checkifbadblock(Oper.returnportion(image, counter * blocksize, blocksize), counter, bigblock))
                {
                    badblocks.Add(counter);
                }
                if (badblocks.Count >= 0x20)
                {
                    Console.WriteLine("Too Many Bad Blocks");
                    return image;
                }
            }

            if ((stealth && variables.debugme) || !stealth) Console.WriteLine("");

            if (badblocks.Count == 0)
            {
                if ((stealth && variables.debugme) || !stealth) if ((stealth && variables.debugme) || !stealth) Console.WriteLine("Bad Blocks Don't Exist");
                return image;
            }
            //find if remapped

            remappedblocks = checkifremapped(Oper.returnportion(image, reservedoffset * blocksize, 0x20 * blocksize), badblocks, bigblock);


            bool found = false;
            foreach (int blockoffset in remappedblocks)
            {
                if (blockoffset != -1)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                if ((stealth && variables.debugme) || !stealth) Console.WriteLine("Can't fix it. Remapped Blocks don't exist.");
                return image;
            }
            if (badblocks.Count == 0)
            {
                if ((stealth && variables.debugme) || !stealth) Console.WriteLine("No Bad Blocks found");
                return image;
            }

            image = remapbadblocks(image, badblocks, remappedblocks, bigblock);
            if ((stealth && variables.debugme) || !stealth) Console.WriteLine("");
            if ((stealth && variables.debugme) || !stealth) Console.WriteLine("Bad Blocks Remapped");
            return image;
        }

        public static string report_bad_blocks(string filename)
        {
            string text = "";
            Console.WriteLine("Checking for bad blocks...");
            long imgsize = 0;
            FileInfo file = new FileInfo(filename);
            byte[] image;
            int blocksize, reservedoffset;
            bool bigblock = false;

            if (file.Length >= 0x4200000)
            {
                bigblock = true;
                image = Oper.openfile(filename, ref imgsize, 0x4200000);
                blocksize = 0x21000;
                reservedoffset = 0x1E0;
            }
            else
            {
                image = Oper.openfile(filename, ref imgsize, 0);
                blocksize = 0x4200;
                reservedoffset = 0x3E0;
            }

            if (image[0x205] != 0xFF && image[0x415] != 0xFF && image[0x200] != 0xFF)
            {
                Console.WriteLine("Can't check for bad blocks. No Spare data. Possibly Corona 4GB");
                return "Can't check for bad blocks. No Spare data. Possibly Corona 4GB";
            }

            List<int> badblocks = new List<int>();
            List<int> remappedblocks = new List<int>();


            if (variables.debugme) Console.WriteLine("-R-Image Size: 0x{0:X} | imagesize: 0x{1:X} | File Size: 0x{0:X}", image.Length, blocksize, file.Length);

            int counter;
            for (counter = 0; counter < image.Length / blocksize; counter++)
            {
                byte[] block = new byte[blocksize];
                Buffer.BlockCopy(image, counter * blocksize, block, 0, blocksize);
                if (checkifbadblock(block, counter, bigblock, true))
                {
                    badblocks.Add(counter);
                    text += ("• Bad Block ID @ 0x" + counter.ToString("X") + " [Offset: 0x" + ((counter) * blocksize).ToString("X") + "]");
                    text += Environment.NewLine;
                }
                if (badblocks.Count >= 0x20)
                {
                    Console.WriteLine("Done");
                    return ("Too Many Bad Blocks");
                }
            }


            if (badblocks.Count == 0)
            {
                Console.WriteLine("Done");
                return "No bad blocks were found";
            }
            //find if remapped
            text += Environment.NewLine;
            text += Environment.NewLine;

            int reserveblockpos;
            if (blocksize == 0x4200) reserveblockpos = 0x3FF;
            else reserveblockpos = 0x1FF;

            int reservestartpos = reserveblockpos - 0x20;
            bool found = false;
            byte[] reserved = Oper.returnportion(image, reservedoffset * blocksize, 0x20 * blocksize);
            if (variables.debugme) Oper.savefile(reserved, "reservedarea.bin");
            image = null;

            remappedblocks = checkifremapped(reserved, badblocks, bigblock, true);

            int i = 0;
            foreach (int blockoffset in remappedblocks)
            {
                if (blockoffset != -1)
                {
                    found = true;
                    text += ("• Bad Block ID @ 0x" + badblocks[i].ToString("X") + " Found @ 0x" + (reservestartpos + blockoffset + 1).ToString("X") + "[Offset: 0x" + (blocksize * (reservestartpos + blockoffset + 1)).ToString("X") + "]");
                    text += Environment.NewLine;
                }
                i++;
            }

            if (!found)
            {
                text += ("Remapped Blocks don't exist.");
                Console.WriteLine("Done");
                return text;
            }

            Console.WriteLine("Done");
            return text;
        }

        /// <summary>
        /// Find Bad Blocks in X Blocks
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <param name="howmany">The number of blocks it will check plus 1</param>
        /// <returns></returns>
        public static byte[] find_bad_blocks_X(string filename, int howmany)
        {
            long imgsize = 0;
            FileInfo file = new FileInfo(filename);
            byte[] image;
            int blocksize, reservedoffset;
            bool bigblock;

            if (file.Length >= 0x4200000)
            {
                blocksize = 0x21000;
                bigblock = true;
                reservedoffset = 0x1E0;
            }
            else
            {
                blocksize = 0x4200;
                bigblock = false;
                reservedoffset = 0x3E0;
            }
            image = Oper.openfile(filename, ref imgsize, blocksize * howmany);

            if (image[0x205] == 0xFF || image[0x415] == 0xFF || image[0x200] == 0xFF)
            { }
            else
            {
                if (variables.debugme) Console.WriteLine("Can't check for bad blocks. No Spare data. Possibly Corona 4GB");
                return image;
            }

            List<int> badblocks = new List<int>();
            List<int> remappedblocks = new List<int>();


            if (variables.debugme) Console.WriteLine("- XS - Image Size: 0x{0:X} | imagesize: 0x{1:X} | File Size: 0x{0:X}", image.Length, blocksize, file.Length);

            int counter;
            byte[] block = new byte[blocksize];
            for (counter = 0; counter < howmany; counter++)
            {
                Buffer.BlockCopy(image, counter * blocksize, block, 0, blocksize);
                if (checkifbadblock(block, counter, bigblock, true))
                {
                    badblocks.Add(counter);
                }
                if (badblocks.Count >= 0x20)
                {
                    Console.WriteLine("Too Many Bad Blocks");
                    return image;
                }
            }

            if (badblocks.Count == 0)
            {
                return image;
            }
            //find if remapped
            byte[] reserved = new byte[0x20 * blocksize];
            reserved = Oper.openfilefromoffset(filename, ref imgsize, 0x20 * blocksize, reservedoffset * blocksize);

            remappedblocks = checkifremapped(reserved, badblocks, bigblock, true);


            bool found = false;
            foreach (int blockoffset in remappedblocks)
            {
                //if (variables.debugme) Console.WriteLine(blockoffset);
                if (blockoffset != -1)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                if (variables.debugme) Console.WriteLine("Can't fix it. Remapped Blocks don't exist.");
                return image;
            }

            image = remapbadblocks(image, reserved, badblocks, remappedblocks, bigblock, true);

            return image;
        }

        public static byte[] find_bad_blocks_X(byte[] image, int howmany)
        {
            int blocksize, reservedoffset;
            bool bigblock;

            if (image.Length >= 0x4200000)
            {
                image = Oper.returnportion(image, 0, 0x4200000);
                blocksize = 0x21000;
                bigblock = true;
                reservedoffset = 0x1E0;
            }
            else
            {
                blocksize = 0x4200;
                bigblock = false;
                reservedoffset = 0x3E0;
            }

            if (image[0x205] == 0xFF || image[0x415] == 0xFF || image[0x200] == 0xFF)
            { }
            else
            {
                if (variables.debugme) Console.WriteLine("Can't check for bad blocks. No Spare data. Possibly Corona 4GB");
                return image;
            }

            List<int> badblocks = new List<int>();
            List<int> remappedblocks = new List<int>();


            if (variables.debugme) Console.WriteLine("- XB - Image Size: 0x{0:X} | imagesize: 0x{1:X}", image.Length, blocksize);

            int counter;
            for (counter = 0; counter < howmany; counter++)
            {
                if (checkifbadblock(Oper.returnportion(image, counter * blocksize, blocksize), counter, bigblock))
                {
                    badblocks.Add(counter);
                }
                if (badblocks.Count >= 0x20)
                {
                    Console.WriteLine("Too Many Bad Blocks");
                    return image;
                }
            }

            if (badblocks.Count == 0)
            {
                return image;
            }
            //find if remapped

            remappedblocks = checkifremapped(Oper.returnportion(image, reservedoffset * blocksize, 0x20 * blocksize), badblocks, bigblock);


            bool found = false;
            foreach (int blockoffset in remappedblocks)
            {
                if (blockoffset != -1)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                if (variables.debugme) Console.WriteLine("Can't fix it. Remapped Blocks don't exist.");
                return image;
            }

            image = remapbadblocks(image, badblocks, remappedblocks, bigblock);

            return image;
        }

        public static byte[] openRemappedImage(string filename, int length, List<int> badBlocks, List<int> remappedBlocks, bool bigblock, bool hasecc)
        {
            long imgsize = 0;
            byte[] image = Oper.openfile(filename, ref imgsize, length);
            if (!hasecc) return image;
            int blocksize, reservedoffset;
            if (bigblock)
            {
                blocksize = 0x21000;
                reservedoffset = 0x1E0;
            }
            else
            {
                blocksize = 0x4200;
                reservedoffset = 0x3E0;
            }
            byte[] reserved = new byte[0x20 * blocksize];
            reserved = Oper.openfilefromoffset(filename, ref imgsize, 0x20 * blocksize, reservedoffset * blocksize);
            image = remapbadblocks(image, reserved, badBlocks, remappedBlocks, bigblock, true);
            return image;
        }
    }
}
