namespace Fanex.Age.DTO.Reports.MatchWinloss
{
    public class MatchWinLossDetailItem
    {
        public string ShowTime { get; set; }

        public bool IsNeutral { get; set; }

        public short BetType { get; set; }

        public int BetGroup { get; set; }

        public int LeagueId { get; set; }

        public int LiveIndicator { get; set; }

        public int MatchId { get; set; }

        public string MatchCode { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public string Race { get; set; }

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

        public decimal CompanyComm { get; set; }

        public decimal CompanyTotal { get; set; }
    }
}