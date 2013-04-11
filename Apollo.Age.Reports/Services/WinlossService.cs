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
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.DTO;
    using Apollo.Age.Reports.Models;

    public class WinlossService : ReportService, IWinlossService
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

                            if(enteredDTO.IsToday)
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
                        }
                    }

                    matchEnteredList.Add(matchEntered);
                }
            }

            return matchEnteredList;
        }
    }
}
