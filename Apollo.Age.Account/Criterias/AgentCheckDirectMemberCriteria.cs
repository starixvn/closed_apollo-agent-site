// --------------------------------------------------------------------------
// <copyright file="AgentCheckDirectMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.24">Created</change>
// </history>

namespace Apollo.Age.Account.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Agent check direct member criteria.
    /// </summary>
    public class AgentCheckDirectMemberCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override bool IsValid()
        {
            return AgentId > 0;
        }

        public override string GetSettingKey()
        {
            return "Age_Agent_CheckDirectMember";
        }
    }
}
