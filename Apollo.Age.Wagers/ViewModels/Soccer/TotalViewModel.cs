// --------------------------------------------------------------------------
// <copyright file="TotalViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.30">Created</change>
//   <change who="Terry.Nguyen" date="2013.7.18">Add Stake Before</change>
// </history>

namespace Apollo.Age.Wagers.ViewModels.Soccer
{
    /// <summary>
    /// Total view model.
    /// </summary>
    public class TotalViewModel : WagerViewModel
    {
        public decimal HomeStakeOE { get; set; }

        public decimal AwayStakeOE { get; set; }

        public decimal HomeStakeHTOE { get; set; }

        public decimal AwayStakeHTOE { get; set; }

        public decimal Total01Stake { get; set; }

        public decimal Total23Stake { get; set; }

        public decimal Total46Stake { get; set; }

        public decimal Total7OverStake { get; set; }

        public decimal HTTotal01Stake { get; set; }

        public decimal HTTotal23Stake { get; set; }

        public decimal HTTotal4OverStake { get; set; }

        public decimal HomeStakeBeforeOE { get; set; }

        public decimal AwayStakeBeforeOE { get; set; }

        public decimal HomeStakeBeforeHTOE { get; set; }

        public decimal AwayStakeBeforeHTOE { get; set; }

        public decimal HTTotal01StakeBefore { get; set; }

        public decimal HTTotal23StakeBefore { get; set; }

        public decimal HTTotal4OverStakeBefore { get; set; }

        public decimal Total01StakeBefore { get; set; }

        public decimal Total23StakeBefore { get; set; }

        public decimal Total46StakeBefore { get; set; }

        public decimal Total7OverStakeBefore { get; set; }

        public int HomeCountOE { get; set; }

        public int AwayCountOE { get; set; }

        public int HomeCountHTOE { get; set; }

        public int AwayCountHTOE { get; set; }

        public int Total01Count { get; set; }

        public int Total23Count { get; set; }

        public int Total46Count { get; set; }

        public int Total7OverCount { get; set; }

        public int HTTotal01Count { get; set; }

        public int HTTotal23Count { get; set; }

        public int HTTotal4OverCount { get; set; }

        public string HomeBetteamOE { get; set; }

        public string AwayBetteamOE { get; set; }

        public string HomeBetteamHTOE { get; set; }

        public string AwayBetteamHTOE { get; set; }

        public string Total01Betteam { get; set; }

        public string Total23Betteam { get; set; }

        public string Total46Betteam { get; set; }

        public string Total7OverBetteam { get; set; }

        public string HTTotal01Betteam { get; set; }

        public string HTTotal23Betteam { get; set; }

        public string HTTotal4OverBetteam { get; set; }
    }
}