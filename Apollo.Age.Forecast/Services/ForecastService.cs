// --------------------------------------------------------------------------
// <copyright file="ForecastService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.1">Created</change>
// </history>

namespace Apollo.Age.Forecast.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Forecast.ViewModels;
    using Apollo.Age.Forecast.Criterias;
    using Fanex.Common.Helper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ForecastService : QueryService
    {
        public IEnumerable<LeagueViewModel> GetRunningLeague(int betType, int agentId)
        {
            Check.Argument.IsNotNegative(betType, "betType");
            Check.Argument.IsNotNegative(agentId, "agentId");

            return GetReportData<LeagueViewModel>(
                    new GetRunningLeagueCriteria
                    {
                        AgentId = agentId,
                        BetTypes = betType.ToString()
                    }
                );
        }
        
    }
}
