// --------------------------------------------------------------------------
// <copyright file="GetMemberByUsernameCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.23">Created</change>
// </history>

namespace Apollo.Age.Account.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get member by username criteria.
    /// </summary>
    public class GetMemberByUsernameCriteria : CriteriaBase
    {
        public int ActorAgentId { get; set; }

        public string Username { get; set; }

        public override bool IsValid()
        {
            return ActorAgentId > 0;
        }

        public override string GetSettingKey()
        {
            return "Age_Customer_GetByUsername";
        }
    }
}
