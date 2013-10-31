// --------------------------------------------------------------------------
// <copyright file="GetScoreMatchesByLeague.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.5">Created</change>
// </history>

namespace Apollo.Age.Forecast.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get score map by league criteria.
    /// </summary>
    public class GetScoreMatchesByLeague : CriteriaBase
    {
        public int LeagueId { get; set; }

        public int AgentId { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Age_ScoreMap_SelMatch";
        }

        public override bool IsValid()
        {
            return LeagueId > 0;
        }
    }
}