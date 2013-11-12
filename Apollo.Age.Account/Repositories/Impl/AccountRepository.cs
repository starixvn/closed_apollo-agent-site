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
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Account.Repositories.Interface;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Fanex.Common;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Account repository.
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        /// <summary>
        /// Get agent by username.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of agent.</returns>
        public IEnumerable<Agent> GetAgentByUsername(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return db.Query<Agent>(criteria.GetSpParams());
                    }
                }

                return new List<Agent>();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get member by username.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of member.</returns>
        public IEnumerable<Member> GetMemberByUsername(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return db.Query<Member>(criteria.GetSpParams());
                    }
                }

                return new List<Member>();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get customer information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Customer information model.</returns>
        public CustomerInfoModel GetCustomerInfo(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return db.QueryEntity<CustomerInfoModel>(criteria.GetSpParams());
                    }
                }

                return new CustomerInfoModel();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get member down line.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of member.</returns>
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

        /// <summary>
        /// Get agent list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of agent list model.</returns>
        public IEnumerable<AgentListModel> GetAgentList(ref AgentListCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var param = criteria.GetSpParams();
                        var list = db.Query<AgentListModel>(param);

                        // get output parameter
                        criteria.TotalItems = Convert.ToInt32(param["totalitems"]);

                        return list;
                    }
                }

                return new List<AgentListModel>();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get member list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of member list model.</returns>
        public IEnumerable<MemberListModel> GetMemberList(ref CustomerListCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var param = criteria.GetSpParams();
                        var list = db.Query<MemberListModel>(param);

                        // get output parameter
                        criteria.TotalItems = Convert.ToInt32(param["totalitems"]);
                        return list;
                    }
                }

                return new List<MemberListModel>();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get agent share.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of agent share model.</returns>
        public IEnumerable<AgentShareModel> GetAgentShare(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var list = db.Query<AgentShareModel>(criteria.GetSpParams());

                        return list;
                    }
                }

                return new List<AgentShareModel>();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get member share.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of agent member model.</returns>
        public IEnumerable<MemberShareModel> GetMemberShare(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var list = db.Query<MemberShareModel>(criteria.GetSpParams());

                        return list;
                    }
                }

                return new List<MemberShareModel>();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get commission to member.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of customer commission model.</returns>
        public IEnumerable<CustomerCommissionModel> GetCommissionToMember(GetCommissionToMemberCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return db.Query<CustomerCommissionModel>(criteria.GetSpParams());
                    }
                }

                return new List<CustomerCommissionModel>();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get commission to agent.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of customer commission model.</returns>
        public IEnumerable<CustomerCommissionModel> GetCommissionToAgent(GetCommissionToAgentCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return db.Query<CustomerCommissionModel>(criteria.GetSpParams());
                    }
                }

                return new List<CustomerCommissionModel>();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get agent log login.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of agent log login model.</returns>
        public IEnumerable<AgentLogLoginModel> GetAgentLogLogin(GetLoginLogCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return db.Query<AgentLogLoginModel>(criteria.GetSpParams());
                    }
                }

                return new List<AgentLogLoginModel>();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Calculate update agent position taking time.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update duration.</returns>
        public int CalculateUpdateAgentPositionTakingTime(GetUpdatePositionTakingEstimatedTimeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    var parameters = criteria.GetSpParams();
                    db.ExecuteNonQuery(parameters);

                    return Converter.ToInt(parameters["duration"].ToString());
                }
            }

            return 0;
        }

        /// <summary>
        /// Check agent has member or not.
        /// </summary>
        /// <param name="criteria">Criteria for get data.</param>
        /// <returns>Has member or not.</returns>
        public bool HasMember(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return (int)db.ExecuteScalar(criteria.GetSpParams()) == 1;
                }
            }

            return false;
        }
    }
}