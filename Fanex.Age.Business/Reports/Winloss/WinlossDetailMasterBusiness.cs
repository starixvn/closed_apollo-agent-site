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

    public class WinlossDetailMasterBusiness
    {
        public IList<WinlossDetailMasterItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new WinlossDetailMasterItem
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
                              Company = c.Field<decimal>("company")
                          }).ToList();

            return result;
        }
    }
}