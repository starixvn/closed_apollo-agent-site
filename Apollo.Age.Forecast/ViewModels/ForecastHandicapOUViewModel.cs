// --------------------------------------------------------------------------
// <copyright file="ForecastHandicapOUViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.3">Created</change>
// </history>


namespace Apollo.Age.Forecast.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ForecastHandicapOUViewModel
    {
        public int MatchId { get; set; }
        public int HomeId { get; set; }
        public int AwayId { get; set; }
        public bool LiveIndicator { get; set; }
        public int LiveHomeScore { get; set; }
        public int LiveAwayScore { get; set; }
        public string ShowTime { get; set; }
        public string MatchCode { get; set; }
        public decimal? HandicapWinLostMinus1 { get; set; }
        public decimal? HandicapWinLostBase { get; set; }
        public decimal? HandicapWinLostPlus1 { get; set; }
        public decimal? OverUnderWinLostBase { get; set; }
        public decimal? OverUnderWinLost1 { get; set; }
        public decimal? OverUnderWinLost2 { get; set; }
        public decimal? HaftTimeHandicapWinLostMinus1 { get; set; }
        public decimal? HaftTimeHandicapWinLostBase { get; set; }
        public decimal? HaftTimeHandicapWinLostPlus1 { get; set; }
        public decimal? HaftTimeOverUnderWinLostBase { get; set; }
        public decimal? HaftTimeOverUnderWinLost1 { get; set; }
        public decimal? HaftTimeOverUnderWinLost2 { get; set; }
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

    }
}
