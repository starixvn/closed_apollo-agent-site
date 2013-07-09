// --------------------------------------------------------------------------
// <copyright file="GetAgentInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.23">Created</change>
// </history>

namespace Apollo.Age.Account.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class GetAgentInfoCriteria : CriteriaBase
    {
        public int ActorAgentId { get; set; }

        public int AgentId { get; set; }

        public string Username { get; set; }

        public bool ExcludeActor { get; set; }

        public override bool IsValid()
        {
            return ActorAgentId > 0 && AgentId > 0;
        }

        public override string GetSettingKey()
        {
            return "Age_Agent_GetAllInfo";
        }
    }
}
