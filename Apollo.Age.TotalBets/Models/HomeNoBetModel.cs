// --------------------------------------------------------------------------
// <copyright file="HomeNoBetModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.1">Created</change>
// </history>

namespace Apollo.Age.TotalBets.Models
{
    using System;
    using Apollo.Age.Services;

    /// <summary>
    /// Home no bet model.
    /// </summary>
    public class HomeNoBetModel
    {
        public string ShowTime { get; set; }

        public int LeagueId { get; set; }

        public string MatchCode { get; set; }

        public bool LiveIndicator { get; set; }

        public short LiveHomeScore { get; set; }

        public short LiveAwayScore { get; set; }

        public bool IsNeutral { get; set; }

        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public decimal HomeOddsDNB { get; set; }

        public decimal AwayOddsDNB { get; set; }

        public decimal HomeOddsHNB { get; set; }

        public decimal AwayOddsHNB { get; set; }

        public decimal HomeOddsANB { get; set; }

        public decimal AwayOddsANB { get; set; }

        public int BetCountDNB { get; set; }

        public int BetCountHNB { get; set; }

        public int BetCountANB { get; set; }

        public DateTime? KickOffTime { get; set; }

        public string HomeTeam
        {
            get
            {
                return CacheRef.GetTeamName(this.HomeId) + (this.IsNeutral ? "(N)" : string.Empty);
            }
        }

        public string AwayTeam
        {
            get
            {
                return CacheRef.GetTeamName(this.AwayId);
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