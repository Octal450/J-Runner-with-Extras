using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JRunner
{
    public static class Oper
    {
        /// <summary>
        /// File Manipulation - Open File - Save File
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="size"></param>
        /// <param name="wantedsize"></param>
        /// <returns>byte[]</returns>
        #region file manipulation

        public static byte[] openfile(string filename, ref long size, int wantedsize)
        {
            try
            {
                FileInfo info = new FileInfo(filename);
                if (wantedsize == 0 || wantedsize > info.Length)
                {
                    size = info.Length;
                }
                else
                    size = wantedsize;
                FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read);
                BinaryReader file = new BinaryReader(infile);
                byte[] data = new byte[size];
                int i = 0;
                for (i = 0; i < size; i++) data[i] = file.ReadByte();
                file.Close();
                infile.Close();
                return data;
            }
            catch (FileNotFoundException ex) { Console.WriteLine("File {0} not found!", filename); if (variables.debugme) Console.WriteLine(ex.ToString()); }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            return null;
        }

        public static byte[] openfilefromoffset(string filename, ref long size, int wantedsize, int offset)
        {
            try
            {
                if (variables.debugme) Console.WriteLine("filename: {0} - wantedsize: {1:X} - offset: {2:X}", filename, wantedsize, offset);
                FileInfo info = new FileInfo(filename);
                if (wantedsize == 0 || wantedsize + offset > info.Length)
                {
                    if (info.Length - offset < 0) size = 0;
                    else size = info.Length - offset;
                }
                else
                    size = wantedsize;
                if (variables.debugme) Console.WriteLine("size: {0:X}", size);
                FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read);
                BinaryReader file = new BinaryReader(infile);
                file.BaseStream.Seek(offset, SeekOrigin.Begin);
                byte[] data = new byte[size];
                int i = 0;
                for (i = 0; i < size; i++) data[i] = file.ReadByte();
                file.Close();
                infile.Close();
                return data;
            }
            catch (FileNotFoundException ex) { Console.WriteLine("File {0} not found!", filename); if (variables.debugme) Console.WriteLine(ex.ToString()); }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
            return null;
        }

        public static bool savefile(byte[] image, string name)
        {
            try
            {
                FileStream kvdf = new FileStream(name, FileMode.Create, FileAccess.Write);
                BinaryWriter kvdfi = new BinaryWriter(kvdf);
                for (int c = 0; c < image.Length; c++) kvdfi.Write(image[c]);
                kvdfi.Close();
                kvdf.Close();
                return true;
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); return false; }

        }

        public static string FilePickerInitialPath(string input)
        {
            if (input.Length > 0)
            {
                try
                {
                    FileAttributes pathattr = File.GetAttributes(Path.GetFullPath(input));
                    if (pathattr.HasFlag(FileAttributes.Directory))
                    {
                        return Path.GetFullPath(input);
                    }
                    else return Path.GetDirectoryName(input);
                }
                catch
                {
                    return "";
                }
            }
            else return "";
        }

        #endregion

        /// <summary>
        /// addtoflash - padto
        /// </summary>
        /// <param name="image"></param>
        /// <param name="padding"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        #region addtoflash - padto

        public static byte[] padto(byte[] image, byte padding, int length)
        {
            byte[] newimage = new byte[length];
            for (int i = 0; i < length; i++)
            {
                if (i < image.Length) newimage[i] = image[i];
                else newimage[i] = padding;
            }
            return newimage;

        }

        public static byte[] addtoflash_v2(byte[] image, byte[] secondimage)
        {
            byte[] tempimage = new byte[image.Length + secondimage.Length];
            Buffer.BlockCopy(image, 0, tempimage, 0, image.Length);
            Buffer.BlockCopy(secondimage, 0, tempimage, image.Length, secondimage.Length);
            return tempimage;
        }

        public static byte[] addtoflash_v1(byte[] image, byte[] secondimage)
        {
            byte[] tempimage = new byte[image.Length + secondimage.Length];
            int i;
            for (i = 0; i < image.Length; i++)
            {
                tempimage[i] = image[i];
            }
            for (; i < image.Length + secondimage.Length; i++)
            {
                tempimage[i] = secondimage[i - (image.Length)];
            }
            return tempimage;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        #region usefull functions

        public static byte[] endianness(byte[] a1)
        {
            if (a1 != null) Array.Reverse(a1);
            return a1;
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
        public static bool ByteArrayCompare(byte[] a1, byte[] a2, int a1startoffset, int a2startoffset, int size)
        {
            if (a1 == null || a2 == null) return false;
            if ((a1.Length - a1startoffset < size || a2.Length - a2startoffset < size) && a1.Length - a1startoffset != a2.Length - a2startoffset) return false;

            int length = a1.Length - a1startoffset < size ? a1.Length - a1startoffset : size;

            for (int i = 0; i < length; i++)
                if (a1[i + a1startoffset] != a2[i + a2startoffset])
                    return false;

            return true;
        }


        public static byte[] concatByteArrays(byte[] byteArray1, byte[] byteArray2, int array1Length, int array2Length)
        {
            int totalLength = array1Length + array2Length;
            byte[] newByteArray = new byte[totalLength];
            int i = 0;

            while (i < array1Length)
                newByteArray[i] = byteArray1[i++];

            while (i < totalLength)
            {
                newByteArray[i] = byteArray2[i - array1Length];
                i++;
            }
            return newByteArray;
            //byteArray1 = newByteArray; // can assignment be made for arrays?  
            // if not, return newByteArray
        }
        public static byte[] concatByte(byte[] byteArray1, byte byteArray2, int array1Length)
        {
            int totalLength = array1Length + 1;
            byte[] newByteArray = new byte[totalLength];
            int i = 0;

            while (i < array1Length)
                newByteArray[i] = byteArray1[i++];

            while (i < totalLength)
            {
                newByteArray[i] = byteArray2;
                i++;
            }
            return newByteArray;
            //byteArray1 = newByteArray; // can assignment be made for arrays?  
            // if not, return newByteArray
        }

        public static byte[] returnportion(ref byte[] data, int offset, int count)
        {
            if (data == null) return null;
            if (count < 0) count = 0;
            byte[] templist = new byte[count];
            if (offset + count > data.Length)
            {
                if (variables.debugme) Console.WriteLine("Bigger - offset: {0:X} - count {1:X}", offset, count);
                count = data.Length - offset;
            }
            if (count <= data.Length && count >= 0)
            {
                Buffer.BlockCopy(data, offset, templist, 0x00, count);
            }
            return templist;
        }
        public static byte[] returnportion(byte[] data, int offset, int count)
        {
            if (data == null) return null;
            if (count < 0) count = 0;
            byte[] templist = new byte[count];
            if (offset + count > data.Length)
            {
                if (variables.debugme) Console.WriteLine("Bigger - offset: {0:X} - count {1:X}", offset, count);
                count = data.Length - offset;
            }
            if (count <= data.Length && count >= 0)
            {
                Buffer.BlockCopy(data, offset, templist, 0x00, count);
            }
            return templist;
        }
        public static byte[] returnportion_ecc(byte[] data, int offset, int count, int startindex = 0)
        {
            if (data == null) return null;
            byte[] templist = new byte[count];
            int i = 0;
            int remain = 0;
            if (startindex != 0)
            {
                remain = 0x200 - (startindex % 0x210);
                Buffer.BlockCopy(data, offset, templist, 0, remain);
                i = remain + 0x10;
            }

            if ((count * 0x210) / 0x200 <= data.Length && count >= 0)
            {
                for (; i < (count * 0x210) / 0x200; i += 0x210)
                {
                    Buffer.BlockCopy(data, offset + i, templist, (i * 0x200) / 0x210, 0x200);
                }
            }
            return templist;
        }

        public static bool allsame(byte[] s, byte n)
        {
            foreach (byte x in s)
            {
                if (x != n) return false;
            }
            return true;
        }

        public static string ByteArrayToString(byte[] ba, int startindex = 0, int length = 0)
        {
            if (ba == null) return "";
            string hex = BitConverter.ToString(ba);
            if (startindex == 0 && length == 0) hex = BitConverter.ToString(ba);
            else if (length == 0 && startindex != 0) hex = BitConverter.ToString(ba, startindex);
            else hex = BitConverter.ToString(ba, startindex, length);
            return hex.Replace("-", "");
        }
        public static string ByteArrayToString_v2(byte[] ba, int startindex = 0, int length = 0)
        {
            if (ba == null) return "";
            return Encoding.ASCII.GetString(ba, startindex, length == 0 ? ba.Length : length);
        }
        public static decimal ByteArrayToDecimal(byte[] src)
        {

            // Create a MemoryStream containing the byte array
            using (MemoryStream stream = new MemoryStream(src))
            {

                // Create a BinaryReader to read the decimal from the stream
                using (BinaryReader reader = new BinaryReader(stream))
                {

                    // Read and return the decimal from the 
                    // BinaryReader/MemoryStream
                    return reader.ReadDecimal();
                }
            }
        }
        public static int ByteArrayToInt(byte[] value)
        {
            return Convert.ToInt32(ByteArrayToString(value), 16);
        }

        public static void removeByteArray(ref byte[] array, int start, int length)
        {
            //Console.WriteLine("{0:X}-{1:X}-{2:X}*{3:X}", array.Length, start, length, array.Length - (start + length));
            byte[] newarr = new byte[array.Length - (length)];
            Buffer.BlockCopy(array, 0, newarr, 0, start);
            Buffer.BlockCopy(array, start + length, newarr, start, array.Length - (start + length));
            //newarr = returnportion(array, length, array.Length - length);
            array = newarr;
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            if (NumberChars % 2 != 0)
            {
                hex = "0" + hex;
                NumberChars++;
            }
            if (NumberChars % 4 != 0)
            {
                hex = "00" + hex;
                NumberChars += 2;
            }
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        public static byte[] StringToByteArray_v2(String hex)
        {
            int NumberChars = hex.Length;
            if (NumberChars % 2 != 0)
            {
                hex = "0" + hex;
                NumberChars++;
            }
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        public static byte[] StringToByteArray_v2(String hex, int length)
        {
            int NumberChars = hex.Length;
            if (NumberChars % 2 != 0)
            {
                hex = "0" + hex;
                NumberChars++;
            }
            if (NumberChars % (length * 2) != 0)
            {
                for (int j = NumberChars; j < (length * 2); j += 2) hex = "00" + hex;
                NumberChars = length * 2;
            }
            byte[] bytes = new byte[length];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        public static byte[] StringToByteArray_v3(String hex)
        {
            return Encoding.ASCII.GetBytes(hex);
        }

        public static bool IndexOfSequence(this byte[] buffer, byte[] pattern, int startIndex)
        {
            bool position = false;
            int i = Array.IndexOf<byte>(buffer, pattern[0], startIndex);
            while (i >= 0 && i <= buffer.Length - pattern.Length)
            {
                byte[] segment = new byte[pattern.Length];
                Buffer.BlockCopy(buffer, i, segment, 0, pattern.Length);
                if (segment.SequenceEqual<byte>(pattern))
                    position = true;
                i = Array.IndexOf<byte>(buffer, pattern[0], i + pattern.Length);
            }
            return position;
        }

        public static byte[] TrimStart(byte[] buffer, byte trim)
        {
            int num = 0;
            while (buffer[num] == trim) num++;
            removeByteArray(ref buffer, 0, num);
            return buffer;
        }

        #endregion

        /// <summary>
        /// HMAC - RC4
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        #region crypto stuff

        public static byte[] HMAC_SHA1(byte[] Key, byte[] Message)
        {
            if (Key.Length < 0x10) return null;
            byte[] K = new byte[0x40];
            byte[] opad = new byte[20 + 0x40];
            byte[] ipad = new byte[Message.Length + 0x40];

            Array.Copy(Key, K, 16);

            for (int i = 0; i < 64; i++)
            {
                opad[i] = (byte)(K[i] ^ 0x5C);
                ipad[i] = (byte)(K[i] ^ 0x36);
            }

            // Copy Buffer
            Array.Copy(Message, 0, ipad, 0x40, Message.Length);

            // Get First Hash
            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            byte[] Hash1 = sha.ComputeHash(ipad);

            // Copy to OPad
            Array.Copy(Hash1, 0, opad, 0x40, 20);

            return sha.ComputeHash(opad);
        }

        public static void RC4_v(ref Byte[] bytes, Byte[] key)
        {
            Byte[] s = new Byte[256];
            Byte[] k = new Byte[256];
            Byte temp;
            int i, j;

            for (i = 0; i < 256; i++)
            {
                s[i] = (Byte)i;
                k[i] = key[i % key.GetLength(0)];
            }

            j = 0;
            for (i = 0; i < 256; i++)
            {
                j = (j + s[i] + k[i]) % 256;
                temp = s[i];
                s[i] = s[j];
                s[j] = temp;
            }

            i = j = 0;
            for (int x = 0; x < bytes.GetLength(0); x++)
            {
                i = (i + 1) % 256;
                j = (j + s[i]) % 256;
                temp = s[i];
                s[i] = s[j];
                s[j] = temp;
                int t = (s[i] + s[j]) % 256;
                bytes[x] ^= s[t];
            }
        }

        #endregion

        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception) { }
            return "";
        }
        public static string GetMD5HashFromFile(byte[] fileName)
        {
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fileName);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception) { }
            return "";
        }
    }

    public static partial class Extensions
    {
        public static void Endianess(this byte[] src)
        {
            Array.Reverse(src);
        }

        public static bool getBit(this byte src, int bitNumber)
        {
            return (src & (1 << bitNumber)) != 0;
        }

        public static uint toUint(this byte[] src, int offset = 0)
        {
            if (src.Length - offset < 4) return 0;
            byte[] temp = new byte[4];
            Buffer.BlockCopy(src, offset, temp, 0, 4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(temp);
            }
            return BitConverter.ToUInt32(temp, 0);
        }

        public static void setInt(this byte[] src, int num, int offset = 0)
        {
            byte[] bytes = BitConverter.GetBytes(num);
            Buffer.BlockCopy(bytes, 0, src, offset, bytes.Length);
        }

        public static bool getBit(this byte[] src, int bitNumber)
        {
            return (src.toUint() & (1 << bitNumber)) != 0;
        }

        public static void Replace(this byte[] src, byte[] data, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                src[offset + i] = data[i];
            }
        }

        public static void Fill(this byte[] src, byte fill)
        {
            for (int i = 0; i < src.Length; i++) src[i] = fill;
        }

        public static bool Contains(this byte[] src, byte check)
        {
            for (int i = 0; i < src.Length; i++)
            {
                if (check == src[i]) return true;
            }
            return false;
        }
    }
}
