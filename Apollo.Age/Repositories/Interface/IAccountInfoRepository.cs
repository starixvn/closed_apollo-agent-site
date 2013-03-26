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
        //Agent GetAgentInfo(ICriteria criteria);

        //IEnumerable<TreeViewNodeDTO> GetTreeViewNode(ICriteria criteria);

        //IEnumerable<Agent>  (ICriteria criteria);
    }
}
