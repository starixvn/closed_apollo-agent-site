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
    using Apollo.Age.DTO.Accounts;
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Services.Interface;
    using Fanex.NAP.Framework.Data;

    public class AgentService : IAgentService
    {
        private readonly IAgentRepository _repository;

        public AgentService(IAgentRepository agentRepository = null)
        {
            _repository = agentRepository ?? new AgentRepository();
        }

        public List<TreeViewNodeDTO> GetTreeViewNode(ICriteria criteria)
        {
            return _repository.GetTreeViewNode(criteria).ToList();
        }

        public Agent GetCustomerInfo(AccountInfoCriteria criteria)
        {
            return _repository.Get(criteria);
        }

        public List<Agent> GetAgentDownlinePath(ICriteria criteria)
        {
            return _repository.Fetch(criteria).ToList();
        }
    }
}
