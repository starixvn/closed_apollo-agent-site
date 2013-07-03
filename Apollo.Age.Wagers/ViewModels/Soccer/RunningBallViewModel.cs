// --------------------------------------------------------------------------
// <copyright file="WagersRunningBallViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.29">Created</change>
// </history>

namespace Apollo.Age.Wagers.ViewModels.Soccer
{
    public class RunningBallViewModel : WagerViewModel
    {
        public decimal HomeStakeHDP { get; set; }

        public decimal AwayStakeHDP { get; set; }

        public decimal HomeStakeOU { get; set; }

        public decimal AwayStakeOU { get; set; }

        public decimal HomeStakeHTHDP { get; set; }

        public decimal AwayStakeHTHDP { get; set; }

        public decimal Stake1 { get; set; }

        public decimal StakeX { get; set; }

        public decimal Stake2 { get; set; }

        public int HomeCountHDP { get; set; }

        public int AwayCountHDP { get; set; }

        public int HomeCountHTHDP { get; set; }

        public int AwayCountHTHDP { get; set; }

        public int HomeCountOU { get; set; }

        public int AwayCountOU { get; set; }

        public int Count1 { get; set; }

        public int CountX { get; set; }

        public int Count2 { get; set; }

        public string HomeBetteamHDP { get; set; }

        public string AwayBetteamHDP { get; set; }

        public string HomeBetteamOU { get; set; }

        public string AwayBetteamOU { get; set; }

        public string HomeBetteamHTHDP { get; set; }

        public string AwayBetteamHTHDP { get; set; }

        public string HomeBetteamHTOU { get; set; }

        public string AwayBetteamHTOU { get; set; }
    }
}