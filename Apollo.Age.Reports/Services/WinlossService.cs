// --------------------------------------------------------------------------
// <copyright file="WinlossService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.11">Created</change>
// </history>

namespace Apollo.Age.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.ViewModels;

    public class WinLossService : ReportService, IWinLossService
    {
        public List<MatchEnteredViewModel> GetMatchEntered()
        {
            var matchEnteredList = this.GetData<MatchEnteredModel>(new MatchEnteredCriteria()).ToList();

            return ConvertToMatchEnteredModel(matchEnteredList);
        }

        public List<AgentWinLossModel> GetAgentWinLoss(int agentId, DateTime fromDate, DateTime toDate)
        {
            var criteria = new AgentWinLossCriteria() 
            { 
                AgentId = agentId,
                FromDate = fromDate,
                ToDate = toDate,
                BalanceUpDay = this.GetBalanceUpDay()
            };

            var agentWinLossList = this.GetData<AgentWinLossModel>(criteria).ToList();

            return agentWinLossList;
        }

        public List<MemberWinLossModel> GetMemberWinLoss(int agentId, DateTime fromDate, DateTime toDate)
        {
            var criteria = new MemberWinLossCriteria()
            {
                AgentId = agentId,
                FromDate = fromDate,
                ToDate = toDate,
                BalanceUpDay = this.GetBalanceUpDay()
            };

            var memberWinLossList = this.GetData<MemberWinLossModel>(criteria).ToList();

            return memberWinLossList;
        }

        private List<MatchEnteredViewModel> ConvertToMatchEnteredModel(List<MatchEnteredModel> enteredFromList)
        {
            List<MatchEnteredViewModel> enteredToList = null;

            if (enteredFromList != null && enteredFromList.Any())
            {
                var sportType = -1;
                MatchEnteredViewModel matchEntered = null;
                enteredToList = new List<MatchEnteredViewModel>();

                foreach (var item in enteredFromList)
                {
                    if (item.SportType == sportType)
                    {
                        matchEntered.TodayEntered = item.Entered;
                        matchEntered.TodayNonEntered = item.NonEntered;
                    }
                    else
                    {
                        if (sportType != -1)
                        {
                            enteredToList.Add(matchEntered);
                            matchEntered = null;
                        }

                        if (item.IsToday == 1)
                        {
                            matchEntered = new MatchEnteredViewModel()
                            {
                                SportTypeId = item.SportType,
                                TodayEntered = item.Entered,
                                TodayNonEntered = item.NonEntered
                            };
                        }
                        else
                        {
                            matchEntered = new MatchEnteredViewModel()
                            {
                                SportTypeId = item.SportType,
                                YesterdayEntered = item.Entered,
                                YesterdayNonEntered = item.NonEntered
                            };
                        }

                        sportType = item.SportType;
                    }
                }

                enteredToList.Add(matchEntered);
            }

            return enteredToList;
        }
    }
}
