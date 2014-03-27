// --------------------------------------------------------------------------
// <copyright file="IAccountRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.8">Created</change>
// </history>

namespace Apollo.Age.Account.Repositories.Interface
{
    using System.Collections.Generic;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Entities;
    using Fanex.NAP.Framework.Data;

    public interface IAccountRepository
    {
        IEnumerable<Agent> GetAgentByUsername(ICriteria criteria);

        IEnumerable<Member> GetMemberByUsername(ICriteria criteria);

        CustomerInfoModel GetCustomerInfo(ICriteria criteria);

        IEnumerable<Member> GetMemberDownline(ref MemberDownlineCriteria criteria);

        IEnumerable<AgentListModel> GetAgentList(ref AgentListCriteria criteria);

        IEnumerable<MemberListModel> GetMemberList(ref CustomerListCriteria criteria);

        IEnumerable<AgentShareModel> GetAgentShare(ICriteria criteria);

        IEnumerable<MemberShareModel> GetMemberShare(ICriteria criteria);

        IEnumerable<CustomerCommissionModel> GetCommissionToMember(GetCommissionToMemberCriteria criteria);

        IEnumerable<CustomerCommissionModel> GetCommissionToAgent(GetCommissionToAgentCriteria criteria);

        IEnumerable<AgentLogLoginModel> GetAgentLogLogin(GetLoginLogCriteria criteria);

        int CalculateUpdateAgentPositionTakingTime(GetUpdatePositionTakingEstimatedTimeCriteria criteria);

        bool HasMember(ICriteria criteria);
    }
}