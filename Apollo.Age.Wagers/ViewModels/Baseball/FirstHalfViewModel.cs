// --------------------------------------------------------------------------
// <copyright file="FirstHalfViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.30">Created</change>
// </history>

namespace Apollo.Age.Wagers.ViewModels.Baseball
{
    public class FirstHalfViewModel : WagerViewModel
    {       
        public decimal HomeStakeHTHDP { get; set; }

        public decimal AwayStakeHTHDP { get; set; }

        public decimal HomeStakeHTOU { get; set; }

        public decimal AwayStakeHTOU { get; set; }

        public decimal HomeStakeML { get; set; }

        public decimal AwayStakeML { get; set; }

        public int HomeCountHTHDP { get; set; }

        public int AwayCountHTHDP { get; set; }

        public int HomeCountHTOU { get; set; }

        public int AwayCountHTOU { get; set; }

        public int HomeCountML { get; set; }

        public int AwayCountML { get; set; }
    }
}