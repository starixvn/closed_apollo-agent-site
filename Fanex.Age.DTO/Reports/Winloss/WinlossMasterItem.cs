/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.Winloss
{
    using System;
    using System.Collections.Generic;

    public class WinlossMasterItem
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public decimal Turnover { get; set; }

        public decimal GrossComm { get; set; }

        public decimal AgentTotal { get; set; }

        public decimal MasterWinloss { get; set; }

        public decimal MasterComm { get; set; }

        public decimal MasterTotal { get; set; }

        public decimal Company { get; set; }
    }
}