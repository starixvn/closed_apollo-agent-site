// --------------------------------------------------------------------------
// <copyright file="BetStatus.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Tring" date="2013.4.12">Created</change>
// </history>
namespace Apollo.Age.Enumerations
{
    public enum BetStatus : int
    {
        /// <summary>
        /// Hard code for bet status running.
        /// </summary>
        Running = 0,

        /// <summary>
        /// Hard code for bet status waiting.
        /// </summary>
        Waiting = 1,

        /// <summary>
        /// Hard code for bet status reject.
        /// </summary>
        Reject = 101,

        /// <summary>
        /// Hard code for bet status void.
        /// </summary>
        Void = 102,

        /// <summary>
        /// Hard code for bet status refund.
        /// </summary>
        Refund = 103,

        /// <summary>
        /// Hard code for bet status draw.
        /// </summary>
        Draw = 111,

        /// <summary>
        /// Hard code for bet status won.
        /// </summary>
        Won = 112,

        /// <summary>
        /// Hard code for bet status lose.
        /// </summary>
        Lose = 113
    }
}