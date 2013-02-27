namespace Fanex.Age.Business.Reports.Winloss
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Reports.Winloss;

    public class WinlossByBetTypeAgentBusiness
    {
        public IList<WinlossByBetTypeAgentItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new WinlossByBetTypeAgentItem
                          {
                              BetType = c.Field<int>("bettype"),
                              LiveIndicator = c.Field<int>("liveindicator"),
                              GrossComm = c.Field<decimal>("GrossComm"),
                              Turnover = c.Field<decimal>("turnover"),
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