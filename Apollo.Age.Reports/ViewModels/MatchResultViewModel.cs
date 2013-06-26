// --------------------------------------------------------------------------
// <copyright file="MatchResultViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.10">Created</change>
// </history>

namespace Apollo.Age.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Apollo.Age.Entities;
    using Apollo.Age.Services;
    using Apollo.Age.Reports.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchResultViewModel
    {
        public DateTime BalanceUpDay { get; set; }

        public DateTime EventDate { get; set; }

        public bool IsOther { get; set; }

        public IEnumerable<MatchResultModel> MatchResult { get; set; }

        public IEnumerable<Sport> SportList
        {
            get
            {
                if (this.MatchResult != null && this.MatchResult.Any())
                {
                    return this.MatchResult
                        .Select(item => new Sport { Id = (byte)item.SportId, Name = item.SportName })
                        .Distinct();
                }

                return new List<Sport>();
            }
        }

        public IEnumerable<League> LeagueList
        {
            get
            {
                var listLeague = new List<League>();

                //byte selectedSportId = ((int)this.SportId <= 0) ? (byte)this.SportList.First().Id : this.SportId;
                //if (this.MatchResult != null && this.MatchResult.Any())
                //{
                //    listLeague = this.MatchResult.Where(item => item.SportId == selectedSportId)
                //        .Select(item => new League { Id = item.LeagueId, Name = item.LeagueName })
                //        .Distinct().ToList();

                //    if (listLeague != null && listLeague.Count() > 1)
                //    {
                //        listLeague.Insert(0, new League { Id = 0, Name = Resources.Lang.allleague });
                //    }
                //}

                return listLeague;
            }
        }
    
        public IEnumerable<SelectListItem> SportSelectListItem
        {
            get
            {
                var selectItemList = SportList.Select(sport => new SelectListItem
                {
                    Value = sport.Id.ToString(),
                    Text = CacheRef.GetSportName(sport.Id)
                }).ToList<SelectListItem>();

                return selectItemList;
            }
        }
    }
}