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
            /// Disables Quick Edit mode on the console’s input buffer (mouse selects no longer pause the app).
            /// </summary>
            /// <returns>
            /// True if the mode was successfully cleared or was already cleared; false on error.
            /// </returns>
            public static bool DisableQuickEdit()
            {
                IntPtr hIn = GetStdHandle(STD_INPUT_HANDLE);
                if (hIn == IntPtr.Zero || hIn == new IntPtr(-1))
                    return false;

                if (!GetConsoleMode(hIn, out uint mode))
                    return false;

                // Clear Quick Edit, ensure Extended Flags
                uint newMode = (mode & ~ENABLE_QUICK_EDIT) | ENABLE_EXTENDED_FLAGS;
                if (!SetConsoleMode(hIn, newMode))
                    return false;

                return true;
            }
        }
    }
}