// --------------------------------------------------------------------------
// <copyright file="AccountRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.8">Created</change>
// </history>

namespace Apollo.Age.Account.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.Account.Repositories.Interface;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    public class AccountRepository : IAccountRepository
    {
        public bool IsExistValueSearch(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            bool result = false;
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    result = (bool)db.ExecuteScalar(criteria.GetSpParams());
                }
            }
            catch
            {
                throw;
            }

            return result;
        }

        public IEnumerable<Customer> GetCustomerSearch(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return db.Query<Customer>(criteria.GetSpParams());
                    }
                }

                return new List<Customer>();
            }
            catch
            {
                throw;
            }
        }

        public CustomerInfoViewModel GetCustomerInfo(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return db.QueryEntity<CustomerInfoViewModel>(criteria.GetSpParams());
                    }
                }

                return new CustomerInfoViewModel();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Member> GetMemberDownline(ref MemberDownlineCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var param = criteria.GetSpParams();
                        var list = db.Query<Member>(param);

                        // get output parameter
                        criteria.TotalPages = Convert.ToInt32(param["totalpages"]);
                        return list;
                    }
                }

                return new List<Member>();
            }
            catch
            {
                throw;
            }
        }
    }
}
