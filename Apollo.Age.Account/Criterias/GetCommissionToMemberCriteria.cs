// --------------------------------------------------------------------------
// <copyright file="GetCommissionToMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.4.18">Created</change>
// </history>

namespace Apollo.Age.Account.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class GetCommissionToMemberCriteria : CriteriaBase
    {
        public int ActorAgentId { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Agent_GetDiscountToCustomer";
        }

        public override bool IsValid()
        {
            return ActorAgentId > 0;
        }
    }
}