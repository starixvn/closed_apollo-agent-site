// --------------------------------------------------------------------------
// <copyright file="MemberShareCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.17">Created</change>
// </history>

namespace Apollo.Age.Account.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class MemberShareCriteria : CriteriaBase
    {
        public int ActorAgentId { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public short SportType { get; set; }

        public short BetType { get; set; }

        public bool IsLive { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Agent_GetPositionTakingToCustomer";
        }

        public override bool IsValid()
        {
            return ActorAgentId > 0 && CustId > 0;
        }
    }
}
