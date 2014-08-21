// --------------------------------------------------------------------------
// <copyright file="FirstGoalModel.cs" company="Nexcel Solutions Vietnam">
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
    /// First Goal Model.
    /// </summary>
    public class FirstGoalModel
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

        public DateTime? KickOffTime { get; set; }

        public decimal WinLostHomeFG { get; set; }

        public decimal WinLostAwayFG { get; set; }

        public decimal WinLostHomeLG { get; set; }

        public decimal WinLostAwayLG { get; set; }

        public decimal WinLostNoGoal { get; set; }

        public decimal HTWinLostHomeFG { get; set; }

        public decimal HTWinLostHomeLG { get; set; }

        public decimal HTWinLostAwayFG { get; set; }

        public decimal HTWinLostAwayLG { get; set; }

        public decimal HTWinLostNoGoal { get; set; }

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