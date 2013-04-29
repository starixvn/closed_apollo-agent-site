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
    using System.Linq;
    using System.Text;
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
                 EventDate = date
             });

            return sports;
        }

        public IEnumerable<League> GetLeagueBySport(DateTime eventDate, byte sportType = 0, string language = "E", bool isSowTestData = false)
        {
            var leagues = GetData<League>(new ResultGetLeagueBySportCriteria
            {
                EventDate = eventDate,
                SportType = sportType,
                IsShowAll = isSowTestData
            });

            return leagues;
        }

        public IEnumerable<MatchResultModel> GetMatchResult(DateTime date, byte sportType, int custId, int leagueId = 0, string language = "E")
        {
            var criteria = new GetMatchResultCriteria
                    {
                        EventDate = date,
                        SportId = sportType,
                        CustId = custId,
                        LeagueId = leagueId,
                        Language = language
                    };

            var matchResult = GetData<MatchResultModel>(criteria);

            return matchResult;
        }

        public IEnumerable<OutrightResultModel> GetOutrightResult(DateTime fromDate, DateTime toDate)
        {
            var criteria = new GetOutrightResultCriteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                IsShowAll = true
            };

            var result = GetData<OutrightResultModel>(criteria);

            return result;
        }
    }
}