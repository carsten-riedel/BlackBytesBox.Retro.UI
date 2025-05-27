using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
  
            [DllImport("user32.dll", SetLastError = true)]
            private static extern bool GetWindowRect(IntPtr hWnd,out RECT lpRect);

            [StructLayout(LayoutKind.Sequential)]
            private struct RECT
            {
                public int Left, Top, Right, Bottom;
            }
        
    }
}