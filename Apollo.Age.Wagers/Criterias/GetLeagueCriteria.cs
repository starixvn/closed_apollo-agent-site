// --------------------------------------------------------------------------
// <copyright file="GetLeagueCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.28">Created</change>
// </history>

namespace Apollo.Age.Wagers.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetLeagueCriteria : CriteriaBase
    {
        public string SportTypes { get; set; }

        public DateTime FromDate { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Wagers_SelLeague";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(SportTypes);
        }
    }
}
