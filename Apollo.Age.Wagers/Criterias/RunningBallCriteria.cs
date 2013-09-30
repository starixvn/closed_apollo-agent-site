// --------------------------------------------------------------------------
// <copyright file="RunningBallCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.7.3">Created</change>
// </history>
namespace Apollo.Age.Wagers.Criterias
{
    /// <summary>
    /// RunningBallCriteria class.
    /// </summary>
    public class RunningBallCriteria : WagerCriteriaBase
    {
        public string Sporttypes { get; set; }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Age_Wagers_RunningBall key.</returns>
        public override string GetSettingKey()
        {
            return "Age_Wagers_RunningBall";
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