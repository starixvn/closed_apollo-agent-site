// --------------------------------------------------------------------------
// <copyright file="IForecastRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.5">Created</change>
// </history>

namespace Apollo.Age.Forecast.Repositories
{
    using Apollo.Age.Forecast.Models;
    using Fanex.NAP.Framework.Data;

    public interface IForecastRepository
    {
        /// <summary>
        /// Gets the score map result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Score Map Result Model.</returns>
        ScoreMapResultModel GetScoreMapResult(ICriteria criteria);

        /// <summary>
        /// Gets the main share holder by member.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The main share holder ID.</returns>
        int GetMainShareHolderByMember(ICriteria criteria);
    }
}