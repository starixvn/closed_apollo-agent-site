// --------------------------------------------------------------------------
// <copyright file="AgentUserRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.PermissionControl.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.Entities;
    using Apollo.Age.PermissionControl.Criterias;
    using Apollo.Age.PermissionControl.Entities;
    using Apollo.Age.PermissionControl.Models;
    using Apollo.Age.PermissionControl.Repositories.Interface;
    using Apollo.Age.Repositories.Impl;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentUserRepository : RepositoryBase<dynamic>, IAgentUserRepository
    {
        /// <summary>
        /// Assigns the user permission.
        /// </summary>
        /// <param name="criteria">The criteria assign user permission.</param>   
        public void Update(ref AgentUserAssignFunctionCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var param = criteria.GetSpParams();

                        objectDb.ExecuteNonQuery(param);

                        criteria.ErrCode = (int)param["errcode"];
                        criteria.ErrMsg = (string)param["errmsg"];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the user function policy.
        /// </summary>
        /// <param name="criteria">The criteria get user function policy.</param>
        /// <returns>List of function with policy of user.</returns>
        public IEnumerable<FunctionPolicy> GetUserFunctionPolicy(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return objectDb.Query<FunctionPolicy, Function, FunctionGroup, FunctionPolicy>(
                            (fp, f, fg) => { fp.Function = f; f.FunctionGroup = fg; return fp; },
                            "FunctionId,FunctionGroupId",
                            criteria.GetSpParams());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        /// <summary>
        /// Gets the parent information with agent user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Parent agent info with shadow information.
        /// </returns>
        public User GetParentInfo(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return objectDb.QueryEntity<User, Agent, User>(
                            (u, a) => { u.Agent = a; return u; },
                            "AgentId",
                            criteria.GetSpParams());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>User information.</returns>
        public User GetAgentUserInfo(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return objectDb.QueryEntity<User, Agent, User>(
                            (u, a) => { u.Agent = a; return u; },
                            "AgentId",
                            criteria.GetSpParams());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        /// <summary>
        /// Gets the user function policy.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Policy of user on function.</returns>
        public byte GetUserPolicy(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return (byte)objectDb.ExecuteScalar(criteria.GetSpParams());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return 0;
        }

        /// <summary>
        /// Gets the by agent.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Agent user list.</returns>
        public IEnumerable<User> GetByAgent(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return objectDb.Query<User, Agent, User>(
                            (u, a) => { u.Agent = a; return u; },
                            "CreatorId",
                            criteria.GetSpParams());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }
    }
}
