using System;
using System.Collections.Generic;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
        public partial class ConsoleManager
        {
            /// <summary>
            /// Attempts to move and/or resize the console window.
            /// Returns <c>true</c> if successful, or <c>false</c> if the operation failed.
            /// </summary>
            /// <param name="x">
            /// New left coordinate in pixels; if &lt;0, preserves the existing X position.</param>
            /// <param name="y">
            /// New top coordinate in pixels; if &lt;0, preserves the existing Y position.</param>
            /// <param name="width">
            /// New width in pixels; if &lt;0, preserves the existing width.</param>
            /// <param name="height">
            /// New height in pixels; if &lt;0, preserves the existing height.</param>
            /// <returns>
            /// <c>true</c> if the console was repositioned/resized successfully; otherwise <c>false</c>.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown if no console window exists.</exception>
            /// <example>
            /// <code>
            /// // Only move to (0,0), keep size
            /// if (!ConsoleManager.MoveAndResize(0, 0))
            ///     Console.WriteLine("Could not move console.");
            ///
            /// // Resize only
            /// bool ok = ConsoleManager.MoveAndResize(width: 1024, height: 768);
            /// </code>
            /// </example>
            public static bool MoveAndResize(
                int? x = null,
                int? y = null,
                int? width = null,
                int? height = null)
            {
                // 1) Get the console window handle
                IntPtr hwnd = NativeMethods.GetConsoleWindow();
                if (hwnd == IntPtr.Zero)
                    return false;

                // 2) Delegate to the WindowManager overload that accepts nullable values
                //    It returns true on success, false on failure.
                return NativeMethods.WindowManager.MoveAndResize(
                    hwnd,
                    x, y,
                    width, height
                );
            }
        }
    }
}