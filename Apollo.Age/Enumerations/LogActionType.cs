// --------------------------------------------------------------------------
// <copyright file="LogActionType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.7.19">Created</change>
// </history>

namespace Apollo.Age.Enumerations
{
    public enum LogActionType : int
    {
        /// <summary>
        /// All action.
        /// </summary>
        All = 0,

        /// <summary>
        /// Commission action.
        /// </summary>
        Commission = 2,

        /// <summary>
        /// Bet limit.
        /// </summary>
        BetLimit = 3,

        /// <summary>
        /// Position taking.
        /// </summary>
        PositionTaking = 4,

        /// <summary>
        /// General information.
        /// </summary>
        GeneralInfo = 5,

        /// <summary>
        /// Permission user control.
        /// </summary>
        PermissionUserControl = 9
    }
}
