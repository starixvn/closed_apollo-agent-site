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
    using Apollo.Age.Wagers.Criterias;

    /// <summary>
    /// SoccerCriteria class.
    /// </summary>
    public class SoccerCriteria : WagerCriteriaBase
    {
        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Setting key.</returns>
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
                    settingKey = "Age_Wagers_RunningBall";
                    break;
                case "specialbet":
                    settingKey = "Age_Wagers_SC_Special";
                    break;
                case "parlay":
                    settingKey = "Age_Wagers_SC_9_29";
                    break;
                default:
                    settingKey = "Age_Wagers_SC_2_12_6";
                    break;
            }

            return settingKey;
        }

        /// <summary>
        /// Check criteria is valid.
        /// </summary>
        /// <returns>If is valid return true.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}