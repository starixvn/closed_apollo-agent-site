// --------------------------------------------------------------------------
// <copyright file="GetForecastHandicapOverUnderCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.3">Created</change>
// </history>

namespace Apollo.Age.Forecast.Criterias
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class GetForecastHandicapOverUnderCriteria : CriteriaBase
    {
        public int BaseOffsetHDP { get; set; }

        public int BaseOffsetOU { get; set; }

        public int BaseOffsetHTHDP { get; set; }

        public int BaseOffsetHTOU { get; set; }

        public int LeagueID { get; set; }

        public int AgentId { get; set; }

        public int LoggedId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Forecast_1_3_7_8_GetByLeague";
        }

        public override bool IsValid()
        {
            return LeagueID > 0;
        }
    }
}