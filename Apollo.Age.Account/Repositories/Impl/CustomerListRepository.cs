namespace Apollo.Age.Account.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Account.Repositories.Interface;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    public class CustomerListRepository : RepositoryBase<CustomerListModel>, ICustomerListRepository
    {
        public IEnumerable<CustomerListModel> GetData(ref CustomerListCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var param = criteria.GetSpParams();
                        var list = db.Query<CustomerListModel>(param);

                        // get output parameter
                        criteria.TotalPages = Convert.ToInt32(param["totalpages"]);
                        criteria.TotalItems = Convert.ToInt32(param["totalitems"]);
                        return list;
                    }
                }

                return new List<CustomerListModel>();
            }
            catch
            {
                throw;
            }
        }

        public ICommandResult Delete(CustomerListModel entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Insert(CustomerListModel entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(CustomerListModel entity)
        {
            throw new NotImplementedException();
        }
    }
}