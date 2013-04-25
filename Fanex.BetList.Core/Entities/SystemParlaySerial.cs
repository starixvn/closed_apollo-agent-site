namespace Fanex.BetList.Core.Entities
{
    using System;

    [Serializable]
    public class SystemParlaySerial : ISystemParlaySerial
    {
        public int AwayId { get; set; }

        public int HomeId { get; set; }

        public long Serial { get; set; }
    }
}