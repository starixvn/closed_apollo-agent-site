// --------------------------------------------------------------------------
// <copyright file="InternalGetAgentByUsernameCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.5.22">Created</change>
// </history>

namespace Apollo.Age.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InternalGetAgentByUsernameCriteria : CriteriaBase
    {
        public string Username { get; set; }

        /// <summary>
        /// Override Get Store Procedure Name.
        /// </summary>
        /// <returns>Return Store Procedure Name.</returns>
        public override string GetSettingKey()
        {
            return "Age_Agent_GetByUserName";
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
