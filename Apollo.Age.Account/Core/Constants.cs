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

    /// <summary>
    /// Constants class.
    /// </summary>
    public static class Constants
    {
        public const string Normal = "normal";

        public const string Special = "special";

        public const string AllSpecial = "allSpecial";

        public const string Private = "private";

        public const string LastPostTimeKey = "lastPostTime";

        public const int LogActionDiscount = 2;

        public const int LogActionBetSetting = 3;

        public const int LogActionPositionTaking = 4;

        public static readonly Dictionary<int, string> AgentLevelDictionary = new Dictionary<int, string>
        {
            { 0, "lblSuper" },
            { 1, "lblMainSH" },
            { 2, "lblShareholder" },
            { 3, "lblMainAgent" },
            { 4, "agent" }
        };
    }
}
