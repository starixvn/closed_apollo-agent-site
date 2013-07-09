// --------------------------------------------------------------------------
// <copyright file="MemberDownlineCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.22">Created</change>
// </history>

namespace Apollo.Age.Criterias.Accounts
{
    using Fanex.NAP.Framework.Data;

    public class MemberDownlineCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public override bool IsValid()
        {
            return AgentId > 0;
        }

        public override string GetSettingKey()
        {
            return "Age_Agent_GetMemberDownline";
        }
    }
}
