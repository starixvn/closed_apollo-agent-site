// --------------------------------------------------------------------------
// <copyright file="UpdateAgentBetLimitCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.5">Created</change>
// </history>

namespace Apollo.CustSetting.Criteria.Agent
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Criteria information.
    /// </summary>
    public class UpdateAgentBetLimitCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int? ActorAgentId { get; set; }

        public int AdminId { get; set; }

        public string BetLimit { get; set; }

        /// <summary>
        /// Get Setting key.
        /// </summary>
        /// <returns>Current setting key.</returns>
        public override string GetSettingKey()
        {
            return "AgentSetting_Age_Agent_UpdateBetLimit";
        }
    }
}