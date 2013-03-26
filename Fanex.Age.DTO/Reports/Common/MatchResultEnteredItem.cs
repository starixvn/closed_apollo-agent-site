/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.Common
{
    public class MatchResultEnteredItem
    {
        public int SportType { get; set; }

        public string SportName { get; set; }

        public int YesterdayEntered { get; set; }

        public int YesterdayNonEntered { get; set; }

        public int TodayEntered { get; set; }

        public int TodayNonEntered { get; set; }
    }
}