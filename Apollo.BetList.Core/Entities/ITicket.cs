// ---------------------------------------------------------------------------------
// <copyright file="ITicket.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Entities
{
    using System;

    /// <summary>
    /// Ticket data interface.
    /// </summary>
    public interface ITicket
    {
        /// <summary>
        /// Gets or sets the user group.
        /// </summary>
        /// <value>
        /// The user group.
        /// </value>
        string UGroup { get; set; }

        /// <summary>
        /// Gets or sets the sport type identifier.
        /// </summary>
        /// <value>
        /// The sport type identifier.
        /// </value>
        int SportTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        string UserName { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        long TransId { get; set; }

        /// <summary>
        /// Gets or sets the bet identifier.
        /// </summary>
        /// <value>
        /// The bet identifier.
        /// </value>
        long? BetId { get; set; }

        /// <summary>
        /// Gets or sets the bet type identifier.
        /// </summary>
        /// <value>
        /// The bet type identifier.
        /// </value>
        int BetTypeId { get; set; }

        /// <summary>
        /// Gets or sets the handicap1.
        /// </summary>
        /// <value>
        /// The handicap1.
        /// </value>
        decimal Handicap1 { get; set; }

        /// <summary>
        /// Gets or sets the handicap2.
        /// </summary>
        /// <value>
        /// The handicap2.
        /// </value>
        decimal Handicap2 { get; set; }

        /// <summary>
        /// Gets or sets the m odds.
        /// </summary>
        /// <value>
        /// The m odds.
        /// </value>
        decimal MOdds { get; set; }

        /// <summary>
        /// Gets or sets the type of the odds.
        /// </summary>
        /// <value>
        /// The type of the odds.
        /// </value>
        string OddsType { get; set; }

        /// <summary>
        /// Gets or sets the odds.
        /// </summary>
        /// <value>
        /// The odds value.
        /// </value>
        decimal Odds { get; set; }

        /// <summary>
        /// Gets or sets the stake.
        /// </summary>
        /// <value>
        /// The stake value.
        /// </value>
        decimal Stake { get; set; }

        /// <summary>
        /// Gets or sets the actual stake.
        /// </summary>
        /// <value>
        /// The actual stake.
        /// </value>
        decimal ActualStake { get; set; }

        /// <summary>
        /// Gets or sets the bet team.
        /// </summary>
        /// <value>
        /// The bet team.
        /// </value>
        string BetTeam { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        string Status { get; set; }

        /// <summary>
        /// Gets or sets the bet status.
        /// </summary>
        /// <value>
        /// The bet status.
        /// </value>
        string BetStatus { get; set; }

        /// <summary>
        /// Gets or sets the status identifier.
        /// </summary>
        /// <value>
        /// The status identifier.
        /// </value>
        int? StatusId { get; set; }

        /// <summary>
        /// Gets or sets the reference no.
        /// </summary>
        /// <value>
        /// The reference no.
        /// </value>
        string RefNo { get; set; }

        /// <summary>
        /// Gets or sets the trans date.
        /// </summary>
        /// <value>
        /// The trans date.
        /// </value>
        DateTime TransDate { get; set; }

        /// <summary>
        /// Gets or sets the win lost date.
        /// </summary>
        /// <value>
        /// The win lost date.
        /// </value>
        DateTime WinlostDate { get; set; }

        /// <summary>
        /// Gets or sets the win lost.
        /// </summary>
        /// <value>
        /// The win lost.
        /// </value>
        decimal Winlost { get; set; }

        /// <summary>
        /// Gets or sets a win lost.
        /// </summary>
        /// <value>
        /// A win lost.
        /// </value>
        decimal AWinlost { get; set; }

        /// <summary>
        /// Gets or sets the master win lost.
        /// </summary>
        /// <value>
        /// The master win lost.
        /// </value>
        decimal MasterWinlost { get; set; }

        /// <summary>
        /// Gets or sets the super win lost.
        /// </summary>
        /// <value>
        /// The super win lost.
        /// </value>
        decimal SuperWinlost { get; set; }

        /// <summary>
        /// Gets or sets the player commission.
        /// </summary>
        /// <value>
        /// The player commission.
        /// </value>
        decimal PlayerComm { get; set; }

        /// <summary>
        /// Gets or sets the commission.
        /// </summary>
        /// <value>
        /// The commission.
        /// </value>
        decimal Comm { get; set; }

        /// <summary>
        /// Gets or sets a commission.
        /// </summary>
        /// <value>
        /// A commission.
        /// </value>
        decimal AComm { get; set; }

        /// <summary>
        /// Gets or sets the agent level.
        /// </summary>
        /// <value>
        /// The agent level.
        /// </value>
        int AgentLevel { get; set; }

        /// <summary>
        /// Gets or sets the agent commission.
        /// </summary>
        /// <value>
        /// The agent commission.
        /// </value>
        decimal AgentComm { get; set; }

        /// <summary>
        /// Gets or sets the super commission.
        /// </summary>
        /// <value>
        /// The super commission.
        /// </value>
        decimal SuperComm { get; set; }

        /// <summary>
        /// Gets or sets a position taking.
        /// </summary>
        /// <value>
        /// A position taking.
        /// </value>
        decimal APositionTaking { get; set; }

        /// <summary>
        /// Gets or sets the master position taking.
        /// </summary>
        /// <value>
        /// The master position taking.
        /// </value>
        decimal MasterPositionTaking { get; set; }

        /// <summary>
        /// Gets or sets the super position taking.
        /// </summary>
        /// <value>
        /// The super position taking.
        /// </value>
        decimal SuperPositionTaking { get; set; }

        /// <summary>
        /// Gets or sets a discount.
        /// </summary>
        /// <value>
        /// A discount.
        /// </value>
        decimal ADiscount { get; set; }

        /// <summary>
        /// Gets or sets the player discount.
        /// </summary>
        /// <value>
        /// The player discount.
        /// </value>
        decimal PlayerDiscount { get; set; }

        /// <summary>
        /// Gets or sets the master discount.
        /// </summary>
        /// <value>
        /// The master discount.
        /// </value>
        decimal MasterDiscount { get; set; }

        /// <summary>
        /// Gets or sets the super discount.
        /// </summary>
        /// <value>
        /// The super discount.
        /// </value>
        decimal SuperDiscount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is live].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is live]; otherwise, <c>false</c>.
        /// </value>
        bool IsLive { get; set; }

        /// <summary>
        /// Gets or sets the live home score.
        /// </summary>
        /// <value>
        /// The live home score.
        /// </value>
        int LiveHomeScore { get; set; }

        /// <summary>
        /// Gets or sets the live away score.
        /// </summary>
        /// <value>
        /// The live away score.
        /// </value>
        int LiveAwayScore { get; set; }

        /// <summary>
        /// Gets or sets the IP.
        /// </summary>
        /// <value>
        /// Return The IP.
        /// </value>
        string IP { get; set; }

        /// <summary>
        /// Gets or sets the league identifier.
        /// </summary>
        /// <value>
        /// The league identifier.
        /// </value>
        int LeagueId { get; set; }

        /// <summary>
        /// Gets or sets the match identifier.
        /// </summary>
        /// <value>
        /// The match identifier.
        /// </value>
        int MatchId { get; set; }

        /// <summary>
        /// Gets or sets the match code.
        /// </summary>
        /// <value>
        /// The match code.
        /// </value>
        string MatchCode { get; set; }

        /// <summary>
        /// Gets or sets the race.
        /// </summary>
        /// <value>
        /// The race value.
        /// </value>
        string Race { get; set; }

        /// <summary>
        /// Gets or sets the home identifier.
        /// </summary>
        /// <value>
        /// The home identifier.
        /// </value>
        long HomeId { get; set; }

        /// <summary>
        /// Gets or sets the away identifier.
        /// </summary>
        /// <value>
        /// The away identifier.
        /// </value>
        long AwayId { get; set; }

        /// <summary>
        /// Gets or sets the event status.
        /// </summary>
        /// <value>
        /// The event status.
        /// </value>
        string EventStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is neutral].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is neutral]; otherwise, <c>false</c>.
        /// </value>
        bool IsNeutral { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        long CustId { get; set; }

        /// <summary>
        /// Gets or sets the show time.
        /// </summary>
        /// <value>
        /// The show time.
        /// </value>
        string ShowTime { get; set; }

        /// <summary>
        /// Gets or sets the bet check.
        /// </summary>
        /// <value>
        /// The bet check.
        /// </value>
        string BetCheck { get; set; }

        /// <summary>
        /// Gets or sets the commission status.
        /// </summary>
        /// <value>
        /// The commission status.
        /// </value>
        string CommStatus { get; set; }

        /// <summary>
        /// Gets or sets the event date.
        /// </summary>
        /// <value>
        /// The event date.
        /// </value>
        DateTime EventDate { get; set; }

        /// <summary>
        /// Gets or sets the final home score.
        /// </summary>
        /// <value>
        /// The final home score.
        /// </value>
        int FinalHomeScore { get; set; }

        /// <summary>
        /// Gets or sets the final away score.
        /// </summary>
        /// <value>
        /// The final away score.
        /// </value>
        int FinalAwayScore { get; set; }

        /// <summary>
        /// Gets or sets the haft time home score.
        /// </summary>
        /// <value>
        /// The haft time home score.
        /// </value>
        int HaftTimeHomeScore { get; set; }

        /// <summary>
        /// Gets or sets the haft time away score.
        /// </summary>
        /// <value>
        /// The haft time away score.
        /// </value>
        int HaftTimeAwayScore { get; set; }
    }
}