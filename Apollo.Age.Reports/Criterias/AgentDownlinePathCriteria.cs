// --------------------------------------------------------------------------
// <copyright file="AgentDownlinePathCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.5.20">Created</change>
// </history>

namespace Apollo.Age.Reports.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Agent down line path criteria.
    /// </summary>
    public class AgentDownlinePathCriteria : CriteriaBase
    {
        public int ParentId { get; set; }

        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Agent_GetAgentDownlinePath";
        }

        public override bool IsValid()
        {
            return ParentId > 0 && AgentId > 0;
        }
    }
}