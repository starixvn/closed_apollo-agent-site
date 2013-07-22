// --------------------------------------------------------------------------
// <copyright file="InternalCheckinCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.5.23">Created</change>
// </history>

namespace Apollo.Age.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InternalCheckinCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string NetSessionId { get; set; }

        public int OnUserId { get; set; }

        public string Ip { get; set; }               

        public override string GetSettingKey()
        {
            return "Age_Admin_CheckIn";
        }

        public override bool IsValid()
        {
            return UserId >= 0;
        }
    }
}