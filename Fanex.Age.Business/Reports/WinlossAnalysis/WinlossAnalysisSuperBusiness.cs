/*
 * Created 20120220@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Business.Reports.WinlossAnalysis
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Reports.WinlossAnalysis;

    public class WinlossAnalysisSuperBusiness
    {
        public IList<WinlossAnalysisSuperItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new WinlossAnalysisSuperItem
                          {
                              BreakDown = c.Field<int>("breakdown"),
                              Turnover = c.Field<decimal>("turnover"),
                              MemberWinloss = c.Field<decimal>("member_winlost"),
                              MemberComm = c.Field<decimal>("member_comm"),
                              MemberTotal = c.Field<decimal>("member_total"),
                              AgentWinloss = c.Field<decimal>("agent_winlost"),
                              AgentComm = c.Field<decimal>("agent_comm"),
                              AgentTotal = c.Field<decimal>("agent_total"),
                              MasterWinloss = c.Field<decimal>("master_winlost"),
                              MasterComm = c.Field<decimal>("master_comm"),
                              MasterTotal = c.Field<decimal>("master_total"),
                              SuperWinloss = c.Field<decimal>("super_winlost"),
                              SuperComm = c.Field<decimal>("super_comm"),
                              SuperTotal = c.Field<decimal>("super_total"),
                              Company = c.Field<decimal>("company"),
                              BetCount = c.Field<int>("betcount")
                          }).ToList();
            return result;
        }
    }
}