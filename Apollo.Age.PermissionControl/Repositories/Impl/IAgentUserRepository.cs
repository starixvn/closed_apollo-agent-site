// --------------------------------------------------------------------------
// <copyright file="IAgentUserRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.PermissionControl.Repositories.Impl
{
    using System.Collections.Generic;
    using Apollo.Age.Entities;
    using Apollo.Age.PermissionControl.Criterias;
    using Apollo.Age.PermissionControl.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAgentUserRepository
    {
        /// <summary>
        /// Assigns the user permission.
        /// </summary>
        /// <param name="criteria">The criteria assign user permission.</param>
        /// <returns>Return command result.</returns>
        void Update(ref AgentUserAssignFunctionCriteria criteria);

        /// <summary>
        /// Gets the user function policy.
        /// </summary>
        /// <param name="criteria">The criteria get user function policy.</param>
        /// <returns>List of function with policy of user.</returns>
        IEnumerable<FunctionPolicy> GetUserFunctionPolicy(ICriteria criteria);

        /// <summary>
        /// Gets the parent information with agent user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Parent agent info with shadow information.
        /// </returns>
        User GetParentInfo(ICriteria criteria);

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>User information.</returns>
        User GetAgentUserInfo(ICriteria criteria);

        /// <summary>
        /// Gets the user function policy.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Policy of user on function.</returns>
        byte GetUserPolicy(ICriteria criteria);

        /// <summary>
        /// Gets the by agent.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Agent user list.</returns>
        IEnumerable<User> GetByAgent(ICriteria criteria);
    }
}
