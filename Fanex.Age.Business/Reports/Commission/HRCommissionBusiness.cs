/*
 * Created 20120223@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Business.Reports.Commission
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Reports.Commission;

    public class HRCommissionBusiness
    {
        public IList<HRCommissionItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new HRCommissionItem
                          {
                              CustId = c.Field<int>("custid"),
                              Username = c.Field<string>("username"),
                              WinComm = c.Field<decimal>("tote_win"),
                              PlaceComm = c.Field<decimal>("tote_palce"),
                              WinPlaceComm = c.Field<decimal>("tote_winplace"),
                              TotalComm = c.Field<decimal>("tote_total")
                          }).ToList();

            return result;
        }
    }
}