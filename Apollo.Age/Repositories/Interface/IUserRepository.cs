// --------------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
//   <change who="Lewis.Le" date="2013.5.3">Added AuthenticateNapUser</change>
// </history>

namespace Apollo.Age.Repositories.Interface
{
    using System.Collections.Generic;
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Models;
    using Fanex.NAP.Framework.Data;

    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Interface for Internal Re-Check In.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return a number.</returns>
        byte InternalReCheckIn(InternalReCheckinCriteria criteria);

        /// <summary>
        /// Interface for get user.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return a number.</returns>
        int GetUserStatus(ICriteria criteria);

        /// <summary>
        /// Interface for internal log-in log.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return a number.</returns>
        int WritelogInternalLogin(InternalLogLoginCriteria criteria);

        /// <summary>
        /// Interface for internal check-in.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return a number.</returns>
        int InternalCheckIn(InternalCheckinCriteria criteria);

        /// <summary>
        /// Interface log out.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return a number.</returns>
        int InternalLogout(InternalLogoutCriteria criteria);

        /// <summary>
        /// Interface authenticate nap user.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <param name="adminId">Input admin Id.</param>
        /// <returns>Return true or false.</returns>
        bool AuthenticateNapUser(AuthenticateNapUserCriteria criteria, ref int adminId);

        /// <summary>
        /// Interface check permission.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return true or false.</returns>
        bool CheckPermission(ICriteria criteria);

        /// <summary>
        /// Interface check belong to up line.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return true or false.</returns>
        bool CheckBelongToUpline(ICriteria criteria);

        IEnumerable<FunctionPolicy> GetAllUserPolicy(ICriteria criteria);

        void AssignUerPermision(ref UserAssignFunctionCriteria criteria);
    }
}