/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.Result
{
    using System;
    using System.Collections.Generic;

    public class HorseRacingMatch
    {
        public virtual int LeagueId { get; set; }

        public virtual int RaceName { get; set; }

        public IList<HorseRacingRace> Races { get; set; }
    }
}