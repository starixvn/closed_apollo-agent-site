// --------------------------------------------------------------------------
// <copyright file="SSOGetTimeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.12.26">Created</change>
// </history>
namespace Apollo.Age.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// SSO Get Time criteria.
    /// </summary>
    public class SSOGetTimeCriteria : CriteriaBase
    {
        public DateTime Time { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_SSOGetTime";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}