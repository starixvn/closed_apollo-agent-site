// --------------------------------------------------------------------------
// <copyright file="AccountService.cs" company="Nexcel Solutions Vietnam">
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
    using System.Linq;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Account.Repositories.Impl;
    using Apollo.Age.Account.Repositories.Interface;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Services.Impl;
    using Fanex.Common.Helper;

    public class AccountService : ServiceBase, IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAgentRepository _agentRepository;

        public AccountService()
        {
            _accountRepository = _accountRepository ?? new AccountRepository();
            _agentRepository = _agentRepository ?? new AgentRepository();
        }

        public IEnumerable<AgentListModel> GetAgentList(ref AgentListCriteria criteria)
        {
            return _accountRepository.GetAgentList(ref criteria);
        }

        public IEnumerable<MemberListModel> GetCustomerList(ref CustomerListCriteria criteria)
        {
            return _accountRepository.GetMemberList(ref criteria);
        }

        public List<Agent> GetAgentByUsername(GetAgentByUsernameCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.ActorAgentId, "actorAgentId");

            return _accountRepository.GetAgentByUsername(criteria).ToList();
        }

        public List<Member> GetMemberByUsername(GetMemberByUsernameCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.ActorAgentId, "actorAgentId");

            return _accountRepository.GetMemberByUsername(criteria).ToList();
        }

        public CustomerInfoModel GetAgentInfo(GetAgentInfoCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return _accountRepository.GetCustomerInfo(criteria);
        }

        public CustomerInfoModel GetMemberInfo(GetMemberInfoCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "actorAgentId");

            return _accountRepository.GetCustomerInfo(criteria);
        }

        public List<Member> GetMemberDownline(ref MemberDownlineCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.AgentId, "agentId");

            var memberList = _accountRepository.GetMemberDownline(ref criteria).ToList<Member>();

            return memberList;
        }

        public List<Agent> GetAgentDownlinePath(AgentDownlinePathCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.ParentId, "parentId");
            Check.Argument.IsNotNegative(criteria.AgentId, "agentId");

            return _agentRepository.Fetch(criteria).ToList();
        }

        public List<Agent> GetDirectDownline(AgentDownlineCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.AgentId, "agentId");

            return _agentRepository.Fetch(criteria).ToList();
        }

        public List<AgentShareModel> GetAgentShare(AgentShareCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.ActorAgentId, "actorAgentId");
            Check.Argument.IsNotNegative(criteria.SportType, "sportType");
            Check.Argument.IsNotNegative(criteria.BetType, "betType");

            return _accountRepository.GetAgentShare(criteria).ToList();
        }

        public List<MemberShareModel> GetMemberShare(MemberShareCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.ActorAgentId, "actorAgentId");
            Check.Argument.IsNotNegative(criteria.SportType, "sportType");
            Check.Argument.IsNotNegative(criteria.BetType, "betType");

            return _accountRepository.GetMemberShare(criteria).ToList();
        }

        public IEnumerable<CustomerCommissionModel> GetCommissionToMember(GetCommissionToMemberCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return _accountRepository.GetCommissionToMember(criteria);
        }

        public IEnumerable<CustomerCommissionModel> GetCommissionToAgent(GetCommissionToAgentCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return _accountRepository.GetCommissionToAgent(criteria);
        }

        public IEnumerable<AgentLogLoginModel> GetAgentLogLogin(GetLoginLogCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return _accountRepository.GetAgentLogLogin(criteria);
        }
    }
}