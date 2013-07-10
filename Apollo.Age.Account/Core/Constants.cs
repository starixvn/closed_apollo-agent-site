// --------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Account.Core
{
    using System.Collections.Generic;
    using Apollo.Age.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Constants
    {
        public const string Normal = "normal";

        public const string Special = "special";

        public const string AllSpecial = "allSpecial";

        public const string Private = "private";

        public const string LastPostTimeKey = "lastPostTime";

        public const int MainShareHolderLevel = 1;

        public const int ShareHolderLevel = 2;

        public const int LogActionDiscount = 2;

        public const int LogActionBetSetting = 3;

        public const int LogActionPositionTaking = 4;

        //public static readonly IList<string> BetTypesHeader = new List<string>
        //{
        //                    Lang.HandicapOverUnderOddEven, 
        //                    Lang.RBHandicapRBOverUnder, 
        //                    Lang._1x2RB1x2, 
        //                    Lang.OtherBetType, 
        //                    Lang.RBOtherBetType,
        //                    Lang.MixSportParlay,
        //                    Lang.lblOutRight
        //            };
    }
}
