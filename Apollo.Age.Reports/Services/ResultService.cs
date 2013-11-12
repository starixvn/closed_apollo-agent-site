// --------------------------------------------------------------------------
// <copyright file="ResultService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.10">Created</change>
// </history>

namespace Apollo.Age.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ResultService : ReportService
    {
        /// <summary>
        /// Get sport which has result in a specified date.
        /// </summary>
        /// <param name="date">Input a day.</param>
        /// <returns>List of sport result.</returns>
        public IEnumerable<Sport> GetResultSports(DateTime date)
        {
            var sports = GetData<Sport>(new ResultGetSportCriteria
             {
                 EventDate = date,
                 BalanceUpDay = GetBalanceUpDay()
             });

            return sports;
        }

        /// <summary>
        /// Gets the match result.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="sportTypes">The sport types.</param>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="isShowDataTest">Include data test or not.</param>
        /// <returns>List of match result.</returns>
        public IEnumerable<MatchResultModel> GetMatchResult(DateTime fromDate, string sportTypes, int custId, bool isShowDataTest)
        {
            var criteria = new GetMatchResultCriteria
                    {
                        FromDate = fromDate,
                        Sporttypes = sportTypes,
                        BalanceUpDay = GetBalanceUpDay(),
                        IsTest = isShowDataTest
                    };

            var cacheKey = string.Join("-", "MatchResult", fromDate.ToString("yyyyMMdd"), sportTypes.Replace(",", "-"), (isShowDataTest ? "1" : "0"));
            var matchResult = CacheManager.Get<IEnumerable<MatchResultModel>>(cacheKey);
            if (matchResult == null)
            {
                matchResult = GetData<MatchResultModel>(criteria);

                if (sportTypes == Apollo.Age.Core.SportGroup.Others && matchResult != null)
                {
                    CacheManager.Set<IEnumerable<MatchResultModel>>(cacheKey, matchResult, TimeSpan.FromMinutes(1));
                }
            }

            return matchResult;
        }

        /// <summary>
        /// Gets the outright result.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="isShowDataTest">Include data test or not.</param>
        /// <returns>List of outright result.</returns>
        public IEnumerable<OutrightResultModel> GetOutrightResult(DateTime fromDate, bool isShowDataTest)
        {
            var criteria = new GetOutrightResultCriteria
            {
                FromDate = fromDate,
                IsShowAll = isShowDataTest,
                BalanceUpDay = GetBalanceUpDay(),
            };
            var cacheKey = string.Join("-", "OutrightResult", fromDate.ToString("yyyyMMdd"), (isShowDataTest ? "1" : "0"));
            var result = CacheManager.Get<IEnumerable<OutrightResultModel>>(cacheKey);

            if (result == null)
            {
                result = GetData<OutrightResultModel>(criteria);
                if (result != null)
                {
                    CacheManager.Set<IEnumerable<OutrightResultModel>>(cacheKey, result, TimeSpan.FromMinutes(1));
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the result by match.
        /// </summary>
        /// <param name="matchId">The match identifier.</param>
        /// <returns>List of match result.</returns>
        public IEnumerable<MatchResultModel> GetResultByMatch(int matchId)
        {
            var criteria = new GetResultByMatchCriteria
            {
                MatchId = matchId
            };

            var matchResult = GetData<MatchResultModel>(criteria);

            return matchResult;
        }

        /// <summary>
        /// Gets the result by parlay.
        /// </summary>
        /// <param name="refNo">The reference number.</param>
        /// <returns>List of parlay result.</returns>
        public IEnumerable<MatchResultModel> GetResultByParlay(long refNo)
        {
            var criteria = new GetResultByParlayCriteria
            {
                RefNo = refNo
            };

            var parlayResult = GetData<MatchResultModel>(criteria);

            return parlayResult;
        }
    }
}