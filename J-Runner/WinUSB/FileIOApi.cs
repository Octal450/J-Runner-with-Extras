using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace WinUsb
{
    ///  <summary>
    ///  API declarations relating to file I/O (and used by WinUsb).
    ///  </summary>

    sealed internal class FileIO
    {
        internal const Int32 FILE_ATTRIBUTE_NORMAL = 0X80;
        internal const Int32 FILE_FLAG_OVERLAPPED = 0X40000000;
        internal const Int32 FILE_SHARE_READ = 1;
        internal const Int32 FILE_SHARE_WRITE = 2;
        internal const UInt32 GENERIC_READ = 0X80000000;
        internal const UInt32 GENERIC_WRITE = 0X40000000;
        internal const Int32 INVALID_HANDLE_VALUE = -1;
        internal const Int32 OPEN_EXISTING = 3;

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern SafeFileHandle CreateFile(String lpFileName, UInt32 dwDesiredAccess, Int32 dwShareMode, IntPtr lpSecurityAttributes, Int32 dwCreationDisposition, Int32 dwFlagsAndAttributes, Int32 hTemplateFile);
    }

}
