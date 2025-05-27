using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace BlackBytesBox.Retro.UI
{
    /// <summary>
    /// Control that displays and controls an animated GIF spinner with scaling and transparency support.
    /// In design mode, the background is solid white for visibility.
    /// </summary>
    public class GifSpinner : Control, IDisposable
    {
        private Image _gifImage;
        private bool _isSpinning;

        /// <summary>
        /// Initializes a new instance of the <see cref="GifSpinner"/> class.
        /// </summary>
        public GifSpinner()
        {
            // Enable optimized painting and double buffering
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.SupportsTransparentBackColor,
                true);

            UpdateStyles();
        }

        /// <summary>
        /// Gets or sets the GIF image used for the spinner.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public Image GifImage
        {
            get => _gifImage;
            set
            {
                _gifImage = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Indicates whether the spinner animation is active.
        /// </summary>
        [Browsable(false)]
        public bool IsSpinning => _isSpinning;

        /// <summary>
        /// Starts the spinner animation.
        /// </summary>
        public void Start()
        {
            if (_gifImage == null) return;
            _isSpinning = true;
            ImageAnimator.Animate(_gifImage, OnFrameChanged);
            Visible = true;
        }

        /// <summary>
        /// Stops the spinner animation.
        /// </summary>
        public void Stop()
        {
            if (_gifImage == null) return;
            ImageAnimator.StopAnimate(_gifImage, OnFrameChanged);
            _isSpinning = false;
            Visible = false;
        }

        /// <summary>
        /// Stops the spinner animation and displays the first frame of the GIF.
        /// </summary>
        /// <remarks>
        /// Useful for showing a static initial image instead of hiding the control.
        /// </remarks>
        public void StopAndShowFirstFrame()
        {
            if (_gifImage == null) return;
            // Stop animation
            ImageAnimator.StopAnimate(_gifImage, OnFrameChanged);
            _isSpinning = false;

            // Select first frame
            var dimension = new FrameDimension(_gifImage.FrameDimensionsList[0]);
            _gifImage.SelectActiveFrame(dimension, 0);

            // Ensure visible and repaint
            Visible = true;
            Invalidate();
        }

        /// <summary>
        /// Set background color based on design/runtime mode.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (DesignMode || (Site != null && Site.DesignMode))
            {
                BackColor = Color.White;
            }
            else
            {
                BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// Overrides window creation parameters to support transparency at runtime.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                if (!(DesignMode || (Site != null && Site.DesignMode)))
                {
                    // WS_EX_TRANSPARENT = 0x20
                    cp.ExStyle |= 0x20;
                }
                return cp;
            }
        }

        /// <summary>
        /// Paints parent background in runtime to preserve transparency; default designer background otherwise.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (!(DesignMode || (Site != null && Site.DesignMode)) && Parent != null)
            {
                var state = pevent.Graphics.Save();
                pevent.Graphics.TranslateTransform(-Left, -Top);
                InvokePaintBackground(Parent, new PaintEventArgs(pevent.Graphics, pevent.ClipRectangle));
                InvokePaint(Parent, new PaintEventArgs(pevent.Graphics, pevent.ClipRectangle));
                pevent.Graphics.Restore(state);
            }
            else
            {
                base.OnPaintBackground(pevent);
            }
        }

        /// <summary>
        /// Updates and draws the current frame of the GIF, scaled within control bounds.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_gifImage == null) return;

            if (_isSpinning)
            {
                ImageAnimator.UpdateFrames(_gifImage);
            }

            e.Graphics.DrawImage(_gifImage, new Rectangle(0, 0, Width, Height));
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Disposes resources used by the control.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_isSpinning && _gifImage != null)
                {
                    Stop();
                }
                _gifImage?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}