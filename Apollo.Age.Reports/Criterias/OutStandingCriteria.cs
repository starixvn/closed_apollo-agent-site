// --------------------------------------------------------------------------
// <copyright file="OutStandingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.29">Created</change>
// </history>

namespace Apollo.Age.Reports.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Outstanding criteria.
    /// </summary>
    public class OutstandingCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string Sporttypes { get; set; }

        public string Bettypes { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Rpt_Outstanding";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}