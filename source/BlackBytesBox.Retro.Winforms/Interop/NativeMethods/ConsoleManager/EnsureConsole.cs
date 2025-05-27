using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.Win32.SafeHandles;

namespace BlackBytesBox.Retro.UI.Interop
{
    public static partial class NativeMethods
    {
        public partial class ConsoleManager
        {
            // Keep these alive so they don't get finalized/disposed
            // Keep these alive for the process lifetime
            private static SafeFileHandle _consoleHandle;
            private static FileStream _consoleStream;
            private static StreamWriter _consoleWriter;

            private static StreamWriter _errWriter;
            private static StreamReader _inReader;

   /// <summary>
            /// Ensures the process has a console, then redirects 
            /// Console.In, Console.Out, and Console.Error to it.
            /// </summary>
            /// <remarks>
            /// <para>First attempts AttachConsole(–1); if that fails, calls AllocConsole().</para>
            /// <para>Then for each of STDIN/STDOUT/STDERR:
            ///   <list type="bullet">
            ///     <item>Retrieves the native handle</item>
            ///     <item>Checks it isn’t INVALID</item>
            ///     <item>Creates the appropriate StreamReader/Writer</item>
            ///     <item>Calls Console.SetIn/Out/Error</item>
            ///   </list>
            /// </para>
            /// <para>Any failure in redirecting one stream will be caught and logged,
            ///   but won’t prevent attempts on the others.</para>
            /// </remarks>
            /// <example>
            /// <code>
            /// ConsoleManager.EnsureConsole();
            /// Console.WriteLine("This is standard output");
            /// Console.Error.WriteLine("This is standard error");
            /// string line = Console.ReadLine();
            /// </code>
            /// </example>
            public static void EnsureConsole()
            {
                // 1) Attach or allocate
                if (GetConsoleWindow() == IntPtr.Zero)
                {
                    if (!AttachConsole(ATTACH_PARENT_PROCESS) && !AllocConsole())
                    {
                        int err = Marshal.GetLastWin32Error();
                        throw new InvalidOperationException(
                            $"Failed to attach or allocate console (Win32 error {err}).");
                    }
                }

                // 2) Redirect stdout
                if (_consoleWriter == null)
                {
                    try
                    {
                        IntPtr outHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                        if (outHandle == IntPtr.Zero || outHandle == new IntPtr(-1))
                            throw new IOException("Invalid STDOUT handle.");

                        _consoleHandle  = new SafeFileHandle(outHandle, ownsHandle: false);
                        _consoleStream  = new FileStream(_consoleHandle, FileAccess.Write);
                        _consoleWriter  = new StreamWriter(_consoleStream) { AutoFlush = true };
                        Console.SetOut(_consoleWriter);
                    }
                    catch (Exception ex)
                    {
                        // you may log ex here if desired
                    }
                }

                // 3) Redirect stderr
                if (_errWriter == null)
                {
                    try
                    {
                        IntPtr errHandle = GetStdHandle(STD_ERROR_HANDLE);
                        if (errHandle == IntPtr.Zero || errHandle == new IntPtr(-1))
                            throw new IOException("Invalid STDERR handle.");

                        var safeErrHandle = new SafeFileHandle(errHandle, ownsHandle: false);
                        var errStream     = new FileStream(safeErrHandle, FileAccess.Write);
                        _errWriter        = new StreamWriter(errStream) { AutoFlush = true };
                        Console.SetError(_errWriter);
                    }
                    catch (Exception ex)
                    {
                        // you may log ex here if desired
                    }
                }

                // 4) Redirect stdin
                if (_inReader == null)
                {
                    try
                    {
                        IntPtr inHandle = GetStdHandle(STD_INPUT_HANDLE);
                        if (inHandle == IntPtr.Zero || inHandle == new IntPtr(-1))
                            throw new IOException("Invalid STDIN handle.");

                        var safeInHandle = new SafeFileHandle(inHandle, ownsHandle: false);
                        var inStream     = new FileStream(safeInHandle, FileAccess.Read);
                        _inReader        = new StreamReader(inStream);
                        Console.SetIn(_inReader);
                    }
                    catch (Exception ex)
                    {
                        // you may log ex here if desired
                    }
                }
            }
        }
    }
}