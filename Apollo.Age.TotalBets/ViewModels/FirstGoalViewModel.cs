// --------------------------------------------------------------------------
// <copyright file="FirstGoalViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.1">Created</change>
// </history>

namespace Apollo.Age.TotalBets.ViewModels
{
    using Apollo.Age.Services;

    public class FirstGoalViewModel
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

        public decimal WinLost_HomeFG { get; set; }

        public decimal WinLost_HomeLG { get; set; }

        public decimal WinLost_AwayFG { get; set; }

        public decimal WinLost_AwayLG { get; set; }

        public decimal WinLost_NoGoal { get; set; }

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
