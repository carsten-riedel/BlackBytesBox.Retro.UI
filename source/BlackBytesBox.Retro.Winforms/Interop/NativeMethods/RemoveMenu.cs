using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {

        private const uint SC_CLOSE = 0xF060;
        private const uint MF_BYCOMMAND = 0x00000000;

        /// <summary>
        /// Deletes an item from the specified menu.
        /// </summary>
        /// <param name="hMenu">Handle to the menu.</param>
        /// <param name="uPosition">Identifier or position of the menu item.</param>
        /// <param name="uFlags">Flags specifying how to interpret <paramref name="uPosition"/>.</param>
        /// <returns>
        /// <c>true</c> on success; <c>false</c> on failure.
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);
    }
}
