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
            /// Closes a desktop handle previously opened by <see cref="OpenCurrentInputDesktop"/>.
            /// </summary>
            /// <param name="hDesktop">The desktop handle to close.</param>
            /// <returns><c>true</c> on success; <c>false</c> on failure.</returns>
            public static bool CloseInputDesktop(IntPtr hDesktop)
            {
                return CloseDesktop(hDesktop);
            }
        }
    }
}
