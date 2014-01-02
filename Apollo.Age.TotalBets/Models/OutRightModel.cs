// --------------------------------------------------------------------------
// <copyright file="OutRightModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.1">Created</change>
// </history>

namespace Apollo.Age.TotalBets.Models
{
    using System;
    using Apollo.Age.Services;

    /// <summary>
    /// Outright model.
    /// </summary>
    public class OutRightModel
    {
        public int LeagueId { get; set; }

        public string MatchCode { get; set; }

        public int MatchId { get; set; }

        public int TeamId { get; set; }

        public decimal Balance { get; set; }

        public int BetCount { get; set; }

        public decimal TotalWinLost { get; set; }

        public bool IsNeutral { get; set; }

        public DateTime? KickOffTime { get; set; }

        public string TeamName
        {
            get
            {
                return CacheRef.GetTeamName(this.TeamId);
            }
        }

        public string LeagueName
        {
            get
            {
                return CacheRef.GetLeagueName(this.LeagueId);
            }
        }
    }
}