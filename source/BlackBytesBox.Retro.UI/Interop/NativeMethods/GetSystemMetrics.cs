using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Thickness, in pixels, of the non‐client area around a window:
        /// the left/right border width and top border+title‐bar height.
        /// </summary>
        public struct WindowDecorations
        {
            /// <summary>The width of the left (and right) window border.</summary>
            public int BorderWidth;
            /// <summary>The height of the top border plus the title bar.</summary>
            public int TitleBarHeight;
        }

        private const int SM_CXFRAME = 32;
        private const int SM_CYFRAME = 33;
        private const int SM_CXPADDEDBORDER = 92;
        private const int SM_CYCAPTION = 4;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetSystemMetrics(int nIndex);
    }
}