// --------------------------------------------------------------------------
// <copyright file="MemberOutstandingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.5.20">Created</change>
// </history>

namespace Apollo.Age.Reports.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Member outstanding criteria.
    /// </summary>
    public class MemberOutstandingCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string Sporttypes { get; set; }

        public string Bettypes { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Rpt_Outstanding_Member";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}