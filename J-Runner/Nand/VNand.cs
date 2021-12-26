using System;
using System.Collections.Generic;
using System.IO;

namespace JRunner.Nand
{
    public class VNand
    {
        public string _flashconfig = "";
        public string _filename = "";
        public consoles _console = variables.cunts[0];
        public List<int> _badBlocks = new List<int>();

        public VNand(string filename, consoles c)
            : this(filename, c, "")
        {
        }

        public VNand(string filename, consoles c, string flashconfig) : this(filename, c, "", new List<int>())
        {
        }

        public VNand(string filename, consoles c, string flashconfig, List<int> badBlocks)
        {
            this._filename = filename;
            this._console = c;
            this._flashconfig = flashconfig;
            this._badBlocks = badBlocks;
        }

        public void create()
        {
            FileStream fs = (File.Open(_filename, FileMode.Create, FileAccess.ReadWrite));
            byte[] buffer = new byte[0x4200];

            for (int i = 0; i <= _console.Nsize.GetHashCode(); i++) fs.Write(buffer, 0, 0x4200);
        }

        public void read_v2(string filename, int startblock = 0, int length = 0)
        {
            byte[] readBuf = new byte[0x4200];

            try
            {
                File.Delete(filename);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); };

            if (length == 0)
            {
                length = _console.Nsize.GetHashCode();
                if (variables.debugme) Console.WriteLine("Length: {0:X} - size: {1}", length, _console.Nsize);
            }
            Console.WriteLine("Reading Nand");
            BinaryWriter sw = new BinaryWriter(File.Open(filename, FileMode.Append, FileAccess.Write));
            BinaryReader br = new BinaryReader(File.Open(_filename, FileMode.Open, FileAccess.Read));

            int i = startblock;
            while (i < (length + startblock) && !variables.escapeloop)
            {
                readBuf = new byte[0x4200];
                int lengthTransfered = 0;

                br.BaseStream.Position = i * 0x4200;
                lengthTransfered = br.Read(readBuf, 0, 0x4200);
                if (lengthTransfered != 0x4200) Console.WriteLine("Error reading block {0:X}", i);

                try
                {
                    sw.Write(readBuf);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                i++;
            }
            readBuf = null;
            variables.escapeloop = false;
            sw.Close();
            br.Close();

            Console.WriteLine("Done");
            Console.WriteLine();
            return;
        }

        public void erase_v2(int startblock = 0, int length = 0)
        {
            if (length == 0)
            {
                length = _console.Nsize.GetHashCode();
                if (variables.debugme) Console.WriteLine("Length: {0:X} - size: {1}", length, _console.Nsize);
            }
            Console.WriteLine("Erasing Nand");
            BinaryWriter bw = new BinaryWriter(File.Open(_filename, FileMode.Open, FileAccess.ReadWrite));
            int i = startblock;
            while (i < (length + startblock) && !variables.escapeloop)
            {
                bw.BaseStream.Seek(i * 0x4200, SeekOrigin.Begin);
                byte[] erasebuf = new byte[0x4200];
                bw.Write(erasebuf);
                i++;
            }
            variables.escapeloop = false;
            bw.Close();

            Console.WriteLine("Done");
            Console.WriteLine();
            return;

        }

        public void write_v2(string filename, int startblock = 0, int length = 0, bool remap = false, bool fixecc = false)
        {
            variables.iswriting = true;

            int layout = 1;
            if (_flashconfig == "00AA3020" || _flashconfig == "008A3020") layout = 2;
            else if (_flashconfig == "01198010") layout = 0;
            else layout = 1;

            byte[] writeBuffer = new byte[0x4200];
            BinaryReader rw = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));
            BinaryWriter bw = new BinaryWriter(File.Open(_filename, FileMode.Open, FileAccess.ReadWrite));

            if (length == 0)
            {
                length = _console.Nsize.GetHashCode();
                if (variables.debugme) Console.WriteLine("Length: {0:X} - size: {1}", length, _console.Nsize);
            }

            long filesize;
            FileInfo fl = new FileInfo(filename);
            filesize = fl.Length / 0x4200;
            if (variables.debugme) Console.WriteLine("FileSize: {0:X}", filesize);

            if (startblock + length > filesize)
            {
                length = (int)filesize - startblock;
            }
            if (length <= 0) length = 0;

            List<int> badblocks = new List<int>();
            Console.WriteLine("Writing Nand");
            Console.WriteLine(Path.GetFileName(filename));
            int i = startblock;
            if (variables.debugme) Console.WriteLine("Start: {0:X} - Length: {1:X}", startblock, length);
            while (i < (length + startblock) && !variables.escapeloop)
            {
                try
                {
                    writeBuffer = rw.ReadBytes(0x4200);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); variables.iswriting = false; }

                if (fixecc) writeBuffer = Nand.addecc_v2(writeBuffer, false, i * 0x4200, layout);

                bw.BaseStream.Seek(i * 0x4200, SeekOrigin.Begin);
                bw.Write(writeBuffer);

                if (_badBlocks.Contains(i))
                {
                    bw.BaseStream.Seek(i * 0x4200, SeekOrigin.Begin);
                    writeBuffer = new byte[0x4200];
                    bw.Write(writeBuffer);
                    Console.WriteLine("Failed to write 0x{0:X} block", i);
                    badblocks.Add(i);
                }

                i++;
            }
            if (badblocks.Count != 0 && remap)
            {
                Console.WriteLine("Starting remapping process");
                int count = badblocks.Count;

                while (count != 0)
                {
                    int number = badblocks.Count - count;
                    int reserveblockpos;
                    i = badblocks[number];
                    rw.BaseStream.Seek(0x4200 * i, SeekOrigin.Begin);
                    writeBuffer = rw.ReadBytes(0x4200);

                    if (fixecc) writeBuffer = Nand.addecc_v2(writeBuffer, false, i * 0x4200, layout);

                    if (_flashconfig == "00AA3020" || _flashconfig == "008A3020")
                    {
                        reserveblockpos = 0x1FF;
                    }
                    else
                    {
                        reserveblockpos = 0x3FF;
                    }

                    Console.WriteLine("Remapping Block {0:X} @ {1:X}", i, reserveblockpos - number);

                    bw.BaseStream.Seek((reserveblockpos - number) * 0x4200, SeekOrigin.Begin);
                    bw.Write(writeBuffer);

                    if (_badBlocks.Contains(reserveblockpos - number))
                    {
                        Console.WriteLine("Failed to write 0x{0:X} block", reserveblockpos - number);
                    }
                    count--;
                }
            }

            variables.escapeloop = false;
            rw.Close();
            bw.Close();
            variables.iswriting = false;
            Console.WriteLine("Done");
            Console.WriteLine();
            return;

        }

    }
}
