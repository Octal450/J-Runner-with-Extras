using System;

namespace JRunner.Nand
{
    public class PatchParser
    {
        UInt32 address;
        UInt32 patchCount;
        UInt32[] patches;
        int index;
        byte[] patchArray;

        public PatchParser(byte[] data)
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
            index += 4;
            return address;
        }

        public UInt32 getCount(byte[] data)
        {
            patchCount = ReverseBytes(BitConverter.ToUInt32(patchArray, index));
            index += 4;
            return patchCount;
        }

        public UInt32[] getPatches(byte[] data)
        {
            patches = new UInt32[patchCount];
            for (int i = 0; i < patchCount; i++)
            {
                patches[i] = ReverseBytes(BitConverter.ToUInt32(patchArray, index));
                index += 4;
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

        public void parseAll(int i = 0)
        {
            index = i;
            while (getAddress(patchArray) != (UInt32)0xFFFFFFFFU)
            {
                index -= 0x4;
                getAddress(patchArray);

                getCount(patchArray);
                getPatches(patchArray);
                printAddress();
                printPatchCount();
                printPatches();
            }
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
}
