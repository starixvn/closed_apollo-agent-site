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
    using System;
    using Apollo.Age.Services;

    /// <summary>
    /// Total Goal Model.
    /// </summary>
    public class TotalGoalModel
    {
        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        /// <summary>
        /// Gets or sets BetTeam in 0-1.      
        /// </summary>
        /// <value>Decimal value.</value>
        public decimal Total01 { get; set; }

        /// <summary>
        /// Gets or sets BetTeam in 2-3.
        /// </summary>
        /// <value>Decimal value.</value>
        public decimal Total23 { get; set; }

        /// <summary>
        /// Gets or sets BetTeam in 4-6. 
        /// </summary>
        /// <value>Decimal value.</value>
        public decimal Total46 { get; set; }

        /// <summary>
        /// Gets or sets BetTeam in 7 - Over.
        /// </summary>
        /// <value>Decimal value.</value>
        public decimal Total7Over { get; set; }

        public bool IsNeutral { get; set; }

        public bool Closed { get; set; }

        public string ShowTime { get; set; }

        public DateTime? KickOffTime { get; set; }

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
    }
}