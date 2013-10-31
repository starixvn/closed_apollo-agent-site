// --------------------------------------------------------------------------
// <copyright file="GetForecast1X2GetByLeagueCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.2">Created</change>
// </history>

namespace Apollo.Age.Forecast.Criterias
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    public class GetForecast1X2GetByLeagueCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int LeagueID { get; set; }

        public int LoggedId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Forecast_5_15_GetByLeague";
        }

        public override bool IsValid()
        {
            return LeagueID > 0;
        }
    }
}