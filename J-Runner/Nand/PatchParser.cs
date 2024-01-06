﻿using System;
using System.Collections.Generic;
using System.Windows;

namespace JRunner.Nand
{
    public class PatchParser // Patch Parser by Mena Azer, 2022
    {
        public UInt32 address;
        public UInt32 patchCount;
        public UInt32[] patches;
        public int index;
        
        public byte[] patchArray;
        private List<ntable._patch> foundPatches = new List<ntable._patch>();

        public PatchParser(byte[] data)
        {
            patchArray = data;
        }

        public void enterData(byte[] data)
        {
            patchArray = data;
        }

        public static UInt32 ReverseBytes(UInt32 value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        public UInt32 getAddress(byte[] data)
        {
            address = ReverseBytes(BitConverter.ToUInt32(patchArray, index));
            index += 0x4;
            return address;
        }

        public UInt32 getCount(byte[] data)
        {
            patchCount = ReverseBytes(BitConverter.ToUInt32(patchArray, index));
            index += 0x4;
            return patchCount;
        }

        public UInt32[] getPatches(byte[] data)
        {
            patches = new UInt32[patchCount];
            for (int i = 0; i < patchCount; i++)
            {
                patches[i] = ReverseBytes(BitConverter.ToUInt32(patchArray, index));
                index += 0x4;
            }
            return patches;
        }

        public void printPatches()
        {
            foreach (var pat in patches)
            {
                Console.WriteLine("Patch: 0x" + pat.ToString("X8"));
            }
        }

        public void printAddress()
        {
            Console.WriteLine("Address: 0x" + address.ToString("X8"));
        }

        public void printPatchCount()
        {
            Console.WriteLine("Patch Count: 0x" + patchCount.ToString("X8"));
        }

        public bool parseAll(int i = 0)
        {
            bool foundAPatch = false;
            index = i;

            while (getAddress(patchArray) != (UInt32)0xFFFFFFFFU) // moves index+4
            {
                index -= 0x4; // return index to original position
                UInt32 detectd2m_devgl = getAddress(patchArray);

                if (detectd2m_devgl == 0x00000000 || detectd2m_devgl == 0xF0000000) // moves index to check
                {
                    index += 0x50; // devgl/g2m detect
                    index -= 0x4; // go back to original location + 0x50
                    continue; // iterate
                }
                else
                {
                    index -= 0x4; //return to original position
                }

                foreach (ntable._patch patch in ntable.patchTable)
                {
                    if (getAddress(patchArray) == patch.address) // moves index and gets address
                    {
                        if (getCount(patchArray) == patch.count || patch.count == 0x00000000) // moves index and gets count
                        {
                            UInt32[] patchlist;
                            patchlist = getPatches(patchArray);

                            if (patchlist[0] == patch.patch) // gets patch
                            {
                                foundAPatch = true;
                                foundPatches.Add(patch);

                                if (patch.consoleMsg != "")
                                {
                                    if (patch.name != "XLHDD" && patch.name != "XLUSB")
                                    {
                                        Console.WriteLine(patch.consoleMsg);
                                    }
                                }

                                if (!variables.noPatchWarnings && patch.messageBox != "")
                                {
                                    if (patch.name != "XLHDD" && patch.name != "XLUSB")
                                    {
                                        MessageBox.Show(patch.messageBox, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }

                                if (patch.name == "XLHDD")
                                {
                                    variables.foundXlHdd = true;
                                }
                                else if (patch.name == "XLUSB")
                                {
                                    variables.foundXlUsb = true;
                                }
                                else if (patch.name == "UsbdSec")
                                {
                                    variables.foundUsbdSec = true;
                                }
                                else if (patch.name == "CoronaKeyFix")
                                {
                                    variables.foundCoronaKeyFix = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        index -= 0x4; //return to origin
                    }
                }

                getAddress(patchArray);

                if (getCount(patchArray) > 0x1000)
                {
                    // assume image has no patches
                    index = 0x0;
                    break;
                }
                getPatches(patchArray);
            }

            return foundAPatch;
        }

        public int getIndex()
        {
            return index;
        }

        public void resetIndex()
        {
            index = 0;
        }
    }

    public class Patches
    {
        public static void patchParseFinal()
        {
            if (variables.foundXlHdd && variables.foundXlUsb)
            {
                variables.foundXlBoth = true;
                variables.foundXlHdd = false;
                variables.foundXlUsb = false;

                Console.WriteLine("Nand includes Both XL patches");
                if (!variables.noPatchWarnings) MessageBox.Show("This NAND has Both XL patches applied\n\nUSBs not formatted via FATXplorer, and all USB memory units, will no longer work\n\nIf you don't want this, generate an image without the Both XL checked under \"Patches/Drive Patches\"", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (variables.foundXlHdd)
            {
                variables.foundXlBoth = false;
                variables.foundXlUsb = false;
                showPatchMsg("XLHDD");
            }
            else if (variables.foundXlUsb)
            {
                variables.foundXlBoth = false;
                variables.foundXlHdd = false;
                showPatchMsg("XLUSB");
            }
        }

        public static void showPatchMsg(string patchName) // Thx DrTrinity!
        {
            int index = Array.FindIndex(ntable.patchTable, patchtable => patchtable.name == patchName);

            if (ntable.patchTable[index].name == patchName)
            {
                Console.WriteLine(ntable.patchTable[index].consoleMsg);
            }

            if (ntable.patchTable[index].name == patchName)
            {
                if (!variables.noPatchWarnings) MessageBox.Show(ntable.patchTable[index].messageBox, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
