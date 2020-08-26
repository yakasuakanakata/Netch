using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace NetSpeedMonitor.Utils
{
    public static class ProcessInfoTool
    {
        #region 程序集信息
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.LPStr)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string szTypeName;
            public SHFILEINFO(bool b)
            {
                hIcon = IntPtr.Zero;
                iIcon = 0;
                dwAttributes = 0u;
                szDisplayName = string.Empty;
                szTypeName = string.Empty;
            }
        }

        private enum SHGFI
        {
            SmallIcon = 1,
            LargeIcon = 0,
            Icon = 256,
            DisplayName = 512,
            Typename = 1024,
            SysIconIndex = 16384,
            UseFileAttributes = 16
        }
        #endregion
    }
}
