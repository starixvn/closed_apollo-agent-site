// --------------------------------------------------------------------------
// <copyright file="HTFTViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.1">Created</change>
// </history>


namespace Apollo.Age.TotalBets.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HTFTViewModel
    {
        public int MatchId { get; set; }
        public int HomeId { get; set; }
        public int AwayId { get; set; }
        public string MatchCode { get; set; }
        public decimal totalHH { get; set; }
        public decimal totalHD { get; set; }
        public decimal totalHA { get; set; }
        public decimal totalDH { get; set; }
        public decimal totalDD { get; set; }
        public decimal totalDA { get; set; }
        public decimal totalAH { get; set; }
        public decimal totalAD { get; set; }
        public decimal totalAA { get; set; }
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
