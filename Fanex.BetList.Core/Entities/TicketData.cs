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

    [Serializable]
    public class TicketData : ITicketData
    {
        #region ITicketData Members

        public string RefNo { get; set; }

        public int BetTypeId { get; set; }

        /// <summary>
        /// Running, Waiting, Refund, Void, Won, Lose, Draw, Reject
        /// </summary>
        public string Status { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public string BetTeam { get; set; }

        public decimal SuperPT { get; set; }

        public decimal MasterPT { get; set; }

        public decimal AgentPT { get; set; }

        #endregion
    }
}