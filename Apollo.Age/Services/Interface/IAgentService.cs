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
    
    using Apollo.Age.Entities;
    using Fanex.NAP.Framework.Data;

    public interface IAgentService
    {
    //    List<TreeViewNode> GetTreeViewNode(ICriteria criteria);

        Agent GetCustomerInfo(AccountInfoCriteria criteria);

        List<Agent> GetAgentDownlinePath(ICriteria criteria);

        /// <summary>
        ///  get all direct downline of a specified agent
        /// </summary>
        /// <param name="agentId"> valid agent id</param>
        /// <returns></returns>
        List<Agent> GetDirectDownline(int agentId);

        /// <summary>
        /// check whether agentid 
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        bool IsDescendant(int parentId, int agentId);

        IEnumerable<Agent> GetAgents(int id);
    }
}
