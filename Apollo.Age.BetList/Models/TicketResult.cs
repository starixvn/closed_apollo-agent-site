// --------------------------------------------------------------------------
// <copyright file="TicketResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.17">Created and Implement</change>
// </history>
namespace Apollo.Age.BetList.Models
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// Ticket result for betlist
    /// </summary>
    public class TicketResult
    {
        public TicketResult()
        {
            TicketList = new List<ITicket>();
            TicketDataList = new List<ITicketData>();
        }

        public IList<ITicket> TicketList { get; set; }

        public List<ITicketData> TicketDataList { get; set; }
    }
}