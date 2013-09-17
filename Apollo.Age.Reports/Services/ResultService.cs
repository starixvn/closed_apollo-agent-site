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
        /// Get sport which has result in a specified date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public IEnumerable<Sport> GetResultSports(DateTime date)
        {
            var sports = GetData<Sport>(new ResultGetSportCriteria
             {
                 EventDate = date,
                 BalanceUpDay = GetBalanceUpDay()
             });

            return sports;
        }

        public IEnumerable<League> GetLeagueBySport(DateTime eventDate, byte sportType = 0, string language = "E", bool isShowDataTest = false)
        {
            var leagues = GetData<League>(new ResultGetLeagueBySportCriteria
            {
                EventDate = eventDate,
                SportType = sportType,
                IsShowAll = isShowDataTest,
                BalanceUpDay = GetBalanceUpDay()
            });

            return leagues;
        }

        public IEnumerable<MatchResultModel> GetMatchResult(DateTime fromDate, string sportTypes, int custId, bool isShowDataTest)
        {
            var criteria = new GetMatchResultCriteria
                    {
                        FromDate = fromDate,
                        Sporttypes = sportTypes,
                        BalanceUpDay = GetBalanceUpDay(),
                        IsTest = isShowDataTest
                    };

            var cacheKey = string.Join("-", fromDate.ToString("yyyyMMdd"), sportTypes.Replace(",", "_"));
            var matchResult = CacheManager.Get<IEnumerable<MatchResultModel>>(cacheKey);
            if (matchResult == null)
            {
                matchResult = GetData<MatchResultModel>(criteria);

                if (sportTypes == Apollo.Age.Core.SportGroup.Others && matchResult != null)
                {
                    CacheManager.Set<IEnumerable<MatchResultModel>>(cacheKey, matchResult, TimeSpan.FromMinutes(5));
                }
            }

            return matchResult;
        }

        public IEnumerable<OutrightResultModel> GetOutrightResult(DateTime fromDate, bool isShowDataTest)
        {
            var criteria = new GetOutrightResultCriteria
            {
                FromDate = fromDate,
                IsShowAll = isShowDataTest,
                BalanceUpDay = GetBalanceUpDay(),                
            };
            var cacheKey = fromDate.ToString("yyyyMMdd");
            var result = CacheManager.Get<IEnumerable<OutrightResultModel>>(cacheKey);

            if (result == null)
            {
                result = GetData<OutrightResultModel>(criteria);
                if (result != null)
                {
                    CacheManager.Set<IEnumerable<OutrightResultModel>>(cacheKey, result, TimeSpan.FromMinutes(5));
                }
            }

            return result;
        }

        public IEnumerable<MatchResultModel> GetResultByMatch(int matchId)
        {
            var criteria = new GetResultByMatchCriteria
            {
                MatchId = matchId
            };

            var matchResult = GetData<MatchResultModel>(criteria);

            return matchResult;
        }

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