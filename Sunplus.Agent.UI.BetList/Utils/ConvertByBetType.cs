namespace Sunplus.Agent.UI.BetList.Utils
{
    using System;
    using System.Collections.Generic;
    using Sunplus.Agent.Utilities;

    public static partial class ConvertByBetType
    {        
        #region [f_GetOddsByBettype], [f_ShowOddsByBettype], [f_GetChoice]

        public const string FontOpenTag = "<font color='#B50000'>";
        public const string FontCloseTag = "</font>";
        private static List<int> oddsBettypes = new List<int>(new int[] { 1, 2, 3, 7, 12, 8, 20, 124, 125, 81, 82, 83, 84, 85, 86, 87, 88 });
        private static string minus = "<font color='#B50000'>{0}</font>";        
        
        public static string Minus
        {
            get { return ConvertByBetType.minus; }
            set { ConvertByBetType.minus = value; }
        }

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
                    returnValue = Resources.Lang.home;
                }
                else if (chars[0] == '2')
                {
                    returnValue = Resources.Lang.away;
                }
                else
                {
                    returnValue = Resources.Lang.no0;
                }

                if (chars[c - 1] == '1')
                {
                    returnValue += " " + Resources.Lang.FG;
                }
                else if (chars[c - 1] == '2')
                {
                    returnValue += " " + Resources.Lang.LG;
                }
                else
                {
                    returnValue += " " + Resources.Lang.goal;
                }
            }
            else if (betType == 16) 
            {
                // HTFT
                if (chars[0] == '1')
                {
                    returnValue = Resources.Lang.h;
                }
                else if (chars[0] == '2')
                {
                    returnValue = Resources.Lang.a;
                }
                else
                {
                    returnValue = Resources.Lang.d;
                }

                if (chars[c - 1] == '1')
                {
                    returnValue += Resources.Lang.h;
                }
                else if (chars[c - 1] == '2')
                {
                    returnValue += Resources.Lang.a;
                }
                else
                {
                    returnValue += Resources.Lang.d;
                }
            }
            else
            {
                return betTeam;
            }

            return returnValue;
        }

        #endregion        
        
        public static string Hdp(object value)
        {
            return Cast.AsDecimal(value).ToString("0.##");
        }

        public static string Odds(object value)
        {
            decimal d = Cast.AsDecimal(value);

            if (d < 0)
            {
                return string.Format(minus, string.Format(Formater.DECIMAL_FORMAT3, d));
            }
            else if (d == 0)
            {
                return "-";
            }
            else
            {
                return string.Format(Formater.DECIMAL_FORMAT3, d);
            }
        }

        public static string Odds(object odds, object betType, object oddsType)
        {
            double d = ConvertByBetType.GetOddsByBetType(Convert.ToDouble(odds), Convert.ToInt32(betType), Convert.ToInt32(oddsType));
            if (d < 0)
            {
                return string.Format(minus, string.Format(Formater.DECIMAL_FORMAT3, d));
            }
            else if (d == 0)
            {
                return "-";
            }
            else
            {
                return string.Format(Formater.DECIMAL_FORMAT3, d);
            }
        }

        public static object OddsType(object betType, object oddsType)
        {
            return ConvertByBetType.ShowOddsByBettype(Convert.ToInt32(betType), Convert.ToInt32(oddsType));
        }

        public static string MOdds(object value)
        {
            decimal d = Cast.AsDecimal(value);

            if (d < 0)
            {
                return string.Format(minus, string.Format(Formater.DECIMAL_FORMAT3, d));
            }
            else
            {
                return string.Format(Formater.DECIMAL_FORMAT3, d);
            }
        }

        public static string MOdds4(object value)
        {
            decimal d = Cast.AsDecimal(value);

            if (d < 0)
            {
                return string.Format(minus, string.Format(Formater.DECIMAL_FORMAT4, d));
            }
            else
            {
                return string.Format(Formater.DECIMAL_FORMAT4, d);
            }
        }

        public static string Winloss(object value)
        {
            decimal d = Cast.AsDecimal(value);

            if (d < 0)
            {
                return string.Format(minus, string.Format(Formater.DECIMAL_FORMAT2, d));
            }
            else
            {
                return string.Format(Formater.DECIMAL_FORMAT2, d);
            }
        }

        public static string Comm(object value)
        {
            return string.Format(Formater.DECIMAL_FORMAT2, value);
        }

        public static string Comm_Percentage(object value)
        {
            return string.Format(Formater.DECIMAL_FORMAT2, Cast.AsDecimal(value) * 100);
        }

        public static string CommValue(object value)
        {
            decimal d = Cast.AsDecimal(value);

            if (d < 0)
            {
                return string.Format(minus, string.Format(Formater.DECIMAL_FORMAT40, d));
            }
            else
            {
                return string.Format(Formater.DECIMAL_FORMAT40, d);
            }
        }

        public static string Stake(object value)
        {
            return string.Format(Formater.DECIMAL_FORMAT2, value);
        }

        public static string Pt(object value)
        {
            return (Cast.AsDecimal(value) * 100).ToString("N0");
        }

        public static string PtValue(object value)
        {
            decimal d = Cast.AsDecimal(value);

            if (d < 0)
            {
                return string.Format(minus, string.Format(Formater.DECIMAL_FORMAT20, d));
            }
            else
            {
                return string.Format(Formater.DECIMAL_FORMAT20, d);
            }
        }
    }
}