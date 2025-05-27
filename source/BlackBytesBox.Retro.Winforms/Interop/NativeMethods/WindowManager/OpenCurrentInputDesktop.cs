using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
        public partial class WindowManager
        {
            /// <summary>
            /// Opens the desktop that is currently receiving user input.
            /// </summary>
            /// <returns>
            /// A handle to the input desktop. Must be closed with <see cref="CloseDesktop"/> when finished.
            /// </returns>
            /// <exception cref="Win32Exception">Thrown if the call fails.</exception>
            public static IntPtr OpenCurrentInputDesktop()
            {
                IntPtr hDesk = OpenInputDesktop(
                    dwFlags: 0,
                    fInherit: false,
                    dwDesiredAccess: DESKTOP_READOBJECTS | DESKTOP_ENUMERATE | DESKTOP_WRITEOBJECTS);
                if (hDesk == IntPtr.Zero)
                    throw new Win32Exception(Marshal.GetLastWin32Error(),
                        "OpenInputDesktop failed");
                return hDesk;
            }
        }
    }
}
