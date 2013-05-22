// --------------------------------------------------------------------------
// <copyright file="LoginByUsernameCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.5.22">Created</change>
// </history>

namespace Apollo.Age.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LoginByUsernameCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Agent_LoginByUserName";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Username);
        }
    }
}
