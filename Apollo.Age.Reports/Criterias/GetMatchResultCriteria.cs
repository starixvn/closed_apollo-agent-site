// --------------------------------------------------------------------------
// <copyright file="GetMatchResultCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.5.20">Created</change>
// </history>

namespace Apollo.Age.Reports.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetMatchResultCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "FromDate")]
        public DateTime EventDate { get; set; }

        public int LeagueId { get; set; }

        public string Language { get; set; }

        [SpParam(SqlParamName = "SportType")]
        public int SportId { get; set; }

        [SpParam(SqlParamName = "IsShowAll")]
        public bool IsTest { get; set; }

        [SpParam(Ignored=true)]
        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Result_MatchSelByLeague";
        }

        public override bool IsValid()
        {
            return this.EventDate <= DateTime.Now && this.EventDate >= BalanceUpDay;
        }
    }
}
