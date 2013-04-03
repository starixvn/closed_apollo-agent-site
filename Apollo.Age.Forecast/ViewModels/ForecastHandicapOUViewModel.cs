// --------------------------------------------------------------------------
// <copyright file="ForecastHandicapOUViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.3">Created</change>
// </history>


namespace Apollo.Age.Forecast.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ForecastHandicapOUViewModel
    {
        public int MatchId { get; set; }
        public int HomeId { get; set; }
        public int AwayId { get; set; }
        public bool LiveIndicator { get; set; }
    }
}
