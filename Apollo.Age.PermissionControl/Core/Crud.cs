// --------------------------------------------------------------------------
// <copyright file="Crud.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.4">Created</change>
// </history>

namespace Apollo.Age.PermissionControl.Core
{
    /// <summary>
    /// Crud permission.
    /// </summary>
    public enum Crud
    {
        /// <summary>
        /// The read permission.
        /// </summary>
        Query = 0,

        /// <summary>
        /// The host permission.
        /// </summary>
        Edit = 1
    }
}
