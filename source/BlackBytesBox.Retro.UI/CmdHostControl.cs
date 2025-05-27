using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using BlackBytesBox.Retro.UI.Interop;

namespace BlackBytesBox.Retro.UI
{
    /// <summary>
    /// A WinForms control that hosts an interactive cmd.exe session.
    /// </summary>
    [ToolboxItem(true)]
    public class CmdHostControl : UserControl
    {
        /// <summary>
        /// Gets a value indicating whether the control is currently in design mode.
        /// </summary>
        /// <remarks>
        /// Combines multiple checks:
        ///  - the Control.DesignMode property,
        ///  - this.Site.DesignMode (if Site is non-null),
        ///  - LicenseManager.UsageMode == Designtime,
        ///  - and process-name scans for “devenv” or “xdesproc”.
        /// </remarks>
        private bool IsInDesignMode
        {
            get
            {
                // 1) WinForms’ own DesignMode flag
                if (this.DesignMode)
                    return true;

                // 2) The Site’s DesignMode (designer wiring)
                if (this.Site != null && this.Site.DesignMode)
                    return true;

                // 3) LicenseManager usage-mode check
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                    return true;

                // 4) Process-name scan for VS designers
                Process proc = Process.GetCurrentProcess();
                string name = proc.ProcessName.ToLower();       // no ToLowerInvariant in .NET 1.0

                if (name.IndexOf("devenv") != -1)     // in-VS designer
                    return true;
                if (name.IndexOf("xdesproc") != -1)   // VS2022’s out-of-proc host
                    return true;

                return false;
            }
        }

        private delegate void AppendDelegate(string text);

        private TextBox outputTextBox;
        private TextBox inputTextBox;
        private Process process;
        private Thread outputThread;
        private Thread errorThread;
        private volatile bool isDisposing = false;

