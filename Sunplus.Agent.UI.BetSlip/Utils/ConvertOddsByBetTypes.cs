using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sunplus.Agent.UI.BetSlip.Utils
{
    public static class ConvertOddsByBetTypes
    {
        #region [f_GetOddsByBettype], [f_ShowOddsByBettype]

        static List<int> OddsBettypes = new List<int>(new int[] { 1, 2, 3, 7, 12, 8, 20, 124, 125, 81, 82, 83, 84, 85, 86, 87, 88 });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="betType"></param>
        /// <param name="oddType"></param>
        /// <returns></returns>

        public static int ShowOddsByBettype(int betType, int oddsType)
        {

            if (OddsBettypes.Contains(betType))
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

            if (betType == 14) // FGLG
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
            else if (betType == 16) // HTFT
            {
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
    }
}