// --------------------------------------------------------------------------
// <copyright file="SportCreditCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.7.31">Created</change>
// </history>

namespace Apollo.Age.Reports.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class SportCreditCriteria : CriteriaBase
    {
        public int LoggedId { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Rpt_LineSportCredits";
        }

        public override bool IsValid()
        {
            return LoggedId > 0 &&  !string.IsNullOrEmpty(Username);
        }
    }
}
