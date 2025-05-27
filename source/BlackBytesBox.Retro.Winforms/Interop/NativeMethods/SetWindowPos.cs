using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
        private const int SWP_SHOWWINDOW = 0x0040;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;
        private static readonly IntPtr HWND_TOP = IntPtr.Zero;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd,IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,uint uFlags);
    }
}