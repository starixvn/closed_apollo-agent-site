/*
 * Created 20120223@Leo
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

    public class WinlossByBetTypeSuperBusiness
    {
        public IList<WinlossByBetTypeSuperItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new WinlossByBetTypeSuperItem
                          {
                              BetType = c.Field<int>("bettype"),
                              LiveIndicator = c.Field<int>("liveindicator"),
                              GrossComm = c.Field<decimal>("GrossComm"),
                              Turnover = c.Field<decimal>("turnover"),
                              MasterTotal = c.Field<decimal>("MAgTotal"),
                              Winloss = c.Field<decimal>("SAgWinlost"),
                              Comm = c.Field<decimal>("comm"),
                              SuperTotal = c.Field<decimal>("SAgTotal"),
                              Company = c.Field<decimal>("company")
                          }).ToList();

            return result;
        }
    }
}