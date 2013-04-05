// --------------------------------------------------------------------------
// <copyright file="ScoreMapBetType.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.Age.Forecast.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScoreMapBetTypeViewModel
    {
        public ScoreMapBetTypeViewModel()
        {
            LiveBetTypes = new List<ScoreMapBetType>();
            DeadBallBetTypes = new List<ScoreMapBetType>();
        }
                
        public List<ScoreMapBetType> LiveBetTypes { get; set; }
        public List<ScoreMapBetType> DeadBallBetTypes { get; set; }
    }
}
