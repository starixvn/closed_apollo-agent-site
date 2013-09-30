// --------------------------------------------------------------------------
// <copyright file="StraightViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.30">Created</change>
// </history>


namespace Apollo.Age.Wagers.ViewModels.Tennis
{
    public class StraightViewModel : WagerViewModel
    {
        public decimal HomeStakeHDP { get; set; }

        public decimal AwayStakeHDP { get; set; }

        public decimal HomeStakeOU { get; set; }

        public decimal AwayStakeOU { get; set; }

        public decimal HomeStakeOE { get; set; }

        public decimal AwayStakeOE { get; set; }

        public int HomeCountHDP { get; set; }

        public int AwayCountHDP { get; set; }

        public int HomeCountOU { get; set; }

        public int AwayCountOU { get; set; }

        public int HomeCountOE { get; set; }

        public int AwayCountOE { get; set; }
    }
}