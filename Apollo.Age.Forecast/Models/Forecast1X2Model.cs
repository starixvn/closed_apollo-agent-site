// --------------------------------------------------------------------------
// <copyright file="Forecast1X2Model.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.2">Created</change>
// </history>

namespace Apollo.Age.Forecast.Models
{
    using Apollo.Age.Services;

    /// <summary>
    /// Forecast 1X2 model.
    /// </summary>
    public class Forecast1X2Model
    {
        public int MatchID { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public bool LiveIndicator { get; set; }

        public short LiveHomeScore { get; set; }

        public short LiveAwayScore { get; set; }

        public string ShowTime { get; set; }

        public string MatchCode { get; set; }

        public bool IsNeutral { get; set; }

        public decimal? AwayWinLost { get; set; }

        public decimal? HomeWinLost { get; set; }

        public decimal? DrawWinLost { get; set; }

        public decimal? HalfTimeAwayWinLost { get; set; }

        public decimal? HalfTimeHomeWinLost { get; set; }

        public decimal? HalfTimeDrawWinLost { get; set; }

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