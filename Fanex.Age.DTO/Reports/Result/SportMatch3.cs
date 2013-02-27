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

    public abstract class SportMatch3
    {
        public virtual int LeagueId { get; set; }

        public virtual DateTime KickOffTime { get; set; }

        public virtual int MatchId { get; set; }

        public virtual string MatchCode { get; set; }

        public virtual byte? Seq { get; set; }

        public virtual byte? Number { get; set; }

        public virtual string EventStatus { get; set; }

        public virtual bool? IsPending { get; set; }
    }
}