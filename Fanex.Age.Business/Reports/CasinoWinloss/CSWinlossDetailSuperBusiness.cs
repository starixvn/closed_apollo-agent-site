/*
 * Created 20120223@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Business.Reports.CasinoWinloss
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Reports.CasinoWinloss;

    public class CSWinlossDetailSuperBusiness
    {
        public IList<CSWinlossDetailSuperItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new CSWinlossDetailSuperItem
                          {
                              GameTypeName = c.Field<string>("gametypename"),
                              Turnover = c.Field<decimal>("turnover"),
                              MasterTotal = c.Field<decimal>("MasterTotal"),
                              SuperWinloss = c.Field<decimal>("SuperWinloss"),
                              SuperComm = c.Field<decimal>("SuperRolling"),
                              SuperTotal = c.Field<decimal>("SuperTotal"),
                              Company = c.Field<decimal>("company")
                          }).ToList();

            return result;
        }
    }
}