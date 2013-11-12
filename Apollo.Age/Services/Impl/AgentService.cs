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
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Services.Interface;

    /// <summary>
    /// Agent service.
    /// </summary>
    public class AgentService : IAgentService
    {
        private readonly IAgentRepository _repository;

        public AgentService(IAgentRepository agentRepository = null)
        {
            _repository = agentRepository ?? new AgentRepository();
        }
    }
}