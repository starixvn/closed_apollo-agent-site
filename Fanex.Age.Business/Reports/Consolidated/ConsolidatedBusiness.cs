/*
 * Created 20120221@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Business.Reports.Consolidated
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Reports.Consolidated;

    public class ConsolidatedBusiness
    {
        public IList<ConsolidatedItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new ConsolidatedItem
                          {
                              BreakDown = c.Field<int>("breakdown"),
                              Username = c.Field<string>("username"),
                              Currency = c.Field<byte>("currency"),
                              Turnover = c.Field<decimal>("turnover"),
                              MemberWinloss = c.Field<decimal>("member_winlost"),
                              MemberComm = c.Field<decimal>("member_comm"),
                              MemberTotal = c.Field<decimal>("member_total"),
                              OwnerWinloss = c.Field<decimal>("owner_winlost"),
                              OwnerComm = c.Field<decimal>("owner_comm"),
                              OwnerTotal = c.Field<decimal>("owner_total"),
                              Company = c.Field<decimal>("company"),
                              BetCount = c.Field<int>("betcount"),
                              RM_Turnover = c.Field<double>("rmturnover"),
                              RM_MemberWinloss = c.Field<double>("rmmember_winlost"),
                              RM_MemberComm = c.Field<double>("rmmember_comm"),
                              RM_MemberTotal = c.Field<double>("rmmember_total"),
                              RM_OwnerWinloss = c.Field<double>("rmowner_winlost"),
                              RM_OwnerComm = c.Field<double>("rmowner_comm"),
                              RM_OwnerTotal = c.Field<double>("rmowner_total"),
                              RM_Company = c.Field<double>("rmcompany"),
                              USD_Turnover = c.Field<double>("usdturnover"),
                              USD_MemberWinloss = c.Field<double>("usdmember_winlost"),
                              USD_MemberComm = c.Field<double>("usdmember_comm"),
                              USD_MemberTotal = c.Field<double>("usdmember_total"),
                              USD_OwnerWinloss = c.Field<double>("usdowner_winlost"),
                              USD_OwnerComm = c.Field<double>("usdowner_comm"),
                              USD_OwnerTotal = c.Field<double>("usdowner_total"),
                              USD_Company = c.Field<double>("usdcompany"),
                          }).ToList();

            return result;
        }
    }
}