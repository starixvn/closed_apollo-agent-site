// --------------------------------------------------------------------------
// <copyright file="AgentDownlinePathCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Agent down line path criteria.
    /// </summary>
    public class AgentDownlinePathCriteria : CriteriaBase
    {
        public int ParentId { get; set; }

        public int AgentId { get; set; }

        public override bool IsValid()
        {
            return AgentId > 0 && ParentId > 0;
        }

        public override string GetSettingKey()
        {
            return "Age_Agent_GetAgentDownlinePath";
        }
    }
}
