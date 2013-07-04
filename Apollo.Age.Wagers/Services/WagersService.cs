// --------------------------------------------------------------------------
// <copyright file="WagersService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.28">Created</change>
// </history>

namespace Apollo.Age.Wagers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Entities;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Wagers.Criterias;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WagersService : QueryService, IWagersService
    {
        public IEnumerable<League> GetLeague(DateTime fromDate, string sportTypes)
        {
            var criteria = new GetLeagueCriteria
            {
                FromDate = fromDate,
                SportTypes = sportTypes
            };

            return this.GetData<League>(criteria);
        }

        public IEnumerable<T> GetWagerData<T>(WagerCriteriaBase criteria)
        {
            var result = this.GetData<T>(criteria);

            if (!ReferenceEquals(result, null))
            {
                criteria.TotalItems = result.Count();

                return result.Skip(criteria.PageSize * criteria.PageIndex).Take(criteria.PageSize).ToList();
            }

            return new List<T>();
        }

        public IEnumerable<DateTime> GetEventDate(string sportTypes)
        {
            var criteria = new GetEventDateCriteria
            {
                Sporttypes = sportTypes
            };
            return this.GetData<DateTime>(criteria);
        }

        public IEnumerable<Agent> GetMainShareHolder(string betTypes)
        {
            var criteria = new GetMainShareHolderCriteria
            {
                Bettypes = betTypes
            };

            return this.GetData<Agent>(criteria);
        }
    }
}
