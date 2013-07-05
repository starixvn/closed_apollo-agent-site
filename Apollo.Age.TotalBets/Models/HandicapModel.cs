// --------------------------------------------------------------------------
// <copyright file="HandicapModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.30">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.3">Update Mapping SP Property</change>
// </history>

namespace Apollo.Age.TotalBets.Models
{
    using System;
    using Apollo.Age.Services;

    /// <summary>
    /// Handicap Model.
    /// </summary>
    public class HandicapModel
    {
        public int LeagueId { get; set; }

        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public bool LiveIndicator { get; set; }

        public short LiveHomeScore { get; set; }

        public short LiveAwayScore { get; set; }

        public bool IsNeutral { get; set; }

        public string ShowTime { get; set; }

        public string MatchCode { get; set; }

        public int BetCount_Hdp { get; set; }

        public decimal HomeOddsHandicap { get; set; }

        public decimal AwayOddsHandicap { get; set; }

        public int BetCountHandicap { get; set; }

        public decimal HomeOddsHalfTimeHandicap { get; set; }

        public decimal AwayOddsHalfTimeHandicap { get; set; }

        public int BetCountHalfTimeHandicap { get; set; }

        public decimal HomeOddsOverUnder { get; set; }

        public decimal AwayOddsOverUnder { get; set; }

        public int BetCountOverUnder { get; set; }

        public decimal HomeOddHalfTimeOverUnder { get; set; }

        public decimal AwayOddHalfTimeOverUnder { get; set; }

        public int BetCountHalfTimeOverUnder { get; set; }

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