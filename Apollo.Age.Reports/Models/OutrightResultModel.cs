// --------------------------------------------------------------------------
// <copyright file="OutrightResultModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.4.29">Created</change>
// </history>

namespace Apollo.Age.Reports.Models
{
    using System;
    using Apollo.Age.Services;

    /// <summary>
    /// Outright result model
    /// </summary>
    public class OutrightResultModel
    {
        public byte SportId { get; set; }

        public string SportName
        {
            get
            {
                return CacheRef.GetSportName(this.SportId);
            }
        }

        public int LeagueId { get; set; }

        public string LeagueName
        {
            get
            {
                return CacheRef.GetLeagueName(this.LeagueId);
            }
        }

        public DateTime EventDate { get; set; }

        public int TeamId { get; set; }

        public string TeamName
        {
            get
            {
                return CacheRef.GetTeamName(this.TeamId);
            }
        }

        public string Status { get; set; }
    }
}