namespace Apollo.Age.Account.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Repositories.Interface;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    public class CustomerListRepository : RepositoryBase<CustomerListViewModel>, ICustomerListRepository
    {
        public IEnumerable<CustomerListViewModel> GetCustomerList(ref CustomerListCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var param = criteria.GetSpParams();
                        var list = db.Query<CustomerListViewModel>(param);

                        // get output parameter
                        criteria.TotalPages = Convert.ToInt32(param["totalpages"]);
                        return list;
                    }
                }

                return new List<CustomerListViewModel>();
            }
            catch
            {
                throw;
            }
        }

        public ICommandResult Delete(CustomerListViewModel entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Insert(CustomerListViewModel entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(CustomerListViewModel entity)
        {
            throw new NotImplementedException();
        }
    }
}