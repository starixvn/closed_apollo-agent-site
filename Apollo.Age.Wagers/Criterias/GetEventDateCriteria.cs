// --------------------------------------------------------------------------
// <copyright file="GetEventDateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.7.4">StyleCop</change>
// </history>

namespace Apollo.Age.Wagers.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Event Date Criteria class.
    /// </summary>
    public class GetEventDateCriteria : CriteriaBase
    {
        public string Sporttypes { get; set; }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Return a store procedure name.</returns>
        public override string GetSettingKey()
        {
            return "Age_Wagers_SelDateBySports";
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
