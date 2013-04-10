// --------------------------------------------------------------------------
// <copyright file="GetExchangeRateByCurrencyCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.4.10">Created</change>
// </history>

namespace Apollo.Age.Admin.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class GetExchangeRateByCurrencyCriteria : CriteriaBase
    {
        public string Currency { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Age_Exchange_GetByCurrency";
        }
    }
}