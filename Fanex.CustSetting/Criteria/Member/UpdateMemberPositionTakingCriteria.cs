// --------------------------------------------------------------------------
// <copyright file="UpdateMemberPositionTakingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.5">Created</change>
// </history>

namespace Fanex.CustSetting.Criteria.Member
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Criteria information.
    /// </summary>
    public class UpdateMemberPositionTakingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

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
            return "PlayerSetting_Age_Customer_UpdatePositionTaking";
        }
    }
}