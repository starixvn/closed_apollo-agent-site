// --------------------------------------------------------------------------
// <copyright file="MoneyLineViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.31">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.3">Update Mapping SP Property</change>
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


        public decimal HomeOdds { get; set; }

        public decimal AwayOdds { get; set; }

        public decimal HomeWinLost { get; set; }

        public decimal AwayWinLost { get; set; }

        public int BetCountHome { get; set; }

        public int BetCountAway { get; set; }


        public decimal HomeOddsHaftTime { get; set; }

        public decimal AwayOddsHaftTime { get; set; }

        public decimal HomeHaftTimeWinLost { get; set; }

        public decimal AwayHaftTimeWinLost { get; set; }

        public int BetCountHomeHaftTime { get; set; }

        public int BetCountAwayHaftTime { get; set; }


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
