// --------------------------------------------------------------------------
// <copyright file="GetWagersRunningBallCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class WagersRunningBallCriteria : CriteriaBase
    {
        public int LoggedId { get; set; }

        public DateTime FromDate { get; set; }

        public string UGroup { get; set; }

        public bool IsDirectMem { get; set; }

        public int LeagueId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Wagers_SC_1_3_7_8_RunBall";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}