using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// &lt;placeholder for actual implementation&gt;
        /// </summary>
        /// <remarks>
        /// Wraps the Win32 MessageBox function from user32.dll.
        /// </remarks>
        /// <param name="hWnd">Handle to the owner window (use IntPtr.Zero for none).</param>
        /// <param name="lpText">The message to display.</param>
        /// <param name="lpCaption">The dialog caption.</param>
        /// <param name="uType">Message box style flags (e.g. MB_OK | MB_ICONINFORMATION).</param>
        /// <returns>
        /// An integer indicating which button the user clicked.
        /// </returns>
        /// <example>
        /// <code>
        /// // Show an information box with OK button
        /// int result = NativeMethods.MessageBox(
        ///     IntPtr.Zero,
        ///     "Hello from native code!",
        ///     "Interop Demo",
        ///     0x00000000 /* MB_OK */);
        /// </code>
        /// </example>

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int MessageBox(
            IntPtr hWnd,
            string lpText,
            string lpCaption,
            uint uType);
    }
}