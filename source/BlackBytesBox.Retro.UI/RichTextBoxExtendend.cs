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
    /// RichTextBox with extended features: output-only display preserving background, optional caret hiding,
    /// utility scrolling, and optional scrollbar disabling.
    /// </summary>
    public class RichTextBoxExtendend3 : RichTextBox
    {
        // Win32 API to hide the text caret
        [DllImport("user32.dll")]
        private static extern bool HideCaret(IntPtr hWnd);

        // Win32 API to control scrollbar visibility
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

        // Win32 API to enable or disable scrollbars (grayed out)
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool EnableScrollBar(IntPtr hWnd, int wSBflags, int wArrows);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref Point lParam);

        private const int WM_VSCROLL = 0x0115;
        private const int WM_HSCROLL = 0x0114;
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int WM_USER = 0x0400;
        private const int EM_GETSCROLLPOS = WM_USER + 221;
        private const int EM_SETSCROLLPOS = WM_USER + 222;
        private const int SB_VERT = 1;
        private const int SB_HORZ = 0;
        private const int SB_BOTH = 3;

        // Flags for EnableScrollBar
        private const int ESB_ENABLE_BOTH = 0x0000;

        private const int ESB_DISABLE_BOTH = 0x0003;

        private readonly Color _originalBackColor;
        private bool _scrollBarDisabled;
        private bool _internalScroll;

        /// <summary>
        /// Initializes a new instance of the <see cref="RichTextBoxExtendend"/> class.
        /// </summary>
        public RichTextBoxExtendend3()
        {
            _originalBackColor = base.BackColor;
            OutputOnlyMode = true;
            HideCaretMode = true;
            ScrollBarDisabled = false;
        }

        /// <summary>
        /// Gets or sets whether the control is in output-only mode (read-only with original background).
        /// </summary>
        [DefaultValue(true)]
        [Category("Extended Features")]
        [Description("When true, the control is read-only and preserves its original background color.")]
        public bool OutputOnlyMode
        {
            get => base.ReadOnly;
            set
            {
                base.ReadOnly = value;
                base.BackColor = value ? _originalBackColor : SystemColors.Window;
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
        /// Gets or sets whether the scrollbars are disabled (grayed out and interaction blocked).
        /// </summary>
        [DefaultValue(false)]
        [Category("Extended Features")]
        [Description("When true, the control's scrollbars will be grayed out and both user/programmatic scrolling are disabled, except internal methods.")]
        public bool ScrollBarDisabled
        {
            get => _scrollBarDisabled;
            set
            {
                _scrollBarDisabled = value;
                // Always show scrollbars
                ShowScrollBar(Handle, SB_BOTH, true);
                // Gray out or restore scrollbars
                EnableScrollBar(Handle, SB_BOTH, value ? ESB_DISABLE_BOTH : ESB_ENABLE_BOTH);
                Invalidate();
            }
        }

        /// <summary>
        /// Appends a line of text without affecting the current scroll position or selection.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void AppendTextLine(string text)
        {
            // Allow internal scroll messages
            _internalScroll = true;

            Point scrollPos = new Point();
            SendMessage(Handle, EM_GETSCROLLPOS, IntPtr.Zero, ref scrollPos);

            int selStart = SelectionStart;
            int selLength = SelectionLength;

            base.AppendText(text + Environment.NewLine);

            SelectionStart = selStart;
            SelectionLength = selLength;

            SendMessage(Handle, EM_SETSCROLLPOS, IntPtr.Zero, ref scrollPos);

            _internalScroll = false;
        }

        /// <summary>
        /// Scrolls the content to the absolute bottom without changing the current selection.
        /// </summary>
        public void ScrollToBottomNotUsingSelection()
        {
            _internalScroll = true;
            SendMessage(Handle, WM_VSCROLL, new IntPtr(7), IntPtr.Zero);
            _internalScroll = false;
        }

        /// <summary>
        /// Overrides window procedure to block scroll messages when scrollbar disabled.
        /// Allows internal messages when _internalScroll is true.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (_scrollBarDisabled && !_internalScroll &&
                (m.Msg == WM_VSCROLL || m.Msg == WM_HSCROLL || m.Msg == WM_MOUSEWHEEL || m.Msg == EM_SETSCROLLPOS))
            {
                return;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Ensures caret behavior on focus when in output-only mode.
        /// </summary>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (OutputOnlyMode && HideCaretMode)
            {
                HideCaret(Handle);
                Parent?.Select();
            }
        }

        /// <summary>
        /// Ensures caret behavior after mouse interactions when in output-only mode.
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            if (OutputOnlyMode && HideCaretMode)
            {
                HideCaret(Handle);
                Parent?.Select();
            }
        }
    }


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
                    base.Text = base.Text.TrimEnd(Environment.NewLine) + Environment.NewLine + _textBuffer.Dequeue();
                }
                    
                //base.AppendText(Environment.NewLine+_textBuffer.Dequeue());
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