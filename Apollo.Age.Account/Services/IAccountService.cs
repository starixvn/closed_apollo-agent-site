// --------------------------------------------------------------------------
// <copyright file="IAccountService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.8">Created</change>
// </history>

namespace Apollo.Age.Account.Services
{
    using System.Collections.Generic;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;

    public interface IAccountService
    {
        CustomerInfoViewModel GetCustomerInfo(bool isMember, int custId, string userName, int agentId);

        List<Member> GetMemberDownline(ref MemberDownlineCriteria criteria);

        List<Agent> GetAgentDownlinePath(int parentId, int agentId);

        List<Agent> GetDirectDownline(int agentId);
    }
}
