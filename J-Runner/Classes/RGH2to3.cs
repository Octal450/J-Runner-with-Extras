using RC4Cryptography;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace JRunner.Classes
{
    public enum BLOCK_TYPE
    {
        NONE,
        SMALL,
        BIG_ON_SMALL,
        BIG
    };

    public enum RGH_CONVERT_ERROR
    {
        ERROR_NONE = 0,
        ERROR_INVALID_ECC_SIZE,
        ERROR_INVALID_ECC_BOOTLOADERS,
        ERROR_INVALID_FLASH_LENGTH,
        ERROR_INVALID_FLASH_BLOCK_TYPE,
        ERROR_XELL_NOT_FOUND,
        ERROR_FLASH_CBB_DECRYPT_FAILED
    };

    public class RGH2to3
    {
        private const int ECC_SIZE_ECC = 1351680;
        private const int ECC_SIZE_NOECC = 1310720;
        private static readonly byte[] _1BL_KEY = { 0xDD, 0x88, 0xAD, 0x0C, 0x9E, 0xD6, 0x69, 0xE7, 0xB5, 0x67, 0x94, 0xFB, 0x68, 0x56, 0x3E, 0xFA };

        internal static byte[] XeCryptHmacSha(byte[] key, byte[] data0, int offset0, int size0, byte[] data1 = null, int offset1 = 0, int size1 = 0, byte[] data2 = null, int offset2 = 0, int size2 = 0)
        {
            using (var h = new HMACSHA1(key))
            using (var ms = new MemoryStream())
            {
                if (data0 != null && size0 > 0 && data0.Length >= size0)
                    ms.Write(data0, offset0, size0);
                if (data1 != null && size1 > 0 && data1.Length >= size1)
                    ms.Write(data1, offset1, size1);
                if (data2 != null && size2 > 0 && data2.Length >= size2)
                    ms.Write(data2, offset2, size2);
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                return h.ComputeHash(ms).Take(0x10).ToArray();
            }
        }

        internal static byte[] Rc4Crypt(byte[] key, byte[] data)
        {
            return RC4.Apply(data, key);
        }

        internal static string Hexlify(byte[] data, int offset, int size)
        {
            return BitConverter.ToString(data.Skip(offset).Take(size).ToArray()).Replace("-", "").ToUpper();
        }

        internal static byte[] Unhexlify(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        internal static bool ByteArrayCompare(byte[] data0, int offset0, byte[] data1, int offset1, int size = 0)
        {
            if (data0.Length < size || data1.Length < size)
                return false;
            if (size == 0)
                size = data0.Length;
            return data0.Skip(offset0).Take(size).SequenceEqual(data1.Skip(offset1).Take(size));
        }

        internal static ushort U16ReadBE(byte[] data, int offset)
        {
            data = data.Skip(offset).Take(2).ToArray();
            Array.Reverse(data);
            return BitConverter.ToUInt16(data, 0);
        }

        internal static uint U32ReadBE(byte[] data, int offset)
        {
            data = data.Skip(offset).Take(4).Reverse().ToArray();
            return BitConverter.ToUInt32(data, 0);
        }

        internal static uint CalcEcc(byte[] data)
        {
            uint val = 0, v = 0;
            for (uint bit = 0; bit < 0x1066; bit++)
            {
                if ((bit & 31) == 0)
                    v = ~BitConverter.ToUInt32(data, (int)(bit / 8));
                val ^= v & 1;
                v >>= 1;
                if ((val & 1) != 0)
                    val ^= 0x6954559;
                val >>= 1;
            }
            val = ~val;
            return (val << 6) & 0xFFFFFFFF;
        }

        internal static byte[] AddEcc(byte[] data, BLOCK_TYPE blockType = BLOCK_TYPE.BIG_ON_SMALL)
        {
            using (var rms = new MemoryStream(data))
            using (var wms = new MemoryStream())
            {
                int block = 0;
                while (rms.Position < data.Length)
                {
                    byte[] buff = new byte[528];
                    rms.Read(buff, 0, 512);

                    using (var ms = new MemoryStream(buff))
                    using (var bw = new BinaryWriter(ms))
                    {
                        ms.Seek(512, SeekOrigin.Begin);
                        if (blockType == BLOCK_TYPE.BIG_ON_SMALL)
                        {
                            bw.Write((byte)0);
                            bw.Write((uint)(block / 32));
                            bw.Write(new byte[] { 0xFF, 0, 0 });
                        }
                        else if (blockType == BLOCK_TYPE.BIG)
                        {
                            bw.Write((byte)0xFF);
                            bw.Write((uint)(block / 256));
                            bw.Write(new byte[] { 0, 0, 0 });
                        }
                        else if (blockType == BLOCK_TYPE.SMALL)
                        {
                            bw.Write((uint)(block / 32));
                            bw.Write(new byte[] { 0, 0xFF, 0, 0 });
                        }
                        else
                            return null;
                        buff = ms.ToArray();
                    }
                    Buffer.BlockCopy(BitConverter.GetBytes(CalcEcc(buff)), 0, buff, 524, 4);
                    block++;
                    wms.Write(buff, 0, buff.Length);
                }
                return wms.ToArray();
            }
        }

        internal static byte[] UnEcc(byte[] data)
        {
            using (var rms = new MemoryStream(data))
            using (var wms = new MemoryStream())
            {
                for (int i = 0; i < data.Length / 528; i++)
                {
                    byte[] buff = new byte[512];
                    rms.Read(buff, 0, buff.Length);
                    rms.Seek(0x10, SeekOrigin.Current);
                    wms.Write(buff, 0, buff.Length);
                }
                return wms.ToArray();
            }
        }

        internal static byte[] DecryptCBA(byte[] cbaData)
        {
            byte[] key = XeCryptHmacSha(_1BL_KEY, cbaData, 0x10, 0x10);
            using (var ms = new MemoryStream())
            {
                ms.Write(cbaData.Take(0x10).ToArray(), 0, 0x10);
                ms.Write(key, 0, 0x10);
                ms.Write(Rc4Crypt(key, cbaData.Skip(0x20).ToArray()), 0, cbaData.Length - 0x20);
                return ms.ToArray();
            }
        }

        internal static byte[] DecryptCBB(byte[] cbbData, byte[] cbaNonce, byte[] cpuKey)
        {
            byte[] key = XeCryptHmacSha(cbaNonce, cbbData, 0x10, 0x10, cpuKey, 0, 0x10);
            using (var ms = new MemoryStream())
            {
                ms.Write(cbbData.Take(0x10).ToArray(), 0, 0x10);
                ms.Write(key, 0, 0x10);
                ms.Write(Rc4Crypt(key, cbbData.Skip(0x20).ToArray()), 0, cbbData.Length - 0x20);
                return ms.ToArray();
            }
        }

        public static RGH_CONVERT_ERROR ConvertRgh2ToRgh3(string eccPath, string flashPath, string cpuKey, string outPath)
        {
            byte[] output;
            var ret = ConvertRgh2ToRgh3(File.ReadAllBytes(eccPath), File.ReadAllBytes(flashPath), Unhexlify(cpuKey), out output);
            File.WriteAllBytes(outPath, output);
            return ret;
        }

        public static RGH_CONVERT_ERROR ConvertRgh2ToRgh3(string eccPath, string flashPath, byte[] cpuKey, string outPath)
        {
            byte[] output;
            var ret = ConvertRgh2ToRgh3(File.ReadAllBytes(eccPath), File.ReadAllBytes(flashPath), cpuKey, out output);
            File.WriteAllBytes(outPath, output);
            return ret;
        }

        public static RGH_CONVERT_ERROR ConvertRgh2ToRgh3(byte[] eccData, byte[] flashData, string cpuKey, out byte[] output)
        {
            return ConvertRgh2ToRgh3(eccData, flashData, Unhexlify(cpuKey), out output);
        }

        public static RGH_CONVERT_ERROR ConvertRgh2ToRgh3(byte[] eccData, byte[] flashData, byte[] cpuKey, out byte[] output)
        {
            output = null;

            if (eccData.Length == ECC_SIZE_ECC)
            {
                eccData = UnEcc(eccData);
            }
            else if (eccData.Length == ECC_SIZE_NOECC)
            {
                // nothing
            }
            else
            {
                return RGH_CONVERT_ERROR.ERROR_INVALID_ECC_SIZE;
            }

            // RGH3 flash header
            uint rgh3SmcLen = U32ReadBE(eccData, 0x78);
            uint rgh3SmcOffs = U32ReadBE(eccData, 0x78 + 4);
            byte[] rgh3Smc = eccData.Skip((int)rgh3SmcOffs).Take((int)rgh3SmcLen).ToArray();
            uint loaderOffs = U32ReadBE(eccData, 8);

            // RGH3 CB_A
            ushort loaderName = U16ReadBE(eccData, (int)loaderOffs);
            ushort loaderVer = U16ReadBE(eccData, (int)(loaderOffs + 2));
            uint loaderFlags = U32ReadBE(eccData, (int)(loaderOffs + 4));
            uint loaderEntry = U32ReadBE(eccData, (int)(loaderOffs + 8));
            uint loaderSize = U32ReadBE(eccData, (int)(loaderOffs + 12));
            byte[] rgh3Cba = eccData.Skip((int)loaderOffs).Take((int)loaderSize).ToArray();
            loaderOffs += loaderSize;

            // RGH3 CB_B
            loaderName = U16ReadBE(eccData, (int)loaderOffs);
            loaderVer = U16ReadBE(eccData, (int)(loaderOffs + 2));
            loaderFlags = U32ReadBE(eccData, (int)(loaderOffs + 4));
            loaderEntry = U32ReadBE(eccData, (int)(loaderOffs + 8));
            loaderSize = U32ReadBE(eccData, (int)(loaderOffs + 12));
            byte[] rgh3Payload = eccData.Skip((int)loaderOffs).Take((int)loaderSize).ToArray();

            if (rgh3Cba.Length == 0 || rgh3Payload.Length == 0)
            {
                return RGH_CONVERT_ERROR.ERROR_INVALID_ECC_BOOTLOADERS;
            }

            bool flashHasEcc = false;
            uint xellOffs = 0;
            byte[] patchFlashData;
            if (flashData.Length == 17301504 || flashData.Length == 69206016)
            {
                xellOffs = 0x73800;
                flashHasEcc = true;
                patchFlashData = flashData.Take((int)xellOffs).ToArray();
                patchFlashData = UnEcc(patchFlashData);
            }
            else if (flashData.Length == 50331648)
            {
                xellOffs = 0x70000;
                patchFlashData = flashData.Take((int)xellOffs).ToArray();
            }
            else
                return RGH_CONVERT_ERROR.ERROR_INVALID_FLASH_LENGTH;

            BLOCK_TYPE blockType = BLOCK_TYPE.NONE;
            if (flashHasEcc)
            {
                byte[] spareSample = flashData.Skip(0x4400).Take(0x10).ToArray();
                if (spareSample[0] == 0xFF)
                {
                    blockType = BLOCK_TYPE.BIG;
                }
                else if (spareSample[5] == 0xFF)
                {
                    if (ByteArrayCompare(spareSample, 0, new byte[] { 0x01, 0x00 }, 0, 2))
                    {
                        blockType = BLOCK_TYPE.SMALL;
                    }
                    else if (ByteArrayCompare(spareSample, 0, new byte[] { 0x00, 0x01 }, 0, 2))
                    {
                        blockType = BLOCK_TYPE.BIG_ON_SMALL;
                    }
                    else
                        return RGH_CONVERT_ERROR.ERROR_INVALID_FLASH_BLOCK_TYPE;
                }
                else
                    return RGH_CONVERT_ERROR.ERROR_INVALID_FLASH_BLOCK_TYPE;
            }
            else
            {
                // 4GB flash image
            }

            if (!ByteArrayCompare(flashData, (int)xellOffs, Unhexlify("48000020480000EC4800000048000000"), 0, 0x10))
            {
                return RGH_CONVERT_ERROR.ERROR_XELL_NOT_FOUND;
            }

            patchFlashData = patchFlashData.Take((int)rgh3SmcOffs)
                .Concat(rgh3Smc)
                .Concat(patchFlashData.Skip((int)(rgh3SmcOffs + rgh3SmcLen)))
                .ToArray();

            loaderOffs = U32ReadBE(patchFlashData, 8);

            // flash CB_A
            loaderName = U16ReadBE(patchFlashData, (int)loaderOffs);
            loaderVer = U16ReadBE(patchFlashData, (int)(loaderOffs + 2));
            loaderFlags = U32ReadBE(patchFlashData, (int)(loaderOffs + 4));
            loaderEntry = U32ReadBE(patchFlashData, (int)(loaderOffs + 8));
            loaderSize = U32ReadBE(patchFlashData, (int)(loaderOffs + 12));
            uint flashCbaOffs = loaderOffs;
            byte[] flashCba = patchFlashData.Skip((int)flashCbaOffs).Take((int)loaderSize).ToArray();
            loaderOffs += loaderSize;

            // flash CB_B
            loaderName = U16ReadBE(patchFlashData, (int)loaderOffs);
            loaderVer = U16ReadBE(patchFlashData, (int)(loaderOffs + 2));
            loaderFlags = U32ReadBE(patchFlashData, (int)(loaderOffs + 4));
            loaderEntry = U32ReadBE(patchFlashData, (int)(loaderOffs + 8));
            loaderSize = U32ReadBE(patchFlashData, (int)(loaderOffs + 12));
            uint flashCbbOffs = loaderOffs;
            byte[] flashCbb = patchFlashData.Skip((int)flashCbbOffs).Take((int)loaderSize).ToArray();
            // loaderOffs += loaderSize;

            flashCba = DecryptCBA(flashCba);
            flashCbb = DecryptCBB(flashCbb, flashCba.Skip(0x10).Take(0x10).ToArray(), cpuKey);
            if (!ByteArrayCompare(flashCbb, 0x392, Unhexlify("58424F585F524F4D"), 0, 8) &&
                !ByteArrayCompare(flashCbb, 0x392, Unhexlify("0000000000000000"), 0, 8)
                )
            {
                return RGH_CONVERT_ERROR.ERROR_FLASH_CBB_DECRYPT_FAILED;
            }

            int origSize = patchFlashData.Length;
            byte[] newCbb = rgh3Payload.Concat(flashCbb).ToArray();

            patchFlashData = patchFlashData.Take((int)flashCbaOffs)
                .Concat(rgh3Cba)
                .Concat(newCbb)
                .Concat(patchFlashData.Skip((int)(flashCbbOffs + flashCbb.Length)))
                .Take(origSize)
                .ToArray();

            if (flashHasEcc)
                patchFlashData = AddEcc(patchFlashData, blockType);

            output = patchFlashData.Concat(flashData.Skip(patchFlashData.Length).Take(flashData.Length - patchFlashData.Length)).ToArray();

            return RGH_CONVERT_ERROR.ERROR_NONE;
        }
    }
}