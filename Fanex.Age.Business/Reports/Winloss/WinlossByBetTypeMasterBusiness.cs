namespace Fanex.Age.Business.Reports.Winloss
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Reports.Winloss;

    public class WinlossByBetTypeMasterBusiness
    {
        public IList<WinlossByBetTypeMasterItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new WinlossByBetTypeMasterItem
                          {
                              BetType = c.Field<int>("bettype"),
                              LiveIndicator = c.Field<int>("liveindicator"),
                              GrossComm = c.Field<decimal>("GrossComm"),
                              Turnover = c.Field<decimal>("turnover"),
                              MasterTotal = c.Field<decimal>("MAgTotal"),
                              Winloss = c.Field<decimal>("MAgWinlost"),
                              Comm = c.Field<decimal>("comm"),
                              AgentTotal = c.Field<decimal>("AgTotal"),
                              Company = c.Field<decimal>("company")
                          }).ToList();

            return result;
        }
    }
}