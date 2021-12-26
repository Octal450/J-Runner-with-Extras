using JRunner;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

internal static class NativeMethods
{
    #region Imports
    [DllImport("inpout32.dll", EntryPoint = "Out32")]
    internal static extern void Output(int adress, int value);

    [DllImport("inpout32.dll")]
    internal static extern UInt32 IsInpOutDriverOpen();

    [DllImport("inpout32.dll")]
    internal static extern void Out32(short PortAddress, short Data);

    [DllImport("inpout32.dll")]
    internal static extern char Inp32(short PortAddress);

    [DllImport("inpout32.dll")]
    private static extern void DlPortWritePortUshort(short PortAddress, ushort Data);

    [DllImport("inpout32.dll")]
    private static extern ushort DlPortReadPortUshort(short PortAddress);

    [DllImport("inpout32.dll")]
    private static extern void DlPortWritePortUlong(int PortAddress, uint Data);

    [DllImport("inpout32.dll")]
    private static extern uint DlPortReadPortUlong(int PortAddress);

    [DllImport("kernel32", SetLastError = true)]
    internal static extern bool FreeLibrary(IntPtr hModule);
    #endregion
}

public static class PortInterop
{
    private static short DATA_OFFSET = 0;
    private static short STATUS_OFFSET = 1;

    public static short TCK = 0;
    public static short TMS = 1;
    public static short TDI = 2;

    private static bool once = false;
    static OutBite out_world;
    public static int base_port = 0x378;

    [global::System.AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    sealed class BitfieldLengthAttribute : Attribute
    {
        uint length;

        public BitfieldLengthAttribute(uint length)
        {
            this.length = length;
        }

        public uint Length { get { return length; } }
    }

    static class PrimitiveConversion
    {
        public static long ToLong<T>(T t) where T : struct
        {
            long r = 0;
            int offset = 0;

            // For every field suitably attributed with a BitfieldLength
            foreach (System.Reflection.FieldInfo f in t.GetType().GetFields())
            {
                object[] attrs = f.GetCustomAttributes(typeof(BitfieldLengthAttribute), false);
                if (attrs.Length == 1)
                {
                    uint fieldLength = ((BitfieldLengthAttribute)attrs[0]).Length;

                    // Calculate a bitmask of the desired length
                    long mask = 0;
                    for (int i = 0; i < fieldLength; i++)
                        mask |= (uint)(1 << i); // IF PROBLEMS REMOVE THE UINT

                    r |= ((UInt32)f.GetValue(t) & mask) << offset;

                    offset += (int)fieldLength;
                }
            }

            return r;
        }
    }

    static void FromShort(short number, out byte byte1, out byte byte2)
    {
        byte2 = (byte)(number >> 8);
        byte1 = (byte)(number & 255);
    }

    struct OutBite
    {
        [BitfieldLength(1)]
        public uint TDI;
        [BitfieldLength(1)]
        public uint TCK;
        [BitfieldLength(1)]
        public uint TMS;
        [BitfieldLength(1)]
        public uint zero;
        [BitfieldLength(1)]
        public uint one;
        [BitfieldLength(1)]
        public uint Bit5;
        [BitfieldLength(1)]
        public uint Bit6;
        [BitfieldLength(1)]
        public uint Bit7;
    };

    public static void UnloadModule(string moduleName)
    {
        foreach (ProcessModule mod in Process.GetCurrentProcess().Modules)
        {
            if (mod.ModuleName == moduleName)
            {
                NativeMethods.FreeLibrary(mod.BaseAddress);
            }
        }
    }

    public static void setPort(short p, byte val)
    {
        /* Initialize static out_word register bits just once */
        if (once)
        {
            out_world.one = 1;
            out_world.zero = 0;
            out_world.Bit5 = 0;
            out_world.Bit6 = 0;
            out_world.Bit7 = 0;
            once = true;
        }

        /* Update the local out_word copy of the JTAG signal to the new value. */
        if (p == TMS)
            out_world.TMS = val;
        if (p == TDI)
            out_world.TDI = val;
        if (p == TCK)
        {
            out_world.TCK = val;
            //    (void) _outp( (unsigned short) (base_port + 0), out_word.value );
            NativeMethods.Out32((short)(base_port + DATA_OFFSET), (short)PrimitiveConversion.ToLong(out_world));

            /* To save HW write cycles, this example only writes the local copy
               of the JTAG signal values to the HW register when TCK changes. */
        }
    }

    public static byte readTDOBit()
    {
        /* Old Win95 example that is similar to a GPIO register implementation.
           The old Win95 reads the hardware input register and extracts the TDO
           value from the bit within the register that is assigned to the
           physical JTAG TDO signal. 
           */
        short input = (short)NativeMethods.Inp32((short)(base_port + STATUS_OFFSET));
        byte byt0 = (byte)(input & 255);
        byte byt1 = (byte)(input >> 8);
        //Console.WriteLine("--{0}--", input);
        //FromShort(input, out byt0, out byt1);
        //Console.WriteLine("-{0}-{1}-", byt0, byt1);
        if ((byt0 & 0x10) != 0x10)
        {
            return (0x01);
        }
        /* You must return the current value of the JTAG TDO signal. */
        return (0x00);
    }

    public static void pulseClock()
    {
        setPort(TCK, 0);  /* set the TCK port to low  */
        setPort(TCK, 1);  /* set the TCK port to high */
    }

    public static void waitTime(long microsec)
    {
        setPort(TCK, 0);
        //Thread.Sleep((int)microsec / 1000);
    }

    public static UInt32 IsInpOutOpen()
    {
        UInt32 op = 0;
        try
        {
            return NativeMethods.IsInpOutDriverOpen();
        }
        catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
        return op;
    }
}
