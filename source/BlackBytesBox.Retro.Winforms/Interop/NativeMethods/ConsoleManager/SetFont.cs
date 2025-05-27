using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
        public partial class ConsoleManager
        {

            /// <summary>
            /// Built-in TrueType console fonts (must be installed, fixed-width, and registry-listed).
            /// </summary>
            public enum ConsoleFont
            {
                /// <summary>“Lucida Console” face (monospaced TrueType).</summary>
                LucidaConsole,

                /// <summary>“Consolas” face (monospaced TrueType).</summary>
                Consolas
            }

            /// <summary>
            /// Sets the console’s font to one of the built-in TrueType faces.
            /// </summary>
            /// <param name="font">The named console font to use.</param>
            public static void SetFont(ConsoleFont font)
            {
                // Map enum to exact registry face name
                string faceName = font switch
                {
                    ConsoleFont.LucidaConsole => "Lucida Console",
                    ConsoleFont.Consolas => "Consolas",
                    _ => throw new ArgumentOutOfRangeException(nameof(font), font, null)
                };

                SetFont(faceName);
            }

            /// <summary>
            /// Attempts to set the legacy console’s font to the specified TrueType face.
            /// </summary>
            /// <param name="faceName">
            /// The exact font name as registered under 
            /// <c>HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Console\TrueTypeFont</c>
            /// (e.g. “Noto Sans Mono CJK JP”). That font must be installed, fixed-pitch, and TrueType.
            /// </param>
            /// <exception cref="InvalidOperationException">
            /// Thrown if the console font cannot be set. The message contains the Win32 error code.
            /// </exception>
            /// <remarks>
            /// <para>
            /// Only fonts listed under the “TrueTypeFont” registry key and marked fixed-width TrueType 
            /// can be selected. You must add your desired CJK or emoji-capable font there beforehand.
            /// </para>
            /// <para>
            /// This API cannot register fonts in the registry for you—it only switches among those already registered.
            /// </para>
            /// </remarks>
            /// <example>
            /// <code>
            /// ConsoleManager.EnsureConsole();
            /// ConsoleManager.SetFont("Noto Sans Mono CJK JP");
            /// ConsoleManager.EnableUtf8();
            /// Console.WriteLine("漢 😊");
            /// </code>
            /// </example>
            public static void SetFont(string faceName)
            {
                var info = new CONSOLE_FONT_INFO_EX
                {
                    cbSize = (uint)Marshal.SizeOf(typeof(CONSOLE_FONT_INFO_EX)),
                    dwFontSize = new COORD { X = 0, Y = 16 },  // Height=16 pixels, width auto
                    FontFamily = FF_DONTCARE | FIXED_PITCH | TMPF_TRUETYPE,
                    FontWeight = 400,                          // Normal weight
                    FaceName = faceName
                };

                IntPtr console = GetStdHandle(STD_OUTPUT_HANDLE);
                if (console == IntPtr.Zero || console == new IntPtr(-1))
                {
                    int err = Marshal.GetLastWin32Error();
                    throw new InvalidOperationException(
                        $"Invalid console output handle (Win32 error {err}).");
                }

                if (!SetCurrentConsoleFontEx(console, false, ref info))
                {
                    int err = Marshal.GetLastWin32Error();
                    throw new InvalidOperationException(
                        $"Failed to set console font to '{faceName}' (Win32 error {err}).");
                }
            }
        }
    }
}