namespace Fanex.BetList.Core.Entities
{
    public interface ISystemParlaySerial
    {
        int AwayId { get; set; }

        int HomeId { get; set; }

        long Serial { get; set; }
    }
}