// --------------------------------------------------------------------------
// <copyright file="GetMainShareHolderCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.7.1">Created</change>
// </history>

namespace Apollo.Age.Wagers.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// GetMainShareHolderCriteria class.
    /// </summary>
    public class GetMainShareHolderCriteria : CriteriaBase
    {
        public string Bettypes { get; set; }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Age_Agent_GetAllMainShareHolder key.</returns>
        public override string GetSettingKey()
        {
            return "Age_Agent_GetAllMainShareHolder";
        }

        /// <summary>
        /// Check criteria is valid.
        /// </summary>
        /// <returns>If is valid return true.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}