using System;
using System.Collections.Generic;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {

        public enum ConsoleCtrlEvent
        {
            CTRL_C = 0,
            CTRL_BREAK = 1,
            CTRL_CLOSE = 2,
            CTRL_LOGOFF = 5,
            CTRL_SHUTDOWN = 6
        }

        // Delegate for console events (close, logoff, shutdown…)
        public delegate bool ConsoleEventDelegate(ConsoleCtrlEvent ctrlType);
    }
}
