// --------------------------------------------------------------------------
// <copyright file="Policy.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.PermissionControl.Core
{
    /// <summary>
    /// The user policy.
    /// </summary>
    public enum Policy
    {
        /// <summary>
        /// The none permission.
        /// </summary>
        None = 0,

        /// <summary>
        /// The read permission.
        /// </summary>
        Query = 1,

        /// <summary>
        /// The host permission.
        /// </summary>
        Edit = 2
    }
}
