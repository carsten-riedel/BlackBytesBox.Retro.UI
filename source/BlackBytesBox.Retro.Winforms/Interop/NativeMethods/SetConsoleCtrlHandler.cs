using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate handler, bool add);

    }
}
