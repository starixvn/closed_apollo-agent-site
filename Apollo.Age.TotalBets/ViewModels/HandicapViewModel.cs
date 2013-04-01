// --------------------------------------------------------------------------
// <copyright file="HandicapViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.30">Created</change>
// </history>


namespace Apollo.Age.TotalBets.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Apollo.Age.Services;

    public class HandicapViewModel
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

        public decimal Home_Odds_HDP { get; set; }
        public decimal Away_Odds_HDP { get; set; }
        public int BetCound_HDP { get; set; }

        public decimal Home_Odds_HTHDP { get; set; }
        public decimal Away_Odds_HTHDP { get; set; }
        public int BetCount_HTHDP { get; set; }

        public decimal Home_Odds_OU { get; set; }
        public decimal Away_Odds_OU { get; set; }
        public int BetCount_OU { get; set; }

        public decimal Home_Odds_HTOU { get; set; }
        public decimal Away_Odds_HTOU { get; set; }
        public int BetCount_HTOU { get; set; }

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
