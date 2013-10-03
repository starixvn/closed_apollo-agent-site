// --------------------------------------------------------------------------
// <copyright file="IFunctionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.PermissionControl.Services
{
    using System.Collections.Generic;
    using Apollo.Age.PermissionControl.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IFunctionService
    {
        /// <summary>
        /// Gets all function.
        /// </summary>
        /// <param name="functionGroupId">The function group identifier.</param>
        /// <param name="isActive">The is active.</param>
        /// <returns>Return list functions.</returns>
        IEnumerable<Function> GetAll(short functionGroupId = 0, bool? isActive = null);
    }
}
