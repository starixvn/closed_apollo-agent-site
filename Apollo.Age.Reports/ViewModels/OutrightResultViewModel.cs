// --------------------------------------------------------------------------
// <copyright file="OutrightResultViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.4.29">Created</change>
// </history>

namespace Apollo.Age.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OutrightResultViewModel
    {
        public DateTime BalanceUpDay { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte SportId { get; set; }

        public IEnumerable<Sport> SportList
        {
            get
            {
                if (OutrightResult != null && OutrightResult.Any())
                {
                    return OutrightResult
                        .Select(item => new Sport { Id = item.SportId, Name = item.SportName })
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

                byte selectedSportId = (this.SportId <= 0) ? SportList.First().Id : this.SportId;
                if (OutrightResult != null && OutrightResult.Any())
                {
                    listLeague = OutrightResult.Where(item => item.SportId == selectedSportId)
                        .Select(item => new League { Id = item.LeagueId, Name = item.LeagueName })
                        .Distinct().ToList();

                    if (listLeague != null && listLeague.Count() > 1)
                    {
                        listLeague.Insert(0, new League { Id = 0, Name = Resources.Lang.allleague });
                    }
                }

                return listLeague;
            }
        }

        public IEnumerable<OutrightResultModel> OutrightResult { get; set; }

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