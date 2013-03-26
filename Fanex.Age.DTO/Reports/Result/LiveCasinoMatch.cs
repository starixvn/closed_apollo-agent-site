/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.Result
{
    public class LiveCasinoMatch : SportMatch1
    {
        public string ShowTime { get; set; }

        public virtual int HomeId2 { get; set; }
    }
}