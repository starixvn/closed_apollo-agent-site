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
        /// <summary>
        /// Get all league which has event happens.
        /// </summary>
        /// <param name="fromDate">Valid from date.</param>
        /// <param name="toDate">Valid to date.</param>
        /// <param name="sportTypes">List of sport type id.</param>
        /// <param name="isOutright">Is outright sport.</param>
        /// <returns>Return a list of league which having match/even happens.</returns>
        public IEnumerable<League> GetLeague(
            DateTime fromDate,
            DateTime? toDate = null,
            string sportTypes = null,
            bool isOutright = false,
            bool isShowDataTest = false)
        {
            var criteria = new GetLeagueCriteria
            {
                FromDate = fromDate,
                SportTypes = sportTypes,
                IsOutright = isOutright,
                ToDate = toDate,
                isShowAll = isShowDataTest

            };

            return this.GetData<League>(criteria);
        }

        /// <summary>
        /// Get wagers data.
        /// </summary>
        /// <typeparam name="T">Type of criteria.</typeparam>
        /// <param name="criteria">Criteria class.</param>
        /// <returns>List wagers.</returns>
        public IEnumerable<T> GetWagerData<T>(WagerCriteriaBase criteria)
        {
            // Revert Comm Group Name.
            criteria.UGroup = RevertCommGroupName(criteria.UGroup);

            var result = this.GetData<T>(criteria);

            if (!ReferenceEquals(result, null))
            {
                criteria.TotalItems = result.Count();

                return result.Skip(criteria.PageSize * criteria.PageIndex).Take(criteria.PageSize).ToList();
            }

            return new List<T>();
        }

        /// <summary>
        /// Get event date for early market.
        /// </summary>
        /// <param name="sportTypes">Sport type ids.</param>
        /// <returns>List date time.</returns>
        public IEnumerable<DateTime> GetEventDate(string sportTypes)
        {
            var criteria = new GetEventDateCriteria
            {
                Sporttypes = sportTypes
            };

            return this.GetData<DateTime>(criteria);
        }

        /// <summary>
        /// Get main shareholder.
        /// </summary>
        /// <param name="betTypes">Bet type id.</param>
        /// <returns>List agent.</returns>
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
