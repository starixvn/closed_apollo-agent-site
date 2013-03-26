// --------------------------------------------------------------------------
// <copyright file="AcountInfoService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.22">Created</change>
// </history>

namespace Apollo.Age.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.DTO.Accounts;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Services.Interface;
    using Fanex.NAP.Framework.Data;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;

    public class AccountInfoService : IAccountInfoService
    {
        private readonly IAccountInfoRepository _accountIfoRepository;
        private readonly IDynamicRepository _repository;

        public AccountInfoService(IAccountInfoRepository userRepository = null)
        {
            _accountIfoRepository = _accountIfoRepository ?? new AccountInfoRepository();
        }

        //public Agent GetCustomerInfo(AccountInfoCriteria criteria)
        //{
        //    return _accountIfoRepository.GetAgentInfo(criteria);
        //}

        //public Member GetCustomerInfo(AccountInfoCriteria criteria)
        //{
        //    return _accountIfoRepository.GetAgentInfo(criteria);
        //}

        //public List<Customer> GetCustomerSearch(ICriteria criteria)
        //{
        //    return _accountIfoRepository.GetCustomerSearch(criteria).ToList();
        //}

        //public List<TreeViewNodeDTO> GetTreeViewNode(ICriteria criteria)
        //{
        //    return _accountIfoRepository.GetTreeViewNode(criteria).ToList();
        //}

        //public List<Member> GetMemberDownline(ref MemberDownlineCriteria criteria)
        //{
        //    return _accountIfoRepository.GetMemberDownline(ref criteria).ToList();
        //}

        //public List<Agent> GetAgentDownlinePath(ICriteria criteria)
        //{
        //    return _accountIfoRepository.GetAgentDownlinePath(criteria).ToList();
        //}
    }
}
