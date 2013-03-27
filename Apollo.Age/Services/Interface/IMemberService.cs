// --------------------------------------------------------------------------
// <copyright file="IMemberService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.26">Created</change>
// </history>

namespace Apollo.Age.Services.Interface
{
    using System.Collections.Generic;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Fanex.NAP.Framework.Data;

    public interface IMemberService
    {
        List<Member> GetCustomerSearch(ICriteria criteria);

        Member GetCustomerInfo(AccountInfoCriteria criteria);

        List<Member> GetMemberDownline(ref MemberDownlineCriteria criteria);
    }
}
