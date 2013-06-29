// --------------------------------------------------------------------------
// <copyright file="GetWagers_1_3_5_Criteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.29">Created</change>
// </history>

namespace Apollo.Age.Wagers.Criterias.Soccer
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetWagersStraightCriteria : CriteriaBase
    {
        public int LoggedId { get; set; }

        public DateTime FromDate { get; set; }

        public string UGroup { get; set; }

        public string Sporttypes { get; set; }

        public bool LiveIndicator { get; set; }

        public bool IsDirectMem { get; set; }

        public int LeagueId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Wagers_SC_1_3_5";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
