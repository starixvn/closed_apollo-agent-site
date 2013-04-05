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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Forecast.Models;
    using Fanex.NAP.Framework.Data;
    using Fanex.Data;
    using System.Data;
    using Fanex.Common.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ForecastRepository : IForecastRepository
    {
        public ScoreMapResult GetScoreMapResult(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (ObjectDb objdb = new ObjectDb(criteria.GetSettingKey()))
                {
                    ScoreMapResult scoreMapResult = new ScoreMapResult();
                    IDataReader reader = objdb.ExecuteReader(criteria.GetSpParams());
                    while (reader.Read())
                    {
                        scoreMapResult.ScoreMapItems.Add(new ScoreMapItem { 
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
                            scoreMapResult.ScoreMapMatches.Add(new ScoreMapMatch { 
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
    }
}
