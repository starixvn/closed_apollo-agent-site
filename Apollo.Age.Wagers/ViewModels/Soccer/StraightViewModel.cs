// --------------------------------------------------------------------------
// <copyright file="StraightViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.29">Created</change>
//   <change who="Terry.Nguyen" date="2013.7.18">Add Stake Before</change>
// </history>

namespace Apollo.Age.Wagers.ViewModels.Soccer
{
    /// <summary>
    /// StraightViewModel class.
    /// </summary>
    public class StraightViewModel : WagerViewModel
    {
        public decimal HomeStakeBeforeHDP { get; set; }

        public decimal AwayStakeBeforeHDP { get; set; }

        public decimal HomeStakeBeforeOU { get; set; }

        public decimal AwayStakeBeforeOU { get; set; }

        public decimal StakeBefore1 { get; set; }

        public decimal StakeBeforeX { get; set; }

        public decimal StakeBefore2 { get; set; }

        public decimal HomeStakeHDP { get; set; }

        public decimal AwayStakeHDP { get; set; }

        public decimal HomeStakeOU { get; set; }

        public decimal AwayStakeOU { get; set; }

        public decimal Stake1 { get; set; }

        public decimal StakeX { get; set; }

        public decimal Stake2 { get; set; }

        public int HomeCountHDP { get; set; }

        public int AwayCountHDP { get; set; }

        public int HomeCountOU { get; set; }

        public int AwayCountOU { get; set; }

        public int Count1 { get; set; }

        public int CountX { get; set; }

        public int Count2 { get; set; }

        public string HomeBetteamHDP { get; set; }

        public string AwayBetteamHDP { get; set; }

        public string HomeBetteamOU { get; set; }

        public string AwayBetteamOU { get; set; }

        public string Betteam1 { get; set; }

        public string BetteamX { get; set; }

        public string Betteam2 { get; set; }
    }
}