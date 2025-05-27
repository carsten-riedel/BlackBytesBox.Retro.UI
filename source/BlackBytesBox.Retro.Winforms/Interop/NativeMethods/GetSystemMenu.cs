using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Retrieves a handle to the specified window’s system menu.
        /// </summary>
        /// <param name="hWnd">Handle to the window whose system menu is to be retrieved.</param>
        /// <param name="bRevert">If <c>true</c>, resets the menu to the default state.</param>
        /// <returns>
        /// Handle to the system menu; <see cref="IntPtr.Zero"/> on failure.
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
    }
}
