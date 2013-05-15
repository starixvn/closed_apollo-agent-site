// --------------------------------------------------------------------------
// <copyright file="TicketText.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.14">Created and Implement</change>
// </history>
namespace Apollo.Age.BetList.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TicketText
    {
        public string UserName { get; set; }

        public string TransTime { get; set; }

        public string Choice { get; set; }

        public string Odds { get; set; }

        public string Stake { get; set; }

        public string WinLoss { get; set; }

        public string AgentComm { get; set; }

        public string AgentShare { get; set; }

        public string Status { get; set; }
    }
}