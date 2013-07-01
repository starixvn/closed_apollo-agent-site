// --------------------------------------------------------------------------
// <copyright file="ParlayViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.7.1">Created</change>
// </history>

namespace Apollo.Age.Wagers.ViewModels.Soccer
{
    public class ParlayViewModel : WagerViewModel
    {
        public decimal Stake9 { get; set; }

        public decimal Stake29 { get; set; }

        public int Count9 { get; set; }

        public int Count29 { get; set; }
    }
}