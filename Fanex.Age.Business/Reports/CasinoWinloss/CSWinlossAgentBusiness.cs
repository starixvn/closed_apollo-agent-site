namespace Fanex.Age.Business.Reports.CasinoWinloss
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Reports.CasinoWinloss;

    public class CSWinlossAgentBusiness
    {
        public IList<CSWinlossAgentItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new CSWinlossAgentItem
                          {
                              Username = c.Field<string>("username"),
                              CustId = c.Field<int>("IBCCustID"),
                              Turnover = c.Field<decimal>("turnover"),
                              RollingTurnover = c.Field<decimal>("RollingTurnover"),
                              MemberWinloss = c.Field<decimal>("MemberWinLoss"),
                              MemberComm = c.Field<decimal>("MemberRolling"),
                              MemberTotal = c.Field<decimal>("MemberTotal"),
                              AgentWinloss = c.Field<decimal>("AgentWinLoss"),
                              AgentComm = c.Field<decimal>("AgentRolling"),
                              AgentTotal = c.Field<decimal>("AgentTotal"),
                              Company = c.Field<decimal>("company")
                          }).ToList();

            return result;
        }
    }
}