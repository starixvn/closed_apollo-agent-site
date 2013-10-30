// --------------------------------------------------------------------------
// <copyright file="ResultGetSportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.10">Created</change>
// </history>

namespace Apollo.Age.Reports.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ResultGetSportCriteria : CriteriaBase
    {
        public DateTime EventDate { get; set; }

        [SpParam(Ignored = true)]
        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Result_SportSelByDate";
        }

        public override bool IsValid()
        {
            return EventDate <= DateTime.Now && EventDate >= BalanceUpDay;
        }
    }
}
