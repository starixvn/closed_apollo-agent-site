// --------------------------------------------------------------------------
// <copyright file="ReportResultViewModel.cs" company="Nexcel Solutions Vietnam">
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
    using System.Text;

    using System.Web.Mvc;
    using Apollo.Age.Entities;
    using Apollo.Age.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReportResultViewModel
    {
        public DateTime BalanceUpDay { get; set; }
        public DateTime EventDate { get; set; }
        public IEnumerable<Sport> SportList { get; set; }
        public IEnumerable<League> LeagueList { get; set; }

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
