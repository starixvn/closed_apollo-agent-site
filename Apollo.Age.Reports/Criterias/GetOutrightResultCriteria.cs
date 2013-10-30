// --------------------------------------------------------------------------
// <copyright file="GetOutrightResultCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.5.20">Created</change>
// </history>

namespace Apollo.Age.Reports.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class GetOutrightResultCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }
       
        [SpParam(Ignored = true)]
        public DateTime BalanceUpDay { get; set; }
       
        public bool IsShowAll { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Result_Outright";
        }

        public override bool IsValid()
        {
            return (FromDate <= DateTime.Now && FromDate >= BalanceUpDay);
        }
    }
}