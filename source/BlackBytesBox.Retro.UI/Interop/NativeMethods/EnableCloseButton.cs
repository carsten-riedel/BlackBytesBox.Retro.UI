using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
        private const uint MF_ENABLED = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;
        private const uint MF_DISABLED = 0x00000002;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
    }
}
