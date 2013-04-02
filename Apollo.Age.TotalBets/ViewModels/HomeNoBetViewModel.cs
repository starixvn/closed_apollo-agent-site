// --------------------------------------------------------------------------
// <copyright file="HomeNoBet.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.1">Created</change>
// </history>

namespace Apollo.Age.TotalBets.ViewModels
{
    using Apollo.Age.Services;

    public class HomeNoBetViewModel
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

        public decimal Home_Odds_DNB { get; set; }

        public decimal Away_Odds_DNB { get; set; }

        public decimal Home_Odds_HNB { get; set; }

        public decimal Away_Odds_HNB { get; set; }

        public decimal Home_Odds_ANB { get; set; }

        public decimal Away_Odds_ANB { get; set; }

        public int BetCount_DNB { get; set; }

        public int BetCount_HNB { get; set; }

        public int BetCount_ANB { get; set; }

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
