/*
 * Created 20130318@Lewis
 * Revision:
 *
 */

namespace Fanex.Age.DTO.Admin.ExchangeRate
{
    public class ExchRate
    {
        public int ExchangeId { get; set; }

        public string Currency { get; set; }

        public int? MultipliedBy { get; set; }

        public double BaseRate { get; set; }

        public double MinBet { get; set; }

        public double MinBetLive { get; set; }

        public string Description { get; set; }
    }
}