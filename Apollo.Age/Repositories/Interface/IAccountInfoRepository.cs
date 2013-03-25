// --------------------------------------------------------------------------
// <copyright file="IAccountInfoRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.22">Created</change>
// </history>

namespace Apollo.Age.Repositories.Interface
{
    using Apollo.Age.DTO.Accounts;
    using Fanex.NAP.Framework.Data;
    using Apollo.Age.Entities;
    using System.Collections.Generic;
    using Apollo.Age.Criterias.Accounts;

    public interface IAccountInfoRepository : IRepository<Agent>
    {
        AccountInfoDTO GetCustomerInfo(ICriteria criteria);

        IEnumerable<CustomerSearching> GetCustomerSearch(ICriteria criteria);

        IEnumerable<CustomerDownline> GetCustomerDownline(ICriteria criteria);

        IEnumerable<CustomerDownline> GetMemberDownline(ref MemberDownlineCriteria criteria);

        IEnumerable<TreeViewNodeDTO> GetTreeViewNode(ICriteria criteria);

        IEnumerable<AgentDownlinePath> GetAgentDownlinePath(ICriteria criteria);
    }
}
