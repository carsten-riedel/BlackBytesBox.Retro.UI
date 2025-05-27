using System;
using System.Collections.Generic;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
        public partial class WindowManager
        {
            /// <summary>
            /// Retrieves the window title text for the specified HWND.
            /// </summary>
            /// <param name="hWnd">Handle of the window.</param>
            /// <returns>
            /// The window’s title bar text, or <c>String.Empty</c> if none or on error.
            /// </returns>
            public static string GetWindowTitle(IntPtr hWnd)
            {
                // Get required buffer length (in chars)
                int length = GetWindowTextLength(hWnd);
                if (length <= 0)
                    return String.Empty;

                // Allocate buffer (length + 1 for null terminator)
                var sb = new StringBuilder(length + 1);

                // Copy the text into our buffer
                int copied = GetWindowText(hWnd, sb, sb.Capacity);
                if (copied <= 0)
                    return String.Empty;

                return sb.ToString();
            }
        }
    }
}
