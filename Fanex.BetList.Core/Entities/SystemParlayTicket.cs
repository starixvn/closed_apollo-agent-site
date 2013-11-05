// --------------------------------------------------------------------------
// <copyright file="SystemParlayTicket.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.11.5">Created</change>
// </history>
namespace Fanex.BetList.Core.Entities
{
    using System;

    /// <summary>
    /// System Parlay Ticket.
    /// </summary>
    [Serializable]
    public class SystemParlayTicket : ISystemParlayTicket
    {
        public decimal MOdds { get; set; }

        public decimal PlayerComm { get; set; }

        public long Serial { get; set; }

        public decimal Stake { get; set; }

        public string Status { get; set; }

        public int StatusId { get; set; }

        public long TransId { get; set; }

        public decimal WinLost { get; set; }

        public DateTime WinLostDate { get; set; }
    }
}