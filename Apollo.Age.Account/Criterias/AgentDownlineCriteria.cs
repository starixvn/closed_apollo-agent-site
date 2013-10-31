// --------------------------------------------------------------------------
// <copyright file="AgentDownlineCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.22">Created</change>
// </history>

namespace Apollo.Age.Criterias.Accounts
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Agent downline criteria.
    /// </summary>
    public class AgentDownlineCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override bool IsValid()
        {
            return AgentId > 0;
        }

        public override string GetSettingKey()
        {
            return "Age_Agent_GetAgentDownline";
        }
    }
}
