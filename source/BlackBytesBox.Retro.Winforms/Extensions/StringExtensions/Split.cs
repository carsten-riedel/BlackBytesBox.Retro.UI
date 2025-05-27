using System;
using System.Collections.Generic;
using System.Text;

namespace BlackBytesBox.Retro.UI.Extensions
{
    public static partial class StringExtensions
    {
        /// <summary>
        /// String.Split with StringSplitOptions and string separator - Enhanced in .NET 4.0+
        /// </summary>
        public static string[] Split(this string source, string separator, StringSplitOptions options = StringSplitOptions.None)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (separator == null) return new string[] { source };

            List<string> result = new List<string>();
            int start = 0;
            int index = 0;

            while ((index = source.IndexOf(separator, start)) != -1)
            {
                string part = source.Substring(start, index - start);
                if (options != StringSplitOptions.RemoveEmptyEntries || part.Length > 0)
                    result.Add(part);
                start = index + separator.Length;
            }

            string remaining = source.Substring(start);
            if (options != StringSplitOptions.RemoveEmptyEntries || remaining.Length > 0)
                result.Add(remaining);

            return result.ToArray();
        }

        /// String.Split with multiple string separators - Enhanced in .NET 4.0+
        /// </summary>
        public static string[] Split(this string source, string[] separators, StringSplitOptions options = StringSplitOptions.None)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (separators == null || separators.Length == 0) return new string[] { source };

            string[] result = new string[] { source };

            foreach (string separator in separators)
            {
                if (string.IsNullOrEmpty(separator)) continue;

                List<string> newResult = new List<string>();
                foreach (string part in result)
                {
                    string[] subParts = part.Split(separator, options);
                    newResult.AddRange(subParts);
                }
                result = newResult.ToArray();
            }

            return result;
        }
    }
}
