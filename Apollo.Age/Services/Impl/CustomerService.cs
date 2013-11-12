// --------------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="Nexcel Solutions Vietnam">
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
    /// Customer service.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly IAgentRepository _repository;

        public CustomerService(IAgentRepository customerRepository = null)
        {
            _repository = customerRepository ?? new AgentRepository();
        }
    }
}
