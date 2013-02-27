namespace Fanex.Age.DTO.Reports.CasinoWinloss
{
    using System;
    using System.Collections.Generic;

    public class CSWinlossDetailMasterItem
    {
        public string GameTypeName { get; set; }

        public decimal Turnover { get; set; }

        public decimal AgentTotal { get; set; }

        public decimal MasterWinloss { get; set; }

        public decimal MasterComm { get; set; }

        public decimal MasterTotal { get; set; }

        public decimal Company { get; set; }
    }
}