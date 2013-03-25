// --------------------------------------------------------------------------
// <copyright file="IAcountInfoService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.22">Created</change>
// </history>

namespace Apollo.Age.Services.Interface
{
    using Apollo.Age.DTO.Accounts;
    using Fanex.NAP.Framework.Data;
    using System.Collections.Generic;
    using Apollo.Age.Criterias.Accounts;

    public interface IAccountInfoService
    {
        AccountInfoDTO GetCustomerInfo(ICriteria criteria);

        List<CustomerSearching> GetCustomerSearch(ICriteria criteria);

        List<TreeViewNodeDTO> GetTreeViewNode(ICriteria criteria);

        List<CustomerDownline> GetCustomerDownline(ICriteria criteria);

        List<CustomerDownline> GetMemberDownline(ref MemberDownlineCriteria criteria);

        List<AgentDownlinePath> GetAgentDownlinePath(ICriteria criteria);
    }
}
