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

        private List<MatchEnteredViewModel> ConvertToMatchEnteredModel(List<MatchEnteredModel> enteredDTOList)
        {
            List<MatchEnteredViewModel> matchEnteredList = null;

            if (enteredDTOList != null)
            {
                if (enteredDTOList.Any())
                {
                    var sportType = -1;
                    MatchEnteredViewModel matchEntered = null;
                    matchEnteredList = new List<MatchEnteredViewModel>();

                    foreach (var enteredDTO in enteredDTOList)
                    {
                        if (enteredDTO.SportType == sportType)
                        {
                            matchEntered.TodayEntered = enteredDTO.Entered;
                            matchEntered.TodayNonEntered = enteredDTO.NonEntered;
                        }
                        else
                        {
                            if(sportType != -1)
                            {
                                matchEnteredList.Add(matchEntered);
                                matchEntered = null;
                            }

                            if(enteredDTO.IsToday == 1)
                            {
                                matchEntered = new MatchEnteredViewModel()
                                {
                                    SportTypeId = enteredDTO.SportType,
                                    TodayEntered = enteredDTO.Entered,
                                    TodayNonEntered = enteredDTO.NonEntered
                                };
                            }
                            else
                            {
                                matchEntered = new MatchEnteredViewModel()
                                {
                                    SportTypeId = enteredDTO.SportType,
                                    YesterdayEntered = enteredDTO.Entered,
                                    YesterdayNonEntered = enteredDTO.NonEntered
                                };
                            }

                            sportType = enteredDTO.SportType;
                        }
                    }

                    matchEnteredList.Add(matchEntered);
                }
            }

            return matchEnteredList;
        }

        public List<AgentWinLossModel> GetAgentWinLoss(int agentId, DateTime fromDate, DateTime toDate)
        {
            var criteria = new AgentWinLossCriteria() { 
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
    }
}
