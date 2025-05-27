using System;
using System.Collections.Generic;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Delegate for the desktop-window enumeration callback.
        /// </summary>
        /// <param name="hwnd">Handle of a top-level window on the desktop.</param>
        /// <param name="lParam">User-supplied data (unused here).</param>
        /// <returns><c>true</c> to continue enumeration; <c>false</c> to stop.</returns>
        private delegate bool EnumDesktopWindowsDelegate(IntPtr hwnd, IntPtr lParam);
    }
}
