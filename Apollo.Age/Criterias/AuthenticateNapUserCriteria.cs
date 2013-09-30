// --------------------------------------------------------------------------
// <copyright file="AuthenticateNapUserCriteria.cs" company="Nexcel Solutions Vietnam" >
// Copyright (c) 2013 Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// <summary>
// Define criteria for authentication function
// </summary>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.5.3">Created</change>
//   <change who="Anders.Le" date="2013.7.22">Add comment to pass stylecop checking</change>
// </history>

namespace Apollo.Age.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// This class contains info for nap authentication.
    /// </summary>
    public class AuthenticateNapUserCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// Gets the setting key of stored procedure setting.
        /// </summary>
        /// <returns> Setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_User_Authenticate";
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }
    }
}