// --------------------------------------------------------------------------
// <copyright file="WagersSpecialViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.29">Created</change>
// </history>

using System;

namespace Apollo.Age.Wagers.ViewModels.Soccer
{
    public class WagersSpecialViewModel
    {
        public string Showtime { get; set; }

        public bool LiveIndicator { get; set; }

        public int leagueid { get; set; }

        public string matchcode { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public string UGroup { get; set; }

        public decimal HomeStakeOE { get; set; }

        public decimal AwayStakeOE { get; set; }

        public decimal HomeStakeHTOE { get; set; }

        public decimal AwayStakeHTOE { get; set; }

        public decimal Total01Stake { get; set; }

        public decimal Total23Stake { get; set; }

        public decimal Total46Stake { get; set; }

        public decimal Total7OverStake { get; set; }

        public int HomeCountOE { get; set; }

        public int AwayCountOE { get; set; }

        public int HomeCountHTOE { get; set; }

        public int AwayCountHTOE { get; set; }

        public int Total01Count { get; set; }

        public int Total23Count { get; set; }

        public int Total46Count { get; set; }

        public int Total7OverCount { get; set; }
    }
}