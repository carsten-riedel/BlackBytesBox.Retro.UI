using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using BlackBytesBox.Retro.UI.Interop;

namespace BlackBytesBox.Retro.UI.Test
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //NativeMethods.ConsoleManager.EnsureConsole();
            //NativeMethods.ConsoleManager.DisableCloseButton();
            //NativeMethods.ConsoleManager.DisableQuickEdit();
            //NativeMethods.ConsoleManager.SetFont(NativeMethods.ConsoleManager.ConsoleFont.Consolas);
            //NativeMethods.ConsoleManager.EnableUtf8();
            //Console.WriteLine("Console initialized with custom settings.");

            //var deco = NativeMethods.WindowManager.GetWindowDecorations();
            //NativeMethods.ConsoleManager.MoveAndResize(x: 20 - deco.BorderWidth, 20);

            //Console.WriteLine("Unicode ✔ Ω 漢 😊");
            //Console.WriteLine("Hello, World!");
            
            //Thread.Sleep(3000);

            //IntPtr desktopHwnd = NativeMethods.GetDesktopWindow();
            //var semiTransBrush = new SolidBrush(Color.FromArgb(128, 255, 0, 0));
            //using (var g = Graphics.FromHwnd(desktopHwnd))
            //using (var pen = new Pen(Color.Red, 1))
            //{
            //    g.FillRectangle(semiTransBrush, 0, 0, 300, 200);
            //}

            //IntPtr hDesk = IntPtr.Zero;
            //try
            //{
            //    // 1) Open the desktop receiving user input
            //    hDesk = NativeMethods.WindowManager.OpenCurrentInputDesktop();
            //    Console.WriteLine($"Opened input desktop: 0x{hDesk.ToInt64():X}");

            //    // 2) Enumerate windows on that desktop
            //    var windows = NativeMethods.WindowManager.EnumerateWindowsOnDesktop(hDesk);
            //    Console.WriteLine("Top-level windows on input desktop:");
            //    foreach (var hwnd in windows)
            //    {
            //        Console.WriteLine($"  HWND = 0x{hwnd.ToInt64():X} {NativeMethods.WindowManager.GetWindowTitle(hwnd)}");
            //    }

            //}
            //catch (Win32Exception ex)
            //{
            //    Console.Error.WriteLine($"Error: {ex.Message} (Win32 code {ex.NativeErrorCode})");
            //}
            //finally
            //{
            //    // 3) Always close the desktop handle when done
            //    if (hDesk != IntPtr.Zero)
            //    {
            //        NativeMethods.WindowManager.CloseInputDesktop(hDesk);
            //        Console.WriteLine("Closed input desktop handle.");
            //    }
            //}

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}