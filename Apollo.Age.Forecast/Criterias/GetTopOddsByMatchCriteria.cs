// --------------------------------------------------------------------------
// <copyright file="GetTopOddsByMatchCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.4">Created</change>
// </history>

namespace Apollo.Age.Forecast.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get top odds by match criteria.
    /// </summary>
    public class GetTopOddsByMatchCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public int BetType { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Age_Forecast_OddsGetTopByMatch";
        }

        public override bool IsValid()
        {
            return MatchId > 0 && BetType > 0;
        }
    }
}