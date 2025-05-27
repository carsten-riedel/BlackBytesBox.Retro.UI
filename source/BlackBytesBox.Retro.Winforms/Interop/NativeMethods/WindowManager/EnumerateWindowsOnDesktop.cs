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
            /// Enumerates the top-level windows on the specified desktop.
            /// </summary>
            /// <param name="hDesktop">Handle returned by <see cref="OpenCurrentInputDesktop"/>.</param>
            /// <returns>A list of window handles (HWNDs) on that desktop.</returns>
            /// <exception cref="Win32Exception">Thrown if enumeration fails.</exception>
            public static IList<IntPtr> EnumerateWindowsOnDesktop(IntPtr hDesktop)
            {
                var windows = new List<IntPtr>();
                bool success = EnumDesktopWindows(
                    hDesktop,
                    (hwnd, lParam) =>
                    {
                        windows.Add(hwnd);
                        return true; // continue enumeration
                    },
                    IntPtr.Zero);
                if (!success)
                    throw new Win32Exception(Marshal.GetLastWin32Error(),
                        "EnumDesktopWindows failed");
                return windows;
            }
        }
    }
}
