// --------------------------------------------------------------------------
// <copyright file="FirstHaftViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.30">Created</change>
//   <change who="Terry.Nguyen" date="2013.7.18">Add Stake Before</change>
// </history>

namespace Apollo.Age.Wagers.ViewModels.Basketball
{
    /// <summary>
    /// First half view model.
    /// </summary>
    public class FirstHaftViewModel : WagerViewModel
    {
        public decimal HomeStakeHTHDP { get; set; }

        public decimal AwayStakeHTHDP { get; set; }

        public decimal HomeStakeHTOU { get; set; }

        public decimal AwayStakeHTOU { get; set; }

        public decimal HomeStakeHTOE { get; set; }

        public decimal AwayStakeHTOE { get; set; }

        public decimal HomeStakeBeforeHTHDP { get; set; }

        public decimal AwayStakeBeforeHTHDP { get; set; }

        public decimal HomeStakeBeforeHTOU { get; set; }

        public decimal AwayStakeBeforeHTOU { get; set; }

        public decimal HomeStakeBeforeHTOE { get; set; }

        public decimal AwayStakeBeforeHTOE { get; set; }

        public int HomeCountHTHDP { get; set; }

        public int AwayCountHTHDP { get; set; }

        public int HomeCountHTOU { get; set; }

        public int AwayCountHTOU { get; set; }

        public int HomeCountHTOE { get; set; }

        public int AwayCountHTOE { get; set; }

        public string HomeBetteamHTHDP { get; set; }

        public string AwayBetteamHTHDP { get; set; }

        public string HomeBetteamHTOU { get; set; }

        public string AwayBetteamHTOU { get; set; }

        public string HomeBetteamHTOE { get; set; }

        public string AwayBetteamHTOE { get; set; }
    }
}
