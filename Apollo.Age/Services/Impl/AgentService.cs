// --------------------------------------------------------------------------
// <copyright file="AgentService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.26">Created</change>
// </history>

namespace Apollo.Age.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Criterias.Accounts;
    
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Services.Interface;
    using Fanex.NAP.Framework.Data;
    using Fanex.Common.Helper;

    public class AgentService : IAgentService
    {
        private readonly IAgentRepository _repository;

        public AgentService(IAgentRepository agentRepository = null)
        {
            _repository = agentRepository ?? new AgentRepository();
        }

        //public List<TreeViewNode> GetTreeViewNode(ICriteria criteria)
        //{
        //    return _repository.GetTreeViewNode(criteria).ToList();
        //}

        public Agent GetCustomerInfo(AccountInfoCriteria criteria)
        {
            return _repository.Get(criteria);
        }

        public List<Agent> GetAgentDownlinePath(ICriteria criteria)
        {
            return _repository.Fetch(criteria).ToList();
        }

        /// <summary>
        ///  get all direct downline of a specified agent
        /// </summary>
        /// <param name="agentId"> valid agent id</param>
        /// <returns></returns>
        public List<Agent> GetDirectDownline(int agentId)
        {
            Check.Argument.IsNotNegativeOrZero(agentId, "agentId");

            return _repository.Fetch(new AgentDownlineCriteria
            {
                AgentId = agentId
            }).ToList();
        }

        public bool IsDescendant(int parentId, int agentId)
        {
            //Check.Argument.IsNotNegativeOrZero(parentId, "parentId");
            //Check.Argument.IsNotNegativeOrZero(agentId, "agentId");

            return true;
        }

        public IEnumerable<Agent> GetAgents(int id)
        {
            return _repository.Fetch(new AgentDownlineCriteria
          {
              AgentId = id
          });
        }
    }
}
