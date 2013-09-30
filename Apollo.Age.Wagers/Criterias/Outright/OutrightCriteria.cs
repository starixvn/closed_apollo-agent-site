// --------------------------------------------------------------------------
// <copyright file="OutrightCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.7.2">Created</change>
// </history>

namespace Apollo.Age.Wagers.Criterias.Outright
{
    using System;

    /// <summary>
    /// OutrightCriteria class.
    /// </summary>
    public class OutrightCriteria : WagerCriteriaBase
    {
        public DateTime ToDate { get; set; }

        public string Sporttypes { get; set; }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Age_Wagers_Outright key.</returns>
        public override string GetSettingKey()
        {
            return "Age_Wagers_Outright";
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