        /// <summary>
        /// Initializes a new instance of the CmdHostControl class and starts the shell.
        /// </summary>
        /// <remarks>
        /// Creates UI and launches cmd.exe in interactive mode.
        /// </remarks>
        /// <example>
        /// <code>
        /// var shell = new CmdHostControl();
        /// shell.SendCommand("dir");
        /// </code>
        /// </example>
        public CmdHostControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Once the WinForms handle exists, start the shell – but only if we're not in design mode.
        /// </summary>
        /// <param name="e">Event args</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // At design-time (in Visual Studio’s designer), DesignMode==true.
            // At runtime, DesignMode==false and we can wire up the console.
            if (!IsInDesignMode)
            {
                // switch back to ANSI for child cmd.exe
                RestoreOemConsolePage();
                StartShell();
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleOutputCP(uint wCodePageID);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleCP(uint wCodePageID);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetOEMCP();

        /// <summary>
        /// Ensures a console exists and switches its input/output code pages back to the OEM default.
        /// </summary>
        /// <remarks>
        /// Replaces the managed Console.Input/OutputEncoding setters (which aren’t in .NET 1.0)
        /// so that cmd.exe’s redirected stdin/stdout still use the ANSI/OEM code page.
        /// </remarks>
        private void RestoreOemConsolePage()
        {
            // get the OEM code page and re-apply it
            uint oem = GetOEMCP();
            SetConsoleOutputCP(oem);
            SetConsoleCP(oem);
        }

        /// <summary>
        /// Releases resources used by the control.
        /// </summary>
        /// <param name="disposing">True if called from Dispose; false if called from finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && !isDisposing)
            {
                isDisposing = true;

                // Clean up process first - this will unblock the reading threads
                if (process != null && !process.HasExited)
                {
                    try
                    {
                        process.StandardInput.Close();
                        if (!process.WaitForExit(1000))  // Reduced timeout
                            process.Kill();
                    }
                    catch { }
                }

                // Give threads a moment to exit naturally after process termination
                Thread.Sleep(100);

                // Don't wait for threads - let them terminate naturally
                // The isDisposing flag will cause them to exit their loops

                if (process != null)
                {
                    process.Close(); // .NET 1.0 compatible
                    process = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Sends a command to the hosted shell.
        /// </summary>
        /// <param name="command">The command text (without trailing newline).</param>
        /// <example>
        /// <code>
        /// cmdHost.SendCommand("dir");
        /// </code>
        /// </example>
        public void SendCommand(string command)
        {
            if (process != null && !process.HasExited && !isDisposing)
            {
                try
                {
                    process.StandardInput.WriteLine(command);
                    process.StandardInput.Flush();
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// Sets up the UI elements: a read-only output box and an input box.
        /// </summary>
        private void InitializeComponent()
        {
            outputTextBox = new TextBox();
            inputTextBox = new TextBox();
            SuspendLayout();

            // outputTextBox
            outputTextBox.Multiline = true;
            outputTextBox.ReadOnly = true;
            outputTextBox.ScrollBars = ScrollBars.Both;
            outputTextBox.Dock = DockStyle.Fill;
            outputTextBox.Font = new System.Drawing.Font("Lucida Console", 11f);
            outputTextBox.BackColor = System.Drawing.Color.Black;
            outputTextBox.ForeColor = System.Drawing.Color.Silver;
            outputTextBox.WordWrap = false;

            // inputTextBox
            inputTextBox.Dock = DockStyle.Bottom;
            inputTextBox.Font = new System.Drawing.Font("Lucida Console", 11f);
            inputTextBox.BackColor = System.Drawing.Color.Black;
            inputTextBox.ForeColor = System.Drawing.Color.Silver;
            inputTextBox.Height = 20;
            inputTextBox.KeyDown += new KeyEventHandler(InputTextBox_KeyDown);

            // CmdHostControl
            Controls.Add(outputTextBox);
            Controls.Add(inputTextBox);
            ResumeLayout(false);
        }

        /// <summary>
        /// Handles Enter key in input box to send command.
        /// </summary>
        /// <param name="sender">The input TextBox.</param>
        /// <param name="e">Key event args.</param>
        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendCommand(inputTextBox.Text);
                inputTextBox.Clear();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Launches cmd.exe and begins asynchronous reading of its output and error streams.
        /// </summary>
        private void StartShell()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "cmd.exe";
                psi.Arguments = "/K"; // interactive mode
                psi.UseShellExecute = false;
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.CreateNoWindow = true;

                process = new Process();
                process.StartInfo = psi;
                process.Start();

                // Start separate threads for stdout and stderr
                outputThread = new Thread(new ThreadStart(ReadOutput));
                outputThread.IsBackground = true;
                outputThread.Start();

                errorThread = new Thread(new ThreadStart(ReadError));
                errorThread.IsBackground = true;
                errorThread.Start();
            }
            catch (Exception ex)
            {
                AppendLine("Failed to start shell: " + ex.Message);
            }
        }

        /// <summary>
        /// Reads the shell's stdout and appends to output box.
        /// </summary>
        private void ReadOutput()
        {
            try
            {
                StreamReader stdout = process.StandardOutput;
                string line;
                while (!isDisposing && (line = stdout.ReadLine()) != null)
                    AppendLine(line);
            }
            catch (Exception ex)
            {
                if (!isDisposing)
                    AppendLine("Output reading error: " + ex.Message);
            }
        }

        /// <summary>
        /// Reads the shell's stderr and appends to output box.
        /// </summary>
        private void ReadError()
        {
            try
            {
                StreamReader stderr = process.StandardError;
                string line;
                while (!isDisposing && (line = stderr.ReadLine()) != null)
                    AppendLine(line);
            }
            catch (Exception ex)
            {
                if (!isDisposing)
                    AppendLine("Error reading error stream: " + ex.Message);
            }
        }

        /// <summary>
        /// Thread-safe append to output box.
        /// </summary>
        /// <param name="text">The line of text to append.</param>
        private void AppendLine(string text)
        {
            if (isDisposing || outputTextBox == null)
                return;

            try
            {
                if (outputTextBox.InvokeRequired)
                    outputTextBox.Invoke(new AppendDelegate(AppendLine), new object[] { text });
                else
                {
                    outputTextBox.AppendText(text + Environment.NewLine);
                    outputTextBox.SelectionStart = outputTextBox.Text.Length;
                    outputTextBox.ScrollToCaret();
                }
            }
            catch { }
        }
    }
}