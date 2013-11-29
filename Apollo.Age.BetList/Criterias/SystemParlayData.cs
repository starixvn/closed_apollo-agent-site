// --------------------------------------------------------------------------
// <copyright file="SystemParlayData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">System Parlay</change>
// </history>
namespace Apollo.Age.BetList.Criterias
{
    using System.Collections.Generic;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// System parlay class.
    /// </summary>
    public class SystemParlayData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemParlayData" /> class.
        /// </summary>
        public SystemParlayData()
        {
            this.SerialList = new List<ISystemParlaySerial>();
            this.TicketList = new List<ISystemParlayTicket>();
        }

        public decimal Outstanding { get; set; }

        public IList<ISystemParlaySerial> SerialList { get; set; }

        public IList<ISystemParlayTicket> TicketList { get; set; }
    }
}