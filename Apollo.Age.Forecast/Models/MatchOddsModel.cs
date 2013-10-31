// --------------------------------------------------------------------------
// <copyright file="MatchOddsModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.4">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.5">StyleCop</change>
// </history>

namespace Apollo.Age.Forecast.Models
{
    using System.Diagnostics.CodeAnalysis;
    using Apollo.Age.Services;

    public class MatchOddsModel
    {
        public double Hdp1 { get; set; }

        public double Hdp2 { get; set; }

        public double Odds1a { get; set; }

        public double Odds2a { get; set; }

        public int LiveHomeScore { get; set; }

        public int LiveAwayScore { get; set; }

        public bool Switch { get; set; }

        public string OddsStatus { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

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