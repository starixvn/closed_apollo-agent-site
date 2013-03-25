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

    public class AcountInfoService : IAcountInfoService
    {
        private readonly IAccountInfoRepository _accountIfoRepository;
        private readonly IDynamicRepository _repository;

        public AcountInfoService(IAccountInfoRepository userRepository = null)
        {
            _accountIfoRepository = _accountIfoRepository ?? new AccountInfoRepository();
        }

        public AcountInfoDTO GetCustomerInfo(ICriteria criteria)
        {
            return _accountIfoRepository.GetCustomerInfo(criteria);
        }

        public List<CustomerDownline> GetCustomerDownline(ICriteria criteria)
        {
            return _accountIfoRepository.Fetch(criteria).ToList();
        }

        public List<CustomerSearching> GetCustomerSearch(ICriteria criteria)
        {
            return _accountIfoRepository.GetCustomerSearch(criteria).ToList();
        }

        public List<TreeViewNodeDTO> GetTreeViewNode(ICriteria criteria)
        {
            return _accountIfoRepository.GetTreeViewNode(criteria).ToList();
        }
    }
}
