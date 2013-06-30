// --------------------------------------------------------------------------
// <copyright file="SoccerCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.29">Created</change>
// </history>

namespace Apollo.Age.Wagers.Criterias.Soccer
{
    using System;
    using Apollo.Age.Wagers.Criterias;
    using Fanex.NAP.Framework.Data;

    public class SoccerCriteria : WagerCriteriaBase
    {
        public override string GetSettingKey()
        {
            var settingKey = string.Empty;

            switch (GroupBetName)
            {
                case "straight":
                    settingKey = "Age_Wagers_SC_1_3_5";
                    break;
                case "firsthalf":
                    settingKey = "Age_Wagers_SC_7_8_15";
                    break;
                case "runningball":
                    settingKey = "Age_Wagers_SC_1_3_7_8_RunBall";
                    break;
                case "special":
                    settingKey = "Age_Wagers_SC_Special";
                    break;
                default:
                    settingKey = "Age_Wagers_SC_2_12_6";
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