// --------------------------------------------------------------------------
// <copyright file="WagersRunningBallViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.30">Created</change>
// </history>

namespace Apollo.Age.Wagers.ViewModels.Basketball
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WagersRunningBallViewModel
    {
        public string ShowTime { get; set; }

        public bool LiveIndicator { get; set; }

        public int leagueid { get; set; }

        public string matchcode { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public string UGroup { get; set; }

        public decimal HomeStakeHDP { get; set; }

        public decimal AwayStakeHDP { get; set; }

        public decimal HomeStakeOU { get; set; }

        public decimal AwayStakeOU { get; set; }

        public int HomeCountHDP { get; set; }

        public int AwayCountHDP { get; set; }

        public int HomeCountOU { get; set; }

        public int AwayCountOU { get; set; }
    }
}
