// --------------------------------------------------------------------------
// <copyright file="CheckBelongToUplinesCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.8.5">Created</change>
// </history>

namespace Apollo.Age.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// This class check belong to up line.
    /// </summary>
    public class CheckBelongToUplinesCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public bool Allowed { get; set; }

        /// <summary>
        /// Override Get Store Procedure Name.
        /// </summary>
        /// <returns>Return Store Procedure Name.</returns>
        public override string GetSettingKey()
        {
            return "Age_Agent_CheckBelongToUplines";
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