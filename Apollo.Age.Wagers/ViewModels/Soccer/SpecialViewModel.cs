// --------------------------------------------------------------------------
// <copyright file="SpecialViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.7.1">Created</change>
//   <change who="Terry.Nguyen" date="2013.7.18">Add Stake Before</change>
// </history>

namespace Apollo.Age.Wagers.ViewModels.Soccer
{
    /// <summary>
    /// SpecialViewModel class.
    /// </summary>
    public class SpecialViewModel : WagerViewModel
    {
        public int Bettype { get; set; }

        public decimal Stake { get; set; }

        public decimal StakeBefore { get; set; }

        public long Count { get; set; }

        public byte Type { get; set; }
    }
}