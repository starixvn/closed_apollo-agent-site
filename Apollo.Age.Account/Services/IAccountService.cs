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
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Entities;
    using Apollo.CustSetting.ViewData;

    public interface IAccountService
    {
        IEnumerable<AgentListModel> GetAgentList(ref AgentListCriteria criteria);

        IEnumerable<MemberListModel> GetCustomerList(ref CustomerListCriteria criteria);

        List<Agent> GetAgentByUsername(GetAgentByUsernameCriteria criteria);

        List<Member> GetMemberByUsername(GetMemberByUsernameCriteria criteria);

        CustomerInfoModel GetAgentInfo(GetAgentAllInfoCriteria criteria);

        CustomerInfoModel GetMemberInfo(GetMemberInfoCriteria criteria);

        List<Member> GetMemberDownline(ref MemberDownlineCriteria criteria);

        List<Agent> GetAgentDownlinePath(AgentDownlinePathCriteria criteria);

        List<Agent> GetDirectDownline(AgentDownlineCriteria criteria);

        List<AgentShareModel> GetAgentShare(AgentShareCriteria criteria);

        List<MemberShareModel> GetMemberShare(MemberShareCriteria criteria);

        IEnumerable<CustomerCommissionModel> GetCommissionToMember(GetCommissionToMemberCriteria criteria);

        IEnumerable<CustomerCommissionModel> GetCommissionToAgent(GetCommissionToAgentCriteria criteria);

        IEnumerable<AgentLogLoginModel> GetAgentLogLogin(GetLoginLogCriteria criteria);

        MemberListModel GetTopMember(int agentId);

        string CalculateUpdateAgentPositionTakingTime(SettingContext settingContext, PositionTakingSetting positionTakingSetting);

        bool HasMember(int agentId);

        bool IsInPeakHours(int agentLevel);
    }
}
