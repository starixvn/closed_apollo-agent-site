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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScoreMapBetType
    {
        public int TypeId { get; set; }
        public bool IsLive { get; set; }
        public bool IsDead { get; set; }
        public bool Id1st { get; set; }
    }
}
