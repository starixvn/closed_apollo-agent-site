// --------------------------------------------------------------------------
// <copyright file="ForecastService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.1">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.5">Update OU, 1X2, ScoreMap</change>
//   <change who="Ricky.Nguyen" date="2013.4.9">Update ScoreMap</change>
//   <change who="Anders.Le" date="2013.6.12">Add Fanex.Caching</change>
// </history>

namespace Apollo.Age.Forecast.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Forecast.Core;
    using Apollo.Age.Forecast.Criterias;
    using Apollo.Age.Forecast.Models;
    using Apollo.Age.Forecast.Repositories;
    using Apollo.Age.Services.Impl;
    using Fanex.Caching;
    using Fanex.Common.Helper;

    /// <summary>
    /// Forecast Service
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ForecastService : QueryService
    {
        private readonly ICache _cacheManager;
        private readonly IForecastRepository _forecastRepository;

        public ForecastService()
            : this(null, null)
        {
        }

        public ForecastService(IForecastRepository forecastRepository = null, ICache cacheManager = null)
        {
            _forecastRepository = _forecastRepository ?? new ForecastRepository();
            _cacheManager = cacheManager ?? CacheService.CreateInstance();
        }

        //private IForecastRepository _forecastRepository { get; set; }

        /// <summary>
        /// Running Leagues
        /// </summary>
        /// <param name="betType"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public IEnumerable<LeagueModel> GetRunningLeague(int betType, int agentId)
        {
            Check.Argument.IsNotNegative(betType, "betType");
            Check.Argument.IsNotNegative(agentId, "agentId");

            return GetData<LeagueModel>(
                    new GetRunningLeagueCriteria
                    {
                        AgentId = agentId,
                        BetTypes = betType.ToString()
                    });
        }

        /// <summary>
        /// Forecast 1X2
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public IEnumerable<Forecast1X2Model> GetForecast1x2(int agentId, int loggedId, int leagueId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            Check.Argument.IsNotNegative(leagueId, "leagueId");

            return GetData<Forecast1X2Model>(
                    new GetForecast1X2GetByLeagueCriteria
                    {
                        AgentId = agentId,
                        LeagueID = leagueId,
                        LoggedId = loggedId
                    });
        }

        /// <summary>
        /// FC Handicap OU
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public IEnumerable<ForecastHandicapOUModel> GetForecastHandicapOverUnder(int agentId, int loggedId, int leagueId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            Check.Argument.IsNotNegative(leagueId, "leagueId");

            return GetData<ForecastHandicapOUModel>(
                    new GetForecastHandicapOverUnderCriteria
                    {
                        AgentId = agentId,
                        LeagueID = leagueId,
                        LoggedId = loggedId,
                        BaseOffsetHDP = 0,
                        BaseOffsetHTHDP = 0,
                        BaseOffsetHTOU = 0,
                        BaseOffsetOU = 0
                    });
        }

        /// <summary>
        /// Forecast Matches
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="currentScore"></param>
        /// <returns></returns>
        public ForecastMatchWinLostModel GetForecastMatcheValue(int agentId, int loggedId, int matchId, int betType, int currentScore)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            Check.Argument.IsNotNegative(matchId, "matchId");
            Check.Argument.IsNotNegative(betType, "betType");

            IEnumerable<ForecastMatchModel> forecastMatches = GetData<ForecastMatchModel>(
                    new GetForecastByMatchCriteria
                    {
                        AgentId = agentId,
                        BetType = betType,
                        MatchId = matchId,
                        LoggedId = loggedId
                    });

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
        public IEnumerable<Forecast1X2Model> GetForecast1X2Matches(int agentId, int loggedId, int matchId, int betType)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            Check.Argument.IsNotNegative(matchId, "matchId");
            Check.Argument.IsNotNegative(betType, "betType");

            return GetData<Forecast1X2Model>(
                    new GetForecastByMatchCriteria
                    {
                        AgentId = agentId,
                        BetType = betType,
                        MatchId = matchId,
                        LoggedId = loggedId
                    });
        }

        /// <summary>
        /// Score Map Results
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="matchIds"></param>
        /// <param name="liveBetTypes"></param>
        /// <param name="deadBallBetTypes"></param>
        /// <returns></returns>
        public List<ScoreMapMatchResultModel> GetScoreMapResults(int agentId, int loggedId, string matchIds, string liveBetTypes, string deadBallBetTypes)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            Check.Argument.IsNotNull(matchIds, "matchIds");
            Check.Argument.IsNotNull(liveBetTypes, "liveBetTypes");
            List<ScoreMapMatchResultModel> scoreMapViewResult = new List<ScoreMapMatchResultModel>();
            try
            {
                //// Get Plain Data From Database
                ScoreMapResultModel scoreMapResult = _forecastRepository.GetScoreMapResult(new GetScoreMapResultCriteria
                {
                    AgentId = agentId,
                    MatchIds = matchIds,
                    LiveBetTypes = liveBetTypes,
                    DeadBetTypes = deadBallBetTypes,
                    LoggedId = loggedId
                });

                //// Map Data
                int homeIndex = 0, awayIndex = 0, length = 0;

                foreach (ScoreMapMatch match in scoreMapResult.ScoreMapMatches)
                {
                    homeIndex = 0;
                    awayIndex = 0;
                    ScoreMapMatchResultModel scoreMapItem = new ScoreMapMatchResultModel
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
        /// Bet Type List Score Map.
        /// </summary>
        /// <returns></returns>
        public List<int> GetListScoreMapBetType()
        {
            var oldBetTypeIds = _cacheManager.Get<List<int>>(Constants.ScoreMapBetTypeList);
            if (oldBetTypeIds == null)
            {
                IEnumerable<ScoreMapBetTypeModel> scoreMapBetTypeList = GetData<ScoreMapBetTypeModel>(new GetScoreMapBetTypesCriteria());
                var betTypeIds = new List<int>();
                foreach (var betType in scoreMapBetTypeList)
                {
                    betTypeIds.Add(betType.TypeId);
                    if (betType.Id1st != 0)
                    {
                        betTypeIds.Add(betType.Id1st);
                    }
                }

                _cacheManager.Set<List<int>>(Constants.ScoreMapBetTypeList, betTypeIds, TimeSpan.FromHours(2));
                return betTypeIds;
            }

            return oldBetTypeIds as List<int>;
        }

        /// <summary>
        /// Score Map Bet Type, Will Render 2 Part Live and Dead ball.
        /// <returns></returns>
        public ScoreMapBetTypesModel GetScoreMapBetType()
        {
            IEnumerable<ScoreMapBetTypeModel> scoreMapBetTypeList = this.LoadScoreMapBetTypes();

            ScoreMapBetTypesModel scoreMapBetTypes = new ScoreMapBetTypesModel();
            foreach (ScoreMapBetTypeModel betType in scoreMapBetTypeList)
            {
                if (betType.IsLive == 1)
                {
                    scoreMapBetTypes.LiveBetTypes.Add(betType);
                }

                if (betType.IsDead == 1)
                {
                    scoreMapBetTypes.DeadBallBetTypes.Add(betType);
                }
            }

            return scoreMapBetTypes;
        }

        /// <summary>
        /// From a bet type 1st or full time will return a couple of 1st and full time.
        /// </summary>
        /// <param name="betTypeId"></param>
        /// <param name="isLive"></param>
        /// <returns></returns>
        public string GetAllBetTypeFromBetType(int betTypeId, bool isLive)
        {
            if (betTypeId == 0)
            {
                return string.Empty;
            }

            string betTypeAll = string.Empty;
            IEnumerable<ScoreMapBetTypeModel> scoreMapBetTypeList = this.LoadScoreMapBetTypes();
            foreach (ScoreMapBetTypeModel betType in scoreMapBetTypeList)
            {
                if (isLive
                    && betType.IsLive == 1
                    && (betType.TypeId == betTypeId || betType.Id1st == betTypeId))
                {
                    betTypeAll += string.Format("{0},{1}", betType.TypeId, betType.Id1st != 0 ? betType.Id1st.ToString() : string.Empty);
                    break;
                }
                else if (!isLive
                    && betType.IsDead == 1
                    && (betType.TypeId == betTypeId || betType.Id1st == betTypeId))
                {
                    betTypeAll += string.Format("{0},{1}", betType.TypeId, betType.Id1st != 0 ? betType.Id1st.ToString() : string.Empty);
                    break;
                }
            }

            return betTypeAll;
        }

        /// <summary>
        /// Score Map Matches.
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public IEnumerable<ScoreMapMatch> GetScoreMapMatches(int agentId, int leagueId)
        {
            IEnumerable<ScoreMapMatch> scoreMapMatches = GetData<ScoreMapMatch>(
                    new GetScoreMatchesByLeague
                    {
                        AgentId = agentId,
                        LeagueId = leagueId
                    });
            return scoreMapMatches.OrderBy(match => match.MatchName);
        }

        /// <summary>
        /// Score Map LeagueModel.
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public IEnumerable<LeagueModel> GetScoreMapLeagues(int agentId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            try
            {
                return GetData<LeagueModel>(new GetScoreMapLeaguesCriteria
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
        public MatchOddsModel GetTopOddsByMatch(int matchId, int betType)
        {
            Check.Argument.IsNotNegative(matchId, "matchId");
            Check.Argument.IsNotNegative(betType, "betType");

            IEnumerable<MatchOddsModel> matchOddsViewModels = GetData<MatchOddsModel>(
                    new GetTopOddsByMatchCriteria
                    {
                        BetType = betType,
                        MatchId = matchId
                    });

            return matchOddsViewModels != null ? matchOddsViewModels.ElementAtOrDefault(0) : null;
        }

        /// <summary>
        /// Get Main shareholder in super view
        /// </summary>
        /// <param name="criteria">Get data parameters</param>
        /// <returns>main shareholder id</returns>
        public int GetMainShareHolderByMember(GetMainShareHolderByMemberCriteria criteria)
        {
            return _forecastRepository.GetMainShareHolderByMember(criteria);
        }

        #region Private Methods

        /// <summary>
        /// Get Odd value of ScoreMap
        /// </summary>
        /// <param name="forecastMatches"></param>
        /// <param name="betType"></param>
        /// <param name="currentScore"></param>
        /// <returns></returns>
        private ForecastMatchWinLostModel GetForecastValueWithCurrentScore(IEnumerable<ForecastMatchModel> forecastMatches, int betType, int currentScore)
        {
            if (!forecastMatches.Any())
            {
                return null;
            }

            ForecastMatchWinLostModel matchWinLost = new ForecastMatchWinLostModel();
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

        /// <summary>
        /// Get Forecast Handicap Value
        /// </summary>
        /// <param name="forecastMatch"></param>
        /// <param name="currentScore"></param>
        /// <param name="scoreOffset"></param>
        /// <param name="forecast"></param>
        /// <returns></returns>
        private decimal GetForecastHandicapValue(ForecastMatchModel forecastMatch, int currentScore, int scoreOffset, decimal forecast)
        {
            decimal forecastScore = currentScore - forecastMatch.Offset + scoreOffset;
            if (forecastScore > 0 && forecastScore < (decimal)0.5)
            {
                forecast += forecastMatch.AwayWinLost / 2; // win 1/2
            }
            else if (forecastScore >= (decimal)0.5)
            {
                forecast += forecastMatch.AwayWinLost;
            }
            else if (forecastScore < 0 && forecastScore > -(decimal)0.5)
            {
                forecast += forecastMatch.HomeWinLost / 2;
            }
            else if (forecastScore <= (decimal)-0.5)
            {
                forecast += forecastMatch.HomeWinLost;
            }

            return forecast;
        }

        /// <summary>
        /// Get Forecast Over Under Value
        /// </summary>
        /// <param name="forecastMatch"></param>
        /// <param name="currentScore"></param>
        /// <param name="scoreOffset"></param>
        /// <param name="forecast"></param>
        /// <returns></returns>
        private decimal GetForecastOverUnderValue(ForecastMatchModel forecastMatch, int currentScore, int scoreOffset, decimal forecast)
        {
            decimal forecastScore = currentScore - forecastMatch.Offset + scoreOffset;
            if (forecastScore == (decimal)0.25)
            {
                forecast += forecastMatch.AwayWinLost / 2;
            }
            else if (forecastScore > (decimal)0.25)
            {
                forecast += forecastMatch.AwayWinLost;
            }
            else if (forecastScore == (decimal)-0.25)
            {
                forecast += forecastMatch.HomeWinLost / 2;
            }
            else if (forecastScore < (decimal)-0.25)
            {
                forecast += forecastMatch.HomeWinLost;
            }

            return forecast;
        }

        /// <summary>
        /// Load Score Map Bet Type from DB or cache
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ScoreMapBetTypeModel> LoadScoreMapBetTypes()
        {
            IEnumerable<ScoreMapBetTypeModel> scoreMapBetTypeList = _cacheManager.Get<IEnumerable<ScoreMapBetTypeModel>>(Constants.ScoreMapBetType);
            if (scoreMapBetTypeList == null)
            {
                scoreMapBetTypeList = GetData<ScoreMapBetTypeModel>(new GetScoreMapBetTypesCriteria());
                _cacheManager.Set<IEnumerable<ScoreMapBetTypeModel>>(Constants.ScoreMapBetType, scoreMapBetTypeList, new TimeSpan(0, 5, 0));
                _cacheManager.Set<IEnumerable<ScoreMapBetTypeModel>>(Constants.ScoreMapBetType, scoreMapBetTypeList, new TimeSpan(0, 5, 0));
            }

            return scoreMapBetTypeList;
        }

        #endregion Private Methods
    }
}