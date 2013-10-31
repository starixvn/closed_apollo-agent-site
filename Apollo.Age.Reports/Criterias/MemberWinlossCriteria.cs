// --------------------------------------------------------------------------
// <copyright file="MemberWinlossCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.12">Created</change>
// </history>

namespace Apollo.Age.Reports.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Member win loss criteria.
    /// </summary>
    public class MemberWinLossCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string Sporttypes { get; set; }

        public string Bettypes { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public DateTime MoveBetDay { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Rpt_WinLoss_Member";
        }

        public override bool IsValid()
        {
            var check = AgentId > 0 && FromDate <= DateTime.Now;
            return check;
        }
    }
}
