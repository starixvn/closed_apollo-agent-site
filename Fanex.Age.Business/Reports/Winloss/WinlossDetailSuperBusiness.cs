/*
 * Created 20120220@Leo
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

    public class WinlossDetailSuperBusiness
    {
        public IList<WinlossDetailSuperItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new WinlossDetailSuperItem
                          {
                              UserName = c.Field<string>("username"),
                              CustId = c.Field<int>("custid"),
                              Turnover = c.Field<decimal>("turnover"),
                              GrossComm = c.Field<decimal>("grosscomm"),
                              MemberWinloss = c.Field<decimal>("memberwinlost"),
                              MemberComm = c.Field<decimal>("playercomm"),
                              MemberTotal = c.Field<decimal>("membertotal"),
                              AgentWinloss = c.Field<decimal>("agwinlost"),
                              AgentComm = c.Field<decimal>("agcomm"),
                              AgentTotal = c.Field<decimal>("agtotal"),
                              MasterWinloss = c.Field<decimal>("magwinlost"),
                              MasterComm = c.Field<decimal>("magcomm"),
                              MasterTotal = c.Field<decimal>("magtotal"),
                              SuperWinloss = c.Field<decimal>("sagwinlost"),
                              SuperComm = c.Field<decimal>("sagcomm"),
                              SuperTotal = c.Field<decimal>("sagtotal"),
                              Company = c.Field<decimal>("company")
                          }).ToList();

            return result;
        }
    }
}