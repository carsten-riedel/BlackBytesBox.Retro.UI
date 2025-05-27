namespace System
{
    /// <summary>
    /// Indicates whether the specified flag is set in the current enumeration value. &lt;implementation hidden&gt;
    /// </summary>
    /// <remarks>
    /// Mirrors the behavior of Enum.HasFlag introduced in .NET 4.0.
    /// </remarks>
    public static class EnumExtensions
    {
        /// <summary>
        /// Determines whether one or more bit fields are set in the current instance.
        /// &lt;implementation hidden&gt;
        /// </summary>
        /// <param name="value">The enumeration value to check.</param>
        /// <param name="flag">An enumeration value representing one or more bit fields.</param>
        /// <returns>
        /// True if all bits in <paramref name="flag"/> are also set in <paramref name="value"/>; otherwise, false.
        /// </returns>
        /// <example>
        /// <code>
        /// [Flags]
        /// enum MyFlags { A = 1, B = 2, C = 4 }
        /// var f = MyFlags.A | MyFlags.C;
        /// bool hasA = f.HasFlagCustom(MyFlags.A);   // true
        /// bool hasB = f.HasFlagCustom(MyFlags.B);   // false
        /// </code>
        /// </example>
        public static bool HasFlag(this Enum value, Enum flag)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (flag == null)
                throw new ArgumentNullException(nameof(flag));
            if (value.GetType() != flag.GetType())
                throw new ArgumentException("Enumeration types must match.", nameof(flag));

            // Convert both to UInt64 to cover all underlying enum types
            ulong numericValue = Convert.ToUInt64(value);
            ulong numericFlag = Convert.ToUInt64(flag);

            return (numericValue & numericFlag) == numericFlag;
        }
    }
}