using System;
using System.Globalization;

/// <summary>
/// Summary description for Cast to String
/// Author:Thai.tran
/// </summary>
namespace Fanex.Age.Utility
{
    public static partial class Cast
    {
        /// <summary>
        /// Converts an object to a String.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>string</returns> 
        public static string AsString(object value)
        {
            return AsString(value, string.Empty, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns an object as a String.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">Value returned if cast fails</param>
        /// <returns>string</returns>
        public static string AsString(object value, string defaultValue)
        {
            return AsString(value, defaultValue, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns an object as a String.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="provider">Instance of IFormatProvider</param>
        /// <returns></returns>
        public static string AsString(object value, IFormatProvider provider)
        {
            return AsString(value, string.Empty, provider);
        }

        /// <summary>
        /// Returns an object as a String.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">Value returned if cast fails</param>
        /// <param name="provider">Instance of IFormatProvider</param>
        /// <returns>string</returns> 
        public static string AsString(object value, string defaultValue, IFormatProvider provider)
        {
            if (value == null || value == System.DBNull.Value)
                return defaultValue;

            if (provider == null)
                provider = CultureInfo.CurrentCulture;

            string retval;

            try
            {
                retval = Convert.ToString(value, provider);
            }
            catch (InvalidCastException)
            {
                retval = defaultValue;
            }
            return retval;
        }

        /// <summary>
        /// Returns an object as a String using invariant culture
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AsStringInvariant(object value)
        {
            return AsString(value, string.Empty, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns an object as a String using invariant culture
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string AsStringInvariant(object value, string defaultValue)
        {
            return AsString(value, defaultValue, CultureInfo.InvariantCulture);
        }
    }
}