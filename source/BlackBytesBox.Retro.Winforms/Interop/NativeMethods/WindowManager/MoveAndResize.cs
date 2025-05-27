using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
        public partial class WindowManager
        {
            /// <summary>
            /// Attempts to move and/or resize the specified window.  
            /// Any parameter left <c>null</c> preserves its current value.
            /// </summary>
            /// <param name="hWnd">Handle of the window to move/resize.</param>
            /// <param name="x">
            /// New left coordinate in pixels; if <c>null</c>, keeps the existing X.</param>
            /// <param name="y">
            /// New top coordinate in pixels; if <c>null</c>, keeps the existing Y.</param>
            /// <param name="width">
            /// New width in pixels; if <c>null</c>, keeps the existing width.</param>
            /// <param name="height">
            /// New height in pixels; if <c>null</c>, keeps the existing height.</param>
            /// <returns>
            /// <c>true</c> if the window was moved/resized successfully; otherwise <c>false</c>.</returns>
            /// <example>
            /// <code>
            /// // Move only:
            /// bool moved = WindowManager.MoveAndResize(hwnd, x: 0, y: 0);
            ///
            /// // Resize only:
            /// bool resized = WindowManager.MoveAndResize(hwnd, width: 800, height: 600);
            ///
            /// // Move and resize:
            /// bool ok = WindowManager.MoveAndResize(hwnd, 50, 50, 1024, 768);
            /// </code>
            /// </example>
            public static bool MoveAndResize(
                IntPtr? hWnd,
                int? x = null,
                int? y = null,
                int? width = null,
                int? height = null)
            {
                if (hWnd == null || hWnd == IntPtr.Zero)
                    return false;

                if (!GetWindowRect(hWnd.Value, out RECT rect))
                    return false;

                int newX = x ?? rect.Left;
                int newY = y ?? rect.Top;
                int newW = width ?? (rect.Right - rect.Left);
                int newH = height ?? (rect.Bottom - rect.Top);

                if (!SetWindowPos(
                    hWnd.Value,
                    HWND_TOP,
                    newX,
                    newY,
                    newW,
                    newH,
                    SWP_NOZORDER | SWP_SHOWWINDOW))
                {
                    return false;
                }

                return true;
            }
        }
    }
}
