namespace Fanex.Age.DTO.Reports.BingoWinloss
{
    using System;
    using System.Collections.Generic;

    public class BingoWinlossAgentItem
    {
        public string Username { get; set; }

        public int CustId { get; set; }

        public decimal Turnover { get; set; }

        public decimal RollingTurnover { get; set; }

        public decimal MemberWinloss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal MemberTotal { get; set; }

        public decimal AgentWinloss { get; set; }

        public decimal AgentComm { get; set; }

        public decimal AgentTotal { get; set; }

        public decimal Company { get; set; }
    }
}