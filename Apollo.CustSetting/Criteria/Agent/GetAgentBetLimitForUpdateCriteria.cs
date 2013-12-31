// --------------------------------------------------------------------------
// <copyright file="GetAgentBetLimitForUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get Agent BetLimit Criteria.
    /// </summary>
    public class GetAgentBetLimitForUpdateCriteria : CriteriaBase
    {
        public int AgentId { get; set; }
        
        public int MaxBetLimit { get; set; }

        /// <summary>
        /// Get Setting key for query database.
        /// </summary>
        /// <returns>Setting key for query database.</returns>
        public override string GetSettingKey()
        {
            return "AgentSetting_Age_Agent_GetBetLimitForUpdate";
        }
    }
}