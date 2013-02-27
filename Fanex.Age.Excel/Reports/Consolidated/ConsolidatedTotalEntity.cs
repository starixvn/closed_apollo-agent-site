using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Age.Excel.Reports.Consolidated
{
    class ConsolidatedTotalEntity
    {
        public byte Currency { get; set; }
        public decimal TotalTurnover { get; set; }
        public decimal TotalMemberWinloss { get; set; }
        public decimal TotalMemberComm { get; set; }
        public decimal TotalMemberTotal { get; set; }
        public decimal TotalOwnerWinloss { get; set; }
        public decimal TotalOwnerComm { get; set; }
        public decimal TotalOwnerTotal { get; set; }
        public decimal TotalCompany { get; set; }
        public decimal TotalBetCount { get; set; }
    }
}
