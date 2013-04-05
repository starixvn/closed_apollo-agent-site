// --------------------------------------------------------------------------
// <copyright file="ForecastService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.1">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.5">Update Function</change>
// </history>

namespace Apollo.Age.Forecast.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Forecast.Criterias;
    using Apollo.Age.Forecast.ViewModels;
    using Apollo.Age.Services.Impl;
    using Fanex.Common.Helper;
    using Apollo.Age.Enumerations;

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

        public IEnumerable<Forecast1X2ViewModel> GetForecast1x2(int agentId, int leagueId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            Check.Argument.IsNotNegative(leagueId, "leagueId");

            return GetReportData<Forecast1X2ViewModel>(
                    new GetForecast1X2GetByLeagueCriteria
                    {
                        AgentId = agentId,
                        LeagueID = leagueId
                    }
                );
        }

        public IEnumerable<ForecastHandicapOUViewModel> GetForecastHandicapOverUnder(int agentId, int leagueId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            Check.Argument.IsNotNegative(leagueId, "leagueId");

            return GetReportData<ForecastHandicapOUViewModel>(
                    new GetForecastHandicapOverUnderCriteria
                    {
                        AgentId = agentId,
                        LeagueID = leagueId,
                        BaseOffsetHDP = 0,
                        BaseOffsetHTHDP = 0,
                        BaseOffsetHTOU = 0,
                        BaseOffsetOU = 0
                    }
                );
        }

        public ForecastMatchWinLostViewModel GetForecastMatcheValue(int agentId, int matchId, int betType, int currentScore)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            Check.Argument.IsNotNegative(matchId, "matchId");
            Check.Argument.IsNotNegative(betType, "betType");

            IEnumerable<ForecastMatchViewModel> forecastMatches = GetReportData<ForecastMatchViewModel>(
                    new GetForecastByMatchCriteria
                    {
                        AgentId = agentId,
                        BetType = betType,
                        MatchId = matchId
                    }
                );

            //// Calculate Value
            try
            {
                return GetForecastValueWithCurrentScore(forecastMatches, betType, currentScore);
            }
            catch (Exception ex)
            {
                //// Log here
                return null;
            }
        }

        public MatchOddsViewModel GetTopOddsByMatch(int matchId, int betType)
        {
            Check.Argument.IsNotNegative(matchId, "matchId");
            Check.Argument.IsNotNegative(betType, "betType");

            IEnumerable<MatchOddsViewModel> matchOddsViewModels = GetReportData<MatchOddsViewModel>(
                    new GetTopOddsByMatchCriteria
                    {
                        BetType = betType,
                        MatchId = matchId
                    }
                );

            return matchOddsViewModels.ElementAtOrDefault(0);
        }

        #region Private Functions

        private ForecastMatchWinLostViewModel GetForecastValueWithCurrentScore(IEnumerable<ForecastMatchViewModel> forecastMatches, int betType, int currentScore)
        {
            if (!forecastMatches.Any())
            {
                return null;
            }

            ForecastMatchWinLostViewModel matchWinLost = new ForecastMatchWinLostViewModel();
            if (betType == (int)BetTypes.Handicap || betType == (int)BetTypes._1st_Handicap)
            {
                foreach (var forecastMatch in forecastMatches)
                {
                    matchWinLost.Value0 = GetForecastHandicapValue(forecastMatch, currentScore, -2, matchWinLost.Value0);
                    matchWinLost.Value1 = GetForecastHandicapValue(forecastMatch, currentScore, -1, matchWinLost.Value1);
                    matchWinLost.Value2 = GetForecastHandicapValue(forecastMatch, currentScore, 0, matchWinLost.Value2);
                    matchWinLost.Value3 = GetForecastHandicapValue(forecastMatch, currentScore, 1, matchWinLost.Value3);
                    matchWinLost.Value4 = GetForecastHandicapValue(forecastMatch, currentScore, 2, matchWinLost.Value4);
                }
            }
            else
            {
                foreach (var forecastMatch in forecastMatches)
                {
                    matchWinLost.Value0 = GetForecastOverUnderValue(forecastMatch, currentScore, 0, matchWinLost.Value0);
                    matchWinLost.Value1 = GetForecastOverUnderValue(forecastMatch, currentScore, 1, matchWinLost.Value1);
                    matchWinLost.Value2 = GetForecastOverUnderValue(forecastMatch, currentScore, 2, matchWinLost.Value2);
                    matchWinLost.Value3 = GetForecastOverUnderValue(forecastMatch, currentScore, 3, matchWinLost.Value3);
                    matchWinLost.Value4 = GetForecastOverUnderValue(forecastMatch, currentScore, 4, matchWinLost.Value4);
                }
            }

            return matchWinLost;
        }

        private decimal GetForecastHandicapValue(ForecastMatchViewModel forecastMatch, int currentScore, int scoreOffset, decimal forecast)
        {
            decimal forecastScore = currentScore - forecastMatch.Offset + scoreOffset;
            if (forecastScore > 0 && forecastScore < (decimal)(0.5))
            {
                forecast += forecastMatch.AwayWinLost / 2; // win 1/2
            }
            else if (forecastScore >= (decimal)(0.5))
            {
                forecast += forecastMatch.AwayWinLost;
            }
            else if (forecastScore < 0 && forecastScore > -(decimal)(0.5))
            {
                forecast += forecastMatch.HomeWinLost / 2;
            }
            else if (forecastScore <= (decimal)(-0.5))
            {
                forecast += forecastMatch.HomeWinLost;
            }

            return forecast;
        }

        private decimal GetForecastOverUnderValue(ForecastMatchViewModel forecastMatch, int currentScore, int scoreOffset, decimal forecast)
        {
            decimal forecastScore = currentScore - forecastMatch.Offset + scoreOffset;
            if (forecastScore == (decimal)(0.25))
            {
                forecast += forecastMatch.AwayWinLost / 2;
            }
            else if (forecastScore > (decimal)(0.25))
            {
                forecast += forecastMatch.AwayWinLost;
            }
            else if (forecastScore == (decimal)(-0.25))
            {
                forecast += forecastMatch.HomeWinLost / 2;
            }
            else if (forecastScore < (decimal)(-0.25))
            {
                forecast += forecastMatch.HomeWinLost;
            }

            return forecast;
        }

        #endregion
    }
}