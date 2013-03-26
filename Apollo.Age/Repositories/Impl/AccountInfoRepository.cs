// --------------------------------------------------------------------------
// <copyright file="AccountInfoRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.22">Created</change>
// </history>

namespace Apollo.Age.Repositories.Impl
{
    using Apollo.Age.DTO.Accounts;
    using Apollo.Age.Repositories.Interface;
    using Fanex.NAP.Framework.Data;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using System;
    using Apollo.Age.Entities;
    using System.Collections.Generic;
    using Apollo.Age.Criterias.Accounts;

    public class AccountInfoRepository : IAccountInfoRepository
    {
        private readonly IDynamicRepository _repository;

        public AccountInfoRepository(IDynamicRepository repository = null)
        {
            _repository = repository ?? new DynamicRepository();
        }

        public AccountInfoDTO GetCustomerInfo(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.QueryEntity<AccountInfoDTO>(criteria.GetSpParams());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Agent GetAgentInfo(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.QueryEntity<Agent>(criteria.GetSpParams());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Customer> GetCustomerSearch(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.Query<Customer>(criteria.GetSpParams());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<TreeViewNodeDTO> GetTreeViewNode(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.Query<TreeViewNodeDTO>(criteria.GetSpParams());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public IEnumerable<CustomerDownline> GetCustomerDownline(ICriteria criteria)
        //{
        //    Check.Argument.IsNotNull(criteria, "criteria");

        //    try
        //    {
        //        if (criteria.IsValid())
        //        {
        //            return _repository.Fetch<CustomerDownline>(criteria);
        //        }

        //        return new List<CustomerDownline>();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Agent> GetAgentDownlinePath(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return _repository.Fetch<Agent>(criteria);
                }

                return new List<Agent>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICommandResult Delete(Agent entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Insert(Agent entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(Agent entity)
        {
            throw new NotImplementedException();
        }

        public int Count(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Agent> Fetch(ICriteria criteria, Action<Orderable<Agent>> order)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Agent> Fetch(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Agent> Fetch()
        {
            throw new NotImplementedException();
        }

        public Agent Get(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public Agent Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Length(dynamic paramaters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Agent> Query(dynamic parameters)
        {
            throw new NotImplementedException();
        }

        public Agent Single(dynamic parameters)
        {
            throw new NotImplementedException();
        }
    }
}
