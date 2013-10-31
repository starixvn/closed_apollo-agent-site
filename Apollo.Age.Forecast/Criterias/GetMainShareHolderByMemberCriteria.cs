// --------------------------------------------------------------------------
// <copyright file="GetMainShareHolderByMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.5.15">Created</change>
// </history>

namespace Apollo.Age.Forecast.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get main shareholder by member criteria.
    /// </summary>
    public class GetMainShareHolderByMemberCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Agent_GetMainShareHolderByMember";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
