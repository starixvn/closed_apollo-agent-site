using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.BetList.Core.Entities;

namespace Apollo.Age.BetList.Models
{
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