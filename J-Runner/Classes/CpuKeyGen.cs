using System;
using System.Collections;
using System.Security.Cryptography;

namespace JRunner
{
    public static class CpuKeyGen
    {
        static int hamming = 0;
        static byte[] bytes = new byte[16];
        static byte[] generatedKey;

        public static string GenerateKey()
        {
            var csprng = new RNGCryptoServiceProvider();
            csprng.GetNonZeroBytes(bytes);
            while (!VerifyKey(bytes))
            {
                csprng.GetNonZeroBytes(bytes);
            }
            return BitConverter.ToString(generatedKey).Replace("-", string.Empty);
        }

        private static bool VerifyKey(byte[] key)
        {
            byte[] hammingArray = new byte[13];

            //CB 74 FC A5 5F 12 64 01 F6 B2 5B 84 2D 
            //A6 C2 97 cut 

            // C    B    7    4
            //1100 1011 0111 0100
            Buffer.BlockCopy(key, 0, hammingArray, 0, 13);
            BitArray bitArray = new BitArray(hammingArray); //array of true/false for 0/1 ez


            foreach (bool s in bitArray)
            {
                if (s) hamming++; //if true, increase hamming
            }
            //if hamming is 53 already, great. make sure both checks below fail.

            //Don't pull your hair out like I did for a bit, 13 is A6 in the cut off portion. I forgot about 0 hehe
            //1010 0110 in binary 
            if (key[13].getBit(0))
            {    //shift 0, get t/f
                hamming++;
            }

            if (key[13].getBit(1)) //shift to left one, get t/f. add 
            {
                hamming++;
            }


            if (hamming != 53)
            {
                hamming = 0;
                return false;
            }

            generatedKey = CalculateCPUKeyECD(key);

            Array.Copy(generatedKey, key, 16);
            if (!ByteArrayCompare(key, generatedKey)) return false;
            else return true;
        }

        public static bool ByteArrayCompare(byte[] a1, byte[] a2, int size = 0)
        {
            if (a1 == null || a2 == null) return false;
            if (size == 0)
            {
                size = a1.Length;
                if (a1.Length != a2.Length)
                    return false;
            }

            for (int i = 0; i < size; i++)
                if (a1[i] != a2[i])
                    return false;

            return true;
        }

        private static byte[] CalculateCPUKeyECD(byte[] key)
        {
            byte[] ecd = new byte[0x10];
            Buffer.BlockCopy(key, 0, ecd, 0, 0x10); //src, offsetstart, dst, offsetstart, len

            uint acc1 = 0, acc2 = 0;

            for (var cnt = 0; cnt < 0x80; cnt++, acc1 >>= 1)
            {

                var bTmp = ecd[cnt >> 3];
                var dwTmp = (uint)((bTmp >> (cnt & 7)) & 1);
                if (cnt < 0x6A)
                {
                    acc1 = dwTmp ^ acc1;
                    if ((acc1 & 1) > 0)
                        acc1 = acc1 ^ 0x360325;
                    acc2 = dwTmp ^ acc2;
                }
                else if (cnt < 0x7F)
                {
                    if (dwTmp != (acc1 & 1))
                        ecd[(cnt >> 3)] = (byte)((1 << (cnt & 7)) ^ (bTmp & 0xFF));
                    acc2 = (acc1 & 1) ^ acc2;
                }
                else if (dwTmp != acc2)
                    ecd[0xF] = (byte)((0x80 ^ bTmp) & 0xFF);
            }

            return ecd;
        }
    }
}
