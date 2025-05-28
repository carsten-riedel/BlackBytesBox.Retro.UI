using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BlackBytesBox.Retro.UI
{
    /// <summary>
    /// RichTextBox with extended features: output-only display, optional caret hiding,
    /// utility scrolling, and buffered append without unintended scrolling.
    /// </summary>
    public class RichTextBoxExtendend : RichTextBox
    {
        // Win32 APIs
        [DllImport("user32.dll")] private static extern bool HideCaret(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)] private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)] private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref Point lParam);

        private const int WM_VSCROLL = 0x0115;
        private static readonly IntPtr SB_BOTTOM = new IntPtr(7);
        private const int WM_USER = 0x0400;
        private const int EM_GETSCROLLPOS = WM_USER + 221;
        private const int EM_SETSCROLLPOS = WM_USER + 222;

        private readonly Color _originalBackColor;
        private readonly Timer _updateTimer;
        private readonly Queue<string> _textBuffer;

        // Timestamp when selection length became zero
        private DateTime _selectionZeroSince = DateTime.MinValue;

        // Delay before auto-scroll after deselection
        private readonly TimeSpan _autoScrollDelay = TimeSpan.FromMilliseconds(250);

        /// <summary>
        /// Initializes a new instance of the <see cref="RichTextBoxExtendend"/> class.
        /// </summary>
        public RichTextBoxExtendend()
        {
            _originalBackColor = BackColor;
            OutputOnlyMode = true;
            HideCaretMode = true;

            _textBuffer = new Queue<string>();
            _updateTimer = new Timer { Interval = 250 };
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();
        }

        /// <summary>
        /// Gets or sets whether the control is in output-only mode (read-only with original background).
        /// </summary>
        [DefaultValue(true)]
        [Category("Extended Features")]
        [Description("When true, the control is read-only and preserves its original background color.")]
        public bool OutputOnlyMode
        {
            get => ReadOnly;
            set
            {
                ReadOnly = value;
                BackColor = value ? _originalBackColor : SystemColors.Window;
                Cursor = value ? Cursors.Arrow : Cursors.IBeam;
                if (HideCaretMode && value)
                    HideCaret(Handle);
            }
        }

        /// <summary>
        /// Gets or sets whether the text caret is hidden when in output-only mode.
        /// </summary>
        [DefaultValue(true)]
        [Category("Extended Features")]
        [Description("When true and OutputOnlyMode is enabled, the text caret will be hidden.")]
        public bool HideCaretMode { get; set; }

        /// <summary>
        /// Adds a line to the internal buffer. Lines are appended every 250ms.
        /// </summary>
        /// <param name="line">The text to append (newline added automatically).</param>
        public void AppendTextLine(string line)
        {
            if (line != null)
                _textBuffer.Enqueue(line);
        }

        /// <summary>
        /// Timer tick: drains buffer and appends without scrolling.
        /// </summary>
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (_textBuffer.Count == 0) return;

            // Save scroll pos and selection
            Point scrollPos = new Point();
            SendMessage(Handle, EM_GETSCROLLPOS, IntPtr.Zero, ref scrollPos);
            int selStart = SelectionStart;
            int selLength = SelectionLength;

            // Append buffered lines without changing scroll
            while (_textBuffer.Count > 0)
            {
                if (base.Text.Length == 0)
                {
                    base.Text = _textBuffer.Dequeue();
                }
                else
                {
                    // .NET 1.0–compatible: remove any trailing CR or LF characters
                    base.Text = base.Text.TrimEnd(new[] { '\r', '\n' }) + Environment.NewLine + _textBuffer.Dequeue();
                }
            }

            // Restore selection and scroll
            SelectionStart = selStart;
            SelectionLength = selLength;
            SendMessage(Handle, EM_SETSCROLLPOS, IntPtr.Zero, ref scrollPos);
            AutoScrollIfIdle();
        }

        /// <summary>
        /// Auto-scrolls to the bottom when there is no text selection for at least the specified delay.
        /// </summary>
        public void AutoScrollIfIdle()
        {
            if (SelectionLength > 0)
            {
                // Reset idle timer when selection is active
                _selectionZeroSince = DateTime.MinValue;
                return;
            }
            if (_selectionZeroSince == DateTime.MinValue)
            {
                // Start timing when selection cleared
                _selectionZeroSince = DateTime.Now;
                return;
            }

            if (DateTime.Now - _selectionZeroSince >= _autoScrollDelay)
            {
                SendMessage(Handle, WM_VSCROLL, SB_BOTTOM, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Disposes resources used by the control.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _updateTimer.Stop();
                _updateTimer.Tick -= UpdateTimer_Tick;
                _updateTimer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}