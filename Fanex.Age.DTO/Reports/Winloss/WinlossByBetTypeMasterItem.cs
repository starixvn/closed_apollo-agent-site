namespace Fanex.Age.DTO.Reports.Winloss
{
    public class WinlossByBetTypeMasterItem
    {
        public int BetType { get; set; }

        public int LiveIndicator { get; set; }

        public decimal GrossComm { get; set; }

        public decimal Turnover { get; set; }

        public decimal Winloss { get; set; }

        public decimal Comm { get; set; }

        public decimal AgentTotal { get; set; }

        public decimal MasterTotal { get; set; }

        public decimal Company { get; set; }
    }
}