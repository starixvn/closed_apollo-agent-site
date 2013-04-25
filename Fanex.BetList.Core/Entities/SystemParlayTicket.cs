namespace Fanex.BetList.Core.Entities
{
    using System;

    [Serializable]
    public class SystemParlayTicket : ISystemParlayTicket
    {
        public decimal MOdds { get; set; }

        public decimal PlayerComm { get; set; }

        public long Serial { get; set; }

        public decimal Stake { get; set; }

        public string Status { get; set; }

        public int StatusId { get; set; }

        public long TransId { get; set; }

        public decimal WinLost { get; set; }

        public DateTime WinLostDate { get; set; }
    }
}