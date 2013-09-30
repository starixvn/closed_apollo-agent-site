// --------------------------------------------------------------------------
// <copyright file="StraightViewModel.cs" company="Nexcel Solutions Vietnam">
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
    /// TODO: Update summary.
    /// </summary>
    public class StraightViewModel : WagerViewModel
    {
        public decimal HomeStakeBeforeHDP { get; set; }

        public decimal AwayStakeBeforeHDP { get; set; }

        public decimal HomeStakeBeforeOE { get; set; }

        public decimal AwayStakeBeforeOE { get; set; }

        public decimal HomeStakeBeforeOU { get; set; }

        public decimal AwayStakeBeforeOU { get; set; }

        public decimal HomeStakeHDP { get; set; }

        public decimal AwayStakeHDP { get; set; }

        public decimal HomeStakeOE { get; set; }

        public decimal AwayStakeOE { get; set; }

        public decimal HomeStakeOU { get; set; }

        public decimal AwayStakeOU { get; set; }

        public int HomeCountHDP { get; set; }

        public int AwayCountHDP { get; set; }

        public int HomeCountOE { get; set; }

        public int AwayCountOE { get; set; }

        public int HomeCountOU { get; set; }

        public int AwayCountOU { get; set; }

        public string HomeBetteamHDP { get; set; }

        public string AwayBetteamHDP { get; set; }

        public string HomeBetteamOE { get; set; }

        public string AwayBetteamOE { get; set; }

        public string HomeBetteamOU { get; set; }

        public string AwayBetteamOU { get; set; }
    }
}
