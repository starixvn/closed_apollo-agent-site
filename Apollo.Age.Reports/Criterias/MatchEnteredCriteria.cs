// --------------------------------------------------------------------------
// <copyright file="MatchEnteredCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.11">Created</change>
// </history>

namespace Apollo.Age.Reports.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class MatchEnteredCriteria : CriteriaBase
    {
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
