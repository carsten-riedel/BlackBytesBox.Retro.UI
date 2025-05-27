using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
        private const uint DESKTOP_READOBJECTS = 0x0001;
        private const uint DESKTOP_ENUMERATE = 0x0040;
        private const uint DESKTOP_WRITEOBJECTS = 0x0080;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr OpenInputDesktop(uint dwFlags, bool fInherit, uint dwDesiredAccess);
    }
}
