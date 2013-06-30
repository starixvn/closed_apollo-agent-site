// --------------------------------------------------------------------------
// <copyright file="SpecialViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.30">Created</change>
// </history>


namespace Apollo.Age.Wagers.ViewModels.Tennis
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SpecialViewModel : WagerViewModel
    {
        public decimal HomeStakeMoney { get; set; }

        public decimal AwayStakeMoney { get; set; }

        public int HomeCountMoney { get; set; }

        public int AwayCountMoney { get; set; }
    }
}
