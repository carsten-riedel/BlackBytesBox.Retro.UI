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
        /// Retrieves the window handle of the console associated with the calling process.
        /// </remarks>
        /// <returns>
        /// A non-zero handle if a console exists; IntPtr.Zero if none is allocated.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = false)]
        public static extern IntPtr GetConsoleWindow();
    }
}
