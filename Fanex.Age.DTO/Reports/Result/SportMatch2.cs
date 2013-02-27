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

    public abstract class SportMatch2
    {
        public virtual int LeagueId { get; set; }

        public virtual int Place { get; set; }

        public virtual int RaceName { get; set; }

        public virtual string HorseNo { get; set; }

        public virtual int HorseId { get; set; }

        public virtual decimal PlaceOdds { get; set; }

        public virtual decimal Odds { get; set; }

        public virtual DateTime EventDate { get; set; }

        public virtual string FOddsStatus { get; set; }

        public virtual bool IsSCR { get; set; }

        public virtual bool IsFavor { get; set; }
    }
}