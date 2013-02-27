namespace Fanex.Age.DTO.Reports.Winloss
{
    using System;
    using System.Collections.Generic;

    public class WinlossDetailSuperItem
    {
        public string UserName { get; set; }

        public int CustId { get; set; }

        public decimal Turnover { get; set; }

        public decimal GrossComm { get; set; }

        public decimal MemberWinloss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal MemberTotal { get; set; }

        public decimal AgentWinloss { get; set; }

        public decimal AgentComm { get; set; }

        public decimal AgentTotal { get; set; }

        public decimal MasterWinloss { get; set; }

        public decimal MasterComm { get; set; }

        public decimal MasterTotal { get; set; }

        public decimal SuperWinloss { get; set; }

        public decimal SuperComm { get; set; }

        public decimal SuperTotal { get; set; }

        public decimal Company { get; set; }
    }
}