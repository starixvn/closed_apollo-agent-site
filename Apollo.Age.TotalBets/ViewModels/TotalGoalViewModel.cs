// --------------------------------------------------------------------------
// <copyright file="TotalGoalModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.3.29">Implement</change>
//   <change who="Ricky.Nguyen" date="2013.4.3">Update Mapping SP Property</change>
// </history>

namespace Apollo.Age.TotalBets.Models
{
    using Apollo.Age.Services;

    public class TotalGoalModel
    {
        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        /// <summary>
        /// BetTeam in 0-1
        /// </summary>
        public decimal Total01 { get; set; }

        /// <summary>
        /// BetTeam in 2-3
        /// </summary>
        public decimal Total23 { get; set; }

        /// <summary>
        /// BetTeam in 4-6
        /// </summary>
        public decimal Total46 { get; set; }

        /// <summary>
        /// BetTeam in 7 - Over
        /// </summary>
        public decimal Total7Over { get; set; }

        public bool IsNeutral { get; set; }

        public bool Closed { get; set; }

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