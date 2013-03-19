/*
 * Created 20130318@Lewis
 * Revision:
 *
 */

namespace Fanex.Age.Business.Admin.ExchangeRate
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Admin.ExchangeRate;

    public class ExchangeRateBusiness
    {
        public IList<ExchRate> ToList(DataTable data)
        {
            if (data == null) return null;

            var result = new List<ExchRate>();

            foreach (var rate in data.AsEnumerable())
            {
                var exchRate = new ExchRate();

                exchRate.ExchangeId = rate.Field<int>("ExchangeId");
                exchRate.Currency = rate.Field<string>("Currency");
                exchRate.MultipliedBy = rate.Field<int>("MultipliedBy");
                exchRate.BaseRate = rate.Field<double>("ActualRate");
                exchRate.MinBet = rate.Field<double>("MinBet");
                exchRate.MinBetLive = rate.Field<double>("MinBetLive");
                exchRate.Description = rate.Field<string>("Description");

                result.Add(exchRate);
            }

            return result;

            //return (from e in data.AsEnumerable()
            //        select new ExchRate
            //        {
            //            ExchangeId = e.Field<int>("ExchangeId"),
            //            Currency = e.Field<string>("Currency"),
            //            MultipliedBy = e.Field<int>("MultipliedBy"),
            //            BaseRate = e.Field<float>("ActualRate"),
            //            MinBet = e.Field<float>("MinBet"),
            //            MinBetLive = e.Field<float>("MinBetLive"),
            //            Description = e.Field<string>("Description")
            //        }).ToList();
        }
    }
}