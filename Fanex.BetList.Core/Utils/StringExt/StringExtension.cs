// ---------------------------------------------------------------------------------
// <copyright file="StringExtension.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.10">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Utils.StringExt
{
    public static class StringExtension
    {
        /// <summary>
        /// Uppercases the first.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>string</returns>
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
        /// Counts the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="countText">The count text.</param>
        /// <returns>int</returns>
        public static int Count(this string s, string countText)
        {
            return (s.Length - s.Replace(countText, "").Length) / countText.Length;
        }

        /// <summary>
        /// Gets the line break total.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>int</returns>
        public static int GetLineBreakTotal(this string s)
        {
            return s.Count("\n") + 1;
        }
    }
}