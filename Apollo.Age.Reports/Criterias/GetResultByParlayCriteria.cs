// --------------------------------------------------------------------------
// <copyright file="GetResultByParlayCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get result by parlay criteria.
    /// </summary>
    public class GetResultByParlayCriteria : CriteriaBase
    {
        public long RefNo { get; set; }

        public override string GetSettingKey()
        {
            return "Age_BetList_SelMatchResultParlay";
        }

        public override bool IsValid()
        {
            return RefNo > 0;
        }
    }
}
