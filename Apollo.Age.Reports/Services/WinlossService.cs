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
    using Apollo.Age.Reports.DTO;
    using Apollo.Age.Reports.Models;

    public class WinLossService : ReportService, IWinLossService
    {
        public List<MatchEnteredModel> GetMatchEntered()
        {
            var matchEnteredList = this.GetData<MatchEnteredDTO>(new MatchEnteredCriteria()).ToList();

            return ConvertToMatchEnteredModel(matchEnteredList);
        }

        private List<MatchEnteredModel> ConvertToMatchEnteredModel(List<MatchEnteredDTO> enteredDTOList)
        {
            List<MatchEnteredModel> matchEnteredList = null;

            if (enteredDTOList != null)
            {
                if (enteredDTOList.Any())
                {
                    var sportType = -1;
                    MatchEnteredModel matchEntered = null;
                    matchEnteredList = new List<MatchEnteredModel>();

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
                                matchEntered = new MatchEnteredModel(){
                                    SportTypeId = enteredDTO.SportType,
                                    TodayEntered = enteredDTO.Entered,
                                    TodayNonEntered = enteredDTO.NonEntered
                                };
                            }
                            else
                            {
                                matchEntered = new MatchEnteredModel(){
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
            var criteria = new AgentWinlossCriteria() { 
                AgentId = agentId,
                FromDate = fromDate,
                ToDate = toDate,
                BalanceUpDay = fromDate
            };

            var agentWinLossList = this.GetData<AgentWinLossModel>(criteria).ToList();

            return agentWinLossList;
        }

        public List<MemberWinLossModel> GetMemberWinLoss(int agentId, DateTime fromDate, DateTime toDate)
        {
            var criteria = new MemberWinlossCriteria()
            {
                AgentId = agentId,
                FromDate = fromDate,
                ToDate = toDate,
                BalanceUpDay = fromDate
            };

            var memberWinLossList = this.GetData<MemberWinLossModel>(criteria).ToList();

            return memberWinLossList;
        }
    }
}
