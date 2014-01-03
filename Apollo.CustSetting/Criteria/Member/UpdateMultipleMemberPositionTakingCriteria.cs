// --------------------------------------------------------------------------
// <copyright file="UpdateMultipleMemberPositionTakingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.5">Created</change>
// </history>

namespace Apollo.CustSetting.Criteria.Member
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Criteria information.
    /// </summary>
    public class UpdateMultipleMemberPositionTakingCriteria : CriteriaBase
    {
        public string CustIds { get; set; }

        public int? ActorAgentId { get; set; }

        public int AdminId { get; set; }

        public int AgentId { get; set; }

        public string PositionTaking { get; set; }

        /// <summary>
        /// Get Setting key.
        /// </summary>
        /// <returns>Current setting key.</returns>
        public override string GetSettingKey()
        {
            return "Age_Customer_UpdatePositionTakingMultiple";
        }
    }
}