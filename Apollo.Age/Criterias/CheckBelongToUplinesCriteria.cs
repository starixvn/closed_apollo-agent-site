// --------------------------------------------------------------------------
// <copyright file="CheckBelongToUplines.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.8.5">Created</change>
// </history>

namespace Apollo.Age.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class CheckBelongToUplinesCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public bool Allowed { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Agent_CheckBelongToUplines";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Username); 
        }
    }
}
