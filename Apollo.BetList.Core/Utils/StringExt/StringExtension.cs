// ---------------------------------------------------------------------------------
// <copyright file="StringExtension.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.10">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Utils.StringExt
{
    /// <summary>
    /// String extension class.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Uppercases the first.
        /// </summary>
        /// <param name="s">Input a string.</param>
        /// <returns>Uppercases the first letter.</returns>
        public static string UppercaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Counts the specified in string.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="countText">The count text.</param>
        /// <returns>
        /// Number of character in a string.
        /// </returns>
        public static int Count(this string s, string countText)
        {
            return (s.Length - s.Replace(countText, string.Empty).Length) / countText.Length;
        }

        /// <summary>
        /// Gets the line break total.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>Number of break line.</returns>
        public static int GetLineBreakTotal(this string s)
        {
            return s.Count("\n") + 1;
        }
    }
}