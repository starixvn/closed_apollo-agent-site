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

    public class WinlossMasterBusiness
    {
        public IList<WinlossMasterItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new WinlossMasterItem
                          {
                              CustId = c.Field<int>("custid"),
                              UserName = c.Field<string>("username"),
                              Turnover = c.Field<decimal>("turnover"),
                              GrossComm = c.Field<decimal>("grosscomm"),
                              AgentTotal = c.Field<decimal>("agtotal"),
                              MasterWinloss = c.Field<decimal>("magwinlost"),
                              MasterComm = c.Field<decimal>("comm"),
                              MasterTotal = c.Field<decimal>("magtotal"),
                              Company = c.Field<decimal>("company")
                          }).ToList();

            return result;
        }
    }
}