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
            NativeMethods.ConsoleManager.EnsureConsole();
            NativeMethods.ConsoleManager.DisableCloseButton();
            NativeMethods.ConsoleManager.DisableQuickEdit();
            NativeMethods.ConsoleManager.SetFont(NativeMethods.ConsoleManager.ConsoleFont.Consolas);
            NativeMethods.ConsoleManager.EnableUtf8();
            Console.WriteLine("Console initialized with custom settings.");

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}