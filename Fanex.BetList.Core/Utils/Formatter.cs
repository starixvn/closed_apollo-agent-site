// ---------------------------------------------------------------------------------
// <copyright file="Formatter.cs" company="Nexcel Solutions Vietnam">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web;

    /// <summary>
    /// Formatter class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    public class Formatter
    {
        public const string CULT_NAME_ENG = "en-US";
        public const string CULT_NAME_VI = "vi-VN";

        public const string DECIMAL_FORMAT = "{0:#,###}";
        public const string DECIMAL_FORMAT0 = "{0:#,##0}";
        public const string DECIMAL_FORMAT1 = "{0:#,##0.#}";
        public const string DECIMAL_FORMAT10 = "{0:#,##0.0}";
        public const string DECIMAL_FORMAT2 = "{0:#,##0.##}";
        public const string DECIMAL_FORMAT20 = "{0:#,##0.00}";
        public const string DECIMAL_FORMAT2C = "{0:###0.00}"; // without comma
        public const string DECIMAL_FORMAT3 = "{0:#,##0.###}";
        public const string DECIMAL_FORMAT30 = "{0:#,##0.000}";
        public const string DECIMAL_FORMAT3C = "{0:###0.000}";
        public const string DECIMAL_FORMAT4 = "{0:#,##0.####}";
        public const string DECIMAL_FORMAT40 = "{0:#,##0.0000}";

        public const string DECIMAL_FORMAT_VI = "{0:#.###}";
        public const string DECIMAL_FORMAT0_VI = "{0:#.##0}";
        public const string DECIMAL_FORMAT1_VI = "{0:#.##0,#}";
        public const string DECIMAL_FORMAT10_VI = "{0:#.##0,0}";
        public const string DECIMAL_FORMAT2_VI = "{0:#.##0,##}";
        public const string DECIMAL_FORMAT20_VI = "{0:#.##0,00}";
        public const string DECIMAL_FORMAT2C_VI = "{0:#.##0,00}";
        public const string DECIMAL_FORMAT3_VI = "{0:#.##0,###}";
        public const string DECIMAL_FORMAT30_VI = "{0:#.##0,000}";
        public const string DECIMAL_FORMAT3C_VI = "{0:#.##0,000}";
        public const string DECIMAL_FORMAT4_VI = "{0:#.##0,####}";
        public const string DECIMAL_FORMAT40_VI = "{0:#.##0,0000}";

        public const string TIME_FORMAT_MILISEC = "HH:mm:ss fff";

        private static System.Globalization.CultureInfo _engCult = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        private static System.Globalization.CultureInfo _viCult = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        private static CultureInfo cult = new System.Globalization.CultureInfo("en-US");

        public Formatter()
        {
        }

        public static CultureInfo EngCult
        {
            get
            {
                return _engCult;
            }
        }

        public static CultureInfo ViCult
        {
            get
            {
                return _viCult;
            }
        }

        #region Decimal

        /// <summary>
        /// Decimal the format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decDigits">The dec digits.</param>
        /// <returns>System String.</returns>
        public static string DecFormat(decimal value, byte decDigits)
        {
            return value.ToString("N" + decDigits);
        }

        /// <summary>
        /// Decimal the format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decDigits">The dec digits.</param>
        /// <returns>System String.</returns>
        public static string DecFormat(object value, byte decDigits)
        {
            return DecFormat(value, decDigits, "0");
        }

        /// <summary>
        /// Decimal the format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decDigits">The dec digits.</param>
        /// <param name="numberDefault">The number default.</param>
        /// <returns>System String.</returns>
        public static string DecFormat(object value, byte decDigits, string numberDefault)
        {
            return (Convert.IsDBNull(value) || value == null) ? numberDefault : DecFormat(Convert.ToDecimal(value), decDigits);
        }

        /// <summary>
        /// Decimal the format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decDigits">The dec digits.</param>
        /// <param name="chop">If set to <c>true</c> [chop].</param>
        /// <returns>System String.</returns>
        public static string DecFormat(decimal value, byte decDigits, bool chop)
        {
            if (!chop)
            {
                return string.Format(DECIMAL_FORMAT2, value.ToString("N" + decDigits));
            }
            else
            {
                int a = (int)Math.Pow(10, decDigits);
                decimal val = value * a;

                // Returns the largest integer less than or equal number
                // If the value is negative, then we use Ceiling() function instead of Floor() function
                if (value < 0)
                {
                    return string.Format(DECIMAL_FORMAT2, Math.Ceiling(val) / a);
                }

                return string.Format(DECIMAL_FORMAT2, Math.Floor(val) / a);
            }
        }

        /// <summary>
        /// Decimal the format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decDigits">The dec digits.</param>
        /// <param name="chop">If set to <c>true</c> [chop].</param>
        /// <returns>System String.</returns>
        public static string DecFormat(object value, byte decDigits, bool chop)
        {
            return DecFormat(Convert.ToDecimal(value), decDigits, chop);
        }

        #endregion Decimal

        #region Float

        /// <summary>
        /// Floats the format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decDigits">The dec digits.</param>
        /// <returns>System String.</returns>
        public static string FloatFormat(float value, byte decDigits)
        {
            return value.ToString("N" + decDigits);
        }

        /// <summary>
        /// Floats the format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decDigits">The dec digits.</param>
        /// <returns>System String.</returns>
        public static string FloatFormat(object value, byte decDigits)
        {
            return FloatFormat(Convert.ToSingle(value), decDigits);
        }

        #endregion Float

        #region Double

        /// <summary>
        /// Doubles the format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decDigits">The dec digits.</param>
        /// <returns>System String.</returns>
        public static string DoubleFormat(double value, byte decDigits)
        {
            return value.ToString("N" + decDigits);
        }

        /// <summary>
        /// Doubles the format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decDigits">The dec digits.</param>
        /// <returns>System String.</returns>
        public static string DoubleFormat(object value, byte decDigits)
        {
            return DoubleFormat(Convert.ToDouble(value), decDigits);
        }

        #endregion Double

        #region Number

        /// <summary>
        /// Format number with 1 decimal, automatically remove redundant '0'.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>System String.</returns>
        public static string FormatNumber1(object obj, string culture)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }

            if (culture == CULT_NAME_VI)
            {
                return string.Format(DECIMAL_FORMAT1_VI, obj);
            }

            return string.Format(DECIMAL_FORMAT1, obj);
        }

        /// <summary>
        /// Format number with 2 decimal, automatically remove redundant '0'.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Format number with 2 decimal.</returns>
        public static string FormatNumber2(object obj, string culture)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }

            if (culture == CULT_NAME_VI)
            {
                return string.Format(DECIMAL_FORMAT2_VI, obj);
            }

            return string.Format(DECIMAL_FORMAT2, obj);
        }

        /// <summary>
        /// Formats the number20.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>System String.</returns>
        public static string FormatNumber20(object obj, string culture)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }

            if (culture == CULT_NAME_VI)
            {
                return string.Format(DECIMAL_FORMAT20_VI, obj);
            }

            return string.Format(DECIMAL_FORMAT20, obj);
        }

        /// <summary>
        /// Format number with maximum 3 significant decimal numbers (numbers that # 0).
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// System String.
        /// </returns>
        public static string FormatNumber3(object obj)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }

            return string.Format(DECIMAL_FORMAT3, obj);
        }

        /// <summary>
        /// Format number with 2 decimal, automatically remove redundant '0'
        /// And do not include comma.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>System String.</returns>
        public static string FormatNumber2C(object obj, string culture)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }

            if (culture == CULT_NAME_VI)
            {
                return string.Format(DECIMAL_FORMAT2C_VI, obj);
            }

            return string.Format(DECIMAL_FORMAT2C, obj);
        }

        /// <summary>
        /// Formats the number 30.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>System String.</returns>
        public static string FormatNumber30(object obj, string culture)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }

            if (culture == CULT_NAME_VI)
            {
                return string.Format(DECIMAL_FORMAT30_VI, obj);
            }

            return string.Format(DECIMAL_FORMAT30, obj);
        }

        /// <summary>
        /// Format number with 3 decimal, automatically remove redundant '0'
        /// And do not include comma.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>System String.</returns>
        public static string FormatNumber3C(object obj, string culture)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }

            if (culture == CULT_NAME_VI)
            {
                return string.Format(DECIMAL_FORMAT3C_VI, obj);
            }

            return string.Format(DECIMAL_FORMAT3C, obj);
        }

        /// <summary>
        /// Format number with 3 decimal, automatically remove redundant '0'.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>System String.</returns>
        public static string FormatNumber3(object obj, string culture)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }

            if (culture == CULT_NAME_VI)
            {
                return string.Format(DECIMAL_FORMAT3_VI, obj);
            }

            return string.Format(DECIMAL_FORMAT3, obj);
        }

        /// <summary>
        /// Format number with 4 decimal, automatically remove redundant '0'.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>System String.</returns>
        public static string FormatNumber4(object obj, string culture)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }

            if (culture == CULT_NAME_VI)
            {
                return string.Format(DECIMAL_FORMAT4_VI, obj);
            }

            return string.Format(DECIMAL_FORMAT4, obj);
        }

        /// <summary>
        /// Formats the number4.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System String.</returns>
        public static string FormatNumber4(object obj)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }

            return string.Format(DECIMAL_FORMAT4, obj);
        }

        /// <summary>
        /// Formats the number40.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>System String.</returns>
        public static string FormatNumber40(object obj, string culture)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }

            if (culture == CULT_NAME_VI)
            {
                return string.Format(DECIMAL_FORMAT40_VI, obj);
            }

            return string.Format(DECIMAL_FORMAT40, obj);
        }

        /// <summary>
        /// Formats the sign number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="decDigits">The decimal digits.</param>
        /// <returns>
        /// System String.
        /// </returns>
        public static string FormatSignNumber(decimal number, byte decDigits = 0)
        {
            if (number > 0)
            {
                return "+" + DecFormat(number, decDigits);
            }
            else
            {
                return DecFormat(number, decDigits);
            }
        }

        #endregion Number

        #region Date

        /// <summary>
        /// Longs the date format.
        /// </summary>
        /// <param name="dt">The object.</param>
        /// <returns>System String.</returns>
        public static string LongDateFormat(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            return string.Format(cult, "{0:yyyy-M-dd HH:mm:ss}", dt);
        }

        /// <summary>
        /// Shorts the date format.
        /// </summary>
        /// <param name="dt">The object.</param>
        /// <returns>System String.</returns>
        public static string ShortDateFormat(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            return string.Format(cult, "{0:yyyy-M-dd}", dt);
        }

        /// <summary>
        /// Get full date format.
        /// </summary>
        /// <param name="dt">The object.</param>
        /// <returns>System String.</returns>
        public static string FullDateFormat(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            return string.Format(cult, "{0:yyyy-MM-dd hh:mm:ss}", dt);
        }

        /// <summary>
        /// Full the date format 2 lines.
        /// </summary>
        /// <param name="dt">The object.</param>
        /// <returns>
        /// System String.
        /// </returns>
        public static string FullDateFormat2Ln(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            return string.Format(cult, "{0:yyyy-M-dd<br/>hh:mm:ss tt}", dt);
        }

        /// <summary>
        /// Full the date format 2 line.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// System String.
        /// </returns>
        public static string FullDateFormat2Line(object obj)
        {
            return FullDateFormat2Ln(obj);
        }

        /// <summary>
        /// Format date time with only month/day hour:minute.
        /// </summary>
        /// <param name="dt">The a date.</param>
        /// <returns>System string.</returns>
        public static string FormatDateTime22(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            try
            {
                return Convert.ToDateTime(dt).ToString("MM-dd<br/>hh:mmt").ToLower();
            }
            catch
            {
                return "Error";
            }
        }

        /// <summary>
        /// Formats the date time22 DDMM.
        /// </summary>
        /// <param name="dt">The a date.</param>
        /// <returns>System String.</returns>
        public static string FormatDateTime22DDMM(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            try
            {
                return Convert.ToDateTime(dt).ToString("dd/MM hh:mm");
            }
            catch
            {
                return "Error";
            }
        }

        /// <summary>
        /// Formats the time millisecond.
        /// </summary>
        /// <param name="dt">The object.</param>
        /// <returns>System String.</returns>
        public static string FormatTimeMiliSec(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            try
            {
                return Convert.ToDateTime(dt).ToString(TIME_FORMAT_MILISEC);
            }
            catch
            {
                return "Error";
            }
        }

        /// <summary>
        /// Formats the date en.
        /// </summary>
        /// <param name="dt">The object.</param>
        /// <returns>System String.</returns>
        public static string FormatDateEn(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            try
            {
                return Convert.ToDateTime(dt).ToString("yyyy-M-dd");
            }
            catch
            {
                return "Error";
            }
        }

        /// <summary>
        /// Formats the date vietnamese.
        /// </summary>
        /// <param name="date">The a date.</param>
        /// <returns>
        /// System String.
        /// </returns>
        public static string FormatDateVi(object date)
        {
            if (Convert.IsDBNull(date) || date == null)
            {
                return string.Empty;
            }

            try
            {
                return Convert.ToDateTime(date).ToString("dd/MM/yyyy");
            }
            catch
            {
                return "Error";
            }
        }

        /// <summary>
        /// Formats the date time english 12.
        /// </summary>
        /// <param name="date">The a date.</param>
        /// <returns>
        /// System String.
        /// </returns>
        public static string FormatDateTimeEn12(object date)
        {
            if (Convert.IsDBNull(date) || date == null)
            {
                return string.Empty;
            }

            try
            {
                return Convert.ToDateTime(date).ToString("yyyy-M-dd hh:mm:ss tt");
            }
            catch
            {
                return "Error";
            }
        }

        /// <summary>
        /// Formats the date time english 12 no sec.
        /// </summary>
        /// <param name="date">The a date.</param>
        /// <returns>
        /// System String.
        /// </returns>
        public static string FormatDateTimeEn12NoSec(object date)
        {
            if (Convert.IsDBNull(date) || date == null)
            {
                return string.Empty;
            }

            try
            {
                return Convert.ToDateTime(date).ToString("yyyy-M-dd hh:mm tt");
            }
            catch
            {
                return "Error";
            }
        }

        /// <summary>
        /// Formats the date time en24.
        /// </summary>
        /// <param name="dt">The object.</param>
        /// <returns>System String.</returns>
        public static string FormatDateTimeEn24(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            try
            {
                return Convert.ToDateTime(dt).ToString("yyyy-M-dd HH:mm:ss");
            }
            catch
            {
                return "Error";
            }
        }

        /// <summary>
        /// IF Time is 12:00, then do not return time.
        /// </summary>
        /// <param name="dt">The object.</param>
        /// <returns>System string.</returns>
        public static string FormatDateOptionalTime(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            try
            {
                DateTime dt2 = Convert.ToDateTime(dt);

                if ((dt2.Hour == 12 || dt2.Hour == 0) && dt2.Minute == 0)
                {
                    return dt2.ToString("yyyy-M-dd");
                }
                else
                {
                    return dt2.ToString("yyyy-M-dd hh:mm:ss tt");
                }
            }
            catch
            {
                return "Error";
            }
        }

        /// <summary>
        /// Resets the start date.
        /// </summary>
        /// <param name="dt">Input a date.</param>
        /// <returns>
        /// Date Time.
        /// </returns>
        public static DateTime ResetStartDate(DateTime dt)
        {
            return dt.AddMilliseconds(-((((((dt.Hour * 60) + dt.Minute) * 60) + dt.Second) * 1000) + dt.Millisecond));
        }

        /// <summary>
        /// Resets the end date.
        /// </summary>
        /// <param name="dt">Input a date.</param>
        /// <returns>
        /// Date Time.
        /// </returns>
        public static DateTime ResetEndDate(DateTime dt)
        {
            return ResetStartDate(dt).Add(new TimeSpan(23, 59, 59));
        }

        #endregion Date

        #region String

        /// <summary>
        /// Truncate string With Tip.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="hasSpanReturn">If set to <c>true</c> [has span return].</param>
        /// <returns>String that truncated.</returns>
        public static string TruncateWithTip(string value, int maxLength, bool hasSpanReturn)
        {
            if (value.Length > maxLength)
            {
                value = string.Format("<span title='{0}'>{1}...</span>", value.Replace("'", "&#39;"), HttpUtility.HtmlEncode(value.Substring(0, maxLength - 3)));
            }
            else
            {
                if (hasSpanReturn)
                {
                    value = string.Format("<span>{0}</span>", HttpUtility.HtmlEncode(value));
                }
            }

            return value;
        }

        /// <summary>
        /// Truncates the with tip.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="maxLength">Length of the max.</param>
        /// <returns>System string.</returns>
        public static string TruncateWithTip(string value, int maxLength)
        {
            return TruncateWithTip(value, maxLength, false);
        }

        #endregion String

        /// <summary>
        /// Formats the handicap.
        /// </summary>
        /// <param name="betTypeId">The bet type identifier.</param>
        /// <param name="handicapValue">The handicap value.</param>
        /// <returns>Handicap formatted.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        public static string FormatHandicap(int betTypeId, decimal handicapValue)
        {
            var handicapBetType = 1;
            var _1stHandicapBetType = 7;
            var isHandicap = betTypeId == handicapBetType || betTypeId == _1stHandicapBetType;

            var handicapSign = string.Empty;

            if (isHandicap && handicapValue != 0)
            {
                handicapSign = handicapValue > 0 ? "+" : "-";
            }

            handicapValue = Math.Abs(handicapValue);

            if ((handicapValue * 10) % 5 == 0)
            {
                return string.Format(" {0}{1}", handicapSign, Formatter.DecFormat(handicapValue, 1, true));
            }
            else
            {
                return string.Format(" {0}{1}/{2}", handicapSign, Formatter.DecFormat(handicapValue - (decimal)0.25, 1, true), Formatter.DecFormat(handicapValue + (decimal)0.25, 1, true));
            }
        }
    }
}