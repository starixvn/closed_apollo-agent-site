// --------------------------------------------------------------------------
// <copyright file="ForecastRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.5">Created</change>
// </history>

namespace Apollo.Age.Forecast.Repositories
{
    using System.Data;
    using Apollo.Age.Forecast.Models;
    using Fanex.Common.Extensions;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Forecast Repository.
    /// </summary>
    public class ForecastRepository : IForecastRepository
    {
        /// <summary>
        /// Get ScoreMap Result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Score map result model.</returns>
        public ScoreMapResultModel GetScoreMapResult(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (var objdb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var scoreMapResult = new ScoreMapResultModel();
                    IDataReader reader = objdb.ExecuteReader(criteria.GetSpParams());

                    while (reader.Read())
                    {
                        scoreMapResult.ScoreMapItems.Add(new ScoreMapItem
                        {
                            MatchId = reader.ToInt("matchid"),
                            Home = reader.ToInt("home"),
                            Away = reader.ToInt("away"),
                            Total = reader.ToDecimal("total")
                        });
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            scoreMapResult.ScoreMapMatches.Add(new ScoreMapMatch
                            {
                                MatchId = reader.ToInt("matchid"),
                                HomeId = reader.ToInt("homeid"),
                                AwayId = reader.ToInt("awayid")
                            });
                        }
                    }

                    return scoreMapResult;
                }
            }

            return null;
        }

        /// <summary>
        /// Get Main shareholder id of member.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Main shareholder.</returns>
        public int GetMainShareHolderByMember(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return (int)db.ExecuteScalar(criteria.GetSpParams());
            }
        }
    }
}