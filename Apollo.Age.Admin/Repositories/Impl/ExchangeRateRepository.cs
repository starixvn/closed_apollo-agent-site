// --------------------------------------------------------------------------
// <copyright file="AdminRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Admin.Repositories.Impl
{
    using System.Collections.Generic;
    using Apollo.Age.Admin.Criterias;
    using Apollo.Age.Admin.Repositories.Interface;
    using Apollo.Age.DTO.Admin.ExchangeRate;
    using Fanex.Common.Helper;
    using Fanex.Data;

    public class ExchangeRateRepository : IExchangeRateRepository
    {
        public IEnumerable<ExchRate> GetAllExchangeRate(ref GetAllExchangeRateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return db.Query<ExchRate>(criteria.GetSpParams());
                    }
                }

                return new List<ExchRate>();
            }
            catch
            {
                throw;
            }
        }
    }
}