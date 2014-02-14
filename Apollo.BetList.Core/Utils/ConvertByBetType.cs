// ---------------------------------------------------------------------------------
// <copyright file="ConvertByBetType.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
//     <change who="Marc.Bui" date="2013.01.23">Update format string for Stake to follow
//                                              general rules: show all significant decimals</change>
//     <change who="Marc.Bui" date="2013.01.29">Update show Odds value: 0 instead of -</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Convert by bet type.
    /// </summary>
    public static partial class ConvertByBetType
    {
        #region [f_GetOddsByBettype], [f_ShowOddsByBettype], [f_GetChoice]

        public const string FontOpenTag = "<span class='red-color'>";
        public const string FontCloseTag = "</span>";
        private static List<int> oddsBettypes = new List<int>(new int[] { 1, 2, 3, 7, 12, 8, 20, 124, 125, 81, 82, 83, 84, 85, 86, 87, 88 });
        private static string minus = "<span class='red-color'>{0}</span>";

        public static string Minus
        {
            get { return ConvertByBetType.minus; }
            set { ConvertByBetType.minus = value; }
        }

        /// <summary>
        /// Shows the odds by bet type.
        /// </summary>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="oddsType">Type of the odds.</param>
        /// <returns>Return odd by bet type.</returns>
        public static int ShowOddsByBettype(int betType, int oddsType)
        {
            if (oddsBettypes.Contains(betType))
            {
                return oddsType;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the type of the odds by bet.
        /// </summary>
        /// <param name="odds">Input the odds.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="oddsType">Type of the odds.</param>
        /// <returns>
        /// Return odd by bet type.
        /// </returns>
        public static double GetOddsByBetType(double odds, int betType, int oddsType)
        {
            bool isShownOddType = ShowOddsByBettype(betType, oddsType) != 0;
            if (isShownOddType && oddsType == 1)
            {
                return odds + 1;
            }
            else if (isShownOddType && oddsType == 5)
            {
                return odds * 100;
            }
            else
            {
                return odds;
            }
        }

        /// <summary>
        /// Gets the bet team.
        /// </summary>
        /// <param name="betTeam">The bet team.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <returns>Return bet team.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public static string GetBetTeam(string betTeam, int betType)
        {
            if (string.IsNullOrEmpty(betTeam))
            {
                return betTeam;
            }

            char[] chars = betTeam.ToCharArray();
            int c = chars.Length;

            string returnValue = string.Empty;

            // FGLG
            if (betType == 14)
            {
                if (chars[0] == '1')
                {
                    returnValue = Apollo.Age.Resources.BetList.home;
                }
                else if (chars[0] == '2')
                {
                    returnValue = Apollo.Age.Resources.BetList.away;
                }
                else
                {
                    returnValue = Apollo.Age.Resources.BetList.no0;
                }

                if (chars[c - 1] == '1')
                {
                    returnValue += "&nbsp;" + Apollo.Age.Resources.BetList.FG;
                }
                else if (chars[c - 1] == '2')
                {
                    returnValue += "&nbsp;" + Apollo.Age.Resources.BetList.LG;
                }
                else
                {
                    returnValue += "&nbsp;" + Apollo.Age.Resources.BetList.goal;
                }
            }
            else if (betType == 16)
            {
                // HTFT
                if (chars[0] == '1')
                {
                    returnValue = Apollo.Age.Resources.BetList.h;
                }
                else if (chars[0] == '2')
                {
                    returnValue = Apollo.Age.Resources.BetList.a;
                }
                else
                {
                    returnValue = Apollo.Age.Resources.BetList.d;
                }

                if (chars[c - 1] == '1')
                {
                    returnValue += Apollo.Age.Resources.BetList.h;
                }
                else if (chars[c - 1] == '2')
                {
                    returnValue += Apollo.Age.Resources.BetList.a;
                }
                else
                {
                    returnValue += Apollo.Age.Resources.BetList.d;
                }
            }
            else
            {
                return betTeam;
            }

            return returnValue;
        }

        #endregion [f_GetOddsByBettype], [f_ShowOddsByBettype], [f_GetChoice]

        /// <summary>
        /// HDPs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Handicap bet type.</returns>
        public static string Hdp(object value)
        {
            return Cast.AsDecimal(value).ToString("0.##");
        }

        /// <summary>
        /// Odds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>String odds.</returns>
        public static string Odds(object value)
        {
            decimal d = Cast.AsDecimal(value);

            if (d < 0)
            {
                return string.Format(minus, string.Format(Formatter.DECIMAL_FORMAT3, d));
            }
            else
            {
                return string.Format(Formatter.DECIMAL_FORMAT3, d);
            }
        }

        /// <summary>
        /// Odds the specified odds.
        /// </summary>
        /// <param name="odds">Input the odds.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="oddsType">Type of the odds.</param>
        /// <returns>String odds.</returns>
        public static string Odds(object odds, object betType, object oddsType)
        {
            double d = ConvertByBetType.GetOddsByBetType(Convert.ToDouble(odds), Convert.ToInt32(betType), Convert.ToInt32(oddsType));
            if (d < 0)
            {
                return string.Format(minus, string.Format(Formatter.DECIMAL_FORMAT3, d));
            }
            else if (d == 0)
            {
                return "-";
            }
            else
            {
                return string.Format(Formatter.DECIMAL_FORMAT3, d);
            }
        }

        /// <summary>
        /// Odds the type.
        /// </summary>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="oddsType">Type of the odds.</param>
        /// <returns>
        /// Odds type.
        /// </returns>
        public static object OddsType(object betType, object oddsType)
        {
            return ConvertByBetType.ShowOddsByBettype(Convert.ToInt32(betType), Convert.ToInt32(oddsType));
        }

        /// <summary>
        /// The M odds.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>String odds.</returns>
        public static string MOdds(object value)
        {
            decimal d = Cast.AsDecimal(value);

            if (d < 0)
            {
                return string.Format(minus, string.Format(Formatter.DECIMAL_FORMAT3, d));
            }
            else
            {
                return string.Format(Formatter.DECIMAL_FORMAT3, d);
            }
        }

        /// <summary>
        /// The M odds 4.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>String odds 4.</returns>
        public static string MOdds4(object value)
        {
            decimal d = Cast.AsDecimal(value);

            if (d < 0)
            {
                return string.Format(minus, string.Format(Formatter.DECIMAL_FORMAT4, d));
            }
            else
            {
                return string.Format(Formatter.DECIMAL_FORMAT4, d);
            }
        }

        /// <summary>
        /// Win loss the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>String win loss.</returns>
        public static string Winloss(object value)
        {
            decimal d = Cast.AsDecimal(value);

            if (d < 0)
            {
                return string.Format(minus, string.Format(Formatter.DECIMAL_FORMAT20, d));
            }
            else
            {
                return string.Format(Formatter.DECIMAL_FORMAT20, d);
            }
        }

        /// <summary>
        /// Wins the color of the loss no.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Win lost value within no red color.</returns>
        public static string WinLossNoColor(object value)
        {
            decimal d = Cast.AsDecimal(value);

            return string.Format(Formatter.DECIMAL_FORMAT20, d);
        }

        /// <summary>
        /// Commission the percentage.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>String commission.</returns>
        public static string Comm_Percentage(object value)
        {
            return (Cast.AsDecimal(value) * 100).ToString("N2");
        }

        /// <summary>
        /// Commission the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>String value commission.</returns>
        public static string CommValue(object value)
        {
            decimal d = Cast.AsDecimal(value);

            if (d < 0)
            {
                return string.Format(minus, string.Format(Formatter.DECIMAL_FORMAT40, d));
            }
            else
            {
                return string.Format(Formatter.DECIMAL_FORMAT40, d);
            }
        }

        /// <summary>
        /// Stakes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>String stake.</returns>
        public static string Stake(object value)
        {
            return string.Format(Formatter.DECIMAL_FORMAT20, value);
        }

        /// <summary>
        /// PT the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>String PT.</returns>
        public static string Pt(object value)
        {
            return (Cast.AsDecimal(value) * 100).ToString("N2");
        }

        /// <summary>
        /// Commission the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>String commission.</returns>
        public static string Comm(object value)
        {
            return string.Format(Formatter.DECIMAL_FORMAT20, value);
        }

        /// <summary>
        /// PT the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// String PT value.
        /// </returns>
        public static string PtValue(object value)
        {
            var newValue = string.Format(Formatter.DECIMAL_FORMAT20, value);
            decimal d = Cast.AsDecimal(newValue);

            if (d < 0)
            {
                return string.Format(minus, newValue);
            }
            else
            {
                return newValue;
            }
        }
    }
}