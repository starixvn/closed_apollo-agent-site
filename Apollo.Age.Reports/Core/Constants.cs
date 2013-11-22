// --------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.17">Created</change>
//   <change who="Anders.Le" date="2013.7.11">Add MatchEnteredSports</change>
//   <change who="Quinn.Vo" date="2013.7.16">Add CacheKeyAgentWinLossReport, CacheKeyMemberWinLossReport</change>
// </history>

namespace Apollo.Age.Reports.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Constants list.
    /// </summary>
    public static class Constants
    {
        public const string NA = "N/A";
        public const string RefundHT = "Refund HT";
        public const string RefundFT = "Refund FT";
        public const string Refund = "Refund";

        public const string SessionKeyAgentWinLossReport = "AgentWinLossReport-{0}";
        public const string SessionKeyMemberWinLossReport = "MemberWinLossReport-{0}";
        public const string SessionKeyAgentOutstandingReport = "AgentOutStandingReport-{0}";
        public const string SessionKeyMemberOutstandingReport = "MemberOutStandingReport-{0}";
        public const string SessionKeyLineSportCreditReport = "LineSportCreditReport-{0}";

        /// <summary>
        /// Dictionary list (sport id, language key).
        /// The order should follow.
        /// </summary>
        public static readonly Dictionary<int, string> MatchEnteredSports = new

        Dictionary<int, string>
        {
            { 1, "soccer" },
            { 2, "bk" },
            { 5, "tennis" },
            { 6, "Volleyball" },
            { 8, "Baseball" },
            { -10, "lblOutRight" },
            { -1, "InterEvent" }
        };

        public static readonly Dictionary<int, string> ReportAgentLevelDictionary = new

        Dictionary<int, string>
        {
            { 0, "lblSuper" },
            { 1, "lblMainShareHolder" },
            { 2, "lblShareholderLevel" },
            { 3, "lblMainAgent" },
            { 4, "agent" }
        };
    }
}
