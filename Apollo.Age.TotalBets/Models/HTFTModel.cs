// --------------------------------------------------------------------------
// <copyright file="HTFTModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.1">Created</change>
// </history>

namespace Apollo.Age.TotalBets.Models
{
    using System;
    using Apollo.Age.Services;

    /// <summary>
    /// Half time / full time model.
    /// </summary>
    public class HTFTModel
    {
        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public string MatchCode { get; set; }

        public decimal TotalHH { get; set; }

        public decimal TotalHD { get; set; }

        public decimal TotalHA { get; set; }

        public decimal TotalDH { get; set; }

        public decimal TotalDD { get; set; }

        public decimal TotalDA { get; set; }

        public decimal TotalAH { get; set; }

        public decimal TotalAD { get; set; }

        public decimal TotalAA { get; set; }

        public string ShowTime { get; set; }

        public bool IsNeutral { get; set; }

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