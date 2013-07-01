// --------------------------------------------------------------------------
// <copyright file="SpecialViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.7.1">Created</change>
// </history>

namespace Apollo.Age.Wagers.ViewModels.Soccer
{
    public class SpecialViewModel : WagerViewModel
    {
        public int bettype { get; set; }

        public decimal Stake { get; set; }

        public long Count { get; set; }

        public byte Type { get; set; }
    }
}