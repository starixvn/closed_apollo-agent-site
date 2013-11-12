// ---------------------------------------------------------------------------------
// <copyright file="ITicketData.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.27">Create</change>
//     <change who="Marc.Bui" date="2013.01.28">Add SuperPT, MasterPT, AgentPT for Win/Place PTs columns</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Entities
{
    /// <summary>
    /// Reference data for bet types:
    /// 1) Horse Racing (33) =&gt; Build Odds
    /// 2) Live Casino Baccarat (1101) =&gt; Build Choice
    /// 3) Live Roulette (1102) =&gt; Build Choice.
    /// </summary>
    public interface ITicketData
    {
        /// <summary>
        /// Gets or sets the reference no.
        /// </summary>
        /// <value>
        /// The reference no.
        /// </value>
        string RefNo { get; set; }

        /// <summary>
        /// Gets or sets the bet type identifier.
        /// </summary>
        /// <value>
        /// The bet type identifier.
        /// </value>
        int BetTypeId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        string Status { get; set; }

        /// <summary>
        /// Gets or sets the odds.
        /// </summary>
        /// <value>
        /// Input The odds.
        /// </value>
        decimal Odds { get; set; }

        /// <summary>
        /// Gets or sets the stake.
        /// </summary>
        /// <value>
        /// The stake.
        /// </value>
        decimal Stake { get; set; }

        /// <summary>
        /// Gets or sets the bet team.
        /// </summary>
        /// <value>
        /// The bet team.
        /// </value>
        string BetTeam { get; set; }

        /// <summary>
        /// Gets or sets the super PT.
        /// </summary>
        /// <value>
        /// The super PT.
        /// </value>
        decimal SuperPT { get; set; }

        /// <summary>
        /// Gets or sets the master PT.
        /// </summary>
        /// <value>
        /// The master PT.
        /// </value>
        decimal MasterPT { get; set; }

        /// <summary>
        /// Gets or sets the agent PT.
        /// </summary>
        /// <value>
        /// The agent PT.
        /// </value>
        decimal AgentPT { get; set; }
    }
}