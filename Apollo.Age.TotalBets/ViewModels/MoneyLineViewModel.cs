// --------------------------------------------------------------------------
// <copyright file="MoneyLineViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.31">Created</change>
// </history>

namespace Apollo.Age.TotalBets.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Apollo.Age.Services;

    public class MoneyLineViewModel
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


        public decimal Home_Odds { get; set; }

        public decimal Away_Odds { get; set; }

        public decimal WinLost_Home { get; set; }

        public decimal WinLost_Away { get; set; }

        public int BetCount_Home { get; set; }

        public int BetCount_Away { get; set; }


        public decimal Home_Odss_HT { get; set; }

        public decimal Away_Odds_HT { get; set; }

        public decimal WinLost_Home_HT { get; set; }

        public decimal WinLost_Away_HT { get; set; }

        public int BetCount_Home_HT { get; set; }

        public int BetCount_Away_HT { get; set; }


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
