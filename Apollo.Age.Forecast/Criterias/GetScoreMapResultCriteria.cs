// --------------------------------------------------------------------------
// <copyright file="GetScoreMapResultCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get score map result criteria.
    /// </summary>
    public class GetScoreMapResultCriteria : CriteriaBase
    {
        public string MatchIds { get; set; }

        public int LoggedId { get; set; }

        public int AgentId { get; set; }

        [SpParam(SqlParamName = "bettypes_live")]
        public string LiveBetTypes { get; set; }

        [SpParam(SqlParamName = "bettypes_dead")]
        public string DeadBetTypes { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Age_ScoreMap_Result";
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>True if is valid.</returns>
        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(MatchIds);
        }
    }
}