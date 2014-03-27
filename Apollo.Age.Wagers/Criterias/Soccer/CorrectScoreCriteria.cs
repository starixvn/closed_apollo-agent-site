// --------------------------------------------------------------------------
// <copyright file="CorrectScoreCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.29">Created</change>
// </history>

namespace Apollo.Age.Wagers.Criterias.Soccer
{
    using Apollo.Age.Wagers.Criterias;

    /// <summary>
    /// CorrectScoreCriteria class.
    /// </summary>
    public class CorrectScoreCriteria : WagerCriteriaBase
    {
        public int Bettype { get; set; }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Age_Wagers_SC_4_30_16 key.</returns>
        public override string GetSettingKey()
        {
            return "Age_Wagers_SC_4_30_16";
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