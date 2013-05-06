// --------------------------------------------------------------------------
// <copyright file="AuthenticateNapUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.5.3">Created</change>
// </history>

namespace Apollo.Age.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class AuthenticateNapUserCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_Authenticate";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }
    }
}