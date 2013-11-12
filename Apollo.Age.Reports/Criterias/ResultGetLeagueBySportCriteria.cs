// --------------------------------------------------------------------------
// <copyright file="ResultGetLeagueBySportCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Result get league by sport criteria.
    /// </summary>
    public class ResultGetLeagueBySportCriteria : CriteriaBase
    {
        public DateTime EventDate { get; set; }

        public byte SportType { get; set; }

        public bool IsShowAll { get; set; }

        [SpParam(Ignored = true)]
        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Result_LeagueSelByDate";
        }

        public override bool IsValid()
        {
            return EventDate <= DateTime.Now && EventDate >= BalanceUpDay;
        }
    }
}
