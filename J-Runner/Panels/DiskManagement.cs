using System;

namespace DiskManagement
{
    using Microsoft.Win32.SafeHandles;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using DWORD = UInt32;
    using HANDLE = IntPtr;
    using LARGE_INTEGER = Int64;
    using LPCTSTR = String;
    using LPOVERLAPPED = IntPtr;
    using LPSECURITY_ATTRIBUTES = IntPtr;
    using LPVOID = IntPtr;

    public static partial class IoCtl /* methods */
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern SafeFileHandle CreateFile(
            LPCTSTR lpFileName,
            DWORD dwDesiredAccess,
            DWORD dwShareMode,
            LPSECURITY_ATTRIBUTES lpSecurityAttributes,
            DWORD dwCreationDisposition,
            DWORD dwFlagsAndAttributes,
            HANDLE hTemplateFile
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern DWORD DeviceIoControl(
            SafeFileHandle hDevice,
            DWORD dwIoControlCode,
            LPVOID lpInBuffer,
            DWORD nInBufferSize,
            LPVOID lpOutBuffer,
            int nOutBufferSize,
            ref DWORD lpBytesReturned,
            LPOVERLAPPED lpOverlapped
            );

        static DWORD CTL_CODE(DWORD DeviceType, DWORD Function, DWORD Method, DWORD Access)
        {
            return (((DeviceType) << 16) | ((Access) << 14) | ((Function) << 2) | (Method));
        }

        public static void Execute<T>(
            ref T x,
            DWORD dwIoControlCode,
            LPCTSTR lpFileName,
            DWORD dwDesiredAccess = GENERIC_READ,
            DWORD dwShareMode = FILE_SHARE_WRITE | FILE_SHARE_READ,
            LPSECURITY_ATTRIBUTES lpSecurityAttributes = default(LPSECURITY_ATTRIBUTES),
            DWORD dwCreationDisposition = OPEN_EXISTING,
            DWORD dwFlagsAndAttributes = 0,
            HANDLE hTemplateFile = default(IntPtr)
            )
        {
            using (
                var hDevice =
                    CreateFile(
                        lpFileName,
                        dwDesiredAccess, dwShareMode,
                        lpSecurityAttributes,
                        dwCreationDisposition, dwFlagsAndAttributes,
                        hTemplateFile
                        )
                )
            {
                if (null == hDevice || hDevice.IsInvalid)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                var nOutBufferSize = Marshal.SizeOf(typeof(T));
                var lpOutBuffer = Marshal.AllocHGlobal(nOutBufferSize);
                var lpBytesReturned = default(DWORD);
                var NULL = IntPtr.Zero;

                var result =
                    DeviceIoControl(
                        hDevice, dwIoControlCode,
                        NULL, 0,
                        lpOutBuffer, nOutBufferSize,
                        ref lpBytesReturned, NULL
                        );

                if (0 == result)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                x = (T)Marshal.PtrToStructure(lpOutBuffer, typeof(T));
                Marshal.FreeHGlobal(lpOutBuffer);
            }
        }
    }

    public enum MEDIA_TYPE : int
    {
        Unknown = 0,
        F5_1Pt2_512 = 1,
        F3_1Pt44_512 = 2,
        F3_2Pt88_512 = 3,
        F3_20Pt8_512 = 4,
        F3_720_512 = 5,
        F5_360_512 = 6,
        F5_320_512 = 7,
        F5_320_1024 = 8,
        F5_180_512 = 9,
        F5_160_512 = 10,
        RemovableMedia = 11,
        FixedMedia = 12,
        F3_120M_512 = 13,
        F3_640_512 = 14,
        F5_640_512 = 15,
        F5_720_512 = 16,
        F3_1Pt2_512 = 17,
        F3_1Pt23_1024 = 18,
        F5_1Pt23_1024 = 19,
        F3_128Mb_512 = 20,
        F3_230Mb_512 = 21,
        F8_256_128 = 22,
        F3_200Mb_512 = 23,
        F3_240M_512 = 24,
        F3_32M_512 = 25
    }

    partial class DiskGeometry /* structures */
    {
        [StructLayout(LayoutKind.Sequential)]
        struct DISK_GEOMETRY
        {
            internal LARGE_INTEGER Cylinders;
            internal MEDIA_TYPE MediaType;
            internal DWORD TracksPerCylinder;
            internal DWORD SectorsPerTrack;
            internal DWORD BytesPerSector;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DISK_GEOMETRY_EX
        {
            internal DISK_GEOMETRY Geometry;
            internal LARGE_INTEGER DiskSize;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            internal byte[] Data;
        }
    }

