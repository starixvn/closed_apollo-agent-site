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
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Fanex.NAP.Framework.Data;

    public interface IAccountRepository
    {
        bool IsExistValueSearch(ICriteria criteria);

        IEnumerable<Customer> GetCustomerSearch(ICriteria criteria);

        CustomerInfoViewModel GetCustomerInfo(ICriteria criteria);

        IEnumerable<Member> GetMemberDownline(ref MemberDownlineCriteria criteria);
    }
}
