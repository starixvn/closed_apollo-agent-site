/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Business.Reports.Winloss
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Reports.Winloss;

    public class WinlossSuperBusiness
    {
        public IList<WinlossSuperItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new WinlossSuperItem
                          {
                              UserName = c.Field<string>("username"),
                              Turnover = c.Field<decimal>("turnover"),
                              GrossComm = c.Field<decimal>("grosscomm"),
                              MasterTotal = c.Field<decimal>("magtotal"),
                              SuperWinloss = c.Field<decimal>("sagwinlost"),
                              SuperComm = c.Field<decimal>("comm"),
                              SuperTotal = c.Field<decimal>("sagtotal"),
                              Company = c.Field<decimal>("company")
                          }).ToList();

            return result;
        }
    }
}