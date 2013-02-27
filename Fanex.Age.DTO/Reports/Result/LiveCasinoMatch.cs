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

    public class LiveCasinoMatch : SportMatch1
    {
        public string ShowTime { get; set; }

        public virtual int HomeId2 { get; set; }
    }
}