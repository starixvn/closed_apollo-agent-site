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

    public abstract class SportMatch1
    {
        public virtual int LeagueId { get; set; }

        public virtual DateTime KickOffTime { get; set; }

        public virtual bool IsPending { get; set; }

        public virtual bool IsHTCal { get; set; }

        public virtual bool Calculate { get; set; }

        public virtual int HomeId { get; set; }

        public virtual int AwayId { get; set; }

        public virtual int? HTHomeScore { get; set; }

        public virtual int? HTAwayScore { get; set; }

        public virtual int? FTHomeScore { get; set; }

        public virtual int? FTAwayScore { get; set; }

        public virtual string EventStatus { get; set; }

        public virtual string BetStatus { get; set; }

        public virtual bool IsNeutral { get; set; }
    }
}