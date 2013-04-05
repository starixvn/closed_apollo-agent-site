// --------------------------------------------------------------------------
// <copyright file="Forecast1X2GetByLeague.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.2">Created</change>
// </history>


namespace Apollo.Age.Forecast.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetForecast1X2GetByLeagueCriteria : CriteriaBase
    {
        
        public int AgentId { get; set; }

        public int LeagueID { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Forecast_5_15_GetByLeague";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && LeagueID > 0;
        }
    }
}
