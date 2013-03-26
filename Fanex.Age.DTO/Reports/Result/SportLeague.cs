/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.Result
{
    using System.Collections.Generic;

    public class SportLeague<T> where T : new()
    {
        public SportLeague()
        {
            this.Matches = new List<T>();
        }

        public int LeagueId { get; set; }

        public IList<T> Matches { get; set; }
    }
}