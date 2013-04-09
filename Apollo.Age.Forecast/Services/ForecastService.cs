// --------------------------------------------------------------------------
// <copyright file="ForecastService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.1">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.5">Update OU, 1X2, ScoreMap</change>
//   <change who="Ricky.Nguyen" date="2013.4.9">Update ScoreMap</change>
// </history>

namespace Apollo.Age.Forecast.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Apollo.Age.Forecast.Criterias;
    using Apollo.Age.Forecast.Models;
    using Apollo.Age.Forecast.Repositories;
    using Apollo.Age.Forecast.ViewModels;
    using Apollo.Age.Services.Impl;
    using Fanex.Common.Helper;
    using Apollo.Age.Forecast.Core;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ForecastService : QueryService
    {
        private IForecastRepository _forecastRepository { get; set; }

        public ForecastService()
            : this(null)
        {
        }

        public ForecastService(IForecastRepository forecastRepository)
        {
            _forecastRepository = _forecastRepository ?? new ForecastRepository();
        }

        /// <summary>
        /// Running Leagues
        /// </summary>
        /// <param name="betType"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public IEnumerable<LeagueViewModel> GetRunningLeague(int betType, int agentId)
        {
            Check.Argument.IsNotNegative(betType, "betType");
            Check.Argument.IsNotNegative(agentId, "agentId");

            return GetData<LeagueViewModel>(
                    new GetRunningLeagueCriteria
                    {
                        AgentId = agentId,
                        BetTypes = betType.ToString()
                    }
                );
        }

        /// <summary>
        /// Forecast 1X2
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public IEnumerable<Forecast1X2ViewModel> GetForecast1x2(int agentId, int leagueId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            Check.Argument.IsNotNegative(leagueId, "leagueId");

            return GetData<Forecast1X2ViewModel>(
                    new GetForecast1X2GetByLeagueCriteria
                    {
                        AgentId = agentId,
                        LeagueID = leagueId
                    }
                );
        }

        /// <summary>
        /// FC Handicap OU
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public IEnumerable<ForecastHandicapOUViewModel> GetForecastHandicapOverUnder(int agentId, int leagueId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            Check.Argument.IsNotNegative(leagueId, "leagueId");

            return GetData<ForecastHandicapOUViewModel>(
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

        /// <summary>
        /// Forecast Matches
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="currentScore"></param>
        /// <returns></returns>
        public ForecastMatchWinLostViewModel GetForecastMatcheValue(int agentId, int matchId, int betType, int currentScore)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            Check.Argument.IsNotNegative(matchId, "matchId");
            Check.Argument.IsNotNegative(betType, "betType");

            IEnumerable<ForecastMatchViewModel> forecastMatches = GetData<ForecastMatchViewModel>(
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
                HandleException(ex);
                return null;
            }
        }

        /// <summary>
        /// 1x2 Matches
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public IEnumerable<Forecast1X2ViewModel> GetForecast1X2Matches(int agentId, int matchId, int betType)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            Check.Argument.IsNotNegative(matchId, "matchId");
            Check.Argument.IsNotNegative(betType, "betType");

            return GetData<Forecast1X2ViewModel>(
                    new GetForecastByMatchCriteria
                    {
                        AgentId = agentId,
                        BetType = betType,
                        MatchId = matchId
                    }
                );
        }

        /// <summary>
        /// Score Map Results
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="matchIds"></param>
        /// <param name="liveBetTypes"></param>
        /// <param name="deadBallBetTypes"></param>
        /// <returns></returns>
        public List<ScoreMapResultViewModel> GetScoreMapResults(int agentId, string matchIds, string liveBetTypes, string deadBallBetTypes)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            Check.Argument.IsNotNull(matchIds, "matchIds");
            Check.Argument.IsNotNull(liveBetTypes, "liveBetTypes");
            List<ScoreMapResultViewModel> scoreMapViewResult = new List<ScoreMapResultViewModel>();
            try
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
                int homeIndex = 0;
                int awayIndex = 0;
                int length = 0;
                foreach (ScoreMapMatch match in scoreMapResult.ScoreMapMatches)
                {
                    homeIndex = 0;
                    awayIndex = 0;
                    ScoreMapResultViewModel scoreMapItem = new ScoreMapResultViewModel
                    {
                        MatchId = match.MatchId,
                        HomeName = match.HomeTeam,
                        AwayName = match.AwayTeam
                    };

                    length = scoreMapItem.ScoreMatrix.GetLength(1);
                    foreach (ScoreMapItem item in scoreMapResult.ScoreMapItems)
                    {
                        if (match.MatchId == item.MatchId)
                        {
                            scoreMapItem.HasData = true;
                            scoreMapItem.ScoreMatrix[awayIndex, homeIndex++] = item.Total;
                            //// If HomeIndex = 7 Col, then increate Away Row
                            if (homeIndex == length)
                            {
                                homeIndex = 0;
                                awayIndex++;
                                //// If away row == 7 then break out
                                if (awayIndex == length)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    scoreMapViewResult.Add(scoreMapItem);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return scoreMapViewResult;
        }

        /// <summary>
        /// Score Map BetType
        /// </summary>
        /// <returns></returns>
        public ScoreMapBetTypeViewModel GetScoreMapBetType()
        {
            IEnumerable<ScoreMapBetType> scoreMapBetTypes = CachingService.Get<IEnumerable<ScoreMapBetType>>(Constants.SCORE_MAP_BETTYPES);
            if (scoreMapBetTypes == null)
            {
                scoreMapBetTypes = GetData<ScoreMapBetType>(new GetScoreMapBetTypesCriteria());
                CachingService.Set<IEnumerable<ScoreMapBetType>>(Constants.SCORE_MAP_BETTYPES, scoreMapBetTypes, new TimeSpan(0,5,0));
            }
            
            ScoreMapBetTypeViewModel scoreMapBetTypeViewModel = new ScoreMapBetTypeViewModel();
            foreach (ScoreMapBetType betType in scoreMapBetTypes)
            {
                if (betType.IsLive == 1)
                {
                    scoreMapBetTypeViewModel.LiveBetTypes.Add(betType);
                }

                if (betType.IsDead == 1)
                {
                    scoreMapBetTypeViewModel.DeadBallBetTypes.Add(betType);
                }
            }
            return scoreMapBetTypeViewModel;
        }

        /// <summary>
        /// Score Map Matches
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public IEnumerable<ScoreMapMatchViewModel> GetScoreMapMatches(int agentId, int leagueId)
        {
            IEnumerable<ScoreMapMatchViewModel> scoreMapMatches = GetData<ScoreMapMatchViewModel>(
                    new GetScoreMatchesByLeague
                    {
                        AgentId = agentId,
                        LeagueId = leagueId
                    }
                );
            return scoreMapMatches.OrderBy(match => match.MatchName);
        }

        /// <summary>
        /// Score Map League
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public IEnumerable<LeagueViewModel> GetScoreMapLeagues(int agentId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            try
            {
                return GetData<LeagueViewModel>(new GetScoreMapLeaguesCriteria
                {
                    AgentId = agentId
                }).OrderBy(league => league.LeagueName);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        /// <summary>
        /// Get Top Odds
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public MatchOddsViewModel GetTopOddsByMatch(int matchId, int betType)
        {
            Check.Argument.IsNotNegative(matchId, "matchId");
            Check.Argument.IsNotNegative(betType, "betType");

            IEnumerable<MatchOddsViewModel> matchOddsViewModels = GetData<MatchOddsViewModel>(
                    new GetTopOddsByMatchCriteria
                    {
                        BetType = betType,
                        MatchId = matchId
                    }
                );

            return matchOddsViewModels.ElementAtOrDefault(0);
        }

        #region Private Methods

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

        #endregion Private Methods
    }
}