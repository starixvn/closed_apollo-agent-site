// --------------------------------------------------------------------------
// <copyright file="GetResultByMatchCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.5.2">Created</change>
// </history>

namespace Apollo.Age.Reports.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get result by match criteria.
    /// </summary>
    public class GetResultByMatchCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_BetList_SelMatchResult";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}
