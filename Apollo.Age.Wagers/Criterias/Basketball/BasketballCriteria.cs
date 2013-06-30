// --------------------------------------------------------------------------
// <copyright file="BasketballCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.30">Created</change>
// </history>

namespace Apollo.Age.Wagers.Criterias.Basketball
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BasketballCriteria : WagerCriteriaBase
    {
        public string Sporttypes { get; set; }

        public override string GetSettingKey()
        {
            var settingKey = string.Empty;

            switch (GroupBetName)
            {
                case "straight":
                    settingKey = "Age_Wagers_BK_1_2_3";
                    break;
                case "firsthalf":
                    settingKey = "Age_Wagers_BK_7_8_12";
                    break;
                case "runningball":
                    settingKey = "Age_Wagers_BK_1_3_RunBall";
                    break;
                case "parlay":
                    settingKey = "";
                    break;
                default:
                    settingKey = "Age_Wagers_BK_Special";
                    break;
            }

            return settingKey;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}