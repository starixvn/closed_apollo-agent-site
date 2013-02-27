// -----------------------------------------------------------------------
// <copyright file="MatchWinLossBusiness.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Age.Business.Reports.MatchWinloss
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Reports.MatchWinloss;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchWinLossBusiness
    {
        public IList<MatchWinLossItem> ToList(DataTable dt)
        {
            var result = (from c in dt.AsEnumerable()
                          select new MatchWinLossItem
                          {
                              BetTypeId = c.Field<short>("bettype"),
                              HomeId = c.Field<int>("homeid"),
                              AwayId = c.Field<int>("awayid"),
                              MatchId = c.Field<int>("matchid"),
                              MatchCode = c.Field<string>("matchcode"),
                              ShowTime = c.Field<string>("showtime"),
                              GrossComm = c.Field<decimal>("grosscomm"),
                              Comm = c.Field<decimal>("commission"),
                              PositionTaking = c.Field<decimal>("positiontaking"),
                              LiveIndicator = c.Field<int>("liveindicator"),
                              IsNeutral = c.Field<bool>("isneutral")
                          }).ToList();

            foreach (var item in result)
            {
                item.Total = item.GrossComm + item.Comm + item.PositionTaking;
            }

            return result;
        }
    }
}