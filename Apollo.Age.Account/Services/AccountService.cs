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

        public List<Customer> GetCustomerSuggest(bool isMember, string userName, int agentId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");

            CustomerSearchCriteria criteria = new CustomerSearchCriteria()
            {
                AgentId = agentId,
                IsMember = isMember,
                UserName = userName
            };

            return _accountRepository.GetCustomerSuggest(criteria).ToList();
        }

        public CustomerInfoModel GetCustomerInfo(bool isMember, int custId, string userName, int agentId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");

            CustomerExistCriteria criteriaExist = new CustomerExistCriteria()
            {
                CurrentAgentId = agentId,
                IsMember = isMember,
                CustId = custId,
                UserName = userName
            };

            if (_accountRepository.IsExistValueSearch(criteriaExist))
            {
                AccountInfoCriteria criteria = new AccountInfoCriteria()
                {
                    CurrentAgentId = agentId,
                    IsMember = isMember,
                    CustId = custId,
                    UserName = userName
                };
                return _accountRepository.GetCustomerInfo(criteria);
            }
            else
            {
                return null;
            }
        }

        public List<Member> GetMemberDownline(int agentId, int pageIndex, int pageSize, ref int totalPage)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");

            MemberDownlineCriteria criteria = new MemberDownlineCriteria()
            {
                AgentId = agentId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPage
            };

            var memberList = _accountRepository.GetMemberDownline(ref criteria).ToList<Member>();
            totalPage = criteria.TotalPages;

            return memberList;
        }

        public List<Agent> GetAgentDownlinePath(int parentId, int agentId)
        {
            Check.Argument.IsNotNegative(parentId, "parentId");
            Check.Argument.IsNotNegative(agentId, "agentId");

            AgentDownlinePathCriteria criteria = new AgentDownlinePathCriteria()
            {
                ParentId = parentId,
                AgentId = agentId
            };
            return _agentRepository.Fetch(criteria).ToList();
        }

        public List<Agent> GetDirectDownline(int agentId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");

            return _agentRepository.Fetch(new AgentDownlineCriteria
            {
                AgentId = agentId
            }).ToList();
        }

        public List<AgentShareModel> GetAgentShare(int actorAgentId, int agentId, string userName, byte sportType, int betType)
        {
            Check.Argument.IsNotNegative(actorAgentId, "actorAgentId");
            Check.Argument.IsNotNegativeOrZero(agentId, "agentId");
            Check.Argument.IsNotNegative(sportType, "sportType");
            Check.Argument.IsNotNegative(betType, "betType");

            var criteria = new AgentShareCriteria() { 
                ActorAgentId = actorAgentId,
                AgentId = agentId,
                Username = userName,
                SporType = sportType,
                BetType = betType
            };

            return _accountRepository.GetAgentShare(criteria).ToList();
        }

        public List<MemberShareModel> GetMemberShare(int actorAgentId, int memberId, string userName, byte sportType, int betType)
        {
            Check.Argument.IsNotNegative(actorAgentId, "actorAgentId");
            Check.Argument.IsNotNegativeOrZero(memberId, "memberId");
            Check.Argument.IsNotNegative(sportType, "sportType");
            Check.Argument.IsNotNegative(betType, "betType");

            var criteria = new MemberShareCriteria()
            {
                ActorAgentId = actorAgentId,
                CustId = memberId,
                Username = userName,
                SporType = sportType,
                BetType = betType
            };

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
    }
}