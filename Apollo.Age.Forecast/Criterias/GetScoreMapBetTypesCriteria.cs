// --------------------------------------------------------------------------
// <copyright file="GetScoreMapBetTypesCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get score map bet type criteria.
    /// </summary>
    public class GetScoreMapBetTypesCriteria : CriteriaBase
    {
        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>True if is valid.</returns>
        public override bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Age_ScoreMap_SelBettype";
        }
    }
}