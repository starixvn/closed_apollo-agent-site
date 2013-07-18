// --------------------------------------------------------------------------
// <copyright file="FirstHalfViewModel.cs" company="Nexcel Solutions Vietnam">
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
    /// Frist Haft View Model.
    /// </summary>
    public class FirstHalfViewModel : WagerViewModel
    {
        public decimal HomeStakeHTHDP { get; set; }

        public decimal AwayStakeHTHDP { get; set; }

        public decimal HomeStakeHTOU { get; set; }

        public decimal AwayStakeHTOU { get; set; }
        
        public decimal StakeHT1 { get; set; }

        public decimal StakeHTX { get; set; }

        public decimal StakeHT2 { get; set; }

        public decimal HomeStakeBeforeHTHDP { get; set; }

        public decimal AwayStakeBeforeHTHDP { get; set; }

        public decimal HomeStakeBeforeHTOU { get; set; }

        public decimal AwayStakeBeforeHTOU { get; set; }

        public decimal StakeBeforeHT1 { get; set; }

        public decimal StakeBeforeHTX { get; set; }

        public decimal StakeBeforeHT2 { get; set; }

        public int HomeCountHTHDP { get; set; }

        public int AwayCountHTHDP { get; set; }

        public int HomeCountHTOU { get; set; }

        public int AwayCountHTOU { get; set; }

        public int CountHT1 { get; set; }

        public int CountHTX { get; set; }

        public int CountHT2 { get; set; }

        public string HomeBetteamHTHDP { get; set; }

        public string AwayBetteamHTHDP { get; set; }

        public string HomeBetteamHTOU { get; set; }

        public string AwayBetteamHTOU { get; set; }

        public string BetteamHT1 { get; set; }

        public string BetteamHTX { get; set; }

        public string BetteamHT2 { get; set; }
    }
}