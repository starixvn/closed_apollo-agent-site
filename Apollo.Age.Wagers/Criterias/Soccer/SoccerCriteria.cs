// --------------------------------------------------------------------------
// <copyright file="SoccerCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.29">Created</change>
// </history>

using System;
using Fanex.NAP.Framework.Data;

namespace Apollo.Age.WebApp
{
    public class SoccerCriteria : CriteriaBase
    {
        private string _groupBetName = string.Empty;

        [SpParam(Ignored = true)]
        public string GroupBetName
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_groupBetName) ? _groupBetName.ToLower() : string.Empty;
            }
            set
            {
                _groupBetName = value;
            }
        }


        public int LoggedId { get; set; }

        public DateTime FromDate { get; set; }

        public string UGroup { get; set; }

        public bool IsDirectMem { get; set; }

        public int LeagueId { get; set; }




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