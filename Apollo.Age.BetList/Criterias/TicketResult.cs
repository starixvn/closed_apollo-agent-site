// --------------------------------------------------------------------------
// <copyright file="TicketResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.17">Created and Implement</change>
// </history>
namespace Apollo.Age.BetList.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// Ticket result for bet list.
    /// </summary>
    [Serializable]
    public class TicketResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketResult" /> class.
        /// </summary>
        public TicketResult()
        {
            this.TicketList = new List<ITicket>();
            this.TicketDataList = new List<ITicketData>();
        }

        public IList<ITicket> TicketList { get; set; }

        public List<ITicketData> TicketDataList { get; set; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>Ticket result.</returns>
        public TicketResult Clone()
        {
            return new TicketResult
            {
                TicketList = this.TicketList.ToList(),
                TicketDataList = this.TicketDataList.ToList()
            };
        }
    }
}