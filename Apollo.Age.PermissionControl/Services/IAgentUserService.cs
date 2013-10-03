// --------------------------------------------------------------------------
// <copyright file="IAgentUserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.PermissionControl.Services
{
    using System.Collections.Generic;
    using Apollo.Age.Entities;
    using Apollo.Age.PermissionControl.Criterias;
    using Apollo.Age.PermissionControl.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAgentUserService
    {
        /// <summary>
        /// Assigns the user permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Command result.</returns>
        int AssignUserPermission(AgentUserAssignFunctionCriteria criteria);

        /// <summary>
        /// Gets the user function policy.
        /// </summary>
        /// <param name="agentUserId">The agent user identifier.</param>
        /// <param name="functionGroupId">The function group identifier.</param>
        /// <param name="isActive">The is active.</param>
        /// <returns>List function with policy of user.</returns>
        IEnumerable<FunctionPolicy> GetUserFunctionPolicy(int agentUserId, short functionGroupId = 0, bool? isActive = null);

        /// <summary>
        /// Gets the parent information with agent user.
        /// </summary>
        /// <param name="agentId">The agent identifier.</param>
        /// <returns>Parent agent info with shadow information.</returns>
        User GetParentInfo(int agentUserId, bool isMaster);

        /// <summary>
        /// Gets the agent user information.
        /// </summary>
        /// <param name="agentUserId">The agent user identifier.</param>
        /// <returns>Agent user information.</returns>
        User GetAgentUserInfo(int agentUserId);
    }
}
