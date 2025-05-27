using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {

        public partial class ConsoleManager
        {
            /// <summary>
            /// Enables full Unicode output in the console by switching both native and managed streams
            /// to UTF-8, so that any Unicode character (✓, Ω, 漢, 😊, etc.) renders correctly.
            /// </summary>
            /// <remarks>
            /// <para>Call <c>EnableUtf8()</c> <b>after</b> <see cref="EnsureConsole"/> so that a console window exists.</para>
            /// <para>Internally this sets the native input and output code pages (<c>SetConsoleCP</c> / <c>SetConsoleOutputCP</c>)
            /// to UTF-8, then configures <c>Console.InputEncoding</c> and <c>Console.OutputEncoding</c> to <c>Encoding.UTF8</c>.</para>
            /// </remarks>
            /// <example>
            /// <code>
            /// ConsoleManager.EnsureConsole();
            /// ConsoleManager.EnableUtf8();
            /// Console.WriteLine("Unicode test: ✓ Ω 漢 😊");
            public static void EnableUtf8()
            {
                // Native: switch output code page to UTF-8
                if (!SetConsoleOutputCP(CP_UTF8))
                {
                    int err = Marshal.GetLastWin32Error();
                    throw new InvalidOperationException(
                        $"Failed to set console output code page to UTF-8 (Win32 error {err}).");
                }

                // Native: switch input code page to UTF-8
                if (!SetConsoleCP(CP_UTF8))
                {
                    int err = Marshal.GetLastWin32Error();
                    throw new InvalidOperationException(
                        $"Failed to set console input code page to UTF-8 (Win32 error {err}).");
                }

                // Managed: switch .NET’s encodings
                Console.OutputEncoding = Encoding.UTF8;
                Console.InputEncoding = Encoding.UTF8;
            }
        }
    }
}
