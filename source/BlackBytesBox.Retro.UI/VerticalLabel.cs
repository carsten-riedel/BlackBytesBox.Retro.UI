using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;

namespace BlackBytesBox.Retro.UI
{
    /// <summary>
    /// A Label that renders its text rotated 90° (vertical).
    /// </summary>
    /// <remarks>
    /// Inherit from this if you need other orientations—just adjust the RotateTransform angle.
    /// </remarks>
    public class VerticalLabel : Label
    {
        /// <summary>
        /// Gets or sets the text orientation angle, in degrees.
        /// </summary>
        [Category("Appearance")]
        [Description("Rotation angle of the rendered text.")]
        public float TextAngle { get; set; } = 90f;

        protected override void OnPaint(PaintEventArgs e)
        {
            // turn on anti-aliasing (or ClearTypeGridFit for subpixel)
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            // now draw rotated text as before
            //e.Graphics.TranslateTransform(ClientSize.Width / 2f, ClientSize.Height / 2f);

            var anchor = new PointF(0, Height);

            // Move origin to the anchor point
            e.Graphics.TranslateTransform(anchor.X, anchor.Y);

            e.Graphics.RotateTransform(TextAngle);
            using var brush = new SolidBrush(ForeColor);
            var sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            e.Graphics.DrawString(Text, Font, brush, 0, 0, sf);
            e.Graphics.ResetTransform();
        }
    }
}