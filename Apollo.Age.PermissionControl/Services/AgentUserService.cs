// --------------------------------------------------------------------------
// <copyright file="AgentUserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.PermissionControl.Services
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.Entities;
    using Apollo.Age.PermissionControl.Criterias;
    using Apollo.Age.PermissionControl.Models;
    using Apollo.Age.PermissionControl.Repositories.Impl;
    using Apollo.Age.Services.Impl;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentUserService : ServiceBase, IAgentUserService
    {
        private readonly IAgentUserRepository _agentUserRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentUserService"/> class.
        /// </summary>
        public AgentUserService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentUserService"/> class.
        /// </summary>
        /// <param name="agentUserRepository">The agent user repository.</param>
        public AgentUserService(IAgentUserRepository agentUserRepository)
        {
            _agentUserRepository = agentUserRepository ?? new AgentUserRepository();
        }

        /// <summary>
        /// Assigns the user permission.
        /// </summary>
        /// <param name="criteria">The criteria assign user permission.</param>
        /// <returns>Return command result.</returns>
        public int AssignUserPermission(AgentUserAssignFunctionCriteria criteria)
        {
            try
            {
                return _agentUserRepository.Update(criteria);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return -1;
        }

        /// <summary>
        /// Gets the user function policy.
        /// </summary>
        /// <param name="agentUserId">The agent user identifier.</param>
        /// <param name="functionGroupId">The function group identifier.</param>
        /// <param name="isActive">The is active.</param>
        /// <returns>List function with policy of user.</returns>
        public IEnumerable<FunctionPolicy> GetUserFunctionPolicy(int agentUserId, short functionGroupId = 0, bool? isActive = null)
        {
            try
            {
                var criteria = new AgentUserGetFunctionPolicyCriteria
                {
                    AgentUserId = agentUserId,
                    FunctionGroupId = functionGroupId,
                    IsActive = isActive
                };

                return _agentUserRepository.GetUserFunctionPolicy(criteria);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the parent information with agent user.
        /// </summary>
        /// <param name="agentId">The agent identifier.</param>
        /// <returns>Parent agent info with shadow information.</returns>
        public User GetParentInfo(int agentUserId, bool isMaster)
        {
            try
            {
                var criteria = new GetParentInfoWithAgentUserCriteria
                {
                    AgentUserId = agentUserId,
                    IsMaster = isMaster
                };

                return _agentUserRepository.GetParentInfo(criteria);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the agent user information.
        /// </summary>
        /// <param name="agentUserId">The agent user identifier.</param>
        /// <returns>Agent user information.</returns>
        public User GetAgentUserInfo(int agentUserId)
        {
            try
            {
                var criteria = new GetAgentUserCriteria
                {
                    AgentUserId = agentUserId
                };

                return _agentUserRepository.GetAgentUserInfo(criteria);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the user function policy.
        /// </summary>
        /// <param name="agentUserId">The agent user identifier.</param>
        /// <param name="functionKey">The function key.</param>
        /// <returns>Policy of user on function.</returns>
        public byte GetUserPolicy(int agentUserId, string functionKey)
        {
            try
            {
                var criteria = new AgentUserGetUserFunctionPolicyCriteria
                {
                    AgentUserId = agentUserId,
                    FunctionKey = functionKey
                };

                return _agentUserRepository.GetUserPolicy(criteria);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return 0;
        }
    }
}
