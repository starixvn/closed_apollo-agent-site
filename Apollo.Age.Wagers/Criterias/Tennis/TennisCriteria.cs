// --------------------------------------------------------------------------
// <copyright file="TennisCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.30">Created</change>
// </history>

namespace Apollo.Age.Wagers.Criterias.Tennis
{
    using Apollo.Age.Wagers.Criterias;

    public class TennisCriteria : WagerCriteriaBase
    {
        public override string GetSettingKey()
        {
            var settingKey = string.Empty;

            switch (GroupBetName)
            {
                case "straight":
                    settingKey = "Age_Wagers_TN_1_3_5";
                    break;
                case "runningball":
                    settingKey = "Age_Wagers_TN_1_3_7_8_RunBall";
                    break;
                case "special":
                    settingKey = "Age_Wagers_TN_Special";
                    break;
                default:
                    settingKey = "Age_Wagers_TN_Parlay";
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