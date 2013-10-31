// --------------------------------------------------------------------------
// <copyright file="GetRunningLeagueCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.1">Created</change>
// </history>

namespace Apollo.Age.Forecast.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get running league criteria.
    /// </summary>
    public class GetRunningLeagueCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string BetTypes { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Age_League_GetRunning";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(BetTypes);
        }
    }
}