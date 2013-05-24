// --------------------------------------------------------------------------
// <copyright file="UserGetByUsernameCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.19">Created</change>
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
    public class UserGetByUsernameCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "Age_AgentUser_GetByUserName";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Username);
        }
    }
}
