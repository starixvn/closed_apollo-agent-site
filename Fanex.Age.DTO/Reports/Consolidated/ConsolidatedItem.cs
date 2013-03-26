/*
 * Created 20120221@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.Consolidated
{
    public class ConsolidatedItem
    {
        public int BreakDown { get; set; }

        public string Username { get; set; }

        public byte Currency { get; set; }

        public decimal Turnover { get; set; }

        public decimal MemberWinloss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal MemberTotal { get; set; }

        public decimal OwnerWinloss { get; set; }

        public decimal OwnerComm { get; set; }

        public decimal OwnerTotal { get; set; }

        public decimal Company { get; set; }

        public int BetCount { get; set; }

        public double RM_Turnover { get; set; }

        public double RM_MemberWinloss { get; set; }

        public double RM_MemberComm { get; set; }

        public double RM_MemberTotal { get; set; }

        public double RM_OwnerWinloss { get; set; }

        public double RM_OwnerComm { get; set; }

        public double RM_OwnerTotal { get; set; }

        public double RM_Company { get; set; }

        public double USD_Turnover { get; set; }

        public double USD_MemberWinloss { get; set; }

        public double USD_MemberComm { get; set; }

        public double USD_MemberTotal { get; set; }

        public double USD_OwnerWinloss { get; set; }

        public double USD_OwnerComm { get; set; }

        public double USD_OwnerTotal { get; set; }

        public double USD_Company { get; set; }
    }
}