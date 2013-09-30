// --------------------------------------------------------------------------
// <copyright file="CustomerStatus.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.7">Created</change>
//   <change who="Ricky.Nguyen" date="2013.5.6">Update Rename</change>
// </history>

namespace Apollo.Age.Enumerations
{
    public enum CustomerStatus : int
    {
        /// <summary>
        /// Status active.
        /// </summary>
        Active = 1,

        /// <summary>
        /// Status stopped.
        /// </summary>
        Stopped = 2,

        /// <summary>
        /// Status locked.
        /// </summary>
        Locked = 3,

        /// <summary>
        /// Status C lock.
        /// </summary>
        CLock = 4
    }
}