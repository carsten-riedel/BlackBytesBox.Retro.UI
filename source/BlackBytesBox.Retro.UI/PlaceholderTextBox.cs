using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BlackBytesBox.Retro.UI
{
    public class PlaceholderTextBox : TextBox
    {
        private const int WM_PAINT = 0x000F;
        private const int WM_ERASEBKGND = 0x0014;

        private string _placeholderText = "";
        private bool _hideOnFocus = false;

        /// <summary>
        /// The text shown when the textbox is empty.
        /// </summary>
        [Category("Placeholder")]
        [Description("The grayed-out text to display when the textbox is empty.")]
        [DefaultValue("")]
        public string PlaceholderText
        {
            get => _placeholderText;
            set
            {
                _placeholderText = value ?? "";
                Invalidate(); // redraw if needed
            }
        }

        /// <summary>
        /// Whether the placeholder should be hidden when the control is focused.
        /// </summary>
        [Category("Placeholder")]
        [Description("If true, hides the placeholder when the control has focus.")]
        [DefaultValue(false)]
        public bool HideOnFocus
        {
            get => _hideOnFocus;
            set
            {
                _hideOnFocus = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Fired when the user presses Enter while the control has focus.
        /// </summary>
        [Category("Placeholder")]
        [Description("Occurs when the Enter key is pressed.")]
        public event EventHandler? EnterKeyPressed;

        protected virtual void OnEnterKeyPressed(EventArgs e)
        {
            EnterKeyPressed?.Invoke(this, e);
        }

        public PlaceholderTextBox()
        {
           
            this.Click += (s, e) => Invalidate();
            this.MouseMove += (s, e) => { if (e.Button != MouseButtons.None) Invalidate(); };
            this.MouseDown += (s, e) => Invalidate();
            //this.KeyDown += (s, e) => { if (this.Text == string.Empty) { Invalidate(); } };
            this.PreviewKeyDown += (s, e) => { Invalidate(); };
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Enter)
            {
                OnEnterKeyPressed(EventArgs.Empty);
                e.Handled = true;   // suppress the ding
                e.SuppressKeyPress = true;
            }
        }

        protected override void WndProc(ref Message m)
        {
            // suppress flicker
            if (m.Msg == WM_ERASEBKGND)
            {
                m.Result = IntPtr.Zero;
                return;
            }

            base.WndProc(ref m);

            // draw placeholder if needed
            if (m.Msg == WM_PAINT
                && string.IsNullOrEmpty(this.Text)
                && !string.IsNullOrEmpty(_placeholderText)
                && (!_hideOnFocus || !this.Focused))
            {
                using (Graphics g = Graphics.FromHwnd(this.Handle))
                using (Brush brush = new SolidBrush(Color.Gray))
                {

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                    g.DrawString(_placeholderText, this.Font, brush, new PointF(0f, 1f));
                }
            }
        }
    }


}
