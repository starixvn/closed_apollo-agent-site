namespace Fanex.Age.Business.Reports.CasinoWinloss
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    //using Fanex.Age.DTO.Reports.CasinoWinloss;

    //public class CSWinlossMasterBusiness
    //{
    //    public IList<CSWinlossMasterItem> ToList(DataTable dt)
    //    {
    //        if (dt == null)
    //        {
    //            return null;
    //        }

    //        var result = (from c in dt.AsEnumerable()
    //                      select new CSWinlossMasterItem
    //                      {
    //                          Username = c.Field<string>("username"),
    //                          CustId = c.Field<int>("IBCCustID"),
    //                          Turnover = c.Field<decimal>("turnover"),
    //                          RollingTurnover = c.Field<decimal>("RollingTurnover"),
    //                          MemberWinloss = c.Field<decimal>("MemberWinLoss"),
    //                          MemberComm = c.Field<decimal>("MemberRolling"),
    //                          MemberTotal = c.Field<decimal>("MemberTotal"),
    //                          AgentWinloss = c.Field<decimal>("AgentWinLoss"),
    //                          AgentComm = c.Field<decimal>("AgentRolling"),
    //                          AgentTotal = c.Field<decimal>("AgentTotal"),
    //                          MasterWinloss = c.Field<decimal>("MasterWinLoss"),
    //                          MasterComm = c.Field<decimal>("MasterRolling"),
    //                          MasterTotal = c.Field<decimal>("MasterTotal"),
    //                          Company = c.Field<decimal>("company")
    //                      }).ToList();

    //        return result;
    //    }
    //}
}