// --------------------------------------------------------------------------
// <copyright file="SSOCheckInCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class SSOCheckInCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int SiteId { get; set; }

        public int CheckOnUserId { get; set; }

        public string NetSessionId { get; set; }

        public int OnUserId { get; set; }

        public string IP { get; set; }

        public DateTime LoginDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_SSOCheckIn";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}