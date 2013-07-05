// --------------------------------------------------------------------------
// <copyright file="MoneyLineModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.31">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.3">Update Mapping SP Property</change>
// </history>

namespace Apollo.Age.TotalBets.Models
{
    using System;
    using Apollo.Age.Services;

    /// <summary>
    /// Money Line Model.
    /// </summary>
    public class MoneyLineModel
    {
        public string ShowTime { get; set; }

        public int LeagueId { get; set; }

        public string MatchCode { get; set; }

        public bool LiveIndicator { get; set; }

        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public short LiveHomeScore { get; set; }

        public short LiveAwayScore { get; set; }

        public decimal HomeOdds { get; set; }

        public decimal AwayOdds { get; set; }

        public decimal HomeWinLost { get; set; }

        public decimal AwayWinLost { get; set; }

        public int BetCountHome { get; set; }

        public int BetCountAway { get; set; }

        public decimal HomeOddsHalfTime { get; set; }

        public decimal AwayOddsHalfTime { get; set; }

        public decimal HomeHalfTimeWinLost { get; set; }

        public decimal AwayHalfTimeWinLost { get; set; }

        public int BetCountHomeHalfTime { get; set; }

        public int BetCountAwayHalfTime { get; set; }

        public bool IsNeutral { get; set; }

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