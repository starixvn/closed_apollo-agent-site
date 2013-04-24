// --------------------------------------------------------------------------
// <copyright file="IAccountService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.8">Created</change>
//   <change who="Lewis.Le" date="2013.4.18">Added GetCommissionToMember, GetCommissionToAgent</change>
// </history>

namespace Apollo.Age.Account.Services
{
    using System.Collections.Generic;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Entities;
    using Apollo.Age.Account.Criterias;

    public interface IAccountService
    {
        IEnumerable<AgentListModel> GetAgentList(ref AgentListCriteria criteria);

        IEnumerable<MemberListModel> GetCustomerList(ref CustomerListCriteria criteria);

        List<Agent> GetAgentByUsername(int actorAgentId, string username);

        List<Member> GetMemberByUsername(int actorAgentId, string username);

        CustomerInfoModel GetAgentInfo(int actorAgentId, int agentId, string username);

        CustomerInfoModel GetMemberInfo(int actorAgentId, int custId, string username);

        List<Member> GetMemberDownline(int agentId, int pageIndex, int pageSize, ref int totalPage);

        List<Agent> GetAgentDownlinePath(int parentId, int agentId);

        List<Agent> GetDirectDownline(int agentId);

        List<AgentShareModel> GetAgentShare(AgentShareCriteria criteria);

        List<MemberShareModel> GetMemberShare(MemberShareCriteria criteria);

        List<Sport> GetAllSportType();

        List<BetType> GetBetTypeBySport(short sportId);

        IEnumerable<CustomerCommissionModel> GetCommissionToMember(GetCommissionToMemberCriteria criteria);

        IEnumerable<CustomerCommissionModel> GetCommissionToAgent(GetCommissionToAgentCriteria criteria);

    }
}
