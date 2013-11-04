// --------------------------------------------------------------------------
// <copyright file="MatchEnteredCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.11">Created</change>
//   <change who="Terry.Nguyen" date="2013.9.17">Add more parameter isShowAll</change>
// </history>

namespace Apollo.Age.Reports.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Match entered criteria.
    /// </summary>
    public class MatchEnteredCriteria : CriteriaBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether [is show all].
        /// </summary>
        /// <value>
        ///   <c>true</c> If [is show all]; otherwise, <c>false</c>.
        /// </value>
        public bool IsShowAll { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Rpt_MatchSelResultEntered";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
