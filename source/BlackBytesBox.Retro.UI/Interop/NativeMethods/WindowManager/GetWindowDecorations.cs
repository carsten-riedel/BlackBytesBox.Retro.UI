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
            /// Returns the non-client decoration sizes of a standard window on the current system:
            /// border width (left/right) and title-bar height (top).
            /// </summary>
            /// <remarks>
            /// <para>
            /// BorderWidth = SM_CXFRAME + SM_CXPADDEDBORDER  
            /// TitleBarHeight = SM_CYCAPTION + SM_CYFRAME + SM_CXPADDEDBORDER
            /// </para>
            /// <para>
            /// You can then offset your MoveAndResize by these values to place the *client* area
            /// exactly at screen (0,0).
            /// </para>
            /// </remarks>
            public static WindowDecorations GetWindowDecorations()
            {
                int frame = GetSystemMetrics(SM_CXFRAME);
                int padded = GetSystemMetrics(SM_CXPADDEDBORDER);
                int caption = GetSystemMetrics(SM_CYCAPTION);
                int borderWidth = frame + padded;
                int titleHeight = caption + frame + padded;
                return new WindowDecorations
                {
                    BorderWidth = borderWidth,
                    TitleBarHeight = titleHeight
                };
            }
        }
    }
}
