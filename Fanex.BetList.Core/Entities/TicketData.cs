// ---------------------------------------------------------------------------------
// <copyright file="TicketData.cs" company="Nexcel Solutions Vietnam">
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
    using System;

    /// <summary>
    /// Ticket data class.
    /// </summary>
    [Serializable]
    public class TicketData : ITicketData
    {
        #region ITicketData Members

        /// <summary>
        /// Gets or sets the reference no.
        /// </summary>
        /// <value>
        /// The reference no.
        /// </value>
        public string RefNo { get; set; }

        /// <summary>
        /// Gets or sets the bet type identifier.
        /// </summary>
        /// <value>
        /// The bet type identifier.
        /// </value>
        public int BetTypeId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the odds.
        /// </summary>
        /// <value>
        /// Input The odds.
        /// </value>
        public decimal Odds { get; set; }

        /// <summary>
        /// Gets or sets the stake.
        /// </summary>
        /// <value>
        /// The stake.
        /// </value>
        public decimal Stake { get; set; }

        /// <summary>
        /// Gets or sets the bet team.
        /// </summary>
        /// <value>
        /// The bet team.
        /// </value>
        public string BetTeam { get; set; }

        /// <summary>
        /// Gets or sets the super PT.
        /// </summary>
        /// <value>
        /// The super PT.
        /// </value>
        public decimal SuperPT { get; set; }

        /// <summary>
        /// Gets or sets the master PT.
        /// </summary>
        /// <value>
        /// The master PT.
        /// </value>
        public decimal MasterPT { get; set; }

        /// <summary>
        /// Gets or sets the agent PT.
        /// </summary>
        /// <value>
        /// The agent PT.
        /// </value>
        public decimal AgentPT { get; set; }

        #endregion ITicketData Members
    }
}