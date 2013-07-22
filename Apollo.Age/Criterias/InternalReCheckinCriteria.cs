// --------------------------------------------------------------------------
// <copyright file="InternalReCheckinCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.5.23">Created</change>
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
    public class InternalReCheckinCriteria : CriteriaBase
    {
        public int OnUserId { get; set; }

        public byte IsKickedOut { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns> Setting key.</returns>
        public override string GetSettingKey()
        {
            return "Age_Admin_ReCheckIn";
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid()
        {
            return OnUserId >= 0;
        }
    }
}
