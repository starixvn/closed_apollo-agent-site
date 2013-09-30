// --------------------------------------------------------------------------
// <copyright file="TennisCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.30">Created</change>
// </history>




namespace Apollo.Age.Wagers.Criterias.Basebal
{
    using System;
using Fanex.NAP.Framework.Data;
using Apollo.Age.Wagers.Criterias;
    public class BaseballCriteria : WagerCriteriaBase
    {
                       
        public override string GetSettingKey()
        {
            var settingKey = string.Empty;

            switch (base.GroupBetName)
            {
                case "straight":
                    settingKey = "Age_Wagers_BS_1_3_5";
                    break;
                case "firsthalf":
                    settingKey = "Age_Wagers_BS_7_8_15";
                    break;
                case "runningball":
                    settingKey = "Age_Wagers_BS_1_3_7_8_RunBall";
                    break;
                case "special":
                    settingKey = "Age_Wagers_BS_Special";
                    break;
                default:
                    settingKey = "Age_Wagers_BS_Parlay";
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