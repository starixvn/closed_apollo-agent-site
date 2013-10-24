// --------------------------------------------------------------------------
// <copyright file="GetParentInfoWithAgentUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all function filter by function group and active.
    /// </summary>
    public class UserGetUserParentInfoCriteria : CriteriaBase
    {
        public int AgentUserId { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>Return Age_Function_GetAll.</returns>
        public override string GetSettingKey()
        {
            return "Age_Agent_GetParentInfoWithAgentUser";
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>Return true when function group id >= 0.</returns>
        public override bool IsValid()
        {
            return AgentUserId > 0;
        }
    }
}
