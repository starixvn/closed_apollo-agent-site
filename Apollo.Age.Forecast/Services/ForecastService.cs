// --------------------------------------------------------------------------
// <copyright file="ForecastService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.1">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.5">Update OU, 1X2, ScoreMap</change>
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
    using Apollo.Age.Forecast.Repositories;
    using Apollo.Age.Forecast.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ForecastService : QueryService
    {
        private IForecastRepository _forecastRepository { get; set; }

        public ForecastService()
        {

        }

        public ForecastService(IForecastRepository forecastRepository)
        {
            _forecastRepository = _forecastRepository ?? new ForecastRepository();
        }

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

        public IEnumerable<Forecast1X2ViewModel> GetForecast1X2Matches(int agentId, int matchId, int betType)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            Check.Argument.IsNotNegative(matchId, "matchId");
            Check.Argument.IsNotNegative(betType, "betType");

            return GetReportData<Forecast1X2ViewModel>(
                    new GetForecastByMatchCriteria
                    {
                        AgentId = agentId,
                        BetType = betType,
                        MatchId = matchId
                    }
                );
        }

        public List<ScoreMapResultViewModel> GetScoreMapResults(int agentId, string matchIds, string liveBetTypes, string deadBallBetTypes)
        {
            //// Get Plain Data From Database
            ScoreMapResult scoreMapResult = _forecastRepository.GetScoreMapResult(new GetScoreMapResultCriteria
            {
                AgentId = agentId,
                MatchIds = matchIds,
                LiveBetTypes = liveBetTypes,
                DeadBetTypes = deadBallBetTypes
            });

            //// Map Data
            List<ScoreMapResultViewModel> scoreMapViewResult = new List<ScoreMapResultViewModel>();
            int homeIndex = 0;
            int awayIndex = 0;
            foreach (ScoreMapMatch match in scoreMapResult.ScoreMapMatches)
            {
                ScoreMapResultViewModel scoreMapItem = new ScoreMapResultViewModel
                {
                    MatchId = match.MatchId,
                    HomeName = match.HomeTeam,
                    AwayName = match.AwayTeam
                };
                
                
                foreach (ScoreMapItem item in scoreMapResult.ScoreMapItems)
                {
                    if (match.MatchId == item.MatchId)
                    {
                        scoreMapItem.ScoreMatrix[awayIndex, homeIndex++] = item.Total; 
                        //// If HomeIndex = 7 Col, then increate Away Row
                        if (homeIndex == scoreMapItem.ScoreMatrix.Length)
                        {
                            awayIndex++;
                            //// If away row == 7 then break out
                            if (awayIndex == scoreMapItem.ScoreMatrix.Length)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return scoreMapViewResult;
        }

        public ScoreMapBetTypeViewModel GetScoreMapBetType()
        {
            IEnumerable<ScoreMapBetType> scoreMapBetTypes = GetReportData<ScoreMapBetType>(new GetScoreMapBetTypesCriteria());
            ScoreMapBetTypeViewModel scoreMapBetTypeViewModel = new ScoreMapBetTypeViewModel();
            foreach (ScoreMapBetType betType in scoreMapBetTypes)
            {
                if (betType.IsLive == true)
                {
                    scoreMapBetTypeViewModel.LiveBetTypes.Add(betType);
                }
                else
                {
                    scoreMapBetTypeViewModel.DeadBallBetTypes.Add(betType);
                }
            }
            return scoreMapBetTypeViewModel;
        }

        public IEnumerable<ScoreMapMatchViewModel> GetScoreMapMatches(int agentId, int leagueId)
        {
            IEnumerable<ScoreMapMatchViewModel> scoreMapMatches = GetReportData<ScoreMapMatchViewModel>(
                    new GetScoreMatchesByLeague
                    {
                        AgentId = agentId,
                        LeagueId = leagueId
                    }
                );
            return scoreMapMatches.OrderBy(match => match.MatchName);
        }

        private ForecastMatchWinLostViewModel GetForecastValueWithCurrentScore(IEnumerable<ForecastMatchViewModel> forecastMatches, int betType, int currentScore)
        {
            if (!forecastMatches.Any())
            {
                return null;
            }

            ForecastMatchWinLostViewModel matchWinLost = new ForecastMatchWinLostViewModel();
            if (betType == 1 || betType == 7)
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
    }
}
