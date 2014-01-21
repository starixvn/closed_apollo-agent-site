// ---------------------------------------------------------------------------------
// <copyright file="BetStatus.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Entities
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

    public struct BetStatusId
    {
        public const string Won = "112";
        public const string Lost = "113";
        public const string Void = "102";
        public const string Reject = "101";
        public const string Refund = "103";
        public const string Draw = "111";
        public const string Running = "0";
        public const string Waiting = "1";
    }
}