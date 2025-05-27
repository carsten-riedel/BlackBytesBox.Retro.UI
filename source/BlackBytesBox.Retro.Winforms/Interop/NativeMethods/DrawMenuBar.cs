using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Redraws the menu bar of the specified window.
        /// </summary>
        /// <param name="hWnd">Handle to the window whose menu bar is to be redrawn.</param>
        /// <returns>
        /// <c>true</c> on success; <c>false</c> on failure.
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DrawMenuBar(IntPtr hWnd);
    }
}
