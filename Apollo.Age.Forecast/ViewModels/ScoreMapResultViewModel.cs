// --------------------------------------------------------------------------
// <copyright file="ScoreMapResultViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.5">Created</change>
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
    public class ScoreMapResultViewModel
    {
        public ScoreMapResultViewModel()
        {
            ScoreMatrix = new decimal[7, 7];
        }
        public int MatchId { get; set; }
        public string HomeName { get; set; }
        public string AwayName { get; set; }
        public decimal[,] ScoreMatrix { get; set; }
    }
}
