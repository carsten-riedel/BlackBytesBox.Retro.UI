using System;
using System.Collections.Generic;
using System.Text;

namespace BlackBytesBox.Retro.UI.Extensions
{
    public static partial class StringExtensions
    {
        /// <summary>
        /// String.ReplaceLineEndings - Added in .NET 5.0
        /// </summary>
        public static string ReplaceLineEndings(this string source, string replacementText = "\r\n")
        {
            if (source == null) throw new ArgumentNullException("source");
            if (replacementText == null) replacementText = Environment.NewLine;

            return source
                .Replace("\r\n", "\n")  // Normalize Windows line endings
                .Replace("\r", "\n")    // Normalize Mac line endings
                .Replace("\n", replacementText); // Replace with desired ending
        }
    }
}
