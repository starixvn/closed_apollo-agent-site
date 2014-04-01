// --------------------------------------------------------------------------
// <copyright file="GetMemberDiscountForUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.5">Created</change>
// </history>

namespace Apollo.CustSetting.Criteria.Member
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Criteria information.
    /// </summary>
    public class GetMemberDiscountForUpdateCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public int CustId { get; set; }

        public string UGroup { get; set; }

        /// <summary>
        /// Get Setting key.
        /// </summary>
        /// <returns>Current setting key.</returns>
        public override string GetSettingKey()
        {
            return "PlayerSetting_Age_Customer_GetDiscountForUpdate";
        }
    }
}