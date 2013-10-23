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
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Models;

    /// <summary>
    /// Outright result view model class.
    /// </summary>
    public class OutrightResultViewModel
    {
        public DateTime BalanceUpDay { get; set; }

        public DateTime FromDate { get; set; }

        public int SportId { get; set; }

        public IEnumerable<OutrightResultModel> OutrightResult { get; set; }

        public List<Sport> SportList
        {
            get
            {
                var sports = new List<Sport>();
                if (this.OutrightResult != null && this.OutrightResult.Any())
                {
                    sports = this.OutrightResult
                        .Select(item => new Sport { Id = item.SportId, Name = item.SportName })
                        .Distinct().ToList();
                }
                else
                {
                    sports.Add(new Sport
                    {
                        Id = -1,
                        Name = string.Empty
                    });
                }

                return sports;
            }
        }
    }
}