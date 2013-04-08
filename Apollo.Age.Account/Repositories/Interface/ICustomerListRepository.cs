namespace Apollo.Age.Account.Repositories.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Entities;
    using Fanex.NAP.Framework.Data;

    public interface ICustomerListRepository : IRepository<CustomerListViewModel>
    {
        IEnumerable<CustomerListViewModel> GetData(ref CustomerListCriteria criteria);
    }
}