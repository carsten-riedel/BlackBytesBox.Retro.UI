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
            /// Disables (grays out) the Close button in the console’s system menu.
            /// </summary>
            public static void DisableCloseButton()
            {
                IntPtr hWnd = GetConsoleWindow();
                if (hWnd == IntPtr.Zero)
                    return;

                IntPtr hMenu = GetSystemMenu(hWnd, false);
                if (hMenu == IntPtr.Zero)
                    return;

                // Gray out SC_CLOSE without removing it :contentReference[oaicite:6]{index=6}
                EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
                DrawMenuBar(hWnd);                              // repaint title bar :contentReference[oaicite:7]{index=7}
            }
        }
        
    }
}