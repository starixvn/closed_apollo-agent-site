// --------------------------------------------------------------------------
// <copyright file="OddEvenViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.1">Created</change>
// </history>


namespace Apollo.Age.TotalBets.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OddEvenViewModel
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
        public decimal home_odds_OE { get; set; }
        public decimal away_odds_OE { get; set; }
        public decimal home_odds_OE_1st { get; set; }
        public decimal away_odds_OE_1st { get; set; }
        public decimal odds_1 { get; set; }
        public decimal odds_x { get; set; }
        public decimal odds_2 { get; set; }
        public decimal odds_1_1st { get; set; }
        public decimal odds_x_1st { get; set; }
        public decimal odds_2_1st { get; set; }
        public decimal home_odds_DND { get; set; }
        public decimal away_odds_DND { get; set; }
        public int BetCount_OE { get; set; }
        public int BetCount_OE_1st { get; set; }
        public int BetCount_1x2 { get; set; }
        public int BetCount_1x2_1st { get; set; }
        public int BetCount_DND { get; set; }
        public bool IsNeutral { get; set; }

        public string HomeTeam
        {
            get
            {
                return CacheRef.GetTeamName(this.HomeId) + ((IsNeutral) ? "(N)" : string.Empty);
            }
        }

        public string AwayTeam
        {
            get
            {
                return  CacheRef.GetTeamName(this.AwayId);
            }
        }

        public string LeagueName
        {
            get
            {
                return CacheRef.GetLeagueName(this.LeagueId);
            }
        }

        public decimal OddEvenTotal { 
            get {
                    return this.home_odds_OE - this.away_odds_OE;
            } 
        }

        public decimal OneXTwoTotal
        {
            get
            {
                return this.odds_1 + this.odds_x + this.odds_2;
            }
        }

        public decimal DrawNoDrawTotal
        {
            get
            {
                return this.home_odds_DND - this.away_odds_DND;
            }
        }

        public decimal OddEvenFirstHalf
        {
            get
            {
                return this.home_odds_OE_1st - this.away_odds_OE_1st;
            }
        }

        public decimal OneXTwoFirstHalf
        {
            get
            {
                return this.odds_1_1st + this.odds_x_1st + this.odds_2_1st;
            }
        }
    }
}
