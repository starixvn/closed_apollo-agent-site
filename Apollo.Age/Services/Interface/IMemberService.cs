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

    public interface IMemberService
    {
        Member GetCustomerInfo(AccountInfoCriteria criteria);

        List<Member> GetMemberDownline(ref MemberDownlineCriteria criteria);
    }
}
