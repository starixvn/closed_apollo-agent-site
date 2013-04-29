// --------------------------------------------------------------------------
// <copyright file="Constant.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Account.Core
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Constants
    {
        public const string NORMAL = "normal";

        public const string SPECIAL = "special";

        public const string ALLSPECIAL = "allSpecial";

        public const string PRIVATE = "private";

        public const string LASTPOSTTIMEKEY = "lastPostTime";

        public const int MAIN_SHAREHOLDER_LEVEL = 1;

        #region const for read XML PTGrid
        public const string PATH_XML = "\\CustSettingEngine\\PTGrid.xml";

        public const string SPORT_TYPE_NODE_XML = "PTGrid/SportType";

        public const string BET_TYPE_NODE_XML = "BetTypeGroup/BetType";

        public const string ATTRIBUTE_SPORT_ID = "id";

        public const string ATTRIBUTE_BETTYPE_ID = "id";

        public const string ATTRIBUTE_SPORT_NAME = "name";

        public const string ATTRIBUTE_BETTYPE_NAME = "name";

        public const string ATTRIBUTE_SPORT_LIVE = "isLive";

        public const string ATTRIBUTE_RESOURCE_KEY = "resourcekey";

        public const string MIX_SPORT_PARLAY = "Mix Sports Parlay";
        #endregion
    }
}
