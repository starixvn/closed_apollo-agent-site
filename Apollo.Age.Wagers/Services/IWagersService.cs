// --------------------------------------------------------------------------
// <copyright file="IWagersService.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.Age.Entities;
    using Apollo.Age.Wagers.Criterias;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWagersService
    {
        /// <summary>
        /// Get league.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="sportTypes">Sport type id.</param>
        /// <returns>List league.</returns>
        IEnumerable<League> GetLeague(DateTime fromDate, DateTime? toDate = null, string sportTypes = null, bool isOutright = false, bool isShowDataTest = false);

        /// <summary>
        /// Get main shareholder.
        /// </summary>
        /// <param name="betTypes">Bet type id.</param>
        /// <returns>List agent.</returns>
        IEnumerable<Agent> GetMainShareHolder(string betTypes);

        /// <summary>
        /// Get wagers data.
        /// </summary>
        /// <typeparam name="T">Type of criteria.</typeparam>
        /// <param name="criteria">Criteria class.</param>
        /// <returns>List wagers.</returns>
        IEnumerable<T> GetWagerData<T>(WagerCriteriaBase criteria);

        /// <summary>
        /// Get event date for early market.
        /// </summary>
        /// <param name="sportTypes">Sport type ids.</param>
        /// <returns>List date time.</returns>
        IEnumerable<DateTime> GetEventDate(string sportTypes);
    }
}
