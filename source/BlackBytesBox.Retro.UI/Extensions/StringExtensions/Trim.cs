using System;
using System.Collections.Generic;
using System.Text;

namespace BlackBytesBox.Retro.UI.Extensions
{
    public static partial class StringExtensions
    {
        /// <summary>
        /// Removes all leading occurrences of specified characters from the current string.
        /// </summary>
        /// <remarks>
        /// Uses ordinal comparison to remove any character contained in <paramref name="trimChars"/> from the start.
        /// </remarks>
        /// <param name="value">The string to trim.</param>
        /// <param name="trimChars">The characters to remove from the start.</param>
        /// <returns>A new string with the specified characters removed from the start.</returns>
        /// <example>
        /// <code>
        /// var result = "<<<data".TrimStart("<");
        /// // result == "data"
        /// </code>
        /// </example>
        public static string TrimStart(this string value, string trimChars)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(trimChars)) return value;

            int start = 0;
            while (start < value.Length && trimChars.IndexOf(value[start]) >= 0)
            {
                start++;
            }

            return value.Substring(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of specified characters from the current string.
        /// </summary>
        /// <remarks>
        /// Uses ordinal comparison to remove any character contained in <paramref name="trimChars"/> from the end.
        /// </remarks>
        /// <param name="value">The string to trim.</param>
        /// <param name="trimChars">The characters to remove from the end.</param>
        /// <returns>A new string with the specified characters removed from the end.</returns>
        /// <example>
        /// <code>
        /// var result = "data>>>".TrimEnd(">");
        /// // result == "data"
        /// </code>
        /// </example>
        public static string TrimEnd(this string value, string trimChars)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(trimChars)) return value;

            int end = value.Length - 1;
            while (end >= 0 && trimChars.IndexOf(value[end]) >= 0)
            {
                end--;
            }

            return value.Substring(0, end + 1);
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of specified characters from the current string.
        /// </summary>
        /// <remarks>
        /// Combines <see cref="TrimStart(string)"/> and <see cref="TrimEnd(string)"/> for complete trimming.
        /// </remarks>
        /// <param name="value">The string to trim.</param>
        /// <param name="trimChars">The characters to remove from both ends.</param>
        /// <returns>A new string with the specified characters removed from both ends.</returns>
        /// <example>
        /// <code>
        /// var result = "<<<data>>>".Trim("<>");
        /// // result == "data"
        /// </code>
        /// </example>
        public static string Trim(this string value, string trimChars)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(trimChars)) return value;

            return value.TrimStart(trimChars).TrimEnd(trimChars);
        }
    }
}
