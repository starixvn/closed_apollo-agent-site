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

    public class WinlossAgentBusiness
    {
        public IList<WinlossAgentItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new WinlossAgentItem
                          {
                              CustId = c.Field<int>("custid"),
                              UserName = c.Field<string>("username"),
                              Turnover = c.Field<decimal>("turnover"),
                              GrossComm = c.Field<decimal>("grosscomm"),
                              MemberWinloss = c.Field<decimal>("memberwinlost"),
                              MemberComm = c.Field<decimal>("playercomm"),
                              MemberTotal = c.Field<decimal>("membertotal"),
                              AgentWinloss = c.Field<decimal>("agwinlost"),
                              AgentComm = c.Field<decimal>("agcomm"),
                              AgentTotal = c.Field<decimal>("agtotal"),
                              Company = c.Field<decimal>("company")
                          }).ToList();

            return result;
        }
    }
}