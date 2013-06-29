// --------------------------------------------------------------------------
// <copyright file="WagersSpecialCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class WagersSpecialCriteria : CriteriaBase
    {
        public int LoggedId { get; set; }

        public DateTime FromDate { get; set; }

        public string UGroup { get; set; }

        public bool IsDirectMem { get; set; }

        public int LeagueId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Wagers_SC_Special";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}