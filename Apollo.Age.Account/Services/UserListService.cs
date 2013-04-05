namespace Apollo.Age.Account.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Repositories.Impl;
    using Apollo.Age.Account.Repositories.Interface;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Services.Impl;
    using Fanex.Common.Helper;
    using Fanex.NAP.Framework.Data;

    public class UserListService : QueryService
    {
        private readonly IAgentListRepository _agentListRepository;
        private readonly ICustomerListRepository _customerListRepository;

        public UserListService() : this(null, null) { }

        public UserListService(IAgentListRepository agentListRepository = null, ICustomerListRepository customerListRepository = null)
        {
            _agentListRepository = agentListRepository ?? new AgentListRepository();
            _customerListRepository = customerListRepository ?? new CustomerListRepository();
        }

        public IEnumerable<AgentListViewModel> GetAgentList(ref AgentListCriteria criteria)
        {
            return _agentListRepository.GetAgentList(ref criteria);
        }

        public IEnumerable<CustomerListViewModel> GetCustomerList(ref CustomerListCriteria criteria)
        {
            return _customerListRepository.GetCustomerList(ref criteria);
        }
    }
}