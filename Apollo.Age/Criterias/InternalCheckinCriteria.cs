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
    /// Internal check in criteria.
    /// </summary>
    public class InternalCheckinCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string NetSessionId { get; set; }

        public int OnUserId { get; set; }

        public string Ip { get; set; }

        /// <summary>
        /// Override Get Store Procedure Name.
        /// </summary>
        /// <returns>Return Store Procedure Name.</returns>
        public override string GetSettingKey()
        {
            return "Age_Admin_CheckIn";
        }

        /// <summary>
        /// Override check is validate parameter.
        /// </summary>
        /// <returns>Return true or false.</returns>
        public override bool IsValid()
        {
            return UserId >= 0;
        }
    }
}