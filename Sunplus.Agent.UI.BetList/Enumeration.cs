/// <summary>
/// All Betlist status definitions
/// </summary>
namespace Sunplus.Agent.UI.BetList
{
    using System;

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