    partial class DiskGeometry /* properties and fields */
    {
        public MEDIA_TYPE MediaType
        {
            get
            {
                return m_Geometry.MediaType;
            }
        }

        public String MediaTypeName
        {
            get
            {
                return Enum.GetName(typeof(MEDIA_TYPE), this.MediaType);
            }
        }

        public override long Cylinder
        {
            get
            {
                return m_Geometry.Cylinders;
            }
        }

        public override uint Head
        {
            get
            {
                return m_Geometry.TracksPerCylinder;
            }
        }

        public override uint Sector
        {
            get
            {
                return m_Geometry.SectorsPerTrack;
            }
        }

        public DWORD BytesPerSector
        {
            get
            {
                return m_Geometry.BytesPerSector;
            }
        }

        public long DiskSize
        {
            get
            {
                return m_DiskSize;
            }
        }

        public long MaximumLinearAddress
        {
            get
            {
                return m_MaximumLinearAddress;
            }
        }

        public CubicAddress MaximumCubicAddress
        {
            get
            {
                return m_MaximumCubicAddress;
            }
        }

        public DWORD BytesPerCylinder
        {
            get
            {
                return m_BytesPerCylinder;
            }
        }

        CubicAddress m_MaximumCubicAddress;
        long m_MaximumLinearAddress;
        DWORD m_BytesPerCylinder;
        LARGE_INTEGER m_DiskSize;
        DISK_GEOMETRY m_Geometry;
    }

    partial class IoCtl /* constants */
    {
        public const DWORD
            DISK_BASE = 0x00000007,
            METHOD_BUFFERED = 0,
            FILE_ANY_ACCESS = 0;

        public const DWORD
            GENERIC_READ = 0x80000000,
            FILE_SHARE_WRITE = 0x2,
            FILE_SHARE_READ = 0x1,
            OPEN_EXISTING = 0x3;

        public static readonly DWORD DISK_GET_DRIVE_GEOMETRY_EX =
            IoCtl.CTL_CODE(DISK_BASE, 0x0028, METHOD_BUFFERED, FILE_ANY_ACCESS);

        public static readonly DWORD DISK_GET_DRIVE_GEOMETRY =
            IoCtl.CTL_CODE(DISK_BASE, 0, METHOD_BUFFERED, FILE_ANY_ACCESS);
    }

    public partial class CubicAddress
    {
        public static CubicAddress Transform(long linearAddress, CubicAddress geometry)
        {
            var cubicAddress = new CubicAddress();
            var sectorsPerCylinder = geometry.Sector * geometry.Head;
            long remainder;
            cubicAddress.Cylinder = Math.DivRem(linearAddress, sectorsPerCylinder, out remainder);
            cubicAddress.Head = (uint)Math.DivRem(remainder, geometry.Sector, out remainder);
            cubicAddress.Sector = 1 + (uint)remainder;
            return cubicAddress;
        }

        public virtual long Cylinder
        {
            get;
            set;
        }

        public virtual uint Head
        {
            get;
            set;
        }

        public virtual uint Sector
        {
            get;
            set;
        }
    }

    public partial class DiskGeometry : CubicAddress
    {
        internal static void ThrowIfDiskSizeOutOfIntegrity(long remainder)
        {
            if (0 != remainder)
            {
                var message = "DiskSize is not an integral multiple of a sector size";
                throw new ArithmeticException(message);
            }
        }

        public static DiskGeometry FromDevice(String deviceName)
        {
            try
            {
                return new DiskGeometry(deviceName);
            }
            catch (Exception) { }
            return null;
        }

        DiskGeometry(String deviceName)
        {
            var x = new DISK_GEOMETRY_EX();
            IoCtl.Execute(ref x, IoCtl.DISK_GET_DRIVE_GEOMETRY_EX, deviceName);
            m_DiskSize = x.DiskSize;
            m_Geometry = x.Geometry;

            long remainder;
            m_MaximumLinearAddress = Math.DivRem(DiskSize, BytesPerSector, out remainder) - 1;
            ThrowIfDiskSizeOutOfIntegrity(remainder);

            m_BytesPerCylinder = BytesPerSector * Sector * Head;
            m_MaximumCubicAddress = DiskGeometry.Transform(m_MaximumLinearAddress, this);
        }
    }
}
