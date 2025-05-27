using System;
using System.Collections.Generic;
using System.Text;

namespace BlackBytesBox.Retro.UI.Extensions
{
    public static partial class StringExtensions
    {
        /// <summary>
        /// String.Contains with StringComparison - Added in .NET 5.0/.NET Core 2.1
        /// </summary>
        public static bool Contains(this string source, string value, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (value == null) throw new ArgumentNullException("value");

            return source.IndexOf(value, comparisonType) >= 0;
        }
    }
}
