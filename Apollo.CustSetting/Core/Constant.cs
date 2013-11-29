// --------------------------------------------------------------------------
// <copyright file="Constant.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.5">Created</change>
// </history>

namespace Apollo.CustSetting.Core
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml.Linq;
    using Fanex.Common;
    using Apollo.CustSetting.ViewData;

    /// <summary>
    /// Constant information.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2100:CodeLineMustNotBeLongerThan",
            Justification = "Reviewed. Suppression is OK here.")]
    public static class Constant
    {
        public const string PT_GRID_CONFIG = "~/CustSettingEngine/PTGrid.xml";
        public const string BETTING_LIMIT_SPORT_NAMES_FILE = "~/CustSettingEngine/BettingLimitSportNames.xml";
        public const int ERROR_CODE_DEFAULT = 1000;
        public const int WARNING_CODE_DEFAULT = 1500;
        public const string WARNING_MESSAGE_DEFAULT = "There is no updating.";
        public const string FORMAT_XML_ROOT = "<ROOT>{0}</ROOT>";
        public const string FORMAT_AGENT_POSITION_TAKING_XML = "<PositionTaking SportType=\"{0}\" BetType=\"{1}\" IsLive=\"{2}\" MaxUpper=\"{3}\" MaxGiven=\"{4}\" MinGiven=\"{5}\"></PositionTaking>";
        public const string FORMAT_MEMBER_POSITION_TAKING_XML = "<PositionTaking SportType=\"{0}\" BetType=\"{1}\" IsLive=\"{2}\" PT=\"{3}\" ></PositionTaking>";
        public const string FORMAT_AGENT_COMMISSION_SETTING_XML = "<Discount SportType=\"{0}\" BetType=\"{1}\" DiscountA=\"{2}\" DiscountB=\"{3}\" DiscountC=\"{4}\" DiscountD=\"{5}\"></Discount>";
        public const string FORMAT_MEMBER_COMMISSION_SETTING_XML = "<Discount SportType=\"{0}\" BetType=\"{1}\" Discount=\"{2}\" ></Discount>";
        public const string FORMAT_BETTING_LIMIT_XML = "<BetLimit SportType=\"{0}\" BetType=\"{1}\" Min=\"{2}\" Max=\"{3}\" MaxPerMatch=\"{4}\"></BetLimit>";
        public const string FORMAT_BETTING_LIMIT_XML_3RD = "<BetLimit SportType=\"{0}\" BetType=\"{1}\" Min=\"{2}\" Max=\"{3}\" MaxPerMatch=\"{4}\"  MaxPerBall=\"{5}\"></BetLimit>";
        public const int NUMBER_GAME_SPORT_ID = 161;
        public const decimal MAX_POSITION_TAKING_AGENT_LEVEL_1 = 100;
        public const int AGENT_LEVEL_EXCEPTION_SETTING = 2;
        public const int STEP_MUL_DIV = 100;
        public const decimal ROUNDING_NUMBER = 0.0049m;
        public const int NUMBER_DIGIST = 2;
        public const string DEFAULT_LANGUAGE = "en-US";

        //// Language
        public const string LANGUAGE_ENGLISH = "EN-en";

        public const string LANGUAGE_CHINESE_SIMPLIFIED = "CHSP-chsp";

        static Constant()
        {
            BETTING_LIMIT_ITEMS_DEFAULT = new Dictionary<string, BettingLimit>();
            COMMISSION_DEFAULT_VALUE = new decimal[] { 0.0225m, 0.0125m, 0.0075m, 0.0m, 0.0m, 0.0m, 0.0m };
            LoadBettingLimitSportNames();
        }

        public static Dictionary<string, BettingLimit> BETTING_LIMIT_ITEMS_DEFAULT { get; set; }

        public static decimal[] COMMISSION_DEFAULT_VALUE { get; set; }

        /// <summary>
        /// Load betting limit from xml file.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NexcelRules.NexcelRulesChecking", "EX1003:MethodMustBeDocumented", Justification = "Checked it.")]
        private static void LoadBettingLimitSportNames()
        {
            XElement root = _Configuration.BettingLimitSportNameConfig.Root;
            foreach (XElement sport in root.Elements())
            {
                BettingLimit bettingLimit = new BettingLimit
                {
                    SportId = Converter.ToInt(sport.Attribute("sportId").Value),
                    BetTypeId = Converter.ToInt(sport.Attribute("betTypeId").Value),
                    Name = sport.Attribute("name").Value,
                    ResourceKey = sport.Attribute("resourcekey").Value
                };
                BETTING_LIMIT_ITEMS_DEFAULT.Add(string.Format("{0}_{1}", bettingLimit.SportId, bettingLimit.BetTypeId), bettingLimit);
            }
        }
    }
}