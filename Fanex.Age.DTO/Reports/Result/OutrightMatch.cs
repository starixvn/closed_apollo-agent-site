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

    public class OutrightMatch
    {
        public int LeagueId { get; set; }

        public byte SportType { get; set; }

        public DateTime EventDate { get; set; }

        public int TeamId { get; set; }

        public bool? IsPending { get; set; }

        public bool? IsHTCal { get; set; }

        public bool? Calculate { get; set; }

        public virtual string Status { get; set; }
    }
}