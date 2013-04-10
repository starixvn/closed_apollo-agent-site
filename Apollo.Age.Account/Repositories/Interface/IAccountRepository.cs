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
    using Apollo.Age.Account.Models;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Fanex.NAP.Framework.Data;

    public interface IAccountRepository
    {
        IEnumerable<Customer> GetCustomerSuggest(ICriteria criteria);

        bool IsExistValueSearch(ICriteria criteria);

        CustomerInfoModel GetCustomerInfo(ICriteria criteria);

        IEnumerable<Member> GetMemberDownline(ref MemberDownlineCriteria criteria);
    }
}
