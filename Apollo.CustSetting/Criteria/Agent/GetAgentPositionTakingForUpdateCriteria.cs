// --------------------------------------------------------------------------
// <copyright file="GetAgentPositionTakingForUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.5">Created</change>
// </history>
namespace Apollo.CustSetting.Criteria.Agent
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Agent Position Taking criteria.
    /// </summary>
    public class GetAgentPositionTakingForUpdateCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "AgentSetting_Age_Agent_GetPositionTakingForUpdate";
        }
    }
}