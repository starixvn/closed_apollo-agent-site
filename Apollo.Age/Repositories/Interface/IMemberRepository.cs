// --------------------------------------------------------------------------
// <copyright file="IMemberRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.26">Created</change>
// </history>

namespace Apollo.Age.Repositories.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;
    using Apollo.Age.Entities;
    using Apollo.Age.Criterias.Accounts;

    public interface IMemberRepository : IRepository<Member>
    {
        IEnumerable<Member> GetMemberDownline(ref MemberDownlineCriteria criteria);

        bool IsExistValueSearch(ICriteria criteria);
    }
}
