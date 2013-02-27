/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Business.Reports.Common
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Reports.Common;

    public class MatchResultEnteredBusiness
    {
        public IList<MatchResultEnteredItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new MatchResultEnteredItem
                          {
                              SportType = c.Field<int>("sporttype"),
                              SportName = c.Field<string>("sportname"),
                              YesterdayEntered = c.Field<int>("yesterdayentered"),
                              YesterdayNonEntered = c.Field<int>("yesterdaynonentered"),
                              TodayEntered = c.Field<int>("todayentered"),
                              TodayNonEntered = c.Field<int>("todaynonentered")
                          }).ToList();

            return result;
        }
    }
}