// --------------------------------------------------------------------------
// <copyright file="GetLeagueCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.28">Created</change>
// </history>

namespace Apollo.Age.Wagers.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get league criteria.
    /// </summary>
    public class GetLeagueCriteria : CriteriaBase
    {
        public string SportTypes { get; set; }

        public DateTime FromDate { get; set; }

        public bool IsOutright { get; set; }

        public DateTime? ToDate { get; set; }

        public bool IsShowAll { get; set; }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Store procedure name.</returns>
        public override string GetSettingKey()
        {
            return "Age_Wagers_SelLeague";
        }

        /// <summary>
        /// Check criteria is valid.
        /// </summary>
        /// <returns>If is valid return true.</returns>
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(SportTypes);
        }
    }
}
