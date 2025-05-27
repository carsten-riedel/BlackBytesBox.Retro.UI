using System;
using System.Collections.Generic;
using System.Text;

namespace BlackBytesBox.Retro.UI.Extensions
{
    public static partial class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return value == null || value.Trim().Length == 0;
        }
    }
}
