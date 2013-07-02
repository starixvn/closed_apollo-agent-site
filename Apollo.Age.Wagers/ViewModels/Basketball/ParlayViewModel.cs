// --------------------------------------------------------------------------
// <copyright file="ParlayViewModel.cs" company="Nexcel Solutions Vietnam">
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
    public class ParlayViewModel : WagerViewModel
    {     
        public decimal Stake9 { get; set; }

        public decimal Stake29 { get; set; }

        public int Count9 { get; set; }

        public int Count29 { get; set; }
    }
}
