// --------------------------------------------------------------------------
// <copyright file="ForecastHandicapOUModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.3">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.5">StyleCop</change>
// </history>

namespace Apollo.Age.Forecast.Models
{
    using System.Diagnostics.CodeAnalysis;
    using Apollo.Age.Services;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ForecastHandicapOUModel
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

        public decimal? HalfTimeHandicapWinLostMinus1 { get; set; }

        public decimal? HalfTimeHandicapWinLostBase { get; set; }

        public decimal? HalfTimeHandicapWinLostPlus1 { get; set; }

        public decimal? HalfTimeOverUnderWinLostBase { get; set; }

        public decimal? HalfTimeOverUnderWinLost1 { get; set; }

        public decimal? HalfTimeOverUnderWinLost2 { get; set; }

        public bool IsNeutral { get; set; }

        public string HomeTeam
        {
            get
            {
                if (this.IsNeutral)
                {
                    return CacheRef.GetTeamName(this.HomeId) + "(N)";
                }

                return CacheRef.GetTeamName(this.HomeId);
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