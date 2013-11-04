// ---------------------------------------------------------------------------------
// <copyright file="ITicket.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Entities
{
    using System;

    /// <summary>
    /// Ticket data interface.
    /// </summary>
    public interface ITicket
    {
        string UGroup { get; set; }

        int SportTypeId { get; set; }

        string UserName { get; set; }

        long TransId { get; set; }

        long? BetId { get; set; }

        int BetTypeId { get; set; }

        decimal Handicap1 { get; set; }

        decimal Handicap2 { get; set; }

        decimal MOdds { get; set; }

        string OddsType { get; set; }

        decimal Odds { get; set; }

        decimal Stake { get; set; }

        decimal ActualStake { get; set; }

        string BetTeam { get; set; }

        /// <summary>
        /// Running, Waiting, Refund, Void, Won, Lose, Draw, Reject.
        /// </summary>
        string Status { get; set; }

        string BetStatus { get; set; }

        int? StatusId { get; set; }

        string RefNo { get; set; }

        DateTime TransDate { get; set; }

        DateTime WinlostDate { get; set; }

        decimal Winlost { get; set; }

        decimal AWinlost { get; set; }

        decimal MasterWinlost { get; set; }

        decimal SuperWinlost { get; set; }

        decimal PlayerComm { get; set; }

        decimal Comm { get; set; }

        decimal AComm { get; set; }

        int AgentLevel { get; set; }

        decimal AgentComm { get; set; }

        decimal SuperComm { get; set; }

        decimal APositionTaking { get; set; }

        decimal MasterPositionTaking { get; set; }

        decimal SuperPositionTaking { get; set; }

        decimal ADiscount { get; set; }

        decimal PlayerDiscount { get; set; }

        decimal MasterDiscount { get; set; }

        decimal SuperDiscount { get; set; }

        bool IsLive { get; set; }

        int LiveHomeScore { get; set; }

        int LiveAwayScore { get; set; }

        string IP { get; set; }

        int LeagueId { get; set; }

        int MatchId { get; set; }

        string MatchCode { get; set; }

        string Race { get; set; }

        long HomeId { get; set; }

        long AwayId { get; set; }

        /// <summary>
        /// Match status: First Goal, Last Goal.
        /// </summary>
        string EventStatus { get; set; }

        bool IsNeutral { get; set; }

        long CustId { get; set; }

        string ShowTime { get; set; }

        string BetCheck { get; set; }

        string CommStatus { get; set; }

        DateTime EventDate { get; set; }

        int FinalHomeScore { get; set; }

        int FinalAwayScore { get; set; }

        int HaftTimeHomeScore { get; set; }

        int HaftTimeAwayScore { get; set; }
    }
}