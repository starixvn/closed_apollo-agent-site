// ---------------------------------------------------------------------------------
// <copyright file="BetStatus.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Entities
{
    /// <summary>
    /// Struct BetStatus.
    /// </summary>
    public struct BetStatus
    {
        public const string Running = "running";
        public const string Waiting = "waiting";
        public const string Refund = "refund";
        public const string Void = "void";
        public const string Won = "won";
        public const string Lose = "lose";
        public const string Draw = "draw";
        public const string Reject = "reject";
    }
}