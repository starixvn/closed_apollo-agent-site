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

    public class TicketText
    {
        public string UserName { get; set; }

        public long TransId { get; set; }

        public string TransTime { get; set; }

        public string Discount { get; set; }

        public string GameType { get; set; }

        public string Choice { get; set; }

        public decimal Stake { get; set; }

        public decimal Result { get; set; }

        public decimal AgentWinlossValue { get; set; }

        public decimal AgentCommValue { get; set; }

        public string WinLoss { get; set; }

        public string AgentComm { get; set; }

        public string AgentShare { get; set; }

        public string Status { get; set; }
    }
}