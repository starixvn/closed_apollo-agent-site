// --------------------------------------------------------------------------
// <copyright file="UserGetByUsernameCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.19">Created</change>
// </history>

namespace Apollo.Age.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get user by username criteria.
    /// </summary>
    public class UserGetByUsernameCriteria : CriteriaBase
    {
        public string Username { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns> Setting key.</returns>
        public override string GetSettingKey()
        {
            return "Age_AgentUser_GetByUserName";
        }

        /// <summary>
        /// Override check is validate parameter.
        /// </summary>
        /// <returns>Return true or false.</returns>
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Username);
        }
    }
}
