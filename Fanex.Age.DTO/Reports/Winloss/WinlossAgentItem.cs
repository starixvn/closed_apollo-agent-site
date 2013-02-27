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

    public class WinlossAgentItem
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public decimal Turnover { get; set; }

        public decimal GrossComm { get; set; }

        public decimal MemberWinloss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal MemberTotal { get; set; }

        public decimal AgentWinloss { get; set; }

        public decimal AgentComm { get; set; }

        public decimal AgentTotal { get; set; }

        public decimal Company { get; set; }
    }
}