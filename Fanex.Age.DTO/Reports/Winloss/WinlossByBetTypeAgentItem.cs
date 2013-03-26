/*
 * Created 20120223@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.Winloss
{
    public class WinlossByBetTypeAgentItem
    {
        public int BetType { get; set; }

        public int LiveIndicator { get; set; }

        public decimal GrossComm { get; set; }

        public decimal Turnover { get; set; }

        public decimal MemberWinloss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal MemberTotal { get; set; }

        public decimal AgentWinloss { get; set; }

        public decimal AgentComm { get; set; }

        public decimal AgentTotal { get; set; }

        public decimal Company { get; set; }
    }
}