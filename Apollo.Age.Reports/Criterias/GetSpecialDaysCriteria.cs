// --------------------------------------------------------------------------
// <copyright file="GetSpecialDaysCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Reports.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get special days criteria.
    /// </summary>
    public class GetSpecialDaysCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Age_SelBalanceUpDay";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
