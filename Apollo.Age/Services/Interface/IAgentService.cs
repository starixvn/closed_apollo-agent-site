// --------------------------------------------------------------------------
// <copyright file="IAgentService.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.Age.DTO.Accounts;
    using Apollo.Age.Entities;
    using Fanex.NAP.Framework.Data;

    public interface IAgentService
    {
        List<TreeViewNodeDTO> GetTreeViewNode(ICriteria criteria);

        Agent GetCustomerInfo(AccountInfoCriteria criteria);

        List<Agent> GetAgentDownlinePath(ICriteria criteria);
    }
}
