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
        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "Sporttypes")]
        public string Sporttypes { get; set; }

        [SpParam(SqlParamName = "IsShowAll")]
        public bool IsTest { get; set; }

        [SpParam(Ignored = true)]
        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Result_MatchSelBySport";
        }

        public override bool IsValid()
        {
            return this.FromDate <= DateTime.Now && this.FromDate >= BalanceUpDay;
        }
    }
}
