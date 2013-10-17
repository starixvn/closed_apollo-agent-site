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
    /// Outright result model class.
    /// </summary>
    public class OutrightResultModel
    {
        public byte SportId { get; set; }

        /// <summary>
        /// Gets the name of the sport.
        /// </summary>
        /// <value>
        /// The name of the sport.
        /// </value>
        public string SportName
        {
            get
            {
                return CacheRef.GetSportName(this.SportId);
            }
        }

        public int LeagueId { get; set; }

        /// <summary>
        /// Gets the name of the league.
        /// </summary>
        /// <value>
        /// The name of the league.
        /// </value>
        public string LeagueName
        {
            get
            {
                return CacheRef.GetLeagueName(this.LeagueId);
            }
        }

        public DateTime EventDate { get; set; }

        public int TeamId { get; set; }

        /// <summary>
        /// Gets the name of the team.
        /// </summary>
        /// <value>
        /// The name of the team.
        /// </value>
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