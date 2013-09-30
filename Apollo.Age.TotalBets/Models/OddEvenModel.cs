// --------------------------------------------------------------------------
// <copyright file="OddEvenModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.1">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.3">Update Mapping SP Property</change>
// </history>

namespace Apollo.Age.TotalBets.Models
{
    using System;
    using Apollo.Age.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OddEvenModel
    {
        public string ShowTime { get; set; }

        public int LeagueId { get; set; }

        public string MatchCode { get; set; }

        public bool LiveIndicator { get; set; }

        public short LiveHomeScore { get; set; }

        public short LiveAwayScore { get; set; }

        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public decimal HomeOddsOddEven { get; set; }

        public decimal AwayOddsOddEven { get; set; }

        public decimal HomeOddsHalfTimeOddEven { get; set; }

        public decimal AwayOddsHalfTimeOddEven { get; set; }

        public decimal Odds1 { get; set; }

        public decimal OddsX { get; set; }

        public decimal Odds2 { get; set; }

        public decimal Odds1HalfTime { get; set; }

        public decimal OddsXHalfTime { get; set; }

        public decimal Odds2HalfTime { get; set; }

        public decimal HomeOddsDrawNoDraw { get; set; }

        public decimal AwayOddsDrawNoDraw { get; set; }

        public int BetCountOddEven { get; set; }

        public int BetCountHalfTimeOddEven { get; set; }

        public int BetCount1X2 { get; set; }

        public int BetCount1X2HalfTime { get; set; }

        public int BetCountDrawNoDraw { get; set; }

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

        public decimal OddEvenTotal
        {
            get
            {
                return this.HomeOddsOddEven - this.AwayOddsOddEven;
            }
        }

        public decimal OneXTwoTotal
        {
            get
            {
                return this.Odds1 + this.OddsX + this.Odds2;
            }
        }

        public decimal DrawNoDrawTotal
        {
            get
            {
                return this.HomeOddsDrawNoDraw - this.AwayOddsDrawNoDraw;
            }
        }

        public decimal OddEvenFirstHalf
        {
            get
            {
                return this.HomeOddsHalfTimeOddEven - this.AwayOddsHalfTimeOddEven;
            }
        }

        public decimal OneXTwoFirstHalf
        {
            get
            {
                return this.Odds1HalfTime + this.OddsXHalfTime + this.Odds2HalfTime;
            }
        }
    }
}