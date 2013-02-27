/*
 * Created 20120220@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.WinlossAnalysis
{
    using System;
    using System.Collections.Generic;

    public class WinlossAnalysisMasterItem
    {
        public int BreakDown { get; set; }

        public decimal Turnover { get; set; }

        public decimal MemberWinloss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal MemberTotal { get; set; }

        public decimal AgentWinloss { get; set; }

        public decimal AgentComm { get; set; }

        public decimal AgentTotal { get; set; }

        public decimal MasterWinloss { get; set; }

        public decimal MasterComm { get; set; }

        public decimal MasterTotal { get; set; }

        public decimal Company { get; set; }

        public int BetCount { get; set; }
    }
}