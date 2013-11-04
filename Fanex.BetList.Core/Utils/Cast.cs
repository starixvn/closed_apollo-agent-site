// ---------------------------------------------------------------------------------
// <copyright file="Cast.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Utils
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Convert class.
    /// </summary>
    public static partial class Cast
    {
        /// <summary>
        /// Returns an object as a Decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Returns an object as a Decimal.
        /// </returns>
        public static Decimal AsDecimal(object value)
        {
            return AsDecimal(value, 0, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns an object as a Decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">Value returned if cast fails.</param>
        /// <returns>
        /// Returns an object as a Decimal.
        /// </returns>
        public static Decimal AsDecimal(object value, Decimal defaultValue)
        {
            return AsDecimal(value, defaultValue, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns an object as a Decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="provider">Instance of IFormatProvider.</param>
        /// <returns>
        /// Returns an object as a Decimal.
        /// </returns>
        public static Decimal AsDecimal(object value, IFormatProvider provider)
        {
            return AsDecimal(value, 0, provider);
        }

        /// <summary>
        /// Returns an object as a Decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">Value returned if cast fails.</param>
        /// <param name="provider">Instance of IFormatProvider.</param>
        /// <returns>
        /// Returns an object as a Decimal.
        /// </returns>
        public static Decimal AsDecimal(object value, Decimal defaultValue, IFormatProvider provider)
        {
            if (value == null || value == System.DBNull.Value)
                return defaultValue;

            Decimal retval;

            if (provider == null)
                provider = CultureInfo.CurrentCulture;

            try
            {
                retval = Convert.ToDecimal(value, provider);
            }
            catch (InvalidCastException)
            {
                retval = defaultValue;
            }
            catch (FormatException)
            {
                retval = defaultValue;
            }

            return retval;
        }

        /// <summary>
        /// Returns an object as a Decimal using invariant culture.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Returns an object as a Decimal using invariant culture.
        /// </returns>
        public static Decimal AsDecimalInvariant(object value)
        {
            return AsDecimal(value, 0, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns an object as a Decimal using invariant culture.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// Returns an object as a Decimal using invariant culture.
        /// </returns>
        public static Decimal AsDecimalInvariant(object value, Decimal defaultValue)
        {
            return AsDecimal(value, defaultValue, CultureInfo.InvariantCulture);
        }
    }
}