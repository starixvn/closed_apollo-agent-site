namespace Apollo.Age.BetList.Criterias
{
    using System;

    public class SystemParlayDetailCriteria
    {
        public long RefNo { get; set; }

        public int CustId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public string Language { get; set; }
    }
}