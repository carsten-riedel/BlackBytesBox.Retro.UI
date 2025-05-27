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
            /// Re-enables (un-grays) the Close button in the console’s system menu.
            /// </summary>
            /// <returns>
            /// True if the Close item was successfully re-enabled; false on error.
            /// </returns>
            public static bool EnableCloseButton()
            {
                IntPtr hWnd = GetConsoleWindow();
                if (hWnd == IntPtr.Zero)
                    return false;

                IntPtr hMenu = GetSystemMenu(hWnd, false);
                if (hMenu == IntPtr.Zero)
                    return false;

                // Restore SC_CLOSE to enabled state :contentReference[oaicite:8]{index=8}
                uint prevState = EnableMenuItem(
                    hMenu,
                    SC_CLOSE,
                    MF_BYCOMMAND | MF_ENABLED
                );
                if (prevState == 0xFFFFFFFF)  // -1 = error fetching previous state :contentReference[oaicite:9]{index=9}
                    return false;

                return DrawMenuBar(hWnd);     // repaint title bar :contentReference[oaicite:10]{index=10}
            }
        }
    }
}