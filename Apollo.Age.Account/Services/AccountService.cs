// --------------------------------------------------------------------------
// <copyright file="AccountService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.8">Created</change>
// </history>

namespace Apollo.Age.Account.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Repositories.Impl;
    using Apollo.Age.Account.Repositories.Interface;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Services.Impl;
    using Fanex.Common.Helper;
    using Apollo.Age.Account.Models;

    public class AccountService : ServiceBase, IAccountService
    {
        private readonly IAgentListRepository _agentListRepository;
        private readonly ICustomerListRepository _customerListRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAgentRepository _agentRepository;

        public AccountService() : this(null, null) { }

        public AccountService(IAgentListRepository agentListRepository = null, ICustomerListRepository customerListRepository = null)
        {
            _agentListRepository = agentListRepository ?? new AgentListRepository();
            _customerListRepository = customerListRepository ?? new CustomerListRepository();
            _accountRepository = _accountRepository ?? new AccountRepository();
            _agentRepository = _agentRepository ?? new AgentRepository();
        }

        public IEnumerable<AgentListViewModel> GetAgentList(ref AgentListCriteria criteria)
        {
            return _agentListRepository.GetData(ref criteria);
        }

        public IEnumerable<CustomerListViewModel> GetCustomerList(ref CustomerListCriteria criteria)
        {
            return _customerListRepository.GetData(ref criteria);
        }

        public List<Customer> GetCustomerSearch(bool isMember, string userName, int agentId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");

            CustomerSearchCriteria criteria = new CustomerSearchCriteria()
            {
                AgentId = agentId,
                IsMember = isMember,
                UserName = userName
            };

            return _accountRepository.GetCustomerSearch(criteria).ToList();
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
            Check.Argument.IsNotNegativeOrZero(agentId, "agentId");

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
    }
}